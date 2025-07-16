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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using CIMS;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class ItemSearch : UserControl
    {
        private List<clsItemTaxVO> ItemTaxDetailList { get; set; }
        public event RoutedEventHandler OnQuantityEnter_Click;

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }
        public bool IsFromIndent { get; set; }

        public ItemSearch(long StoreID, long SupplierID)
        {
            InitializeComponent();
            this.StoreID = StoreID;
            this.SupplierID = SupplierID;
            this.Loaded += new RoutedEventHandler(ItemSearch_Loaded);
        }

        public ItemSearch()
        {
            InitializeComponent();
            _ItemSelected = new ObservableCollection<clsItemMasterVO>();
            this.StoreID = StoreID;
            this.SupplierID = SupplierID;
            this.Loaded += new RoutedEventHandler(ItemSearch_Loaded);
            //SetFocus();
            cmbItemName.Focus();
        }

        void ItemSearch_Loaded(object sender, RoutedEventArgs e)
        {
            _ItemSelected = new ObservableCollection<clsItemMasterVO>();
            ItemTaxDetailList = new List<clsItemTaxVO>();
            DataList = new List<MasterListItem>();

            if (StoreID > 0 && SupplierID > 0 && IsFromIndent == false)
            {
                GetData();
            }

            if (StoreID > 0 && IsFromIndent == true)
            {
                GetData();
            }
            
            cmbItemName.Focus();
            
        }
        WaitIndicator indicator = new WaitIndicator();
        public MasterListItem SelectedItem { get; set; }
        bool fromSetFocus = false;
        public bool IsFromClearControl { get; set; }
        bool IsFromCmbItemCodeEnter { get; set; }
        bool IsFromCmbItemNameEnter { get; set; }
        public List<MasterListItem> DataList { get; private set; }
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
        public bool ShowZeroStockBatches { get; set; }
        public bool ShowScrapItems { get; set; }
        public bool IsFromOpeningBalance { get; set; }
        public bool ShowNotShowPlusThreeMonthExp { get; set; }

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


        }

        private void cmbItemCode_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        bool IsFromItemCodeKeyDown = false;
        private void cmbItemCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (StoreID > 0)
            {
                if (SupplierID > 0 && IsFromIndent == false)
                {
                    if (e.Key.Equals(Key.Enter))
                    {
                        IsFromItemCodeKeyDown = true;
                        GetfromItemCodeSelectedItem();
                    }
                    else if (e.Key.Equals(Key.Tab))
                    {
                        IsFromItemCodeKeyDown = true;
                        GetfromItemCodeSelectedItem();
                    }
                }
                else if (SupplierID == 0 && IsFromIndent == true)
                {
                    if (e.Key.Equals(Key.Enter))
                    {
                        IsFromItemCodeKeyDown = true;
                        GetfromItemCodeSelectedItem();
                    }
                    else if (e.Key.Equals(Key.Tab))
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
                            txtQuantity.Focus();


                        }
                        else if (!string.IsNullOrEmpty(cmbItemCode.Text))
                        {
                            if (DataList.SingleOrDefault(S => S.Code.ToUpper().Equals(cmbItemCode.Text.ToUpper())) != null)
                            {
                                cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.ToUpper().Equals(cmbItemCode.Text.ToUpper()));
                                SelectedItem = ((MasterListItem)cmbItemCode.SelectedItem);
                                cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                                IsFromCmbItemCodeEnter = true;
                                txtQuantity.Focus();


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
                if (SupplierID > 0 && IsFromIndent == false)
                {
                    if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Tab))
                    {
                        IsFromItemNameKeyDown = true;
                        GetfromItemNameSelectedItem();
                    }
                }
                else if (SupplierID == 0 && IsFromIndent == true)
                {
                    if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Tab))
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
            txtMRP.Text = "";
            txtPurchaseRate.Text = "";
            txtQuantity.Text = "";
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
                        txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                        txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                        txtQuantity.Focus();
                        txtQuantity.SelectAll();

                    }
                    else if (!string.IsNullOrEmpty(cmbItemName.Text))
                    {
                        if (DataList.SingleOrDefault(S => S.Description.ToUpper().Equals(cmbItemName.Text.ToUpper())) != null)
                        {
                            cmbItemName.SelectedItem = DataList.SingleOrDefault(S => S.Description.ToUpper().Equals(cmbItemName.Text.ToUpper()));
                            SelectedItem = ((MasterListItem)cmbItemName.SelectedItem);
                            cmbItemCode.SelectedItem = DataList.SingleOrDefault(S => S.Code.Equals(SelectedItem.Code));
                            IsFromCmbItemNameEnter = true;
                            txtMRP.Text = Convert.ToString(SelectedItem.MRP);
                            txtPurchaseRate.Text = Convert.ToString(SelectedItem.PurchaseRate);
                            txtQuantity.Focus();
                            txtQuantity.SelectAll();
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

        public void clearControl()
        {
            cmbItemCode.SelectedItem = null;
            cmbItemName.SelectedItem = null;
            txtMRP.Text = "0";
            txtPurchaseRate.Text = "0";
            txtQuantity.Text = "0";
            cmbItemCode.Text = "";
            cmbItemName.Text = "";
            if (!fromSetFocus)
            {
                IsFromClearControl = true;
            }
            else
            {
                fromSetFocus = false;
            }
            cmbItemName.Focus();
            txtMRP.IsEnabled = true;
            IsFromCmbItemNameEnter = false;
            IsFromCmbItemCodeEnter = false;
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
            if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Tab))
            {
                if (cmbItemName.SelectedItem != null)
                {
                    if (OnQuantityEnter_Click != null)
                    {
                        indicator.Show();
                        clsGetItemListForSearchBizActionVO BizActionObject = new clsGetItemListForSearchBizActionVO();
                        BizActionObject.ItemList = new List<clsItemMasterVO>();
                        BizActionObject.ShowScrapItems = false;
                        BizActionObject.StoreID = this.StoreID;
                        if (cmbItemCode.SelectedItem != null)
                        {
                            BizActionObject.ItemCode = ((MasterListItem)cmbItemCode.SelectedItem).Code;
                        }
                        BizActionObject.ItemName = ((MasterListItem)cmbItemName.SelectedItem).Description; //((MasterListItem)cmbItemCode.SelectedItem).Description;
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
                                        _ItemSelected = new ObservableCollection<clsItemMasterVO>();
                                        clsItemMasterVO item = result.ItemList[0];
                                        clsItemMasterVO Obj = new clsItemMasterVO();
                                        Obj.ItemID = item.ID;
                                        Obj.ItemName = item.ItemName;
                                        Obj.ItemCode = item.ItemCode;
                                        Obj.BaseUM = item.BaseUM;
                                        Obj.BaseUMString = item.BaseUMString;
                                        Obj.PUM = item.PUM;
                                        Obj.PUOM = item.PUOM;
                                        Obj.SUM = item.SUM;
                                        Obj.SUOM = item.SUOM;
                                        Obj.VatPer = item.VatPer;
                                        Obj.PurchaseRate = item.PurchaseRate;
                                        Obj.MRP = item.MRP;
                                        if (!string.IsNullOrEmpty(txtQuantity.Text))
                                            Obj.RequiredQuantity = Convert.ToDouble(txtQuantity.Text);
                                        Obj.ConversionFactor = item.ConversionFactor;
                                        _ItemSelected.Add(Obj);
                                      
                                        //By Anjali......................
                                        cmbItemName.Text = null;
                                        cmbItemName.Focus();
                                       
                                       
                                        //...............................
                                        OnQuantityEnter_Click(this, new RoutedEventArgs());

                                    }
                                }
                            }
                            indicator.Close();
                        };
                        client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();





                    }
                }
                else
                {
                    cmbItemCode.Focus();
                }

            }
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

        private void cmbItemBatch_KeyDown(object sender, KeyEventArgs e)
        {

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

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtQuantity_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
