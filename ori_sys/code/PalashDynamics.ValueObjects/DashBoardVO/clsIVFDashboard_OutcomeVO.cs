using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.DashBoardVO
{
    public class clsIVFDashboard_OutcomeVO : IValueObject, INotifyPropertyChanged
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

        public bool IsDonorCycle { get; set; }

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
        private DateTime? _BHCGAss1Date = null;
        public DateTime? BHCGAss1Date
        {
            get
            {
                return _BHCGAss1Date;
            }
            set
            {
                _BHCGAss1Date = value;

            }
        }
        private DateTime? _BHCGAss2Date = null;
        public DateTime? BHCGAss2Date
        {
            get
            {
                return _BHCGAss2Date;
            }
            set
            {
                _BHCGAss2Date = value;

            }
        }

        private DateTime? _PregnanacyConfirmDate = null;
        public DateTime? PregnanacyConfirmDate
        {
            get
            {
                return _PregnanacyConfirmDate;
            }
            set
            {
                _PregnanacyConfirmDate = value;

            }
        }

        private bool? _BHCGAss2IsBSCG;
        public bool? BHCGAss2IsBSCG
        {
            get
            {
                return _BHCGAss2IsBSCG;
            }
            set
            {
                _BHCGAss2IsBSCG = value;

            }
        }

        private bool? _BHCGAss2IsBSCGPostive;
        public bool? BHCGAss2IsBSCGPostive
        {
            get
            {
                return _BHCGAss2IsBSCGPostive;
            }
            set
            {
                _BHCGAss2IsBSCGPostive = value;

            }
        }
        private bool? _BHCGAss2IsBSCGNagative;
        public bool? BHCGAss2IsBSCGNagative
        {
            get
            {
                return _BHCGAss2IsBSCGNagative;
            }
            set
            {
                _BHCGAss2IsBSCGNagative = value;

            }
        }
        private string _BHCGAss2BSCGValue;
        public string BHCGAss2BSCGValue
        {
            get
            {
                return _BHCGAss2BSCGValue;
            }
            set
            {
                _BHCGAss2BSCGValue = value;

            }
        }
        private string _BHCGAss2USG;
        public string BHCGAss2USG
        {
            get
            {
                return _BHCGAss2USG;
            }
            set
            {
                _BHCGAss2USG = value;

            }
        }

        private bool? _IsPregnancyAchieved;
        public bool? IsPregnancyAchieved
        {
            get
            {
                return _IsPregnancyAchieved;
            }
            set
            {
                _IsPregnancyAchieved = value;

            }
        }
        private bool _IsPregnancyAchievedPostive;
        public bool IsPregnancyAchievedPostive
        {
            get
            {
                return _IsPregnancyAchievedPostive;
            }
            set
            {
                _IsPregnancyAchievedPostive = value;

            }
        }
        private bool _IsPregnancyAchievedNegative;
        public bool IsPregnancyAchievedNegative
        {
            get
            {
                return _IsPregnancyAchievedNegative;
            }
            set
            {
                _IsPregnancyAchievedNegative = value;

            }
        }

        private bool? _BHCGAss1IsBSCG;
        public bool? BHCGAss1IsBSCG
        {
            get
            {
                return _BHCGAss1IsBSCG;
            }
            set
            {
                _BHCGAss1IsBSCG = value;

            }
        }
        private bool _BHCGAss1IsBSCGPositive;
        public bool BHCGAss1IsBSCGPositive
        {
            get
            {
                return _BHCGAss1IsBSCGPositive;
            }
            set
            {
                _BHCGAss1IsBSCGPositive = value;

            }
        }
        private bool _BHCGAss1IsBSCGNagative;
        public bool BHCGAss1IsBSCGNagative
        {
            get
            {
                return _BHCGAss1IsBSCGNagative;
            }
            set
            {
                _BHCGAss1IsBSCGNagative = value;

            }
        }
        private string _BHCGAss1BSCGValue;
        public string BHCGAss1BSCGValue
        {
            get
            {
                return _BHCGAss1BSCGValue;
            }
            set
            {
                _BHCGAss1BSCGValue = value;

            }
        }
        private string _BHCGAss1SrProgest;
        public string BHCGAss1SrProgest
        {
            get
            {
                return _BHCGAss1SrProgest;
            }
            set
            {
                _BHCGAss1SrProgest = value;

            }
        }
        private bool _IsClosed;
        public bool IsClosed
        {
            get
            {
                return _IsClosed;
            }
            set
            {
                _IsClosed = value;

            }
        }
        private bool _BiochemPregnancy;
        public bool BiochemPregnancy
        {
            get
            {
                return _BiochemPregnancy;
            }
            set
            {
                _BiochemPregnancy = value;

            }
        }

        private bool _Ectopic;
        public bool Ectopic
        {
            get
            {
                return _Ectopic;
            }
            set
            {
                _Ectopic = value;

            }
        }
        private bool _Abortion;
        public bool Abortion
        {
            get
            {
                return _Abortion;
            }
            set
            {
                _Abortion = value;

            }
        }
        private bool _Missed;
        public bool Missed
        {
            get
            {
                return _Missed;
            }
            set
            {
                _Missed = value;

            }
        }
        private bool _Incomplete;
        public bool Incomplete
        {
            get
            {
                return _Incomplete;
            }
            set
            {
                _Incomplete = value;

            }
        }
        private bool _IUD;
        public bool IUD
        {
            get
            {
                return _IUD;
            }
            set
            {
                _IUD = value;

            }
        }

        private bool _PretermDelivery;
        public bool PretermDelivery
        {
            get
            {
                return _PretermDelivery;
            }
            set
            {
                _PretermDelivery = value;

            }
        }
        private bool _LiveBirth;
        public bool LiveBirth
        {
            get
            {
                return _LiveBirth;
            }
            set
            {
                _LiveBirth = value;

            }
        }
        private bool _Congenitalabnormality;
        public bool Congenitalabnormality
        {
            get
            {
                return _Congenitalabnormality;
            }
            set
            {
                _Congenitalabnormality = value;

            }
        }
        private string _Count;
        public string Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;

            }
        }

        private bool _FetalHeartSound;
        public bool FetalHeartSound
        {
            get
            {
                return _FetalHeartSound;
            }
            set
            {
                _FetalHeartSound = value;

            }
        }
        private DateTime? _FetalDate = null;
        public DateTime? FetalDate
        {
            get
            {
                return _FetalDate;
            }
            set
            {
                _FetalDate = value;

            }
        }
        private string _OHSSRemark;
        public string OHSSRemark
        {
            get
            {
                return _OHSSRemark;
            }
            set
            {
                _OHSSRemark = value;

            }
        }
        private bool _IsChemicalPregnancy;
        public bool IsChemicalPregnancy
        {
            get
            {
                return _IsChemicalPregnancy;
            }
            set
            {
                _IsChemicalPregnancy = value;

            }
        }
        private bool _IsFullTermDelivery;
        public bool IsFullTermDelivery
        {
            get
            {
                return _IsFullTermDelivery;
            }
            set
            {
                _IsFullTermDelivery = value;

            }
        }
        private long _BabyTypeID;
        public long BabyTypeID
        {
            get
            {
                return _BabyTypeID;
            }
            set
            {
                _BabyTypeID = value;

            }
        }
        private bool _OHSSEarly;
        public bool OHSSEarly
        {
            get
            {
                return _OHSSEarly;
            }
            set
            {
                _OHSSEarly = value;

            }
        }
        private bool _OHSSLate;
        public bool OHSSLate
        {
            get
            {
                return _OHSSLate;
            }
            set
            {
                _OHSSLate = value;

            }
        }
        private bool _OHSSMild;
        public bool OHSSMild
        {
            get
            {
                return _OHSSMild;
            }
            set
            {
                _OHSSMild = value;

            }
        }

        private bool _OHSSMode;
        public bool OHSSMode
        {
            get
            {
                return _OHSSMode;
            }
            set
            {
                _OHSSMode = value;

            }
        }
        private bool _OHSSSereve;
        public bool OHSSSereve
        {
            get
            {
                return _OHSSSereve;
            }
            set
            {
                _OHSSSereve = value;

            }
        }
        private string _OutComeRemarks;
        public string OutComeRemarks
        {
            get
            {
                return _OutComeRemarks;
            }
            set
            {
                _OutComeRemarks = value;

            }
        }

        //added by neena
        private bool _IsUnlinkSurrogate;
        public bool IsUnlinkSurrogate
        {
            get
            {
                return _IsUnlinkSurrogate;
            }
            set
            {
                _IsUnlinkSurrogate = value;

            }
        }

        private bool _IsFreeze;
        public bool IsFreeze
        {
            get
            {
                return _IsFreeze;
            }
            set
            {
                _IsFreeze = value;

            }
        }
        

        private string _SacRemarks;
        public string SacRemarks
        {
            get
            {
                return _SacRemarks;
            }
            set
            {
                _SacRemarks = value;

            }
        }

        private long _NoOfSacs;
        public long NoOfSacs
        {
            get
            {
                return _NoOfSacs;
            }
            set
            {
                _NoOfSacs = value;

            }
        }

        private DateTime? _SacsObservationDate = null;
        public DateTime? SacsObservationDate
        {
            get
            {
                return _SacsObservationDate;
            }
            set
            {
                _SacsObservationDate = value;

            }
        }

        private long _PregnancyAchievedID;
        public long PregnancyAchievedID
        {
            get
            {
                return _PregnancyAchievedID;
            }
            set
            {
                _PregnancyAchievedID = value;

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

        private long _FetalHeartCount;
        public long FetalHeartCount
        {
            get { return _FetalHeartCount; }
            set
            {
                if (_FetalHeartCount != value)
                {
                    _FetalHeartCount = value;
                    OnPropertyChanged("FetalHeartCount");
                }
            }
        }

        List<MasterListItem> _SurrogatePatientList = new List<MasterListItem>();
        public List<MasterListItem> SurrogatePatientList
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

        private long _ResultListID;
        public long ResultListID
        {
            get { return _ResultListID; }
            set
            {
                if (_ResultListID != value)
                {
                    _ResultListID = value;
                    OnPropertyChanged("ResultListID");
                }
            }
        }

        private List<MasterListItem> _BHCGResultListVO = new List<MasterListItem>();
        public List<MasterListItem> BHCGResultListVO
        {
            get
            {
                return _BHCGResultListVO;
            }
            set
            {
                _BHCGResultListVO = value;
            }
        }
        private MasterListItem _selectedBHCGResultList = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedBHCGResultList
        {
            get
            {
                return _selectedBHCGResultList;
            }
            set
            {
                _selectedBHCGResultList = value;
            }
        }

        private long _SurrogateTypeID;
        public long SurrogateTypeID
        {
            get { return _SurrogateTypeID; }
            set
            {
                if (_SurrogateTypeID != value)
                {
                    _SurrogateTypeID = value;
                    OnPropertyChanged("SurrogateTypeID");
                }
            }
        }

        private List<clsPregnancySacsDetailsVO> _PregnancySacsList = new List<clsPregnancySacsDetailsVO>();
        public List<clsPregnancySacsDetailsVO> PregnancySacsList
        {
            get
            {
                return _PregnancySacsList;
            }
            set
            {
                _PregnancySacsList = value;
            }
        }

        public bool IsSurrogate { get; set; }
        public bool FreeSurrogate { get; set; }

        private long _OutcomeID;
        public long OutcomeID
        {
            get { return _OutcomeID; }
            set
            {
                if (_OutcomeID != value)
                {
                    _OutcomeID = value;
                    OnPropertyChanged("OutcomeID");
                }
            }
        }
        //

        #endregion
    }

    //added by neena
    public class clsPregnancySacsDetailsVO : IValueObject, INotifyPropertyChanged
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
        public Int64 PregnancySacID { get; set; }
        public string SaceNoStr { get; set; }
        public long ResultListID { get; set; }
        public long PregnancyListID { get; set; }

        public bool Status { get; set; }

        public bool IsFetalHeart { get; set; }
        public bool CongenitalAbnormalityYes { get; set; }
        public bool CongenitalAbnormalityNo { get; set; }
        public string CongenitalAbnormalityReason { get; set; }
        public bool IsUnlinkSurrogate { get; set; }


        public long PlanTherapy { get; set; }
        public long PlanTherapyUnitID { get; set; }

        private List<MasterListItem> _OutcomeResultList = new List<MasterListItem>();
        public List<MasterListItem> OutcomeResultListVO
        {
            get
            {
                return _OutcomeResultList;
            }
            set
            {
                _OutcomeResultList = value;
            }
        }
        private MasterListItem _selectedResultList = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedResultList
        {
            get
            {
                return _selectedResultList;
            }
            set
            {
                _selectedResultList = value;
            }
        }

        private List<MasterListItem> _OutcomePregnancyList = new List<MasterListItem>();
        public List<MasterListItem> OutcomePregnancyListVO
        {
            get
            {
                return _OutcomePregnancyList;
            }
            set
            {
                _OutcomePregnancyList = value;
            }
        }
        private MasterListItem _selectedPregnancyList = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem selectedPregnancyList
        {
            get
            {
                return _selectedPregnancyList;
            }
            set
            {
                _selectedPregnancyList = value;
            }
        }

        private string _SacRemarks;
        public string SacRemarks
        {
            get
            {
                return _SacRemarks;
            }
            set
            {
                _SacRemarks = value;

            }
        }

        private long _NoOfSacs;
        public long NoOfSacs
        {
            get
            {
                return _NoOfSacs;
            }
            set
            {
                _NoOfSacs = value;

            }
        }

        private DateTime? _SacsObservationDate = null;
        public DateTime? SacsObservationDate
        {
            get
            {
                return _SacsObservationDate;
            }
            set
            {
                _SacsObservationDate = value;

            }
        }

        private long _PregnancyAchievedID;
        public long PregnancyAchievedID
        {
            get
            {
                return _PregnancyAchievedID;
            }
            set
            {
                _PregnancyAchievedID = value;

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

        private long _SurrogateTypeID;
        public long SurrogateTypeID
        {
            get { return _SurrogateTypeID; }
            set
            {
                if (_SurrogateTypeID != value)
                {
                    _SurrogateTypeID = value;
                    OnPropertyChanged("SurrogateTypeID");
                }
            }
        }

        private long _FetalHeartCount;
        public long FetalHeartCount
        {
            get { return _FetalHeartCount; }
            set
            {
                if (_FetalHeartCount != value)
                {
                    _FetalHeartCount = value;
                    OnPropertyChanged("FetalHeartCount");
                }
            }
        }

        private long _OutcomeID;
        public long OutcomeID
        {
            get { return _OutcomeID; }
            set
            {
                if (_OutcomeID != value)
                {
                    _OutcomeID = value;
                    OnPropertyChanged("OutcomeID");
                }
            }
        }

    }

    //
}
