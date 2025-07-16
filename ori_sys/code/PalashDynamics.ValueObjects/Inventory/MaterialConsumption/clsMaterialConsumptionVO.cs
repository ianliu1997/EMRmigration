using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Inventory.MaterialConsumption
{
    public class clsMaterialConsumptionVO : IValueObject, INotifyPropertyChanged
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

        #region Property
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private string _StoreName;
        public string StoreName
        {
            get
            {
                return _StoreName;
            }
            set
            {
                if (value != _StoreName)
                {
                    _StoreName = value;
                    OnPropertyChanged("StoreName");
                }
            }
        }

        private decimal _TotalItems;
        public decimal TotalItems
        {
            get
            {
                return _TotalItems;
            }
            set
            {
                if (value != _TotalItems)
                {
                    _TotalItems = value;
                    OnPropertyChanged("TotalItems");
                }
            }
        }

        private long _UnitID;

        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (value != _UnitID)
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
                if (value != _StoreID)
                {
                    _StoreID = value;
                    OnPropertyChanged("StoreID");

                }
            }
        }

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (value != _PackageID)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        private string _ConsumptionNo;

        public string ConsumptionNo
        {
            get { return _ConsumptionNo; }
            set
            {
                if (value != _ConsumptionNo)
                {
                    _ConsumptionNo = value;
                    OnPropertyChanged("ConsumptionNo");
                }
            }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set
            {
                if (value != _Date)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private DateTime _Time;

        public DateTime Time
        {
            get { return _Time; }
            set
            {
                if (value != _Time)
                {
                    _Time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        private decimal _TotalAmount;

        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (value != _TotalAmount)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }

        private string _Remark;

        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private long _PatientId;
        public long PatientId
        {
            get { return _PatientId; }
            set
            {
                if (value != _PatientId)
                {
                    _PatientId = value;
                    OnPropertyChanged("PatientId");

                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (value != _BillNo)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        private DateTime? _BillDate;

        public DateTime? BillDate
        {
            get { return _BillDate; }
            set
            {
                if (value != _BillDate)
                {
                    _BillDate = value;
                    OnPropertyChanged("BillDate");
                }
            }
        }

        private decimal _TotalBillAmount;

        public decimal TotalBillAmount
        {
            get { return _TotalBillAmount; }
            set
            {
                if (value != _TotalBillAmount)
                {
                    _TotalBillAmount = value;
                    OnPropertyChanged("TotalBillAmount");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (value != _PatientName)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private List<clsIssueItemDetailsVO> _IssueItemDetailsList = new List<clsIssueItemDetailsVO>();
        public List<clsIssueItemDetailsVO> IssueItemDetailsList
        {
            get
            {
                return _IssueItemDetailsList;
            }
            set
            {
                if (value != _IssueItemDetailsList)
                {
                    _IssueItemDetailsList = value;
                    OnPropertyChanged("IssueItemDetailsList");
                }
            }
        }

        private List<clsMaterialConsumptionItemDetailsVO> _ConsumptionItemDetailsList = new List<clsMaterialConsumptionItemDetailsVO>();
        public List<clsMaterialConsumptionItemDetailsVO> ConsumptionItemDetailsList
        {
            get
            {
                return _ConsumptionItemDetailsList;
            }
            set
            {
                if (value != _ConsumptionItemDetailsList)
                {
                    _ConsumptionItemDetailsList = value;
                    OnPropertyChanged("ConsumptionItemDetailsList");
                }
            }
        }

        //By Umesh For Against Patient
        //public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public bool IsAgainstPatient { get; set; }
        //public string MRNO { get; set; }
        //public string PatientFirstName { get; set; }
        //public string PatientLastName { get; set; }
        //public string PatientFullName { get; set; }

        // END

              

        //Added by AJ date 2/1/2017

      
        private long _Opd_Ipd_External_Id;
        public long Opd_Ipd_External_Id
        {
            get { return _Opd_Ipd_External_Id; }
            set
            {
                if (_Opd_Ipd_External_Id != value)
                {
                    _Opd_Ipd_External_Id = value;
                    OnPropertyChanged("Opd_Ipd_External_Id");
                }
            }
        }

        private long _Opd_Ipd_External_UnitId;
        public long Opd_Ipd_External_UnitId
        {
            get { return _Opd_Ipd_External_UnitId; }
            set
            {
                if (_Opd_Ipd_External_UnitId != value)
                {
                    _Opd_Ipd_External_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitId");
                }
            }
        }

        private long _Opd_Ipd_External;
        public long Opd_Ipd_External
        {
            get { return _Opd_Ipd_External; }
            set
            {
                if (_Opd_Ipd_External != value)
                {
                    _Opd_Ipd_External = value;
                    OnPropertyChanged("Opd_Ipd_External");
                }
            }
        }

        private long _LinkPatientID;
        public long LinkPatientID
        {
            get { return _LinkPatientID; }
            set
            {
                if (_LinkPatientID != value)
                {
                    _LinkPatientID = value;
                    OnPropertyChanged("LinkPatientID");
                }
            }
        }

        private long _LinkPatientUnitID;
        public long LinkPatientUnitID
        {
            get { return _LinkPatientUnitID; }
            set
            {
                if (_LinkPatientUnitID != value)
                {
                    _LinkPatientUnitID = value;
                    OnPropertyChanged("LinkPatientUnitID");
                }
            }
        }

        public double ConsumptionAmount { get; set; } //Date 2/2/2017
        public double PreviousConsumptionAmount { get; set; } //Date 18/4/2017
        public double PharmacyFixedRate { get; set; } //Date 18/2/2017    
        //Added by AJ date 19/1/2017
        public long ItemId { get; set; }
        public long BatchId { get; set; }
        public decimal UsedQty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string BatchCode { get; set; }
        public DateTime? _ExpiryDate;
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
                    _MRP = value;
                    OnPropertyChanged("MRP");
                }
            }
        }
        public string ItemName { get; set; }
        public double AvailableStock { get; set; }
        public string TransactionUOM { get; set; }
        public string PackageName { get; set; }

        private decimal _TotalMRPAmount;

        public decimal TotalMRPAmount
        {
            get { return _TotalMRPAmount; }
            set
            {
                if (value != _TotalMRPAmount)
                {
                    _TotalMRPAmount = value;
                    OnPropertyChanged("TotalMRPAmount");
                }
            }
        }

        //Date 18/4/2017
        private long _PackageBillID;
        public long PackageBillID
        {
            get { return _PackageBillID; }
            set
            {
                if (value != _PackageBillID)
                {
                    _PackageBillID = value;
                    OnPropertyChanged("PackageBillID");
                }
            }
        }

        private long _PackageBillUnitID;
        public long PackageBillUnitID
        {
            get { return _PackageBillUnitID; }
            set
            {
                if (value != _PackageBillUnitID)
                {
                    _PackageBillUnitID = value;
                    OnPropertyChanged("PackageBillUnitID");
                }
            }
        }

        //***//------------------
        #endregion

        #region For PatientIndentReceiveStock
        public bool IsAgainstPatientIndent { get; set; }

        public string ItemIDs { get; set; }
        #endregion

        #region For Package New Changes Added on 30042018

        public bool IsPackageConsumable { get; set; }
        public double PackageConsumableLimit { get; set; }

        #endregion

    }
    public class clsMaterialConsumptionItemDetailsVO : INotifyPropertyChanged
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

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
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
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
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

        private long _ConsumptionID;
        public long ConsumptionID
        {
            get
            {
                return _ConsumptionID;
            }
            set
            {
                if (value != _ConsumptionID)
                {
                    _ConsumptionID = value;
                    OnPropertyChanged("ConsumptionID");
                }
            }
        }

        private long _BatchId;
        public long BatchId
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

        private decimal _UsedQty;
        public decimal UsedQty
        {
            get
            {
                return _UsedQty;
            }
            set
            {
                if (value != _UsedQty)
                {
                    //if (value <= 0)
                    //    _UsedQty = 1;
                    //if (value.ToString().Length > 5)
                    //{
                    //    throw new ValidationException("Quantity Length Should Not Be Greater Than 5 Digits.");
                    //}
                    //else
                        _UsedQty = value;
                    _UsedQty = Math.Round(_UsedQty, 3);
                    if (!this.Flag)
                     //   Amount = _UsedQty * Rate;// *Convert.ToDecimal(ConversionFactor);
                    OnPropertyChanged("UsedQty");
                }
            }
        }

        private decimal _Rate;
        public decimal Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                if (value != _Amount)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        private string _Remark;
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _BatchCode;
        public string BatchCode
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
                }
            }
        }
        private string _ItemName;
        public string ItemName
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
        public bool Flag { get; set; }

        public double BaseOty { get; set; }        

        # region // FOR CONVERSION FACTOR

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

        public string SUOM { get; set; }
        public string TransactionUOM { get; set; }

        public long TransactionUOMID { get; set; }
        public decimal StockOty { get; set; }

        public float StockToBase { get; set; }

        //Added by AJ Date 18/1/2017
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

        public double PurchaseRate { get; set; }

        public bool IsAgainstPatientIndent { get; set; }

        //***//------------
        
        # endregion

        #region PatientIndentReceiveStock

        private double _TotalPatientIndentReceiveQty;
        public double TotalPatientIndentReceiveQty
        {
            get { return _TotalPatientIndentReceiveQty; }
            set
            {
                if (_TotalPatientIndentReceiveQty != value)
                {
                    _TotalPatientIndentReceiveQty = value;
                    OnPropertyChanged("TotalPatientIndentReceiveQty");
                }
            }
        }

        private double _TotalPatientIndentConsumptionQty;
        public double TotalPatientIndentConsumptionQty
        {
            get { return _TotalPatientIndentConsumptionQty; }
            set
            {
                if (_TotalPatientIndentConsumptionQty != value)
                {
                    _TotalPatientIndentConsumptionQty = value;
                    OnPropertyChanged("_TotalPatientIndentConsumptionQty");
                }
            }
        }

        #endregion
    }
}
