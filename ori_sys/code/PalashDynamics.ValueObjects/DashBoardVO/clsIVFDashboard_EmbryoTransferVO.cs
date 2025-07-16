using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_EmbryoTransferVO : IValueObject, INotifyPropertyChanged
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
        private long _AssitantEmbryologistID;
        public long AssitantEmbryologistID
        {
            get { return _AssitantEmbryologistID; }
            set
            {
                if (_AssitantEmbryologistID != value)
                {
                    _AssitantEmbryologistID = value;
                    OnPropertyChanged("AssitantEmbryologistID");
                }
            }
        }
        private decimal _EndometriumThickness;
        public decimal EndometriumThickness
        {
            get { return _EndometriumThickness; }
            set
            {
                if (_EndometriumThickness != value)
                {
                    _EndometriumThickness = value;
                    OnPropertyChanged("EndometriumThickness");
                }
            }
        }

        private long _PatternID;
        public long PatternID
        {
            get { return _PatternID; }
            set
            {
                if (_PatternID != value)
                {
                    _PatternID = value;
                    OnPropertyChanged("PatternID");
                }
            }
        }
        private bool _UterineArtery_PI;
        public bool UterineArtery_PI
        {
            get { return _UterineArtery_PI; }
            set
            {
                if (_UterineArtery_PI != value)
                {
                    _UterineArtery_PI = value;
                    OnPropertyChanged("UterineArtery_PI");
                }
            }
        }

        private bool _UterineArtery_RI;
        public bool UterineArtery_RI
        {
            get { return _UterineArtery_RI; }
            set
            {
                if (_UterineArtery_RI != value)
                {
                    _UterineArtery_RI = value;
                    OnPropertyChanged("UterineArtery_RI");
                }
            }
        }

        private bool _UterineArtery_SD;
        public bool UterineArtery_SD
        {
            get { return _UterineArtery_SD; }
            set
            {
                if (_UterineArtery_SD != value)
                {
                    _UterineArtery_SD = value;
                    OnPropertyChanged("UterineArtery_SD");
                }
            }
        }

        private bool _Endometerial_PI;
        public bool Endometerial_PI
        {
            get { return _Endometerial_PI; }
            set
            {
                if (_Endometerial_PI != value)
                {
                    _Endometerial_PI = value;
                    OnPropertyChanged("Endometerial_PI");
                }
            }
        }

        private bool _Endometerial_RI;
        public bool Endometerial_RI
        {
            get { return _Endometerial_RI; }
            set
            {
                if (_Endometerial_RI != value)
                {
                    _Endometerial_RI = value;
                    OnPropertyChanged("Endometerial_RI");
                }
            }
        }

        private bool _Endometerial_SD;
        public bool Endometerial_SD
        {
            get { return _Endometerial_SD; }
            set
            {
                if (_Endometerial_SD != value)
                {
                    _Endometerial_SD = value;
                    OnPropertyChanged("Endometerial_SD");
                }
            }
        }
        private long _CatheterTypeID;
        public long CatheterTypeID
        {
            get { return _CatheterTypeID; }
            set
            {
                if (_CatheterTypeID != value)
                {
                    _CatheterTypeID = value;
                    OnPropertyChanged("CatheterTypeID");
                }
            }
        }

        private decimal _DistanceFundus;
        public decimal DistanceFundus
        {
            get { return _DistanceFundus; }
            set
            {
                if (_DistanceFundus != value)
                {
                    _DistanceFundus = value;
                    OnPropertyChanged("DistanceFundus");
                }
            }
        }

        private bool _TeatmentUnderGA;
        public bool TeatmentUnderGA
        {
            get { return _TeatmentUnderGA; }
            set
            {
                if (_TeatmentUnderGA != value)
                {
                    _TeatmentUnderGA = value;
                    OnPropertyChanged("TeatmentUnderGA");
                }
            }
        }

        private bool _Difficulty;
        public bool Difficulty
        {
            get { return _Difficulty; }
            set
            {
                if (_Difficulty != value)
                {
                    _Difficulty = value;
                    OnPropertyChanged("Difficulty");
                }
            }
        }

        private long _DifficultyID;
        public long DifficultyID
        {
            get { return _DifficultyID; }
            set
            {
                if (_DifficultyID != value)
                {
                    _DifficultyID = value;
                    OnPropertyChanged("DifficultyID");
                }
            }
        }


        private bool _TenaculumUsed;
        public bool TenaculumUsed
        {
            get { return _TenaculumUsed; }
            set
            {
                if (_TenaculumUsed != value)
                {
                    _TenaculumUsed = value;
                    OnPropertyChanged("TenaculumUsed");
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

        private bool _IsOnlyET;
        public bool IsOnlyET
        {
            get { return _IsOnlyET; }
            set
            {
                if (_IsOnlyET != value)
                {
                    _IsOnlyET = value;
                    OnPropertyChanged("IsOnlyET");
                }
            }
        }
        private long _AnethetistID;
        public long AnethetistID
        {
            get { return _AnethetistID; }
            set
            {
                if (_AnethetistID != value)
                {
                    _AnethetistID = value;
                    OnPropertyChanged("AnethetistID");
                }
            }
        }
        private long _AssistantAnethetistID;
        public long AssistantAnethetistID
        {
            get { return _AssistantAnethetistID; }
            set
            {
                if (_AssistantAnethetistID != value)
                {
                    _AssistantAnethetistID = value;
                    OnPropertyChanged("AssistantAnethetistID");
                }
            }
        }
        private long _SrcOoctyID;
        public long SrcOoctyID
        {
            get { return _SrcOoctyID; }
            set
            {
                if (_SrcOoctyID != value)
                {
                    _SrcOoctyID = value;
                    OnPropertyChanged("SrcOoctyID");
                }
            }
        }
        private long _SrcSemenID;
        public long SrcSemenID
        {
            get { return _SrcSemenID; }
            set
            {
                if (_SrcSemenID != value)
                {
                    _SrcSemenID = value;
                    OnPropertyChanged("SrcSemenID");
                }
            }
        }
        private string _SrcOoctyCode;
        public string SrcOoctyCode
        {
            get { return _SrcOoctyCode; }
            set
            {
                if (_SrcOoctyCode != value)
                {
                    _SrcOoctyCode = value;
                    OnPropertyChanged("SrcOoctyCode");
                }
            }
        }
        private string _SrcSemenCode;
        public string SrcSemenCode
        {
            get { return _SrcSemenCode; }
            set
            {
                if (_SrcSemenCode != value)
                {
                    _SrcSemenCode = value;
                    OnPropertyChanged("SrcSemenCode");
                }
            }
        }

        //added by neena
        private bool _IsAnesthesia;
        public bool IsAnesthesia
        {
            get { return _IsAnesthesia; }
            set
            {
                if (_IsAnesthesia != value)
                {
                    _IsAnesthesia = value;
                    OnPropertyChanged("IsAnesthesia");
                }
            }
        }

        private long _FreshEmb;
        public long FreshEmb
        {
            get { return _FreshEmb; }
            set
            {
                if (_FreshEmb != value)
                {
                    _FreshEmb = value;
                    OnPropertyChanged("FreshEmb");
                }
            }
        }

        private long _FrozenEmb;
        public long FrozenEmb
        {
            get { return _FrozenEmb; }
            set
            {
                if (_FrozenEmb != value)
                {
                    _FrozenEmb = value;
                    OnPropertyChanged("FrozenEmb");
                }
            }
        }       

        //

        #endregion
    }

    public class clsIVFDashboard_EmbryoTransferDetailsVO : IValueObject, INotifyPropertyChanged
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
        private long _ET_ID;
        public long ET_ID
        {
            get { return _ET_ID; }
            set
            {
                if (_ET_ID != value)
                {
                    _ET_ID = value;
                    OnPropertyChanged("ET_ID");
                }
            }
        }
        private long _ET_UnitID;
        public long ET_UnitID
        {
            get { return _ET_UnitID; }
            set
            {
                if (_ET_UnitID != value)
                {
                    _ET_UnitID = value;
                    OnPropertyChanged("ET_UnitID");
                }
            }
        }
        private long _OocyteNumber;
        public long OocyteNumber
        {
            get { return _OocyteNumber; }
            set
            {
                if (_OocyteNumber != value)
                {
                    _OocyteNumber = value;
                    OnPropertyChanged("OocyteNumber");
                }
            }
        }
        private long _SerialOocyteNumber;
        public long SerialOocyteNumber
        {
            get { return _SerialOocyteNumber; }
            set
            {
                if (_SerialOocyteNumber != value)
                {
                    _SerialOocyteNumber = value;
                    OnPropertyChanged("SerialOocyteNumber");
                }
            }
        }
        private DateTime? _Date = DateTime.Now;
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
        private string _TransferDay = string.Empty;
        public string TransferDay
        {
            get { return _TransferDay; }
            set
            {
                if (_TransferDay != value)
                {
                    _TransferDay = value;
                    OnPropertyChanged("TransferDay");
                }
            }
        }

        private DateTime? _TransferDate = DateTime.Now;
        public DateTime? TransferDate
        {
            get { return _TransferDate; }
            set
            {
                if (_TransferDate != value)
                {
                    _TransferDate = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private long _GradeID;
        public long GradeID
        {
            get { return _GradeID; }
            set
            {
                if (_GradeID != value)
                {
                    _GradeID = value;
                    OnPropertyChanged("GradeID");
                }
            }
        }

        private string _Grade;
        public string Grade
        {
            get { return _Grade; }
            set
            {
                if (_Grade != value)
                {
                    _Grade = value;
                    OnPropertyChanged("Grade");
                }
            }
        }

        private string _FertStage;
        public string FertStage
        {
            get { return _FertStage; }
            set
            {
                if (_FertStage != value)
                {
                    _FertStage = value;
                    OnPropertyChanged("FertStage");
                }
            }
        }
        public string FertilizationStage { get; set; }
        public long FertilizationStageID { get; set; }
        private string _Score = string.Empty;
        public string Score
        {
            get { return _Score; }
            set
            {
                if (_Score != value)
                {
                    _Score = value;
                    OnPropertyChanged("Score");
                }
            }
        }

        private string _OocNo;
        public string OocNo
        {
            get { return _OocNo; }
            set
            {
                if (_OocNo != value)
                {
                    _OocNo = value;
                    OnPropertyChanged("OocNo");
                }
            }
        }
        private long _FertStageID;
        public long FertStageID
        {
            get { return _FertStageID; }
            set
            {
                if (_FertStageID != value)
                {
                    _FertStageID = value;
                    OnPropertyChanged("FertStageID");
                }
            }
        }

        private string _EmbStatus;
        public string EmbStatus
        {
            get { return _EmbStatus; }
            set
            {
                _EmbStatus = value;
                OnPropertyChanged("EmbStatus");
            }
        }

        public byte[] FileContents { get; set; }

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        private List<MasterListItem> _FertilizationList = new List<MasterListItem>();
        public List<MasterListItem> FertilizationList
        {
            get
            {
                return _FertilizationList;
            }
            set
            {
                _FertilizationList = value;
            }
        }

        private MasterListItem _SelectedFertilizationStage = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedFertilizationStage
        {
            get
            {
                return _SelectedFertilizationStage;
            }
            set
            {
                _SelectedFertilizationStage = value;
            }
        }

        private List<MasterListItem> _GradeList = new List<MasterListItem>();
        public List<MasterListItem> GradeList
        {
            get
            {
                return _GradeList;
            }
            set
            {
                _GradeList = value;
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
        private long _OocyteDonorID;
        public long OocyteDonorID
        {
            get { return _OocyteDonorID; }
            set
            {
                if (_OocyteDonorID != value)
                {
                    _OocyteDonorID = value;
                    OnPropertyChanged("OocyteDonorID");
                }
            }
        }

        private long _OocyteDonorUnitID;
        public long OocyteDonorUnitID
        {
            get { return _OocyteDonorUnitID; }
            set
            {
                if (_OocyteDonorUnitID != value)
                {
                    _OocyteDonorUnitID = value;
                    OnPropertyChanged("OocyteDonorUnitID");
                }
            }
        }

        
        //added by neena
        private long _SurrogateID;
        public long SurrogateID
        {
            get { return _SurrogateID; }
            set
            {
                if (_SurrogateID != value)
                {
                    _SurrogateID = value;
                    OnPropertyChanged("SurrogateID");
                }
            }
        }

        private long _SurrogateUnitID;
        public long SurrogateUnitID
        {
            get { return _SurrogateUnitID; }
            set
            {
                if (_SurrogateUnitID != value)
                {
                    _SurrogateUnitID = value;
                    OnPropertyChanged("SurrogateUnitID");
                }
            }
        }

        private string _SurrogatePatientMrNo;
        public string SurrogatePatientMrNo
        {
            get { return _SurrogatePatientMrNo; }
            set
            {
                if (_SurrogatePatientMrNo != value)
                {
                    _SurrogatePatientMrNo = value;
                    OnPropertyChanged("SurrogatePatientMrNo");
                }
            }
        }

        private bool _IsFreshEmbryoPGDPGS;
        public bool IsFreshEmbryoPGDPGS
        {
            get { return _IsFreshEmbryoPGDPGS; }
            set
            {
                if (_IsFreshEmbryoPGDPGS != value)
                {
                    _IsFreshEmbryoPGDPGS = value;
                    OnPropertyChanged("IsFreshEmbryoPGDPGS");
                }
            }
        }

        private bool _IsFrozenEmbryoPGDPGS;
        public bool IsFrozenEmbryoPGDPGS
        {
            get { return _IsFrozenEmbryoPGDPGS; }
            set
            {
                if (_IsFrozenEmbryoPGDPGS != value)
                {
                    _IsFrozenEmbryoPGDPGS = value;
                    OnPropertyChanged("IsFrozenEmbryoPGDPGS");
                }
            }
        }

        private string _PGDPGS;
        public string PGDPGS
        {
            get { return _PGDPGS; }
            set
            {
                if (_PGDPGS != value)
                {
                    _PGDPGS = value;
                    OnPropertyChanged("PGDPGS");
                }
            }
        }


        List<MasterListItem> _SurrogatePatientList = new List<MasterListItem>();
        public  List<MasterListItem> SurrogatePatientList
        {
            get
            {
                return _SurrogatePatientList;
            }
            set
            {
                _SurrogatePatientList = value;
            }
        }

        private MasterListItem _SelectedSurrogatePatientList = new MasterListItem();
        public MasterListItem SelectedSurrogatePatientList
        {
            get
            {
                return _SelectedSurrogatePatientList;
            }
            set
            {
                _SelectedSurrogatePatientList = value;
            }
        }

        private long _EmbTransferDay;
        public long EmbTransferDay
        {
            get { return _EmbTransferDay; }
            set
            {
                if (_EmbTransferDay != value)
                {
                    _EmbTransferDay = value;
                    OnPropertyChanged("EmbTransferDay");
                }
            }
        }

        private DateTime? _ServerTransferDate = DateTime.Now;
        public DateTime? ServerTransferDate
        {
            get { return _ServerTransferDate; }
            set
            {
                if (_ServerTransferDate != value)
                {
                    _ServerTransferDate = value;
                    OnPropertyChanged("ServerTransferDate");
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

        private string _StageofDevelopmentGradeDesc;
        public string StageofDevelopmentGradeDesc
        {
            get { return _StageofDevelopmentGradeDesc; }
            set
            {
                if (_StageofDevelopmentGradeDesc != value)
                {
                    _StageofDevelopmentGradeDesc = value;
                    OnPropertyChanged("StageofDevelopmentGradeDesc");
                }
            }
        }

        private string _InnerCellMassGradeDesc;
        public string InnerCellMassGradeDesc
        {
            get { return _InnerCellMassGradeDesc; }
            set
            {
                if (_InnerCellMassGradeDesc != value)
                {
                    _InnerCellMassGradeDesc = value;
                    OnPropertyChanged("InnerCellMassGradeDesc");
                }
            }
        }

        private string _TrophoectodermGradeDesc;
        public string TrophoectodermGradeDesc
        {
            get { return _TrophoectodermGradeDesc; }
            set
            {
                if (_TrophoectodermGradeDesc != value)
                {
                    _TrophoectodermGradeDesc = value;
                    OnPropertyChanged("TrophoectodermGradeDesc");
                }
            }
        }

        private long _StageofDevelopmentGrade;
        public long StageofDevelopmentGrade
        {
            get { return _StageofDevelopmentGrade; }
            set
            {
                if (_StageofDevelopmentGrade != value)
                {
                    _StageofDevelopmentGrade = value;
                    OnPropertyChanged("StageofDevelopmentGrade");
                }
            }
        }

        private long _InnerCellMassGrade;
        public long InnerCellMassGrade
        {
            get { return _InnerCellMassGrade; }
            set
            {
                if (_InnerCellMassGrade != value)
                {
                    _InnerCellMassGrade = value;
                    OnPropertyChanged("InnerCellMassGrade");
                }
            }
        }

        private long _TrophoectodermGrade;
        public long TrophoectodermGrade
        {
            get { return _TrophoectodermGrade; }
            set
            {
                if (_TrophoectodermGrade != value)
                {
                    _TrophoectodermGrade = value;
                    OnPropertyChanged("TrophoectodermGrade");
                }
            }
        }

        private long _CleavageGrade;
        public long CleavageGrade
        {
            get { return _CleavageGrade; }
            set
            {
                if (_CleavageGrade != value)
                {
                    _CleavageGrade = value;
                    OnPropertyChanged("CleavageGrade");
                }
            }
        }

        private string _CellStage;
        public string CellStage
        {
            get { return _CellStage; }
            set
            {
                if (_CellStage != value)
                {
                    _CellStage = value;
                    OnPropertyChanged("CellStage");
                }
            }
        }

        //

        #endregion
    }
}
