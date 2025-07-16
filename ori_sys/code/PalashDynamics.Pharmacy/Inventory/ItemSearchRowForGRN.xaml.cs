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
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pharmacy.ItemSearch
{
    public partial class ItemSearchRowForGRN : UserControl
    {
        #region Properties

        WaitIndicator indicator = new WaitIndicator();
        public bool IsFromClearControl { get; set; }
        public MasterListItem SelectedItem { get; set; }
        public List<MasterListItem> DataList { get; private set; }
        public List<clsItemStockVO> BatchList { get; private set; }
        bool IsFromCmbItemCodeEnter { get; set; }
        bool IsFromCmbItemNameEnter { get; set; }
        private List<clsItemTaxVO> ItemTaxDetailList { get; set; }
        public event RoutedEventHandler OnQuantityEnter_Click;
        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set
            {
                if (_StoreID != value)
                {
                    _StoreID = value;
                }
            }
        }

        private long _SupplierID;
        public long SupplierID
        {
            get { return _SupplierID; }
            set
            {
                if (_SupplierID != value)
                {
                    _SupplierID = value;
                }
            }
        }

        private ObservableCollection<clsGRNDetailsVO> _ItemSelected;
        public ObservableCollection<clsGRNDetailsVO> SelectedItems { get { return _ItemSelected; } }

        #endregion
        //long StoreID , long SupplierID
        public ItemSearchRowForGRN(long StoreID, long SupplierID)
        {
            InitializeComponent();
            this.StoreID = StoreID;
            this.SupplierID = SupplierID;
            this.Loaded += new RoutedEventHandler(ItemSearchRowForGRN_Loaded);
        }

        public ItemSearchRowForGRN()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ItemSearchRowForGRN_Loaded);
        }
        clsItemStockVO SelectedBatchItem = new clsItemStockVO();
        void ItemSearchRowForGRN_Loaded(object sender, RoutedEventArgs e)
        {
            ItemTaxDetailList = new List<clsItemTaxVO>();
            DataList = new List<MasterListItem>();
            BatchList = new List<clsItemStockVO>();
            SelectedBatchItem = new clsItemStockVO();
            //dtpExpDate.SelectedDate = DateTime.Now;
            if (StoreID > 0 && SupplierID > 0)
            {
                GetData();
            }
            dtpExpDate.BlackoutDates.Add(new CalendarDateRange(new DateTime(2001, 1, 1), DateTime.Now.AddDays(-1)));
            cmbItemName.Focus();
        }

        public void GetData()
        {
            indicator.Show();
            clsGetAllItemListBizActionVO BizActionObject = new clsGetAllItemListBizActionVO();
            BizActionObject.ItemList = new List<clsItemMasterVO>();
            BizActionObject.MasterList = new List<MasterListItem>();
            BizActionObject.StoreID = this.StoreID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetAllItemListBizActionVO result = ea.Result as clsGetAllItemListBizActionVO;
                    DataList.Clear();
                    DataList.AddRange(result.MasterList);

                    cmbItemCode.ItemsSource = null;
                    cmbItemCode.ItemsSource = result.MasterList;
                    cmbItemName.ItemsSource = null;
                    cmbItemName.ItemsSource = result.MasterList;
                    cmbItemName.Focus();


                    indicator.Close();
                    //GetItemTaxDetail(indicator);

                }
                else
                {
                    indicator.Close();
                }
            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

            #region Commented
            //clsGetSearchMasterListBizActionVO BizAction = new clsGetSearchMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_DiagosisMaster;
            //BizAction.MasterList = new List<MasterListItem>();

            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //Client.ProcessCompleted += (s, e) =>
            //{
            //    List<MasterListItem> objList = new List<MasterListItem>();
            //    List<MasterListItem> objListCode = new List<MasterListItem>();
            //    //objList.Add(new MasterListItem(0, "- Select -"));
            //    if (e.Error == null && e.Result != null)
            //    {
            //        objList.AddRange(((clsGetSearchMasterListBizActionVO)e.Result).MasterList);
            //    }
            //    foreach (MasterListItem item in objList)
            //    {
            //        objListCode.Add(new MasterListItem() { ID = item.ID, Description = item.Code, Code = item.Description });
            //    }
            //    cmbItemCode.ItemsSource = null;
            //    cmbItemCode.ItemsSource = objListCode;
            //    //cmbDiagnosisCode.SelectedItem = objListCode[0];

            //    cmbItemName.ItemsSource = null;
            //    cmbItemName.ItemsSource = objList;
            //    // cmbDiagnosisDiscription.SelectedItem = objList[0];
            //};

            //Client.ProcessAsync(BizAction, loggedinUser);
            //Client.CloseAsync();

            #endregion
        }

        private void GetItemTaxDetail(WaitIndicator indicator)
        {
            clsGetAllItemTaxDetailBizActionVO BizAction = new clsGetAllItemTaxDetailBizActionVO();
            BizAction.ItemTaxList = new List<clsItemTaxVO>();
            BizAction.ApplicableFor = 1;
            BizAction.StoreID = this.StoreID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetAllItemTaxDetailBizActionVO result = ea.Result as clsGetAllItemTaxDetailBizActionVO;

                    if (ItemTaxDetailList == null)
                        ItemTaxDetailList = new List<clsItemTaxVO>();
                    if (result.ItemTaxList != null)
                    {
                        ItemTaxDetailList = result.ItemTaxList;
                    }
                    indicator.Close();
                }
                else
                {
                    indicator.Close();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        public void GetBatches()
        {
            clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = 100;
            BizAction.StartIndex = 0;
            BizAction.StoreID = StoreID;
            BizAction.ItemID = SelectedItem.ID;
            BizAction.ShowExpiredBatches = false;
            BizAction.ShowZeroStockBatches = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;
                    BatchList.Clear();
                    if (result.BatchList.Count > 0)
                    {
                        foreach (clsItemStockVO item in result.BatchList)
                        {
                            BatchList.Add(item);
                        }

                        cmbItemBatch.ItemsSource = null;
                        if (BatchList.Count > 0)
                        {
                            cmbItemBatch.ItemsSource = BatchList;
                            IsFromBatchSelection = false;
                            frmMultiBatchChildWinForGRN _NewBatchDisplayControl = new frmMultiBatchChildWinForGRN(BatchList);
                            _NewBatchDisplayControl.OnBatchSelection += new RoutedEventHandler(_NewBatchDisplayControl_OnBatchSelection);
                            _NewBatchDisplayControl.Unloaded += new RoutedEventHandler(_NewBatchDisplayControl_Unloaded);
                            _NewBatchDisplayControl.Show();
                        }
                        else
                        {
                            txtQuantity.Focus();
                        }
                    }
                    else
                    {
                        cmbItemBatch.Focus();
                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        void _NewBatchDisplayControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (!IsFromBatchSelection)
                cmbItemCode.Focus();
        }

        bool IsFromBatchSelection = false;
        void _NewBatchDisplayControl_OnBatchSelection(object sender, RoutedEventArgs e)
        {
            if (BatchList != null)
            {
                frmMultiBatchChildWinForGRN _BatchControl = (frmMultiBatchChildWinForGRN)sender;
                if (_BatchControl.SelectedBatch != null)
                {
                    if (_BatchControl.SelectedBatch.BatchCode.Equals("New Batch"))
                    {
                        cmbItemBatch.SelectedItem = null;
                        cmbItemBatch.Focus();
                    }
                    else if (_BatchControl.SelectedBatch.BatchCode != null)
                    {
                        if (BatchList.Where(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode).Count() > 1)
                        {
                            cmbItemBatch.SelectedItem = BatchList.FirstOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                            BindSelectedBatchToControl();
                        }
                        else
                        {
                            cmbItemBatch.SelectedItem = BatchList.SingleOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                            BindSelectedBatchToControl();
                        }
                    }
                }
                _BatchControl.Close();
                IsFromBatchSelection = true;
                if (cmbItemBatch.SelectedItem != null)
                    txtQuantity.Focus();
            }
        }

        private void BindSelectedBatchToControl()
        {
            if (cmbItemBatch.SelectedItem != null)
            {
                clsItemStockVO SelectedItem = (clsItemStockVO)cmbItemBatch.SelectedItem;

                SelectedBatchItem = SelectedItem;
                dtpExpDate.SelectedDate = Convert.ToDateTime(SelectedItem.ExpiryDate);
                txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                dtpExpDate.IsEnabled = txtMRP.IsEnabled = false;
                txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
            }
        }

        private void cmbItemCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        bool IsFromItemCodeKeyDown = false;
        private void cmbItemCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (StoreID > 0)
            {
                if (SupplierID > 0)
                {
                    if (e.Key.Equals(Key.Enter))
                    {
                        IsFromItemCodeKeyDown = true;
                        GetfromItemCodeSelectedItem();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select supplier.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    cmbItemCode.Text = "";
                    cmbItemCode.Focus();
                    cmbItemName.Text = "";
                    cmbItemName.Focus();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbItemCode.Text = "";
                cmbItemCode.Focus();
                cmbItemName.Text = "";
                cmbItemName.Focus();
            }
        }

        public void SetFocus()
        {
            fromSetFocus = true;
            IsFromClearControl = false;
            clearControl();
            cmbItemName.Focus();
        }

        private void GetfromItemCodeSelectedItem()
        {
            if (!IsFromClearControl)
            {
                if (DataList != null)
                {
                    if (DataList.Count > 0)
                    {
                        if (cmbItemCode.SelectedItem != null)
                        {
                            SelectedItem = ((MasterListItem)cmbItemCode.SelectedItem);
                            cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                            IsFromCmbItemCodeEnter = true;
                            if (SelectedItem.isChecked)
                            {
                                GetBatches();
                            }
                            else
                            {
                                txtMRP.Focus();
                                txtMRP.SelectAll();
                            }
                        }
                        else if (!string.IsNullOrEmpty(cmbItemCode.Text))
                        {
                            if (DataList.SingleOrDefault(S => S.Code.ToUpper().Equals(cmbItemCode.Text.ToUpper())) != null)
                            {
                                cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.ToUpper().Equals(cmbItemCode.Text.ToUpper()));
                                SelectedItem = ((MasterListItem)cmbItemCode.SelectedItem);
                                cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                                IsFromCmbItemCodeEnter = true;
                                if (SelectedItem.isChecked)
                                {
                                    GetBatches();
                                }
                                else
                                {
                                    txtMRP.Focus();
                                    txtMRP.SelectAll();
                                }
                            }
                        }
                        else
                        {
                            cmbItemName.Focus();
                        }
                    }
                }
                else
                {
                    cmbItemName.Focus();
                }
            }
            else
            {
                IsFromClearControl = false;
                cmbItemCode.Text = string.Empty;
            }
        }

        bool IsFromItemNameKeyDown = false;
        private void cmbItemName_KeyUp(object sender, KeyEventArgs e)
        {
            if (StoreID > 0)
            {
                if (SupplierID > 0)
                {
                    if (e.Key.Equals(Key.Enter))
                    {
                        IsFromItemNameKeyDown = true;
                        GetfromItemNameSelectedItem();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select supplier.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    cmbItemCode.Text = "";
                    cmbItemCode.Focus();
                    cmbItemName.Text = "";
                    cmbItemName.Focus();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                cmbItemCode.Text = "";
                cmbItemCode.Focus();
                cmbItemName.Text = "";
                cmbItemName.Focus();
            }
            if (cmbItemName.Text == "")
            {
                Clear();
            }
        }

        private void Clear()
        {
            cmbItemCode.Text = "";
            cmbItemBatch.Text = "";
            dtpExpDate.SelectedDate = null;
            txtMRP.Text = "";
            txtPurchaseRate.Text = "";
            txtQuantity.Text = "";
            txtFreeQuantity.Text = "";
        }

        private void GetfromItemNameSelectedItem()
        {
            if (DataList != null)
            {
                if (DataList.Count > 0)
                {
                    if (cmbItemName.SelectedItem != null)
                    {
                        SelectedItem = ((MasterListItem)cmbItemName.SelectedItem);
                        cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                        IsFromCmbItemNameEnter = true;
                        if (SelectedItem.isChecked)
                        {
                            GetBatches();
                        }
                        else
                        {
                            SelectedBatchItem = new clsItemStockVO();
                            txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                            txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                            txtMRP.Focus();
                            txtMRP.SelectAll();
                        }
                    }
                    else if (!string.IsNullOrEmpty(cmbItemName.Text))
                    {
                        if (DataList.SingleOrDefault(S => S.Description.ToUpper().Equals(cmbItemName.Text.ToUpper())) != null)
                        {
                            cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Description.ToUpper().Equals(cmbItemName.Text.ToUpper()));
                            SelectedItem = ((MasterListItem)cmbItemName.SelectedItem);
                            cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                            IsFromCmbItemNameEnter = true;
                            if (SelectedItem.isChecked)
                            {
                                GetBatches();
                            }
                            else
                            {
                                txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                                txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                                txtMRP.Focus();
                                txtMRP.SelectAll();
                            }
                        }
                    }
                    else
                    {
                        cmbItemName.Focus();
                    }
                }
            }
            else
            {
                cmbItemName.Focus();
            }
        }

        private void cmbItemBatch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                ////dtpExpDate.Focus();
                //GetfromItemNameSelectedItem();

                if (cmbItemBatch.SelectedItem != null)
                {
                    clsItemStockVO SelectedItem = new clsItemStockVO();
                    SelectedItem = (clsItemStockVO)cmbItemBatch.SelectedItem;
                    SelectedBatchItem = SelectedItem;
                    dtpExpDate.SelectedDate = Convert.ToDateTime(SelectedItem.ExpiryDate);
                    txtMRP.Text = Convert.ToString(SelectedItem.MRP);

                    dtpExpDate.IsEnabled = txtMRP.IsEnabled = false;
                    txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                    // txtQuantity.Focus();

                }
                else
                {
                    SelectedBatchItem = new clsItemStockVO();
                }
                dtpExpDate.Focus();
            }
        }

        private void cmbItemBatch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dtpExpDate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                txtMRP.Focus();
                txtMRP.SelectAll();
            }
        }

        private void txtMRP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleNumber(sender, e);
            }

            if (e.Key.Equals(Key.Enter))
            {
                if (!string.IsNullOrEmpty(txtMRP.Text))
                {
                    txtPurchaseRate.Focus();
                    txtPurchaseRate.SelectAll();
                }
            }
        }

        private void txtPurchaseRate_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleNumber(sender, e);
            }
            if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Tab))
            {
                if (!string.IsNullOrEmpty(txtPurchaseRate.Text))
                {
                    double PurchaseAmount = Convert.ToDouble(txtPurchaseRate.Text);
                    double MRP = Convert.ToDouble(txtMRP.Text);

                    if (MRP >= PurchaseAmount)
                    {
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow PurchaseRateValidationmsgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Purchase amount should be less than MRP. ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        PurchaseRateValidationmsgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(PurchaseRateValidationmsgW_OnMessageBoxClosed);
                        PurchaseRateValidationmsgW.Show();

                    }
                }
            }
        }

        void PurchaseRateValidationmsgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            txtPurchaseRate.Focus();
        }

        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleNumber(sender, e);
            }

            if (e.Key.Equals(Key.Enter))
            {
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    txtFreeQuantity.Focus();
                    txtFreeQuantity.SelectAll();
                }
            }
        }

        private void txtFreeQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleNumber(sender, e);
            }

            if (e.Key.Equals(Key.Enter))
            {
                if (cmbItemCode.SelectedItem != null)
                {
                    if (OnQuantityEnter_Click != null)
                    {
                        _ItemSelected = new ObservableCollection<clsGRNDetailsVO>();
                        if (((MasterListItem)cmbItemName.SelectedItem).isChecked == true && cmbItemBatch.Text == "" && dtpExpDate.SelectedDate == null)
                        {

                            string strMsg = "Select batch and expiry for the item.";
                            MessageBoxControl.MessageBoxChildWindow msgWinUnload =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWinUnload.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinMonth_OnMessageBoxClosed);
                            msgWinUnload.Show();
                            cmbItemName.Focus();
                            cmbItemName.UpdateLayout();

                        }
                        else
                        {


                            indicator.Show();
                            clsGetItemListForSearchBizActionVO BizActionObject = new clsGetItemListForSearchBizActionVO();
                            BizActionObject.ItemList = new List<clsItemMasterVO>();
                            BizActionObject.ShowScrapItems = false;
                            BizActionObject.StoreID = this.StoreID;
                            BizActionObject.ItemCode = ((MasterListItem)cmbItemCode.SelectedItem).Code;
                            BizActionObject.ItemName = ((MasterListItem)cmbItemCode.SelectedItem).Description;
                            BizActionObject.MaximumRows = 15;
                            BizActionObject.StartIndex = 0;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, ea) =>
                            {
                                if (ea.Result != null && ea.Error == null)
                                {
                                    clsGetItemListForSearchBizActionVO result = ea.Result as clsGetItemListForSearchBizActionVO;

                                    if (result.ItemList != null)
                                    {
                                        if (result.ItemList.Count.Equals(1))
                                        {
                                            clsItemMasterVO item = result.ItemList[0];

                                            clsGRNDetailsVO grnObj = new clsGRNDetailsVO();
                                            grnObj.IsBatchRequired = item.BatchesRequired;
                                            grnObj.ItemID = item.ID;
                                            grnObj.ItemTax = item.TotalPerchaseTaxPercent;
                                            grnObj.ItemName = item.ItemName;
                                            grnObj.ItemCode = item.ItemCode;
                                            grnObj.PurchaseUOM = item.PurchaseUOM;
                                            grnObj.StockUOM = item.StockUOM;
                                            //grnObj.VATPercent = Convert.ToDouble(item.VatPer);

                                            if (((MasterListItem)cmbItemCode.SelectedItem).isChecked)
                                            {
                                                //if (cmbItemBatch.SelectedItem != null)
                                                //{
                                                //    grnObj.BatchCode = ((MasterListItem)cmbItemBatch.SelectedItem).Description;
                                                //}
                                                //else
                                                //{
                                                grnObj.BatchID = SelectedBatchItem.BatchID;
                                                grnObj.BatchCode = cmbItemBatch.Text;

                                                grnObj.AvailableQuantity = SelectedBatchItem.AvailableStock;

                                                //}
                                                if (dtpExpDate.SelectedDate != null)
                                                    grnObj.ExpiryDate = Convert.ToDateTime(dtpExpDate.SelectedDate);
                                            }
                                            if (item.BatchesRequired == false)
                                            {
                                                grnObj.BarCode = ((MasterListItem)cmbItemName.SelectedItem).NonBatchItemBarcode;
                                            }
                                            else
                                            {
                                                if (!String.IsNullOrEmpty(SelectedBatchItem.BarCode))
                                                {
                                                    grnObj.BarCode = SelectedBatchItem.BarCode;
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(txtQuantity.Text))
                                                grnObj.Quantity = Convert.ToDouble(txtQuantity.Text);
                                            if (!string.IsNullOrEmpty(txtMRP.Text) || (Convert.ToDouble(txtMRP.Text) != 0.0))
                                                grnObj.MRP = Convert.ToDouble(txtMRP.Text);
                                            else
                                                grnObj.MRP = Convert.ToDouble(item.MRP);
                                            if (!string.IsNullOrEmpty(txtFreeQuantity.Text))
                                                grnObj.FreeQuantity = Convert.ToDouble(txtFreeQuantity.Text);
                                            if (!string.IsNullOrEmpty(txtPurchaseRate.Text) || (Convert.ToDouble(txtPurchaseRate.Text) != 0.0))
                                                grnObj.Rate = Convert.ToDouble(txtPurchaseRate.Text);
                                            else
                                                grnObj.Rate = Convert.ToDouble(item.PurchaseRate);

                                            grnObj.VATPercent = Convert.ToDouble(item.VatPer);
                                            #region Code commnted by pravin

                                            //if (this.ItemTaxDetailList != null)
                                            //{
                                            //    List<clsItemTaxVO> _List = this.ItemTaxDetailList.Where(S => S.ItemID.Equals(grnObj.ItemID)).ToList<clsItemTaxVO>();
                                            //    grnObj.GRNSaleTaxDetailsList = new List<clsGRNSaleTaxDetailsVO>();
                                            //    if (_List != null)
                                            //    {
                                            //        if (_List.Count > 0)
                                            //        {
                                            //            double TotalTax = 0, TaxPercentage = 0;
                                            //            foreach (clsItemTaxVO _item in _List)
                                            //            {
                                            //                double TaxAmount = 0;
                                            //                grnObj.VatApplicableOn = Convert.ToInt32(_item.ApplicableOn.ID);
                                            //                if (_item.ApplicableOn.ID.Equals(1))
                                            //                {
                                            //                    TaxAmount = (((grnObj.Amount - grnObj.CDiscountAmount - grnObj.SchDiscountAmount) * Convert.ToDouble(_item.Percentage)) / 100);
                                            //                    //TaxAmount = ((grnObj.Rate* grnObj.Quantity) * Convert.ToDouble(_item.Percentage)) / 100;
                                            //                    TotalTax += TaxAmount;
                                            //                }
                                            //                else if (_item.ApplicableOn.ID.Equals(2))
                                            //                {
                                            //                    TaxAmount = (((grnObj.FreeQuantity + grnObj.Quantity) * (grnObj.MRP / (1 + (Convert.ToDouble(_item.Percentage) / 100))))) * ((Convert.ToDouble(_item.Percentage)) / 100);
                                            //                    //TaxAmount = ((grnObj.MRP* grnObj.Quantity) * Convert.ToDouble(_item.Percentage)) / 100;
                                            //                    TotalTax += TaxAmount;
                                            //                }
                                            //                TaxPercentage += Convert.ToDouble(_item.Percentage);

                                            //                clsGRNSaleTaxDetailsVO _NewTDetails = new clsGRNSaleTaxDetailsVO();
                                            //                _NewTDetails.IsFromGRN = true;
                                            //                _NewTDetails.TaxPercentage = Convert.ToDouble(_item.Percentage);
                                            //                _NewTDetails.TaxID = _item.TaxID;
                                            //                _NewTDetails.TaxAmount = TaxAmount;

                                            //                grnObj.GRNSaleTaxDetailsList.Add(_NewTDetails);
                                            //            }
                                            //            grnObj.TaxPercent = TaxPercentage;
                                            //            //grnObj.TaxAmount = TotalTax;
                                            //        }
                                            //    }
                                            //}

                                            #endregion


                                            //////if (item.ItemTaxList != null)
                                            //////{
                                            //////    List<clsItemTaxVO> _List = item.ItemTaxList.Where(S => S.ItemID.Equals(grnObj.ItemID)).ToList<clsItemTaxVO>();
                                            //////    grnObj.GRNSaleTaxDetailsList = new List<clsGRNSaleTaxDetailsVO>();
                                            //////    if (_List != null)
                                            //////    {
                                            //////        if (_List.Count > 0)
                                            //////        {
                                            //////            double TotalTax = 0, TaxPercentage = 0;
                                            //////            foreach (clsItemTaxVO _item in _List)
                                            //////            {
                                            //////                double TaxAmount = 0;
                                            //////               // grnObj.VatApplicableOn = Convert.ToInt32(_item.ApplicableOn.ID);
                                            //////                if (_item.ApplicableOn.ID.Equals(1))
                                            //////                {
                                            //////                    TaxAmount = (((grnObj.Amount - grnObj.CDiscountAmount - grnObj.SchDiscountAmount) * Convert.ToDouble(_item.Percentage)) / 100);
                                            //////                    //TaxAmount = ((grnObj.Rate* grnObj.Quantity) * Convert.ToDouble(_item.Percentage)) / 100;
                                            //////                    TotalTax += TaxAmount;
                                            //////                }
                                            //////                else if (_item.ApplicableOn.ID.Equals(2))
                                            //////                {
                                            //////                    TaxAmount = (((grnObj.FreeQuantity + grnObj.Quantity) * (grnObj.MRP / (1 + (Convert.ToDouble(_item.Percentage) / 100))))) * ((Convert.ToDouble(_item.Percentage)) / 100);
                                            //////                    //TaxAmount = ((grnObj.MRP* grnObj.Quantity) * Convert.ToDouble(_item.Percentage)) / 100;
                                            //////                    TotalTax += TaxAmount;
                                            //////                }
                                            //////                TaxPercentage += Convert.ToDouble(_item.Percentage);

                                            //////                clsGRNSaleTaxDetailsVO _NewTDetails = new clsGRNSaleTaxDetailsVO();
                                            //////                _NewTDetails.IsFromGRN = true;
                                            //////                _NewTDetails.TaxPercentage = Convert.ToDouble(_item.Percentage);
                                            //////                _NewTDetails.TaxID = _item.TaxID;
                                            //////                _NewTDetails.TaxAmount = TaxAmount;
                                            //////                _NewTDetails.ApplicableOn.ID = _item.ApplicableOn.ID;
                                            //////                if (_NewTDetails.ApplicableOnList != null)
                                            //////                {
                                            //////                    _NewTDetails.ApplicableOn = _NewTDetails.ApplicableOnList.SingleOrDefault(S => S.ID.Equals(_NewTDetails.ApplicableOn.ID));
                                            //////                }
                                            //////                grnObj.GRNSaleTaxDetailsList.Add(_NewTDetails);

                                            //////                grnObj.ApplicableOn.ID = _item.ApplicableOn.ID;
                                            //////                if (grnObj.ApplicableOnList != null)
                                            //////                {
                                            //////                    grnObj.ApplicableOn = grnObj.ApplicableOnList.SingleOrDefault(S => S.ID.Equals(grnObj.ApplicableOn.ID));
                                            //////                }
                                            //////            }
                                            //////            grnObj.TaxPercent = TaxPercentage;
                                            //////            //grnObj.TaxAmount = TotalTax;
                                            //////        }
                                            //////    }
                                            //////}



                                            grnObj.ConversionFactor = Convert.ToDouble(item.ConversionFactor);
                                            //grnObj.VatApplicableOn = item.VatApplicableOn;
                                            if (item.AssignSupplier == false)
                                            {
                                                #region Remove msg as per requirement given by vishal mane
                                                //MessageBoxResult reply = MessageBox.Show("Do you want to assign supplier for the item:" + item.ItemName, "Supplier is not assigned to item", MessageBoxButton.OKCancel);

                                                //if (reply == MessageBoxResult.OK)
                                                //{
                                                //  grnObj.AssignSupplier = true;
                                                //}
                                                //else
                                                //    grnObj.AssignSupplier = false;

                                                #endregion
                                                grnObj.AssignSupplier = true;

                                            }
                                            else
                                                grnObj.AssignSupplier = item.AssignSupplier;

                                            _ItemSelected.Add(grnObj);

                                            cmbItemName.Text = null;
                                            cmbItemName.Focus();
                                            OnQuantityEnter_Click(this, new RoutedEventArgs());
                                            //....................................

                                            //....................................

                                            // clearControl();

                                        }
                                    }
                                }
                                indicator.Close();
                            };
                            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();

                        }
                    }
                }
                else
                {
                    cmbItemCode.Focus();
                }
            }
        }

        bool fromSetFocus = false;
        public void clearControl()
        {
            cmbItemCode.SelectedItem = null;
            cmbItemName.SelectedItem = null;
            cmbItemBatch.SelectedItem = null;
            cmbItemBatch.ItemsSource = null;

            txtMRP.Text = "0";
            txtPurchaseRate.Text = "0";
            txtFreeQuantity.Text = "0";
            txtQuantity.Text = "0";
            cmbItemCode.Text = "";
            cmbItemName.Text = "";
            cmbItemBatch.Text = "";
            dtpExpDate.SelectedDate = null;

            if (!fromSetFocus)
            {
                IsFromClearControl = true;
            }
            else
            {
                fromSetFocus = false;
            }

            //if (IsFromCmbItemCodeEnter)
            //{
            //    cmbItemCode.Focus();
            //}
            //else if (IsFromCmbItemNameEnter)
            //{


            //}
            //else
            //{
            //    cmbItemCode.Focus();
            //}

            dtpExpDate.IsEnabled = true;
            txtMRP.IsEnabled = true;

            IsFromCmbItemNameEnter = false;
            IsFromCmbItemCodeEnter = false;
            cmbItemName.Focus();
        }

        private void txtMRP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }

        private void txtPurchaseRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }

        private void txtFreeQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.PlatformKeyCode.Equals(109))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            }
        }

        private void dtpExpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            long month = 0;
            long years = 0;
            if (((DatePicker)sender).SelectedDate != null)
            {
                month = CIMS.DateTimeExtensions.TotalMonthDifference(DateTime.Now, (DateTime)((DatePicker)sender).SelectedDate);
                years = Convert.ToInt64(Math.Round(CIMS.DateTimeExtensions.TotalYearsDifference(DateTime.Now, (DateTime)((DatePicker)sender).SelectedDate), 0));
                // long NearExpiryPeriodForGRN=((IApplicationConfiguration)App.Current).ApplicationConfigurations.NearExpiryPeriodForGRN;
                //Commented By Somnath
                //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.NearExpiryPeriodForGRN >= month)
                //{
                //    string strMsg = "Item is nearer expiry or equal to expiry. Do you want to Continue?";
                //    MessageBoxControl.MessageBoxChildWindow msgWinUnload =
                //               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWinUnload.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinMonth_OnMessageBoxClosed);
                //    msgWinUnload.Show();
                //}
                //else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.HigherExpiryPeriodForGRN < years)
                //{
                //    string strMsg = "Item is greater expiry. Do you want to Continue?";
                //    MessageBoxControl.MessageBoxChildWindow msgWinUnload =
                //               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                //    msgWinUnload.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWinYear_OnMessageBoxClosed);
                //    msgWinUnload.Show();
                //}
            }
        }

        void msgWinMonth_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.No)
            {
                dtpExpDate.SelectedDate = null;
            }
        }

        void msgWinYear_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.No)
            {
                dtpExpDate.SelectedDate = null;
            }
        }
    }
}
