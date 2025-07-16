using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsWOGRNVO : IValueObject, INotifyPropertyChanged
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

             public clsWOGRNVO()
             {
                 _GRNType = InventoryGRNType.Direct;
                 _PaymentModeID = MaterPayModeList.Cash;
                 int gnt = Convert.ToInt32(InventoryGRNType.Direct);
             }

             private List<clsWOGRNDetailsVO> _Items = new List<clsWOGRNDetailsVO>();
             public List<clsWOGRNDetailsVO> Items
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

             private List<clsWOGRNDetailsVO> _ItemsWOGRN = new List<clsWOGRNDetailsVO>();
             public List<clsWOGRNDetailsVO> ItemsWOGRN
             {
                 get { return _ItemsWOGRN; }
                 set
                 {
                     if (_ItemsWOGRN != value)
                     {
                         _ItemsWOGRN = value;
                         OnPropertyChanged("ItemsWOGRN");
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


             private DateTime? _WODate;
             public DateTime? WODate
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

             private string _WONowithDate;
             public string WONowithDate
             {
                 get { return _WONowithDate; }
                 set
                 {
                     if (_WONowithDate != value)
                     {
                         _WONowithDate = value;
                         OnPropertyChanged("WONowithDate");
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

         }
    public class clsWOGRNDetailsVO : IValueObject, INotifyPropertyChanged
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
        private double _WOQuantity;
        public double WOQuantity
        {
            get { return _WOQuantity; }
            set
            {
                if (_WOQuantity != value)
                {
                    _WOQuantity = value;
                    OnPropertyChanged("WOQuantity");
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
                    OnPropertyChanged("TotalItemQuantity");
                    OnPropertyChanged("TotalQuantity");
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("SchDiscountAmount");
                    #region Added By Pallavi
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("TaxAmount");
                    #endregion
                    ////
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _WOPendingQuantity;
        public double WOPendingQuantity
        {
            get { return _WOPendingQuantity; }
            set
            {
                if (_WOPendingQuantity != value)
                {

                    _WOPendingQuantity = value;
                    OnPropertyChanged("WOPendingQuantity");

                }
            }
        }
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
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("CDiscountAmount");
                    OnPropertyChanged("SchDiscountAmount");
                    OnPropertyChanged("VATAmount");

                    OnPropertyChanged("TaxAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _Amount;
        public double Amount
        {
            get { return _Amount = _Rate * _Quantity; }
            set
            {
                if (_Amount != value)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
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
                    OnPropertyChanged("VATAmount");
                    OnPropertyChanged("NetAmount");


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
                _VATAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _VATPercent) / 100);
                return _VATAmount;
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

        private double _TaxAmount;
        public double TaxAmount
        {
            get
            {
                //if (_VATPercent != 0)
                //{
                //    return _VATAmount = ((_Amount * _VATPercent) / 100);
                //}
                _TaxAmount = (((_Amount - _CDiscountAmount - _SchDiscountAmount) * _ItemTax) / 100);
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
                return _CDiscountAmount = ((_Amount * _CDiscountPercent) / 100);
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
                return _SchDiscountAmount = ((_Amount * _SchDiscountPercent) / 100);


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
                // _NetAmount = _Amount + _VATAmount - _SchDiscountAmount - _CDiscountAmount;
                #region Added By Pallavi
                //double itemTaxAmount = ((_Amount * ItemTax) / 100);
                _NetAmount = _Amount - _SchDiscountAmount - _CDiscountAmount + _VATAmount + TaxAmount;
                #endregion

                return _NetAmount;
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


        

        private Boolean _IsBatchAssign=false;
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


        private long _WODetailID;
        public long WODetailID
        {
            get { return _WODetailID; }
            set
            {
                if (_WODetailID != value)
                {
                    _WODetailID = value;
                    OnPropertyChanged("WODetailID");
                }
            }
        }

        private long _WODetailUnitID;
        public long WODetailUnitID
        {
            get { return _WODetailUnitID; }
            set
            {
                if (_WODetailUnitID != value)
                {
                    _WODetailUnitID = value;
                    OnPropertyChanged("WODetailUnitID");
                }
            }
        }


    }
    }

