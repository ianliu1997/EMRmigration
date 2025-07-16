using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class cls_NewThawingVO
    {
        public DateTime? Date { get; set; }
        public long LabPerseonID { get; set; }
        public string Impression { get; set; }

        private List<cls_NewThawingDetailsVO> _ThawingDetails = new List<cls_NewThawingDetailsVO>();
        public List<cls_NewThawingDetailsVO> ThawingDetails
        {
            get
            {
                return _ThawingDetails;
            }
            set
            {
                _ThawingDetails = value;
            }
        }

        private List<FileUpload> _FUSetting = new List<FileUpload>();
        public List<FileUpload> FUSetting
        {
            get
            {
                return _FUSetting;
            }
            set
            {
                _FUSetting = value;
            }
        }
    }

    public class cls_NewThawingDetailsVO
    {
        public Int64 ID { get; set; }
        public Int64 VitrificationID { get; set; }

        public string EmbNo { get; set; }
        public DateTime? Date { get; set; }
        public long CellStangeID { get; set; }
        public Int64 GradeID { get; set; }
        public Boolean Plan { get; set; }
        //By Anjali..........
        public string SerialOccyteNo { get; set; }
        //.....................

        private DateTime? _SpremFreezingDate = DateTime.Now;
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

        private DateTime? _SpremFreezingTime = DateTime.Now;
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

        private DateTime? _ThawingDate = DateTime.Now;
        public DateTime? ThawingDate
        {
            get { return _ThawingDate; }
            set
            {
                if (_ThawingDate != value)
                {
                    _ThawingDate = value;
                    OnPropertyChanged("ThawingDate");
                }
            }
        }

        private DateTime? _ThawingTime = DateTime.Now;
        public DateTime? ThawingTime
        {
            get { return _ThawingTime; }
            set
            {
                if (_ThawingTime != value)
                {
                    _ThawingTime = value;
                    OnPropertyChanged("ThawingTime");
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
        private long _AssisEmbryologistID;
        public long AssisEmbryologistID
        {
            get { return _AssisEmbryologistID; }
            set
            {
                if (_AssisEmbryologistID != value)
                {
                    _AssisEmbryologistID = value;
                    OnPropertyChanged("AssisEmbryologistID");
                }
            }
        }

        private long _CoupleID;
        public long CoupleID
        {
            get { return _CoupleID; }
            set
            {
                if (_CoupleID != value)
                {
                    _CoupleID = value;
                    OnPropertyChanged("CoupleID");
                }
            }
        }

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
        private long _FreezingID;
        public long FreezingID
        {
            get { return _FreezingID; }
            set
            {
                if (_FreezingID != value)
                {
                    _FreezingID = value;
                    OnPropertyChanged("FreezingID");
                }
            }
        }

        private long _FreezingUnitID;
        public long FreezingUnitID
        {
            get { return _FreezingUnitID; }
            set
            {
                if (_FreezingUnitID != value)
                {
                    _FreezingUnitID = value;
                    OnPropertyChanged("FreezingUnitID");
                }
            }
        }
        private float _UsedVolume;
        public float UsedVolume
        {
            get { return _UsedVolume; }
            set
            {
                if (_UsedVolume != value)
                {
                    _UsedVolume = value;
                    OnPropertyChanged("UsedVolume");
                }
            }
        }
        private long _SpremNo;
        public long SpremNo
        {
            get { return _SpremNo; }
            set
            {
                if (_SpremNo != value)
                {
                    _SpremNo = value;
                    OnPropertyChanged("SpremNo");
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

        private long _CoupleUnitID;
        public long CoupleUnitID
        {
            get { return _CoupleUnitID; }
            set
            {
                if (_CoupleUnitID != value)
                {
                    _CoupleUnitID = value;
                    OnPropertyChanged("CoupleUnitID");
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

        private long _PlanTherapyID;
        public long PlanTherapyID
        {
            get { return _PlanTherapyID; }
            set
            {
                if (_PlanTherapyID != value)
                {
                    _PlanTherapyID = value;
                    OnPropertyChanged("PlanTherapyID");
                }
            }
        }

        private long _PlanTherapyUnitID;
        public long PlanTherapyUnitID
        {
            get { return _PlanTherapyUnitID; }
            set
            {
                if (_PlanTherapyUnitID != value)
                {
                    _PlanTherapyUnitID = value;
                    OnPropertyChanged("PlanTherapyUnitID");
                }
            }
        }

        private long _SemenWashID;
        public long SemenWashID
        {
            get { return _SemenWashID; }
            set
            {
                if (_SemenWashID != value)
                {
                    _SemenWashID = value;
                    OnPropertyChanged("SemenWashID");
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

        private string _ThawedFrom;
        public string ThawedFrom
        {
            get { return _ThawedFrom; }
            set
            {
                if (_ThawedFrom != value)
                {
                    _ThawedFrom = value;
                    OnPropertyChanged("ThawedFrom");
                }
            }
        }

        private string _WitnessBy;
        public string WitnessBy
        {
            get { return _WitnessBy; }
            set
            {
                if (_WitnessBy != value)
                {
                    _WitnessBy = value;
                    OnPropertyChanged("WitnessBy");
                }
            }
        }

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

        private long _MainID;
        public long MainID
        {
            get { return _MainID; }
            set
            {
                if (_MainID != value)
                {
                    _MainID = value;
                    OnPropertyChanged("MainID");
                }
            }
        }

        private long _MainUnitID;
        public long MainUnitID
        {
            get { return _MainUnitID; }
            set
            {
                if (_MainUnitID != value)
                {
                    _MainUnitID = value;
                    OnPropertyChanged("MainUnitID");
                }
            }
        }
        private long _TotalSpearmCount;
        public long TotalSpearmCount
        {
            get { return _TotalSpearmCount; }
            set
            {
                if (_TotalSpearmCount != value)
                {
                    _TotalSpearmCount = value;
                    OnPropertyChanged("TotalSpearmCount");
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

        private string _ThawComments;
        public string ThawComments
        {
            get { return _ThawComments; }
            set
            {
                if (_ThawComments != value)
                {
                    _ThawComments = value;
                    OnPropertyChanged("ThawComments");
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
        private string _Others;
        public string Others
        {
            get { return _Others; }
            set
            {
                if (_Others != value)
                {
                    _Others = value;
                    OnPropertyChanged("Others");
                }
            }
        }

        private decimal _Volume;
        public decimal Volume
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
        private decimal _Motility;
        public decimal Motility
        {
            get { return _Motility; }
            set
            {
                if (_Motility != value)
                {
                    _Motility = value;
                    OnPropertyChanged("_Motility");
                }
            }
        }

        public long DonorID { get; set; }
        public long DonorUnitID { get; set; }
        public string DonorName { get; set; }
        public string DonorMrno { get; set; }

        private long _LabPersonId;
        public long LabPersonId
        {
            get { return _LabPersonId; }
            set
            {
                if (_LabPersonId != value)
                {
                    _LabPersonId = value;
                    OnPropertyChanged("LabPersonId");
                }
            }
        }
        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
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
        private List<MasterListItem> _CellStage = new List<MasterListItem>();
        public List<MasterListItem> CellStage
        {
            get
            {
                return _CellStage;
            }
            set
            {
                _CellStage = value;
            }
        }

        private MasterListItem _SelectedCellStage = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedCellStage
        {
            get
            {
                return _SelectedCellStage;
            }
            set
            {
                _SelectedCellStage = value;
            }
        }

        private List<MasterListItem> _Grade = new List<MasterListItem>();
        public List<MasterListItem> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                _Grade = value;
            }
        }

        private MasterListItem _SelectedGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGrade
        {
            get
            {
                return _SelectedGrade;
            }
            set
            {
                _SelectedGrade = value;
            }
        }
        private List<MasterListItem> _SelectedInchargeList = new List<MasterListItem>();
        public List<MasterListItem> SelectedInchargeList
        {
            get
            {
                return _SelectedInchargeList;
            }
            set
            {
                _SelectedInchargeList = value;
            }
        }

        private List<MasterListItem> _InchargeIdList = new List<MasterListItem>();
        public List<MasterListItem> InchargeIdList
        {
            get
            {
                return _InchargeIdList;
            }
            set
            {
                _InchargeIdList = value;
            }
        }  

        private MasterListItem _SelectedIncharge = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedIncharge
        {
            get
            {
                return _SelectedIncharge;
            }
            set
            {
                _SelectedIncharge = value;
            }
        }

        private List<clsFemaleMediaDetailsVO> _MediaDetails = new List<clsFemaleMediaDetailsVO>();
        public List<clsFemaleMediaDetailsVO> MediaDetails
        {
            get
            {
                return _MediaDetails;
            }
            set
            {
                _MediaDetails = value;
            }
        }

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
}
