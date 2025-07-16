using System;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsEMRReferralLetterVO : IValueObject, INotifyPropertyChanged
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

        private string _ReferralRemark;
        public string ReferralRemark
        {
            get { return _ReferralRemark; }
            set
            {
                if (_ReferralRemark != value)
                {
                    _ReferralRemark = value;
                    OnPropertyChanged("ReferralRemark");
                }
            }
        }

        private string _ReferralTreatment;
        public string ReferralTreatment
        {
            get { return _ReferralTreatment; }
            set
            {
                if (_ReferralTreatment != value)
                {
                    _ReferralTreatment = value;
                    OnPropertyChanged("ReferralTreatment");
                }
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

        private DateTime? _JointCareDate;
        public DateTime? JointCareDate
        {
            get
            {
                return _JointCareDate;
            }
            set
            {
                _JointCareDate = value;
                OnPropertyChanged("JointCareDate");
            }
        }

        private DateTime? _TakeOverDate;
        public DateTime? TakeOverDate
        {
            get
            {
                return _TakeOverDate;
            }
            set
            {
                _TakeOverDate = value;
                OnPropertyChanged("TakeOverDate");
            }
        }

        private DateTime? _NextConsultDate;
        public DateTime? NextConsultDate
        {
            get
            {
                return _NextConsultDate;
            }
            set
            {
                _NextConsultDate = value;
                OnPropertyChanged("NextConsultDate");
            }
        }

        public long AcknowledgementID { get; set; }

        private DateTime? _ConsultEndDate;
        public DateTime? ConsultEndDate
        {
            get
            {
                return _ConsultEndDate;
            }
            set
            {
                _ConsultEndDate = value;
                OnPropertyChanged("ConsultEndDate");
            }
        }


        private long _ID;
        public long ID { get; set; }

        public string Unit { get; set; }

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

        private string _DisplayDate;
        public string DisplayDate
        {
            get
            {
                return _DisplayDate;
            }
            set
            {
                if (value != _DisplayDate)
                {
                    _DisplayDate = value;
                    OnPropertyChanged("DisplayDate");
                }
            }
        }

        private long _PatientId;
        public long PatientId { get; set; }

        private long _PatientUnitId;
        public long PatientUnitId { get; set; }

        private string _PatientName = "";
        public string PatientName
        {
            get
            {
                return _PatientName;
            }

            set
            {
                if (value != _PatientName)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private DateTime _AckDate;
        public DateTime AckDate
        {
            get { return _AckDate; }
            set
            {
                if (_AckDate != value)
                {
                    _AckDate = value;
                    OnPropertyChanged("AckDate");
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

        public long ReferredDoctorID { get; set; }
        private string _ReferalDoctor = "";
        public string ReferalDoctor
        {
            get
            {
                return _ReferalDoctor;
            }

            set
            {
                if (value != _ReferalDoctor)
                {
                    _ReferalDoctor = value;
                    OnPropertyChanged("ReferalDoctor");
                }
            }
        }

        private Int16 _ReferralType;
        public Int16 ReferalType
        {
            get
            {
                return _ReferralType;
            }

            set
            {
                if (value != _ReferralType)
                {
                    _ReferralType = value;
                    OnPropertyChanged("ReferralType");
                }
            }
        }

        private String _SelectedReferral;
        public String SelectedReferal
        {
            get
            {
                return _SelectedReferral;
            }

            set
            {
                if (value != _SelectedReferral)
                {
                    _SelectedReferral = value;
                    OnPropertyChanged("SelectedReferral");
                }
            }
        }

        private string _ReferredDoctorCode = String.Empty;
        public string ReferredDoctorCode
        {
            get
            {
                return _ReferredDoctorCode;
            }

            set
            {
                if (value != _ReferredDoctorCode)
                {
                    _ReferredDoctorCode = value;
                    OnPropertyChanged("ReferredDoctorCode");
                }
            }
        }
        private string _ReferalDoctorCode = String.Empty;
        public string ReferalDoctorCode
        {
            get
            {
                return _ReferalDoctorCode;
            }

            set
            {
                if (value != _ReferalDoctorCode)
                {
                    _ReferalDoctorCode = value;
                    OnPropertyChanged("ReferalDoctorCode");
                }
            }
        }

        private string _ReferredSpeciality = string.Empty;
        public string ReferredSpeciality
        {
            get
            {
                return _ReferredSpeciality;
            }
            set
            {
                if (value != _ReferredSpeciality)
                {
                    _ReferredSpeciality = value;
                    OnPropertyChanged("ReferredSpeciality");
                }
            }
        }

        private string _ReferredSpecCode = string.Empty;
        public string ReferredSpecCode
        {
            get
            {
                return _ReferredSpecCode;
            }
            set
            {
                if (value != _ReferredSpecCode)
                {
                    _ReferredSpecCode = value;
                    OnPropertyChanged("ReferredSpecCode");
                }
            }
        }

        private string _ReferalSpeciality = string.Empty;
        public string ReferalSpeciality
        {
            get
            {
                return _ReferalSpeciality;
            }
            set
            {
                if (value != _ReferalSpeciality)
                {
                    _ReferalSpeciality = value;
                    OnPropertyChanged("ReferalSpeciality");
                }
            }
        }
        private string _ReferalSpecialityCode = string.Empty;
        public string ReferalSpecialityCode
        {
            get
            {
                return _ReferalSpecialityCode;
            }
            set
            {
                if (value != _ReferalSpecialityCode)
                {
                    _ReferalSpecialityCode = value;
                    OnPropertyChanged("ReferalSpecialityCode");
                }
            }
        }

        private string _VisitDetails = "";
        public string VisitDetails
        {
            get
            {
                return _VisitDetails;
            }

            set
            {
                if (value != _VisitDetails)
                {
                    _VisitDetails = value;
                    OnPropertyChanged("VisitDetails");
                }
            }
        }

        private string _WorkingDiagnosis = "";
        public string Diagnosis
        {
            get
            {
                return _WorkingDiagnosis;
            }

            set
            {
                if (value != _WorkingDiagnosis)
                {
                    _WorkingDiagnosis = value;
                    OnPropertyChanged("Diagnosis");
                }
            }
        }

        private string _Treatment = "";
        public string Treatment
        {
            get
            {
                return _Treatment;
            }

            set
            {
                if (value != _Treatment)
                {
                    _Treatment = value;
                    OnPropertyChanged("Treatment ");
                }
            }
        }
        #endregion

        #region Common Properties

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
