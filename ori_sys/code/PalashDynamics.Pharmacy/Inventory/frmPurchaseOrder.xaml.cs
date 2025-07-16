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
using PalashDynamics.ValueObjects.Inventory.Quotation;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.RateContract;
using System.Windows.Input;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Log;
using System.Reflection;

namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class frmPurchaseOrder : UserControl
    {
        #region Variale Declarations
        public long POApproveLvlID = 0;
        private SwivelAnimation objAnimation;
        Boolean IsPageLoded = false;
        public ObservableCollection<clsPurchaseOrderDetailVO> PurchaseOrderItems { get; set; }
        public ObservableCollection<clsPurchaseOrderDetailVO> POIndentdetails { get; set; }
        public ObservableCollection<clsPurchaseOrderVO> purchaseOrderList { get; set; }
        public List<MasterListItem> PurchaseOrderTerms { get; set; }
        public long _EnquiryID { get; set; }
        public Boolean ISEditMode = false;
        public PurchaseOrderSource enmPurchaseOorder { get; set; }
        public CheckBox chkFreez;
        public string msgTitle = "Palash";
        public string msgText = String.Empty;
        WaitIndicator indicator = new WaitIndicator();
        public List<MasterListItem> CurrencyList { get; set; }
        public List<MasterListItem> ConversionFactorList { get; set; }
        public enum PurchaseOrderSource
        {
            Indent,
            Quotation
        };

        //Added By CDS
        public bool IsApproveded = false;
        public bool ISFromAprovePO = false;
        public clsPurchaseOrderVO SelectedPO = null;
        public Boolean ISEditAndAprove = false;
        List<long> IDS = new System.Collections.Generic.List<long>();
        bool Approved = false;
        long lngSupplierID = -1;
        string IndentNo = String.Empty;
        List<clsRateContractMasterVO> lstRateContract;
        List<clsRateContractItemDetailsVO> lstRateContractItemDetails;

        List<clsRateContractDetailsVO> lstRateContractItemDetailsNew;

        List<clsRateContractDetailsVO> POBestPriceList;

        public enum PaymentMode
        {
            Cash = 1,
            Cheque = 2,
            AccountTransfer = 3
        };

        public bool IsForDirectApprove = false; // added by Ashish Z on 280716 for Direct Approve from PO Approval Form
        // For Activity Log By Umesh
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;
        bool IsAuditTrail = false;
        // Activity log end
        #endregion
        #region Constructor and Loaded
        public frmPurchaseOrder()
        {
            InitializeComponent();
            this.DataContext = new clsPurchaseOrderVO();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            grdpodetails.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(grdpodetails_CellEditEnded);
            DataList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 4;
            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;// By Umesh For Enable/Disable Audit Trail
        }

        public List<MasterListItem> ApplicableOnList { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicableOnList = new List<MasterListItem>();
            FillState(); ////Added By Bhushanp For GST 21062017
            FillApplicableOn(); //Added By ARZ on Dated 21042017
            FillCurrency();
            dtpDate.SelectedDate = DateTime.Now.Date;
            dtpDate.IsEnabled = false;
            CurrencyList = new List<MasterListItem>();

            ConversionFactorList = new List<MasterListItem>();

            if (!IsPageLoded)
            {
                dtpDate.SelectedDate = DateTime.Now.Date;
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;
                FillSchedule();
                rdbIndent.IsChecked = true;
                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();//FillSchedule();
                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                grdpodetails.ItemsSource = null;
                grdpodetails.ItemsSource = null;
                purchaseOrderList = new ObservableCollection<clsPurchaseOrderVO>();
                dgPOList.ItemsSource = PurchaseOrderItems;
                dgPOList.ItemsSource = purchaseOrderList;
                FillPurchseOrderTerms();
                rdbCash.IsChecked = true;
                SetCommandButtonState("New");
                FillPurchaseOrderDataGrid();
                IsPageLoded = true;
            }
        }
        #endregion

        #region Added By ARZ for Itemwise Tax mapping during Transaction in Discussion with Mangesh and Nilesh Sir on Dated 21042017
        private void FillApplicableOn()
        {
            List<MasterListItem> mlApplicableOn = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlApplicableOn.Insert(0, Default);
            EnumToList(typeof(InventoryTaxApplicaleOn), mlApplicableOn);
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.AddRange(mlApplicableOn);
            ApplicableOnList = objList;
        }

        private static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if (Value > 0)
                {
                    string Display = Enum.GetName(EnumType, Value);
                    MasterListItem Item = new MasterListItem(Value, Display);
                    TheMasterList.Add(Item);
                }
            }
        }

        private static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }

        private void cmbApplicableOn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {
                FrameworkElement result = GetParent((FrameworkElement)sender, typeof(DataGridCell));
                if (result != null)
                {
                    var comboBox = (AutoCompleteBox)sender;
                    if (comboBox.SelectedItem != null)
                    {
                        clsPurchaseOrderDetailVO objPODetailsVO = new clsPurchaseOrderDetailVO();
                        objPODetailsVO = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;
                        if (((MasterListItem)comboBox.SelectedItem).Description == InventoryTaxApplicaleOn.PurchaseRate.ToString())
                        {
                            objPODetailsVO.POItemVatType = 2;       // 2 == Exclusive
                            objPODetailsVO.POItemOtherTaxType = 2;  // 2 == Exclusive
                            objPODetailsVO.POCGSTVatType = 2;       // 2 == Exclusive      
                            objPODetailsVO.POSGSTVatType = 2;       // 2 == Exclusive
                            objPODetailsVO.POIGSTVatType = 2;       // 2 == Exclusive
                        }
                        else if (((MasterListItem)comboBox.SelectedItem).Description == InventoryTaxApplicaleOn.MRP.ToString())
                        {
                            objPODetailsVO.POItemVatType = 1;        // 1 == Inclusive
                            objPODetailsVO.POItemOtherTaxType = 1;  // 1 == Inclusive
                            objPODetailsVO.POCGSTVatType = 1;       // 1 == Inclusive    
                            objPODetailsVO.POSGSTVatType = 1;       // 1 == Inclusive
                            objPODetailsVO.POIGSTVatType = 1;       // 1 == Inclusive
                        }
                        else
                        {
                            objPODetailsVO.POItemVatType = 0;
                            objPODetailsVO.POItemOtherTaxType = 0;
                            objPODetailsVO.POCGSTVatType = 0;
                            objPODetailsVO.POSGSTVatType = 0;
                            objPODetailsVO.POIGSTVatType = 0;
                        }

                        objPODetailsVO.POItemVatApplicationOn = objPODetailsVO.POItemOtherTaxApplicationOn = Convert.ToInt32(((MasterListItem)comboBox.SelectedItem).ID);
                        objPODetailsVO.POCGSTVatApplicationOn = objPODetailsVO.POSGSTVatApplicationOn = objPODetailsVO.POIGSTVatApplicationOn = Convert.ToInt32(((MasterListItem)comboBox.SelectedItem).ID);
                    }
                    CalculateOpeningBalanceSummary();
                }
            }
        }
        #endregion Endded By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017



        #region 'Paging'

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
        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }

        #endregion

        #region Clicked Events
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            lngSupplierID = -1;
            SetCommandButtonState("ClickNew");
            PurchaseOrderItems.Clear();
            POIndentdetails.Clear();
            cboStore.IsEnabled = true;
            cmbStore.SelectedValue = (long)0;
            cmbStore.IsEnabled = true;
            cmbDeliveryDays.SelectedValue = (long)0;
            cmbDeliveryDays.IsEnabled = true;
            cboSupplier.SelectedValue = (long)0;
            cboSupplier.IsEnabled = true;
            cmbSupplier.SelectedValue = (long)0;
            cboDelivery.SelectedValue = (long)0;
            cboDelivery.IsEnabled = true;
            cmbModeofPayment.SelectedValue = (long)0;
            cboTOP.SelectedValue = (long)0;
            rdbIndent.IsChecked = true;
            cmbSchedule.SelectedValue = (long)0;
            FillSchedule();
            txtDiscountAmount.Text = string.Empty;
            txtGrossAmount.Text = string.Empty;
            txtGuarantee.Text = string.Empty;
            txtIndentNo.Text = string.Empty;
            txtNETAmount.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtVATAmount.Text = string.Empty;
            ISEditMode = false;
            if (cmbTermsNCondition.ItemsSource != null)
            {
                foreach (MasterListItem item in PurchaseOrderTerms)
                {
                    item.Status = false;
                }
                cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
            }
            BdrItemSearch.Visibility = Visibility.Collapsed;

            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (ISFromAprovePO == true)
            {
                frmApprovePO AprvPO = new frmApprovePO();

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Approve Purchase Order";
                ((IApplicationConfiguration)App.Current).OpenMainContent(AprvPO);
            }
            else
            {
                PurchaseOrderItems.Clear();
                POIndentdetails.Clear();
                SetCommandButtonState("New");
                cboStore.SelectedValue = (long)0;
                cmbStore.SelectedValue = (long)0;
                cboSupplier.SelectedValue = (long)0;
                cmbSupplier.SelectedValue = (long)0;
                cboDelivery.SelectedValue = (long)0;
                cmbSchedule.SelectedValue = (long)0;
                FillSchedule();
                txtDiscountAmount.Text = string.Empty;
                txtGrossAmount.Text = string.Empty;
                txtGuarantee.Text = string.Empty;
                txtNETAmount.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtVATAmount.Text = string.Empty;
                if (cmbTermsNCondition.ItemsSource != null)
                {
                    foreach (MasterListItem item in PurchaseOrderTerms)
                    {
                        item.Status = false;
                    }
                    cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
                }
                FillPurchaseOrderDataGrid();
                objAnimation.Invoke(RotationType.Backward);
            }
        }

        private void cmdIndent_Click(object sender, RoutedEventArgs e)
        {
            if (((clsStoreVO)cboStore.SelectedItem).StoreId == null || ((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            //else if (((MasterListItem)cboSupplier.SelectedItem).ID == null || ((MasterListItem)cboSupplier.SelectedItem).ID == 0)// && rdbDirect.IsChecked == true) //|| cboSupplier.SelectedItem == null
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                  new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW1.Show();
            //}
            else if (rdbIndent.IsChecked == true)
            {
                if (PurchaseOrderItems != null && PurchaseOrderItems.Count.Equals(0))
                {
                    PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                }

                enmPurchaseOorder = PurchaseOrderSource.Indent;
                SupplierIndentSearch objIndentSearch = new SupplierIndentSearch();
                objIndentSearch.IsOnlyItems = true;
                objIndentSearch.cmbToIndentStore.SelectedItem = (cboStore.SelectedItem);
                objIndentSearch.ToStoreID = ((PalashDynamics.ValueObjects.Inventory.clsStoreVO)(cboStore.SelectedItem)).StoreId;
                if (cboSupplier.SelectedItem != null && (cboSupplier.SelectedItem as MasterListItem).ID > 0)
                    objIndentSearch.SupplierId = (cboSupplier.SelectedItem as MasterListItem).ID;
                else
                    objIndentSearch.SupplierId = 0;
                objIndentSearch.OnItemSelectionCompleted += new SupplierIndentSearch.ItemSelection(objIndentSearch_OnItemSelectionCompleted);
                objIndentSearch.Show();
            }
            else if (rdbDirect.IsChecked == true)
            {
                if (PurchaseOrderItems != null && PurchaseOrderItems.Count.Equals(0))
                {
                    PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                }

                ItemList Itemswin = new ItemList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Itemswin.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                Itemswin.cmbStore.IsEnabled = false;
                Itemswin.ShowBatches = false;
                Itemswin.ShowQuantity = true;
                Itemswin.objSupplierList = objSupplierList;
                if (cboSupplier.SelectedItem != null && (cboSupplier.SelectedItem as MasterListItem).ID > 0)
                    Itemswin.SupplierId = (cboSupplier.SelectedItem as MasterListItem).ID;
                else
                    Itemswin.SupplierId = 0;
                Itemswin.IsFromPO = true;
                Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                Itemswin.Show();
            }
        }

        void objIndentSearch_OnItemSelectionCompleted(object sender, EventArgs e)
        {
            String strItemIDs = string.Empty;
            SupplierIndentSearch objItemSearch = (SupplierIndentSearch)sender;
            ObservableCollection<clsItemListByIndentId> selectedIndentItems;

            if (cboSupplier.SelectedItem != null)
            {
                cboSupplier.SelectedValue = objItemSearch.SupplierId;

                if ((cboSupplier.SelectedItem as MasterListItem).ID > 0)
                    cboSupplier.IsEnabled = false;
                else
                    cboSupplier.IsEnabled = true;
            }


            if (rdbIndent.IsChecked == true)
            {
                grdpodetails.Columns[3].Visibility = Visibility.Visible;
                grdpodetails.Columns[4].Visibility = Visibility.Visible;
                grdpodetails.Columns[5].Visibility = Visibility.Visible;
                grdpodetails.Columns[5].IsReadOnly = true;
                grdpodetails.Columns[6].Visibility = Visibility.Visible;
                grdpodetails.Columns[7].Visibility = Visibility.Collapsed;
                grdpodetails.Columns[13].IsReadOnly = false;
                grdpodetails.Columns[15].IsReadOnly = false;
            }

            objItemSearch.IsOnlyItems = true;
            selectedIndentItems = objItemSearch.ocSelectedItemList;
            //if (lngSupplierID == -1 || lngSupplierID != selectedIndentItems[0].SupplierID)
            //{
            //    PurchaseOrderItems.Clear();
            //    PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();

            //    POIndentdetails.Clear();
            //    POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
            //}

            lngSupplierID = selectedIndentItems[0].SupplierID;

            #region Added by Ashish Z. For Delivery Loc Combo Selection on 12Apr16
            List<clsItemListByIndentId> tempListForDelvryLocCombo = new List<clsItemListByIndentId>();

            foreach (var item in PurchaseOrderItems.ToList())
            {
                clsItemListByIndentId tempVO = new clsItemListByIndentId();
                tempVO.FromStoreID = item.FromStoreID;
                tempListForDelvryLocCombo.Add(tempVO);
            }

            foreach (var item in selectedIndentItems.ToList())
            {
                tempListForDelvryLocCombo.Add(item);
            }

            var count = tempListForDelvryLocCombo.Select(p => p.FromStoreID).Distinct().Count();
            if (count == 1)
            {
                cboDelivery.SelectedValue = selectedIndentItems[0].FromStoreID;
                cboStore.IsEnabled = false;
                cboDelivery.IsEnabled = true;
            }
            else
            {
                cboDelivery.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID;
                cboStore.IsEnabled = false;
                cboDelivery.IsEnabled = false;
            }
            #endregion
            // Get the PO list for multiple indent
            if (selectedIndentItems != null && selectedIndentItems.Count > 0)
            {
                foreach (clsItemListByIndentId item in selectedIndentItems)
                {

                    var item1 = from r in POIndentdetails
                                where (r.ItemID == item.ItemId && r.IndentID == item.IndentId && r.IndentUnitID == item.IndentUnitID)
                                select r;
                    if (item1.ToList().Count == 0)
                    {

                        if (!txtIndentNo.Text.Trim().Contains(item.IndentNumber.Trim()))
                            txtIndentNo.Text = (txtIndentNo.Text + ", " + item.IndentNumber.Trim()).Trim(',');

                        //var results1 = from r in ((List<MasterListItem>)cboSupplier.ItemsSource)
                        //               where r.ID == item.SupplierID
                        //               select r;

                        //foreach (MasterListItem Supplier in results1)
                        //{
                        //    cboSupplier.SelectedItem = Supplier;
                        //}
                        POIndentdetails.Add(new clsPurchaseOrderDetailVO
                        {
                            IndentID = Convert.ToInt64(item.IndentId),
                            IndentUnitID = Convert.ToInt64(item.IndentUnitID),
                            IndentDetailID = item.IndentDetailsID,
                            IndentDetailUnitID = item.IndentUnitID,
                            ItemID = item.ItemId,
                            ItemName = item.ItemName,
                            ItemGroup = item.ItemGroup,
                            ItemCategory = item.ItemCategory,
                            ItemCode = item.ItemCode,
                            PUM = item.PUM,
                            SelectedUOM = new MasterListItem { ID = item.UOMID, Description = item.TransUOM },
                            TransUOM = item.TransUOM,
                            ConversionFactor = item.ConversionFactor,
                            BaseConversionFactor = Convert.ToSingle(item.BaseConversionFactor),
                            BaseRate = Convert.ToSingle(item.PurchaseRate),
                            BaseMRP = Convert.ToSingle(item.MRP),
                            BaseQuantity = item.BaseQuantity,
                            SUOM = item.SUOM,
                            SUOMID = item.SUOMID,
                            MainRate = Convert.ToSingle(item.PurchaseRate),
                            MainMRP = Convert.ToSingle(item.MRP),
                            PUOMID = item.UOMID,
                            POPUOMID = item.PUOMID,
                            BaseUOMID = item.BaseUOMID,
                            BaseUOM = item.BaseUOM,
                            Quantity = 0,
                            Rate = Convert.ToDecimal(item.PurchaseRate) * Convert.ToDecimal(item.BaseConversionFactor),
                            PRQuantity = Convert.ToDecimal(item.IndentQty),//PRQuantity = Convert.ToDecimal(item.BalanceQty) / Convert.ToDecimal(item.BaseConversionFactor),  // COMMENTED BY Ashish on 14-3-2016 for showing the wrong PR Qty.
                            PendingQuantity = Convert.ToDecimal(item.BalanceQty),
                            PRPendingQuantity = Convert.ToDouble(item.BalanceQty),
                            ItemVATPercent = item.ItemVatPer,
                            POItemVatType = item.ItemVatType,
                            POItemVatApplicationOn = item.ItemVatApplicationOn,
                            POItemOtherTaxType = item.ItemOtherTaxType,
                            POItemOtherTaxApplicationOn = item.OtherItemApplicationOn,
                            VATPercent = (decimal)item.VATPer,
                            MRP = Convert.ToDecimal(item.MRP) * Convert.ToDecimal(item.BaseConversionFactor),
                            IndentNumber = item.IndentNumber,
                            PurchaseToBaseCF = item.PurchaseToBaseCF,
                            StockingToBaseCF = item.PurchaseToBaseCF / item.StockingToBaseCF,
                            POApprItemQty = item.POApprItemQty,
                            POPendingItemQty = item.POPendingItemQty,
                            ApplicableOnList = ApplicableOnList,                                                                   //Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                            SelectedApplicableOn = ApplicableOnList.Where(z => z.ID == item.CGSTVatApplicationOn).FirstOrDefault(), // item.ItemVatApplicationOn Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                            POSGSTVatType = item.SGSTVatType,
                            POSGSTVatApplicationOn = item.SGSTVatApplicationOn,
                            POCGSTVatType = item.CGSTVatType,
                            POCGSTVatApplicationOn = item.CGSTVatApplicationOn,
                            POIGSTVatType = item.CGSTVatType,
                            POIGSTVatApplicationOn = item.IGSTVatApplicationOn,
                            SGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.SGSTPercent : 0,
                            CGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.CGSTPercent : 0,
                            IGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID != ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.IGSTPercent : 0,
                            HSNCode = item.HSNCodes
                        });
                        if (PurchaseOrderItems.Count > 0)
                        {
                            var Purchase = (from r in PurchaseOrderItems
                                            where (r.ItemID == item.ItemId && r.IndentID == item.IndentId && r.IndentUnitID == item.IndentUnitID && r.IndentDetailID == item.IndentDetailsID && r.IndentDetailUnitID == item.IndentDetailsUnitID)
                                            select r).ToList().Count;

                            if (Purchase > 0)
                            {
                                clsPurchaseOrderDetailVO obj = PurchaseOrderItems.Where(z => z.ItemID == item.ItemId && z.IndentID == item.IndentId && z.IndentUnitID == item.IndentUnitID && z.IndentDetailID == item.IndentDetailsID && z.IndentDetailUnitID == item.IndentDetailsUnitID).FirstOrDefault();
                                if (obj != null)
                                {
                                    obj.Quantity += Convert.ToDecimal(item.BalanceQty);
                                }
                            }

                            else
                            {
                                clsPurchaseOrderDetailVO obj = new clsPurchaseOrderDetailVO();
                                obj.IndentID = Convert.ToInt64(item.IndentId);
                                obj.IndentUnitID = Convert.ToInt64(item.IndentUnitID);
                                obj.IndentDetailID = item.IndentDetailsID;
                                obj.IndentDetailUnitID = item.IndentUnitID;
                                obj.ItemID = item.ItemId;
                                obj.ItemGroup = item.ItemGroup;
                                obj.ItemCategory = item.ItemCategory;
                                obj.ItemName = item.ItemName;
                                obj.ItemCode = item.ItemCode;
                                obj.PUM = item.PUM;
                                obj.SelectedUOM = new MasterListItem { ID = item.UOMID, Description = item.TransUOM };
                                obj.TransUOM = item.TransUOM;
                                obj.ConversionFactor = item.ConversionFactor;
                                obj.BaseConversionFactor = Convert.ToSingle(item.BaseConversionFactor);
                                obj.BaseRate = Convert.ToSingle(item.PurchaseRate);
                                obj.BaseMRP = Convert.ToSingle(item.MRP);
                                obj.BaseQuantity = item.BaseQuantity;
                                obj.SUOM = item.SUOM;
                                obj.SUOMID = item.SUOMID;
                                obj.MainRate = Convert.ToSingle(item.PurchaseRate);
                                obj.MainMRP = Convert.ToSingle(item.MRP);
                                obj.PUOMID = item.UOMID;
                                obj.POPUOMID = item.PUOMID;
                                obj.BaseUOMID = item.BaseUOMID;
                                obj.BaseUOM = item.BaseUOM;
                                obj.Quantity = 0;
                                obj.Rate = Convert.ToDecimal(item.PurchaseRate) * Convert.ToDecimal(item.BaseConversionFactor);
                                obj.PRQuantity = Convert.ToDecimal(item.IndentQty); //obj.PRQuantity = Convert.ToDecimal(item.BalanceQty) / Convert.ToDecimal(item.BaseConversionFactor);  //// COMMENTED BY Ashish on 14-3-2016 for showing the wrong PR Qty.
                                obj.PendingQuantity = Convert.ToDecimal(item.BalanceQty);
                                obj.PRPendingQuantity = Convert.ToDouble(item.BalanceQty);
                                obj.ItemVATPercent = item.ItemVatPer;
                                obj.POItemVatType = item.ItemVatType;
                                obj.POItemVatApplicationOn = item.ItemVatApplicationOn;
                                obj.POItemOtherTaxType = item.ItemOtherTaxType;
                                obj.POItemOtherTaxApplicationOn = item.OtherItemApplicationOn;
                                obj.VATPercent = (decimal)item.VATPer;
                                obj.MRP = Convert.ToDecimal(item.MRP) * Convert.ToDecimal(item.BaseConversionFactor);
                                obj.IndentNumber = item.IndentNumber;
                                obj.RateContractID = 0;
                                obj.RateContractUnitID = 0;
                                obj.CurrencyList = CurrencyList;
                                obj.SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault();
                                obj.CostRate = (obj.Rate * obj.Quantity);
                                obj.PurchaseToBaseCF = item.PurchaseToBaseCF;
                                obj.StockingToBaseCF = item.PurchaseToBaseCF / item.StockingToBaseCF;
                                obj.POApprItemQty = item.POApprItemQty;
                                obj.POPendingItemQty = item.POPendingItemQty;
                                obj.FromStoreID = item.FromStoreID;
                                obj.ApplicableOnList = ApplicableOnList;                                                                     //Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                                obj.SelectedApplicableOn = ApplicableOnList.Where(z => z.ID == item.CGSTVatApplicationOn).FirstOrDefault();  //item.ItemVatApplicationOn Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                                //Added By Bhushanp For GST 22062017
                                obj.POSGSTVatType = item.SGSTVatType;
                                obj.POSGSTVatApplicationOn = item.SGSTVatApplicationOn;
                                obj.POCGSTVatType = item.CGSTVatType;
                                obj.POCGSTVatApplicationOn = item.CGSTVatApplicationOn;
                                obj.POIGSTVatType = item.CGSTVatType;
                                obj.POIGSTVatApplicationOn = item.IGSTVatApplicationOn;
                                obj.SGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.SGSTPercent : 0;
                                obj.CGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.CGSTPercent : 0;
                                obj.IGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID != ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.IGSTPercent : 0;
                                obj.HSNCode = item.HSNCodes;
                                obj.TotalBatchAvailableStock = item.TotalBatchAvailableStock;
                                PurchaseOrderItems.Add(obj);
                                strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemId) + ",");
                            }
                        }
                        else
                        {
                            clsPurchaseOrderDetailVO obj = new clsPurchaseOrderDetailVO();
                            obj.IndentID = Convert.ToInt64(item.IndentId);
                            obj.IndentUnitID = Convert.ToInt64(item.IndentUnitID);
                            obj.IndentDetailID = item.IndentDetailsID;
                            obj.IndentDetailUnitID = item.IndentUnitID;
                            obj.ItemID = item.ItemId;
                            obj.ItemGroup = item.ItemGroup;
                            obj.ItemCategory = item.ItemCategory;
                            obj.ItemName = item.ItemName;
                            obj.ItemCode = item.ItemCode;
                            obj.PUM = item.PUM;
                            obj.SelectedUOM = new MasterListItem { ID = item.UOMID, Description = item.TransUOM };
                            obj.TransUOM = item.TransUOM;
                            obj.ConversionFactor = item.ConversionFactor;
                            obj.BaseConversionFactor = Convert.ToSingle(item.BaseConversionFactor);
                            obj.BaseRate = Convert.ToSingle(item.PurchaseRate);
                            obj.BaseMRP = Convert.ToSingle(item.MRP);
                            obj.BaseQuantity = item.BaseQuantity;
                            obj.SUOM = item.SUOM;
                            obj.SUOMID = item.SUOMID;
                            obj.MainRate = Convert.ToSingle(item.PurchaseRate);
                            obj.MainMRP = Convert.ToSingle(item.MRP);
                            obj.PUOMID = item.UOMID;
                            obj.POPUOMID = item.PUOMID;
                            obj.BaseUOMID = item.BaseUOMID;
                            obj.BaseUOM = item.BaseUOM;
                            obj.Quantity = 0;
                            obj.Rate = Convert.ToDecimal(item.PurchaseRate) * Convert.ToDecimal(item.BaseConversionFactor);
                            obj.PRQuantity = Convert.ToDecimal(item.IndentQty); //obj.PRQuantity = Convert.ToDecimal(item.BalanceQty) / Convert.ToDecimal(item.BaseConversionFactor);  // COMMENTED BY Ashish on 14-3-2016 for showing the wrong PR Qty.
                            obj.PendingQuantity = Convert.ToDecimal(item.BalanceQty);
                            obj.PRPendingQuantity = Convert.ToDouble(item.BalanceQty);
                            obj.ItemVATPercent = item.ItemVatPer;
                            obj.POItemVatType = item.ItemVatType;
                            obj.POItemVatApplicationOn = item.ItemVatApplicationOn;
                            obj.POItemOtherTaxType = item.ItemOtherTaxType;
                            obj.POItemOtherTaxApplicationOn = item.OtherItemApplicationOn;
                            obj.VATPercent = (decimal)item.VATPer;
                            obj.MRP = Convert.ToDecimal(item.MRP) * Convert.ToDecimal(item.BaseConversionFactor);
                            obj.IndentNumber = item.IndentNumber;
                            obj.RateContractID = 0;
                            obj.RateContractUnitID = 0;
                            obj.CurrencyList = CurrencyList;
                            obj.SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault();
                            obj.CostRate = (obj.Rate * obj.Quantity);
                            obj.PurchaseToBaseCF = item.PurchaseToBaseCF;
                            obj.StockingToBaseCF = item.PurchaseToBaseCF / item.StockingToBaseCF;
                            obj.POApprItemQty = item.POApprItemQty;
                            obj.POPendingItemQty = item.POPendingItemQty;
                            obj.FromStoreID = item.FromStoreID;
                            obj.ApplicableOnList = ApplicableOnList;                                                                     //Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                            obj.SelectedApplicableOn = ApplicableOnList.Where(z => z.ID == item.CGSTVatApplicationOn).FirstOrDefault();  //item.ItemVatApplicationOn Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                            //Added By Bhushanp For GST 22062017
                            obj.POSGSTVatType = item.SGSTVatType;
                            obj.POSGSTVatApplicationOn = item.SGSTVatApplicationOn;
                            obj.POCGSTVatType = item.CGSTVatType;
                            obj.POCGSTVatApplicationOn = item.CGSTVatApplicationOn;
                            obj.POIGSTVatType = item.CGSTVatType;
                            obj.POIGSTVatApplicationOn = item.IGSTVatApplicationOn;
                            obj.SGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.SGSTPercent : 0;
                            obj.CGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.CGSTPercent : 0;
                            obj.IGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID != ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.IGSTPercent : 0;
                            obj.HSNCode = item.HSNCodes;
                            obj.TotalBatchAvailableStock = item.TotalBatchAvailableStock;
                            PurchaseOrderItems.Add(obj);
                            strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemId) + ",");
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Item Purchase Requisition combination already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                        msgW1.Show();
                    }
                }

                //if (grdpodetails.ItemsSource == null || ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource)).Count == 0)
                //{
                CheckRateContractForItems(strItemIDs.Trim(','), (grdpodetails.ItemsSource == null || ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource)).Count == 0) ? false : true);
                //}
            }
            grdpodetails.ItemsSource = null;
            grdpodetails.ItemsSource = PurchaseOrderItems;
            grdpodetails.Focus();
            grdpodetails.UpdateLayout();
            CalculateOpeningBalanceSummary();
            grdpodetails.UpdateLayout();
            if (IsAuditTrail)   // By Umesh For Audit Trail
            {
                LogInformation = new LogInfo();
                LogInformation.guid = new Guid();
                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                LogInformation.TimeStamp = DateTime.Now;
                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                LogInformation.Message = " 30 : Item List For PO Against PR " + "\r\n"
                                        + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                        + "Supplier : " + Convert.ToString(((MasterListItem)cboSupplier.SelectedItem).Description) + " "
                                        + "Supplier ID : " + Convert.ToString(((MasterListItem)cboSupplier.SelectedItem).ID) + " ";
                foreach (clsPurchaseOrderDetailVO item in PurchaseOrderItems)
                {
                    LogInformation.Message = LogInformation.Message
                    + "Indent ID : " + Convert.ToString(item.IndentID) + " "
                    + "Indent UnitID : " + Convert.ToString(item.IndentUnitID) + " "
                    + "ItemID : " + Convert.ToString(item.ItemID) + " "
                    + "ItemGroup : " + Convert.ToString(item.ItemGroup) + " "
                    + "Item Category : " + Convert.ToString(item.ItemCategory) + " "
                    + "Item Name : " + Convert.ToString(item.ItemName) + " "
                    + "Item Code : " + Convert.ToString(item.ItemCode) + " "
                    + "Purchase Unit : " + Convert.ToString(item.PUM) + " "
                    + "Purchase UOMID : " + Convert.ToString(item.PUOMID) + " "
                    + "Transaction UOM : " + Convert.ToString(item.TransUOM) + " "
                    + "Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                    + "Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                    + "Base Rate : " + Convert.ToString(item.BaseRate) + " "
                    + "Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                    + "Base Quantity : " + Convert.ToString(item.BaseQuantity) + " "
                    + "Stocking UOM : " + Convert.ToString(item.SUOM) + " "
                    + "Stocking UOMID : " + Convert.ToString(item.SUOMID) + " "
                    + "Main Rate : " + Convert.ToString(item.MainRate) + " "
                    + "Main MRP : " + Convert.ToString(item.MainMRP) + " "
                    + "Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                    + "Base UOMID : " + Convert.ToString(item.BaseUOMID) + " "
                    + "Quantity : " + Convert.ToString(item.Quantity) + " "
                    + "Rate : " + Convert.ToString(item.Rate) + " "
                    + "PR Quantity : " + Convert.ToString(item.PRQuantity) + " "
                    + "Pending Quantity : " + Convert.ToString(item.PendingQuantity) + " "
                    + "PR Pending Quantity : " + Convert.ToString(item.PRPendingQuantity) + " "
                    + "Item VAT Percent : " + Convert.ToString(item.ItemVATPercent) + " "
                    + "PO Item VatType : " + Convert.ToString(item.POItemVatType) + " "
                    + "PO Item Vat ApplicationOn : " + Convert.ToString(item.POItemVatApplicationOn) + " "
                    + "PO Item Other Tax Type : " + Convert.ToString(item.POItemOtherTaxType) + " "
                    + "PO Item Other Tax Application On : " + Convert.ToString(item.POItemOtherTaxApplicationOn) + " "
                    + "VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                    + "MRP : " + Convert.ToString(item.MRP) + " "
                    + "PR Number : " + Convert.ToString(item.IndentNumber) + " "
                    + "Rate Contract ID : " + Convert.ToString(item.RateContractID) + " "
                    + "Rate Contract UnitID : " + Convert.ToString(item.RateContractUnitID) + " "
                    + "Selected Currency : " + Convert.ToString(item.SelectedCurrency.Description) + " "
                    + "Selected Currency ID : " + Convert.ToString(item.SelectedCurrency.ID) + " "
                    + "Cost Rate : " + Convert.ToString(item.CostRate) + " "
                    + "Purchase To BaseCF : " + Convert.ToString(item.PurchaseToBaseCF) + " "
                    + "Stocking ToBaseCF : " + Convert.ToString(item.StockingToBaseCF) + " "
                    + "PO Appr Item Qty : " + Convert.ToString(item.POApprItemQty) + " "
                    + "PO Pending Item Qty : " + Convert.ToString(item.POPendingItemQty) + " "
                    + "From Store ID : " + Convert.ToString(item.FromStoreID) + " "
                    + "\r\n";
                }
                LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                LogInfoList.Add(LogInformation);
            }
        }

        /// <summary>
        /// On View link Click event get handled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkViewContractDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdpodetails.SelectedItem != null)
                {
                    clsPurchaseOrderDetailVO objPODEtails = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;

                    if (lstRateContractItemDetailsNew != null && lstRateContractItemDetailsNew.Count > 0 && lstRateContractItemDetailsNew.Where(PODetails => PODetails.ItemID == objPODEtails.ItemID).ToList().Count > 0)
                    {
                        //Show Rate Contract Details.
                        frmContractsForItems frmContract = new frmContractsForItems();
                        frmContract.POItem = objPODEtails;
                        frmContract.lstRateContractItemDetailsNew = lstRateContractItemDetailsNew.Where(PODetails => PODetails.ItemID == objPODEtails.ItemID).ToList();
                        frmContract.OnOKButton_Click += new RoutedEventHandler(SaveRateContract_Click);
                        frmContract.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Contract not assigned for selected Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                    //if (lstRateContract != null && lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0 && lstRateContract.Count > 0)
                    //{

                    //    //Show Rate Contract Details.
                    //    frmContractsForItems frmContract = new frmContractsForItems();
                    //    frmContract.POItem = objPODEtails;
                    //    frmContract.lstRateContract = lstRateContract;
                    //    frmContract.lstRateContractItemDetails = lstRateContractItemDetails.Where(PODetails => PODetails.ItemID == objPODEtails.ItemID).ToList();
                    //    frmContract.OnOKButton_Click += new RoutedEventHandler(SaveRateContract_Click);
                    //    frmContract.Show();
                    //}
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        /// <summary>
        /// Save the discount for the item after selecting the valid condition.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveRateContract_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clsPurchaseOrderDetailVO objPODetail = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;
                frmContractsForItems frmContract = (frmContractsForItems)sender;
                


                if (frmContract.DialogResult == true && frmContract.dgRateContracts.SelectedItem != null)
                {
                    if (this.ISFromAprovePO == true && this.POApproveLvlID == 2) // Added by Prashant Channe on 26/10/2018, to apply revised rates from Rate Contract while login with level 2 user for PO approval
                    {
                        objPODetail.BaseRate = frmContract.SelectedItemNew.BaseRate;
                        objPODetail.BaseMRP = frmContract.SelectedItemNew.BaseMRP;
                        objPODetail.MainRate = objPODetail.BaseRate;
                        objPODetail.MainMRP = objPODetail.BaseMRP;
                        objPODetail.Rate = frmContract.SelectedItemNew.Rate;
                        objPODetail.MRP = frmContract.SelectedItemNew.MRP;
                        objPODetail.DiscountPercent = frmContract.SelectedItemNew.DiscountPercent;     //Set Property for Discount from Revised Rate Contract on 31/10/2018
                        objPODetail.IsRateContractLinkEnable = false;
                        CalculateOpeningBalanceSummary();

                    }
                    else
                    {

                        objPODetail.RateContractID = frmContract.SelectedItemNew.ContractID;
                        objPODetail.RateContractUnitID = frmContract.SelectedItemNew.ContractUnitId;
                        objPODetail.Quantity = frmContract.SelectedItemNew.Quantity;
                        objPODetail.SelectedUOM = frmContract.SelectedItemNew.SelectedUOM;
                        objPODetail.BaseConversionFactor = frmContract.SelectedItemNew.BaseConversionFactor;
                        objPODetail.BaseUOMID = frmContract.SelectedItemNew.BaseUOMID;
                        objPODetail.ConversionFactor = frmContract.SelectedItemNew.ConversionFactor;  //stock cf
                        objPODetail.SUOMID = frmContract.SelectedItemNew.SUOMID;
                        objPODetail.BaseRate = frmContract.SelectedItemNew.BaseRate;
                        objPODetail.BaseMRP = frmContract.SelectedItemNew.BaseMRP;
                        objPODetail.MainRate = objPODetail.BaseRate;
                        objPODetail.MainMRP = objPODetail.BaseMRP;
                        objPODetail.Rate = frmContract.SelectedItemNew.Rate;
                        objPODetail.MRP = frmContract.SelectedItemNew.MRP;
                        objPODetail.DiscountPercent = frmContract.SelectedItemNew.DiscountPercent;
                        objPODetail.Specification = frmContract.SelectedItemNew.Remarks;
                        CalculateOpeningBalanceSummary();
                    }

                    //ItemWiseRateContracts objItemWiseRateContracts = ((ItemWiseRateContracts)frmContract.dgRateContracts.SelectedItem);
                    //objPODetail.DiscountPercent = Convert.ToDecimal(objItemWiseRateContracts.DiscountPercent);
                    //objPODetail.RateContractID = objItemWiseRateContracts.RateContractID;
                    //objPODetail.RateContractUnitID = objItemWiseRateContracts.RateContractUnitID;
                    //objPODetail.RateContractCondition = objItemWiseRateContracts.Condition;

                    //objPODetail.Rate = Convert.ToDecimal(objItemWiseRateContracts.Rate) * Convert.ToDecimal(objPODetail.BaseConversionFactor);              
                    //objPODetail.BaseRate = Convert.ToSingle(objItemWiseRateContracts.Rate);
                    //objPODetail.MainRate = Convert.ToSingle(objItemWiseRateContracts.Rate);

                    //objPODetail.MRP = Convert.ToDecimal(objItemWiseRateContracts.MRP) * Convert.ToDecimal(objPODetail.BaseConversionFactor); ;                    
                    //objPODetail.BaseMRP = Convert.ToSingle(objItemWiseRateContracts.MRP);
                    //objPODetail.MainMRP = Convert.ToSingle(objItemWiseRateContracts.MRP);

                    //CalculateOpeningBalanceSummary();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillPurchaseOrderDataGrid();
        }
        int ClickedFlag1 = 0;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ISFromAprovePO)
                    cmdSave.IsEnabled = false;
                else
                    cmdSave.IsEnabled = false;
                ClickedFlag1 = ClickedFlag1 + 1;
                if (ClickedFlag1 == 1)
                {
                    bool isValid = true;
                    MessageBoxControl.MessageBoxChildWindow mgbx = null;
                    if (PurchaseOrderItems == null || PurchaseOrderItems.Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, "At least one item is compulsory for saving purchase order", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                        isValid = false;
                        ClickedFlag1 = 0;
                        cmdSave.IsEnabled = true;
                        return;

                    }
                    if (((clsStoreVO)cboStore.SelectedItem) == null || ((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
                    {
                        cboStore.TextBox.SetValidation("Please Select The Store");
                        cboStore.TextBox.RaiseValidationError();
                        cboStore.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        cmdSave.IsEnabled = true;
                        return;

                    }
                    else
                        cboStore.TextBox.ClearValidationError();

                    if (((clsStoreVO)cboDelivery.SelectedItem) == null || ((clsStoreVO)cboDelivery.SelectedItem).StoreId == 0)
                    {
                        cboDelivery.TextBox.SetValidation("Please Select The Delivery Location");
                        cboDelivery.TextBox.RaiseValidationError();
                        cboDelivery.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        cmdSave.IsEnabled = true;
                        return;

                    }
                    else
                        cboDelivery.TextBox.ClearValidationError();


                    if (((MasterListItem)cboSupplier.SelectedItem) == null || ((MasterListItem)cboSupplier.SelectedItem).ID == 0)
                    {
                        cboSupplier.TextBox.SetValidation("Please Select The Supplier");
                        cboSupplier.TextBox.RaiseValidationError();
                        cboSupplier.Focus();
                        isValid = false;
                        ClickedFlag1 = 0;
                        cmdSave.IsEnabled = true;
                        return;
                    }
                    else
                        cboSupplier.TextBox.ClearValidationError();
                    if (PurchaseOrderItems.Where(z => z.SelectedCurrency == null).Any())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select Currency.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        isValid = false;
                        ClickedFlag1 = 0;
                        cmdSave.IsEnabled = true;
                        return;
                    }
                    if (PurchaseOrderItems.GroupBy(z => z.SelectedCurrency.ID).Count() > 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Currency must be same for all items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        isValid = false;
                        ClickedFlag1 = 0;
                        cmdSave.IsEnabled = true;
                        return;
                    }
                    foreach (var item in PurchaseOrderItems)
                    {
                        if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                        {
                            if (item.MRP < item.Rate)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW4.Show();
                                isValid = false;
                                ClickedFlag1 = 0;
                                cmdSave.IsEnabled = true;
                                return;
                            }
                        }
                    }
                    if (PurchaseOrderItems != null && PurchaseOrderItems.Count > 0)
                    {
                        List<clsPurchaseOrderDetailVO> objList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        if (objList != null && objList.Count > 0)
                        {
                            foreach (var item in objList)
                            {
                                if (item.Quantity == 0)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    ClickedFlag1 = 0;
                                    cmdSave.IsEnabled = true;
                                    return;
                                    break;
                                }
                                if (item.DiscountPercent > 100)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Discount percent must not be greater than 100.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    ClickedFlag1 = 0;
                                    cmdSave.IsEnabled = true;
                                    return;
                                    break;
                                }
                            }
                        }
                    }
                    //Added By CDS
                    if (PurchaseOrderItems != null && PurchaseOrderItems.Count > 0)
                    {
                        List<clsPurchaseOrderDetailVO> objList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        if (objList != null && objList.Count > 0)
                        {
                            foreach (var item in objList)
                            {
                                if (item.Rate == 0)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Purchase Cost Rate In The List Can't Be Zero. Please Enter Purchase Cost Rate Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    ClickedFlag1 = 0;
                                    cmdSave.IsEnabled = true;
                                    return;
                                    break;
                                }
                                if (item.MRP == 0)
                                {
                                    mgbx = new MessageBoxControl.MessageBoxChildWindow(msgTitle, "MPR In The List Can't Be Zero. Please Enter MPR Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    mgbx.Show();
                                    isValid = false;
                                    ClickedFlag1 = 0;
                                    cmdSave.IsEnabled = true;
                                    return;
                                    break;
                                }
                            }
                        }
                    }


                    // Added By CDS 4/02/2016
                    // Validation For PR Quantiy By CDS On Save/Modify/ChangeAndApprovePO
                    if (PurchaseOrderItems != null && PurchaseOrderItems.Count > 0)
                    {
                        List<clsPurchaseOrderDetailVO> objList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        if (objList != null && objList.Count > 0)
                        {
                            foreach (var item in objList)
                            {
                                float fltOne = 1;
                                float fltZero = 0;
                                float Infinity = fltOne / fltZero;
                                float NaN = fltZero / fltZero;
                                if (item.ConversionFactor <= 0 || item.ConversionFactor == Infinity || item.ConversionFactor == NaN)
                                {
                                    ClickedFlag1 = 0;
                                    MessageBoxControl.MessageBoxChildWindow mgbx1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    mgbx1.Show();
                                    cmdSave.IsEnabled = true;
                                    return;
                                }
                                else if (item.BaseConversionFactor <= 0 || item.BaseConversionFactor == Infinity || item.BaseConversionFactor == NaN)
                                {
                                    ClickedFlag1 = 0;
                                    MessageBoxControl.MessageBoxChildWindow mgbx1 = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    mgbx1.Show();
                                    cmdSave.IsEnabled = true;
                                    return;
                                }

                                if (item.POPendingItemQty > 0)  //&& item.POApprItemQty==0
                                {
                                    if ((item.PendingQuantity < ((Convert.ToDecimal(item.POPendingItemQty) - Convert.ToDecimal(item.PODetailsViewTimeQty)) + (item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)))) && rdbDirect.IsChecked == false && ISEditMode == true)
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }
                                    if (item.POApprItemQty > 0 && (((item.PendingQuantity) - Convert.ToDecimal(item.POPendingItemQty)) < (item.Quantity * Convert.ToDecimal(item.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == false)
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }
                                    else if ((((item.PendingQuantity + Convert.ToDecimal(item.POApprItemQty)) - Convert.ToDecimal(item.POPendingItemQty)) < (item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) && rdbDirect.IsChecked == false && ISEditMode == false))
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToDecimal(item.POApprItemQty) > 0 && (item.PendingQuantity < ((item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) + Convert.ToDecimal(item.POApprItemQty))) && (((item.PendingQuantity + Convert.ToDecimal(item.POApprItemQty))) < (item.Quantity * Convert.ToDecimal(item.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == true)
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }
                                    else if ((item.PendingQuantity < ((item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)))) && (((item.PendingQuantity + Convert.ToDecimal(item.POApprItemQty))) < (item.Quantity * Convert.ToDecimal(item.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == true)
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }

                                    if (Convert.ToDecimal(item.POApprItemQty) > 0 && ((item.PendingQuantity) < item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) && rdbDirect.IsChecked == false && ISEditMode == false)
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }
                                    else if ((((item.PendingQuantity)) < (item.Quantity * Convert.ToDecimal(item.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == false)
                                    {
                                        ClickedFlag1 = 0;
                                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        msgW3.Show();
                                        item.Quantity = 0;
                                        item.PendingQuantity = item.PendingQuantity;
                                        cmdSave.IsEnabled = true;
                                        return;
                                    }
                                }

                            }
                        }
                    }
                    //END
                    // Validation For Pending Quantiy By CDS On Save/Modify/ChangeAndApprovePO
                    if (PurchaseOrderItems != null && PurchaseOrderItems.Count > 0)
                    {
                        List<clsPurchaseOrderDetailVO> objList = PurchaseOrderItems.ToList<clsPurchaseOrderDetailVO>();
                        if (objList != null && objList.Count > 0)
                        {
                            foreach (var item in objList)
                            {
                                if ((item.PendingQuantity < item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) && rdbDirect.IsChecked == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();

                                    item.Quantity = 0;
                                    item.PRPendingQuantity = Convert.ToDouble(item.PendingQuantity - item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                                    CalculateOpeningBalanceSummary();

                                    isValid = false;
                                    ClickedFlag1 = 0;
                                    cmdSave.IsEnabled = true;
                                    return;
                                }
                                else
                                {
                                    if (rdbDirect.IsChecked != true)
                                    {
                                        item.PRPendingQuantity = Convert.ToDouble(item.PendingQuantity - item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                                    }
                                }
                            }
                        }
                    }
                    //End
                    else
                        isValid = true;
                    if (isValid)
                    {
                        msgTitle = "Palash";

                        if (ISEditMode == false)
                        {
                            msgText = "Are you sure you want to Save Purchase Order?";
                        }
                        else if (ISEditMode == true)
                        {
                            if (ISEditAndAprove == true)
                            {
                                if (this.IsForDirectApprove)  // added by Ashish Z on 280716 for Direct Approve from PO Approval Form
                                    msgText = "Are you sure you want to Approve Purchase Order ?";
                                else
                                    msgText = "Are you sure you want to Modify And Update Purchase Order ?";
                            }
                            else
                            {
                                msgText = "Are you sure you want to Modify And Update Purchase Order?";
                            }
                        }
                        MessageBoxControl.MessageBoxChildWindow msgWin =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                        msgWin.Show();
                    }
                    else
                        ClickedFlag1 = 0;
                }
                else
                    ClickedFlag1 = 0;
            }
            catch (Exception Ex)
            {
                ClickedFlag1 = 0;
                throw;
            }
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SavePO();
            }
            else if (result == MessageBoxResult.No)
            {
                ClickedFlag1 = 0;
                if (this.ISFromAprovePO)
                    cmdSave.IsEnabled = true;
                else
                    cmdSave.IsEnabled = true;
            }
        }

        private void SavePO()
        {

            #region PO Update And Approve From Purchase Order
            if (ISFromAprovePO == true)    // PO Update And Aproved From Purchase Order 
            {
                clsAddPurchaseOrderBizActionVO objBizActionVO = new clsAddPurchaseOrderBizActionVO();
                ObservableCollection<clsPurchaseOrderDetailVO> objObservable = ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource));

                #region Test
                IApplicationConfiguration applicationConfiguration = ((IApplicationConfiguration)App.Current);
                ObservableCollection<clsPurchaseOrderDetailVO> ocPOItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                foreach (clsPurchaseOrderDetailVO item in objObservable)
                {
                    if (ocPOItems.Count > 0 && ocPOItems.Where(poItems => poItems.ItemID == item.ItemID && poItems.IndentID != item.IndentID).Any() == true)
                    {
                        clsPurchaseOrderDetailVO obj = ocPOItems.Where(poItems => poItems.ItemID == item.ItemID && poItems.IndentID != item.IndentID).ToList().First();
                        obj.Quantity += Convert.ToDecimal(item.Quantity);
                    }
                    else
                    {
                        ocPOItems.Add(new clsPurchaseOrderDetailVO { IndentID = Convert.ToInt64(item.IndentID), IndentUnitID = Convert.ToInt64(item.IndentUnitID), ItemID = item.ItemID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.Quantity), Rate = (decimal)item.Rate, VATPercent = (decimal)item.VATPer, MRP = (decimal)item.MRP, IndentNumber = item.IndentNumber });
                    }
                    // if same indent item is get added
                }
                #endregion Test

                objBizActionVO.PurchaseOrder = new clsPurchaseOrderVO();
                objBizActionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PurchaseOrder.Date = dtpDate.SelectedDate.Value;
                objBizActionVO.PurchaseOrder.Time = DateTime.Now;
                objBizActionVO.PurchaseOrder.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                objBizActionVO.PurchaseOrder.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
                objBizActionVO.IsEditMode = ISEditMode;
                objBizActionVO.PurchaseOrder.EditForApprove = true;
                objBizActionVO.PurchaseOrder.ID = ((clsPurchaseOrderVO)this.DataContext).ID;
                if (enmPurchaseOorder == PurchaseOrderSource.Indent)
                {
                    objBizActionVO.PurchaseOrder.EnquiryID = 0;
                }
                else if (enmPurchaseOorder == PurchaseOrderSource.Quotation)
                {
                    objBizActionVO.PurchaseOrder.IndentID = 0;
                    objBizActionVO.PurchaseOrder.EnquiryID = _EnquiryID;
                }

                if (rdbDirect.IsChecked == true)
                {
                    objBizActionVO.PurchaseOrder.Direct = true;
                }
                objBizActionVO.PurchaseOrder.DeliveryDuration = ((clsStoreVO)cboDelivery.SelectedItem).StoreId;
                objBizActionVO.PurchaseOrder.PaymentModeID = ((MasterListItem)cmbModeofPayment.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.PaymentTerms = ((MasterListItem)cboTOP.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.DeliveryDays = ((MasterListItem)cmbDeliveryDays.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.Guarantee_Warrantee = ((clsPurchaseOrderVO)this.DataContext).Guarantee_Warrantee;
                objBizActionVO.PurchaseOrder.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.Remarks = ((clsPurchaseOrderVO)this.DataContext).Remarks;
                objBizActionVO.PurchaseOrder.TotalAmount = ((clsPurchaseOrderVO)this.DataContext).TotalAmount;
                objBizActionVO.PurchaseOrder.TotalDiscount = ((clsPurchaseOrderVO)this.DataContext).TotalDiscount;
                objBizActionVO.PurchaseOrder.TotalVAT = ((clsPurchaseOrderVO)this.DataContext).TotalVAT;
                //Added By Bhushanp 22062017
                objBizActionVO.PurchaseOrder.TotalSGST = ((clsPurchaseOrderVO)this.DataContext).TotalSGST;
                objBizActionVO.PurchaseOrder.TotalCGST = ((clsPurchaseOrderVO)this.DataContext).TotalCGST;
                objBizActionVO.PurchaseOrder.TotalIGST = ((clsPurchaseOrderVO)this.DataContext).TotalIGST;
                // Added By CDS 
                objBizActionVO.PurchaseOrder.OtherCharges = ((clsPurchaseOrderVO)this.DataContext).OtherCharges;
                objBizActionVO.PurchaseOrder.PODiscount = ((clsPurchaseOrderVO)this.DataContext).PODiscount;
                objBizActionVO.PurchaseOrder.PrevTotalNet = ((clsPurchaseOrderVO)this.DataContext).PrevTotalNet;
                objBizActionVO.PurchaseOrder.TotalNet = ((clsPurchaseOrderVO)this.DataContext).TotalNet;
                objBizActionVO.PurchaseOrder.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.PurchaseOrder.AddedDateTime = DateTime.Now;
                objBizActionVO.PurchaseOrder.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objBizActionVO.PurchaseOrder.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                objBizActionVO.PurchaseOrder.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PurchaseOrder.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.PurchaseOrder.UpdatedDateTime = DateTime.Now;
                objBizActionVO.PurchaseOrder.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objBizActionVO.PurchaseOrder.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                objBizActionVO.PurchaseOrder.UpdatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PurchaseOrder.Items = objObservable.ToList<clsPurchaseOrderDetailVO>();
                objBizActionVO.PurchaseOrder.OrgItems = objObservable.ToList<clsPurchaseOrderDetailVO>().DeepCopy();
                var res = objBizActionVO.PurchaseOrder.Items.GroupBy(m => m.ItemID)
                   .Select(g => g.OrderByDescending(x => x.Quantity)
                           .First())
                   .ToList().DeepCopy();

                foreach (clsPurchaseOrderDetailVO item in objBizActionVO.PurchaseOrder.Items)
                {
                    var ItemCnt = (from r in objBizActionVO.PurchaseOrder.Items
                                   where (r.ItemID == item.ItemID)
                                   select r).ToList().Count;

                    foreach (clsPurchaseOrderDetailVO item1 in res.ToList())
                    {
                        if (ItemCnt == 1)
                        {
                            if (item.ItemID == item1.ItemID)
                            {
                                item1.TotalQty += item.Quantity;
                                item1.Quantity = item1.TotalQty;
                            }
                        }
                        else
                        {
                            if (item.ItemID == item1.ItemID)
                            {
                                item1.TotalQty += item.Quantity * Convert.ToDecimal(item.BaseConversionFactor);
                                item1.Quantity = item1.TotalQty;
                                item1.SinleLineItem = true;
                                item1.PurchaseToBaseCF = item.PurchaseToBaseCF;
                                item1.StockingToBaseCF = item.StockingToBaseCF;
                                item1.SelectedUOM.ID = item.POPUOMID;
                                item.SinleLineItem = true;
                                //** Added By Ashish Z on Dated 10082016 for Putting the Rate,MRP(Purchase UOM wise) in T_PurchaseOrderItems table
                                item1.Rate = Convert.ToDecimal(item.BaseRate * item1.PurchaseToBaseCF);
                                item1.MRP = Convert.ToDecimal(item.BaseMRP * item1.PurchaseToBaseCF);
                                //**
                            }
                        }
                    }

                    //to set edited Cost Price & MRP in PO - Indent List
                    foreach (clsPurchaseOrderDetailVO POIndentItem in POIndentdetails)
                    {
                        if (item.IndentID == POIndentItem.IndentID && item.IndentUnitID == POIndentItem.IndentUnitID && item.IndentDetailID == POIndentItem.IndentDetailID && item.IndentDetailUnitID == POIndentItem.IndentDetailUnitID && item.ItemID == POIndentItem.ItemID)
                        {
                            POIndentItem.BaseRate = item.BaseRate;
                            POIndentItem.BaseMRP = item.BaseMRP;
                            POIndentItem.MainRate = item.MainRate;
                            POIndentItem.MainMRP = item.MainMRP;
                            POIndentItem.Rate = item.Rate;
                            POIndentItem.MRP = item.MRP;
                        }
                    }
                }
                objBizActionVO.PurchaseOrder.Items = res.ToList();
                objBizActionVO.POIndentList = POIndentdetails.ToList();
                objBizActionVO.POTerms = new List<clsPurchaseOrderTerms>();
                foreach (var item in PurchaseOrderTerms)
                {
                    if (item.Status == true && item.ID != 0)
                    {
                        objBizActionVO.POTerms.Add(new clsPurchaseOrderTerms { TermsAndConditionID = item.ID, Status = item.Status });
                    }
                }
                if (objBizActionVO.PurchaseOrder.Items.Count > 0)
                {
                    WaitIndicator indicator = new WaitIndicator();
                    indicator.Show();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag1 = 0;
                        if (arg.Error == null && ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder != null)
                        {
                            if (arg.Result != null)
                            {

                                #region Only For Approval
                                IDS = new System.Collections.Generic.List<long>();
                                IDS.Add(SelectedPO.ID);
                                if (IDS.Count > 0)
                                {
                                    clsUpdatePurchaseOrderForApproval bizactionVO = new clsUpdatePurchaseOrderForApproval();
                                    bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
                                    Approved = true;
                                    bizactionVO.PurchaseOrder.Freezed = SelectedPO.IsApproveded == true ? true : false;
                                    try
                                    {
                                        bizactionVO.PurchaseOrder.ids = IDS;
                                        bizactionVO.PurchaseOrder.UnitId = SelectedPO.UnitId;
                                        bizactionVO.PurchaseOrder.POApproveLvlID = this.POApproveLvlID;
                                        bizactionVO.PurchaseOrder.ApprovedBy = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
                                        bizactionVO.PurchaseOrder.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                        bizactionVO.PurchaseOrder.Items = PurchaseOrderItems.ToList();
                                        foreach (clsPurchaseOrderDetailVO item1 in PurchaseOrderItems)
                                        {
                                            foreach (clsPurchaseOrderDetailVO item2 in POIndentdetails)
                                            {
                                                if (item2.ItemID == item1.ItemID && item2.IndentID == item1.IndentID && item2.IndentUnitID == item1.IndentUnitID && item2.IndentDetailID == item1.IndentDetailID && item2.IndentDetailUnitID == item1.IndentDetailUnitID)
                                                {
                                                    item2.Quantity = item1.Quantity;
                                                    item2.BaseConversionFactor = item1.BaseConversionFactor;
                                                }
                                            }
                                        }
                                        //Added by Ashish Z. on Dated 181116 for Updating the PR Qty. after changing the PODetailID.
                                        if (((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder != null)
                                        {
                                            foreach (var item in ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder.OrgItems.ToList())
                                            {
                                                foreach (clsPurchaseOrderDetailVO item2 in POIndentdetails.ToList())
                                                {
                                                    if (item2.ItemID == item.ItemID && item2.IndentID == item.IndentID && item2.IndentUnitID == item.IndentUnitID
                                                        && item2.IndentDetailID == item.IndentDetailID && item2.IndentDetailUnitID == item.IndentDetailUnitID)
                                                    {
                                                        item2.POID = item.POID;
                                                        item2.PoDetailsID = item.PoDetailsID;
                                                        item2.PoDetailsUnitID = item.PoDetailsUnitID;
                                                    }
                                                }
                                            }
                                        }
                                        //End

                                        bizactionVO.POIndentList = POIndentdetails.ToList();
                                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                        client.ProcessCompleted += (s1, args1) =>
                                        {
                                            if (args1.Error == null && args1.Result != null)
                                            {
                                                if (dgPOList.Columns[8].IsReadOnly)
                                                {
                                                    dgPOList.Columns[8].IsReadOnly = true;
                                                }
                                                if (ISEditMode == false)
                                                    msgText = "Purchase Order Saved Successfully With PO Number";
                                                else if (ISEditMode == true)
                                                    msgText = "Purchase Order Approve And Updated Successfully With PO Number";
                                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText + " " + ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder.PONO, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                SetCommandButtonState("Save");
                                                msgW1.Show();
                                                PurchaseOrderItems.Clear();
                                                POIndentdetails.Clear();
                                                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                                                frmApprovePO AprvPO = new frmApprovePO();
                                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                                mElement.Text = "Approve Purchase Order";
                                                ((IApplicationConfiguration)App.Current).OpenMainContent(AprvPO);
                                            }
                                        };
                                        client.ProcessAsync(bizactionVO, new clsUserVO());
                                        client.CloseAsync();
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                #endregion   //Only For Approval
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    indicator.Close();
                    Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }  // End PO Update And Aproved
            #endregion
            else   // Add Or Update PO Only From This Form 
            {
                clsAddPurchaseOrderBizActionVO objBizActionVO = new clsAddPurchaseOrderBizActionVO();
                ObservableCollection<clsPurchaseOrderDetailVO> objObservable = ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource));
                #region Test
                IApplicationConfiguration applicationConfiguration = ((IApplicationConfiguration)App.Current);
                ObservableCollection<clsPurchaseOrderDetailVO> ocPOItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                foreach (clsPurchaseOrderDetailVO item in objObservable)
                {
                    if (ocPOItems.Count > 0 && ocPOItems.Where(poItems => poItems.ItemID == item.ItemID && poItems.IndentID != item.IndentID).Any() == true)
                    {
                        clsPurchaseOrderDetailVO obj = ocPOItems.Where(poItems => poItems.ItemID == item.ItemID && poItems.IndentID != item.IndentID).ToList().First();
                        obj.Quantity += Convert.ToDecimal(item.Quantity);
                    }
                    else
                    {
                        ocPOItems.Add(new clsPurchaseOrderDetailVO { IndentID = Convert.ToInt64(item.IndentID), IndentUnitID = Convert.ToInt64(item.IndentUnitID), ItemID = item.ItemID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUM, Quantity = Convert.ToDecimal(item.Quantity), Rate = (decimal)item.Rate, VATPercent = (decimal)item.VATPer, MRP = (decimal)item.MRP, IndentNumber = item.IndentNumber });
                    }
                    // if same indent item is get added
                }
                #endregion Test

                objBizActionVO.PurchaseOrder = new clsPurchaseOrderVO();
                objBizActionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PurchaseOrder.Date = dtpDate.SelectedDate.Value;
                objBizActionVO.PurchaseOrder.Time = DateTime.Now;
                objBizActionVO.PurchaseOrder.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                objBizActionVO.PurchaseOrder.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
                objBizActionVO.IsEditMode = ISEditMode;
                objBizActionVO.PurchaseOrder.EditForApprove = false;
                objBizActionVO.PurchaseOrder.ID = ((clsPurchaseOrderVO)this.DataContext).ID;
                if (enmPurchaseOorder == PurchaseOrderSource.Indent)
                {
                    objBizActionVO.PurchaseOrder.EnquiryID = 0;
                }
                else if (enmPurchaseOorder == PurchaseOrderSource.Quotation)
                {
                    objBizActionVO.PurchaseOrder.IndentID = 0;
                    objBizActionVO.PurchaseOrder.EnquiryID = _EnquiryID;
                }

                if (rdbDirect.IsChecked == true)
                {
                    objBizActionVO.PurchaseOrder.Direct = true;
                }

                objBizActionVO.PurchaseOrder.DeliveryDuration = ((clsStoreVO)cboDelivery.SelectedItem).StoreId;
                objBizActionVO.PurchaseOrder.PaymentModeID = ((MasterListItem)cmbModeofPayment.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.PaymentTerms = ((MasterListItem)cboTOP.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.DeliveryDays = ((MasterListItem)cmbDeliveryDays.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.Guarantee_Warrantee = ((clsPurchaseOrderVO)this.DataContext).Guarantee_Warrantee;
                objBizActionVO.PurchaseOrder.Schedule = ((MasterListItem)cmbSchedule.SelectedItem).ID;
                objBizActionVO.PurchaseOrder.Remarks = ((clsPurchaseOrderVO)this.DataContext).Remarks;
                objBizActionVO.PurchaseOrder.TotalAmount = ((clsPurchaseOrderVO)this.DataContext).TotalAmount;
                objBizActionVO.PurchaseOrder.TotalDiscount = ((clsPurchaseOrderVO)this.DataContext).TotalDiscount;
                objBizActionVO.PurchaseOrder.TotalVAT = ((clsPurchaseOrderVO)this.DataContext).TotalVAT;
                //Added By Bhushanp 22062017
                objBizActionVO.PurchaseOrder.TotalSGST = ((clsPurchaseOrderVO)this.DataContext).TotalSGST;
                objBizActionVO.PurchaseOrder.TotalCGST = ((clsPurchaseOrderVO)this.DataContext).TotalCGST;
                objBizActionVO.PurchaseOrder.TotalIGST = ((clsPurchaseOrderVO)this.DataContext).TotalIGST;
                // Added By CDS 
                objBizActionVO.PurchaseOrder.OtherCharges = ((clsPurchaseOrderVO)this.DataContext).OtherCharges;
                objBizActionVO.PurchaseOrder.PODiscount = ((clsPurchaseOrderVO)this.DataContext).PODiscount;
                objBizActionVO.PurchaseOrder.PrevTotalNet = ((clsPurchaseOrderVO)this.DataContext).PrevTotalNet;
                //
                objBizActionVO.PurchaseOrder.TotalNet = ((clsPurchaseOrderVO)this.DataContext).TotalNet;
                objBizActionVO.PurchaseOrder.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.PurchaseOrder.AddedDateTime = DateTime.Now;
                objBizActionVO.PurchaseOrder.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objBizActionVO.PurchaseOrder.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                objBizActionVO.PurchaseOrder.CreatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PurchaseOrder.UpdatedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                objBizActionVO.PurchaseOrder.UpdatedDateTime = DateTime.Now;
                objBizActionVO.PurchaseOrder.UpdatedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                objBizActionVO.PurchaseOrder.UpdatedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                objBizActionVO.PurchaseOrder.UpdatedUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PurchaseOrder.Items = objObservable.ToList<clsPurchaseOrderDetailVO>();
                objBizActionVO.PurchaseOrder.OrgItems = objObservable.ToList<clsPurchaseOrderDetailVO>().DeepCopy();
                var res = objBizActionVO.PurchaseOrder.Items.GroupBy(m => m.ItemID)
                   .Select(g => g.OrderByDescending(x => x.Quantity)
                           .First())
                   .ToList().DeepCopy();

                foreach (clsPurchaseOrderDetailVO item in objBizActionVO.PurchaseOrder.Items)
                {
                    var ItemCnt = (from r in objBizActionVO.PurchaseOrder.Items
                                   where (r.ItemID == item.ItemID)
                                   select r).ToList().Count;

                    foreach (clsPurchaseOrderDetailVO item1 in res.ToList())
                    {
                        if (ItemCnt == 1)
                        {
                            if (item.ItemID == item1.ItemID)
                            {
                                item1.TotalQty += item.Quantity;
                                item1.Quantity = item1.TotalQty;
                            }
                        }
                        else
                        {
                            if (item.ItemID == item1.ItemID)
                            {
                                item1.TotalQty += item.Quantity * Convert.ToDecimal(item.BaseConversionFactor);
                                item1.Quantity = item1.TotalQty;
                                item1.SinleLineItem = true;
                                item1.PurchaseToBaseCF = item.PurchaseToBaseCF;
                                item1.StockingToBaseCF = item.StockingToBaseCF;
                                item1.SelectedUOM.ID = item.POPUOMID;
                                item.SinleLineItem = true;
                                //** Added By Ashish Z on Dated 10082016 for Putting the Rate,MRP(Purchase UOM wise) in T_PurchaseOrderItems table
                                item1.Rate = Convert.ToDecimal(item.BaseRate * item1.PurchaseToBaseCF);
                                item1.MRP = Convert.ToDecimal(item.BaseMRP * item1.PurchaseToBaseCF);
                                //**
                            }
                        }
                    }

                    //to set edited Cost Price & MRP in PO - Indent List
                    foreach (clsPurchaseOrderDetailVO POIndentItem in POIndentdetails)
                    {
                        if (item.IndentID == POIndentItem.IndentID && item.IndentUnitID == POIndentItem.IndentUnitID && item.IndentDetailID == POIndentItem.IndentDetailID && item.IndentDetailUnitID == POIndentItem.IndentDetailUnitID && item.ItemID == POIndentItem.ItemID)
                        {
                            POIndentItem.BaseRate = item.BaseRate;
                            POIndentItem.BaseMRP = item.BaseMRP;
                            POIndentItem.MainRate = item.MainRate;
                            POIndentItem.MainMRP = item.MainMRP;
                            POIndentItem.Rate = item.Rate;
                            POIndentItem.MRP = item.MRP;
                        }
                    }
                }

                objBizActionVO.PurchaseOrder.Items = res.ToList();
                objBizActionVO.POIndentList = POIndentdetails.ToList();
                objBizActionVO.POTerms = new List<clsPurchaseOrderTerms>();
                foreach (var item in PurchaseOrderTerms)
                {
                    if (item.Status == true && item.ID != 0)
                    {
                        objBizActionVO.POTerms.Add(new clsPurchaseOrderTerms { TermsAndConditionID = item.ID, Status = item.Status });
                    }
                }
                if (IsAuditTrail)   // By Umesh For Audit Trail
                {
                    LogInformation = new LogInfo();
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 38 : Final Data To Save PO " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                            + "Date : " + Convert.ToString(objBizActionVO.PurchaseOrder.Date) + " "
                                            + "Store ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.StoreID) + " "
                                            + "Supplier ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.SupplierID) + " "
                                            + "IsEdit Mode : " + Convert.ToString(objBizActionVO.IsEditMode) + " "
                                            + "Edit For Approve : " + Convert.ToString(objBizActionVO.PurchaseOrder.EditForApprove) + " "
                                            + "ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.ID) + " "
                                            + "Enquiry ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.EnquiryID) + " "
                                            + "Direct : " + Convert.ToString(objBizActionVO.PurchaseOrder.Direct) + " "
                                            + "Delivery Location : " + Convert.ToString(objBizActionVO.PurchaseOrder.DeliveryDuration) + " "
                                            + "Payment Mode ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.PaymentModeID) + " "
                                            + "Payment Terms : " + Convert.ToString(objBizActionVO.PurchaseOrder.PaymentTerms) + " "
                                            + "Delivery Days : " + Convert.ToString(objBizActionVO.PurchaseOrder.DeliveryDays) + " "
                                            + "Guarantee_Warrantee : " + Convert.ToString(objBizActionVO.PurchaseOrder.Guarantee_Warrantee) + " "
                                            + "Schedule : " + Convert.ToString(objBizActionVO.PurchaseOrder.Schedule) + " "
                                            + "Remarks : " + Convert.ToString(objBizActionVO.PurchaseOrder.Remarks) + " "
                                            + "Total Amount : " + Convert.ToString(objBizActionVO.PurchaseOrder.TotalAmount) + " "
                                            + "Total Discount : " + Convert.ToString(objBizActionVO.PurchaseOrder.TotalDiscount) + " "
                                            + "Total VAT : " + Convert.ToString(objBizActionVO.PurchaseOrder.TotalVAT) + " "
                                            + "Other Charges : " + Convert.ToString(objBizActionVO.PurchaseOrder.OtherCharges) + " "
                                            + "PO Discount : " + Convert.ToString(objBizActionVO.PurchaseOrder.PODiscount) + " "
                                            + "Prev Total Net : " + Convert.ToString(objBizActionVO.PurchaseOrder.PrevTotalNet) + " "
                                            + "Total Net : " + Convert.ToString(objBizActionVO.PurchaseOrder.TotalNet) + " "
                                            + "Added DateTime : " + Convert.ToString(objBizActionVO.PurchaseOrder.AddedDateTime) + "\r\n";
                    foreach (clsPurchaseOrderDetailVO item in objBizActionVO.PurchaseOrder.Items)
                    {
                        if (item.SinleLineItem == true)
                        {
                            LogInformation.Message = LogInformation.Message
                                + "Item ID : " + Convert.ToString(item.ItemID) + " "
                                + "Quantity : " + Convert.ToString(item.Quantity) + " "
                                + "Amount : " + Convert.ToString(item.Amount / Convert.ToDecimal(item.PurchaseToBaseCF)) + " "
                                + "Discount Percent : " + Convert.ToString(item.DiscountPercent) + " "
                                + "DiscountAmount : " + Convert.ToString(item.DiscountAmount / Convert.ToDecimal(item.PurchaseToBaseCF)) + " "
                                + "VatPercent : " + Convert.ToString(item.VATPercent) + " "
                                + "VAT Amount : " + Convert.ToString(item.VATAmount / Convert.ToDecimal(item.PurchaseToBaseCF)) + " "
                                + "Item tax : " + Convert.ToString(item.ItemVATPercent) + " "
                                + "Item Tax Amount : " + Convert.ToString(item.ItemVATAmount / Convert.ToDecimal(item.PurchaseToBaseCF)) + " "
                                + "Net Amount : " + Convert.ToString(item.NetAmount / Convert.ToDecimal(item.PurchaseToBaseCF)) + " "
                                + "Input Transaction Quantity : " + Convert.ToString(item.Quantity / Convert.ToDecimal(item.PurchaseToBaseCF)) + " "
                                + "BaseCF : " + Convert.ToString(item.PurchaseToBaseCF) + " "
                                + "Base Quantity : " + Convert.ToString(item.Quantity) + " "
                                + "StockCF : " + Convert.ToString(item.StockingToBaseCF) + " "
                                + "Stocking Quantity : " + Convert.ToString(Convert.ToSingle(item.Quantity / Convert.ToDecimal(item.PurchaseToBaseCF)) * item.StockingToBaseCF) + "\r\n"
                                ;
                        }
                        else
                        {
                            LogInformation.Message = LogInformation.Message
                                + "Item ID : " + Convert.ToString(item.ItemID) + " "
                                + "Quantity : " + Convert.ToString(item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) + " "
                                + "Amount : " + Convert.ToString(item.Amount) + " "
                                + "Discount Percent : " + Convert.ToString(item.DiscountPercent) + " "
                                + "DiscountAmount : " + Convert.ToString(item.DiscountAmount) + " "
                                + "VatPercent : " + Convert.ToString(item.VATPercent) + " "
                                + "VAT Amount : " + Convert.ToString(item.VATAmount) + " "
                                + "Item tax : " + Convert.ToString(item.ItemVATPercent) + " "
                                + "Item Tax Amount : " + Convert.ToString(item.ItemVATAmount) + " "
                                + "Net Amount : " + Convert.ToString(item.NetAmount) + " "
                                + "Input Transaction Quantity : " + Convert.ToString(item.Quantity) + " "
                                + "BaseCF : " + Convert.ToString(item.BaseConversionFactor) + " "
                                + "Base Quantity : " + Convert.ToString(item.Quantity * Convert.ToDecimal(item.BaseConversionFactor)) + " "
                                + "StockCF : " + Convert.ToString(item.ConversionFactor) + " "
                                + "Stocking Quantity : " + Convert.ToString(Convert.ToSingle(item.Quantity) * item.ConversionFactor) + "\r\n"
                                ;
                        }
                        LogInformation.Message = LogInformation.Message
                                + "Rate : " + Convert.ToString(item.Rate) + " "
                                + "MRP : " + Convert.ToString(item.MRP) + " "
                                + "Transaction UOMID : " + Convert.ToString(item.SelectedUOM.ID) + " "
                                + "Base UMID : " + Convert.ToString(item.BaseUOMID) + " "
                                + "Stock UOMID : " + Convert.ToString(item.SUOMID) + " "
                                + "Tax type : " + Convert.ToString(item.POItemVatType) + " "
                                + "Vat Applicableon : " + Convert.ToString(item.POItemVatApplicationOn) + " "
                                + "other Tax Type : " + Convert.ToString(item.POItemOtherTaxType) + " "
                                + "other tax Applicableon : " + Convert.ToString(item.POItemOtherTaxApplicationOn) + " "
                                + "Remarks : " + Convert.ToString(item.Specification) + " "
                                + "Edit For Approve : " + Convert.ToString(item.SUOMID) + " "
                                + "Tax type : " + Convert.ToString(item.POItemVatType) + " "
                                + "Currency ID : " + Convert.ToString(item.SelectedCurrency.ID) + "\r\n"
                                ;
                        if (item.ConditionFound)
                        {
                            LogInformation.Message = LogInformation.Message
                                + "Rate Contract ID : " + Convert.ToString(item.RateContractID) + " "
                                + "Rate Contract UnitID : " + Convert.ToString(item.RateContractUnitID) + " "
                                + "Rate Contract Condition : " + Convert.ToString(item.RateContractCondition) + "\r\n"
                                ;
                        }
                        else
                        {
                            LogInformation.Message = LogInformation.Message
                                + "Rate Contract ID : " + Convert.ToString(0) + " "
                                + "Rate Contract UnitID : " + Convert.ToString(0) + " "
                                + "Rate Contract Condition : " + Convert.ToString("") + "\r\n"
                                ;
                        }
                        foreach (clsPurchaseOrderTerms term in objBizActionVO.POTerms)
                        {
                            LogInformation.Message = LogInformation.Message
                                                   + "Term ID : " + Convert.ToString(term.TermsAndConditionID) + ",";
                        }
                    }
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    LogInfoList.Add(LogInformation);
                }
                if (objBizActionVO.PurchaseOrder.Items.Count > 0)
                {
                    objBizActionVO.LogInfoList = LogInfoList;
                    WaitIndicator indicator = new WaitIndicator();
                    indicator.Show();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag1 = 0;
                        if (((clsAddPurchaseOrderBizActionVO)arg.Result).SuccessStatus == -2)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Unable to Save!, PR Pending Quantity is Less than Purchace Quantity for ItemCode "
                                       + ((clsAddPurchaseOrderBizActionVO)arg.Result).ItemCode, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            cmdSave.IsEnabled = true;
                        }
                        else if (arg.Error == null && ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder != null)
                        {
                            if (arg.Result != null)
                            {
                                if (ISEditMode == false)
                                    msgText = "Purchase Order Saved Successfully With PO Number";
                                else if (ISEditMode == true)
                                    msgText = "Purchase Order Updated Successfully With PO Number";

                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText + " " + ((clsAddPurchaseOrderBizActionVO)arg.Result).PurchaseOrder.PONO, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                SetCommandButtonState("Save");
                                msgW1.Show();
                                PurchaseOrderItems.Clear();
                                POIndentdetails.Clear();
                                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();

                                objAnimation.Invoke(RotationType.Backward);
                                FillPurchaseOrderDataGrid();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            cmdSave.IsEnabled = true;
                        }
                    };
                    indicator.Close();
                    Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Please select Items", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }  // END Add Or Update PO Only From This Form 
        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;

            if (cboSupplier.SelectedItem != null)
            {
                cboSupplier.SelectedValue = Itemswin.SupplierId;

                if ((cboSupplier.SelectedItem as MasterListItem).ID > 0)
                    cboSupplier.IsEnabled = false;
                else
                    cboSupplier.IsEnabled = true;
            }

            String strItemIDs = string.Empty;
            if (Itemswin.SelectedItems != null)
            {
                if (rdbDirect.IsChecked == true)
                {
                    grdpodetails.Columns[3].Visibility = Visibility.Collapsed;
                    grdpodetails.Columns[4].Visibility = Visibility.Collapsed;
                    grdpodetails.Columns[5].Visibility = Visibility.Collapsed;
                    grdpodetails.Columns[6].Visibility = Visibility.Collapsed;
                    grdpodetails.Columns[7].Visibility = Visibility.Visible;
                    grdpodetails.Columns[13].IsReadOnly = false;
                    grdpodetails.Columns[15].IsReadOnly = false;
                }

                foreach (var item in Itemswin.SelectedItems)
                {
                    if (PurchaseOrderItems.Where(poItems => poItems.ItemID == item.ID).Any() == false)
                    {
                        clsPurchaseOrderDetailVO objPOItems = new clsPurchaseOrderDetailVO();
                        objPOItems.ItemID = item.ID;
                        objPOItems.ItemName = item.ItemName;
                        objPOItems.ItemCode = item.ItemCode;
                        objPOItems.PUM = item.PUOM;
                        objPOItems.UOM = item.PUOM;
                        objPOItems.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                        objPOItems.ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                        objPOItems.BaseConversionFactor = item.PurchaseToBaseCF;
                        objPOItems.BaseRate = Convert.ToSingle(item.PurchaseRate);
                        objPOItems.BaseMRP = Convert.ToSingle(item.MRP);
                        objPOItems.SUOM = item.SUOM;
                        objPOItems.SUOMID = item.SUM;
                        objPOItems.MainRate = Convert.ToSingle(item.PurchaseRate);
                        objPOItems.MainMRP = Convert.ToSingle(item.MRP);
                        objPOItems.PUOMID = item.PUM;
                        objPOItems.POPUOMID = item.PUM;
                        objPOItems.BaseUOMID = item.BaseUM;
                        objPOItems.BaseUOM = item.BaseUMString;
                        objPOItems.SellingUOMID = item.SellingUM;
                        objPOItems.SellingUOM = item.SellingUMString;
                        objPOItems.POItemVatType = item.ItemVatType;
                        objPOItems.POItemVatApplicationOn = item.ItemVatApplicationOn;
                        objPOItems.POItemOtherTaxType = item.ItemOtherTaxType;
                        objPOItems.POItemOtherTaxApplicationOn = item.OtherItemApplicationOn;
                        objPOItems.VATPercent = item.VatPer;
                        objPOItems.ItemVATPercent = item.ItemVatPer;
                        objPOItems.Rate = Convert.ToDecimal(item.PurchaseRate) * Convert.ToDecimal(item.PurchaseToBaseCF);
                        objPOItems.Quantity = Convert.ToDecimal(item.RequiredQuantity);
                        objPOItems.MRP = Convert.ToDecimal(item.MRP) * Convert.ToDecimal(item.PurchaseToBaseCF);
                        objPOItems.CurrencyList = CurrencyList;
                        objPOItems.RateContractID = 0;
                        objPOItems.RateContractUnitID = 0;
                        objPOItems.RateContractCondition = null;
                        objPOItems.ItemGroup = item.ItemGroup;
                        objPOItems.ItemCategory = item.ItemCategory;
                        objPOItems.SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault();
                        objPOItems.ApplicableOnList = ApplicableOnList;                                                                    //Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                        objPOItems.SelectedApplicableOn = ApplicableOnList.Where(z => z.ID == item.CGSTapplicableon).FirstOrDefault(); //item.ItemVatApplicationOn //Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                        objPOItems.POSGSTVatType = item.SGSTtaxtype;
                        objPOItems.POSGSTVatApplicationOn = item.SGSTapplicableon;
                        objPOItems.POCGSTVatType = item.CGSTtaxtype;
                        objPOItems.POCGSTVatApplicationOn = item.CGSTapplicableon;
                        objPOItems.POIGSTVatType = item.IGSTtaxtype;
                        objPOItems.POIGSTVatApplicationOn = item.IGSTapplicableon;
                        objPOItems.SGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.SGSTPercent : 0;
                        objPOItems.CGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID == ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.CGSTPercent : 0;
                        objPOItems.IGSTPercent = ((MasterListItem)cboStoreState.SelectedItem).ID != ((MasterListItem)cboSupplierState.SelectedItem).ID ? item.IGSTPercent : 0;
                        objPOItems.HSNCode = item.HSNCodes;
                        strItemIDs = String.Format(strItemIDs + Convert.ToString(objPOItems.ItemID) + ",");
                        PurchaseOrderItems.Add(objPOItems);
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Product is alredy added!", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                }
                //foreach (var item2 in PurchaseOrderItems)
                //{
                //    strItemIDs = String.Format(strItemIDs + Convert.ToString(item2.ItemID) + ",");

                //}

                //if (grdpodetails.ItemsSource == null || ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource)).Count == 0)
                //{
                CheckRateContractForItems(strItemIDs.Trim(','), (grdpodetails.ItemsSource == null || ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource)).Count == 0) ? false : true);
                //}
                grdpodetails.ItemsSource = null;
                grdpodetails.ItemsSource = PurchaseOrderItems;
                CalculateOpeningBalanceSummary();
                grdpodetails.Focus();
                grdpodetails.UpdateLayout();
                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                if (IsAuditTrail)   // By Umesh For Audit Trail
                {
                    LogInformation = new LogInfo();
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 31 : Item List For Direct PO " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                            + "Supplier : " + Convert.ToString(((MasterListItem)cboSupplier.SelectedItem).Description) + " "
                                            + "Supplier ID : " + Convert.ToString(((MasterListItem)cboSupplier.SelectedItem).ID) + " ";
                    foreach (clsPurchaseOrderDetailVO item in PurchaseOrderItems)
                    {
                        LogInformation.Message = LogInformation.Message
                        + "ItemID : " + Convert.ToString(item.ItemID) + " "
                        + "ItemGroup : " + Convert.ToString(item.ItemGroup) + " "
                        + "Item Category : " + Convert.ToString(item.ItemCategory) + " "
                        + "Item Name : " + Convert.ToString(item.ItemName) + " "
                        + "Item Code : " + Convert.ToString(item.ItemCode) + " "
                        + "Purchase Unit : " + Convert.ToString(item.PUM) + " "
                        + "Purchase UOMID : " + Convert.ToString(item.PUOMID) + " "
                        + "Transaction UOM : " + Convert.ToString(item.TransUOM) + " "
                        + "Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                        + "Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                        + "Base Rate : " + Convert.ToString(item.BaseRate) + " "
                        + "Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                        + "Base Quantity : " + Convert.ToString(item.BaseQuantity) + " "
                        + "Stocking UOM : " + Convert.ToString(item.SUOM) + " "
                        + "Stocking UOMID : " + Convert.ToString(item.SUOMID) + " "
                        + "Main Rate : " + Convert.ToString(item.MainRate) + " "
                        + "Main MRP : " + Convert.ToString(item.MainMRP) + " "
                        + "Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                        + "Base UOMID : " + Convert.ToString(item.BaseUOMID) + " "
                        + "Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                        + "Selling UOMID : " + Convert.ToString(item.SellingUOMID) + " "
                        + "Item VAT Percent : " + Convert.ToString(item.ItemVATPercent) + " "
                        + "PO Item VatType : " + Convert.ToString(item.POItemVatType) + " "
                        + "PO Item Vat ApplicationOn : " + Convert.ToString(item.POItemVatApplicationOn) + " "
                        + "PO Item Other Tax Type : " + Convert.ToString(item.POItemOtherTaxType) + " "
                        + "PO Item Other Tax Application On : " + Convert.ToString(item.POItemOtherTaxApplicationOn) + " "
                        + "VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                        + "MRP : " + Convert.ToString(item.MRP) + " "
                        + "Rate Contract UnitID : " + Convert.ToString(item.RateContractUnitID) + " "
                        + "Selected Currency : " + Convert.ToString(item.SelectedCurrency.Description) + " "
                        + "Selected Currency ID : " + Convert.ToString(item.SelectedCurrency.ID) + " "
                        + "Cost Rate : " + Convert.ToString(item.CostRate) + " "
                        + "Purchase To BaseCF : " + Convert.ToString(item.PurchaseToBaseCF) + " "
                        + "Stocking ToBaseCF : " + Convert.ToString(item.StockingToBaseCF) + " "
                        + "Rate : " + Convert.ToString(item.Rate) + " "
                        + "Base Quantity : " + Convert.ToString(item.BaseQuantity) + " "
                        + "\r\n";
                    }
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    LogInfoList.Add(LogInformation);
                }
            }
        }

        private void cmdQuotation_Click(object sender, RoutedEventArgs e)
        {
            enmPurchaseOorder = PurchaseOrderSource.Quotation;
            QuotaionSearch OBJWindow = new QuotaionSearch();
            OBJWindow.onOKButton_Click += new RoutedEventHandler(OBJWindow_onOKButton_Click);
            OBJWindow.Show();
        }

        void OBJWindow_onOKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                QuotaionSearch objItemSearch = (QuotaionSearch)sender;
                if (objItemSearch.SelectedItems != null)
                {
                    List<clsQuotationDetailsVO> objList = objItemSearch.SelectedItems.ToList<clsQuotationDetailsVO>();
                    if (objList != null)
                    {
                        foreach (var item in objList)
                        {
                            PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO
                            {
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                ItemCode = item.ItemCode,
                                Quantity = (decimal)item.Quantity,
                                Rate = (decimal)item.Rate,
                                Amount = (decimal)item.Amount,
                                DiscountPercent = (decimal)item.ConcessionPercent,
                                DiscountAmount = (decimal)item.ConcessionAmount,
                                VATPercent = (decimal)item.TAXPercent,
                                VATAmount = (decimal)item.TAXAmount,
                                NetAmount = (decimal)item.NetAmount,
                                Specification = item.Specification,
                                PUM = item.PUM
                            });
                        }
                        grdpodetails.Focus();
                        grdpodetails.UpdateLayout();
                        CalculateOpeningBalanceSummary();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dgPOList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grdpo.ItemsSource = null;
            if (dgPOList.SelectedIndex != -1)
            {
                try
                {
                    clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                    clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                    objBizActionVO.SearchID = objList.ID;
                    objBizActionVO.UnitID = objList.UnitId;//((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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

                                if (obj.PurchaseOrderList != null && obj.PurchaseOrderList.Count > 0)
                                {
                                    grdpo.ItemsSource = obj.PurchaseOrderList;
                                }
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };

                    Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void Freez_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chkFreez = (CheckBox)sender;
                if (chkFreez != null)
                {
                    if (chkFreez.IsChecked == true)
                    {
                        msgText = "Are you sure you want to Freeze the  Details";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                              new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                        msgW.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            try
            {
                if (result == MessageBoxResult.Yes)
                {
                    clsPurchaseOrderVO objSelectedPO = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                    clsFreezPurchaseOrderBizActionVO objFreezPurchaseOrder = new clsFreezPurchaseOrderBizActionVO();
                    objFreezPurchaseOrder.PurchaseOrder = new clsPurchaseOrderVO();
                    objFreezPurchaseOrder.PurchaseOrder.ID = objSelectedPO.ID;
                    objFreezPurchaseOrder.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "Purchase Order Freezed Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Freezing details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    Client.ProcessAsync(objFreezPurchaseOrder, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                {
                    chkFreez.IsChecked = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtIndentNo.Text = "";
                clsPurchaseOrderVO obj = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsPurchaseOrderVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsPurchaseOrderVO)this.DataContext).ID = obj.ID;
                    if (obj.Direct == true)
                    {
                        rdbDirect.IsChecked = true;
                        grdpodetails.Columns[3].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[4].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[5].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[6].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[7].Visibility = Visibility.Visible;
                        grdpodetails.Columns[13].IsReadOnly = false;
                        grdpodetails.Columns[15].IsReadOnly = false;
                    }
                    else
                    {
                        rdbIndent.IsChecked = true;
                        grdpodetails.Columns[3].Visibility = Visibility.Visible;
                        grdpodetails.Columns[4].Visibility = Visibility.Visible;
                        grdpodetails.Columns[5].Visibility = Visibility.Visible;
                        grdpodetails.Columns[5].IsReadOnly = true;
                        grdpodetails.Columns[6].Visibility = Visibility.Visible;
                        grdpodetails.Columns[7].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[13].IsReadOnly = false;
                        grdpodetails.Columns[15].IsReadOnly = false;
                    }

                    if (cboStore.ItemsSource != null)
                    {
                        var results = from r in ((List<clsStoreVO>)cboStore.ItemsSource)
                                      where r.StoreId == obj.StoreID
                                      select r;

                        foreach (clsStoreVO item in results)
                        {
                            cboStore.SelectedItem = item;
                        }
                    }

                    cboStore.IsEnabled = false;
                    cboSupplier.IsEnabled = true;
                    if (cboSupplier.ItemsSource != null)
                    {

                        var results1 = from r in ((List<MasterListItem>)cboSupplier.ItemsSource)
                                       where r.ID == obj.SupplierID
                                       select r;

                        foreach (MasterListItem item in results1)
                        {
                            cboSupplier.SelectedItem = item;
                            lngSupplierID = item.ID;
                        }
                    }

                    if (cboDelivery.ItemsSource != null)
                    {
                        var results3 = from r in ((List<clsStoreVO>)cboDelivery.ItemsSource)
                                       where r.StoreId == obj.DeliveryDuration
                                       select r;

                        foreach (clsStoreVO item in results3)
                        {
                            cboDelivery.SelectedItem = item;
                            if (item.StoreId == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID)
                            {
                                cboDelivery.IsEnabled = false;
                            }
                            else
                                cboDelivery.IsEnabled = true;
                        }
                    }

                    if (cboTOP.ItemsSource != null)
                    {
                        var results4 = from r in ((List<MasterListItem>)cboTOP.ItemsSource)
                                       where r.ID == obj.PaymentTerms
                                       select r;

                        foreach (MasterListItem item in results4)
                        {
                            cboTOP.SelectedItem = item;
                        }
                    }

                    if (cmbSchedule.ItemsSource != null)
                    {
                        var results5 = from r in ((List<MasterListItem>)cmbSchedule.ItemsSource)
                                       where r.ID == obj.Schedule
                                       select r;

                        foreach (MasterListItem item in results5)
                        {
                            cmbSchedule.SelectedItem = item;
                        }
                    }

                    if (cmbDeliveryDays.ItemsSource != null)
                    {
                        var results6 = from r in ((List<MasterListItem>)cmbDeliveryDays.ItemsSource)
                                       where r.ID == obj.DeliveryDays
                                       select r;

                        foreach (MasterListItem item in results6)
                        {
                            cmbDeliveryDays.SelectedItem = item;
                        }

                    }

                    cmbModeofPayment.SelectedValue = obj.PaymentMode;
                    if (obj.Freezed == true && obj.Status == true)
                    {
                        cmdSave.IsEnabled = false;
                        cmdNew.IsEnabled = false;
                        cmdPrint.IsEnabled = true;
                        cmdSave.Content = "Modify";
                        cmdCancel.IsEnabled = true;
                        obj.IsEnabledFreezed = false;
                    }
                    else if (obj.Status == false)
                    {
                        cmdSave.IsEnabled = false;
                        cmdNew.IsEnabled = false;
                        cmdCancel.IsEnabled = true;

                    }
                    else
                    {
                        cmdSave.IsEnabled = true;

                        SetCommandButtonState("Modify");
                    }
                    FillPODetails(obj.ID, obj.UnitId);
                    ISEditMode = true;
                    dgPOList.UpdateLayout();
                    dgPOList.Focus();
                    cmdPrint.IsEnabled = false;
                    txtIndentNo.Text = ((clsPurchaseOrderVO)dgPOList.SelectedItem).IndentNumber.Trim();
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
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
                msgText = "Please Select a PO to Print";
                MessageBoxControl.MessageBoxChildWindow msgW =
                      new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                msgW.Show();
            }
        }

        private void cmdDeleteFreeService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((clsPurchaseOrderVO)dgPOList.SelectedItem) != null)
                {
                    if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsApproveded == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to cancel PO, PO is already Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD1.Show();
                        return;
                    }
                    else if (((clsPurchaseOrderVO)dgPOList.SelectedItem).IsCancelded == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to cancel PO, PO is already Approved", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD1.Show();
                        return;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to cancel the P.O. ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                POCancellationWindow Win = new POCancellationWindow();
                                Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
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

        bool CancelPO = false;
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            clsUpdateRemarkForCancellationPO bizactionVO = new clsUpdateRemarkForCancellationPO();
            bizactionVO.PurchaseOrder = new clsPurchaseOrderVO();
            try
            {
                bizactionVO.PurchaseOrder.ID = ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID;
                bizactionVO.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                bizactionVO.PurchaseOrder.CancellationRemark = ((POCancellationWindow)sender).txtAppReason.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsCancelPurchaseOrderBizActionVO bizActionObj = new clsCancelPurchaseOrderBizActionVO();
                        bizActionObj.PurchaseOrder = new clsPurchaseOrderVO();
                        bizActionObj.PurchaseOrder.ID = ((clsPurchaseOrderVO)dgPOList.SelectedItem).ID;
                        bizActionObj.PurchaseOrder.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        Client.ProcessCompleted += (s1, arg1) =>
                        {
                            if (arg1.Error == null)
                            {
                                if (arg1.Result != null)
                                {
                                    CancelPO = true;
                                    FillPurchaseOrderDataGrid();
                                }
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                            }
                        };
                        Client.ProcessAsync(bizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
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

        List<clsPurchaseOrderDetailVO> lstDeletedPODetails = new List<clsPurchaseOrderDetailVO>();
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {
                string msgText = "Are you sure you want to delete the item ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        lstDeletedPODetails.Add(PurchaseOrderItems[grdpodetails.SelectedIndex]);
                        DeleteItemRateContract();
                        String strIndentNo = ((clsPurchaseOrderDetailVO)(grdpodetails.SelectedItem)).IndentNumber;
                        foreach (var item in POIndentdetails.ToList())
                        {
                            if (item.ItemID == ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID
                                && item.IndentID == ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).IndentID
                                && item.IndentUnitID == ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).IndentUnitID
                                && item.IndentDetailID == ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).IndentDetailID
                                && item.IndentDetailUnitID == ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).IndentDetailUnitID)
                            {
                                POIndentdetails.Remove(item);
                            }
                        }

                        PurchaseOrderItems.RemoveAt(grdpodetails.SelectedIndex);

                        if (strIndentNo != "" && strIndentNo != null)
                        {
                            Boolean blnIsIndentExist = PurchaseOrderItems.Where(z => z.IndentNumber.Trim().Equals(strIndentNo.Trim())).Any();
                            if (txtIndentNo.Text != null && !blnIsIndentExist && txtIndentNo.Text.Trim().Contains(strIndentNo.Trim()))
                                txtIndentNo.Text = txtIndentNo.Text.Replace(strIndentNo, String.Empty).Trim(',');
                        }
                        grdpodetails.Focus();
                        grdpodetails.ItemsSource = PurchaseOrderItems;
                        grdpodetails.UpdateLayout();
                        grdpodetails.SelectedIndex = PurchaseOrderItems.Count - 1;
                        CalculateOpeningBalanceSummary();
                    }
                };
                msgWD.Show();
            }
        }

        private void chkCancelPO_Click(object sender, RoutedEventArgs e)
        {
            if (chkCancelPO.IsChecked == true)
            {
                dgPOList.Columns[25].Visibility = Visibility.Collapsed;
                dgPOList.Columns[10].IsReadOnly = true;
                dgPOList.Columns[9].IsReadOnly = true;
            }
            else
            {
                dgPOList.Columns[25].Visibility = Visibility.Visible;
                dgPOList.Columns[10].IsReadOnly = false;
                dgPOList.Columns[9].IsReadOnly = false;
            }
            FillPurchaseOrderDataGrid();
        }
        #endregion

        #region Item Search Control
        ItemSearch _ItemSearchRowControl = null;
        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {
            BdrItemSearch.Visibility = Visibility.Visible;
            ItemSearchStackPanel.Children.Clear();
            _ItemSearchRowControl = new ItemSearch(StoreID, SupplierID);
            _ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);
        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {
            String strItemIDs = string.Empty;//Only For PO
            ItemSearch _ItemSearchRowControl = (ItemSearch)sender;
            if (PurchaseOrderItems != null)
            {
                if (PurchaseOrderItems.Count.Equals(0))
                {
                    PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                }
            }
            else
            {
                PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
            }
            _ItemSearchRowControl.SelectedItems[0].RowID = PurchaseOrderItems.Count + 1;
            foreach (var item in _ItemSearchRowControl.SelectedItems)
            {
                if (PurchaseOrderItems.Where(poItems => poItems.ItemID == item.ItemID).Any() == false)
                {
                    PurchaseOrderItems.Add(new clsPurchaseOrderDetailVO { ItemID = item.ItemID, ItemName = item.ItemName, ItemCode = item.ItemCode, PUM = item.PUOM, VATPercent = item.VatPer, Rate = item.PurchaseRate, Quantity = Convert.ToDecimal(item.RequiredQuantity), MRP = item.MRP, CurrencyList = CurrencyList, RateContractID = -1, RateContractUnitID = -1, RateContractCondition = null, SelectedCurrency = CurrencyList.Where(z => z.ID == 1).FirstOrDefault() }); //, MRP = item.MRP                
                    strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please add another item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }

            grdpodetails.ItemsSource = null;
            grdpodetails.ItemsSource = PurchaseOrderItems;
            dgPOList.UpdateLayout();
            CalculateOpeningBalanceSummary();
            //CheckRateContractForItems(strItemIDs.Trim(',')); //Only For PO
            _ItemSearchRowControl.SetFocus();
            _ItemSearchRowControl = null;
            _ItemSearchRowControl = new ItemSearch();
        }
        #endregion

        #region Private Methods
        private void FillSchedule()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Schedule;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbSchedule.ItemsSource = null;
                        cmbSchedule.ItemsSource = objList;
                        if (objList != null)
                        {
                            cmbSchedule.SelectedItem = objList[0];
                        }
                        // Newly Addedd By CDS 
                        FillTermsOfPayment();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        List<clsStoreVO> objStoreList = new List<clsStoreVO>();

        // User Rights Wise Fill Store 
        private void FillStore()
        {
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

                    NonQSAndUserDefinedStores.ToList().Insert(0, Default);

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (result.ToList().Count > 0)
                        {
                            objStoreList = (List<clsStoreVO>)result.ToList();
                        }
                    }
                    else
                    {
                        if (NonQSAndUserDefinedStores.ToList().Count > 0)
                        {
                            objStoreList = (List<clsStoreVO>)NonQSAndUserDefinedStores.ToList();
                        }
                    }
                    if (objStoreList != null)
                    {
                        cmbStore.ItemsSource = null;
                        cmbStore.ItemsSource = objStoreList;
                        cboStore.ItemsSource = null;
                        cboStore.ItemsSource = objStoreList;
                        cmbStore.SelectedItem = objStoreList[0];
                        cboStore.SelectedItem = objStoreList[0];

                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCentralPurchaseStore)
                        {
                            cboStore.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID;
                        }
                        else
                        {
                            cboStore.IsEnabled = true;
                        }
                    }
                    FillSupplier();
                    FillDelivery();
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillTermsOfPayment()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TermsofPayment;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cboTOP.ItemsSource = null;
                        cboTOP.ItemsSource = objList;

                        if (objList.Count > 1)
                            cboTOP.SelectedItem = objList[1];
                        else
                            cboTOP.SelectedItem = objList[0];
                        FillStore();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FillDeliveryDays()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TermsofPayment;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cmbDeliveryDays.ItemsSource = null;
                        cmbDeliveryDays.ItemsSource = objList;
                        cmbDeliveryDays.SelectedItem = objList[0];
                        //Added By CDS
                        if (ISFromAprovePO == true)
                        {
                            FillPODetailsFromAprovalForm();
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillDelivery()
        {
            try
            {
                cboDelivery.ItemsSource = null;
                cboDelivery.ItemsSource = objStoreList;
                if (objStoreList != null)
                {
                    cboDelivery.SelectedItem = objStoreList[0];
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        List<MasterListItem> objSupplierList = new List<MasterListItem>();
        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsGST = true;//Added By Bhushan For GST Get StateID 24062017
                BizAction.IsFromPOGRN = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        //List<MasterListItem> objList = new List<MasterListItem>();
                        objSupplierList = new List<MasterListItem>();
                        objSupplierList.Add(new MasterListItem(0, "-- Select --"));
                        objSupplierList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        foreach (var item in objSupplierList)
                        {
                            item.Description = item.Code + " - " + item.Description;
                        }
                        cboSupplier.ItemsSource = null;
                        cboSupplier.ItemsSource = objSupplierList;
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objSupplierList;
                        cboSupplier.SelectedItem = objSupplierList[0];
                        cmbSupplier.SelectedItem = objSupplierList[0];
                        //Newly Added By CDS 
                        FillModeofPayment();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillState()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_StateMaster;
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
                        foreach (var item in objList)
                        {
                            if (item.ID != 0)
                            {
                                item.Description = item.Description + " - (" + item.Code + ")";
                            }
                        }
                        cboStoreState.ItemsSource = null;
                        cboStoreState.ItemsSource = objList;
                        cboSupplierState.ItemsSource = null;
                        cboSupplierState.ItemsSource = objList;
                        cboStoreState.SelectedItem = objList[0];
                        cboSupplierState.SelectedItem = objList[0];
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void FillModeofPayment()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ModeOfPayment;
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
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbModeofPayment.ItemsSource = null;
                        cmbModeofPayment.ItemsSource = objList;
                        cmbModeofPayment.SelectedItem = objList[0];
                        //Newly Added By CDS 
                        FillDeliveryDays();
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Fills the Currency Master.
        /// </summary>
        private void FillCurrency()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Currency;
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
                        CurrencyList = objList;
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

        private void FillPurchseOrderTerms()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_TermAndCondition;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        PurchaseOrderTerms = new List<MasterListItem>();
                        PurchaseOrderTerms.Add(new MasterListItem(0, "-- Select --"));
                        PurchaseOrderTerms.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        foreach (var item in PurchaseOrderTerms)
                        {
                            item.Status = false;
                        }
                        cmbTermsNCondition.ItemsSource = null;
                        cmbTermsNCondition.ItemsSource = PurchaseOrderTerms;
                        if (PurchaseOrderTerms != null)
                        {
                            cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// This method is used to check for the rate contract for the items 
        /// </summary>
        /// <param name="strItemIDs"></param>
        public void CheckRateContractForItems(string strItemIDs, bool IsItemAddedInGrid)
        {
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            clsGetRateContractAgainstSupplierAndItemBizActionVO objBizAction = new clsGetRateContractAgainstSupplierAndItemBizActionVO();
            objBizAction.ItemIDs = strItemIDs;
            objBizAction.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objBizAction = args.Result as clsGetRateContractAgainstSupplierAndItemBizActionVO;
                        lstRateContractItemDetailsNew = new List<clsRateContractDetailsVO>();
                        lstRateContractItemDetailsNew = objBizAction.RateContractListNew;
                        List<clsPurchaseOrderDetailVO> NonRCSelectedPOList = new List<clsPurchaseOrderDetailVO>();

                        foreach (var itemPO in PurchaseOrderItems)
                        {
                            clsRateContractDetailsVO objRC = new clsRateContractDetailsVO();
                            objRC = lstRateContractItemDetailsNew.Where(z => z.ItemID == itemPO.ItemID).FirstOrDefault();
                            if (objRC != null && itemPO.ItemID == objRC.ItemID)
                            {
                                itemPO.IsRateContractAppliedToItem = true;
                                itemPO.lnkContract = "Applied";
                                itemPO.RateContractID = objRC.ContractID;
                                itemPO.RateContractUnitID = objRC.ContractUnitId;
                                itemPO.Quantity = objRC.Quantity;
                                itemPO.SelectedUOM = objRC.SelectedUOM;
                                itemPO.BaseConversionFactor = objRC.BaseConversionFactor;
                                itemPO.BaseUOMID = objRC.BaseUOMID;
                                itemPO.ConversionFactor = objRC.ConversionFactor;  //stock cf
                                itemPO.SUOMID = objRC.SUOMID;
                                itemPO.BaseRate = objRC.BaseRate;
                                itemPO.BaseMRP = objRC.BaseMRP;
                                itemPO.MainRate = itemPO.BaseRate;
                                itemPO.MainMRP = itemPO.BaseMRP;
                                itemPO.Rate = objRC.Rate;
                                itemPO.MRP = objRC.MRP;
                                itemPO.DiscountPercent = objRC.DiscountPercent;
                                itemPO.Specification = objRC.Remarks;
                            }
                            else
                            {
                                if (itemPO.IsRateContractAppliedToItem == false)
                                {
                                    itemPO.lnkContract = "Not Applied";
                                    NonRCSelectedPOList.Add(itemPO);
                                }
                            }
                        }

                        if (NonRCSelectedPOList != null && NonRCSelectedPOList.Count() > 0 && (IsItemAddedInGrid == false && PurchaseOrderItems.Where(z => z.IsRateContractAppliedToItem == true).Any()))
                        {
                            foreach (var item in NonRCSelectedPOList)
                            {
                                PurchaseOrderItems.Remove(item);

                                foreach (var itemPOIndent in POIndentdetails.ToList())
                                {
                                    if (itemPOIndent.ItemID == item.ItemID && itemPOIndent.IndentID == item.IndentID && itemPOIndent.IndentUnitID == item.IndentUnitID && itemPOIndent.IndentDetailID == item.IndentDetailID && itemPOIndent.IndentDetailUnitID == item.IndentDetailUnitID)
                                    {
                                        POIndentdetails.Remove(itemPOIndent);
                                    }
                                }
                            }
                        }

                        #region For POBest Price
                        POBestPriceList = new List<clsRateContractDetailsVO>();
                        this.POBestPriceList = objBizAction.POBestPriceList;

                        foreach (var itemBestList in POBestPriceList.ToList())
                        {
                            foreach (var itemPOList in PurchaseOrderItems)
                            {
                                if (itemBestList.IsItemInRateContract == false && itemBestList.ItemID == itemPOList.ItemID)
                                {
                                    itemPOList.BaseRate = itemBestList.BestBaseRate;
                                    itemPOList.MainRate = itemBestList.BestBaseRate;

                                    itemPOList.BaseMRP = itemBestList.BestBaseMRP;
                                    itemPOList.MainMRP = itemBestList.BestBaseMRP;

                                    itemPOList.Rate = Convert.ToDecimal(itemBestList.BestBaseRate * itemPOList.BaseConversionFactor);
                                    itemPOList.MRP = Convert.ToDecimal(itemBestList.BestBaseMRP * itemPOList.BaseConversionFactor);

                                }
                            }
                        }
                        #endregion

                        //old
                        lstRateContract = new List<clsRateContractMasterVO>();
                        lstRateContractItemDetails = new List<clsRateContractItemDetailsVO>();
                        lstRateContract = objBizAction.RateContractMasterList;
                        lstRateContractItemDetails = objBizAction.RateContractItemDetailsList;
                        //old

                        CalculateOpeningBalanceSummary();
                        //GetRateContractDiscount();
                    }
                };
                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                indicator.Close();
            }
        }

        private void GetRateContractDiscount()
        {
            var lstGroupByItemID = lstRateContractItemDetails.GroupBy(z => z.ItemID).ToList();
            for (int iCount = 0; iCount < lstGroupByItemID.Count; iCount++)
            {
                // Check weather the Item having a more than one Rate Contract.
                var lstContracts = lstGroupByItemID[iCount].ToList();
                if (lstContracts != null && lstContracts.Count > 1)
                {
                    List<clsPurchaseOrderDetailVO> lstPOItems = PurchaseOrderItems.Where(z => z.ItemID == lstContracts[0].ItemID).ToList();
                    foreach (clsPurchaseOrderDetailVO objPODetail in lstPOItems)
                    {
                        objPODetail.IsMultipleContract = true;
                    }
                }
                // If the Item Having a single rate contract then assign the discount for that item.
                else if (lstContracts != null && lstContracts.Count == 1)
                {
                    List<clsPurchaseOrderDetailVO> lstPODetail = PurchaseOrderItems.Where(POItems => POItems.ItemID == lstContracts[0].ItemID).ToList();
                    foreach (clsPurchaseOrderDetailVO objPODetail in lstPODetail)
                    {
                        if (objPODetail != null && objPODetail.RateContractID == -1)
                        {
                            objPODetail.RateContractID = lstContracts[0].RateContractID;
                            objPODetail.RateContractUnitID = lstContracts[0].RateContractUnitID;
                            objPODetail.RateContractCondition = lstContracts[0].Condition;
                            if (objPODetail.Quantity > 0)
                            {
                                switch (lstContracts[0].Condition)
                                {
                                    case "Between":
                                        if (Convert.ToDouble(objPODetail.Quantity) >= lstContracts[0].MinQuantity && Convert.ToDouble(objPODetail.Quantity) <= lstContracts[0].MaxQuantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                    case ">":
                                        if (Convert.ToDouble(objPODetail.Quantity) > lstContracts[0].Quantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                    case "<":
                                        if (Convert.ToDouble(objPODetail.Quantity) < lstContracts[0].Quantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                    case "No Limit":
                                        objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                        objPODetail.ConditionFound = true;

                                        break;
                                    case "=":
                                        if (Convert.ToDouble(objPODetail.Quantity) == lstContracts[0].Quantity)
                                        {
                                            objPODetail.DiscountPercent = Convert.ToDecimal(lstContracts[0].DiscountPercent);
                                            objPODetail.ConditionFound = true;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            if (ISEditMode == false)
            {
                foreach (clsPurchaseOrderDetailVO objPODetail in PurchaseOrderItems.ToList())
                {
                    if (objPODetail.IsMultipleContract == false)
                    {
                    }
                }

            }
            CalculateOpeningBalanceSummary();
        }

        private void CalculateNewNetAmount()
        {
            if (!string.IsNullOrEmpty(txtOtherCharges.Text))
                ((clsPurchaseOrderVO)this.DataContext).OtherCharges = Convert.ToDecimal(txtOtherCharges.Text);
            else
                ((clsPurchaseOrderVO)this.DataContext).OtherCharges = 0;

            if (!string.IsNullOrEmpty(txtPODiscount.Text))
                ((clsPurchaseOrderVO)this.DataContext).PODiscount = Convert.ToDecimal(txtPODiscount.Text);
            else
                ((clsPurchaseOrderVO)this.DataContext).PODiscount = 0;

            string netamt = Convert.ToString(((clsPurchaseOrderVO)this.DataContext).PrevTotalNet + ((clsPurchaseOrderVO)this.DataContext).OtherCharges - ((clsPurchaseOrderVO)this.DataContext).PODiscount);
            txtNewNetAmount.Text = Convert.ToString(System.Math.Round(Convert.ToDecimal(netamt), 2));
            ((clsPurchaseOrderVO)this.DataContext).TotalNet = Convert.ToDecimal(txtNewNetAmount.Text);
        }

        private void FillPurchaseOrderDataGrid()
        {
            try
            {
                indicator.Show();
                clsGetPurchaseOrderBizActionVO objBizActionVO = new clsGetPurchaseOrderBizActionVO();
                if (dtpFromDate.SelectedDate != null)
                    objBizActionVO.searchFromDate = dtpFromDate.SelectedDate.Value;
                if (dtpToDate.SelectedDate != null)
                    objBizActionVO.searchToDate = dtpToDate.SelectedDate.Value;
                if (cmbStore.SelectedItem != null)
                    objBizActionVO.SearchStoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId == null ? 0 : ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                if (cmbSupplier.SelectedItem != null)
                {
                    objBizActionVO.SearchSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                }
                objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objBizActionVO.PagingEnabled = true;
                objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.MaximumRows = DataList.PageSize;
                objBizActionVO.PONO = txtPONO.Text;
                if (chkCancelPO.IsChecked == true)
                {
                    objBizActionVO.CancelPO = true;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderBizActionVO objPurchaseOrderList = ((clsGetPurchaseOrderBizActionVO)arg.Result);
                            dgPOList.ItemsSource = null;
                            DataList.Clear();
                            DataList.TotalItemCount = objPurchaseOrderList.TotalRowCount;
                            if (objPurchaseOrderList.PurchaseOrderList != null)
                            {
                                foreach (var item in objPurchaseOrderList.PurchaseOrderList)
                                {
                                    DataList.Add(item);
                                }
                                dgPOList.ItemsSource = null;
                                dgPOList.ItemsSource = DataList;
                                grdpo.ItemsSource = null;
                                purchaseOrderList.Clear();
                                datapager.Source = DataList;
                            }
                            indicator.Close();
                            txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                        }
                        else
                            indicator.Close();
                    }
                    else
                    {
                        System.Windows.Browser.HtmlPage.Window.Alert("Error occured while processing.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FillPODetails(long POID, long UnitID)
        {
            WaitIndicator indicator1 = new WaitIndicator();
            indicator1.Show();
            try
            {
                clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();
                clsPurchaseOrderVO objList = (clsPurchaseOrderVO)dgPOList.SelectedItem;
                string strItemIDs = String.Empty;
                objBizActionVO.SearchID = POID;
                objBizActionVO.UnitID = UnitID; //((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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
                            POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                            List<clsPurchaseOrderDetailVO> objListOfItems = (List<clsPurchaseOrderDetailVO>)obj.PurchaseOrderList;
                            List<clsPurchaseOrderDetailVO> PoIndent = (List<clsPurchaseOrderDetailVO>)obj.PoIndentList;
                            PurchaseOrderItems = new ObservableCollection<clsPurchaseOrderDetailVO>();
                            List<clsPurchaseOrderTerms> lstTerms = (List<clsPurchaseOrderTerms>)obj.POTerms;
                            if (objListOfItems != null)
                            {
                                foreach (var item in objListOfItems)
                                {
                                    strItemIDs = String.Format(strItemIDs + Convert.ToString(item.ItemID) + ",");
                                    clsPurchaseOrderDetailVO obj11 = new clsPurchaseOrderDetailVO();
                                    obj11.IndentID = Convert.ToInt64(item.IndentID);
                                    obj11.IndentUnitID = Convert.ToInt64(item.IndentUnitID);
                                    obj11.IndentDetailID = item.IndentDetailID;
                                    obj11.IndentDetailUnitID = item.IndentUnitID;
                                    obj11.PoDetailsID = item.PoDetailsID;
                                    obj11.PoDetailsUnitID = item.PoDetailsUnitID;
                                    obj11.ItemID = item.ItemID;
                                    obj11.ItemName = item.ItemName;
                                    obj11.ItemCode = item.ItemCode;
                                    obj11.Quantity = (decimal)item.Quantity;
                                    obj11.Rate = (decimal)item.Rate;
                                    obj11.PRQuantity = Convert.ToDecimal(item.PRQTY);
                                    obj11.PendingQuantity = Convert.ToDecimal(item.PRPendingQty);
                                    obj11.PRPendingQuantity = Convert.ToDouble(item.PRPendingQty);
                                    obj11.TransUOM = item.PRUOM;
                                    obj11.Amount = (decimal)item.Amount;
                                    obj11.MRP = (decimal)item.MRP;
                                    obj11.DiscountPercent = (decimal)item.DiscountPercent;
                                    obj11.DiscountAmount = (decimal)item.DiscountAmount;
                                    // Added By CDS 
                                    obj11.ItemVATPercent = item.ItemVATPercent;
                                    obj11.ItemVATAmount = item.ItemVATAmount;
                                    obj11.POItemVatType = item.POItemVatType;
                                    obj11.POItemVatApplicationOn = item.POItemVatApplicationOn;
                                    obj11.POItemOtherTaxType = item.POItemOtherTaxType;
                                    obj11.POItemOtherTaxApplicationOn = item.POItemOtherTaxApplicationOn;
                                    obj11.CostRate = (decimal)item.Quantity * (decimal)item.Rate;
                                    obj11.VATPercent = (decimal)item.VATPercent;
                                    obj11.VATAmount = (decimal)item.VATAmount;
                                    obj11.OtherCharges = (decimal)item.OtherCharges;
                                    obj11.PODiscount = (decimal)item.PODiscount;
                                    obj11.ConversionFactor = item.ConversionFactor;
                                    obj11.BaseConversionFactor = Convert.ToSingle(item.BaseConversionFactor);
                                    obj11.BaseRate = item.BaseRate;
                                    obj11.BaseMRP = item.BaseMRP;
                                    obj11.BaseQuantity = item.BaseQuantity;
                                    obj11.BaseUOM = item.BaseUOM;
                                    obj11.SUOM = item.SUOM;
                                    obj11.MainRate = item.BaseRate;
                                    obj11.MainMRP = item.BaseMRP;
                                    obj11.SelectedUOM = new MasterListItem { ID = item.SelectedUOM.ID, Description = item.TransUOM };
                                    obj11.SUOMID = item.SUOMID;
                                    obj11.BaseUOMID = item.BaseUOMID;
                                    obj11.POPUOMID = item.PUOMID; //item.SelectedUOM.ID;   // commented by Ashish on Dated 070816
                                    obj11.Specification = item.Specification;
                                    obj11.PUM = item.PUM;
                                    obj11.RateContractID = item.RateContractID;
                                    obj11.RateContractUnitID = item.RateContractUnitID;
                                    obj11.RateContractCondition = item.RateContractCondition;
                                    obj11.IndentNumber = item.IndentNumber;
                                    obj11.ItemGroup = item.ItemGroup;
                                    obj11.ItemCategory = item.ItemCategory;
                                    obj11.CurrencyList = CurrencyList;
                                    obj11.PurchaseToBaseCF = item.PurchaseToBaseCF;
                                    obj11.StockingToBaseCF = item.StockingToBaseCF;
                                    obj11.SelectedCurrency = CurrencyList.Where(z => z.ID == item.SelectedCurrency.ID).FirstOrDefault();
                                    obj11.NetAmount = (decimal)item.NetAmount;
                                    obj11.POApprItemQty = item.POApprItemQty;
                                    obj11.POPendingItemQty = item.POPendingItemQty;
                                    obj11.PODetailsViewTimeQty = item.PODetailsViewTimeQty;  // to check pending quantity validation at the time of PO Item Qyantity view & Edit.
                                    obj11.ApplicableOnList = ApplicableOnList;                                                                       //Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                                    obj11.SelectedApplicableOn = ApplicableOnList.Where(z => z.ID == obj11.POCGSTVatApplicationOn).FirstOrDefault(); // obj11.POItemVatApplicationOn Added By ARZ for Itemwise Tax mapping during Transaction on Dated 21042017
                                    //Added By Bhushanp For GST 22062017
                                    obj11.SGSTPercent = item.SGSTPercent;
                                    obj11.SGSTAmount = item.SGSTAmount;
                                    obj11.CGSTPercent = item.CGSTPercent;
                                    obj11.CGSTAmount = item.CGSTAmount;
                                    obj11.IGSTPercent = item.IGSTPercent;
                                    obj11.IGSTAmount = item.IGSTAmount;
                                    obj11.POSGSTVatType = item.POSGSTVatType;
                                    obj11.POSGSTVatApplicationOn = item.POSGSTVatApplicationOn;
                                    obj11.POCGSTVatType = item.POCGSTVatType;
                                    obj11.POCGSTVatApplicationOn = item.POSGSTVatApplicationOn;
                                    obj11.POIGSTVatType = item.POIGSTVatType;
                                    obj11.POIGSTVatApplicationOn = item.POIGSTVatApplicationOn;
                                    obj11.HSNCode = item.HSNCode;
                                    obj11.TotalBatchAvailableStock = item.TotalBatchAvailableStock;
                                    obj11.IsRateContractAppliedToItem = obj11.RateContractID > 0 ? true : false;
                                    obj11.lnkContract = obj11.IsRateContractAppliedToItem == true ? "Applied" : "Not Applied";

                                    //if (this.ISFromAprovePO == true && this.POApproveLvlID == 2)
                                    //{
                                    //    //add property on 25Oct2018 to enable Rate Contract link while click on Verify And Approve PO
                                    //    if (obj11.IsRateContractAppliedToItem == true)
                                    //        obj11.IsRateContractLinkEnable = false;
                                    //    else
                                    //        obj11.IsRateContractLinkEnable = false;  // changed on false on 26/10/18 by Prashant Channe
                                    //}
                                    //else
                                    //{
                                    //    obj11.IsRateContractLinkEnable = false;
                                    //}



                                    PurchaseOrderItems.Add(obj11);
                                }
                                foreach (var item in PoIndent)
                                {
                                    POIndentdetails.Add(item);
                                }
                                foreach (var item in PurchaseOrderTerms)
                                {
                                    foreach (var item2 in lstTerms)
                                    {
                                        if (item.ID == item2.TermsAndConditionID)
                                        {
                                            item.Status = item2.Status;
                                        }
                                    }
                                    cmbTermsNCondition.SelectedItem = PurchaseOrderTerms[0];
                                }

                                if (this.ISFromAprovePO == true && this.POApproveLvlID == 2)
                                {
                                    CheckRateContractForItemsFromApprove(strItemIDs.Trim(','), (grdpodetails.ItemsSource == null || ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource)).Count == 0) ? false : true);
                                }

                                grdpodetails.ItemsSource = PurchaseOrderItems;
                                grdpodetails.Focus();
                                grdpodetails.UpdateLayout();
                                CalculateOpeningBalanceSummary();
                            }
                        }
                        indicator1.Close();
                    }
                    else
                    {
                        indicator1.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding P O details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
                indicator1.Close();
            }
        }

        public void CheckRateContractForItemsFromApprove(string strItemIDs, bool IsItemAddedInGrid)
        {
            bool blnRateDiffered = false;
            WaitIndicator indicator = new WaitIndicator();
            indicator.Show();
            clsGetRateContractAgainstSupplierAndItemBizActionVO objBizAction = new clsGetRateContractAgainstSupplierAndItemBizActionVO();
            objBizAction.ItemIDs = strItemIDs;
            objBizAction.SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objBizAction = args.Result as clsGetRateContractAgainstSupplierAndItemBizActionVO;
                        lstRateContractItemDetailsNew = new List<clsRateContractDetailsVO>();
                        lstRateContractItemDetailsNew = objBizAction.RateContractListNew;
                        List<clsPurchaseOrderDetailVO> NonRCSelectedPOList = new List<clsPurchaseOrderDetailVO>();
                        
                        //Begin to check whether Rate Differs from Items on PO, Added by Prashant Channe on 26/10/2018
                        foreach (var itemPO in PurchaseOrderItems)
                        {
                            clsRateContractDetailsVO objRC = new clsRateContractDetailsVO();
                            objRC = lstRateContractItemDetailsNew.Where(z => ((z.ItemID == itemPO.ItemID && z.ContractID == itemPO.RateContractID) && (z.BaseRate != itemPO.BaseRate || z.BaseMRP != itemPO.BaseMRP))).FirstOrDefault();
                            if (objRC != null)
                            {
                                blnRateDiffered = true;
                                break;
                            }
                        }
                        //End of check

                        if (blnRateDiffered == true)
                        {
                            string msgText = "PO Rate differs from Rate Contract, Apply revised rates from Rate Contract ?";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    //check if level 2 user applying rate contract, Added by Prashant Channe on 26/10/2018
                                    if (this.POApproveLvlID == 2)
                                    {
                                        foreach (var itemPO in PurchaseOrderItems)
                                        {
                                            clsRateContractDetailsVO objRC = new clsRateContractDetailsVO();
                                            objRC = lstRateContractItemDetailsNew.Where(z => ((z.ItemID == itemPO.ItemID && z.ContractID == itemPO.RateContractID) && (z.BaseRate != itemPO.BaseRate || z.BaseMRP != itemPO.BaseMRP))).FirstOrDefault();

                                            if (objRC != null && itemPO.ItemID == objRC.ItemID)
                                            {
                                                //itemPO.IsRateContractAppliedToItem = true;
                                                //itemPO.lnkContract = "Applied";
                                                //itemPO.RateContractID = objRC.ContractID;
                                                //itemPO.RateContractUnitID = objRC.ContractUnitId;
                                                //itemPO.Quantity = objRC.Quantity;
                                                //itemPO.SelectedUOM = objRC.SelectedUOM;
                                                //itemPO.BaseConversionFactor = objRC.BaseConversionFactor;
                                                //itemPO.BaseUOMID = objRC.BaseUOMID;
                                                //itemPO.ConversionFactor = objRC.ConversionFactor;  //stock cf
                                                //itemPO.SUOMID = objRC.SUOMID;
                                                itemPO.BaseRate = objRC.BaseRate;
                                                itemPO.BaseMRP = objRC.BaseMRP;
                                                itemPO.MainRate = itemPO.BaseRate;
                                                itemPO.MainMRP = itemPO.BaseMRP;
                                                itemPO.Rate = objRC.Rate;
                                                itemPO.MRP = objRC.MRP;
                                                itemPO.DiscountPercent = objRC.DiscountPercent;     //Set Property for Discount from Revised Rate Contract on 31/10/2018
                                                //itemPO.Specification = objRC.Remarks; // need to confirm
                                            }
                                            else
                                            {
                                                if (itemPO.IsRateContractAppliedToItem == false)
                                                {
                                                    itemPO.lnkContract = "Not Applied";
                                                    NonRCSelectedPOList.Add(itemPO);
                                                }
                                            }
                                        }
                                    }

                                    //if (NonRCSelectedPOList != null && NonRCSelectedPOList.Count() > 0 && (IsItemAddedInGrid == false && PurchaseOrderItems.Where(z => z.IsRateContractAppliedToItem == true).Any()))
                                    //{
                                    //    foreach (var item in NonRCSelectedPOList)
                                    //    {
                                    //        PurchaseOrderItems.Remove(item);

                                    //        foreach (var itemPOIndent in POIndentdetails.ToList())
                                    //        {
                                    //            if (itemPOIndent.ItemID == item.ItemID && itemPOIndent.IndentID == item.IndentID && itemPOIndent.IndentUnitID == item.IndentUnitID && itemPOIndent.IndentDetailID == item.IndentDetailID && itemPOIndent.IndentDetailUnitID == item.IndentDetailUnitID)
                                    //            {
                                    //                POIndentdetails.Remove(itemPOIndent);
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    #region For POBest Price
                                    POBestPriceList = new List<clsRateContractDetailsVO>();
                                    this.POBestPriceList = objBizAction.POBestPriceList;

                                    foreach (var itemBestList in POBestPriceList.ToList())
                                    {
                                        foreach (var itemPOList in PurchaseOrderItems)
                                        {
                                            if (itemBestList.IsItemInRateContract == false && itemBestList.ItemID == itemPOList.ItemID)
                                            {
                                                itemPOList.BaseRate = itemBestList.BestBaseRate;
                                                itemPOList.MainRate = itemBestList.BestBaseRate;

                                                itemPOList.BaseMRP = itemBestList.BestBaseMRP;
                                                itemPOList.MainMRP = itemBestList.BestBaseMRP;

                                                itemPOList.Rate = Convert.ToDecimal(itemBestList.BestBaseRate * itemPOList.BaseConversionFactor);
                                                itemPOList.MRP = Convert.ToDecimal(itemBestList.BestBaseMRP * itemPOList.BaseConversionFactor);

                                            }
                                        }
                                    }
                                    #endregion

                                    ////old
                                    //lstRateContract = new List<clsRateContractMasterVO>();
                                    //lstRateContractItemDetails = new List<clsRateContractItemDetailsVO>();
                                    //lstRateContract = objBizAction.RateContractMasterList;
                                    //lstRateContractItemDetails = objBizAction.RateContractItemDetailsList;
                                    ////old

                                    CalculateOpeningBalanceSummary();
                                    ////GetRateContractDiscount();

                                }
                                else 
                                {
                                    //ToDo:  Enable Apply Link on PO Item Details grid for respective items in RC

                                    foreach (var itemPO in PurchaseOrderItems)
                                    {
                                        clsRateContractDetailsVO objRC = new clsRateContractDetailsVO();
                                        objRC = lstRateContractItemDetailsNew.Where(z => ((z.ItemID == itemPO.ItemID && z.ContractID == itemPO.RateContractID) && (z.BaseRate != itemPO.BaseRate || z.BaseMRP != itemPO.BaseMRP))).FirstOrDefault();
                                        if (objRC != null && itemPO.IsRateContractAppliedToItem == true)
                                        {
                                            //enable the link for the same item and bolded
                                            itemPO.IsRateContractLinkEnable = true;
                                        }
                                    }
                                
                                }

                            };
                            msgWD.Show();
                        }
                    }
                };
                client.ProcessAsync(objBizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }            
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                indicator.Close();
            }
        }

        /// <summary>
        /// On Item delete the Rate contract for the item deleted.
        /// </summary>
        private void DeleteItemRateContract()
        {
            if (lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0)
            {
                clsPurchaseOrderDetailVO objPODetails = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem);
                List<clsRateContractItemDetailsVO> lstRateContractDetails = lstRateContractItemDetails.Where(RateContract => RateContract.ItemID == objPODetails.ItemID).ToList();
                if (lstRateContractDetails != null && lstRateContractDetails.Count > 0)
                {
                    foreach (clsRateContractItemDetailsVO objRateContractItem in lstRateContractDetails)
                    {
                        lstRateContractItemDetails.Remove(objRateContractItem);
                    }
                }
            }
        }
        //Added By CDS
        public void FillPODetailsFromAprovalForm()
        {
            try
            {
                txtIndentNo.Text = "";
                clsPurchaseOrderVO obj = SelectedPO;
                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsPurchaseOrderVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsPurchaseOrderVO)this.DataContext).ID = obj.ID;
                    if (obj.Direct == true)
                    {
                        rdbDirect.IsChecked = true;
                        grdpodetails.Columns[3].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[4].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[5].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[6].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[7].Visibility = Visibility.Visible;
                        grdpodetails.Columns[13].IsReadOnly = false;
                        grdpodetails.Columns[15].IsReadOnly = false;
                    }
                    else
                    {
                        rdbIndent.IsChecked = true;
                        grdpodetails.Columns[3].Visibility = Visibility.Visible;
                        grdpodetails.Columns[4].Visibility = Visibility.Visible;
                        grdpodetails.Columns[5].Visibility = Visibility.Visible;
                        grdpodetails.Columns[5].IsReadOnly = true;
                        grdpodetails.Columns[6].Visibility = Visibility.Visible;
                        grdpodetails.Columns[7].Visibility = Visibility.Collapsed;
                        grdpodetails.Columns[13].IsReadOnly = false;
                        grdpodetails.Columns[15].IsReadOnly = false;

                    }
                    var results = from r in ((List<clsStoreVO>)cboStore.ItemsSource)
                                  where r.StoreId == obj.StoreID
                                  select r;

                    foreach (clsStoreVO item in results)
                    {
                        cboStore.SelectedItem = item;
                    }
                    cboStore.IsEnabled = false;

                    var results1 = from r in ((List<MasterListItem>)cboSupplier.ItemsSource)
                                   where r.ID == obj.SupplierID
                                   select r;

                    foreach (MasterListItem item in results1)
                    {
                        cboSupplier.SelectedItem = item;
                        lngSupplierID = item.ID;
                    }

                    var results3 = from r in ((List<clsStoreVO>)cboDelivery.ItemsSource)
                                   where r.StoreId == obj.DeliveryDuration
                                   select r;

                    foreach (clsStoreVO item in results3)
                    {
                        cboDelivery.SelectedItem = item;

                        if (item.StoreId == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID)
                        {
                            cboDelivery.IsEnabled = false;
                        }
                        else
                            cboDelivery.IsEnabled = true;
                    }
                    var results4 = from r in ((List<MasterListItem>)cboTOP.ItemsSource)
                                   where r.ID == obj.PaymentTerms
                                   select r;

                    foreach (MasterListItem item in results4)
                    {
                        cboTOP.SelectedItem = item;
                    }

                    var results5 = from r in ((List<MasterListItem>)cmbSchedule.ItemsSource)
                                   where r.ID == obj.Schedule
                                   select r;

                    foreach (MasterListItem item in results5)
                    {
                        cmbSchedule.SelectedItem = item;
                    }
                    var results6 = from r in ((List<MasterListItem>)cmbDeliveryDays.ItemsSource)
                                   where r.ID == obj.DeliveryDays
                                   select r;

                    foreach (MasterListItem item in results6)
                    {
                        cmbDeliveryDays.SelectedItem = item;
                    }
                    cmbModeofPayment.SelectedValue = obj.PaymentMode;
                    if (obj.Freezed == true && obj.Status == true)
                    {
                        cmdSave.IsEnabled = false;
                        cmdNew.IsEnabled = false;
                        cmdPrint.IsEnabled = true;
                        if (this.IsForDirectApprove) // added by Ashish Z on 280716 for Direct Approve from PO Approval Form
                        {
                            cmdSave.Content = "Approve";
                            cboSupplier.IsEnabled = false;
                            cmbModeofPayment.IsEnabled = false;
                            cboTOP.IsEnabled = false;
                            cboDelivery.IsEnabled = false;
                            cmbDeliveryDays.IsEnabled = false;
                            cmdIndent.IsEnabled = false;
                            txtGuarantee.IsEnabled = false;
                            cmbSchedule.IsEnabled = false;
                            txtOtherCharges.IsEnabled = false;
                            cmbTermsNCondition.IsEnabled = false;
                            txtPODiscount.IsEnabled = false;
                            txtRemarks.IsEnabled = false;
                        }
                        else
                        {
                            cmdSave.Content = "Modify";
                            cboSupplier.IsEnabled = true;
                            cmbModeofPayment.IsEnabled = true;
                            cboTOP.IsEnabled = true;
                            cboDelivery.IsEnabled = true;
                            cmbDeliveryDays.IsEnabled = true;
                            cmdIndent.IsEnabled = true;
                            txtGuarantee.IsEnabled = true;
                            cmbSchedule.IsEnabled = true;
                            txtOtherCharges.IsEnabled = true;
                            cmbTermsNCondition.IsEnabled = true;
                            txtPODiscount.IsEnabled = true;
                            txtRemarks.IsEnabled = true;
                        }
                        cmdSave.IsEnabled = true;
                        cmdCancel.IsEnabled = true;
                        obj.IsEnabledFreezed = false;
                    }
                    else if (obj.Status == false)
                    {
                        cmdSave.IsEnabled = false;
                        cmdNew.IsEnabled = false;
                        cmdCancel.IsEnabled = true;
                    }
                    else
                    {
                        cmdSave.IsEnabled = true;
                        SetCommandButtonState("Modify");
                    }
                    FillPODetails(obj.ID, obj.UnitId);
                    ISEditMode = true;
                    ISEditAndAprove = true;
                    dgPOList.UpdateLayout();
                    dgPOList.Focus();
                    cmdPrint.IsEnabled = false;
                    txtIndentNo.Text = obj.IndentNumber;
                    objAnimation.Invoke(RotationType.Forward);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    cmdPrint.IsEnabled = true;
                    break;

                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdPrint.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    cboStore.SelectedValue = 0;
                    cmbStore.SelectedValue = 0;
                    cboSupplier.SelectedValue = 0;
                    cboSupplier.SelectedValue = 0;
                    cboDelivery.SelectedValue = 0;
                    cmbSchedule.SelectedValue = 0;
                    cboTOP.SelectedValue = 0;
                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Modify";
                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region On Data Grid Cell Edit
        decimal orginalVatPer = 0;
        private void UpdateAllItems(clsPurchaseOrderDetailVO objDetailCopy, clsPurchaseOrderDetailVO objPODetailsVO)
        {
            decimal dVatPercent = 0;
            if (objDetailCopy.DiscountPercent > 100)
                objDetailCopy.DiscountPercent = 0;
            else
                objDetailCopy.DiscountPercent = objPODetailsVO.DiscountPercent;
            if (objPODetailsVO.DiscountPercent == 100 && objPODetailsVO.VATPercent != 0)
            {
                dVatPercent = objPODetailsVO.VATPercent;
                objDetailCopy.VATPercent = 0;
            }
            else
                objDetailCopy.VATPercent = objPODetailsVO.VATPercent;
            if (objPODetailsVO.VATPercent == 0 && objPODetailsVO.DiscountPercent < 100)
            {
                objDetailCopy.VATPercent = dVatPercent;
            }
            else
                objDetailCopy.VATPercent = objPODetailsVO.VATPercent;
            if (objPODetailsVO.VATPercent > 100 || (objPODetailsVO.DiscountPercent == 100 && objPODetailsVO.VATPercent > 0))
            {
                objDetailCopy.VATPercent = 0;
            }
            else objDetailCopy.VATPercent = objPODetailsVO.VATPercent;
            if (objPODetailsVO.MRP < objPODetailsVO.Rate)
            {
                objDetailCopy.MRP = objDetailCopy.Rate + 1;
            }
            else
                objDetailCopy.MRP = objPODetailsVO.MRP;

            objDetailCopy.Rate = objPODetailsVO.Rate;
        }

        void grdpodetails_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            try
            {
                bool IsCellEdit = false;  // By Umesh For Audit Trail
                clsPurchaseOrderDetailVO objPODetailsVO = new clsPurchaseOrderDetailVO();
                objPODetailsVO = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;

                int iRateContractCount = -1;
                //objPODetailsVO.ConditionFound = false;  //Commented for Rate Contract 16042018
                if (lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0)
                {
                    if (objPODetailsVO.RateContractID > 0 && objPODetailsVO.RateContractUnitID > 0 && !String.IsNullOrEmpty(objPODetailsVO.RateContractCondition))
                        iRateContractCount = lstRateContractItemDetails.Where(z => z.ItemID == objPODetailsVO.ItemID && z.RateContractID == objPODetailsVO.RateContractID && z.RateContractUnitID == objPODetailsVO.RateContractUnitID && z.Condition.Trim().Equals(objPODetailsVO.RateContractCondition.Trim())).Count();
                }
                if (e.Column.Header.ToString().Equals("Purchase Quantity"))          //  if (e.Column.Header.ToString().Equals("Pack Quantity"))              
                {
                    #region Commented for Rate Contract 16042018
                    //if (lstRateContractItemDetails != null && lstRateContractItemDetails.Count > 0)
                    //{
                    //    if (iRateContractCount == 1)
                    //    {
                    //        clsRateContractItemDetailsVO objRateContractDetails = lstRateContractItemDetails.Where(z => z.ItemID == objPODetailsVO.ItemID && z.RateContractID == objPODetailsVO.RateContractID && z.RateContractUnitID == objPODetailsVO.RateContractUnitID && z.Condition == objPODetailsVO.RateContractCondition).First();

                    //        switch (objRateContractDetails.Condition)
                    //        {
                    //            case "Between":
                    //                if (Convert.ToDouble(objPODetailsVO.Quantity) >= objRateContractDetails.MinQuantity && Convert.ToDouble(objPODetailsVO.Quantity) <= objRateContractDetails.MaxQuantity)
                    //                {
                    //                    objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                    //                    objPODetailsVO.ConditionFound = true;
                    //                }
                    //                break;
                    //            case ">":
                    //                if (Convert.ToDouble(objPODetailsVO.Quantity) > objRateContractDetails.Quantity)
                    //                {
                    //                    objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                    //                    objPODetailsVO.ConditionFound = true;
                    //                }
                    //                break;
                    //            case "<":
                    //                if (Convert.ToDouble(objPODetailsVO.Quantity) < objRateContractDetails.Quantity)
                    //                {
                    //                    objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                    //                    objPODetailsVO.ConditionFound = true;
                    //                }
                    //                break;
                    //            case "No Limit":
                    //                objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                    //                objPODetailsVO.ConditionFound = true;
                    //                break;
                    //            case "=":
                    //                if (Convert.ToDouble(objPODetailsVO.Quantity) == objRateContractDetails.Quantity)
                    //                {
                    //                    objPODetailsVO.DiscountPercent = Convert.ToDecimal(objRateContractDetails.DiscountPercent);
                    //                    objPODetailsVO.ConditionFound = true;
                    //                }
                    //                break;
                    //        }
                    //    }
                    //    else if ((iRateContractCount > 1 || objPODetailsVO.IsMultipleContract) && !String.IsNullOrEmpty(objPODetailsVO.RateContractCondition) && objPODetailsVO.RateContractCondition.Trim().Equals("No Limit"))
                    //    {
                    //        objPODetailsVO.ConditionFound = true;
                    //    }
                    //    if (!objPODetailsVO.ConditionFound && !String.IsNullOrEmpty(objPODetailsVO.RateContractCondition) && !objPODetailsVO.RateContractCondition.Equals("No Limit") && iRateContractCount != -1)
                    //        if (!objPODetailsVO.ConditionFound && !objPODetailsVO.RateContractCondition.Equals("No Limit") && iRateContractCount != -1)
                    //        {
                    //            objPODetailsVO.DiscountPercent = 0;
                    //        }
                    //}
                    #endregion

                    // Added By CDS 4/02/2016
                    if (objPODetailsVO.POPendingItemQty > 0) //&& objPODetailsVO.POApprItemQty==0
                    {
                        if ((objPODetailsVO.PendingQuantity < ((Convert.ToDecimal(objPODetailsVO.POPendingItemQty) - Convert.ToDecimal(objPODetailsVO.PODetailsViewTimeQty)) + (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor)))) && rdbDirect.IsChecked == false && ISEditMode == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }
                        if (objPODetailsVO.POApprItemQty > 0 && (((objPODetailsVO.PendingQuantity) - Convert.ToDecimal(objPODetailsVO.POPendingItemQty)) < (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }
                        else if ((((objPODetailsVO.PendingQuantity + Convert.ToDecimal(objPODetailsVO.POApprItemQty)) - Convert.ToDecimal(objPODetailsVO.POPendingItemQty)) < (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor)) && rdbDirect.IsChecked == false && ISEditMode == false))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }
                    }
                    else
                    {
                        if (Convert.ToDecimal(objPODetailsVO.POApprItemQty) > 0 && (objPODetailsVO.PendingQuantity < ((objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor)) + Convert.ToDecimal(objPODetailsVO.POApprItemQty))) && (((objPODetailsVO.PendingQuantity + Convert.ToDecimal(objPODetailsVO.POApprItemQty))) < (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }
                        else if ((objPODetailsVO.PendingQuantity < ((objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor)))) && (((objPODetailsVO.PendingQuantity + Convert.ToDecimal(objPODetailsVO.POApprItemQty))) < (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == true)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }

                        if (Convert.ToDecimal(objPODetailsVO.POApprItemQty) > 0 && ((objPODetailsVO.PendingQuantity) < (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }
                        else if ((((objPODetailsVO.PendingQuantity)) < (objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor))) && rdbDirect.IsChecked == false && ISEditMode == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds PR Quantity Against Total Quantity Of PO Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PendingQuantity = objPODetailsVO.PendingQuantity;
                            return;
                        }
                    }
                    if ((objPODetailsVO.PendingQuantity < objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor)) && rdbDirect.IsChecked == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objPODetailsVO.Quantity = 0;
                        objPODetailsVO.PRPendingQuantity = Convert.ToDouble(objPODetailsVO.PendingQuantity - objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor));
                        CalculateOpeningBalanceSummary();
                        return;
                    }
                    else
                    {
                        if (rdbDirect.IsChecked != true)
                        {
                            objPODetailsVO.PRPendingQuantity = Convert.ToDouble(objPODetailsVO.PendingQuantity - objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor));
                            if (IsAuditTrail)   // By Umesh For Audit Trail
                            {
                                LogInformation = new LogInfo();
                                LogInformation.guid = new Guid();
                                LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                                LogInformation.TimeStamp = DateTime.Now;
                                LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                                LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                                LogInformation.Message = " 33 : PR Pending Quantity Change " + "\r\n"
                                                        + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                        + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                        + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                        + "PR Pending Quantity : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Quantity) + " "
                                                        + "\r\n";
                                LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                                LogInfoList.Add(LogInformation);
                            }
                        }
                    }
                }
                if (e.Column.Header.ToString().Equals("Discount %"))
                {
                    #region Commented for Rate Contract 16042018
                    //if (iRateContractCount > 0)
                    //{
                    //    clsRateContractItemDetailsVO objRateContractDetails = lstRateContractItemDetails.Where(z => z.ItemID == objPODetailsVO.ItemID && z.RateContractID == objPODetailsVO.RateContractID && z.RateContractUnitID == objPODetailsVO.RateContractUnitID && z.Condition == objPODetailsVO.RateContractCondition).First();
                    //    if (objRateContractDetails != null && objRateContractDetails.DiscountPercent == Convert.ToDouble(objPODetailsVO.DiscountPercent))
                    //    { }
                    //    else if (objPODetailsVO.RateContractCondition.Equals("No Limit"))
                    //    {
                    //        objPODetailsVO.RateContractID = 0;
                    //        objPODetailsVO.RateContractUnitID = 0;
                    //    }
                    //    else
                    //    {
                    //        objPODetailsVO.RateContractID = 0;
                    //        objPODetailsVO.RateContractUnitID = 0;
                    //        objPODetailsVO.RateContractCondition = string.Empty;
                    //    }
                    //}
                    //else if (iRateContractCount != -1)
                    //{
                    //    objPODetailsVO.RateContractID = 0;
                    //    objPODetailsVO.RateContractUnitID = 0;
                    //    objPODetailsVO.RateContractCondition = string.Empty;
                    //}
                    #endregion
                    if (objPODetailsVO.DiscountPercent < 0)
                    {
                        ShowMessageBox("Discount Percentage can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent > 100)
                    {
                        ShowMessageBox("Discount Percentage Greater Than 100%");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent == 100)
                    {
                        if (objPODetailsVO.VATPercent != 0)
                        {
                            orginalVatPer = objPODetailsVO.VATPercent;
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        }
                    }
                    if (objPODetailsVO.VATPercent == 0)
                    {
                        if (objPODetailsVO.DiscountPercent < 100)
                        {
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = orginalVatPer;
                        }
                    }
                    if (IsAuditTrail && objPODetailsVO.DiscountPercent > 0 && objPODetailsVO.DiscountPercent < 100)   // By Umesh For Audit Trail
                    {
                        IsCellEdit = true;   // By Umesh For Audit Trail
                        LogInformation = new LogInfo();
                        LogInformation.guid = new Guid();
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 38 : Discount % Change " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                + "Discount % : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountPercent) + " ";
                    }
                }

                if (e.Column.Header.ToString().Equals("Item Tax %") || e.Column.Header.ToString().Equals("Discount Amount"))  //e.Column.DisplayIndex == 10 || 
                {
                    objPODetailsVO.RateContractID = 0;
                    objPODetailsVO.RateContractUnitID = 0;
                    objPODetailsVO.RateContractCondition = string.Empty;
                    if (objPODetailsVO.ItemTax < 0)
                    {
                        ShowMessageBox("Item Tax Percentage can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATPercent = 0;
                        return;
                    }

                    if (objPODetailsVO.ItemVATPercent > 100)
                    {
                        ShowMessageBox("Item Tax Percentage Greater Than 100%");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent == 100)
                    {
                        if (objPODetailsVO.ItemVATPercent > 0)
                        {
                            ShowMessageBox("Discount percentage 100%. Can't Add Vat Percent");
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATPercent = 0;
                            return;
                        }
                    }
                    if (IsAuditTrail && objPODetailsVO.ItemVATPercent > 0 && objPODetailsVO.ItemVATPercent < 100)   // By Umesh For Audit Trail
                    {
                        IsCellEdit = true;   // By Umesh For Audit Trail
                        LogInformation = new LogInfo();
                        LogInformation.guid = new Guid();
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 37 : Item Tax % Change " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                + "Item Tax % : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATPercent) + " ";
                    }
                }

                if (e.Column.Header.ToString().Equals("Discount Amount"))  //e.Column.DisplayIndex == 10 || 
                {
                    //objPODetailsVO.RateContractID = 0;
                    //objPODetailsVO.RateContractUnitID = 0;
                    //objPODetailsVO.RateContractCondition = string.Empty;
                    if (objPODetailsVO.VATPercent < 0)
                    {
                        ShowMessageBox("VAT Percentage can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        return;
                    }

                    if (objPODetailsVO.VATPercent > 100)
                    {
                        ShowMessageBox("VAT Percentage Greater Than 100%");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                        return;
                    }
                    if (objPODetailsVO.DiscountPercent == 100)
                    {
                        if (objPODetailsVO.VATPercent > 0)
                        {
                            ShowMessageBox("Discount percentage 100%. Can't Add Vat Percent");
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATPercent = 0;
                            return;
                        }
                    }
                    if (IsAuditTrail && objPODetailsVO.VATPercent > 0 && objPODetailsVO.VATPercent < 100 && objPODetailsVO.DiscountPercent != 100)   // By Umesh For Audit Trail
                    {
                        IsCellEdit = true;   // By Umesh For Audit Trail
                        LogInformation = new LogInfo();
                        LogInformation.guid = new Guid();
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 37 : Item Tax % Change " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                + "Item Tax % : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATPercent) + " ";
                    }
                }

                if (e.Column.Header.ToString().Equals("Pack M.R.P"))  //e.Column.DisplayIndex == 7 || 
                {
                    if (objPODetailsVO.MRP < 0)
                    {
                        ShowMessageBox("MRP can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP = 0;
                        return;
                    }

                    if (objPODetailsVO.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                    {
                        if (objPODetailsVO.MRP < objPODetailsVO.Rate)
                        {
                            ShowMessageBox("MRP must be greater than the cost price.");
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP = objPODetailsVO.Rate + 1;
                            return;
                        }
                    }
                }



                if (e.Column.Header.ToString().Equals("Pack Cost Price"))  //e.Column.DisplayIndex == 5 || 
                {
                    if (objPODetailsVO.Rate < 0)
                    {
                        ShowMessageBox("Cost Price can not be Negative");
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate = 0;
                        return;
                    }
                }

                // Reset Base Cost Price if user edit the Cost Price
                if (e.Column.Header.ToString().Equals("Purchase Cost Price"))   //e.Column.DisplayIndex == 13 || 
                {
                    if (grdpodetails.SelectedItem != null && ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor > 0)
                    {
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseRate = Convert.ToSingle(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate) / ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor;
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainRate = Convert.ToSingle(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate) / ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor;
                        if (IsAuditTrail)   // By Umesh For Audit Trail
                        {
                            IsCellEdit = true;   // By Umesh For Audit Trail
                            LogInformation = new LogInfo();
                            LogInformation.guid = new Guid();
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 35 : Purchase Cost Price Change " + "\r\n"
                                                    + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                    + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                    + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                    + "Base Rate : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseRate) + " "
                                                    + "Main Rate : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseMRP) + " ";
                        }
                    }
                }

                // Reset Base MRP if user edit the MRP
                if (e.Column.Header.ToString().Equals("M.R.P"))  //e.Column.DisplayIndex == 15 || 
                {
                    if (grdpodetails.SelectedItem != null && ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor > 0)
                    {
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseMRP = Convert.ToSingle(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP) / ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor;
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainMRP = Convert.ToSingle(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP) / ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor;
                        if (IsAuditTrail)   // By Umesh For Audit Trail
                        {
                            IsCellEdit = true;   // By Umesh For Audit Trail
                            LogInformation = new LogInfo();
                            LogInformation.guid = new Guid();
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 35 : Purchase Cost Price Change " + "\r\n"
                                                    + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                    + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                    + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                    + "Base MRP : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseMRP) + " "
                                                    + "Main MRP : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainMRP) + " ";
                        }
                    }
                }
                if (e.Column.Header.ToString().Equals("Purchase Quantity"))
                    if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList == null || ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList.Count == 0)
                    {
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseQuantity = Convert.ToSingle(Convert.ToSingle(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Quantity) * ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor);
                        if (IsAuditTrail)   // By Umesh For Audit Trail
                        {
                            IsCellEdit = true;   // By Umesh For Audit Trail
                            LogInformation = new LogInfo();
                            LogInformation.guid = new Guid();
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 32 : Quantity Change " + "\r\n"
                                                    + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                    + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                    + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                    + "Change Quantity : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Quantity) + " "
                                                    + "Base Quantity : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseQuantity) + " "
                                                    + "Item Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Amount) + " "
                                                    + "Item Discount Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountAmount) + " "
                                                    + "Item Tax Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATAmount) + " "
                                                    + "Item VAT Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATAmount) + " "
                                                    + "Item Net Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).NetAmount) + " "
                                                    + "\r\n";
                        }
                    }
                    else if (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM != null && ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM.ID > 0)
                    {
                        CalculateConversionFactorCentral(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM.ID, ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SUOMID);
                    }
                    else
                    {
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).RequiredQuantity = 0;
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Quantity = 0;

                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ConversionFactor = 0;
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor = 0;

                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP = Convert.ToDecimal(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainMRP);
                        ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate = Convert.ToDecimal(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainRate);

                    }

                foreach (var item in PurchaseOrderItems)
                {
                    if (item.IndentID > 0 && objPODetailsVO.IndentID == item.IndentID && objPODetailsVO.IndentUnitID == item.IndentUnitID && objPODetailsVO.ItemID == item.ItemID)//if (objPODetailsVO.ItemID == item.ItemID && item.IndentID > 0)
                    {
                        item.DiscountPercent = objPODetailsVO.DiscountPercent;
                        item.VATPercent = objPODetailsVO.VATPercent;
                        item.ItemVATPercent = objPODetailsVO.ItemVATPercent;
                        //added for Single Line Item from Diff. PR on 14-03-2016
                        item.Rate = objPODetailsVO.Rate;
                        item.BaseRate = objPODetailsVO.BaseRate;
                        item.MainRate = objPODetailsVO.MainRate;
                        item.MRP = objPODetailsVO.MRP;
                        item.BaseMRP = objPODetailsVO.BaseMRP;
                        item.MainMRP = objPODetailsVO.MainMRP;
                    }
                }

                grdpodetails.UpdateLayout();

                if (grdpodetails.ItemsSource != null)
                {
                    CalculateOpeningBalanceSummary();
                }

                // Added To Calculate NetAmount On Cell Change
                if (grdpodetails.SelectedItem != null)
                {
                    CalculateOpeningBalanceSummary();
                    if (IsAuditTrail && IsCellEdit)
                    {
                        LogInformation.Message = LogInformation.Message
                                                    + "Gross Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalAmount) + " "
                                                    + "Others Charges : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).OtherCharges) + " "
                                                    + "PO Discount Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalDiscount) + " "
                                                    + "PO Discount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).PODiscount) + " "
                                                    + "PO VAT Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalVAT) + " "
                                                    + "PO Net Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalNet) + " ";
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                        IsCellEdit = false;   // By Umesh For Audit Trail
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        //...................................
        private void FillUOMConversions()
        {
            WaitIndicator IndicatiorConversions = new WaitIndicator();
            try
            {
                IndicatiorConversions.Show();
                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();
                BizAction.ItemID = Convert.ToInt64(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID);
                BizAction.UOMConversionList = new List<clsConversionsVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        IndicatiorConversions.Close();
                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);
                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);
                        if (grdpodetails.SelectedItem != null)
                        {
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }
                        CalculateConversionFactorCentral(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM.ID, ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SUOMID);
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                IndicatiorConversions.Close();
                throw;
            }
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;
        }


        private void ShowMessageBox(string strMessage)
        {
            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            msgW3.Show();
        }

        #endregion

        #region calculate summary
        private void CalculateOpeningBalanceSummary()
        {
            decimal VATAmount, Amount, NetAmount1, DiscountAmount, othertax, NetAmount, SGSTAmount, CGSTAmount, IGSTAmount; //SGSTAmount,CGSTAmount,IGSTAmount Added By Bhushanp For GST 22062017
            VATAmount = Amount = NetAmount1 = DiscountAmount = othertax = NetAmount = SGSTAmount = CGSTAmount = IGSTAmount = 0;

            decimal OtherCha, PODis;
            OtherCha = PODis = 0;

            ObservableCollection<clsPurchaseOrderDetailVO> ocPODetails = ((ObservableCollection<clsPurchaseOrderDetailVO>)(grdpodetails.ItemsSource));
            if (ocPODetails != null)
            {
                foreach (var item in ocPODetails.ToList())
                {
                    //Amount += item.Amount; // Commnted By CDS
                    Amount += item.CostRate;
                    DiscountAmount += item.DiscountAmount;
                    VATAmount += item.VATAmount;
                    othertax += item.ItemVATAmount;
                    SGSTAmount += item.SGSTAmount;
                    CGSTAmount += item.CGSTAmount;
                    IGSTAmount += item.IGSTAmount;
                    item.NetAmount = ((item.Rate * item.Quantity) - item.DiscountAmount) + item.VATAmount + item.ItemVATAmount + item.SGSTAmount + item.CGSTAmount + item.IGSTAmount;
                    NetAmount1 += item.NetAmount;  // use to set for Net Amount (later which get added with Other Charges & Discount Amount on whole PO)
                    OtherCha = item.OtherCharges;
                    PODis = item.PODiscount;
                }
            }
            ((clsPurchaseOrderVO)this.DataContext).TotalAmount = Amount;
            ((clsPurchaseOrderVO)this.DataContext).TotalDiscount = DiscountAmount;
            ((clsPurchaseOrderVO)this.DataContext).TotalVAT = VATAmount + othertax;  //Added by CDS
            //Added By Bhushanp For GST 22062017
            ((clsPurchaseOrderVO)this.DataContext).TotalSGST = SGSTAmount;
            ((clsPurchaseOrderVO)this.DataContext).TotalCGST = CGSTAmount;
            ((clsPurchaseOrderVO)this.DataContext).TotalIGST = IGSTAmount;
            // Added By CDS
            ((clsPurchaseOrderVO)this.DataContext).PrevTotalNet = NetAmount1;
            ((clsPurchaseOrderVO)this.DataContext).OtherCharges = OtherCha;
            ((clsPurchaseOrderVO)this.DataContext).PODiscount = PODis;
            //OtherCharges =OtherCha.ToString();
            ((clsPurchaseOrderVO)this.DataContext).TotalNet = NetAmount1 + OtherCha - PODis;
        }
        #endregion

        #region SelectionChanged/Checked/TextChanged/KeyDown/LostFocus Control Events
        private void rdbIndent_Checked(object sender, RoutedEventArgs e)
        {
            if (PurchaseOrderItems != null)
            {
                BdrItemSearch.Visibility = System.Windows.Visibility.Collapsed;
                PurchaseOrderItems.Clear();
                POIndentdetails.Clear();
                txtIndentNo.Text = "";
                IndentNo = "";
                if (grdpodetails.ItemsSource != null)
                    CalculateOpeningBalanceSummary();
            }
        }

        private long StoreID { get; set; }
        private long SupplierID { get; set; }
        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboStore.SelectedItem != null)
            {
                if (cboStoreState.ItemsSource != null)
                {
                    cboStoreState.SelectedValue = ((clsStoreVO)cboStore.SelectedItem).StateID;
                }
                if (PurchaseOrderItems != null)
                {
                    txtIndentNo.Text = String.Empty;
                    PurchaseOrderItems.Clear();
                    POIndentdetails.Clear();
                    CalculateOpeningBalanceSummary();
                }
                StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                if (StoreID > 0 && SupplierID > 0 && rdbDirect.IsChecked == true)
                {
                }
            }
        }

        private void rdbDirect_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StoreID > 0 && SupplierID > 0 && rdbDirect.IsChecked == true)
                {
                }
                if (IsPageLoded)
                {
                    if (PurchaseOrderItems != null)
                    {
                        PurchaseOrderItems.Clear();
                        POIndentdetails.Clear();
                        txtIndentNo.Text = "";
                        if (grdpodetails.ItemsSource != null)
                            CalculateOpeningBalanceSummary();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void cboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClsGetSupplierDetailsBizActionVO objBizActionVO = new ClsGetSupplierDetailsBizActionVO();

            if (cboSupplier.SelectedItem != null)
            {
                objBizActionVO.SupplierId = ((MasterListItem)cboSupplier.SelectedItem).ID;
                SupplierID = ((MasterListItem)cboSupplier.SelectedItem).ID;
                if (cboSupplierState.ItemsSource != null)
                {
                    cboSupplierState.SelectedValue = ((MasterListItem)cboSupplier.SelectedItem).StateID;
                }
            }
            if (StoreID > 0 && SupplierID > 0 && rdbDirect.IsChecked == true)
            {
            }
            //if (objBizActionVO.SupplierId != lngSupplierID)
            //{
            //    PurchaseOrderItems.Clear();
            //    POIndentdetails.Clear();
            //    POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
            //    txtIndentNo.Text = String.Empty;
            //    CalculateOpeningBalanceSummary();
            //}
            if (objBizActionVO.SupplierId > 0)
            {
                PurchaseOrderItems.Clear();
                POIndentdetails.Clear();
                POIndentdetails = new ObservableCollection<clsPurchaseOrderDetailVO>();
                txtIndentNo.Text = String.Empty;
                CalculateOpeningBalanceSummary();
            }

            objBizActionVO.SupplierPaymentMode = true;
            objBizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        ClsGetSupplierDetailsBizActionVO objPurchaseOrderList = ((ClsGetSupplierDetailsBizActionVO)arg.Result);
                        if (objPurchaseOrderList.ModeOfPayment > 0)
                            cmbModeofPayment.SelectedValue = objPurchaseOrderList.ModeOfPayment;
                    }
                }
                else
                {
                    System.Windows.Browser.HtmlPage.Window.Alert("Error occured while processing.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStore.SelectedItem != null && (cmbStore.SelectedItem as clsStoreVO).StoreId > 0)
            {
                FillPurchaseOrderDataGrid();
            }
        }

        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSupplier.SelectedItem != null && (cmbSupplier.SelectedItem as MasterListItem).ID > 0)
            {
                FillPurchaseOrderDataGrid();
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

        private void txtOtherCharges_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsValueDouble()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void txtPODiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsValueDouble()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtOtherCharges_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtPODiscount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtOtherCharges_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtOtherCharges.Text) && txtOtherCharges.Text.IsValueDouble())
            {
                CalculateNewNetAmount();
            }
            else
            {
                txtOtherCharges.Text = "0.000";
                ((clsPurchaseOrderVO)this.DataContext).OtherCharges = Convert.ToDecimal(txtOtherCharges.Text);
                CalculateNewNetAmount();
            }
        }

        private void txtPODiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPODiscount.Text) && txtPODiscount.Text.IsValueDouble())
            {
                CalculateNewNetAmount();
            }
            else
            {
                txtPODiscount.Text = "0.000";
                ((clsPurchaseOrderVO)this.DataContext).PODiscount = Convert.ToDecimal(txtPODiscount.Text);
                CalculateNewNetAmount();
            }
        }
        #endregion

        #region Conversion Factor/ UOM
        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList == null || ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList.Count == 0))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        WaitIndicator Indicatior = null;
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();
                BizAction.ItemID = Convert.ToInt64(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID);
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
                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (grdpodetails.SelectedItem != null)
                        {
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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

        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;
                ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Quantity = 0;
                ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).RequiredQuantity = 0;
                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SUOMID);
                }
                else
                {
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ConversionFactor = 0;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor = 0;
                }
            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (grdpodetails.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;
                    objConversionVO.MainMRP = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Quantity);
                    long BaseUOMID = ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseUOMID;
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate = Convert.ToDecimal(objConversionVO.Rate);
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP = Convert.ToDecimal(objConversionVO.MRP);
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).RequiredQuantity = objConversionVO.Quantity;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).StockingQuantity = objConversionVO.Quantity;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseMRP = objConversionVO.BaseMRP;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;
                    clsPurchaseOrderDetailVO objPODetailsVO = (clsPurchaseOrderDetailVO)grdpodetails.SelectedItem;
                    if (grdpodetails.SelectedItem != null)
                    {
                        if ((objPODetailsVO.PendingQuantity < objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor)) && rdbDirect.IsChecked == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objPODetailsVO.Quantity = 0;
                            objPODetailsVO.PRPendingQuantity = Convert.ToDouble(objPODetailsVO.PendingQuantity - objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor));
                            CalculateOpeningBalanceSummary();
                            return;
                        }
                        else
                        {
                            if (rdbDirect.IsChecked != true)
                            {
                                objPODetailsVO.PRPendingQuantity = Convert.ToDouble(objPODetailsVO.PendingQuantity - objPODetailsVO.Quantity * Convert.ToDecimal(objPODetailsVO.BaseConversionFactor));
                            }
                        }
                    }
                    CalculateOpeningBalanceSummary();
                    if (IsAuditTrail)   // By Umesh For Audit Trail
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = new Guid();
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 34 : Transaction UOM Change " + "\r\n"
                                                + "User Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Item ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID) + " "
                                                + "Item Name : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemName) + " "
                                                + "Selected UOM : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM.Description) + " "
                                                + "Selected UOM ID : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM.ID) + " "
                                                + "Base Quantity : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseQuantity) + " "
                                                + "Required Quantity : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).RequiredQuantity) + " "
                                                + "Stocking Quantity : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).StockingQuantity) + " "
                                                + "Purchase Cost Price : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Rate) + " "
                                                + "MRP : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).MRP) + " "
                                                + "Base Rate : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseRate) + " "
                                                + "Base MRP : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseMRP) + " "
                                                + "Conversion Factor : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ConversionFactor) + " "
                                                + "Base Conversion Factor : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).BaseConversionFactor) + " "
                                                + "Item Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).Amount) + " "
                                                + "Item Discount Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).DiscountAmount) + " "
                                                + "Item Tax Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemVATAmount) + " "
                                                + "Item VAT Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).VATAmount) + " "
                                                + "Item Net Amount : " + Convert.ToString(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).NetAmount) + " "
                                                + "Gross Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalAmount) + " "
                                                + "Others Charges : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).OtherCharges) + " "
                                                + "PO Discount Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalDiscount) + " "
                                                + "PO Discount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).PODiscount) + " "
                                                + "PO VAT Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalVAT) + " "
                                                + "PO Net Amount : " + Convert.ToString(((clsPurchaseOrderVO)this.DataContext).TotalNet) + "\r\n";
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID), ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM.ID);
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
            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;
            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).SUOMID);
        }
        #endregion

        private void grdpodetails_LoadingRow(object sender, DataGridRowEventArgs e) // added by Ashish Z on 280716 for Direct Approve from PO Approval Form
        {
            if (this.IsForDirectApprove)
            {
                e.Row.IsEnabled = false;
            }
            else
            {
                e.Row.IsEnabled = true;
            }
        }

        private void HyperlinkPOListButton_Click(object sender, RoutedEventArgs e)
        {
            if (grdpodetails.SelectedItem != null)
            {
                PurchaseInfo win = new PurchaseInfo();
                win.POLastPurchase(Convert.ToInt64(((clsPurchaseOrderDetailVO)grdpodetails.SelectedItem).ItemID), ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);               
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }
    }
}
