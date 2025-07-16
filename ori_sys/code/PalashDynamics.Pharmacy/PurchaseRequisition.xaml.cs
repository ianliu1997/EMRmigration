using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using CIMS;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Inventory.Indent;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Pharmacy.Inventory;
using System.Windows.Input;


namespace PalashDynamics.Pharmacy
{
    public partial class PurchaseRequisition : UserControl, IInitiateCIMS
    {
        public PurchaseRequisition()
        {
            InitializeComponent();

            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            grdStoreIndent.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(grdStoreIndent_CellEditEnded);
            dtpAddIndentDate.SelectedDate = DateTime.Now.Date;
            dtpAddIndentDate.IsEnabled = false;
            //======================================================
            DeleteIndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
        }


        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "New":
                    FlagApprove = false;
                    CmdForward.Visibility = Visibility.Collapsed;
                    CmdApprove.Visibility = Visibility.Collapsed;


                    break;
                case "Approve":
                    FlagApprove = true;
                    CmdForward.Visibility = Visibility.Visible;
                    CmdApprove.Visibility = Visibility.Visible;
                    CmdNew.Visibility = Visibility.Collapsed;
                    CmdSave.Visibility = Visibility.Collapsed;
                    dgIndentList.Columns[0].Visibility = Visibility.Visible; //make visible for modify
                    //By Umesh For Modify Quantity
                    //CmdModify.IsEnabled = true;
                    BdrItemSearch.Visibility = Visibility.Collapsed;
                    cmbFromStoreName.IsEnabled = false;
                    break;
            }
        }
        clsIndentMasterVO SelectedIndent { get; set; }
        public bool FlagApprove { get; set; }
        string SelectedCommand = "Cancel";
        private SwivelAnimation objAnimation;

        List<MasterListItem> UOMLIstNew = new List<MasterListItem>();

        private long _ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                _ClinicID = value;
            }
        }

        public ObservableCollection<clsIndentDetailVO> IndentAddedItems { get; set; }
        public ObservableCollection<clsIndentDetailVO> DeleteIndentAddedItems { get; set; }
        bool IsPageLoaded = false;
        WaitIndicator Indicatior = null;

        bool UserStoreAssigned = false;

        //Added By Pallavi
        MessageBoxControl.MessageBoxChildWindow mgbx = null;
        void grdStoreIndent_CellEditEnded(object Sender, DataGridCellEditEndedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {

                clsIndentDetailVO obj = new clsIndentDetailVO();
                obj = (clsIndentDetailVO)grdStoreIndent.SelectedItem;

                //if (Convert.ToDouble(grdStoreIndent.Columns[1].GetCellContent(obj.RequiredQuantity)) == 0)
                //{
                //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    mgbx.Show();
                //    obj.RequiredQuantity = 1;
                //    return;
                //}

                if (e.Column.DisplayIndex == 1)
                {
                    if (obj.SingleQuantity == 0)   //if (obj.RequiredQuantity == 0)
                    {
                        //MessageBoxControl.MessageBoxChildWindow mgbx = null;

                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.SingleQuantity = 1;   //obj.RequiredQuantity = 1;

                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity;

                        return;
                    }

                    if (obj.SingleQuantity < 0)  //if (obj.RequiredQuantity < 0)
                    {
                        //MessageBoxControl.MessageBoxChildWindow mgbx = null;

                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.SingleQuantity = 1;   //obj.RequiredQuantity = 1;

                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity;

                        return;
                    }

                    if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.Description != ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOM)
                    {
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor); //ConversionFactor
                    }
                    else
                    {
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity);
                    }

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity;
                    if (SelectedCommand == "View")
                    {
                        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
                        {
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockingQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockCF;
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity * ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor;
                        }
                        else
                        {
                            if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
                            {
                                CalculateConversionFactorCentral(((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                            }
                        }
                    }
                    else
                    {
                        if (((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM != null && ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID > 0)
                        {
                            CalculateConversionFactorCentral(((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM.ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                        }
                        else
                        {
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;
                            //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = 0;
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseConversionFactor = 0;
                        }
                    }
                }

            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoaded)
            {



                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.dtpFromDate.SelectedDate = DateTime.Now;
                this.dtpToDate.SelectedDate = DateTime.Now;
                this.rdbAll.IsChecked = true;
                FillStore();
                //FillStores(ClinicID);
                FillIndentList();
                FillUOM();
                Indicatior.Close();
                IsPageLoaded = true;
                IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                grdStoreIndent.ItemsSource = IndentAddedItems;
            }

            //IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
            //grdStoreIndent.ItemsSource = IndentAddedItems;


            //FillOrderBookingList();
        }

        List<clsStoreVO> UserStores = new List<clsStoreVO>();
        private void FillStore()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
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

                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                    BizActionObj.ItemMatserDetails.Insert(0, Default);

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    //List<clsStoreVO> UserStores = new List<clsStoreVO>();
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    UserStores.Clear();

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {    //item.StoreDetails       
                            UserStores = item.UserUnitStore;

                            //if (item.IsDefault == true)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                            //else if (item.IsDefault == false)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                        }
                    }
                    //UserStores.Insert(0, Default);
                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }

                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);

                    //List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 
                    // StoreListForClinicUser.Insert(0, Default);
                    // cmbToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                    cmbAddToStoreName.ItemsSource = result1.ToList();

                    cmbToStoreName.ItemsSource = result1.ToList();
                    if (result1.ToList().Count > 0)
                    {
                        cmbToStoreName.SelectedItem = result1.ToList()[0];
                        cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    }
                    //cmbAddToStoreName.ItemsSource = BizActionObj.ItemMatserDetails;

                    //if (StoreListForClinicUser.Count > 0)
                    //    cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = BizActionObj.ItemMatserDetails;
                        cmbAddFromStoreName.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }

                    else
                    {
                        //Commented By Harish on 17 Apr
                        //cmbFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();
                        //cmbAddFromStoreName.ItemsSource = (List<clsStoreVO>)result.ToList();

                        //cmbAddFromStoreName.ItemsSource = StoreListForClinicUser; //StoreListForClinic;
                        // Rohit
                        //cmbAddFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = StoreListForClinic;
                        cmbFromStoreName.ItemsSource = StoreListForClinic;
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbAddFromStoreName.SelectedItem = StoreListForClinic[0];
                            cmbFromStoreName.SelectedItem = StoreListForClinic[0];
                        }
                        // End
                    }
                }
            };

            client.CloseAsync();


            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.Parent = new KeyValue();
            //BizAction.Parent.Key = "1";
            //BizAction.Parent.Value = "Status";

            //BizAction.MasterList = new List<MasterListItem>();
            //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            //client1.ProcessCompleted += (s, args) =>
            //{
            //    if (args.Error == null && args.Result != null)
            //    {
            //        //cmbFromStoreName.ItemsSource = null;
            //        BizAction.MasterList = ((clsGetMasterListBizActionVO)args.Result).MasterList;
            //        MasterListItem Default = new MasterListItem { ID = 0, Description = "--Select--" };
            //        BizAction.MasterList.Insert(0, Default);
            //        //cmbFromStoreName.ItemsSource = BizAction.MasterList;
            //        //cmbFromStoreName.SelectedItem = Default;

            //        cmbToStoreName.ItemsSource = BizAction.MasterList;
            //        cmbToStoreName.SelectedItem = Default;

            //        //cmbAddFromStoreName.ItemsSource = BizAction.MasterList;
            //        //cmbAddFromStoreName.SelectedItem = Default;

            //        cmbAddToStoreName.ItemsSource = BizAction.MasterList;
            //        cmbAddToStoreName.SelectedItem = Default;
            //    }
            //};
            //client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client1.CloseAsync();
        }

        private void NewStoreFill()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
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

                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                    BizActionObj.ItemMatserDetails.Insert(0, Default);

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    List<clsStoreVO> UserStores = new List<clsStoreVO>();
                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {
                            UserStores = item.UserUnitStore;
                            //if (item.IsDefault == true)
                            //{
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                            //else if (item.IsDefault == false)
                            //{
                            //    //item.UserUnitStore.Insert(0, Default);
                            //    UserStores = item.UserUnitStore;
                            //    break;
                            //}
                        }
                    }

                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }
                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);
                    List<clsStoreVO> StoreListForClinicUser = (List<clsStoreVO>)resultnew.ToList(); //(List<clsStoreVO>)resultnew.ToList(); 

                    cmbAddToStoreName.ItemsSource = result1.ToList();
                    //cmbToStoreName.ItemsSource = result1.ToList();
                    if (result1.ToList().Count > 0)
                    {
                        //cmbToStoreName.SelectedItem = result1.ToList()[0];
                        cmbAddToStoreName.SelectedItem = result1.ToList()[0];
                    }

                    if (StoreListForClinicUser.Count > 0)
                        cmbAddFromStoreName.SelectedItem = StoreListForClinicUser[0]; //StoreListForClinic[0];
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        //cmbFromStoreName.ItemsSource = result1.ToList();
                        cmbAddFromStoreName.ItemsSource = result1.ToList();
                        if (result1.ToList().Count > 0)
                        {
                            //cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                            cmbAddFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }

                    else
                    {
                        cmbAddFromStoreName.ItemsSource = StoreListForClinic;
                        //cmbFromStoreName.ItemsSource = StoreListForClinic;
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbAddFromStoreName.SelectedItem = StoreListForClinic[0];
                            //cmbFromStoreName.SelectedItem = StoreListForClinic[0];
                        }
                    }
                }
            };

            client.CloseAsync();
        }

        private void FillUOM()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_UnitOfMeasure;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        UOMLIstNew = new List<MasterListItem>();
                        UOMLIstNew.Add(new MasterListItem(0, "- Select -"));
                        UOMLIstNew.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        #region 'Paging'

        public PagedSortableCollectionView<clsIndentMasterVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillIndentList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillIndentList()
        {
            try
            {
                indicator.Show();
                dgIndentList.ItemsSource = null;
                dgIndentDetailList.ItemsSource = null;

                clsGetIndenListBizActionVO BizAction = new clsGetIndenListBizActionVO();
                if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
                {
                    if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                    {
                        dtpFromDate.SetValidation("From Date should be less than To Date");
                        dtpFromDate.RaiseValidationError();
                        dtpFromDate.Focus();
                        indicator.Close();
                        return;
                    }
                    else
                    {
                        dtpFromDate.ClearValidationError();
                    }

                }
                if (this.dtpFromDate.SelectedDate != null && this.dtpToDate.SelectedDate != null)
                {
                    BizAction.FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                    BizAction.ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
                }
                if (this.cmbFromStoreName.SelectedItem != null)
                    BizAction.FromStoreID = ((clsStoreVO)this.cmbFromStoreName.SelectedItem).StoreId;
                if (this.cmbToStoreName.SelectedItem != null)
                    BizAction.ToStoreID = ((clsStoreVO)this.cmbToStoreName.SelectedItem).StoreId;

                BizAction.IndentNO = txtPRNumber.Text;

                //Set Paging Variables
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                if ((bool)rdbAll.IsChecked)
                {
                    BizAction.CheckStatusType = false;
                }
                else if ((bool)rdbPending.IsChecked)
                {
                    BizAction.CheckStatusType = true;
                    //BizAction.IndentStatus = 3;
                    BizAction.IndentStatus = 1;
                }
                else if ((bool)rdbCompleted.IsChecked)
                {
                    BizAction.CheckStatusType = true;
                    BizAction.IndentStatus = 4;
                }
                else if ((bool)rdbApproved.IsChecked)
                {
                    BizAction.isApproved = true;
                }
                else if ((bool)rdbForwarded.IsChecked)
                {
                    BizAction.isFrowrded = true;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    BizAction.UnitID = 0;
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }
                //By Anjali.....................
                // BizAction.isIndent = false;
                BizAction.isIndent = 0;
                //.............................
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            //dgIndentList.ItemsSource = ((clsGetIndenListBizActionVO)arg.Result).IndentList;

                            clsGetIndenListBizActionVO result = arg.Result as clsGetIndenListBizActionVO;
                            DataList.TotalItemCount = result.TotalRows;

                            if (result.IndentList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.IndentList)
                                {
                                    DataList.Add(item);
                                }

                                dgIndentList.ItemsSource = null;
                                dgIndentList.ItemsSource = DataList;

                                DataPager.Source = null;
                                DataPager.PageSize = BizAction.MaximumRows;
                                DataPager.Source = DataList;
                            }
                        }
                    }
                    indicator.Close();
                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception ex)
            {
                indicator.Close();
            }
        }
        #endregion

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            FillIndentList();
        }

        private void CmdNew_Click(object sender, RoutedEventArgs e)
        {

            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = true;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdModify.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            SelectedCommand = "New";
            //cmbAddFromStoreName.SelectedValue = 0;
            //cmbAddToStoreName.SelectedValue = 0;

            //SelectedIndent = new clsIndentMasterVO();
            //this.cmbAddFromStoreName.SelectedItem = new MasterListItem { ID = 0, Description = "--Select--" };
            //this.cmbAddToStoreName.SelectedItem = new MasterListItem { ID = 0, Description = "--Select--" };
            //cmbAddFromStoreName.SelectedItem = null;
            //cmbAddToStoreName.SelectedItem = null;
            this.dtpAddIndentDate.SelectedDate = DateTime.Now;
            this.dtpAddDueDate.SelectedDate = DateTime.Now;
            //this.chkIsAuthorized.IsChecked = false;
            //this.dtpAuthorizationDate.SelectedDate = DateTime.Now;


            NewStoreFill();

            this.IndentNumber.Text = "";
            this.txtRemark.Text = "";
            grdStoreIndent.IsEnabled = true;
            chkFreezIndent.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            chkFreezIndent.IsChecked = false;
            this.DataContext = new clsIndentDetailVO();
            //if (GRNAddedItems != null)
            IndentAddedItems.Clear();
            objAnimation.Invoke(RotationType.Forward);

        }

        int ClickedFlag1 = 0;
        clsUserVO userVO = new clsUserVO();
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag = Validate();
                int Count = 0;
                //ClickedFlag1 += 1;
                //if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId > 0 && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId > 0 && IndentAddedItems.Count != 0) //grdStoreIndent.ItemsSource != 0)
                //{
                //    foreach (clsStoreVO item in UserStores)
                //    {
                //        if (item.StoreId == ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId)
                //        {
                Count = 0;
                ClickedFlag1 += 1;
                if (ClickedFlag1 == 1)
                {

                    if (flag)
                    {

                        string msgTitle = string.Empty;
                        string msgText = string.Empty;

                        if ((bool)chkFreezIndent.IsChecked)
                        {
                            msgTitle = "Freeze and Save Purchase Requisition";
                            msgText = "Do You want to Freeze and Save the Purchase Requisition";
                        }
                        else
                        {

                            msgTitle = "Save Purchase Requisition";
                            msgText = "Do You want to Save the Purchase Requisition";

                        }

                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);

                        msgWin.Show();


                    }
                    else
                        ClickedFlag1 = 0;
                }


            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveIndent();
            }
            else if (result == MessageBoxResult.No)
                ClickedFlag1 = 0;
        }

        private void SaveIndent()
        {
            clsAddIndentBizActionVO BizAction = new clsAddIndentBizActionVO();

            BizAction.objIndent = new clsIndentMasterVO();
            BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
            BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
            BizAction.objIndent.IndentNumber = null;
            BizAction.objIndent.TransactionMovementID = 0;
            BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
            BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
            BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
            BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            BizAction.objIndent.IsAuthorized = true;// chkIsAuthorized.IsChecked;
            BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
            BizAction.objIndent.AuthorizationDate = DateTime.Now; // dtpAuthorizationDate.SelectedDate;
            BizAction.objIndent.Remark = txtRemark.Text;
            BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
            BizAction.objIndent.Status = true;
            //By Anjali....................................
            BizAction.objIndent.IsIndent = 0;
            //BizAction.objIndent.IsIndent = false;
            //...................................................
            BizAction.objIndent.InventoryIndentType = InventoryIndentType.PurchaseRequisition;

            BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
            BizAction.objIndent.IsForwarded = false;
            BizAction.objIndent.IsApproved = false;


            BizAction.objIndentDetailList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
            BizAction.objIndentDetailList.Select(indentItems => { indentItems.IndentUnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId; return indentItems; }).ToList();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag1 = 0;
                if (arg.Error == null && ((clsAddIndentBizActionVO)arg.Result).objIndent != null)
                {
                    if (arg.Result != null)
                    {
                        CmdPrint.IsEnabled = true;
                        CmdNew.IsEnabled = true;
                        CmdSave.IsEnabled = false;
                        CmdCancel.IsEnabled = false;
                        CmdForward.IsEnabled = true;
                        CmdModify.IsEnabled = false;
                        CmdApprove.IsEnabled = true;
                        SelectedCommand = "Save";
                        cmbAddFromStoreName.SelectedValue = (long)0;
                        cmbAddToStoreName.SelectedValue = (long)0;


                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Saved Successfully With Purchase Requisition Number " + ((clsAddIndentBizActionVO)arg.Result).objIndent.IndentNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            objAnimation.Invoke(RotationType.Backward);
                        };
                        mgbox.Show();

                        FillIndentList();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SelectedCommand = "Cancel";
            CmdPrint.IsEnabled = true;
            CmdNew.IsEnabled = true;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = false;
            CmdModify.IsEnabled = false;
            CmdForward.IsEnabled = true;
            CmdApprove.IsEnabled = true;

            IndentAddedItems.Clear();
            //cmbAddFromStoreName.SelectedValue = 0;
            //cmbAddToStoreName.SelectedValue = 0;
            cmbAddFromStoreName.SelectedItem = null;
            cmbAddToStoreName.SelectedItem = null;
            dtpAddDueDate.SelectedDate = DateTime.Now;
            dtpAddIndentDate.SelectedDate = DateTime.Now;
            //chkIsAuthorized.IsChecked = false;
            //dtpAuthorizationDate.SelectedDate = DateTime.Now;

            //Enable after Forward
            grdStoreIndent.IsEnabled = true;
            chkFreezIndent.IsEnabled = true;
            lnkAddItems.IsEnabled = true;
            this.cmbAddFromStoreName.IsEnabled = true;
            this.cmbAddToStoreName.IsEnabled = true;
            this.dtpAddIndentDate.IsEnabled = true;
            this.dtpAddDueDate.IsEnabled = true;
            //this.chkIsAuthorized.IsEnabled = true;
            //this.dtpAuthorizationDate.IsEnabled = true;
            this.txtRemark.IsEnabled = true;
            FillIndentList();

            objAnimation.Invoke(RotationType.Backward);

        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAddToStoreName.SelectedItem != null && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId != 0)
            {
                ItemList Itemswin = new ItemList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                //Itemswin.ShowZeroStockBatches = true;
                Itemswin.ShowZeroStockBatches = false;

                Itemswin.ShowBatches = false;
                //Itemswin.cmbStore.SelectedItem = cmbAddToStoreName.SelectedItem;
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.ShowQuantity = true;
                Itemswin.StoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                Itemswin.ClinicID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).ClinicId;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select To Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
            }

        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (Itemswin.SelectedItems != null)
            {
                //if (GRNAddedItems == null)
                //    GRNAddedItems = new ObservableCollection<clsGRNDetailsVO>();

                foreach (var item in Itemswin.SelectedItems)
                {
                    IEnumerator<clsIndentDetailVO> list = (IEnumerator<clsIndentDetailVO>)IndentAddedItems.GetEnumerator();
                    bool IsExist = false;

                    while (list.MoveNext())
                    {
                        if (item.ID == ((clsIndentDetailVO)list.Current).ItemID)
                        {
                            IsExist = true;
                            break;
                        }
                    }
                    if (IsExist == false)
                        IndentAddedItems.Add(new clsIndentDetailVO()
                        {
                            ItemID = item.ID,
                            ItemName = item.ItemName,
                            RequiredQuantity = item.RequiredQuantity,
                            ConversionFactor = Convert.ToSingle(item.ConversionFactor),
                            //UOMList = UOMLIstNew, 
                            //SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.PUM)), 
                            PUOMID = item.PUM,
                            BaseUOMID = item.BaseUM,
                            BaseUOM = item.BaseUMString,
                            SellingUOMID = item.SellingUM,
                            SellingUOM = item.SellingUMString,
                            UOM = item.PUOM,
                            SUOM = item.SUOM,
                            SUOMID = item.SUM
                        });
                }

                grdStoreIndent.Focus();
                grdStoreIndent.UpdateLayout();

                grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;
            }
        }

        private void dgIndentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                if (((clsIndentMasterVO)dgIndentList.SelectedItem).IndentStatus == InventoryIndentStatus.Completed)
                {
                    CmdForward.IsEnabled = false;
                }
                else
                {
                    CmdForward.IsEnabled = true;
                }

                SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
                clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
                BizAction.IndentDetailList = new List<clsIndentDetailVO>();
                BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)


                //    BizAction.UnitID = 0;
                //else
                //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                            SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                            cmbAddFromStoreName.SelectedValue = ((clsIndentMasterVO)dgIndentList.SelectedItem).FromStoreName;
                            cmbAddToStoreName.SelectedValue = ((clsIndentMasterVO)dgIndentList.SelectedItem).ToStoreName;
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
        }

        private bool Validate()
        {
            //if (!FlagApprove)
            //{
            // Changes done by Harish 17 Apr

            if ((cmbAddFromStoreName.SelectedItem == null || ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId == 0))
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            //else if ((long)cmbAddFromStoreName.SelectedValue == (long)0)
            //{ 
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    return false;
            //}
            // Changes done by Harish 17 Apr
            if ((cmbAddToStoreName.SelectedItem == null || ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId == 0))
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "To Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            if (((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId == ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From and To store should not be same! Please select different from store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            //else if ((long)cmbAddToStoreName.SelectedValue == (long)0)
            //{
            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    return false;
            //} 
            if ((DateTime?)dtpAddDueDate.SelectedDate == null)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Due Date can not be Empty. Please Select a Due Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            if ((DateTime?)dtpAddIndentDate.SelectedDate == null)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Date can not be Empty. Please Select a Purchase Requisition Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }
            if (IndentAddedItems.Count == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Items List can not be Empty. Please Select Purchase Requisition Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            //Added By Pallavi
            List<clsIndentDetailVO> objList = IndentAddedItems.ToList<clsIndentDetailVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    if (item.SingleQuantity == 0)   //if (item.RequiredQuantity == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        item.SingleQuantity = 1;   //item.RequiredQuantity = 1;
                        item.RequiredQuantity = 1;
                        return false;
                    }
                    if (item.SingleQuantity < 0)  //if (item.RequiredQuantity <= 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        item.SingleQuantity = 1;   //item.RequiredQuantity = 0;
                        item.RequiredQuantity = 1;
                        return false;
                    }
                    if (item.SelectedUOM.ID == 0 && SelectedCommand != "Forward")   //if (item.RequiredQuantity == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Transaction UOM", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        item.SelectedUOM.ID = (long)0;   //item.RequiredQuantity = 1;

                        return false;
                    }
                }

            }
            // }
            return true;

        }

        private void CmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Inventory_New/StoreIndentPrint.aspx?IndentId=" + ((clsIndentMasterVO)dgIndentList.SelectedItem).ID + "&UnitID=" + (((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID) + "&IsIndent=" + false), "_blank");
            }
        }


        private void ViewIndent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
                    clsGetIndentDetailListBizActionVO BizAction = new clsGetIndentDetailListBizActionVO();
                    BizAction.IndentID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).ID);
                    BizAction.UnitID = Convert.ToInt64(((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID);
                    BizAction.IndentDetailList = new List<clsIndentDetailVO>();

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //acTextBox = new TextBox();
                                dgIndentDetailList.ItemsSource = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;
                                SelectedIndent.IndentDetailsList = ((clsGetIndentDetailListBizActionVO)arg.Result).IndentDetailList;

                                this.DataContext = new clsIndentDetailVO();
                                IndentAddedItems.Clear();

                                foreach (var item in SelectedIndent.IndentDetailsList)
                                {
                                    item.UOMList = UOMLIstNew;


                                    item.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID));
                                    item.UOM = UOMLIstNew.Single(u => u.ID.Equals(item.UOMID)).Description;

                                    item.SUOM = UOMLIstNew.Single(u => u.ID.Equals(item.SUOMID)).Description;

                                    IndentAddedItems.Add(item);
                                    //cmbAddFromStoreName.SelectedItem = item.fr;
                                }

                                var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                              where r.ClinicId == this.SelectedIndent.IndentUnitID && r.StoreId == this.SelectedIndent.FromStoreID
                                              //&& r.ClinicId == this.SelectedIndent.UnitID
                                              select r;

                                //cmbAddFromStoreName.SelectedItem  = SelectedIndent.IndentDetailsList.
                                foreach (clsStoreVO item in results)
                                {
                                    cmbAddFromStoreName.SelectedItem = item;
                                }

                                var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                               where r.StoreId == this.SelectedIndent.ToStoreID
                                               select r;

                                foreach (clsStoreVO item in results1)
                                {
                                    cmbAddToStoreName.SelectedItem = item;
                                }
                                //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                                //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                                this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                                this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                                //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                                //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                                this.IndentNumber.Text = SelectedIndent.IndentNumber;
                                this.txtRemark.Text = SelectedIndent.Remark;
                                this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;
                                ViewIndent();
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ViewIndent()
        {
            CmdPrint.IsEnabled = false;
            CmdNew.IsEnabled = false;
            CmdSave.IsEnabled = false;
            CmdCancel.IsEnabled = true;
            CmdForward.IsEnabled = false;
            CmdApprove.IsEnabled = false;
            SelectedCommand = "View";
            objAnimation.Invoke(RotationType.Forward);

            if (SelectedIndent.IsFreezed)
            {
                if (FlagApprove == true && !SelectedIndent.IsApproved)
                {
                    CmdModify.IsEnabled = true;
                }
                else
                {
                    CmdModify.IsEnabled = false;
                }
                chkFreezIndent.IsEnabled = false;
                lnkAddItems.IsEnabled = false;
            }
            else
            {
                chkFreezIndent.IsEnabled = true;
                grdStoreIndent.IsEnabled = true;
                CmdModify.IsEnabled = true;
                lnkAddItems.IsEnabled = true;
            }
        }


        private void chkFreeze_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null && ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed == true)
                {
                    MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Freeze Purchase Requisition Number " + SelectedIndent.IndentNumber + "", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsUpdateIndentOnlyForFreezeBizActionVO BizAction = new clsUpdateIndentOnlyForFreezeBizActionVO();
                            BizAction.IndentID = Convert.ToInt64(SelectedIndent.ID);

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null)
                                {
                                    if (arg.Result != null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Freezed & Updated Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        mgbox.Show();
                                    }
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                        else
                        {
                            ((clsIndentMasterVO)dgIndentList.SelectedItem).IsFreezed = false;
                        }
                    };

                    mgBox.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CmdModify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag1 = ClickedFlag1 + 1;
                if (ClickedFlag1 == 1)
                {
                    bool flag = Validate();



                    if (flag)
                    {
                        if (FlagApprove && SelectedCommand != "Forward")
                        {
                            UpdateForChangeAndApprove();
                        }
                        else if (SelectedCommand == "Forward")
                        {
                            if (SelectedIndent.ToStoreID == ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "ToStore is not changed.\n Change ToStore then Modify it.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgBox.Show();
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to forward Purchase Requisition ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                        BizAction.objIndent = new clsIndentMasterVO();
                                        BizAction.objIndent.ID = SelectedIndent.ID;
                                        BizAction.objIndent.Date = SelectedIndent.Date;
                                        BizAction.objIndent.Time = SelectedIndent.Time;
                                        BizAction.objIndent.TransactionMovementID = SelectedIndent.TransactionMovementID;
                                        BizAction.objIndent.FromStoreID = SelectedIndent.FromStoreID;
                                        BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                                        BizAction.objIndent.DueDate = SelectedIndent.DueDate;
                                        BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                        BizAction.objIndent.IsAuthorized = SelectedIndent.IsAuthorized;
                                        BizAction.objIndent.AuthorizedByID = SelectedIndent.AuthorizedByID;
                                        BizAction.objIndent.AuthorizationDate = SelectedIndent.AuthorizationDate;
                                        BizAction.objIndent.Remark = SelectedIndent.Remark;
                                        BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                                        BizAction.objIndent.Status = true;
                                        BizAction.objIndent.IsFreezed = SelectedIndent.IsFreezed;
                                        BizAction.objIndent.IsForwarded = true;
                                        BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
                                        BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                                        BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                        BizAction.objIndent.InventoryIndentType = InventoryIndentType.PurchaseRequisition;
                                        //By Anjali................................
                                        // BizAction.objIndent.IsIndent = false;
                                        BizAction.objIndent.IsIndent = 0;
                                        //..................................................
                                        BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();

                                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                        client.ProcessCompleted += (s, arg) =>
                                        {
                                            ClickedFlag1 = 0;
                                            if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                                            {
                                                if (arg.Result != null)
                                                {
                                                    CmdPrint.IsEnabled = true;
                                                    CmdNew.IsEnabled = true;
                                                    CmdSave.IsEnabled = false;
                                                    CmdCancel.IsEnabled = false;
                                                    CmdModify.IsEnabled = false;
                                                    CmdForward.IsEnabled = true;
                                                    CmdApprove.IsEnabled = true;
                                                    //Enable after Forward
                                                    chkFreezIndent.IsEnabled = true;
                                                    lnkAddItems.IsEnabled = true;
                                                    this.cmbAddFromStoreName.IsEnabled = true;
                                                    this.cmbAddToStoreName.IsEnabled = true;
                                                    this.dtpAddIndentDate.IsEnabled = true;
                                                    this.dtpAddDueDate.IsEnabled = true;
                                                    //this.chkIsAuthorized.IsEnabled = true;
                                                    //this.dtpAuthorizationDate.IsEnabled = true;
                                                    this.txtRemark.IsEnabled = true;

                                                    SelectedCommand = "Modify";

                                                    FillIndentList();
                                                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Forwarded Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                    mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                                    {
                                                        objAnimation.Invoke(RotationType.Backward);
                                                    };
                                                    mgbox.Show();
                                                }
                                            }
                                            else
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while forwarding Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                                mgbox.Show();
                                            }
                                        };
                                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    }
                                    else
                                        ClickedFlag1 = 0;
                                };
                                mgBox.Show();
                            }
                        }
                        else
                        {
                            string msgTitle = "";
                            string msgDesc = "";

                            if ((bool)chkFreezIndent.IsChecked)
                            {
                                msgTitle = "Freeze and Update Purchase Requisition";
                                msgDesc = "Do You want to Freeze and Update the Purchase Requisition";
                            }
                            else
                            {
                                msgTitle = "Update Purchase Requisition";
                                msgDesc = "Do You want to Update the Purchase Requisition";
                            }

                            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", msgDesc, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();

                                    BizAction.objIndent = new clsIndentMasterVO();
                                    BizAction.objIndent.ID = SelectedIndent.ID;
                                    BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
                                    BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
                                    BizAction.objIndent.TransactionMovementID = 0;
                                    BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                                    BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                                    BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
                                    BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                    BizAction.objIndent.IsAuthorized = true;// chkIsAuthorized.IsChecked;
                                    BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                                    BizAction.objIndent.AuthorizationDate = DateTime.Now;// dtpAuthorizationDate.SelectedDate;
                                    BizAction.objIndent.Remark = txtRemark.Text;
                                    BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                                    BizAction.objIndent.Status = true;
                                    BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
                                    BizAction.objIndent.IsForwarded = SelectedIndent.IsForwarded;
                                    BizAction.objIndent.IsChangeAndApprove = true;
                                    BizAction.objIndent.IsApproved = SelectedIndent.IsApproved;
                                    BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                                    BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                    //By Anjali...............................................
                                    //BizAction.objIndent.IsIndent = false;
                                    BizAction.objIndent.IsIndent = 0;
                                    //............................................................
                                    BizAction.objIndent.InventoryIndentType = InventoryIndentType.PurchaseRequisition;
                                    BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();

                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client.ProcessCompleted += (s, arg) =>
                                    {
                                        ClickedFlag1 = 0;
                                        if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                CmdPrint.IsEnabled = true;
                                                CmdNew.IsEnabled = true;
                                                CmdSave.IsEnabled = false;
                                                CmdCancel.IsEnabled = false;
                                                CmdModify.IsEnabled = false;
                                                CmdForward.IsEnabled = true;
                                                CmdApprove.IsEnabled = true;
                                                SelectedCommand = "Modify";
                                                FillIndentList();
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                                {
                                                    objAnimation.Invoke(RotationType.Backward);
                                                };
                                                mgbox.Show();
                                            }
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while updating Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                            mgbox.Show();
                                        }

                                    };
                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                }
                                else
                                    ClickedFlag1 = 0;
                            };
                            mgBox.Show();
                        }
                    }
                    else
                        ClickedFlag1 = 0;
                }
            }
            catch (Exception ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void UpdateForChangeAndApprove()
        {
            string msgTitle = "";
            string msgDesc = "";

            if ((bool)chkFreezIndent.IsChecked)
            {
                if (FlagApprove == true)
                {
                    msgTitle = "Update And Approve Purchase Requisition";
                    msgDesc = "Do You want to Update And Approve the Purchase Requisition?";
                }
                else
                {
                    msgTitle = "Freeze and Update Purchase Requisition";
                    msgDesc = "Do You want to Freeze and Update the Purchase Requisition?";
                }
            }
            else
            {
                msgTitle = "Update Purchase Requisition";
                msgDesc = "Do You want to Update the Purchase Requisition?";
            }

            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", msgDesc, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                    BizAction.IsForChangeAndApproveIndent = true;
                    BizAction.objIndent = new clsIndentMasterVO();
                    BizAction.objIndent.ID = SelectedIndent.ID;
                    BizAction.objIndent.Date = Convert.ToDateTime(dtpAddIndentDate.SelectedDate.Value.Date);
                    BizAction.objIndent.Time = dtpAddIndentDate.SelectedDate;
                    BizAction.objIndent.TransactionMovementID = 0;
                    BizAction.objIndent.FromStoreID = ((clsStoreVO)cmbAddFromStoreName.SelectedItem).StoreId;
                    BizAction.objIndent.ToStoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;
                    BizAction.objIndent.DueDate = Convert.ToDateTime(dtpAddDueDate.SelectedDate);
                    BizAction.objIndent.IndentCreatedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                    BizAction.objIndent.IsAuthorized = true;// chkIsAuthorized.IsChecked;
                    BizAction.objIndent.AuthorizedByID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).ID;
                    BizAction.objIndent.AuthorizationDate = DateTime.Now;// dtpAuthorizationDate.SelectedDate;
                    BizAction.objIndent.Remark = txtRemark.Text;
                    BizAction.objIndent.IndentStatus = InventoryIndentStatus.New;
                    BizAction.objIndent.Status = true;
                    BizAction.objIndent.IsFreezed = (bool)chkFreezIndent.IsChecked;
                    BizAction.objIndent.IsForwarded = SelectedIndent.IsForwarded;
                    //if (IsFromChangAndApprove == true)
                    //{ 
                    BizAction.objIndent.IsChangeAndApprove = true; //}
                    //else
                    //{ BizAction.objIndent.IsApproved = SelectedIndent.IsApproved; }
                    BizAction.objIndent.IndentUnitID = SelectedIndent.IndentUnitID;
                    BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                    //   BizAction.objIndent.IsModify = true;
                    BizAction.objIndent.IndentDetailsList = (List<clsIndentDetailVO>)IndentAddedItems.ToList<clsIndentDetailVO>();
                    BizAction.objIndent.DeletedIndentDetailsList = (List<clsIndentDetailVO>)DeleteIndentAddedItems.ToList<clsIndentDetailVO>();
                    //By Anjali......................................................
                    BizAction.objIndent.IsIndent = 0;
                    // BizAction.objIndent.IsIndent = true;
                    //................................................................
                    BizAction.objIndent.InventoryIndentType = InventoryIndentType.Indent;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag1 = 0;
                        if (arg.Error == null && ((clsUpdateIndentBizActionVO)arg.Result).objIndent != null)
                        {
                            if (arg.Result != null)
                            {
                                CmdPrint.IsEnabled = true;
                                CmdNew.IsEnabled = true;
                                CmdSave.IsEnabled = false;
                                CmdCancel.IsEnabled = false;
                                CmdModify.IsEnabled = false;
                                CmdForward.IsEnabled = true;
                                CmdApprove.IsEnabled = true;
                                SelectedCommand = "Modify";
                                FillIndentList();
                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgbox.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                {
                                    objAnimation.Invoke(RotationType.Backward);
                                };
                                mgbox.Show();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while updating Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            mgbox.Show();
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                }
                else
                    ClickedFlag1 = 0;
            };
            mgBox.Show();
        }

        private void CmdForward_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    //if (SelectedIndent.IsApproved)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition is already approved. \n This Purchase Requisition can not be forward.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    mgbox.Show();
                    //}
                    //else
                    //{
                    if (!SelectedIndent.IsForwarded)
                    {
                        if (SelectedIndent.IsFreezed)
                        {
                            if (SelectedIndent.IsApproved)
                            {
                                this.DataContext = new clsIndentDetailVO();
                                IndentAddedItems.Clear();
                                foreach (var item in SelectedIndent.IndentDetailsList)
                                {
                                    IndentAddedItems.Add(item);
                                }

                                //var results = from r in ((List<clsStoreVO>)cmbAddFromStoreName.ItemsSource)
                                //              where r.StoreId == this.SelectedIndent.FromStoreID
                                //              select r;

                                //foreach (clsStoreVO item in results)
                                //{
                                //    cmbAddFromStoreName.SelectedItem = item;
                                //}

                                var results1 = from r in ((List<clsStoreVO>)cmbAddToStoreName.ItemsSource)
                                               where r.StoreId == this.SelectedIndent.ToStoreID
                                               select r;

                                foreach (clsStoreVO item in results1)
                                {
                                    cmbAddFromStoreName.SelectedItem = item;
                                    //cmbAddToStoreName.SelectedItem = item;
                                }
                                //this.cmbAddFromStoreName.SelectedValue = SelectedIndent.FromStoreID;
                                //this.cmbAddToStoreName.SelectedValue = SelectedIndent.ToStoreID;
                                this.dtpAddIndentDate.SelectedDate = SelectedIndent.Date;
                                this.dtpAddDueDate.SelectedDate = SelectedIndent.DueDate;
                                //this.chkIsAuthorized.IsChecked = SelectedIndent.IsAuthorized;
                                //this.dtpAuthorizationDate.SelectedDate = SelectedIndent.AuthorizationDate;
                                this.IndentNumber.Text = SelectedIndent.IndentNumber;
                                this.txtRemark.Text = SelectedIndent.Remark;
                                this.chkFreezIndent.IsChecked = SelectedIndent.IsFreezed;

                                CmdPrint.IsEnabled = false;
                                CmdNew.IsEnabled = false;
                                CmdSave.IsEnabled = false;
                                CmdCancel.IsEnabled = true;
                                CmdModify.IsEnabled = true;
                                CmdForward.IsEnabled = false;
                                CmdApprove.IsEnabled = false;
                                //Disable for Forward
                                grdStoreIndent.IsEnabled = false;
                                chkFreezIndent.IsEnabled = false;
                                lnkAddItems.IsEnabled = false;
                                this.cmbAddFromStoreName.IsEnabled = false;
                                this.cmbAddToStoreName.IsEnabled = true;
                                this.dtpAddIndentDate.IsEnabled = false;
                                this.dtpAddDueDate.IsEnabled = false;
                                //this.chkIsAuthorized.IsEnabled = false;
                                //this.dtpAuthorizationDate.IsEnabled = false;
                                this.txtRemark.IsEnabled = false;
                                this.chkFreezIndent.IsEnabled = false;

                                objAnimation.Invoke(RotationType.Forward);
                                SelectedCommand = "Forward";
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition is not Approved \n Please Approve Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                mgbox.Show();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition is not Freezed \n Please Freeze Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition is already Forwarded", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    // }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Purchase Requisition is Selected \n Please select a Indent", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgIndentList.SelectedItem != null)
                {
                    if (SelectedIndent.IsApproved)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        if (SelectedIndent.IsFreezed)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Approve Purchase Requisition ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    clsUpdateIndentBizActionVO BizAction = new clsUpdateIndentBizActionVO();
                                    BizAction.objIndent = SelectedIndent;
                                    if (BizAction.objIndent.IndentStatus == InventoryIndentStatus.ForceFullyCompleted)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Can't approve Purchase Requisition it is forcefully closed.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        mgbox.Show();
                                        return;
                                    }
                                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)


                                        BizAction.objIndent.UnitID = SelectedIndent.UnitID;
                                    else
                                        BizAction.objIndent.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                                    BizAction.objIndent.IsApproved = true;
                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    client.ProcessCompleted += (s, arg) =>
                                    {
                                        if (arg.Error == null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition Approved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                mgbox.Show();
                                                FillIndentList();
                                            }
                                        }
                                    };
                                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                }
                            };
                            mgBox.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition is not Freezed \n Please Freeze Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbox.Show();
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No Purchase Requisition is Selected \n Please select a Purchase Requisition", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (chkFreezIndent.IsChecked == true && !FlagApprove)
            {
                MessageBoxControl.MessageBoxChildWindow msgWD =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Purchase Requisition is freezed you can't delete the item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                return;
            }

            if (grdStoreIndent.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        clsIndentDetailVO objVO = (clsIndentDetailVO)grdStoreIndent.SelectedItem;
                        clsIndentDetailVO obj;
                        if (objVO != null)
                        {
                            obj = IndentAddedItems.Where(z => z.ItemID == objVO.ItemID).FirstOrDefault();
                            IndentAddedItems.Remove(obj);
                            DeleteIndentAddedItems.Add(obj);
                        }
                        //IndentAddedItems.RemoveAt(grdStoreIndent.SelectedIndex);
                        grdStoreIndent.Focus();
                        grdStoreIndent.UpdateLayout();
                        grdStoreIndent.SelectedIndex = IndentAddedItems.Count - 1;

                    }
                };
                msgWD.Show();
            }
        }

        private void cmbAddToStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (IndentAddedItems != null)
                {
                    if (IndentAddedItems.Count > 0)
                    {
                        if (SelectedIndent != null)
                        {
                            if (!SelectedIndent.IsFreezed)
                                if (cmbAddToStoreName.SelectedItem != null && ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId != 0)
                                {

                                    if (SelectedIndent.ToStoreID != ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId)
                                        IndentAddedItems.Clear();
                                }
                        }
                    }

                    long StoreID = 0;

                    if (cmbAddToStoreName.SelectedItem != null)
                        StoreID = ((clsStoreVO)cmbAddToStoreName.SelectedItem).StoreId;

                    //if (StoreID > 0)
                    //{
                    //    BdrItemSearch.Visibility = System.Windows.Visibility.Visible;
                    //    AttachItemSearchControl(StoreID, 0);
                    //}
                    //else
                    //{
                    //    BdrItemSearch.Visibility = System.Windows.Visibility.Collapsed;
                    //}

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void cmdCancelIndent_Click(object sender, RoutedEventArgs e)
        {

            if (((clsIndentMasterVO)dgIndentList.SelectedItem) != null)
            {
                if (((clsIndentMasterVO)dgIndentList.SelectedItem).IsApproved == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Cannot cancel the Purchase Requisition.The Purchase Requisition is Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD1.Show();
                }
                else
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to cancel the Purchase Requisition?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            POCancellationWindow Win = new POCancellationWindow();
                            Win.Title = "Purchase Requisition Cancellation";
                            Win.tblkCAncellationReason.Text = "Purchase Requisition Cancellation";
                            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                            Win.Show();
                        }
                    };
                    msgWD.Show();
                }
            }

        }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedIndent = (clsIndentMasterVO)dgIndentList.SelectedItem;
            POCancellationWindow Win = (POCancellationWindow)sender;

            clsUpdateRemarkForIndentCancellationBizActionVO objBizActionVO = new clsUpdateRemarkForIndentCancellationBizActionVO();
            objBizActionVO.IndentMaster = new clsIndentMasterVO();
            objBizActionVO.IndentMaster.ID = SelectedIndent.ID;
            objBizActionVO.IndentMaster.UnitID = SelectedIndent.UnitID;
            objBizActionVO.CancellationRemark = Win.txtAppReason.Text;
            try
            {

                objBizActionVO.CancellationRemark = ((POCancellationWindow)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase Requisition cancelled successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
                        DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                        DataListPageSize = 5;
                        FillIndentList();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Item Search Control
        PalashDynamics.Pharmacy.Inventory.ItemSearch _ItemSearchRowControl = null;
        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {
            if (FlagApprove == false)
                BdrItemSearch.Visibility = Visibility.Visible;
            else
            {
                BdrItemSearch.Visibility = Visibility.Collapsed;
            }
            ItemSearchStackPanel.Children.Clear();
            _ItemSearchRowControl = new PalashDynamics.Pharmacy.Inventory.ItemSearch(StoreID, SupplierID);
            _ItemSearchRowControl.IsFromIndent = true;
            _ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);
        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {

            String strItemIDs = string.Empty;//Only For PO
            PalashDynamics.Pharmacy.Inventory.ItemSearch _ItemSearchRowControl = (PalashDynamics.Pharmacy.Inventory.ItemSearch)sender;
            if (IndentAddedItems != null)
            {
                if (IndentAddedItems.Count.Equals(0))
                {
                    IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
                }
            }
            else
            {
                IndentAddedItems = new ObservableCollection<clsIndentDetailVO>();
            }
            _ItemSearchRowControl.SelectedItems[0].RowID = IndentAddedItems.Count + 1;
            foreach (var item in _ItemSearchRowControl.SelectedItems)
            {
                if (IndentAddedItems.Where(indentItems => indentItems.ItemID == item.ItemID).Any() == false)
                {
                    clsIndentDetailVO itemObj = new clsIndentDetailVO();
                    itemObj.ItemID = item.ItemID;
                    itemObj.ItemName = item.ItemName;
                    itemObj.SingleQuantity = Convert.ToSingle(item.RequiredQuantity);
                    itemObj.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                    //itemObj.UOMList = UOMLIstNew;
                    //itemObj.SelectedUOM = UOMLIstNew.Single(u => u.ID.Equals(item.PUM));
                    itemObj.UOM = item.PUOM;
                    itemObj.SUOM = item.SUOM;
                    itemObj.SUOMID = item.SUM;

                    itemObj.PUOMID = item.PUM;
                    itemObj.SUOMID = item.SUM;
                    itemObj.BaseUOMID = item.BaseUM;
                    itemObj.BaseUOM = item.BaseUMString;
                    itemObj.SellingUOMID = item.SellingUM;
                    itemObj.SellingUOM = item.SellingUMString;

                    if (itemObj.SelectedUOM.Description != itemObj.SUOM)
                    {
                        itemObj.RequiredQuantity = itemObj.SingleQuantity * itemObj.ConversionFactor;
                    }
                    else
                    {
                        itemObj.RequiredQuantity = itemObj.SingleQuantity;
                    }

                    IndentAddedItems.Add(itemObj);

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Item is alredy added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

                strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");

            }

            grdStoreIndent.ItemsSource = null;
            grdStoreIndent.ItemsSource = IndentAddedItems;
            grdStoreIndent.SelectedItem = -1;
            grdStoreIndent.UpdateLayout();

            //_ItemSearchRowControl.SetFocus();
            //_ItemSearchRowControl = null;
            //_ItemSearchRowControl = new PalashDynamics.Pharmacy.Inventory.ItemSearch();
            //_ItemSearchRowControl.cmbItemName.Focus();

            _ItemSearchRowControl.SetFocus();
            _ItemSearchRowControl = null;
            _ItemSearchRowControl = new PalashDynamics.Pharmacy.Inventory.ItemSearch();

        }

        #endregion

        private void txtPRNumber_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillIndentList();
            }
            //else if (e.Key == Key.Tab)
            //{
            //    FillIndentList();
            //}
        }

        private void cmbFromStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbFromStoreName.SelectedItem != null)
            //{
            //    FillIndentList();
            //}
        }

        private void cmbToStoreName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbToStoreName.SelectedItem != null)
            //{
            //    FillIndentList();
            //}
        }

        private void cmbFromStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbFromStoreName.SelectedItem != null)
                {
                    FillIndentList();
                }
            }
        }

        private void cmbToStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbToStoreName.SelectedItem != null)
                {
                    FillIndentList();
                }
            }
        }

        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList == null || ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList.Count == 0))
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

                BizAction.ItemID = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ItemID;
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

                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (grdStoreIndent.SelectedItem != null)
                        {
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (grdStoreIndent.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;
                    objConversionVO.SingleQuantity = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseUOMID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    if (objConversionVO.BaseQuantity > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = objConversionVO.BaseQuantity;
                    if (objConversionVO.Quantity > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockingQuantity = objConversionVO.Quantity;

                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    if (objConversionVO.BaseRate > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    if (objConversionVO.BaseMRP > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    if (objConversionVO.ConversionFactor > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).StockCF = objConversionVO.ConversionFactor;
                    if (objConversionVO.BaseConversionFactor > 0)
                        ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = objConversionVO.BaseConversionFactor;
                    //((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;
                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdStoreIndent.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsIndentDetailVO)grdStoreIndent.SelectedItem).SingleQuantity = 0;
                ((clsIndentDetailVO)grdStoreIndent.SelectedItem).RequiredQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsIndentDetailVO)grdStoreIndent.SelectedItem).SUOMID);
                }
                else
                {
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).ConversionFactor = 0;
                    ((clsIndentDetailVO)grdStoreIndent.SelectedItem).BaseConversionFactor = 0;
                }
            }
        }

    }
}
