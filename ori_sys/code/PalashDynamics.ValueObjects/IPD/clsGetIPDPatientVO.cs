using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Patient;


namespace PalashDynamics.ValueObjects.IPD
{
    public class clsGetIPDPatientVO : IValueObject, INotifyPropertyChanged
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


        public bool SelectPatient { get; set; }

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



        private double _Weight;
        public double Weight
        {
            get { return _Weight; }
            set
            {
                if (_Weight != value)
                {
                    _Weight = value;
                    OnPropertyChanged("Weight");
                }
            }
        }


        private double _BMI;
        public double BMI
        {
            get { return _BMI; }
            set
            {
                if (_BMI != value)
                {
                    _BMI = value;
                    OnPropertyChanged("BMI");
                }
            }
        }


        private DateTime _DateofBirth;
        public DateTime DateofBirth
        {
            get { return _DateofBirth; }
            set
            {
                if (_DateofBirth != value)
                {
                    _DateofBirth = value;
                    OnPropertyChanged("DateofBirth");
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

        public bool IsTransferred { get; set; }

        private bool _IsIPDPatient = false;
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

        private long _MobileCountryCode;
        public long MobileCountryCode
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

        private long? _OccupationId;
        public long? OccupationId
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
        /* Added By SUDHIR PATIL on 01/03/2014 */
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

        private long _PatientSourceID;
        public long PatientSourceID
        {
            get { return _PatientSourceID; }
            set
            {
                if (_PatientSourceID != value)
                {
                    _PatientSourceID = value;
                    OnPropertyChanged("PatientSourceID");
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

        //Added By SUDHIR on 28/Feb/2014

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


        //By Anjali...
        private List<clsKinInformationVO> _KinInformationList = new List<clsKinInformationVO>();
        public List<clsKinInformationVO> KinInformationList
        {
            get { return _KinInformationList; }
            set { _KinInformationList = value; }
        }
    }
}
