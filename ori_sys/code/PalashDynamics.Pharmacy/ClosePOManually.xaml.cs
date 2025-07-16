using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using PalashDynamics.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Input;

namespace PalashDynamics.Pharmacy
{
    public partial class ClosePOManually : UserControl
    {
        public ClosePOManually()
        {
            InitializeComponent();
            this.DataContext = new clsPurchaseOrderVO();
            DataList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 4;
        }
        Boolean IsPageLoded = false;

        public ObservableCollection<clsPurchaseOrderDetailVO> PoIndentDetails { get; set; }
        public ObservableCollection<clsPurchaseOrderDetailVO> PurchaseOrderItems { get; set; }
        public ObservableCollection<clsPurchaseOrderVO> purchaseOrderList { get; set; }
        public string msgTitle = "";
        public string msgText = "";

        public PagedSortableCollectionView<clsPurchaseOrderVO> DataList { get; private set; }

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
            if (cmbStore.ItemsSource != null && cmbSupplier.ItemsSource != null)
            {
                cmbStore.SelectedItem = ((List<MasterListItem>)cmbStore.ItemsSource)[0];
                cmbSupplier.SelectedItem = ((List<MasterListItem>)cmbSupplier.ItemsSource)[0];
            }
            FillPurchaseOrderDataGrid();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderItems.Clear();
            cmbStore.SelectedValue = 0;
            cmbSupplier.SelectedValue = 0;
            FillPurchaseOrderDataGrid();
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                FillStore();
                PoIndentDetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                purchaseOrderList = new ObservableCollection<clsPurchaseOrderVO>();
                dgPOList.ItemsSource = null;
                dgPOList.ItemsSource = purchaseOrderList;
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;
                IsPageLoded = true;
                FillSupplier();
            }
        }

        private void FillStore()
        {
            try
            {
                clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
                long clinicId = User.UserLoginInfo.UnitId;
                clsGetMasterListBizActionVO BizAction1 = new clsGetMasterListBizActionVO();
                BizAction1.MasterTable = MasterTableNameList.M_Store;
                BizAction1.MasterList = new List<MasterListItem>();

                if (!((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                {
                    if (clinicId > 0)
                    {
                        BizAction1.Parent = new KeyValue { Value = "ClinicID", Key = clinicId.ToString() };
                    }
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = objList;
                        cmbStore.SelectedItem = objList[0];
                    }
                };
                Client.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
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
                        FillPurchaseOrderDataGrid();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }

        private void FillPurchaseOrderDataGrid()
        {
            try
            {
                indicator.Show();
                clsGetPurchaseOrderForCloseBizActionVO objBizActionVO = new clsGetPurchaseOrderForCloseBizActionVO();
                if (dtpFromDate.SelectedDate != null)
                    objBizActionVO.searchFromDate = dtpFromDate.SelectedDate.Value;
                if (dtpToDate.SelectedDate != null)
                    objBizActionVO.searchToDate = dtpToDate.SelectedDate.Value;

                objBizActionVO.SearchStoreID = cmbStore.SelectedItem == null ? 0 : ((MasterListItem)cmbStore.SelectedItem).ID;

                if (cmbSupplier.SelectedItem != null)
                {
                    objBizActionVO.SearchSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                }

                objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                objBizActionVO.PagingEnabled = true;
                objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.MaximumRows = DataList.PageSize;
                objBizActionVO.PONO = txtPONO.Text;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderForCloseBizActionVO objPurchaseOrderList = ((clsGetPurchaseOrderForCloseBizActionVO)arg.Result);

                            dgPOList.ItemsSource = null;
                            if (objPurchaseOrderList.PurchaseOrderList != null && objPurchaseOrderList.PurchaseOrderList.Count > 0)
                            {
                                DataList.Clear();
                                DataList.TotalItemCount = objPurchaseOrderList.TotalRowCount;
                                foreach (var item in objPurchaseOrderList.PurchaseOrderList)
                                {
                                    DataList.Add(item);
                                }
                                dgPOList.ItemsSource = null;
                                dgPOList.ItemsSource = DataList;
                                grdpo.ItemsSource = null;
                                purchaseOrderList.Clear();
                                datapager.Source = DataList;
                                grdpo.ItemsSource = null;
                            }
                            indicator.Close();
                        }
                        else
                            indicator.Close();
                    }
                    else
                    {
                        System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding .");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dgPOList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdpo.ItemsSource = null;
            if (dgPOList.SelectedIndex != -1)
            {
                cmdClose.Visibility = Visibility.Visible;
                cmdPrint.Visibility = Visibility.Visible;
                cmdCancel.Visibility= Visibility.Visible;
                try
                {
                    clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                    clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                    if (objList.IsApproveded == true)
                        cmdClose.IsEnabled = false;
                    else
                        cmdClose.IsEnabled = true;

                    objBizActionVO.SearchID = objList.ID;
                    objBizActionVO.UnitID = objList.UnitId;
                    objBizActionVO.IsPagingEnabled = true;
                    objBizActionVO.StartIndex = 0;
                    objBizActionVO.MinRows = 20;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);
                                grdpo.ItemsSource = null;
                                grdpo.ItemsSource = obj.PurchaseOrderList;
                                foreach (var item in obj.PurchaseOrderList)
                                {
                                    PurchaseOrderItems.Add(item);
                                }
                                foreach (var item2 in obj.PoIndentList)
                                {
                                    PoIndentDetails.Add(item2);
                                }
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                cmdClose.Visibility = Visibility.Collapsed;
                cmdPrint.Visibility = Visibility.Collapsed;
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ClosePurchaseOrderManually();
            }
        }

        private void FillPODetails(long POID)
        {
            try
            {
                clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;

                objBizActionVO.SearchID = POID;
                objBizActionVO.UnitID = objList.UnitId;
                objBizActionVO.IsPagingEnabled = true;
                objBizActionVO.StartIndex = 0;
                objBizActionVO.MinRows = 20;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);

                            List<clsPurchaseOrderDetailVO> objListOfItems = (List<clsPurchaseOrderDetailVO>)obj.PurchaseOrderList;
                            if (objListOfItems != null)
                            {
                                foreach (var item in objListOfItems)
                                {
                                    PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO
                                    {
                                        ItemID = item.ItemID,
                                        ItemName = item.ItemName,
                                        ItemCode = item.ItemCode,
                                        Quantity = (decimal)item.Quantity,
                                        Rate = (decimal)item.Rate,
                                        Amount = (decimal)item.Amount,
                                        MRP = (decimal)item.MRP,
                                        DiscountPercent = (decimal)item.DiscountPercent,
                                        DiscountAmount = (decimal)item.DiscountAmount,
                                        VATPercent = (decimal)item.VATPercent,
                                        VATAmount = (decimal)item.VATAmount,
                                        NetAmount = (decimal)item.NetAmount,
                                        Specification = item.Specification,
                                        PUM = item.PUM
                                    });
                                };
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                };

                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            long POId = 0;
            long PUnitID = 0;
            clsPurchaseOrderVO obj = (clsPurchaseOrderVO)dgPOList.SelectedItem;
            if (obj != null)
            {
                this.DataContext = obj;
                POId = obj.ID;
                PUnitID = obj.UnitId;
                string URL = "../Reports/InventoryPharmacy/PurchaseOrderPrint.aspx?POId=" + POId + "&PUnitID=" + PUnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Please Select a PO to Print.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }

        }

        private void ClosePO_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                msgText = "Are you sure you want to Close the PO";

                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        private void cmdClosePO_Click(object sender, RoutedEventArgs e)
        {
            ClosePurchaseOrderManually();
        }

        private void ClosePurchaseOrderManually()
        {
            clsClosePurchaseOrderManuallyBizActionVO bizactionVO = new clsClosePurchaseOrderManuallyBizActionVO();
            bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();

            bizactionVO.PurchaseOrder.ID = (((clsPurchaseOrderVO)dgPOList.SelectedItem)).ID;
            bizactionVO.PurchaseOrder.UnitId = (((clsPurchaseOrderVO)dgPOList.SelectedItem)).UnitId;
            bizactionVO.PurchaseOrder.Remarks = "Manually Closed.";
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            try
            {
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        msgText = "Purchase Order Closed successfully .";
                        if (dgPOList.Columns[8].IsReadOnly)
                        {
                            dgPOList.Columns[8].IsReadOnly = true;
                        }
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                        if (cmbStore.ItemsSource != null && cmbSupplier.ItemsSource != null)
                        {
                            cmbStore.SelectedItem = ((List<MasterListItem>)cmbStore.ItemsSource)[0];
                            cmbSupplier.SelectedItem = ((List<MasterListItem>)cmbSupplier.ItemsSource)[0];
                        }
                        FillPurchaseOrderDataGrid();
                    }

                };
                client.ProcessAsync(bizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtPONO_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillPurchaseOrderDataGrid();
            }
            else if (e.Key == Key.Tab)
            {
                FillPurchaseOrderDataGrid();
            }
        }

        

        private void cmbStore_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStore.SelectedItem != null)
            {
                FillPurchaseOrderDataGrid();
            }
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSupplier.SelectedItem != null)
            {
                FillPurchaseOrderDataGrid();
            }
        }
    }
}


