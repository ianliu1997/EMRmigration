using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsFemaleLabDay5VO : IValueObject, INotifyPropertyChanged
    {
        public clsLabDaysSummaryVO LabDaySummary { get; set; }

        public List<clsFemaleLabDay5FertilizationAssesmentVO> FertilizationAssesmentDetails { get; set; }
        
        public List<FileUpload> FUSetting { get; set; }
       
        public clsFemaleSemenDetailsVO SemenDetails { get; set; }

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

        private long _AssEmbryologistID;
        public long AssEmbryologistID
        {
            get { return _AssEmbryologistID; }
            set
            {
                if (_AssEmbryologistID != value)
                {
                    _AssEmbryologistID = value;
                    OnPropertyChanged("AssEmbryologistID");
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

        private long _AssAnesthetistID;
        public long AssAnesthetistID
        {
            get { return _AssAnesthetistID; }
            set
            {
                if (_AssAnesthetistID != value)
                {
                    _AssAnesthetistID = value;
                    OnPropertyChanged("AssAnesthetistID");
                }
            }
        }

        private long _IVFCycleCount;
        public long IVFCycleCount
        {
            get { return _IVFCycleCount; }
            set
            {
                if (_IVFCycleCount != value)
                {
                    _IVFCycleCount = value;
                    OnPropertyChanged("IVFCycleCount");
                }
            }
        }


        private string _InfectionObserved;
        public string InfectionObserved
        {
            get { return _InfectionObserved; }
            set
            {
                if (_InfectionObserved != value)
                {
                    _InfectionObserved = value;
                    OnPropertyChanged("InfectionObserved");
                }
            }
        }


        private long _SourceNeedleID;
        public long SourceNeedleID
        {
            get { return _SourceNeedleID; }
            set
            {
                if (_SourceNeedleID != value)
                {
                    _SourceNeedleID = value;
                    OnPropertyChanged("SourceNeedleID");
                }
            }
        }


        private int _TotNoOfOocytes;
        public int TotNoOfOocytes
        {
            get { return _TotNoOfOocytes; }
            set
            {
                if (_TotNoOfOocytes != value)
                {
                    _TotNoOfOocytes = value;
                    OnPropertyChanged("TotNoOfOocytes");
                }
            }
        }


        private int _TotNoOf2PN;
        public int TotNoOf2PN
        {
            get { return _TotNoOf2PN; }
            set
            {
                if (_TotNoOf2PN != value)
                {
                    _TotNoOf2PN = value;
                    OnPropertyChanged("TotNoOf2PN");
                }
            }
        }

        private int _TotNoOf3PN;
        public int TotNoOf3PN
        {
            get { return _TotNoOf3PN; }
            set
            {
                if (_TotNoOf3PN != value)
                {
                    _TotNoOf3PN = value;
                    OnPropertyChanged("TotNoOf3PN ");
                }
            }
        }

        private int _TotNoOf2PB;
        public int TotNoOf2PB
        {
            get { return _TotNoOf2PB; }
            set
            {
                if (_TotNoOf2PB != value)
                {
                    _TotNoOf2PB = value;
                    OnPropertyChanged("TotNoOf2PB ");
                }
            }
        }

        private int _ToNoOfMI;
        public int ToNoOfMI
        {
            get { return _ToNoOfMI; }
            set
            {
                if (_ToNoOfMI != value)
                {
                    _ToNoOfMI = value;
                    OnPropertyChanged("ToNoOfMI");
                }
            }
        }

        private int _ToNoOfMII;
        public int ToNoOfMII
        {
            get { return _ToNoOfMII; }
            set
            {
                if (_ToNoOfMII != value)
                {
                    _ToNoOfMII = value;
                    OnPropertyChanged("ToNoOfMII");
                }
            }
        }

        private int _ToNoOfGV;
        public int ToNoOfGV
        {
            get { return _ToNoOfGV; }
            set
            {
                if (_ToNoOfGV != value)
                {
                    _ToNoOfGV = value;
                    OnPropertyChanged("ToNoOfGV");
                }
            }
        }

        private int _ToNoOfDeGenerated;
        public int ToNoOfDeGenerated
        {
            get { return _ToNoOfDeGenerated; }
            set
            {
                if (_ToNoOfDeGenerated != value)
                {
                    _ToNoOfDeGenerated = value;
                    OnPropertyChanged("ToNoOfDeGenerated");
                }
            }
        }


        private int _ToNoOfLost;
        public int ToNoOfLost
        {
            get { return _ToNoOfLost; }
            set
            {
                if (_ToNoOfLost != value)
                {
                    _ToNoOfLost = value;
                    OnPropertyChanged("ToNoOfLost");
                }
            }
        }


        private long _SrcOfOocyteID;
        public long SrcOfOocyteID
        {
            get { return _SrcOfOocyteID; }
            set
            {
                if (_SrcOfOocyteID != value)
                {
                    _SrcOfOocyteID = value;
                    OnPropertyChanged("SrcOfOocyteID");
                }
            }
        }

        private long _SrcOfSemenID;
        public long SrcOfSemenID
        {
            get { return _SrcOfSemenID; }
            set
            {
                if (_SrcOfSemenID != value)
                {
                    _SrcOfSemenID = value;
                    OnPropertyChanged("SrcOfSemenID");
                }
            }
        }

        private long _TreatmentTypeID;
        public long TreatmentTypeID
        {
            get { return _TreatmentTypeID; }
            set
            {
                if (_TreatmentTypeID != value)
                {
                    _TreatmentTypeID = value;
                    OnPropertyChanged("TreatmentTypeID");
                }
            }
        }

        private DateTime? _ICSICompletionTime = null;
        public DateTime? ICSICompletionTime
        {
            get { return _ICSICompletionTime; }
            set
            {
                if (_ICSICompletionTime != value)
                {
                    _ICSICompletionTime = value;
                    OnPropertyChanged("ICSICompletionTime");
                }
            }
        }

        private long _SourceOfDenudingNeedle;
        public long SourceOfDenudingNeedle
        {
            get { return _SourceOfDenudingNeedle; }
            set
            {
                if (_SourceOfDenudingNeedle != value)
                {
                    _SourceOfDenudingNeedle = value;
                    OnPropertyChanged("SourceOfDenudingNeedle");
                }
            }
        }

        private DateTime? _FertilizationCheckTime;
        public DateTime? FertilizationCheckTime
        {
            get { return _FertilizationCheckTime; }
            set
            {
                if (_FertilizationCheckTime != value)
                {
                    _FertilizationCheckTime = value;
                    OnPropertyChanged("FertilizationCheckTime");
                }
            }
        }

        public bool IsFreezed { get; set; }
       
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


    public class clsFemaleLabDay5FertilizationAssesmentVO : IValueObject, INotifyPropertyChanged
    {
        public List<clsFemaleMediaDetailsVO> MediaDetails { get; set; }

        public clsFemaleLabDay5CalculateDetailsVO Day5CalculateDetails { get; set; }


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

        private long _FertilisationStage;
        public long FertilisationStage
        {
            get { return _FertilisationStage; }
            set
            {
                if (_FertilisationStage != value)
                {
                    _FertilisationStage = value;
                    OnPropertyChanged("FertilisationStage");
                }
            }
        }


        private long _Score;
        public long Score
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

        private bool _PV;
        public bool PV
        {
            get { return _PV; }
            set
            {
                if (_PV != value)
                {
                    _PV = value;
                    OnPropertyChanged("PV");
                }
            }
        }


        private bool _XFactor;
        public bool XFactor
        {
            get { return _XFactor; }
            set
            {
                if (_XFactor != value)
                {
                    _XFactor = value;
                    OnPropertyChanged("XFactor");
                }
            }
        }

        private bool _ProceedDay6;
        public bool ProceedDay6
        {
            get { return _ProceedDay6; }
            set
            {
                if (_ProceedDay6 != value)
                {
                    _ProceedDay6 = value;
                    OnPropertyChanged("ProceedDay6");
                }
            }
        }



        List<MasterListItem> _Grade = new List<MasterListItem>();
        public List<MasterListItem> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                if (value != _Grade)
                {
                    _Grade = value;
                }
            }

        }

        MasterListItem _SelectedGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGrade
        {
            get
            {
                return _SelectedGrade;
            }
            set
            {
                if (value != _SelectedGrade)
                {
                    _SelectedGrade = value;
                    OnPropertyChanged("SelectedGrade");
                }
            }


        }

        List<MasterListItem> _ICM = new List<MasterListItem>();
        public List<MasterListItem> ICM
        {
            get
            {
                return _ICM;
            }
            set
            {
                if (value != _ICM)
                {
                    _ICM = value;
                }
            }

        }

        MasterListItem _SelectedICM = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedICM
        {
            get
            {
                return _SelectedICM;
            }
            set
            {
                if (value != _SelectedICM)
                {
                    _SelectedICM = value;
                    OnPropertyChanged("SelectedICM");
                }
            }


        }

        List<MasterListItem> _Expansion = new List<MasterListItem>();
        public List<MasterListItem> Expansion 
        {
            get
            {
                return _Expansion;
            }
            set
            {
                if (value != _Expansion)
                {
                    _Expansion = value;
                }
            }

        }

        MasterListItem _SelectedExpansion = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedExpansion 
        {
            get
            {
                return _SelectedExpansion;
            }
            set
            {
                if (value != _SelectedExpansion)
                {
                    _SelectedExpansion = value;
                    OnPropertyChanged("SelectedExpansion ");
                }
            }


        }
        List<MasterListItem> _BlastocytsGrade = new List<MasterListItem>();
        public List<MasterListItem> BlastocytsGrade
        {
            get
            {
                return _BlastocytsGrade;
            }
            set
            {
                if (value != _BlastocytsGrade)
                {
                    _BlastocytsGrade = value;
                }
            }

        }

        MasterListItem _SelectedBlastocytsGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedBlastocytsGrade
        {
            get
            {
                return _SelectedBlastocytsGrade;
            }
            set
            {
                if (value != _SelectedBlastocytsGrade)
                {
                    _SelectedBlastocytsGrade = value;
                    OnPropertyChanged("SelectedBlastocytsGrade");
                }
            }


        }
        List<MasterListItem> _Trophectoderm = new List<MasterListItem>();
        public List<MasterListItem> Trophectoderm
        {
            get
            {
                return _Trophectoderm;
            }
            set
            {
                if (value != _Trophectoderm)
                {
                    _Trophectoderm = value;
                }
            }

        }

        MasterListItem _SelectedTrophectoderm = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedTrophectoderm
        {
            get
            {
                return _SelectedTrophectoderm;
            }
            set
            {
                if (value != _SelectedTrophectoderm)
                {
                    _SelectedTrophectoderm = value;
                    OnPropertyChanged("SelectedTrophectoderm");
                }
            }


        }
        List<MasterListItem> _FerPlan = new List<MasterListItem>();
        public List<MasterListItem> FerPlan
        {
            get
            {
                return _FerPlan;
            }
            set
            {
                if (value != _FerPlan)
                {
                    _FerPlan = value;
                }
            }

        }

        MasterListItem _SelectedFerPlan = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedFePlan
        {
            get
            {
                return _SelectedFerPlan;
            }
            set
            {
                if (value != _SelectedFerPlan)
                {
                    _SelectedFerPlan = value;
                    OnPropertyChanged("SelectedFePlan");
                }
            }


        }



        List<MasterListItem> _Plan = new List<MasterListItem>();
        public List<MasterListItem> Plan
        {
            get
            {
                return _Plan;
            }
            set
            {
                if (value != _Plan)
                {
                    _Plan = value;
                }
            }

        }

        MasterListItem _SelectedPlan = new MasterListItem { ID = 3, Description = "Followup" };
        public MasterListItem SelectedPlan
        {
            get
            {
                return _SelectedPlan;
            }
            set
            {
                if (value != _SelectedPlan)
                {
                    _SelectedPlan = value;
                    OnPropertyChanged("SelectedPlan");
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

        private long _PlanTreatmentID;
        public long PlanTreatmentID
        {
            get { return _PlanTreatmentID; }
            set
            {
                if (_PlanTreatmentID != value)
                {
                    _PlanTreatmentID = value;
                    OnPropertyChanged("PlanTreatmentID");
                }
            }
        }


        private string _PlanTreatment;
        public string PlanTreatment
        {
            get { return _PlanTreatment; }
            set
            {
                if (_PlanTreatment != value)
                {
                    _PlanTreatment = value;
                    OnPropertyChanged("PlanTreatment");
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



        private long _SrcOocyteID;
        public long SrcOocyteID
        {
            get { return _SrcOocyteID; }
            set
            {
                if (_SrcOocyteID != value)
                {
                    _SrcOocyteID = value;
                    OnPropertyChanged("SrcOocyteID");
                }
            }
        }

        private string _OocyteDonorID;
        public string OocyteDonorID
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

        private string _SemenDonorID;
        public string SemenDonorID
        {
            get { return _SemenDonorID; }
            set
            {
                if (_SemenDonorID != value)
                {
                    _SemenDonorID = value;
                    OnPropertyChanged("SemenDonorID");
                }
            }
        }



        private string _SrcOocyteDescription;
        public string SrcOocyteDescription
        {
            get { return _SrcOocyteDescription; }
            set
            {
                if (_SrcOocyteDescription != value)
                {
                    _SrcOocyteDescription = value;
                    OnPropertyChanged("SrcOocyteDescription");
                }
            }
        }


        private long _OSCode;
        public long OSCode
        {
            get { return _OSCode; }
            set
            {
                if (_OSCode != value)
                {
                    _OSCode = value;
                    OnPropertyChanged("OSCode");
                }
            }
        }


        private long _OoNo;
        public long OoNo
        {
            get { return _OoNo; }
            set
            {
                if (_OoNo != value)
                {
                    _OoNo = value;
                    OnPropertyChanged("OoNo");
                }
            }
        }
        //By Anjali..........................
        private long _SerialOccyteNo;
        public long SerialOccyteNo
        {
            get { return _SerialOccyteNo; }
            set
            {
                if (_SerialOccyteNo != value)
                {
                    _SerialOccyteNo = value;
                    OnPropertyChanged("SerialOccyteNo");
                }
            }
        }
        //..................................
        private long _SrcOfSemen;
        public long SrcOfSemen
        {
            get { return _SrcOfSemen; }
            set
            {
                if (_SrcOfSemen != value)
                {
                    _SrcOfSemen = value;
                    OnPropertyChanged("SrcOfSemen");
                }
            }
        }
        private string _SrcOfSemenDescription;
        public string SrcOfSemenDescription
        {
            get { return _SrcOfSemenDescription; }
            set
            {
                if (_SrcOfSemenDescription != value)
                {
                    _SrcOfSemenDescription = value;
                    OnPropertyChanged("SrcOfSemenDescription");
                }
            }
        }

        private long _SSCode;
        public long SSCode
        {
            get { return _SSCode; }
            set
            {
                if (_SSCode != value)
                {
                    _SSCode = value;
                    OnPropertyChanged("SSCode");
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

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                if (_FileName != value)
                {
                    _FileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        public byte[] FileContents { get; set; }

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


    public class clsFemaleLabDay5CalculateDetailsVO : IValueObject, INotifyPropertyChanged
    {
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

        private bool _BlastocoelsCavity;
        public bool BlastocoelsCavity
        {
            get { return _BlastocoelsCavity; }
            set
            {
                if (_BlastocoelsCavity != value)
                {
                    _BlastocoelsCavity = value;
                    OnPropertyChanged("BlastocoelsCavity");
                }
            }
        }

        private bool _TightlyPackedCells;
        public bool TightlyPackedCells
        {
            get { return _TightlyPackedCells; }
            set
            {
                if (_TightlyPackedCells != value)
                {
                    _TightlyPackedCells = value;
                    OnPropertyChanged("TightlyPackedCells");
                }
            }
        }

        private bool _FormingEpithelium;
        public bool FormingEpithelium
        {
            get { return _FormingEpithelium; }
            set
            {
                if (_FormingEpithelium != value)
                {
                    _FormingEpithelium = value;
                    OnPropertyChanged("FormingEpithelium");
                }
            }
        }


        public long FertilizationID { get; set; }

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
