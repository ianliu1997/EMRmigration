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
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;
using System.Text;

namespace PalashDynamics.Pharmacy
{
    public partial class frmPhysicalItemStock : UserControl
    {
        private SwivelAnimation objAnimation;
        public frmPhysicalItemStock()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            DataList = new PagedSortableCollectionView<clsPhysicalItemsMainVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgdpIssueList.PageSize = DataListPageSize;
            dgdpIssueList.Source = DataList;
            FillStoreCombobox(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
        }
        List<clsStoreVO> objlistStore;
        public void FillStoreCombobox(long ClinicID)
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.IsActive = true;
            //if (ClinicID > 0)
            //{
            //    BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ClinicID.ToString() };
            //}

            //// BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString() };
            //BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //client.ProcessCompleted += (s, args) =>
            //{

            //    if (args.Error == null && args.Result != null)
            //    {

            //        List<MasterListItem> objList = new List<MasterListItem>();


            //        objList.Add(new MasterListItem(0, "-- Select --", true));
            //        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

            //        cmbBackStore.ItemsSource = null;
            //        cmbStoreSrch.ItemsSource = null;
            //        cmbStoreSrch.ItemsSource = objList;
            //        cmbBackStore.ItemsSource = objList;
            //        if (objList.Count > 1)
            //        {
            //            cmbBackStore.SelectedItem = objList[0];

            //            cmbStoreSrch.SelectedItem = objList[0];
            //            //dgfrontStockAdj.ItemsSource = null;


            //        }
            //        else
            //        {
            //            cmbBackStore.SelectedItem = objList[0];
            //            cmbStoreSrch.SelectedItem = objList[0];

            //        }


            //    }

            //    if (this.DataContext != null)
            //    {
            //       // cmbBackStore.SelectedValue = ((clsAdjustmentStockVO)this.DataContext).StoreID;
            //    }


            //};

            //client.ProcessAsync(BizAction, new clsUserVO());
            //client.CloseAsync();




            //clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //BizActionObj.IsUserwiseStores = true;
            //BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            //BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            ////False when we want to fetch all items
            //clsItemMasterVO obj = new clsItemMasterVO();
            //obj.RetrieveDataFlag = false;

            //BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            //client.ProcessCompleted += (s, args) =>
            //{
            //    if (args.Error == null && args.Result != null)
            //    {
            //        objlistStore = new List<clsStoreVO>();
            //        clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "-Select--", ClinicId = ClinicID, Status = true, IsQuarantineStore = false, Parent = 0 };
            //        //BizActionObj.ItemMatserDetails.Insert(0, Default);
            //        objlistStore.Add(Default);
            //        objlistStore.AddRange(((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList);

            //        // cmbFromStore.ItemsSource = BizActionObj.ItemMatserDetails;


            //        var result = from item in objlistStore
            //                     where item.ClinicId == ClinicID && item.IsQuarantineStore == false //&& item.Status == true
            //                     select item;


            //        cmbBackStore.ItemsSource = null;
            //        cmbStoreSrch.ItemsSource = null;
            //        cmbStoreSrch.ItemsSource = result;
            //        cmbBackStore.ItemsSource = result;
            //        //if (result.ToList().Count > 1)
            //        //{
            //        //    cmbBackStore.SelectedItem = result.ToList()[0];

            //        //    cmbStoreSrch.SelectedItem = result.ToList()[0];



            //        //}
            //        //else
            //        //{
            //        //    cmbBackStore.SelectedItem = result.ToList()[0];
            //        //    cmbStoreSrch.SelectedItem = result.ToList()[0];

            //        //}

            //        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
            //        {
            //            if (result.ToList().Count > 0)
            //            {
            //                cmbStoreSrch.ItemsSource = result.ToList();
            //                cmbStoreSrch.SelectedItem = result.ToList()[0];
            //                cmbBackStore.ItemsSource = result.ToList();
            //                cmbBackStore.SelectedItem = result.ToList()[0];
            //            }
            //        }
            //        else
            //        {
            //            if (NonQSAndUserDefinedStores != null)
            //            {
            //                cmbStoreSrch.ItemsSource = NonQSAndUserDefinedStores.ToList();
            //                cmbStoreSrch.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
            //                cmbBackStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
            //                cmbBackStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
            //            }
            //        }

            //        //if (cmbBackStore.SelectedItem == null)
            //        //{
            //        //    cmbBackStore.TextBox.SetValidation("Please Select Store");
            //        //    cmbBackStore.TextBox.RaiseValidationError();
            //        //    cmbBackStore.Focus();

            //        //}
            //        //else if (((clsStoreVO)cmbBackStore.SelectedItem).StoreId == 0)
            //        //{
            //        //    cmbBackStore.TextBox.SetValidation("Please Select Store");
            //        //    cmbBackStore.TextBox.RaiseValidationError();
            //        //    cmbBackStore.Focus();

            //        //}

            //    }

            //};
            //client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();


            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;

                    var NonQSAndUserDefinedStores = from item in BizActionObj.ToStoreList.ToList()
                                                    where item.IsQuarantineStore == false
                                                    select item;

                    NonQSAndUserDefinedStores.ToList().Insert(0, select);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbStoreSrch.ItemsSource = result.ToList();
                            cmbStoreSrch.SelectedItem = result.ToList()[0];
                            cmbBackStore.ItemsSource = result.ToList();
                            cmbBackStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores != null)
                        {
                            cmbStoreSrch.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbStoreSrch.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                            cmbBackStore.ItemsSource = NonQSAndUserDefinedStores.ToList();
                            cmbBackStore.SelectedItem = NonQSAndUserDefinedStores.ToList()[0];
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("ClickNew");
            cmbBackStore.SelectedValue = (long)0;
            dgstockadjUpdate.ItemsSource = null;
            AddedItems = new ObservableCollection<clsPhysicalItemsVO>();
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdCloseBack_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            objAnimation.Invoke(RotationType.Backward);
        }
        int ClickedFlag1 = 0;
        MessageBoxControl.MessageBoxChildWindow mgbx = null;
        private bool Validate()
        {

            bool result = true;

            if (cmbBackStore.SelectedItem == null || ((clsStoreVO)cmbBackStore.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Store can not be Empty. Please Select a Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                result = false;
            }
            else if (AddedItems.Count == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Items List can not be Empty. Please Select Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                result = false;
            }
            else if (AddedItems != null && AddedItems.Count > 0)
            {
                foreach (var item in AddedItems)
                {
                    if (item.SelectedUOM == null)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select UOM", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        result = false;
                        break;
                    }
                    else if (item.SelectedUOM != null && item.SelectedUOM.ID == 0)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select UOM", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        result = false;
                        break;
                    }
                }

            }
            return result;

        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                ClickedFlag1 += 1;
                if (ClickedFlag1 == 1)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are You Sure\n You Want To Save Details", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD_1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_1_OnMessageBoxClosed);
                    msgWD_1.Show();
                }
            }
        }
        private void msgWD_1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveDetails();
            }
            else
            {

                ClickedFlag1 = 0;
            }
        }

        private void SaveDetails()
        {
            WaitIndicator wt = new WaitIndicator();

            clsAddUpdatePhysicalItemStockBizActionVO BizAction = new clsAddUpdatePhysicalItemStockBizActionVO();
            BizAction.ItemDetails = new clsPhysicalItemsMainVO();
            BizAction.ItemDetailsList = new List<clsPhysicalItemsVO>();

            if (cmbBackStore.SelectedItem != null)
                BizAction.ItemDetails.StoreID = ((clsStoreVO)cmbBackStore.SelectedItem).StoreId;
            BizAction.ItemDetails.RequestedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

            BizAction.ItemDetailsList = AddedItems.ToList();



            wt.Show();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                ClickedFlag1 = 0;
                if (args.Error == null && args.Result != null)
                {


                    objAnimation.Invoke(RotationType.Backward);
                    cmdNew.IsEnabled = true;
                    FillMainDetails();
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash.", "Details Saved Successfully ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash.", "Some Error Occured while saving information.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                }
                wt.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        WaitIndicator indicator = new WaitIndicator();
        public PagedSortableCollectionView<clsPhysicalItemsMainVO> DataList { get; private set; }

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
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillMainDetails();

        }

        private void FillMainDetails()
        {
            indicator.Show();


            clsGetPhysicalItemStockBizActionVO BizAction = new clsGetPhysicalItemStockBizActionVO();
            //BizAction.IsActive = true;
            if (dpFromDate.SelectedDate != null)
                BizAction.FromDate = dpFromDate.SelectedDate.Value.Date.Date;
            if (dpToDate.SelectedDate != null)
                BizAction.ToDate = dpToDate.SelectedDate.Value.Date.Date;

            if (cmbStoreSrch.SelectedItem != null)
                BizAction.StoreID = ((clsStoreVO)cmbStoreSrch.SelectedItem).StoreId;

            BizAction.PagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetPhysicalItemStockBizActionVO result = e.Result as clsGetPhysicalItemStockBizActionVO;
                    DataList.TotalItemCount = result.TotalRowCount;

                    if (result.ItemDetailsList != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.ItemDetailsList)
                        {
                            DataList.Add(item);
                        }

                        dgPhysicalItem.ItemsSource = null;
                        dgPhysicalItem.ItemsSource = DataList;
                        dgdpIssueList.Source = null;
                        dgdpIssueList.PageSize = BizAction.MaximumRows;
                        dgdpIssueList.Source = DataList;
                        dgPhysicalItem.SelectedIndex = 0;
                        txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                    }

                }
                indicator.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void cmdGetItems_Click(object sender, RoutedEventArgs e)
        {

            bool Result = true;
            if (cmbBackStore.SelectedItem == null)
            {
                cmbBackStore.TextBox.SetValidation("Please Select Store");
                cmbBackStore.TextBox.RaiseValidationError();
                cmbBackStore.Focus();
                Result = false;
            }
            else if (((clsStoreVO)cmbBackStore.SelectedItem).StoreId == 0)
            {
                cmbBackStore.TextBox.SetValidation("Please Select Store");
                cmbBackStore.TextBox.RaiseValidationError();
                cmbBackStore.Focus();
                Result = false;
            }
            else
            {
                cmbBackStore.TextBox.ClearValidationError();
                Result = true;
            }




            if (Result == true)
            {
                frmBlockItemSearch win = new frmBlockItemSearch();
                win.StoreID = ((clsStoreVO)cmbBackStore.SelectedItem).StoreId;
                win.cmbStore.IsEnabled = false;
                win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                win.Show();
            }
        }
        public ObservableCollection<clsPhysicalItemsVO> AddedItems { get; set; }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            frmBlockItemSearch Itemswin = (frmBlockItemSearch)sender;

            if (Itemswin.SelectedBatches != null && Itemswin.SelectedBatches != null)
            {

                StringBuilder strError = new StringBuilder();

                foreach (var item in Itemswin.SelectedBatches)
                {

                    bool Additem = true;
                    if (AddedItems != null && AddedItems.Count > 0)
                    {
                        var item1 = from r in AddedItems
                                    where (r.BatchID == item.BatchID)
                                    select new clsPhysicalItemsVO
                                    {
                                        ID = r.ID,
                                        ItemName = r.ItemName
                                    };

                        if (item1.ToList().Count > 0)
                        {
                            if (strError.ToString().Length > 0)
                                strError.Append(",");
                            strError.Append(item1.ToList()[0].ItemName);
                            Additem = false;
                        }
                    }

                    if (Additem)
                    {
                        clsPhysicalItemsVO Item = new clsPhysicalItemsVO();
                        Item.ItemID = item.ItemID;
                        Item.ItemName = item.ItemName;
                        Item.ItemCode = item.ItemCode;
                        Item.BatchID = item.BatchID;
                        Item.BatchCode = item.BatchCode;
                        Item.ExpiryDate = item.ExpiryDate;
                        Item.AvailableStock = item.AvailableStock;
                        Item.AvailableStockInBase = item.AvailableStockInBase;
                        Item.Container = item.Container;
                        Item.ContainerID = item.ContainerID;
                        Item.Rack = item.Rack;
                        Item.RackID = item.RackID;
                        Item.Shelf = item.Shelf;
                        Item.ShelfID = item.ShelfID;
                        Item.Store = item.Store;
                        Item.StoreID = item.StoreID;
                        Item.AdjustmentQunatity = 1;
                        Item.RadioStatusYes = true;
                        Item.intOperationType = (int)InventoryStockOperationType.Addition;

                        Item.StockingUM = item.StockingUM;
                        Item.StockingUMID = item.StockingUMID;
                        Item.PurchaseUM = item.PurchaseUM;
                        Item.PurchaseUMID = item.PurchaseUMID;
                        Item.SellingUM = item.SellingUM;
                        Item.SellingUMID = item.SellingUMID;
                        Item.BaseUM = item.BaseUM;
                        Item.BaseUMID = item.BaseUMID;


                        //For Conversion factor.........................
                        Item.SelectedUOM = new MasterListItem(item.StockingUMID, item.StockingUM);
                        Item.ConversionFactor = item.StockingCF / item.StockingCF;
                        Item.BaseConversionFactor = item.StockingCF;
                        // Item.BaseQuantity = 1 * item.StockingCF;
                        //..............................................



                        AddedItems.Add(Item);
                    }
                }
                dgstockadjUpdate.ItemsSource = AddedItems;
                dgstockadjUpdate.Focus();
                dgstockadjUpdate.UpdateLayout();
                dgstockadjUpdate.SelectedIndex = AddedItems.Count - 1;

                if (!string.IsNullOrEmpty(strError.ToString()))
                {
                    string strMsg = "Items Already Added";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdClose.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    break;
                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        private void cmdSearchItem_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;

            if (dpFromDate.SelectedDate != null && dpToDate.SelectedDate != null)
            {
                if (dpFromDate.SelectedDate.Value.Date > dpToDate.SelectedDate.Value.Date)
                {
                    dpFromDate.SetValidation("From Date should be less than To Date");
                    dpFromDate.RaiseValidationError();
                    dpFromDate.Focus();
                    res = false;
                }
                else
                {
                    dpFromDate.ClearValidationError();
                }
            }

            if (dpFromDate.SelectedDate != null && dpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                dpFromDate.SetValidation("From Date should not be greater than Today's Date");
                dpFromDate.RaiseValidationError();
                dpFromDate.Focus();
                res = false;
            }
            else
            {
                dpFromDate.ClearValidationError();
            }

            if (dpFromDate.SelectedDate != null && dpToDate.SelectedDate == null)
            {
                dpToDate.SetValidation("Plase Select To Date");
                dpToDate.RaiseValidationError();
                dpToDate.Focus();
                res = false;
            }
            else
            {
                dpToDate.ClearValidationError();
            }

            if (dpToDate.SelectedDate != null && dpFromDate.SelectedDate == null)
            {
                dpFromDate.SetValidation("Plase Select From Date");
                dpFromDate.RaiseValidationError();
                dpFromDate.Focus();
                res = false;
            }
            else
            {
                dpFromDate.ClearValidationError();
            }

            if (res)
            {

                dgdpIssueList.PageIndex = 0;
                FillMainDetails();

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddedItems = new ObservableCollection<clsPhysicalItemsVO>();
            dpFromDate.SelectedDate = DateTime.Now.Date;
            dpToDate.SelectedDate = DateTime.Now.Date;
            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
            TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleHeader");
            mElement1.Text = "Physical Item Stock";

            FillMainDetails();
        }

        private void cmbBackStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddedItems.Clear();
        }

        private void cmbStoreSrch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgPhysicalItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPhysicalItem.SelectedItem != null)
            {
                FillItemDetails();

            }
        }

        List<clsPhysicalItemsVO> objList = new List<clsPhysicalItemsVO>();
        private void FillItemDetails()
        {
            WaitIndicator wait = new WaitIndicator();
            try
            {
                wait.Show();
                clsGetPhysicalItemStockDetailsBizActionVO BizAction = new clsGetPhysicalItemStockDetailsBizActionVO();
                BizAction.PhysicalItemID = ((clsPhysicalItemsMainVO)dgPhysicalItem.SelectedItem).ID;
                BizAction.PhysicalItemUnitID = ((clsPhysicalItemsMainVO)dgPhysicalItem.SelectedItem).UnitID;
                objList = new List<clsPhysicalItemsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objList.Clear();
                        objList = ((clsGetPhysicalItemStockDetailsBizActionVO)args.Result).ItemDetailsList;
                        dgPhysicalItemList.ItemsSource = null;
                        dgPhysicalItemList.ItemsSource = objList;
                    }
                    wait.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        private void RdoAdd_Checked(object sender, RoutedEventArgs e)
        {

        }
        int ClickedFlagSA = 0;
        private void btnStockAdjustment_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlagSA += 1;
            if (ClickedFlagSA == 1)
            {
                if (dgPhysicalItem.SelectedItem != null)
                {

                    if (((clsPhysicalItemsMainVO)dgPhysicalItem.SelectedItem).IsConvertedToSA == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD_1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Transction Of Physical Item Stock To Stock Adjustment Already Done", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD_1.Show();
                        ClickedFlagSA = 0;
                    }
                    else
                    {
                        StockAdjustment _Stock = new StockAdjustment();
                        _Stock.Initiate("New");
                        _Stock.IsFromPhysicalStock = true;
                        _Stock.PhysicalStockMain = (clsPhysicalItemsMainVO)dgPhysicalItem.SelectedItem;
                        //_Stock.PhysicalStockList = (List<clsPhysicalItemsVO>)dgPhysicalItemList.ItemsSource;
                        _Stock.PhysicalStockList = this.objList.ToList();
                        // _Stock.IsFromRequestApproval = true;
                        //  _IPDBills.SelectedBill = (clsBillVO)dgBillList.SelectedItem;
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                        mElement.Text = "Stock Adjustment";
                        ((IApplicationConfiguration)App.Current).OpenMainContent(_Stock);
                        ClickedFlagSA = 0;
                    }
                }
            }
        }

        private void cmbSUM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList == null || ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        private void cmbSUM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgstockadjUpdate.SelectedItem != null)
            {


                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SingleQuantity = 0;
                ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).Quantity = 0;


                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).StockingUMID);
                }
                else
                {
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).ConversionFactor = 0;
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor = 0;
                }

            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgstockadjUpdate.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity);
                    long BaseUOMID = ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseUMID;
                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    if (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM != null && ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID == ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseUMID && (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).Quantity % 1) != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Physical Stock Can't Be Fraction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity = 0;
                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;
                        return;
                    }
                    //else if (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity == ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).AvailableStockInBase)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Physical Stock And System Stock Cannot Be Same", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    mgbx.Show();
                    //    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity = 0;
                    //    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;
                    //}
                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }
        WaitIndicator Indicatior;
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).ItemID;
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

                        UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgstockadjUpdate.SelectedItem != null)
                        {
                            ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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

        private void dgstockadjUpdate_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.DisplayIndex == 12)     //Quantity     if (e.Column.DisplayIndex == 7) 
            {
                clsPhysicalItemsVO obj = new clsPhysicalItemsVO();
                obj = (clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem;
                if (obj.PhysicalQuantity == 0)
                {
                    //MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    //mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Ph Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //mgbx.Show();
                    obj.PhysicalQuantity = 0;
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).Quantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;
                    return;


                }
                else
                    if (obj.PhysicalQuantity < 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = null;
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Physical Stock Can't Be Negative. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        mgbx.Show();
                        obj.PhysicalQuantity = 0;
                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;
                        return;
                    }
                    else
                        if (obj.SelectedUOM != null && obj.SelectedUOM.ID == obj.BaseUMID && (obj.Quantity % 1) != 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow mgbx = null;
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Physical Stock Can't Be Fraction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            obj.PhysicalQuantity = 0;
                            ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;
                            return;
                        }

                if (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList == null || ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList.Count == 0)
                {
                    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;

                    //if (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity == ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).AvailableStockInBase)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Physical Stock And System Stock Cannot Be Same", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    mgbx.Show();
                    //    obj.PhysicalQuantity = 0;
                    //    ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseQuantity = Convert.ToSingle(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity) * ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor;

                    //}
                }
                else
                    if (((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM != null && ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID > 0)
                    {
                        CalculateConversionFactorCentral(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID, ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).StockingUMID);
                    }
                    else
                    {
                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).PhysicalQuantity = 0;
                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SingleQuantity = 0;

                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).ConversionFactor = 0;
                        ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).BaseConversionFactor = 0;


                    }

            }

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgstockadjUpdate.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).ItemID, ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }
        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).StockingUMID);



        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {

            if (dgstockadjUpdate.SelectedItem != null)
            {

                AddedItems.RemoveAt(dgstockadjUpdate.SelectedIndex);

            }


            //if (this.dgstockadjUpdate.SelectedItem is clsPhysicalItemsVO)
            //{
            //    clsPhysicalItemsVO person = ((clsPhysicalItemsVO)dgstockadjUpdate.SelectedItem).DeepCopy<clsPhysicalItemsVO>();
            //    ((List<clsPhysicalItemsVO>)dgPhysicalItemList.ItemsSource).ToList().Remove(((clsPhysicalItemsVO)this.dgstockadjUpdate.SelectedItem));
            //    int i = 0;
            //    foreach (clsPhysicalItemsVO item in ((List<clsPhysicalItemsVO>)dgPhysicalItemList.ItemsSource).ToList())
            //    {
            //        if (item.ItemID == person.ItemID && item.BatchID == person.BatchID)
            //        {

            //            item.Status = false;
            //            ((List<clsPhysicalItemsVO>)dgPhysicalItemList.ItemsSource).ToList().Remove(item);
            //            ((List<clsPhysicalItemsVO>)dgPhysicalItemList.ItemsSource).ToList().Insert(i, item);


            //            break;
            //        }
            //        i++;
            //    }


            //}
        }

    }
}
