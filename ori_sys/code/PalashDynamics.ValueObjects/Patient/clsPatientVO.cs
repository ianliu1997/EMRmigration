using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace PalashDynamics.ValueObjects.Patient
{


    public class clsPatientVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section

        private clsPatientGeneralVO _GeneralDetails = new clsPatientGeneralVO();
        public clsPatientGeneralVO GeneralDetails
        {
            get { return _GeneralDetails; }
            set { _GeneralDetails = value; }
        }

        private clsPatientSpouseVO _SpouseDetails = new clsPatientSpouseVO();
        public clsPatientSpouseVO SpouseDetails
        {
            get { return _SpouseDetails; }
            set { _SpouseDetails = value; }
        }      

        private List<clsKinInformationVO> _KinInformationList = new List<clsKinInformationVO>();
        public List<clsKinInformationVO> KinInformationList
        {
            get { return _KinInformationList; }
            set { _KinInformationList = value; }
        }

        public bool SelectPatient { get; set; }

        public bool IsVisitForPatho { get; set; }


        private long _DonorID;
        public long DonorID
        {
            get { return _DonorID; }
            set
            {
                if (_DonorID != value)
                {
                    _DonorID = value;
                    OnPropertyChanged("DonorID");
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
        private long _DonorUnitID;
        public long DonorUnitID
        {
            get { return _DonorUnitID; }
            set
            {
                if (_DonorUnitID != value)
                {
                    _DonorUnitID = value;
                    OnPropertyChanged("DonorUnitID");
                }
            }
        }
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
        private string _SurrogateOtherDetails;
        public string SurrogateOtherDetails
        {
            get { return _SurrogateOtherDetails; }
            set
            {
                if (_SurrogateOtherDetails != value)
                {
                    _SurrogateOtherDetails = value;
                    OnPropertyChanged("SurrogateOtherDetails");
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

        //* Added by - Ajit Jadhav
        //* Added Date - 5/8/2016
        //* Comments - Save BDID,PanNumber To T_Registration
        //***//---------------------
        private long _BDID;
        public long BDID
        {
            get { return _BDID; }
            set
            {
                if (_BDID != value)
                {
                    _BDID = value;
                    OnPropertyChanged("BDID");
                }
            }
        }

        //* Added Date - 11/8/2016
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

        public bool IsStaffPatient { get; set; }
        public long StaffID { get; set; }

        public bool IsPanNoSave { get; set; }

        private long _CampID; //***//
        public long CampID
        {
            get { return _CampID; }
            set
            {
                if (value != _CampID)
                {
                    _CampID = value;
                    OnPropertyChanged("CampID");
                }
            }
        }

        private long _AgentID; //***//
        public long AgentID
        {
            get { return _AgentID; }
            set
            {
                if (value != _AgentID)
                {
                    _AgentID = value;
                    OnPropertyChanged("AgentID");
                }
            }
        }

        public long NoOfYearsOfMarriage { get; set; }
        public long NoOfExistingChildren { get; set; }

        private long _FamilyTypeID;
        public long FamilyTypeID
        {
            get { return _FamilyTypeID; }
            set
            {
                if (value != _FamilyTypeID)
                {
                    _FamilyTypeID = value;
                    OnPropertyChanged("FamilyTypeID");
                }

            }
        }

        //-----------------------------------------
        private long _SpecialRegistrationID;
        public long SpecialRegistrationID
        {
            get { return _SpecialRegistrationID; }
            set
            {
                if (_SpecialRegistrationID != value)
                {
                    _SpecialRegistrationID = value;
                    OnPropertyChanged("SpecialRegistrationID");
                }
            }
        }

        private bool _IsIPDPatient;
        public bool IsIPDPatient
        {
            get { return _IsIPDPatient; }
            set
            {
                if (value != _IsIPDPatient)
                {
                    _IsIPDPatient = value;
                    OnPropertyChanged("IsIPDPatient");
                }
            }
        }

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
        public bool IsExistingBatch { get; set; }

        private long _MaritalStatusID;
        public long MaritalStatusID
        {
            get { return _MaritalStatusID; }
            set
            {
                if (value != _MaritalStatusID)
                {
                    _MaritalStatusID = value;
                    OnPropertyChanged("MaritalStatusID");
                }

            }
        }



        private long _IdentityID;
        public long IdentityID
        {
            get { return _IdentityID; }
            set
            {
                if (value != _IdentityID)
                {
                    _IdentityID = value;
                    OnPropertyChanged("IdentityID");
                }

            }
        }
        private string _IdentityNumber;
        public string IdentityNumber
        {
            get { return _IdentityNumber; }
            set
            {
                if (value != _IdentityNumber)
                {
                    _IdentityNumber = value;
                    OnPropertyChanged("IdentityNumber");
                }

            }
        }
        private string _Education;
        public string Education
        {
            get { return _Education; }
            set
            {
                if (value != _Education)
                {
                    _Education = value;
                    OnPropertyChanged("Education");
                }

            }
        }
        private string _RemarkForPatientType;
        public string RemarkForPatientType
        {
            get { return _RemarkForPatientType; }
            set
            {
                if (value != _RemarkForPatientType)
                {
                    _RemarkForPatientType = value;
                    OnPropertyChanged("RemarkForPatientType");
                }

            }
        }
        private bool _IsInternationalPatient;
        public bool IsInternationalPatient
        {
            get { return _IsInternationalPatient; }
            set
            {
                if (value != _IsInternationalPatient)
                {
                    _IsInternationalPatient = value;
                    OnPropertyChanged("IsInternationalPatient");
                }

            }
        }
        private string _ContactNo1;
        public string ContactNo1
        {
            get { return _ContactNo1; }
            set
            {
                if (_ContactNo1 != value)
                {
                    _ContactNo1 = value;
                    OnPropertyChanged("ContactNo1");
                }
            }
        }

        private string _CivilID = "";
        public string CivilID
        {
            get { return _CivilID; }
            set
            {
                if (_CivilID != value)
                {
                    _CivilID = value;
                    OnPropertyChanged("CivilID");
                }
            }
        }

        private string _ContactNo2 = "";
        public string ContactNo2
        {
            get { return _ContactNo2; }
            set
            {
                if (_ContactNo2 != value)
                {
                    _ContactNo2 = value;
                    OnPropertyChanged("ContactNo2");
                }
            }
        }

        private string _FaxNo = "";
        public string FaxNo
        {
            get { return _FaxNo; }
            set
            {
                if (_FaxNo != value)
                {
                    _FaxNo = value;
                    OnPropertyChanged("FaxNo");
                }
            }
        }

        private string _Email = "";
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

        private string _AddressLine1 = "";
        public string AddressLine1
        {
            get { return _AddressLine1; }
            set
            {
                if (_AddressLine1 != value)
                {
                    _AddressLine1 = value;
                    OnPropertyChanged("AddressLine1");
                }
            }
        }

        private string _AddressLine2 = "";
        public string AddressLine2
        {
            get { return _AddressLine2; }
            set
            {
                if (_AddressLine2 != value)
                {
                    _AddressLine2 = value;
                    OnPropertyChanged("AddressLine2");
                }
            }
        }

        private string _AddressLine3 = "";
        public string AddressLine3
        {
            get { return _AddressLine3; }
            set
            {
                if (_AddressLine3 != value)
                {
                    _AddressLine3 = value;
                    OnPropertyChanged("AddressLine3");
                }
            }
        }

        private string _Country = "";
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

        private string _State = "";
        public string State
        {
            get
            {
                if (_State == null)
                    return "";

                return _State;
            }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        private string _District = "";
        public string District
        {
            get
            {
                if (_District == null)
                    return "";
                return _District;
            }
            set
            {
                if (_District != value)
                {
                    _District = value;
                    OnPropertyChanged("District");
                }
            }
        }

        private string _Taluka = "";
        public string Taluka
        {
            get
            {
                if (_Taluka == null)
                    return "";
                return _Taluka;
            }
            set
            {
                if (_Taluka != value)
                {
                    _Taluka = value;
                    OnPropertyChanged("Taluka");
                }
            }
        }

        private string _City = "";
        public string City
        {
            get
            {
                if (_City == null)
                    return "";

                return _City;
            }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Area = "";
        public string Area
        {
            get
            {
                if (_Area == null)
                    return "";

                return _Area;
            }
            set
            {
                if (_Area != value)
                {
                    _Area = value;
                    OnPropertyChanged("Area");
                }
            }
        }

        private string _Pincode = "";
        public string Pincode
        {
            get
            {
                if (_Pincode == null)
                    return "";
                return _Pincode;
            }
            set
            {
                if (_Pincode != value)
                {
                    _Pincode = value;
                    OnPropertyChanged("Pincode");
                }
            }
        }

        private string _MobileCountryCode;
        public string MobileCountryCode
        {
            get { return _MobileCountryCode; }
            set
            {
                if (_MobileCountryCode != value)
                {
                    _MobileCountryCode = value;
                    OnPropertyChanged("MobileCountryCode");
                }
            }
        }

        private long _ResiNoCountryCode;
        public long ResiNoCountryCode
        {
            get { return _ResiNoCountryCode; }
            set
            {
                if (_ResiNoCountryCode != value)
                {
                    _ResiNoCountryCode = value;
                    OnPropertyChanged("ResiNoCountryCode");
                }
            }
        }

        private long _ResiSTDCode;
        public long ResiSTDCode
        {
            get { return _ResiSTDCode; }
            set
            {
                if (_ResiSTDCode != value)
                {
                    _ResiSTDCode = value;
                    OnPropertyChanged("ResiSTDCode");
                }
            }
        }

        private long _ReligionID;
        public long ReligionID
        {
            get { return _ReligionID; }
            set
            {
                if (_ReligionID != value)
                {
                    _ReligionID = value;
                    OnPropertyChanged("ReligionID");
                }
            }
        }

        private long _OccupationId;
        public long OccupationId
        {
            get { return _OccupationId; }
            set
            {
                if (_OccupationId != value)
                {
                    _OccupationId = value;
                    OnPropertyChanged("OccupationId");
                }
            }
        }

        private bool _IsLoyaltyMember;
        public bool IsLoyaltyMember
        {
            get { return _IsLoyaltyMember; }
            set
            {
                if (_IsLoyaltyMember != value)
                {
                    _IsLoyaltyMember = value;
                    OnPropertyChanged("IsLoyaltyMember");
                }
            }
        }

        private long? _LoyaltyCardID;
        public long? LoyaltyCardID
        {
            get { return _LoyaltyCardID; }
            set
            {
                if (_LoyaltyCardID != value)
                {
                    _LoyaltyCardID = value;
                    OnPropertyChanged("LoyaltyCardID");
                }
            }
        }

        private DateTime? _IssueDate;
        public DateTime? IssueDate
        {
            get { return _IssueDate; }
            set
            {
                if (_IssueDate != value)
                {
                    _IssueDate = value;
                    OnPropertyChanged("IssueDate");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate != value)
                {
                    _ExpiryDate = value;
                    OnPropertyChanged("ExpiryDate");
                }
            }
        }

        private DateTime? _EffectiveDate;
        public DateTime? EffectiveDate
        {
            get { return _EffectiveDate; }
            set
            {
                if (_EffectiveDate != value)
                {
                    _EffectiveDate = value;
                    OnPropertyChanged("EffectiveDate");
                }
            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (_TariffID != value)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }
        public long PatientSourceID { get; set; }
        public long CompanyID { get; set; }
        private DateTime _RegistrationDate;
        public DateTime RegistrationDate
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
        private string _ReferralName;
        public string ReferralName
        {
            get { return _ReferralName; }
            set
            {
                if (_ReferralName != value)
                {
                    _ReferralName = value;
                    OnPropertyChanged("ReferralName");
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
        public string UniversalID { get; set; }
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
        public long PatientSponsorCategoryID { get; set; }

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
        //

        private long _ParentPatientID;
        public long ParentPatientID
        {
            get { return _ParentPatientID; }
            set
            {
                if (_ParentPatientID != value)
                {
                    _ParentPatientID = value;
                    OnPropertyChanged("ParentPatientID");
                }
            }
        }


        private string _Remark = "";
        public string Remark
        {
            get
            {
                if (_Remark == null)
                    return "";

                return _Remark;
            }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _DocName = "";
        public string Doctor
        {
            get
            {
                if (_DocName == null)
                    return "";

                return _DocName;
            }
            set
            {
                if (_DocName != value)
                {
                    _DocName = value;
                    OnPropertyChanged("Doctor");
                }
            }
        }

        private string _LoyaltyCardNo = "";
        public string LoyaltyCardNo
        {
            get
            {
                if (_LoyaltyCardNo == null)
                    return "";

                return _LoyaltyCardNo;
            }
            set
            {
                if (_LoyaltyCardNo != value)
                {
                    _LoyaltyCardNo = value;
                    OnPropertyChanged("LoyaltyCardNo");
                }
            }
        }

        private string _PreferNameonLoyaltyCard = "";
        public string PreferNameonLoyaltyCard
        {
            get
            {
                if (_PreferNameonLoyaltyCard == null)
                    return "";

                return _PreferNameonLoyaltyCard;
            }
            set
            {
                if (_PreferNameonLoyaltyCard != value)
                {
                    _PreferNameonLoyaltyCard = value;
                    OnPropertyChanged("PreferNameonLoyaltyCard");
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

        private string _OldRegistrationNo;
        public string OldRegistrationNo
        {
            get { return _OldRegistrationNo; }
            set
            {
                if (value != _OldRegistrationNo)
                {
                    _OldRegistrationNo = value;
                }
            }
        }
        //

        private List<clsPatientScanDocumentVO> _ScanDocList;
        public List<clsPatientScanDocumentVO> ScanDocList
        {
            get
            {
                if (_ScanDocList == null)
                    _ScanDocList = new List<clsPatientScanDocumentVO>();

                return _ScanDocList;
            }

            set
            {

                _ScanDocList = value;

            }
        }

        private List<clsPatientFamilyDetailsVO> _LoyaltyProgramDetails;
        public List<clsPatientFamilyDetailsVO> FamilyDetails
        {
            get
            {
                if (_LoyaltyProgramDetails == null)
                    _LoyaltyProgramDetails = new List<clsPatientFamilyDetailsVO>();

                return _LoyaltyProgramDetails;
            }

            set
            {

                _LoyaltyProgramDetails = value;

            }
        }

        private clsPatientOtherDetailsVO _OtherDetails = new clsPatientOtherDetailsVO();
        public clsPatientOtherDetailsVO OtherDetails
        {
            get { return _OtherDetails; }
            set { _OtherDetails = value; }
        }
        private List<clsPatientServiceDetails> _ServiceDetails;
        public List<clsPatientServiceDetails> ServiceDetails
        {
            get
            {
                if (_ServiceDetails == null)
                    _ServiceDetails = new List<clsPatientServiceDetails>();

                return _ServiceDetails;
            }

            set
            {

                _ServiceDetails = value;

            }
        }
        private string _FirstName = "";
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

        private long _PrefixId;
        public long PrefixId
        {
            get { return _PrefixId; }
            set
            {
                if (_PrefixId != value)
                {
                    _PrefixId = value;
                    OnPropertyChanged("PrefixId");
                }
            }
        }
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

        private long _PatientCategoryIDForPath;
        public long PatientCategoryIDForPath
        {
            get { return _PatientCategoryIDForPath; }
            set
            {
                if (_PatientCategoryIDForPath != value)
                {
                    _PatientCategoryIDForPath = value;
                    OnPropertyChanged("PatientCategoryIDForPath");
                }
            }
        }

        private string _Prefix;
        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                if (_Prefix != value)
                {
                    _Prefix = value;
                    OnPropertyChanged("Prefix");
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

        private byte[] _GenderImage;
        public byte[] GenderImage
        {
            get { return _GenderImage; }
            set
            {
                if (_GenderImage != value)
                {
                    _GenderImage = value;
                    OnPropertyChanged("GenderImage");
                }
            }
        }
        private long _CountryID;
        public long CountryID
        {
            get { return _CountryID; }
            set
            {
                if (_CountryID != value)
                {
                    _CountryID = value;
                    OnPropertyChanged("CountryID");
                }
            }
        }

        private long _StateID;
        public long StateID
        {
            get { return _StateID; }
            set
            {
                if (_StateID != value)
                {
                    _StateID = value;
                    OnPropertyChanged("StateID");
                }
            }
        }

        private long _CityID;
        public long CityID
        {
            get { return _CityID; }
            set
            {
                if (_CityID != value)
                {
                    _CityID = value;
                    OnPropertyChanged("CityID");
                }
            }
        }

        private long _RegionID;
        public long RegionID
        {
            get { return _RegionID; }
            set
            {
                if (_RegionID != value)
                {
                    _RegionID = value;
                    OnPropertyChanged("RegionID");
                }
            }
        }

        //added by neena
        private string _Region;
        public string Region
        {
            get
            {
                return _Region;
            }

            set
            {
                if (value != _Region)
                {
                    _Region = value;
                    OnPropertyChanged("Region");
                }
            }
        }

        private string _RegionCode;
        public string RegionCode
        {
            get
            {
                return _RegionCode;
            }

            set
            {
                if (value != _RegionCode)
                {
                    _RegionCode = value;
                    OnPropertyChanged("RegionCode");
                }
            }
        }

        private string _CityN;
        public string CityN
        {
            get
            {
                return _CityN;
            }

            set
            {
                if (value != _CityN)
                {
                    _CityN = value;
                    OnPropertyChanged("CityN");
                }
            }
        }

        private string _CityCode;
        public string CityCode
        {
            get
            {
                return _CityCode;
            }

            set
            {
                if (value != _CityCode)
                {
                    _CityCode = value;
                    OnPropertyChanged("_CityCode");
                }
            }
        }

        private string _StateN;
        public string StateN
        {
            get
            {
                return _StateN;
            }

            set
            {
                if (value != _StateN)
                {
                    _StateN = value;
                    OnPropertyChanged("_StateN");
                }
            }
        }

        private string _StateCode;
        public string StateCode
        {
            get
            {
                return _StateCode;
            }

            set
            {
                if (value != _StateCode)
                {
                    _StateCode = value;
                    OnPropertyChanged("_StateCode");
                }
            }
        }

        private string _CountryN;
        public string CountryN
        {
            get
            {
                return _CountryN;
            }

            set
            {
                if (value != _CountryN)
                {
                    _CountryN = value;
                    OnPropertyChanged("_CountryN");
                }
            }
        }

        private string _CountryCode;
        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }

            set
            {
                if (value != _CountryCode)
                {
                    _CountryCode = value;
                    OnPropertyChanged("_CountryCode");
                }
            }
        }
        //

        //For Donor
        private long _DonorSourceID;
        public long DonorSourceID
        {
            get { return _DonorSourceID; }
            set
            {
                if (_DonorSourceID != value)
                {
                    _DonorSourceID = value;
                    OnPropertyChanged("DonorSourceID");
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
        public string _Eye;
        public string Eye
        {
            get { return _Eye; }
            set
            {
                if (_Eye != value)
                {
                    _Eye = value;
                    OnPropertyChanged("Eye");
                }
            }
        }
        private string _Hair;
        public string Hair
        {
            get { return _Hair; }
            set
            {
                if (_Hair != value)
                {
                    _Hair = value;
                    OnPropertyChanged("Hair");
                }
            }
        }
        private long _SkinColorID;
        public long SkinColorID
        {
            get { return _SkinColorID; }
            set
            {
                if (_SkinColorID != value)
                {
                    _SkinColorID = value;
                    OnPropertyChanged("SkinColorID");
                }
            }
        }
        private long _HairColorID;
        public long HairColorID
        {
            get { return _HairColorID; }
            set
            {
                if (_HairColorID != value)
                {
                    _HairColorID = value;
                    OnPropertyChanged("HairColorID");
                }
            }
        }
        private long _EyeColorID;
        public long EyeColorID
        {
            get { return _EyeColorID; }
            set
            {
                if (_EyeColorID != value)
                {
                    _EyeColorID = value;
                    OnPropertyChanged("EyeColorID");
                }
            }
        }
        private string _BoneStructure;
        public string BoneStructure
        {
            get { return _BoneStructure; }
            set
            {
                if (_BoneStructure != value)
                {
                    _BoneStructure = value;
                    OnPropertyChanged("BoneStructure");
                }
            }
        }
        private string _Skin;
        public string Skin
        {
            get { return _Skin; }
            set
            {
                if (_Skin != value)
                {
                    _Skin = value;
                    OnPropertyChanged("Skin");
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
        private string _BloodGroup;
        public string BloodGroup
        {
            get { return _BloodGroup; }
            set
            {
                if (_BloodGroup != value)
                {
                    _BloodGroup = value;
                    OnPropertyChanged("BloodGroup");
                }
            }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    _Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }




        //...................



        #region For IPD Module

        private bool _IsLatestPatient = false;
        public bool IsLatestPatient
        {
            get { return _IsLatestPatient; }
            set
            {
                if (_IsLatestPatient != value)
                {
                    _IsLatestPatient = value;
                    OnPropertyChanged("IsLatestPatient");
                }
            }
        }

        private string _MobileNo2 = "";
        public string MobileNo2
        {
            get { return _MobileNo2; }
            set
            {
                if (_MobileNo2 != value)
                {
                    _MobileNo2 = value;
                    OnPropertyChanged("MobileNo2");
                }
            }
        }

        private DateTime? _AdmissionDate;
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

        private bool _ISDischarged = false;
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

        #endregion

        #region For Pediatric Flow

        private int? _BabyNo;
        public int? BabyNo
        {
            get { return _BabyNo; }
            set
            {
                if (_BabyNo != value)
                {
                    _BabyNo = value;
                    OnPropertyChanged("BabyNo");
                }
            }
        }

        private int? _BabyOfNo;
        public int? BabyOfNo
        {
            get { return _BabyOfNo; }
            set
            {
                if (_BabyOfNo != value)
                {
                    _BabyOfNo = value;
                    OnPropertyChanged("BabyOfNo");
                }
            }
        }

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

        //For Pediatric flow : to maintain link parent patient id with baby
        private long _LinkPatientID;
        public long LinkPatientID
        {
            get { return _LinkPatientID; }
            set
            {
                if (_LinkPatientID != value)
                {
                    _LinkPatientID = value;
                    OnPropertyChanged("LinkPatientID");
                }
            }
        }

        //For Pediatric flow : to maintain link parent patient unitid with baby
        private long _LinkPatientUnitID;
        public long LinkPatientUnitID
        {
            get { return _LinkPatientUnitID; }
            set
            {
                if (_LinkPatientUnitID != value)
                {
                    _LinkPatientUnitID = value;
                    OnPropertyChanged("LinkPatientUnitID");
                }
            }
        }

        private string _LinkPatientMrNo;
        public string LinkPatientMrNo
        {
            get { return _LinkPatientMrNo; }
            set
            {
                if (_LinkPatientMrNo != value)
                {
                    _LinkPatientMrNo = value;
                    OnPropertyChanged("LinkPatientMrNo");
                }
            }
        }


        private string _LinkParentName;
        public string LinkParentName
        {
            get { return _LinkParentName; }
            set
            {
                if (_LinkParentName != value)
                {
                    _LinkParentName = value;
                    OnPropertyChanged("LinkParentName");
                }
            }
        }

        #endregion

        #endregion

        #region Common Properties



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

    //Added By Somnath

    public class clsPatientAttachmentVO : IValueObject
    {

        public DateTime Date { get; set; }

        public byte[] Attachment { get; set; }

        public string CasePaperType { get; set; }

        public string AttachedFileName { get; set; }

        public string Doctor { get; set; }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }
    //End
    public class clsPatientScanDocumentVO : IValueObject
    {
        public string EMRImage { get; set; }
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    // OnPropertyChanged("ID");
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
                    //OnPropertyChanged("UnitID");
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
                    //OnPropertyChanged("PatientID");
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
                    //OnPropertyChanged("PatientUnitID");
                }
            }
        }
        private long _IdentityID;
        public long IdentityID
        {
            get { return _IdentityID; }
            set
            {
                if (_IdentityID != value)
                {
                    _IdentityID = value;
                    //OnPropertyChanged("Title");
                }
            }
        }
        private string _Identity;
        public string Identity
        {
            get { return _Identity; }
            set
            {
                if (_Identity != value)
                {
                    _Identity = value;
                    //OnPropertyChanged("Title");
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
                    //OnPropertyChanged("Title");
                }
            }
        }
        private bool _IsForSpouse;
        public bool IsForSpouse
        {
            get { return _IsForSpouse; }
            set
            {
                if (_IsForSpouse != value)
                {
                    _IsForSpouse = value;
                    //OnPropertyChanged("Description");
                }
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    //OnPropertyChanged("Description");
                }
            }
        }
        private string _AttachedFileName;
        public string AttachedFileName
        {
            get { return _AttachedFileName; }
            set
            {
                if (_AttachedFileName != value)
                {
                    _AttachedFileName = value;
                    //OnPropertyChanged("AttachedFileName");
                }
            }
        }
        private byte[] _AttachedFileContent;
        public byte[] AttachedFileContent
        {
            get { return _AttachedFileContent; }
            set
            {
                if (_AttachedFileContent != value)
                {
                    _AttachedFileContent = value;
                    // OnPropertyChanged("AttachedFileContent");
                }
            }
        }
        private bool _IsDeleted;
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                if (_IsDeleted != value)
                {
                    _IsDeleted = value;
                    //OnPropertyChanged("IsDeleted");
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
                    //OnPropertyChanged("Isfreezed");
                }
            }
        }
        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    // OnPropertyChanged("Status");
                }
            }
        }

        //***//
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

        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set
            {
                if (_DoctorID != value)
                {
                    _DoctorID = value;                    
                }
            }
        }
        //--

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }

    public class clsPatientSpouseVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section



        //* Added by - Ajit Jadhav
        //* Added Date - 11/8/2016
        //* Comments - new valu object to save pan no To T_Registration
        private string _SpousePanNumber;
        public string SpousePanNumber
        {
            get { return _SpousePanNumber; }
            set
            {
                if (_SpousePanNumber != value)
                {
                    _SpousePanNumber = value;
                    OnPropertyChanged("SpousePanNumber");
                }
            }

        }

        private string _IdentityNumber;
        public string IdentityNumber
        {
            get { return _IdentityNumber; }
            set
            {
                if (value != _IdentityNumber)
                {
                    _IdentityNumber = value;
                    OnPropertyChanged("IdentityNumber");
                }

            }
        }


        private string _Education;
        public string Education
        {
            get { return _Education; }
            set
            {
                if (value != _Education)
                {
                    _Education = value;
                    OnPropertyChanged("Education");
                }

            }
        }
        private string _RemarkForPatientType;
        public string RemarkForPatientType
        {
            get { return _RemarkForPatientType; }
            set
            {
                if (value != _RemarkForPatientType)
                {
                    _RemarkForPatientType = value;
                    OnPropertyChanged("RemarkForPatientType");
                }

            }
        }
        private bool _IsInternationalPatient;
        public bool IsInternationalPatient
        {
            get { return _IsInternationalPatient; }
            set
            {
                if (value != _IsInternationalPatient)
                {
                    _IsInternationalPatient = value;
                    OnPropertyChanged("IsInternationalPatient");
                }

            }
        }


        private long _PrefixId;
        public long PrefixId
        {
            get { return _PrefixId; }
            set
            {
                if (_PrefixId != value)
                {
                    _PrefixId = value;
                    OnPropertyChanged("PrefixId");
                }
            }
        }

        private long _IdentityID;
        public long IdentityID
        {
            get { return _IdentityID; }
            set
            {
                if (value != _IdentityID)
                {
                    _IdentityID = value;
                    OnPropertyChanged("IdentityID");
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
        private long _CountryID;
        public long CountryID
        {
            get { return _CountryID; }
            set
            {
                if (_CountryID != value)
                {
                    _CountryID = value;
                    OnPropertyChanged("CountryID");
                }
            }
        }

        private long _StateID;
        public long StateID
        {
            get { return _StateID; }
            set
            {
                if (_StateID != value)
                {
                    _StateID = value;
                    OnPropertyChanged("StateID");
                }
            }
        }

        private long _CityID;
        public long CityID
        {
            get { return _CityID; }
            set
            {
                if (_CityID != value)
                {
                    _CityID = value;
                    OnPropertyChanged("CityID");
                }
            }
        }

        private long _RegionID;
        public long RegionID
        {
            get { return _RegionID; }
            set
            {
                if (_RegionID != value)
                {
                    _RegionID = value;
                    OnPropertyChanged("RegionID");
                }
            }
        }

        //added by neena
        private string _Region;
        public string Region
        {
            get
            {
                return _Region;
            }

            set
            {
                if (value != _Region)
                {
                    _Region = value;
                    OnPropertyChanged("Region");
                }
            }
        }

        private string _RegionCode;
        public string RegionCode
        {
            get
            {
                return _RegionCode;
            }

            set
            {
                if (value != _RegionCode)
                {
                    _RegionCode = value;
                    OnPropertyChanged("RegionCode");
                }
            }
        }

        private string _CityN;
        public string CityN
        {
            get
            {
                return _CityN;
            }

            set
            {
                if (value != _CityN)
                {
                    _CityN = value;
                    OnPropertyChanged("CityN");
                }
            }
        }

        private string _CityCode;
        public string CityCode
        {
            get
            {
                return _CityCode;
            }

            set
            {
                if (value != _CityCode)
                {
                    _CityCode = value;
                    OnPropertyChanged("_CityCode");
                }
            }
        }

        private string _StateN;
        public string StateN
        {
            get
            {
                return _StateN;
            }

            set
            {
                if (value != _StateN)
                {
                    _StateN = value;
                    OnPropertyChanged("_StateN");
                }
            }
        }

        private string _StateCode;
        public string StateCode
        {
            get
            {
                return _StateCode;
            }

            set
            {
                if (value != _StateCode)
                {
                    _StateCode = value;
                    OnPropertyChanged("_StateCode");
                }
            }
        }

        private string _CountryN;
        public string CountryN
        {
            get
            {
                return _CountryN;
            }

            set
            {
                if (value != _CountryN)
                {
                    _CountryN = value;
                    OnPropertyChanged("_CountryN");
                }
            }
        }

        private string _CountryCode;
        public string CountryCode
        {
            get
            {
                return _CountryCode;
            }

            set
            {
                if (value != _CountryCode)
                {
                    _CountryCode = value;
                    OnPropertyChanged("_CountryCode");
                }
            }
        }

        private string _SpouseOldRegistrationNo;
        public string SpouseOldRegistrationNo
        {
            get { return _SpouseOldRegistrationNo; }
            set
            {
                if (value != _SpouseOldRegistrationNo)
                {
                    _SpouseOldRegistrationNo = value;
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

        // BYBHUSHAn . . 

        private string _SpouseCompanyName = "";
        public string SpouseCompanyName
        {
            get { return _SpouseCompanyName; }
            set
            {
                if (_SpouseCompanyName != value)
                {
                    _SpouseCompanyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }


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

        //public string UniversalID { get; set; }


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

        private long _MaritalStatusID;
        public long MaritalStatusID
        {
            get { return _MaritalStatusID; }
            set
            {
                if (value != _MaritalStatusID)
                {
                    _MaritalStatusID = value;
                    OnPropertyChanged("MaritalStatusID");
                }

            }
        }

        private string _ContactNo1 = "";
        public string ContactNo1
        {
            get { return _ContactNo1; }
            set
            {
                if (_ContactNo1 != value)
                {
                    _ContactNo1 = value;
                    OnPropertyChanged("ContactNo1");
                }
            }
        }

        private string _CivilID = "";
        public string CivilID
        {
            get { return _CivilID; }
            set
            {
                if (_CivilID != value)
                {
                    _CivilID = value;
                    OnPropertyChanged("CivilID");
                }
            }
        }

        private string _ContactNo2 = "";
        public string ContactNo2
        {
            get { return _ContactNo2; }
            set
            {
                if (_ContactNo2 != value)
                {
                    _ContactNo2 = value;
                    OnPropertyChanged("ContactNo2");
                }
            }
        }

        private string _FaxNo = "";
        public string FaxNo
        {
            get { return _FaxNo; }
            set
            {
                if (_FaxNo != value)
                {
                    _FaxNo = value;
                    OnPropertyChanged("FaxNo");
                }
            }
        }

        private string _Email = "";
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

        private string _AddressLine1 = "";
        public string AddressLine1
        {
            get { return _AddressLine1; }
            set
            {
                if (_AddressLine1 != value)
                {
                    _AddressLine1 = value;
                    OnPropertyChanged("AddressLine1");
                }
            }
        }

        private string _AddressLine2 = "";
        public string AddressLine2
        {
            get { return _AddressLine2; }
            set
            {
                if (_AddressLine2 != value)
                {
                    _AddressLine2 = value;
                    OnPropertyChanged("AddressLine2");
                }
            }
        }

        private string _AddressLine3 = "";
        public string AddressLine3
        {
            get { return _AddressLine3; }
            set
            {
                if (_AddressLine3 != value)
                {
                    _AddressLine3 = value;
                    OnPropertyChanged("AddressLine3");
                }
            }
        }

        private string _Country = "";
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

        private string _State = "";
        public string State
        {
            get
            {
                if (_State == null)
                    return "";

                return _State;
            }
            set
            {
                if (_State != value)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        private string _District = "";
        public string District
        {
            get
            {
                if (_District == null)
                    return "";
                return _District;
            }
            set
            {
                if (_District != value)
                {
                    _District = value;
                    OnPropertyChanged("District");
                }
            }
        }

        private string _Taluka = "";
        public string Taluka
        {
            get
            {
                if (_Taluka == null)
                    return "";
                return _Taluka;
            }
            set
            {
                if (_Taluka != value)
                {
                    _Taluka = value;
                    OnPropertyChanged("Taluka");
                }
            }
        }

        private string _City = "";
        public string City
        {
            get
            {
                if (_City == null)
                    return "";

                return _City;
            }
            set
            {
                if (_City != value)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Area = "";
        public string Area
        {
            get
            {
                if (_Area == null)
                    return "";

                return _Area;
            }
            set
            {
                if (_Area != value)
                {
                    _Area = value;
                    OnPropertyChanged("Area");
                }
            }
        }

        private string _Pincode = "";
        public string Pincode
        {
            get
            {
                if (_Pincode == null)
                    return "";
                return _Pincode;
            }
            set
            {
                if (_Pincode != value)
                {
                    _Pincode = value;
                    OnPropertyChanged("Pincode");
                }
            }
        }

        private string _MobileCountryCode;
        public string MobileCountryCode
        {
            get { return _MobileCountryCode; }
            set
            {
                if (_MobileCountryCode != value)
                {
                    _MobileCountryCode = value;
                    OnPropertyChanged("MobileCountryCode");
                }
            }
        }

        private long _ResiNoCountryCode;
        public long ResiNoCountryCode
        {
            get { return _ResiNoCountryCode; }
            set
            {
                if (_ResiNoCountryCode != value)
                {
                    _ResiNoCountryCode = value;
                    OnPropertyChanged("ResiNoCountryCode");
                }
            }
        }

        private long _ResiSTDCode;
        public long ResiSTDCode
        {
            get { return _ResiSTDCode; }
            set
            {
                if (_ResiSTDCode != value)
                {
                    _ResiSTDCode = value;
                    OnPropertyChanged("ResiSTDCode");
                }
            }
        }

        private long _ReligionID;
        public long ReligionID
        {
            get { return _ReligionID; }
            set
            {
                if (_ReligionID != value)
                {
                    _ReligionID = value;
                    OnPropertyChanged("ReligionID");
                }
            }
        }

        private long _OccupationId;
        public long OccupationId
        {
            get { return _OccupationId; }
            set
            {
                if (_OccupationId != value)
                {
                    _OccupationId = value;
                    OnPropertyChanged("OccupationId");
                }
            }
        }









        public byte[] Photo { get; set; }

        //added By akshays On 17/11/2015
        private byte[] _SpouseBarcodeImage;
        public byte[] SpouseBarcodeImage
        {
            get { return _SpouseBarcodeImage; }
            set
            {
                if (_SpouseBarcodeImage != value)
                {
                    _SpouseBarcodeImage = value;
                    OnPropertyChanged("SpouseBarcodeImage");
                }
            }
        }
        private int _Flag1;
        public int Flag1
        {
            get { return _Flag1; }
            set
            {
                if (_Flag1 != value)
                {
                    _Flag1 = value;
                    OnPropertyChanged("Flag1");
                }
            }
        }
        //closed by akshays on 17/11/2015

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
        public bool IsAge { get; set; }
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
        //


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


        #endregion

        #region Common Properties




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

    public class clsPatientPenPusherInfoVO : IValueObject, INotifyPropertyChanged
    {
        private List<clsPatientPerscriptionInfoVO> _PatientPerscriptionInfoList;
        public List<clsPatientPerscriptionInfoVO> PatientPerscriptionInfoList
        {
            get
            {
                if (_PatientPerscriptionInfoList == null)
                    _PatientPerscriptionInfoList = new List<clsPatientPerscriptionInfoVO>();

                return _PatientPerscriptionInfoList;
            }

            set
            {

                _PatientPerscriptionInfoList = value;

            }
        }
        private clsPatientPerscriptionInfoVO _PatientPerscriptionInfo;
        public clsPatientPerscriptionInfoVO PatientPerscriptionInfo
        {
            get
            {
                if (_PatientPerscriptionInfo == null)
                    _PatientPerscriptionInfo = new clsPatientPerscriptionInfoVO();

                return _PatientPerscriptionInfo;
            }

            set
            {

                _PatientPerscriptionInfo = value;

            }
        }

        #region Property Declaration Section


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

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }

            }
        }

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

        //private string strPatientName = "";
        //public string PatientName
        //{
        //    get { return strPatientName = _FirstName + " " + _MiddleName + " " + _LastName; }
        //    set
        //    {
        //        if (value != strPatientName)
        //        {
        //            strPatientName = value;
        //            OnPropertyChanged("PatientName");
        //        }
        //    }
        //}
        private string _PatientName = "";
        public string PatientName
        {
            get { return _PatientName; }
            set
            {
                if (_PatientName != value)
                {
                    _PatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }


        //private long _Days;
        //public long Days
        //{
        //    get { return _Days; }
        //    set
        //    {
        //        if (_Days != value)
        //        {
        //            _Days = value;
        //            OnPropertyChanged("Days");
        //        }
        //    }
        //}

        //private string _Dose = "";
        //public string Dose
        //{
        //    get { return _Dose; }
        //    set
        //    {
        //        if (_Dose != value)
        //        {
        //            _Dose = value;
        //            OnPropertyChanged("Dose");
        //        }
        //    }
        //}



        //private long _DrugID;
        //public long DrugID
        //{
        //    get { return _DrugID; }
        //    set
        //    {
        //        if (_DrugID != value)
        //        {
        //            _DrugID = value;
        //            OnPropertyChanged("DrugID");
        //        }
        //    }
        //}



        //private string _Frequency = "";
        //public string Frequency
        //{
        //    get { return _Frequency; }
        //    set
        //    {
        //        if (_Frequency != value)
        //        {
        //            _Frequency = value;
        //            OnPropertyChanged("Frequency");
        //        }
        //    }
        //}

        //private string _ItemName = "";
        //public string ItemName
        //{
        //    get { return _ItemName; }
        //    set
        //    {
        //        if (_ItemName != value)
        //        {
        //            _ItemName = value;
        //            OnPropertyChanged("ItemName");
        //        }
        //    }
        //}



        //private long _PrescriptionID;
        //public long PrescriptionID
        //{
        //    get { return _PrescriptionID; }
        //    set
        //    {
        //        if (_PrescriptionID != value)
        //        {
        //            _PrescriptionID = value;
        //            OnPropertyChanged("PrescriptionID");
        //        }
        //    }
        //}


        //private int _Quantity;
        //public int Quantity
        //{
        //    get { return _Quantity; }
        //    set
        //    {
        //        if (_Quantity != value)
        //        {
        //            _Quantity = value;
        //            OnPropertyChanged("Quantity");
        //        }
        //    }
        //}


        //private string _Reason = "";
        //public string Reason
        //{
        //    get { return _Reason; }
        //    set
        //    {
        //        if (_Reason != value)
        //        {
        //            _Reason = value;
        //            OnPropertyChanged("Reason");
        //        }
        //    }
        //}


        //private string _Route = "";
        //public string Route
        //{
        //    get { return _Route; }
        //    set
        //    {
        //        if (_Route != value)
        //        {
        //            _Route = value;
        //            OnPropertyChanged("Route");
        //        }
        //    }
        //}

        #endregion

        #region Common Properties




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

    public class clsPatientPerscriptionInfoVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section

        private DateTime _Date;
        public DateTime Date
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
        private long _Days;
        public long Days
        {
            get { return _Days; }
            set
            {
                if (_Days != value)
                {
                    _Days = value;
                    OnPropertyChanged("Days");
                }
            }
        }

        private string _Dose = "";
        public string Dose
        {
            get { return _Dose; }
            set
            {
                if (_Dose != value)
                {
                    _Dose = value;
                    OnPropertyChanged("Dose");
                }
            }
        }



        private long _DrugID;
        public long DrugID
        {
            get { return _DrugID; }
            set
            {
                if (_DrugID != value)
                {
                    _DrugID = value;
                    OnPropertyChanged("DrugID");
                }
            }
        }



        private string _Frequency = "";
        public string Frequency
        {
            get { return _Frequency; }
            set
            {
                if (_Frequency != value)
                {
                    _Frequency = value;
                    OnPropertyChanged("Frequency");
                }
            }
        }

        private string _ItemName = "";
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }



        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set
            {
                if (_PrescriptionID != value)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
                }
            }
        }


        private int _Quantity;
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }


        private string _Reason = "";
        public string Reason
        {
            get { return _Reason; }
            set
            {
                if (_Reason != value)
                {
                    _Reason = value;
                    OnPropertyChanged("Reason");
                }
            }
        }


        private string _Route = "";
        public string Route
        {
            get { return _Route; }
            set
            {
                if (_Route != value)
                {
                    _Route = value;
                    OnPropertyChanged("Route");
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
    public class clsPatientLinkFileBizActionVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {
                if (_TemplateID != value)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
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



        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                if (_IsCompleted != value)
                {
                    _IsCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }

        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
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

        private string _DocumentName;
        public string DocumentName
        {
            get { return _DocumentName; }
            set
            {
                if (_DocumentName != value)
                {
                    _DocumentName = value;
                    OnPropertyChanged("DocumentName");
                }
            }
        }

        public string SourceURL { get; set; }
        public byte[] AttachmentFileContent { get; set; }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }


        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
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



        private string _ReferredBy;
        public string ReferredBy
        {
            get { return _ReferredBy; }
            set
            {
                if (_ReferredBy != value)
                {
                    _ReferredBy = value;
                    OnPropertyChanged("ReferredBy");
                }
            }
        }



        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                if (Path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
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

    public class clsPatientFollowUpImageVO : IValueObject, INotifyPropertyChanged
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
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
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }

            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }

            }
        }

        private string _SourceURL;
        public string SourceURL
        {
            get { return _SourceURL; }
            set
            {
                if (value != _SourceURL)
                {
                    _SourceURL = value;
                    OnPropertyChanged("SourceURL");
                }

            }
        }

        private Byte[] _EditImage;
        public Byte[] EditImage
        {
            get { return _EditImage; }
            set
            {
                if (_EditImage != value)
                {
                    _EditImage = value;
                    OnPropertyChanged("EditImage");
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

    public class clsPatientProspectServiceInfoVO : IValueObject
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long ProspectSaleID { get; set; }
        public long ServiceID { get; set; }
        public bool Status { get; set; }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }

    public class clsPatientProspectInterestedInfoVO : IValueObject
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long ProspectSaleID { get; set; }
        public long InterestedID { get; set; }
        public bool Status { get; set; }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }


    public class clsPatientProspectSalesInfoVO : IValueObject
    {
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long ProspectID { get; set; }
        public long ProspectUnitID { get; set; }
        public bool Interested { get; set; }
        public long TypeOfLead { get; set; }
        public long StatusID { get; set; }
        public long L1 { get; set; }
        public long L2 { get; set; }
        public long L3 { get; set; }
        public long EventID { get; set; }
        public long FollowUpID { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string FollowUpRemark { get; set; }
        public bool Status { get; set; }

        private List<clsPatientProspectServiceInfoVO> objSaleServiceList = null;
        public List<clsPatientProspectServiceInfoVO> SaleServiceList
        {
            get { return objSaleServiceList; }
            set { objSaleServiceList = value; }
        }

        private List<clsPatientProspectInterestedInfoVO> objSaleInterestedList = null;
        public List<clsPatientProspectInterestedInfoVO> SaleInterestedList
        {
            get { return objSaleInterestedList; }
            set { objSaleInterestedList = value; }
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }

    public class clsTemplateImageBizActionVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section

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



        private bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }
            set
            {
                if (_IsCompleted != value)
                {
                    _IsCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }

        private bool _IsFinalized;
        public bool IsFinalized
        {
            get { return _IsFinalized; }
            set
            {
                if (_IsFinalized != value)
                {
                    _IsFinalized = value;
                    OnPropertyChanged("IsFinalized");
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

        private string _DocumentName;
        public string DocumentName
        {
            get { return _DocumentName; }
            set
            {
                if (_DocumentName != value)
                {
                    _DocumentName = value;
                    OnPropertyChanged("DocumentName");
                }
            }
        }

        public string SourceURL { get; set; }
        public byte[] AttachmentFileContent { get; set; }

        private byte[] _Report;
        public byte[] Report
        {
            get { return _Report; }
            set
            {
                if (_Report != value)
                {
                    _Report = value;
                    OnPropertyChanged("Report");
                }
            }
        }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }


        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                if (_Remarks != value)
                {
                    _Remarks = value;
                    OnPropertyChanged("Remarks");
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

        private bool _IsModifyandAttRep;
        public bool IsModifyandAttRep
        {
            get { return _IsModifyandAttRep; }
            set
            {
                if (_IsModifyandAttRep != value)
                {
                    _IsModifyandAttRep = value;
                    OnPropertyChanged("IsModifyandAttRep");
                }
            }
        }

        private string _ReferredBy;
        public string ReferredBy
        {
            get { return _ReferredBy; }
            set
            {
                if (_ReferredBy != value)
                {
                    _ReferredBy = value;
                    OnPropertyChanged("ReferredBy");
                }
            }
        }



        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                if (Path != value)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        private string _AddedUnitName;
        public string AddedUnitName
        {
            get { return _AddedUnitName; }
            set
            {
                if (_AddedUnitName != value)
                {
                    _AddedUnitName = value;
                    OnPropertyChanged("AddedUnitName");
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

    public class clsPatientFollowUpVO : IValueObject, INotifyPropertyChanged
    {
        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                if (value != _ID)
                {
                    _ID = value;
                    OnPropertyChanged("ID");
                }

            }
        }
        private string _FollowUp;
        public string FollowUp
        {
            get { return _FollowUp; }
            set
            {
                if (value != _FollowUp)
                {
                    _FollowUp = value;
                    OnPropertyChanged("FollowUp");
                }

            }
        }


        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
                }

            }
        }

        private string _Clinic;
        public string Clinic
        {
            get { return _Clinic; }
            set
            {
                if (value != _Clinic)
                {
                    _Clinic = value;
                    OnPropertyChanged("Clinic");
                }

            }
        }

        private long _DepartmentID;
        public long DepartmentID
        {
            get { return _DepartmentID; }
            set
            {
                if (value != _DepartmentID)
                {
                    _DepartmentID = value;
                    OnPropertyChanged("DepartmentID");
                }

            }
        }
        private long _ServiceId;
        public long ServiceId
        {
            get { return _ServiceId; }
            set
            {
                if (_ServiceId != value)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceId");
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
        private string _Doctor;
        public string Doctor
        {
            get { return _Doctor; }
            set
            {
                if (value != _Doctor)
                {
                    _Doctor = value;
                    OnPropertyChanged("Doctor");
                }

            }
        }

        private string _Department;
        public string Department
        {
            get { return _Department; }
            set
            {
                if (value != _Department)
                {
                    _Department = value;
                    OnPropertyChanged("Department");
                }

            }
        }

        private DateTime? _PreviousFollowUpDate;
        public DateTime? PreviousFollowUpDate
        {
            get { return _PreviousFollowUpDate; }
            set
            {
                if (value != _PreviousFollowUpDate)
                {
                    _PreviousFollowUpDate = value;
                    OnPropertyChanged("PreviousFollowUpDate");
                }

            }
        }
        public string Flag { get; set; }


        private long _FollowUpStatus;
        public long FollowUpStatus
        {
            get { return _FollowUpStatus; }
            set
            {
                if (value != _FollowUpStatus)
                {
                    _FollowUpStatus = value;
                    OnPropertyChanged("FollowUpStatus");
                }

            }
        }
        private DateTime? _FollowUpDate;
        public DateTime? FollowUpDate
        {
            get { return _FollowUpDate; }
            set
            {
                if (value != _FollowUpDate)
                {
                    _FollowUpDate = value;
                    OnPropertyChanged("FollowUpDate");
                }

            }
        }

        private string _FollowUpDisplayDate;
        public string FollowUpDisplayDate
        {
            get { return _FollowUpDisplayDate; }
            set
            {
                if (value != _FollowUpDisplayDate)
                {
                    _FollowUpDisplayDate = value;
                    OnPropertyChanged("FollowUpDisplayDate");
                }

            }
        }
        private DateTime? _FollowUpTime;
        public DateTime? FollowUpTime
        {
            get { return _FollowUpTime; }
            set
            {
                if (value != _FollowUpTime)
                {
                    _FollowUpTime = value;
                    OnPropertyChanged("FollowUpTime");
                }

            }
        }
        private DateTime? _SinceDate;
        public DateTime? SinceDate
        {
            get { return _SinceDate; }
            set
            {
                if (value != _SinceDate)
                {
                    _SinceDate = value;
                    OnPropertyChanged("SinceDate");
                }

            }
        }

        private Int32 _SinceDay;
        public Int32 SinceDay
        {
            get { return _SinceDay; }
            set
            {
                if (value != _SinceDay)
                {
                    _SinceDay = value;
                    OnPropertyChanged("SinceDay");
                }

            }
        }


        private string _FollowUpFor;
        public string FollowUpFor
        {
            get { return _FollowUpFor; }
            set
            {
                if (value != _FollowUpFor)
                {
                    _FollowUpFor = value;
                    OnPropertyChanged("FollowUpFor");
                }

            }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (value != _Remark)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }

            }
        }
        private string _FollowUpRemarks;
        public string FollowUpRemarks
        {
            get { return _FollowUpRemarks; }
            set
            {
                if (value != _FollowUpRemarks)
                {
                    _FollowUpRemarks = value;
                    OnPropertyChanged("FollowUpRemarks");
                }

            }
        }

        private Int32 _FollowUpNo;
        public Int32 FollowUpNo
        {
            get { return _FollowUpNo; }
            set
            {
                if (value != _FollowUpNo)
                {
                    _FollowUpNo = value;
                    OnPropertyChanged("FollowUpNo");
                }

            }
        }
        private bool _IsClose;
        public bool IsClose
        {
            get { return _IsClose; }
            set
            {
                if (value != _IsClose)
                {
                    _IsClose = value;
                    OnPropertyChanged("IsClose");
                }
            }
        }
        private bool _IsPostPone;
        public bool IsPostPone
        {
            get { return _IsPostPone; }
            set
            {
                if (value != _IsPostPone)
                {
                    _IsPostPone = value;
                    OnPropertyChanged("IsPostPone");
                }
            }
        }

        private bool _IsPostPoneUsed;
        public bool IsPostPoneUsed
        {
            get { return _IsPostPoneUsed; }
            set
            {
                if (value != _IsPostPoneUsed)
                {
                    _IsPostPoneUsed = value;
                    OnPropertyChanged("IsPostPoneUsed");
                }
            }
        }

        private DateTime? _FollowUpPostPoneDate;
        public DateTime? FollowUpPostPoneDate
        {
            get { return _FollowUpPostPoneDate; }
            set
            {
                if (value != _FollowUpPostPoneDate)
                {
                    _FollowUpPostPoneDate = value;
                    OnPropertyChanged("FollowUpPostPoneDate");
                }

            }
        }

        private DateTime? _RegistrationDate;
        public DateTime? RegistrationDate
        {
            get { return _RegistrationDate; }
            set
            {
                if (value != _RegistrationDate)
                {
                    _RegistrationDate = value;
                    OnPropertyChanged("RegistrationDate");
                }

            }
        }
        private string _RegisteredClinic;
        public string RegisteredClinic
        {
            get { return _RegisteredClinic; }
            set
            {
                if (value != _RegisteredClinic)
                {
                    _RegisteredClinic = value;
                    OnPropertyChanged("RegisteredClinic");
                }

            }
        }

        private string _MaritalStatus;
        public string MaritalStatus
        {
            get { return _MaritalStatus; }
            set
            {
                if (value != _MaritalStatus)
                {
                    _MaritalStatus = value;
                    OnPropertyChanged("MaritalStatus");
                }

            }
        }

        private long _FollowUpPostPoneFromId;
        public long FollowUpPostPoneFromId
        {
            get { return _FollowUpPostPoneFromId; }
            set
            {
                if (value != _FollowUpPostPoneFromId)
                {
                    _FollowUpPostPoneFromId = value;
                    OnPropertyChanged("FollowUpPostPoneFromId");
                }

            }
        }

        private string _FollowUpFrom;
        public string FollowUpFrom
        {
            get { return _FollowUpFrom; }
            set
            {
                if (value != _FollowUpFrom)
                {
                    _FollowUpFrom = value;
                    OnPropertyChanged("FollowUpFrom");
                }

            }
        }


        private DateTime? _ActualServiceConsumedDate;
        public DateTime? ActualServiceConsumedDate
        {
            get { return _ActualServiceConsumedDate; }
            set
            {
                if (value != _ActualServiceConsumedDate)
                {
                    _ActualServiceConsumedDate = value;
                    OnPropertyChanged("ActualServiceConsumedDate");
                }

            }
        }
        private DateTime? _FollowUpCompletedDate;
        public DateTime? FollowUpCompletedDate
        {
            get { return _FollowUpCompletedDate; }
            set
            {
                if (value != _FollowUpCompletedDate)
                {
                    _FollowUpCompletedDate = value;
                    OnPropertyChanged("FollowUpCompletedDate");
                }

            }
        }

        private DateTime? _FollowUpScheduleDate;
        public DateTime? FollowUpScheduleDate
        {
            get { return _FollowUpScheduleDate; }
            set
            {
                if (value != _FollowUpScheduleDate)
                {
                    _FollowUpScheduleDate = value;
                    OnPropertyChanged("FollowUpScheduleDate");
                }

            }
        }

        private bool _IsSchedule;
        public bool IsSchedule
        {
            get { return _IsSchedule; }
            set
            {
                if (value != _IsSchedule)
                {
                    _IsSchedule = value;
                    OnPropertyChanged("IsSchedule");
                }

            }
        }

        private DateTime? _FollowUpPostponeTime;
        public DateTime? FollowUpPostponeTime
        {
            get { return _FollowUpPostponeTime; }
            set
            {
                if (value != _FollowUpPostponeTime)
                {
                    _FollowUpPostponeTime = value;
                    OnPropertyChanged("FollowUpPostponeTime");
                }

            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (_TariffID != value)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
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

        private long _PackageServiceID;
        public long PackageServiceID
        {
            get { return _PackageServiceID; }
            set
            {
                if (_PackageServiceID != value)
                {
                    _PackageServiceID = value;
                    OnPropertyChanged("PackageServiceID");
                }
            }
        }

        private bool _Status;
        public bool Status
        {
            get { return _Status; }
            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
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

    //***//
    public class clsBankDetailsInfoVO : INotifyPropertyChanged, IValueObject
    {
        public clsBankDetailsInfoVO()
        {

        }

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        private long _ID;
        public long ID
        {
            get
            {
                return _ID;
            }

            set
            {
                if (value != _ID)
                {
                    _ID = value;

                }
            }
        }

        private long _BankId;
        public long BankId
        {
            get { return _BankId; }
            set
            {
                if (value != _BankId)
                {
                    _BankId = value;
                    OnPropertyChanged("BankId");
                }
            }
        }            

        private long _BranchId;
        public long BranchId
        {
            get { return _BranchId; }
            set
            {
                if (value != _BranchId)
                {
                    _BranchId = value;
                    OnPropertyChanged("BranchId");
                }
            }
        }       

        private string _AccountNumber;
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set
            {
                if (value != _AccountNumber)
                {
                    _AccountNumber = value;
                    OnPropertyChanged("AccountNumber");
                }
            }
        }
    

        private bool _AccountTypeId;
        public bool AccountTypeId
        {
            get { return _AccountTypeId; }
            set
            {
                if (value != _AccountTypeId)
                {
                    _AccountTypeId = value;
                    OnPropertyChanged("AccountTypeId");
                }
            }
        }


        private string _AccountHolderName;
        public string AccountHolderName
        {
            get { return _AccountHolderName; }
            set
            {
                if (value != _AccountHolderName)
                {
                    _AccountHolderName = value;
                    OnPropertyChanged("AccountHolderName");
                }
            }
        }

        private string _IFSCCode;
        public string IFSCCode
        {
            get { return _IFSCCode; }
            set
            {
                if (value != _IFSCCode)
                {
                    _IFSCCode = value;
                    OnPropertyChanged("IFSCCode");
                }
            }
        }      

    }

}
