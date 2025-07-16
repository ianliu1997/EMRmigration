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
using PalashDynamics.ValueObjects;
using System.Windows.Browser;

namespace PalashDynamics.Pharmacy.Inventory_DashBoard
{
    public partial class PendingPR : UserControl
    {
        public PendingPR()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
            ItemPRList.OnRefresh += new EventHandler<RefreshEventArgs>(PRList_OnRefresh);

            DataListPageSize = 15;
            this.dgDataPager.PageSize = DataListPageSize;
            this.dgDataPager.Source = ItemPRList;

            this.dgDataPager.DataContext = ItemPRList;
            this.dgPendingPRList.DataContext = ItemPRList;
        }
        
        # region // Variable Declarartion
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        bool UserStoreAssigned = false;
        public PagedSortableCollectionView<clsIndentMasterVO> ItemPRList { get; private set; }
        //   ExpiryItemList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();

        # endregion

        #region Paging
        public int DataListPageSize
        {
            get
            {
                return ItemPRList.PageSize;
            }
            set
            {
                if (value == ItemPRList.PageSize) return;
                ItemPRList.PageSize = value;
            }
        }

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                FillStore();
            //    GetPRList();
                DateTime now = DateTime.Now;
                dtpFromDate.SelectedDate = new DateTime(now.Year, now.Month, 1);
                dtpToDate.SelectedDate = DateTime.Now.Date;
                cmbFromStoreName.Focus();
                Indicatior.Close();
            }
            IsPageLoded = true;
        }

        public void GetPRList()
        {
            clsGetIndentListForInventorDashBoardBizActionVO BizAction = new clsGetIndentListForInventorDashBoardBizActionVO();
            BizAction.IndentList = new List<clsIndentMasterVO>();
            Indicatior.Show();
            //if (rdbPRAsc.IsChecked == true)
            //    BizAction.IsOrderBy = false;
            //else if (rdbPRDesc.IsChecked == true)
            //    BizAction.IsOrderBy = true;

            BizAction.Date = null;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
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
                
                BizAction.FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
                BizAction.ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
            }
            if (this.cmbFromStoreName.SelectedItem != null)
                BizAction.FromStoreID = ((clsStoreVO)this.cmbFromStoreName.SelectedItem).StoreId;
            if (this.cmbToStoreName.SelectedItem != null)
                BizAction.ToStoreID = ((clsStoreVO)this.cmbToStoreName.SelectedItem).StoreId;

            BizAction.IndentNO = txtPRNumber.Text;

          //  ItemPRList.PageSize = 24;
            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecords = ItemPRList.PageSize;
            BizAction.StartRowIndex = ItemPRList.PageIndex * ItemPRList.PageSize;
            BizAction.IsIndent = false;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetIndentListForInventorDashBoardBizActionVO)arg.Result != null)
                    {
                        clsGetIndentListForInventorDashBoardBizActionVO result = arg.Result as clsGetIndentListForInventorDashBoardBizActionVO;
                        ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
                        ItemPRList.TotalItemCount = (int)result.TotalRow;
                        if (result.IndentList != null)
                        {
                            ItemPRList.Clear();
                            foreach (var item in result.IndentList)
                            {
                                item.isPurchaseRequisitionClosed = true;
                                ItemPRList.Add(item);
                            }

                            dgPendingPRList.ItemsSource = null;
                            dgPendingPRList.ItemsSource = ItemPRList;

                            dgDataPager.Source = null;
                            dgDataPager.PageSize = (int)BizAction.NoOfRecords;
                            dgDataPager.Source = ItemPRList;
                        }
                    }
                }

                Indicatior.Close();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        void PRList_OnRefresh(object sender, RefreshEventArgs e)
        {
            rdbMultiplePurchaseRequisitionClose.IsChecked = false;
            rdbSinglePurchaseRequisitionClose.IsChecked = false;

            cmdMultiplePurchaseRequisitionClose.Visibility = System.Windows.Visibility.Collapsed;

            GetPRList();
        }

        List<clsStoreVO> UserStores = new List<clsStoreVO>();
        private void FillStore()
        {
            UserStoreAssigned = false;
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
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
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };

                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();

                    UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;

                    UserStores.Clear();

                    foreach (var item in UserUnit)
                    {
                        if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                        {    
                            UserStores = item.UserUnitStore;
                        }
                    }
                
                    var resultnew = from item in UserStores select item;
                    if (UserStores != null && UserStores.Count > 0)
                    {
                        UserStoreAssigned = true;
                    }

                    List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
                    StoreListForClinic.Insert(0, Default);

                    cmbToStoreName.ItemsSource = result1.ToList();
                    if (result1.ToList().Count > 0)
                    {
                        cmbToStoreName.SelectedItem = result1.ToList()[0];
                    }
                    
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        cmbFromStoreName.ItemsSource = result1.ToList();                        
                        if (result1.ToList().Count > 0)
                        {
                            cmbFromStoreName.SelectedItem = (result1.ToList())[0];
                        }
                    }

                    else
                    {
                      //  cmbFromStoreName.ItemsSource = StoreListForClinic;
                        cmbFromStoreName.ItemsSource = BizActionObj.ToStoreList.ToList();
                        if (StoreListForClinic.Count > 0)
                        {
                            cmbFromStoreName.SelectedItem = BizActionObj.ToStoreList[0];//StoreListForClinic[0];
                        }
                        // End
                    }
                }
                GetPRList();
            };

            client.CloseAsync();
        }

        private void txtPRNumber_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetPRList();
            }
            //else if (e.Key == Key.Tab)
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
                    GetPRList();
                }
            }
        }

        private void cmbToStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbToStoreName.SelectedItem != null)
                {
                    GetPRList();
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            //DataListPageSize = 5;

            rdbMultiplePurchaseRequisitionClose.IsChecked = false;
            rdbSinglePurchaseRequisitionClose.IsChecked = false;
            cmdMultiplePurchaseRequisitionClose.Visibility = System.Windows.Visibility.Collapsed;

            DataListPageSize = 15;

            GetPRList();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            long FromStore = ((clsStoreVO)this.cmbFromStoreName.SelectedItem).StoreId;
            long ToStore = ((clsStoreVO)this.cmbToStoreName.SelectedItem).StoreId;
            string PRNo = txtPRNumber.Text;
            DateTime FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
            DateTime ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;

            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

            string URL = "../Reports/InventoryPharmacy/PendingPR_DashBoard.aspx?FromStore=" + FromStore + "&UnitID=" + UnitID + "&ToStore=" + ToStore +
                         "&PRNo=" + PRNo + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&UserID="+UserID;

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }


        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Rex Mathew
         * 
         * The below code is called on close indent check box event to close one indent at a time.
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        private void chkClosePurchaseRequisition_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

                if (rdbSinglePurchaseRequisitionClose.IsChecked == true && chk.IsChecked == true)
                    CallClosePurchaseRequisitionWindow();
                else if (rdbSinglePurchaseRequisitionClose.IsChecked == false && rdbMultiplePurchaseRequisitionClose.IsChecked == false)
                {
                    DataListPageSize = 15;
                    GetPRList();
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash IVF", "You haven't selected the above Single OR Multiple selection option ...", MessageBoxControl.MessageBoxButtons.Ok,
                                            MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();

                }
        }



        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By :  : Rex Mathew
         * 
         * The below code Is called by cmdMultipleIndentCloseButton_Click function to allow users to enter comman Remarks for all the selected indents
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        void CallClosePurchaseRequisitionWindow()
        {
            try
            {
                string msgText = "Are you sure you want close the Purchase Requisition for the selected item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ForcefullyClosedPR win1 = new ForcefullyClosedPR();
                        win1.indentNo = ((clsIndentMasterVO)dgPendingPRList.SelectedItem).ID;
                        win1.isBulkPRClosed = (rdbMultiplePurchaseRequisitionClose.IsChecked == true ? true : false);
                        win1.OnSaveButton_Click += new RoutedEventHandler(win1_OnSaveButton_Click);
                        win1.OnCanelButton_Click += new RoutedEventHandler(UserControl_Loaded);
                        win1.Closed += new EventHandler(win1_Closed);

                        win1.BulkClosePRList = new List<clsIndentMasterVO>();

                        if (dgPendingPRList.ItemsSource != null)
                        {
                            foreach (clsIndentMasterVO item in (PagedSortableCollectionView<clsIndentMasterVO>)(dgPendingPRList.ItemsSource))
                            {
                                if (item.PurchaseRequisitionStatus == true)
                                    win1.BulkClosePRList.Add(item);
                            }
                        }
                        win1.Show();
                    }
                    else
                    {
                        ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
                        foreach (clsIndentMasterVO item in dgPendingPRList.ItemsSource)
                        {
                            item.isPurchaseRequisitionClosed = true;
                            item.IndentDashBordStatus = false;
                            ItemPRList.Add(item);
                        }
                        dgPendingPRList.ItemsSource = ItemPRList;

                        rdbMultiplePurchaseRequisitionClose.IsChecked = false;
                        rdbSinglePurchaseRequisitionClose.IsChecked = false;
                        cmdMultiplePurchaseRequisitionClose.Visibility = System.Windows.Visibility.Collapsed;
                        //DataListPageSize = 15;
                        GetPRList();
                    }
                };
                msgWD.Show();
            }
            catch (Exception)
            {
                throw;
            }
        }

  
        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */


        private void win1_Closed(object sender, EventArgs e)
        {
            ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
            ItemPRList.OnRefresh += new EventHandler<RefreshEventArgs>(PRList_OnRefresh);
            DataListPageSize = 15;

            foreach (clsIndentMasterVO item in dgPendingPRList.ItemsSource)
            {
                item.isPurchaseRequisitionClosed = false;
                item.IndentDashBordStatus = false;
                ItemPRList.Add(item);
            }
            dgPendingPRList.ItemsSource = ItemPRList;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = ItemPRList;

            rdbMultiplePurchaseRequisitionClose.IsChecked = false;
            rdbSinglePurchaseRequisitionClose.IsChecked = false;
            cmdMultiplePurchaseRequisitionClose.Visibility = System.Windows.Visibility.Collapsed;

            GetPRList();
        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */

        void win1_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsCloseIndentFromDashboard BizAction = new clsCloseIndentFromDashboard();
                clsGetIndentListForInventorDashBoardBizActionVO BizAction1 = new clsGetIndentListForInventorDashBoardBizActionVO();
                //if (((clsIndentMasterVO)dgPendingIndentList.SelectedItem) != null)
                //{
                ForcefullyClosedPR win1 = (ForcefullyClosedPR)sender;               //new ForcefullyClosedIndent();


                BizAction.isBulkPRClosed = win1.isBulkPRClosed;                 // x-x-x-x-x-x-x-x-x-x Gets Bulk Indent Close status (Rex Mathew) x-x-x-x-x-x-x-x-x-x

                BizAction.BulkCloseIndetList = win1.BulkClosePRList;          // x-x-x-x-x-x-x-x-x-x Gets Bulk Indent Data (Rex Mathew) x-x-x-x-x-x-x-x-x-x

                BizAction.isPRCloseCall = true;                                                  // x-x-x-x-x-x-x-x-x-x The below variable is added to CALL Bulk PR Close function (Rex Mathew) x-x-x-x-x-x-x-x-x-x

                BizAction.IndentID = win1.indentNo; //((clsIndentMasterVO)dgPendingIndentList.SelectedItem).ID;
                BizAction.UnitID = ((clsIndentMasterVO)dgPendingPRList.SelectedItem).UnitID;
                BizAction.Remarks = ((ForcefullyClosedPR)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        //HomeInventoryViewModel win2 = new HomeInventoryViewModel();
                        //win2.GetIndentList();
                        DataListPageSize = 15;
                        GetPRList();
                        //dataGridIndentPager.PageIndex = 0;
                        // ((HomeInventoryViewModel)this.DataContext).GetIndentList();
                        //dgDataPager.PageIndex = 0;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                // }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Rex Mathew
         * 
         * The Below Code allows the system to provide Bulk Indent Close Option as well as Single Indent Close Option
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        private void cmdMultiplePurchaseRequisitionCloseButton_Click(object sender, RoutedEventArgs e)
        {
            CallClosePurchaseRequisitionWindow();
        }


        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */

        private void MultiplePurchaseRequisitionCloseSelection_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsIndentMasterVO> ItemPRSelectList = new PagedSortableCollectionView<clsIndentMasterVO>();


            if (rdbMultiplePurchaseRequisitionClose.IsChecked == true)
            {
                cmdMultiplePurchaseRequisitionClose.Visibility = Visibility.Visible;

                ItemPRSelectList = ((PagedSortableCollectionView<clsIndentMasterVO>)dgPendingPRList.ItemsSource);

                try
                {
                    if (ItemPRSelectList != null)
                    {
                        foreach (clsIndentMasterVO item in ItemPRSelectList)
                        {
                            item.PurchaseRequisitionStatus = true;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            dgPendingPRList.UpdateLayout();
        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */

        private void SinglePurchaseRequisitionCloseSelection_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsIndentMasterVO> ItemPRSelectList = new PagedSortableCollectionView<clsIndentMasterVO>();

            if (rdbSinglePurchaseRequisitionClose.IsChecked == true)
            {
                cmdMultiplePurchaseRequisitionClose.Visibility = Visibility.Collapsed;

                ItemPRSelectList = ((PagedSortableCollectionView<clsIndentMasterVO>)dgPendingPRList.ItemsSource);

                try
                {
                    if (ItemPRSelectList != null)
                    {
                        foreach (clsIndentMasterVO item in ItemPRSelectList)
                        {
                            item.PurchaseRequisitionStatus = false;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            dgPendingPRList.UpdateLayout();
        }
        
        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Rex Mathew
         * 
         * The Above Code allows the system to provide Bulk Indent Close Option as well as Single Indent Close Option
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */


    }

}
