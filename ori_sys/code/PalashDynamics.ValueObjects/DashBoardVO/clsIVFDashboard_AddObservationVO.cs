using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.ValueObjects.DashBoardVO
{   
    public class clsIVFDashboard_AddObservationVO : IValueObject, INotifyPropertyChanged
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

        #endregion
    }
}
