using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGRNVO : IValueObject, INotifyPropertyChanged
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

        public clsGRNVO()
        {
            _GRNType = InventoryGRNType.Direct;
            _PaymentModeID = MaterPayModeList.Cash;
            int gnt = Convert.ToInt32(InventoryGRNType.Direct);
        }

        private List<clsGRNDetailsVO> _Items = new List<clsGRNDetailsVO>();
        public List<clsGRNDetailsVO> Items
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

        // Use to maintain list of Deleted Main Items
        private List<clsGRNDetailsVO> _ItemsDeletedMain = new List<clsGRNDetailsVO>();
        public List<clsGRNDetailsVO> ItemsDeletedMain
        {
            get { return _ItemsDeletedMain; }
            set
            {
                if (_ItemsDeletedMain != value)
                {
                    _ItemsDeletedMain = value;
                    OnPropertyChanged("ItemsDeletedMain");
                }
            }
        }

        private List<clsGRNDetailsVO> _ItemsPOGRN = new List<clsGRNDetailsVO>();
        public List<clsGRNDetailsVO> ItemsPOGRN
        {
            get { return _ItemsPOGRN; }
            set
            {
                if (_ItemsPOGRN != value)
                {
                    _ItemsPOGRN = value;
                    OnPropertyChanged("ItemsPOGRN");
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


        private DateTime? _PODate;
        public DateTime? PODate
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

        private InventoryGRNType _GRNType;
        public InventoryGRNType GRNType
        {
            get { return _GRNType; }
            set
            {
                if (_GRNType != value)
                {
                    _GRNType = value;
                    OnPropertyChanged("GRNType");
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


        public string GRNTypeName
        {
            get { return _GRNType.ToString(); }

        }

        private string _InvoiceNo;
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set
            {
                if (_InvoiceNo != value)
                {
                    _InvoiceNo = value;
                    OnPropertyChanged("InvoiceNo");
                }
            }
        }

        private DateTime? _InvoiceDate;
        public DateTime? InvoiceDate
        {
            get { return _InvoiceDate; }
            set
            {
                if (_InvoiceDate != value)
                {
                    _InvoiceDate = value;
                    OnPropertyChanged("InvoiceDate");
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

        private string _PONowithDate;
        public string PONowithDate
        {
            get { return _PONowithDate; }
            set
            {
                if (_PONowithDate != value)
                {
                    _PONowithDate = value;
                    OnPropertyChanged("PONowithDate");
                }
            }
        }

        private string _DeliveryChallanNo;
        public string DeliveryChallanNo
        {
            get { return _DeliveryChallanNo; }
            set
            {
                if (_DeliveryChallanNo != value)
                {
                    _DeliveryChallanNo = value;
                    OnPropertyChanged("DeliveryChallanNo");
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

        private string _GatePassNo;
        public string GatePassNo
        {
            get { return _GatePassNo; }
            set
            {
                if (_GatePassNo != value)
                {
                    _GatePassNo = value;
                    OnPropertyChanged("GatePassNo");
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

        private long _ReceivedByID;
        public long ReceivedByID
        {
            get { return _ReceivedByID; }
            set
            {
                if (_ReceivedByID != value)
                {
                    _ReceivedByID = value;
                    OnPropertyChanged("ReceivedByID");
                }
            }
        }

        private double _TotalCDiscount;
        public double TotalCDiscount
        {
            get { return _TotalCDiscount; }
            set
            {
                if (_TotalCDiscount != value)
                {
                    _TotalCDiscount = value;
                    OnPropertyChanged("TotalCDiscount");
                }
            }
        }

        private double _TotalSchDiscount;
        public double TotalSchDiscount
        {
            get { return _TotalSchDiscount; }
            set
            {
                if (_TotalSchDiscount != value)
                {
                    _TotalSchDiscount = value;
                    OnPropertyChanged("TotalSchDiscount");
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

        private double _Other;
        public double Other
        {
            get { return _Other; }
            set
            {
                if (_Other != value)
                {
                    _Other = value;
                    OnPropertyChanged("Other");
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

        //Added By Somnath
        private double _TotalTaxAmount;
        public double TotalTAxAmount
        {
            get { return _TotalTaxAmount; }
            set
            {
                if (_TotalTaxAmount != value)
                {
                    _TotalTaxAmount = value;
                    OnPropertyChanged("TotalTAxAmount");
                }
            }
        }

        //End
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

        // Added  By CDS 

        private double _OtherCharges;
        public double OtherCharges
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

        private double _GRNDiscount;
        public double GRNDiscount
        {
            get { return _GRNDiscount; }
            set
            {
                if (_GRNDiscount != value)
                {
                    _GRNDiscount = value;
                    OnPropertyChanged("GRNDiscount");
                }
            }
        }

        private double _GRNRoundOff;
        public double GRNRoundOff
        {
            get { return _GRNRoundOff; }
            set
            {
                if (_GRNRoundOff != value)
                {
                    _GRNRoundOff = value;
                    OnPropertyChanged("GRNRoundOff");
                }
            }
        }

        private double _PrevNetAmount;
        public double PrevNetAmount
        {
            get { return _PrevNetAmount; }
            set
            {
                if (_PrevNetAmount != value)
                {
                    _PrevNetAmount = value;
                    OnPropertyChanged("PrevNetAmount");
                }
            }
        }

        // END

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

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (value != _FileName)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        private byte[] _File;
        public byte[] File
        {
            get { return _File; }
            set
            {
                if (value != _File)
                {
                    _File = value;
                    OnPropertyChanged("File");
                }
            }
        }

        private bool _IsFileAttached;
        public bool IsFileAttached
        {
            get { return _IsFileAttached; }
            set
            {
                _IsFileAttached = value;
                OnPropertyChanged("IsFileAttached");
            }
        }

        private string _FileAttached;
        public string FileAttached
        {
            get { return _FileAttached; }
            set
            {
                _FileAttached = value;
                OnPropertyChanged("FileAttached");
            }
        }
        private Boolean _IsConsignment;
        public Boolean IsConsignment
        {
            get { return _IsConsignment; }
            set
            {
                if (_IsConsignment != value)
                {
                    _IsConsignment = value;
                    OnPropertyChanged("IsConsignment");

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


        //Added By CDS
        private Boolean _Approve;
        public Boolean Approve
        {
            get { return _Approve; }
            set
            {
                if (_Approve != value)
                {
                    _Approve = value;
                    OnPropertyChanged("Approve");
                }
            }
        }

        private Boolean _Reject;
        public Boolean Reject
        {
            get { return _Reject; }
            set
            {
                if (_Reject != null)
                    _Reject = value;
                OnPropertyChanged("Reject");
            }
        }

        public bool EditForApprove { get; set; }  //Added By CDS for Save GRN In to  T_GRNHistory  And  T_GRNItemsHistory

        private Boolean _DirectApprove = false;
        public Boolean DirectApprove
        {
            get { return _DirectApprove; }
            set
            {
                if (_DirectApprove != value)
                {
                    _DirectApprove = value;
                    OnPropertyChanged("DirectApprove");
                }
            }
        }
        //End

        private string _ApprovedOrRejectedBy;
        public string ApprovedOrRejectedBy
        {
            get { return _ApprovedOrRejectedBy; }
            set
            {
                _ApprovedOrRejectedBy = value;
                OnPropertyChanged("ApprovedOrRejectedBy");
            }
        }

        //Added By Bhushanp 24062017 For GST
        private double _TotalSGST;
        public double TotalSGST
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
        private double _TotalCGST;
        public double TotalCGST
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
        private double _TotalIGST;
        public double TotalIGST
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

        #region For Free Items

        private double _TotalVATFree;
        public double TotalVATFree
        {
            get { return _TotalVATFree; }
            set
            {
                if (_TotalVATFree != value)
                {
                    _TotalVATFree = value;
                    OnPropertyChanged("TotalVATFree");
                }
            }
        }

        private double _TotalTaxAmountFree;
        public double TotalTAxAmountFree
        {
            get { return _TotalTaxAmountFree; }
            set
            {
                if (_TotalTaxAmountFree != value)
                {
                    _TotalTaxAmountFree = value;
                    OnPropertyChanged("TotalTAxAmountFree");
                }
            }
        }

        private List<clsGRNDetailsFreeVO> _ItemsFree = new List<clsGRNDetailsFreeVO>();
        public List<clsGRNDetailsFreeVO> ItemsFree
        {
            get { return _ItemsFree; }
            set
            {
                if (_ItemsFree != value)
                {
                    _ItemsFree = value;
                    OnPropertyChanged("ItemsFree");
                }
            }
        }

        // Use to maintain list of Deleted Free Items
        private List<clsGRNDetailsFreeVO> _ItemsDeletedFree = new List<clsGRNDetailsFreeVO>();
        public List<clsGRNDetailsFreeVO> ItemsDeletedFree
        {
            get { return _ItemsDeletedFree; }
            set
            {
                if (_ItemsDeletedFree != value)
                {
                    _ItemsDeletedFree = value;
                    OnPropertyChanged("ItemsDeletedFree");
                }
            }
        }
        #endregion
    }

    public class clsGRNDetailsVO : IValueObject, INotifyPropertyChanged
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

        private string _LinkServer = "";
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

        //Added by Ashish Z. for Add Item Link 
        private float _BaseRateBeforAddItem;
        public float BaseRateBeforAddItem
        {
            get { return _BaseRateBeforAddItem; }
            set
            {
                if (_BaseRateBeforAddItem != value)
                {
                    _BaseRateBeforAddItem = value;
                    OnPropertyChanged("BaseRateBeforAddItem");
                }
            }
        }

        //private string _POIDList; // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
        //public string POIDList
        //{
        //    get { return _POIDList; }
        //    set
        //    {
        //        if (_POIDList != value)
        //        {
        //            _POIDList = value;
        //            OnPropertyChanged("POIDList");
        //        }
        //    }
        //}

        //private string _PODetailsIDList; // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
        //public string PODetailsIDList
        //{
        //    get { return _PODetailsIDList; }
        //    set
        //    {
        //        if (_PODetailsIDList != value)
        //        {
        //            _PODetailsIDList = value;
        //            OnPropertyChanged("PODetailsIDList");
        //        }
        //    }
        //}


        private float _BaseMRPBeforAddItem;
        public float BaseMRPBeforAddItem
        {
            get
            {
                _BaseMRPBeforAddItem = (float)Math.Round((decimal)_BaseMRPBeforAddItem, 2);
                return _BaseMRPBeforAddItem;
            }
            set
            {
                if (value != _BaseMRPBeforAddItem)
                {
                    _BaseMRPBeforAddItem = value;
                    OnPropertyChanged("BaseMRPBeforAddItem");
                }
            }
        }
        //End

        private DateTime? _GRNDate;
        public DateTime? GRNDate
        {
            get { return _GRNDate; }
            set
            {
                if (_GRNDate != value)
                {
                    _GRNDate = value;
                    OnPropertyChanged("GRNDate");
                }
            }
        }

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

        public bool SelectItem { get; set; }
        private bool _IsReadOnly;
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }
        // Added by Rohit
        private double _POQuantity;
        public double POQuantity
        {
            get { return _POQuantity; }
            set
            {
                if (_POQuantity != value)
                {
                    _POQuantity = value;
                    OnPropertyChanged("POQuantity");
                }
            }
        }
        private Boolean _IsConsignment;
        public Boolean IsConsignment
        {
            get { return _IsConsignment; }
            set
            {
                if (_IsConsignment != value)
                {
                    _IsConsignment = value;
                    OnPropertyChanged("IsConsignment");

                }
            }
        }

        private long _RowId;
        public long RowID
        {
            get { return _RowId; }
            set
            {
                if (_RowId != value)
                {
                    _RowId = value;
                    OnPropertyChanged("RowID");
                }
            }
        }
        private double _PendingQuantity;
        public double PendingQuantity
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

        // For Getting Values The Defined For Conversion Factor

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
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");


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

        private string _POTransUOM;
        public string POTransUOM
        {
            get { return _POTransUOM; }
            set
            {
                if (_POTransUOM != value)
                {
                    _POTransUOM = value;
                    OnPropertyChanged("POTransUOM");
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");

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
        // END


        private double _ItemQuantity;
        public double ItemQuantity
        {
            get { return _ItemQuantity; }
            set
            {
                if (_ItemQuantity != value)
                {
                    _ItemQuantity = value;
                    OnPropertyChanged("ItemQuantity");
                }
            }
        }
        private string _BarCode;
        public string BarCode
        {
            get
            {
                _BarCode = _BatchID == 0 ? string.Empty : Convert.ToString(_BatchID);
                int BarcodeLenght = _BarCode.Length;
                int TotalWidth = 5;
                if (BarcodeLenght > 0)
                {
                    _BarCode = _BarCode.PadLeft(TotalWidth, '0');
                }
                return _BarCode;
            }
            set
            {
                if (_BarCode != value)
                {
                    _BarCode = value;
                    OnPropertyChanged("BarCode");
                }
            }
        }
        private double _TotalItemQuantity;
        public double TotalItemQuantity
        {
            get { return (_Quantity + _FreeQuantity); }

        }

        private long _ItemCategory;
        public long ItemCategory
        {
            get { return _ItemCategory; }
            set
            {
                if (_ItemCategory != value)
                {
                    _ItemCategory = value;
                    OnPropertyChanged("ItemCategory");
                }
            }
        }
        private long _ItemGroup;
        public long ItemGroup
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

        // End
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

        public bool AssignSupplier { get; set; }

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
                    OnPropertyChanged("BarCode");
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

        private double _BaseAvailableQuantity;
        public double BaseAvailableQuantity
        {
            get { return _BaseAvailableQuantity; }
            set
            {
                if (_BaseAvailableQuantity != value)
                {
                    _BaseAvailableQuantity = value;
                    OnPropertyChanged("BaseAvailableQuantity");
                }
            }
        }

        private string _AvailableStockUOM;
        public string AvailableStockUOM
        {
            get { return _AvailableStockUOM; }
            set
            {
                if (_AvailableStockUOM != value)
                {
                    _AvailableStockUOM = value;
                    OnPropertyChanged("AvailableStockUOM");
                }
            }
        }

        //OLD
        //private double _Quantity;
        //public double Quantity
        //{
        //    get { return _Quantity; }
        //    set
        //    {
        //        if (_Quantity != value)
        //        {
        //            if (value < 0)
        //                value = 1;

        //            _Quantity = value;
        //            OnPropertyChanged("Quantity");
        //            OnPropertyChanged("TotalItemQuantity");
        //            OnPropertyChanged("TotalQuantity");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("SchDiscountAmount");
        //            #region Added By Pallavi
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("CDiscountAmount");
        //            OnPropertyChanged("SchDiscountAmount");
        //            OnPropertyChanged("TaxAmount");
        //            #endregion
        //            ////
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    if (value < 0)
                        value = 1;

                    _Quantity = value;

                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("TotalItemQuantity");
                    OnPropertyChanged("TotalQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("SchDiscountAmount");
                    #region Added By Pallavi
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    #endregion
                    ////
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");

                }
            }
        }


        private double _POPendingQuantity;
        public double POPendingQuantity
        {
            get { return _POPendingQuantity; }
            set
            {
                if (_POPendingQuantity != value)
                {

                    _POPendingQuantity = value;
                    OnPropertyChanged("POPendingQuantity");

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


        private double _FreeQuantity;
        public double FreeQuantity
        {
            get { return _FreeQuantity; }
            set
            {
                if (_FreeQuantity != value)
                {
                    if (value < 0)
                        value = 0;

                    _FreeQuantity = value;
                    OnPropertyChanged("FreeQuantity");
                    OnPropertyChanged("TotalQuantity");
                }
            }
        }

        public double TotalQuantity
        {
            get { return (_Quantity + _FreeQuantity) * _ConversionFactor; }

        }
        private double _GRNReturnTotalQuantity;
        public double GRNReturnTotalQuantity
        {
            get { return (_Quantity + _FreeQuantity) * _ConversionFactor; }
            set
            {
                _GRNReturnTotalQuantity = value;
            }


        }

        // OLD 
        //private double _Rate;
        //public double Rate
        //{
        //    get { return _Rate; }
        //    set
        //    {
        //        if (_Rate != value)
        //        {
        //            _Rate = value;
        //            OnPropertyChanged("Rate");
        //            OnPropertyChanged("Amount");
        //            OnPropertyChanged("CDiscountAmount");
        //            OnPropertyChanged("SchDiscountAmount");
        //            OnPropertyChanged("VATAmount");

        //            OnPropertyChanged("TaxAmount");
        //            OnPropertyChanged("NetAmount");

        //        }
        //    }
        //}

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
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");

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



        //OLD
        //private double _Amount;
        //public double Amount
        //{
        //    get { return _Amount = _Rate * _Quantity; }
        //    set
        //    {
        //        if (_Amount != value)
        //        {
        //            _Amount = value;
        //            OnPropertyChanged("Amount");
        //            #region Added by Pallavi
        //            OnPropertyChanged("CDiscountAmount");
        //            OnPropertyChanged("SchDiscountAmount");
        //            OnPropertyChanged("VATAmount");
        //            OnPropertyChanged("TaxAmount");
        //            #endregion

        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        private double _Amount;
        public double Amount
        {
            get
            {
                //if (_GRNItemVatApplicationOn == 1)
                //{
                //    return _Amount = _Rate * _Quantity;
                //}
                //else
                //{
                return _Amount = _MRP * _Quantity;
                //}
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    #endregion
                    OnPropertyChanged("NetAmount");

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
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
                    //if (value < 0)
                    //    value = 0;

                    _VATPercent = value;
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("NetAmount");

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");


                }
            }
        }



        private double _CostRate;
        public double CostRate
        {
            get
            {
                return _CostRate = _Rate * _Quantity;
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

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
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

        //OLD
        //private double _TaxAmount;
        //public double TaxAmount
        //{
        //    get
        //    {
        //        //if (_VATPercent != 0)
        //        //{
        //        //    return _VATAmount = ((_Amount * _VATPercent) / 100);
        //        //}
        //        _TaxAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
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

        private double _TaxAmount;
        public double TaxAmount
        {
            get
            {
                if (_Quantity > 0)
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

                    OnPropertyChanged("SchDiscountPercent"); //new on 13082018
                    OnPropertyChanged("SchDiscountAmount");  //new on 13082018


                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");

                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
                }
            }
        }

        private double _CDiscountAmount;
        public double CDiscountAmount
        {
            get
            {
                //if (_CDiscountPercent != 0)
                //{
                //    return _CDiscountAmount = ((_Amount * _CDiscountPercent) / 100);
                //}
                //else
                //return _CDiscountAmount; 
                //return _CDiscountAmount = ((_Amount * _CDiscountPercent) / 100);

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
                    OnPropertyChanged("SchDiscountPercent"); //new on 13082018
                    OnPropertyChanged("SchDiscountAmount");  //new on 13082018
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");

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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
                }
            }
        }

        private double _SchDiscountAmount;
        public double SchDiscountAmount
        {
            get
            {
                //return _SchDiscountAmount = ((_CostRate * _SchDiscountPercent) / 100);   previous code

                if (_CDiscountPercent > 0)
                {
                    _SchDiscountAmount = ((_CostRate - _CDiscountAmount) * _SchDiscountPercent) / 100;
                }
                else
                {
                    _SchDiscountAmount = ((_CostRate * _SchDiscountPercent) / 100);
                }
                return _SchDiscountAmount;
            }
            set
            {
                if (_SchDiscountAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _SchDiscountAmount = value;
                    //_SchDiscountPercent = 0;
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("SchDiscountPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
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

        //private double _MRP;
        //public double MRP
        //{
        //    get { return _MRP; }
        //    set
        //    {
        //        if (_MRP != value)
        //        {
        //            if (value < 0)
        //                value = 0;

        //            _MRP = value;
        //            OnPropertyChanged("MRP");
        //        }
        //    }
        //}

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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");

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

                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
                }
            }
        }

        //private double _NetAmount;
        //public double NetAmount
        //{
        //    get
        //    {
        //        // _NetAmount = _Amount + _VATAmount - _SchDiscountAmount - _CDiscountAmount;
        //        #region Added By Pallavi
        //        //double itemTaxAmount = ((_Amount * ItemTax) / 100);
        //        _NetAmount = _Amount - _SchDiscountAmount - _CDiscountAmount + _VATAmount + TaxAmount;
        //        #endregion

        //        return _NetAmount;
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


        private double _NetAmount;
        public double NetAmount
        {
            get
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
                        #region Added By Pallavi
                        _NetAmount = (_CostRate - _SchDiscountAmount - _CDiscountAmount) + _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        #endregion
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
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                    OnPropertyChanged("AvgCost");
                    OnPropertyChanged("AvgCostAmount");
                }
            }
        }

        // Added  By CDS 

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

        private float _GRNDetailsViewTimeQty;  // to check pending quantity validation at the time of GRN Item Qyantity view & Edit.
        public float GRNDetailsViewTimeQty
        {
            get
            {
                return _GRNDetailsViewTimeQty;
            }
            set
            {
                if (_GRNDetailsViewTimeQty != value)
                {
                    _GRNDetailsViewTimeQty = value;
                    OnPropertyChanged("GRNDetailsViewTimeQty");
                }
            }
        }

        private double _OtherCharges;
        public double OtherCharges
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

        private double _GRNDiscount;
        public double GRNDiscount
        {
            get { return _GRNDiscount; }
            set
            {
                if (_GRNDiscount != value)
                {
                    _GRNDiscount = value;
                    OnPropertyChanged("GRNDiscount");
                }
            }
        }

        private double _GRNRoundOff;
        public double GRNRoundOff
        {
            get { return _GRNRoundOff; }
            set
            {
                if (_GRNRoundOff != value)
                {
                    _GRNRoundOff = value;
                    OnPropertyChanged("GRNRoundOff");
                }
            }
        }

        private double _PrevNetAmount;
        public double PrevNetAmount
        {
            get { return _PrevNetAmount; }
            set
            {
                if (_PrevNetAmount != value)
                {
                    _PrevNetAmount = value;
                    OnPropertyChanged("PrevNetAmount");
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

        //Commented on dated 22042017
        //private double _AbatedMRP;
        //public double AbatedMRP
        //{
        //    get
        //    {
        //        if (_GRNItemVatType == 2)      // Exclusive 
        //        {
        //            return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
        //        }
        //        else if (_GRNItemVatType == 1)  // Inclusive 
        //        {
        //            return _AbatedMRP = ((_MRP) / (_VATPercent + 100)) * 100;
        //        }
        //        else
        //        {
        //            return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
        //        }
        //    }
        //    set
        //    {
        //        if (_AbatedMRP != value)
        //        {
        //            _AbatedMRP = value;
        //            OnPropertyChanged("AbatedMRP");
        //        }
        //    }
        //}
        // Endded

        private double _AbatedMRP;
        public double AbatedMRP
        {
            get
            {
                ////if (_GRNItemVatType == 2)      // Exclusive 
                ////{
                ////    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
                ////}
                ////else if (_GRNItemVatType == 1)  // Inclusive 
                ////{
                //    return _AbatedMRP = ((_MRP) / (_VATPercent + 100)) * 100;  //set by default Inclusive of Abated MRP on dated 22042017.
                ////}
                ////else
                ////{
                ////    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
                ////}

                return _AbatedMRP = ((_MRP) / (_VATPercent + _CGSTPercent + _SGSTPercent + _IGSTPercent + 100)) * 100;  //set by default Inclusive of Abated MRP on dated 22042017.
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

        // END


        private Boolean _IsBatchAssign = false;
        public Boolean IsBatchAssign
        {
            get { return _IsBatchAssign; }
            set
            {
                if (_IsBatchAssign != value)
                {
                    _IsBatchAssign = value;
                    OnPropertyChanged("IsBatchAssign");

                }
            }
        }

        private double _ConversionFactor = 1;
        public double ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value && value > 0)
                {
                    _ConversionFactor = value;

                    OnPropertyChanged("ConversionFactor");
                    OnPropertyChanged("TotalItemQuantity");
                    OnPropertyChanged("TotalQuantity");
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

        private string _ItemCode;
        public string ItemCode
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

        private bool _ResultStatus;
        public bool ResultStatus
        {
            get { return _ResultStatus; }
            set
            {
                if (_ResultStatus != value)
                {
                    _ResultStatus = value;
                    OnPropertyChanged("ResultStatus");
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

        private long _GRNDetailID;
        public long GRNDetailID
        {
            get { return _GRNDetailID; }
            set
            {
                if (_GRNDetailID != value)
                {
                    _GRNDetailID = value;
                    OnPropertyChanged("GRNDetailID");
                }
            }
        }

        private long _GRNDetailUnitID;
        public long GRNDetailUnitID
        {
            get { return _GRNDetailUnitID; }
            set
            {
                if (_GRNDetailUnitID != value)
                {
                    _GRNDetailUnitID = value;
                    OnPropertyChanged("GRNDetailUnitID");
                }
            }
        }


        private long _PODetailID;
        public long PODetailID
        {
            get { return _PODetailID; }
            set
            {
                if (_PODetailID != value)
                {
                    _PODetailID = value;
                    OnPropertyChanged("PODetailID");
                }
            }
        }

        private long _PODetailUnitID;
        public long PODetailUnitID
        {
            get { return _PODetailUnitID; }
            set
            {
                if (_PODetailUnitID != value)
                {
                    _PODetailUnitID = value;
                    OnPropertyChanged("PODetailUnitID");
                }
            }
        }

        private long _ReceivedID;
        public long ReceivedID
        {
            get { return _ReceivedID; }
            set
            {
                if (_ReceivedID != value)
                {
                    _ReceivedID = value;
                    OnPropertyChanged("ReceivedID");
                }
            }
        }

        private long _ReceivedUnitID;
        public long ReceivedUnitID
        {
            get { return _ReceivedUnitID; }
            set
            {
                if (_ReceivedUnitID != value)
                {
                    _ReceivedUnitID = value;
                    OnPropertyChanged("ReceivedUnitID");
                }
            }
        }

        public long ReceivedDetailID { get; set; }

        #region For Free Items

        private long _SrNo;  // Set & use to link Main Item with Free Item
        public long SrNo
        {
            get { return _SrNo; }
            set
            {
                if (_SrNo != value)
                {
                    _SrNo = value;
                    OnPropertyChanged("SrNo");
                }
            }
        }

        #endregion

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

        private double _FPNetAmount;    // use for Front Pannel List only..
        public double FPNetAmount
        {
            get
            {
                return _FPNetAmount;
            }
            set
            {
                if (_FPNetAmount != value)
                {
                    _FPNetAmount = value;
                    OnPropertyChanged("FPNetAmount");
                }
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

        private Boolean _IsFromAddItem = false;
        public Boolean IsFromAddItem
        {
            get { return _IsFromAddItem; }
            set
            {
                if (_IsFromAddItem != value)
                {
                    _IsFromAddItem = value;
                    OnPropertyChanged("IsFromAddItem");

                }
            }
        }

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

                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");
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

                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");
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

                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");
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

        #region Average Cost Calculations
        private double _FreeItemQuantity;
        public double FreeItemQuantity
        {
            get
            {
                return _FreeItemQuantity;
            }
            set
            {
                if (_FreeItemQuantity != value)
                {
                    _FreeItemQuantity = value;

                    OnPropertyChanged("FreeItemQuantity");
                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");
                }
            }
        }

        private double _FreeGSTAmount;
        public double FreeGSTAmount
        {
            get
            {
                return _FreeGSTAmount;
            }
            set
            {
                if (_FreeGSTAmount != value)
                {
                    _FreeGSTAmount = value;

                    OnPropertyChanged("FreeGSTAmount");
                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");
                }
            }
        }

        private double _AvgCost;
        public double AvgCost
        {
            get
            {
                //_AvgCost = (_CostRate + (_CGSTAmount + _SGSTAmount + _IGSTAmount) + _FreeGSTAmount) / (_Quantity + _FreeItemQuantity); 
                _AvgCost = _NetAmount / (_Quantity + _FreeItemQuantity);
                return _AvgCost;
            }
            set
            {
                if (_AvgCost != value)
                {
                    _AvgCost = value;

                    OnPropertyChanged("AvgCost");
                }
            }
        }

        private double _AvgCostAmount;
        public double AvgCostAmount
        {
            get
            {
                //_AvgCostAmount = _CostRate + (_CGSTAmount + _SGSTAmount + _IGSTAmount) + _FreeGSTAmount;
                _AvgCostAmount = (_NetAmount / (_Quantity + _FreeItemQuantity)) * (_Quantity + _FreeItemQuantity);

                return _AvgCostAmount;
            }
            set
            {
                if (_AvgCostAmount != value)
                {
                    _AvgCostAmount = value;

                    OnPropertyChanged("AvgCostAmount");
                    OnPropertyChanged("AvgCost");
                }
            }
        }

        public double AvgCostForFront { get; set; }
        public double AvgCostAmountForFront { get; set; }
        #endregion
    }

    public class clsGRNDetailsFreeVO : IValueObject, INotifyPropertyChanged
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

        private string _LinkServer = "";
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

        public bool SelectItem { get; set; }
        private bool _IsReadOnly;
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                if (_IsReadOnly != value)
                {
                    _IsReadOnly = value;
                    OnPropertyChanged("IsReadOnly");
                }
            }
        }
        // Added by Rohit
        private double _POQuantity;
        public double POQuantity
        {
            get { return _POQuantity; }
            set
            {
                if (_POQuantity != value)
                {
                    _POQuantity = value;
                    OnPropertyChanged("POQuantity");
                }
            }
        }

        //private Boolean _IsConsignment;
        //public Boolean IsConsignment
        //{
        //    get { return _IsConsignment; }
        //    set
        //    {
        //        if (_IsConsignment != value)
        //        {
        //            _IsConsignment = value;
        //            OnPropertyChanged("IsConsignment");

        //        }
        //    }
        //}

        private long _RowId;
        public long RowID
        {
            get { return _RowId; }
            set
            {
                if (_RowId != value)
                {
                    _RowId = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private double _PendingQuantity;
        public double PendingQuantity
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

        // For Getting Values The Defined For Conversion Factor

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



        // For Conversion Factor

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

        private string _POTransUOM;
        public string POTransUOM
        {
            get { return _POTransUOM; }
            set
            {
                if (_POTransUOM != value)
                {
                    _POTransUOM = value;
                    OnPropertyChanged("POTransUOM");
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
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
        // END


        private double _ItemQuantity;
        public double ItemQuantity
        {
            get { return _ItemQuantity; }
            set
            {
                if (_ItemQuantity != value)
                {
                    _ItemQuantity = value;
                    OnPropertyChanged("ItemQuantity");
                }
            }
        }
        private string _BarCode;
        public string BarCode
        {
            get
            {
                _BarCode = _FreeBatchID == 0 ? string.Empty : Convert.ToString(_FreeBatchID);
                int BarcodeLenght = _BarCode.Length;
                int TotalWidth = 5;
                if (BarcodeLenght > 0)
                {
                    _BarCode = _BarCode.PadLeft(TotalWidth, '0');
                }
                return _BarCode;
            }
            set
            {
                if (_BarCode != value)
                {
                    _BarCode = value;
                    OnPropertyChanged("BarCode");
                }
            }
        }
        private double _TotalItemQuantity;
        public double TotalItemQuantity
        {
            get { return (_Quantity + _FreeQuantity); }

        }

        private long _ItemCategory;
        public long ItemCategory
        {
            get { return _ItemCategory; }
            set
            {
                if (_ItemCategory != value)
                {
                    _ItemCategory = value;
                    OnPropertyChanged("ItemCategory");
                }
            }
        }
        private long _ItemGroup;
        public long ItemGroup
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

        // End
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

        public bool AssignSupplier { get; set; }

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

        private long _MainItemID;
        public long MainItemID
        {
            get { return _MainItemID; }
            set
            {
                if (_MainItemID != value)
                {
                    _MainItemID = value;
                    OnPropertyChanged("MainItemID");
                }
            }
        }


        private long _FreeItemID;
        public long FreeItemID
        {
            get { return _FreeItemID; }
            set
            {
                if (_FreeItemID != value)
                {
                    _FreeItemID = value;
                    OnPropertyChanged("FreeItemID");
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

        private string _MainItemName;
        public string MainItemName
        {
            get { return _MainItemName; }
            set
            {
                if (_MainItemName != value)
                {
                    _MainItemName = value;
                    OnPropertyChanged("MainItemName");
                }
            }
        }

        private string _FreeItemName;
        public string FreeItemName
        {
            get { return _FreeItemName; }
            set
            {
                if (_FreeItemName != value)
                {
                    _FreeItemName = value;
                    OnPropertyChanged("FreeItemName");
                }
            }
        }

        private long _MainBatchID;
        public long MainBatchID
        {
            get { return _MainBatchID; }
            set
            {
                if (_MainBatchID != value)
                {
                    _MainBatchID = value;
                    OnPropertyChanged("MainBatchID");
                }
            }
        }

        private long _FreeBatchID;
        public long FreeBatchID
        {
            get { return _FreeBatchID; }
            set
            {
                if (_FreeBatchID != value)
                {
                    _FreeBatchID = value;
                    OnPropertyChanged("FreeBatchID");
                    OnPropertyChanged("BarCode");
                }
            }
        }

        private string _MainBatchCode;
        public string MainBatchCode
        {
            get { return _MainBatchCode; }
            set
            {
                if (_MainBatchCode != value)
                {
                    _MainBatchCode = value;
                    OnPropertyChanged("MainBatchCode");
                }
            }
        }

        private string _FreeBatchCode;
        public string FreeBatchCode
        {
            get { return _FreeBatchCode; }
            set
            {
                if (_FreeBatchCode != value)
                {
                    _FreeBatchCode = value;
                    OnPropertyChanged("FreeBatchCode");
                }
            }
        }

        private DateTime? _MainExpiryDate;
        public DateTime? MainExpiryDate
        {
            get { return _MainExpiryDate; }
            set
            {
                if (_MainExpiryDate != value)
                {
                    _MainExpiryDate = value;
                    OnPropertyChanged("MainExpiryDate");
                }
            }
        }

        private DateTime? _FreeExpiryDate;
        public DateTime? FreeExpiryDate
        {
            get { return _FreeExpiryDate; }
            set
            {
                if (_FreeExpiryDate != value)
                {
                    _FreeExpiryDate = value;
                    OnPropertyChanged("FreeExpiryDate");
                }
            }
        }

        private float _MainItemMRP;
        public float MainItemMRP
        {
            get
            {
                _MainItemMRP = (float)Math.Round(_MainItemMRP, 2);
                return _MainItemMRP;
            }
            set
            {
                if (value != _MainItemMRP)
                {
                    _MainItemMRP = value;
                    OnPropertyChanged("MainItemMRP");
                }
            }
        }

        private double _MainItemCostRate;
        public double MainItemCostRate
        {
            get
            {
                return _MainItemCostRate;
            }
            set
            {
                if (_MainItemCostRate != value)
                {
                    _MainItemCostRate = value;
                    OnPropertyChanged("MainItemCostRate");
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

        private double _BaseAvailableQuantity;
        public double BaseAvailableQuantity
        {
            get { return _BaseAvailableQuantity; }
            set
            {
                if (_BaseAvailableQuantity != value)
                {
                    _BaseAvailableQuantity = value;
                    OnPropertyChanged("BaseAvailableQuantity");
                }
            }
        }

        private string _AvailableStockUOM;
        public string AvailableStockUOM
        {
            get { return _AvailableStockUOM; }
            set
            {
                if (_AvailableStockUOM != value)
                {
                    _AvailableStockUOM = value;
                    OnPropertyChanged("AvailableStockUOM");
                }
            }
        }

        private double _Quantity;
        public double Quantity       //use for Free Quantity Cloumn
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    if (value < 0)
                        value = 1;

                    _Quantity = value;

                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("CostRate");
                    OnPropertyChanged("TotalItemQuantity");
                    OnPropertyChanged("TotalQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("SchDiscountAmount");
                    #region Added By Pallavi
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("TotalAmount");
                    #endregion
                    ////
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _POPendingQuantity;
        public double POPendingQuantity
        {
            get { return _POPendingQuantity; }
            set
            {
                if (_POPendingQuantity != value)
                {

                    _POPendingQuantity = value;
                    OnPropertyChanged("POPendingQuantity");

                }
            }
        }

        //private long _PoItemsID;
        //public long PoItemsID
        //{
        //    get { return _PoItemsID; }
        //    set
        //    {
        //        if (_PoItemsID != value)
        //        {
        //            _PoItemsID = value;
        //            OnPropertyChanged("PoItemsID");
        //        }
        //    }
        //}


        private double _FreeQuantity;
        public double FreeQuantity
        {
            get { return _FreeQuantity; }
            set
            {
                if (_FreeQuantity != value)
                {
                    if (value < 0)
                        value = 0;

                    _FreeQuantity = value;
                    OnPropertyChanged("FreeQuantity");
                    OnPropertyChanged("TotalQuantity");
                }
            }
        }

        public double TotalQuantity
        {
            get { return (_Quantity + _FreeQuantity) * _ConversionFactor; }

        }
        private double _GRNReturnTotalQuantity;
        public double GRNReturnTotalQuantity
        {
            get { return (_Quantity + _FreeQuantity) * _ConversionFactor; }
            set
            {
                _GRNReturnTotalQuantity = value;
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
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
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

        private double _Amount;
        public double Amount
        {
            get
            {
                //if (_GRNItemVatApplicationOn == 1)
                //{
                //    return _Amount = _Rate * _Quantity;
                //}
                //else
                //{
                return _Amount = _MRP * _Quantity;
                //}
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    #endregion
                    OnPropertyChanged("NetAmount");
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
                    //if (value < 0)
                    //    value = 0;

                    _VATPercent = value;
                    OnPropertyChanged("VATPercent");
                    OnPropertyChanged("AbatedMRP");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");


                }
            }
        }

        private double _CostRate;
        public double CostRate
        {
            get
            {
                return _CostRate = _Rate * _Quantity;
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


        private double _TaxAmount;
        public double TaxAmount
        {
            get
            {
                if (_Quantity > 0)
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");

                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _CDiscountAmount;
        public double CDiscountAmount
        {
            get
            {
                //if (_CDiscountPercent != 0)
                //{
                //    return _CDiscountAmount = ((_Amount * _CDiscountPercent) / 100);
                //}
                //else
                //return _CDiscountAmount; 
                //return _CDiscountAmount = ((_Amount * _CDiscountPercent) / 100);

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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _SchDiscountAmount;
        public double SchDiscountAmount
        {
            get
            {
                //if (_SchDiscountPercent != 0)
                //{
                //    return _SchDiscountAmount = ((_Amount * _SchDiscountPercent) / 100);
                //}
                //else
                //return _SchDiscountAmount; 
                //return _SchDiscountAmount = ((_Amount * _SchDiscountPercent) / 100);

                return _SchDiscountAmount = ((_CostRate * _SchDiscountPercent) / 100);

            }
            set
            {
                if (_SchDiscountAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _SchDiscountAmount = value;
                    //_SchDiscountPercent = 0;
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("SchDiscountPercent");
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
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

        //private double _MRP;
        //public double MRP
        //{
        //    get { return _MRP; }
        //    set
        //    {
        //        if (_MRP != value)
        //        {
        //            if (value < 0)
        //                value = 0;

        //            _MRP = value;
        //            OnPropertyChanged("MRP");
        //        }
        //    }
        //}

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
                    OnPropertyChanged("SGSTAmount");
                    OnPropertyChanged("CGSTAmount");
                    OnPropertyChanged("IGSTAmount");
                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

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

        private double _NetAmount;
        public double NetAmount
        {
            get
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
                            return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount; // 0;  // _CostRate - _SchDiscountAmount - _CDiscountAmount;
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
                        return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                    }
                    else
                    {
                        if ((_GRNSGSTVatApplicationOn == 1 && _GRNCGSTVatApplicationOn == 1) && _GRNIGSTVatApplicationOn == 1) //for Applicable on CP
                        {
                            return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        }
                        else  //for Applicable on MRP
                        {
                            return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
                        }
                    }
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
                            return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;    //0;  // _CostRate - _SchDiscountAmount - _CDiscountAmount;
                        }
                        else  //for Applicable on MRP
                        {
                            return _NetAmount = _VATAmount + _TaxAmount + _SGSTAmount + _CGSTAmount + _IGSTAmount;
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

        private float _GRNDetailsViewTimeQty;  // to check pending quantity validation at the time of GRN Item Qyantity view & Edit.
        public float GRNDetailsViewTimeQty
        {
            get
            {
                return _GRNDetailsViewTimeQty;
            }
            set
            {
                if (_GRNDetailsViewTimeQty != value)
                {
                    _GRNDetailsViewTimeQty = value;
                    OnPropertyChanged("GRNDetailsViewTimeQty");
                }
            }
        }

        private double _OtherCharges;
        public double OtherCharges
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

        private double _GRNDiscount;
        public double GRNDiscount
        {
            get { return _GRNDiscount; }
            set
            {
                if (_GRNDiscount != value)
                {
                    _GRNDiscount = value;
                    OnPropertyChanged("GRNDiscount");
                }
            }
        }

        private double _GRNRoundOff;
        public double GRNRoundOff
        {
            get { return _GRNRoundOff; }
            set
            {
                if (_GRNRoundOff != value)
                {
                    _GRNRoundOff = value;
                    OnPropertyChanged("GRNRoundOff");
                }
            }
        }

        private double _PrevNetAmount;
        public double PrevNetAmount
        {
            get { return _PrevNetAmount; }
            set
            {
                if (_PrevNetAmount != value)
                {
                    _PrevNetAmount = value;
                    OnPropertyChanged("PrevNetAmount");
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

        private double _AbatedMRP;
        public double AbatedMRP
        {
            get
            {
                //if (_GRNItemVatType == 2)      // Exclusive 
                //{
                //    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
                //}
                //else if (_GRNItemVatType == 1)  // Inclusive 
                //{
                //    return _AbatedMRP = ((_MRP) / (_VATPercent + 100)) * 100;
                //}
                //else
                //{
                //    return _AbatedMRP = (_MRP) - ((_MRP * _VATPercent) / 100);
                //}
                return _AbatedMRP = ((_MRP) / (_VATPercent + _CGSTPercent + _SGSTPercent + _IGSTPercent + 100)) * 100;
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

        // END


        private Boolean _IsBatchAssign = false;
        public Boolean IsBatchAssign
        {
            get { return _IsBatchAssign; }
            set
            {
                if (_IsBatchAssign != value)
                {
                    _IsBatchAssign = value;
                    OnPropertyChanged("IsBatchAssign");

                }
            }
        }

        private double _ConversionFactor = 1;
        public double ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value && value > 0)
                {
                    _ConversionFactor = value;

                    OnPropertyChanged("ConversionFactor");
                    OnPropertyChanged("TotalItemQuantity");
                    OnPropertyChanged("TotalQuantity");
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

        private string _MainItemCode;
        public string MainItemCode
        {
            get
            {
                return _MainItemCode;
            }
            set
            {
                if (value != _MainItemCode)
                {
                    _MainItemCode = value;
                    OnPropertyChanged("MainItemCode");
                }
            }
        }

        private string _FreeItemCode;
        public string FreeItemCode
        {
            get
            {
                return _FreeItemCode;
            }
            set
            {
                if (value != _FreeItemCode)
                {
                    _FreeItemCode = value;
                    OnPropertyChanged("FreeItemCode");
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

        private long _GRNDetailID;
        public long GRNDetailID
        {
            get { return _GRNDetailID; }
            set
            {
                if (_GRNDetailID != value)
                {
                    _GRNDetailID = value;
                    OnPropertyChanged("GRNDetailID");
                }
            }
        }

        private long _GRNDetailUnitID;
        public long GRNDetailUnitID
        {
            get { return _GRNDetailUnitID; }
            set
            {
                if (_GRNDetailUnitID != value)
                {
                    _GRNDetailUnitID = value;
                    OnPropertyChanged("GRNDetailUnitID");
                }
            }
        }


        private long _FreeGRNDetailID;
        public long FreeGRNDetailID
        {
            get { return _FreeGRNDetailID; }
            set
            {
                if (_FreeGRNDetailID != value)
                {
                    _FreeGRNDetailID = value;
                    OnPropertyChanged("FreeGRNDetailID");
                }
            }
        }

        private long _PODetailID;
        public long PODetailID
        {
            get { return _PODetailID; }
            set
            {
                if (_PODetailID != value)
                {
                    _PODetailID = value;
                    OnPropertyChanged("PODetailID");
                }
            }
        }

        private long _PODetailUnitID;
        public long PODetailUnitID
        {
            get { return _PODetailUnitID; }
            set
            {
                if (_PODetailUnitID != value)
                {
                    _PODetailUnitID = value;
                    OnPropertyChanged("PODetailUnitID");
                }
            }
        }

        clsFreeMainItems _SelectedMainItem = new clsFreeMainItems();
        public clsFreeMainItems SelectedMainItem
        {
            get
            {
                return _SelectedMainItem;
            }
            set
            {
                if (value != _SelectedMainItem)
                {
                    _SelectedMainItem = value;
                    OnPropertyChanged("SelectedMainItem");
                }
            }


        }

        List<clsFreeMainItems> _MainList = new List<clsFreeMainItems>();
        public List<clsFreeMainItems> MainList
        {
            get
            {
                return _MainList;
            }
            set
            {
                if (value != _MainList)
                {
                    _MainList = value;

                }
            }

        }

        private long _MainSrNo;  // Use to link Main Item with Free Item
        public long MainSrNo
        {
            get { return _MainSrNo; }
            set
            {
                if (_MainSrNo != value)
                {
                    _MainSrNo = value;
                    OnPropertyChanged("MainSrNo");
                }
            }
        }


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
                        _SGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _SGSTPercent) / 100);
                        //_SGSTAmount = (((_CostRate) * _SGSTPercent) / 100);
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
                            _IGSTAmount = (((_CostRate - _CDiscountAmount - _SchDiscountAmount) * _IGSTPercent) / 100);
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

        private double _FPNetAmount;    // use for Front Pannel List only..
        public double FPNetAmount
        {
            get
            {
                return _FPNetAmount;
            }
            set
            {
                if (_FPNetAmount != value)
                {
                    _FPNetAmount = value;
                    OnPropertyChanged("FPNetAmount");
                }
            }
        }

    }

    public class clsFreeMainItems
    {
        public string ToXml()
        {
            return this.ToXml();
        }

        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public long BatchID { get; set; }
        public string BatchCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ExpiryDateString { get; set; }
        public string MainItemName { get; set; }
        public double MRP { get; set; }
        public double CostRate { get; set; }

        public long SrNo { get; set; }  // Use to set SrNo of Main Item with Free Item

        public override string ToString()
        {
            return this.ItemName + " - " + this.BatchCode + " - " + this.ExpiryDateString;
        }


    }

}
