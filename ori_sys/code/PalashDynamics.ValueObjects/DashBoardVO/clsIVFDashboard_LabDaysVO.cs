using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_LabDaysVO : IValueObject, INotifyPropertyChanged
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

        private long _Status;
        public long Status
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

        private string _Impression;
        public string Impression
        {
            get { return _Impression; }
            set
            {
                if (_Impression != value)
                {
                    _Impression = value;
                    OnPropertyChanged("Impression");
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

        private string _OocytePreparationMedia;
        public string OocytePreparationMedia
        {
            get { return _OocytePreparationMedia; }
            set
            {
                if (_OocytePreparationMedia != value)
                {
                    _OocytePreparationMedia = value;
                    OnPropertyChanged("OocytePreparationMedia");
                }
            }
        }

        private string _SpermPreperationMedia;
        public string SpermPreperationMedia
        {
            get { return _SpermPreperationMedia; }
            set
            {
                if (_SpermPreperationMedia != value)
                {
                    _SpermPreperationMedia = value;
                    OnPropertyChanged("SpermPreperationMedia");
                }
            }
        }

        private string _FinalLayering;
        public string FinalLayering
        {
            get { return _FinalLayering; }
            set
            {
                if (_FinalLayering != value)
                {
                    _FinalLayering = value;
                    OnPropertyChanged("FinalLayering");
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
        private DateTime? _Time = DateTime.Now;
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

        private long _WitnessedEmbryologistID;
        public long WitnessedEmbryologistID
        {
            get { return _WitnessedEmbryologistID; }
            set
            {
                if (_WitnessedEmbryologistID != value)
                {
                    _WitnessedEmbryologistID = value;
                    OnPropertyChanged("WitnessedEmbryologistID");
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

        private bool _IsEmbryoCompacted;
        public bool IsEmbryoCompacted
        {
            get { return _IsEmbryoCompacted; }
            set
            {
                if (_IsEmbryoCompacted != value)
                {
                    _IsEmbryoCompacted = value;
                    OnPropertyChanged("IsEmbryoCompacted");
                }
            }
        }

        private long _AnesthetistID;
        public long AnesthetistID
        {
            get { return _AnesthetistID; }
            set
            {
                if (_AnesthetistID != value)
                {
                    _AnesthetistID = value;
                    OnPropertyChanged("AnesthetistID");
                }
            }
        }
        private long _AssitantAnesthetistID;
        public long AssitantAnesthetistID
        {
            get { return _AssitantAnesthetistID; }
            set
            {
                if (_AssitantAnesthetistID != value)
                {
                    _AssitantAnesthetistID = value;
                    OnPropertyChanged("AssitantAnesthetistID");
                }
            }
        }
        private long _CumulusID;
        public long CumulusID
        {
            get { return _CumulusID; }
            set
            {
                if (_CumulusID != value)
                {
                    _CumulusID = value;
                    OnPropertyChanged("CumulusID");
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
        private long _MOIID;
        public long MOIID
        {
            get { return _MOIID; }
            set
            {
                if (_MOIID != value)
                {
                    _MOIID = value;
                    OnPropertyChanged("MOIID");
                }
            }
        }
        private long _CellStageID;
        public long CellStageID
        {
            get { return _CellStageID; }
            set
            {
                if (_CellStageID != value)
                {
                    _CellStageID = value;
                    OnPropertyChanged("CellStageID");
                }
            }
        }
        private long _IncubatorID;
        public long IncubatorID
        {
            get { return _IncubatorID; }
            set
            {
                if (_IncubatorID != value)
                {
                    _IncubatorID = value;
                    OnPropertyChanged("IncubatorID");
                }
            }
        }
        private string _OccDiamension;
        public string OccDiamension
        {
            get { return _OccDiamension; }
            set
            {
                if (_OccDiamension != value)
                {
                    _OccDiamension = value;
                    OnPropertyChanged("OccDiamension");
                }
            }
        }
        private long _NextPlanID;
        public long NextPlanID
        {
            get { return _NextPlanID; }
            set
            {
                if (_NextPlanID != value)
                {
                    _NextPlanID = value;
                    OnPropertyChanged("NextPlanID");
                }
            }
        }
        private long _TreatmentID;
        public long TreatmentID
        {
            get { return _TreatmentID; }
            set
            {
                if (_TreatmentID != value)
                {
                    _TreatmentID = value;
                    OnPropertyChanged("TreatmentID");
                }
            }
        }
        private long _FrgmentationID;
        public long FrgmentationID
        {
            get { return _FrgmentationID; }
            set
            {
                if (_FrgmentationID != value)
                {
                    _FrgmentationID = value;
                    OnPropertyChanged("FrgmentationID");
                }
            }
        }
        private long _BlastmereSymmetryID;
        public long BlastmereSymmetryID
        {
            get { return _BlastmereSymmetryID; }
            set
            {
                if (_BlastmereSymmetryID != value)
                {
                    _BlastmereSymmetryID = value;
                    OnPropertyChanged("BlastmereSymmetryID");
                }
            }
        }
        public byte[] Photo { get; set; }
        private string _OtherDetails;
        public string OtherDetails
        {
            get { return _OtherDetails; }
            set
            {
                if (_OtherDetails != value)
                {
                    _OtherDetails = value;
                    OnPropertyChanged("OtherDetails");
                }
            }
        }

        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }

        private long _DecisionID;
        public long DecisionID
        {
            get { return _DecisionID; }
            set
            {
                if (_DecisionID != value)
                {
                    _DecisionID = value;
                    OnPropertyChanged("DecisionID");
                }
            }
        }

        private bool _AssistedHatching;
        public bool AssistedHatching
        {
            get { return _AssistedHatching; }
            set
            {
                if (_AssistedHatching != value)
                {
                    _AssistedHatching = value;
                    OnPropertyChanged("AssistedHatching");
                }
            }
        }

        private long _PlannedNoOfEmb;
        public long PlannedNoOfEmb
        {
            get { return _PlannedNoOfEmb; }
            set
            {
                if (_PlannedNoOfEmb != value)
                {
                    _PlannedNoOfEmb = value;
                    OnPropertyChanged("PlannedNoOfEmb");
                }
            }
        }
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

        private string _MBD;
        public string MBD
        {
            get { return _MBD; }
            set
            {
                if (_MBD != value)
                {
                    _MBD = value;
                    OnPropertyChanged("MBD");
                }
            }
        }
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        private string _IC;
        public string IC
        {
            get { return _IC; }
            set
            {
                if (_IC != value)
                {
                    _IC = value;
                    OnPropertyChanged("IC");
                }
            }
        }
        private long _DOSID;
        public long DOSID
        {
            get { return _DOSID; }
            set
            {
                if (_DOSID != value)
                {
                    _DOSID = value;
                    OnPropertyChanged("DOSID");
                }
            }
        }
        private long _PICID;
        public long PICID
        {
            get { return _PICID; }
            set
            {
                if (_PICID != value)
                {
                    _PICID = value;
                    OnPropertyChanged("PICID");
                }
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

        private string _Day;
        public string Day
        {
            get { return _Day; }
            set
            {
                if (_Day != value)
                {
                    _Day = value;
                    OnPropertyChanged("Day");
                }
            }
        }

        private long _DayNo;
        public long DayNo
        {
            get { return _DayNo; }
            set
            {
                if (_DayNo != value)
                {
                    _DayNo = value;
                    OnPropertyChanged("DayNo");
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
        private List<MasterListItem> _OcyteListList = new List<MasterListItem>();
        public List<MasterListItem> OcyteListList
        {
            get
            {
                return _OcyteListList;
            }
            set
            {
                _OcyteListList = value;
            }
        }


        private DateTime? _TreatmentStartDate = DateTime.Now;
        public DateTime? TreatmentStartDate
        {
            get { return _TreatmentStartDate; }
            set
            {
                if (_TreatmentStartDate != value)
                {
                    _TreatmentStartDate = value;
                    OnPropertyChanged("TreatmentStartDate");
                }
            }
        }

        private DateTime? _TreatmentEndDate = DateTime.Now;
        public DateTime? TreatmentEndDate
        {
            get { return _TreatmentEndDate; }
            set
            {
                if (_TreatmentEndDate != value)
                {
                    _TreatmentEndDate = value;
                    OnPropertyChanged("TreatmentEndDate");
                }
            }
        }

        private DateTime? _ObservationDate = DateTime.Now;
        public DateTime? ObservationDate
        {
            get { return _ObservationDate; }
            set
            {
                if (_ObservationDate != value)
                {
                    _ObservationDate = value;
                    OnPropertyChanged("ObservationDate");
                }
            }
        }

        private DateTime? _ObservationTime = DateTime.Now;
        public DateTime? ObservationTime
        {
            get { return _ObservationTime; }
            set
            {
                if (_ObservationTime != value)
                {
                    _ObservationTime = value;
                    OnPropertyChanged("ObservationTime");
                }
            }
        }

        private long _FertilizationID;
        public long FertilizationID
        {
            get { return _FertilizationID; }
            set
            {
                if (_FertilizationID != value)
                {
                    _FertilizationID = value;
                    OnPropertyChanged("FertilizationID");
                }
            }
        }

        private DateTime? _CellObservationDate = DateTime.Now;
        public DateTime? CellObservationDate
        {
            get { return _CellObservationDate; }
            set
            {
                if (_CellObservationDate != value)
                {
                    _CellObservationDate = value;
                    OnPropertyChanged("CellObservationDate");
                }
            }
        }

        private DateTime? _CellObservationTime = DateTime.Now;
        public DateTime? CellObservationTime
        {
            get { return _CellObservationTime; }
            set
            {
                if (_CellObservationTime != value)
                {
                    _CellObservationTime = value;
                    OnPropertyChanged("CellObservationTime");
                }
            }
        }

        private long _CellNo;
        public long CellNo
        {
            get { return _CellNo; }
            set
            {
                if (_CellNo != value)
                {
                    _CellNo = value;
                    OnPropertyChanged("CellNo");
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

        private string _VitrificationNo;
        public string VitrificationNo
        {
            get { return _VitrificationNo; }
            set
            {
                if (_VitrificationNo != value)
                {
                    _VitrificationNo = value;
                    OnPropertyChanged("VitrificationNo");
                }
            }
        }

        private TimeSpan? _VitrificationTime;
        public TimeSpan? VitrificationTime
        {
            get { return _VitrificationTime; }
            set
            {
                if (_VitrificationTime != value)
                {
                    _VitrificationTime = value;
                    OnPropertyChanged("VitrificationTime");
                }
            }
        }

        private DateTime? _VitrificationDate;
        public DateTime? VitrificationDate
        {
            get { return _VitrificationDate; }
            set
            {
                if (_VitrificationDate != value)
                {
                    _VitrificationDate = value;
                    OnPropertyChanged("VitrificationDate");
                }
            }
        }

        private long _OocyteMaturityID;
        public long OocyteMaturityID
        {
            get { return _OocyteMaturityID; }
            set
            {
                if (_OocyteMaturityID != value)
                {
                    _OocyteMaturityID = value;
                    OnPropertyChanged("OocyteMaturityID");
                }
            }
        }

        private long _OocyteCytoplasmDysmorphisim;
        public long OocyteCytoplasmDysmorphisim
        {
            get { return _OocyteCytoplasmDysmorphisim; }
            set
            {
                if (_OocyteCytoplasmDysmorphisim != value)
                {
                    _OocyteCytoplasmDysmorphisim = value;
                    OnPropertyChanged("OocyteCytoplasmDysmorphisim");
                }
            }
        }

        private long _ExtracytoplasmicDysmorphisim;
        public long ExtracytoplasmicDysmorphisim
        {
            get { return _ExtracytoplasmicDysmorphisim; }
            set
            {
                if (_ExtracytoplasmicDysmorphisim != value)
                {
                    _ExtracytoplasmicDysmorphisim = value;
                    OnPropertyChanged("ExtracytoplasmicDysmorphisim");
                }
            }
        }

        private long _OocyteCoronaCumulusComplex;
        public long OocyteCoronaCumulusComplex
        {
            get { return _OocyteCoronaCumulusComplex; }
            set
            {
                if (_OocyteCoronaCumulusComplex != value)
                {
                    _OocyteCoronaCumulusComplex = value;
                    OnPropertyChanged("OocyteCoronaCumulusComplex");
                }
            }
        }

        private DateTime? _ProcedureDate = DateTime.Now;
        public DateTime? ProcedureDate
        {
            get { return _ProcedureDate; }
            set
            {
                if (_ProcedureDate != value)
                {
                    _ProcedureDate = value;
                    OnPropertyChanged("ProcedureDate");
                }
            }
        }

        private DateTime? _ProcedureTime = DateTime.Now;
        public DateTime? ProcedureTime
        {
            get { return _ProcedureTime; }
            set
            {
                if (_ProcedureTime != value)
                {
                    _ProcedureTime = value;
                    OnPropertyChanged("ProcedureTime");
                }
            }
        }

        private long _SourceOfSperm;
        public long SourceOfSperm
        {
            get { return _SourceOfSperm; }
            set
            {
                if (_SourceOfSperm != value)
                {
                    _SourceOfSperm = value;
                    OnPropertyChanged("SourceOfSperm");
                }
            }
        }

        private long _OocyteZonaID;
        public long OocyteZonaID
        {
            get { return _OocyteZonaID; }
            set
            {
                if (_OocyteZonaID != value)
                {
                    _OocyteZonaID = value;
                    OnPropertyChanged("OocyteZonaID");
                }
            }
        }

        private string _OocyteZona;
        public string OocyteZona
        {
            get { return _OocyteZona; }
            set
            {
                if (_OocyteZona != value)
                {
                    _OocyteZona = value;
                    OnPropertyChanged("OocyteZona");
                }
            }
        }

        private long _PVSID;
        public long PVSID
        {
            get { return _PVSID; }
            set
            {
                if (_PVSID != value)
                {
                    _PVSID = value;
                    OnPropertyChanged("PVSID");
                }
            }
        }

        private string _PVS;
        public string PVS
        {
            get { return _PVS; }
            set
            {
                if (_PVS != value)
                {
                    _PVS = value;
                    OnPropertyChanged("PVS");
                }
            }
        }

        private long _IstPBID;
        public long IstPBID
        {
            get { return _IstPBID; }
            set
            {
                if (_IstPBID != value)
                {
                    _IstPBID = value;
                    OnPropertyChanged("IstPBID");
                }
            }
        }

        private string _IstPB;
        public string IstPB
        {
            get { return _IstPB; }
            set
            {
                if (_IstPB != value)
                {
                    _IstPB = value;
                    OnPropertyChanged("IstPB");
                }
            }
        }

        private long _CytoplasmID;
        public long CytoplasmID
        {
            get { return _CytoplasmID; }
            set
            {
                if (_CytoplasmID != value)
                {
                    _CytoplasmID = value;
                    OnPropertyChanged("CytoplasmID");
                }
            }
        }

        private string _Cytoplasm ;
        public string Cytoplasm
        {
            get { return _Cytoplasm ; }
            set
            {
                if (_Cytoplasm != value)
                {
                    _Cytoplasm = value;
                    OnPropertyChanged("Cytoplasm");
                }
            }
        }

        private long _SpermCollectionMethod;
        public long SpermCollectionMethod
        {
            get { return _SpermCollectionMethod; }
            set
            {
                if (_SpermCollectionMethod != value)
                {
                    _SpermCollectionMethod = value;
                    OnPropertyChanged("SpermCollectionMethod");
                }
            }
        }

        private string _SemenSample;
        public string SemenSample
        {
            get { return _SemenSample; }
            set
            {
                if (_SemenSample != value)
                {
                    _SemenSample = value;
                    OnPropertyChanged("SemenSample");
                }
            }
        }

        private string _SemenSampleICSI;
        public string SemenSampleICSI
        {
            get { return _SemenSampleICSI; }
            set
            {
                if (_SemenSampleICSI != value)
                {
                    _SemenSampleICSI = value;
                    OnPropertyChanged("SemenSampleICSI");
                }
            }
        }

        private bool _IMSI;
        public bool IMSI
        {
            get { return _IMSI; }
            set
            {
                if (_IMSI != value)
                {
                    _IMSI = value;
                    OnPropertyChanged("IMSI");
                }
            }
        }

        private bool _Embryoscope;
        public bool Embryoscope
        {
            get { return _Embryoscope; }
            set
            {
                if (_Embryoscope != value)
                {
                    _Embryoscope = value;
                    OnPropertyChanged("Embryoscope");
                }
            }
        }

        private string _DiscardReason;
        public string DiscardReason
        {
            get { return _DiscardReason; }
            set
            {
                if (_DiscardReason != value)
                {
                    _DiscardReason = value;
                    OnPropertyChanged("DiscardReason");
                }
            }
        }

        private string _DonorCode;
        public string DonorCode
        {
            get { return _DonorCode; }
            set
            {
                if (_DonorCode != value)
                {
                    _DonorCode = value;
                    OnPropertyChanged("DonorCode");
                }
            }
        }

        private string _RecepientPatientName;
        public string RecepientPatientName
        {
            get { return _RecepientPatientName; }
            set
            {
                if (_RecepientPatientName != value)
                {
                    _RecepientPatientName = value;
                    OnPropertyChanged("RecepientPatientName");
                }
            }
        }

        private string _RecepientMrNO;
        public string RecepientMrNO
        {
            get { return _RecepientMrNO; }
            set
            {
                if (_RecepientMrNO != value)
                {
                    _RecepientMrNO = value;
                    OnPropertyChanged("RecepientMrNO");
                }
            }
        }

        public long PlannedTreatmentID { get; set; } 

        private bool _IsFreezeOocytes;      //Flag set while saving Freeze Oocytes under Freeze All Oocytes Cycle 
        public bool IsFreezeOocytes
        {
            get { return _IsFreezeOocytes; }
            set
            {
                if (_IsFreezeOocytes != value)
                {
                    _IsFreezeOocytes = value;
                    OnPropertyChanged("IsFreezeOocytes");
                }
            }
        }

        private string _SampleNo;
        public string SampleNo
        {
            get { return _SampleNo; }
            set
            {
                if (_SampleNo != value)
                {
                    _SampleNo = value;
                    OnPropertyChanged("SampleNo");
                }
            }
        }

        private bool _IsDonate;
        public bool IsDonate
        {
            get { return _IsDonate; }
            set
            {
                if (_IsDonate != value)
                {
                    _IsDonate = value;
                    OnPropertyChanged("IsDonate");
                }
            }
        }

        private bool _IsDonateCryo;
        public bool IsDonateCryo
        {
            get { return _IsDonateCryo; }
            set
            {
                if (_IsDonateCryo != value)
                {
                    _IsDonateCryo = value;
                    OnPropertyChanged("IsDonateCryo");
                }
            }
        }

        private bool _IsDonorCycleDonate;
        public bool IsDonorCycleDonate
        {
            get { return _IsDonorCycleDonate; }
            set
            {
                if (_IsDonorCycleDonate != value)
                {
                    _IsDonorCycleDonate = value;
                    OnPropertyChanged("IsDonorCycleDonate");
                }
            }
        }

        private bool _IsDonorCycleDonateCryo;
        public bool IsDonorCycleDonateCryo
        {
            get { return _IsDonorCycleDonateCryo; }
            set
            {
                if (_IsDonorCycleDonateCryo != value)
                {
                    _IsDonorCycleDonateCryo = value;
                    OnPropertyChanged("IsDonorCycleDonateCryo");
                }
            }
        }

        private long _RecepientPatientID;
        public long RecepientPatientID
        {
            get { return _RecepientPatientID; }
            set
            {
                if (_RecepientPatientID != value)
                {
                    _RecepientPatientID = value;
                    OnPropertyChanged("RecepientPatientID");
                }
            }
        }

        private long _RecepientPatientUnitID;
        public long RecepientPatientUnitID
        {
            get { return _RecepientPatientUnitID; }
            set
            {
                if (_RecepientPatientUnitID != value)
                {
                    _RecepientPatientUnitID = value;
                    OnPropertyChanged("RecepientPatientUnitID");
                }
            }
        }

        private List<clsAddImageVO> _ImgList = new List<clsAddImageVO>();
        public List<clsAddImageVO> ImgList
        {
            get
            {
                return _ImgList;
            }
            set
            {
                _ImgList = value;
            }
        }

        private List<clsIVFDashboard_TherapyDocumentVO> _DetailList = new List<clsIVFDashboard_TherapyDocumentVO>();
        public List<clsIVFDashboard_TherapyDocumentVO> DetailList
        {
            get
            {
                return _DetailList;
            }
            set
            {
                _DetailList = value;
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

        private DateTime? _PlanDate ;
        public DateTime? PlanDate
        {
            get { return _PlanDate; }
            set
            {
                if (_PlanDate != value)
                {
                    _PlanDate = value;
                    OnPropertyChanged("PlanDate");
                }
            }
        }

        private bool _IsFrozenEmbryo;
        public bool IsFrozenEmbryo
        {
            get { return _IsFrozenEmbryo; }
            set
            {
                if (_IsFrozenEmbryo != value)
                {
                    _IsFrozenEmbryo = value;
                }
            }
        }

        private bool _IsFreshEmbryo;
        public bool IsFreshEmbryo
        {
            get { return _IsFreshEmbryo; }
            set
            {
                if (_IsFreshEmbryo != value)
                {
                    _IsFreshEmbryo = value;
                }
            }
        }
        

        private bool _IsBiopsy;
        public bool IsBiopsy
        {
            get { return _IsBiopsy; }
            set
            {
                if (_IsBiopsy != value)
                {
                    _IsBiopsy = value;
                    OnPropertyChanged("IsBiopsy");
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

        private DateTime? _BiopsyDate = DateTime.Now;
        public DateTime? BiopsyDate
        {
            get { return _BiopsyDate; }
            set
            {
                if (_BiopsyDate != value)
                {
                    _BiopsyDate = value;
                    OnPropertyChanged("BiopsyDate");
                }
            }
        }

        private DateTime? _BiopsyTime = DateTime.Now;
        public DateTime? BiopsyTime
        {
            get { return _BiopsyTime; }
            set
            {
                if (_BiopsyTime != value)
                {
                    _BiopsyTime = value;
                    OnPropertyChanged("BiopsyTime");
                }
            }
        }

        private long _NoOfCell;
        public long NoOfCell
        {
            get { return _NoOfCell; }
            set
            {
                if (_NoOfCell != value)
                {
                    _NoOfCell = value;
                    OnPropertyChanged("NoOfCell");
                }
            }
        }

      
        #endregion
    }


    //added by neena
    public class clsIVFDashboard_FertCheck : IValueObject, INotifyPropertyChanged
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

        private long _Status;
        public long Status
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

        //added by neena 
        private List<MasterListItem> _OcyteListList = new List<MasterListItem>();
        public List<MasterListItem> OcyteListList
        {
            get
            {
                return _OcyteListList;
            }
            set
            {
                _OcyteListList = value;
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
        private DateTime? _Date ;
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
        
        private bool _Isfreezed;
        public bool Isfreezed
        {
            get { return _Isfreezed; }
            set
            {
                if (_Isfreezed != value)
                {
                    _Isfreezed = value;
                    OnPropertyChanged("Isfreezed");
                }
            }
        }

        private long _FertCheck;
        public long FertCheck
        {
            get { return _FertCheck; }
            set
            {
                if (_FertCheck != value)
                {
                    _FertCheck = value;
                    OnPropertyChanged("FertCheck");
                }
            }
        }
        private long _FertCheckResult;
        public long FertCheckResult
        {
            get { return _FertCheckResult; }
            set
            {
                if (_FertCheckResult != value)
                {
                    _FertCheckResult = value;
                    OnPropertyChanged("FertCheckResult");
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
       


        #endregion
    }
    //
}
