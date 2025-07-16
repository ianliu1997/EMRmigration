using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.EMR;
using System.IO;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsEmbryoTransferVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Declaration Section
        
        public List<FileUpload> FUSetting { get; set; }
        public clsLabDaysSummaryVO LabDaySummary { get; set; }
        public List<clsEmbryoTransferDetailsVO> Details { get; set; }

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

        private DateTime? _ProcDate;
        public DateTime? ProcDate
        {
            get { return _ProcDate; }
            set
            {
                if (_ProcDate != value)
                {
                    _ProcDate = value;
                    OnPropertyChanged("ProcDate");
                }
            }
        }

        private DateTime? _ProcTime;
        public DateTime? ProcTime
        {
            get { return _ProcTime; }
            set
            {
                if (_ProcTime != value)
                {
                    _ProcTime = value;
                    OnPropertyChanged("ProcTime");
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

        private bool _IsTreatmentUnderGA;
        public bool IsTreatmentUnderGA
        {
            get { return _IsTreatmentUnderGA; }
            set
            {
                if (_IsTreatmentUnderGA != value)
                {
                    _IsTreatmentUnderGA = value;
                    OnPropertyChanged("IsTreatmentUnderGA");
                }
            }
        }

        private bool _IsDifficult;
        public bool IsDifficult
        {
            get { return _IsDifficult; }
            set
            {
                if (_IsDifficult != value)
                {
                    _IsDifficult = value;
                    OnPropertyChanged("IsDifficult");
                }
            }
        }

        private long? _DifficultyTypeID;
        public long? DifficultyTypeID
        {
            get { return _DifficultyTypeID; }
            set
            {
                if (_DifficultyTypeID != value)
                {
                    _DifficultyTypeID = value;
                    OnPropertyChanged("DifficultyTypeID");
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

    public class clsEmbryoTransferDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _EmbryoNumber;
        public long EmbryoNumber
        {
            get { return _EmbryoNumber; }
            set
            {
                if (_EmbryoNumber != value)
                {
                    _EmbryoNumber = value;
                    OnPropertyChanged("EmbryoNumber");
                }
            }
        }

        private long _RecID;
        public long RecID
        {
            get { return _RecID; }
            set
            {
                if (_RecID != value)
                {
                    _RecID = value;
                    OnPropertyChanged("RecID");
                }
            }
        }

        private IVFLabDay _TransferDay;
        public IVFLabDay TransferDay
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

        private DateTime? _TransferDate;
        public DateTime? TransferDate
        {
            get { return _TransferDate; }
            set
            {
                if (_TransferDate != value)
                {
                    _TransferDate = value;
                    OnPropertyChanged("TransferDate");
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

        private int _Score;
        public int Score
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

        private long _FertilizationStageID;
        public long FertilizationStageID
        {
            get { return _FertilizationStageID; }
            set
            {
                if (_FertilizationStageID != value)
                {
                    _FertilizationStageID = value;
                    OnPropertyChanged("FertilizationStageID");
                }
            }
        }

        private string _FertilizationStage;
        public string FertilizationStage
        {
            get { return _FertilizationStage; }
            set
            {
                if (_FertilizationStage != value)
                {
                    _FertilizationStage = value;
                    OnPropertyChanged("FertilizationStage");
                }
            }
        }

        private string _EmbryoStatus;
        public string EmbryoStatus
        {
            get { return _EmbryoStatus; }
            set
            {
                if (_EmbryoStatus != value)
                {
                    _EmbryoStatus = value;
                    OnPropertyChanged("EmbryoStatus");
                }
            }
        }

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
