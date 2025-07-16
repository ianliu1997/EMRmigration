using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsPatientPrescriptionDetailVO : IValueObject, INotifyPropertyChanged
    {
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

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        public bool Billed { get; set; }       
        public string VisibleBill { get; set; }
        public string collapseBill { get; set; }
       

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
        public bool IsItemBlock { get; set; }

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

        private string _sComponentQuantity = string.Empty;
        public string sComponentQuantity
        {
            get { return _sComponentQuantity; }
            set
            {
                _sComponentQuantity = value;
                OnPropertyChanged("sComponentQuantity");
            }
        }
        private string _sCompoundQuantity = string.Empty;
        public string sCompoundQuantity
        {
            get { return _sCompoundQuantity; }
            set
            {
                _sCompoundQuantity = value;
                OnPropertyChanged("sCompoundQuantity");
            }
        }

        private string _Instruction;
        public string Instruction
        {
            get { return _Instruction; }
            set
            {
                if (_Instruction != value)
                {
                    _Instruction = value;
                    OnPropertyChanged("Instruction");
                }
            }
        }

        private string _NewInstruction;
        public string NewInstruction
        {
            get { return _NewInstruction; }
            set
            {
                if (_NewInstruction != value)
                {
                    _NewInstruction = value;
                    OnPropertyChanged("NewInstruction");
                }
            }
        }

        private long _DiagnosisItemID;
        public long DiagnosisItemID
        {
            get { return _DiagnosisItemID; }
            set
            {
                if (_DiagnosisItemID != value)
                {
                    _DiagnosisItemID = value;
                    OnPropertyChanged("DiagnosisItemID");
                }
            }
        }

        private DateTime _VisitDate;
        public DateTime VisitDate
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

        private bool _FromHistory;
        public bool FromHistory
        {
            get { return _FromHistory; }
            set
            {
                if (_FromHistory != value)
                {
                    _FromHistory = value;
                    OnPropertyChanged("FromHistory");
                }
            }
        }

        public string Datetime { get; set; }

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

        private string _Year;
        public string Year
        {
            get { return _Year; }
            set
            {
                if (_Year != value)
                {
                    _Year = value;
                    OnPropertyChanged("Year");
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
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }


        private double _NewPendingQuantity;
        public double NewPendingQuantity
        {
            get { return _NewPendingQuantity; }
            set
            {
                if (_NewPendingQuantity != value)
                {
                    _NewPendingQuantity = value;
                    OnPropertyChanged("NewPendingQuantity");
                }
            }
        }

        //by neena
        private double _TotalNewPendingQuantity;
        public double TotalNewPendingQuantity
        {
            get { return _TotalNewPendingQuantity; }
            set
            {
                if (_TotalNewPendingQuantity != value)
                {
                    _TotalNewPendingQuantity = value;
                    OnPropertyChanged("TotalNewPendingQuantity");
                }
            }
        }
        //

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
        private double _PastPrescriptionQuantity;
        public double PastPrescriptionQuantity
        {
            get { return _PastPrescriptionQuantity; }
            set
            {
                if (_PastPrescriptionQuantity != value)
                {
                    _PastPrescriptionQuantity = value;
                    OnPropertyChanged("PastPrescriptionQuantity");
                }
            }
        }

        //added by neena
        private float _ActualPrescribedBaseQuantity;
        public float ActualPrescribedBaseQuantity
        {
            get
            {

                return _ActualPrescribedBaseQuantity;
            }
            set
            {
                if (_ActualPrescribedBaseQuantity != value)
                {

                    _ActualPrescribedBaseQuantity = value;
                    OnPropertyChanged("ActualPrescribedBaseQuantity");
                }
            }
        }
        //

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

        private bool _NewAdded;
        public bool NewAdded
        {
            get
            {
                return _NewAdded;
            }
            set
            {
                if (value != _NewAdded)
                {
                    _NewAdded = value;
                    OnPropertyChanged("NewAdded");
                }
            }
        }

        MasterListItem _SelectedInstruction = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedInstruction
        {
            get
            {
                return _SelectedInstruction;
            }
            set
            {
                if (value != _SelectedInstruction)
                {
                    _SelectedInstruction = value;
                    OnPropertyChanged("SelectedInstruction");
                }
            }


        }

        MasterListItem _SelectedRoute = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedRoute
        {
            get
            {
                return _SelectedRoute;
            }
            set
            {
                if (value != _SelectedRoute)
                {
                    _SelectedRoute = value;
                    OnPropertyChanged("SelectedRoute");
                }
            }


        }

        FrequencyMaster _SelectedFrequency = new FrequencyMaster { ID = 0, Description = "--Select--" };
        public FrequencyMaster SelectedFrequency
        {
            get
            {
                return _SelectedFrequency;
            }
            set
            {
                if (value != _SelectedFrequency)
                {
                    _SelectedFrequency = value;
                    OnPropertyChanged("SelectedFrequency");
                }
            }


        }

        MasterListItem _SelectedItem = new MasterListItem();
        public MasterListItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (value != _SelectedItem)
                {
                    _SelectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }


        }

        //Added By Somu
        public Boolean InclusiveOfTax { get; set; }

        List<FrequencyMaster> _FrequencyName = new List<FrequencyMaster>();
        public List<FrequencyMaster> FrequencyName
        {
            get
            {
                return _FrequencyName;
            }
            set
            {
                if (value != _FrequencyName)
                {
                    _FrequencyName = value;
                }
            }

        }
        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }


        }
        List<MasterListItem> _RouteName = new List<MasterListItem>();
        public List<MasterListItem> RouteName
        {
            get
            {
                return _RouteName;
            }
            set
            {
                if (value != _RouteName)
                {
                    _RouteName = value;
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
        List<MasterListItem> _InstructionName = new List<MasterListItem>();
        public List<MasterListItem> InstructionName
        {
            get
            {
                return _InstructionName;
            }
            set
            {
                if (value != _InstructionName)
                {
                    _InstructionName = value;
                }
            }

        }

        private long _CompoundDrugID;
        public long CompoundDrugID
        {
            get { return _CompoundDrugID; }
            set
            {
                if (_CompoundDrugID != value)
                {
                    _CompoundDrugID = value;
                    OnPropertyChanged("CompoundDrugID");
                }
            }
        }
        private string _CompoundDrug;
        public string CompoundDrug
        {
            get { return _CompoundDrug; }
            set
            {
                if (_CompoundDrug != value)
                {
                    _CompoundDrug = value;
                    OnPropertyChanged("CompoundDrug");
                }
            }
        }


        private long _CompoundDrugUnitID;
        public long CompoundDrugUnitID
        {
            get { return _CompoundDrugUnitID; }
            set
            {
                if (_CompoundDrugUnitID != value)
                {
                    _CompoundDrugUnitID = value;
                    OnPropertyChanged("CompoundDrugUnitID");
                }
            }
        }
        private double _CompoundQuantity;
        public double CompoundQuantity
        {
            get
            {
                return _CompoundQuantity;
            }
            set
            {
                if (_CompoundQuantity != value)
                {
                    _CompoundQuantity = value;
                    OnPropertyChanged("CompoundQuantity");
                }
            }
        }


        private float? _LabourChargeAmt;
        public float? LabourChargeAmt
        {
            get
            {
                return _LabourChargeAmt;
            }
            set
            {
                if (_LabourChargeAmt != value)
                {
                    _LabourChargeAmt = value;
                    OnPropertyChanged("LabourChargeAmt");
                }
            }
        }
        private float? _LabourChargePer;
        public float? LabourChargePer
        {
            get
            {
                return _LabourChargePer;
            }
            set
            {
                if (_LabourChargePer != value)
                {
                    _LabourChargePer = value;
                    OnPropertyChanged("LabourChargePer");
                }
            }
        }
        //End

        // Added By CDS 

        private bool? _IsPrescribedMedicine = false;
        public bool? IsPrescribedMedicine
        {
            get { return _IsPrescribedMedicine; }
            set
            {
                if (_IsPrescribedMedicine != value)
                {
                    _IsPrescribedMedicine = value;
                    OnPropertyChanged("IsPrescribedMedicine");
                }
            }
        }

        private bool? _IsInclusiveOfTax;
        public bool? IsInclusiveOfTax
        {
            get { return _IsInclusiveOfTax; }
            set
            {
                if (_IsInclusiveOfTax != value)
                {
                    _IsInclusiveOfTax = value;
                    OnPropertyChanged("IsInclusiveOfTax");
                }
            }
        }

        public long ItemCategory { get; set; }
        public long ItemGroup { get; set; }

        public string Manufacture { get; set; }
        public string PregnancyClass { get; set; }

        #region// GST For Sale

        private decimal _SGSTPercentSale;
        public decimal SGSTPercentSale
        {
            get
            {
                return _SGSTPercentSale;
            }
            set
            {
                if (value != _SGSTPercentSale)
                {
                    _SGSTPercentSale = value;
                    OnPropertyChanged("SGSTPercentSale");
                }
            }
        }

        private decimal _CGSTPercentSale;
        public decimal CGSTPercentSale
        {
            get
            {
                return _CGSTPercentSale;
            }
            set
            {
                if (value != _CGSTPercentSale)
                {
                    _CGSTPercentSale = value;
                    OnPropertyChanged("CGSTPercentSale");
                }
            }
        }

        private decimal _IGSTPercentSale;
        public decimal IGSTPercentSale
        {
            get
            {
                return _IGSTPercentSale;
            }
            set
            {
                if (value != _IGSTPercentSale)
                {
                    _IGSTPercentSale = value;
                    OnPropertyChanged("IGSTPercentSale");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _SGSTtaxtypeSale;
        public int SGSTtaxtypeSale
        {
            get
            {
                return _SGSTtaxtypeSale;
            }
            set
            {
                if (value != _SGSTtaxtypeSale)
                {
                    _SGSTtaxtypeSale = value;
                    OnPropertyChanged("SGSTtaxtypeSale");
                }
            }
        }
        private int _SGSTapplicableonSale;
        public int SGSTapplicableonSale
        {
            get
            {
                return _SGSTapplicableonSale;
            }
            set
            {
                if (value != _SGSTapplicableonSale)
                {
                    _SGSTapplicableonSale = value;
                    OnPropertyChanged("SGSTapplicableonSale");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _CGSTtaxtypeSale;
        public int CGSTtaxtypeSale
        {
            get
            {
                return _CGSTtaxtypeSale;
            }
            set
            {
                if (value != _CGSTtaxtypeSale)
                {
                    _CGSTtaxtypeSale = value;
                    OnPropertyChanged("CGSTtaxtypeSale");
                }
            }
        }
        private int _CGSTapplicableonSale;
        public int CGSTapplicableonSale
        {
            get
            {
                return _CGSTapplicableonSale;
            }
            set
            {
                if (value != _CGSTapplicableonSale)
                {
                    _CGSTapplicableonSale = value;
                    OnPropertyChanged("CGSTapplicableonSale");
                }
            }
        }

        ///////////////////////////////////////////////////

        private int _IGSTtaxtypeSale;
        public int IGSTtaxtypeSale
        {
            get
            {
                return _IGSTtaxtypeSale;
            }
            set
            {
                if (value != _IGSTtaxtypeSale)
                {
                    _IGSTtaxtypeSale = value;
                    OnPropertyChanged("IGSTtaxtypeSale");
                }
            }
        }
        private int _IGSTapplicableonSale;
        public int IGSTapplicableonSale
        {
            get
            {
                return _IGSTapplicableonSale;
            }
            set
            {
                if (value != _IGSTapplicableonSale)
                {
                    _IGSTapplicableonSale = value;
                    OnPropertyChanged("IGSTapplicableonSale");
                }
            }
        }
        #endregion

        #endregion

        #region Common Properties


        private string _Comment;
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get
            {
                return _DoctorName;
            }
            set
            {
                if (_DoctorName != value)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _DoctorSpec;
        public string DoctorSpec
        {
            get
            {
                return _DoctorSpec;
            }
            set
            {
                if (_DoctorSpec != value)
                {
                    _DoctorSpec = value;
                    OnPropertyChanged("DoctorSpec");
                }
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

        private bool _PrvStatus = false;
        public bool PrvStatus
        {
            get { return _PrvStatus; }
            set
            {
                if (_PrvStatus != value)
                {
                    _PrvStatus = value;
                    OnPropertyChanged("PrvStatus");
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

        private string _DrugCode;
        public string DrugCode
        {
            get { return _DrugCode; }
            set
            {
                if (_DrugCode != value)
                {
                    _DrugCode = value;
                    OnPropertyChanged("DrugCode");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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

        private double _SaleQuantity;
        public double SaleQuantity
        {
            get { return _SaleQuantity; }
            set
            {
                if (_SaleQuantity != value)
                {
                    _SaleQuantity = value;
                    OnPropertyChanged("SaleQuantity");
                }
            }
        }

        #endregion

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



        //by Anjali........................

        private decimal _SVatPer;
        public decimal SVatPer
        {
            get
            {
                return _SVatPer;
            }
            set
            {
                if (value != _SVatPer)
                {
                    _SVatPer = value;
                    OnPropertyChanged("SVatPer");
                }
            }
        }

        private decimal _SItemVatPer;
        public decimal SItemVatPer
        {
            get
            {
                return _SItemVatPer;
            }
            set
            {
                if (value != _SItemVatPer)
                {
                    _SItemVatPer = value;
                    OnPropertyChanged("SItemVatPer");
                }
            }
        }

        private int _SItemVatType;
        public int SItemVatType
        {
            get
            {
                return _SItemVatType;
            }
            set
            {
                if (value != _SItemVatType)
                {
                    _SItemVatType = value;
                    OnPropertyChanged("SItemVatType");
                }
            }
        }
        private int _SItemVatApplicationOn;
        public int SItemVatApplicationOn
        {
            get
            {
                return _SItemVatApplicationOn;
            }
            set
            {
                if (value != _SItemVatApplicationOn)
                {
                    _SItemVatApplicationOn = value;
                    OnPropertyChanged("SItemVatApplicationOn");
                }
            }
        }

        private int _SItemOtherTaxType;
        public int SItemOtherTaxType
        {
            get
            {
                return _SItemOtherTaxType;
            }
            set
            {
                if (value != _SItemOtherTaxType)
                {
                    _SItemOtherTaxType = value;
                    OnPropertyChanged("SItemVatType");
                }
            }
        }
        private int _SOtherItemApplicationOn;
        public int SOtherItemApplicationOn
        {
            get
            {
                return _SOtherItemApplicationOn;
            }
            set
            {
                if (value != _SOtherItemApplicationOn)
                {
                    _SOtherItemApplicationOn = value;
                    OnPropertyChanged("SItemVatApplicationOn");
                }
            }
        }

        private string _PurchaseUOM;
        public string PurchaseUOM
        {
            get
            {
                return _PurchaseUOM;
            }
            set
            {
                if (value != _PurchaseUOM)
                {
                    _PurchaseUOM = value;
                    OnPropertyChanged("PurchaseUOM");
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
        private string _StockUOM;
        public string StockUOM
        {
            get
            {
                return _StockUOM;
            }
            set
            {
                if (value != _StockUOM)
                {
                    _StockUOM = value;
                    OnPropertyChanged("StockUOM");
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

        private string _PUMString;
        public string PUMString
        {
            get
            {
                return _PUMString;
            }
            set
            {
                if (value != _PUMString)
                {
                    _PUMString = value;
                    OnPropertyChanged("PUMString");
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

        //added by Ashish Z. for Base and Selling UOM.
        private long _BaseUM;
        public long BaseUM
        {
            get
            {
                return _BaseUM;
            }
            set
            {
                if (value != _BaseUM)
                {
                    _BaseUM = value;
                    OnPropertyChanged("BaseUM");
                }
            }
        }

        private long _SellingUM;
        public long SellingUM
        {
            get
            {
                return _SellingUM;
            }
            set
            {
                if (value != _SellingUM)
                {
                    _SellingUM = value;
                    OnPropertyChanged("SellingUM");
                }
            }
        }

        private string _BaseUMString;
        public string BaseUMString
        {
            get
            {
                return _BaseUMString;
            }
            set
            {
                if (value != _BaseUMString)
                {
                    _BaseUMString = value;
                    OnPropertyChanged("BaseUMString");
                }
            }
        }
        private float _StockingCF;
        public float StockingCF
        {
            get
            {
                return _StockingCF;
            }
            set
            {
                if (value != _StockingCF)
                {
                    _StockingCF = value;
                    OnPropertyChanged("StockingCF");
                }
            }
        }
        private float _SellingCF;
        public float SellingCF
        {
            get
            {
                return _SellingCF;
            }
            set
            {
                if (value != _SellingCF)
                {
                    _SellingCF = value;
                    OnPropertyChanged("SellingCF");
                }
            }
        }
        private string _SellingUMString;
        public string SellingUMString
        {
            get
            {
                return _SellingUMString;
            }
            set
            {
                if (value != _SellingUMString)
                {
                    _SellingUMString = value;
                    OnPropertyChanged("SellingUMString");
                }
            }
        }

        //added by neena
        private bool _ArtEnabled;
        public bool ArtEnabled
        {
            get
            {
                return _ArtEnabled;
            }
            set
            {
                if (value != _ArtEnabled)
                {
                    _ArtEnabled = value;
                    OnPropertyChanged("ArtEnabled");
                }
            }
        }

        private long _DrugSource;
        public long DrugSource
        {
            get
            {
                return _DrugSource;
            }
            set
            {
                if (value != _DrugSource)
                {
                    _DrugSource = value;
                    OnPropertyChanged("DrugSource");
                }
            }
        }

        MasterListItem _SelectedDrugSource = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedDrugSource
        {
            get
            {
                return _SelectedDrugSource;
            }
            set
            {
                if (value != _SelectedDrugSource)
                {
                    _SelectedDrugSource = value;
                    OnPropertyChanged("SelectedDrugSource");
                }
            }
        }


        List<MasterListItem> _DrugSourceList = new List<MasterListItem>();
        public List<MasterListItem> DrugSourceList
        {
            get
            {
                return _DrugSourceList;
            }
            set
            {
                if (value != _DrugSourceList)
                {
                    _DrugSourceList = value;
                }
            }

        }

        private long _PlanTherapyId;
        public long PlanTherapyId
        {
            get
            {
                return _PlanTherapyId;
            }
            set
            {
                if (value != _PlanTherapyId)
                {
                    _PlanTherapyId = value;
                    OnPropertyChanged("PlanTherapyId");
                }
            }
        }

        private long _PlanTherapyUnitId;
        public long PlanTherapyUnitId
        {
            get
            {
                return _PlanTherapyUnitId;
            }
            set
            {
                if (value != _PlanTherapyUnitId)
                {
                    _PlanTherapyUnitId = value;
                    OnPropertyChanged("PlanTherapyUnitId");
                }
            }
        }

        private DateTime _Date;
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

        //
        //....................................
    }

    //added by neena
    public class clsPatientPrescriptionReasonOncounterSaleVO : IValueObject, INotifyPropertyChanged
    {

        #region Common Properties
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

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

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

        private long _PrescriptionUnitID;
        public long PrescriptionUnitID
        {
            get { return _PrescriptionUnitID; }
            set
            {
                if (_PrescriptionUnitID != value)
                {
                    _PrescriptionUnitID = value;
                    OnPropertyChanged("PrescriptionUnitID");
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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



    }
    //


    public class clsPatientPrescriptionDetailManuallyVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _GenericName;
        public string GenericName
        {
            get { return _GenericName; }
            set
            {
                if (_GenericName != value)
                {
                    _GenericName = value;
                    OnPropertyChanged("GenericName");
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


        private string _Instruction;
        public string Instruction
        {
            get { return _Instruction; }
            set
            {
                if (_Instruction != value)
                {
                    _Instruction = value;
                    OnPropertyChanged("Instruction");
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

        private string _Days;
        public string Days
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

        private string _Quantity;
        public string Quantity
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

        private bool? _IsPrint;
        public bool? IsPrint
        {
            get { return _IsPrint; }
            set
            {
                if (_IsPrint != value)
                {
                    _IsPrint = value;
                    OnPropertyChanged("IsPrint");
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

        private long _SrNO;
        public long SrNO
        {
            get { return _SrNO; }
            set
            {
                if (_SrNO != value)
                {
                    _SrNO = value;
                    OnPropertyChanged("SrNO");
                }
            }
        }
        #endregion

        #region Common Properties


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
    }

    public class clsDoctorSuggestedServiceDetailVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private clsEMRReferralLetterVO _ReferralLetter = null;
        public clsEMRReferralLetterVO ReferralLetter
        {
            get { return _ReferralLetter; }
            set { _ReferralLetter = value; }
        }

        private string _ReferalSpecializationCode;
        public string ReferalSpecializationCode
        {
            get { return _ReferalSpecializationCode; }
            set
            {
                if (_ReferalSpecializationCode != value)
                {
                    _ReferalSpecializationCode = value;
                    OnPropertyChanged("ReferalSpecializationCode");
                }
            }
        }

        private string _ReferalSpecialization;
        public string ReferalSpecialization
        {
            get { return _ReferalSpecialization; }
            set
            {
                if (_ReferalSpecialization != value)
                {
                    _ReferalSpecialization = value;
                    OnPropertyChanged("ReferalSpecialization");
                }
            }
        }

        private string _ReferalDoctor = string.Empty;
        public string ReferalDoctor
        {
            get
            {
                return _ReferalDoctor;
            }
            set
            {
                _ReferalDoctor = value;
                OnPropertyChanged("ReferalDoctor");
            }
        }

        private string _ReferalDoctorCode = string.Empty;
        public string ReferalDoctorCode
        {
            get
            {
                return _ReferalDoctorCode;
            }
            set
            {
                _ReferalDoctorCode = value;
                OnPropertyChanged("ReferalDoctorCode");
            }
        }

        private string _Specialization;
        public string Specialization
        {
            get { return _Specialization; }
            set
            {
                if (_Specialization != value)
                {
                    _Specialization = value;
                    OnPropertyChanged("Specialization");
                }
            }
        }

        private long _SpecializationId;
        public long SpecializationId
        {
            get { return _SpecializationId; }
            set
            {
                if (_SpecializationId != value)
                {
                    _SpecializationId = value;
                    OnPropertyChanged("SpecializationId");
                }
            }
        }

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

        private long _DeptID;
        public long DeptID
        {
            get { return _DeptID; }
            set
            {
                if (_DeptID != value)
                {
                    _DeptID = value;
                    OnPropertyChanged("DeptID");
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
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

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

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }

        private double _ServiceRate;
        public double ServiceRate
        {
            get { return _ServiceRate; }
            set
            {
                if (_ServiceRate != value)
                {
                    _ServiceRate = value;
                    OnPropertyChanged("ServiceRate");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (_ServiceCode != value)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        private bool _IsRefferal;
        public bool IsRefferal
        {
            get { return _IsRefferal; }
            set
            {
                if (_IsRefferal != value)
                {
                    _IsRefferal = value;
                    OnPropertyChanged("IsRefferal");
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
        public string Datetime { get; set; }
        private DateTime _Date;
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


        List<MasterListItem> _Doctor = new List<MasterListItem>();
        public List<MasterListItem> Doctor
        {
            get
            {
                return _Doctor;
            }
            set
            {
                if (value != _Doctor)
                {
                    _Doctor = value;
                }
            }

        }


        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (_DoctorName != value)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _DoctorCode;
        public string DoctorCode
        {
            get { return _DoctorCode; }
            set
            {
                if (_DoctorCode != value)
                {
                    _DoctorCode = value;
                    OnPropertyChanged("DoctorCode");
                }
            }
        }

        private string _PrintFlag;
        public string PrintFlag
        {
            get { return _PrintFlag; }
            set
            {
                if (_PrintFlag != value)
                {
                    _PrintFlag = value;
                    OnPropertyChanged("PrintFlag");
                }
            }
        }
        //

        private string _DepertmentName;
        public string DepartmentName
        {
            get { return _DepertmentName; }
            set
            {
                if (_DepertmentName != value)
                {
                    _DepertmentName = value;
                    OnPropertyChanged("DepertmentName");
                }
            }
        }

        private string _DepertmentCode;
        public string DepertmentCode
        {
            get { return _DepertmentCode; }
            set
            {
                if (_DepertmentCode != value)
                {
                    _DepertmentCode = value;
                    OnPropertyChanged("DepertmentCode");
                }
            }

        }


        MasterListItem _SelectedDoctor = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedDoctor
        {
            get
            {
                return _SelectedDoctor;
            }
            set
            {
                if (value != _SelectedDoctor)
                {
                    _SelectedDoctor = value;
                    OnPropertyChanged("SelectedDoctor");
                }
            }
        }

        List<MasterListItem> _SpName = new List<MasterListItem>();
        public List<MasterListItem> SpecilizationName
        {
            get
            {
                return _SpName;
            }
            set
            {
                if (value != _SpName)
                {
                    _SpName = value;
                }
            }

        }

        MasterListItem _SelectedSpecilization = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedSpecilization
        {
            get
            {
                return _SelectedSpecilization;
            }
            set
            {
                if (value != _SelectedSpecilization)
                {
                    _SelectedSpecilization = value;
                    OnPropertyChanged("SelectedSpecilization");
                }
            }
        }

        List<MasterListItem> _SubpName = new List<MasterListItem>();
        public List<MasterListItem> SubSpecilizationName
        {
            get
            {
                return _SubpName;
            }
            set
            {
                if (value != _SubpName)
                {
                    _SubpName = value;
                }
            }
        }

        MasterListItem _SelectedSubSpecilization = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedSubSpecilization
        {
            get
            {
                return _SelectedSubSpecilization;
            }
            set
            {
                if (value != _SelectedSubSpecilization)
                {
                    _SelectedSubSpecilization = value;
                    OnPropertyChanged("SelectedSubSpecilization");
                }
            }


        }


        List<clsServiceMasterVO> _Service = new List<clsServiceMasterVO>();
        public List<clsServiceMasterVO> Service
        {
            get
            {
                return _Service;
            }
            set
            {
                if (value != _Service)
                {
                    _Service = value;
                }
            }

        }

        clsServiceMasterVO _SelectedService = new clsServiceMasterVO();
        public clsServiceMasterVO SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                if (value != _SelectedService)
                {
                    _SelectedService = value;
                    OnPropertyChanged("SelectedService");
                }
            }
        }

        private List<MasterListItem> _Priority;
        public List<MasterListItem> Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                if (value != _Priority)
                {
                    _Priority = value;
                }
            }

        }

        private MasterListItem _SelectedPriority;
        public MasterListItem SelectedPriority
        {
            get
            {
                return _SelectedPriority;
            }
            set
            {
                if (value != _SelectedPriority)
                {
                    _SelectedPriority = value;
                    OnPropertyChanged("SelectedPriority");
                }
            }
        }

        public int PriorityIndex { get; set; }
        #endregion

        #region Common Properties

        private string _GroupName = String.Empty;
        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                if (_GroupName != value)
                {
                    _GroupName = value;
                    OnPropertyChanged("GroupName");
                }
            }
        }

        private string _SpecializationCode;
        public string SpecializationCode
        {
            get { return _SpecializationCode; }
            set
            {
                if (_SpecializationCode != value)
                {
                    _SpecializationCode = value;
                    OnPropertyChanged("SpecializationCode");
                }
            }
        }

        private string _ServiceType;
        public string ServiceType
        {
            get { return _ServiceType; }
            set
            {
                if (_ServiceType != value)
                {
                    _ServiceType = value;
                    OnPropertyChanged("ServiceType");
                }
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsEMRVitalsVO : IValueObject, INotifyPropertyChanged
    {
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

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }

        }


        private DateTime? _Time;
        public DateTime? Time
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

        private long _PatientVitalID;
        public long PatientVitalID
        {
            get { return _PatientVitalID; }
            set
            {
                if (_PatientVitalID != value)
                {
                    _PatientVitalID = value;
                    OnPropertyChanged("PatientVitalID");
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


        private double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }

        }

        private double _MinValue;
        public double MinValue
        {
            get { return _MinValue; }
            set
            {
                if (_MinValue != value)
                {
                    _MinValue = value;
                    OnPropertyChanged("MinValue");
                }
            }

        }

        private double _MaxValue;
        public double MaxValue
        {
            get { return _MaxValue; }
            set
            {
                if (_MaxValue != value)
                {
                    _MaxValue = value;
                    OnPropertyChanged("MaxValue");
                }

            }
        }



        private double _DefaultValue;
        public double DefaultValue
        {
            get { return _DefaultValue; }
            set
            {
                if (_DefaultValue != value)
                {
                    _DefaultValue = value;
                    OnPropertyChanged("DefaultValue");
                }

            }

        }
        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }

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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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

        private string _ValueDescription;
        public string ValueDescription
        {
            get { return _ValueDescription; }
            set
            {
                if (_ValueDescription != value)
                {
                    _ValueDescription = value;
                    OnPropertyChanged("ValueDescription");
                }
            }

        }
    }

    public class clsEMRDiagnosisVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (_ServiceCode != value)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        public Boolean IsICOPIMHead { get; set; }

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

        private string _Class;
        public string Class
        {
            get { return _Class; }
            set
            {
                if (_Class != value)
                {
                    _Class = value;
                    OnPropertyChanged("Class");
                }
            }
        }

        private string _Diagnosis;
        public string Diagnosis
        {
            get { return _Diagnosis; }
            set
            {
                if (_Diagnosis != value)
                {
                    _Diagnosis = value;
                    OnPropertyChanged("Diagnosis");
                }
            }

        }


        private long _DiagnosisTypeId;
        public long DiagnosisTypeId
        {
            get { return _DiagnosisTypeId; }
            set
            {
                if (_DiagnosisTypeId != value)
                {
                    _DiagnosisTypeId = value;
                    OnPropertyChanged("DiagnosisTypeId");
                }
            }

        }

        private string _DiagnosisType;
        public string DiagnosisType
        {
            get { return _DiagnosisType; }
            set
            {
                if (_DiagnosisType != value)
                {
                    _DiagnosisType = value;
                    OnPropertyChanged("DiagnosisType");
                }
            }

        }

        private long _ICDId;
        public long ICDId
        {
            get { return _ICDId; }
            set
            {
                if (_ICDId != value)
                {
                    _ICDId = value;
                    OnPropertyChanged("ICDId");
                }
            }

        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }

        }

        private bool _SelectDiagnosis;
        public bool SelectDiagnosis
        {
            get { return _SelectDiagnosis; }
            set
            {
                if (value != _SelectDiagnosis)
                {
                    _SelectDiagnosis = value;
                    OnPropertyChanged("SelectDiagnosis");
                }
            }
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

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }

        }

        private string _TemplateName;
        public string TemplateName
        {
            get { return _TemplateName; }
            set
            {
                if (_TemplateName != value)
                {
                    _TemplateName = value;
                    OnPropertyChanged("TemplateName");
                }
            }

        }

        private string _DTD;
        public string DTD
        {
            get { return _DTD; }
            set
            {
                if (_DTD != value)
                {
                    _DTD = value;
                    OnPropertyChanged("DTD");
                }
            }
        }

        private string _Categori;
        public string Categori
        {
            get { return _Categori; }
            set
            {
                if (_Categori != value)
                {
                    _Categori = value;
                    OnPropertyChanged("Categori");
                }
            }

        }

        private double _ServiceRate;
        public double ServiceRate
        {
            get { return _ServiceRate; }
            set
            {
                if (_ServiceRate != value)
                {
                    _ServiceRate = value;
                    OnPropertyChanged("ServiceRate");
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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

    }

    public class clsEMRAddDiagnosisVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _Class;
        public string Class
        {
            get { return _Class; }
            set
            {
                if (_Class != value)
                {
                    _Class = value;
                    OnPropertyChanged("Class");
                }
            }
        }

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (_ServiceCode != value)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        public string DoctorName { get; set; }
        public string DocSpec { get; set; }
        public Boolean IsICOPIMHead { get; set; }

        public Boolean IsICD9 { get; set; }

        public string Datetime { get; set; }

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

        private long _DiagnosisId;
        public long DiagnosisId
        {
            get { return _DiagnosisId; }
            set
            {
                if (_DiagnosisId != value)
                {
                    _DiagnosisId = value;
                    OnPropertyChanged("DiagnosisId");
                }
            }

        }

        private string _Diagnosis;
        public string Diagnosis
        {
            get { return _Diagnosis; }
            set
            {
                if (_Diagnosis != value)
                {
                    _Diagnosis = value;
                    OnPropertyChanged("Diagnosis");
                }
            }

        }

        private long _DiagnosisTypeId;
        public long DiagnosisTypeId
        {
            get { return _DiagnosisTypeId; }
            set
            {
                if (_DiagnosisTypeId != value)
                {
                    _DiagnosisTypeId = value;
                    OnPropertyChanged("DiagnosisTypeId");
                }
            }

        }

        private string _DiagnosisType;
        public string DiagnosisType
        {
            get { return _DiagnosisType; }
            set
            {
                if (_DiagnosisType != value)
                {
                    _DiagnosisType = value;
                    OnPropertyChanged("DiagnosisType");
                }
            }

        }

        private long _ICDId;
        public long ICDId
        {
            get { return _ICDId; }
            set
            {
                if (_ICDId != value)
                {
                    _ICDId = value;
                    OnPropertyChanged("ICDId");
                }
            }

        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }

        }

        private bool _SelectDiagnosis;
        public bool SelectDiagnosis
        {
            get { return _SelectDiagnosis; }
            set
            {
                if (value != _SelectDiagnosis)
                {
                    _SelectDiagnosis = value;
                    OnPropertyChanged("SelectDiagnosis");
                }
            }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (value != _IsEnabled)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        private bool _ISARTENABLED;
        public bool ISARTENABLED
        {
            get { return _ISARTENABLED; }
            set
            {
                if (value != _ISARTENABLED)
                {
                    _ISARTENABLED = value;
                    OnPropertyChanged("ISARTENABLED");
                }
            }
        }

        private bool _IsEnabledArt;
        public bool IsEnabledArt
        {
            get { return _IsEnabledArt; }
            set
            {
                if (value != _IsEnabledArt)
                {
                    _IsEnabledArt = value;
                    OnPropertyChanged("IsEnabledArt");
                }
            }
        }


        private bool _ARTENABLE;
        public bool ARTENABLE
        {
            get { return _ARTENABLE; }
            set
            {
                if (value != _ARTENABLE)
                {
                    _ARTENABLE = value;
                    OnPropertyChanged("ARTENABLE");
                }
            }
        }


        private bool _PlanTreatmentEnabled;
        public bool PlanTreatmentEnabled
        {
            get { return _PlanTreatmentEnabled; }
            set
            {
                if (value != _PlanTreatmentEnabled)
                {
                    _PlanTreatmentEnabled = value;
                    OnPropertyChanged("PlanTreatmentEnabled");
                }
            }
        }


        public long VisitID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        //added by neena
        private bool _IsSurrogate;
        public bool IsSurrogate
        {
            get { return _IsSurrogate; }
            set
            {
                if (_IsSurrogate != value)
                {
                    _IsSurrogate = value;
                    OnPropertyChanged("IsSurrogate");
                }
            }
        }

        private bool _IsSelectSurrogate;
        public bool IsSelectSurrogate
        {
            get { return _IsSelectSurrogate; }
            set
            {
                if (_IsSelectSurrogate != value)
                {
                    _IsSelectSurrogate = value;
                    OnPropertyChanged("IsSelectSurrogate");
                }
            }
        }

        private string _SurrogateMRNO;
        public string SurrogateMRNO
        {
            get { return _SurrogateMRNO; }
            set
            {
                if (_SurrogateMRNO != value)
                {
                    _SurrogateMRNO = value;
                    OnPropertyChanged("SurrogateMRNO");
                }
            }
        }

        private bool _IsSelectPatient;
        public bool IsSelectPatient
        {
            get { return _IsSelectPatient; }
            set
            {
                if (_IsSelectPatient != value)
                {
                    _IsSelectPatient = value;
                    OnPropertyChanged("IsSelectPatient");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _MRNO;
        public string MRNO
        {
            get { return _MRNO; }
            set
            {
                if (_MRNO != value)
                {
                    _MRNO = value;
                    OnPropertyChanged("MRNO");
                }
            }
        }

        private string _CoupleMRNO = "";
        public string CoupleMRNO { get; set; }

        //

        // Added by Rajshree for primary diagnosis on 18th july 2013
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private bool _IsDeleted;
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                if (_IsDeleted != value)
                {
                    _IsDeleted = value;
                    OnPropertyChanged("IsDeleted");
                }
            }
        }

        private DateTime _Date;
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

        // end by Rajshree

        //added by neena
        private bool _IsDonorCycle;
        public bool IsDonorCycle
        {
            get { return _IsDonorCycle; }
            set
            {
                if (_IsDonorCycle != value)
                {
                    _IsDonorCycle = value;
                    OnPropertyChanged("IsDonorCycle");
                }
            }
        }

        private long _DonorID;
        public long DonorID
        {
            get { return _DonorID; }
            set
            {
                if (_DonorID != value)
                {
                    _DonorID = value;
                    OnPropertyChanged("DonorID");
                }
            }
        }

        private long _DonarUnitID;
        public long DonarUnitID
        {
            get { return _DonarUnitID; }
            set
            {
                if (_DonarUnitID != value)
                {
                    _DonarUnitID = value;
                    OnPropertyChanged("DonarUnitID");
                }
            }
        }


        private bool _ArtEnabled;
        public bool ArtEnabled
        {
            get { return _ArtEnabled; }
            set
            {
                if (_ArtEnabled != value)
                {
                    _ArtEnabled = value;
                    OnPropertyChanged("ArtEnabled");
                }
            }
        }

        private bool _PACEnabled;
        public bool PACEnabled
        {
            get { return _PACEnabled; }
            set
            {
                if (_PACEnabled != value)
                {
                    _PACEnabled = value;
                    OnPropertyChanged("PACEnabled");
                }
            }
        }

        private bool _DonorEnabled;
        public bool DonorEnabled
        {
            get { return _DonorEnabled; }
            set
            {
                if (_DonorEnabled != value)
                {
                    _DonorEnabled = value;
                    OnPropertyChanged("DonorEnabled");
                }
            }
        }



        private bool _PACBilledEnabled;
        public bool PACBilledEnabled
        {
            get { return _PACBilledEnabled; }
            set
            {
                if (_PACBilledEnabled != value)
                {
                    _PACBilledEnabled = value;
                    OnPropertyChanged("PACBilledEnabled");
                }
            }
        }

        private bool _BilledEnabled;
        public bool BilledEnabled
        {
            get { return _BilledEnabled; }
            set
            {
                if (_BilledEnabled != value)
                {
                    _BilledEnabled = value;
                    OnPropertyChanged("BilledEnabled");
                }
            }
        }

        private bool _IsClosedEnabled;
        public bool IsClosedEnabled
        {
            get { return _IsClosedEnabled; }
            set
            {
                if (_IsClosedEnabled != value)
                {
                    _IsClosedEnabled = value;
                    OnPropertyChanged("IsClosedEnabled");
                }
            }
        }

        private List<MasterListItem> _PlanTreatmentList = new List<MasterListItem>();
        public List<MasterListItem> PlanTreatmentList
        {
            get
            {
                return _PlanTreatmentList;
            }
            set
            {
                _PlanTreatmentList = value;
            }
        }

        private MasterListItem _SelectedPlanTreatmentId = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPlanTreatmentId
        {
            get
            {
                return _SelectedPlanTreatmentId;
            }
            set
            {
                _SelectedPlanTreatmentId = value;
            }
        }

        private long _PlanTreatmentId;
        public long PlanTreatmentId
        {
            get { return _PlanTreatmentId; }
            set
            {
                if (_PlanTreatmentId != value)
                {
                    _PlanTreatmentId = value;
                    OnPropertyChanged("PlanTreatmentId");
                }
            }
        }

        private List<MasterListItem> _Priority = new List<MasterListItem>();
        public List<MasterListItem> Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                _Priority = value;
            }
        }

        private MasterListItem _SelectedPriority = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPriority
        {
            get
            {
                return _SelectedPriority;
            }
            set
            {
                _SelectedPriority = value;
            }
        }

        private long _PriorityId;
        public long PriorityId
        {
            get { return _PriorityId; }
            set
            {
                if (_PriorityId != value)
                {
                    _PriorityId = value;
                    OnPropertyChanged("PriorityId");
                }
            }
        }

        private bool _IsArtStatus;
        public bool IsArtStatus
        {
            get { return _IsArtStatus; }
            set
            {
                if (_IsArtStatus != value)
                {
                    _IsArtStatus = value;
                    OnPropertyChanged("IsArtStatus");
                }
            }
        }

        private bool _IsPACEn;
        public bool IsPACEn
        {
            get { return _IsPACEn; }
            set
            {
                if (_IsPACEn != value)
                {
                    _IsPACEn = value;
                    OnPropertyChanged("IsPACEn");
                }
            }
        }

        private long _PlanTherapyId;
        public long PlanTherapyId
        {
            get { return _PlanTherapyId; }
            set
            {
                if (_PlanTherapyId != value)
                {
                    _PlanTherapyId = value;
                    OnPropertyChanged("PlanTherapyId");
                }
            }
        }

        private long _PlanTherapyUnitId;
        public long PlanTherapyUnitId
        {
            get { return _PlanTherapyUnitId; }
            set
            {
                if (_PlanTherapyUnitId != value)
                {
                    _PlanTherapyUnitId = value;
                    OnPropertyChanged("PlanTherapyUnitId");
                }
            }
        }

        //

        #endregion

        #region Common Properties

        public List<MasterListItem> DiagnosisTypeList { get; set; }

        private MasterListItem _SelectedDiagnosisType = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedDiagnosisType
        {
            get { return _SelectedDiagnosisType; }
            set
            {
                if (_SelectedDiagnosisType != value)
                {
                    _SelectedDiagnosisType = value;
                    OnPropertyChanged("SelectedDiagnosisType");
                }
            }
        }

        private string _DTD;
        public string DTD
        {
            get { return _DTD; }
            set
            {
                if (_DTD != value)
                {
                    _DTD = value;
                    OnPropertyChanged("DTD");
                }
            }
        }

        public string Image { get; set; }

        private long _TemplateID;
        public long TemplateID
        {
            get
            {
                return _TemplateID;
            }
            set
            {
                _TemplateID = value;
                OnPropertyChanged("TemplateID");
            }
        }

        public string TemplateName { get; set; }

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

        private string _Categori;
        public string Categori
        {
            get { return _Categori; }
            set
            {
                if (_Categori != value)
                {
                    _Categori = value;
                    OnPropertyChanged("Categori");
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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

        public List<clsPatientEMRDetailsVO> listPatientEMRDetails { get; set; }

        #endregion

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
    }

    public class clsBPControlVO : IValueObject, INotifyPropertyChanged
    {
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

        private int? _BP1;
        public int? BP1
        {
            get { return _BP1; }
            set
            {
                if (_BP1 != value)
                {
                    _BP1 = value;
                    OnPropertyChanged("BP1");
                }
            }
        }

        private int? _BP2;
        public int? BP2
        {
            get { return _BP2; }
            set
            {
                if (_BP2 != value)
                {
                    _BP2 = value;
                    OnPropertyChanged("BP2");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _PatientEMRDataID;
        public long PatientEMRDataID
        {
            get { return _PatientEMRDataID; }
            set
            {
                if (_PatientEMRDataID != value)
                {
                    _PatientEMRDataID = value;
                    OnPropertyChanged("PatientEMRDataID");
                }
            }
        }

        private string _Doctor;
        public string Doctor
        {
            get { return _Doctor; }
            set
            {
                if (_Doctor != value)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
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

        //private string _Doctor;
        //public string Doctor
        //{
        //    get { return _Doctor; }
        //    set
        //    {
        //        if (_Doctor != value)
        //        {
        //            _Doctor = value;
        //            OnPropertyChanged("Doctor");
        //        }
        //    }
        //}

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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsVisionVO : IValueObject, INotifyPropertyChanged
    {
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

        private decimal? _SPH;
        public decimal? SPH
        {
            get { return _SPH; }
            set
            {
                if (_SPH != value)
                {
                    _SPH = value;
                    OnPropertyChanged("SPH");
                }
            }
        }

        private decimal? _CYL;
        public decimal? CYL
        {
            get { return _CYL; }
            set
            {
                if (_CYL != value)
                {
                    _CYL = value;
                    OnPropertyChanged("CYL");
                }
            }
        }

        private decimal? _Axis;
        public decimal? Axis
        {
            get { return _Axis; }
            set
            {
                if (_Axis != value)
                {
                    _Axis = value;
                    OnPropertyChanged("Axis");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _PatientEMRDataID;
        public long PatientEMRDataID
        {
            get { return _PatientEMRDataID; }
            set
            {
                if (_PatientEMRDataID != value)
                {
                    _PatientEMRDataID = value;
                    OnPropertyChanged("PatientEMRDataID");
                }
            }
        }

        private string _Doctor;
        public string Doctor
        {
            get { return _Doctor; }
            set
            {
                if (_Doctor != value)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsGlassPowerVO : IValueObject, INotifyPropertyChanged
    {
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

        private decimal? _SPH1;
        public decimal? SPH1
        {
            get { return _SPH1; }
            set
            {
                if (_SPH1 != value)
                {
                    _SPH1 = value;
                    OnPropertyChanged("SPH1");
                }
            }
        }

        private decimal? _CYL1;
        public decimal? CYL1
        {
            get { return _CYL1; }
            set
            {
                if (_CYL1 != value)
                {
                    _CYL1 = value;
                    OnPropertyChanged("CYL1");
                }
            }
        }

        private decimal? _Axis1;
        public decimal? Axis1
        {
            get { return _Axis1; }
            set
            {
                if (_Axis1 != value)
                {
                    _Axis1 = value;
                    OnPropertyChanged("Axis1");
                }
            }
        }

        private decimal? _VA1;
        public decimal? VA1
        {
            get { return _VA1; }
            set
            {
                if (_VA1 != value)
                {
                    _VA1 = value;
                    OnPropertyChanged("VA1");
                }
            }
        }

        private decimal? _SPH2;
        public decimal? SPH2
        {
            get { return _SPH2; }
            set
            {
                if (_SPH2 != value)
                {
                    _SPH2 = value;
                    OnPropertyChanged("SPH2");
                }
            }
        }

        private decimal? _CYL2;
        public decimal? CYL2
        {
            get { return _CYL2; }
            set
            {
                if (_CYL2 != value)
                {
                    _CYL2 = value;
                    OnPropertyChanged("CYL2");
                }
            }
        }

        private decimal? _Axis2;
        public decimal? Axis2
        {
            get { return _Axis2; }
            set
            {
                if (_Axis2 != value)
                {
                    _Axis2 = value;
                    OnPropertyChanged("Axis2");
                }
            }
        }

        private decimal? _VA2;
        public decimal? VA2
        {
            get { return _VA2; }
            set
            {
                if (_VA2 != value)
                {
                    _VA2 = value;
                    OnPropertyChanged("VA2");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _PatientEMRDataID;
        public long PatientEMRDataID
        {
            get { return _PatientEMRDataID; }
            set
            {
                if (_PatientEMRDataID != value)
                {
                    _PatientEMRDataID = value;
                    OnPropertyChanged("PatientEMRDataID");
                }
            }
        }

        private string _Doctor;
        public string Doctor
        {
            get { return _Doctor; }
            set
            {
                if (_Doctor != value)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsEyeControlVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _Eye1;
        public string Eye1
        {
            get { return _Eye1; }
            set
            {
                if (_Eye1 != value)
                {
                    _Eye1 = value;
                    OnPropertyChanged("_Eye1");
                }
            }
        }

        private string _RightSPH;
        public string RightSPH
        {
            get { return _RightSPH; }
            set
            {
                if (_RightSPH != value)
                {
                    _RightSPH = value;
                    OnPropertyChanged("RightSPH");
                }
            }
        }
        private string _RightCYL;
        public string RightCYL
        {
            get { return _RightCYL; }
            set
            {
                if (_RightCYL != value)
                {
                    _RightCYL = value;
                    OnPropertyChanged("RightCYL");
                }
            }
        }

        private string _RightAXIS;
        public string RightAXIS
        {
            get { return _RightAXIS; }
            set
            {
                if (_RightAXIS != value)
                {
                    _RightAXIS = value;
                    OnPropertyChanged("RightAXIS");
                }
            }
        }



        private string _LeftSPH;
        public string LeftSPH
        {
            get { return _LeftSPH; }
            set
            {
                if (_LeftSPH != value)
                {
                    _LeftSPH = value;
                    OnPropertyChanged("LeftSPH");
                }
            }
        }
        private string _LeftCYL;
        public string LeftCYL
        {
            get { return _LeftCYL; }
            set
            {
                if (_LeftCYL != value)
                {
                    _LeftCYL = value;
                    OnPropertyChanged("LeftCYL");
                }
            }
        }

        private string _LeftAXIS;
        public string LeftAXIS
        {
            get { return _LeftAXIS; }
            set
            {
                if (_LeftAXIS != value)
                {
                    _LeftAXIS = value;
                    OnPropertyChanged("LeftAXIS");
                }
            }
        }
        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _PatientEMRDataID;
        public long PatientEMRDataID
        {
            get { return _PatientEMRDataID; }
            set
            {
                if (_PatientEMRDataID != value)
                {
                    _PatientEMRDataID = value;
                    OnPropertyChanged("PatientEMRDataID");
                }
            }
        }

        private string _Doctor;
        public string Doctor
        {
            get { return _Doctor; }
            set
            {
                if (_Doctor != value)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
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

        //private string _Doctor;
        //public string Doctor
        //{
        //    get { return _Doctor; }
        //    set
        //    {
        //        if (_Doctor != value)
        //        {
        //            _Doctor = value;
        //            OnPropertyChanged("Doctor");
        //        }
        //    }
        //}

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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsEyeSubjectiveCorrectnControlVO : IValueObject, INotifyPropertyChanged
    {
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

        private string _Eye1;
        public string Eye1
        {
            get { return _Eye1; }
            set
            {
                if (_Eye1 != value)
                {
                    _Eye1 = value;
                    OnPropertyChanged("Eye1");
                }
            }
        }

        private string _Spherical;
        public string Spherical
        {
            get { return _Spherical; }
            set
            {
                if (_Spherical != value)
                {
                    _Spherical = value;
                    OnPropertyChanged("Spherical");
                }
            }
        }
        private string _Cylindrical;
        public string Cylindrical
        {
            get { return _Cylindrical; }
            set
            {
                if (_Cylindrical != value)
                {
                    _Cylindrical = value;
                    OnPropertyChanged("Cylindrical");
                }
            }
        }

        private string _Axis;
        public string Axis
        {
            get { return _Axis; }
            set
            {
                if (_Axis != value)
                {
                    _Axis = value;
                    OnPropertyChanged("_Axis");
                }
            }
        }

        private string _Vision;
        public string Vision
        {
            get { return _Vision; }
            set
            {
                if (_Vision != value)
                {
                    _Vision = value;
                    OnPropertyChanged("Vision");
                }
            }
        }
        private string _Add;
        public string Add
        {
            get { return _Add; }
            set
            {
                if (_Add != value)
                {
                    _Add = value;
                    OnPropertyChanged("Add");
                }
            }
        }
        private string _NV;
        public string NV
        {
            get { return _NV; }
            set
            {
                if (_NV != value)
                {
                    _NV = value;
                    OnPropertyChanged("NV");
                }
            }
        }
        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {
                if (_VisitID != value)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {
                if (_PatientID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("PatientID");
                }
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {
                if (_PatientUnitID != value)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        private long _PatientEMRDataID;
        public long PatientEMRDataID
        {
            get { return _PatientEMRDataID; }
            set
            {
                if (_PatientEMRDataID != value)
                {
                    _PatientEMRDataID = value;
                    OnPropertyChanged("PatientEMRDataID");
                }
            }
        }

        private string _Doctor;
        public string Doctor
        {
            get { return _Doctor; }
            set
            {
                if (_Doctor != value)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
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

        //private string _Doctor;
        //public string Doctor
        //{
        //    get { return _Doctor; }
        //    set
        //    {
        //        if (_Doctor != value)
        //        {
        //            _Doctor = value;
        //            OnPropertyChanged("Doctor");
        //        }
        //    }
        //}

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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsEMRAllergiesVO : IValueObject, INotifyPropertyChanged
    {

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

        public DateTime Date { get; set; }
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

        private Int16 _AllergyTypeID;
        public Int16 AllergyTypeID
        {
            get { return _AllergyTypeID; }
            set
            {
                if (value != _AllergyTypeID)
                {
                    _AllergyTypeID = value;
                    OnPropertyChanged("AllergyTypeID");
                }
            }
        }

        private string _Allergy;
        public string Allergy
        {
            get
            {
                return _Allergy;
            }
            set
            {
                _Allergy = value;
                OnPropertyChanged("Allergy");
            }
        }
        private string _FoodAllergy;
        public string FoodAllergy
        {
            get
            {
                return _FoodAllergy;
            }
            set
            {
                _FoodAllergy = value;
                OnPropertyChanged("FoodAllergy");
            }
        }
        private string _DrugAllergy;
        public string DrugAllergy
        {
            get
            {
                return _DrugAllergy;
            }
            set
            {
                _DrugAllergy = value;
                OnPropertyChanged("DrugAllergy");
            }
        }
        private string _OtherAllergy;
        public string OtherAllergy
        {
            get
            {
                return _OtherAllergy;
            }
            set
            {
                _OtherAllergy = value;
                OnPropertyChanged("OtherAllergy");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

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
    }

    public class clsEMRChiefComplaintsVO : IValueObject, INotifyPropertyChanged
    {

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

        public DateTime Date { get; set; }
        private string _ChiefComplaints;
        public string ChiefComplaints
        {
            get { return _ChiefComplaints; }
            set
            {
                if (_ChiefComplaints != value)
                {
                    _ChiefComplaints = value;
                    OnPropertyChanged("ChiefComplaints");
                }
            }
        }
        private string _AssChiefComplaints;
        public string AssChiefComplaints
        {
            get { return _AssChiefComplaints; }
            set
            {
                if (_AssChiefComplaints != value)
                {
                    _AssChiefComplaints = value;
                    OnPropertyChanged("AssChiefComplaints");
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
        #endregion

        #region INotifyPropertyChanged Members

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
    }

    public class clsGetServiceBySelectedDiagnosisVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }


        private decimal _ServiceRate;
        public decimal ServiceRate
        {
            get { return _ServiceRate; }
            set
            {
                if (_ServiceRate != value)
                {
                    _ServiceRate = value;
                    OnPropertyChanged("ServiceRate");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
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

        List<MasterListItem> _Doctor = new List<MasterListItem>();
        public List<MasterListItem> Doctor
        {
            get
            {
                return _Doctor;
            }
            set
            {
                if (value != _Doctor)
                {
                    _Doctor = value;
                }
            }

        }

        MasterListItem _SelectedDoctor = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedDoctor
        {
            get
            {
                return _SelectedDoctor;
            }
            set
            {
                if (value != _SelectedDoctor)
                {
                    _SelectedDoctor = value;
                    OnPropertyChanged("SelectedDoctor");
                }
            }


        }


        List<MasterListItem> _SpName = new List<MasterListItem>();
        public List<MasterListItem> SpecilizationName
        {
            get
            {
                return _SpName;
            }
            set
            {
                if (value != _SpName)
                {
                    _SpName = value;
                }
            }

        }

        MasterListItem _SelectedSpecilization = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedSpecilization
        {
            get
            {
                return _SelectedSpecilization;
            }
            set
            {
                if (value != _SelectedSpecilization)
                {
                    _SelectedSpecilization = value;
                    OnPropertyChanged("SelectedSpecilization");
                }
            }


        }


        List<MasterListItem> _SubpName = new List<MasterListItem>();
        public List<MasterListItem> SubSpecilizationName
        {
            get
            {
                return _SubpName;
            }
            set
            {
                if (value != _SubpName)
                {
                    _SubpName = value;
                }
            }

        }

        MasterListItem _SelectedSubSpecilization = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedSubSpecilization
        {
            get
            {
                return _SelectedSubSpecilization;
            }
            set
            {
                if (value != _SelectedSubSpecilization)
                {
                    _SelectedSubSpecilization = value;
                    OnPropertyChanged("SelectedSubSpecilization");
                }
            }


        }


        List<clsServiceMasterVO> _Service = new List<clsServiceMasterVO>();
        public List<clsServiceMasterVO> Service
        {
            get
            {
                return _Service;
            }
            set
            {
                if (value != _Service)
                {
                    _Service = value;
                }
            }

        }

        clsServiceMasterVO _SelectedService = new clsServiceMasterVO();
        public clsServiceMasterVO SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                if (value != _SelectedService)
                {
                    _SelectedService = value;
                    OnPropertyChanged("SelectedService");
                }
            }
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsGetItemBySelectedDiagnosisVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section

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

        private long _RouteID;
        public long RouteID
        {
            get { return _RouteID; }
            set
            {
                if (_RouteID != value)
                {
                    _RouteID = value;
                    OnPropertyChanged("RouteID");
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

        private string _Instruction;
        public string Instruction
        {
            get { return _Instruction; }
            set
            {
                if (_Instruction != value)
                {
                    _Instruction = value;
                    OnPropertyChanged("Instruction");
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

        private double _Quantity;
        public double Quantity
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


        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }


        }


        private bool _NewAdded;
        public bool NewAdded
        {
            get
            {
                return _NewAdded;
            }
            set
            {
                if (value != _NewAdded)
                {
                    _NewAdded = value;
                    OnPropertyChanged("NewAdded");
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
        List<FrequencyMaster> _FrequencyName = new List<FrequencyMaster>();
        public List<FrequencyMaster> FrequencyName
        {
            get
            {
                return _FrequencyName;
            }
            set
            {
                if (value != _FrequencyName)
                {
                    _FrequencyName = value;
                }
            }

        }

        private string _Command;
        public string Command
        {
            get { return _Command; }
            set
            {
                if (_Command != value)
                {
                    _Command = value;
                    OnPropertyChanged("Command");
                }
            }
        }


        FrequencyMaster _SelectedFrequency = new FrequencyMaster { ID = 0, Description = "--Select--" };
        public FrequencyMaster SelectedFrequency
        {
            get
            {
                return _SelectedFrequency;
            }
            set
            {
                if (value != _SelectedFrequency)
                {
                    _SelectedFrequency = value;
                    OnPropertyChanged("SelectedFrequency");
                }
            }


        }


        List<MasterListItem> _RouteName = new List<MasterListItem>();
        public List<MasterListItem> RouteName
        {
            get
            {
                return _RouteName;
            }
            set
            {
                if (value != _RouteName)
                {
                    _RouteName = value;
                }
            }

        }


        MasterListItem _SelectedRoute = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedRoute
        {
            get
            {
                return _SelectedRoute;
            }
            set
            {
                if (value != _SelectedRoute)
                {
                    _SelectedRoute = value;
                    OnPropertyChanged("SelectedRoute");
                }
            }


        }


        List<MasterListItem> _InstructionName = new List<MasterListItem>();
        public List<MasterListItem> InstructionName
        {
            get
            {
                return _InstructionName;
            }
            set
            {
                if (value != _InstructionName)
                {
                    _InstructionName = value;
                }
            }

        }


        MasterListItem _SelectedInstruction = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedInstruction
        {
            get
            {
                return _SelectedInstruction;
            }
            set
            {
                if (value != _SelectedInstruction)
                {
                    _SelectedInstruction = value;
                    OnPropertyChanged("SelectedInstruction");
                }
            }


        }


        List<MasterListItem> _ItemName = new List<MasterListItem>();
        public List<MasterListItem> ItemName
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
                }
            }

        }

        MasterListItem _SelectedItem = new MasterListItem();
        public MasterListItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (value != _SelectedItem)
                {
                    _SelectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }


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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
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

        private string _AddedOn;
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedOn;
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

        private string _UpdatedWindowsLoginName;
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
    }

    public class clsGetDrugForAllergies
    {
        private long _DrugId;
        public long DrugId
        {
            get { return _DrugId; }
            set
            {
                if (_DrugId != value)
                {
                    _DrugId = value;
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
                }
            }
        }

    }


    public class clsEMRFollowNoteVO : IValueObject, INotifyPropertyChanged
    {

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

        public DateTime Date { get; set; }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
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
        #endregion

        #region INotifyPropertyChanged Members

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
    }

}
