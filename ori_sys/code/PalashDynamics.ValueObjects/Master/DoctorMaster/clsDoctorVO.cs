using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
//using System.Windows.Media.Imaging;




namespace PalashDynamics.ValueObjects.Master
{
    public class clsDoctorVO : INotifyPropertyChanged,IValueObject
    {

        public clsDoctorVO()
        {

        }
        private List<clsDoctorVO> _DoctorDepartmentDetails;
        public List<clsDoctorVO> DoctorDepartmentDetails
        {
            get
            {
                if (_DoctorDepartmentDetails == null)
                    _DoctorDepartmentDetails = new List<clsDoctorVO>();

                return _DoctorDepartmentDetails;
            }

            set
            {

                _DoctorDepartmentDetails = value;

            }
        }

        private List<clsDepartmentsDetailsVO> _DepartmentDetails;
        public List<clsDepartmentsDetailsVO> DepartmentDetails
        {
            get
            {
                if (_DepartmentDetails == null)
                    _DepartmentDetails = new List<clsDepartmentsDetailsVO>();

                return _DepartmentDetails;
            }

            set
            {

                _DepartmentDetails = value;

            }
        }

        private List<clsUnitDepartmentsDetailsVO> _UnitDepartmentDetails;
        public List<clsUnitDepartmentsDetailsVO> UnitDepartmentDetails
        {
            get
            {
                if (_UnitDepartmentDetails == null)
                    _UnitDepartmentDetails = new List<clsUnitDepartmentsDetailsVO>();

                return _UnitDepartmentDetails;
            }

            set
            {

                _UnitDepartmentDetails = value;

            }
        }


        //Added By Somnath
        private List<clsClassificationDetailVO> _ClassificationDetails;
        public List<clsClassificationDetailVO> ClassificationDetails
        {
            get
            {
                if (_ClassificationDetails == null)
                    _ClassificationDetails = new List<clsClassificationDetailVO>();

                return _ClassificationDetails;
            }

            set
            {

                _ClassificationDetails = value;

            }
        }

        private clsUnitClassificationsDetailsVO _UnitClassificationDetails;
        public clsUnitClassificationsDetailsVO UnitClassificationDetails
        {
            get
            {
                if (_UnitClassificationDetails == null)
                    _UnitClassificationDetails = new clsUnitClassificationsDetailsVO();

                return _UnitClassificationDetails;
            }

            set
            {

                _UnitClassificationDetails = value;

            }
        }


        private List<clsUnitClassificationsDetailsVO> _UnitClassificationDetailsList;
        public List<clsUnitClassificationsDetailsVO> UnitClassificationDetailsList
        {
            get
            {
                if (_UnitClassificationDetailsList == null)
                    _UnitClassificationDetailsList = new List<clsUnitClassificationsDetailsVO>();

                return _UnitClassificationDetailsList;
            }

            set
            {

                _UnitClassificationDetailsList = value;

            }
        }

        //end

        private List<clsDoctorBankInfoVO> _DoctorBankInfo;
        public List<clsDoctorBankInfoVO> DoctorBankInfo
        {
            get
            {
                if (_DoctorBankInfo == null)
                    _DoctorBankInfo = new List<clsDoctorBankInfoVO>();

                return _DoctorBankInfo;
            }

            set
            {

                _DoctorBankInfo = value;

            }
        }

        private clsDoctorBankInfoVO _DoctorBankInformation;
        public clsDoctorBankInfoVO DoctorBankInformation
        {
            get
            {
                if (_DoctorBankInformation == null)
                    _DoctorBankInformation = new clsDoctorBankInfoVO();

                return _DoctorBankInformation;
            }

            set
            {

                _DoctorBankInformation = value;

            }
        }




        private List<clsDoctorAddressInfoVO> _DoctorAddressInfo;
        public List<clsDoctorAddressInfoVO> DoctorAddressInfo
        {
            get
            {
                if (_DoctorAddressInfo == null)
                    _DoctorAddressInfo = new List<clsDoctorAddressInfoVO>();

                return _DoctorAddressInfo;
            }

            set
            {

                _DoctorAddressInfo = value;

            }
        }

        private clsDoctorAddressInfoVO _DoctorAddressInformation;
        public clsDoctorAddressInfoVO DoctorAddressInformation
        {
            get
            {
                if (_DoctorAddressInformation == null)
                    _DoctorAddressInformation = new clsDoctorAddressInfoVO();

                return _DoctorAddressInformation;
            }

            set
            {

                _DoctorAddressInformation = value;

            }
        }


        private List<clsDoctorWaiverDetailVO> _DoctorWaiverInfoList;
        public List<clsDoctorWaiverDetailVO> DoctorWaiverInfoList
        {
            get
            {
                if (_DoctorWaiverInfoList == null)
                    _DoctorWaiverInfoList = new List<clsDoctorWaiverDetailVO>();

                return _DoctorWaiverInfoList;
            }

            set
            {

                _DoctorWaiverInfoList = value;

            }
        }

        private clsDoctorWaiverDetailVO _DoctorWaiverInformation;
        public clsDoctorWaiverDetailVO DoctorWaiverInformation
        {
            get
            {
                if (_DoctorWaiverInformation == null)
                    _DoctorWaiverInformation = new clsDoctorWaiverDetailVO();

                return _DoctorWaiverInformation;
            }

            set
            {

                _DoctorWaiverInformation = value;

            }
        }


        private List<MasterListItem> _MasterListItem;

        public List<MasterListItem> MasterListItem
        {
            get { return _MasterListItem; }
            set { _MasterListItem = value; }
        }



        private long lngDoctorId;
        public long DoctorId
        {
            get { return lngDoctorId; }
            set
            {
                if (value != lngDoctorId)
                {
                    lngDoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }


        //ID For M_DoctorDepartmentDetails
        private long lngDoctorDepartmentDetailID;
        public long DoctorDepartmentDetailID
        {
            get { return lngDoctorDepartmentDetailID; }
            set
            {
                if (value != lngDoctorDepartmentDetailID)
                {
                    lngDoctorDepartmentDetailID = value;
                    OnPropertyChanged("DoctorDepartmentDetailID");
                }
            }
        }
        
        
        private bool _DoctorDepartmentDetailStatus;
        public bool DoctorDepartmentDetailStatus
        {
            get { return _DoctorDepartmentDetailStatus; }
            set
            {
                if (value != _DoctorDepartmentDetailStatus)
                {
                    _DoctorDepartmentDetailStatus = value;
                    OnPropertyChanged("DoctorDepartmentDetailStatus");
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


        public byte[] Photo { get; set; }

        public byte[] Signature { get; set; }
       
       // public BitmapImage BitmapImg { get; set; }

        public bool IsFromReferralDoctorChildWindow { get; set; }
       

        private long lngSpecialization;
        public long Specialization
        {
            get { return lngSpecialization; }
            set
            {
                if (value != lngSpecialization)
                {
                    lngSpecialization = value;
                    OnPropertyChanged("Specialization");
                }
            }
        }
       

        private string _Specialization;
        public string SpecializationDis
        {
            get { return _Specialization; }
            set
            {
                if (value != _Specialization)
                {
                    _Specialization = value;
                    OnPropertyChanged("SpecializationDis");
                }
            }
        }

        private long lngSubSpecialization;
        public long SubSpecialization
        {
            get { return lngSubSpecialization; }
            set
            {
                if (value != lngSubSpecialization)
                {
                    lngSubSpecialization = value;
                    OnPropertyChanged("SubSpecialization");
                }
            }
        }

        private string  _SubSpecialization;
        public string SubSpecializationDis
        {
            get { return _SubSpecialization; }
            set
            {
                if (value != _SubSpecialization)
                {
                    _SubSpecialization = value;
                    OnPropertyChanged("SubSpecializationDis");
                }
            }
        }

        private long lngDoctorType;
        public long DoctorType
        {
            get { return lngDoctorType; }
            set
            {
                if (value != lngDoctorType)
                {
                    lngDoctorType = value;
                    OnPropertyChanged("DoctorType");
                }
            }
        }

        private string _DoctorType;
        public string DoctorTypeDis
        {
            get { return _DoctorType; }
            set
            {
                if (value != _DoctorType)
                {
                    _DoctorType = value;
                    OnPropertyChanged("DoctorTypeDis");
                }
            }
        }


        private long lngDepartmentId;
        public long DepartmentID
        {
            get { return lngDepartmentId; }
            set
            {
                if (value != lngDepartmentId)
                {
                    lngDepartmentId = value;
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private long lngGenderId;
        public long GenderId
        {
            get { return lngGenderId; }
            set
            {
                if (value != lngGenderId)
                {
                    lngGenderId = value;
                    OnPropertyChanged("GenderId");
                }
            }
        }

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
        public long DoctorShareLinkingTypeID { get; set; }
        private string strDoctorName = "";
        public string DoctorName
        {
            get { return strDoctorName = strFirstName + " " + strMiddleName + " " + strLastName; }
            set
            {
                if (value != strDoctorName)
                {
                    strDoctorName = value;
                    OnPropertyChanged("DoctorName");
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

        private string strMiddleName = "";
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

        // Added By Somnath


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
        private long _DoctorCategoryId;
        public long DoctorCategoryId
        {
            get { return _DoctorCategoryId; }
            set
            {
                if (value != _DoctorCategoryId)
                {
                    _DoctorCategoryId = value;
                    OnPropertyChanged("DoctorCategoryId");
                }
            }
        }

        private string _DoctorCategory;
        public string DoctorCategoryDesc
        {
            get { return _DoctorCategory; }
            set
            {
                if (value != _DoctorCategory)
                {
                    _DoctorCategory = value;
                    OnPropertyChanged("DoctorCategoryDesc");
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

        private string _ProvidentFund;
        public string ProvidentFund
        {
            get { return _ProvidentFund; }
            set
            {
                if (value != _ProvidentFund)
                {
                    _ProvidentFund = value;
                    OnPropertyChanged("ProvidentFund");
                }
            }
        }

        private string _PermanentAccountNumber;
        public string PermanentAccountNumber
        {
            get { return _PermanentAccountNumber; }
            set
            {
                if (value != _PermanentAccountNumber)
                {
                    _PermanentAccountNumber = value;
                    OnPropertyChanged("PermanentAccountNumber");
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

        private string _RegistrationNumber;
        public string RegistrationNumber
        {
            get { return _RegistrationNumber; }
            set
            {
                if (value != _RegistrationNumber)
                {
                    _RegistrationNumber = value;
                    OnPropertyChanged("RegistrationNumber");
                }
            }
        }

        //End

       

        private string strDepartmentName;
        public string DepartmentName
            {
                get { return strDepartmentName; }
                set { strDepartmentName = value; }
            }

        private string _DepartmentCode;
        public string DepartmentCode
        {
            get { return _DepartmentCode; }
            set { _DepartmentCode = value; }
        }


        private bool _DepartmentStatus;
        public bool DepartmentStatus
        {
            get { return _DepartmentStatus; }
            set
            {
                if (value != _DepartmentStatus)
                {
                    _DepartmentStatus = value;
                    OnPropertyChanged("DepartmentStatus");
                }
            }
        }

        private long lngUnitDepartmentUnitID;
        public long UnitDepartmentUnitID
        {
            get { return lngUnitDepartmentUnitID; }
            set
            {
                if (value != lngUnitDepartmentUnitID)
                {
                    lngUnitDepartmentUnitID = value;
                    OnPropertyChanged("UnitDepartmentUnitID");
                }
            }
        }

        private string strUnitName;
        public string UnitName
            {
                get { return strUnitName; }
                set { strUnitName = value; }
            }

        private string strUnitDepartmentName;
        public string UnitDepartName
        {
            get { return strUnitDepartmentName = strUnitName + "-" + strDepartmentName; }
            set { strUnitDepartmentName = value; }
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


        private TimeSpan dtpFromTime;
        public TimeSpan FromTime
        {
            get { return dtpFromTime; }
            set
            {
                if (value != dtpFromTime)
                {
                    dtpFromTime = value;
                    OnPropertyChanged("FromTime");
                }
            }
        }

        private TimeSpan dtpToTime;
        public TimeSpan ToTime
        {
            get { return dtpToTime; }
            set
            {
                if (value != dtpToTime)
                {
                    dtpToTime = value;
                    OnPropertyChanged("ToTime");
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


        private long _ClassificationD;
        public long ClassificationID
        {
            get
            {
                return _ClassificationD;
            }

            set
            {
                if (value != _ClassificationD)
                {
                    _ClassificationD = value;

                }
            }
        }

        private string _UnitClassificationName;
        public string UnitClassificationName
        {
            get
            {
                return _UnitClassificationName;
            }

            set
            {
                if (value != _UnitClassificationName)
                {
                    _UnitClassificationName = value;

                }
            }
        }
        private bool _IsSelectedDoctor = true;
        public bool IsSelectedDoctor
        {
            get { return _IsSelectedDoctor; }
            set
            {
                if (value != _IsSelectedDoctor)
                {
                    _IsSelectedDoctor = value;
                    OnPropertyChanged("IsSelectedDoctor");
                }
            }
        }
        private bool _SelectDoctor;
        public bool SelectDoctor
        {
            get { return _SelectDoctor; }
            set
            {
                if (value != _SelectDoctor)
                {
                    _SelectDoctor = value;
                    OnPropertyChanged("SelectDoctor");
                }
            }
        }
        private bool _SelectedDoctor = false;
        public bool SelectedDoctor
        {
            get { return _SelectedDoctor; }
            set
            {
                if (value != _SelectedDoctor)
                {
                    _SelectedDoctor = value;
                    OnPropertyChanged("SelectedDoctor");
                }
            }
        }
        private Boolean _IsEnabled = false;
        public Boolean IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                OnPropertyChanged("IsEnabled");
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

                }
            }
        }

        private string _DoctName;
        public string DoctName
        {
            get { return _DoctName; }
            set
            {
                if (value != _DoctName)
                {
                    _DoctName = value;
                    OnPropertyChanged("DoctName");
                }
            }
        }

        private Boolean _IsSelected = false;
        public Boolean IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        //***//--------------
        private long _MarketingExecutivesID; 
        public long MarketingExecutivesID
        {
            get { return _MarketingExecutivesID; }
            set { _MarketingExecutivesID = value; }
        }

        public bool IsDocAttached { get; set; }
        public string AttachedImage { get; set; }
        //------------

        #region CommonField
       
        private bool blnStatus;
        public bool Status
        {
            get { return blnStatus; }
            set
            {
                if (value != blnStatus)
                {
                    blnStatus = value;
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
                if (value != _AddedDateTime)
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

        private DateTime? _UpdatedDateTime  = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
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


        //public override string ToString()
        //{
        //    return this.;
        //}
    }

    public class clsDepartmentsDetailsVO
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
                    
                }
            }
        }

        public string DepartmentName { get; set; }

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
                    
                }
            }
        }

    }

    public class clsClassificationDetailVO
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

                }
            }
        }

        public string DepartmentName { get; set; }

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

                }
            }
        }
    }

    public class clsUnitDepartmentsDetailsVO
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

                }
            }
        }

        private string strDepartmentName;
        public string DepartmentName
        {
            get { return strDepartmentName; }
            set { strDepartmentName = value; }
        }

        private string strUnitName;
        public string UnitName
        {
            get { return strUnitName; }
            set { strUnitName = value; }
        }



        public string UnitDepartName { get; set; }


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

                }
            }
        }

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

                }
            }
        }


    }

    public class clsUnitClassificationsDetailsVO
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

                }
            }
        }

        private string strClassificationName;
        public string ClassificationName
        {
            get { return strClassificationName; }
            set { strClassificationName = value; }
        }

        private string strUnitName;
        public string UnitName
        {
            get { return strUnitName; }
            set { strUnitName = value; }
        }



        public string UnitClassificationName { get; set; }


        private long _ClassificationD;
        public long ClassificationID
        {
            get
            {
                return _ClassificationD;
            }

            set
            {
                if (value != _ClassificationD)
                {
                    _ClassificationD = value;

                }
            }
        }

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

                }
            }
        }


        private bool _IsAvailable;
        public bool IsAvailable
        {
            get
            {
                return _IsAvailable;
            }

            set
            {
                if (value != _IsAvailable)
                {
                    _IsAvailable = value;

                }
            }
        }

        private string _IsAvailableStr;
        public string IsAvailableStr
        {
            get
            {
                return _IsAvailableStr;
            }

            set
            {
                if (value != _IsAvailableStr)
                {
                    _IsAvailableStr = value;

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

                }
            }
        }


    }

    public class clsDoctorBankInfoVO : INotifyPropertyChanged,IValueObject
    {
        public clsDoctorBankInfoVO()
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

        private long _MICRNumber;
        public long MICRNumber
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


//***//----

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
//------------
        private long _DoctorId;
        public long DoctorId
        {
            get { return _DoctorId; }
            set
            {
                if (value != _DoctorId)
                {
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
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

    public class clsDoctorAddressInfoVO : INotifyPropertyChanged, IValueObject
    {
        public clsDoctorAddressInfoVO()
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


        private long _DoctorId;
        public long DoctorId
        {
            get { return _DoctorId; }
            set
            {
                if (value != _DoctorId)
                {
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
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
       



    }

    public class clsDoctorWaiverDetailVO : INotifyPropertyChanged, IValueObject
    {
        public clsDoctorWaiverDetailVO()
        {

        }
        public clsDoctorWaiverDetailVO(long Id, string Description)
        {
            this.ServiceID = Id;
            this.ServiceName = Description;
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

        //Added By Somanath For Doctor Waiver Detail 

        private DateTime? _AddedDateTime = DateTime.Now;
        public DateTime? AddedDateTime
        {
            get { return _AddedDateTime; }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private string _PageName;
        public string PageName
        {
            get { return _PageName; }
            set
            {
                if (value != _PageName)
                {
                    _PageName = value;
                    OnPropertyChanged("PageName");
                }
            }
        }

        private DateTime? _UpdatedDateTime = DateTime.Now;
        public DateTime? UpdatedDateTime
        {
            get { return _UpdatedDateTime; }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }

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

        private string _DoctorName;
        public string DoctorName
        {
            get { return _DoctorName; }
            set
            {
                if (value != _DoctorName)
                {
                    _DoctorName = value;
                    OnPropertyChanged("DoctorName");
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

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
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


        private decimal _DoctorShareAmount = Convert.ToDecimal("0.00");
        public decimal DoctorShareAmount
        {
            get { return _DoctorShareAmount; }
            set
            {
                if (value != _DoctorShareAmount)
                {
                    _DoctorShareAmount = value;
                    OnPropertyChanged("DoctorShareAmount");
                }
            }
        }

        private decimal _EmergencyDoctorSharePercentage = Convert.ToDecimal("0.00");
        public decimal EmergencyDoctorSharePercentage
        {
            get { return _EmergencyDoctorSharePercentage; }
            set
            {
                if (value != _EmergencyDoctorSharePercentage)
                {
                    _EmergencyDoctorSharePercentage = value;
                    OnPropertyChanged("EmergencyDoctorSharePercentage");
                }
            }
        }

        private decimal _DoctorSharePercentage=Convert.ToDecimal("0.00");
        public decimal DoctorSharePercentage
        {
            get { return _DoctorSharePercentage; }
            set
            {
                if (value != _DoctorSharePercentage)
                {
                    _DoctorSharePercentage = value;
                    OnPropertyChanged("DoctorSharePercentage");
                }
            }
        }

        private decimal _EmergencyDoctorShareAmount=Convert.ToDecimal("0.00");
        public decimal EmergencyDoctorShareAmount
        {
            get { return _EmergencyDoctorShareAmount; }
            set
            {
                if (value != _EmergencyDoctorShareAmount)
                {
                    _EmergencyDoctorShareAmount = value;
                    OnPropertyChanged("EmergencyDoctorShareAmount");
                }
            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        private string _TariffName;
        public string TariffName
        {
            get { return _TariffName; }
            set
            {
                if (value != _TariffName)
                {
                    _TariffName = value;
                    OnPropertyChanged("TariffName");
                }
            }
        }

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (value != _ServiceID)
                {
                    _ServiceID = value;
                    OnPropertyChanged("ServiceID");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private decimal _Rate;
        public decimal Rate
        {
            get { return _Rate; }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

        private decimal _EmergencyRate;
        public decimal EmergencyRate
        {
            get { return _EmergencyRate; }
            set
            {
                if (value != _EmergencyRate)
                {
                    _EmergencyRate = value;
                    OnPropertyChanged("EmergencyRate");
                }
            }
        }

        private long _WaiverDays;
        public long WaiverDays
        {
            get { return _WaiverDays; }
            set
            {
                if (value != _WaiverDays)
                {
                    _WaiverDays = value;
                    OnPropertyChanged("WaiverDays");
                }
            }
        }
        //#region IValueObject Members

        //public string ToXml1()
        //{
        //    return this.ToString();
        //}

        //#endregion

        public override string ToString()
        {
            return this.ServiceName;
        }

        
    }
}
