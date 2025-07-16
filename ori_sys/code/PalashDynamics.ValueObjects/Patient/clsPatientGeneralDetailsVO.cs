using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsPatientGeneralVO : IValueObject, INotifyPropertyChanged
    {
        public bool IsEMR { get; set; }
        public bool IsFromNewCouple { get; set; } //added by neena 
        private long _FromForm;
        public long FromForm
        {
            get { return _FromForm; }
            set
            {
                if (_FromForm != value)
                {
                    _FromForm = value;
                    OnPropertyChanged("FromForm");
                }
            }
        }
        public bool IsFromAppointment { get; set; }
        public bool FromFollowUp { get; set; }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _PatientID = value;
                    OnPropertyChanged("ID");
                }
            }
        }
        private bool _IsSignificant;
        public bool IsSignificant
        {
            get { return _IsSignificant; }
            set
            {
                if (_IsSignificant != value)
                {
                    _IsSignificant = value;
                    OnPropertyChanged("IsSignificant");
                }
            }
        }
        private bool _BringScanDoc;
        public bool BringScanDoc
        {
            get { return _BringScanDoc; }
            set
            {
                if (_BringScanDoc != value)
                {
                    _BringScanDoc = value;
                    OnPropertyChanged("BringScanDoc");
                }
            }
        }
        private string strPatient = "";
        public string PName
        {
            get { return strPatient; }
            set
            {
                if (value != strPatient)
                {
                    strPatient = value;
                    OnPropertyChanged("PName");
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
        private long _ReferralDoctorID;
        public long ReferralDoctorID
        {
            get { return _ReferralDoctorID; }
            set
            {
                if (_ReferralDoctorID != value)
                {
                    _ReferralDoctorID = value;
                    OnPropertyChanged("ReferralDoctorID");
                }
            }
        }

        //***//---------
        private bool _IsAgainstDonor = false;
        public bool IsAgainstDonor
        {
            get { return _IsAgainstDonor; }
            set
            {
                if (_IsAgainstDonor != value)
                {
                    _IsAgainstDonor = value;
                    OnPropertyChanged("IsAgainstDonor");
                }
            }
        }

        private long _CoConsultantDoctorID; //***//19
        public long CoConsultantDoctorID
        {
            get { return _CoConsultantDoctorID; }
            set
            {
                if (_CoConsultantDoctorID != value)
                {
                    _CoConsultantDoctorID = value;
                    OnPropertyChanged("CoConsultantDoctorID");
                }
            }
        }
        

        //--------------

        private string _ReferralDetail;
        public string ReferralDetail
        {
            get { return _ReferralDetail; }
            set
            {
                if (_ReferralDetail != value)
                {
                    _ReferralDetail = value;
                    OnPropertyChanged("ReferralDetail");
                }
            }
        }

        private string _CompanyName = "";
        public string CompanyName
        {
            get { return _CompanyName; }
            set
            {
                if (_CompanyName != value)
                {
                    _CompanyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }

        private long _ReferredToDoctorID;
        public long ReferredToDoctorID
        {
            get { return _ReferredToDoctorID; }
            set
            {
                if (_ReferredToDoctorID != value)
                {
                    _ReferredToDoctorID = value;
                    OnPropertyChanged("ReferredToDoctorID");
                }
            }
        }
        public long _PatientPROID;
        public long PatientPROID
        {
            get { return _PatientPROID; }
            set
            {
                if (_PatientPROID != value)
                {
                    _PatientPROID = value;
                    OnPropertyChanged("PatientPROID");
                }
            }
        }

        //added by neena
        public bool IsSurrogate { get; set; } 

        private bool _IsPatientChecked;
        public bool IsPatientChecked
        {
            get { return _IsPatientChecked; }
            set
            {
                if (_IsPatientChecked != value)
                {
                    _IsPatientChecked = value;
                    OnPropertyChanged("IsPatientChecked");
                }
            }
        }

        private bool _IsPatientEnabled;
        public bool IsPatientEnabled
        {
            get { return _IsPatientEnabled; }
            set
            {
                if (_IsPatientEnabled != value)
                {
                    _IsPatientEnabled = value;
                    OnPropertyChanged("_IsPatientEnabled");
                }
            }
        }
        //

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

        private long _SpouseID;
        public long SpouseID
        {
            get { return _SpouseID; }
            set
            {
                if (_SpouseID != value)
                {
                    _SpouseID = value;
                    OnPropertyChanged("SpouseID");
                }
            }
        }

        public DateTime? VisitDate { get; set; }

        private long _PatientTypeID = 0;
        public long PatientTypeID
        {
            get { return _PatientTypeID; }
            set
            {
                if (_PatientTypeID != value)
                {
                    _PatientTypeID = value;
                    OnPropertyChanged("PatientTypeID");
                }

            }
        } 

        // Added By CDS        
        private long _NewPatientCategoryID = 0;
        public long NewPatientCategoryID
        {
            get { return _NewPatientCategoryID; }
            set
            {
                if (_NewPatientCategoryID != value)
                {
                    _NewPatientCategoryID = value;
                    OnPropertyChanged("NewPatientCategoryID");
                }

            }
        }

        private double _BillBalanceAmountSelf;
        public double BillBalanceAmountSelf
        {
            get { return _BillBalanceAmountSelf; }
            set
            {
                if (_BillBalanceAmountSelf != value)
                {
                    _BillBalanceAmountSelf = value;
                    OnPropertyChanged("BillBalanceAmountSelf");
                }
            }
        }
        // Added By CDS
        private long _RelationID;
        public long RelationID
        {
            get { return _RelationID; }
            set
            {
                if (_RelationID != value)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");
                }

            }
        }

        private string _Relation;
        public string Relation
        {
            get { return _Relation; }
            set
            {
                if (_Relation != value)
                {
                    _Relation = value;
                    OnPropertyChanged("Relation");
                }

            }
        }

        private string _Tariff;
        public string Tariff
        {
            get { return _Tariff; }
            set
            {
                if (_Tariff != value)
                {
                    _Tariff = value;
                    OnPropertyChanged("Tariff");
                }

            }
        }

        public long TariffID { get; set; }
        public long PatientSourceID { get; set; }
        public long PatientSponsorCategoryID { get; set; }
        public long CompanyID { get; set; }

        private bool _IsSurrogateUsed;
        public bool IsSurrogateUsed
        {
            get { return _IsSurrogateUsed; }
            set
            {
                if (_IsSurrogateUsed != value)
                {
                    _IsSurrogateUsed = value;
                    OnPropertyChanged("IsSurrogateUsed");
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

        private bool _IsBedReservation = false;
        public bool IsBedReservation
        {
            get { return _IsBedReservation; }
            set
            {
                if (_IsBedReservation != value)
                {
                    _IsBedReservation = value;
                    OnPropertyChanged("IsBedReservation");
                }
            }
        }

        public long VisitID { get; set; }
        public string SearchKeyword { get; set; }

        private string _OPDNo;
        public string OPDNO
        {
            get { return _OPDNo; }
            set
            {
                if (_OPDNo != value)
                {
                    _OPDNo = value;
                    OnPropertyChanged("OPDNO");
                }

            }
        }
        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;
                    OnPropertyChanged("DoctorID");
                }
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (_DoctorName != value)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        public long IPDAdmissionID { get; set; }
      
        //Added By Bhushanp 09/01/2017
        private long _AdmissionUnitID;

        public long AdmissionUnitID
        {
            get { return _AdmissionUnitID; }
            set { _AdmissionUnitID = value; }
        }

        private Boolean _IsReadyForDischarged;
        public Boolean IsReadyForDischarged
        {
            get
            {
                return _IsReadyForDischarged;
            }
            set
            {
                _IsReadyForDischarged = value;
            }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit != value)
                {
                    _Unit = value;
                    OnPropertyChanged("Unit");
                }
            }
        }

        private string _IPDAdmissionNo;
        public string IPDAdmissionNo
        {
            get { return _IPDAdmissionNo; }
            set
            {
                if (_IPDAdmissionNo != value)
                {
                    _IPDAdmissionNo = value;
                    OnPropertyChanged("IPDAdmissionNo");
                }

            }
        }

        private string _IPDPatientName = "";
        public string IPDPatientName
        {
            get { return _IPDPatientName; }
            set
            {
                if (_IPDPatientName != value)
                {
                    _IPDPatientName = value;
                    OnPropertyChanged("IPDPatientName");
                }
            }
        }


        public PatientsKind PatientKind { get; set; }


        private DateTime? _RegistrationDate = DateTime.Now;
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

        public string AttachedImage { get; set; }
        public bool IsDocAttached { get; set; }
        public string Email { get; set; }
        public string IdentityType { get; set; }
        public string IdentityNumber { get; set; }
        public string RemarkForPatientType { get; set; }
        public bool IsSpouse { get; set; }
        
        private string _FirstName = "";
        //[Required(ErrorMessage = "First Name Required")]
        //[StringLength(50, ErrorMessage = "First Name must be in between 1 to 50 Characters")]
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                if (_MiddleName != value)
                {
                    _MiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }


        private DateTime? _DateOfBirthFromAge = null;
        public DateTime? DateOfBirthFromAge
        {
            get
            {
                return _DateOfBirthFromAge;
            }
            set
            {
                if (value != _DateOfBirthFromAge)
                {

                    _DateOfBirthFromAge = value;
                    OnPropertyChanged("DateOfBirthFromAge");
                }
            }
        }
       
        public bool IsAge { get; set; }


        private Int16 _RegType;
        public Int16 RegType
        {
            get { return _RegType; }
            set
            {
                if (_RegType != value)
                {
                    _RegType = value;
                    OnPropertyChanged("RegType");
                }
            }
        }


        private long _BedID;
        public long BedID
        {
            get { return _BedID; }
            set
            {
                if (_BedID != value)
                {
                    _BedID = value;
                    OnPropertyChanged("BedID");
                }
            }
        }
        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set
            {
                if (_ClassName != value)
                {
                    _ClassName = value;
                    OnPropertyChanged("ClassName");
                }
            }
        }
        private string _WardName;
        public string WardName
        {
            get { return _WardName; }
            set
            {
                if (_WardName != value)
                {
                    _WardName = value;
                    OnPropertyChanged("WardName");
                }
            }
        }

        private long _ClassID;
        public long ClassID
        {
            get { return _ClassID; }
            set
            {
                if (_ClassID != value)
                {
                    _ClassID = value;
                    OnPropertyChanged("ClassID");
                }
            }
        }

        private long _WardID;
        public long WardID
        {
            get { return _WardID; }
            set
            {
                if (_WardID != value)
                {
                    _WardID = value;
                    OnPropertyChanged("WardID");
                }
            }
        }

        private long _BillingToBedCategoryID;
        public long BillingToBedCategoryID
        {
            get
            {
                return _BillingToBedCategoryID;
            }
            set
            {
                _BillingToBedCategoryID = value;
                OnPropertyChanged("BillingToBedCategoryID");
            }
        }

        private bool _IsOPDIPD = false;
        public bool IsOPDIPD
        {
            get { return _IsOPDIPD; }
            set
            {
                if (_IsOPDIPD != value)
                {
                    _IsOPDIPD = value;

                }
            }
        }

        private bool _IsDischarged = false;
        public bool IsDischarged
        {
            get { return _IsDischarged; }
            set
            {
                if (_IsDischarged != value)
                {
                    _IsDischarged = value;
                }
            }
        }

        private bool _IsOPD = false;
        public bool IsOPD
        {
            get { return _IsOPD; }
            set
            {
                if (_IsOPD != value)
                {
                    _IsOPD = value;
                }
            }
        }

        private bool _IsIPD = false;
        public bool IsIPD
        {
            get { return _IsIPD; }
            set
            {
                if (_IsIPD != value)
                {
                    _IsIPD = value;
                }
            }
        }
        
        // Added By CDS
        private long _AdmissionTypeID;
        public long AdmissionTypeID
        {
            get { return _AdmissionTypeID; }
            set
            {
                if (_AdmissionTypeID != value)
                {
                    _AdmissionTypeID = value;
                    OnPropertyChanged("AdmissionTypeID");
                }
            }
        }
        private string _BedName;
        public string BedName
        {
            get { return _BedName; }
            set
            {
                if (_BedName != value)
                {
                    _BedName = value;
                    OnPropertyChanged("BedName");
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
        private string _FamilyName = "";
        public string FamilyName
        {
            get { return _FamilyName; }
            set
            {
                if (_FamilyName != value)
                {
                    _FamilyName = value;
                    OnPropertyChanged("FamilyName");
                }
            }
        }

        private string _LastName = "";
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

        private DateTime? _DateOfBirth = null;
        public DateTime? DateOfBirth
        {
            get
            {
                return _DateOfBirth;
            }
            set
            {
                if (value != _DateOfBirth)
                {

                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
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

        private string _DonarCode;
        public string DonarCode
        {
            get { return _DonarCode; }
            set
            {
                if (_DonarCode != value)
                {
                    _DonarCode = value;
                    OnPropertyChanged("DonarCode");
                }

            }
        }

        private string _OldRegistrationNo;
        public string OldRegistrationNo
        {
            get { return _OldRegistrationNo; }
            set
            {
                if (_OldRegistrationNo != value)
                {
                    _OldRegistrationNo = value;
                    OnPropertyChanged("OldRegistrationNo");
                }

            }
        }
        //

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }

        private string _AgencyName;
        public string AgencyName
        {
            get { return _AgencyName; }
            set
            {
                if (_AgencyName != value)
                {
                    _AgencyName = value;
                    OnPropertyChanged("AgencyName");
                }
            }
        }
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

        private string _ContactNO1;
        public string ContactNO1
        {
            get { return _ContactNO1; }
            set
            {
                if (_ContactNO1 != value)
                {
                    _ContactNO1 = value;
                    OnPropertyChanged("ContactNO1");
                }
            }
        }

        private string _Education;
        public string Education
        {
            get
            {
                return _Education;
            }
            set
            {
                _Education = value;
                OnPropertyChanged("Education");
            }
        }

        private string _Religion;
        public string Religion
        {
            get
            {
                return _Religion;
            }
            set
            {
                _Religion = value;
                OnPropertyChanged("Religion");
            }
        }

        private string _Complaint;
        public string Complaint
        {
            get { return _Complaint; }
            set
            {
                if (_Complaint != value)
                {
                    _Complaint = value;
                    OnPropertyChanged("Complaint");
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

        public string UniversalID { get; set; }
        public Double Height { get; set; }
        public Double Weight { get; set; }
        public Double BMI { get; set; }
        public string Alerts { get; set; }

        private string _RegistrationType;
        public string RegistrationType
        {
            get { return _RegistrationType; }
            set
            {
                if (_RegistrationType != value)
                {
                    _RegistrationType = value;
                    OnPropertyChanged("RegistrationType");
                }
            }
        }

        public bool SelectedStatus { get; set; }
        public string PRORemark { get; set; }

        // BY BHUSHAN  . . . . . . 
        private bool _IsReferralDoc = true;
        public bool IsReferralDoc
        {
            get { return _IsReferralDoc; }
            set
            {
                if (_IsReferralDoc != value)
                {
                    _IsReferralDoc = value;
                    OnPropertyChanged("IsReferralDoc");
                }
            }
        }
        private long _ReferralTypeID;
        public long ReferralTypeID
        {
            get { return _ReferralTypeID; }
            set
            {
                if (_ReferralTypeID != value)
                {
                    _ReferralTypeID = value;
                    OnPropertyChanged("ReferralTypeID");
                }
            }
        }
        private long _AgencyID;
        public long AgencyID
        {
            get { return _AgencyID; }
            set
            {
                if (_AgencyID != value)
                {
                    _AgencyID = value;
                    OnPropertyChanged("AgencyID");
                }
            }
        }
        //added by akshays on 17-11-2015
        
        private byte[] _GeneralDetailsBarcodeImage;
        public byte[] GeneralDetailsBarcodeImage
        {
            get { return _GeneralDetailsBarcodeImage; }
            set
            {
                if (_GeneralDetailsBarcodeImage != value)
                {
                    _GeneralDetailsBarcodeImage = value;
                    OnPropertyChanged("GeneralDetailsBarcodeImage");
                }
            }
        }
        //closed by akshays on 17-11-2015

        public long MemberRelationID { get; set; }

        //added by Ashish Z.
        private long _EducationID;
        public long EducationID
        {
            get { return _EducationID; }
            set
            {
                if (_EducationID != value)
                {
                    _EducationID = value;
                    OnPropertyChanged("EducationID");
                }
            }
        }

        //Added by AJ Date 3/11/2016
        private long _BloodGroupID;
        public long BloodGroupID
        {
            get { return _BloodGroupID; }
            set
            {
                if (value != _BloodGroupID)
                {
                    _BloodGroupID = value;
                    OnPropertyChanged("BloodGroupID");

                }
            }
        }


        private string _AgentName = "";
        public string AgentName
        {
            get
            {
                return _AgentName;
            }
            set
            {
                if (_AgentName != value)
                {
                    _AgentName = value;
                    OnPropertyChanged("AgentName");
                }
            }
        }
        // Date 2/1/2017

        private long _Opd_Ipd_External_Id;
        public long Opd_Ipd_External_Id
        {
            get { return _Opd_Ipd_External_Id; }
            set
            {
                if (_Opd_Ipd_External_Id != value)
                {
                    _Opd_Ipd_External_Id = value;
                    OnPropertyChanged("Opd_Ipd_External_Id");
                }
            }
        }

        private long _Opd_Ipd_External_UnitId;
        public long Opd_Ipd_External_UnitId
        {
            get { return _Opd_Ipd_External_UnitId; }
            set
            {
                if (_Opd_Ipd_External_UnitId != value)
                {
                    _Opd_Ipd_External_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitId");
                }
            }
        }

        private long _Opd_Ipd_External;
        public long Opd_Ipd_External
        {
            get { return _Opd_Ipd_External; }
            set
            {
                if (_Opd_Ipd_External != value)
                {
                    _Opd_Ipd_External = value;
                    OnPropertyChanged("Opd_Ipd_External");
                }
            }
        }


        private string _SourceofReference;
        public string SourceofReference
        {
            get { return _SourceofReference; }
            set
            {
                if (_SourceofReference != value)
                {
                    _SourceofReference = value;
                    OnPropertyChanged("SourceofReference");
                }
            }
        }
        private string _Camp;
        public string Camp
        {
            get { return _Camp; }
            set
            {
                if (_Camp != value)
                {
                    _Camp = value;
                    OnPropertyChanged("Camp");
                }
            }
        }

        private string _FollowUpRemark;
        public string FollowUpRemark
        {
            get { return _FollowUpRemark; }
            set
            {
                if (_FollowUpRemark != value)
                {
                    _FollowUpRemark = value;
                    OnPropertyChanged("FollowUpRemark");
                }
            }
        }

        private long _FollowUpReasonID;
        public long FollowUpReasonID
        {
            get { return _FollowUpReasonID; }
            set
            {
                if (_FollowUpReasonID != value)
                {
                    _FollowUpReasonID = value;
                    OnPropertyChanged("FollowUpReasonID");
                }
            }
        }


        private long _FollowUpID;
        public long FollowUpID
        {
            get { return _FollowUpID; }
            set
            {
                if (_FollowUpID != value)
                {
                    _FollowUpID = value;
                    OnPropertyChanged("FollowUpID");
                }
            }
        }

        private DateTime? _FollowUpDate = null;
        public DateTime? FollowUpDate
        {
            get
            {
                return _FollowUpDate;
            }
            set
            {
                if (value != _FollowUpDate)
                {

                    _FollowUpDate = value;
                    OnPropertyChanged("FollowUpDate");
                }
            }
        }

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (_DepartmentID != value)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }



        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                if (_DepartmentName != value)
                {
                    _DepartmentName = value;
                    OnPropertyChanged("DepartmentName");
                }
            }
        }

        private string _Designation;
        public string Designation
        {
            get { return _Designation; }
            set
            {
                if (_Designation != value)
                {
                    _Designation = value;
                    OnPropertyChanged("Designation");
                }
            }
        }

        //***//----------
        private long _PreferredLangID;
        public long PreferredLangID
        {
            get { return _PreferredLangID; }
            set
            {
                if (_PreferredLangID != value)
                {
                    _PreferredLangID = value;
                    OnPropertyChanged("PreferredLangID");
                }
            }
        }

        private long _TreatRequiredID;
        public long TreatRequiredID
        {
            get { return _TreatRequiredID; }
            set
            {
                if (_TreatRequiredID != value)
                {
                    _TreatRequiredID = value;
                    OnPropertyChanged("TreatRequiredID");
                }
            }
        }

        private long _NationalityID;
        public long NationalityID
        {
            get { return _NationalityID; }
            set
            {
                if (_NationalityID != value)
                {
                    _NationalityID = value;
                    OnPropertyChanged("NationalityID");
                }
            }
        }

        private DateTime? _MarriageAnnDate = null;
        public DateTime? MarriageAnnDate
        {
            get
            {
                return _MarriageAnnDate;
            }
            set
            {
                if (value != _MarriageAnnDate)
                {

                    _MarriageAnnDate = value;
                    OnPropertyChanged("MarriageAnnDate");
                }
            }
        }

        private int _NoOfPeople;
        public int NoOfPeople
        {
            get { return _NoOfPeople; }
            set
            {
                if (_NoOfPeople != value)
                {
                    _NoOfPeople = value;
                    OnPropertyChanged("NoOfPeople");
                }
            }
        }

        private string _ClinicName;
        public string ClinicName
        {
            get { return _ClinicName; }
            set
            {
                if (_ClinicName != value)
                {
                    _ClinicName = value;
                    OnPropertyChanged("ClinicName");
                }
            }
        }

        private string _SonDaughterOf;
        public string SonDaughterOf
        {
            get { return _SonDaughterOf; }
            set
            {
                if (_SonDaughterOf != value)
                {
                    _SonDaughterOf = value;
                    OnPropertyChanged("SonDaughterOf");
                }
            }
        }

        private bool _IsClinicVisited;
        public bool IsClinicVisited
        {
            get { return _IsClinicVisited; }
            set
            {
                if (value != _IsClinicVisited)
                {
                    _IsClinicVisited = value;
                    OnPropertyChanged("IsClinicVisited");
                }

            }
        }

        private long _SpecialRegID;
        public long SpecialRegID
        {
            get { return _SpecialRegID; }
            set
            {
                if (_SpecialRegID != value)
                {
                    _SpecialRegID = value;
                    OnPropertyChanged("SpecialRegID");
                }
            }
        }

        private string _strSpecialReg;
        public string SpecialRegName
        {
            get { return _strSpecialReg; }
            set
            {
                if (_strSpecialReg != value)
                {
                    _strSpecialReg = value;
                    OnPropertyChanged("SpecialRegName");
                }
            }
        }
        //
        //* Added by Ajit Date - 25/8/2016
        private string _PanNumber;
        public string PanNumber
        {
            get { return _PanNumber; }
            set
            {
                if (_PanNumber != value)
                {
                    _PanNumber = value;
                    OnPropertyChanged("PanNumber");
                }
            }

        }

        
        private string _FemaleName = "";
        public string FemaleName
        {
            get { return _FemaleName; }
            set
            {
                if (_FemaleName != value)
                {
                    _FemaleName = value;
                    OnPropertyChanged("FemaleName");
                }
            }
        }

        private string _MaleName = "";
        public string MaleName
        {
            get { return _MaleName; }
            set
            {
                if (_MaleName != value)
                {
                    _MaleName = value;
                    OnPropertyChanged("MaleName");
                }
            }
        }


        private long _FemaleGenderID;
        public long FemaleGenderID
        {
            get { return _FemaleGenderID; }
            set
            {
                if (_FemaleGenderID != value)
                {
                    _FemaleGenderID = value;
                    OnPropertyChanged("FemaleGenderID");
                }
            }
        }

        private long _MaleGenderID;
        public long MaleGenderID
        {
            get { return _MaleGenderID; }
            set
            {
                if (_MaleGenderID != value)
                {
                    _MaleGenderID = value;
                    OnPropertyChanged("MaleGenderID");
                }
            }
        }

        private long _MaleAge;
        public long MaleAge
        {
            get { return _MaleAge; }
            set
            {
                if (_MaleAge != value)
                {
                    _MaleAge = value;
                    OnPropertyChanged("MaleAge");
                }
            }
        }

        private long _FemaleAge;
        public long FemaleAge
        {
            get { return _FemaleAge; }
            set
            {
                if (_FemaleAge != value)
                {
                    _FemaleAge = value;
                    OnPropertyChanged("FemaleAge");
                }
            }
        }
        


        //-----------------------------------------

        private string _Package;
        public string Package
        {
            get { return _Package; }
            set
            {
                if (_Package != value)
                {
                    _Package = value;
                    OnPropertyChanged("Package");
                }
            }
        }

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (_PackageID != value)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        private long _BillID;
        public long BillID
        {
            get { return _BillID; }
            set
            {
                if (_BillID != value)
                {
                    _BillID = value;
                    OnPropertyChanged("BillID");
                }
            }
        }

        private long _BillUnitID;
        public long BillUnitID
        {
            get { return _BillUnitID; }
            set
            {
                if (_BillUnitID != value)
                {
                    _BillUnitID = value;
                    OnPropertyChanged("BillUnitID");
                }
            }
        }

        private string _BillNo;
        public string BillNo
        {
            get { return _BillNo; }
            set
            {
                if (_BillNo != value)
                {
                    _BillNo = value;
                    OnPropertyChanged("BillNo");
                }
            }
        }

        public DateTime? BillDate { get; set; }
        //------------------------------------------------

        private double _TotalBillAmount;
        public double TotalBillAmount
        {
            get { return _TotalBillAmount; }
            set
            {
                if (_TotalBillAmount != value)
                {
                    _TotalBillAmount = value;
                    OnPropertyChanged("TotalBillAmount");
                }
            }
        }

        private double _PaidAmount;
        public double PaidAmount
        {
            get { return _PaidAmount; }
            set
            {
                if (_PaidAmount != value)
                {
                    _PaidAmount = value;
                    OnPropertyChanged("PaidAmount");
                }
            }
        }



        private double _TotalDiscountAmount;
        public double TotalDiscountAmount
        {
            get { return _TotalDiscountAmount; }
            set
            {
                if (_TotalDiscountAmount != value)
                {
                    _TotalDiscountAmount = value;
                    OnPropertyChanged("TotalDiscountAmount");
                }
            }
        }

        private double _TotalConcessionAmount;
        public double TotalConcessionAmount
        {
            get { return _TotalConcessionAmount; }
            set
            {
                if (_TotalConcessionAmount != value)
                {
                    _TotalConcessionAmount = value;
                    OnPropertyChanged("TotalConcessionAmount");
                }
            }
        }

        private double _NetBillAmount;
        public double NetBillAmount
        {
            get { return _NetBillAmount; }
            set
            {
                if (_NetBillAmount != value)
                {
                    _NetBillAmount = value;
                    OnPropertyChanged("NetBillAmount");
                }
            }
        }
        //-------------------------------------------------------------------------------------

        private int _IsPatientIndentReceiveExists;
        public int IsPatientIndentReceiveExists
        {
            get { return _IsPatientIndentReceiveExists; }
            set
            {
                if (_IsPatientIndentReceiveExists != value)
                {
                    _IsPatientIndentReceiveExists = value;
                    OnPropertyChanged("IsPatientIndentReceiveExists");
                }
            }
        }

        #region For Pediatric Flow

        private string _BabyWeight;
        public string BabyWeight
        {
            get { return _BabyWeight; }
            set
            {
                if (_BabyWeight != value)
                {
                    _BabyWeight = value;
                    OnPropertyChanged("BabyWeight");
                }
            }
        }

        #endregion

        #region For IPD Module

        private DateTime? _AdmissionDate = DateTime.Now;
        public DateTime? AdmissionDate
        {
            get { return _AdmissionDate; }
            set
            {
                if (_AdmissionDate != value)
                {
                    _AdmissionDate = value;
                    OnPropertyChanged("AdmissionDate");
                }
            }
        }

        private string _AddressLine1 = "";
        public string AddressLine1
        {
            get
            {
                return _AddressLine1;
            }
            set
            {
                if (_AddressLine1 != value)
                {
                    _AddressLine1 = value;
                    OnPropertyChanged("AddressLine1");
                }
            }
        }

        public int Age { get; set; }

        private long _VisitUnitID;
        public long VisitUnitID
        {
            get { return _VisitUnitID; }
            set
            {
                if (_VisitUnitID != value)
                {
                    _VisitUnitID = value;
                    OnPropertyChanged("VisitUnitID");
                }
            }
        }

        private long _VisitUnitId;
        public long VisitUnitId
        {
            get { return _VisitUnitId; }
            set { _VisitUnitId = value; }
        }

        public string ReferenceNo { get; set; }
        public string City { get; set; }

        private bool _SearchFromIPD = false;
        public bool SearchFromIPD
        {
            get { return _SearchFromIPD; }
            set
            {
                if (value != _SearchFromIPD)
                {
                    _SearchFromIPD = value;
                    OnPropertyChanged("SearchFromIPD");
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

        //added by neena
        private bool _IsDonor;
        public bool IsDonor
        {
            get { return _IsDonor; }
            set
            {
                if (_IsDonor != value)
                {
                    _IsDonor = value;
                    OnPropertyChanged("IsDonor");
                }
            }
        }
        //

        private bool _IsSurrogateAlreadyLinked;
        public bool IsSurrogateAlreadyLinked
        {
            get { return _IsSurrogateAlreadyLinked; }
            set
            {
                if (_IsSurrogateAlreadyLinked != value)
                {
                    _IsSurrogateAlreadyLinked = value;
                    OnPropertyChanged("IsSurrogateAlreadyLinked");
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
