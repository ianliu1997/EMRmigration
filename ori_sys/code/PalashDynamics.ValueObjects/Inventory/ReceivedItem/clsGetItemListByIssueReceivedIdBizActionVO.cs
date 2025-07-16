using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Inventory
{

    public class clsGetItemListByIssueReceivedIdBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListByIssueReceivedIdBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {

                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }

        public List<clsReceivedItemDetailsVO> ItemList { get; set; }
        public long? ReceivedId { get; set; }
        public long? UnitID { get; set; }
    }

    public class clsReceivedItemDetailsVO : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        # region Conversion Factor

        private String _ReceivedUOM;
        public String ReceivedUOM
        {
            get
            {
                return _ReceivedUOM;
            }
            set
            {
                if (value != _ReceivedUOM)
                {
                    _ReceivedUOM = value;
                    OnPropertyChanged("ReceivedUOM");
                }
            }
        }

        private String _IssuedUOM;
        public String IssuedUOM
        {
            get
            {
                return _IssuedUOM;
            }
            set
            {
                if (value != _IssuedUOM)
                {
                    _IssuedUOM = value;
                    OnPropertyChanged("IssuedUOM");
                }
            }
        }

        private float _SingleQuantity;
        public float SingleQuantity
        {
            get
            {

                return _SingleQuantity;
            }
            set
            {
                if (_SingleQuantity != value)
                {

                    _SingleQuantity = value;
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("RequiredQuantity");

                }
            }
        }

        private long _UOMID;
        public long UOMID
        {
            get { return _UOMID; }
            set
            {
                if (_UOMID != value)
                {
                    _UOMID = value;
                    OnPropertyChanged("UOMID");
                }
            }
        }

        private string _SUOM;
        public string SUOM
        {
            get
            {
                return _SUOM;
            }
            set
            {
                if (value != _SUOM)
                {
                    _SUOM = value;
                    OnPropertyChanged("SUOM");
                }
            }
        }

        private long _SUOMID;
        public long SUOMID
        {
            get { return _SUOMID; }
            set
            {
                if (_SUOMID != value)
                {
                    _SUOMID = value;
                    OnPropertyChanged("SUOMID");
                }
            }
        }

        private double _StockingQuantity;
        public double StockingQuantity
        {
            get { return _StockingQuantity; }
            set
            {
                if (_StockingQuantity != value)
                {
                    _StockingQuantity = value;
                    OnPropertyChanged("StockingQuantity");
                }
            }
        }

        MasterListItem _SelectedUOM = new MasterListItem();
        public MasterListItem SelectedUOM
        {
            get
            {
                return _SelectedUOM;
            }
            set
            {
                if (value != _SelectedUOM)
                {
                    _SelectedUOM = value;
                    OnPropertyChanged("SelectedUOM");
                }
            }


        }

        List<MasterListItem> _UOMList = new List<MasterListItem>();
        public List<MasterListItem> UOMList
        {
            get
            {
                return _UOMList;
            }
            set
            {
                if (value != _UOMList)
                {
                    _UOMList = value;

                }
            }

        }

        List<clsConversionsVO> _UOMConversionList = new List<clsConversionsVO>();
        public List<clsConversionsVO> UOMConversionList
        {
            get
            {
                return _UOMConversionList;
            }
            set
            {
                if (value != _UOMConversionList)
                {
                    _UOMConversionList = value;
                    OnPropertyChanged("UOMConversionList");
                }
            }

        }

        private long _PUOMID;
        public long PUOMID
        {
            get { return _PUOMID; }
            set
            {
                if (_PUOMID != value)
                {
                    _PUOMID = value;
                    OnPropertyChanged("PUOMID");
                }
            }
        }

        private long _BaseUOMID;
        public long BaseUOMID
        {
            get { return _BaseUOMID; }
            set
            {
                if (_BaseUOMID != value)
                {
                    _BaseUOMID = value;
                    OnPropertyChanged("BaseUOMID");
                }
            }
        }


        private long _SellingUOMID;
        public long SellingUOMID
        {
            get { return _SellingUOMID; }
            set
            {
                if (_SellingUOMID != value)
                {
                    _SellingUOMID = value;
                    OnPropertyChanged("SellingUOMID");
                }
            }
        }

        private string _BaseUOM;
        public string BaseUOM
        {
            get { return _BaseUOM; }
            set
            {
                if (_BaseUOM != value)
                {
                    _BaseUOM = value;
                    OnPropertyChanged("BaseUOM");
                }
            }
        }


        private string _SellingUOM;
        public string SellingUOM
        {
            get { return _SellingUOM; }
            set
            {
                if (_SellingUOM != value)
                {
                    _SellingUOM = value;
                    OnPropertyChanged("SellingUOM");
                }
            }
        }

        private float _BaseConversionFactor;
        public float BaseConversionFactor
        {
            get { return _BaseConversionFactor; }
            set
            {
                if (_BaseConversionFactor != value)
                {
                    _BaseConversionFactor = value;
                    OnPropertyChanged("BaseConversionFactor");
                }
            }
        }



        private float _BaseQuantity;
        public float BaseQuantity
        {
            get
            {

                return _BaseQuantity;
            }
            set
            {
                if (_BaseQuantity != value)
                {

                    _BaseQuantity = value;
                    OnPropertyChanged("BaseQuantity");
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");



                }
            }
        }

        private float _BaseRate;
        public float BaseRate
        {
            get { return _BaseRate; }
            set
            {
                if (_BaseRate != value)
                {
                    _BaseRate = value;
                    OnPropertyChanged("BaseRate");
                }
            }
        }


        private float _BaseMRP;
        public float BaseMRP
        {
            get
            {
                _BaseMRP = (float)Math.Round((decimal)_BaseMRP, 2);
                return _BaseMRP;
            }
            set
            {
                if (value != _BaseMRP)
                {
                    _BaseMRP = value;
                    OnPropertyChanged("BaseMRP");
                }
            }
        }

        # endregion

        private float _ConversionFactor;
        public float ConversionFactor
        {
            get
            {
                return _ConversionFactor;
            }
            set
            {
                if (value != _ConversionFactor)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
                }
            }
        }
        private Boolean? _IsChecked = false;
        public Boolean? IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

        private Boolean? _IsEnable;
        public Boolean? IsEnable
        {
            get
            {
                // if (_AvailableStock == 0 || _AvailableStock == null || _ExpiryDate < DateTime.Now || _ExpiryDate == null)
                //_IsEnable = false;
                // else
                //  _IsEnable = true;

                return _IsEnable;
            }
            set
            {
                if (value != _IsEnable)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }

        private long _ItemId;
        public long ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                if (value != _ItemId)
                {
                    _ItemId = value;
                    OnPropertyChanged("ItemId");
                }
            }
        }

        private long _IssueDetailsID;
        public long IssueDetailsID
        {
            get
            {
                return _IssueDetailsID;
            }
            set
            {
                if (value != _IssueDetailsID)
                {
                    _IssueDetailsID = value;
                    OnPropertyChanged("IssueDetailsID");
                }
            }
        }

        private String _ItemCode;
        public String ItemCode
        {
            get
            {
                return _ItemCode;
            }
            set
            {
                if (value != _ItemCode)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private String _ItemName;
        public String ItemName
        {
            get
            {
                return _ItemName;
            }
            set
            {
                if (value != _ItemName)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private long? _BatchId;
        public long? BatchId
        {
            get
            {
                return _BatchId;
            }
            set
            {
                if (value != _BatchId)
                {
                    _BatchId = value;
                    OnPropertyChanged("BatchId");
                }
            }
        }

        private String _BatchCode;
        public String BatchCode
        {
            get
            {
                return _BatchCode;
            }
            set
            {
                if (value != _BatchCode)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get
            {
                return _ExpiryDate;
            }
            set
            {
                if (value != _ExpiryDate)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                    OnPropertyChanged("IsEnable");
                }
            }
        }


        private decimal _IssueQty;
        public decimal IssueQty
        {
            get
            {
                return _IssueQty;
            }
            set
            {
                if (value != _IssueQty)
                {
                    _IssueQty = value;
                    OnPropertyChanged("IssueQty");
                }
            }
        }

        private float _IssueQtyBaseCF;
        public float IssueQtyBaseCF
        {
            get { return _IssueQtyBaseCF; }
            set
            {
                if (_IssueQtyBaseCF != value)
                {
                    _IssueQtyBaseCF = value;
                    OnPropertyChanged("IssueQtyBaseCF");
                }
            }
        }

        private long _IssueUnitID;
        public long IssueUnitID
        {
            get
            {
                return _IssueUnitID;
            }
            set
            {
                if (value != _IssueUnitID)
                {
                    _IssueUnitID = value;
                    OnPropertyChanged("IssueUnitID");
                }
            }
        }

        #region Added By Pallavi
        private decimal _BalanceQty;
        public decimal BalanceQty
        {
            get
            {
                return _BalanceQty;
            }
            set
            {
                if (value != _BalanceQty)
                {
                    _BalanceQty = value;
                    OnPropertyChanged("BalanceQty");
                }
            }
        }
        #endregion

        private decimal _ReceivedQty;
        public decimal ReceivedQty
        {
            get
            {
                return _ReceivedQty;
            }
            set
            {
                if (value != _ReceivedQty)
                {
                    //_ReceivedQty = System.Math.Round(value, 1);
                    //if (((int)value).ToString().Length > 5)
                    //{
                    //    _ReceivedQty = 1;
                    //    throw new ValidationException("Received Quantity Length Should Not Be Greater Than 5 Digits.");
                    //}
                    //else if (value < 0)
                    //    _ReceivedQty = 1;
                    //else
                    _ReceivedQty = value;
                    OnPropertyChanged("ReceivedQty");
                    OnPropertyChanged("PurchaseRate");
                    OnPropertyChanged("PurchaseRateAmt");
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("MRPAmt");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");



                    OnPropertyChanged("ItemTotalAmount");
                    OnPropertyChanged("ItemVATAmount");

                    //if (value.ToString().Length > 5)
                    //{
                    //    _ReceivedQty = 0;
                    //    throw new ValidationException("Received Quantity Length Should Not Be Greater Than 5 Digits.");
                    //}
                    //else if (value < 0)
                    //    _ReceivedQty = 1;
                    //else
                    //    _ReceivedQty = value;
                    //OnPropertyChanged("ReceivedQty");
                    ////ItemTotalAmount = ReceivedQty * PurchaseRate;
                    //ItemVATAmount = ItemTotalAmount * (ItemVATPercentage / 100);
                    //// BalanceQty = ReceivedQty-ReceivedQty ;
                }
            }
        }
        //By Anjali..........................................
        //private bool _IsIndent;
        //public bool IsIndent
        //{
        //    get
        //    {
        //        return _IsIndent;
        //    }
        //    set
        //    {
        //        if (value != _IsIndent)
        //        {
        //            _IsIndent = value;
        //            OnPropertyChanged("IsIndent");

        //        }
        //    }
        //}
        private int _IsIndent;
        public int IsIndent
        {
            get
            {
                return _IsIndent;
            }
            set
            {
                if (value != _IsIndent)
                {
                    _IsIndent = value;
                    OnPropertyChanged("IsIndent");

                }
            }
        }

        private bool _IsItemBlock;
        public bool IsItemBlock
        {
            get
            {
                return _IsItemBlock;
            }
            set
            {
                if (value != _IsItemBlock)
                {
                    _IsItemBlock = value;
                    OnPropertyChanged("IsItemBlock");

                }
            }
        }
        //........................................................

        private decimal? _ItemTotalAmount;
        public decimal? ItemTotalAmount
        {
            get
            {
                //decimal _temp = Convert.ToDecimal(BaseQuantity);
                //return _temp * PurchaseRate;  //return ReceivedQty * PurchaseRate;
                return _ItemTotalAmount;
            }
            set
            {
                if (value != _ItemTotalAmount)
                {
                    _ItemTotalAmount = value;
                    OnPropertyChanged("ItemTotalAmount");
                }
            }
        }

        private decimal? _ReceiveItemTotalAmount;
        public decimal? ReceiveItemTotalAmount
        {
            get
            {

                return _ReceiveItemTotalAmount;
            }
            set
            {
                if (value != _ReceiveItemTotalAmount)
                {
                    _ReceiveItemTotalAmount = value;
                    OnPropertyChanged("ReceiveItemTotalAmount");
                }
            }
        }

        private string _Rack;
        public string Rack
        {
            get
            {
                return _Rack;
            }
            set
            {
                if (value != _Rack)
                {
                    _Rack = value;
                    OnPropertyChanged("Rack");
                }
            }
        }

        private string _Shelf;
        public string Shelf
        {
            get
            {
                return _Shelf;
            }
            set
            {
                if (value != _Shelf)
                {
                    _Shelf = value;
                    OnPropertyChanged("Shelf");
                }
            }
        }

        private string _Bin;
        public string Bin
        {
            get
            {
                return _Bin;
            }
            set
            {
                if (value != _Bin)
                {
                    _Bin = value;
                    OnPropertyChanged("Bin");
                }
            }
        }

        private string _GRNNo;
        public string GRNNo
        {
            get
            {
                return _GRNNo;
            }
            set
            {
                if (value != _GRNNo)
                {
                    _GRNNo = value;
                    OnPropertyChanged("GRNNo");
                }
            }
        }

        private string _SupplierName;
        public string SupplierName
        {
            get
            {
                return _SupplierName;
            }
            set
            {
                if (value != _SupplierName)
                {
                    _SupplierName = value;
                    OnPropertyChanged("SupplierName");
                }
            }
        }

        private string _BarCode;  //***//
        public string BarCode
        {
            get { return _BarCode; }
            set
            {
                if (_BarCode != value)
                {
                    _BarCode = value;
                    OnPropertyChanged("BarCode");
                }
            }
        }

        public long GRNID { get; set; }
        public long GRNUnitID { get; set; }
        public long GRNDetailID { get; set; }
        public long GRNDetailUnitID { get; set; }

        #region For Quarantine Items (Expired, DOS)
        // Use For Vat/Tax Calculations
        private Boolean _InclusiveOfTax;
        public Boolean InclusiveOfTax
        {
            get
            {
                return _InclusiveOfTax;
            }
            set
            {
                if (value != _InclusiveOfTax)
                {
                    _InclusiveOfTax = value;
                    OnPropertyChanged("InclusiveOfTax");
                }
            }
        }

        private decimal? _PurchaseRate;
        public decimal? PurchaseRate
        {
            get
            {
                return _PurchaseRate;
            }
            set
            {
                if (value != _PurchaseRate)
                {
                    _PurchaseRate = value;
                    OnPropertyChanged("PurchaseRate");
                    OnPropertyChanged("PurchaseRateAmt");
                    OnPropertyChanged("MRPAmt");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal? _MRP;
        public decimal? MRP
        {
            get
            {
                return _MRP;
            }
            set
            {
                if (value != _MRP)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("MRPAmt");
                    OnPropertyChanged("PurchaseRateAmt");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private float _MainRate;
        public float MainRate
        {
            get { return _MainRate; }
            set
            {
                if (_MainRate != value)
                {
                    _MainRate = value;
                    OnPropertyChanged("MainRate");
                }
            }
        }

        private float _MainMRP;
        public float MainMRP
        {
            get { return _MainMRP; }
            set
            {
                if (_MainMRP != value)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
                }
            }
        }

        private decimal? _ItemVATPercentage;
        public decimal? ItemVATPercentage
        {
            get
            {
                return _ItemVATPercentage;
            }
            set
            {
                if (value != _ItemVATPercentage)
                {
                    _ItemVATPercentage = value;
                    OnPropertyChanged("ItemVATPercentage");
                }
            }
        }

        private int _GRNItemVatApplicationOn;
        public int GRNItemVatApplicationOn
        {
            get { return _GRNItemVatApplicationOn; }
            set
            {
                if (_GRNItemVatApplicationOn != value)
                {
                    _GRNItemVatApplicationOn = value;
                }
            }
        }

        private int _GRNItemVatType;
        public int GRNItemVatType
        {
            get { return _GRNItemVatType; }
            set
            {
                if (_GRNItemVatType != value)
                {
                    _GRNItemVatType = value;
                }
            }
        }

        private decimal? _ItemVATAmount;
        public decimal? ItemVATAmount
        {
            get
            {
                //return ItemTotalAmount * (ItemVATPercentage / 100);
                if (_ItemVATPercentage != 0)
                {
                    if (_GRNItemVatType == 2)  // For Exlusive
                    {
                        if (_GRNItemVatApplicationOn == 1)  // For Applicable on CP (Rate)
                        {
                            _ItemVATAmount = ((_PurchaseRateAmt * _ItemVATPercentage) / 100);  //_PurchaseRateAmt- _CDiscountAmount - _SchDiscountAmount
                            return _ItemVATAmount;
                        }
                        else  // For Applicable on MRP
                        {
                            _ItemVATAmount = ((_MRPAmt * _ItemVATPercentage) / 100);  //_MRPAmt - _CDiscountAmount - _SchDiscountAmount
                            return _ItemVATAmount;
                        }

                    }
                    else if (_GRNItemVatType == 1)  // For Inclusive
                    {
                        if (_GRNItemVatApplicationOn == 1) // For Applicable on CP (Rate)
                        {
                            decimal? calculation = _PurchaseRateAmt; //- _CDiscountAmount - _SchDiscountAmount
                            _ItemVATAmount = ((calculation) / (100 + _ItemVATPercentage) * 100);
                            _ItemVATAmount = calculation - _ItemVATAmount;
                            return _ItemVATAmount;
                        }
                        else // For Applicable on MRP
                        {
                            decimal? calculation2 = ((_MRPAmt) / (100 + _ItemVATPercentage) * 100);
                            _ItemVATAmount = (calculation2 * (_ItemVATPercentage / 100));
                            return _ItemVATAmount;
                        }
                    }
                    else
                    {
                        _ItemVATAmount = ((_PurchaseRateAmt * _ItemVATPercentage) / 100);  //_VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                        return _ItemVATAmount;
                    }
                }
                return _ItemVATAmount = 0;
            }
            set
            {
                if (value != _ItemVATAmount)
                {
                    _ItemVATAmount = value;
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("ItemVATPercentage");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal? _ItemTaxPercentage;
        public decimal? ItemTaxPercentage
        {
            get
            {
                return _ItemTaxPercentage;
            }
            set
            {
                if (value != _ItemTaxPercentage)
                {
                    _ItemTaxPercentage = value;
                    OnPropertyChanged("ItemTaxPercentage");
                }
            }
        }

        private int _OtherGRNItemTaxApplicationOn;
        public int OtherGRNItemTaxApplicationOn
        {
            get { return _OtherGRNItemTaxApplicationOn; }
            set
            {
                if (_OtherGRNItemTaxApplicationOn != value)
                {
                    _OtherGRNItemTaxApplicationOn = value;
                }
            }
        }

        private int _OtherGRNItemTaxType;
        public int OtherGRNItemTaxType
        {
            get { return _OtherGRNItemTaxType; }
            set
            {
                if (_OtherGRNItemTaxType != value)
                {
                    _OtherGRNItemTaxType = value;
                }
            }
        }

        private decimal? _ItemTaxAmount;
        public decimal? ItemTaxAmount
        {
            get
            {
                //return _ItemTaxAmount;// return ItemTotalAmount * (ItemVATPercentage / 100);
                if (_ReceivedQty > 0)
                {
                    if (_ItemTaxPercentage > 0)
                    {
                        if (_OtherGRNItemTaxType == 2)  // For Exclusive
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)  // For Applicable on CP (Rate)
                            {
                                _ItemTaxAmount = ((_PurchaseRateAmt * _ItemTaxPercentage) / 100);  //(((_PurchaseRateAmt - _CDiscountAmount - _SchDiscountAmount) * _ItemTaxPercentage) / 100);
                                return _ItemTaxAmount;
                            }
                            else // For Applicable on MRP
                            {
                                _ItemTaxAmount = ((_MRPAmt * _ItemTaxPercentage) / 100); //(((_MRPAmt - _CDiscountAmount - _SchDiscountAmount) * _ItemTaxPercentage) / 100);
                                return _ItemTaxAmount;
                            }
                        }
                        else if (_OtherGRNItemTaxType == 1)  // For Inclusive
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1) // For Applicable on CP (Rate)
                            {
                                decimal? calculation = _PurchaseRateAmt;//- _CDiscountAmount - _SchDiscountAmount;
                                _ItemTaxAmount = ((calculation) / (100 + _ItemTaxPercentage) * 100);
                                _ItemTaxAmount = calculation - _ItemTaxAmount;
                                return _ItemTaxAmount;
                            }
                            else  // For Applicable on MRP
                            {
                                decimal? calculation2 = ((_MRPAmt) / (100 + _ItemTaxPercentage) * 100);
                                _ItemTaxAmount = (calculation2 * (_ItemTaxPercentage / 100));
                                return _ItemTaxAmount;
                            }
                        }
                        else
                        {
                            _ItemTaxAmount = ((_PurchaseRateAmt * _ItemTaxPercentage) / 100);  //(((_PurchaseRateAmt - _CDiscountAmount - _SchDiscountAmount) * _ItemTaxPercentage) / 100);
                            return _ItemTaxAmount;
                        }
                    }
                    return _ItemTaxAmount = 0;
                }
                return _ItemTaxAmount;
            }
            set
            {
                if (value != _ItemTaxAmount)
                {
                    _ItemTaxAmount = value;
                    OnPropertyChanged("ItemTaxAmount");
                }
            }
        }

        private decimal? _PurchaseRateAmt;
        public decimal? PurchaseRateAmt
        {
            get
            {
                _PurchaseRateAmt = _PurchaseRate * _ReceivedQty;
                return _PurchaseRateAmt;
            }
            set
            {
                if (value != _PurchaseRateAmt)
                {
                    _PurchaseRateAmt = value;
                    OnPropertyChanged("PurchaseRateAmt");
                }
            }
        }

        private decimal? _MRPAmt;
        public decimal? MRPAmt
        {
            get
            {
                _MRPAmt = _MRP * _ReceivedQty;
                return _MRPAmt;
            }
            set
            {
                if (value != _MRPAmt)
                {
                    _MRPAmt = value;
                    OnPropertyChanged("MRPAmt");
                }
            }
        }

        private double _AbatedMRP;
        public double AbatedMRP
        {
            get
            {
                if (_GRNItemVatType == 2)      // Exclusive 
                {
                    _AbatedMRP = Convert.ToDouble((_MRP) - ((_MRP * _ItemVATPercentage) / 100));
                    return _AbatedMRP;
                }
                else if (_GRNItemVatType == 1)  // Inclusive 
                {
                    _AbatedMRP = Convert.ToDouble(((_MRP) / (_ItemVATPercentage + 100)) * 100);
                    return _AbatedMRP;
                }
                else
                {
                    _AbatedMRP = Convert.ToDouble((_MRP) - ((_MRP * _ItemVATPercentage) / 100));
                    return _AbatedMRP;
                }
            }
            set
            {
                if (_AbatedMRP != value)
                {
                    _AbatedMRP = value;
                    OnPropertyChanged("AbatedMRP");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_ItemVATPercentage != 0)
                {
                    //check type inclusive or exclusive
                    if (_GRNItemVatType == 2)  // For Exclusive
                    {
                        _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);  //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                        return _NetAmount;
                    }
                    else   // For Inclusive
                    {
                        if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt);   // _PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount;
                            return _NetAmount;
                        }
                        else  //for Applicable on MRP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);  //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                            return _NetAmount;
                        }

                    }
                }
                else
                {
                    if (_OtherGRNItemTaxType == 2) // For Exclusive
                    {
                        _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);   //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                        return _NetAmount;
                    }
                    else  // For Inclusive
                    {
                        if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt);   //_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount;
                            return _NetAmount;
                        }
                        else  //for Applicable on MRP
                        {
                            _NetAmount = Convert.ToDouble(_PurchaseRateAmt + _ItemVATAmount + _ItemTaxAmount);  //(_PurchaseRateAmt - _SchDiscountAmount - _CDiscountAmount) + _ItemVATAmount + _ItemTaxAmount;
                            return _NetAmount;
                        }
                    }
                }
            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        #endregion

        private String _ReasonForIssueName;  //use to show whether its a issue against Damaged,Obsolete,Scrap,Expired (DOSE)
        public String ReasonForIssueName
        {
            get
            {
                return _ReasonForIssueName;
            }
            set
            {
                if (value != _ReasonForIssueName)
                {
                    _ReasonForIssueName = value;
                    OnPropertyChanged("ReasonForIssueName");
                }
            }
        }

    }
}
