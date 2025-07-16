using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashBoard_ThawingVO : IValueObject, INotifyPropertyChanged
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

        private DateTime? _AddedDateTime;
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

        private DateTime? _UpdatedDateTime;
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
                    OnPropertyChanged("PlantherapyID");
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
                    OnPropertyChanged("PlantherapyUnitID");
                }
            }
        }

       private DateTime? _DateTime ;
       public DateTime? DateTime
        {
            get { return _DateTime; }
            set
            {
                if (_DateTime != value)
                {
                    _DateTime = value;
                    OnPropertyChanged("DateTime");
                }
            }
        }

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
        private bool _IsETFreezed;
        public bool IsETFreezed
        {
            get { return _IsETFreezed; }
            set
            {
                if (_IsETFreezed != value)
                {
                    _IsETFreezed = value;
                    OnPropertyChanged("IsETFreezed");
                }
            }
        }
        private bool _UsedByOtherCycle;
        public bool UsedByOtherCycle
        {
            get { return _UsedByOtherCycle; }
            set
            {
                if (_UsedByOtherCycle != value)
                {
                    _UsedByOtherCycle = value;
                    OnPropertyChanged("UsedByOtherCycle");
                }
            }
        }
        private long _UsedTherapyID;
        public long UsedTherapyID
        {
            get { return _UsedTherapyID; }
            set
            {
                if (_UsedTherapyID != value)
                {
                    _UsedTherapyID = value;
                    OnPropertyChanged("UsedTherapyID");
                }
            }
        }

        private long _UsedTherapyUnitID;
        public long UsedTherapyUnitID
        {
            get { return _UsedTherapyUnitID; }
            set
            {
                if (_UsedTherapyUnitID != value)
                {
                    _UsedTherapyUnitID = value;
                    OnPropertyChanged("UsedTherapyUnitID");
                }
            }
        }
    }

    public class clsIVFDashBoard_ThawingDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _ThawingID;
        public long ThawingID
        {
            get { return _ThawingID; }
            set
            {
                if (_ThawingID != value)
                {
                    _ThawingID = value;
                    OnPropertyChanged("ThawingID");
                }
            }
        }

        private long _ThawingUnitID;
        public long ThawingUnitID
        {
            get { return _ThawingUnitID; }
            set
            {
                if (_ThawingUnitID != value)
                {
                    _ThawingUnitID = value;
                    OnPropertyChanged("ThawingUnitID");
                }
            }
        }

        private long _EmbNumber;
        public long EmbNumber
        {
            get { return _EmbNumber; }
            set
            {
                if (_EmbNumber != value)
                {
                    _EmbNumber = value;
                    OnPropertyChanged("EmbNumber");
                }
            }
        }

       private DateTime? _DateTime ;
       public DateTime? DateTime
        {
            get { return _DateTime; }
            set
            {
                if (_DateTime != value)
                {
                    _DateTime = value;
                    OnPropertyChanged("DateTime");
                }
            }
        }

        private long _VitrificationID;
        public long VitrificationID
        {
            get { return _VitrificationID; }
            set
            {
                if (_VitrificationID != value)
                {
                    _VitrificationID = value;
                    OnPropertyChanged("VitrificationID");
                }
            }
        }

        private long _EmbSerialNumber;
        public long EmbSerialNumber
        {
            get { return _EmbSerialNumber; }
            set
            {
                if (_EmbSerialNumber != value)
                {
                    _EmbSerialNumber = value;
                    OnPropertyChanged("EmbSerialNumber");
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

        private long _StageOfDevelopmentGradeID;
        public long StageOfDevelopmentGradeID
        {
            get { return _StageOfDevelopmentGradeID; }
            set
            {
                if (_StageOfDevelopmentGradeID != value)
                {
                    _StageOfDevelopmentGradeID = value;
                    OnPropertyChanged("StageOfDevelopmentGradeID");
                }
            }
        }

        private long _InnerCellMassGradeID;
        public long InnerCellMassGradeID
        {
            get { return _InnerCellMassGradeID; }
            set
            {
                if (_InnerCellMassGradeID != value)
                {
                    _InnerCellMassGradeID = value;
                    OnPropertyChanged("InnerCellMassGradeID");
                }
            }
        }

        private long _TrophoectodermGradeID;
        public long TrophoectodermGradeID
        {
            get { return _TrophoectodermGradeID; }
            set
            {
                if (_TrophoectodermGradeID != value)
                {
                    _TrophoectodermGradeID = value;
                    OnPropertyChanged("TrophoectodermGradeID");
                }
            }
        }

        private string _EmbStatus;
        public string EmbStatus
        {
            get { return _EmbStatus; }
            set
            {
                if (_EmbStatus != value)
                {
                    _EmbStatus = value;
                    OnPropertyChanged("EmbStatus");
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

       private bool _Plan;
        public bool Plan
        {
            get { return _Plan; }
            set
            {
                if (_Plan != value)
                {
                    _Plan = value;
                    OnPropertyChanged("Plan");
                }
            }
        }

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
        private List<MasterListItem> _CellStageList = new List<MasterListItem>();
        public List<MasterListItem> CellStageList
        {
            get
            {
                return _CellStageList;
            }
            set
            {
                _CellStageList = value;
            }
        }

        private List<MasterListItem> _StageOfDevelopmentGradeList = new List<MasterListItem>();
        public List<MasterListItem> StageOfDevelopmentGradeList
        {
            get
            {
                return _StageOfDevelopmentGradeList;
            }
            set
            {
                _StageOfDevelopmentGradeList = value;
            }
        }

        private List<MasterListItem> _InnerCellMassGradeList = new List<MasterListItem>();
        public List<MasterListItem> InnerCellMassGradeList
        {
            get
            {
                return _InnerCellMassGradeList;
            }
            set
            {
                _InnerCellMassGradeList = value;
            }
        }

        private List<MasterListItem> _TrophoectodermGradeList = new List<MasterListItem>();
        public List<MasterListItem> TrophoectodermGradeList
        {
            get
            {
                return _TrophoectodermGradeList;
            }
            set
            {
                _TrophoectodermGradeList = value;
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

        private long _OocyteGradeID;
        public long OocyteGradeID
        {
            get { return _OocyteGradeID; }
            set
            {
                if (_OocyteGradeID != value)
                {
                    _OocyteGradeID = value;
                    OnPropertyChanged("OocyteGradeID");
                }
            }
        }

        private List<MasterListItem> _OocyteGradeList = new List<MasterListItem>();
        public List<MasterListItem> OocyteGradeList
        {
            get
            {
                return _OocyteGradeList;
            }
            set
            {
                _OocyteGradeList = value;
            }
        }

        private MasterListItem _SelectedOocyteGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedOocyteGrade
        {
            get
            {
                return _SelectedOocyteGrade;
            }
            set
            {
                _SelectedOocyteGrade = value;
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

        private MasterListItem _SelectedStageOfDevelopmentGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedStageOfDevelopmentGrade
        {
            get
            {
                return _SelectedStageOfDevelopmentGrade;
            }
            set
            {
                _SelectedStageOfDevelopmentGrade = value;
            }
        }
        private MasterListItem _SelectedInnerCellMassGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedInnerCellMassGrade
        {
            get
            {
                return _SelectedInnerCellMassGrade;
            }
            set
            {
                _SelectedInnerCellMassGrade = value;
            }
        }
        private MasterListItem _SelectedTrophoectodermGrade = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedTrophoectodermGrade
        {
            get
            {
                return _SelectedTrophoectodermGrade;
            }
            set
            {
                _SelectedTrophoectodermGrade = value;
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

        private bool _IsFreezeOocytes;      //Flag for fetching Thaw Oocytes under FE ICSI Cycle from Freeze All Oocytes Cycle 
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

        private string _StageOfDevelopmentGrade;
        public string StageOfDevelopmentGrade
        {
            get { return _StageOfDevelopmentGrade; }
            set
            {
                if (_StageOfDevelopmentGrade != value)
                {
                    _StageOfDevelopmentGrade = value;
                    OnPropertyChanged("StageOfDevelopmentGrade");
                }
            }
        }

        private string _InnerCellMassGrade;
        public string InnerCellMassGrade
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

        private string _TrophoectodermGrade;
        public string TrophoectodermGrade
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

        //Added By CDS 
        private List<MasterListItem> _PostThawingPlanList = new List<MasterListItem>();
        public List<MasterListItem> PostThawingPlanList
        {
            get
            {
                return _PostThawingPlanList;
            }
            set
            {
                _PostThawingPlanList = value;
            }
        }

        private MasterListItem _SelectedPostThawingPlan = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPostThawingPlan
        {
            get
            {
                return _SelectedPostThawingPlan;
            }
            set
            {
                _SelectedPostThawingPlan = value;
            }
        }

        private string _PostThawingPlan;
        public string PostThawingPlan
        {
            get { return _PostThawingPlan; }
            set
            {
                if (_PostThawingPlan != value)
                {
                    _PostThawingPlan = value;
                    OnPropertyChanged("PostThawingPlan");
                }
            }
        }

        private long _PostThawingPlanID;
        public long PostThawingPlanID
        {
            get { return _PostThawingPlanID; }
            set
            {
                if (_PostThawingPlanID != value)
                {
                    _PostThawingPlanID = value;
                    OnPropertyChanged("PostThawingPlanID");
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

        private string _TransferDay;
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

        private long _TransferDayNo;
        public long TransferDayNo
        {
            get { return _TransferDayNo; }
            set
            {
                if (_TransferDayNo != value)
                {
                    _TransferDayNo = value;
                    OnPropertyChanged("TransferDayNo");
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

        private bool _GradeEnable;
        public bool GradeEnable
        {
            get { return _GradeEnable; }
            set
            {
                if (_GradeEnable != value)
                {
                    _GradeEnable = value;
                    OnPropertyChanged("GradeEnable");
                }
            }
        }

        private bool _Grade5Enable;
        public bool Grade5Enable
        {
            get { return _Grade5Enable; }
            set
            {
                if (_Grade5Enable != value)
                {
                    _Grade5Enable = value;
                    OnPropertyChanged("Grade5Enable");
                }
            }
        }
        //END 

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
    }
}
