using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.OutPatientDepartment
{
    public class clsVisitVO : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

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

        #region Property Declaration Section

        private long _ID;
        public long ID { get; set; }

        private DateTime _Date = DateTime.Now;
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                if (value != _Date)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private DateTime _CurrentDate;
        public DateTime CurrentDate
        {
            get
            {
                return _CurrentDate;
            }
            set
            {
                if (value != _CurrentDate)
                {
                    _CurrentDate = value;
                    OnPropertyChanged("CurrentDate");
                }
            }
        }

        private string _ControlUnit = "";
        public string ControlUnit { get; set; }

        public string DoctorSpecialization { get; set; }

        private string _DoctorCode;
        public string DoctorCode
        {
            get
            {
                return _DoctorCode;
            }

            set
            {
                if (value != _DoctorCode)
                {
                    _DoctorCode = value;
                    OnPropertyChanged("DoctorCode");
                }
            }
        }




        private string _DepartmentCode;
        public string DepartmentCode
        {
            get
            {
                return _DepartmentCode;
            }

            set
            {
                if (value != _DepartmentCode)
                {
                    _DepartmentCode = value;
                    OnPropertyChanged("DepartmentCode");
                }
            }
        }

        private string _EncounterListDoctorCode;
        public string EncounterListDoctorCode
        {
            get
            {
                return _EncounterListDoctorCode;
            }
            set
            {
                if (value != _EncounterListDoctorCode)
                {
                    _EncounterListDoctorCode = value;
                    OnPropertyChanged("EncounterListDoctorCode");
                }
            }
        }

        private long _RoundId;
        public long RoundId { get; set; }

        private Boolean _OPDIPD;
        public Boolean OPDIPD { get; set; }




        private long _FreeDaysDuration;
        public long FreeDaysDuration { get; set; }

        private Boolean _Isfree;
        public Boolean IsFree { get; set; }




        public string DocSpecCode { get; set; }

        private long _PatientId;
        public long PatientId { get; set; }

        private long _PatientUnitId;
        public long PatientUnitId { get; set; }

        private long _SpouseID;
        public long SpouseID { get; set; }

        private string _OPDNO = "";
        public string OPDNO { get; set; }

        private Boolean _ISIPDDischarge = false;
        public Boolean ISIPDDischarge { get; set; }

        private string _Clinic = "";
        public string Clinic { get; set; }

        private long _TemplateID;
        public long TemplateID { get; set; }

        private long _VisitTypeID;
        public long VisitTypeID
        {
            get
            {
                return _VisitTypeID;
            }

            set
            {
                if (value != _VisitTypeID)
                {
                    _VisitTypeID = value;
                    OnPropertyChanged("VisitTypeID");
                }
            }
        }

        private long _ConsultationVisitTypeID;
        public long ConsultationVisitTypeID
        {
            get
            {
                return _ConsultationVisitTypeID;
            }

            set
            {
                if (value != _ConsultationVisitTypeID)
                {
                    _ConsultationVisitTypeID = value;
                    OnPropertyChanged("ConsultationVisitTypeID");
                }
            }
        }

        private long _VisitID;
        public long VisitID
        {
            get
            {
                return _VisitID;
            }
            set
            {
                if (value != _VisitID)
                {
                    _VisitID = value;
                    OnPropertyChanged("VisitID");
                }
            }
        }


        private long _VisitCount;
        public long VisitCount
        {
            get
            {
                return _VisitCount;
            }
            set
            {
                if (value != _VisitCount)
                {
                    _VisitCount = value;
                    OnPropertyChanged("VisitCount");
                }
            }
        }

        public string VisitType { get; set; }

        private string _Complaints = "";
        public string Complaints
        {
            get
            {
                return _Complaints;
            }

            set
            {
                if (value != _Complaints)
                {
                    _Complaints = value;
                    OnPropertyChanged("Complaints");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
        {
            get
            {
                return _DepartmentID;
            }

            set
            {
                if (value != _DepartmentID)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        public string Department { get; set; }
        public string Unit { get; set; }

        private long _DoctorID;
        public long DoctorID
        {
            get
            {
                return _DoctorID;
            }

            set
            {
                if (value != _DoctorID)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        public string Doctor { get; set; }

        private long _CabinID;
        public long CabinID
        {
            get
            {
                return _CabinID;
            }

            set
            {
                if (value != _CabinID)
                {
                    _CabinID = value;
                    OnPropertyChanged("CabinID");
                }
            }
        }

        public string Cabin { get; set; }

        private long? _ReferredDoctorID;
        public long? ReferredDoctorID
        {
            get
            {
                return _ReferredDoctorID;
            }

            set
            {
                if (value != _ReferredDoctorID)
                {
                    _ReferredDoctorID = value;
                    OnPropertyChanged("ReferredDoctorID");
                }
            }
        }

        private string _ReferredDoctor = "";
        public string ReferredDoctor
        {
            get
            {
                return _ReferredDoctor;
            }

            set
            {
                if (value != _ReferredDoctor)
                {
                    _ReferredDoctor = value;
                    OnPropertyChanged("ReferredDoctor");
                }
            }
        }

        private string _PatientCaseRecord;
        public string PatientCaseRecord
        {
            get
            {
                return _PatientCaseRecord;
            }

            set
            {
                if (value != _PatientCaseRecord)
                {
                    _PatientCaseRecord = value;
                    OnPropertyChanged("PatientCaseRecord");
                }
            }
        }

        private string _CaseReferralSheet;
        public string CaseReferralSheet
        {
            get
            {
                return _CaseReferralSheet;
            }

            set
            {
                if (value != _CaseReferralSheet)
                {
                    _CaseReferralSheet = value;
                    OnPropertyChanged("CaseReferralSheet");
                }
            }
        }

        private string _VisitNotes = "";
        public string VisitNotes
        {
            get
            {
                return _VisitNotes;
            }

            set
            {
                if (value != _VisitNotes)
                {
                    _VisitNotes = value;
                    OnPropertyChanged("VisitNotes");
                }
            }
        }

        private bool _VisitStatus = true;
        public bool VisitStatus
        {
            get { return _VisitStatus; }
            set
            {
                if (_VisitStatus != value)
                {
                    _VisitStatus = value;
                    OnPropertyChanged("VisitStatus");
                }
            }
        }

        private string _PastVisitDate;
        public string PastVisitDate
        {
            get
            {
                return _PastVisitDate;
            }

            set
            {
                if (value != _PastVisitDate)
                {
                    _PastVisitDate = value;
                    OnPropertyChanged("_PastVisitDate");
                }
            }
        }

        private string _PastVisitInTime;
        public string PastVisitInTime
        {
            get
            {
                return _PastVisitInTime;
            }

            set
            {
                if (value != _PastVisitInTime)
                {
                    _PastVisitInTime = value;
                    OnPropertyChanged("PastVisitInTime");
                }
            }
        }

        private double? _BalanceAmount;
        public double? BalanceAmount
        {
            get
            {
                return _BalanceAmount;
            }

            set
            {
                if (value != _BalanceAmount)
                {
                    _BalanceAmount = value;
                    OnPropertyChanged("BalanceAmount");
                }
            }
        }

        private string _MRNO = "";
        public string MRNO { get; set; }

        private string _CoupleMRNO = "";
        public string CoupleMRNO { get; set; }
        private string _DonorMRNO = "";
        public string DonorMRNO { get; set; }
        private string _SurrogateMRNO = "";
        public string SurrogateMRNO { get; set; }

        public string CurrentVisitStatusString
        {
            get
            {
                return _CurrentVisitStatus.ToString();
            }
        }

        private VisitCurrentStatus _CurrentVisitStatus;
        public VisitCurrentStatus CurrentVisitStatus
        {
            get { return _CurrentVisitStatus; }
            set
            {
                if (_CurrentVisitStatus != value)
                {
                    _CurrentVisitStatus = value;
                    OnPropertyChanged("CurrentVisitStatus");
                }
            }
        }

        public bool IsReferral { get; set; }

        public string Allergies { get; set; }

        public string ArtDashboardDate { get; set; }

        public string HistoryFlag { get; set; }

        public string IsOPDIPDFlag { get; set; }

        public long TakenBy { get; set; }

        public string LoginNm { get; set; }

        public long EMRID { get; set; }

        public string Value { get; set; }

        // Added BY CDS
        public bool IsBillGeneratedAgainstVisit { get; set; }

        // added by EMR Changes Added by Ashish Z. on dated 31052017
        private DateTime _EMRModVisitDate = DateTime.Now;
        public DateTime EMRModVisitDate
        {
            get
            {
                return _EMRModVisitDate;
            }

            set
            {
                if (value != _EMRModVisitDate)
                {
                    _EMRModVisitDate = value;
                    OnPropertyChanged("EMRModVisitDate");
                }
            }
        }
        //End

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

        private long _UnitId;
        public long UnitId
        {
            get { return _UnitId; }
            set
            {
                if (_UnitId != value)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
                }
            }
        }

        private long _CreatedUnitID;
        public long CreatedUnitId
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitId");
                }
            }
        }

        private long? _UpdatedUnitID;
        public long? UpdatedUnitId
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (_UpdatedUnitID != value)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitId");
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

        private string _AddedOn = "";
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

        private string _AddedWindowsLoginName = "";
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

        private string _UpdatedOn = "";
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

        private string _UpdatedWindowsLoginName = "";
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


        #endregion


    }
}
