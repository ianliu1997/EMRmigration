using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.User;
using PalashDynamics.ValueObjects.Administration.UserRights;
//using PalashDynamics.ValueObjects.Administration.UserMaster;
//using PalashDynamics.ValueObjects.Masters.Administration.Billing.CashCounter;
//using System.ComponentModel.DataAnnotations;
//using PalashDynamics.ValueObjects.Administration.Menu;

namespace PalashDynamics.ValueObjects
{

    public class clsSecretQtnVO : IValueObject
    {
        private long _Id;
        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string Value { get; set; }


        public string ToXml()
        {
            return this.ToString();
        }
    }

    public class clsUserVO : IValueObject, INotifyPropertyChanged
    {
        public bool IsAuditTrail { get; set; }
        public bool IsEmailMandatory { get; set; }

        private long _Id;

        public long ID
        {
            get { return _Id; }
            set { _Id = value; }
        }

        //rohinee
        public bool IsCounterLogin
        {
            get;
            set;
        }
        //
        public string Password { get; set; }
        public string PasswordDe { get; set; }

        private string _LoginName;
        public string LoginName
        {
            get { return _LoginName; }
            set
            {
                if (value != _LoginName)
                    _LoginName = value;
            }
        }

        private string _UserNameNew;
        public string UserNameNew
        {
            get { return _UserNameNew; }
            set
            {
                if (value != _UserNameNew)
                    _UserNameNew = value;
            }
        }
        //Password Configuration
        //  public long ID { get; set; }
        //public Int16 MinPasswordLength { get; set; }
        //public Int16 MaxPasswordLength { get; set; }
        //public bool AtLeastOneDigit { get; set; }
        //public bool AtLeastOneLowerCaseChar { get; set; }
        //public bool AtLeastOneUpperCaseChar { get; set; }
        //public bool AtLeastOneSpecialChar { get; set; }

        //public Int16 NoOfPasswordsToRemember { get; set; }
        //public Int16 MinPasswordAge { get; set; }
        //public Int16 MaxPasswordAge { get; set; }
        //public Int16 AccountLockThreshold { get; set; }

        //public float AccountLockDuration { get; set; }

        //public clsPassConfigurationVO PassConfig { get; set; }
        private clsPassConfigurationVO _PassConfig = new clsPassConfigurationVO();

        public clsPassConfigurationVO PassConfig
        {
            get { return _PassConfig; }
            set { _PassConfig = value; }
        }
        //Password Configuration end.

        public string UserTypeName { get; set; }

        public string RoleName { get; set; }

        // [Required(ErrorMessage = "LoginName is Required")]

        public string EmailId { get; set; }
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

        public long UserType { get; set; }

        private clsUserLoginInfoVO _UserLoginInfoVO = new clsUserLoginInfoVO();

        public clsUserLoginInfoVO UserLoginInfo
        {
            get { return _UserLoginInfoVO; }
            set { _UserLoginInfoVO = value; }
        }

        //by rohini      
        private clsUserRightsVO _UserAditionalRights = new clsUserRightsVO();

        public clsUserRightsVO UserAditionalRights
        {
            get { return _UserAditionalRights; }
            set { _UserAditionalRights = value; }
        }


        private clsUserGeneralDetailVO _UserGeneralDetailVO = new clsUserGeneralDetailVO();

        public clsUserGeneralDetailVO UserGeneralDetailVO
        {
            get { return _UserGeneralDetailVO; }
            set { _UserGeneralDetailVO = value; }
        }


        private List<clsUserCategoryLinkVO> _UserCategoryLinkList;
        public List<clsUserCategoryLinkVO> UserCategoryLinkList
        {
            get
            {
                if (_UserCategoryLinkList == null)
                    _UserCategoryLinkList = new List<clsUserCategoryLinkVO>();
                return _UserCategoryLinkList;
            }
            set
            { _UserCategoryLinkList = value; }
        }



       
        //private clsUserOtherDetailVO _UserOtherDetailVO = new clsUserOtherDetailVO();

        //public clsUserOtherDetailVO UserOtherDetailVO
        //{
        //    get { return _UserOtherDetailVO; }
        //    set { _UserOtherDetailVO = value; }
        //}

        #region Added by Prashant Channe on 27/11/2019, For ReportsFolder Configuration
        //
        public string ReportsFolder { get; set; }

        #endregion

        # region For Item Selection Control

        public bool IsCSControlEnable { get; set; }     // use to Enable Item Selection control on Counter Sale Screen

        #endregion

        public bool ValidationsFlag { get; set; } //Added by NileshD for patient registration screen on 19April2019


        #region  For User rightswise fill Unit List 02032017

        private List<clsUserUnitDetailsVO> _UserUnitList = new List<clsUserUnitDetailsVO>();
        public List<clsUserUnitDetailsVO> UserUnitList
        {
            get { return _UserUnitList; }
            set { _UserUnitList = value; }
        }

        #endregion

        #region Added By Bhushanp For Package ID
        private long _PackageID;

        public long PackageID
        {
            get { return _PackageID; }
            set { _PackageID = value; }
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

    public class clsUserLoginInfoVO
    {

        public long UnitId { get; set; }
        public long RetunUnitId { get; set; }

        private string _LoginUnitName;
        public string UnitName
        {
            get { return _LoginUnitName; }
            set
            {
                if (value != _LoginUnitName)
                    _LoginUnitName = value;
            }
        }

        private string _LoginName;
        public string Name
        {
            get { return _LoginName; }
            set
            {
                if (value != _LoginName)
                    _LoginName = value;
            }
        }

        private string _LoginMachineName;
        public string MachineName
        {
            get { return _LoginMachineName; }
            set
            {
                if (value != _LoginMachineName)
                    _LoginMachineName = value;
            }
        }

        private string _LoginUserName;
        public string UserName
        {
            get { return _LoginUserName; }
            set
            {
                if (value != _LoginUserName)
                    _LoginUserName = value;
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (value != _Password)
                    _Password = value;
            }
        }

        private string _WindowsUserName;
        public string WindowsUserName
        {
            get { return _WindowsUserName; }
            set
            {
                if (value != _WindowsUserName)
                    _WindowsUserName = value;
            }
        }

        // Added By Changdeo Sase

        public long CashCounterID { get; set; }

        private string _CashCounterName;
        public string CashCounterName
        {
            get { return _CashCounterName; }
            set
            {
                if (value != _CashCounterName)
                    _CashCounterName = value;
            }
        }
    }

    public class clsUserGeneralDetailVO : IValueObject, INotifyPropertyChanged
    {

        //private long  lngCode;

        //public long Code
        //{
        //    get { return lngCode; }
        //    set { lngCode = value; }
        //}

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (value != _UserName)
                {
                    _UserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }


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

        private string _DoctorName;
        public string DoctorName
        {
            get
            {
                return _DoctorName;
            }

            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
                }
            }
        }

        private string _DoctorSpecialization;
        public string DoctorSpecialization
        {
            get
            {
                return _DoctorSpecialization;
            }

            set
            {
                if (value != _DoctorSpecialization)
                {
                    _DoctorSpecialization = value;
                    OnPropertyChanged("DoctorSpecialization");
                }
            }
        }

        private string _DoctorSpecCode;
        public string DoctorSpecCode
        {
            get
            {
                return _DoctorSpecCode;
            }

            set
            {
                if (value != _DoctorSpecCode)
                {
                    _DoctorSpecCode = value;
                    OnPropertyChanged("DoctorSpecCode");
                }
            }
        }
        //added by Bhushanp
        private long _DepartmentID;

        public long DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }


        //private string _UnitName;
        //public string UnitName
        //{
        //    get { return _UnitName; }
        //    set
        //    {
        //        if (value != _UnitName)
        //        {
        //            _UnitName = value;
        //            OnPropertyChanged("UnitName");
        //        }
        //    }
        //}

        //private string _UnitDescription;
        //public string UnitDescription
        //{
        //    get { return _UnitDescription; }
        //    set
        //    {
        //        if (value != _UnitDescription)
        //        {
        //            _UnitDescription = value;
        //            OnPropertyChanged("UnitDescription");
        //        }
        //    }
        //}


        private bool _FirstPasswordChanged;
        public bool FirstPasswordChanged
        {
            get
            {
                return _FirstPasswordChanged;
            }

            set
            {
                if (value != _FirstPasswordChanged)
                {
                    _FirstPasswordChanged = value;
                    OnPropertyChanged("FirstPasswordChanged");
                }
            }
        }


        private bool _EnablePasswordExpiration;
        public bool EnablePasswordExpiration
        {
            get
            {
                return _EnablePasswordExpiration;
            }

            set
            {
                if (value != _EnablePasswordExpiration)
                {
                    _EnablePasswordExpiration = value;
                    OnPropertyChanged("EnablePasswordExpiration");
                }
            }
        }

        private int _PasswordExpirationInterval;
        public int PasswordExpirationInterval
        {
            get
            {
                return _PasswordExpirationInterval;
            }

            set
            {
                if (value != _PasswordExpirationInterval)
                {
                    _PasswordExpirationInterval = value;
                    OnPropertyChanged("PasswordExpirationInterval");
                }
            }
        }

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

        private bool _IsEmployee;
        public bool IsEmployee
        {
            get
            {
                return _IsEmployee;
            }

            set
            {
                if (value != _IsEmployee)
                {
                    _IsEmployee = value;
                    OnPropertyChanged("IsEmployee");
                }
            }
        }

        private bool _IsDoctor;
        public bool IsDoctor
        {
            get
            {
                return _IsDoctor;
            }

            set
            {
                if (value != _IsDoctor)
                {
                    _IsDoctor = value;
                    OnPropertyChanged("IsDoctor");
                }
            }
        }

        private bool _IsPatient;
        public bool IsPatient
        {
            get
            {
                return _IsPatient;
            }

            set
            {
                if (value != _IsPatient)
                {
                    _IsPatient = value;
                    OnPropertyChanged("IsPatient");
                }
            }
        }

        private long _EmployeeID;
        public long EmployeeID
        {
            get
            {
                return _EmployeeID;
            }

            set
            {
                if (value != _EmployeeID)
                {
                    _EmployeeID = value;
                    OnPropertyChanged("EmployeeID");
                }
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }

            set
            {
                if (value != _PatientID)
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
                if (value != _PatientUnitID)
                {
                    _PatientUnitID = value;
                    OnPropertyChanged("PatientUnitID");
                }
            }
        }

        private long _SecreteQtn;
        public long SecreteQtn
        {
            get
            {
                return _SecreteQtn;
            }
            set
            {
                if (value != _SecreteQtn)
                {
                    _SecreteQtn = value;
                    OnPropertyChanged("SecreteQtn");
                }
            }
        }

        private string _SecretQtnValue;
        public string SecretQtnValue
        {
            get
            {
                return _SecretQtnValue;
            }
            set
            {
                if (value != _SecretQtnValue)
                {
                    _SecretQtnValue = value;
                    OnPropertyChanged("SecreteQtn");
                }
            }
        }


        private string _SecreteAns;
        public string SecreteAns
        {
            get
            {
                return _SecreteAns;
            }

            set
            {
                if (value != _SecreteAns)
                {
                    _SecreteAns = value;
                    OnPropertyChanged("SecreteAns");
                }
            }
        }

        private bool _Locked;
        public bool Locked
        {
            get
            {
                return _Locked;
            }

            set
            {
                if (value != _Locked)
                {
                    _Locked = value;
                    OnPropertyChanged("Locked");
                }
            }
        }

        private List<clsUserUnitDetailsVO> _UnitDetails;
        public List<clsUserUnitDetailsVO> UnitDetails
        {
            get
            {
                if (_UnitDetails == null)
                    _UnitDetails = new List<clsUserUnitDetailsVO>();

                return _UnitDetails;
            }

            set
            {

                _UnitDetails = value;

            }
        }

        private clsUserRoleVO _RoleDetails;
        public clsUserRoleVO RoleDetails
        {
            get
            {
                if (_RoleDetails == null)
                    _RoleDetails = new clsUserRoleVO();

                return _RoleDetails;
            }

            set
            {

                _RoleDetails = value;

            }
        }



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

        public DateTime LokedDateTime { get; set; }

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



        #endregion

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
    }

    public class clsUserUnitDetailsVO : IValueObject
    {
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
                    // OnPropertyChanged("UserTypeID");
                }
            }
        }

        public string UnitName { get; set; }

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }

            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                    // OnPropertyChanged("UserTypeID");
                }
            }
        }


        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    // OnPropertyChanged("UserTypeID");
                }
            }
        }

        private bool _IsDefault;
        public bool IsDefault
        {
            get
            {
                return _IsDefault;
            }

            set
            {
                if (value != _IsDefault)
                {
                    _IsDefault = value;
                    // OnPropertyChanged("UserTypeID");
                }
            }
        }

        public bool StoreEditMode { get; set; }
        private List<clsItemStoreVO> _StoreDetails;
        public List<clsItemStoreVO> StoreDetails
        {
            get
            {
                if (_StoreDetails == null)
                    _StoreDetails = new List<clsItemStoreVO>();
                return _StoreDetails;
            }
            set
            { _StoreDetails = value; }
        }

        private List<clsStoreVO> _UserUnitStore;
        public List<clsStoreVO> UserUnitStore
        {
            get
            {
                if (_UserUnitStore == null)
                    _UserUnitStore = new List<clsStoreVO>();
                return _UserUnitStore;
            }
            set
            { _UserUnitStore = value; }
        }

        public string ToXml()
        {
            return this.ToString();
        }
        public override string ToString()
        {
            return UnitName;
        }

    }

    public class clsUserEMRTemplateDetailsVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            return this.ToString();
        }



        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
                    OnPropertyChanged("ID");
                }
            }
        }

        private long _UserID;
        public long UserID
        {
            get
            {
                return _UserID;
            }

            set
            {
                if (value != _UserID)
                {
                    _UserID = value;
                    OnPropertyChanged("UserID");
                }
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get
            {
                return _TemplateID;
            }

            set
            {
                if (value != _TemplateID)
                {
                    _TemplateID = value;
                    OnPropertyChanged("TemplateID");
                }
            }
        }

        public string TemplateName { get; set; }




        private bool _Status;
        public bool Status
        {
            get
            {
                return _Status;
            }

            set
            {
                if (value != _Status)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

    }


    //public class clsUserOtherDetailVO:IValueObject
    //{
    //    private List<clsCashCounterGeneralDetailVO> _UnitCashCounterDetailList;

    //    public List<clsCashCounterGeneralDetailVO> UnitCashCounterDetailList
    //    {
    //        get
    //        {
    //            if (_UnitCashCounterDetailList == null)
    //                _UnitCashCounterDetailList = new List<clsCashCounterGeneralDetailVO>();
    //            return _UnitCashCounterDetailList;
    //        }
    //        set { _UnitCashCounterDetailList = value; }
    //    }

    //    private List<clsMenuVO> _MenuList;

    //    public List<clsMenuVO> MenuList
    //    {
    //        get
    //        {
    //            if (_MenuList == null)
    //                _MenuList = new List<clsMenuVO>();
    //            return _MenuList;
    //        }
    //        set { _MenuList = value; }
    //    }

    //    private List<clsUnitCashCounterDetailsVO> _UserUnitCashCounterList;

    //    public List<clsUnitCashCounterDetailsVO> UserUnitCashCounterList
    //    {
    //        get
    //        {
    //            if (_UserUnitCashCounterList == null)
    //                _UserUnitCashCounterList = new List<clsUnitCashCounterDetailsVO>();
    //            return _UserUnitCashCounterList;
    //        }
    //        set { _UserUnitCashCounterList = value; }
    //    }
    //    private List<clsUserMenuDetailsVO> _UserMenuDetailsList;

    //    public List<clsUserMenuDetailsVO> UserMenuDetailsList
    //    {
    //        get
    //        {
    //            if (_UserMenuDetailsList == null)
    //                _UserMenuDetailsList = new List<clsUserMenuDetailsVO>();
    //            return _UserMenuDetailsList;
    //        }

    //        set { _UserMenuDetailsList = value; }
    //    }

    //    private List<MasterListItem> _UserRoleDetailsList;

    //    public List<MasterListItem> UserRoleDetailsList
    //    {
    //        get
    //        {
    //            if (_UserRoleDetailsList == null)
    //                _UserRoleDetailsList = new List<MasterListItem>();
    //            return _UserRoleDetailsList;
    //        }

    //        set { _UserRoleDetailsList = value; }
    //    }

    //    private List<MasterListItem> _UserGroupDetailsList;

    //    public List<MasterListItem> UserGroupDetailsList
    //    {
    //        get
    //        {
    //            if (_UserGroupDetailsList == null)
    //                _UserGroupDetailsList = new List<MasterListItem>();
    //            return _UserGroupDetailsList;
    //        }

    //        set { _UserGroupDetailsList = value; }
    //    }


    //    private string strpassword;

    //    //[Required(ErrorMessage = "NewPassword is Required")]
    //    //[StringLength(10, MinimumLength = 6, ErrorMessage = "Password must be in between 6 to 10 Characters")]
    //    public string Password
    //    {
    //        get { return strpassword; }
    //        set
    //        {
    //            //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Password" });
    //            strpassword = value;
    //        }
    //    }

    //    private string confirmpassword;
    //    //[Required(ErrorMessage = "ConfirmPassword is Required")]
    //    //[StringLength(10, MinimumLength = 6, ErrorMessage = "Confirm Password must be in between 6 to 10 Characters")]
    //    public string ConfirmPassword
    //    {
    //        get { return confirmpassword; }
    //        set
    //        {
    //            //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ConfirmPassword" });
    //            //if (Password != null && ConfirmPassword != null && Password != value)
    //            //{
    //            //    throw new ValidationException("Confirm Password and Password doesnot match.");
    //            //}
    //            confirmpassword = value;
    //        }
    //    }

    //    private long lngDefaultCashCounter;

    //     public long DefaultCashCounter
    //     {
    //         get { return lngDefaultCashCounter; }
    //         set { lngDefaultCashCounter = value; }
    //     }
    //     private long lngDefaultStore;

    //     public long DefaultStore
    //     {
    //         get { return lngDefaultStore; }
    //         set { lngDefaultStore = value; }
    //     }
    //     private long lngDefaultUnit;

    //     public long DefaultUnit
    //     {
    //         get { return lngDefaultUnit; }
    //         set { lngDefaultUnit = value; }
    //     }
    //     private long lngPwdExpirationDays;

    //     public long PwdExpirationDays
    //     {
    //         get { return lngPwdExpirationDays; }
    //         set { lngPwdExpirationDays = value; }
    //     }

    //     private bool bolFirstLevel;

    //     public bool FirstLevel
    //     {
    //         get { return bolFirstLevel; }
    //         set { bolFirstLevel = value; }
    //     }
    //     private bool bolSecondLevel;

    //     public bool SecondLevel
    //     {
    //         get { return bolSecondLevel; }
    //         set { bolSecondLevel = value; }
    //     }
    //     private bool bolThirdLevel;

    //     public bool ThirdLevel
    //     {
    //         get { return bolThirdLevel; }
    //         set { bolThirdLevel = value; }
    //     }
    //     private bool bolCheckRight;

    //     public bool CheckRight
    //     {
    //         get { return bolCheckRight; }
    //         set { bolCheckRight = value; }
    //     }

    //     private int intPermissionType;

    //     public int PermissionType
    //     {
    //         get { return intPermissionType; }
    //         set { intPermissionType = value; }
    //     }

    //     #region IValueObject Members

    //     public string ToXml()
    //     {
    //         return this.ToString();
    //     }

    //     #endregion
    // }
}
