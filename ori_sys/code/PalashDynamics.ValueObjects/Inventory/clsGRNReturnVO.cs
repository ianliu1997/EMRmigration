using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class    clsGRNReturnVO : IValueObject, INotifyPropertyChanged
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

        public clsGRNReturnVO()
        {
            _PaymentModeID = MaterPayModeList.Cash;
        }

        private List<clsGRNReturnDetailsVO> _Items = new List<clsGRNReturnDetailsVO>();
        public List<clsGRNReturnDetailsVO> Items
        {
            get { return _Items; }
            set
            {
                if (_Items != value)
                {
                    _Items = value;
                    OnPropertyChanged("Items");
                }
            }
        }

        private long _Id;
        public long ID
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private InventoryGoodReturnType _GoodReturnType;
        public InventoryGoodReturnType GoodReturnType
        {
            get { return _GoodReturnType; }
            set
            {
                if (_GoodReturnType != value)
                {
                    _GoodReturnType = value;
                    OnPropertyChanged("GoodReturnType");
                }
            }
        }

        public string GoodReturnTypeStr
        {
            get
            {
                return _GoodReturnType.ToString();
            }
        }
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

        private string _GRNReturnNO;
        public string GRNReturnNO
        {
            get { return _GRNReturnNO; }
            set
            {
                if (_GRNReturnNO != value)
                {
                    _GRNReturnNO = value;
                    OnPropertyChanged("GRNReturnNO");
                }
            }
        }

        private long _GRNID;
        public long GRNID
        {
            get { return _GRNID; }
            set
            {
                if (_GRNID != value)
                {
                    _GRNID = value;
                    OnPropertyChanged("GRNID");
                }
            }
        }
        private long _GRNUnitID;
        public long GRNUnitID
        {
            get { return _GRNUnitID; }
            set
            {
                if (_GRNUnitID != value)
                {
                    _GRNUnitID = value;
                    OnPropertyChanged("GRNUnitID");
                }
            }
        }



        private string _GRNNO;
        public string GRNNO
        {
            get { return _GRNNO; }
            set
            {
                if (_GRNNO != value)
                {
                    _GRNNO = value;
                    OnPropertyChanged("GRNNO");
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

        private long _SupplierID;
        public long SupplierID
        {
            get { return _SupplierID; }
            set
            {
                if (_SupplierID != value)
                {
                    _SupplierID = value;
                    OnPropertyChanged("SupplierID");
                }
            }
        }

        public string SupplierName { get; set; }
        public string StoreName { get; set; }


        private MaterPayModeList _PaymentModeID;
        public MaterPayModeList PaymentModeID
        {
            get { return _PaymentModeID; }
            set
            {
                if (_PaymentModeID != value)
                {
                    _PaymentModeID = value;
                    OnPropertyChanged("PaymentModeID");
                }
            }
        }


        private double _TotalItemTax;
        public double TotalItemTax
        {
            get { return _TotalItemTax; }
            set
            {
                if (_TotalItemTax != value)
                {
                    _TotalItemTax = value;
                    OnPropertyChanged("TotalItemTax");
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

        private double _TotalSGSTAmount;
        public double TotalSGSTAmount
        {
            get
            {
                return _TotalSGSTAmount;
            }
            set
            {
                if (value != _TotalSGSTAmount)
                {
                    _TotalSGSTAmount = value;
                    OnPropertyChanged("TotalSGSTAmount");
                }
            }
        }

        private double _TotalCGSTAmount;
        public double TotalCGSTAmount
        {
            get
            {
                return _TotalCGSTAmount;
            }
            set
            {
                if (value != _TotalCGSTAmount)
                {
                    _TotalCGSTAmount = value;
                    OnPropertyChanged("TotalCGSTAmount");
                }
            }
        }
        private double _TotalIGSTAmount;
        public double TotalIGSTAmount
        {
            get
            {
                return _TotalIGSTAmount;
            }
            set
            {
                if (value != _TotalIGSTAmount)
                {
                    _TotalIGSTAmount = value;
                    OnPropertyChanged("TotalIGSTAmount");
                }
            }
        }


        private double _NetAmount;
        public double NetAmount
        {
            get { return _NetAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
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

        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsRejected;
        public bool IsRejected
        {
            get { return _IsRejected; }
            set
            {
                if (_IsRejected != value)
                {
                    _IsRejected = value;
                    OnPropertyChanged("IsRejected");
                }
            }
        }

        private string _ApprovedOrRejectedBy;
        public string ApprovedOrRejectedBy
        {
            get { return _ApprovedOrRejectedBy; }
            set
            {
                if (_ApprovedOrRejectedBy != value)
                {
                    _ApprovedOrRejectedBy = value;
                    OnPropertyChanged("ApprovedOrRejectedBy");
                }
            }
        }

        private string _RejectionRemarks;
        public string RejectionRemarks
        {
            get { return _RejectionRemarks; }
            set
            {
                if (_RejectionRemarks != value)
                {
                    _RejectionRemarks = value;
                    OnPropertyChanged("RejectionRemarks");
                }
            }
        }
    }

    public class clsGRNReturnDetailsVO : IValueObject, INotifyPropertyChanged
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

        private string _StoreName;
        public string StoreName
        {
            get { return _StoreName; }
            set
            {
                if (_StoreName != value)
                {
                    _StoreName = value;
                    OnPropertyChanged("StoreName");
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
                    OnPropertyChanged("SupplierID");
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


        #endregion

        private long _Id;
        public long ID
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    OnPropertyChanged("ID");
                }
            }
        }

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

        private double _ConversionFactor;
        public double ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {

                _ConversionFactor = value;
            }



        }

        private double _ItemTax;
        public double ItemTax
        {
            get { return _ItemTax; }
            set
            {
                if (_ItemTax != value)
                {
                    if (value < 0)
                        value = 0;

                    _ItemTax = value;
                    OnPropertyChanged("ItemTax");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
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

        private double _CostRate;
        public double CostRate
        {
            get
            {
                return _CostRate = _Rate * _ReturnedQuantity;
            }
            set
            {
                if (_CostRate != value)
                {
                    _CostRate = value;
                    OnPropertyChanged("CostRate");
                }
            }
        }

        #region Old- TaxAmount
        //private double _TaxAmount;
        //public double TaxAmount
        //{
        //    get
        //    {
        //        //if (_VATPercent != 0)
        //        //{
        //        //    return _VATAmount = ((_Amount * _VATPercent) / 100);
        //        //}
        //        if (_Amount > 0)
        //            _TaxAmount = (((_Amount - TotalDiscAmt) * _ItemTax) / 100);
        //        else
        //            _TaxAmount = 0;
        //        return _TaxAmount;
        //    }


        //    set
        //    {
        //        if (_TaxAmount != value)
        //        {
        //            if (value < 0)
        //                value = 0;

        //            _TaxAmount = value;
        //        }
        //    }
        //}
        #endregion

        private double _TaxAmount;
        public double TaxAmount
        {
            get
            {
                if (_ReturnedQuantity > 0)
                {
                    if (_ItemTax > 0)
                    {
                        if (_OtherGRNItemTaxType == 2)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                _TaxAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
                                return _TaxAmount;
                            }
                            else
                            {
                                //_TaxAmount = (((_Amount) * _ItemTax) / 100);
                                _TaxAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
                                return _TaxAmount;
                            }
                        }
                        else if (_OtherGRNItemTaxType == 1)
                        {
                            if (_OtherGRNItemTaxApplicationOn == 1)
                            {
                                double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                                _TaxAmount = ((calculation) / (100 + _ItemTax) * 100);
                                _TaxAmount = calculation - _TaxAmount;
                                return _TaxAmount;
                            }
                            else
                            {
                                ////_TaxAmount = ((_Amount) / (100 + _ItemTax) * 100);
                                //_TaxAmount = ((_Amount - _CDiscountAmount - _SchDiscountAmount) / (100 + _ItemTax) * 100);
                                //_TaxAmount = _Amount - _TaxAmount;
                                //return _TaxAmount;

                                double calculation2 = ((_Amount) / (100 + _ItemTax) * 100);
                                _TaxAmount = (calculation2 * (_ItemTax / 100)); //_TaxAmount = ((calculation2 - CDiscountAmount - _SchDiscountAmount) * (_ItemTax / 100)); Commented by ashish Z. on dated 140416
                                //_TaxAmount = _Amount - _TaxAmount;
                                return _TaxAmount;
                            }
                        }
                        else
                        {
                            _TaxAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
                            return _TaxAmount;
                        }
                    }
                    return _TaxAmount = 0;
                }
                return _TaxAmount;
            }
            set
            {
                if (_TaxAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _TaxAmount = value;
                }
            }
        }


        //public double _SchDiscountAmount;
        //public double _CDiscountAmount;
        //private double _TotalDiscAmt;
        //public double TotalDiscAmt
        //{
        //    get
        //    {
        //        _SchDiscountAmount = ((_Amount * _SchDiscountPercent) / 100);
        //        _CDiscountAmount = ((_Amount * _CDiscountPercent) / 100);
        //        _TotalDiscAmt = _CDiscountAmount + _SchDiscountAmount;
        //        return _TotalDiscAmt;
        //    }
        //    set
        //    {
        //        _TotalDiscAmt = value;
        //        OnPropertyChanged("TotalDiscAmt");
        //        OnPropertyChanged("VATAmount");
        //        OnPropertyChanged("TaxAmount");
        //        OnPropertyChanged("NetAmount");


        //    }
        //}

        private long _GRNReturnID;
        public long GRNReturnID
        {
            get { return _GRNReturnID; }
            set
            {
                if (_GRNReturnID != value)
                {
                    _GRNReturnID = value;
                    OnPropertyChanged("GRNReturnID");
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
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }

        private double _AvailableQuantity;
        public double AvailableQuantity
        {
            get { return _AvailableQuantity; }
            set
            {
                if (_AvailableQuantity != value)
                {
                    _AvailableQuantity = value;
                    OnPropertyChanged("AvailableQuantity");
                }
            }
        }


        private double _FreeQuantity;
        public double FreeQuantity
        {
            get { return _FreeQuantity; }
            set
            {
                if (_FreeQuantity != value)
                {
                    _FreeQuantity = value;
                    OnPropertyChanged("FreeQuantity");
                }
            }
        }



        private double _ReceivedQuantity;
        public double ReceivedQuantity
        {
            get { return _ReceivedQuantity; }
            set
            {
                if (_ReceivedQuantity != value)
                {
                    _ReceivedQuantity = value;
                    OnPropertyChanged("ReceivedQuantity");
                }
            }
        }

        private string _ReceivedQuantityUOM;
        public string ReceivedQuantityUOM
        {
            get { return _ReceivedQuantityUOM; }
            set
            {
                if (_ReceivedQuantityUOM != value)
                {
                    _ReceivedQuantityUOM = value;
                    OnPropertyChanged("ReceivedQuantityUOM");
                }
            }
        }

        private double _ReturnedQuantity;
        public double ReturnedQuantity
        {
            get { return _ReturnedQuantity; }
            set
            {
                if (_ReturnedQuantity != value)
                {
                    if (value <= 0)
                        _ReturnedQuantity = 0;
                    else
                        _ReturnedQuantity = value;
                    OnPropertyChanged("ReturnedQuantity");
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("PendingQuantity");
                    OnPropertyChanged("TotalQuantity");
                    OnPropertyChanged("Amount");
                    //OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("TotalDiscAmt");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _Rate;
        public double Rate
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
                    OnPropertyChanged("TotalDiscAmt");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        //new
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

        private double _MRP;
        public double MRP
        {
            get { return _MRP; }
            set
            {
                if (_MRP != value)
                {
                    if (value < 0)
                        value = 0;

                    _MRP = value;
                    OnPropertyChanged("MRP");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        //New
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

        private double _UnitRate;
        public double UnitRate
        {
            get { return _UnitRate; }
            set
            {
                if (_UnitRate != value)
                {
                    if (value < 0)
                        value = 0;

                    _UnitRate = value;
                    OnPropertyChanged("UnitRate");
                }
            }
        }
        private double _UnitMRP;
        public double UnitMRP
        {
            get { return _UnitMRP; }
            set
            {
                if (_UnitMRP != value)
                {
                    if (value < 0)
                        value = 0;

                    _UnitMRP = value;
                    OnPropertyChanged("UnitMRP");
                }
            }
        }

        #region Old- Amount
        //private double _Amount;
        //public double Amount
        //{
        //    get
        //    {
        //        if (_ReturnedQuantity != 0 && _ReturnedQuantity >FreeQuantity)
        //            _Amount = _Rate * (_ReturnedQuantity - FreeQuantity);
        //        else
        //            _Amount = 0;
        //        return _Amount;
        //    }
        //    set
        //    {
        //        if (_Amount != value)
        //        {
        //            _Amount = value;
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("TotalDiscAmt");
        //            OnPropertyChanged("TaxAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}
        #endregion

        private double _Amount;
        public double Amount
        {
            get
            {
                return _Amount = _MRP * _ReturnedQuantity;  //_Quantity
            }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("MRP");
                    #region Added by Pallavi
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    #endregion
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _CDiscountPercent;
        public double CDiscountPercent
        {
            get { return _CDiscountPercent; }
            set
            {
                if (_CDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    _CDiscountPercent = value;
                    OnPropertyChanged("CDiscountPercent");
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");

                    OnPropertyChanged("NetAmount");
                }
            }
        }

        //New
        private double _CDiscountAmount;
        public double CDiscountAmount
        {
            get
            {
                return _CDiscountAmount = ((_CostRate * _CDiscountPercent) / 100);
            }
            set
            {
                if (_CDiscountAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _CDiscountAmount = value;
                    //_CDiscountPercent = 0;
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("CDiscountPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }


        private double _SchDiscountPercent;
        public double SchDiscountPercent
        {
            get { return _SchDiscountPercent; }
            set
            {
                if (_SchDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    _SchDiscountPercent = value;
                    OnPropertyChanged("SchDiscountPercent");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        //New 
        private double _SchDiscountAmount;
        public double SchDiscountAmount
        {
            get
            {
                return _SchDiscountAmount = ((_CostRate * _SchDiscountPercent) / 100);

            }
            set
            {
                if (_SchDiscountAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _SchDiscountAmount = value;
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("SchDiscountPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TotalDiscAmt;
        public double TotalDiscAmt
        {
            get
            {
                return _TotalDiscAmt = _CDiscountAmount + _SchDiscountAmount;
            }

        }

        //New
        private double _AbatedMRP;
        public double AbatedMRP
        {
            get
            {
                if (_GRNItemVatType == 2)      // Exclusive 
                {
                    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
                }
                else if (_GRNItemVatType == 1)  // Inclusive 
                {
                    return _AbatedMRP = ((_MRP) / (_VATPercent + 100)) * 100;
                }
                else
                {
                    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
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

        private double _VATPercent;
        public double VATPercent
        {
            get { return _VATPercent; }
            set
            {
                if (_VATPercent != value)
                {
                    _VATPercent = value;
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        #region Old- VATAmount
        //private double _VATAmount;
        //public double VATAmount
        //{
        //    get
        //    {
        //        if (_VATPercent != 0)
        //        {
        //            if (_Amount > 0)
        //                return _VATAmount = (((_Amount - _TotalDiscAmt) * _VATPercent) / 100);
        //            else
        //                return 0;
        //        }

        //        else
        //            return _VATAmount;
        //    }
        //    set
        //    {
        //        if (_VATAmount != value)
        //        {

        //            _VATAmount = value;
        //            //_VATPercent = 0;
        //            OnPropertyChanged("VATAmount");
        //            //OnPropertyChanged("VATPercent");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}
        #endregion

        private double _VATAmount;
        public double VATAmount
        {
            get
            {
                //if (_VATPercent != 0)
                //{
                //    return _VATAmount = ((_Amount * _VATPercent) / 100);
                //}
                if (_VATPercent != 0)
                {
                    if (_GRNItemVatType == 2)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            _VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                            return _VATAmount;
                        }
                        else
                        {
                            //_VATAmount = (((_Amount) * _VATPercent) / 100);
                            _VATAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                            return _VATAmount;
                        }

                    }
                    else if (_GRNItemVatType == 1)
                    {
                        if (_GRNItemVatApplicationOn == 1)
                        {
                            double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                            _VATAmount = ((calculation) / (100 + _VATPercent) * 100);
                            _VATAmount = calculation - _VATAmount;
                            return _VATAmount;
                        }
                        else
                        {
                            ////_VATAmount = ((_Amount) / (100 + _VATPercent) * 100);
                            //_VATAmount = ((_Amount - _CDiscountAmount - _SchDiscountAmount) / (100 + _VATPercent) * 100);
                            //_VATAmount = _Amount - _VATAmount;
                            //return _VATAmount;

                            double calculation2 = ((_Amount) / (100 + _VATPercent) * 100);
                            _VATAmount = (calculation2 * (_VATPercent / 100));  //_VATAmount = ((calculation2 - CDiscountAmount - _SchDiscountAmount) * (_VATPercent / 100));
                            //_VATAmount = _Amount - _VATAmount;
                            return _VATAmount;
                        }
                    }
                    else
                    {
                        _VATAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                        return _VATAmount;
                    }
                }
                return _VATAmount = 0;
            }
            set
            {
                if (_VATAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _VATAmount = value;
                    _VATPercent = 0;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("NetAmount");
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

        #region Old- NetAmount
        //private double _NetAmount;
        //public double NetAmount
        //{
        //    get
        //    {
        //        if (_Amount > 0)
        //            return _NetAmount = _Amount + -_TotalDiscAmt + _VATAmount + _TaxAmount;
        //        else
        //            return 0;
        //    }
        //    set
        //    {
        //        if (_NetAmount != value)
        //        {
        //            _NetAmount = value;
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}
        #endregion

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                if (_IsFreeItem) // For Free Items
                {
                    if (_VATPercent != 0)
                    {
                        //check type inclusive or exclusive
                        if (_GRNItemVatType == 2)
                        {
                            _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            return _NetAmount;
                        }
                        else
                        {
                            if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                            {
                                return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
                            else  //for Applicable on MRP
                            {
                                return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
                        }
                    }
                    else if ((_SGSTPercent != 0 && _CGSTPercent != 0) || _IGSTPercent != 0)
                    {
                        //check type inclusive or exclusive
                        if (_GRNSGSTVatType == 2 && _GRNCGSTVatType == 2 || _GRNIGSTVatType == 2)
                        {
                            _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        }
                        else
                        {
                            if ((_GRNSGSTVatApplicationOn == 1 && _GRNCGSTVatApplicationOn == 1) && _GRNIGSTVatApplicationOn == 1) //for Applicable on CP
                            {
                                _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
                            else  //for Applicable on MRP
                            {
                                _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }

                        }
                        return _NetAmount;
                    }
                    else
                    {
                        if (_OtherGRNItemTaxType == 2)
                        {
                            _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            return _NetAmount;
                        }
                        else
                        {
                            if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                            {
                                return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
                            else  //for Applicable on MRP
                            {
                                return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
                        }
                    }
                }
                else  // For Non Free Items
                {
                    if (_VATPercent != 0)
                    {
                        //check type inclusive or exclusive
                        if (_GRNItemVatType == 2)
                        {
                            _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            return _NetAmount;
                        }
                        else
                        {
                            if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                            {
                                return _NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount;
                            }
                            else  //for Applicable on MRP
                            {
                                return _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }

                        }
                    }
                    else if ((_SGSTPercent != 0 && _CGSTPercent != 0) || _IGSTPercent != 0)
                    {
                        //check type inclusive or exclusive
                        if (_GRNSGSTVatType == 2 && _GRNCGSTVatType == 2 || _GRNIGSTVatType == 2)
                        {
                            return _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount; ;  //for exclusive VAT
                        }
                        else
                        {
                            if ((_GRNSGSTVatApplicationOn == 1 && _GRNCGSTVatApplicationOn == 1) && _GRNIGSTVatApplicationOn == 1) //for Applicable on CP
                            {
                                return _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount);  // for Inclusive VAT
                            }
                            else  //for Applicable on MRP
                            {
                                return _NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
                        }
                    }
                    else
                    {
                        if (_OtherGRNItemTaxType == 2)
                        {
                            _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            return _NetAmount;
                        }
                        else
                        {
                            if (_GRNItemVatApplicationOn == 1) //for Applicable on CP
                            {
                                return _NetAmount = _CostRate - _SchDiscountAmount - _CDiscountAmount;
                            }
                            else  //for Applicable on MRP
                            {
                                return _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                            }
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

        private double _NetAmount1;
        public double NetAmount1
        {
            get
            {
                //if (_Amount > 0)
                    return _NetAmount1;
                //else
                   // return 0;
            }
            set
            {
                if (_NetAmount1 != value)
                {
                    _NetAmount1 = value;
                    OnPropertyChanged("NetAmount1");
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

        private string _ReturnReason;
        public string ReturnReason
        {
            get { return _ReturnReason; }
            set
            {
                if (_ReturnReason != value)
                {
                    _ReturnReason = value;
                    OnPropertyChanged("ReturnReason");
                }
            }
        }

        private long _GRNReturnUnitID;
        public long GRNReturnUnitID
        {
            get { return _GRNReturnUnitID; }
            set
            {
                if (_GRNReturnUnitID != value)
                {
                    _GRNReturnUnitID = value;
                    OnPropertyChanged("GRNReturnUnitID");
                }
            }
        }
        public double TotalQuantity
        {
            get { return (_ReturnedQuantity) * _ConversionFactor; }
        }

        #region PendingQuantity
        //private double _PendingQuantity;
        //public double PendingQuantity
        //{
        //    get { return (ReturnPendingQty - _ReturnedQuantity); }
        //    set
        //    {
        //        if (_PendingQuantity != value)
        //        {
        //            _PendingQuantity = value;
        //            OnPropertyChanged("PendingQuantity");
        //        }
        //    }
        //}
        #endregion

        private double _PendingQuantity;
        public double PendingQuantity
        {
            get { return (_PendingQuantity); }   //ReturnPendingQty - _ReturnedQuantity
            set
            {
                if (_PendingQuantity != value)
                {
                    _PendingQuantity = value;
                    OnPropertyChanged("PendingQuantity");
                }
            }
        }

        public double ReturnPendingQty { get; set; }

        #region Conversion Factor
        private string _PurchaseUOM;
        public string PurchaseUOM
        {
            get { return _PurchaseUOM; }
            set
            {
                if (_PurchaseUOM != value)
                {
                    _PurchaseUOM = value;
                    OnPropertyChanged("PurchaseUOM");
                }
            }
        }

        private String _StockUOM;
        public string StockUOM
        {
            get { return _StockUOM; }
            set
            {
                if (_StockUOM != value)
                {
                    _StockUOM = value;
                    OnPropertyChanged("StockUOM");
                }
            }
        }

        private long _TransactionUOMID;
        public long TransactionUOMID
        {
            get { return _TransactionUOMID; }
            set
            {
                if (_TransactionUOMID != value)
                {
                    _TransactionUOMID = value;
                    OnPropertyChanged("TransactionUOMID");
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
        #endregion

        private string _GRNNo;
        public string GRNNo
        {
            get { return _GRNNo; }
            set
            {
                if (_GRNNo != value)
                {
                    _GRNNo = value;
                    OnPropertyChanged("GRNNo");
                }
            }
        }

        private string _ReceivedNo;
        public string ReceivedNo
        {
            get { return _ReceivedNo; }
            set
            {
                if (_ReceivedNo != value)
                {
                    _ReceivedNo = value;
                    OnPropertyChanged("ReceivedNo");
                }
            }
        }

        private bool _IsFreeItem;
        public bool IsFreeItem
        {
            get { return _IsFreeItem; }
            set
            {
                if (_IsFreeItem != value)
                {
                    _IsFreeItem = value;
                    OnPropertyChanged("IsFreeItem");
                }
            }
        }

       

        public long GRNID { get; set; }
        public long GRNUnitID { get; set; }
        public long GRNDetailID { get; set; }
        public long GRNDetailUnitID { get; set; }
        public long ReceivedID { get; set; }
        public long ReceivedUnitID { get; set; }
        public long ReceivedDetailID { get; set; }

        //***//
        private double _SGSTPercent;
        public double SGSTPercent
        {
            get { return _SGSTPercent; }
            set
            {
                if (_SGSTPercent != value)
                {
                    _SGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _CGSTPercent;
        public double CGSTPercent
        {
            get { return _CGSTPercent; }
            set
            {
                if (_CGSTPercent != value)
                {
                    _CGSTPercent = value;
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _IGSTPercent;
        public double IGSTPercent
        {
            get { return _IGSTPercent; }
            set
            {
                if (_IGSTPercent != value)
                {
                    _IGSTPercent = value;
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _SGSTAmount;
        public double SGSTAmount
        {
            get
            {
                if (_SGSTPercent != 0)
                {
                    if (_GRNSGSTVatType == 2)
                    {
                        if (_GRNSGSTVatApplicationOn == 1)
                        {
                            _SGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _SGSTPercent) / 100);
                            //_SGSTAmount = (((_CostRate) * _SGSTPercent) / 100);
                            return _SGSTAmount;
                        }
                        else
                        {
                            _SGSTAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _SGSTPercent) / 100);
                            //_SGSTAmount = (((_Amount) * _SGSTPercent) / 100);
                            return _SGSTAmount;
                        }
                    }
                    else if (_GRNSGSTVatType == 1)
                    {
                        if (_GRNSGSTVatApplicationOn == 1)
                        {
                            //double calculation = _CostRate;
                            double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                            _SGSTAmount = ((calculation) / (100 + _SGSTPercent) * 100);
                            _SGSTAmount = calculation - _SGSTAmount;
                            return _SGSTAmount;
                        }
                        else
                        {
                            double calculation2 = ((_Amount) / (100 + _SGSTPercent) * 100);
                            _SGSTAmount = (calculation2 * (_SGSTPercent / 100));
                            return _SGSTAmount;
                        }
                    }
                    else
                    {
                        //_SGSTAmount = (((_CostRate) * _SGSTPercent) / 100);
                        _SGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _SGSTPercent) / 100);
                        return _SGSTAmount;
                    }
                }
                return _SGSTAmount = 0;
            }
            set
            {
                if (_SGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _SGSTAmount = value;
                    _SGSTPercent = 0;
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _CGSTAmount;
        public double CGSTAmount
        {
            get
            {
                if (_CGSTPercent != 0)
                {
                    if (_GRNCGSTVatType == 2)
                    {
                        if (_GRNCGSTVatApplicationOn == 1)
                        {
                            _CGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _CGSTPercent) / 100);
                            //_CGSTAmount = (((_CostRate) * _CGSTPercent) / 100);
                            return _CGSTAmount;
                        }
                        else
                        {
                            _CGSTAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _CGSTPercent) / 100);
                            //_CGSTAmount = (((_Amount) * _CGSTPercent) / 100);
                            return _CGSTAmount;
                        }
                    }
                    else if (_GRNCGSTVatType == 1)
                    {
                        if (_GRNCGSTVatApplicationOn == 1)
                        {
                            //double calculation = _CostRate;
                            double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                            _CGSTAmount = ((calculation) / (100 + _CGSTPercent) * 100);
                            _CGSTAmount = calculation - _CGSTAmount;
                            return _CGSTAmount;
                        }
                        else
                        {
                            double calculation2 = ((_Amount) / (100 + _CGSTPercent) * 100);
                            _CGSTAmount = (calculation2 * (_CGSTPercent / 100));
                            return _CGSTAmount;
                        }
                    }
                    else
                    {
                        _CGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _CGSTPercent) / 100);
                        //_CGSTAmount = (((_CostRate) * _CGSTPercent) / 100);
                        return _CGSTAmount;
                    }
                }
                return _CGSTAmount = 0;
            }
            set
            {
                if (_CGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _CGSTAmount = value;
                    _CGSTPercent = 0;
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _IGSTAmount;
        public double IGSTAmount
        {
            get
            {
                if (_IGSTPercent != 0)
                {
                    if (_GRNIGSTVatType == 2)
                    {
                        if (_GRNIGSTVatApplicationOn == 1)
                        {
                            _IGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _IGSTPercent) / 100);
                            //_IGSTAmount = (((_CostRate) * _IGSTPercent) / 100);
                            return _IGSTAmount;
                        }
                        else
                        {
                            _IGSTAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _IGSTPercent) / 100);
                            //_IGSTAmount = (((_Amount) * _IGSTPercent) / 100);
                            return _IGSTAmount;
                        }
                    }
                    else if (_GRNIGSTVatType == 1)
                    {
                        if (_GRNIGSTVatApplicationOn == 1)
                        {
                            //double calculation = _CostRate;
                            double calculation = _CostRate - _CDiscountAmount - _SchDiscountAmount;
                            _IGSTAmount = ((calculation) / (100 + _IGSTPercent) * 100);
                            _IGSTAmount = calculation - _IGSTAmount;
                            return _IGSTAmount;
                        }
                        else
                        {
                            double calculation2 = ((_Amount) / (100 + _IGSTPercent) * 100);
                            _IGSTAmount = (calculation2 * (_IGSTPercent / 100));
                            return _IGSTAmount;
                        }
                    }
                    else
                    {
                        _IGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _IGSTPercent) / 100);
                        //_IGSTAmount = (((_CostRate) * _IGSTPercent) / 100);
                        return _IGSTAmount;
                    }
                }
                return _IGSTAmount = 0;
            }
            set
            {
                if (_IGSTAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _IGSTAmount = value;
                    _IGSTPercent = 0;
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private int _GRNSGSTVatType;
        public int GRNSGSTVatType
        {
            get { return _GRNSGSTVatType; }
            set
            {
                if (_GRNSGSTVatType != value)
                {
                    _GRNSGSTVatType = value;
                }
            }
        }
        private int _GRNSGSTVatApplicationOn;
        public int GRNSGSTVatApplicationOn
        {
            get { return _GRNSGSTVatApplicationOn; }
            set
            {
                if (_GRNSGSTVatApplicationOn != value)
                {
                    _GRNSGSTVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


                }
            }
        }

        private int _GRNCGSTVatType;
        public int GRNCGSTVatType
        {
            get { return _GRNCGSTVatType; }
            set
            {
                if (_GRNCGSTVatType != value)
                {
                    _GRNCGSTVatType = value;
                }
            }
        }
        private int _GRNCGSTVatApplicationOn;
        public int GRNCGSTVatApplicationOn
        {
            get { return _GRNCGSTVatApplicationOn; }
            set
            {
                if (_GRNCGSTVatApplicationOn != value)
                {
                    _GRNCGSTVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


                }
            }
        }

        private int _GRNIGSTVatType;
        public int GRNIGSTVatType
        {
            get { return _GRNIGSTVatType; }
            set
            {
                if (_GRNIGSTVatType != value)
                {
                    _GRNIGSTVatType = value;
                }
            }
        }
        private int _GRNIGSTVatApplicationOn;
        public int GRNIGSTVatApplicationOn
        {
            get { return _GRNIGSTVatApplicationOn; }
            set
            {
                if (_GRNIGSTVatApplicationOn != value)
                {
                    _GRNIGSTVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


                }
            }
        }
        //***//
    }
}
