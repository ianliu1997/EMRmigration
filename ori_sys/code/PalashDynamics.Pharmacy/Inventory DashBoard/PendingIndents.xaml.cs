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
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using System.Windows.Data;

namespace PalashDynamics.Pharmacy.Inventory_DashBoard
{


    public partial class PendingIndents : UserControl
    {

        #region

        public List<clsIndentMasterVO> SelectedStoreList = null;
        public List<clsIndentMasterVO> DeletedStoreList = null;

        #endregion


        public PendingIndents()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
            ItemIndentList.OnRefresh += new EventHandler<RefreshEventArgs>(IndentList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = ItemIndentList;


            SelectedStoreList = new List<clsIndentMasterVO>();
            DeletedStoreList = new List<clsIndentMasterVO>();
        }

        # region // Variable Declarartion
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public PagedSortableCollectionView<clsIndentMasterVO> ItemIndentList { get; private set; }
        bool UserStoreAssigned = false;
        List<clsStoreVO> UserStores = new List<clsStoreVO>();

        # endregion

        #region Paging
        public int DataListPageSize
        {
            get
            {
                return ItemIndentList.PageSize;
            }
            set
            {
                if (value == ItemIndentList.PageSize) return;
                ItemIndentList.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
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
                DateTime now = DateTime.Now;
                this.dtpFromDate.SelectedDate = new DateTime(now.Year, now.Month, 1);
                this.dtpToDate.SelectedDate = DateTime.Now;
                cmbFromStoreName.Focus();
                Indicatior.Close();
            }


            IsPageLoded = true;
        }

        public void GetIndentList()
        {
            clsGetIndentListForInventorDashBoardBizActionVO BizAction = new clsGetIndentListForInventorDashBoardBizActionVO();
            BizAction.IndentList = new List<clsIndentMasterVO>();
            Indicatior.Show();
            //if (BizAction.IsOrderBy != null)
            //    BizAction.IsOrderBy = BizAction.IsOrderBy;

            BizAction.Date = null;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            { BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; }
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;


            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                {
                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    dtpFromDate.Focus();
                    Indicatior.Close();
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

            BizAction.IndentNO = txtIndentNumber.Text;

            

            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecords = ItemIndentList.PageSize;
            BizAction.StartRowIndex = ItemIndentList.PageIndex * ItemIndentList.PageSize;
            //By Anjali.................
            BizAction.IsIndent = true;
            //...........................

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetIndentListForInventorDashBoardBizActionVO)arg.Result != null)
                    {
                        clsGetIndentListForInventorDashBoardBizActionVO result = arg.Result as clsGetIndentListForInventorDashBoardBizActionVO;
                        ItemIndentList.TotalItemCount = (int)result.TotalRow;
                        if (result.IndentList != null)
                        {
                            ItemIndentList.Clear();
                            foreach (var item in result.IndentList)
                            {
                                item.isIndentClosed = true;
                                ItemIndentList.Add(item);
                            }
                            //PagedCollectionView pcvDiagnosisListHistory = new PagedCollectionView(ItemIndentList);
                            //dgPendingIndentList.ItemsSource = pcvDiagnosisListHistory;
                            dgPendingIndentList.ItemsSource = null;
                            dgPendingIndentList.ItemsSource = ItemIndentList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = (int)BizAction.NoOfRecords;
                            dgDataPager.Source = ItemIndentList;
                        }
                    }
                }
                Indicatior.Close();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

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
                        // cmbFromStoreName.ItemsSource = StoreListForClinic;
                        cmbFromStoreName.ItemsSource = BizActionObj.ToStoreList.ToList();
                        if (StoreListForClinic.Count > 0)
                        {
                            //  cmbFromStoreName.SelectedItem = StoreListForClinic[0];
                            cmbFromStoreName.SelectedItem = BizActionObj.ToStoreList[0];
                        }
                    }
                }
                GetIndentList();
            };

            client.CloseAsync();

        }

        void IndentList_OnRefresh(object sender, RefreshEventArgs e)
        {
            rdbMultipleIndentClose.IsChecked = false;
            rdbSingleIndentClose.IsChecked = false;

            cmdMultipleIndentClose.Visibility = System.Windows.Visibility.Collapsed;

            

            GetIndentList();
        }

        private void cmbFromStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbFromStoreName.SelectedItem != null)
                {
                    GetIndentList();
                }
            }
        }

        private void cmbToStoreName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (cmbToStoreName.SelectedItem != null)
                {
                    GetIndentList();
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            //DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            //DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            //DataListPageSize = 5;

            rdbMultipleIndentClose.IsChecked = false;
            rdbSingleIndentClose.IsChecked = false;
            cmdMultipleIndentClose.Visibility = System.Windows.Visibility.Collapsed;

            DataListPageSize = 15;

            GetIndentList();
        }

        private void txtIndentNumber_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetIndentList();
            }
            //else if (e.Key == Key.Tab)
            //{
            //    FillIndentList();
            //}
        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {

            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID; ;
            long FromStore = ((clsStoreVO)this.cmbFromStoreName.SelectedItem).StoreId;
            long ToStore = ((clsStoreVO)this.cmbToStoreName.SelectedItem).StoreId;
            string IndentNo = txtIndentNumber.Text;
            DateTime FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
            DateTime ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;

            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

            string URL = "../Reports/InventoryPharmacy/PendingIndents_Dash.aspx?FromStore=" + FromStore + "&UnitID=" + UnitID + "&ToStore=" + ToStore +
                         "&IndentNo=" + IndentNo + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&UserID=" + UserID;

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Pranav Chorge
         * 
         * The below code is called on close indent check box event to close one indent at a time.
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        private void chkCloseIndent_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            if (rdbSingleIndentClose.IsChecked == true && chk.IsChecked == true)
            {
                if (dgPendingIndentList.SelectedItem != null)
                {
                    if ((((clsIndentMasterVO)dgPendingIndentList.SelectedItem).IssueBaseItemQty == ((clsIndentMasterVO)dgPendingIndentList.SelectedItem).ReceivedBaseItemQty) || (((clsIndentMasterVO)
                        dgPendingIndentList.SelectedItem).IssueBaseItemQty == 0 && ((clsIndentMasterVO)dgPendingIndentList.SelectedItem).ReceivedBaseItemQty == 0))
                    {
                        CallCloseIndentWindow();
                    }
                    else
                    {
                        chk.IsChecked = false;
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash IVF", "Issued Item are not completely Received...", MessageBoxControl.MessageBoxButtons.Ok,
                                                MessageBoxControl.MessageBoxIcon.Question);
                        msgWD.Show();
                    }
                }
            }
            else if (rdbSingleIndentClose.IsChecked == false && rdbMultipleIndentClose.IsChecked == false)
            {
                DataListPageSize = 15;
                GetIndentList();
                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash IVF", "You haven't selected the above Single OR Multiple selection option ...", MessageBoxControl.MessageBoxButtons.Ok,
                                        MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();

            }
        }


        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By :  : Pranav Chorge
         * 
         * The below code Is called by cmdMultipleIndentCloseButton_Click function to allow users to enter comman Remarks for all the selected indents
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        void CallCloseIndentWindow()
        {
            try
            {
                string msgText = "Are you sure you want close the indent for the selected item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ForcefullyClosedIndent win1 = new ForcefullyClosedIndent();
                        win1.indentNo = ((clsIndentMasterVO)dgPendingIndentList.SelectedItem).ID;
                        win1.isBulkIndentClosed = (rdbMultipleIndentClose.IsChecked == true ? true : false);
                        win1.OnSaveButton_Click += new RoutedEventHandler(win1_OnSaveButton_Click);
                        win1.OnCanelButton_Click += new RoutedEventHandler(UserControl_Loaded);
                        win1.Closed += new EventHandler(win1_Closed);

                        win1.BulkCloseIndetList = new List<clsIndentMasterVO>();

                        if (dgPendingIndentList.ItemsSource != null)
                        {
                            foreach (clsIndentMasterVO item in (PagedSortableCollectionView<clsIndentMasterVO>)(dgPendingIndentList.ItemsSource))
                            {
                                if (item.IndentCloseStatus == true)
                                    win1.BulkCloseIndetList.Add(item);
                            }
                        }
                        win1.Show();
                    }
                    else
                    {
                        ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
                        foreach (clsIndentMasterVO item in dgPendingIndentList.ItemsSource)
                        {
                            item.isIndentClosed = true;
                            item.IndentDashBordStatus = false;
                            ItemIndentList.Add(item);
                        }
                        dgPendingIndentList.ItemsSource = ItemIndentList;

                        rdbMultipleIndentClose.IsChecked = false;
                        rdbSingleIndentClose.IsChecked = false;
                        cmdMultipleIndentClose.Visibility = System.Windows.Visibility.Collapsed;
                        //DataListPageSize = 15;
                        GetIndentList();

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
            ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
            ItemIndentList.OnRefresh += new EventHandler<RefreshEventArgs>(IndentList_OnRefresh);
            DataListPageSize = 15;

            foreach (clsIndentMasterVO item in dgPendingIndentList.ItemsSource)
            {
                item.isIndentClosed = false;
                item.IndentDashBordStatus = false;
                ItemIndentList.Add(item);
            }
            dgPendingIndentList.ItemsSource = ItemIndentList;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = ItemIndentList;

            rdbMultipleIndentClose.IsChecked = false;
            rdbSingleIndentClose.IsChecked = false;
            cmdMultipleIndentClose.Visibility = System.Windows.Visibility.Collapsed;

            GetIndentList();
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
                ForcefullyClosedIndent win1 = (ForcefullyClosedIndent)sender;               //new ForcefullyClosedIndent();


                BizAction.isBulkIndentClosed = win1.isBulkIndentClosed;                 // x-x-x-x-x-x-x-x-x-x Gets Bulk Indent Close status (Rex Mathew) x-x-x-x-x-x-x-x-x-x

                BizAction.BulkCloseIndetList = win1.BulkCloseIndetList;                 // x-x-x-x-x-x-x-x-x-x Gets Bulk Indent Data (Rex Mathew) x-x-x-x-x-x-x-x-x-x

                BizAction.IndentID = win1.indentNo; //((clsIndentMasterVO)dgPendingIndentList.SelectedItem).ID;
                BizAction.UnitID = ((clsIndentMasterVO)dgPendingIndentList.SelectedItem).UnitID;
                BizAction.Remarks = ((ForcefullyClosedIndent)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        //HomeInventoryViewModel win2 = new HomeInventoryViewModel();
                        //win2.GetIndentList();
                        DataListPageSize = 15;
                        GetIndentList();
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

        private void cmdMultipleIndentCloseButton_Click(object sender, RoutedEventArgs e)
        {
            CallCloseIndentWindow();
        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */

        private void MultipleIndentCloseSelection_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsIndentMasterVO> ItemIndentSelectList = new PagedSortableCollectionView<clsIndentMasterVO>();

            if (rdbMultipleIndentClose.IsChecked == true)
            {

                cmdMultipleIndentClose.Visibility = Visibility.Visible;

                ItemIndentSelectList = ((PagedSortableCollectionView<clsIndentMasterVO>)dgPendingIndentList.ItemsSource);

                try
                {
                    if (ItemIndentSelectList != null)
                    {
                        foreach (clsIndentMasterVO item in ItemIndentSelectList)
                        {
                            if ((item.IssueBaseItemQty == item.ReceivedBaseItemQty) || (item.IssueBaseItemQty == 0 && item.ReceivedBaseItemQty == 0))
                                item.IndentCloseStatus = true;
                            else
                            {

                                if (dgPendingIndentList.ItemsSource != null && ((PagedSortableCollectionView<clsIndentMasterVO>)dgPendingIndentList.ItemsSource).Count > 0)
                                {
                                    foreach (var item2 in dgPendingIndentList.Columns)
                                    {
                                        if (item2.Header.ToString() == "Close Indent")
                                        {
                                            ((CheckBox)item2.GetCellContent(item)).IsEnabled = false;
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            dgPendingIndentList.UpdateLayout();
        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x  */

        private void SingleIndentCloseSelection_Click(object sender, RoutedEventArgs e)
        {

            PagedSortableCollectionView<clsIndentMasterVO> ItemIndentSelectList = new PagedSortableCollectionView<clsIndentMasterVO>();

            if (rdbSingleIndentClose.IsChecked == true)
            {
                cmdMultipleIndentClose.Visibility = Visibility.Collapsed;

                ItemIndentSelectList = ((PagedSortableCollectionView<clsIndentMasterVO>)dgPendingIndentList.ItemsSource);

                try
                {
                    if (ItemIndentSelectList != null)
                    {
                        foreach (clsIndentMasterVO item in ItemIndentSelectList)
                        {
                            item.IndentCloseStatus = false;

                            if (dgPendingIndentList.ItemsSource != null && ((PagedSortableCollectionView<clsIndentMasterVO>)dgPendingIndentList.ItemsSource).Count > 0)
                            {
                                foreach (var item2 in dgPendingIndentList.Columns)
                                {
                                    if (item2.Header.ToString() == "Close Indent")
                                    {
                                        ((CheckBox)item2.GetCellContent(item)).IsEnabled = true;
                                        break;
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            dgPendingIndentList.UpdateLayout();
        }

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x 
         * 
         * Designed By : Rex Mathew
         * 
         * The Above Code allows the system to provide Bulk Indent Close Option as well as Single Indent Close Option
         * 
          x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */


        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }


    }
}
