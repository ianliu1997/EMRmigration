using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory.WorkOrder
{
   public class clsWorkOrderVO:INotifyPropertyChanged, IValueObject
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

        #region W O Master Properties

        public List<long> ids { get; set; }

        public bool IsEnable { get; set; }

        public string CancellationRemark { get; set; }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
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

        //public bool Direct = false;
        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                if (_Type != value)
                {
                    _Type = value;
                    OnPropertyChanged("Type");
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

        private string _WONO;
        public string WONO
        {
            get { return _WONO; }
            set
            {
                if (_WONO != value)
                {
                    _WONO = value;
                    OnPropertyChanged("WONO");
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

        private string _SupplierName;
        public string SupplierName
        {
            get { return _SupplierName; }
            set
            {
                if (_SupplierName != value)
                {
                    _SupplierName = value;
                    OnPropertyChanged("SupplierName");
                }
            }
        }

        public bool Direct = false;
        
#region commented
        //private long _IndentID;
        //public long IndentID
        //{
        //    get { return _IndentID; }
        //    set
        //    {
        //        if (_IndentID != value)
        //        {
        //            _IndentID = value;
        //            OnPropertyChanged("IndentID");
        //        }
        //    }
        //}

        //private long _IndentUnitID;
        //public long IndentUnitID
        //{
        //    get { return _IndentUnitID; }
        //    set
        //    {
        //        if (_IndentUnitID != value)
        //        {
        //            _IndentUnitID = value;
        //            OnPropertyChanged("IndentUnitID");
        //        }
        //    }
        //}

        //private string _IndentNumber;
        //public string IndentNumber
        //{
        //    get { return _IndentNumber; }
        //    set
        //    {
        //        if (_IndentNumber != value)
        //        {
        //            _IndentNumber = value;
        //            OnPropertyChanged("IndentNumber");
        //        }
        //    }
        //}

#endregion

        private string _GRNNowithDate;
        public string GRNNowithDate
        {
            get { return _GRNNowithDate; }
            set
            {
                if (_GRNNowithDate != value)
                {
                    _GRNNowithDate = value;
                    OnPropertyChanged("GRNNowithDate");
                }
            }
        }

#region commented
        //private string _IndentNowithDate;
        //public string IndentNowithDate
        //{
        //    get { return _IndentNowithDate; }
        //    set
        //    {
        //        if (_IndentNowithDate != value)
        //        {
        //            _IndentNowithDate = value;
        //            OnPropertyChanged("IndentNowithDate");
        //        }
        //    }
        //}

        //private long _EnquiryID;
        //public long EnquiryID
        //{
        //    get { return _EnquiryID; }
        //    set
        //    {
        //        if (_EnquiryID != value)
        //        {
        //            _EnquiryID = value;
        //            OnPropertyChanged("EnquiryID");
        //        }
        //    }
        //}

#endregion 

        private long _DeliveryDuration;
        public long DeliveryDuration
        {
            get { return _DeliveryDuration; }
            set
            {
                if (_DeliveryDuration != value)
                {
                    _DeliveryDuration = value;
                    OnPropertyChanged("DeliveryDuration");
                }
            }
        }
        private string _DeliveryLocation;
        public string DeliveryLocation
        {
            get { return _DeliveryLocation; }
            set
            {
                if (_DeliveryLocation != value)
                {
                    _DeliveryLocation = value;
                    OnPropertyChanged("DeliveryLocation");
                }
            }
        }
        private long _DeliveryDays;
        public long DeliveryDays
        {
            get { return _DeliveryDays; }
            set
            {
                if (_DeliveryDays != value)
                {
                    _DeliveryDays = value;
                    OnPropertyChanged("DeliveryDays");
                }
            }
        }
        private long _PaymentMode;
        public long PaymentMode
        {
            get { return _PaymentMode; }
            set
            {
                if (_PaymentMode != value)
                {
                    _PaymentMode = value;
                    OnPropertyChanged("PaymentMode");
                }
            }
        }

        private long _PaymentTerms;
        public long PaymentTerms
        {
            get { return _PaymentTerms; }
            set
            {
                if (_PaymentTerms != value)
                {
                    _PaymentTerms = value;
                    OnPropertyChanged("PaymentTerms");
                }
            }
        }

        private string _Guarantee_Warrantee;
        public string Guarantee_Warrantee
        {
            get { return _Guarantee_Warrantee; }
            set
            {
                if (_Guarantee_Warrantee != value)
                {
                    _Guarantee_Warrantee = value;
                    OnPropertyChanged("Guarantee_Warrantee");
                }
            }
        }

        private long _Schedule;
        public long Schedule
        {
            get { return _Schedule; }
            set
            {
                if (_Schedule != value)
                {
                    _Schedule = value;
                    OnPropertyChanged("Schedule");
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

        private decimal _TotalVAT;
        public decimal TotalVAT
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

        private decimal _TotalAmount;
        public decimal TotalAmount
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

        private decimal _TotalNet;
        public decimal TotalNet
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

        private decimal _TotalDiscount;
        public decimal TotalDiscount
        {
            get { return _TotalDiscount; }
            set
            {
                if (_TotalDiscount != value)
                {
                    _TotalDiscount = value;
                    OnPropertyChanged("TotalDiscount");
                }
            }
        }

        private long _PaymentModeID;
        public long PaymentModeID
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

        private Boolean _Freezed;
        public Boolean Freezed
        {
            get { return _Freezed; }
            set
            {
                if (_Freezed != value)
                {
                    _Freezed = value;
                    OnPropertyChanged("Freezed");
                    OnPropertyChanged("IsEnabledFreezed");
                }
            }
        }

        private Boolean _IsEnabledFreezed;
        public Boolean IsEnabledFreezed
        {
            get
            {
                if (_Freezed == true)
                {
                    _IsEnabledFreezed = false;
                }
                else
                {
                    _IsEnabledFreezed = true;
                }
                return _IsEnabledFreezed;


            }
            set
            {
                if (_IsEnabledFreezed != value)
                {
                    _IsEnabledFreezed = value;
                    OnPropertyChanged("IsEnabledFreezed");
                }
            }
        }


        private bool _IsApproveded;
        public bool IsApproveded
        {
            get { return _IsApproveded; }
            set
            {
                if (_IsApproveded != value)
                {
                    _IsApproveded = value;
                    OnPropertyChanged("IsApproveded");

                }
            }
        }

        private bool _IsApprovededEnable = true;
        public bool IsApprovededEnable
        {
            get { return _IsApprovededEnable; }
            set
            {
                if (_IsApprovededEnable != value)
                {
                    _IsApprovededEnable = value;
                    OnPropertyChanged("IsApprovededEnable");

                }
            }
        }

        private string _ApprovedBy;
        public string ApprovedBy
        {
            get { return _ApprovedBy; }
            set
            {
                if (_ApprovedBy != value)
                {
                    _ApprovedBy = value;
                    OnPropertyChanged("ApprovedBy");

                }
            }
        }


        public string StoreName { get; set; }

        #endregion

        private List<clsWorkOrderDetailVO> _Items;
        public List<clsWorkOrderDetailVO> Items
        {
            get
            {
                return _Items;
            }

            set
            {
                _Items = value;
                OnPropertyChanged("Items");
            }
        }
        private List<clsWorkOrderTerms> _WorkTermsList;
        public List<clsWorkOrderTerms> WorkTermsList
        {
            get
            {
                return _WorkTermsList;
            }
            set
            {
                if (_WorkTermsList != value)
                {
                    _WorkTermsList = value;
                    OnPropertyChanged("WorkTermsList");
                }
            }

        }

        public bool EditForApprove { get; set; }  // Added by Anumani
    }

    public class clsWorkOrderDetailVO : INotifyPropertyChanged, IValueObject
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

        #region PW O Detail Properties

        private Boolean _ManuallyClosed;
        public Boolean ManuallyClosed
        {
            get
            {
                return _ManuallyClosed;
            }
            set
            {
                _ManuallyClosed = value;
                OnPropertyChanged("Manuallyclosed");
            }
        }

        private Boolean _ConditionFound;
        public Boolean ConditionFound
        {
            get
            {
                return _ConditionFound;
            }
            set
            {
                if (_ConditionFound != value)
                {
                    _ConditionFound = value;
                    OnPropertyChanged("ConditionFound");
                }
            }
        }

        //private long _RateContractID;
        //private long _RateContractUnitID;
        //public long RateContractID
        //{
        //    get { return _RateContractID; }
        //    set
        //    {
        //        if (_RateContractID != value)
        //        {
        //            _RateContractID = value;
        //            OnPropertyChanged("RateContractID");
        //        }
        //    }
        //}
        //public long RateContractUnitID
        //{
        //    get { return _RateContractUnitID; }
        //    set
        //    {
        //        if (_RateContractUnitID != value)
        //        {
        //            _RateContractUnitID = value;
        //            OnPropertyChanged("RateContractUnitID");
        //        }
        //    }
        //}
        //private String _RateContractCondition;
        //public string RateContractCondition
        //{
        //    get
        //    {
        //        return _RateContractCondition;
        //    }
        //    set
        //    {
        //        if (_RateContractCondition != value)
        //        {
        //            _RateContractCondition = value;
        //            OnPropertyChanged("RateContractCondition");
        //        }
        //    }
        //}

        //private long _IndentQuantity;
        //public long IndentQuantity
        //{
        //    get { return _IndentQuantity; }
        //    set
        //    {
        //        if (_IndentQuantity != value)
        //        {
        //            _IndentQuantity = value;
        //            OnPropertyChanged("IndentQuantity");
        //        }
        //    }
        //}
        //private long _IndentID;
        //public long IndentID
        //{
        //    get { return _IndentID; }
        //    set
        //    {
        //        if (_IndentID != value)
        //        {
        //            _IndentID = value;
        //            OnPropertyChanged("IndentID");
        //        }
        //    }
        //}
        //private long _IndentDetailID;
        //public long IndentDetailID
        //{
        //    get { return _IndentDetailID; }
        //    set
        //    {
        //        if (_IndentDetailID != value)
        //        {
        //            _IndentDetailID = value;
        //            OnPropertyChanged("IndentDetailID");
        //        }
        //    }
        //}
        private Boolean _IsMultipleContract;
        public Boolean IsMultipleContract
        {
            get { return _IsMultipleContract; }
            set
            {
                if (_IsMultipleContract != value)
                {
                    _IsMultipleContract = value;
                    OnPropertyChanged("IsMultipleContract");
                }

            }
        }
       
        private string _BarCode;
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
        private DateTime _WODate;
        public DateTime WODate
        {
            get { return _WODate; }
            set
            {
                if (_WODate != value)
                {
                    _WODate = value;
                    OnPropertyChanged("WODate");
                }
            }
        }
        
        public long ItemCategory { get; set; }
        public long ItemGroup { get; set; }
        private long _IndentDetailUnitID;
        public long IndentDetailUnitID
        {
            get { return _IndentDetailUnitID; }
            set
            {
                if (_IndentDetailUnitID != value)
                {
                    _IndentDetailUnitID = value;
                    OnPropertyChanged("IndentDetailUnitID");
                }
            }
        }

        private string _IndentNumber;
        public String IndentNumber
        {
            get { return _IndentNumber; }
            set
            {
                if (_IndentNumber != value)
                {
                    _IndentNumber = value;
                    OnPropertyChanged("IndentNumber");
                }
            }
        }
        private long _IndentUnitID;
        public long IndentUnitID
        {
            get { return _IndentUnitID; }
            set
            {
                if (_IndentUnitID != value)
                {
                    _IndentUnitID = value;
                    OnPropertyChanged("IndentUnitID");
                }
            }
        }

        private long? _ItemID;
        public long? ItemID
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
        private string _WONO;
        public String WONO
        {
            get { return _WONO; }
            set
            {
                if (_WONO != value)
                {
                    _WONO = value;
                    OnPropertyChanged("WONO");
                }
            }
        }
        private long _WOID;
        public long WOID
        {
            get { return _WOID; }
            set
            {
                if (_WOID != value)
                {
                    _WOID = value;
                    OnPropertyChanged("WOID");
                }
            }
        }

        public bool SelectItem { get; set; }

        private long _WoItemsID;
        public long WoItemsID
        {
            get { return _WoItemsID; }
            set
            {
                if (_WoItemsID != value)
                {
                    _WoItemsID = value;
                    OnPropertyChanged("WoItemsID");
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
                    if (value <= 1)
                        _Quantity = 1;
                    else
                        _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
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
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount = _Rate * _Quantity; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("TotalAmount");
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
                    OnPropertyChanged("NetAmount");

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

        private decimal _VATPer;
        public decimal VATPer
        {
            get { return _VATPer; }
            set
            {
                if (_VATPer != value)
                {
                    _VATPer = value;
                    //OnPropertyChanged("VATPer");
                    //OnPropertyChanged("VATPer");
                    //OnPropertyChanged("VATPer");
                }
            }
        }



        private decimal _VATAmount;
        public decimal VATAmount
        {
            get
            {
                _VATAmount = (((_Amount - _DiscountAmount) * _VATPercent) / 100);
                return _VATAmount;
            }
            //{
            //    if (_VATPercent != 0)
            //    {
            //        _VATAmount = ((_Amount * _VATPercent) / 100);
            //       _VATAmount = Math.Round(_VATAmount, 2);
            //        return _VATAmount; //= ((_Amount * _VATPercent) / 100);
            //    }
            //    else
            //        return _VATAmount;

            set
            {
                if (_VATAmount != value)
                {

                    _VATAmount = value;
                    //_VATPercent = 0;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("NetAmount");
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
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _DiscPercent;
        public decimal DiscPercent
        {
            get { return _DiscPercent; }
            set
            {
                if (_DiscPercent != value)
                {
                    _DiscPercent = value;
                    //OnPropertyChanged("DiscountPercent");
                    //OnPropertyChanged("DiscountAmount");
                    //OnPropertyChanged("NetAmount");
                }
            }
        }



        private decimal _DiscountAmount;
        public decimal DiscountAmount
        {
            get
            {
                return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
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
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("TotalDiscount");

                }
            }
        }


        private decimal _NetAmount;
        public decimal NetAmount
        {
            get { return _NetAmount = _Amount - _DiscountAmount + _VATAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("TotalNet");
                }
            }
        }


        private string _Specification;
        public string Specification
        {
            get { return _Specification; }
            set
            {
                if (_Specification != value)
                {
                    _Specification = value;
                    OnPropertyChanged("Specification");
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

        private string _PUM;
        public string PUM
        {
            get { return _PUM; }
            set
            {
                if (_PUM != value)
                {
                    _PUM = value;
                    OnPropertyChanged("PUM");
                }
            }
        }


        private bool _BatchesRequired;
        public bool BatchesRequired
        {
            get { return _BatchesRequired; }
            set
            {
                if (_BatchesRequired != value)
                {
                    _BatchesRequired = value;
                    OnPropertyChanged("BatchesRequired");
                }
            }
        }
        private long _WOUnitID;
        public long WOUnitID
        {
            get { return _WOUnitID; }
            set
            {
                if (_WOUnitID != value)
                {
                    _WOUnitID = value;
                    OnPropertyChanged("WOUnitID");
                }
            }
        }
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

        public double ItemTax
        {
            get;
            set;
        }

        private double _ConversionFactor = 1;
        public double ConversionFactor
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
        private List<MasterListItem> _CurrencyList;
        public List<MasterListItem> CurrencyList
        {
            get
            {
                return _CurrencyList;
            }
            set
            {
                if (value != null)
                {
                    _CurrencyList = value;
                }
            }
        }
        private MasterListItem _SelectedCurrency;
        public MasterListItem SelectedCurrency
        {
            get
            {
                return _SelectedCurrency;
            }
            set
            {
                if (value != SelectedCurrency)
                {
                    _SelectedCurrency = value;
                }
            }
        }

        #endregion
    }


    public partial class WOIndent
    {
#region Commented

        //private long _IndentId;

        //public long IndentId
        //{
        //    get { return _IndentId; }
        //    set { _IndentId = value; }
        //}
        //private long _IndentUnitId;
        //public long IndentUnitId { get; set; }
#endregion

        private long _ItemId;

        public long ItemId
        {
            get { return _ItemId; }
            set { _ItemId = value; }
        }
        private long _WOID;

        public long WOID
        {
            get { return _WOID; }
            set { _WOID = value; }
        }
        private long _WOUnitId;

        public long WOUnitId
        {
            get { return _WOUnitId; }
            set { _WOUnitId = value; }
        }
        
    }

    public partial class clsWorkOrderTerms
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

        #region Properties

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private long _WOID;

        public long WOID
        {
            get
            {
                return _WOID;
            }
            set
            {
                if (_WOID != value)
                {
                    _WOID = value;
                    OnPropertyChanged("WOID");                }
            }
        }
        private long _WOUnitId;

        public long WOUnitId
        {
            get
            {
                return _WOUnitId;
            }
            set
            {
                if (_WOUnitId != value)
                {
                    _WOUnitId = value;
                    OnPropertyChanged("WOUnitId");
                }
            }
        }

        private long _TermsAndConditionID;
        public long TermsAndConditionID
        {
            get
            {
                return _TermsAndConditionID;
            }
            set
            {
                if (_TermsAndConditionID != value)
                {
                    _TermsAndConditionID = value;
                    OnPropertyChanged("TermsAndConditionID");
                }
            }
        }

        private Boolean _Status;
        public Boolean Status
        {
            get
            {
                return _Status;
            }
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
    }
    }


