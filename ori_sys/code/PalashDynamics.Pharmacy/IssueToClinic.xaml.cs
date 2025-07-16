using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
//using PalashDynamics.PalashDynamicsServiceRef;
using PalashDynamics.ValueObjects;
using CIMS;

using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Data;
using SerRef = PalashDynamics.Service.PalashServiceReferance;
using PalashDynamics.UserControls;
using System.Windows.Browser;
using PalashDynamics.Collections;
using System.ComponentModel;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.ValueObjects.Inventory.BarCode;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Administration.UserRights;

namespace PalashDynamics.Pharmacy
{
    public partial class IssueToClinic : UserControl
    {
        #region Variable Declarations
        private SwivelAnimation objAnimation;
        PagedCollectionView pcvItemList;
        //Added By Pallavi
        bool flagResetControls = false;
        bool IsDirectIndent = false; //***//
        bool IsInterClinicIndent = false; //***//
        bool IsApprovedDirect = false;//***//
        public bool IsFromExpiredItem;
        public long ToStore;
        public long FromStore;
        List<clsStoreVO> StoreList;
        WaitIndicator Indicatior;
        int ClickedFlag1 = 0;
        #region 'Paging'

        public PagedSortableCollectionView<clsIssueListVO> IssueDataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return IssueDataList.PageSize;
            }
            set
            {
                if (value == IssueDataList.PageSize) return;
                IssueDataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        #endregion
        private List<MasterListItem> _RessonIssue = new List<MasterListItem>();
        public List<MasterListItem> RessonIssue
        {
            get
            {
                return _RessonIssue;
            }
            set
            {
                _RessonIssue = value;
            }
        }
        private clsIndentMasterVO _SelectedIndent = new clsIndentMasterVO();
        public clsIndentMasterVO SelectedIndent
        {
            get
            {
                return _SelectedIndent;
            }
            set
            {
                _SelectedIndent = value;
            }
        }

        private ObservableCollection<clsIssueItemDetailsVO> _ocIssueItemDetailsList = new ObservableCollection<clsIssueItemDetailsVO>();
        public ObservableCollection<clsIssueItemDetailsVO> ocIssueItemDetailsList
        {
            get
            {
                return _ocIssueItemDetailsList;
            }
            set
            {
                _ocIssueItemDetailsList = value;
            }
        }

        bool flagZeroTransit = false;

        private clsIssueItemDetailsVO _SelectedRowItem = new clsIssueItemDetailsVO();

        public clsIssueItemDetailsVO SelectedRowItem
        {
            get
            {
                return _SelectedRowItem;
            }
            set
            {
                _SelectedRowItem = value;
            }
        }

        public int SelectedRowItemIndex { get; set; }

        public long StoreId { get; set; }

        float PendingQty = 0;
        #endregion

        #region Constructor and Loaded
        public IssueToClinic()
        {
            Indicatior = new WaitIndicator();
            InitializeComponent();
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);//***//
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //Paging
            IssueDataList = new PagedSortableCollectionView<clsIssueListVO>();
            IssueDataList.OnRefresh += new EventHandler<RefreshEventArgs>(IssueDataList_OnRefresh);
            dgItemList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgItemList_CellEditEnded);
            DataListPageSize = 15;
            dgdpIssueList.PageSize = DataListPageSize;
            dgdpIssueList.Source = IssueDataList;           
            FillRessonIssue();  
            FillStore();   
            FillReceivedByList();  
            //FillIssueList();
            this.Loaded += new RoutedEventHandler(IssueToClinic_Loaded);  
        }

        void IssueToClinic_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsIssueListVO();

            dpIssueDate.SelectedDate = DateTime.Now;
            dpIssueFromDate.SelectedDate = DateTime.Now;
            dpIssueToDate.SelectedDate = DateTime.Now;
            dpIssueDate.IsEnabled = false;          
            FillIssueList();

          

            //By Anjali....................
            if (IsFromExpiredItem == true)
            {
                ResetControls();
                cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                cmbReceivedBy.IsEnabled = false;
                objAnimation.Invoke(RotationType.Forward);
                grp1.Visibility = System.Windows.Visibility.Collapsed;
                grp.Visibility = System.Windows.Visibility.Collapsed;
                //pcvItemList = new PagedCollectionView(this.ocIssueItemDetailsList.ToList());
                //pcvItemList.GroupDescriptions.Add(new
                //PropertyGroupDescription("ItemName"));
                //dgItemList.ItemsSource = pcvItemList;
                dgItemList.ItemsSource = this.ocIssueItemDetailsList.ToList();
                dgItemList.Columns[15].IsReadOnly = true;
                this.SumOfTotals();
            }
            else
            {
                grp1.Visibility = System.Windows.Visibility.Visible;
                grp.Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[15].IsReadOnly = false;
            }
        }

        void IssueDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //throw new NotImplementedException();
            FillIssueList();
        }
        #endregion

        #region Private Methods
        private void FillItemListByIssueId()
        {
            if (dgIssueList.SelectedItem != null)
            {
                clsGetItemListByIssueIdBizActionVO BizAction = new clsGetItemListByIssueIdBizActionVO();

                BizAction.IssueId = ((clsIssueListVO)dgIssueList.SelectedItem).IssueId;
                BizAction.UnitID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueUnitID;
                BizAction.flagReceivedIssue = false;

                BizAction.ItemList = new List<clsIssueItemDetailsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);//SerRef.PalashServiceClient Client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ItemList = ((clsGetItemListByIssueIdBizActionVO)e.Result).ItemList;
                        dgIssueItemList.ItemsSource = null;
                        dgIssueItemList.ItemsSource = BizAction.ItemList;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                dgIssueItemList.ItemsSource = null;
            }
        }

        private void FillReceivedByList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri); //SerRef.PalashServiceClient Client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    cmbReceivedBy.ItemsSource = null;
                    cmbReceivedBy.ItemsSource = objList;
                    cmbReceivedBy.SelectedItem = objList[0];
                    if (IsFromExpiredItem == true)
                        cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;



                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillRessonIssue()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ReasonForIssue;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        ((clsGetMasterListBizActionVO)args.Result).MasterList.Insert(0, new MasterListItem { ID = 0, Description = "--Select--" });
                        RessonIssue = ((clsGetMasterListBizActionVO)args.Result).MasterList;

                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillIssueList()
        {
            Indicatior.Show();
            try
            {
                clsGetIssueListBizActionVO BizAction = new clsGetIssueListBizActionVO();
                BizAction.IssueList = new List<clsIssueListVO>();
                BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                BizAction.flagReceiveIssue = false;
                BizAction.IndentCriteria = 0;
                if (rdbWithIndent.IsChecked == true)
                    BizAction.IndentCriteria = 1;
                if (rdbWithoutIndent.IsChecked == true)
                    BizAction.IndentCriteria = 2;

                if (!string.IsNullOrEmpty(txtIssueNo.Text))
                    BizAction.IssueDetailsVO.IssueNumber = txtIssueNo.Text;

                BizAction.IssueFromDate = dpIssueFromDate.SelectedDate;
                //Commented By pallavi
                // BizAction.IssueToDate = dpIssueToDate.SelectedDate.Value;
                //Added By Pallavi
                if (dpIssueToDate.SelectedDate == null)
                    BizAction.IssueToDate = dpIssueToDate.SelectedDate;
                else
                    BizAction.IssueToDate = dpIssueToDate.SelectedDate.Value.AddDays(1);

                if (cmbIssueFromStoreSrch.SelectedItem != null)
                {
                    BizAction.IssueFromStoreId = ((clsStoreVO)cmbIssueFromStoreSrch.SelectedItem).StoreId;
                    if (((clsStoreVO)cmbIssueFromStoreSrch.SelectedItem).StoreId == 0)
                        BizAction.IssueFromStoreId = null;
                }

                if (cmbIssueToStoreSrch.SelectedItem != null)
                {
                    BizAction.IssueToStoreId = ((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId;
                    if (((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId == 0)
                        BizAction.IssueToStoreId = null;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                BizAction.intIsIndent = 1;  //For Getting the Non QS Items 


                //***//-----
                if (txtMrno.Text.Trim() != "")
                    BizAction.MRNo = txtMrno.Text.Trim();
                else
                    BizAction.MRNo = "";


                if (txtName.Text.Trim() != "")
                    BizAction.PatientName = txtName.Text.Trim();
                else
                    BizAction.PatientName = "";
                //-------

                #region Paging
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = IssueDataList.PageIndex * IssueDataList.PageSize + 1;
                BizAction.MaximumRows = IssueDataList.PageSize;



                foreach (SortDescription sortDesc in IssueDataList.SortDescriptions)
                {
                    BizAction.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                }
                #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);//SerRef.PalashServiceClient Client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    Indicatior.Close();
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetIssueListBizActionVO)e.Result;
                        BizAction.IssueList = ((clsGetIssueListBizActionVO)e.Result).IssueList;
                        IssueDataList.TotalItemCount = BizAction.TotalRows;

                        IssueDataList.Clear();
                        foreach (var item in BizAction.IssueList)
                        {
                            IssueDataList.Add(item);
                        }

                        dgIssueList.ItemsSource = null;
                        dgIssueList.ItemsSource = IssueDataList;

                        dgdpIssueList.Source = null;
                        dgdpIssueList.PageSize = BizAction.MaximumRows;
                        dgdpIssueList.Source = IssueDataList;
                        txtTotalCountRecords.Text = IssueDataList.TotalItemCount.ToString();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }


        }


        private void FillStore()
        {
            try
            {
                if (Indicatior == null) Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
                BizActionObj.IsUserwiseStores = true;
                BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //False when we want to fetch all items
                clsItemMasterVO obj = new clsItemMasterVO();
                obj.RetrieveDataFlag = false;
                BizActionObj.StoreType = 2;
                BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);//SerRef.PalashServiceClient client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                        BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true, IsQuarantineStore = false, Parent = 0 };


                        BizActionObj.ItemMatserDetails.Insert(0, Default);
                        BizActionObj.ToStoreList.Insert(0, Default);

                        StoreList = new List<clsStoreVO>();
                        StoreList.AddRange(BizActionObj.ItemMatserDetails);//StoreList.AddRange(BizActionObj.ToStoreList);

                        var result = from item in StoreList
                                     where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                     select item;

                        var result2 = from item in StoreList
                                      where item.Status == true && item.IsQuarantineStore != true
                                      select item;



                        flagResetControls = true;

                        var NonQSAndUserAssignedStores = from item in BizActionObj.ToStoreList.ToList()
                                                         where item.IsQuarantineStore == false
                                                         select item;

                        NonQSAndUserAssignedStores.ToList().Insert(0, Default);

                        cmbFromStore.ItemsSource = NonQSAndUserAssignedStores.ToList();   //(List<clsStoreVO>)result.ToList();
                        cmbFromStore.SelectedItem = NonQSAndUserAssignedStores.ToList()[0];

                        cmbIssueFromStoreSrch.ItemsSource = NonQSAndUserAssignedStores.ToList(); //(List<clsStoreVO>)result.ToList();
                        cmbIssueFromStoreSrch.SelectedItem = NonQSAndUserAssignedStores.ToList()[0];//result.ToList()[0];
                   
                        if (IsInterClinicIndent == true) //***//
                        {
                            cmbToStore.ItemsSource = result2.ToList();
                            cmbToStore.SelectedItem = result2.ToList()[0];
                            cmbIssueToStoreSrch.ItemsSource = result2.ToList(); 
                            cmbIssueToStoreSrch.SelectedItem = result2.ToList()[0]; 
                        }
                        else
                        {
                            cmbToStore.ItemsSource = (List<clsStoreVO>)result.ToList();//BizActionObj.ToStoreList.ToList(); //result2.ToList();
                            cmbToStore.SelectedItem = ((List<clsStoreVO>)result.ToList())[0]; //result2.ToList()[0];
                            cmbIssueToStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();//BizActionObj.ToStoreList.ToList();  //result2.ToList();
                            cmbIssueToStoreSrch.SelectedItem = ((List<clsStoreVO>)result.ToList())[0]; //result2.ToList()[0];
                        }  

                        if (IsFromExpiredItem == true)
                        {
                            cmbToStore.ItemsSource = null;
                            cmbToStore.ItemsSource = StoreList.Where(t => t.IsQuarantineStore == true);
                            cmbFromStore.SelectedValue = FromStore;
                            cmbToStore.SelectedValue = ToStore;
                            cmbFromStore.IsEnabled = false;
                            cmbToStore.IsEnabled = false;
                        }
                        else
                        {
                            cmbFromStore.IsEnabled = true;
                            cmbToStore.IsEnabled = true;
                        }

                        #region commented by Ashish Z.
                        //    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                        //    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                        //    //clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "-Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = false,IsQuarantineStore=false,Parent=0 };
                        //    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true, IsQuarantineStore = false, Parent = 0 };


                        //    BizActionObj.ItemMatserDetails.Insert(0, Default);
                        //    BizActionObj.ToStoreList.Insert(0, Default);

                        //    StoreList = new List<clsStoreVO>();
                        //    StoreList.AddRange(BizActionObj.ItemMatserDetails);


                        //    //var result = from item in BizActionObj.ItemMatserDetails
                        //    //             where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                        //    //             select item;

                        //    //var result2 = from item in BizActionObj.ItemMatserDetails
                        //    //              where item.Status == true
                        //    //              select item;
                        //    //flagResetControls = true;
                        //    //cmbFromStore.ItemsSource = (List<clsStoreVO>)result.ToList();
                        //    //cmbFromStore.SelectedItem = BizActionObj.ItemMatserDetails[0];

                        //    //cmbIssueFromStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();
                        //    //cmbIssueFromStoreSrch.SelectedItem = BizActionObj.ItemMatserDetails[0];


                        //    //cmbToStore.ItemsSource = result2.ToList();
                        //    //cmbToStore.SelectedItem = result2.ToList()[0];


                        //    //cmbIssueToStoreSrch.ItemsSource = result2.ToList();
                        //    //cmbIssueToStoreSrch.SelectedItem = result2.ToList()[0];


                        //    var result = from item in StoreList
                        //                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore != true
                        //                 select item;

                        //    var result2 = from item in StoreList
                        //                  where item.Status == true && item.IsQuarantineStore != true
                        //                  select item;



                        //    flagResetControls = true;
                        //    cmbFromStore.ItemsSource = (List<clsStoreVO>)result.ToList();
                        //    cmbFromStore.SelectedItem = result.ToList()[0];

                        //    cmbIssueFromStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();
                        //    cmbIssueFromStoreSrch.SelectedItem = result.ToList()[0];


                        //    cmbToStore.ItemsSource = BizActionObj.ToStoreList.ToList(); //result2.ToList();
                        //    cmbToStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0]; //result2.ToList()[0];


                        //    cmbIssueToStoreSrch.ItemsSource = BizActionObj.ToStoreList.ToList();  //result2.ToList();
                        //    cmbIssueToStoreSrch.SelectedItem = BizActionObj.ToStoreList.ToList()[0]; //result2.ToList()[0];



                        //    if (IsFromExpiredItem == true)
                        //    {
                        //        cmbToStore.ItemsSource = null;
                        //        cmbToStore.ItemsSource = StoreList.Where(t => t.IsQuarantineStore == true);
                        //        cmbFromStore.SelectedValue = FromStore;
                        //        cmbToStore.SelectedValue = ToStore;
                        //        cmbFromStore.IsEnabled = false;
                        //        cmbToStore.IsEnabled = false;
                        //    }
                        //    else
                        //    {
                        //        cmbFromStore.IsEnabled = true;
                        //        cmbToStore.IsEnabled = true;
                        //    }
                        #endregion
                    }
                    Indicatior.Close();
                };

                client.CloseAsync();
            }
            catch (Exception EX)
            {
                Indicatior.Close();
                throw;
            }

        }

        //private void FillStore()
        //{
        //    clsFillStoreMasterListBizActionVO BizActionObj = new clsFillStoreMasterListBizActionVO();
        //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
        //        BizActionObj.UnitID = 0;
        //    else
        //        BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    BizActionObj.StoreType = 2;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    SerRef.PalashServiceClient client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            List<clsStoreVO> ObjList = new List<clsStoreVO>();
        //            ObjList.Add( new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true,ParentStoreID=0 });
        //           if(((clsFillStoreMasterListBizActionVO)args.Result).StoreMasterDetails !=null)
        //               ObjList.AddRange(((clsFillStoreMasterListBizActionVO)args.Result).StoreMasterDetails);

        //           var result = from item in ObjList
        //                         where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
        //                         select item;

        //           var result2 = from item in ObjList
        //                          where item.Status == true
        //                          select item;

        //            flagResetControls = true;
        //            cmbFromStore.ItemsSource = (List<clsStoreVO>)result.ToList();
        //            cmbFromStore.SelectedItem = ObjList[0];

        //            cmbIssueFromStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();
        //            cmbIssueFromStoreSrch.SelectedItem = ObjList[0];
        //            cmbToStore.ItemsSource = result2.ToList();
        //            cmbToStore.SelectedItem = result2.ToList()[0];
        //            cmbIssueToStoreSrch.ItemsSource = result2.ToList();
        //            cmbIssueToStoreSrch.SelectedItem = result2.ToList()[0];

        //        }

        //    };
        //    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}

        private bool Validation()
        {
            Boolean IsValidationComplete = true;

            if (dpIssueDate.SelectedDate == null)
            {
                dpIssueDate.SetValidation("Issue Date can not be blank.");
                dpIssueDate.RaiseValidationError();
                dpIssueDate.Focus();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;

            }
            // else if (cmbFromStore.SelectedItem == null || (long)cmbFromStore.SelectedValue == (long)0)
            if (cmbFromStore.SelectedItem == null || ((clsStoreVO)cmbFromStore.SelectedItem).StoreId == 0)//|| (long)cmbFromStore.SelectedValue == (long)0)
            {
                cmbFromStore.SetValidation("Issue From Store can not be blank.");
                cmbFromStore.RaiseValidationError();
                cmbFromStore.Focus();
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issue From Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWin.Show();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }
            if (cmbToStore.SelectedItem == null || ((clsStoreVO)cmbToStore.SelectedItem).StoreId == 0)// || (long)cmbToStore.SelectedValue == (long)0)
            {
                cmbToStore.SetValidation("Issue To Store can not be blank.");
                cmbToStore.RaiseValidationError();
                cmbToStore.Focus();
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issue To Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWin.Show();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }
            if (((clsStoreVO)cmbFromStore.SelectedItem).StoreId == ((clsStoreVO)cmbToStore.SelectedItem).StoreId)
            {
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issue from store and issue to store cannot be same. Please select different stores .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWin.Show();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }
            if (this.ocIssueItemDetailsList != null)
            {

                if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>().Count <= 0)
                {
                    IsValidationComplete = false;
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Select Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                    IsValidationComplete = false;
                    Indicatior.Close();
                    ClickedFlag1 = 0;
                    return IsValidationComplete;

                }
                else
                {
                    for (int i = 0; i < this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>().Count; i++)
                    {
                        float fltOne = 1;
                        float fltZero = 0;
                        float Infinity = fltOne / fltZero;
                        float NaN = fltZero / fltZero;

                        if (rdbIsIndent.IsChecked == true)
                        {
                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ConversionFactor <= 0 || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ConversionFactor == Infinity || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ConversionFactor == NaN)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                                return false;
                            }
                            else if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseConversionFactor <= 0 || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseConversionFactor == Infinity || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseConversionFactor == NaN)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                                return false;
                            }

                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty <= 0)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                                return false;
                            }

                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].SelectedUOM.ID == 0)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please select Issued UOM for '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                IsValidationComplete = false;
                                break;
                            }
                            else if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssuePendingQuantity != 0)
                            {
                                if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty == 0 || string.IsNullOrEmpty(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty.ToString()))
                                {
                                    IsValidationComplete = false;
                                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Issued Quantity for '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWin.Show();
                                    IsValidationComplete = false;
                                    break;
                                }
                            }
                            else if (Convert.ToDecimal(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseQuantity) > this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssuePendingQuantity)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issued Quantity Should Not Be Greater Than Pending Indent Quatity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                IsValidationComplete = false;
                                break;
                            }
                            else if (Convert.ToDecimal(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].StockingQuantity) > this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].AvailableStock)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issued Quantity Should Not Be Greater Than Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                IsValidationComplete = false;
                                break;
                            }
                        }
                        else if (rdbIsPurchaseRequisition.IsChecked == true)
                        {
                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ConversionFactor <= 0 || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ConversionFactor == Infinity || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ConversionFactor == NaN)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                                return false;
                            }
                            else if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseConversionFactor <= 0 || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseConversionFactor == Infinity || this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseConversionFactor == NaN)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                                return false;
                            }

                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty <= 0)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                                return false;
                            }

                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].SelectedUOM.ID == 0)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please select Issued UOM for '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                IsValidationComplete = false;
                                break;
                            }
                            else if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssuePendingQuantity != 0)
                            {
                                if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty == 0 || string.IsNullOrEmpty(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty.ToString()))
                                {
                                    IsValidationComplete = false;
                                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Issued Quantity For Selected Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWin.Show();
                                    IsValidationComplete = false;
                                    break;
                                }
                            }
                            else if (Convert.ToDecimal(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].BaseQuantity) > this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssuePendingQuantity)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Issue Quantity for '" + this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                IsValidationComplete = false;
                                break;
                            }
                            else
                                if (Convert.ToDecimal(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].StockingQuantity) > this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].AvailableStock)
                                {
                                    IsValidationComplete = false;
                                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issued Quantity Should Not Be Greater Than Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWin.Show();
                                    IsValidationComplete = false;
                                    break;
                                }
                        }
                        else
                            if (rdbIsWithoutIndent.IsChecked == true)
                            {
                                if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty <= 0)
                                {
                                    IsValidationComplete = false;
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    return false;
                                }

                                if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty == 0 || string.IsNullOrEmpty(this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty.ToString()))
                                {
                                    IsValidationComplete = false;
                                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Issued Quantity For Selected Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgWin.Show();
                                    IsValidationComplete = false;
                                    break;
                                }
                                else
                                    if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssueQty > this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].AvailableStock)
                                    {
                                        IsValidationComplete = false;
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issued Quantity Should Not Be Greater Than Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgWin.Show();
                                        IsValidationComplete = false;
                                        break;
                                    }

                            }
                    }
                    bool flagIssueItem = false;

                    for (int i = 0; i < this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>().Count; i++)
                    {
                        if (rdbIsIndent.IsChecked == true)
                        {
                            if (this.ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>()[i].IssuePendingQuantity != 0)
                            {
                                flagIssueItem = false;
                                break;
                            }
                        }

                    }
                    decimal? Qty = 0;
                    long iID = 0;
                    foreach (var item in ocIssueItemDetailsList)
                    {
                        if (rdbIsIndent.IsChecked == true && rdbIsPurchaseRequisition.IsChecked == true)
                        {
                            if (item.IssueQty > 0)
                            {
                                var item2 = from r in ocIssueItemDetailsList
                                            where r.ItemId == item.ItemId
                                            select r;
                                if (item2.ToList().Count > 0)
                                {
                                    if (iID != item.ItemId)
                                    {
                                        Qty = 0;
                                    }
                                    Qty += (item.IssueQty);
                                    if (Qty > item.IndentQty)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issued quantity should be less than indent/PR quantity!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgWin.Show();
                                        IsValidationComplete = false;
                                        break;
                                    }
                                }

                                iID = item.ItemId;
                            }
                        }
                    }


                    var list = from r in ocIssueItemDetailsList
                               let k = new { ItemName = r.ItemName, ItemID = r.ItemId, PendingIssueQty = r.IssuePendingQuantity }
                               group r by k into IssueItem
                               select new { ItemName = IssueItem.Key.ItemName, IssueQuantity = IssueItem.Sum(r => r.BaseQuantity), PendingIssueQty = IssueItem.Key.PendingIssueQty }; //select new { ItemName = IssueItem.Key.ItemName, IssueQuantity = IssueItem.Sum(r => r.IssueQty), PendingIssueQty = IssueItem.Key.PendingIssueQty };


                    foreach (var item in list.ToList())
                    {
                        if (item.PendingIssueQty < Convert.ToDecimal(item.IssueQuantity))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "SUM of Issued Quantity Should not be greater than Pending Quantity for '" + item.ItemName + "'", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();
                            IsValidationComplete = false;
                            break;
                        }
                    }
                    if (flagIssueItem)
                    {
                        IsValidationComplete = false;
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Unable To Save! Indent/PR is already issued!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        flagIssueItem = false;
                        Indicatior.Close();
                        ClickedFlag1 = 0;
                        return IsValidationComplete;
                    }
                }
            }
            return IsValidationComplete;
        }

        void win_OnOnIndentSelected(object sender, EventArgs e)
        {

            //this.SelectedIndent = ((IndentSearch)sender).SelectedIndent;
            this.SelectedIndent = ((IndnetSearch1)sender).SelectedIndent;
            //By Anjali.........................................................................
            //if (SelectedIndent.IsIndent == true)
            //{
            //    txtIndentNumber.Text = this.SelectedIndent.IndentNumber;
            //}
            //else
            //{
            //    txtPRNumber.Text = this.SelectedIndent.IndentNumber;
            //}
            if (SelectedIndent.IsIndent == 1)
            {
                txtIndentNumber.Text = this.SelectedIndent.IndentNumber;
            }
            else if (SelectedIndent.IsIndent == 0)
            {
                txtPRNumber.Text = this.SelectedIndent.IndentNumber;
            }

            //......................................................................

            var results = from r in ((List<clsStoreVO>)cmbFromStore.ItemsSource)
                          where r.StoreId == this.SelectedIndent.ToStoreID
                          select r;

            foreach (clsStoreVO item in results)
            {
                cmbFromStore.SelectedItem = item;
                cmbFromStore.IsEnabled = false;

            }

            var results1 = from r in ((List<clsStoreVO>)cmbToStore.ItemsSource)
                           where r.StoreId == this.SelectedIndent.FromStoreID
                           select r;

            foreach (clsStoreVO item in results1)
            {
                cmbToStore.SelectedItem = item;
                cmbToStore.IsEnabled = false;
            }

            BindItemListByIndentId();
        }

        private void BindItemListByIndentId()
        {
            dgItemList.ItemsSource = null;
            this.ocIssueItemDetailsList.Clear();

            GetItemListByIndentIdForIssueItemBizActionVO BizActionObj = new GetItemListByIndentIdForIssueItemBizActionVO();
            BizActionObj.IndentID = this.SelectedIndent.ID;
            //BizActionObj. = this.SelectedIndent.ID;
            BizActionObj.IndentID = this.SelectedIndent.ID;

            BizActionObj.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri); //SerRef.PalashServiceClient client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.IssueItemDetailsList = ((GetItemListByIndentIdForIssueItemBizActionVO)args.Result).IssueItemDetailsList;
                    this.ocIssueItemDetailsList = new ObservableCollection<clsIssueItemDetailsVO>();
                    foreach (clsIssueItemDetailsVO item in BizActionObj.IssueItemDetailsList)
                    {
                        this.ocIssueItemDetailsList.Add(item);
                    }

                    // dgItemList.ItemsSource = this.ocIssueItemDetailsList;

                    pcvItemList = new PagedCollectionView(this.ocIssueItemDetailsList.ToList());
                    pcvItemList.GroupDescriptions.Add(new
                        PropertyGroupDescription("ItemName"));
                    //   pcvItemList.GroupDescriptions.Add(new
                    //     PropertyGroupDescription("IndentQty"));
                    dgItemList.ItemsSource = pcvItemList;

                    this.SumOfTotals();


                    //cmbFromStore.ItemsSource = BizActionObj.ItemMatserDetails;
                    //var result = from item in BizActionObj.ItemMatserDetails
                    //             where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                    //             select item;


                    //cmbFromStore.ItemsSource = (List<clsStoreVO>)result.ToList();

                    //cmbToStore.ItemsSource = BizActionObj.ItemMatserDetails;

                }

            };

            client.CloseAsync();

        }

        private void SumOfTotals()
        {

            var results = from r in this.ocIssueItemDetailsList
                          select r;

            decimal? TotalIssueItem = results.Sum(cnt => cnt.IssueQty);
            //txtNoOfItems.Text = TotalIssueItem.ToString();
            txtNoOfItems.Text = String.Format("{0:0.00}", TotalIssueItem);

            decimal? TotalAmount = results.Sum(cnt => cnt.ItemTotalAmount);
            // String.Format("{0:0.##}", TotalAmount);
            txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);//TotalAmount.ToString();
            decimal? TotalVATAmount = results.Sum(cnt => cnt.ItemVATAmount);
            //txtTotalVAT.Text = TotalVATAmount.ToString();
        }

        private void ApplyRulesForIssueItem()
        {
            //List<clsIssueItemDetailsVO> t = new List<clsIssueItemDetailsVO>();
            //t = ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>();

            var results = from r in this.ocIssueItemDetailsList
                          where r.ItemId == SelectedRowItem.ItemId
                          select r;


            foreach (clsIssueItemDetailsVO item in results)
            {

            }

            decimal? IssueQtyTotal = results.Sum(cnt => cnt.IssueQty);

            decimal? ActualIndentQty = ((clsIssueItemDetailsVO)results.First()).IndentQty;


            if (IssueQtyTotal > ActualIndentQty)
            {
                this.ocIssueItemDetailsList[SelectedRowItemIndex].IssueQty = 0;

                //var results1 = from r in this.ocIssueItemDetailsList
                //               where r.ItemId == SelectedRowItem.ItemId && r.BatchId == SelectedRowItem.BatchId
                //               select r;

                //foreach (clsIssueItemDetailsVO item in results1)
                //{
                //    for (int i = 0; i < this.ocIssueItemDetailsList.Count(); i++)
                //    {
                //        if (this.ocIssueItemDetailsList[i].ItemId == item.ItemId && this.ocIssueItemDetailsList[i].BatchId == item.BatchId)
                //        {
                //            this.ocIssueItemDetailsList[i].IssueQty = 0;
                //        }
                //    }
                //}

            }



            // get AvailableStock from list
            var resAvailableStock = from r in this.ocIssueItemDetailsList
                                    where r.ItemId == SelectedRowItem.ItemId && r.BatchId == SelectedRowItem.BatchId
                                    select r;

            decimal? AvailableStock = 0;
            decimal? IssueQtyForItem = 0;

            foreach (clsIssueItemDetailsVO item in resAvailableStock)
            {
                AvailableStock = item.AvailableStock;
                IssueQtyForItem = item.IssueQty;
            }

            // get AvailableStock from list
        }

        private void ReFillItemList()
        {
            pcvItemList = new PagedCollectionView(this.ocIssueItemDetailsList.ToList());
            pcvItemList.GroupDescriptions.Add(new
                PropertyGroupDescription("ItemName"));
            pcvItemList.GroupDescriptions.Add(new
               PropertyGroupDescription("IndentQty"));
            dgItemList.ItemsSource = pcvItemList;
        }

        private bool CheckFrontPannelSearch()
        {
            bool result = true;

            DateTime? _FromDate = dpIssueFromDate.SelectedDate;
            DateTime? _ToDate = dpIssueToDate.SelectedDate;

            if (_FromDate > _ToDate)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter From Date less than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                result = false;
            }
            else
            {
                dpIssueFromDate.ClearValidationError();
                dpIssueToDate.ClearValidationError();
                result = true;
            }
            return result;
        }

        private void ResetControls()
        {
            dpIssueDate.SelectedDate = DateTime.Now;
            cmbFromStore.SelectedValue = (long)0;
            cmbToStore.SelectedValue = (long)0;
            txtIndentNumber.Text = String.Empty;
            txtPRNumber.Text = String.Empty;
            dgItemList.ItemsSource = null;
            if (IsFromExpiredItem != true)
            {
                this.ocIssueItemDetailsList.Clear();

            }

            txtNoOfItems.Text = String.Empty;
            txtTotalAmount.Text = String.Empty;
            //txtTotalVAT.Text = String.Empty;
            txtRemark.Text = String.Empty;
            //cmbReceivedBy.SelectedItem = null;
            flagResetControls = true;
            flagZeroTransit = false;
        }

        private void ResetControls1()
        {
            dpIssueDate.SelectedDate = DateTime.Now;
            cmbFromStore.SelectedValue = (long)0;
            cmbToStore.SelectedValue = (long)0;
            txtIndentNumber.Text = String.Empty;
            txtPRNumber.Text = String.Empty;
            dgItemList.ItemsSource = null;
            this.ocIssueItemDetailsList.Clear();
            txtNoOfItems.Text = String.Empty;
            txtTotalAmount.Text = String.Empty;
            //txtTotalVAT.Text = String.Empty;
            txtRemark.Text = String.Empty;
            //cmbReceivedBy.SelectedItem = null;
            flagZeroTransit = false;
        }

        public void fillItemGridOnBar()
        {
            clsCounterSaleBarCodeBizActionVO BizActionObj = new clsCounterSaleBarCodeBizActionVO();
            BizActionObj.IssueList = new List<clsItemSalesDetailsVO>();
            WaitIndicator w = new WaitIndicator();
            w.Show();
            try
            {
                string[] str = txtBarCode.Text.Split('-');
                if (str.Length > 1)
                {
                    BizActionObj.ItemID = Convert.ToInt64(str[0]);
                    string inputString = null;
                    string[] str1 = null;
                    bool blnFlag = false;
                    if (str[1].Contains("/"))
                    {
                        str1 = str[1].Split('/');
                        inputString = str1[1];
                    }
                    else
                    {
                        str1 = str[1].Split();
                        // str1[0] = str[1];
                        inputString = str1[0];
                    }


                    string BatchID = null;

                    string lastCharacter = inputString.Substring(inputString.Length - 1);
                    string lastCharacter1 = inputString.Substring(inputString.Length - 2);

                    if (lastCharacter == "B")
                    {
                        BatchID = inputString.Substring(0, inputString.Length - 1);
                        BizActionObj.BatchCode = str1[0];
                        BizActionObj.BatchID = Convert.ToInt64(BatchID);



                        foreach (var item in ocIssueItemDetailsList.Where(x => x.ItemId == Convert.ToInt64(str[0]) & x.BatchId == Convert.ToInt64(BatchID)))
                        {
                            if (item.AvailableStock > Convert.ToDecimal(item.IndentQty))
                            {
                                item.IssueQty = item.IssueQty + 1;
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                mgbx.Show();
                            }
                            blnFlag = true;
                        }
                    }
                    else if (lastCharacter == "I")
                    {
                        if (lastCharacter1 == "BI")
                        {
                            str[1] = inputString.Substring(0, inputString.Length - 2);
                            BizActionObj.BatchID = Convert.ToInt64(str[1]);

                            foreach (var item in ocIssueItemDetailsList.Where(x => x.ItemId == Convert.ToInt64(str[0]) & x.BatchId == Convert.ToInt64(str[1])))
                            {
                                if (item.AvailableStock > Convert.ToDecimal(item.IndentQty))
                                {
                                    item.IssueQty = item.IssueQty + 1;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                }
                                blnFlag = true;
                            }

                        }
                        else
                        {
                            str[1] = inputString.Substring(0, inputString.Length - 1);
                            BizActionObj.ItemCode = str[1];
                            foreach (var item in ocIssueItemDetailsList.Where(x => x.ItemId == Convert.ToInt64(str[0])))
                            {
                                if (item.AvailableStock > Convert.ToDecimal(item.IndentQty))
                                {
                                    item.IssueQty = item.IssueQty + 1;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                }
                                blnFlag = true;
                            }
                        }
                    }


                    //foreach (var item in PharmacyItems.Where(x => x.ID ==Convert.ToInt64(str[0]) & x.BatchID == Convert.ToInt64(str[1])))
                    //{
                    //    if (Convert.ToDouble(item.AvailableQuantity) > item.Quantity)
                    //    {
                    //        item.Quantity = item.Quantity + 1;
                    //    }
                    //    else
                    //    {
                    //        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    //        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item is out of stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //        mgbx.Show();
                    //    }
                    //    blnFlag = true;
                    //}
                    dgItemList.ItemsSource = null;
                    dgItemList.ItemsSource = this.ocIssueItemDetailsList;
                    dgItemList.UpdateLayout();

                    if (blnFlag == false)
                    {
                        BizActionObj.StoreId = StoreId;
                        BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, e) =>
                        {
                            if (e.Error == null && e.Result != null)
                            {
                                BizActionObj = (clsCounterSaleBarCodeBizActionVO)e.Result;
                                BizActionObj.IssueList = ((clsCounterSaleBarCodeBizActionVO)e.Result).IssueList;
                                //MasterList.TotalItemCount = BizActionObj.TotalRows;
                                //MasterList.Clear();
                                //foreach (var item in BizActionObj.IssueList)
                                //{
                                //                     foreach (var item in (((ItemListNew)sender).ItemBatchList))
                                //{
                                foreach (clsItemSalesDetailsVO item in BizActionObj.IssueList)
                                {
                                    //item.Quantity = 1;

                                    //item.ItemCode = str[0];
                                    //item.BatchCode = str[1];
                                    //this.ocIssueItemDetailsList.Add(item);






                                    //var result = from r in ((ItemListNew)sender).SelectedItems
                                    //             where r.ID == item.ItemID
                                    //             select r;

                                    String ItemCode = String.Empty, ItemName = String.Empty;

                                    //if (result.Count() > 0)
                                    //{
                                    //    ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
                                    //    ItemName = ((clsItemMasterVO)result.First()).ItemName;
                                    //}

                                    ItemCode = item.ItemCode;
                                    ItemName = item.ItemName;



                                    clsIssueItemDetailsVO IssueItem = new clsIssueItemDetailsVO
                                    {
                                        AvailableStock = Convert.ToDecimal(item.AvailableQuantity),
                                        BatchCode = item.BatchCode,
                                        BatchId = item.BatchID,
                                        ExpiryDate = item.ExpiryDate,
                                        ItemId = item.ItemID,
                                        MRP = Convert.ToDecimal(item.MRP),
                                        PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
                                        ItemName = ItemName,
                                        ItemCode = ItemCode,
                                        IssueQty = 1

                                        //ConversionFactor = item.ConversionFactor
                                    };

                                    if (ocIssueItemDetailsList.Where(issueItems => issueItems.ItemId == item.ItemID).Any() == true)
                                    {
                                        if (ocIssueItemDetailsList.Where(issueItems => issueItems.BatchId == item.BatchID).Any() == false)
                                            this.ocIssueItemDetailsList.Add(IssueItem);
                                    }
                                    else
                                        this.ocIssueItemDetailsList.Add(IssueItem);


                                }

                                dgItemList.ItemsSource = this.ocIssueItemDetailsList;
                            }
                        };
                        client.ProcessAsync(BizActionObj, new clsUserVO());
                        client.CloseAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            w.Close();
        }
        #endregion

        #region Clicked Events
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            chkToQuarantine.IsChecked = false;
            chkFromQuarantine.IsChecked = false;
            chkFromQuarantine.IsEnabled = true;
            chkToQuarantine.IsEnabled = true;
            cmbFromStore.IsEnabled = true;
            cmbToStore.IsEnabled = true;
            cmbToStore.ItemsSource = null;
            //cmbToStore.ItemsSource = StoreList.ToList();//StoreList.Where(s => s.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && s.Status == true);
            if (StoreList != null)
            {

                if (IsInterClinicIndent == true) //***//
                {
                var t = from r in StoreList.ToList() 
                        where r.Status == true && r.IsQuarantineStore == false
                        select r;

                if (t != null && t.ToList().Count > 0)
                {
                    cmbToStore.ItemsSource = (List<clsStoreVO>)t.ToList();
                    cmbToStore.SelectedItem = ((List<clsStoreVO>)t.ToList())[0];
                }

                }
                else
                {
                    var t = from r in StoreList.ToList()
                        where r.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && r.Status == true && r.IsQuarantineStore == false
                        select r;

                    if (t != null && t.ToList().Count > 0)
                    {
                        cmbToStore.ItemsSource = (List<clsStoreVO>)t.ToList();
                        cmbToStore.SelectedItem = ((List<clsStoreVO>)t.ToList())[0];
                    }
                }
                
            }
            cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            cmbReceivedBy.IsEnabled = false;
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdAccept_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;
            try
            {
                if (ClickedFlag1 == 1)
                {
                    if (Validation() == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                if (Indicatior == null) Indicatior = new WaitIndicator();
                                Indicatior.Show();
                                this.SumOfTotals();
                                var resultSelectedItem = from r in this.ocIssueItemDetailsList
                                                         where r.IssueQty > 0
                                                         select r;

                                clsAddIssueItemToStoreBizActionVO BizAction = new clsAddIssueItemToStoreBizActionVO();
                                BizAction.IssueItemDetails = new clsIssueItemVO(); ;
                                BizAction.IssueItemDetails.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();
                                if (SelectedIndent != null)
                                {
                                    BizAction.IssueItemDetails.IndentID = this.SelectedIndent.ID;
                                    BizAction.IssueItemDetails.IndentUnitID = this.SelectedIndent.UnitID;

                                    BizAction.IssueItemDetails.IsIndent = this.SelectedIndent.IsIndent;

                                    BizAction.IssueItemDetails.IsAgainstPatient = this.SelectedIndent.IsAgainstPatient;
                                    BizAction.IssueItemDetails.PatientID = this.SelectedIndent.PatientID;
                                    BizAction.IssueItemDetails.PatientUnitID = this.SelectedIndent.PatientUnitID;
                                }

                                //By Anjali........................................................
                                if (IsFromExpiredItem == true)
                                {
                                    BizAction.IssueItemDetails.IsIndent = 2;
                                    BizAction.IssueItemDetails.ReasonForIssue = 1;
                                }
                                if (rdbIsWithoutIndent.IsChecked == true)
                                {
                                    //BizAction.IssueItemDetails.IsIndent = 2;
                                    BizAction.IssueItemDetails.IsIndent = 6; //***//
                                    if (IsApprovedDirect == true)
                                    {
                                        BizAction.IssueItemDetails.IsApprovedDirect = false;
                                    }
                                    else
                                    {
                                        BizAction.IssueItemDetails.IsApprovedDirect = true;
                                    }
                                }

                                BizAction.IssueItemDetails.IsToStoreQuarantine = chkToQuarantine.IsChecked;
                                BizAction.IssueItemDetails.IsFromStoreQuarantine = chkFromQuarantine.IsChecked;
                                //.........................................................................
                                BizAction.IssueItemDetails.IssueDate = Convert.ToDateTime(dpIssueDate.SelectedDate);
                                BizAction.IssueItemDetails.IssueFromStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                                BizAction.IssueItemDetails.IssueID = 0;
                                BizAction.IssueItemDetails.IssueNumber = null;
                                BizAction.IssueItemDetails.IssueToStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                                if (cmbReceivedBy.SelectedItem != null)
                                    BizAction.IssueItemDetails.ReceivedById = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;
                                BizAction.IssueItemDetails.Remark = txtRemark.Text.Trim();
                                BizAction.IssueItemDetails.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim());
                                BizAction.IssueItemDetails.TotalItems = Convert.ToDecimal(txtNoOfItems.Text.Trim());
                                BizAction.IssueItemDetails.IssueItemDetailsList = resultSelectedItem.ToList<clsIssueItemDetailsVO>();

                                

                                foreach (var item in BizAction.IssueItemDetails.IssueItemDetailsList.ToList())
                                {
                                    item.MainMRP = Convert.ToSingle(item.MRP);   //Base MRP
                                    item.MainRate = Convert.ToSingle(item.PurchaseRate); //Base Rate
                                    if (rdbIsWithoutIndent.IsChecked == true) //***//
                                    {
                                        item.IsIndent = 6;
                                    }
                                }

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri); // SerRef.PalashServiceClient Client = new SerRef.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, e1) =>
                                {
                                    Indicatior.Close();
                                    ClickedFlag1 = 0;
                                    if (e1.Error == null && e1.Result != null)
                                    {
                                        if (((clsAddIssueItemToStoreBizActionVO)e1.Result).SuccessStatus == -10)
                                        {
                                            Indicatior.Close();
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Insufficient Stock ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW1.Show();
                                        }
                                        else if (((clsAddIssueItemToStoreBizActionVO)e1.Result).IssueItemDetails != null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", "Issue details saved successfully With Issue No. " + ((clsAddIssueItemToStoreBizActionVO)e1.Result).IssueItemDetails.IssueNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgWin.Show();
                                            FillIssueList();
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgWin.Show();
                                        }
                                    }
                                };
                                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                                objAnimation.Invoke(RotationType.Backward);

                                   txtMrno1.Text = String.Empty;
                                   txtName1.Text = String.Empty;
                            }
                        };
                        msgWindow.Show();
                        ClickedFlag1 = 0;
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                        ClickedFlag1 = 0;
                    }
                }
                else
                    ClickedFlag1 = 0;
            }
            catch (Exception)
            {
                Indicatior.Close();
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rdbIsIndent_Click(object sender, RoutedEventArgs e)
        {
            if (ocIssueItemDetailsList.Count > 0)
                ResetControls();
            if (rdbIsIndent.IsChecked == true)
            {
                cmdGetIndent.IsEnabled = true;
                cmdAddItem.IsEnabled = false;
                cmdGetPurchaseRequisition.IsEnabled = false;
                dgItemList.Columns[4].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[5].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[6].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[7].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[4].Header = "Indent Quantity";

            }
            else
            {
                cmdGetPurchaseRequisition.IsEnabled = true;
                cmdGetIndent.IsEnabled = false;
                cmdAddItem.IsEnabled = false;
                dgItemList.Columns[4].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[5].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[6].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[7].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[4].Header = "PR Quantity";
            }
        }

        private void rdbIsWithoutIndent_Click(object sender, RoutedEventArgs e)
        {
            if (ocIssueItemDetailsList.Count > 0)
                ResetControls();

            if (rdbIsWithoutIndent.IsChecked == true)
            {
                cmdGetIndent.IsEnabled = false;               
                cmdGetPurchaseRequisition.IsEnabled = false;
                if (IsDirectIndent == true) //***//
                {
                    cmdAddItem.IsEnabled = true;
                }
                else
                {
                    cmdAddItem.IsEnabled = false;
                }
                SelectedIndent = null;
                dgItemList.Columns[4].Visibility = System.Windows.Visibility.Collapsed;
                dgItemList.Columns[5].Visibility = System.Windows.Visibility.Collapsed;
                dgItemList.Columns[6].Visibility = System.Windows.Visibility.Collapsed;
                dgItemList.Columns[7].Visibility = System.Windows.Visibility.Collapsed;

            }
            else
            {
                cmdGetIndent.IsEnabled = true;
                cmdAddItem.IsEnabled = false;
                cmdGetPurchaseRequisition.IsEnabled = false;

            }
        }

        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFromStore.SelectedItem != null && ((clsStoreVO)cmbFromStore.SelectedItem).StoreId != 0)
            {
                ItemListNew win = new ItemListNew();
                win.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                win.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                win.cmbStore.IsEnabled = false;
                win.ClinicID = ((clsStoreVO)cmbFromStore.SelectedItem).ClinicId;
                win.StoreID = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);

                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select From Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
            }


        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (dgItemList.ItemsSource == null)
                dgItemList.ItemsSource = this.ocIssueItemDetailsList;

            //foreach (clsItemStockVO item in (((ItemList)sender).SelectedBatches))
            //{
            //    //(List<clsItemMasterVO>(((ItemList)sender).SelectedItems).Where(q => q.ItemID == item.ItemID));

            //    //String ItemCode = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemCode;
            //    //String ItemName = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemName;

            //    //String ItemName = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;
            //    //String ItemCo = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;

            //    var result = from r in ((ItemList)sender).SelectedItems
            //                 where r.ID == item.ItemID
            //                 select r;

            //    String ItemCode = String.Empty, ItemName = String.Empty;

            //    if (result.Count() > 0)
            //    {
            //        ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
            //        ItemName = ((clsItemMasterVO)result.First()).ItemName;
            //    }


            //    clsIssueItemDetailsVO IssueItem = new clsIssueItemDetailsVO
            //    {
            //        AvailableStock = Convert.ToDecimal(item.AvailableStock),
            //        BatchCode = item.BatchCode,
            //        BatchId = item.BatchID,
            //        ExpiryDate = item.ExpiryDate,
            //        ItemId = item.ItemID,
            //        MRP = Convert.ToDecimal(item.MRP),
            //        PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
            //        ItemName = ItemName,
            //        ItemCode = ItemCode,
            //        IssueQty=0
            //        //ConversionFactor = item.ConversionFactor
            //    };

            foreach (var item in (((ItemListNew)sender).ItemBatchList))
            {
                //(List<clsItemMasterVO>(((ItemList)sender).SelectedItems).Where(q => q.ItemID == item.ItemID));

                //String ItemCode = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemCode;
                //String ItemName = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemName;

                //String ItemName = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;
                //String ItemCo = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;

                //var result = from r in ((ItemList)sender).SelectedItems
                //             where r.ID == item.ItemID
                //             select r;

                String ItemCode = String.Empty, ItemName = String.Empty;

                //if (result.Count() > 0)
                //{
                //    ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
                //    ItemName = ((clsItemMasterVO)result.First()).ItemName;
                //}

                ItemCode = item.ItemCode;
                ItemName = item.ItemName;
                IsApprovedDirect = item.IsApprovedDirect;
                clsIssueItemDetailsVO IssueItem = new clsIssueItemDetailsVO
                {
                    AvailableStock = Convert.ToDecimal(item.AvailableStock),
                    BatchCode = item.BatchCode,
                    BatchId = item.BatchID,
                    ExpiryDate = item.ExpiryDate,
                    ItemId = item.ItemID,
                    MRP = Convert.ToDecimal(item.MRP),
                    PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
                    ItemName = ItemName,
                    ItemCode = ItemCode,
                    IssueQty = 0,
                    IsIndent = 2
                    ,
                    ReasonForIssueList = RessonIssue
                    ,
                    SelectedReasonForIssue = RessonIssue.FirstOrDefault(p => p.ID == 0)
                    ,
                    Re_Order = item.Re_Order,

                    //..........By Anjali.............................
                    SUOMID = item.SUOMID,
                    SUOM = item.SUOM,
                    //obj.UOMID = item.StockUOMID;
                    AvailableStockUOM = item.SUOM,
                    SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM),
                    ConversionFactor = 1,
                    BaseUOM = item.BaseUMString,
                    BaseUOMID = item.BaseUM,
                    BaseQuantity = 0,
                    BaseConversionFactor = item.StockingToBaseCF
                    //.........................................




                    //ConversionFactor = item.ConversionFactor
                };
                if (ocIssueItemDetailsList.Where(issueItems => issueItems.ItemId == item.ItemID).Any() == true)
                {
                    if (ocIssueItemDetailsList.Where(issueItems => issueItems.BatchId == item.BatchID).Any() == false)
                        this.ocIssueItemDetailsList.Add(IssueItem);
                }
                else
                    this.ocIssueItemDetailsList.Add(IssueItem);
            }

        }

        private void cmdGetIndent_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedIndent = new clsIndentMasterVO();
            IndnetSearch1 win = new IndnetSearch1();
            win.IsOpenFromPO = false;
            //By Anjali...........................
            win.IsIndent = 1;
            // win.IsIndent = true;
            //............................................
            if (cmbToStore.SelectedItem != null)
                win.IndentFromStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
            if (cmbFromStore.SelectedItem != null)
                win.IndentToStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;

            win.OnItemSelectionCompleted += new IndnetSearch1.ItemSelection(win_OnItemSelectionCompleted);
            win.Show();

        }

        void win_OnItemSelectionCompleted(object sender, EventArgs e)
        {

            if (this.SelectedIndent.ID != ((IndnetSearch1)sender).SelectedIndent.ID)
            {
                this.ocIssueItemDetailsList.Clear();
                this.SelectedIndent = ((IndnetSearch1)sender).SelectedIndent;

                foreach (var item in ((IndnetSearch1)sender).ocSelectedItemDetailsList)
                {
                    clsIssueItemDetailsVO obj = new clsIssueItemDetailsVO();
                    obj = item;

                    obj.ReasonForIssueList = RessonIssue;
                    obj.SelectedReasonForIssue = RessonIssue.FirstOrDefault(p => p.ID == 0);
                    obj.IsIndent = item.IsIndent;

                    obj.UOMID = item.UOMID;
                    obj.UOM = item.UOM;
                    obj.BaseUOMID = item.BaseUOMID;
                    obj.BaseUOM = item.BaseUOM;
                    obj.SUOMID = item.SUOMID;
                    obj.SUOM = item.SUOM;

                    if (obj.IsIndent == 0)
                    {
                        //obj.AvailableStock = (obj.AvailableStock / Convert.ToDecimal(obj.ConversionFactor));  //commented by Ashish becaze AvailableStock is already in Stock UOM (SP-CIMS_GetBatchListForIndentItemIdForIssueItem)
                        obj.AvailableStock = Convert.ToDecimal(Math.Floor(Convert.ToDouble(obj.AvailableStock)));
                        obj.PurchaseRate = item.PurchaseRate * Convert.ToDecimal(item.ConversionFactor);
                        obj.MRP = item.MRP * Convert.ToDecimal(item.ConversionFactor);
                    }

                    this.ocIssueItemDetailsList.Add(obj);

                }
                //By Anjali...............................................
                //if (SelectedIndent.IsIndent == true)
                //{
                //    txtIndentNumber.Text = this.SelectedIndent.IndentNumber;
                //}
                //else
                //{
                //    txtPRNumber.Text = this.SelectedIndent.IndentNumber;
                //}
                if (SelectedIndent.IsIndent == 1)
                {
                    txtIndentNumber.Text = this.SelectedIndent.IndentNumber;
                }
                else if (SelectedIndent.IsIndent == 0)
                {
                    txtPRNumber.Text = this.SelectedIndent.IndentNumber;
                }
                //...................................................

                if (this.SelectedIndent.MRNo != null)
                {
                    txtMrno1.Text = this.SelectedIndent.MRNo;
                }
                if (this.SelectedIndent.PatientName != "")
                {
                    txtName1.Text = this.SelectedIndent.PatientName;
                }

                dgItemList.ItemsSource = this.ocIssueItemDetailsList;

            }

            else
            {

                foreach (clsIssueItemDetailsVO item in ((IndnetSearch1)sender).ocSelectedItemDetailsList)
                {
                    if (this.ocIssueItemDetailsList.Where(q => q.BatchId == item.BatchId).Count() == 0)
                    {
                        this.ocIssueItemDetailsList.Add(item);
                    }
                }
            }

            flagResetControls = true;

            var results = from r in ((List<clsStoreVO>)cmbFromStore.ItemsSource).ToList()
                          where r.StoreId == this.SelectedIndent.ToStoreID
                          select r;

            foreach (clsStoreVO item in results)
            {
                cmbFromStore.SelectedItem = item;


            }

            var results1 = from r in ((List<clsStoreVO>)cmbToStore.ItemsSource).ToList()
                           where r.StoreId == this.SelectedIndent.FromStoreID
                           select r;

            foreach (clsStoreVO item in results1)
            {
                cmbToStore.SelectedItem = item;
                //     cmbToStore.IsEnabled = false;
            }

            flagResetControls = false;

            this.SumOfTotals();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            txtMrno1.Text = String.Empty;
            txtName1.Text = String.Empty;
            objAnimation.Invoke(RotationType.Backward);
            IsFromExpiredItem = false;
            this.Loaded += new RoutedEventHandler(IssueToClinic_Loaded);
        }

        private void cmdSearchIssue_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFrontPannelSearch())
            {
                FillIssueList();
                dgdpIssueList.PageIndex = 0;
            }
        }

        private void cmdprintreport_Click(object sender, RoutedEventArgs e)
        {
            if (dgIssueList.SelectedItem != null)
            {
                long iUnitID = 0;
                long? iIssueID = 0;
                iIssueID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueId;
                iUnitID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueUnitID;
                string URL = "../Reports/InventoryPharmacy/ItemIssuePrint.aspx?IssueID=" + iIssueID + "&UnitID=" + iUnitID;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/ItemIssuePrint.aspx?IssueID=" + ((clsIssueListVO)dgIssueList.SelectedItem).IssueId), "_blank");
            }
        }

        private void cmdDeleteItemList_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ocIssueItemDetailsList.RemoveAt(dgItemList.SelectedIndex);
                        SumOfTotals();
                        dgItemList.ItemsSource = null;
                        dgItemList.ItemsSource = ocIssueItemDetailsList;
                        dgItemList.UpdateLayout();
                    }
                };

                msgWD.Show();
            }
        }

        private void cmdGetPurchaseRequisition_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedIndent = new clsIndentMasterVO();
            IndentSearch win = new IndentSearch();
            win.IsOpenFromPO = false;
            //By Anjali.........................
            // win.IsIndent = false;
            win.IsIndent = 0;
            //......................................
            if (cmbToStore.SelectedItem != null)
                win.IndentFromStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
            if (cmbFromStore.SelectedItem != null)
                win.IndentToStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;

            win.OnItemSelectionCompleted += new IndentSearch.ItemSelection(win_OnItemSelectionCompleted2);
            win.Show();
        }

        void win_OnItemSelectionCompleted2(object sender, EventArgs e)
        {


            if (this.SelectedIndent.ID != ((IndentSearch)sender).SelectedIndent.ID)
            {
                this.ocIssueItemDetailsList.Clear();
                this.SelectedIndent = ((IndentSearch)sender).SelectedIndent;

                foreach (var item in ((IndentSearch)sender).ocSelectedItemDetailsList)
                {
                    clsIssueItemDetailsVO obj = new clsIssueItemDetailsVO();
                    obj = item;

                    obj.ReasonForIssueList = RessonIssue;
                    obj.SelectedReasonForIssue = RessonIssue.FirstOrDefault(p => p.ID == 0);
                    obj.IsIndent = item.IsIndent;

                    if (obj.IsIndent == 0)//obj.IsIndent == false
                    {
                        obj.AvailableStock = (obj.AvailableStock);
                        //obj.AvailableStock = Convert.ToDecimal(Math.Floor(Convert.ToDouble(obj.AvailableStock)));
                        obj.PurchaseRate = item.PurchaseRate;
                        obj.MRP = item.MRP; //item.MRP * Convert.ToDecimal(item.ConversionFactor);
                    }
                    this.ocIssueItemDetailsList.Add(obj);

                }
                //By Anjali.........................................
                //if (SelectedIndent.IsIndent == true)
                //{
                //    txtIndentNumber.Text = this.SelectedIndent.IndentNumber;
                //}
                //else
                //{
                //    txtPRNumber.Text = this.SelectedIndent.IndentNumber;
                //}
                if (SelectedIndent.IsIndent == 1)
                {
                    txtIndentNumber.Text = this.SelectedIndent.IndentNumber;
                }
                else if (SelectedIndent.IsIndent == 0)
                {
                    txtPRNumber.Text = this.SelectedIndent.IndentNumber;
                }
                //........................................................

                dgItemList.ItemsSource = this.ocIssueItemDetailsList;

            }

            else
            {

                foreach (clsIssueItemDetailsVO item in ((IndentSearch)sender).ocSelectedItemDetailsList)
                {
                    if (this.ocIssueItemDetailsList.Where(q => q.BatchId == item.BatchId).Count() == 0)
                    {
                        this.ocIssueItemDetailsList.Add(item);
                    }
                }
            }

            flagResetControls = true;

            var results = from r in ((List<clsStoreVO>)cmbFromStore.ItemsSource)
                          where r.StoreId == this.SelectedIndent.ToStoreID
                          select r;

            foreach (clsStoreVO item in results)
            {
                cmbFromStore.SelectedItem = item;


            }

            var results1 = from r in ((List<clsStoreVO>)cmbToStore.ItemsSource)
                           where r.StoreId == this.SelectedIndent.FromStoreID
                           select r;

            foreach (clsStoreVO item in results1)
            {
                cmbToStore.SelectedItem = item;
                //     cmbToStore.IsEnabled = false;
            }

            flagResetControls = false;

            this.SumOfTotals();
        }

        private void rdbIsPurchaseRequisition_Click(object sender, RoutedEventArgs e)
        {
            if (ocIssueItemDetailsList.Count > 0)
                ResetControls();
            if (rdbIsIndent.IsChecked == true)
            {
                cmdGetPurchaseRequisition.IsEnabled = false;
                cmdGetIndent.IsEnabled = true;
                cmdAddItem.IsEnabled = false;
                dgItemList.Columns[4].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[5].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[6].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[7].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[4].Header = "Indent Quantity";
            }
            else
            {
                cmdGetPurchaseRequisition.IsEnabled = true;
                cmdGetIndent.IsEnabled = false;
                cmdAddItem.IsEnabled = false;
                dgItemList.Columns[4].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[5].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[6].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[7].Visibility = System.Windows.Visibility.Visible;
                dgItemList.Columns[4].Header = "PR Quantity";
            }

        }

        private void chkFromQuarantine_Click(object sender, RoutedEventArgs e)
        {
            //if (IsFromExpiredItem != true)
            //{
            //    if (chkFromQuarantine.IsChecked == true)
            //    {
            //        cmbFromStore.ItemsSource = null;
            //        cmbToStore.ItemsSource = null;
            //        cmbFromStore.ItemsSource = StoreList.Where(s => s.IsQuarantineStore == true);
            //        cmbToStore.ItemsSource = StoreList;
            //        cmbToStore.SelectedItem = StoreList[0];
            //        chkToQuarantine.IsChecked = false;
            //    }

            //}
        }

        private void rdbIsIndent_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void rdbIsIndent_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void chkToQuarantine_Click(object sender, RoutedEventArgs e)
        {
            if (IsFromExpiredItem != true)
            {
                if (chkToQuarantine.IsChecked == true)
                {

                    cmbToStore.ItemsSource = null;
                    cmbToStore.ItemsSource = StoreList.Where(s => s.IsQuarantineStore == true);
                    if (cmbFromStore.SelectedItem != null && ((clsStoreVO)cmbFromStore.SelectedItem).StoreId > 0)
                        cmbToStore.SelectedValue = StoreList.Where(s => s.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && s.Parent == ((clsStoreVO)cmbFromStore.SelectedItem).StoreId && s.IsQuarantineStore == true).SingleOrDefault().StoreId;
                    chkFromQuarantine.IsChecked = false;
                    cmbToStore.IsEnabled = false;
                }
                else if (chkToQuarantine.IsChecked == false)
                {
                    cmbToStore.ItemsSource = null;
                    cmbToStore.ItemsSource = StoreList.Where(s => s.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && s.Status == true && s.IsQuarantineStore != true);//s.IsQuarantineStore != true
                    cmbToStore.SelectedValue = (long)0;
                    cmbToStore.IsEnabled = true;
                }

            }
        }
        #endregion

        #region KeyDown/LostFocus/SelectionChanged/Other Events
        private void cmbFromStore_KeyDown(object sender, KeyEventArgs e)
        {
            // e.Handled = false;
        }

        private void txtIssueItem_KeyDown(object sender, KeyEventArgs e)
        {
            //ApplyRulesForIssueItem();

            SelectedRowItem = ((clsIssueItemDetailsVO)dgItemList.SelectedItem);
            SelectedRowItemIndex = dgItemList.SelectedIndex;
        }

        private void dgItemList_KeyUp(object sender, KeyEventArgs e)
        {
            //  ApplyRulesForIssueItem();
        }

        private void txtIssueItem_LostFocus(object sender, RoutedEventArgs e)
        {
            // this.ApplyRulesForIssueItem();
        }

        private void cmbFromStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Added By Pallavi
            if (!flagResetControls)
                //
                ResetControls1();
            else
            {
                if (rdbIsWithoutIndent.IsChecked == true)
                    ocIssueItemDetailsList.Clear();
                if (cmbFromStore.SelectedItem != null)
                {
                    StoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                    if (cmbToStore.SelectedItem != null && ((clsStoreVO)cmbToStore.SelectedItem).ID > 0 && ((clsStoreVO)cmbFromStore.SelectedItem).ID > 0)
                    {
                        if (((clsStoreVO)cmbToStore.SelectedItem).IsQuarantineStore == ((clsStoreVO)cmbFromStore.SelectedItem).IsQuarantineStore)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Quarantine Store To Quarantine Store Transfer Is Not possible", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWin.Show();
                            cmbToStore.SelectedItem = StoreList[0];
                            cmbFromStore.SelectedItem = StoreList[0];
                            chkToQuarantine.IsChecked = false;
                            chkFromQuarantine.IsChecked = false;
                        }
                    }
                }
                flagResetControls = true;
            }

        }

        private void cmbToStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Added By Pallavi
            if (!flagResetControls)
                //
                ResetControls1();
            else
                flagResetControls = true;

        }

        private void dgItemList_KeyDown(object sender, KeyEventArgs e)
        {
            //ApplyRulesForIssueItem();
        }

        private void dgIssueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillItemListByIssueId();
        }

        private void txtBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //if (cmbStore.SelectedItem != null && ((MasterListItem)cmbStore.SelectedItem).ID != 0)
                //{



                // }

                fillItemGridOnBar();
                SumOfTotals();
                //CalculatePharmacySummary();
                txtBarCode.Focus();
                //}
                //else
                //{
                //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select From Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    mgbx.Show();
                //}
                txtBarCode.Text = "";

            }
        }

        private void cmbReasonForIssue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ocIssueItemDetailsList.Count; i++)
            {
                if (ocIssueItemDetailsList[i] == ((clsIssueItemDetailsVO)dgItemList.SelectedItem))
                {
                    if (((MasterListItem)((AutoCompleteBox)sender).SelectedItem) != null)
                    {
                        ocIssueItemDetailsList[i].ReasonForIssue = ((MasterListItem)((AutoCompleteBox)sender).SelectedItem).ID;
                    }
                }
            }
        }

        private void dgItemList_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void txtIssueNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FillIssueList();
                dgdpIssueList.PageIndex = 0;
            }
        }
        #endregion

        #region DataGrid CellEditEnded Event
        private void dgItemList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                clsIssueItemDetailsVO objVO = new clsIssueItemDetailsVO();
                objVO = dgItemList.SelectedItem as clsIssueItemDetailsVO;

                if (e.Column.Header.ToString().Equals("Issued Quantity") || e.Column.Header.ToString().Equals("Issued UOM"))
                {
                    if (objVO.IssueQty < 0)
                    {
                        objVO.IssueQty = 0;
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return;
                        //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                    }
                    objVO.IssueQty = System.Math.Round(objVO.IssueQty, 1);
                    if (((int)objVO.IssueQty).ToString().Length > 5)
                    {
                        objVO.IssueQty = 1;
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Issued Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return;
                        //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                    }
                    if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                    {
                        if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID == objVO.BaseUOMID && (objVO.IssueQty % 1) != 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Issued Quantity Cannot Be In Fraction for Base UOM", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbx.Show();
                            objVO.IssueQty = 0;
                            objVO.ItemTotalAmount = 0;
                            return;
                        }

                        (dgItemList.SelectedItem as clsIssueItemDetailsVO).StockingQuantity = Convert.ToDouble(objVO.IssueQty * Convert.ToDecimal(objVO.ConversionFactor));
                        (dgItemList.SelectedItem as clsIssueItemDetailsVO).BaseQuantity = Convert.ToSingle(objVO.IssueQty) * objVO.BaseConversionFactor;
                        (dgItemList.SelectedItem as clsIssueItemDetailsVO).ItemTotalAmount = Convert.ToDecimal((dgItemList.SelectedItem as clsIssueItemDetailsVO).BaseQuantity) * (dgItemList.SelectedItem as clsIssueItemDetailsVO).PurchaseRate;

                        if (Convert.ToDecimal(objVO.BaseQuantity) > objVO.IssuePendingQuantity)  //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Indent Pending Quantity. Please Enter Issue Quantity Less Than Or Equal To Indent Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            (dgItemList.SelectedItem as clsIssueItemDetailsVO).IssueQty = 0;
                            (dgItemList.SelectedItem as clsIssueItemDetailsVO).ItemTotalAmount = 0;
                            return;
                        }

                        if (Convert.ToDecimal(objVO.StockingQuantity) > objVO.AvailableStock) // (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                              new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Available Stock. Please Enter Issue Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            (dgItemList.SelectedItem as clsIssueItemDetailsVO).IssueQty = 0;
                            (dgItemList.SelectedItem as clsIssueItemDetailsVO).ItemTotalAmount = 0;
                            return;
                        }

                    }
                    else
                    {
                        // Function Parameters
                        // FromUOMID - Transaction UOM
                        // ToUOMID - Stocking UOM
                        CalculateConversionFactorCentral(objVO.SelectedUOM.ID, objVO.SUOMID);
                    }
                }
                this.SumOfTotals();
            }


            #region Commented by Ashish Z. for Issued UOM HyperLink
            //// this.ApplyRulesForIssueItem();
            //if (e.Column.DisplayIndex == 10 || e.Column.DisplayIndex == 11) //Issue Qunatity
            //{
            //    if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM != null && ((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID > 0)
            //    {
            //        // Function Parameters
            //        // FromUOMID - Transaction UOM
            //        // ToUOMID - Stocking UOM
            //        CalculateConversionFactorCentral(((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID, ((clsIssueItemDetailsVO)dgItemList.SelectedItem).SUOMID);
            //    }
            //    else
            //    {
            //        ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity = 0;
            //        ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ConversionFactor = 0;
            //        ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor = 0;
            //        ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemTotalAmount = 0;
            //    }

            //    //if (rdbIsIndent.IsChecked == true || rdbIsPurchaseRequisition.IsChecked == true)
            //    //{
            //    //    if (Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity) > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock) // (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
            //    //    {
            //    //        MessageBoxControl.MessageBoxChildWindow msgW3 =
            //    //                          new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Available Stock. Please Enter Issue Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    //        msgW3.Show();
            //    //        if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock == 0)
            //    //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
            //    //        else
            //    //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
            //    //        // return;
            //    //    }
            //    //}

            //    //if (rdbWithIndent.IsChecked == true)
            //    //{
            //    //    if (Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).StockingQuantity) > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock) //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock
            //    //    {
            //    //        MessageBoxControl.MessageBoxChildWindow msgW3 =
            //    //                          new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Available Stock. Please Enter Issue Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    //        msgW3.Show();
            //    //        if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock == 0)
            //    //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
            //    //        else
            //    //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock;
            //    //        // return;
            //    //    }
            //    //}
            //    if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity == 0)
            //    {
            //        if (!flagZeroTransit)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                                  new MessageBoxControl.MessageBoxChildWindow("Can't Enter Issue Quantity!", "Indent quantity is already issued!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            msgW3.Show();
            //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
            //            return;
            //        }
            //    }

            //    if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IndentQty != 0 && ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IndentQty != null)
            //    {
            //        if (Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity) > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity)  //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                         new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Indent Pending Quantity. Please Enter Issue Quantity Less Than Or Equal To Indent Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            msgW3.Show();
            //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
            //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemTotalAmount = 0;
            //            //((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID = 0;
            //            return;
            //        }
            //    }

            //    if (rdbIsIndent.IsChecked == true || rdbIsPurchaseRequisition.IsChecked == true)
            //    {
            //        if (Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).StockingQuantity) > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock) // (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                              new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Available Stock. Please Enter Issue Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            msgW3.Show();
            //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
            //            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemTotalAmount = 0;
            //            //((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID = 0;
            //            return;
            //        }
            //    }

            //    //foreach (clsIssueItemDetailsVO item in ocIssueItemDetailsList)
            //    //{
            //    //    if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemId == item.ItemId && ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BatchId != item.BatchId)
            //    //    {
            //    //        if (item.IssueQty != 0)
            //    //        {

            //    //        }

            //    //    }
            //    //}
            //}
            //if (e.Column.DisplayIndex == 10)
            //{
            //    if (((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock < Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).Re_Order))
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Item Is Already Under Reorder Level", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        msgW3.Show();

            //    }
            //    else if ((((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock - ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty) < Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).Re_Order))
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Item Quantity Is Reaching Reorder Level", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        msgW3.Show();
            //    }
            //}
            //this.SumOfTotals();
            #endregion
        }

        private void dgItemList_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            //List<clsIssueItemDetailsVO> t = new List<clsIssueItemDetailsVO>();
            //t= ocIssueItemDetailsList.ToList<clsIssueItemDetailsVO>();
            // this.ocIssueItemDetailsListBeforeEditing = this.ocIssueItemDetailsList;
        }

        private void dgItemList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //ApplyRulesForIssueItem();
        }

        private void dgItemList_CurrentCellChanged(object sender, EventArgs e)
        {
            //ApplyRulesForIssueItem();
        }
        #endregion

        #region Conversion Factor
        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsIssueItemDetailsVO)dgItemList.SelectedItem).UOMConversionList == null || ((clsIssueItemDetailsVO)dgItemList.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemId;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        Indicatior.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        //List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                        //List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                        //MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());



                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgItemList.SelectedItem != null)
                        {
                            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsIssueItemDetailsVO)dgItemList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }


                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }


        }

        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty = 0;
                ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsIssueItemDetailsVO)dgItemList.SelectedItem).SUOMID);
                }
                else
                {

                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ConversionFactor = 0;
                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor = 0;

                    //((clsIssueItemDetailsVO)dgItemList.SelectedItem).MRP = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).MainMRP;
                    //(((clsIssueItemDetailsVO)grdStoreIndent.SelectedItem).Rate = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).MainRate;
                }
            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgItemList.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    //objConversionVO.MainMRP = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).MainMRP;
                    //objConversionVO.MainRate = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty);

                    long BaseUOMID = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //((clsIssueItemDetailsVO)dgItemList.SelectedItem).Rate = objConversionVO.Rate;
                    //((clsIssueItemDetailsVO)dgItemList.SelectedItem).MRP = objConversionVO.MRP;


                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity = objConversionVO.Quantity;
                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).StockingQuantity = objConversionVO.Quantity;
                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    ((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemTotalAmount = Convert.ToDecimal(((clsIssueItemDetailsVO)dgItemList.SelectedItem).BaseQuantity) * ((clsIssueItemDetailsVO)dgItemList.SelectedItem).PurchaseRate;

                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsIssueItemDetailsVO)dgItemList.SelectedItem).ItemId), ((clsIssueItemDetailsVO)dgItemList.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnCFSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnCFSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;
            clsIssueItemDetailsVO objVO = new clsIssueItemDetailsVO();
            objVO = dgItemList.SelectedItem as clsIssueItemDetailsVO;
            if (objVO != null)
            {
                objVO.SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
                objVO.UOMList = Itemswin.UOMConvertLIst;
                objVO.UOMConversionList = Itemswin.UOMConversionLIst;

                CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, objVO.SUOMID);

                if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                {
                    objVO.StockingQuantity = Convert.ToDouble(objVO.IssueQty * Convert.ToDecimal(objVO.ConversionFactor));
                    objVO.BaseQuantity = Convert.ToSingle(objVO.IssueQty) * objVO.BaseConversionFactor;

                    if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID == objVO.BaseUOMID && (objVO.IssueQty % 1) != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Issued Quantity Cannot Be In Fraction for Base UOM", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        objVO.IssueQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }

                    if (Convert.ToDecimal(objVO.BaseQuantity) > objVO.IssuePendingQuantity)  //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Indent Pending Quantity. Please Enter Issue Quantity Less Than Or Equal To Indent Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.IssueQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }

                    if (Convert.ToDecimal(objVO.StockingQuantity) > objVO.AvailableStock) // (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                          new MessageBoxControl.MessageBoxChildWindow("Greater Issue Quantity!", "Issued Quantity In The List Can't Be Greater Than Available Stock. Please Enter Issue Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.IssueQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }
                }
                else
                {
                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    CalculateConversionFactorCentral(objVO.SelectedUOM.ID, objVO.SUOMID);
                }
            }
            this.SumOfTotals();
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void txtMrno_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmdSearchIssue_Click(sender, e);

            }
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtName.Text = txtName.Text.ToTitleCase();
        }

        int selectionStart = 0;
        int selectionLength = 0;
        string textBefore = null;
        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        clsUserRightsVO objUser;
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                        
                        if (objUser.IsDirectIndent)
                        {
                            this.IsDirectIndent = true;
                            rdbIsWithoutIndent.IsEnabled = true;
                        }
                        else
                        {
                            this.IsDirectIndent = false;
                        }

                        if (objUser.IsInterClinicIndent)
                        {
                            this.IsInterClinicIndent = true;                           
                        }
                        else
                        {
                            this.IsInterClinicIndent = false;
                        }
                        
                    }                    
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
        }
    }
}
