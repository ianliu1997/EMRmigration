using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration.CompoundMaster;

namespace PalashDynamics.ValueObjects.NursingStation.EMR
{
    public class clsPrescriptionMasterVO : IValueObject, INotifyPropertyChanged
    {
        public clsPrescriptionMasterVO()
        {

        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _PrescriptionName;
        public string PrescriptionName
        {
            get { return _PrescriptionName; }
            set
            {
                if (value != _PrescriptionName)
                {
                    _PrescriptionName = value;
                    OnPropertyChanged("PrescriptionName");
                }
            }
        }

        private string _IPDOPDNO;
        public string IPDOPDNO
        {
            get { return _IPDOPDNO; }
            set
            {
                if (value != _IPDOPDNO)
                {
                    _IPDOPDNO = value;
                    OnPropertyChanged("IPDOPDNO");
                }
            }
        }

        private long _Priority;
        public long Priority
        {
            get { return _Priority; }
            set
            {
                if (value != _Priority)
                {
                    _Priority = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        private string _PriorityName;
        public string PriorityName
        {
            get { return _PriorityName; }
            set
            {
                if (value != _PriorityName)
                {
                    _PriorityName = value;
                    OnPropertyChanged("PriorityName");
                }
            }
        }

        private string _IPDOPD;
        public string IPDOPD
        {
            get { return _IPDOPD; }
            set
            {
                if (value != _IPDOPD)
                {
                    _IPDOPD = value;
                    OnPropertyChanged("IPDOPD");
                }
            }
        }

        private string _strOPD_IPD;
        public string strOPD_IPD
        {
            get { return _strOPD_IPD; }
            set
            {
                if (value != _strOPD_IPD)
                {
                    _strOPD_IPD = value;
                    OnPropertyChanged("strOPD_IPD");
                }
            }
        }

        private long _Opd_Ipd_Id;
        public long Opd_Ipd_Id
        {
            get { return _Opd_Ipd_Id; }
            set
            {
                if (value != _Opd_Ipd_Id)
                {
                    _Opd_Ipd_Id = value;
                    OnPropertyChanged("Opd_Ipd_Id");
                }
            }
        }

        private long _Opd_Ipd_UnitID;
        public long Opd_Ipd_UnitID
        {
            get { return _Opd_Ipd_UnitID; }
            set
            {
                if (value != _Opd_Ipd_UnitID)
                {
                    _Opd_Ipd_UnitID = value;
                    OnPropertyChanged("Opd_Ipd_UnitID");
                }
            }
        }


        private Int16 _OPD_IPD;
        public Int16 OPD_IPD
        {
            get { return _OPD_IPD; }
            set
            {
                if (value != _OPD_IPD)
                {
                    _OPD_IPD = value;
                    OnPropertyChanged("OPD_IPD");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
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
        private DateTime? _FollowUpDate;
        public DateTime? FollowUpDate
        {
            get { return _FollowUpDate; }
            set
            {
                if (value != _FollowUpDate)
                {
                    _FollowUpDate = value;
                    OnPropertyChanged("FollowUpDate");
                }
            }
        }
        private DateTime? _Time;
        public DateTime? Time
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

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (value != _DoctorID)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }
        private string _FollowUpAdvice;
        public string FollowUpAdvice
        {
            get { return _FollowUpAdvice; }
            set
            {
                if (value != _FollowUpAdvice)
                {
                    _FollowUpAdvice = value;
                    OnPropertyChanged("FollowUpAdvice");
                }
            }
        }
        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (value != _Remarks)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
                }
            }
        }

        private string _PrescriptionDate;
        public string PrescriptionDate
        {
            get { return _PrescriptionDate; }
            set
            {
                if (value != _PrescriptionDate)
                {
                    _PrescriptionDate = value;
                    OnPropertyChanged("PrescriptionDate");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (value != _Code)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string UnitName { get; set; }

        private bool _IsOrderSet = false;
        public bool IsOrderSet
        {
            get { return _IsOrderSet; }
            set
            {
                if (value != _IsOrderSet)
                {
                    _IsOrderSet = value;
                    OnPropertyChanged("IsOrderSet");
                }
            }
        }

        #region CommonField

        private bool _IsDoctorFavorite;
        public bool IsDoctorFavorite
        {
            get { return _IsDoctorFavorite; }
            set
            {
                if (value != _IsDoctorFavorite)
                {
                    _IsDoctorFavorite = value;
                }
            }
        }

        private bool? blnStatus = null;
        public bool? Status
        {
            get { return blnStatus; }
            set
            {
                if (value != blnStatus)
                {
                    blnStatus = value;
                    //OnPropertyChanged("Status");
                }
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }


        private string _UpdatedBy;
        public string UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsPrescriptionDetailsVO : IValueObject, INotifyPropertyChanged
    {
        public clsPrescriptionDetailsVO()
        {

        }

        #region Property Declaration Section
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


        private double _ConsumeQuantity;
        public double ConsumeQuantity
        {
            get { return _ConsumeQuantity; }
            set
            {
                if (_ConsumeQuantity != value)
                {
                    _ConsumeQuantity = value;
                    OnPropertyChanged("ConsumeQuantity");
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

        public Boolean InclusiveOfTax { get; set; }

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
        List<MasterListItem> _FrequencyName = new List<MasterListItem>();
        public List<MasterListItem> FrequencyName
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

        MasterListItem _SelectedFrequency = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedFrequency
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

        public long ItemCategory { get; set; }
        public long ItemGroup { get; set; }

        public string Manufacture { get; set; }
        public string PregnancyClass { get; set; }

        #endregion

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _PriorityID;
        public long PriorityID
        {
            get { return _PriorityID; }
            set
            {
                if (value != _PriorityID)
                {
                    _PriorityID = value;
                    OnPropertyChanged("PriorityID");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (value != _DepartmentID)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (value != _DoctorID)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }
        private List<clsCompoundMasterDetailsVO> _CompoundMasterDetailsList;
        public List<clsCompoundMasterDetailsVO> CompoundMasterDetailsList
        {
            get { return _CompoundMasterDetailsList; }
            set
            {
                if (_CompoundMasterDetailsList != value)
                {
                    _CompoundMasterDetailsList = value;
                    OnPropertyChanged("CompoundMasterDetailsList");
                }
            }
        }
        private clsCompoundMasterDetailsVO _CompoundMasterDetails;
        public clsCompoundMasterDetailsVO CompoundMasterDetails
        {
            get { return _CompoundMasterDetails; }
            set
            {
                if (_CompoundMasterDetails != value)
                {
                    _CompoundMasterDetails = value;
                    OnPropertyChanged("CompoundMasterDetails");
                }
            }
        }

        private long _SrNo;
        public long SrNo
        {
            get { return _SrNo; }
            set
            {
                if (value != _SrNo)
                {
                    _SrNo = value;
                    OnPropertyChanged("SrNo");
                }
            }
        }

        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private long _TakenID;
        public long TakenID
        {
            get { return _TakenID; }
            set
            {
                if (value != _TakenID)
                {
                    _TakenID = value;
                    OnPropertyChanged("TakenID");
                }
            }
        }

        private string _TakenName;
        public string TakenBy
        {
            get { return _TakenName; }
            set
            {
                if (value != _TakenName)
                {
                    _TakenName = value;
                    OnPropertyChanged("TakenName");
                }
            }
        }

        private long _PrescriptionUnitID;
        public long PrescriptionUnitID
        {
            get { return _PrescriptionUnitID; }
            set
            {
                if (value != _PrescriptionUnitID)
                {
                    _PrescriptionUnitID = value;
                    OnPropertyChanged("PrescriptionUnitID");
                }
            }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set
            {
                if (value != _PrescriptionID)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
                }
            }
        }

        private long _ReadymadePrescriptionID;
        public long ReadymadePrescriptionID
        {
            get { return _ReadymadePrescriptionID; }
            set
            {
                if (value != _ReadymadePrescriptionID)
                {
                    _ReadymadePrescriptionID = value;
                    OnPropertyChanged("ReadymadePrescriptionID");
                }
            }
        }

        private long _ReadymadePrescriptionUnitID;
        public long ReadymadePrescriptionUnitID
        {
            get { return _ReadymadePrescriptionUnitID; }
            set
            {
                if (value != _ReadymadePrescriptionUnitID)
                {
                    _ReadymadePrescriptionUnitID = value;
                    OnPropertyChanged("ReadymadePrescriptionUnitID");
                }
            }
        }

        private long _DrugId;
        public long DrugId
        {
            get { return _DrugId; }
            set
            {
                if (value != _DrugId)
                {
                    _DrugId = value;
                    OnPropertyChanged("DrugId");
                }
            }
        }

        private string _VisibleCompaundDetail;
        public string VisibleCompaundDetail
        {
            get { return _VisibleCompaundDetail; }
            set
            {
                if (_VisibleCompaundDetail != value)
                {
                    _VisibleCompaundDetail = value;
                    OnPropertyChanged("VisibleCompaundDetail");
                }
            }
        }


        private string _DrugName;
        public string DrugName
        {
            get { return _DrugName; }
            set
            {
                if (value != _DrugName)
                {
                    _DrugName = value;
                    OnPropertyChanged("DrugName");
                }
            }
        }

        private long _MoleculeId;
        public long MoleculeId
        {
            get { return _MoleculeId; }
            set
            {
                if (value != _MoleculeId)
                {
                    _MoleculeId = value;
                    OnPropertyChanged("MoleculeId");
                }
            }
        }

        private string _moleculeName;
        public string MoleculeName
        {
            get { return _moleculeName; }
            set
            {
                if (value != _moleculeName)
                {
                    _moleculeName = value;
                    OnPropertyChanged("MoleculeName");
                }
            }
        }

        private long _DespenseId;
        public long DespenseId
        {
            get { return _DespenseId; }
            set
            {
                if (value != _DespenseId)
                {
                    _DespenseId = value;
                    OnPropertyChanged("DespenseId");
                }
            }
        }

        private string _Despense;
        public string Despense
        {
            get { return _Despense; }
            set
            {
                if (value != _Despense)
                {
                    _Despense = value;
                    OnPropertyChanged("Despense");
                }
            }
        }

        private long _DoseId;
        public long DoseId
        {
            get { return _DoseId; }
            set
            {
                if (value != _DoseId)
                {
                    _DoseId = value;
                    OnPropertyChanged("DoseId");
                }
            }
        }

        private string _Dose;
        public string Dose
        {
            get { return _Dose; }
            set
            {
                if (value != _Dose)
                {
                    _Dose = value;
                    OnPropertyChanged("Dose");
                }
            }
        }

        private Double _Duration;
        public Double Duration
        {
            get { return _Duration; }
            set
            {
                if (value != _Duration)
                {
                    _Duration = value;
                    OnPropertyChanged("Duration");
                }
            }
        }

        private string _Refill;
        public string Refill
        {
            get { return _Refill; }
            set
            {
                if (value != _Refill)
                {
                    _Refill = value;
                    OnPropertyChanged("Refill");
                }
            }
        }

        private long _InstructionId;
        public long InstructionId
        {
            get { return _InstructionId; }
            set
            {
                if (value != _InstructionId)
                {
                    _InstructionId = value;
                    OnPropertyChanged("InstructionId");
                }
            }
        }

        private string _Instruction;
        public string Instruction
        {
            get { return _Instruction; }
            set
            {
                if (value != _Instruction)
                {
                    _Instruction = value;
                    OnPropertyChanged("Instruction");
                }
            }
        }

        private long _RouteId;
        public long RouteId
        {
            get { return _RouteId; }
            set
            {
                if (value != _RouteId)
                {
                    _RouteId = value;
                    OnPropertyChanged("RouteId");
                }
            }
        }

        private double _Strength;
        public double Strength
        {
            get { return _Strength; }
            set
            {
                if (value != _Strength)
                {
                    _Strength = value;
                    OnPropertyChanged("Strength");
                }
            }
        }

        private long _StrengthUnit;
        public long StrengthUnit
        {
            get { return _StrengthUnit; }
            set
            {
                if (value != _StrengthUnit)
                {
                    _StrengthUnit = value;
                    OnPropertyChanged("StrengthUnit");
                }
            }
        }

        private string _Route;
        public string Route
        {
            get { return _Route; }
            set
            {
                if (value != _Route)
                {
                    _Route = value;
                    OnPropertyChanged("Route");
                }
            }
        }

        private string _IPDOPD;
        public string IPDOPD
        {
            get { return _IPDOPD; }
            set
            {
                if (value != _IPDOPD)
                {
                    _IPDOPD = value;
                    OnPropertyChanged("IPDOPD");
                }
            }
        }

        private string _OPD_IPD;
        public string OPD_IPD
        {
            get { return _OPD_IPD; }
            set
            {
                if (value != _OPD_IPD)
                {
                    _OPD_IPD = value;
                    OnPropertyChanged("OPD_IPD");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
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

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (value != _Code)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string UnitName { get; set; }

        private string _Opd_Ipd_Id;
        public string Opd_Ipd_Id
        {
            get { return _Opd_Ipd_Id; }
            set
            {
                if (value != _Opd_Ipd_Id)
                {
                    _Opd_Ipd_Id = value;
                    OnPropertyChanged("Opd_Ipd_Id");
                }
            }
        }

        private string _Opd_Ipd_UnitId;
        public string Opd_Ipd_UnitId
        {
            get { return _Opd_Ipd_UnitId; }
            set
            {
                if (value != _Opd_Ipd_UnitId)
                {
                    _Opd_Ipd_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_UnitId");
                }
            }
        }

        private bool _IsFreeze;
        public bool IsFreeze
        {
            get { return _IsFreeze; }
            set
            {
                if (value != _IsFreeze)
                {
                    _IsFreeze = value;
                    OnPropertyChanged("IsFreeze");
                }
            }
        }
        private bool _IsDelete;
        public bool IsDelete
        {
            get { return _IsDelete; }
            set
            {
                if (value != _IsDelete)
                {
                    _IsDelete = value;
                    OnPropertyChanged("IsDelete");
                }
            }
        }
        private bool _IsCompound;
        public bool IsCompound
        {
            get { return _IsCompound; }
            set { _IsCompound = value; }
        }

        // Added By CDS On 28/02/17
        private Boolean? _IsChecked = false;
        public Boolean? IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
            }
        }


        List<MasterListItem> _ListTakenBy = new List<MasterListItem>();
        public List<MasterListItem> ListTakenBy
        {
            get
            {
                return _ListTakenBy;
            }

            set
            {
                if (value != _ListTakenBy)
                {
                    _ListTakenBy = value;

                }
            }


        }

        MasterListItem _SelectedTakenBy = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedTakenBy
        {
            get
            {
                return _SelectedTakenBy;

            }
            set
            {
                if (value != _SelectedTakenBy)
                {
                    _SelectedTakenBy = value;
                    OnPropertyChanged("SelectedTakenBy");
                }
            }
        }

        #region CommonField

        private bool? blnStatus = null;
        public bool? Status
        {
            get { return blnStatus; }
            set
            {
                if (value != blnStatus)
                {
                    blnStatus = value;
                    //OnPropertyChanged("Status");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }


        private string _UpdatedBy;
        public string UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class FrequencyMaster : NotificationModel
    {
        public FrequencyMaster()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public FrequencyMaster(long Id, string Description)
        {
            this.ID = Id;
            this.Description = Description;
        }

        /// <summary>
        /// MasterListItem
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public FrequencyMaster(long Id, string Description, double Quantity)
        {
            this.ID = Id;
            this.Description = Description;
            this.Quantity = Quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Description">Field Description</param>
        public FrequencyMaster(long Id, string Description, bool Status)
        {
            this.ID = Id;
            this.Description = Description;
            this.Status = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">MasterId</param>
        /// <param name="Code">Code</param>
        /// <param name="Description">Field Description</param>
        /// <param name="Status">Record Status</param>
        public FrequencyMaster(long Id, string Code, string Description, bool Status, double Quantity)
        {
            this.ID = Id;
            this.Code = Code;
            this.Description = Description;
            this.Status = Status;

        }

        private long _Id;
        private string _Code;
        private string _Description = string.Empty;
        private string _PrintDescription = string.Empty;
        private bool _Status = false;
        public bool isChecked { get; set; }
        private double _Quantity;


        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Id To Represent Record.
        /// </summary>
        public long ID { get { return _Id; } set { _Id = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Code Of Record.
        /// </summary>
        public string Code { get { return _Code; } set { _Code = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Description.
        /// </summary>
        public string Description { get { return _Description; } set { _Description = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Description.
        /// </summary>
        public string PrintDescription { get { return _PrintDescription; } set { _PrintDescription = value; } }

        /// <summary>
        /// ParameterDirection:
        ///         Input/Output Parameter.
        /// Summary:
        ///         Status Of Record.
        /// </summary>
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        public double Quantity { get { return _Quantity; } set { _Quantity = value; } }

        public override string ToString()
        {
            return this.Description;
        }

    }

    public class clsPatientCompoundPrescriptionVO : INotifyPropertyChanged, IValueObject
    {
        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region INotifyPropertyChanged

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


        #region Properties
        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }
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

        private long _CompoundDrugID;
        public long CompoundDrugID
        {
            get
            {
                return _CompoundDrugID;
            }
            set
            {
                if (_CompoundDrugID != value)
                {
                    _CompoundDrugID = value;
                    OnPropertyChanged("CompoundDrugID");
                }
            }
        }

        private long _CompoundDrugUnitID;
        public long CompoundDrugUnitID
        {
            get
            {
                return _CompoundDrugUnitID;
            }
            set
            {
                if (_CompoundDrugUnitID != value)
                {
                    _CompoundDrugUnitID = value;
                    OnPropertyChanged("CompoundDrugUnitID");
                }
            }
        }

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get
            {
                return _PrescriptionID;
            }
            set
            {
                if (_PrescriptionID != value)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
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

        private String _Reason;
        public String Reason
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



        private double _ComponentQuantity;
        public double ComponentQuantity
        {
            get { return _ComponentQuantity; }
            set
            {
                if (_ComponentQuantity != value)
                {
                    _ComponentQuantity = value;
                    OnPropertyChanged("ComponentQuantity");
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

        private int _Days;
        public int Days
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

        private string _Dose;
        public string Dose
        {
            get
            {
                return _Dose;
            }
            set
            {
                if (_Dose != value)
                {
                    _Dose = value;
                    OnPropertyChanged("Dose");
                }
            }
        }


        #endregion

    }

    public class clsItemMoleculeVO : IValueObject, INotifyPropertyChanged
    {
        public clsItemMoleculeVO()
        {

        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }
            }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
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
                }
            }
        }

        private string _BrandName;
        public string BrandName
        {
            get { return _BrandName; }
            set
            {
                if (_BrandName != value)
                {
                    _BrandName = value;
                }
            }
        }

        private string _Form;
        public string Form
        {
            get { return _Form; }
            set
            {
                if (_Form != value)
                {
                    _Form = value;
                }
            }
        }

        private double _ItemQuntity;
        public double BalanceQuantity
        {
            get { return _ItemQuntity; }
            set
            {
                if (_ItemQuntity != value)
                {
                    _ItemQuntity = value;
                }
            }
        }

        private long _MoleculeName;
        public long MoleculeName
        {
            get { return _MoleculeName; }
            set
            {
                if (_MoleculeName != value)
                {
                    _MoleculeName = value;
                }
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
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
