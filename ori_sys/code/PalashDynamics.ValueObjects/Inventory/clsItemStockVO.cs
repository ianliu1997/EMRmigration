using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.CompoundDrug;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsItemStockVO : IValueObject, INotifyPropertyChanged
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
        // Added By Rohit
        private double _ConversionFactor = 1;
        public double ConversionFactor
        {
            get { return _ConversionFactor; }
            set
            {
                if (_ConversionFactor != value && _ConversionFactor > 0)
                {
                    _ConversionFactor = value;
                    OnPropertyChanged("ConversionFactor");
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
        // End

        private Boolean _IsConsignment;
        public Boolean IsConsignment
        {
            get
            {
                return _IsConsignment;
            }
            set
            {
                _IsConsignment = value;
                OnPropertyChanged("IsConsignment");
            }
        }


        private Boolean _IsSelected = false;
        public Boolean IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private Boolean _IsBatchEnabled;
        public Boolean IsBatchEnabled
        {
            get
            {
                return _IsBatchEnabled;
            }
            set
            {
                _IsBatchEnabled = value;
                OnPropertyChanged("IsBatchEnabled");
            }
        }

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

        #region Property Declaration Section
        
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

        private long? _StoreID;
        public long? StoreID
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
        
        private long? _DepartmentID;
        public long? DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (_DepartmentID != value)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
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

        private InventoryTransactionType _TransactionTypeID;
        public InventoryTransactionType TransactionTypeID
        {
            get { return _TransactionTypeID; }
            set
            {
                if (_TransactionTypeID != value)
                {
                    _TransactionTypeID = value;
                    OnPropertyChanged("TransactionTypeID");
                }
            }
        }

        private long _TransactionID;
        public long TransactionID
        {
            get { return _TransactionID; }
            set
            {
                if (_TransactionID != value)
                {
                    _TransactionID = value;
                    OnPropertyChanged("TransactionID");
                }
            }
        }

        private double _PreviousBalance;
        public double PreviousBalance
        {
            get { return _PreviousBalance; }
            set
            {
                if (_PreviousBalance != value)
                {
                    _PreviousBalance = value;
                    OnPropertyChanged("PreviousBalance");
                }
            }
        }
        
        private InventoryStockOperationType _OperationType;
        public InventoryStockOperationType OperationType
        {
            get { return _OperationType; }
            set
            {
                if (_OperationType != value)
                {
                    _OperationType = value;
                    OnPropertyChanged("OperationType");
                }
            }
        }

        private double _TransactionQuantity;
        public double TransactionQuantity
        {
            get { return _TransactionQuantity; }
            set
            {
                if (_TransactionQuantity != value)
                {
                    _TransactionQuantity = value;
                    OnPropertyChanged("TransactionQuantity");
                }
            }
        }

        public int intOperationType { get; set; }  //By Umesh

        private double? _StockInHand;
        public double? StockInHand
        {
            get { return _StockInHand; }
            set
            {
                if (_StockInHand != value)
                {
                    _StockInHand = value;
                    OnPropertyChanged("StockInHand");
                }
            }
        }

        private double? _BlockedStock;
        public double? BlockedStock
        {
            get { return _BlockedStock; }
            set
            {
                if (_BlockedStock != value)
                {
                    _BlockedStock = value;
                    OnPropertyChanged("BlockedStock");
                }
            }
        }

        private double _AvailableStock;
        public double AvailableStock
        {
            get { return _AvailableStock; }
            set
            {
                if (_AvailableStock != value)
                {
                    _AvailableStock = value;
                    OnPropertyChanged("AvailableStock");
                }
            }
        }
        private double _CurrentStock;
        public double CurrentStock
        {
            get { return _CurrentStock; }
            set
            {
                if (_CurrentStock != value)
                {
                    _CurrentStock = value;
                    OnPropertyChanged("CurrentStock");
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

        public string BatchCode { get; set; }

        public Double VATPerc { get; set; }
        public Double VATAmt { get; set; }
        public float BaseCP { get; set; }
        public float BaseMRP { get; set; }
        public float TotalNetCP { get; set; }

        
        public DateTime? ExpiryDate { get; set; }
       // public double MRP { get; set; }

        private double _MRP;
        public double MRP
        {
            get { return _MRP; }
            set
            {
                if (_MRP != value)
                {
                    _MRP = value;
                    OnPropertyChanged("MRP");
                }
            }
        }
        private double _DiscountOnSale;
        public double DiscountOnSale
        {
            get { return _DiscountOnSale; }
            set
            {
                if (_DiscountOnSale != value)
                {
                    _DiscountOnSale = value;
                    OnPropertyChanged("DiscountOnSale");
                }
            }
        }
        public double PurchaseRate { get; set; }
        public string UnitName { get; set; }
        public string StoreName { get; set; }
        public string ItemName { get; set; }



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

        private double _PhyStock;
        public double PhysicalStock
        {
            get { return _PhyStock; }
            set
            {
                if (_PhyStock != value)
                {
                    _PhyStock = value;
                    OnPropertyChanged("PhysicalStock");
                }
            }
        }

        private double _VarianceStock;
        public double VarianceStock
        {
            get { return _VarianceStock; }
            set
            {
                if (_VarianceStock != value)
                {
                    _VarianceStock = value;
                    OnPropertyChanged("VarianceStock");
                }
            }
        }

        private double _VarianceAmount;
        public double VarianceAmount
        {
            get { return _VarianceAmount; }
            set
            {
                if (_VarianceAmount != value)
                {
                    _VarianceAmount = value;
                    OnPropertyChanged("VarianceAmount");
                }
            }
        }




        //Added By Somnath
        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set
            {
                if (_PrescriptionID != value)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
                }
            }
        }

        private long _DrugID;
        public long DrugID
        {
            get { return _DrugID; }
            set
            {
                if (_DrugID != value)
                {
                    _DrugID = value;
                    OnPropertyChanged("DrugID");
                }
            }
        }

        private string _DrugName;
        public string DrugName
        {
            get { return _DrugName; }
            set
            {
                if (_DrugName != value)
                {
                    _DrugName = value;
                    OnPropertyChanged("DrugName");
                }
            }
        }

        private string _Dose;
        public string Dose
        {
            get { return _Dose; }
            set
            {
                if (_Dose != value)
                {
                    _Dose = value;
                    OnPropertyChanged("Dose");
                }
            }
        }

        private string _Route;
        public string Route
        {
            get { return _Route; }
            set
            {
                if (_Route != value)
                {
                    _Route = value;
                    OnPropertyChanged("Route");
                }
            }
        }

        private string _Frequency;
        public string Frequency
        {
            get { return _Frequency; }
            set
            {
                if (_Frequency != value)
                {
                    _Frequency = value;
                    OnPropertyChanged("Frequency");
                }
            }
        }

        private int? _Days;
        public int? Days
        {
            get { return _Days; }
            set
            {
                if (_Days != value)
                {
                    _Days = value;
                    OnPropertyChanged("Days");
                }
            }
        }

        private int? _Quantity;
        public int? Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }
        private DateTime? _VisitDate;
        public DateTime? VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (_VisitDate != value)
                {
                    _VisitDate = value;
                    OnPropertyChanged("VisitDate");
                }
            }
        }

        private bool? _IsBatchRequired;
        public bool? IsBatchRequired
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

        private bool _IsOther;
        public bool IsOther
        {
            get { return _IsOther; }
            set
            {
                if (_IsOther != value)
                {
                    _IsOther = value;
                    OnPropertyChanged("IsOther");
                }
            }
        }

        private string _Reason;
        public string Reason
        {
            get { return _Reason; }
            set
            {
                if (_Reason != value)
                {
                    _Reason = value;
                    OnPropertyChanged("Reason");
                }
            }
        }

        public string StockingUOM { get; set; } //By Umesh

        //End
        //By Anjali...........................
        private float _Re_Order;
        public float Re_Order
        {
            get { return _Re_Order; }
            set
            {
                if (_Re_Order != value)
                {
                    _Re_Order = value;
                    OnPropertyChanged("Re_Order");
                }
            }
        }
        private float _AvailableStockInBase;
        public float AvailableStockInBase
        {
            get { return _AvailableStockInBase; }
            set
            {
                if (_AvailableStockInBase != value)
                {
                    _AvailableStockInBase = value;
                    OnPropertyChanged("AvailableStockInBase");
                }
            }
        }
        //....................................
        // Added By CDS
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
        
        private string _PUOM;
        public string PUOM
        {
            get
            {
                return _PUOM;
            }
            set
            {
                if (value != _PUOM)
                {
                    _PUOM = value;
                    OnPropertyChanged("PUOM");
                }
            }
        }

        private long _SUM;
        public long SUM
        {
            get
            {
                return _SUM;
            }
            set
            {
                if (value != _SUM)
                {
                    _SUM = value;
                    OnPropertyChanged("SUM");
                }
            }
        }
        
        private long _PUM;
        public long PUM
        {
            get
            {
                return _PUM;
            }
            set
            {
                if (value != _PUM)
                {
                    _PUM = value;
                    OnPropertyChanged("PUM");
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
        //END

        public bool IsInclusiveOfTax { get; set; }

        public clsCompoundDrugMasterVO CompoundDrugMaster { get; set; }

        public float InputTransactionQuantity { get; set; }
        public bool IsFromOpeningBalance { get; set; }

        private Boolean _IsItemBlock;
        public Boolean IsItemBlock
        {
            get
            {
                return _IsItemBlock;
            }
            set
            {
                _IsItemBlock = value;
                OnPropertyChanged("IsItemBlock");
            }
        }
        #endregion 
        
        # region For Conversion Factor

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

        public long TransactionUOMID { get; set; }

        # endregion

        private Boolean _IsFreeItem;
        public Boolean IsFreeItem
        {
            get
            {
                return _IsFreeItem;
            }
            set
            {
                _IsFreeItem = value;
                OnPropertyChanged("IsFreeItem");
            }
        }

        //added by neena
        public decimal SGSTPercentage { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal CGSTPercentage { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTPercentage { get; set; }
        public decimal IGSTAmount { get; set; }
        //

        #region For Item Selection Control

        public clsItemMasterVO SelectedItemObj { get; set; }   //For Item Selection Control

        #endregion


        //***//-----------
        private double _StaffDiscount;
        public double StaffDiscount
        {
            get
            {
                return _StaffDiscount;
            }
            set
            {
                if (value != _StaffDiscount)
                {
                    _StaffDiscount = value;
                    OnPropertyChanged("StaffDiscount");
                }
            }
        }

        private double _WalkinDiscount;
        public double WalkinDiscount
        {
            get
            {
                return _WalkinDiscount;
            }
            set
            {
                if (value != _WalkinDiscount)
                {
                    _WalkinDiscount = value;
                    OnPropertyChanged("WalkinDiscount");
                }
            }
        }

        private double _RegisteredPatientsDiscount;
        public double RegisteredPatientsDiscount
        {
            get
            {
                return _RegisteredPatientsDiscount;
            }
            set
            {
                if (value != _RegisteredPatientsDiscount)
                {
                    _RegisteredPatientsDiscount = value;
                    OnPropertyChanged("RegisteredPatientsDiscount");
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
        //---------------------

        // Begin : added by aniketk on 20-Oct-2018 for Item Group & Category filter
        public string ItemGroupString { get; set; }
        public string ItemCategoryString { get; set; }
        // End : added by aniketk on 20-Oct-2018 for Item Group & Category filter

    } 

    
}
