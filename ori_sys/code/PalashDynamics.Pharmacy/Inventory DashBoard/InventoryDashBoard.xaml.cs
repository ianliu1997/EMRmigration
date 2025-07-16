using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.UserControls;
using PalashDynamics.Pharmacy.ViewModels;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;


namespace PalashDynamics.Pharmacy
{
    public partial class InventoryDashBoard : UserControl
    {
        public InventoryDashBoard()
        {
            InitializeComponent();
            this.DataContext = new HomeInventoryViewModel();
            this.Loaded += new RoutedEventHandler(InventoryDashBoard_Loaded);
        }

        PagedCollectionView pcv = null;
        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        HomeInventoryViewModel CurrentStock; // = new HomeInventoryViewModel();
        public PagedSortableCollectionView<clsIndentMasterVO> ItemIndentList { get; private set; }
        public PagedSortableCollectionView<clsItemReorderDetailVO> ReorderItemList { get; private set; }

        //By Anjali
        public PagedSortableCollectionView<clsIndentMasterVO> ItemPRList { get; private set; }
        //................

        void InventoryDashBoard_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                CurrentStock = new HomeInventoryViewModel();

                pcv = new PagedCollectionView(CurrentStock.ItemStockList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("StoreName"));
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("ItemName"));

                // grdAlerts.ItemsSource = pcv;

                rdbExpiredItemsAsc.IsChecked = true;
           //     rdbPendingPOAsc.IsChecked = true;    //By Umesh
          //     rdbIndentAsc.IsChecked = true;       //By Umesh

                ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();

                //      dgIndentList.ItemsSource = CurrentStock.ItemIndentList;     //By Umesh
                //      dataGridIndentPager.Source = CurrentStock.ItemIndentList;         //By Umesh

                dgExpiredListGrid.ItemsSource = CurrentStock.ExpiryItemList;
                DataPagerExpired.Source = CurrentStock.ExpiryItemList;

       //         dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;     //By Umesh
        //        grdPendingPO.Source = CurrentStock.PurchaseOrderList;         //By Umesh

                //By Anjali..........
                ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
                ItemPRList.PageSize = 10;
                //  GetPRList();   //By Umesh
                //.....................................................

                ReorderItemList = new PagedSortableCollectionView<clsItemReorderDetailVO>();
          //      dgItemReorderList.ItemsSource = CurrentStock.ReorderItemList;        //By Umesh
           //     dataGridItemReorderPager.Source = CurrentStock.ReorderItemList;      //By Umesh

                PageSize = 10;
                Indicatior.Close();
            }
        }

        private void ExpiredItems_Maximized(object sender, EventArgs e)
        {


        }

        private void ExpiredItems_Minimized(object sender, EventArgs e)
        {

        }

        #region //By Umesh

        //void win1_OnSaveButton_Click(object sender, RoutedEventArgs e)   
        //{
        //    try
        //    {
        //        clsCloseIndentFromDashboard BizAction = new clsCloseIndentFromDashboard();
        //        clsGetIndentListForInventorDashBoardBizActionVO BizAction1 = new clsGetIndentListForInventorDashBoardBizActionVO();

        //        if (((clsIndentMasterVO)dgIndentList.SelectedItem) != null)
        //        {

        //            BizAction.IndentID = ((clsIndentMasterVO)dgIndentList.SelectedItem).ID;
        //            BizAction.UnitID = ((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID;
        //            BizAction.Remarks = ((ForcefullyClosedIndent)sender).txtAppReason.Text;

        //            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //            client.ProcessCompleted += (s, arg) =>
        //            {
        //                if (arg.Error == null && arg.Result != null)
        //                {
        //                    //HomeInventoryViewModel win2 = new HomeInventoryViewModel();
        //                    //win2.GetIndentList();
        //                    GetIndentList();
        //                    //dataGridIndentPager.PageIndex = 0;
        //                    // ((HomeInventoryViewModel)this.DataContext).GetIndentList();


        //                    dataGridIndentPager.PageIndex = 0;
        //                }

        //            };
        //            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //            client.CloseAsync();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        public int PageSize
        {
            get
            {
                return ItemIndentList.PageSize;
            }
            set
            {
                if (value == ItemIndentList.PageSize) return;
                ItemIndentList.PageSize = value;


            }
        }

        //public void GetIndentList()
        //{
        //    clsGetIndentListForInventorDashBoardBizActionVO BizAction = new clsGetIndentListForInventorDashBoardBizActionVO();
        //    BizAction.IndentList = new List<clsIndentMasterVO>();

        //    if (BizAction.IsOrderBy != null)
        //        BizAction.IsOrderBy = BizAction.IsOrderBy;

        //    BizAction.Date = null;
        //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
        //    {
        //        BizAction.UnitID = 0;
        //    }
        //    else
        //        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


        //    BizAction.IsPagingEnabled = true;
        //    BizAction.NoOfRecords = ItemIndentList.PageSize;
        //    BizAction.StartRowIndex = ItemIndentList.PageIndex * ItemIndentList.PageSize;
        //    //By Anjali.................
        //    BizAction.IsIndent = true;
        //    //...........................


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            if ((clsGetIndentListForInventorDashBoardBizActionVO)arg.Result != null)
        //            {
        //                clsGetIndentListForInventorDashBoardBizActionVO result = arg.Result as clsGetIndentListForInventorDashBoardBizActionVO;
        //                ItemIndentList.TotalItemCount = (int)result.TotalRow;
        //                if (result.IndentList != null)
        //                {
        //                    ItemIndentList.Clear();
        //                    foreach (var item in result.IndentList)
        //                    {
        //                        item.isIndentClosed = true;
        //                        ItemIndentList.Add(item);
        //                    }

        //                    dgIndentList.ItemsSource = ItemIndentList;
        //                    dataGridIndentPager.Source = ItemIndentList;

        //                }
        //            }



        //        }


        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}
        //private void chkCloseIndent_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string msgText = "Are you sure you want close the indent for the selected item ?";

        //        MessageBoxControl.MessageBoxChildWindow msgWD =
        //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

        //        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
        //        {
        //            if (res == MessageBoxResult.Yes)
        //            {
        //                ForcefullyClosedIndent win1 = new ForcefullyClosedIndent();
        //                win1.OnSaveButton_Click += new RoutedEventHandler(win1_OnSaveButton_Click);
        //                win1.OnCanelButton_Click += new RoutedEventHandler(InventoryDashBoard_Loaded);
        //                win1.Closed += new EventHandler(win1_Closed);
        //                win1.Show();
        //            }
        //            else
        //            {
        //                ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //                foreach (clsIndentMasterVO item in dgIndentList.ItemsSource)
        //                {
        //                    item.isIndentClosed = true;
        //                    item.IndentDashBordStatus = false;
        //                    ItemIndentList.Add(item);
        //                }
        //                dgIndentList.ItemsSource = ItemIndentList;
        //            }
        //        };

        //        msgWD.Show();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        //private void win1_Closed(object sender, EventArgs e)
        //{
        //    ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //    foreach (clsIndentMasterVO item in dgIndentList.ItemsSource)
        //    {
        //        item.isIndentClosed = true;
        //        item.IndentDashBordStatus = false;
        //        ItemIndentList.Add(item);
        //    }
        //    dgIndentList.ItemsSource = ItemIndentList;
        //}

        #endregion

        private void btnExpiredItemsAsc_Click(object sender, RoutedEventArgs e)
        {

            if (rdbExpiredItemsAsc.IsChecked == true)
            {
                CurrentStock.isOrderBy = false;
                CurrentStock = new HomeInventoryViewModel(CurrentStock.isOrderBy);
            }

            ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();

            //dgIndentList.ItemsSource = CurrentStock.ItemIndentList;
            //dataGridIndentPager.Source = CurrentStock.ItemIndentList;

            dgExpiredListGrid.ItemsSource = CurrentStock.ExpiryItemList;
            DataPagerExpired.Source = CurrentStock.ExpiryItemList;

            //dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;
            //grdPendingPO.Source = CurrentStock.PurchaseOrderList;

            //dgExpiredListGrid.ItemsSource = CurrentStock.ExpiryItemList;
            //DataPagerExpired.Source = CurrentStock.ExpiryItemList;

            //PageSize = 10;

        }

        private void btnExpiredItemsDesc_Click(object sender, RoutedEventArgs e)
        {

            if (rdbExpiredItemsDesc.IsChecked == true)
            {
                CurrentStock.isOrderBy = true;
                CurrentStock = new HomeInventoryViewModel(CurrentStock.isOrderBy);
            }

            ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();

            //dgIndentList.ItemsSource = CurrentStock.ItemIndentList;
            //dataGridIndentPager.Source = CurrentStock.ItemIndentList;

            dgExpiredListGrid.ItemsSource = CurrentStock.ExpiryItemList;
            DataPagerExpired.Source = CurrentStock.ExpiryItemList;

            //dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;
            //grdPendingPO.Source = CurrentStock.PurchaseOrderList;

        }


   # region  //Commented By Umesh
        //private void rdbPendingPOAsc_Click(object sender, RoutedEventArgs e)
        //{

        //    if (rdbPendingPOAsc.IsChecked == true)
        //    {
        //        CurrentStock.isOrderBy = false;
        //        CurrentStock = new HomeInventoryViewModel(CurrentStock.isOrderBy);
        //    }

        //    ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();

        //    //dgIndentList.ItemsSource = CurrentStock.ItemIndentList;
        //    //dataGridIndentPager.Source = CurrentStock.ItemIndentList;

        //    //dgExpiredListGrid.ItemsSource = CurrentStock.ExpiryItemList;
        //    //DataPagerExpired.Source = CurrentStock.ExpiryItemList;

        //    dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;
        //    grdPendingPO.Source = CurrentStock.PurchaseOrderList;

        //}

        //private void rdbPendingPODesc_Click(object sender, RoutedEventArgs e)
        //{

        //    if (rdbPendingPODesc.IsChecked == true)
        //    {
        //        CurrentStock.isOrderBy = true;
        //        CurrentStock = new HomeInventoryViewModel(CurrentStock.isOrderBy);
        //    }

        //    //CurrentStock = new HomeInventoryViewModel(rdbExpiredItemsDesc.IsChecked);
        //    //ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //    //CurrentStock.isOrderBy = true;
        //    dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;
        //    grdPendingPO.Source = CurrentStock.PurchaseOrderList;

        //}

        //private void rdbIndentAsc_Click(object sender, RoutedEventArgs e)
        //{

        //    if (rdbIndentAsc.IsChecked == true)
        //    {
        //        CurrentStock.isOrderBy = false;
        //        CurrentStock = new HomeInventoryViewModel(CurrentStock.isOrderBy);
        //    }

        //    ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();

        //    dgIndentList.ItemsSource = CurrentStock.ItemIndentList;
        //    dataGridIndentPager.Source = CurrentStock.ItemIndentList;

        //    //dgExpiredListGrid.ItemsSource = CurrentStock.ExpiryItemList;
        //    //DataPagerExpired.Source = CurrentStock.ExpiryItemList;

        //    //dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;
        //    //grdPendingPO.Source = CurrentStock.PurchaseOrderList;

        //}

        //private void rdbIndentDesc_Click(object sender, RoutedEventArgs e)
        //{

        //    if (rdbIndentDesc.IsChecked == true)
        //    {
        //        CurrentStock.isOrderBy = true;
        //        CurrentStock = new HomeInventoryViewModel(CurrentStock.isOrderBy);
        //    }

        //    //CurrentStock = new HomeInventoryViewModel(rdbExpiredItemsDesc.IsChecked);
        //    //ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //    //CurrentStock.isOrderBy = true;

        //    //dgPendingPO.ItemsSource = CurrentStock.PurchaseOrderList;
        //    //grdPendingPO.Source = CurrentStock.PurchaseOrderList;

        //    dgIndentList.ItemsSource = CurrentStock.ItemIndentList;
        //    dataGridIndentPager.Source = CurrentStock.ItemIndentList;

        //}

        ////By Anjali......................
        //public void GetPRList()
        //{
        //    clsGetIndentListForInventorDashBoardBizActionVO BizAction = new clsGetIndentListForInventorDashBoardBizActionVO();
        //    BizAction.IndentList = new List<clsIndentMasterVO>();

        //    if (rdbPRAsc.IsChecked == true)
        //        BizAction.IsOrderBy = false;
        //    else if (rdbPRDesc.IsChecked == true)
        //        BizAction.IsOrderBy = true;

        //    BizAction.Date = null;
        //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
        //    {
        //        BizAction.UnitID = 0;
        //    }
        //    else
        //        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

        //    ItemPRList.PageSize = 10;
        //    BizAction.IsPagingEnabled = true;
        //    BizAction.NoOfRecords = ItemPRList.PageSize;
        //    BizAction.StartRowIndex = ItemPRList.PageIndex * ItemPRList.PageSize;
        //    BizAction.IsIndent = false;


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            if ((clsGetIndentListForInventorDashBoardBizActionVO)arg.Result != null)
        //            {
        //                clsGetIndentListForInventorDashBoardBizActionVO result = arg.Result as clsGetIndentListForInventorDashBoardBizActionVO;
        //                ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //                ItemPRList.TotalItemCount = (int)result.TotalRow;
        //                if (result.IndentList != null)
        //                {
        //                    ItemPRList.Clear();
        //                    foreach (var item in result.IndentList)
        //                    {
        //                        item.isIndentClosed = true;
        //                        ItemPRList.Add(item);
        //                    }

        //                    dgPRList.ItemsSource = ItemPRList;
        //                    dataGridPRPager.Source = ItemPRList;

        //                }
        //            }



        //        }


        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}

        //private void rdbPRAsc_Click(object sender, RoutedEventArgs e)
        //{
        //    GetPRList();
        //}

        //private void rdbPRDesc_Click(object sender, RoutedEventArgs e)
        //{
        //    GetPRList();
        //}

        //private void chkClosePR_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string msgText = "Are you sure you want close the Purchase Requisition for the selected item ?";

        //        MessageBoxControl.MessageBoxChildWindow msgWD =
        //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

        //        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
        //        {
        //            if (res == MessageBoxResult.Yes)
        //            {
        //                ForcefullyClosedPR win1 = new ForcefullyClosedPR();
        //                win1.OnSaveButton_Click += new RoutedEventHandler(win2_OnSaveButton_Click);
        //                win1.OnCanelButton_Click += new RoutedEventHandler(InventoryDashBoard_Loaded);
        //                win1.Closed += new EventHandler(win12Closed);
        //                win1.Show();
        //            }
        //            else
        //            {
        //                ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //                foreach (clsIndentMasterVO item in dgPRList.ItemsSource)
        //                {
        //                    item.isIndentClosed = true;
        //                    item.IndentDashBordStatus = false;
        //                    ItemPRList.Add(item);
        //                }
        //                dgPRList.ItemsSource = ItemPRList;
        //            }
        //        };

        //        msgWD.Show();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //private void win12Closed(object sender, EventArgs e)
        //{
        //    ItemPRList = new PagedSortableCollectionView<clsIndentMasterVO>();
        //    foreach (clsIndentMasterVO item in dgPRList.ItemsSource)
        //    {
        //        item.isIndentClosed = true;
        //        item.IndentDashBordStatus = false;
        //        ItemPRList.Add(item);
        //    }
        //    dgPRList.ItemsSource = ItemPRList;
        //}
        //void win2_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        clsCloseIndentFromDashboard BizAction = new clsCloseIndentFromDashboard();
        //        clsGetIndentListForInventorDashBoardBizActionVO BizAction1 = new clsGetIndentListForInventorDashBoardBizActionVO();

        //        if (((clsIndentMasterVO)dgPRList.SelectedItem) != null)
        //        {

        //            BizAction.IndentID = ((clsIndentMasterVO)dgPRList.SelectedItem).ID;
        //            BizAction.UnitID = ((clsIndentMasterVO)dgPRList.SelectedItem).UnitID;
        //            BizAction.Remarks = ((ForcefullyClosedPR)sender).txtAppReason.Text;

        //            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //            client.ProcessCompleted += (s, arg) =>
        //            {
        //                if (arg.Error == null && arg.Result != null)
        //                {
        //                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Purchase Requisition closed successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                    msgWin.Show();
        //                    GetPRList();
        //                    dataGridPRPager.PageIndex = 0;
        //                }

        //            };
        //            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //            client.CloseAsync();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        //................................
  # endregion
    }
}
