using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pharmacy
{
    public partial class SearchIssue : ChildWindow
    {
        public delegate void ItemSelection(object sender, EventArgs e);
        public event ItemSelection OnItemSelectionCompleted;

        public long? IssueFromStoreId { get; set; }
        public long? IssueToStoreId { get; set; }
        public clsIssueListVO SelectedIssue { get; set; }
        public PagedSortableCollectionView<clsIssueListVO> IssueDataList { get; private set; }

        public long PatientID = 0;
        public long PatientunitID = 0;
        public bool IsAgainstPatient = false;

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
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        public SearchIssue()
        {
            InitializeComponent();
            IssueDataList = new PagedSortableCollectionView<clsIssueListVO>();
            IssueDataList.OnRefresh += new EventHandler<RefreshEventArgs>(IssueDataList_OnRefresh);
            DataListPageSize = 15;
            dgdpIssueList.PageSize = DataListPageSize;
            dgdpIssueList.Source = IssueDataList;
            this.Loaded += new RoutedEventHandler(SearchIssue_Loaded);
        }
        void SearchIssue_Loaded(object sender, RoutedEventArgs e)
        {

            FillStore();
        }

        void IssueDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillIssueList();
        }
        private void FillStore()
        {

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;


                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var NonQSAndUserAssignedStores = from item in BizActionObj.ToStoreList.ToList()
                                                     where item.IsQuarantineStore == false
                                                     select item;

                    NonQSAndUserAssignedStores.ToList().Insert(0, Default);


                    cmbIssueToStoreSrch.ItemsSource = NonQSAndUserAssignedStores.ToList();//(List<clsStoreVO>)result.ToList();

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;
                    //cmbIssueFromStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();

                    var res2 = from r in BizActionObj.ItemMatserDetails
                               where r.Status == true
                               select r;

                    cmbIssueFromStoreSrch.ItemsSource = res2.ToList();

                    if (IssueToStoreId != null && IssueToStoreId != 0)
                    {
                        //var res = from r in result.ToList()
                        //          where r.StoreId == IssueToStoreId
                        //          select r;

                        var res = from r in res2.ToList()
                                  where r.StoreId == IssueToStoreId
                                  select r;

                        cmbIssueFromStoreSrch.SelectedItem = ((clsStoreVO)res.First());
                    }
                    else
                    {
                        //cmbIssueFromStoreSrch.SelectedItem = result.ToList()[0];
                        cmbIssueFromStoreSrch.SelectedItem = res2.ToList()[0];
                    }

                    if (IssueFromStoreId != null && IssueFromStoreId != 0)
                    {
                        var res = from r in NonQSAndUserAssignedStores.ToList()
                                  where r.StoreId == IssueFromStoreId
                                  select r;

                        cmbIssueToStoreSrch.SelectedItem = ((clsStoreVO)res.First());
                    }
                    else
                    {
                        cmbIssueToStoreSrch.SelectedItem = NonQSAndUserAssignedStores.ToList()[0];
                    }
                }



                //if (args.Error == null && args.Result != null)
                //{
                //    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;



                //    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                //    BizActionObj.ItemMatserDetails.Insert(0, Default);


                //    var res2 = from r in BizActionObj.ItemMatserDetails
                //              where  r.Status == true
                //              select r;

                //    cmbIssueFromStoreSrch.ItemsSource = res2.ToList();
                //    clsStoreVO Default1 = new clsStoreVO { StoreId = 0, StoreName = "--Select--",ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId,Status = true };
                //    //Change by harish 17 apr


                //    if (IssueToStoreId != null &&  IssueToStoreId != 0)
                //    {                                                                        
                //        //var res = from r in BizActionObj.ItemMatserDetails
                //        //          where r.StoreId == IssueToStoreId
                //        //          select r;

                //        var res = from r in res2.ToList()
                //                  where r.StoreId == IssueToStoreId 
                //                  select r;

                //        cmbIssueFromStoreSrch.SelectedItem = ((clsStoreVO)res.First());
                //    }
                //    else
                //    {


                //        //BizActionObj.ItemMatserDetails.Insert(0, Default);
                //        //cmbIssueFromStoreSrch.SelectedItem = BizActionObj.ItemMatserDetails[0];

                //        cmbIssueFromStoreSrch.SelectedItem = res2.ToList()[0];

                //    }


                //    var result = from item in BizActionObj.ItemMatserDetails
                //                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                //                 select item;


                //    cmbIssueToStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList();

                //    if (IssueFromStoreId != null && IssueFromStoreId!=0)
                //    {
                //        var res = from r in (List<clsStoreVO>)result.ToList()
                //                  where r.StoreId == IssueFromStoreId
                //                  select r;

                //        cmbIssueToStoreSrch.SelectedItem = ((clsStoreVO)res.First());
                //    }
                //    else
                //    {
                //        //clsStoreVO Default1 = new clsStoreVO { StoreId = 0, StoreName = "--Select--" };

                //        //BizActionObj.ItemMatserDetails.Insert(0, Default);
                //        //cmbIssueToStoreSrch.SelectedItem = BizActionObj.ItemMatserDetails[0];
                //        cmbIssueToStoreSrch.SelectedItem = res2.ToList()[0];

                //    }
                //}

            };

            client.CloseAsync();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ocSelectedItemDetailsList.Count() > 0)
            {
                if (SelectedIssue == null)
                    SelectedIssue = new clsIssueListVO();

                this.SelectedIssue = (clsIssueListVO)dgSearchIssueList.SelectedItem;


                OnItemSelectionCompleted(this, e);
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdDisplay_Click(object sender, RoutedEventArgs e)
        {

        }
        private ObservableCollection<clsItemListByIssueId> _ocSelectedItemList = new ObservableCollection<clsItemListByIssueId>();
        public ObservableCollection<clsItemListByIssueId> ocSelectedItemList //clsGetItemListByIssueIdBizActionVO
        {
            get
            {
                return _ocSelectedItemList;
            }
            set
            {
                _ocSelectedItemList = value;
            }
        }
        private ObservableCollection<clsReturnItemDetailsVO> _ocSelectedItemDetailsList = new ObservableCollection<clsReturnItemDetailsVO>();
        public ObservableCollection<clsReturnItemDetailsVO> ocSelectedItemDetailsList
        {
            get
            {
                return _ocSelectedItemDetailsList;
            }
            set
            {
                _ocSelectedItemDetailsList = value;
            }
        }
        /// <summary>
        /// Fills Issue list in grid according to stores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;

                if (cmbIssueFromStoreSrch.SelectedItem == null || ((clsStoreVO)cmbIssueFromStoreSrch.SelectedItem).StoreId == 0)
                {
                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                }
                else if (cmbIssueToStoreSrch.SelectedItem == null || ((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId == 0)
                {
                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                }
                else
                    FillIssueList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fills Issue list
        /// </summary>
        private void FillIssueList()
        {

            try
            {
                clsGetIssueListBizActionVO BizAction = new clsGetIssueListBizActionVO();
                BizAction.IssueList = new List<clsIssueListVO>();
                BizAction.IsFromReceiveIssue = false;
                BizAction.IndentCriteria = 4;
                BizAction.flagReceiveIssue = true;

                BizAction.PatientID = this.PatientID;
                BizAction.PatientunitID = this.PatientunitID;
                BizAction.IsAgainstPatient = this.IsAgainstPatient;


                if (cmbIssueFromStoreSrch.SelectedItem != null)
                    BizAction.IssueFromStoreId = ((clsStoreVO)cmbIssueFromStoreSrch.SelectedItem).StoreId;

                if (cmbIssueToStoreSrch.SelectedItem != null)
                    BizAction.IssueToStoreId = ((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId;

                BizAction.IssueToDate = null;
                BizAction.IssueFromDate = null;
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
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {

                        BizAction = (clsGetIssueListBizActionVO)e.Result;

                        BizAction.IssueList = ((clsGetIssueListBizActionVO)e.Result).IssueList;
                        BizAction.IssueToStoreId = ((clsStoreVO)cmbIssueToStoreSrch.SelectedItem).StoreId;

                        IssueDataList.TotalItemCount = BizAction.TotalRows;

                        IssueDataList.Clear();
                        //foreach (var item in BizAction.IssueList)
                        //{

                        var result = from item1 in BizAction.IssueList
                                     where item1.IssueToStoreId == BizAction.IssueToStoreId
                                     select item1;

                        List<clsIssueListVO> obj = new List<clsIssueListVO>();
                        obj = result.ToList();
                        foreach (var item in obj)
                        {
                            IssueDataList.Add(item);
                        }
                        // IssueDataList.da = obj;
                        // IssueDataList.Add((clsIssueListVO)result.);
                        //}

                        dgSearchIssueList.ItemsSource = null;
                        dgSearchIssueList.ItemsSource = IssueDataList;

                        dgdpIssueList.Source = null;
                        dgdpIssueList.PageSize = IssueDataList.PageSize;
                        dgdpIssueList.Source = IssueDataList;
                        //dgSearchIssueList.Source = null;
                        //dgSearchIssueList.PageSize = BizAction.MaximumRows;
                        //dgSearchIssueList.Source = IssueDataList;

                        //Indicatior.Close();
                    }


                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                //Indicatior.Close();
                throw;
            }


        }

        private void dgSearchIssueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillItemListByIssueId();
        }
        private void FillItemListByIssueId()
        {
            if (dgSearchIssueList.SelectedItem != null)
            {
                clsGetItemListByIssueIdBizActionVO BizAction = new clsGetItemListByIssueIdBizActionVO();

                BizAction.IssueId = ((clsIssueListVO)dgSearchIssueList.SelectedItem).IssueId;
                BizAction.ReturnItemList = new List<clsReturnItemDetailsVO>();
                BizAction.ToStoreID = (long)((clsIssueListVO)dgSearchIssueList.SelectedItem).IssueToStoreId;
                BizAction.UnitID = ((clsIssueListVO)dgSearchIssueList.SelectedItem).IssueUnitID;
                //BizAction.flagReceivedIssue = true;
                BizAction.TransactionType = ValueObjects.InventoryTransactionType.ReceiveItemReturn;

                if (this.IsAgainstPatient)
                {
                    BizAction.PatientID = this.PatientID;
                    BizAction.PatientunitID = this.PatientunitID;
                    BizAction.IsAgainstPatient = this.IsAgainstPatient;
                }
                else
                {
                    BizAction.PatientID = 0;
                    BizAction.PatientunitID = 0;
                    BizAction.IsAgainstPatient = false;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ReturnItemList = ((clsGetItemListByIssueIdBizActionVO)e.Result).ReturnItemList;
                        // this.ocReturnItemDetailsList = BizAction.ReturnItemList;
                        //  dgIssueItemList.ItemsSource = null;
                        this.ocSelectedItemDetailsList.Clear();
                        foreach (clsReturnItemDetailsVO item in BizAction.ReturnItemList)
                        {
                            item.SelectedUOM = new ValueObjects.MasterListItem { ID = item.UOMID, Description = item.UOM };  // UOMID = TransactionUOMID and UOM = TransactionUOM of Received Qty 

                            //var result = from item1 in BizAction.ReturnItemList
                            //             where item1.ToStoreID == BizAction.ToStoreID
                            //             select item1;

                            //List<clsReturnItemDetailsVO> obj = new List<clsReturnItemDetailsVO>();
                            //obj = result.ToList();
                            //foreach (var item in obj)
                            //{
                            // if (item.ToStoreID == BizAction.ToStoreID)
                            if (item.BalanceQty > 0)
                                this.ocSelectedItemDetailsList.Add(item);
                            //}
                            //this.ocSelectedItemDetailsList.Add((clsReturnItemDetailsVO)result);
                        }

                        //dgIssueItemList.ItemsSource = this.ocReturnItemDetailsList;
                        //SumOfTotals();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }


            else
            {
                //dgIssueItemList.ItemsSource = null;
                this.ocSelectedItemDetailsList.Clear();
            }
        }
    }
}

