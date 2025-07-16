using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PalashDynamics.ValueObjects.Administration.StaffMaster
{
    public class clsStaffMasterVO : INotifyPropertyChanged, IValueObject
    {
        //rohinee

        public byte[] Photo { get; set; }

        private string strEmployeeNumber;
        public string EmployeeNumber
        {
            get { return strEmployeeNumber; }
            set
            {
                if (value != strEmployeeNumber)
                {
                    strEmployeeNumber = value;
                    OnPropertyChanged("EmployeeNumber");
                }
            }
        }
        private DateTime? _DateofJoining;
        public DateTime? DateofJoining
        {
            get { return _DateofJoining; }
            set
            {
                if (value != _DateofJoining)
                {
                    _DateofJoining = value;
                    OnPropertyChanged("DateofJoining");
                }
            }
        }
        private string _AccessCardNumber;
        public string AccessCardNumber
        {
            get { return _AccessCardNumber; }
            set
            {
                if (value != _AccessCardNumber)
                {
                    _AccessCardNumber = value;
                    OnPropertyChanged("AccessCardNumber");
                }
            }
        }
        private string strPANNumber;
        public string PANNumber
        {
            get { return strPANNumber; }
            set
            {
                if (value != strPANNumber)
                {
                    strPANNumber = value;
                    OnPropertyChanged("PANNumber");
                }
            }
        }
        private string strPFNumber;
        public string PFNumber
        {
            get { return strPFNumber; }
            set
            {
                if (value != strPFNumber)
                {
                    strPFNumber = value;
                    OnPropertyChanged("PFNumber");
                }
            }
        }
        private string strEducation;
        public string Education
        {
            get { return strEducation; }
            set
            {
                if (value != strEducation)
                {
                    strEducation = value;
                    OnPropertyChanged("Education");
                }
            }
        }


        private string strExperience = "";
        public string Experience
        {
            get { return strExperience; }
            set
            {
                if (value != strExperience)
                {
                    strExperience = value;
                    OnPropertyChanged("Experience");
                }
            }
        }
        public string DepartmentName { get; set; }
        
        private long? _DepartmentID;
        public long? DepartmentID
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

                }
            }
        }
        private long _ClinicId;
        public long ClinicId
        {
            get
            {
                return _ClinicId;
            }

            set
            {
                if (value != _ClinicId)
                {
                    _ClinicId = value;

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

        private long _BankBranchId;
        public long BankBranchId
        {
            get { return _BankBranchId; }
            set
            {
                if (value != _BankBranchId)
                {
                    _BankBranchId = value;
                    OnPropertyChanged("BankBranchId");
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
        private long _StaffId;
        public long StaffId
        {
            get { return _StaffId; }
            set
            {
                if (value != _StaffId)
                {
                    _StaffId = value;
                    OnPropertyChanged("StaffId");
                }
            }
        }
        private List<clsStaffBankInfoVO> _StaffBankInfo;
        public List<clsStaffBankInfoVO> StaffBankInfo
        {
            get
            {
                if (_StaffBankInfo == null)
                    _StaffBankInfo = new List<clsStaffBankInfoVO>();

                return _StaffBankInfo;
            }

            set
            {

                _StaffBankInfo = value;

            }
        }

        private clsStaffBankInfoVO _StaffBankInformation;
        public clsStaffBankInfoVO StaffBankInformation
        {
            get
            {
                if (_StaffBankInformation == null)
                    _StaffBankInformation = new clsStaffBankInfoVO();

                return _StaffBankInformation;
            }

            set
            {

                _StaffBankInformation = value;

            }
        }

        private List<clsStaffAddressInfoVO> _StaffAddressInfo;
        public List<clsStaffAddressInfoVO> StaffAddressInfo
        {
            get
            {
                if (_StaffAddressInfo == null)
                    _StaffAddressInfo = new List<clsStaffAddressInfoVO>();

                return _StaffAddressInfo;
            }

            set
            {

                _StaffAddressInfo = value;

            }
        }

        private clsStaffAddressInfoVO _StaffAddressInformation;
        public clsStaffAddressInfoVO StaffAddressInformation
        {
            get
            {
                if (_StaffAddressInformation == null)
                    _StaffAddressInformation = new clsStaffAddressInfoVO();

                return _StaffAddressInformation;
            }

            set
            {

                _StaffAddressInformation = value;

            }
        }
        //

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

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                if (value != _Value)
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }
        private string strStaffName = "";
        public string StaffName
        {
            get { return strStaffName = strFirstName + " " + strMiddleName + " " + strLastName; }
            set
            {
                if (value != strStaffName)
                {
                    strStaffName = value;
                    OnPropertyChanged("StaffName");
                }
            }
        }


        private string strFirstName;
        public string FirstName
        {
            get { return strFirstName; }
            set
            {
                if (value != strFirstName)
                {
                    strFirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string strMiddleName;
        public string MiddleName
        {
            get { return strMiddleName; }
            set
            {
                if (value != strMiddleName)
                {
                    strMiddleName = value;
                    OnPropertyChanged("MiddleName");
                }
            }
        }

        private string strLastName;
        public string LastName
        {
            get { return strLastName; }
            set
            {
                if (value != strLastName)
                {
                    strLastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private DateTime? _DOB;
        public DateTime? DOB
        {
            get { return _DOB; }
            set
            {
                if (value != _DOB)
                {
                    _DOB = value;
                    OnPropertyChanged("DOB");
                }
            }

        }

        private string _StaffNo;
        [Required]
        public string StaffNo
        {
            get { return _StaffNo; }
            set
            {
                if (value != _StaffNo)
                {
                    _StaffNo = value;
                    OnPropertyChanged("StaffNo");
                }
            }
        }

        private string strUserName = string.Empty;
        public string UserName
        {
            get { return strUserName = strFirstName + " " + strLastName; }
            set
            {
                if (value != strUserName)
                {
                    strUserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private long _UserID;
        public long UserID
        {
            get { return _UserID; }
            set
            {
                if (value != _UserID)
                {
                    _UserID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }

        private long _UserUnitID;
        public long UserUnitID
        {
            get { return _UserUnitID; }
            set
            {
                if (value != _UserUnitID)
                {
                    _UserUnitID = value;
                    OnPropertyChanged("UserUnitID");
                }
            }
        }

        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (value != _RowID)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

       
        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long lngGenderId;
        public long GenderID
        {
            get { return lngGenderId; }
            set
            {
                if (value != lngGenderId)
                {
                    lngGenderId = value;
                    OnPropertyChanged("GenderID");
                }
            }
        }

        private string strUnitName;
        public string UnitName
        {
            get { return strUnitName; }
            set
            {
                if (value != strUnitName)
                {
                    strUnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private long lngDesignationId;
        public long DesignationID
        {
            get { return lngDesignationId; }
            set
            {
                if (value != lngDesignationId)
                {
                    lngDesignationId = value;
                    OnPropertyChanged("DesignationID");
                }
            }
        }

        private string _Designation;
        public string Designation
        {
            get { return _Designation; }
            set
            {
                if (value != _Designation)
                {
                    _Designation = value;
                    OnPropertyChanged("Designation");
                }
            }
        }

        private string _EmailId;
        public string EmailId
        {
            get { return _EmailId; }
            set
            {
                if (value != _EmailId)
                {
                    _EmailId = value;
                    OnPropertyChanged("EmailId");
                }
            }
        }

        private string _ClinicName;
        public string ClinicName
        {
            get { return _ClinicName; }
            set
            {
                if (value != _ClinicName)
                {
                    _ClinicName = value;
                    OnPropertyChanged("ClinicName");
                }
            }
        }

        private long _BloodGroupId;
        public long BloodGroupId
        {
            get { return _BloodGroupId; }
            set
            {
                if (value != _BloodGroupId)
                {
                    _BloodGroupId = value;
                    OnPropertyChanged("BloodGroupId");
                }
            }
        }

        private long _AgeDays;
        public long AgeDays
        {
            get { return _AgeDays; }
            set
            {
                if (value != _AgeDays)
                {
                    _AgeDays = value;
                    OnPropertyChanged("AgeDays");
                }
            }
        }

        private long _AgeMonth;
        public long AgeMonth
        {
            get { return _AgeMonth; }
            set
            {
                if (value != _AgeMonth)
                {
                    _AgeMonth = value;
                    OnPropertyChanged("AgeMonth");
                }
            }
        }

        private long _AgeYears;
        public long AgeYears
        {
            get { return _AgeYears; }
            set
            {
                if (value != _AgeYears)
                {
                    _AgeYears = value;
                    OnPropertyChanged("AgeYears");
                }
            }
        }

        //Added by AJ  date 10/11/2016      

        private bool _IsDischargeApprove;
        public bool IsDischargeApprove
        {
            get { return _IsDischargeApprove; }
            set { _IsDischargeApprove = value; }
        }

        private bool _IsMarketingExecutives;
        public bool IsMarketingExecutives
        {
            get { return _IsMarketingExecutives; }
            set { _IsMarketingExecutives = value; }
        }
        
        //***//-------------------------

        private long _MaritalStatusId;
        public long MaritalStatusId
        {
            get { return _MaritalStatusId; }
            set
            {
                if (value != _MaritalStatusId)
                {
                    _MaritalStatusId = value;
                    OnPropertyChanged("MaritalStatusId");
                }
            }
        }

        private long _ReligionId;
        public long ReligionId
        {
            get { return _ReligionId; }
            set
            {
                if (value != _ReligionId)
                {
                    _ReligionId = value;
                    OnPropertyChanged("ReligionId");
                }
            }
        }

        private long _MobileNo;
        public long MobileNo
        {
            get { return _MobileNo; }
            set
            {
                if (value != _MobileNo)
                {
                    _MobileNo = value;
                    OnPropertyChanged("MobileNo");
                }
            }
        }

        private long _MobileCountryCode;
        public long MobileCountryCode
        {
            get { return _MobileCountryCode; }
            set
            {
                if (value != _MobileCountryCode)
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
                if (value != _ResiNoCountryCode)
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
                if (value != _ResiSTDCode)
                {
                    _ResiSTDCode = value;
                    OnPropertyChanged("ResiSTDCode");
                }
            }
        }

        private long _ResidenceNo;
        public long ResidenceNo
        {
            get { return _ResidenceNo; }
            set
            {
                if (value != _ResidenceNo)
                {
                    _ResidenceNo = value;
                    OnPropertyChanged("ResidenceNo");
                }
            }
        }

        private long _PrefixID;
        public long PrefixID
        {
            get { return _PrefixID; }
            set
            {
                if (value != _PrefixID)
                {
                    _PrefixID = value;
                    OnPropertyChanged("PrefixID");
                }
            }
        }

        
        #region For IPD Module

        //private long lngDepartmentID;
        //public long DepartmentID
        //{
        //    get { return lngDepartmentID; }
        //    set
        //    {
        //        if (value != lngDepartmentID)
        //        {
        //            lngDepartmentID = value;
        //            OnPropertyChanged("DepartmentID");
        //        }
        //    }
        //}

        private bool _IsApplicableAdvise;
        public bool IsApplicableAdvise
        {
            get { return _IsApplicableAdvise; }
            set
            {
                if (value != _IsApplicableAdvise)
                {
                    _IsApplicableAdvise = value;
                    OnPropertyChanged("IsApplicableAdvise");
                }
            }
        }

        #endregion

        #region CommonField

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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (value != _CreatedUnitID)
                {
                    _CreatedUnitID = value;
                    OnPropertyChanged("CreatedUnitID");
                }
            }
        }

        private long _UpdatedUnitID;
        public long UpdatedUnitID
        {
            get { return _UpdatedUnitID; }
            set
            {
                if (value != _UpdatedUnitID)
                {
                    _UpdatedUnitID = value;
                    OnPropertyChanged("UpdatedUnitID");
                }
            }
        }

        private long _AddedBy;
        public long AddedBy
        {
            get { return _AddedBy; }
            set
            {
                if (value != _AddedBy)
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
                if (value != _AddedOn)
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
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }


        private string _UpdatedBy;
        public string UpdatedBy
        {
            get { return _UpdatedBy; }
            set
            {
                if (value != _UpdatedBy)
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
                if (value != _UpdatedOn)
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
                if (value != _UpdatedWindowsLoginName)
                {
                    _UpdatedWindowsLoginName = value;
                    OnPropertyChanged("UpdatedWindowsLoginName");
                }
            }
        }




        #endregion

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
    }
    
    public class clsStaffBankInfoVO : INotifyPropertyChanged, IValueObject
    {
        public clsStaffBankInfoVO()
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

        private string _BankName;
        public string BankName
        {
            get { return _BankName; }
            set
            {
                if (value != _BankName)
                {
                    _BankName = value;
                    OnPropertyChanged("BankName");
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

        private string _BranchName;
        public string BranchName
        {
            get { return _BranchName; }
            set
            {
                if (value != _BranchName)
                {
                    _BranchName = value;
                    OnPropertyChanged("BranchName");
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

        private bool _AccountType;
        public bool AccountType
        {
            get { return _AccountType; }
            set
            {
                if (value != _AccountType)
                {
                    _AccountType = value;
                    OnPropertyChanged("AccountType");
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

        private string _BranchAddress;
        public string BranchAddress
        {
            get { return _BranchAddress; }
            set
            {
                if (value != _BranchAddress)
                {
                    _BranchAddress = value;
                    OnPropertyChanged("BranchAddress");
                }
            }
        }


        private string _MICRNumber;
        public string MICRNumber
        {
            get { return _MICRNumber; }
            set
            {
                if (value != _MICRNumber)
                {
                    _MICRNumber = value;
                    OnPropertyChanged("MICRNumber");
                }
            }
        }

        private long _StaffId;
        public long StaffId
        {
            get { return _StaffId; }
            set
            {
                if (value != _StaffId)
                {
                    _StaffId = value;
                    OnPropertyChanged("StaffId");
                }
            }
        }

        
        private string _StaffName;
        public string StaffName
        {
            get { return _StaffName; }
            set
            {
                if (value != _StaffName)
                {
                    _StaffName = value;
                    OnPropertyChanged("StaffName");
                }
            }
        }

        private string _AccountTypeName;
        public string AccountTypeName
        {
            get { return _AccountTypeName; }
            set
            {
                if (value != _AccountTypeName)
                {
                    _AccountTypeName = value;
                    OnPropertyChanged("AccountTypeName");
                }
            }
        }



    }

    public class clsStaffAddressInfoVO : INotifyPropertyChanged, IValueObject
    {
        public clsStaffAddressInfoVO()
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


        private long _StaffId;
        public long StaffId
        {
            get { return _StaffId; }
            set
            {
                if (value != _StaffId)
                {
                    _StaffId = value;
                    OnPropertyChanged("StaffId");
                }
            }
        }

        private string _AddressType;
        public string AddressType
        {
            get { return _AddressType; }
            set
            {
                if (value != _AddressType)
                {
                    _AddressType = value;
                    OnPropertyChanged("AddressType");
                }
            }
        }

        private long _AddressTypeID;
        public long AddressTypeID
        {
            get { return _AddressTypeID; }
            set
            {
                if (value != _AddressTypeID)
                {
                    _AddressTypeID = value;
                    OnPropertyChanged("AddressTypeID");
                }
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
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

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                if (value != _Address)
                {
                    _Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        private long _MOVID;
        public long MOVID
        {
            get
            {
                return _MOVID;
            }

            set
            {
                if (value != _MOVID)
                {
                    _MOVID = value;

                }
            }
        }



        private string _Contact1;
        public string Contact1
        {
            get { return _Contact1; }
            set
            {
                if (value != _Contact1)
                {
                    _Contact1 = value;
                    OnPropertyChanged("Contact1");
                }
            }
        }

        private string _Contact2;
        public string Contact2
        {
            get { return _Contact2; }
            set
            {
                if (value != _Contact2)
                {
                    _Contact2 = value;
                    OnPropertyChanged("Contact2");
                }
            }
        }

        private Boolean _IsFromMarketing = false;
        public Boolean IsFromMarketing
        {
            get
            {
                return _IsFromMarketing;
            }
            set
            {
                _IsFromMarketing = value;
                OnPropertyChanged("IsFromMarketing");
            }
        }





    }

}
