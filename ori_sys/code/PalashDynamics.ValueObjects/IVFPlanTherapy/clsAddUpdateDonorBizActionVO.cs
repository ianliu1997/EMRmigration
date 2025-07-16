using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsAddUpdateDonorBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsAddUpdateDonorBizAction";
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

        private clsPatientVO _DonorDetails = new clsPatientVO();
        public clsPatientVO DonorDetails
        {
            get
            {
                return _DonorDetails;
            }
            set
            {
                _DonorDetails = value;
            }
        }
        private clsDonorBatchVO _BatchDetails = new clsDonorBatchVO();
        public clsDonorBatchVO BatchDetails
        {
            get
            {
                return _BatchDetails;
            }
            set
            {
                _BatchDetails = value;
            }
        }

       
    }
    public class clsGetDonorDetailsForIUIBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDonorDetailsForIUIBizAction";
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


        private clsPatientVO _DonorDetails = new clsPatientVO();
        public clsPatientVO DonorDetails
        {
            get
            {
                return _DonorDetails;
            }
            set
            {
                _DonorDetails = value;
            }
        }
        private clsDonorBatchVO _BatchDetails = new clsDonorBatchVO();
        public clsDonorBatchVO BatchDetails
        {
            get
            {
                return _BatchDetails;
            }
            set
            {
                _BatchDetails = value;
            }
        }
       


    }
    public class clsDonorBatchVO : IValueObject, INotifyPropertyChanged
    {

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
        private long _LabID;
        public long LabID
        {
            get { return _LabID; }
            set
            {
                if (_LabID != value)
                {
                    _LabID = value;
                    OnPropertyChanged("LabID");
                }
            }
        }
        private long _ReceivedByID;
        public long ReceivedByID
        {
            get { return _ReceivedByID; }
            set
            {
                if (_ReceivedByID != value)
                {
                    _ReceivedByID = value;
                    OnPropertyChanged("ReceivedByID");
                }
            }
        }
        private DateTime _ReceivedDate;
        public DateTime ReceivedDate
        {
            get { return _ReceivedDate; }
            set
            {
                if (_ReceivedDate != value)
                {
                    _ReceivedDate = value;
                    OnPropertyChanged("ReceivedDate");
                }
            }
        }
        private string _InvoiceNo;
        public string InvoiceNo
        {
            get { return _InvoiceNo; }
            set
            {
                if (_InvoiceNo != value)
                {
                    _InvoiceNo = value;
                    OnPropertyChanged("InvoiceNo");
                }
            }
        }

        private string _BatchCode;
        public string BatchCode
        {
            get { return _BatchCode; }
            set
            {
                if (_BatchCode != value)
                {
                    _BatchCode = value;
                    OnPropertyChanged("BatchCode");
                }
            }
        }
        private int _NoOfVails;
        public int NoOfVails
        {
            get { return _NoOfVails; }
            set
            {
                if (_NoOfVails != value)
                {
                    _NoOfVails = value;
                    OnPropertyChanged("NoOfVails");
                }
            }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                if (_Remark != value)
                {
                    _Remark = value;
                    OnPropertyChanged("Remark");
                }
            }
        }

        private string _Eye;
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
        private string _Lab;
        public string Lab
        {
            get { return _Lab; }
            set
            {
                if (_Lab != value)
                {
                    _Lab = value;
                    OnPropertyChanged("Lab");
                }
            }
        }
        private string _ReceivedBy;
        public string ReceivedBy
        {
            get { return _ReceivedBy; }
            set
            {
                if (_ReceivedBy != value)
                {
                    _ReceivedBy = value;
                    OnPropertyChanged("ReceivedBy");
                }
            }
        }
        
        private float _Height;
        public float Height
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

    public class clsDonorGeneralDetailsVO : IValueObject, INotifyPropertyChanged
    {

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
        private long _LabID;
        public long LabID
        {
            get { return _LabID; }
            set
            {
                if (_LabID != value)
                {
                    _LabID = value;
                    OnPropertyChanged("LabID");
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
        private string _Lab;
        public string Lab
        {
            get { return _Lab; }
            set
            {
                if (_Lab != value)
                {
                    _Lab = value;
                    OnPropertyChanged("Lab");
                }
            }
        }

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

        private float _Height;
        public float Height
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
        private string _Eye;
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

        public long TariffID { get; set; }
        public long PatientSourceID { get; set; }
        public long CompanyID { get; set; }

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

        
        public string UniversalID { get; set; }
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
        public string SearchKeyword { get; set; }
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

    public class clsGetDonorDetailsAgainstSearchBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDonorDetailsAgainstSearchBizAction";
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


        private clsPatientVO _DonorDetails = new clsPatientVO();
        public clsPatientVO DonorDetails
        {
            get
            {
                return _DonorDetails;
            }
            set
            {
                _DonorDetails = value;
            }
        }
        private clsDonorGeneralDetailsVO _DonorGeneralDetails = new clsDonorGeneralDetailsVO();
        public clsDonorGeneralDetailsVO DonorGeneralDetails
        {
            get
            {
                return _DonorGeneralDetails;
            }
            set
            {
                _DonorGeneralDetails = value;
            }
        }
        private List<clsDonorGeneralDetailsVO> _DonorGeneralDetailsList = new List<clsDonorGeneralDetailsVO>();
        public List<clsDonorGeneralDetailsVO> DonorGeneralDetailsList
        {
            get
            {
                return _DonorGeneralDetailsList;
            }
            set
            {
                _DonorGeneralDetailsList = value;
            }
        }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string sortExpression { get; set; }
        public long UnitID { get; set; }


    }
    public class clsGetDonorDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDonorDetailsBizAction";
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

        private clsPatientVO _DonorDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO DonorDetails
        {
            get { return _DonorDetails; }
            set { _DonorDetails = value; }
        }
    
    }

    public class clsGetDonorBatchDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDonorBatchDetailsBizAction";
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

        private clsDonorBatchVO _BatchDetails = new clsDonorBatchVO();
        public clsDonorBatchVO BatchDetails
        {
            get
            {
                return _BatchDetails;
            }
            set
            {
                _BatchDetails = value;
            }
        }

        private List<clsDonorBatchVO> _BatchDetailsList = new List<clsDonorBatchVO>();
        public List<clsDonorBatchVO> BatchDetailsList
        {
            get
            {
                return _BatchDetailsList;
            }
            set
            {
                _BatchDetailsList = value;
            }
        }
    }

    public class clsCheckDuplicasyDonorCodeAndBLabBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsCheckDuplicasyDonorCodeAndBLabBizAction";
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

        public long LabID { get; set; }
        public string DonorCode { get; set; }
        private bool _IsDuplicate;
        public bool IsDuplicate
        {
            get { return _IsDuplicate; }
            set
            {
                if (_IsDuplicate != value)
                {
                    _IsDuplicate = value;
                }
            }
        }

    }

    public class clsGetDonorListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetDonorListBizAction";
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
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string sortExpression { get; set; }
        private clsPatientVO _DonorDetails = new clsPatientVO();
        public clsPatientVO DonorDetails
        {
            get
            {
                return _DonorDetails;
            }
            set
            {
                _DonorDetails = value;
            }
        }

        private List<clsPatientVO> _DonorGeneralDetailsList = new List<clsPatientVO>();
        public List<clsPatientVO> DonorGeneralDetailsList
        {
            get
            {
                return _DonorGeneralDetailsList;
            }
            set
            {
                _DonorGeneralDetailsList = value;
            }
        }


    }
}
