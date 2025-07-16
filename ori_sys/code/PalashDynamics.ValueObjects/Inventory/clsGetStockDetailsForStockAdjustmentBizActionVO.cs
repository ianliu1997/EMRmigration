using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetStockDetailsForStockAdjustmentBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetStockDetailsForStockAdjustmentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        public long StoreID { get; set; }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private List<clsAdjustmentStockVO> _objStock = new List<clsAdjustmentStockVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsAdjustmentStockVO> StockList
        {
            get { return _objStock; }
            set { _objStock = value; }
        }

    }
    public class clsAdjustmentStockVO : IValueObject, INotifyPropertyChanged
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


        private long _StockAdjustmentID;
        public long StockAdjustmentID
        {
            get { return _StockAdjustmentID; }
            set
            {
                if (_StockAdjustmentID != value)
                {
                    _StockAdjustmentID = value;
                    OnPropertyChanged("StockAdjustmentID");
                }
            }
        }
        private long _StockAdjustmentUnitID;
        public long StockAdjustmentUnitID
        {
            get { return _StockAdjustmentUnitID; }
            set
            {
                if (_StockAdjustmentUnitID != value)
                {
                    _StockAdjustmentUnitID = value;
                    OnPropertyChanged("StockAdjustmentUnitID");
                }
            }
        }


        public long StockId { get; set; }
        public string Store { get; set; }
        public double AvailableStock { get; set; }
        public double AvailableStockInBase { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Double Quantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public string BatchCode { get; set; }
        public string Remarks { get; set; }
        public long? BatchId { get; set; }
        public long StoreID { get; set; }
        public long UnitID { get; set; }
        //public Double AdjustmentQunatitiy { get; set; }


        private double _AdjustmentQunatitiy;
        public double AdjustmentQunatitiy
        {
            get { return _AdjustmentQunatitiy; }
            set
            {
                if (_AdjustmentQunatitiy != value)
                {
                    if (value <= 0)
                    {
                        value = 0;
                        _AdjustmentQunatitiy = value;
                    }
                    if (value.ToString().Length > 5)
                    {
                        throw new ValidationException("Quantity Length Should Not Be Greater Than 5 Digits.");
                    }
                    else
                    {
                        _AdjustmentQunatitiy = value;
                        OnPropertyChanged("AdjustmentQunatitiy");
                    }

                }
            }
        }

        public InventoryStockOperationType OperationType { get; set; }
        public Double UpdatedBalance { get; set; }
        public DateTime? DateTime { get; set; }
        public string Time { get; set; }
        public long Authorizedby { get; set; }
        //public bool Status { get; set; }
        public string StockAdjustmentNo { get; set; }
        public string stOperationType { get; set; }
        public int intOperationType { get; set; }

        private bool _Status;
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

        private bool _RadioStatusYes = true;
        public bool RadioStatusYes
        {
            get { return _RadioStatusYes; }
            set
            {
                if (_RadioStatusYes != value)
                {
                    _RadioStatusYes = value;
                    OnPropertyChanged("RadioStatusYes");
                }
            }

        }
        private bool _RadioStatusNo;
        public bool RadioStatusNo
        {
            get { return _RadioStatusNo; }
            set
            {
                if (_RadioStatusNo != value)
                {
                    _RadioStatusNo = value;
                    OnPropertyChanged("RadioStatusNo");
                }
            }
        }

        //.............................................................

        private long _SellingUMID;
        public long SellingUMID
        {
            get { return _SellingUMID; }
            set
            {
                if (_SellingUMID != value)
                {
                    _SellingUMID = value;
                    OnPropertyChanged("SellingUMID");
                }
            }
        }
        private string _SellingUM;
        public string SellingUM
        {
            get { return _SellingUM; }
            set
            {
                if (_SellingUM != value)
                {
                    _SellingUM = value;
                    OnPropertyChanged("SellingUM");
                }
            }
        }

        private long _BaseUMID;
        public long BaseUMID
        {
            get { return _BaseUMID; }
            set
            {
                if (_BaseUMID != value)
                {
                    _BaseUMID = value;
                    OnPropertyChanged("BaseUMID");
                }
            }
        }
        private string _BaseUM;
        public string BaseUM
        {
            get { return _BaseUM; }
            set
            {
                if (_BaseUM != value)
                {
                    _BaseUM = value;
                    OnPropertyChanged("BaseUM");
                }
            }
        }

        private long _PurchaseUMID;
        public long PurchaseUMID
        {
            get { return _PurchaseUMID; }
            set
            {
                if (_PurchaseUMID != value)
                {
                    _PurchaseUMID = value;
                    OnPropertyChanged("PurchaseUMID");
                }
            }
        }
        private string _PurchaseUM;
        public string PurchaseUM
        {
            get { return _PurchaseUM; }
            set
            {
                if (_PurchaseUM != value)
                {
                    _PurchaseUM = value;
                    OnPropertyChanged("PurchaseUM");
                }
            }
        }

        private long _StockingUMID;
        public long StockingUMID
        {
            get { return _StockingUMID; }
            set
            {
                if (_StockingUMID != value)
                {
                    _StockingUMID = value;
                    OnPropertyChanged("StockingUMID");
                }
            }
        }
        private string _StockingUM;
        public string StockingUM
        {
            get { return _StockingUM; }
            set
            {
                if (_StockingUM != value)
                {
                    _StockingUM = value;
                    OnPropertyChanged("StockingUM");
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




                }
            }
        }

        public float StockingQuantity { get; set; }   //By Umesh
        public long TransactionUOMID { get; set; }    //By Umesh
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
                    // OnPropertyChanged("StockingToBaseCF");
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
                    // OnPropertyChanged("PurchaseToBaseCF");
                }
            }
        }

        private double _ActualQuantity;
        public double ActualQuantity
        {
            get
            {
                if (intOperationType == 1)
                {
                    _ActualQuantity = AvailableStock + _AdjustmentQunatitiy;
                }
                else if (intOperationType == 2)
                {
                    _ActualQuantity = AvailableStock - _AdjustmentQunatitiy;
                }
                else
                {
                    _ActualQuantity = 0;
                }
                return _ActualQuantity;
            }
            set
            {
                if (value != _ActualQuantity)
                {
                    _ActualQuantity = value;
                }
            }
        }


        //.............................................................
    }

    public class clsMRPAdjustmentMainVO : IValueObject, INotifyPropertyChanged
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

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
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

        private string _MRPAdjustmentNo;
        public string MRPAdjustmentNo
        {
            get { return _MRPAdjustmentNo; }
            set
            {
                if (_MRPAdjustmentNo != value)
                {
                    _MRPAdjustmentNo = value;
                    OnPropertyChanged("MRPAdjustmentNo");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
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

        private bool _IsFreeze;
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

        private bool _IsApprove;
        public bool IsApprove
        {
            get { return _IsApprove; }
            set
            {
                if (_IsApprove != value)
                {
                    _IsApprove = value;
                    OnPropertyChanged("IsApprove");
                }
            }
        }

        private bool _IsReject;
        public bool IsReject
        {
            get { return _IsReject; }
            set
            {
                if (_IsReject != value)
                {
                    _IsReject = value;
                    OnPropertyChanged("IsReject");
                }
            }
        }

        private long _ApproveRejectBy;
        public long ApproveRejectBy
        {
            get { return _ApproveRejectBy; }
            set
            {
                if (_ApproveRejectBy != value)
                {
                    _ApproveRejectBy = value;
                    OnPropertyChanged("ApproveRejectBy");
                }
            }
        }

        private string _stApproveRejectBy;
        public string stApproveRejectBy
        {
            get { return _stApproveRejectBy; }
            set
            {
                if (_stApproveRejectBy != value)
                {
                    _stApproveRejectBy = value;
                    OnPropertyChanged("stApproveRejectBy");
                }
            }
        }

        private DateTime? _ApproveRejectDate;
        public DateTime? ApproveRejectDate
        {
            get { return _ApproveRejectDate; }
            set
            {
                if (_ApproveRejectDate != value)
                {
                    _ApproveRejectDate = value;
                    OnPropertyChanged("ApproveRejectDate");
                }
            }
        }

        private string _ApproveRejectRemark = "";
        public string ApproveRejectRemark
        {
            get { return _ApproveRejectRemark; }
            set
            {
                if (_ApproveRejectRemark != value)
                {
                    _ApproveRejectRemark = value;
                    OnPropertyChanged("ApproveRejectRemark");
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
    }

    public class clsMRPAdjustmentVO : IValueObject, INotifyPropertyChanged
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


        public long AdjustmentId { get; set; }
        public string Store { get; set; }
        public Double Quantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public string BatchCode { get; set; }

        public bool IsFreeBatch { get; set; }

        private string _UpdatedBatchCode;
        public string UpdatedBatchCode
        {
            get { return _UpdatedBatchCode; }
            set
            {
                if (_UpdatedBatchCode != value)
                {
                    _UpdatedBatchCode = value;
                    OnPropertyChanged("UpdatedBatchCode");
                }
            }

        }

        public string Remarks { get; set; }
        public long? BatchId { get; set; }
        public long StoreID { get; set; }

        public DateTime? ExpiryDate;
        public string ExpiryDateString { get; set; }

        private DateTime? _UpdatedExpiryDate;
        public DateTime? UpdatedExpiryDate
        {
            get { return _UpdatedExpiryDate; }
            set
            {
                if (_UpdatedExpiryDate != value)
                {
                    _UpdatedExpiryDate = value;
                    OnPropertyChanged("UpdatedExpiryDate");
                }
            }
        }


        public Double MRP { get; set; }
        private Double _UpdatedMRP;
        public Double UpdatedMRP
        {
            get { return _UpdatedMRP; }
            set
            {
                if (_UpdatedMRP != value)
                {
                    _UpdatedMRP = value;
                    OnPropertyChanged("UpdatedMRP");
                }
            }

        }
        private Double _UpdatedPurchaseRate;
        public Double UpdatedPurchaseRate
        {
            get
            {
                return _UpdatedPurchaseRate;
            }
            set
            {
                _UpdatedPurchaseRate = value;
                OnPropertyChanged("UpdatedPurchaseRate");
            }
        }
        public Double PurchaseRate { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string Time { get; set; }
        public long Authorizedby { get; set; }
        public string MRPAdjustmentNo { get; set; }
        public string stOperationType { get; set; }
        public int intOperationType { get; set; }

        private bool _Status;
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

        private bool _IsBatcheRequired;
        public bool IsBatcheRequired
        {
            get { return _IsBatcheRequired; }
            set
            {
                if (_IsBatcheRequired != value)
                {
                    _IsBatcheRequired = value;
                    OnPropertyChanged("IsBatcheRequired");
                }
            }
        }

        public string stBatchUpdateStatus { get; set; }
    }



    //By Anjali............................................
    public class clsAdjustmentStockMainVO : IValueObject, INotifyPropertyChanged
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




        private bool _RadioStatusYes;
        public bool RadioStatusYes
        {
            get { return _RadioStatusYes; }
            set
            {
                if (_RadioStatusYes != value)
                {
                    _RadioStatusYes = value;
                    OnPropertyChanged("RadioStatusYes");
                    OnPropertyChanged("intOperationType");
                    OnPropertyChanged("PhysicalQuantity");
                }
            }

        }
        private bool _RadioStatusNo;
        public bool RadioStatusNo
        {
            get { return _RadioStatusNo; }
            set
            {
                if (_RadioStatusNo != value)
                {
                    _RadioStatusNo = value;
                    OnPropertyChanged("RadioStatusNo");
                    OnPropertyChanged("intOperationType");
                    OnPropertyChanged("PhysicalQuantity");
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

        private bool _IsFromPST;
        public bool IsFromPST
        {
            get { return _IsFromPST; }
            set
            {
                if (_IsFromPST != value)
                {
                    _IsFromPST = value;
                    OnPropertyChanged("IsFromPST");
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
        private string _ResonForRejection;
        public string ResonForRejection
        {
            get { return _ResonForRejection; }
            set
            {
                if (_ResonForRejection != value)
                {
                    _ResonForRejection = value;
                    OnPropertyChanged("ResonForRejection");
                }
            }
        }


        private long _RejectedBy;
        public long RejectedBy
        {
            get { return _RejectedBy; }
            set
            {
                if (_RejectedBy != value)
                {
                    _RejectedBy = value;
                    OnPropertyChanged("RejectedBy");
                }
            }
        }
        private DateTime? _RejectedDateTime;
        public DateTime? RejectedDateTime
        {
            get { return _RejectedDateTime; }
            set
            {
                if (_RejectedDateTime != value)
                {
                    _RejectedDateTime = value;
                    OnPropertyChanged("RejectedDateTime");
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
        private long _ApprovedBy;
        public long ApprovedBy
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
        private DateTime? _ApprovedDateTime;
        public DateTime? ApprovedDateTime
        {
            get { return _ApprovedDateTime; }
            set
            {
                if (_ApprovedDateTime != value)
                {
                    _ApprovedDateTime = value;
                    OnPropertyChanged("ApprovedDateTime");
                }
            }
        }
        private long _PhysicalItemID;
        public long PhysicalItemID
        {
            get { return _PhysicalItemID; }
            set
            {
                if (_PhysicalItemID != value)
                {
                    _PhysicalItemID = value;
                    OnPropertyChanged("PhysicalItemID");
                }
            }
        }

        private long _PhysicalItemUnitID;
        public long PhysicalItemUnitID
        {
            get { return _PhysicalItemUnitID; }
            set
            {
                if (_PhysicalItemUnitID != value)
                {
                    _PhysicalItemUnitID = value;
                    OnPropertyChanged("PhysicalItemUnitID");
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

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }
        private string _StockAdjustmentNo;
        public string StockAdjustmentNo
        {
            get { return _StockAdjustmentNo; }
            set
            {
                if (_StockAdjustmentNo != value)
                {
                    _StockAdjustmentNo = value;
                    OnPropertyChanged("StockAdjustmentNo");
                }
            }
        }

        private DateTime? _RequestDateTime;
        public DateTime? RequestDateTime
        {
            get { return _RequestDateTime; }
            set
            {
                if (_RequestDateTime != value)
                {
                    _RequestDateTime = value;
                    OnPropertyChanged("RequestDateTime");
                }
            }
        }

        private long _RequestedBy;
        public long RequestedBy
        {
            get { return _RequestedBy; }
            set
            {
                if (_RequestedBy != value)
                {
                    _RequestedBy = value;
                    OnPropertyChanged("RequestedBy");
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


        private string _ApprovedByName;
        public string ApprovedByName
        {
            get { return _ApprovedByName; }
            set
            {
                if (_ApprovedByName != value)
                {
                    _ApprovedByName = value;
                    OnPropertyChanged("ApprovedByName");
                }
            }
        }
        private string _Store;
        public string Store
        {
            get { return _Store; }
            set
            {
                if (_Store != value)
                {
                    _Store = value;
                    OnPropertyChanged("Store");
                }
            }
        }

        private string _RequestedByName;
        public string RequestedByName
        {
            get { return _RequestedByName; }
            set
            {
                if (_RequestedByName != value)
                {
                    _RequestedByName = value;
                    OnPropertyChanged("RequestedByName");
                }
            }
        }
    }
    //.....................................................
}
