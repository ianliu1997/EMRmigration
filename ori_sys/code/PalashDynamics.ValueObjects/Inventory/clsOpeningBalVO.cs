using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace PalashDynamics.ValueObjects.Inventory
{

    public class clsOpeningBalVO : IValueObject, INotifyPropertyChanged
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        public long UserID { get; set; }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

        private DateTime _Date = DateTime.Now;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }
        private long _ItemExpiredInDays;
        public long ItemExpiredInDays
        {
            get
            {
                return _ItemExpiredInDays;
            }
            set
            {
                if (value != _ItemExpiredInDays)
                {
                    _ItemExpiredInDays = value;
                    OnPropertyChanged("ItemExpiredInDays");
                }
            }
        }

        private DateTime _Time = DateTime.Now;
        public DateTime Time
        {
            get { return _Time; }
            set
            {
                if (_Time != value)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private long _StoreID;
        public long StoreID
        {
            get { return _StoreID; }
            set
            {
                if (_StoreID != value)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");
                }
            }
        }



        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private long _BatchID;
        public long BatchID
        {
            get { return _BatchID; }
            set
            {
                if (_BatchID != value)
                {
                    _BatchID = value;
                    OnPropertyChanged("BatchID");
                }
            }
        }

        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
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
                if (_ExpiryDate.HasValue)
                    return _ExpiryDate.Value.Date;
                else
                    return _ExpiryDate;
            }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }


        [Required]
        [RegularExpression(@"^([0-9]{0,5})$")]
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
                    //if (value.ToString().Length > 5)
                    //{
                    //  //  _SingleQuantity = 0;
                    //    throw new ValidationException("Quantity Length Should Not Be Greater Than 5 Digits.");
                    //}
                    //else
                    //{
                    _SingleQuantity = value;
                    OnPropertyChanged("SingleQuantity");
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    // }
                }
            }
        }

        private float _Quantity;
        public float Quantity
        {
            get
            {

                return _Quantity;
            }
            set
            {
                if (_Quantity != value)
                {
                    //if (_Quantity == 0)
                    //    throw new ValidationException("Quantity should not be zero");

                    //else
                    //{
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    //}


                }
            }
        }

        private float _Rate;
        public float Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
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

        private float _Amount;
        public float Amount
        {
            get
            {
                _MainRate = (_Rate / _BaseConversionFactor);// *_BaseQuantity;
                _Amount = _MainRate * _BaseQuantity;   //_Amount = _Rate * _Quantity;

                //   _Amount = (_Rate/_BaseConversionFactor) * _BaseQuantity;   // For editable CP & MRP
                _Amount = (float)Math.Round(_Amount, 2);
                return _Amount;
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private float _VATPercent;
        public float VATPercent
        {
            get
            {

                _VATPercent = (float)Math.Round(_VATPercent, 2);
                return _VATPercent;
            }
            set
            {
                if (_VATPercent != value)
                {
                    _VATPercent = value;
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        public float VatAmt { get; set; }
        private float _VATAmount;
        public float VATAmount
        {
            get
            {
                //_VATAmount = ((_Amount * _VATPercent) / 100);
                //_VATAmount = (float)Math.Round(_VATAmount, 2);
                //return _VATAmount;

                if (_VATPercent != 0)
                {
                    if (InclusiveOfTax == 2)
                    {
                        if (ApplicableOn == 1)
                        {
                            // _VATAmount = (((Amount - _DiscountAmount) * _VATPercent) / 100);
                            _VATAmount = (((Amount) * _VATPercent) / 100);
                            return _VATAmount;
                        }
                        else
                        {

                            //  _VATAmount = ((((_MRP / _BaseConversionFactor) - _DiscountAmount) * _VATPercent) / 100);
                            _VATAmount = ((((Amount)) * _VATPercent) / 100);
                            return _VATAmount;
                        }

                    }
                    else if (InclusiveOfTax == 1)
                    {
                        if (ApplicableOn == 1)
                        {
                            // float calculation = (Amount - _DiscountAmount);
                            float calculation = (Amount);
                            _VATAmount = ((calculation) / (100 + _VATPercent) * 100);
                            _VATAmount = calculation - _VATAmount;
                            return _VATAmount;
                        }
                        else
                        {

                            float calculation2 = (Amount / (100 + _VATPercent) * 100);  //(_MRP / _BaseConversionFactor)
                            //  _VATAmount = ((calculation2 - _DiscountAmount) * (_VATPercent / 100));
                            _VATAmount = ((calculation2) * (_VATPercent / 100));

                            return _VATAmount;
                        }
                    }
                    else
                    {
                        //  _VATAmount = (((_Amount - _DiscountAmount) * _VATPercent) / 100);
                        _VATAmount = (((_Amount) * _VATPercent) / 100);
                        return _VATAmount;
                    }
                }
                return _VATAmount = 0;
            }
            set
            {
                if (_VATAmount != value)
                {

                    _VATAmount = value;
                    //_VATPercent = 0;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("TotalVAT");
                }
            }
        }



        private float _NetAmount;
        public float NetAmount
        {
            get
            {

                //_NetAmount = _Amount + _VATAmount;
                //_NetAmount = (float)Math.Round(_NetAmount, 2);
                //return _NetAmount;

                if (_VATPercent != 0)
                {
                    //check type inclusive or exclusive
                    if (InclusiveOfTax == 2)
                    {
                        // return _NetAmount = (Amount - _DiscountAmount) + _VATAmount;  //for exclusive VAT
                        return _NetAmount = (Amount) + _VATAmount;  //for exclusive VAT
                    }
                    else
                    {

                        // return _NetAmount = (Amount - _DiscountAmount);  // for Inclusive VAT
                        return _NetAmount = (Amount);  // for Inclusive VAT
                    }
                }
                else
                {
                    //if (_POItemOtherTaxType == 2)
                    //{

                    //    return _NetAmount = (_CostRate - _DiscountAmount) + _VATAmount + _ItemVATAmount;

                    //}
                    //else
                    //{

                    //    return _NetAmount = (Amount - _DiscountAmount);  // for Inclusive VAT
                    //}

                    _NetAmount = _Amount + _VATAmount;
                    _NetAmount = (float)Math.Round(_NetAmount, 2);
                    return _NetAmount;
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

        private float _DiscountPercent;

        private float _DiscountAmount;
        public float DiscountAmount
        {
            get
            {
                //if (_POItemVatApplicationOn == 1)
                //{
                return _DiscountAmount = (float)((Amount * _DiscountOnSale) / 100);
                //}
                //else
                //{
                //    return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
                //}


                //if (_DiscountPercent != 0)
                //{
                //    return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
                //}
            }
            set
            {
                if (_DiscountAmount != value)
                {
                    _DiscountAmount = value;
                    //_DiscountPercent = 0;
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("DiscountPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        private double _DiscountOnSale;
        public double DiscountOnSale
        {
            get
            {
                _DiscountOnSale = (float)Math.Round(_DiscountOnSale, 2);
                return _DiscountOnSale;
            }
            set
            {
                if (value != _DiscountOnSale)
                {
                    _DiscountOnSale = value;
                    OnPropertyChanged("DiscountOnSale");
                }
            }
        }

        MasterListItem _SelectedPUM = new MasterListItem();
        public MasterListItem SelectedPUM
        {
            get
            {
                return _SelectedPUM;
            }
            set
            {
                if (value != _SelectedPUM)
                {
                    _SelectedPUM = value;
                    OnPropertyChanged("SelectedPUM");
                }
            }


        }

        List<MasterListItem> _PUOMList = new List<MasterListItem>();
        public List<MasterListItem> PUOMList
        {
            get
            {
                return _PUOMList;
            }
            set
            {
                if (value != _PUOMList)
                {
                    _PUOMList = value;

                }
            }

        }


        private float _ConversionFactor = 1;
        public float ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
                }
            }
        }

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }


        private bool _IsBatchRequired;
        public bool IsBatchRequired
        {
            get { return _IsBatchRequired; }
            set
            {
                if (_IsBatchRequired != value)
                {
                    _IsBatchRequired = value;
                    OnPropertyChanged("IsBatchRequired");
                }
            }
        }

        private string _MainPUOM;
        public string MainPUOM
        {
            get
            {
                return _MainPUOM;
            }
            set
            {
                if (value != _MainPUOM)
                {
                    _MainPUOM = value;
                    OnPropertyChanged("MainPUOM");
                }
            }
        }
        private string _PUOM;
        public string PUOM
        {
            get { return _PUOM; }
            set
            {
                if (_PUOM != value)
                {
                    _PUOM = value;
                    OnPropertyChanged("PUOM");
                }
            }
        }


        private string _SUOM;
        public string SUOM
        {
            get { return _SUOM; }
            set
            {
                if (_SUOM != value)
                {
                    _SUOM = value;
                    OnPropertyChanged("SUOM");
                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private string _Barcode;
        public string Barcode
        {
            get { return _Barcode; }
            set
            {
                if (_Barcode != value)
                {
                    _Barcode = value;
                    OnPropertyChanged("Barcode");
                }
            }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private float _MRP;
        public float MRP
        {
            get
            {
                _MRP = (float)Math.Round((decimal)_MRP, 2);
                return _MRP;
            }
            set
            {
                if (value != _MRP)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                }
            }
        }

        private float _MainMRP;
        public float MainMRP
        {
            get
            {
                _MainMRP = (float)Math.Round(_MainMRP, 2);
                return _MainMRP;
            }
            set
            {
                if (value != _MainMRP)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
                }
            }
        }

        private int _InclusiveOfTax;
        public int InclusiveOfTax
        {
            get { return _InclusiveOfTax; }
            set
            {
                if (_InclusiveOfTax != value)
                {
                    _InclusiveOfTax = value;
                    OnPropertyChanged("InclusiveOfTax");
                }
            }
        }

        public int ApplicableOn { get; set; }

        private bool _EnableInclusiveOfTax;
        public bool EnableInclusiveOfTax
        {
            get
            {

                return _EnableInclusiveOfTax;
            }
            set
            {
                if (_EnableInclusiveOfTax != value)
                {

                    _EnableInclusiveOfTax = value;
                    OnPropertyChanged("EnableInclusiveOfTax");


                }
            }
        }


        public long DepartmentID { get; set; }

        public int TransactionTypeID { get; set; }

        public int OperationType { get; set; }

        public long ID { get; set; }


        private clsItemStockVO _StockDetails = new clsItemStockVO();
        public clsItemStockVO StockDetails
        {
            get { return _StockDetails; }
            set
            {
                if (_StockDetails != value)
                {
                    _StockDetails = value;
                    OnPropertyChanged("StockDetails");
                }
            }
        }



        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate ");
                }
            }
        }


        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate ");
                }
            }
        }


        private double _TotalVAT;
        public double TotalVAT
        {
            get { return _TotalVAT; }
            set
            {
                if (_TotalVAT != value)
                {
                    _TotalVAT = value;
                    OnPropertyChanged("TotalVAT");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        //    public double TotalNetCP { get; set; }

        private double _TotalNet;
        public double TotalNet
        {
            get { return _TotalNet; }
            set
            {
                if (_TotalNet != value)
                {
                    _TotalNet = value;
                    OnPropertyChanged("TotalNet");
                }
            }
        }

        private double _TotalMRP;
        public double TotalMRP
        {
            get { return _TotalMRP; }
            set
            {
                if (_TotalMRP != value)
                {
                    _TotalMRP = value;
                    OnPropertyChanged("TotalMRP");
                }
            }
        }


        public Boolean IsPagingEnabled { get; set; }

        public int StartIndex { get; set; }

        public int MaxRows { get; set; }

        public int TotalRows { get; set; }

        public float InputTransactionQuantity { get; set; }  //By Umesh
        public string TransactionUOM { get; set; }

        # region For Conversion Factor

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


        public float Re_Order { get; set; }
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
                    OnPropertyChanged("DiscountAmount");
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

        public float StockCF { get; set; }

        # endregion

        private long _ItemGroupID;
        public long ItemGroupID
        {
            get { return _ItemGroupID; }
            set
            {
                if (_ItemGroupID != value)
                {
                    _ItemGroupID = value;
                    OnPropertyChanged("ItemGroupID");
                }
            }
        }

        private string _ItemGroup;
        public string ItemGroup
        {
            get { return _ItemGroup; }
            set
            {
                if (_ItemGroup != value)
                {
                    _ItemGroup = value;
                    OnPropertyChanged("ItemGroup");
                }
            }
        }

    }


    public class clsRateContractVO : IValueObject, INotifyPropertyChanged
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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged("FromDate");
                }
            }
        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged("ToDate");
                }
            }
        }

        private DateTime? _ContractDate;
        public DateTime? ContractDate
        {
            get { return _ContractDate; }
            set
            {
                if (_ContractDate != value)
                {
                    _ContractDate = value;
                    OnPropertyChanged("ContractDate");
                }
            }
        }


        public long SupplierID { get; set; }
        public string Supplier { get; set; }

        public long ID { get; set; }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private long _ClinicRepresentativeID;
        public long ClinicRepresentativeID
        {
            get { return _ClinicRepresentativeID; }
            set
            {
                if (_ClinicRepresentativeID != value)
                {
                    _ClinicRepresentativeID = value;
                    OnPropertyChanged("ClinicRepresentativeID");
                }
            }
        }

        private string _SupplierRepresentative;
        public string SupplierRepresentative
        {
            get { return _SupplierRepresentative; }
            set
            {
                if (_SupplierRepresentative != value)
                {
                    _SupplierRepresentative = value;
                    OnPropertyChanged("SupplierRepresentative");
                }
            }
        }


        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }



        private List<clsRateContractDetailsVO> _StockDetails = new List<clsRateContractDetailsVO>();
        public List<clsRateContractDetailsVO> ContractDetails
        {
            get { return _StockDetails; }
            set
            {
                if (_StockDetails != value)
                {
                    _StockDetails = value;
                    OnPropertyChanged("StockDetails");
                }
            }
        }




        private bool _IsFreeze = true;
        public bool IsFreeze
        {
            get { return _IsFreeze; }
            set
            {
                if (_IsFreeze != value)
                {
                    _IsFreeze = value;
                    OnPropertyChanged("IsFreeze");
                }
            }
        }


        private decimal _ContractValue;
        public decimal ContractValue
        {
            get { return _ContractValue; }
            set
            {
                if (_ContractValue != value)
                {
                    _ContractValue = value;
                    OnPropertyChanged("ContractValue");
                }
            }
        }



        //Added by Prashant Channe 17/10/2018
        private string _ReasonForEdit;

        public string ReasonForEdit
        {
            get
            {
                return _ReasonForEdit;
            }
            set
            {
                _ReasonForEdit = value;
                OnPropertyChanged("ReasonForEdit");
            }

        }

        //Added by Prashant Channe 19/10/2018
        private Boolean _IsEditAfterFreeze = false;
        public Boolean IsEditAfterFreeze
        {
            get { return _IsEditAfterFreeze; }
            set
            {
                if (_IsEditAfterFreeze != value)
                {
                    _IsEditAfterFreeze = value;
                    OnPropertyChanged("IsEditAfterFreeze");
                }
            }
        }

    }

    public class clsRateContractDetailsVO : IValueObject, INotifyPropertyChanged
    {

        private List<MasterListItem> _ConditionList = new List<MasterListItem>();
        public List<MasterListItem> ConditionList
        {
            get { return _ConditionList; }
            set
            {
                if (_ConditionList != value)
                {
                    _ConditionList = value;
                    OnPropertyChanged("ConditionList");
                }
            }
        }


        MasterListItem _SelectedCondition = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedCondition
        {
            get
            {
                return _SelectedCondition;
            }
            set
            {
                if (value != _SelectedCondition)
                {
                    _SelectedCondition = value;
                    OnPropertyChanged("SelectedCondition");
                }
            }


        }

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region Common Properties

        private string _LinkServer;
        public string LinkServer
        {
            get { return _LinkServer; }
            set
            {
                if (_LinkServer != value)
                {
                    _LinkServer = value;
                    OnPropertyChanged("LinkServer");
                }
            }
        }

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
                }
            }
        }

        private bool _Status = true;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        private long? _AddedBy;
        public long? AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (_AddedBy != value)
                {
                    _AddedBy = value;
                    OnPropertyChanged("AddedBy");
                }
            }
        }

        private string _AddedOn = "";
        public string AddedOn
        {
            get { return _AddedOn; }
            set
            {
                if (_AddedOn != value)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (_AddedDateTime != value)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _AddedWindowsLoginName = "";
        public string AddedWindowsLoginName
        {
            get { return _AddedWindowsLoginName; }
            set
            {
                if (_AddedWindowsLoginName != value)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private long? _UpdatedBy;
        public long? UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (_UpdatedBy != value)
                {
                    _UpdatedBy = value;
                    OnPropertyChanged("UpdatedBy");
                }
            }
        }

        private string _UpdatedOn = "";
        public string UpdatedOn
        {
            get { return _UpdatedOn; }
            set
            {
                if (_UpdatedOn != value)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (_UpdatedDateTime != value)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

        private string _UpdatedWindowsLoginName = "";
        public string UpdatedWindowsLoginName
        {
            get { return _UpdatedWindowsLoginName; }
            set
            {
                if (_UpdatedWindowsLoginName != value)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }



        #endregion

        private string _ContractCode;
        public string ContractCode
        {
            get { return _ContractCode; }
            set
            {
                if (_ContractCode != value)
                {
                    _ContractCode = value;
                    OnPropertyChanged("ContractCode");
                }
            }
        }

        private string _ContractDescription;
        public string ContractDescription
        {
            get { return _ContractDescription; }
            set
            {
                if (_ContractDescription != value)
                {
                    _ContractDescription = value;
                    OnPropertyChanged("ContractDescription");
                }
            }
        }

        private long _ContractID;
        public long ContractID
        {
            get { return _ContractID; }
            set
            {
                if (_ContractID != value)
                {
                    _ContractID = value;
                    OnPropertyChanged("ContractID");
                }
            }
        }

        private long _ContractUnitId;
        public long ContractUnitId
        {
            get { return _ContractUnitId; }
            set
            {
                if (_ContractUnitId != value)
                {
                    _ContractUnitId = value;
                    OnPropertyChanged("ContractUnitId");
                }
            }
        }

        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (_ItemID != value)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (_ItemCode != value)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private string _ManufactureCompany;
        public string ManufactureCompany
        {
            get { return _ManufactureCompany; }
            set
            {
                if (_ManufactureCompany != value)
                {
                    _ManufactureCompany = value;
                    OnPropertyChanged("ManufactureCompany");
                }
            }
        }

        private string _HSNCode;
        public string HSNCode
        {
            get { return _HSNCode; }
            set
            {
                if (_HSNCode != value)
                {
                    _HSNCode = value;
                    OnPropertyChanged("HSNCode");
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

        private string _TransUOM;
        public string TransUOM
        {
            get { return _TransUOM; }
            set
            {
                if (_TransUOM != value)
                {
                    _TransUOM = value;
                    OnPropertyChanged("TransUOM");
                }
            }
        }

        private decimal _Rate;
        public decimal Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _CostRate;
        public decimal CostRate
        {
            get
            {
                _CostRate = _Rate * _Quantity;
                return _CostRate;
            }
            set
            {
                if (_CostRate != value)
                {
                    _CostRate = value;
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("DiscountPercent");
                    OnPropertyChanged("NetAmount");

                }
            }

        }

        private decimal _MRP;
        public decimal MRP
        {
            get { return _MRP; }
            set
            {
                if (_MRP != value)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get
            {
                return _Amount = _MRP * _Quantity;
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;

                }
            }
        }

        private float _MainMRP;
        public float MainMRP
        {
            get
            {
                _MainMRP = (float)Math.Round(_MainMRP, 2);
                return _MainMRP;
            }
            set
            {
                if (value != _MainMRP)
                {
                    _MainMRP = value;
                    OnPropertyChanged("MainMRP");
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

        private decimal _DiscountPercent;
        public decimal DiscountPercent
        {
            get { return _DiscountPercent; }
            set
            {
                if (_DiscountPercent != value)
                {
                    _DiscountPercent = value;
                    OnPropertyChanged("DiscountPercent");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _DiscountAmount;
        public decimal DiscountAmount
        {
            get
            {
                return _DiscountAmount = ((_CostRate * _DiscountPercent) / 100);
            }
            set
            {
                if (_DiscountAmount != value)
                {
                    _DiscountAmount = value;
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("DiscountPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private float _ConversionFactor;
        public float ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }

        private decimal _NetAmount;
        public decimal NetAmount
        {
            get
            {
                return _NetAmount = (_CostRate - _DiscountAmount);
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


        #region only for Best PO Price
        private float _BestBaseRate;
        public float BestBaseRate
        {
            get
            {
                _BestBaseRate = (float)Math.Round(_BestBaseRate, 2);
                return _BestBaseRate;
            }
            set
            {
                if (value != _BestBaseRate)
                {
                    _BestBaseRate = value;
                    OnPropertyChanged("BestBaseRate");
                }
            }
        }


        private float _BestBaseMRP;
        public float BestBaseMRP
        {
            get
            {
                _BestBaseMRP = (float)Math.Round(_BestBaseMRP, 2);
                return _BestBaseMRP;
            }
            set
            {
                if (value != _BestBaseMRP)
                {
                    _BestBaseMRP = value;
                    OnPropertyChanged("BestBaseMRP");
                }
            }
        }

        public bool IsItemInRateContract { get; set; }
        #endregion

        

        private string _Condition;
        public string Condition
        {
            get { return _Condition; }
            set
            {
                if (_Condition != value)
                {
                    _Condition = value;
                    OnPropertyChanged("Condition");
                }
            }
        }

        
        private decimal _MinQuantity;
        public decimal MinQuantity
        {
            get { return _MinQuantity; }
            set
            {
                if (_MinQuantity != value)
                {

                    _MinQuantity = value;
                    OnPropertyChanged("MinQuantity");

                }
            }
        }

        private decimal _MaxQuantity;
        public decimal MaxQuantity
        {
            get { return _MaxQuantity; }
            set
            {
                if (_MaxQuantity != value)
                {

                    _MaxQuantity = value;
                    OnPropertyChanged("MaxQuantity");

                }
            }
        }

        private bool _UnlimitedQuantity;
        public bool UnlimitedQuantity
        {
            get { return _UnlimitedQuantity; }
            set
            {
                if (_UnlimitedQuantity != value)
                {
                    _UnlimitedQuantity = value;
                    OnPropertyChanged("UnlimitedQuantity");

                }
            }
        }


        private decimal _PendingQuantity;
        public decimal PendingQuantity
        {
            get { return _PendingQuantity; }
            set
            {
                if (_PendingQuantity != value)
                {
                    _PendingQuantity = value;
                    OnPropertyChanged("PendingQuantity");

                }
            }
        }




        private decimal _VATPercent;
        public decimal VATPercent
        {
            get { return _VATPercent; }
            set
            {
                if (_VATPercent != value)
                {
                    _VATPercent = value;
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }



        
        private string _PUOM;
        public string PUOM
        {
            get { return _PUOM; }
            set
            {
                if (_PUOM != value)
                {
                    _PUOM = value;
                    OnPropertyChanged("PUOM");
                }
            }
        }



        
        private bool _InclusiveOfTax;
        public bool InclusiveOfTax
        {
            get { return _InclusiveOfTax; }
            set
            {
                if (_InclusiveOfTax != value)
                {
                    _InclusiveOfTax = value;
                    OnPropertyChanged("InclusiveOfTax");
                }
            }
        }



        private bool _EnableInclusiveOfTax;
        public bool EnableInclusiveOfTax
        {
            get
            {

                return _EnableInclusiveOfTax;
            }
            set
            {
                if (_EnableInclusiveOfTax != value)
                {

                    _EnableInclusiveOfTax = value;
                    OnPropertyChanged("EnableInclusiveOfTax");


                }
            }
        }


        public long ID { get; set; }

        //Added by Prashant Channe 17/10/2018
        private string _ReasonForEdit;

        public string ReasonForEdit
        {
            get
            {
                return _ReasonForEdit;
            }
            set
            {
                _ReasonForEdit = value;
                OnPropertyChanged("ReasonForEdit");
            }

        }

        //Added by Prashant Channe 19/10/2018
        private Boolean _IsEditAfterFreeze = false;
        public Boolean IsEditAfterFreeze
        {
            get { return _IsEditAfterFreeze; }
            set
            {
                if (_IsEditAfterFreeze != value)
                {
                    _IsEditAfterFreeze = value;
                    OnPropertyChanged("IsEditAfterFreeze");
                }
            }
        }

    }
}
