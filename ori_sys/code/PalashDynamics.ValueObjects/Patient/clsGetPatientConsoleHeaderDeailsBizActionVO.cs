using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects
{
    public class clsGetPatientConsoleHeaderDeailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientConsoleHeaderDeailsBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long VisitID { get; set; }
        public Boolean ISOPDIPD { get; set; }

        private clsPatientConsoleHeaderVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientConsoleHeaderVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }
    }

    public class clsPatientConsoleHeaderVO
    {
        public long ID { get; set; }
        public long PatientId { get; set; }

        public long RowNum { get; set; }

        public double Weight { get; set; }
        public double Height { get; set; }
        public double BMI { get; set; }

        public long RoundID { get; set; }
        public long OPD_IPD_ID { get; set; }

        public string DoctorCode { get; set; }
        public string DocSpecName { get; set; }
        public string DocSpecCode { get; set; }
        public string DoctorName { get; set; }


        public string CurrDoctorCode { get; set; }
        public string currDocSpecName { get; set; }
        public string currDocSpecCode { get; set; }
        public string currDoctorName { get; set; }

        private string _Bed;
        public string Bed
        {
            get { return _Bed; }
            set
            {
                if (value != _Bed)
                {
                    _Bed = value;
                    OnPropertyChanged("Bed");
                }
            }
        }


        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (value != _DoctorID)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private string _DepartmentCode;
        public string DepartmentCode
        {
            get { return _DepartmentCode; }
            set
            {
                if (value != _DepartmentCode)
                {
                    _DepartmentCode = value;
                    OnPropertyChanged("DepartmentCode");
                }
            }
        }

        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                if (value != _DepartmentName)
                {
                    _DepartmentName = value;
                    OnPropertyChanged("DepartmentName");
                }
            }
        }

        private DateTime? _Date;
        public DateTime? Date
        {
            get { return _Date; }
            set
            {
                if (value != _Date)
                {
                    _Date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        private DateTime? _DischargeDate;
        public DateTime? DischargeDate
        {
            get { return _DischargeDate; }
            set
            {
                if (value != _DischargeDate)
                {
                    _DischargeDate = value;
                    OnPropertyChanged("DischargeDate");
                }
            }
        }

        private bool _ISDischarged;
        public bool ISDischarged
        {
            get { return _ISDischarged; }
            set
            {
                if (value != _ISDischarged)
                {
                    _ISDischarged = value;
                    OnPropertyChanged("ISDischarged");
                }
            }
        }

        private string _IsLaboratoryRight = "Collapsed";
        public string IsLaboratoryRight
        {
            get { return _IsLaboratoryRight; }
            set
            {
                if (value != _IsLaboratoryRight)
                {
                    _IsLaboratoryRight = value;
                    OnPropertyChanged("IsLaboratoryRight");
                }
            }
        }

        private string _IsLaboratoryWrong = "Visible";
        public string IsLaboratoryWrong
        {
            get { return _IsLaboratoryWrong; }
            set
            {
                if (value != _IsLaboratoryWrong)
                {
                    _IsLaboratoryWrong = value;
                    OnPropertyChanged("IsLaboratoryWrong");
                }
            }
        }

        private string _IsRadiologyRight = "Colapsed";
        public string IsRadiologyRight
        {
            get { return _IsRadiologyRight; }
            set
            {
                if (value != _IsRadiologyRight)
                {
                    _IsRadiologyRight = value;
                    OnPropertyChanged("IsRadiologyRight");
                }
            }
        }

        private string _IsRadiologyWrong = "Visible";
        public string IsRadiologyWrong
        {
            get { return _IsRadiologyWrong; }
            set
            {
                if (value != _IsRadiologyWrong)
                {
                    _IsRadiologyWrong = value;
                    OnPropertyChanged("IsRadiologyWrong");
                }
            }
        }

        private string _IsCompdPrescRight = "Collapsed";
        public string IsCompdPrescRight
        {
            get { return _IsCompdPrescRight; }
            set
            {
                if (value != _IsCompdPrescRight)
                {
                    _IsCompdPrescRight = value;
                    OnPropertyChanged("IsCompdPrescRight");
                }
            }
        }

        private string _IsCompdPrescWrong = "Visible";
        public string IsCompdPrescWrong
        {
            get { return _IsCompdPrescWrong; }
            set
            {
                if (value != _IsCompdPrescWrong)
                {
                    _IsCompdPrescWrong = value;
                    OnPropertyChanged("IsCompdPrescWrong");
                }
            }
        }

        private string _IsDiagnostickRight = "Visible";
        public string IsDiagnostickRight
        {
            get { return _IsDiagnostickRight; }
            set
            {
                if (value != _IsDiagnostickRight)
                {
                    _IsDiagnostickRight = value;
                    OnPropertyChanged("IsDiagnostickRight");
                }
            }
        }

        private string _IsDiagnostickWrong = "Visible";
        public string IsDiagnostickWrong
        {
            get { return _IsDiagnostickWrong; }
            set
            {
                if (value != _IsDiagnostickWrong)
                {
                    _IsDiagnostickWrong = value;
                    OnPropertyChanged("IsDiagnostickWrong");
                }
            }
        }

        private string _IsPrescriptionRight = "Collapsed";
        public string IsPrescriptionRight
        {
            get { return _IsPrescriptionRight; }
            set
            {
                if (value != _IsPrescriptionRight)
                {
                    _IsPrescriptionRight = value;
                    OnPropertyChanged("IsPrescriptionRight");
                }
            }
        }

        private string _IsPrescriptionwrong = "Visible";
        public string IsPrescriptionwrong
        {
            get { return _IsPrescriptionwrong; }
            set
            {
                if (value != _IsPrescriptionwrong)
                {
                    _IsPrescriptionwrong = value;
                    OnPropertyChanged("IsPrescriptionwrong");
                }
            }
        }

        private string _IsDocumentRight = "Collapsed";
        public string IsDocumentRight
        {
            get { return _IsDocumentRight; }
            set
            {
                if (value != _IsDocumentRight)
                {
                    _IsDocumentRight = value;
                    OnPropertyChanged("IsDocumentRight");
                }
            }
        }

        private string _IsAllergy = "Collapsed";
        public string IsAllergy
        {
            get { return _IsAllergy; }
            set
            {
                if (value != _IsAllergy)
                {
                    _IsAllergy = value;
                    OnPropertyChanged("IsAllergy");
                }
            }
        }

        private string _IsDiagnosis = "Collapsed";
        public string IsDiagnosisRight
        {
            get { return _IsDiagnosis; }
            set
            {
                if (value != _IsDiagnosis)
                {
                    _IsDiagnosis = value;
                    OnPropertyChanged("IsDiagnosisRight");
                }
            }
        }

        private string _IsDiagnosisWrong = "Visible";
        public string IsDiagnosisWrong
        {
            get { return _IsDiagnosisWrong; }
            set
            {
                if (value != _IsDiagnosisWrong)
                {
                    _IsDiagnosisWrong = value;
                    OnPropertyChanged("IsDiagnosisWrong");
                }
            }
        }

        private string _IsDocumentwrong = "Visible";
        public string IsDocumentwrong
        {
            get { return _IsDocumentwrong; }
            set
            {
                if (value != _IsDocumentwrong)
                {
                    _IsDocumentwrong = value;
                    OnPropertyChanged("IsDocumentwrong");
                }
            }
        }

        private string _IsNonAllergy = "Visible";
        public string IsNonAllergy
        {
            get { return _IsNonAllergy; }
            set
            {
                if (value != _IsNonAllergy)
                {
                    _IsNonAllergy = value;
                    OnPropertyChanged("IsNonAllergy");
                }
            }
        }

        private string _IsProcedure = "Collapsed";
        public string IsProcedure
        {
            get { return _IsProcedure; }
            set
            {
                if (value != _IsProcedure)
                {
                    _IsProcedure = value;
                    OnPropertyChanged("IsProcedure");
                }
            }
        }

        private string _IsNonProcedure = "Visible";
        public string IsNonProcedure
        {
            get { return _IsNonProcedure; }
            set
            {
                if (value != _IsNonProcedure)
                {
                    _IsNonProcedure = value;
                    OnPropertyChanged("IsNonProcedure");
                }
            }
        }
        
        private string _IPDOPDNO;
        public string IPDOPDNO
        {
            get { return _IPDOPDNO; }
            set
            {
                if (value != _IPDOPDNO)
                {
                    _IPDOPDNO = value;
                    OnPropertyChanged("IPDOPDNO");
                }
            }
        }

        private string _IPDOPD;
        public string IPDOPD
        {
            get { return _IPDOPD; }
            set
            {
                if (value != _IPDOPD)
                {
                    _IPDOPD = value;
                    OnPropertyChanged("IPDOPD");
                }
            }
        }

        private bool _OPD_IPD;
        public bool OPD_IPD
        {
            get { return _OPD_IPD; }
            set
            {
                if (value != _OPD_IPD)
                {
                    _OPD_IPD = value;
                    OnPropertyChanged("OPD_IPD");
                }
            }
        }


        private string _PreFix;
        public string PreFix
        {
            get { return _PreFix; }
            set
            {
                if (_PreFix != value)
                {
                    _PreFix = value;
                    OnPropertyChanged("PreFix");
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

        private string _MiddleName;
        public string MiddleName
        {
            get { return _MiddleName; }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }


        private string _MRNo;
        public string MRNo
        {
            get { return _MRNo; }
            set
            {
                if (_MRNo != value)
                {
                    _MRNo = value;
                    OnPropertyChanged("MRNo");
                }
            }
        }

        private string _Education;
        public string Education
        {
            get { return _Education; }
            set
            {
                if (_Education != value)
                {
                    _Education = value;
                    OnPropertyChanged("Education");
                }
            }
        }

        public string Allergies { get; set; }
        private string _Religion;
        public string Religion
        {
            get { return _Religion; }
            set
            {
                if (_Religion != value)
                {
                    _Religion = value;
                    OnPropertyChanged("Religion");
                }
            }
        }

        private string _PatientName;
        public string PatientName
        {
            get
            {
                return _PatientName;
            }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _RegisteredClinic;
        public string RegisteredClinic
        {
            get
            {
                return _RegisteredClinic;
            }
            set
            {
                if (_RegisteredClinic != value)
                {
                    _RegisteredClinic = value;
                    OnPropertyChanged("RegisteredClinic");
                }
            }
        }

        private int _GenderID;
        public int GenderID
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

        private string _MaritalStatus;
        public string MaritalStatus
        {
            get { return _MaritalStatus; }
            set
            {
                if (_MaritalStatus != value)
                {
                    _MaritalStatus = value;
                    OnPropertyChanged("MaritalStatus");
                }
            }
        }



        private string _MOB;
        public string MOB
        {
            get { return _MOB; }
            set
            {
                if (_MOB != value)
                {
                    _MOB = value;
                    OnPropertyChanged("MOB");
                }
            }
        }


        private string _AgeInYearMonthDays;
        public string AgeInYearMonthDays
        {
            get { return _AgeInYearMonthDays; }
            set
            {
                if (_AgeInYearMonthDays != value)
                {
                    _AgeInYearMonthDays = value;
                    OnPropertyChanged("AgeInYearMonthDays");
                }
            }
        }


        private int _Age;
        public int Age
        {
            get { return _Age; }
            set
            {
                if (_Age != value)
                {
                    _Age = value;
                    OnPropertyChanged("Age");
                }
            }
        }

        public byte[] Photo { get; set; }

        //added by neena
        private string _ImageName;
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if (value != _ImageName)
                {
                    _ImageName = value;
                }
            }
        }
        //

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        private DateTime? _RegistrationDate;
        public DateTime? RegistrationDate
        {
            get { return _RegistrationDate; }
            set
            {
                if (_RegistrationDate != value)
                {
                    _RegistrationDate = value;
                    OnPropertyChanged("RegistrationDate");
                }
            }
        }

        private DateTime? _BirthDate;
        public DateTime? BirthDate
        {
            get { return _BirthDate; }
            set
            {
                if (_BirthDate != value)
                {
                    _BirthDate = value;
                    OnPropertyChanged("BirthDate");
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

    }

    public class clsSavePhotoBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsSavePhotoBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long PatientID { get; set; }

        public byte[] Photo { get; set; }

    }

    public class clsGetEMRAdmVisitListByPatientIDBizActionVO : IBizActionValueObject
    {
        private clsPatientConsoleHeaderVO _PatientDetails = null;
        public clsPatientConsoleHeaderVO PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }
        }

        private List<clsPatientConsoleHeaderVO> _EMRList = null;
        public List<clsPatientConsoleHeaderVO> EMRList
        {
            get { return _EMRList; }
            set { _EMRList = value; }
        }

        public bool IsForNursingConsol = false;

        private bool _IsEMRVisitAdmission = false;
        public bool IsEMRVisitAdmission
        {
            get { return _IsEMRVisitAdmission; }
            set { _IsEMRVisitAdmission = value; }
        }

        public long ID { get; set; }
        public Boolean ISOPDIPD { get; set; }

        public long OPD_IPD_ID { get; set; }
        public long OPD_IPD_UnitID { get; set; }
        public int OPD_IPD { get; set; }
        public long UnitID { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public int CallFrom { get; set; }
        public string sortExpression { get; set; }

        private string _SearchExpression = "";
        public string SearchExpression
        {
            get { return _SearchExpression; }
            set { _SearchExpression = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMRAdmVisitListByPatientIDBizAction";
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
