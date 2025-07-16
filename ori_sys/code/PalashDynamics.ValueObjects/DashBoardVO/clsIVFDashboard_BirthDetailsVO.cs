using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_BirthDetailsVO : IValueObject, INotifyPropertyChanged
    {
        #region Property Delcaration
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

        private long _ChildID;
        public long ChildID
        {
            get { return _ChildID; }
            set
            {
                if (_ChildID != value)
                {
                    _ChildID = value;
                    OnPropertyChanged("ChildID");
                }
            }
        }
        private string _Child;
        public string Child
        {
            get { return _Child; }
            set
            {
                if (_Child != value)
                {
                    _Child = value;
                    OnPropertyChanged("Child");
                }
            }
        }
        private DateTime? _DateOfBirth = DateTime.Now;
        public DateTime? DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
                }
            }
        }

        private long _Week;
        public long Week
        {
            get { return _Week; }
            set
            {
                if (_Week != value)
                {
                    _Week = value;
                    OnPropertyChanged("Week");
                }
            }
        }

        private long _DeliveryMethodID;
        public long DeliveryMethodID
        {
            get { return _DeliveryMethodID; }
            set
            {
                if (_DeliveryMethodID != value)
                {
                    _DeliveryMethodID = value;
                    OnPropertyChanged("DeliveryMethodID");
                }
            }
        }
        private string _DeliveryMethod;
        public string DeliveryMethod
        {
            get { return _DeliveryMethod; }
            set
            {
                if (_DeliveryMethod != value)
                {
                    _DeliveryMethod = value;
                    OnPropertyChanged("DeliveryMethod");
                }
            }
        }
        private float _WeightAtBirth;
        public float WeightAtBirth
        {
            get { return _WeightAtBirth; }
            set
            {
                if (_WeightAtBirth != value)
                {
                    _WeightAtBirth = value;
                    OnPropertyChanged("WeightAtBirth");
                }
            }
        }

        private float _LengthAtBirth;
        public float LengthAtBirth
        {
            get { return _LengthAtBirth; }
            set
            {
                if (_LengthAtBirth != value)
                {
                    _LengthAtBirth = value;
                    OnPropertyChanged("LengthAtBirth");
                }
            }
        }

        private long _ConditionID;
        public long ConditionID
        {
            get { return _ConditionID; }
            set
            {
                if (_ConditionID != value)
                {
                    _ConditionID = value;
                    OnPropertyChanged("ConditionID");
                }
            }
        }
        private string _Condition;
        public string Condition
        {
            get { return _Condition; }
            set
            {
                if (_Condition != value)
                {
                    _Condition = value;
                    OnPropertyChanged("Condition");
                }
            }
        }
        private long _DiedPerinatallyID;
        public long DiedPerinatallyID
        {
            get { return _DiedPerinatallyID; }
            set
            {
                if (_DiedPerinatallyID != value)
                {
                    _DiedPerinatallyID = value;
                    OnPropertyChanged("DiedPerinatallyID");
                }
            }
        }
        private string _DiedPerinatally;
        public string DiedPerinatally
        {
            get { return _DiedPerinatally; }
            set
            {
                if (_DiedPerinatally != value)
                {
                    _DiedPerinatally = value;
                    OnPropertyChanged("DiedPerinatally");
                }
            }
        }
        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                if (_Surname != value)
                {
                    _Surname = value;
                    OnPropertyChanged("Surname");
                }
            }
        }
        private string _IdentityNumber;
        public string IdentityNumber
        {
            get { return _IdentityNumber; }
            set
            {
                if (_IdentityNumber != value)
                {
                    _IdentityNumber = value;
                    OnPropertyChanged("IdentityNumber");
                }
            }
        }
        
        private string _TownOfBirth;
        public string TownOfBirth
        {
            get { return _TownOfBirth; }
            set
            {
                if (_TownOfBirth != value)
                {
                    _TownOfBirth = value;
                    OnPropertyChanged("TownOfBirth");
                }
            }
        }

        private long _GenderID;
        public long GenderID
        {
            get { return _GenderID; }
            set
            {
                if (_GenderID != value)
                {
                    _GenderID = value;
                    OnPropertyChanged("GenderID");
                }
            }
        }
        private string _Gender;
        public string Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender != value)
                {
                    _Gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }
        private long _CountryOfBirthID;
        public long CountryOfBirthID
        {
            get { return _CountryOfBirthID; }
            set
            {
                if (_CountryOfBirthID != value)
                {
                    _CountryOfBirthID = value;
                    OnPropertyChanged("CountryOfBirthID");
                }
            }
        }
        private string _Country;
        public string Country
        {
            get { return _Country; }
            set
            {
                if (_Country != value)
                {
                    _Country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private long _DeathPostportumID;
        public long DeathPostportumID
        {
            get { return _DeathPostportumID; }
            set
            {
                if (_DeathPostportumID != value)
                {
                    _DeathPostportumID = value;
                    OnPropertyChanged("DeathPostportumID");
                }
            }
        }
        private string _DeathPostportum;
        public string DeathPostportum
        {
            get { return _DeathPostportum; }
            set
            {
                if (_DeathPostportum != value)
                {
                    _DeathPostportum = value;
                    OnPropertyChanged("DeathPostportum");
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

        //added by neena
        private long _FetalHeartNo;
        public long FetalHeartNo
        {
            get { return _FetalHeartNo; }
            set
            {
                if (_FetalHeartNo != value)
                {
                    _FetalHeartNo = value;
                    OnPropertyChanged("FetalHeartNo");
                }
            }
        }

        private long _BirthDetailsID;
        public long BirthDetailsID
        {
            get { return _BirthDetailsID; }
            set
            {
                if (_BirthDetailsID != value)
                {
                    _BirthDetailsID = value;
                    OnPropertyChanged("BirthDetailsID");
                }
            }
        }

        private string _ChildNoStr;
        public string ChildNoStr
        {
            get { return _ChildNoStr; }
            set
            {
                if (_ChildNoStr != value)
                {
                    _ChildNoStr = value;
                    OnPropertyChanged("ChildNoStr");
                }
            }
        }

        private string _BirthWeight;
        public string BirthWeight
        {
            get { return _BirthWeight; }
            set
            {
                if (_BirthWeight != value)
                {
                    _BirthWeight = value;
                    OnPropertyChanged("BirthWeight");
                }
            }
        }

        private string _MedicalConditions;
        public string MedicalConditions
        {
            get { return _MedicalConditions; }
            set
            {
                if (_MedicalConditions != value)
                {
                    _MedicalConditions = value;
                    OnPropertyChanged("MedicalConditions");
                }
            }
        }

        private string _WeeksofGestation;
        public string WeeksofGestation
        {
            get { return _WeeksofGestation; }
            set
            {
                if (_WeeksofGestation != value)
                {
                    _WeeksofGestation = value;
                    OnPropertyChanged("WeeksofGestation");
                }
            }
        }
        private long _DeliveryTypeID;
        public long DeliveryTypeID
        {
            get { return _DeliveryTypeID; }
            set
            {
                if (_DeliveryTypeID != value)
                {
                    _DeliveryTypeID = value;
                    OnPropertyChanged("DeliveryTypeID");
                }
            }
        }

        private long _ActivityID;
        public long ActivityID
        {
            get { return _ActivityID; }
            set
            {
                if (_ActivityID != value)
                {
                    _ActivityID = value;
                    OnPropertyChanged("ActivityID");
                }
            }
        }

        private long _ActivityPoint;
        public long ActivityPoint
        {
            get { return _ActivityPoint; }
            set
            {
                if (_ActivityPoint != value)
                {
                    _ActivityPoint = value;
                    OnPropertyChanged("ActivityPoint");
                }
            }
        }

        private long _Pulse;
        public long Pulse
        {
            get { return _Pulse; }
            set
            {
                if (_Pulse != value)
                {
                    _Pulse = value;
                    OnPropertyChanged("Pulse");
                }
            }
        }

        private long _PulsePoint;
        public long PulsePoint
        {
            get { return _PulsePoint; }
            set
            {
                if (_PulsePoint != value)
                {
                    _PulsePoint = value;
                    OnPropertyChanged("PulsePoint");
                }
            }
        }

        private long _Grimace;
        public long Grimace
        {
            get { return _Grimace; }
            set
            {
                if (_Grimace != value)
                {
                    _Grimace = value;
                    OnPropertyChanged("Grimace");
                }
            }
        }

        private long _GrimacePoint;
        public long GrimacePoint
        {
            get { return _GrimacePoint; }
            set
            {
                if (_GrimacePoint != value)
                {
                    _GrimacePoint = value;
                    OnPropertyChanged("GrimacePoint");
                }
            }
        }

        private long _Appearance;
        public long Appearance
        {
            get { return _Appearance; }
            set
            {
                if (_Appearance != value)
                {
                    _Appearance = value;
                    OnPropertyChanged("Appearance");
                }
            }
        }

        private long _AppearancePoint;
        public long AppearancePoint
        {
            get { return _AppearancePoint; }
            set
            {
                if (_AppearancePoint != value)
                {
                    _AppearancePoint = value;
                    OnPropertyChanged("AppearancePoint");
                }
            }
        }

        private long _Respiration;
        public long Respiration
        {
            get { return _Respiration; }
            set
            {
                if (_Respiration != value)
                {
                    _Respiration = value;
                    OnPropertyChanged("Respiration");
                }
            }
        }

        private long _RespirationPoint;
        public long RespirationPoint
        {
            get { return _RespirationPoint; }
            set
            {
                if (_RespirationPoint != value)
                {
                    _RespirationPoint = value;
                    OnPropertyChanged("RespirationPoint");
                }
            }
        }

        private long _APGARScore;
        public long APGARScore
        {
            get { return _APGARScore; }
            set
            {
                if (_APGARScore != value)
                {
                    _APGARScore = value;
                    OnPropertyChanged("APGARScore");
                }
            }
        }

        private long _APGARScoreID;
        public long APGARScoreID
        {
            get { return _APGARScoreID; }
            set
            {
                if (_APGARScoreID != value)
                {
                    _APGARScoreID = value;
                    OnPropertyChanged("APGARScoreID");
                    OnPropertyChanged("Conclusion");

                }
            }
        }

        private List<MasterListItem> _TypeOfDeliveryList = new List<MasterListItem>();
        public List<MasterListItem> TypeOfDeliveryList
        {
            get
            {
                return _TypeOfDeliveryList;
            }
            set
            {
                _TypeOfDeliveryList = value;
            }
        }

        private MasterListItem _SelectedTypeOfDelivery = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedTypeOfDelivery
        {
            get
            {
                return _SelectedTypeOfDelivery;
            }
            set
            {
                _SelectedTypeOfDelivery = value;
            }
        }

        private List<MasterListItem> _ActivityList = new List<MasterListItem>();
        public List<MasterListItem> ActivityList
        {
            get
            {
                return _ActivityList;
            }
            set
            {
                _ActivityList = value;
            }
        }

        private MasterListItem _SelectedActivity = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedActivity
        {
            get
            {
                return _SelectedActivity;
            }
            set
            {
                _SelectedActivity = value;
            }
        }

        private List<MasterListItem> _PulseList = new List<MasterListItem>();
        public List<MasterListItem> PulseList
        {
            get
            {
                return _PulseList;
            }
            set
            {
                _PulseList = value;
            }
        }

        private MasterListItem _SelectedPulse = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedPulse
        {
            get
            {
                return _SelectedPulse;
            }
            set
            {
                _SelectedPulse = value;
            }
        }

        private List<MasterListItem> _GrimaceList = new List<MasterListItem>();
        public List<MasterListItem> GrimaceList
        {
            get
            {
                return _GrimaceList;
            }
            set
            {
                _GrimaceList = value;
            }
        }

        private MasterListItem _SelectedGrimace = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGrimace
        {
            get
            {
                return _SelectedGrimace;
            }
            set
            {
                _SelectedGrimace = value;
            }
        }

        private List<MasterListItem> _AppearanceList = new List<MasterListItem>();
        public List<MasterListItem> AppearanceList
        {
            get
            {
                return _AppearanceList;
            }
            set
            {
                _AppearanceList = value;
            }
        }

        private MasterListItem _SelectedAppearance = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedAppearance
        {
            get
            {
                return _SelectedAppearance;
            }
            set
            {
                _SelectedAppearance = value;
            }
        }

        private List<MasterListItem> _RespirationList = new List<MasterListItem>();
        public List<MasterListItem> RespirationList
        {
            get
            {
                return _RespirationList;
            }
            set
            {
                _RespirationList = value;
            }
        }

        private MasterListItem _SelectedRespiration = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedRespiration
        {
            get
            {
                return _SelectedRespiration;
            }
            set
            {
                _SelectedRespiration = value;
            }
        }

        private List<MasterListItem> _ConclusionList = new List<MasterListItem>();
        public List<MasterListItem> ConclusionList
        {
            get
            {
                return _ConclusionList;
            }
            set
            {
                _ConclusionList = value;
            }
        }

        private MasterListItem _SelectedConclusion = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedConclusion
        {
            get
            {
                return _SelectedConclusion;
            }
            set
            {
                _SelectedConclusion = value;
            }
        }

        private string _Conclusion;
        public string Conclusion
        {
            get { return _Conclusion; }
            set
            {
                if (_Conclusion != value)
                {
                    _Conclusion = value;
                    OnPropertyChanged("Conclusion");
                }
            }
        }

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
        //private long _Conclusion;
        //public long Conclusion
        //{
        //    get
        //    {
        //        return _Conclusion;
        //    }
        //    set
        //    {
        //        _Conclusion = value;
        //    }
        //}
        //

        #endregion

        #region Common Properties
        
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
