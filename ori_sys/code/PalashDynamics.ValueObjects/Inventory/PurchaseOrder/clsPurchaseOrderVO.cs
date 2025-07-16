using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsPurchaseOrderVO : INotifyPropertyChanged, IValueObject
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

        #region P O Master Properties

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

        public bool Direct = false;
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

        private string _PONO;
        public string PONO
        {
            get { return _PONO; }
            set
            {
                if (_PONO != value)
                {
                    _PONO = value;
                    OnPropertyChanged("PONO");
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

        private long _IndentID;
        public long IndentID
        {
            get { return _IndentID; }
            set
            {
                if (_IndentID != value)
                {
                    _IndentID = value;
                    OnPropertyChanged("IndentID");
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

        private string _IndentNumber;
        public string IndentNumber
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

        private string _IndentNowithDate;
        public string IndentNowithDate
        {
            get { return _IndentNowithDate; }
            set
            {
                if (_IndentNowithDate != value)
                {
                    _IndentNowithDate = value;
                    OnPropertyChanged("IndentNowithDate");
                }
            }
        }

        private long _EnquiryID;
        public long EnquiryID
        {
            get { return _EnquiryID; }
            set
            {
                if (_EnquiryID != value)
                {
                    _EnquiryID = value;
                    OnPropertyChanged("EnquiryID");
                }
            }
        }

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

        private bool _IsCancelded;
        public bool IsCancelded
        {
            get { return _IsCancelded; }
            set
            {
                if (_IsCancelded != value)
                {
                    _IsCancelded = value;
                    OnPropertyChanged("IsCancelded");

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

        private long _POApproveLvlID;
        public long POApproveLvlID
        {
            get { return _POApproveLvlID; }
            set
            {
                if (_POApproveLvlID != value)
                {
                    _POApproveLvlID = value;
                    OnPropertyChanged("POApproveLvlID");

                }
            }
        }

        private long _ApprovedByID;
        public long ApprovedByID
        {
            get { return _ApprovedByID; }
            set
            {
                if (_ApprovedByID != value)
                {
                    _ApprovedByID = value;
                    OnPropertyChanged("ApprovedByID");

                }
            }
        }

        private DateTime _ApprovedByDate;
        public DateTime ApprovedByDate
        {
            get { return _ApprovedByDate; }
            set
            {
                if (_ApprovedByDate != value)
                {
                    _ApprovedByDate = value;
                    OnPropertyChanged("ApprovedByDate");

                }
            }
        }

        private bool _IsApprovedLvl2;
        public bool IsApprovedLvl2
        {
            get { return _IsApprovedLvl2; }
            set
            {
                if (_IsApprovedLvl2 != value)
                {
                    _IsApprovedLvl2 = value;
                    OnPropertyChanged("IsApprovedLvl2");

                }
            }
        }

        private long _ApprovedByLvl2ID;
        public long ApprovedByLvl2ID
        {
            get { return _ApprovedByLvl2ID; }
            set
            {
                if (_ApprovedByLvl2ID != value)
                {
                    _ApprovedByLvl2ID = value;
                    OnPropertyChanged("ApprovedByLvl2ID");

                }
            }
        }

        private DateTime _ApprovedByLvl2Date;
        public DateTime ApprovedByLvl2Date
        {
            get { return _ApprovedByLvl2Date; }
            set
            {
                if (_ApprovedByLvl2Date != value)
                {
                    _ApprovedByLvl2Date = value;
                    OnPropertyChanged("ApprovedByLvl2Date");

                }
            }
        }

        private string _ApprovedLvl1Details;
        public string ApprovedLvl1Details
        {
            get { return _ApprovedLvl1Details; }
            set
            {
                if (_ApprovedLvl1Details != value)
                {
                    _ApprovedLvl1Details = value;
                    OnPropertyChanged("ApprovedLvl1Details");

                }
            }
        }

        private string _ApprovedLvl2Details;
        public string ApprovedLvl2Details
        {
            get { return _ApprovedLvl2Details; }
            set
            {
                if (_ApprovedLvl2Details != value)
                {
                    _ApprovedLvl2Details = value;
                    OnPropertyChanged("ApprovedLvl2Details");

                }
            }
        }


        // Added  By CDS 

        private decimal _OtherCharges;
        public decimal OtherCharges
        {
            get { return _OtherCharges; }
            set
            {
                if (_OtherCharges != value)
                {
                    _OtherCharges = value;
                    OnPropertyChanged("OtherCharges");
                }
            }
        }


        private decimal _PODiscount;
        public decimal PODiscount
        {
            get { return _PODiscount; }
            set
            {
                if (_PODiscount != value)
                {
                    _PODiscount = value;
                    OnPropertyChanged("PODiscount");
                }
            }
        }

        private decimal _PrevTotalNet;
        public decimal PrevTotalNet
        {
            get { return _PrevTotalNet; }
            set
            {
                if (_PrevTotalNet != value)
                {
                    _PrevTotalNet = value;
                    OnPropertyChanged("PrevTotalNet");
                }
            }
        }
        // END

        public string StoreName { get; set; }

        #endregion

        private List<clsPurchaseOrderDetailVO> _Items;
        public List<clsPurchaseOrderDetailVO> Items
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

        private List<clsPurchaseOrderDetailVO> _OrgItems;
        public List<clsPurchaseOrderDetailVO> OrgItems
        {
            get
            {
                return _OrgItems;
            }

            set
            {
                _OrgItems = value;
                OnPropertyChanged("OrgItems");
            }
        }
        private List<clsPurchaseOrderTerms> _PurchaseTermsList;
        public List<clsPurchaseOrderTerms> PurchaseTermsList
        {
            get
            {
                return _PurchaseTermsList;
            }
            set
            {
                if (_PurchaseTermsList != value)
                {
                    _PurchaseTermsList = value;
                    OnPropertyChanged("PurchaseTermsList");
                }
            }

        }
        //Addde By Bhushanp 22062017

        private decimal _TotalSGST;

        public decimal TotalSGST
        {
            get { return _TotalSGST; }
            set
            {
                if (_TotalSGST != value)
                {
                    _TotalSGST = value;
                    OnPropertyChanged("TotalSGST");
                }
            }
        }

        private decimal _TotalCGST;

        public decimal TotalCGST
        {
            get { return _TotalCGST; }
            set
            {
                if (_TotalCGST != value)
                {
                    _TotalCGST = value;
                    OnPropertyChanged("TotalCGST");
                }
            }
        }

        private decimal _TotalIGST;

        public decimal TotalIGST
        {
            get { return _TotalIGST; }
            set
            {
                if (_TotalIGST != value)
                {
                    _TotalIGST = value;
                    OnPropertyChanged("TotalIGST");
                }
            }
        }

        public bool EditForApprove { get; set; }  //Added By Umesh for save po history

        #region PO Auto Close Functionality

        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x Begin : Creating 2 properities POAutoCloseDuration & POAutoCloseDurationDate x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        private int _POAutoCloseDuration;
        public int POAutoCloseDuration
        {
            get { return _POAutoCloseDuration; }
            set
            {
                if (_POAutoCloseDuration != value)
                {
                    _POAutoCloseDuration = value;
                    OnPropertyChanged("POAutoCloseDuration");

                }
            }
        }

        private DateTime? _POAutoCloseDurationDate;
        public DateTime? POAutoCloseDurationDate
        {
            get { return _POAutoCloseDurationDate; }
            set
            {
                if (_POAutoCloseDurationDate != value)
                {
                    _POAutoCloseDurationDate = value;
                    OnPropertyChanged("POAutoCloseDurationDate");

                }
            }
        }



        private string _POAutoCloseReason;
        public string POAutoCloseReason
        {
            get { return _POAutoCloseReason; }
            set
            {
                if (_POAutoCloseReason != value)
                {
                    _POAutoCloseReason = value;
                    OnPropertyChanged("POAutoCloseReason");

                }
            }
        }


        private bool _IsAutoClose = false;
        public bool IsAutoClose
        {
            get { return _IsAutoClose; }
            set
            {
                if (_IsAutoClose != value)
                {
                    _IsAutoClose = value;
                    OnPropertyChanged("IsAutoClose");

                }
            }
        }


        /*  x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x End : Creating 2 properities POAutoCloseDuration & POAutoCloseDurationDate x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x */

        #endregion
    }

    public class clsPurchaseOrderDetailVO : INotifyPropertyChanged, IValueObject
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

        #region P O Detail Properties

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

        private long _RateContractID;
        private long _RateContractUnitID;
        public long RateContractID
        {
            get { return _RateContractID; }
            set
            {
                if (_RateContractID != value)
                {
                    _RateContractID = value;
                    OnPropertyChanged("RateContractID");
                }
            }
        }
        public long RateContractUnitID
        {
            get { return _RateContractUnitID; }
            set
            {
                if (_RateContractUnitID != value)
                {
                    _RateContractUnitID = value;
                    OnPropertyChanged("RateContractUnitID");
                }
            }
        }
        private String _RateContractCondition;
        public string RateContractCondition
        {
            get
            {
                return _RateContractCondition;
            }
            set
            {
                if (_RateContractCondition != value)
                {
                    _RateContractCondition = value;
                    OnPropertyChanged("RateContractCondition");
                }
            }
        }
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
        //Added by Rohitkumar on 8thOctomber2013 for the BarCode 
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
        private DateTime _PODate;
        public DateTime PODate
        {
            get { return _PODate; }
            set
            {
                if (_PODate != value)
                {
                    _PODate = value;
                    OnPropertyChanged("PODate");
                }
            }
        }
        private long _IndentQuantity;
        public long IndentQuantity
        {
            get { return _IndentQuantity; }
            set
            {
                if (_IndentQuantity != value)
                {
                    _IndentQuantity = value;
                    OnPropertyChanged("IndentQuantity");
                }
            }
        }
        private long _IndentID;
        public long IndentID
        {
            get { return _IndentID; }
            set
            {
                if (_IndentID != value)
                {
                    _IndentID = value;
                    OnPropertyChanged("IndentID");
                }
            }
        }
        private long _IndentDetailID;
        public long IndentDetailID
        {
            get { return _IndentDetailID; }
            set
            {
                if (_IndentDetailID != value)
                {
                    _IndentDetailID = value;
                    OnPropertyChanged("IndentDetailID");
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

        //OLD
        //private long _ItemID;
        //public long ItemID
        //{
        //    get { return _ItemID; }
        //    set
        //    {
        //        if (_ItemID != value)
        //        {
        //            _ItemID = value;
        //            OnPropertyChanged("ItemID");
        //        }
        //    }
        //}
        private string _PONO;
        public String PONO
        {
            get { return _PONO; }
            set
            {
                if (_PONO != value)
                {
                    _PONO = value;
                    OnPropertyChanged("PONO");
                }
            }
        }
        private long _POID;
        public long POID
        {
            get { return _POID; }
            set
            {
                if (_POID != value)
                {
                    _POID = value;
                    OnPropertyChanged("POID");
                }
            }
        }

        private long _PoDetailsID;
        public long PoDetailsID
        {
            get { return _PoDetailsID; }
            set
            {
                if (_PoDetailsID != value)
                {
                    _PoDetailsID = value;
                    OnPropertyChanged("PoDetailsID");
                }
            }
        }

        private long _PoDetailsUnitID;
        public long PoDetailsUnitID
        {
            get { return _PoDetailsUnitID; }
            set
            {
                if (_PoDetailsUnitID != value)
                {
                    _PoDetailsUnitID = value;
                    OnPropertyChanged("PoDetailsUnitID");
                }
            }
        }

        public bool SelectItem { get; set; }

        private bool _CheckInserted = false;
        public bool CheckInserted
        {
            get
            {
                return _CheckInserted;
            }
            set
            {
                if (_CheckInserted != value)
                {
                    _CheckInserted = value;
                    OnPropertyChanged("CheckInserted");
                }
            }
        }

        private long _PoItemsID;
        public long PoItemsID
        {
            get { return _PoItemsID; }
            set
            {
                if (_PoItemsID != value)
                {
                    _PoItemsID = value;
                    OnPropertyChanged("PoItemsID");
                }
            }
        }

        //OLD
        //private decimal _Quantity;
        //public decimal Quantity
        //{
        //    get { return _Quantity; }
        //    set
        //    {
        //        if (_Quantity != value)
        //        {
        //            if (value <= 1)
        //                _Quantity = 1;
        //            else
        //                _Quantity = value;
        //            OnPropertyChanged("Quantity");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("DiscountAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {

                if (_Quantity != value)
                {
                    //if (value <= 1)
                    //    _Quantity = 1;
                    //else
                    //    _Quantity = value;
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("NetAmount");

                }

            }
        }

        private decimal _TotalQty;
        public decimal TotalQty
        {
            get { return _TotalQty; }
            set
            {
                if (_TotalQty != value)
                {

                    _TotalQty = value;
                    OnPropertyChanged("TotalQty");
                }
            }
        }

        public bool SinleLineItem = false;

        private double _PRPendingQuantity;
        public double PRPendingQuantity
        {
            get { return _PRPendingQuantity; }
            set
            {
                if (_PRPendingQuantity != value)
                {

                    _PRPendingQuantity = value;
                    OnPropertyChanged("PRPendingQuantity");
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

                //if (_POItemVatType == 2) //for exclusive Cost Rate
                //{
                //    return _CostRate = _Rate * _Quantity;
                //}
                //else //for inclusive Cost Rate
                //{

                //    return _CostRate = (_Rate / ((1 + (VATPercent / 100)))) * _Quantity;

                //}
            }
            set
            {
                if (_CostRate != value)
                {
                    _CostRate = value;
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("DiscountPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                }
            }

        }


        private decimal _PRQuantity;
        public decimal PRQuantity
        {
            get { return _PRQuantity; }
            set
            {
                if (_PRQuantity != value)
                {
                    _PRQuantity = value;
                    OnPropertyChanged("PRQuantity");

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

        private decimal _PRQTY;
        public decimal PRQTY
        {
            get { return _PRQTY; }
            set
            {
                if (_PRQTY != value)
                {
                    _PRQTY = value;
                    OnPropertyChanged("PRQTY");
                }
            }
        }


        private decimal _PRPendingQty;
        public decimal PRPendingQty
        {
            get { return _PRPendingQty; }
            set
            {
                if (_PRPendingQty != value)
                {
                    _PRPendingQty = value;
                    OnPropertyChanged("PRPendingQty");
                }
            }
        }

        //OLD
        //private decimal _Rate;
        //public decimal Rate
        //{
        //    get { return _Rate; }
        //    set
        //    {
        //        if (_Rate != value)
        //        {
        //            _Rate = value;
        //            OnPropertyChanged("Rate");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("DiscountAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

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
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("DiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        //OLD
        //private decimal _Amount;
        //public decimal Amount
        //{
        //    get { return _Amount = _Rate * _Quantity; }
        //    set
        //    {
        //        if (_Amount != value)
        //        {
        //            _Amount = value;
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("DiscountAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //            OnPropertyChanged("TotalAmount");
        //        }
        //    }
        //}

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

        //OLD
        //private decimal _MRP;
        //public decimal MRP
        //{
        //    get { return _MRP; }
        //    set
        //    {
        //        if (_MRP != value)
        //        {
        //            _MRP = value;
        //            OnPropertyChanged("MRP");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("NetAmount");

        //        }
        //    }
        //}

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
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("TotalAmount");

                }
            }
        }
        private decimal _AbatedMRP;
        public decimal AbatedMRP
        {
            get { return _AbatedMRP; }
            set
            {
                if (_AbatedMRP != value)
                {
                    _AbatedMRP = value;
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

        private decimal _ItemVATPercent;
        public decimal ItemVATPercent
        {
            get { return _ItemVATPercent; }
            set
            {
                if (_ItemVATPercent != value)
                {
                    _ItemVATPercent = value;
                    OnPropertyChanged("ItemVATPercent");
                    OnPropertyChanged("ItemVATAmount");
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


        //OLD
        //private decimal _VATAmount;
        //public decimal VATAmount
        //{
        //    get
        //    {
        //        _VATAmount = (((_Amount - _DiscountAmount) * _VATPercent) / 100);
        //        return _VATAmount;
        //    }
        //    //{
        //    //    if (_VATPercent != 0)
        //    //    {
        //    //        _VATAmount = ((_Amount * _VATPercent) / 100);
        //    //       _VATAmount = Math.Round(_VATAmount, 2);
        //    //        return _VATAmount; //= ((_Amount * _VATPercent) / 100);
        //    //    }
        //    //    else
        //    //        return _VATAmount;

        //    set
        //    {
        //        if (_VATAmount != value)
        //        {

        //            _VATAmount = value;
        //            //_VATPercent = 0;
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("VATPercent");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private decimal _ItemVATAmount;
        public decimal ItemVATAmount
        {
            get
            {
                if (_Quantity > 0)
                {
                    if (ItemVATPercent > 0)
                    {
                        //exclusive tax
                        //check type inclusive or exclusive
                        if (_POItemOtherTaxType == 2)
                        {
                            if (_POItemOtherTaxApplicationOn == 1)
                            {
                                _ItemVATAmount = (((_CostRate - _DiscountAmount) * _ItemVATPercent) / 100);
                                return _ItemVATAmount;
                            }
                            else
                            {
                                //_ItemVATAmount = (((_Amount) * _ItemVATPercent) / 100);
                                _ItemVATAmount = (((_Amount - _DiscountAmount) * _ItemVATPercent) / 100);
                                return _ItemVATAmount;
                            }
                        }
                        else if (_POItemOtherTaxType == 1)
                        {
                            if (_POItemOtherTaxApplicationOn == 1)
                            {
                                //inclusive tax
                                decimal calculation = _CostRate - _DiscountAmount;
                                _ItemVATAmount = ((calculation) / (100 + _ItemVATPercent) * 100);
                                return _ItemVATAmount = calculation - _ItemVATAmount;
                            }
                            else
                            {
                                //inclusive tax
                                ////_ItemVATAmount = ((_Amount) / (100 + _ItemVATPercent) * 100);
                                //_ItemVATAmount = ((_Amount - _DiscountAmount) / (100 + _ItemVATPercent) * 100);
                                //return _ItemVATAmount = _Amount - _ItemVATAmount;

                                decimal calculation2 = ((_Amount) / (100 + _ItemVATPercent) * 100);
                                _ItemVATAmount = (calculation2 * (_ItemVATPercent / 100));  //_ItemVATAmount = ((calculation2 - _DiscountAmount) * (_ItemVATPercent / 100)); commented by Ashish Z. on 140416
                                //_ItemVATAmount = _Amount - _ItemVATAmount;
                                return _ItemVATAmount;
                            }
                        }
                        else
                        {
                            _ItemVATAmount = (((_CostRate - _DiscountAmount) * _ItemVATPercent) / 100);
                            return _ItemVATAmount;
                        }
                    }
                }
                return _ItemVATAmount = 0;
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
                if (_ItemVATAmount != value)
                {
                    _ItemVATAmount = value;
                    //_VATPercent = 0;
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _VATAmount;
        public decimal VATAmount
        {
            get
            {
                if (_Quantity > 0)
                {
                    if (VATPercent > 0)
                    {
                        //check type inclusive or exclusive
                        if (_POItemVatType == 2)
                        {
                            if (_POItemVatApplicationOn == 1)
                            {
                                //for cost price
                                _VATAmount = (((_CostRate - _DiscountAmount) * _VATPercent) / 100);
                                // _VATAmount = (((_CostRate) * _VATPercent) / 100);
                                return _VATAmount;
                            }
                            else
                            {
                                //mrp
                                _VATAmount = (((_Amount - _DiscountAmount) * _VATPercent) / 100);
                                //_VATAmount = (((_Amount) * _VATPercent) / 100);
                                return _VATAmount;
                            }
                        }
                        else if (_POItemVatType == 1)
                        {
                            if (_POItemVatApplicationOn == 1)
                            {
                                //cost price exclusive 
                                decimal calculation = _CostRate - _DiscountAmount;
                                _VATAmount = ((calculation) / (100 + _VATPercent) * 100);
                                _VATAmount = calculation - _VATAmount;
                                return _VATAmount;
                            }
                            else
                            {
                                //mrp inclusive
                                ////_VATAmount = ((_Amount) / (100 + _VATPercent) * 100);
                                //_VATAmount = ((_Amount - _DiscountAmount) / (100 + _VATPercent) * 100);
                                //_VATAmount = _Amount - _VATAmount;
                                //return _VATAmount;

                                decimal calculation2 = ((_Amount) / (100 + _VATPercent) * 100);
                                _VATAmount = (calculation2 * (_VATPercent / 100)); //_VATAmount = ((calculation2 - _DiscountAmount) * (_VATPercent / 100));  commented by Ashish Z. on 140416
                                //_VATAmount = _Amount - _VATAmount;
                                return _VATAmount;
                            }
                        }
                        else
                        {
                            _VATAmount = (((_CostRate - _DiscountAmount) * _VATPercent) / 100);
                            // _VATAmount = (((_CostRate) * _VATPercent) / 100);
                            return _VATAmount;
                        }
                    }
                }
                return _VATAmount = 0;
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

        //OLD
        //private decimal _DiscountPercent;
        //public decimal DiscountPercent
        //{
        //    get { return _DiscountPercent; }
        //    set
        //    {
        //        if (_DiscountPercent != value)
        //        {
        //            _DiscountPercent = value;
        //            OnPropertyChanged("DiscountPercent");
        //            OnPropertyChanged("DiscountAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

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
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
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


        //OLD
        //private decimal _DiscountAmount;
        //public decimal DiscountAmount
        //{
        //    get
        //    {
        //        return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
        //        //if (_DiscountPercent != 0)
        //        //{
        //        //    return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
        //        //}

        //    }
        //    set
        //    {
        //        if (_DiscountAmount != value)
        //        {
        //            _DiscountAmount = value;
        //            //_DiscountPercent = 0;
        //            OnPropertyChanged("DiscountAmount");
        //            OnPropertyChanged("DiscountPercent");
        //            OnPropertyChanged("NetAmount");
        //            OnPropertyChanged("TotalDiscount");

        //        }
        //    }
        //}

        private decimal _DiscountAmount;
        public decimal DiscountAmount
        {
            get
            {
                //check type inclusive or exclusive
                if (_POItemVatType == 2) //for exclusive VAT
                {
                    return _DiscountAmount = ((_CostRate * _DiscountPercent) / 100);
                }
                else //for inclusive VAT
                {

                    //return _DiscountAmount = (((_CostRate - _VATAmount ) * _DiscountPercent) / 100);
                    return _DiscountAmount = ((_CostRate * _DiscountPercent) / 100);
                }

                ////if (_POItemVatApplicationOn == 1)
                ////{
                //return _DiscountAmount = ((_CostRate * _DiscountPercent) / 100);
                ////}
                ////else
                ////{
                ////    return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
                ////}


                ////if (_DiscountPercent != 0)
                ////{
                ////    return _DiscountAmount = ((_Amount * _DiscountPercent) / 100);
                ////}
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
                    OnPropertyChanged("TotalDiscount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");
                    OnPropertyChanged("NetAmount");


                }
            }
        }

        //OLD
        //private decimal _NetAmount;
        //public decimal NetAmount
        //{
        //    get { return _NetAmount = _Amount - _DiscountAmount + _VATAmount; }
        //    set
        //    {
        //        if (_NetAmount != value)
        //        {
        //            _NetAmount = value;
        //            OnPropertyChanged("NetAmount");
        //            OnPropertyChanged("TotalNet");
        //        }
        //    }
        //}

        private decimal _NetAmount;
        public decimal NetAmount
        {
            get
            {
                if (_VATPercent != 0)
                {
                    //check type inclusive or exclusive
                    if (_POItemVatType == 2)
                    {
                        //if (_POItemVatApplicationOn == 1)
                        //{
                        return _NetAmount = (_CostRate - _DiscountAmount) + _VATAmount + _ItemVATAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;  //for exclusive VAT
                        //}
                        //else
                        //{
                        //    return _NetAmount = _CostRate - _DiscountAmount + _VATAmount + _ItemVATAmount;
                        //}
                    }
                    else
                    {
                        //    //if (_POItemVatApplicationOn == 1)
                        //{
                        //return _NetAmount = _CostRate - _DiscountAmount;
                        //}
                        //else
                        //{
                        //    return _NetAmount = _Amount - _DiscountAmount;
                        //}
                        if (_POItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            return _NetAmount = (_CostRate - _DiscountAmount);  // for Inclusive VAT
                        }
                        else  //for Applicable on MRP
                        {
                            return _NetAmount = _CostRate - _DiscountAmount + _VATAmount + _ItemVATAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        }
                    }
                }
                else if ((_SGSTPercent != 0 && _CGSTPercent != 0) || _IGSTPercent != 0)
                {
                    //check type inclusive or exclusive
                    if (_POSGSTVatType == 2 && _POCGSTVatType == 2 || _POIGSTVatType == 2)
                    {
                        return _NetAmount = (_CostRate - _DiscountAmount) + _VATAmount + _ItemVATAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;  //for exclusive VAT
                    }
                    else
                    {
                        if ((_POSGSTVatApplicationOn == 1 && _POCGSTVatApplicationOn == 1) && _POIGSTVatApplicationOn == 1) //for Applicable on CP
                        {
                            return _NetAmount = (_CostRate - _DiscountAmount);  // for Inclusive VAT
                        }
                        else  //for Applicable on MRP
                        {
                            return _NetAmount = _CostRate - _DiscountAmount + _VATAmount + _ItemVATAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        }
                    }
                }
                else
                {
                    if (_POItemOtherTaxType == 2)
                    {
                        //if (_POItemOtherTaxApplicationOn == 1)
                        //{
                        return _NetAmount = (_CostRate - _DiscountAmount) + _VATAmount + _ItemVATAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        //}
                        //else
                        //{
                        //    return _NetAmount = _CostRate - _DiscountAmount + _VATAmount + _ItemVATAmount;
                        //}
                    }
                    else
                    {
                        //if (_POItemOtherTaxApplicationOn == 1)
                        //{
                        // return _NetAmount = _CostRate - _DiscountAmount;
                        //}
                        //else
                        //{
                        //    return _NetAmount = _Amount - _DiscountAmount;
                        //}
                        if (_POItemVatApplicationOn == 1) //for Applicable on CP
                        {
                            return _NetAmount = (_CostRate - _DiscountAmount);  // for Inclusive VAT
                        }
                        else  //for Applicable on MRP
                        {
                            return _NetAmount = _CostRate - _DiscountAmount + _VATAmount + _ItemVATAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
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
        private long _POUnitID;
        public long POUnitID
        {
            get { return _POUnitID; }
            set
            {
                if (_POUnitID != value)
                {
                    _POUnitID = value;
                    OnPropertyChanged("POUnitID");
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

        private decimal _OtherCharges;
        public decimal OtherCharges
        {
            get { return _OtherCharges; }
            set
            {
                if (_OtherCharges != value)
                {
                    _OtherCharges = value;
                    OnPropertyChanged("OtherCharges");
                }
            }
        }

        private decimal _PODiscount;
        public decimal PODiscount
        {
            get { return _PODiscount; }
            set
            {
                if (_PODiscount != value)
                {
                    _PODiscount = value;
                    OnPropertyChanged("PODiscount");
                }
            }
        }

        private decimal _PrevTotalNet;
        public decimal PrevTotalNet
        {
            get { return _PrevTotalNet; }
            set
            {
                if (_PrevTotalNet != value)
                {
                    _PrevTotalNet = value;
                    OnPropertyChanged("PrevTotalNet");
                }
            }
        }

        //OLD
        //private double _ConversionFactor = 1;
        //public double ConversionFactor
        //{
        //    get { return _ConversionFactor; }
        //    set
        //    {
        //        if (_ConversionFactor != value)
        //        {
        //            _ConversionFactor = value;
        //            OnPropertyChanged("ConversionFactor");
        //        }
        //    }
        //}

        //Added BY CDS

        private int _POItemVatType;
        public int POItemVatType
        {
            get { return _POItemVatType; }
            set
            {
                if (_POItemVatType != value)
                {
                    _POItemVatType = value;
                }
            }
        }
        private int _POItemVatApplicationOn;
        public int POItemVatApplicationOn
        {
            get { return _POItemVatApplicationOn; }
            set
            {
                if (_POItemVatApplicationOn != value)
                {
                    _POItemVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


                }
            }
        }

        private int _POItemOtherTaxType;
        public int POItemOtherTaxType
        {
            get { return _POItemOtherTaxType; }
            set
            {
                if (_POItemOtherTaxType != value)
                {
                    _POItemOtherTaxType = value;
                }
            }
        }
        private int _POItemOtherTaxApplicationOn;
        public int POItemOtherTaxApplicationOn
        {
            get { return _POItemOtherTaxApplicationOn; }
            set
            {
                if (_POItemOtherTaxApplicationOn != value)
                {
                    _POItemOtherTaxApplicationOn = value;
                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***

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

        private bool _IsItemBlock;
        public bool IsItemBlock
        {
            get { return _IsItemBlock; }
            set
            {
                if (_IsItemBlock != value)
                {
                    _IsItemBlock = value;
                    OnPropertyChanged("IsItemBlock");
                }
            }
        }
        //END

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

        # region Conversion Factor

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

        private float _RequiredQuantity;
        public float RequiredQuantity
        {
            get { return _RequiredQuantity; }
            set
            {
                if (_RequiredQuantity != value)
                {
                    _RequiredQuantity = value;
                    OnPropertyChanged("RequiredQuantity");
                }
            }
        }

        //private float _ConversionFactor = 1;
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

        private string _UOM;
        public string UOM
        {
            get { return _UOM; }
            set
            {
                if (_UOM != value)
                {
                    _UOM = value;
                    OnPropertyChanged("UOM");
                }
            }
        }

        //private string _SelectedUOM.Description;
        //public string SelectedUOM.Description
        //{
        //    get { return _SelectedUOM; }
        //    set
        //    {
        //        if (_SelectedUOM != value)
        //        {
        //            _SelectedUOM = value;
        //            OnPropertyChanged("SelectedUOM.Description");
        //        }
        //    }
        //}

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

        private long _POPUOMID;
        public long POPUOMID
        {
            get { return _POPUOMID; }
            set
            {
                if (_POPUOMID != value)
                {
                    _POPUOMID = value;
                    OnPropertyChanged("POPUOMID");
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


        private string _PRUOM;
        public string PRUOM
        {
            get { return _PRUOM; }
            set
            {
                if (_PRUOM != value)
                {
                    _PRUOM = value;
                    OnPropertyChanged("PRUOM");
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private float _GRNApprItemQuantity;
        public float GRNApprItemQuantity
        {
            get
            {
                return _GRNApprItemQuantity;
            }
            set
            {
                if (_GRNApprItemQuantity != value)
                {
                    _GRNApprItemQuantity = value;
                    OnPropertyChanged("GRNApprItemQuantity");
                }
            }
        }

        private float _GRNPendItemQuantity;
        public float GRNPendItemQuantity
        {
            get
            {
                return _GRNPendItemQuantity;
            }
            set
            {
                if (_GRNPendItemQuantity != value)
                {
                    _GRNPendItemQuantity = value;
                    OnPropertyChanged("GRNPendItemQuantity");
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

        private float _StockingToBaseCF;
        public float StockingToBaseCF
        {
            get
            {
                return _StockingToBaseCF;
            }
            set
            {
                if (value != _StockingToBaseCF)
                {
                    _StockingToBaseCF = value;
                    OnPropertyChanged("StockingToBaseCF");
                }
            }
        }
        private float _PurchaseToBaseCF;
        public float PurchaseToBaseCF
        {
            get
            {
                return _PurchaseToBaseCF;
            }
            set
            {
                if (value != _PurchaseToBaseCF)
                {
                    _PurchaseToBaseCF = value;
                    OnPropertyChanged("PurchaseToBaseCF");
                }
            }
        }

        private string _Rackname;
        public string Rackname
        {
            get
            {
                return _Rackname;
            }
            set
            {
                if (value != _Rackname)
                {
                    _Rackname = value;
                    OnPropertyChanged("Rackname");
                }
            }
        }
        private string _Shelfname;
        public string Shelfname
        {
            get
            {
                return _Shelfname;
            }
            set
            {
                if (value != _Shelfname)
                {
                    _Shelfname = value;
                    OnPropertyChanged("Shelfname");
                }
            }
        }
        private string _Containername;
        public string Containername
        {
            get
            {
                return _Containername;
            }
            set
            {
                if (value != _Containername)
                {
                    _Containername = value;
                    OnPropertyChanged("Containername");
                }
            }
        }

        private float _POApprItemQty;
        public float POApprItemQty
        {
            get
            {
                return _POApprItemQty;
            }
            set
            {
                if (_POApprItemQty != value)
                {
                    _POApprItemQty = value;
                    OnPropertyChanged("POApprItemQty");
                }
            }
        }

        private float _POPendingItemQty;
        public float POPendingItemQty
        {
            get
            {
                return _POPendingItemQty;
            }
            set
            {
                if (_POPendingItemQty != value)
                {
                    _POPendingItemQty = value;
                    OnPropertyChanged("POPendingItemQty");
                }
            }
        }

        private float _PODetailsViewTimeQty;  // to check pending quantity validation at the time of PO Item Qyantity view & Edit.
        public float PODetailsViewTimeQty
        {
            get
            {
                return _PODetailsViewTimeQty;
            }
            set
            {
                if (_PODetailsViewTimeQty != value)
                {
                    _PODetailsViewTimeQty = value;
                    OnPropertyChanged("PODetailsViewTimeQty");
                }
            }
        }

        # endregion

        private long _FromStoreID;
        public long FromStoreID
        {
            get { return _FromStoreID; }
            set
            {
                if (_FromStoreID != value)
                {
                    _FromStoreID = value;
                    OnPropertyChanged("FromStoreID");
                }
            }
        }

        #endregion

        private Boolean _IsRateContractAppliedToItem;
        public Boolean IsRateContractAppliedToItem
        {
            get
            {
                return _IsRateContractAppliedToItem;
            }
            set
            {
                _IsRateContractAppliedToItem = value;
                OnPropertyChanged("IsRateContractAppliedToItem");
            }
        }

        private string _lnkContract = "Not Applied";
        public string lnkContract
        {
            get
            {
                return _lnkContract;
            }
            set
            {
                _lnkContract = value;
                OnPropertyChanged("lnkContract");
            }
        }

        //add property on 25Oct2018 to enable Rate Contract link while click on Verify And Approve PO
        private Boolean _IsRateContractLinkEnable;
        public Boolean IsRateContractLinkEnable
        {
            get
            {
                return _IsRateContractLinkEnable;
            }
            set
            {
                _IsRateContractLinkEnable = value;
                OnPropertyChanged("IsRateContractLinkEnable");
            }
        }

        private double _POFinalPendingQuantity;
        public double POFinalPendingQuantity
        {
            get { return _POFinalPendingQuantity; }
            set
            {
                if (_POFinalPendingQuantity != value)
                {

                    _POFinalPendingQuantity = value;
                    OnPropertyChanged("POFinalPendingQuantity");
                }
            }
        }

        private List<MasterListItem> _ApplicableOnList;
        public List<MasterListItem> ApplicableOnList
        {
            get
            {
                return _ApplicableOnList;
            }
            set
            {
                if (value != null)
                {
                    _ApplicableOnList = value;
                }
            }
        }


        private MasterListItem _SelectedApplicableOn;
        public MasterListItem SelectedApplicableOn
        {
            get
            {
                return _SelectedApplicableOn;
            }
            set
            {
                if (value != SelectedApplicableOn)
                {
                    _SelectedApplicableOn = value;
                }
            }
        }
        private decimal _SGSTPercent;
        public decimal SGSTPercent
        {
            get
            {
                return _SGSTPercent;
            }
            set
            {
                if (value != _SGSTPercent)
                {
                    _SGSTPercent = value;
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        private decimal _CGSTPercent;
        public decimal CGSTPercent
        {
            get
            {
                return _CGSTPercent;
            }
            set
            {
                if (value != _CGSTPercent)
                {
                    _CGSTPercent = value;
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        private decimal _IGSTPercent;
        public decimal IGSTPercent
        {
            get
            {
                return _IGSTPercent;
            }
            set
            {
                if (value != _IGSTPercent)
                {
                    _IGSTPercent = value;
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private decimal _SGSTAmount;
        public decimal SGSTAmount
        {
            get
            {
                if (_Quantity > 0)
                {
                    if (_SGSTPercent > 0)
                    {
                        if (_POSGSTVatType == 2)
                        {
                            //check type inclusive or exclusive                       
                            if (_POSGSTVatApplicationOn == 1)
                            {
                                //for cost price
                                _SGSTAmount = (((_CostRate - _DiscountAmount) * _SGSTPercent) / 100);
                                // _SGSTAmount = (((_CostRate) * _SGSTPercent) / 100);
                                return _SGSTAmount;
                            }
                            else
                            {
                                //mrp
                                _SGSTAmount = (((_Amount - _DiscountAmount) * _SGSTPercent) / 100);
                                //_SGSTAmount = (((_Amount) * _SGSTPercent) / 100);
                                return _SGSTAmount;
                            }
                        }
                        else if (_POSGSTVatType == 1)
                        {
                            if (_POSGSTVatApplicationOn == 1)
                            {
                                //cost price exclusive 
                                decimal calculation = _CostRate - _DiscountAmount;
                                //decimal calculation = _CostRate ;
                                _SGSTAmount = ((calculation) / (100 + _SGSTPercent) * 100);
                                _SGSTAmount = calculation - _SGSTAmount;
                                return _SGSTAmount;
                            }
                            else
                            {
                                decimal calculation2 = ((_Amount) / (100 + _SGSTPercent) * 100);
                                _SGSTAmount = (calculation2 * (_SGSTPercent / 100)); //_VATAmount = ((calculation2 - _DiscountAmount) * (_VATPercent / 100));  commented by Ashish Z. on 140416
                                return _SGSTAmount;
                            }
                        }
                        else
                        {
                            _SGSTAmount = (((_CostRate - _DiscountAmount) * _SGSTPercent) / 100);
                            //_SGSTAmount = (((_CostRate) * _SGSTPercent) / 100);
                            return _SGSTAmount;
                        }


                    }
                }
                return _SGSTAmount = 0;
            }
            set
            {
                if (_SGSTAmount != value)
                {

                    _SGSTAmount = value;
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("SGSTPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _CGSTAmount;
        public decimal CGSTAmount
        {
            get
            {
                if (_Quantity > 0)
                {
                    if (_CGSTPercent > 0)
                    {
                        if (_POCGSTVatType == 2)
                        {
                            //check type inclusive or exclusive                       
                            if (_POCGSTVatApplicationOn == 1)
                            {
                                //for cost price
                                _CGSTAmount = (((_CostRate - _DiscountAmount) * _CGSTPercent) / 100);
                                //_CGSTAmount = (((_CostRate ) * _CGSTPercent) / 100);
                                return _CGSTAmount;
                            }
                            else
                            {
                                //mrp
                                _CGSTAmount = (((_Amount - _DiscountAmount) * _CGSTPercent) / 100);
                                //_CGSTAmount = (((_Amount) * _CGSTPercent) / 100);
                                return _CGSTAmount;
                            }
                        }
                        else if (_POCGSTVatType == 1)
                        {
                            if (_POCGSTVatApplicationOn == 1)
                            {
                                //cost price exclusive 
                                decimal calculation = _CostRate - _DiscountAmount;
                                //decimal calculation = _CostRate;
                                _CGSTAmount = ((calculation) / (100 + _CGSTPercent) * 100);
                                _CGSTAmount = calculation - _CGSTAmount;
                                return _CGSTAmount;
                            }
                            else
                            {
                                decimal calculation2 = ((_Amount) / (100 + _CGSTPercent) * 100);
                                _CGSTAmount = (calculation2 * (_CGSTPercent / 100)); //_VATAmount = ((calculation2 - _DiscountAmount) * (_VATPercent / 100));  commented by Ashish Z. on 140416
                                return _CGSTAmount;
                            }
                        }
                        else
                        {
                            _CGSTAmount = (((_CostRate - _DiscountAmount) * _CGSTPercent) / 100);
                            //_CGSTAmount = (((_CostRate) * _CGSTPercent) / 100);
                            return _CGSTAmount;
                        }
                    }
                }
                return _CGSTAmount = 0;
            }
            set
            {
                if (_CGSTAmount != value)
                {

                    _CGSTAmount = value;
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("CGSTPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private decimal _IGSTAmount;
        public decimal IGSTAmount
        {
            get
            {
                if (_Quantity > 0)
                {
                    if (_IGSTPercent > 0)
                    {
                        if (_POIGSTVatType == 2)
                        {
                            //check type inclusive or exclusive                       
                            if (_POIGSTVatApplicationOn == 1)
                            {
                                //for cost price
                                _IGSTAmount = (((_CostRate - _DiscountAmount) * _IGSTPercent) / 100);
                                //_IGSTAmount = (((_CostRate) * _IGSTPercent) / 100);
                                return _IGSTAmount;
                            }
                            else
                            {
                                //mrp
                                _IGSTAmount = (((_Amount - _DiscountAmount) * _IGSTPercent) / 100);
                                //_IGSTAmount = (((_Amount) * _IGSTPercent) / 100);
                                return _IGSTAmount;
                            }
                        }
                        else if (_POIGSTVatType == 1)
                        {
                            if (_POIGSTVatApplicationOn == 1)
                            {
                                //cost price exclusive 
                                decimal calculation = _CostRate - _DiscountAmount;
                                //decimal calculation = _CostRate;
                                _IGSTAmount = ((calculation) / (100 + _IGSTPercent) * 100);
                                _IGSTAmount = calculation - _IGSTAmount;
                                return _IGSTAmount;
                            }
                            else
                            {
                                decimal calculation2 = ((_Amount) / (100 + _IGSTPercent) * 100);
                                _IGSTAmount = (calculation2 * (_IGSTPercent / 100)); //_VATAmount = ((calculation2 - _DiscountAmount) * (_VATPercent / 100));  commented by Ashish Z. on 140416
                                return _IGSTAmount;
                            }
                        }
                        else
                        {
                            _IGSTAmount = (((_CostRate - _DiscountAmount) * _IGSTPercent) / 100);
                            //_IGSTAmount = (((_CostRate) * _IGSTPercent) / 100);
                            return _IGSTAmount;
                        }


                    }
                }
                return _IGSTAmount = 0;
            }
            set
            {
                if (_IGSTAmount != value)
                {

                    _IGSTAmount = value;
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("IGSTPercent");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private string _HSNCode;

        public string HSNCode
        {
            get { return _HSNCode; }
            set { _HSNCode = value; }
        }

        private int _POSGSTVatType;
        public int POSGSTVatType
        {
            get { return _POSGSTVatType; }
            set
            {
                if (_POSGSTVatType != value)
                {
                    _POSGSTVatType = value;
                }
            }
        }
        private int _POSGSTVatApplicationOn;
        public int POSGSTVatApplicationOn
        {
            get { return _POSGSTVatApplicationOn; }
            set
            {
                if (_POSGSTVatApplicationOn != value)
                {
                    _POSGSTVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


                }
            }
        }

        private int _POCGSTVatType;
        public int POCGSTVatType
        {
            get { return _POCGSTVatType; }
            set
            {
                if (_POCGSTVatType != value)
                {
                    _POCGSTVatType = value;
                }
            }
        }
        private int _POCGSTVatApplicationOn;
        public int POCGSTVatApplicationOn
        {
            get { return _POCGSTVatApplicationOn; }
            set
            {
                if (_POCGSTVatApplicationOn != value)
                {
                    _POCGSTVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


                }
            }
        }

        private int _POIGSTVatType;
        public int POIGSTVatType
        {
            get { return _POIGSTVatType; }
            set
            {
                if (_POIGSTVatType != value)
                {
                    _POIGSTVatType = value;
                }
            }
        }
        private int _POIGSTVatApplicationOn;
        public int POIGSTVatApplicationOn
        {
            get { return _POIGSTVatApplicationOn; }
            set
            {
                if (_POIGSTVatApplicationOn != value)
                {
                    _POIGSTVatApplicationOn = value;

                    OnPropertyChanged("VATAmount");  //***
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("ItemVATAmount");  //***
                    OnPropertyChanged("NetAmount");  //***


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
        private int _GRNSGSTVatApplicableOn;
        public int GRNSGSTVatApplicableOn
        {
            get { return _GRNSGSTVatApplicableOn; }
            set
            {
                if (_GRNSGSTVatApplicableOn != value)
                {
                    _GRNSGSTVatApplicableOn = value;
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
        private int _GRNCGSTVatApplicableOn;
        public int GRNCGSTVatApplicableOn
        {
            get { return _GRNCGSTVatApplicableOn; }
            set
            {
                if (_GRNCGSTVatApplicableOn != value)
                {
                    _GRNCGSTVatApplicableOn = value;

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
        private int _GRNIGSTVatApplicableOn;

        public int GRNIGSTVatApplicableOn
        {
            get { return _GRNIGSTVatApplicableOn; }
            set { _GRNIGSTVatApplicableOn = value; }
        }

        private decimal _TotalBatchAvailableStock;
        public decimal TotalBatchAvailableStock
        {
            get { return _TotalBatchAvailableStock; }
            set
            {
                if (_TotalBatchAvailableStock != value)
                {
                    _TotalBatchAvailableStock = value;
                    OnPropertyChanged("TotalBatchAvailableStock");
                }
            }
        }

        #region only for Last 3 PO Price //***//19

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
 #endregion

    }


    public partial class POIndent
    {

        private long _IndentId;

        public long IndentId
        {
            get { return _IndentId; }
            set { _IndentId = value; }
        }
        private long _IndentUnitId;
        private long _ItemId;

        public long ItemId
        {
            get { return _ItemId; }
            set { _ItemId = value; }
        }
        private long _POID;

        public long POID
        {
            get { return _POID; }
            set { _POID = value; }
        }
        private long _POUnitId;

        public long POUnitId
        {
            get { return _POUnitId; }
            set { _POUnitId = value; }
        }
        public long IndentUnitId { get; set; }
    }

    public partial class clsPurchaseOrderTerms
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
        private long _POID;

        public long POID
        {
            get
            {
                return _POID;
            }
            set
            {
                if (_POID != value)
                {
                    _POID = value;
                    OnPropertyChanged("POID");
                }
            }
        }
        private long _POUnitId;

        public long POUnitId
        {
            get
            {
                return _POUnitId;
            }
            set
            {
                if (_POUnitId != value)
                {
                    _POUnitId = value;
                    OnPropertyChanged("POUnitId");
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
        #endregion Properties
    }
}
