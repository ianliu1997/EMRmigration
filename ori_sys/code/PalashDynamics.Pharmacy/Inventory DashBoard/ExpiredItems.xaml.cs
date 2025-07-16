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
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pharmacy.Inventory_DashBoard
{
    public partial class ExpiredItems : UserControl
    {
        public ExpiredItems()
        {
            InitializeComponent();
            ExpiryItemList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            //Loaded += new RoutedEventHandler(ExpiredItems_Loaded);
            DataListPageSize = 24;
            ExpiryItemList.OnRefresh += new EventHandler<RefreshEventArgs>(ExpiryItemList_OnRefresh);
            FillRessonIssue();
        }
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
        # region // Variable Declarartion
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        MessageBoxControl.MessageBoxChildWindow mgbx = null;
        public PagedSortableCollectionView<clsExpiredItemReturnDetailVO> ExpiryItemList { get; private set; }
        public clsGetExpiryItemForDashBoardBizActionVO BizActionObjectForExpiryItemList { get; set; }
        //   ExpiryItemList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();

        # endregion
        #region Paging
        public int DataListPageSize
        {
            get
            {
                return ExpiryItemList.PageSize;
            }
            set
            {
                if (value == ExpiryItemList.PageSize) return;
                ExpiryItemList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        #endregion
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                long clinicId = 0;

                //Added by Sayali 21th Aug 2018
                DateTime now = DateTime.Now;
                dtpFromDate.SelectedDate = new DateTime(now.Year, now.Month, now.Day);
                dtpToDate.SelectedDate = DateTime.Now.Date;
                //

                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                //{
                //    clinicId = 0;
                //}
                //else
                //{
                clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //}
                FillStores(clinicId);
                Indicatior.Close();
            }
            IsPageLoded = true;
        }

        public void GetExpiryList()
        {
            //Commented by Sayali 

            //if (_ListSelected != null)
            //{
            //    _ListSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();
            //    _ListSelected.Clear();
            //}

            clsGetExpiryItemForDashBoardBizActionVO BizAction = new clsGetExpiryItemForDashBoardBizActionVO();
            BizAction.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();

            //if (BizActionObjectForExpiryItemList.IsOrderBy != null)
            //    BizAction.IsOrderBy = BizActionObjectForExpiryItemList.IsOrderBy;

            //     BizAction.Date = DateTime.Now;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitId = 0;
            }
            else
            {
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            //Added by Sayali 21th Aug 2018
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    return;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }

            }
            if (this.dtpFromDate.SelectedDate != null && this.dtpToDate.SelectedDate != null)
            {

                BizAction.ExpiryFromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                BizAction.ExpiryToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
            }
            //

            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizAction.IsPaging = true;
            BizAction.NoOfRecordShow = ExpiryItemList.PageSize;
            BizAction.StartIndex = ExpiryItemList.PageIndex * ExpiryItemList.PageSize;
            //      BizAction.Day =((IApplicationConfiguration)App.Current).ApplicationConfigurations.ItemExpiredIndays;
            //...................By Anjali.....................................................
            //BizAction.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            BizAction.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            //...................................................................................

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetExpiryItemForDashBoardBizActionVO)arg.Result != null)
                    {
                        clsGetExpiryItemForDashBoardBizActionVO result = arg.Result as clsGetExpiryItemForDashBoardBizActionVO;
                        ExpiryItemList.TotalItemCount = (int)result.TotalRow;
                        if (result.ExpiredItemList != null)
                        {
                            ExpiryItemList.Clear();
                            foreach (var item in result.ExpiredItemList)
                            {
                                ExpiryItemList.Add(item);
                            }
                        }
                    }
                    dgExpiredItemList.ItemsSource = ExpiryItemList;
                    dgDataPager.Source = ExpiryItemList;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        //private void FillStores(long pClinicID)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    if (pClinicID > 0)
        //    {
        //        BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
        //    }

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //            //cmbBloodGroup.ItemsSource = null;
        //            //cmbBloodGroup.ItemsSource = objList;
        //            cmbStore.ItemsSource = null;
        //            cmbStore.ItemsSource = objList;

        //            //if (objList.Count > 1)
        //            //    cmbStore.SelectedItem = objList[1];
        //            //else
        //            cmbStore.SelectedItem = objList[0];
        //            if (this.DataContext != null)
        //            {
        //                cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;
        //            }
        //            GetExpiryList();
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        List<clsStoreVO> objlistStore;
        private void FillStores(long pClinicID)
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.StoreType = 2;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    objlistStore = new List<clsStoreVO>();
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = pClinicID, Status = true, IsQuarantineStore = false, Parent = 0 };
                    //BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    objlistStore.Add(Default);
                    objlistStore.AddRange(((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails);

                    //// cmbFromStore.ItemsSource = BizActionObj.ItemMatserDetails;

                    //var result = from item in objlistStore
                    //             where item.ClinicId == pClinicID && item.Status == true
                    //             select item;

                    var result = from item in objlistStore
                                 where item.Status == true && item.IsQuarantineStore != true
                                 select item;

                    var result2 = from item in BizActionObj.ToStoreList
                                  where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.IsQuarantineStore != true  //&& item.Status == true
                                  select item;

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            cmbStore.ItemsSource = result.ToList();
                            cmbStore.SelectedItem = result.ToList()[0];
                        }
                    }
                    else
                    {
                        if (BizActionObj.ToStoreList.ToList().Count > 0)
                        {
                            //cmbStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                            //cmbStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];

                            if (BizActionObj.ToStoreList.ToList().Count == 1)
                            {
                                cmbStore.ItemsSource = result2.ToList();
                                cmbStore.SelectedItem = result2.ToList()[0];
                            }
                            else
                            {
                                cmbStore.ItemsSource = result2.ToList();
                                cmbStore.SelectedItem = result2.ToList()[1];
                            }
                        }
                    }


                    //cmbStore.ItemsSource = null;
                    //cmbStore.ItemsSource = (List<clsStoreVO>)result.ToList();
                    //cmbStore.SelectedItem = objlistStore[0];
                    //if (this.DataContext != null)
                    //{
                    //    cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;
                    //}
                    GetExpiryList();
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private bool Validation()
        {
            if (cmbStore.SelectedItem == null || ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)
            {
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select  Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
                return false;
            }

            return true;
        }

        void ExpiryItemList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetExpiryList();
        }

        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            //  if (Validation())
            GetExpiryList();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            long Days = 0;
            long StoreID = 0;
            Days = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ItemExpiredIndays;
            StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;

            //Added by Sayali 21th Aug 2018
            DateTime ExpiryFromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
            DateTime ExpiryToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;

            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

            string URL = "../Reports/InventoryPharmacy/ExpiredItems_DashB.aspx?Days=" + Days + "&UnitID=" + UnitID + "&StoreID=" + StoreID + "&ExpiryFromDate=" + ExpiryFromDate + "&ExpiryToDate=" + ExpiryToDate + "&Excel=" + chkExcel.IsChecked + "&UserID=" + UserID;// +"&GRNType=" + GRNType;  
            //

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


        }
        int ClickedFlagSA = 0;
        private void cmdSendToQuarntine_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlagSA += 1;
            if (ClickedFlagSA == 1)
            {
                if (_ListSelected != null && _ListSelected.Count > 0)
                {
                    if (cmbStore.SelectedItem == null)
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        ClickedFlagSA = 0;
                    }
                    else if (!(objlistStore.Where(s => s.Parent == ((clsStoreVO)cmbStore.SelectedItem).StoreId && s.IsQuarantineStore == true).Any()))
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Selected Store Does not have Quarantine Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        ClickedFlagSA = 0;
                    }
                    else if ((_ListSelected != null && _ListSelected.Where(s => s.StoreId != ((clsStoreVO)cmbStore.SelectedItem).StoreId).Any()))
                    {
                        mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Selected Store Does not match with selected item's Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        ClickedFlagSA = 0;
                    }
                    else
                    {
                        IssueToQSDOSE _Stock = new IssueToQSDOSE();  //IssueToClinic _Stock = new IssueToClinic();
                        _Stock.IsFromExpiredItem = true;
                        _Stock.FromStore = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                        // _Stock.ToStore = ((clsStoreVO)cmbStore.SelectedItem).Parent;
                        _Stock.ToStore = (objlistStore.Where(s => s.Parent == ((clsStoreVO)cmbStore.SelectedItem).StoreId && s.IsQuarantineStore == true).SingleOrDefault().StoreId);
                        _Stock.chkToQuarantine.IsChecked = true;
                        _Stock.chkFromQuarantine.IsEnabled = false;
                        _Stock.chkToQuarantine.IsEnabled = false;
                        _Stock.SelectedIndent = null;


                        foreach (var item in _ListSelected)
                        {
                            clsIssueItemDetailsVO obj = new clsIssueItemDetailsVO();

                            obj.ItemCode = item.ItemCode;
                            obj.ItemId = item.ItemID;
                            obj.ItemName = item.ItemName;
                            obj.BatchId = Convert.ToInt64(item.BatchID);
                            obj.BatchCode = item.BatchCode;
                            obj.AvailableStock = Convert.ToDecimal(item.AvailableStock); // Convert.ToDecimal(Math.Floor(Convert.ToDouble(item.AvailableStock)));
                            obj.ExpiryDate = item.BatchExpiryDate;
                            obj.IsIndent = 2;
                            obj.ReasonForIssue = 1;
                            obj.ReasonForIssueList = RessonIssue;
                            obj.SelectedReasonForIssue = RessonIssue.FirstOrDefault(p => p.ID == 1);
                            obj.Re_Order = item.Re_Order;
                            obj.SUOMID = item.StockUOMID;
                            obj.SUOM = item.StockUOM;
                            obj.AvailableStockUOM = item.StockUOM;
                            obj.SelectedUOM = new MasterListItem(item.TransactionUOMID, item.UOM); //obj.SelectedUOM = new MasterListItem(item.StockUOMID, item.StockUOM);
                            obj.ConversionFactor = item.StockCF;
                            obj.BaseUOM = item.BaseUOM;
                            obj.BaseUOMID = item.BaseUOMID;
                            obj.BaseQuantity = 0;
                            obj.BaseConversionFactor = item.BaseCF;

                            obj.MRP = Convert.ToDecimal(item.MRP * obj.BaseConversionFactor);   // Transaction MRP
                            obj.PurchaseRate = Convert.ToDecimal(item.PurchaseRate * obj.BaseConversionFactor); // Transaction CP
                            //.........................................

                            #region For Quarantine Items (Expired, DOS)
                            // Use For Vat/Tax Calculations
                            obj.OtherGRNItemTaxApplicationOn = item.OtherGRNItemTaxApplicationOn;
                            obj.OtherGRNItemTaxType = item.OtherGRNItemTaxType;
                            obj.ItemTaxPercentage = Convert.ToDecimal(item.OtherGRNItemTaxPer);
                            obj.GRNItemVatApplicationOn = item.GRNItemVatApplicationOn;
                            obj.GRNItemVatType = item.GRNItemVatType;
                            obj.ItemVATPercentage = Convert.ToDecimal(item.GRNItemVATPer);
                            #endregion

                            obj.MainMRP = Convert.ToSingle(item.MRP);   //Base MRP
                            obj.MainRate = Convert.ToSingle(item.PurchaseRate); //Base CP

                            _Stock.ocIssueItemDetailsList.Add(obj);

                        }
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                        mElement.Text = "Issue To QS";
                        ((IApplicationConfiguration)App.Current).OpenMainContent(_Stock);
                        ClickedFlagSA = 0;
                    }
                }
                else
                {
                    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    mgbx.Show();
                    ClickedFlagSA = 0;
                }
            }
        }

        //Added by Sayali 22 Aug 2018
        private ObservableCollection<clsExpiredItemReturnDetailVO> _ListSelected;
        public ObservableCollection<clsExpiredItemReturnDetailVO> SelectedList { get { return _ListSelected; } }
        //
        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dgExpiredItemList.SelectedItem != null)
            {
                if (_ListSelected == null)
                    _ListSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _ListSelected.Add((clsExpiredItemReturnDetailVO)dgExpiredItemList.SelectedItem);
                else
                    _ListSelected.Remove((clsExpiredItemReturnDetailVO)dgExpiredItemList.SelectedItem);

            }

        }

        private void dgExpiredItemList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dgExpiredItemList.ItemsSource != null)
            {
                if (((clsExpiredItemReturnDetailVO)e.Row.DataContext).IsItemBlock)
                    e.Row.IsEnabled = false;
                else
                    e.Row.IsEnabled = true;
            }
        }

        //Added by Sayali 22 Aug 2018
        private object GetChildControl(DependencyObject parent, string controlName)
        {

            Object tempObj = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int counter = 0; counter < count; counter++)
            {
                //Get The Child Control based on Index
                tempObj = VisualTreeHelper.GetChild(parent, counter);

                //If Control's name Property matches with the argument control
                //name supplied then Return Control
                if ((tempObj as DependencyObject).GetValue(NameProperty).ToString() == controlName)
                    return tempObj;
                else //Else Search Recursively
                {
                    tempObj = GetChildControl(tempObj as DependencyObject, controlName);
                    if (tempObj != null)
                        return tempObj;
                }
            }
            return null;
        }

        
        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsExpiredItemReturnDetailVO> ObjList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();
            if (dgExpiredItemList.ItemsSource != null)
            {
                ObjList = (PagedSortableCollectionView<clsExpiredItemReturnDetailVO>)dgExpiredItemList.ItemsSource;
                CheckBox chkSelectAll = GetChildControl(dgExpiredItemList, "chkSelectAll") as CheckBox;
                if (ObjList != null && ObjList.Count > 0)
                {
                    if (chkSelectAll.IsChecked == true)
                    {
                        foreach (var item in ObjList)
                        {

                            if (_ListSelected == null)
                                _ListSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();
                            if (item.IsItemBlock == false) //&& !IsItemSuspend
                            {
                                _ListSelected.Add(item);
                                item.IsSelect = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ObjList)
                        {
                            item.IsSelect = false;
                            if (_ListSelected != null)
                                _ListSelected.Remove(item);
                        }
                    }
                    dgExpiredItemList.ItemsSource = null;
                    dgExpiredItemList.ItemsSource = ObjList;
                }
                else
                    chkSelectAll.IsChecked = false;
            }
          }
        //

    }
}
