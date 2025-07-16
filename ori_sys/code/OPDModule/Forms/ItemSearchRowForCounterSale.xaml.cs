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

namespace OPDModule.Forms
{
    public partial class ItemSearchRowForCounterSale : UserControl
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

        private ObservableCollection<clsItemSalesDetailsVO> _ItemSelected;    // private ObservableCollection<clsGRNDetailsVO> _ItemSelected;
        public ObservableCollection<clsItemSalesDetailsVO> SelectedItems { get { return _ItemSelected; } }      // public ObservableCollection<clsGRNDetailsVO> SelectedItems { get { return _ItemSelected; } }

        #endregion

        //long StoreID , long SupplierID
        public ItemSearchRowForCounterSale(long StoreID, long SupplierID)
        {
            InitializeComponent();
            this.StoreID = StoreID;
            this.SupplierID = SupplierID;
            this.Loaded += new RoutedEventHandler(ItemSearchRowForCounterSale_Loaded);
        }

        public ItemSearchRowForCounterSale()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ItemSearchRowForCounterSale_Loaded);
        }

        clsItemStockVO SelectedBatchItem = new clsItemStockVO();
        //public clsItemMasterVO SelectedItemObj { get; set; }   //For Item Selection Control

        void ItemSearchRowForCounterSale_Loaded(object sender, RoutedEventArgs e)
        {
            ItemTaxDetailList = new List<clsItemTaxVO>();
            DataList = new List<MasterListItem>();
            BatchList = new List<clsItemStockVO>();
            SelectedBatchItem = new clsItemStockVO();
            //dtpExpDate.SelectedDate = DateTime.Now;
            if (StoreID > 0)      //if (StoreID > 0 && SupplierID > 0)
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
            BizAction.ShowZeroStockBatches = false;      //BizAction.ShowZeroStockBatches = true;

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
                            frmMultiBatchChildWinForCS _NewBatchDisplayControl = new frmMultiBatchChildWinForCS(BatchList);
                            _NewBatchDisplayControl.OnBatchSelection += new RoutedEventHandler(_NewBatchDisplayControl_OnBatchSelection);
                            _NewBatchDisplayControl.Unloaded += new RoutedEventHandler(_NewBatchDisplayControl_Unloaded);
                            _NewBatchDisplayControl.Show();
                        }
                        else
                        {
                            //txtQuantity.Focus();
                            txtFreeQuantity.Focus();
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
                frmMultiBatchChildWinForCS _BatchControl = (frmMultiBatchChildWinForCS)sender;
                if (_BatchControl.SelectedBatch != null)
                {
                    //if (_BatchControl.SelectedBatch.BatchCode.Equals("New Batch"))
                    //{
                    //    cmbItemBatch.SelectedItem = null;
                    //    //SelectedItemObj = null;   //For Item Selection Control
                    //    cmbItemBatch.Focus();
                    //}
                    //else 

                    clsItemStockVO objBatchItem = new clsItemStockVO();

                    if (_BatchControl.SelectedBatch.SelectedItemObj != null && _BatchControl.SelectedBatch.SelectedItemObj.BatchesRequired == true && _BatchControl.SelectedBatch.BatchCode != null)
                    {


                        //if (BatchList.Where(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode).Count() > 1)
                        if (BatchList.Where(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode && S.ExpiryDate == _BatchControl.SelectedBatch.ExpiryDate && S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP).Count() > 1)
                        {
                            //cmbItemBatch.SelectedItem = BatchList.FirstOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                            //cmbItemBatch.SelectedItem = BatchList.FirstOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode && S.ExpiryDate == _BatchControl.SelectedBatch.ExpiryDate && S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem = BatchList.FirstOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode && S.ExpiryDate == _BatchControl.SelectedBatch.ExpiryDate && S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem.SelectedItemObj = _BatchControl.SelectedBatch.SelectedItemObj;
                            cmbItemBatch.SelectedItem = objBatchItem;
                            //SelectedItemObj = _BatchControl.SelectedItemObj;   //For Item Selection Control
                            BindSelectedBatchToControl();
                        }
                        else
                        {
                            //cmbItemBatch.SelectedItem = BatchList.SingleOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                            //cmbItemBatch.SelectedItem = BatchList.SingleOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode && S.ExpiryDate == _BatchControl.SelectedBatch.ExpiryDate && S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem = BatchList.SingleOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode && S.ExpiryDate == _BatchControl.SelectedBatch.ExpiryDate && S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem.SelectedItemObj = _BatchControl.SelectedBatch.SelectedItemObj;
                            cmbItemBatch.SelectedItem = objBatchItem;
                            //SelectedItemObj = _BatchControl.SelectedItemObj;   //For Item Selection Control
                            BindSelectedBatchToControl();
                        }
                    }
                    else if (_BatchControl.SelectedBatch.SelectedItemObj != null && _BatchControl.SelectedBatch.SelectedItemObj.BatchesRequired == false && _BatchControl.SelectedBatch.PurchaseRate != null && _BatchControl.SelectedBatch.MRP != null)
                    {
                        //if (BatchList.Where(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode).Count() > 1)
                        if (BatchList.Where(S => S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP).Count() > 1)
                        {
                            //cmbItemBatch.SelectedItem = BatchList.FirstOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                            //cmbItemBatch.SelectedItem = BatchList.FirstOrDefault(S => S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem = BatchList.FirstOrDefault(S => S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem.SelectedItemObj = _BatchControl.SelectedBatch.SelectedItemObj;
                            cmbItemBatch.SelectedItem = objBatchItem;
                            //SelectedItemObj = _BatchControl.SelectedItemObj;   //For Item Selection Control
                            BindSelectedBatchToControl();
                        }
                        else
                        {
                            //cmbItemBatch.SelectedItem = BatchList.SingleOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                            //cmbItemBatch.SelectedItem = BatchList.SingleOrDefault(S => S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem = BatchList.SingleOrDefault(S => S.PurchaseRate == _BatchControl.SelectedBatch.PurchaseRate && S.MRP == _BatchControl.SelectedBatch.MRP && S.IsFreeItem == _BatchControl.SelectedBatch.IsFreeItem);
                            objBatchItem.SelectedItemObj = _BatchControl.SelectedBatch.SelectedItemObj;
                            cmbItemBatch.SelectedItem = objBatchItem;
                            //SelectedItemObj = _BatchControl.SelectedItemObj;   //For Item Selection Control
                            BindSelectedBatchToControl();
                        }
                    }
                }
                else
                {
                    cmbItemBatch.SelectedItem = null;
                    //SelectedItemObj = null;   //For Item Selection Control
                }

                _BatchControl.Close();
                IsFromBatchSelection = true;
                if (cmbItemBatch.SelectedItem != null)
                    txtFreeQuantity.Focus();        //txtQuantity.Focus();
            }
        }

        private void BindSelectedBatchToControl()
        {
            if (cmbItemBatch.SelectedItem != null)
            {
                clsItemStockVO SelectedItem = (clsItemStockVO)cmbItemBatch.SelectedItem;

                SelectedBatchItem = SelectedItem;
                if (SelectedItem.ExpiryDate != null)
                    dtpExpDate.SelectedDate = Convert.ToDateTime(SelectedItem.ExpiryDate);
                txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                //dtpExpDate.IsEnabled = txtMRP.IsEnabled = false;
                txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);

                if (SelectedItem.SelectedItemObj != null)
                    txtSellingUom.Text = SelectedItem.SelectedItemObj.SellingUMString;

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
                //if (SupplierID > 0)
                //{

                if (e.Key.Equals(Key.Enter))
                {
                    if (cmbItemCode.SelectedItem != null && ((MasterListItem)cmbItemCode.SelectedItem).IsItemBlock == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Suspended item can not be selected.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            cmbItemName.Text = "";
                            //cmbItemName.Focus();
                            cmbItemCode.Text = "";
                            cmbItemCode.Focus();
                        };
                    }
                    else
                    {
                        IsFromItemCodeKeyDown = true;
                        GetfromItemCodeSelectedItem();
                    }
                }


                //}
                //else
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select supplier.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();
                //    cmbItemCode.Text = "";
                //    cmbItemCode.Focus();
                //    cmbItemName.Text = "";
                //    cmbItemName.Focus();
                //}
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
                            //if (SelectedItem.isChecked)
                            //{
                            GetBatches();
                            //}
                            //else
                            //{
                            //    txtMRP.Focus();
                            //    txtMRP.SelectAll();
                            //}
                        }
                        else if (!string.IsNullOrEmpty(cmbItemCode.Text))
                        {
                            if (DataList.SingleOrDefault(S => S.Code.ToUpper().Equals(cmbItemCode.Text.ToUpper())) != null)
                            {
                                cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.ToUpper().Equals(cmbItemCode.Text.ToUpper()));
                                SelectedItem = ((MasterListItem)cmbItemCode.SelectedItem);
                                cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                                IsFromCmbItemCodeEnter = true;
                                //if (SelectedItem.isChecked)
                                //{
                                GetBatches();
                                //}
                                //else
                                //{
                                //    txtMRP.Focus();
                                //    txtMRP.SelectAll();
                                //}
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
                //if (SupplierID > 0)
                //{


                if (e.Key.Equals(Key.Enter))
                {
                    if (cmbItemName.SelectedItem != null && ((MasterListItem)cmbItemName.SelectedItem).IsItemBlock == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Suspended item can not be selected.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            cmbItemCode.Text = "";
                            //cmbItemCode.Focus();
                            cmbItemName.Text = "";
                            cmbItemName.Focus();
                        };
                    }
                    else
                    {
                        IsFromItemNameKeyDown = true;
                        GetfromItemNameSelectedItem();
                    }
                }


                //}
                //else
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select supplier.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();
                //    cmbItemCode.Text = "";
                //    cmbItemCode.Focus();
                //    cmbItemName.Text = "";
                //    cmbItemName.Focus();
                //}
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
            txtSellingUom.Text = "";
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
                        //if (SelectedItem.isChecked)
                        //{
                        GetBatches();
                        //}
                        //else
                        //{
                        //    SelectedBatchItem = new clsItemStockVO();
                        //    txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                        //    txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                        //    txtMRP.Focus();
                        //    txtMRP.SelectAll();
                        //}
                    }
                    else if (!string.IsNullOrEmpty(cmbItemName.Text))
                    {
                        if (DataList.SingleOrDefault(S => S.Description.ToUpper().Equals(cmbItemName.Text.ToUpper())) != null)
                        {
                            cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Description.ToUpper().Equals(cmbItemName.Text.ToUpper()));
                            SelectedItem = ((MasterListItem)cmbItemName.SelectedItem);
                            cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                            IsFromCmbItemNameEnter = true;
                            //if (SelectedItem.isChecked)
                            //{
                            GetBatches();
                            //}
                            //else
                            //{
                            //    txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                            //    txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                            //    txtMRP.Focus();
                            //    txtMRP.SelectAll();
                            //}
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
                    if (SelectedItem.ExpiryDate != null)
                        dtpExpDate.SelectedDate = Convert.ToDateTime(SelectedItem.ExpiryDate);
                    txtMRP.Text = Convert.ToString(SelectedItem.MRP);

                    //dtpExpDate.IsEnabled = txtMRP.IsEnabled = false;
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

        double vatper;

        private void txtFreeQuantity_KeyUp(object sender, KeyEventArgs e)
        {


            if (e.PlatformKeyCode.Equals(109) || e.PlatformKeyCode.Equals(110) || e.PlatformKeyCode.Equals(190))
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
                        if (cmbItemBatch.SelectedItem != null && ChkValidationForQuantity((clsItemStockVO)cmbItemBatch.SelectedItem))      // ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj,
                        {

                            _ItemSelected = new ObservableCollection<clsItemSalesDetailsVO>();    //  _ItemSelected = new ObservableCollection<clsGRNDetailsVO>();
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


                                //indicator.Show();
                                //clsGetItemListForSearchBizActionVO BizActionObject = new clsGetItemListForSearchBizActionVO();
                                //BizActionObject.ItemList = new List<clsItemMasterVO>();
                                //BizActionObject.ShowScrapItems = false;
                                //BizActionObject.StoreID = this.StoreID;
                                //BizActionObject.ItemCode = ((MasterListItem)cmbItemCode.SelectedItem).Code;
                                //BizActionObject.ItemName = ((MasterListItem)cmbItemCode.SelectedItem).Description;
                                //BizActionObject.MaximumRows = 15;
                                //BizActionObject.StartIndex = 0;

                                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                //client.ProcessCompleted += (s, ea) =>
                                //{
                                //if (ea.Result != null && ea.Error == null)
                                //{
                                //    clsGetItemListForSearchBizActionVO result = ea.Result as clsGetItemListForSearchBizActionVO;

                                //    if (result.ItemList != null)
                                //    {
                                //        if (result.ItemList.Count.Equals(1))
                                //        {
                                //            clsItemMasterVO item = result.ItemList[0];

                                clsItemSalesDetailsVO ObjAddItem = new clsItemSalesDetailsVO();     //clsGRNDetailsVO grnObj = new clsGRNDetailsVO();

                                //................BEGIN....................

                                //cmbItemBatch.SelectedItem = BatchList.FirstOrDefault(S => S.BatchCode == _BatchControl.SelectedBatch.BatchCode);
                                //SelectedItemObj = _BatchControl.SelectedItemObj;   //For Item Selection Control

                                ObjAddItem.ItemCode = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.ItemCode; //_ItemSearchRowControl.SelectedItems[0].ItemCode;              //item.ItemCode;
                                ObjAddItem.ItemID = ((clsItemStockVO)cmbItemBatch.SelectedItem).ItemID;     //_ItemSearchRowControl.SelectedItems[0].ItemID;                  // item.ItemID;
                                ObjAddItem.ItemName = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.ItemName;  //_ItemSearchRowControl.SelectedItems[0].ItemName;              // item.ItemName;
                                ObjAddItem.Manufacture = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.Manufacturer;  //_ItemSearchRowControl.SelectedItems[0].Manufacture;        // item.Manufacturer;
                                ObjAddItem.PregnancyClass = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.PreganancyClass;  //_ItemSearchRowControl.SelectedItems[0].PregnancyClass;  // item.PreganancyClass;
                                ObjAddItem.BatchID = ((clsItemStockVO)cmbItemBatch.SelectedItem).BatchID;     //_ItemSearchRowControl.SelectedItems[0].BatchID;                // item.BatchID;
                                ObjAddItem.BatchCode = ((clsItemStockVO)cmbItemBatch.SelectedItem).BatchCode;     //_ItemSearchRowControl.SelectedItems[0].BatchCode;            // item.BatchCode;
                                ObjAddItem.ExpiryDate = ((clsItemStockVO)cmbItemBatch.SelectedItem).ExpiryDate;     //_ItemSearchRowControl.SelectedItems[0].ExpiryDate;          // item.ExpiryDate;

                                if (!string.IsNullOrEmpty(txtFreeQuantity.Text))
                                    ObjAddItem.Quantity = Convert.ToInt64(txtFreeQuantity.Text);

                                ObjAddItem.InclusiveOfTax = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.InclusiveOfTax; //_ItemSearchRowControl.SelectedItems[0].InclusiveOfTax;  // item.InclusiveOfTax;

                                #region Commented

                                //if (item.InclusiveOfTax == false)
                                //    ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                                //else
                                //    ObjAddItem.MRP = item.MRP;


                                //if (Itemswin.SelectedItems != null)                         // BY BHUSHAN . . . 
                                //{
                                //    if (Itemswin.SelectedItems[0].InclusiveOfTax == false)
                                //        ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                                //    else
                                //        ObjAddItem.MRP = item.MRP;
                                //}
                                //else
                                //{                                                                        // BY BHUSHAN . . . 
                                //    if (item.InclusiveOfTax == false)
                                //        ObjAddItem.MRP = (((item.MRP * item.TotalSalesTaxPercent) / 100) + item.MRP);
                                //    else
                                //        ObjAddItem.MRP = item.MRP;
                                //}

                                #endregion

                                ObjAddItem.OriginalMRP = ((clsItemStockVO)cmbItemBatch.SelectedItem).MRP;     //_ItemSearchRowControl.SelectedItems[0].MRP;                                // ObjAddItem.MRP;
                                ObjAddItem.AvailableQuantity = ((clsItemStockVO)cmbItemBatch.SelectedItem).AvailableStock;     //_ItemSearchRowControl.SelectedItems[0].AvailableStockInBase;         //item.AvailableStock;
                                ObjAddItem.PurchaseRate = ((clsItemStockVO)cmbItemBatch.SelectedItem).PurchaseRate;     //_ItemSearchRowControl.SelectedItems[0].PurchaseRate;                      // item.PurchaseRate;
                                ObjAddItem.ConcessionPercentage = ((clsItemStockVO)cmbItemBatch.SelectedItem).DiscountOnSale;     //_ItemSearchRowControl.SelectedItems[0].DiscountOnSale;            // item.DiscountOnSale;       // NA

                                ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;  //_ItemSearchRowControl.SelectedItems[0].ConcessionAmount;              // ObjAddItem.ConcessionAmount;
                                ObjAddItem.Amount = ObjAddItem.Amount;      //_ItemSearchRowControl.SelectedItems[0].Amount;                                  // ObjAddItem.Amount;
                                ObjAddItem.VATPercent = Convert.ToDouble(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SVatPer);  //_ItemSearchRowControl.SelectedItems[0].VATPerc;                             // item.VATPerc;      // NA
                                vatper = Convert.ToDouble(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SVatPer);  //_ItemSearchRowControl.SelectedItems[0].VATPerc;                                            // item.VATPerc;      // NA

                                //by Anjali.............................
                                ObjAddItem.MRP = ((clsItemStockVO)cmbItemBatch.SelectedItem).MRP;     //_ItemSearchRowControl.SelectedItems[0].MRP;                                    // item.MRP;
                                ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                                ObjAddItem.ItemVatType = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SItemVatType;        //_ItemSearchRowControl.SelectedItems[0].ItemVatType;                    // item.ItemVatType;
                                ObjAddItem.AvailableQuantity = ((clsItemStockVO)cmbItemBatch.SelectedItem).AvailableStock;     //_ItemSearchRowControl.SelectedItems[0].AvailableStockInBase;     // item.AvailableStock;
                                ObjAddItem.PurchaseRate = ((clsItemStockVO)cmbItemBatch.SelectedItem).PurchaseRate;     //_ItemSearchRowControl.SelectedItems[0].PurchaseRate;                  // item.PurchaseRate;
                                ObjAddItem.ConcessionPercentage = ((clsItemStockVO)cmbItemBatch.SelectedItem).DiscountOnSale;     //_ItemSearchRowControl.SelectedItems[0].DiscountOnSale;        // item.DiscountOnSale;       // NA
                                ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;  //_ItemSearchRowControl.SelectedItems[0].ConcessionAmount;          // ObjAddItem.ConcessionAmount;
                                ObjAddItem.Amount = ObjAddItem.Amount;      //_ItemSearchRowControl.SelectedItems[0].Amount;                              // ObjAddItem.Amount;

                                //////ObjAddItem.VATPercent = item.VATPerc;
                                //////Updated by MMBABU
                                //////if (item.VATPerc > 0)
                                //////    ObjAddItem.VATPercent = item.VATPerc / 100;
                                //////else
                                //////ObjAddItem.VATPercent = item.VATPerc;

                                ObjAddItem.VATPercent = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.TotalSalesTaxPercent;    //_ItemSearchRowControl.SelectedItems[0].TotalSalesTaxPercent;            // item.TotalSalesTaxPercent;     // NA

                                ////........................................
                                ////ObjAddItem.VATAmount = ObjAddItem.VATAmount;

                                ObjAddItem.NetAmount = ObjAddItem.NetAmount;    //_ItemSearchRowControl.SelectedItems[0].NetAmount;                      // ObjAddItem.NetAmount;                           

                                //By Anjali.................
                                ObjAddItem.Shelfname = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.Shelfname;  //_ItemSearchRowControl.SelectedItems[0].Shelfname;                        // item.Shelfname;
                                ObjAddItem.Containername = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.Containername;  //_ItemSearchRowControl.SelectedItems[0].Containername;                // item.Containername;
                                ObjAddItem.Rackname = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.Rackname;  //_ItemSearchRowControl.SelectedItems[0].Rackname;                          // item.Rackname;

                                ObjAddItem.AvailableStockInBase = ((clsItemStockVO)cmbItemBatch.SelectedItem).AvailableStockInBase; //_ItemSearchRowControl.SelectedItems[0].AvailableStockInBase;  // item.AvailableStockInBase;

                                ObjAddItem.StockUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SUOM;     //_ItemSearchRowControl.SelectedItems[0].SUOM;                              // item.SUOM;
                                ObjAddItem.PurchaseUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.PUOM;     //_ItemSearchRowControl.SelectedItems[0].PUOM;                           // item.PUOM;
                                ObjAddItem.PUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.PUOM;     //_ItemSearchRowControl.SelectedItems[0].PUOM;                                  // item.PUOM;
                                ObjAddItem.MainPUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.PUOM;     //_ItemSearchRowControl.SelectedItems[0].PUOM;                              // item.PUOM;
                                ObjAddItem.SUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SUOM;     //_ItemSearchRowControl.SelectedItems[0].SUOM;                                  // item.SUOM;

                                ObjAddItem.ConversionFactor = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.ConversionFactor);   // Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].ConversionFactor);      // item.ConversionFactor
                                ObjAddItem.PUOMID = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.PUM;    //_ItemSearchRowControl.SelectedItems[0].PUOMID;                                            // item.PUM;
                                ObjAddItem.SUOMID = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SUM;    //_ItemSearchRowControl.SelectedItems[0].SUOMID;                                            // item.SUM;
                                ObjAddItem.BaseUOMID = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.BaseUM;  //_ItemSearchRowControl.SelectedItems[0].BaseUOMID;                                      // item.BaseUM;
                                ObjAddItem.BaseUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.BaseUMString;  //_ItemSearchRowControl.SelectedItems[0].BaseUOM;                                          // item.BaseUMString;
                                ObjAddItem.SellingUOMID = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingUM;    //_ItemSearchRowControl.SelectedItems[0].SellingUOMID;                                // item.SellingUM;
                                ObjAddItem.SellingUOM = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingUMString;    //_ItemSearchRowControl.SelectedItems[0].SellingUOM;                                    // item.SellingUMString;
                                ObjAddItem.MainMRP = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).MRP) / Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF);    //Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].MRP);                            // Convert.ToSingle(item.MRP);
                                ObjAddItem.MainRate = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).PurchaseRate) / Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF);  //Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].PurchaseRate);                  // Convert.ToSingle(item.PurchaseRate);

                                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsSellBySellingUnit == true)
                                {
                                    ObjAddItem.SelectedUOM = new MasterListItem(ObjAddItem.SellingUOMID, ObjAddItem.SellingUOM);

                                    float CalculatedFromCF = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingCF / ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF; ; // NA    _ItemSearchRowControl.SelectedItems[0].SellingCF / _ItemSearchRowControl.SelectedItems[0].StockingCF;        // item.SellingCF / item.StockingCF;

                                    ObjAddItem.ConversionFactor = CalculatedFromCF;
                                    ObjAddItem.BaseConversionFactor = ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingCF;   // _ItemSearchRowControl.SelectedItems[0].SellingCF;     // item.SellingCF;    // NA  

                                    ObjAddItem.BaseQuantity = Convert.ToInt64(txtFreeQuantity.Text) * ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingCF;  //1 * _ItemSearchRowControl.SelectedItems[0].SellingCF;       // 1 * item.SellingCF;  // NA

                                    //////ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate);
                                    ObjAddItem.MainRate = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).PurchaseRate) / Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF);    //Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].PurchaseRate) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);      // Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                                    //////ObjAddItem.PurchaseRate = item.PurchaseRate * item.SellingCF;
                                    ObjAddItem.BaseRate = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).PurchaseRate) / Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF);    //Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].PurchaseRate) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);      // Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);

                                    ObjAddItem.PurchaseRate = ObjAddItem.BaseRate * ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingCF;   //ObjAddItem.BaseRate * _ItemSearchRowControl.SelectedItems[0].SellingCF;     // ObjAddItem.BaseRate * item.SellingCF;        // NA

                                    //////ObjAddItem.MainMRP = Convert.ToSingle(item.MRP);
                                    ObjAddItem.MainMRP = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).MRP) / Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF);    //Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].MRP) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);    // Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                                    //////ObjAddItem.MRP = item.MRP * item.SellingCF;
                                    ObjAddItem.BaseMRP = Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).MRP) / Convert.ToSingle(((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.StockingCF);    //Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].MRP) / Convert.ToSingle(_ItemSearchRowControl.SelectedItems[0].StockCF);    // Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);

                                    ObjAddItem.MRP = ObjAddItem.BaseMRP * ((clsItemStockVO)cmbItemBatch.SelectedItem).SelectedItemObj.SellingCF;   //ObjAddItem.BaseMRP * _ItemSearchRowControl.SelectedItems[0].SellingCF;   // ObjAddItem.BaseMRP * item.SellingCF;     // NA

                                }
                                else
                                {
                                    ObjAddItem.SelectedUOM = new MasterListItem(0, "--Select--");
                                }

                                //***//----------------------------
                                ObjAddItem.RegisteredPatientsDiscount = ((clsItemStockVO)cmbItemBatch.SelectedItem).RegisteredPatientsDiscount;
                                ObjAddItem.StaffDiscount = ((clsItemStockVO)cmbItemBatch.SelectedItem).StaffDiscount;
                                ObjAddItem.WalkinDiscount = ((clsItemStockVO)cmbItemBatch.SelectedItem).WalkinDiscount;
                                ObjAddItem.SGSTPercent = Convert.ToDouble(((clsItemStockVO)cmbItemBatch.SelectedItem).SGSTPercentage);
                                ObjAddItem.CGSTPercent = Convert.ToDouble(((clsItemStockVO)cmbItemBatch.SelectedItem).CGSTPercentage);
                                ObjAddItem.VATPercent = Convert.ToDouble(((clsItemStockVO)cmbItemBatch.SelectedItem).VATPerc);

                                ObjAddItem.Rackname = ((clsItemStockVO)cmbItemBatch.SelectedItem).Rackname;
                                ObjAddItem.Shelfname = ((clsItemStockVO)cmbItemBatch.SelectedItem).Shelfname;
                                ObjAddItem.Containername = ((clsItemStockVO)cmbItemBatch.SelectedItem).Containername;

                                //..........................


                                //For Multiple Sponser.........................
                                //if (cmbCompany.SelectedItem != null)
                                //    ObjAddItem.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                                //if (cmbPatientSource.SelectedItem != null)
                                //    ObjAddItem.PatientSourceID = ((clsPatientSourceVO)cmbPatientSource.SelectedItem).ID;
                                //if (cmbTariff.SelectedItem != null)
                                //   ObjAddItem.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                                //if (cmbPatientCategory.SelectedItem != null)
                                //   ObjAddItem.PatientCategoryID = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                                //.............................................

                                //Log Write here   

                                // lineNumber = stackFrame.GetFileLineNumber();


                                //.............END.......................


                                _ItemSelected.Add(ObjAddItem);

                                //cmbItemName.Text = null;
                                clearControl();
                                cmbItemName.Focus();

                                OnQuantityEnter_Click(this, new RoutedEventArgs());



                                //....................................

                                //....................................

                                // clearControl();

                                //            }
                                //        }
                                //    }
                                //    indicator.Close();
                                //};
                                //client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                                //client.CloseAsync();

                            }

                        }
                        else
                        {
                            txtFreeQuantity.Focus();
                        }
                    }
                }
                else
                {
                    cmbItemCode.Focus();
                }
            }
        }

        private bool ChkValidationForQuantity(clsItemStockVO SelectedItem)     //clsItemMasterVO SelectedItemObj,
        {
            bool result = true;





            if (string.IsNullOrEmpty(txtFreeQuantity.Text))
            {
                string msgText = "Quantity should be entered";

                MessageBoxControl.MessageBoxChildWindow msgWD1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD1.Show();
                msgWD1.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    txtFreeQuantity.Focus();
                };
                result = false;
            }
            else if (txtFreeQuantity.Text.IsItNumber() == true && Convert.ToInt64(txtFreeQuantity.Text) <= 0)
            {
                string msgText = "Quantity cannot be zero or less than zero";

                MessageBoxControl.MessageBoxChildWindow msgWD2 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD2.Show();
                msgWD2.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    txtFreeQuantity.Focus();
                };
                result = false;
            }
            else if (Convert.ToInt64(txtFreeQuantity.Text) * SelectedItem.SelectedItemObj.SellingCF > SelectedItem.AvailableStockInBase)  //ObjAddItem.BaseQuantity > ObjAddItem.AvailableStockInBase   //(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity > ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase)
            {
                //float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;

                //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase / ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor)));

                ////Log Write here   

                //// lineNumber = stackFrame.GetFileLineNumber();
                //if (IsAuditTrail)
                //{
                //    LogInformation = new LogInfo();
                //    LogInformation.guid = objGUID;
                //    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                //    LogInformation.TimeStamp = DateTime.Now;
                //    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                //    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                //    LogInformation.Message = " III : Line Number : " //+ Convert.ToString(lineNumber)
                //                                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                //                                            + " , StoreID : " + Convert.ToString(StoreID) + " "
                //                                            + " , ItemID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ItemID) + " "
                //                                            + " , BatchID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchID) + " "
                //                                            + " , BatchCode : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BatchCode) + " "
                //                                            + " , ExpiryDate : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ExpiryDate) + " "
                //                                            + " , Transaction UOMID : " + Convert.ToString(TransactionUOMID) + " "
                //                                            + " , Transaction Quantity (InputTransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity) + " "
                //                                            + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor) + " "
                //                                            + " , Base Quantity (TransactionQuantity) : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseQuantity) + " "
                //                                            + " , Base UOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseUOMID) + " "
                //                                            + " , SUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).SUOMID) + " "
                //                                            + " , StockCF : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                //                                            + " , Stocking Quantity : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConversionFactor) + " "
                //                                            + " , PUOMID : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).PUOMID) + " "
                //                                            + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionPercentage) + " "
                //                                            + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).ConcessionAmount) + " "
                //                                            + " , AvailableStockInBase : " + Convert.ToString(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase) + " ";

                //    LogInfoList.Add(LogInformation);
                //}
                ////CallLogBizAction(LogBizAction);
                //////

                string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                //ConversionsForAvailableStock();
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    txtFreeQuantity.Focus();
                };

                result = false;
            }

            return result;
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
            txtSellingUom.Text = "";
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

            //dtpExpDate.IsEnabled = true;
            //txtMRP.IsEnabled = true;

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
            if (e.PlatformKeyCode.Equals(109) || e.PlatformKeyCode.Equals(110) || e.PlatformKeyCode.Equals(190))
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
