using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_NewSpremFreezingMainVO : IValueObject, INotifyPropertyChanged
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

        private Boolean _Status;
        public Boolean Status
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

        private bool _Synchronized;
        public bool Synchronized
        {
            get { return _Synchronized; }
            set
            {
                if (_Synchronized != value)
                {
                    _Synchronized = value;
                    OnPropertyChanged("Synchronized");
                }
            }
        }

        #endregion


        //.................................................................................
        private long _TherapyID;
        public long TherapyID
        {
            get { return _TherapyID; }
            set
            {
                if (_TherapyID != value)
                {
                    _TherapyID = value;
                    OnPropertyChanged("TherapyID");
                }
            }
        }

        private long _TherapyUnitID;
        public long TherapyUnitID
        {
            get { return _TherapyUnitID; }
            set
            {
                if (_TherapyUnitID != value)
                {
                    _TherapyUnitID = value;
                    OnPropertyChanged("TherapyUnitID");
                }
            }
        }
        private string _CycleCode;
        public string CycleCode
        {
            get { return _CycleCode; }
            set
            {
                if (_CycleCode != value)
                {
                    _CycleCode = value;
                    OnPropertyChanged("CycleCode");
                }
            }
        }
        private string _CollectionMethod;
        public string CollectionMethod
        {
            get { return _CollectionMethod; }
            set
            {
                if (_CollectionMethod != value)
                {
                    _CollectionMethod = value;
                    OnPropertyChanged("CollectionMethod");
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
        private string _Embryologist;
        public string Embryologist
        {
            get { return _Embryologist; }
            set
            {
                if (_Embryologist != value)
                {
                    _Embryologist = value;
                    OnPropertyChanged("Embryologist");
                }
            }
        }
        //.................................................................................

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

        private long _SpermTypeID;
        public long SpermTypeID
        {
            get { return _SpermTypeID; }
            set
            {
                if (_SpermTypeID != value)
                {
                    _SpermTypeID = value;
                    OnPropertyChanged("SpermTypeID");
                }
            }
        }

        private string _SpermType;
        public string SpermType
        {
            get { return _SpermType; }
            set
            {
                if (_SpermType != value)
                {
                    _SpermType = value;
                    OnPropertyChanged("SpermType");
                }
            }
        }



        private string _SampleCode;
        public string SampleCode
        {
            get { return _SampleCode; }
            set
            {
                if (_SampleCode != value)
                {
                    _SampleCode = value;
                    OnPropertyChanged("SampleCode");
                }
            }
        }

        private float _Sperm5thPercentile;
        public float Sperm5thPercentile
        {
            get { return _Sperm5thPercentile; }
            set
            {
                if (_Sperm5thPercentile != value)
                {
                    _Sperm5thPercentile = value;
                    OnPropertyChanged("Sperm5thPercentile");
                }
            }
        }

        private float _Sperm75thPercentile;
        public float Sperm75thPercentile
        {
            get { return _Sperm75thPercentile; }
            set
            {
                if (_Sperm75thPercentile != value)
                {
                    _Sperm75thPercentile = value;
                    OnPropertyChanged("Sperm75thPercentile");
                }
            }
        }

        private float _Ejaculate5thPercentile;
        public float Ejaculate5thPercentile
        {
            get { return _Ejaculate5thPercentile; }
            set
            {
                if (_Ejaculate5thPercentile != value)
                {
                    _Ejaculate5thPercentile = value;
                    OnPropertyChanged("Ejaculate5thPercentile");
                }
            }
        }

        private float _Ejaculate75thPercentile;
        public float Ejaculate75thPercentile
        {
            get { return _Ejaculate75thPercentile; }
            set
            {
                if (_Ejaculate75thPercentile != value)
                {
                    _Ejaculate75thPercentile = value;
                    OnPropertyChanged("Ejaculate75thPercentile");
                }
            }
        }

        private float _TotalMotility5thPercentile;
        public float TotalMotility5thPercentile
        {
            get { return _TotalMotility5thPercentile; }
            set
            {
                if (_TotalMotility5thPercentile != value)
                {
                    _TotalMotility5thPercentile = value;
                    OnPropertyChanged("TotalMotility5thPercentile");
                }
            }
        }

        private float _TotalMotility75thPercentile;
        public float TotalMotility75thPercentile
        {
            get { return _TotalMotility75thPercentile; }
            set
            {
                if (_TotalMotility75thPercentile != value)
                {
                    _TotalMotility75thPercentile = value;
                    OnPropertyChanged("TotalMotility75thPercentile");
                }
            }
        }

        private DateTime? _SpremFreezingDate;
        public DateTime? SpremFreezingDate
        {
            get { return _SpremFreezingDate; }
            set
            {
                if (_SpremFreezingDate != value)
                {
                    _SpremFreezingDate = value;
                    OnPropertyChanged("SpremFreezingDate");
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
        private float _LabID;
        public float LabID
        {
            get { return _LabID; }
            set
            {
                if (_LabID != value)
                {
                    _LabID = value;
                    OnPropertyChanged("LabID");
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
        private long _BatchUnitID;
        public long BatchUnitID
        {
            get { return _BatchUnitID; }
            set
            {
                if (_BatchUnitID != value)
                {
                    _BatchUnitID = value;
                    OnPropertyChanged("BatchUnitID");
                }
            }
        }
        private DateTime? _SpremFreezingTime;
        public DateTime? SpremFreezingTime
        {
            get { return _SpremFreezingTime; }
            set
            {
                if (_SpremFreezingTime != value)
                {
                    _SpremFreezingTime = value;
                    OnPropertyChanged("SpremFreezingTime");
                }
            }
        }

        private DateTime? _SpremExpiryDate;
        public DateTime? SpremExpiryDate
        {
            get { return _SpremExpiryDate; }
            set
            {
                if (_SpremExpiryDate != value)
                {
                    _SpremExpiryDate = value;
                    OnPropertyChanged("SpremExpiryDate");
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

        private long _EmbryologistID;
        public long EmbryologistID
        {
            get { return _EmbryologistID; }
            set
            {
                if (_EmbryologistID != value)
                {
                    _EmbryologistID = value;
                    OnPropertyChanged("EmbryologistID");
                }
            }
        }

        private long _CollectionMethodID;
        public long CollectionMethodID
        {
            get { return _CollectionMethodID; }
            set
            {
                if (_CollectionMethodID != value)
                {
                    _CollectionMethodID = value;
                    OnPropertyChanged("CollectionMethodID");
                }
            }
        }
        private float _NoOfVails;
        public float NoOfVails
        {
            get { return _NoOfVails; }
            set
            {
                if (_NoOfVails != value)
                {
                    _NoOfVails = value;
                    OnPropertyChanged("NoOfVails");
                }
            }
        }
        private long _ViscosityID;
        public long ViscosityID
        {
            get { return _ViscosityID; }
            set
            {
                if (_ViscosityID != value)
                {
                    _ViscosityID = value;
                    OnPropertyChanged("ViscosityID");
                }
            }
        }

        private string _CollectionProblem;
        public string CollectionProblem
        {
            get { return _CollectionProblem; }
            set
            {
                if (_CollectionProblem != value)
                {
                    _CollectionProblem = value;
                    OnPropertyChanged("CollectionProblem");
                }
            }
        }

        private float _Volume;
        public float Volume
        {
            get { return _Volume; }
            set
            {
                if (_Volume != value)
                {
                    _Volume = value;
                    OnPropertyChanged("Volume");
                }
            }
        }

        private string _Abstience;
        public string Abstience
        {
            get { return _Abstience; }
            set
            {
                if (_Abstience != value)
                {
                    _Abstience = value;
                    OnPropertyChanged("Abstience");
                }
            }
        }

        private long _TotalSpremCount;
        public long TotalSpremCount
        {
            get { return _TotalSpremCount; }
            set
            {
                if (_TotalSpremCount != value)
                {
                    _TotalSpremCount = value;
                    OnPropertyChanged("TotalSpremCount");
                }
            }
        }


        //private float _TotalSpremCount;     // For Andrology Flow changed datatype from long to float on 15052017
        //public float TotalSpremCount
        //{
        //    get { return _TotalSpremCount; }
        //    set
        //    {
        //        if (_TotalSpremCount != value)
        //        {
        //            _TotalSpremCount = value;
        //            OnPropertyChanged("TotalSpremCount");
        //        }
        //    }
        //}

        private decimal _Motility;
        public decimal Motility
        {
            get { return _Motility; }
            set
            {
                if (_Motility != value)
                {
                    _Motility = value;
                    OnPropertyChanged("Motility");
                }
            }
        }

        private decimal _GradeA;
        public decimal GradeA
        {
            get { return _GradeA; }
            set
            {
                if (_GradeA != value)
                {
                    _GradeA = value;
                    OnPropertyChanged("GradeA");
                }
            }
        }

        private decimal _GradeB;
        public decimal GradeB
        {
            get { return _GradeB; }
            set
            {
                if (_GradeB != value)
                {
                    _GradeB = value;
                    OnPropertyChanged("GradeB");
                }
            }
        }

        private decimal _GradeC;
        public decimal GradeC
        {
            get { return _GradeC; }
            set
            {
                if (_GradeC != value)
                {
                    _GradeC = value;
                    OnPropertyChanged("GradeC");
                }
            }
        }

        private string _Other;
        public string Other
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

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set
            {
                if (_Comments != value)
                {
                    _Comments = value;
                    OnPropertyChanged("Comments");
                }
            }
        }

        #region For Andrology Flow added on 15052017

        private string _Spillage;
        public string Spillage
        {
            get { return _Spillage; }
            set
            {
                if (_Spillage != value)
                {
                    _Spillage = value;
                    OnPropertyChanged("Spillage");
                }
            }
        }

        private decimal _SpermCount;
        public decimal SpermCount
        {
            get { return _SpermCount; }
            set
            {
                if (_SpermCount != value)
                {
                    _SpermCount = value;
                    OnPropertyChanged("SpermCount");
                }
            }
        }

        private decimal _DFI;
        public decimal DFI
        {
            get { return _DFI; }
            set
            {
                if (_DFI != value)
                {
                    _DFI = value;
                    OnPropertyChanged("DFI");
                }
            }
        }

        private decimal _ROS;
        public decimal ROS
        {
            get { return _ROS; }
            set
            {
                if (_ROS != value)
                {
                    _ROS = value;
                    OnPropertyChanged("ROS");
                }
            }
        }

        private long _HIV;
        public long HIV
        {
            get { return _HIV; }
            set
            {
                if (_HIV != value)
                {
                    _HIV = value;
                    OnPropertyChanged("HIV");
                }
            }
        }
        private long _HBSAG;
        public long HBSAG
        {
            get { return _HBSAG; }
            set
            {
                if (_HBSAG != value)
                {
                    _HBSAG = value;
                    OnPropertyChanged("HBSAG");
                }
            }
        }

        private long _HCV;
        public long HCV
        {
            get { return _HCV; }
            set
            {
                if (_HCV != value)
                {
                    _HCV = value;
                    OnPropertyChanged("HCV");
                }
            }
        }

        private long _VDRL;
        public long VDRL
        {
            get { return _VDRL; }
            set
            {
                if (_VDRL != value)
                {
                    _VDRL = value;
                    OnPropertyChanged("VDRL");
                }
            }
        }


        private long _CheckedByDoctorID;
        public long CheckedByDoctorID
        {
            get { return _CheckedByDoctorID; }
            set
            {
                if (_CheckedByDoctorID != value)
                {
                    _CheckedByDoctorID = value;
                    OnPropertyChanged("CheckedByDoctorID");
                }
            }
        }

        private bool _IsConsentCheck;
        public bool IsConsentCheck
        {
            get { return _IsConsentCheck; }
            set
            {
                if (_IsConsentCheck != value)
                {
                    _IsConsentCheck = value;
                    OnPropertyChanged("IsConsentCheck");
                }
            }
        }

        private bool _IsFreezed;
        public bool IsFreezed
        {
            get { return _IsFreezed; }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }

        private long _AbstienceID;
        public long AbstienceID
        {
            get { return _AbstienceID; }
            set
            {
                if (_AbstienceID != value)
                {
                    _AbstienceID = value;
                    OnPropertyChanged("AbstienceID");
                }
            }
        }

        private string _PusCells;
        public string PusCells
        {
            get { return _PusCells; }
            set
            {
                if (_PusCells != value)
                {
                    _PusCells = value;
                    OnPropertyChanged("PusCells");
                }
            }
        }

        private string _RoundCells;
        public string RoundCells
        {
            get { return _RoundCells; }
            set
            {
                if (_RoundCells != value)
                {
                    _RoundCells = value;
                    OnPropertyChanged("RoundCells");
                }
            }
        }

        private string _EpithelialCells;
        public string EpithelialCells
        {
            get { return _EpithelialCells; }
            set
            {
                if (_EpithelialCells != value)
                {
                    _EpithelialCells = value;
                    OnPropertyChanged("EpithelialCells");
                }
            }
        }

        private string _OtherCells;
        public string OtherCells
        {
            get { return _OtherCells; }
            set
            {
                if (_OtherCells != value)
                {
                    _OtherCells = value;
                    OnPropertyChanged("OtherCells");
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

        private long _VisitUnitID;
        public long VisitUnitID
        {
            get { return _VisitUnitID; }
            set
            {
                if (_VisitUnitID != value)
                {
                    _VisitUnitID = value;
                    OnPropertyChanged("VisitUnitID");
                }
            }
        }

        #endregion

    }

    public class clsNew_SpremFreezingVO : IValueObject, INotifyPropertyChanged
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

        public Int64 ID { get; set; }
        public Int64 UnitID { get; set; }

        private bool _IsThaw;
        public bool IsThaw
        {
            get { return _IsThaw; }
            set
            {
                if (_IsThaw != value)
                {
                    _IsThaw = value;
                    OnPropertyChanged("IsThaw");
                }
            }
        }

        public long SpremNo { get; set; }
        public string SpremNostr { get; set; }
        public DateTime VitrificationDate { get; set; }
        public DateTime VitrificationTime { get; set; }
        public string MRNo { get; set; }
        public string PatientUnitName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string GobletColor { get; set; }
        public long GobletColorID { get; set; }
        public long CanID { get; set; }
        public string Cane { get; set; }
        //public bool IsThaw { get; set; }
        public bool Status { get; set; }
        public bool IsModify { get; set; }
        public long PlanTherapy { get; set; }
        public long PlanTherapyUnitID { get; set; }
        //public string CanisterNo { get; set; }

        public long ProgressionID { get; set; }
        public string Progression { get; set; }
        public double AbnormalSperm { get; set; }
        public double RoundCell { get; set; }
        public long AgglutinationID { get; set; }
        public string Agglutination { get; set; }
        public bool IsEnabled { get; set; }
        private bool _SelectFreezed;
        public bool SelectFreezed
        {
            get { return _SelectFreezed; }
            set
            {
                if (value != _SelectFreezed)
                {
                    _SelectFreezed = value;
                    OnPropertyChanged("SelectFreezed");
                }
            }
        }
        public long StrawId { get; set; }
        public string Straw { get; set; }
        public long GobletShapeId { get; set; }
        public string GolbletShape { get; set; }
        public long GobletSizeId { get; set; }
        public string GobletSize { get; set; }
        public long CanisterId { get; set; }
        public string Canister { get; set; }
        public long TankId { get; set; }
        public string Tank { get; set; }
        public long CollectionMethod { get; set; }
        public string strCollectionMethod { get; set; }

        public double Volume { get; set; }
        public string Abstinence { get; set; }
        public long SpermCount { get; set; }
        public double Motility { get; set; }
        public string Code { get; set; }
        public string Comments { get; set; }
        public bool IsNewlyAdded { get; set; }
        public string DescriptionValue { get; set; }

        public long SampleLinkID { get; set; }//For Tese/tesa/pese sample ID linking from table T_PatientEMRData.ID to T_IVF_SpremFreezing.SampleLinkID        

        private List<MasterListItem> _ColorList = new List<MasterListItem>();
        public List<MasterListItem> ColorListVO
        {
            get
            {
                return _ColorList;
            }
            set
            {
                _ColorList = value;
            }
        }
        private MasterListItem _selectedColorList = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedColorList
        {
            get
            {
                return _selectedColorList;
            }
            set
            {
                _selectedColorList = value;
            }
        }

        private List<MasterListItem> _CanList = new List<MasterListItem>();
        public List<MasterListItem> CanListVO
        {
            get
            {
                return _CanList;
            }
            set
            {
                _CanList = value;
            }
        }
        private MasterListItem _selectedCanListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedCanListVO
        {
            get
            {
                return _selectedCanListVO;
            }
            set
            {
                _selectedCanListVO = value;
            }
        }

        private List<MasterListItem> _StrawList = new List<MasterListItem>();
        public List<MasterListItem> StrawListVO
        {
            get
            {
                return _StrawList;
            }
            set
            {
                _StrawList = value;
            }
        }
        private MasterListItem _selectedStrawListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedStrawListVO
        {
            get
            {
                return _selectedStrawListVO;
            }
            set
            {
                _selectedStrawListVO = value;
            }
        }

        private List<MasterListItem> _GlobletShapeList = new List<MasterListItem>();
        public List<MasterListItem> GlobletShapeListVO
        {
            get
            {
                return _GlobletShapeList;
            }
            set
            {
                _GlobletShapeList = value;
            }
        }
        private MasterListItem _selectedGlobletShapeListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedGlobletShapeListVO
        {
            get
            {
                return _selectedGlobletShapeListVO;
            }
            set
            {
                _selectedGlobletShapeListVO = value;
            }
        }

        private List<MasterListItem> _CanisterList = new List<MasterListItem>();
        public List<MasterListItem> CanisterListVO
        {
            get
            {
                return _CanisterList;
            }
            set
            {
                _CanisterList = value;
            }
        }
        private MasterListItem _selectedCanisterListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedCanisterListVO
        {
            get
            {
                return _selectedCanisterListVO;
            }
            set
            {
                _selectedCanisterListVO = value;
            }
        }

        private List<MasterListItem> _GlobletSizeList = new List<MasterListItem>();
        public List<MasterListItem> GlobletSizeListVO
        {
            get
            {
                return _GlobletSizeList;
            }
            set
            {
                _GlobletSizeList = value;
            }
        }
        private MasterListItem _selectedGlobletSizeListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedGlobletSizeListVO
        {
            get
            {
                return _selectedGlobletSizeListVO;
            }
            set
            {
                _selectedGlobletSizeListVO = value;
            }
        }

        private List<MasterListItem> _TankList = new List<MasterListItem>();
        public List<MasterListItem> TankListVO
        {
            get
            {
                return _TankList;
            }
            set
            {
                _TankList = value;
            }
        }
        private MasterListItem _selectedTankListVO = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedTankListVO
        {
            get
            {
                return _selectedTankListVO;
            }
            set
            {
                _selectedTankListVO = value;
            }
        }
    }
}
