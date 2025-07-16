using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_OPUVO : IValueObject, INotifyPropertyChanged
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
        public override string ToString()
        {
            return Description;
        }
        #region Properties
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
        private long _OPUID;
        public long OPUID
        {
            get { return _OPUID; }
            set
            {
                if (_OPUID != value)
                {
                    _OPUID = value;
                    OnPropertyChanged("OPUID");
                }
            }
        }
        public string Description { get; set; }
        private long _OocyteForED;
        public long OocyteForED
        {
            get { return _OocyteForED; }
            set
            {
                if (_OocyteForED != value)
                {
                    _OocyteForED = value;
                    OnPropertyChanged("OocyteForED");
                }
            }
        }

        private long _BalanceOocyte;
        public long BalanceOocyte
        {
            get { return _BalanceOocyte; }
            set
            {
                if (_BalanceOocyte != value)
                {
                    _BalanceOocyte = value;
                    OnPropertyChanged("BalanceOocyte");
                }
            }
        }
        private bool _IsSetForED;
        public bool IsSetForED
        {
            get { return _IsSetForED; }
            set
            {
                if (_IsSetForED != value)
                {
                    _IsSetForED = value;
                    OnPropertyChanged("IsSetForED");
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


        private decimal _MI;
        public decimal MI
        {
            get { return _MI; }
            set
            {
                if (_MI != value)
                {
                    _MI = value;
                    OnPropertyChanged("MI");
                }
            }
        }
        private decimal _MII;
        public decimal MII
        {
            get { return _MII; }
            set
            {
                if (_MII != value)
                {
                    _MII = value;
                    OnPropertyChanged("MII");
                }
            }
        }
        private decimal _GV;
        public decimal GV
        {
            get { return _GV; }
            set
            {
                if (_GV != value)
                {
                    _GV = value;
                    OnPropertyChanged("GV");
                }
            }
        }

        //added by neena
        private decimal _OocyteCytoplasmDysmorphisimPresent;
        public decimal OocyteCytoplasmDysmorphisimPresent
        {
            get { return _OocyteCytoplasmDysmorphisimPresent; }
            set
            {
                if (_OocyteCytoplasmDysmorphisimPresent != value)
                {
                    _OocyteCytoplasmDysmorphisimPresent = value;
                    OnPropertyChanged("OocyteCytoplasmDysmorphisimPresent");
                }
            }
        }

        private decimal _OocyteCytoplasmDysmorphisimAbsent;
        public decimal OocyteCytoplasmDysmorphisimAbsent
        {
            get { return _OocyteCytoplasmDysmorphisimAbsent; }
            set
            {
                if (_OocyteCytoplasmDysmorphisimAbsent != value)
                {
                    _OocyteCytoplasmDysmorphisimAbsent = value;
                    OnPropertyChanged("OocyteCytoplasmDysmorphisimAbsent");
                }
            }
        }

        private decimal _ExtracytoplasmicDysmorphisimPresent;
        public decimal ExtracytoplasmicDysmorphisimPresent
        {
            get { return _ExtracytoplasmicDysmorphisimPresent; }
            set
            {
                if (_ExtracytoplasmicDysmorphisimPresent != value)
                {
                    _ExtracytoplasmicDysmorphisimPresent = value;
                    OnPropertyChanged("_ExtracytoplasmicDysmorphisimPresent");
                }
            }
        }

        private decimal _ExtracytoplasmicDysmorphisimAbsent;
        public decimal ExtracytoplasmicDysmorphisimAbsent
        {
            get { return _ExtracytoplasmicDysmorphisimAbsent; }
            set
            {
                if (_ExtracytoplasmicDysmorphisimAbsent != value)
                {
                    _ExtracytoplasmicDysmorphisimAbsent = value;
                    OnPropertyChanged("_ExtracytoplasmicDysmorphisimAbsent");
                }
            }
        }

        private decimal _OocyteCoronaCumulusComplexPresent;
        public decimal OocyteCoronaCumulusComplexPresent
        {
            get { return _OocyteCoronaCumulusComplexPresent; }
            set
            {
                if (_OocyteCoronaCumulusComplexPresent != value)
                {
                    _OocyteCoronaCumulusComplexPresent = value;
                    OnPropertyChanged("_OocyteCoronaCumulusComplexPresent");
                }
            }
        }

        private decimal _OocyteCoronaCumulusComplexAbsent;
        public decimal OocyteCoronaCumulusComplexAbsent
        {
            get { return _OocyteCoronaCumulusComplexAbsent; }
            set
            {
                if (_OocyteCoronaCumulusComplexAbsent != value)
                {
                    _OocyteCoronaCumulusComplexAbsent = value;
                    OnPropertyChanged("_OocyteCoronaCumulusComplexAbsent");
                }
            }
        }

        //

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
        private string _AnesthetiaDetails;
        public string AnesthetiaDetails
        {
            get { return _AnesthetiaDetails; }
            set
            {
                if (_AnesthetiaDetails != value)
                {
                    _AnesthetiaDetails = value;
                    OnPropertyChanged("AnesthetiaDetails");
                }
            }
        }
        private long _NeedleUsedID;
        public long NeedleUsedID
        {
            get { return _NeedleUsedID; }
            set
            {
                if (_NeedleUsedID != value)
                {
                    _NeedleUsedID = value;
                    OnPropertyChanged("NeedleUsedID");
                }
            }
        }
        private long _OocyteRetrived;
        public long OocyteRetrived
        {
            get { return _OocyteRetrived; }
            set
            {
                if (_OocyteRetrived != value)
                {
                    _OocyteRetrived = value;
                    OnPropertyChanged("OocyteRetrived");
                }
            }
        }
        private long _OocyteQualityID;
        public long OocyteQualityID
        {
            get { return _OocyteQualityID; }
            set
            {
                if (_OocyteQualityID != value)
                {
                    _OocyteQualityID = value;
                    OnPropertyChanged("OocyteQualityID");
                }
            }
        }
        private string _OocyteRemark;
        public string OocyteRemark
        {
            get { return _OocyteRemark; }
            set
            {
                if (_OocyteRemark != value)
                {
                    _OocyteRemark = value;
                    OnPropertyChanged("OocyteRemark");
                }
            }
        }
        private long _ELevelDayID;
        public long ELevelDayID
        {
            get { return _ELevelDayID; }
            set
            {
                if (_ELevelDayID != value)
                {
                    _ELevelDayID = value;
                    OnPropertyChanged("ELevelDayID");
                }
            }
        }
        private string _Evalue;
        public string Evalue
        {
            get { return _Evalue; }
            set
            {
                if (_Evalue != value)
                {
                    _Evalue = value;
                    OnPropertyChanged("Evalue");
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
        private string _ELevelDayremark;
        public string ELevelDayremark
        {
            get { return _ELevelDayremark; }
            set
            {
                if (_ELevelDayremark != value)
                {
                    _ELevelDayremark = value;
                    OnPropertyChanged("ELevelDayremark");
                }
            }
        }
        
        /* Added by Anumani
         * As per the new Changes in the OPU Form
         */

        private DateTime? _TriggerDate;
        public DateTime? TriggerDate
        {
            get { return _TriggerDate; }
            set
            {
                if (_TriggerDate != value)
                {
                    _TriggerDate = value;
                    OnPropertyChanged("TriggerDate");
                }
            }
        }
        private DateTime? _TriggerTime;
        public DateTime? TriggerTime
        {
            get { return _TriggerTime; }
            set
            {
                if (_TriggerTime != value)
                {
                    _TriggerTime = value;
                    OnPropertyChanged("TriggerTime");
                }
            }
        }

        private long _TypeOfNeedleID;
        public long TypeOfNeedleID
        {
            get { return _TypeOfNeedleID; }
            set
            {
                if (_TypeOfNeedleID != value)
                {
                    _TypeOfNeedleID = value;
                    OnPropertyChanged("TypeOfNeedleID");
                }
            }
        }


        private long _AnesthesiaID;
        public long AnesthesiaID
        {
            get { return _AnesthesiaID; }
            set
            {
                if (_AnesthesiaID != value)
                {
                    _AnesthesiaID = value;
                    OnPropertyChanged("AnesthesiaID");
                }
            }
        }

        private long _NeedleUsed;
        public long NeedleUsed
        {
            get { return _NeedleUsed; }
            set
            {
                if (_NeedleUsed != value)
                {
                    _NeedleUsed = value;
                    OnPropertyChanged("NeedleUsed");
                }
            }
        }

        private bool _IsPreAnesthesia;
        public bool IsPreAnesthesia
        {
            get { return _IsPreAnesthesia; }
            set
            {
                if (_IsPreAnesthesia != value)
                {
                    _IsPreAnesthesia = value;
                    OnPropertyChanged("IsPreAnesthesia");
                }
            }
        }

        private bool _IsCycleCancellation;
        public bool IsCycleCancellation
        {
            get { return _IsCycleCancellation; }
            set
            {
                if (_IsCycleCancellation != value)
                {
                    _IsCycleCancellation = value;
                    OnPropertyChanged("IsCycleCancellation");
                }
            }
        }


        private string _LeftFollicule;
        public string LeftFollicule
        {
            get { return _LeftFollicule; }
            set
            {
                if (_LeftFollicule != value)
                {
                    _LeftFollicule = value;
                    OnPropertyChanged("LeftFollicule");
                }
            }
        }

        private string _RightFollicule;
        public string RightFollicule
        {
            get { return _RightFollicule; }
            set
            {
                if (_RightFollicule != value)
                {
                    _RightFollicule = value;
                    OnPropertyChanged("RightFollicule");
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

        private string _Reasons;
        public string Reasons
        {
            get { return _Reasons; }
            set
            {
                if (_Reasons != value)
                {
                    _Reasons = value;
                    OnPropertyChanged("Reasons");
                }
            }
        }

        private bool _IsFlushing;
        public bool IsFlushing
        {
            get { return _IsFlushing; }
            set
            {
                if (_IsFlushing != value)
                {
                    _IsFlushing = value;
                    OnPropertyChanged("IsFlushing");
                }
            }
        }


        #endregion

    }

    public class clsIVFDashboard_EmbryologySummary : IValueObject, INotifyPropertyChanged
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

        private long _NoOocyte;
        public long NoOocyte
        {
            get { return _NoOocyte; }
            set
            {
                if (_NoOocyte != value)
                {
                    _NoOocyte = value;
                    OnPropertyChanged("NoOocyte");
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


        private decimal _MI;
        public decimal MI
        {
            get { return _MI; }
            set
            {
                if (_MI != value)
                {
                    _MI = value;
                    OnPropertyChanged("MI");
                }
            }
        }
        private decimal _MII;
        public decimal MII
        {
            get { return _MII; }
            set
            {
                if (_MII != value)
                {
                    _MII = value;
                    OnPropertyChanged("MII");
                }
            }
        }
        private decimal _GV;
        public decimal GV
        {
            get { return _GV; }
            set
            {
                if (_GV != value)
                {
                    _GV = value;
                    OnPropertyChanged("GV");
                }
            }
        }

        //added by neena
        private decimal _OocyteCytoplasmDysmorphisimPresent;
        public decimal OocyteCytoplasmDysmorphisimPresent
        {
            get { return _OocyteCytoplasmDysmorphisimPresent; }
            set
            {
                if (_OocyteCytoplasmDysmorphisimPresent != value)
                {
                    _OocyteCytoplasmDysmorphisimPresent = value;
                    OnPropertyChanged("OocyteCytoplasmDysmorphisimPresent");
                }
            }
        }

        private decimal _OocyteCytoplasmDysmorphisimAbsent;
        public decimal OocyteCytoplasmDysmorphisimAbsent
        {
            get { return _OocyteCytoplasmDysmorphisimAbsent; }
            set
            {
                if (_OocyteCytoplasmDysmorphisimAbsent != value)
                {
                    _OocyteCytoplasmDysmorphisimAbsent = value;
                    OnPropertyChanged("OocyteCytoplasmDysmorphisimAbsent");
                }
            }
        }

        private decimal _ExtracytoplasmicDysmorphisimPresent;
        public decimal ExtracytoplasmicDysmorphisimPresent
        {
            get { return _ExtracytoplasmicDysmorphisimPresent; }
            set
            {
                if (_ExtracytoplasmicDysmorphisimPresent != value)
                {
                    _ExtracytoplasmicDysmorphisimPresent = value;
                    OnPropertyChanged("ExtracytoplasmicDysmorphisimPresent");
                }
            }
        }

        private decimal _ExtracytoplasmicDysmorphisimAbsent;
        public decimal ExtracytoplasmicDysmorphisimAbsent
        {
            get { return _ExtracytoplasmicDysmorphisimAbsent; }
            set
            {
                if (_ExtracytoplasmicDysmorphisimAbsent != value)
                {
                    _ExtracytoplasmicDysmorphisimAbsent = value;
                    OnPropertyChanged("ExtracytoplasmicDysmorphisimAbsent");
                }
            }
        }

        private decimal _OocyteCoronaCumulusComplexNormal;
        public decimal OocyteCoronaCumulusComplexNormal
        {
            get { return _OocyteCoronaCumulusComplexNormal; }
            set
            {
                if (_OocyteCoronaCumulusComplexNormal != value)
                {
                    _OocyteCoronaCumulusComplexNormal = value;
                    OnPropertyChanged("OocyteCoronaCumulusComplexNormal");
                }
            }
        }

        private decimal _OocyteCoronaCumulusComplexAbnormal;
        public decimal OocyteCoronaCumulusComplexAbnormal
        {
            get { return _OocyteCoronaCumulusComplexAbnormal; }
            set
            {
                if (_OocyteCoronaCumulusComplexAbnormal != value)
                {
                    _OocyteCoronaCumulusComplexAbnormal = value;
                    OnPropertyChanged("OocyteCoronaCumulusComplexAbnormal");
                }
            }
        }

        //

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
       
        private long _OocyteRetrived;
        public long OocyteRetrived
        {
            get { return _OocyteRetrived; }
            set
            {
                if (_OocyteRetrived != value)
                {
                    _OocyteRetrived = value;
                    OnPropertyChanged("OocyteRetrived");
                }
            }
        }

        private DateTime? _TriggerDate;
        public DateTime? TriggerDate
        {
            get { return _TriggerDate; }
            set
            {
                if (_TriggerDate != value)
                {
                    _TriggerDate = value;
                    OnPropertyChanged("TriggerDate");
                }
            }
        }

        private DateTime? _TriggerTime;
        public DateTime? TriggerTime
        {
            get { return _TriggerTime; }
            set
            {
                if (_TriggerTime != value)
                {
                    _TriggerTime = value;
                    OnPropertyChanged("TriggerTime");
                }
            }
        }

        private DateTime? _OPUDate ;
        public DateTime? OPUDate
        {
            get { return _OPUDate; }
            set
            {
                if (_OPUDate != value)
                {
                    _OPUDate = value;
                    OnPropertyChanged("OPUDate");
                }
            }
        }

        private DateTime? _OPUTime ;
        public DateTime? OPUTime
        {
            get { return _OPUTime; }
            set
            {
                if (_OPUTime != value)
                {
                    _OPUTime = value;
                    OnPropertyChanged("OPUTime");
                }
            }
        }

        private long _FreshPGDPGS;
        public long FreshPGDPGS
        {
            get { return _FreshPGDPGS; }
            set
            {
                if (_FreshPGDPGS != value)
                {
                    _FreshPGDPGS = value;
                    OnPropertyChanged("FreshPGDPGS");
                }
            }
        }

        private long _FrozenPGDPGS;
        public long FrozenPGDPGS
        {
            get { return _FrozenPGDPGS; }
            set
            {
                if (_FrozenPGDPGS != value)
                {
                    _FrozenPGDPGS = value;
                    OnPropertyChanged("FrozenPGDPGS");
                }
            }
        }

        private long _ThawedPGDPGS;
        public long ThawedPGDPGS
        {
            get { return _ThawedPGDPGS; }
            set
            {
                if (_ThawedPGDPGS != value)
                {
                    _ThawedPGDPGS = value;
                    OnPropertyChanged("ThawedPGDPGS");
                }
            }
        }

        private long _PostThawedPGDPGS;
        public long PostThawedPGDPGS
        {
            get { return _PostThawedPGDPGS; }
            set
            {
                if (_PostThawedPGDPGS != value)
                {
                    _PostThawedPGDPGS = value;
                    OnPropertyChanged("PostThawedPGDPGS");
                }
            }
        }

        #endregion

    }
}
