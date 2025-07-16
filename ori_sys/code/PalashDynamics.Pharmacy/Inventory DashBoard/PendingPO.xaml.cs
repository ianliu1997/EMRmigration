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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;

namespace PalashDynamics.Pharmacy.Inventory_DashBoard
{
    public partial class PendingPO : UserControl
    {
        public PendingPO()
        {
            InitializeComponent();
            PurchaseOrderList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            this.Loaded += new RoutedEventHandler(UserControl_Loaded);
            DataListPageSize = 24;
            PurchaseOrderList.OnRefresh += new EventHandler<RefreshEventArgs>(PurchaseOrderList_OnRefresh);
        }

        # region // Variable Declarartion

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        public PagedSortableCollectionView<clsPurchaseOrderVO> PurchaseOrderList { get; private set; }

        # endregion

        #region Paging
        public int DataListPageSize
        {
            get
            {
                return PurchaseOrderList.PageSize;
            }
            set
            {
                if (value == PurchaseOrderList.PageSize) return;
                PurchaseOrderList.PageSize = value;
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
                DateTime now = DateTime.Now;
                dtpFromDate.SelectedDate = new DateTime(now.Year, now.Month, 1);  //DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;
                FillStore();
                FillSupplier();
                // GetPendingPurchaseOrder();
                cmbStore.Focus();
                Indicatior.Close();
            }
            IsPageLoded = true;
        }

        void PurchaseOrderList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetPendingPurchaseOrder();
        }

        public void GetPendingPurchaseOrder()
        {
            clsGetPendingPurchaseOrderBizActionVO BizAction = new clsGetPendingPurchaseOrderBizActionVO();
            BizAction.PurchaseOrderList = new List<clsPurchaseOrderVO>();
            BizAction.Date = null;

            //if (BizActionObjectForExpiryItemList.IsOrderBy != null)
            //    BizAction.IsOrderBy = BizActionObjectForPO.IsOrderBy;

            if (dtpFromDate.SelectedDate != null)
                BizAction.searchFromDate = dtpFromDate.SelectedDate.Value;
            if (dtpToDate.SelectedDate != null)
                BizAction.searchToDate = dtpToDate.SelectedDate.Value;
            // BizAction.SearchStoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId == null ? 0 : ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            if ((clsStoreVO)cmbStore.SelectedItem != null)
                BizAction.SearchStoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            if (cmbSupplier.SelectedItem != null)
            {
                BizAction.SearchSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

            BizAction.PONO = txtPONO.Text;
            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecordShow = PurchaseOrderList.PageSize;
            BizAction.StartIndex = PurchaseOrderList.PageIndex * PurchaseOrderList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    if ((clsGetPendingPurchaseOrderBizActionVO)arg.Result != null)
                    {
                        clsGetPendingPurchaseOrderBizActionVO result = arg.Result as clsGetPendingPurchaseOrderBizActionVO;
                        PurchaseOrderList.TotalItemCount = (int)result.OutputTotalRows;
                        if (result.PurchaseOrderList != null)
                        {
                            PurchaseOrderList.Clear();
                            foreach (var item in result.PurchaseOrderList)
                            {
                                PurchaseOrderList.Add(item);
                            }
                        }
                    }
                    dgPendingPOList.ItemsSource = PurchaseOrderList;
                    dgDataPager.Source = PurchaseOrderList;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        List<clsStoreVO> objStoreList = new List<clsStoreVO>();
        private void FillStore()
        {
            try
            {
                clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();

                //False when we want to fetch all items
                clsItemMasterVO obj = new clsItemMasterVO();

                BizActionObj.IsUserwiseStores = true;
                BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObj.ToStoreList = new List<clsStoreVO>();
                obj.RetrieveDataFlag = false;
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
                        // BizActionObj.ToStoreList.Insert(0, Default);
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                        {
                            var result = from item in BizActionObj.ItemMatserDetails
                                         where item.Status == true // item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId &&
                                         select item;
                            objStoreList = (List<clsStoreVO>)result.ToList();
                        }
                        else
                        {
                            var result = from item in BizActionObj.ItemMatserDetails
                                         where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                         select item;
                            objStoreList = BizActionObj.ToStoreList.ToList();//(List<clsStoreVO>)result.ToList();
                        }

                        objStoreList.Insert(0, Default);
                        if (objStoreList != null)
                        {
                            cmbStore.ItemsSource = null;
                            cmbStore.ItemsSource = objStoreList;
                            cmbStore.SelectedItem = objStoreList[0];

                        }

                        FillSupplier();
                    }

                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;
                        cmbSupplier.SelectedItem = objList[0];
                    }
                    GetPendingPurchaseOrder();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void txtPONO_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetPendingPurchaseOrder();
            }

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            GetPendingPurchaseOrder();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long StoreID = 0;
            long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            long UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            long SuplierID = 0;
            string PONo = "";
            StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            SuplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
            PONo = txtPONO.Text;
            DateTime FromDate = ((DateTime)this.dtpFromDate.SelectedDate).Date;
            DateTime ToDate = ((DateTime)this.dtpToDate.SelectedDate).Date;
            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

            string URL = "../Reports/InventoryPharmacy/PendingPOForDashBoard.aspx?StoreID=" + StoreID + "&SuplierID=" + SuplierID + "&PONo=" + PONo + "&UnitID=" + UnitID + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&UserID=" + UserID;

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }


        /* x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x																							x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        private void cmdUpdatePOAutoCloseDuration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((clsPurchaseOrderVO)dgPendingPOList.SelectedItem) != null)
                {
                    if (((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).IsCancelded == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash IVF", "Unable to modify PO Close Duration, PO is already Cancelled", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD1.Show();
                        return;
                    }
                    else
                    {


                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash IVF", "Are you sure you want to modify the PO Close Duration ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                POCloseDurationWindow Win = new POCloseDurationWindow();
                                Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                                Win.txtPOAutoCloseDays.Text = ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).POAutoCloseDuration.ToString();
                                Win.txtPOAutoCloseDate.Text = ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).POAutoCloseDurationDate.ToString().Length == 0 ? "" : ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).POAutoCloseDurationDate.Value.ToShortDateString(); 
                                Win.txtReasonForChange.Text = ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).POAutoCloseReason;
                                Win.ApprovedByLvl2Date = ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).ApprovedByLvl2Date;
                                Win.Show();
                            }
                        };
                        msgWD.Show();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /* x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x																							x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateRemarkForCancellationPO bizactionVO = new clsUpdateRemarkForCancellationPO();
            bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
            try
            {
                bizactionVO.IsPOAutoCloseCalled = true;     // this variable is set to True to call the PO Auto Close Duration Form
                bizactionVO.PurchaseOrder.PONO = ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).PONO;
                bizactionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizactionVO.PurchaseOrder.POAutoCloseDuration =  Convert.ToInt32(((POCloseDurationWindow)sender).txtPOAutoCloseDays.Text);
                bizactionVO.PurchaseOrder.POAutoCloseDurationDate = Convert.ToDateTime(((POCloseDurationWindow)sender).txtPOAutoCloseDate.Text);
                bizactionVO.PurchaseOrder.POAutoCloseReason  = ((POCloseDurationWindow)sender).txtReasonForChange.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {

                    if (args.Error == null && args.Result != null)
                    {
                        clsCancelPurchaseOrderBizActionVO bizActionObj = new clsCancelPurchaseOrderBizActionVO();
                        bizActionObj.PurchaseOrder = new clsPurchaseOrderVO();
                        bizActionObj.PurchaseOrder.PONO = ((clsPurchaseOrderVO)dgPendingPOList.SelectedItem).PONO;
                        bizActionObj.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        bizactionVO.PurchaseOrder.POAutoCloseDuration = Convert.ToInt32(((POCloseDurationWindow)sender).txtPOAutoCloseDays.Text);
                        bizactionVO.PurchaseOrder.POAutoCloseDurationDate = Convert.ToDateTime(((POCloseDurationWindow)sender).txtPOAutoCloseDate.Text);
                        bizactionVO.PurchaseOrder.POAutoCloseReason = ((POCloseDurationWindow)sender).txtReasonForChange.Text;

                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        Client.ProcessCompleted += (s1, arg1) =>
                        {
                            if (arg1.Error == null)
                            {
                                if (arg1.Result != null)
                                {
                                    GetPendingPurchaseOrder();
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while modifing P O Close Duration.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        };

                        Client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                };
                client.ProcessAsync(bizactionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
