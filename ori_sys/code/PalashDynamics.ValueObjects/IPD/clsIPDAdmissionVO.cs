using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.IPD
{
   public class clsIPDAdmissionVO : IValueObject,INotifyPropertyChanged
    {
        public string ToXml()
        {
            return this.ToString();
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
       
        #region Common Properties

            
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
        private long _AdmissionUnitID;
        public long AdmissionUnitID
        {
            get { return _AdmissionUnitID; }
            set
            {
                if (_AdmissionUnitID != value)
                {
                    _AdmissionUnitID = value;
                    OnPropertyChanged("AdmissionUnitID");
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

       //Added By Bhushanp 02/02/2017

        private long _BillUnitID;

        public long BillUnitID
        {
            get { return _BillUnitID; }
            set { _BillUnitID = value; }
        }

        private string _BabyWeight;

        public string BabyWeight
        {
            get { return _BabyWeight; }
            set { _BabyWeight = value; }
        }

        private long _GenderID;

        public long GenderID
        {
            get { return _GenderID; }
            set { _GenderID = value; }
        }

        #endregion

        #region Property Declaration Section
        private string _strWard;
        public string strWard
        {
            get { return _strWard; }
            set
            {
                if (_strWard != value)
                {
                    _strWard = value;
                    OnPropertyChanged("strWard");
                }
            }
        }
        private bool _IsCancelled;
        public bool IsCancelled
        {
            get { return _IsCancelled; }
            set
            {
                if (_IsCancelled != value)
                {
                    _IsCancelled = value;
                    OnPropertyChanged("IsCancelled");
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

        private string _PatientName;
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
        private string _GenderName;
        public string GenderName
        {
            get { return _GenderName; }
            set
            {
                if (_GenderName != value)
                {
                    _GenderName = value;
                    OnPropertyChanged("GenderName");
                }
            }
        }

        private string _Authority;
        public string Authority
        {
            get { return _Authority; }
            set
            {
                if (_Authority != value)
                {
                    _Authority = value;
                    OnPropertyChanged("Authority");
                }
            }
        }

        private string _Number;
        public string Number
        {
            get { return _Number; }
            set
            {
                if (_Number != value)
                {
                    _Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                if (_Address != value)
                {
                    _Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }
        private long _AdmID;
        public long AdmID
        {
            get { return _AdmID; }
            set
            {
                if (_AdmID != value)
                {
                    _AdmID = value;
                    OnPropertyChanged("AdmID");
                }
            }
        }
        private string _IsCheckBoxVisible;
        public string IsCheckBoxVisible
        {
            get { return _IsCheckBoxVisible; }
            set
            {
                if (_IsCheckBoxVisible != value)
                {
                    _IsCheckBoxVisible = value;
                    OnPropertyChanged("IsCheckBoxVisible");
                }
            }
        }

        private bool? _IsAppliedForApproval;
        public bool? IsAppliedForApproval
        {
            get { return _IsAppliedForApproval; }
            set
            {
                if (_IsAppliedForApproval != value)
                {
                    _IsAppliedForApproval = value;
                    OnPropertyChanged("IsAppliedForApproval");
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
        private DateTime? _DateOfBirth;
        public DateTime? DateOfBirth
        {
            get { return _DateOfBirth; }
            set
            {
                if (_DateOfBirth != value)
                {
                    _DateOfBirth = value;
                    OnPropertyChanged("DateOfBirth");
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
        private string _Bed;
        public string Bed
        {
            get { return _Bed; }
            set
            {
                if (_Bed != value)
                {
                    _Bed = value;
                    OnPropertyChanged("Bed");
                }
            }
        }

        private string _Ward;
        public string Ward
        {
            get { return _Ward; }
            set
            {
                if (_Ward != value)
                {
                    _Ward = value;
                    OnPropertyChanged("Ward");
                }
            }
        }
        private decimal _BillAmount;
        public decimal BillAmount
        {
            get { return _BillAmount; }
            set
            {
                if (_BillAmount != value)
                {
                    _BillAmount = value;
                    OnPropertyChanged("BillAmount");
                }
            }
        }

        private long _InterOrFinal;
        public long InterOrFinal
        {
            get { return _InterOrFinal; }
            set
            {
                if (_InterOrFinal != value)
                {
                    _InterOrFinal = value;
                    OnPropertyChanged("InterOrFinal");
                }
            }
        }

        private decimal _SelfPay;
        public decimal SelfPay
        {
            get { return _SelfPay; }
            set
            {
                if (_SelfPay != value)
                {
                    _SelfPay = value;
                    OnPropertyChanged("SelfPay");
                }
            }
        }

        private decimal _CompanyPay;
        public decimal CompanyPay
        {
            get { return _CompanyPay; }
            set
            {
                if (_CompanyPay != value)
                {
                    _CompanyPay = value;
                    OnPropertyChanged("CompanyPay");
                }
            }
        }
        private decimal _Balance;
        public decimal Balance
        {
            get { return _Balance; }
            set
            {
                if (_Balance != value)
                {
                    _Balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        private decimal _TotalBalance;
        public decimal TotalBalance
        {
            get { return _TotalBalance; }
            set
            {
                if (_TotalBalance != value)
                {
                    _TotalBalance = value;
                    OnPropertyChanged("TotalBalance");
                }
            }
        }

        private decimal _Advance;
        public decimal Advance
        {
            get { return _Advance; }
            set
            {
                if (_Advance != value)
                {
                    _Advance = value;
                    OnPropertyChanged("Advance");
                }
            }
        }
        private decimal _CompanyApprovedAmount;
        public decimal CompanyApprovedAmount
        {
            get { return _CompanyApprovedAmount; }
            set
            {
                if (_CompanyApprovedAmount != value)
                {
                    _CompanyApprovedAmount = value;
                    OnPropertyChanged("CompanyApprovedAmount");
                }
            }
        }
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set
            {
                if (_IsClosed != value)
                {
                    _IsClosed = value;
                    OnPropertyChanged("IsClosed");
                }
            }
        }
        private long _CompanyId;
        public long CompanyId
        {
            get { return _CompanyId; }
            set
            {
                if (_CompanyId != value)
                {
                    _CompanyId = value;
                    OnPropertyChanged("CompanyId");
                }
            }
        }


        private decimal _CreditLimitAmount;
        public decimal CreditLimitAmount
        {
            get { return _CreditLimitAmount; }
            set
            {
                if (_CreditLimitAmount != value)
                {
                    _CreditLimitAmount = value;
                    OnPropertyChanged("CreditLimitAmount");
                }
            }
        }

        private bool _IsCancelAdmEnable = true;
        public bool IsCancelAdmEnable
        {
            get { return _IsCancelAdmEnable; }
            set
            {
                if (_IsCancelAdmEnable != value)
                {
                    _IsCancelAdmEnable = value;
                    OnPropertyChanged("IsCancelAdmEnable");
                }
            }
        }

        private bool _IsAdviseEnable = true;
        public bool IsAdviseEnable
        {
            get { return _IsAdviseEnable; }
            set
            {
                if (_IsAdviseEnable != value)
                {
                    _IsAdviseEnable = value;
                    OnPropertyChanged("IsAdviseEnable");
                }
            }
        }


        private string _BedClass;
        public string BedClass
        {
            get { return _BedClass; }
            set
            {
                if (_BedClass != value)
                {
                    _BedClass = value;
                    OnPropertyChanged("_BedClass");
                }
            }
        }

        public long classID { get; set; }
        private long _ReferingEntityID;
        public long ReferingEntityID
        {
            get { return _ReferingEntityID; }
            set
            {
                if (_ReferingEntityID != value)
                {
                    _ReferingEntityID = value;
                    OnPropertyChanged("ReferingEntityID");
                }
            }
        }
        private string _DFName;
        public string DFName
        {
            get { return _DFName; }
            set
            {
                if (_DFName != value)
                {
                    _DFName = value;
                    OnPropertyChanged("DFName");
                }
            }
        }

        private string _DLName;
        public string DLName
        {
            get { return _DLName; }
            set
            {
                if (_DLName != value)
                {
                    _DLName = value;
                    OnPropertyChanged("DLName");
                }
            }
        }

        private string _RefDoctor;
        public string RefDoctor
        {
            get { return _RefDoctor; }
            set
            {
                if (_RefDoctor != value)
                {
                    _RefDoctor = value;
                    OnPropertyChanged("RefDoctor");
                }
            }
        }

        private string _CompanyName;
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

        private clsIPDBedTransferVO _BedTransfer = new clsIPDBedTransferVO();
        public clsIPDBedTransferVO BedTransfer
        {
            get { return _BedTransfer; }
            set { value = _BedTransfer; }
        }

        private int _NextapColor;
        public int NextapColor
        {
            get
            {
                return _NextapColor;
            }
            set
            {
                _NextapColor = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NextapColor"));
                }
            }
        }

        private bool _IsEnableDischarge = false;
        public bool IsEnableDischarge
        {
            get { return _IsEnableDischarge; }
            set
            {
                if (_IsEnableDischarge != value)
                {
                    _IsEnableDischarge = value;
                    OnPropertyChanged("IsEnableDischarge");
                }
            }
        }

        private bool _IsEnableIPDBill = false;
        public bool IsEnableIPDBill
        {
            get { return _IsEnableIPDBill; }
            set
            {
                if (_IsEnableIPDBill != value)
                {
                    _IsEnableIPDBill = value;
                    OnPropertyChanged("IsEnableIPDBill");
                }
            }
        }

        //Added by AJ Date 5/1/2017
        private bool _hlMConsumption = false;
        public bool hlMConsumption
        {
            get { return _hlMConsumption; }
            set
            {
                if (_hlMConsumption != value)
                {
                    _hlMConsumption = value;
                    OnPropertyChanged("hlMConsumption");
                }
            }
        }
        //***//---------------
     
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        private long _ID;
        public  long ID
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
        public  long UnitId
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

        private long _PatientId;
        public  long PatientId
        {
            get { return _PatientId; }
            set
            {
                if (_PatientId != value)
                {
                    _PatientId = value;
                    OnPropertyChanged("PatientId");
                }
            }
        }

        private long _PatientUnitID;
        public  long PatientUnitID
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

        private DateTime? _Date = System.DateTime.Now;
       public  DateTime? Date
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

        private DateTime _Time;
        public  DateTime Time
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

        private string _AdmissionNO;
        public  string AdmissionNO
        {
            get { return _AdmissionNO; }
            set
            {
                if (_AdmissionNO != value)
                {
                    _AdmissionNO = value;
                    OnPropertyChanged("AdmissionNO");
                }
            }
        }

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

        private long _DepartmentID;
        public  long DepartmentID
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

        private long _DoctorID;
        public  long DoctorID
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
        private long _RefEntityID;
        public long RefEntityID
        {
            get { return _RefEntityID; }
            set
            {
                if (_RefEntityID != value)
                {
                    _RefEntityID = value;
                    OnPropertyChanged("RefEntityID");
                }
            }
        }
        private long _RefEntityTypeID;
        public long RefEntityTypeID
        {
            get { return _RefEntityTypeID; }
            set
            {
                if (_RefEntityTypeID != value)
                {
                    _RefEntityTypeID = value;
                    OnPropertyChanged("RefEntityTypeID");
                }
            }
        }
        private long _CompanyID;
        public long CompanyID
        {
            get
            {
                return _CompanyID;
            }
            set
            {
                if (value != _CompanyID)
                {
                    _CompanyID = value;
                    OnPropertyChanged("CompanyID");
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

        private long _BillCount;
        public long BillCount
        {
            get { return _BillCount; }
            set
            {
                if (_BillCount != value)
                {
                    _BillCount = value;
                    OnPropertyChanged("BillCount");
                }
            }
        }

        private long _UnfreezedBillCount;
        public long UnfreezedBillCount
        {
            get { return _UnfreezedBillCount; }
            set
            {
                if (_UnfreezedBillCount != value)
                {
                    _UnfreezedBillCount = value;
                    OnPropertyChanged("UnfreezedBillCount");
                }
            }
        }
        private long _ConcessionAuthorizedBy;
        public long ConcessionAuthorizedBy
        {
            get { return _ConcessionAuthorizedBy; }
            set
            {
                if (_ConcessionAuthorizedBy != value)
                {
                    _ConcessionAuthorizedBy = value;
                    OnPropertyChanged("ConcessionAuthorizedBy");
                }
            }
        }

        private long _GrossDiscountReasonID;
        public long GrossDiscountReasonID
        {
            get { return _GrossDiscountReasonID; }
            set
            {
                if (_GrossDiscountReasonID != value)
                {
                    _GrossDiscountReasonID = value;
                    OnPropertyChanged("GrossDiscountReasonID");
                }
            }
        }

        private bool _IsFreezed;
        public bool IsFreezed
        {
            get { return _IsFreezed; }
            set
            {
                if (_IsFreezed != value)
                {
                    _IsFreezed = value;
                    OnPropertyChanged("IsFreezed");
                }
            }
        }

        private long? _TariffID;
        public long? TariffID
        {
            get
            {
                return _TariffID;
            }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }
        private string _RefferedDoctor;
        public  string RefferedDoctor
        {
            get { return _RefferedDoctor; }
            set
            {
                if (_RefferedDoctor != value)
                {
                    _RefferedDoctor = value;
                    OnPropertyChanged("RefferedDoctor");
                }
            }
        }

        private bool _MLC;
        public  bool MLC
        {
            get { return _MLC; }
            set
            {
                if (_MLC != value)
                {
                    _MLC = value;
                    OnPropertyChanged("MLC");
                }
            }
        }

        private long _BedCategoryID;
        public   long BedCategoryID
        {
            get { return _BedCategoryID; }
            set
            {
                if (_BedCategoryID != value)
                {
                    _BedCategoryID = value;
                    OnPropertyChanged("BedCategoryID");
                }
            }
        }

        private long _WardID;
        public   long WardID
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

        private long _BedID;
        public   long BedID
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


        public string BedNo { get; set; }
       
        private string _KinName;
        public   string KinName
        {
            get { return _KinName; }
            set
            {
                if (_KinName != value)
                {
                    _KinName = value;
                    OnPropertyChanged("KinName");
                }
            }
        }

        private long _KinRelationID;
        public  long KinRelationID
        {
            get { return _KinRelationID; }
            set
            {
                if (_KinRelationID != value)
                {
                    _KinRelationID = value;
                    OnPropertyChanged("KinRelationID");
                }
            }
        }

        private string _KinAddress;
        public   string KinAddress
        {
            get { return _KinAddress; }
            set
            {
                if (_KinAddress != value)
                {
                    _KinAddress = value;
                    OnPropertyChanged("KinAddress");
                }
            }
        }

        private string _kinPhone;
        public   string kinPhone
        {
            get { return _kinPhone; }
            set
            {
                if (_kinPhone != value)
                {
                    _kinPhone = value;
                    OnPropertyChanged("kinPhone");
                }
            }
        }

        private string _KinMobile;
        public   string KinMobile
        {
            get { return _KinMobile; }
            set
            {
                if (_KinMobile != value)
                {
                    _KinMobile = value;
                    OnPropertyChanged("KinMobile");
                }
            }
        }

        private long _Doctor1_ID;
        public  long Doctor1_ID
        {
            get { return _Doctor1_ID; }
            set
            {
                if (_Doctor1_ID != value)
                {
                    _Doctor1_ID = value;
                    OnPropertyChanged("Doctor1_ID");
                }
            }
        }

        private long _Doctor2_ID;
        public  long Doctor2_ID
        {
            get { return _Doctor2_ID; }
            set
            {
                if (_Doctor2_ID != value)
                {
                    _Doctor2_ID = value;
                    OnPropertyChanged("Doctor2_ID");
                }
            }
        }

        private string _Remarks;
        public  string Remarks
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
        private string _ProvisionalDiagnosis;
        public string ProvisionalDiagnosis
        {
            get { return _ProvisionalDiagnosis; }
            set
            {
                if (_ProvisionalDiagnosis != value)
                {
                    _ProvisionalDiagnosis = value;
                    OnPropertyChanged("ProvisionalDiagnosis");
                }
            }
        }

        private string _IPDNO;
        public string IPDNO
        {
            get { return _IPDNO; }
            set
            {
                if (_IPDNO != value)
                {
                    _IPDNO = value;
                    OnPropertyChanged("IPDNO");
                }
            }
        }
        private string _IPDAdmissionNO;
        public string IPDAdmissionNO
        {
            get 
            { 
                return _IPDAdmissionNO; 
            }
            set
            {
                if (_IPDAdmissionNO != value)
                {
                    _IPDAdmissionNO = value;
                    OnPropertyChanged("IPDAdmissionNO");
                }
            }
        }
        private bool _IsNonPresence;
        public bool IsNonPresence
        {
            get { return _IsNonPresence; }
            set
            {
                if (_IsNonPresence != value)
                {
                    _IsNonPresence = value;
                    OnPropertyChanged("IsNonPresence");
                }
            }
        }
        private bool _IsDischarge;
        public bool IsDischarge
        {
            get { return _IsDischarge; }
            set
            {
                if (_IsDischarge != value)
                {
                    _IsDischarge = value;
                    OnPropertyChanged("IsDischarge");
                }
            }
        }

        //private DateTime? _DischargeDate = System.DateTime.Now;
        //public DateTime? DischargeDate
        //{
        //    get { return _DischargeDate; }
        //    set
        //    {
        //        if (_DischargeDate != value)
        //        {
        //            _DischargeDate = value;
        //            OnPropertyChanged("DischargeDate");
        //        }
        //    }
        //}

        //private decimal _AdvanceBalance;
        //public decimal AdvanceBalance
        //{
        //    get { return _AdvanceBalance; }
        //    set
        //    {
        //        if (_AdvanceBalance != value)
        //        {
        //            _AdvanceBalance = value;
        //            OnPropertyChanged("AdvanceBalance");
        //        }
        //    }
        //}

        private bool _IsAllPatient;
        public bool IsAllPatient
        {
            get { return _IsAllPatient; }
            set
            {
                if (_IsAllPatient != value)
                {
                    _IsAllPatient = value;
                    OnPropertyChanged("IsAllPatient");
                }
            }
        }

        private bool _IsMedicoLegalCase;
        public bool IsMedicoLegalCase
        {
            get { return _IsMedicoLegalCase; }
            set
            {
                if (_IsMedicoLegalCase != value)
                {
                    _IsMedicoLegalCase = value;
                    OnPropertyChanged("IsMedicoLegalCase");
                }
            }
        }

        private bool _CurrentAdmittedList;
        public bool CurrentAdmittedList
        {
            get { return _CurrentAdmittedList; }
            set
            {
                if (_CurrentAdmittedList != value)
                {
                    _CurrentAdmittedList = value;
                    OnPropertyChanged("CurrentAdmittedList");
                }
            }
        }

        private bool _UnRegisterd;
        public bool UnRegistered
        {
            get { return _UnRegisterd; }
            set { _UnRegisterd = value; }
        }

        private bool _IsBilled;
        public bool IsBilled
        {
            get { return _IsBilled; }
            set { _IsBilled = value; }
        }

       //Added by AJ Date 12/12/2016
        private DateTime? _DischargeDate;
        public DateTime? DischargeDate
        {
            get { return _DischargeDate; }
            set
            {
                if (_DischargeDate != value)
                {
                    _DischargeDate = value;
                    OnPropertyChanged("DischargeDate");
                }
            }
        }

        private long _PatientCategoryID;   //Added by AJ Date 22/2/2017
        public long PatientCategoryID
        {
            get { return _PatientCategoryID; }
            set
            {
                if (_PatientCategoryID != value)
                {
                    _PatientCategoryID = value;
                    OnPropertyChanged("PatientCategoryID");
                }
            }
        }
        //***//--------------------
       //***//--------------------
        #endregion


        #region For Nursing Station 20022017

        private long _AdmUnitID;
        public long AdmUnitID
        {
            get { return _AdmUnitID; }
            set
            {
                if (_AdmUnitID != value)
                {
                    _AdmUnitID = value;
                    OnPropertyChanged("AdmUnitID");
                }
            }
        }

        #endregion

    }

   public class clsIPDAdmMLCDetailsVO : IValueObject, INotifyPropertyChanged
   {
       public string ToXml()
       {
           return this.ToString();
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

       //Added By santosh Patil

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

       private long _AdmID;
       public long AdmID
       {
           get { return _AdmID; }
           set
           {
               if (_AdmID != value)
               {
                   _AdmID = value;
                   OnPropertyChanged("AdmID");
               }
           }
       }

       private long _AdmUnitID;
       public long AdmUnitID
       {
           get { return _AdmUnitID; }
           set
           {
               if (_AdmUnitID != value)
               {
                   _AdmUnitID = value;
                   OnPropertyChanged("AdmUnitID");
               }
           }
       }

       private bool _IsMLC;
       public bool IsMLC
       {
           get { return _IsMLC; }
           set
           {
               if (_IsMLC != value)
               {
                   _IsMLC = value;
                   OnPropertyChanged("IsMLC");
               }
           }
       }

       private string _PoliceStation;
       public string PoliceStation
       {
           get { return _PoliceStation; }
           set
           {
               if (_PoliceStation != value)
               {
                   _PoliceStation = value;
                   OnPropertyChanged("PoliceStation");
               }
           }
       }

       private string _Authority;
       public string Authority
       {
           get { return _Authority; }
           set
           {
               if (_Authority != value)
               {
                   _Authority = value;
                   OnPropertyChanged("Authority");
               }
           }
       }

       private string _Number;
       public string Number
       {
           get { return _Number; }
           set
           {
               if (_Number != value)
               {
                   _Number = value;
                   OnPropertyChanged("Number");
               }
           }
       }

       private string _Address;
       public string Address
       {
           get { return _Address; }
           set
           {
               if (_Address != value)
               {
                   _Address = value;
                   OnPropertyChanged("Address");
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

       private int _OPD_IPD;
       public int OPD_IPD
       {
           get { return _OPD_IPD; }
           set
           {
               if (_OPD_IPD != value)
               {
                   _OPD_IPD = value;
                   OnPropertyChanged("OPD_IPD");
               }
           }
       }
       public string Title { get; set; }
       public string Description { get; set; }
       public string AttachedFileName { get; set; }
       public byte[] AttachedFileContent { get; set; }
   }


   public class clsAddIPDAdviseDischargeListBizActionVO : IBizActionValueObject
   {

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsAddIPDAdviseDischargeListBizAction";
       }

       public string ToXml()
       {
           //throw new NotImplementedException();
           return this.ToString();
       }

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

       public long ID { get; set; }
       public long UnitID { get; set; }
       public long PatientID { get; set; }
       public long PatientUnitID { get; set; }

       private clsIPDAdmissionVO _AddAdviseDetails = null;
       public clsIPDAdmissionVO AddAdviseDetails
       {
           get { return _AddAdviseDetails; }
           set { _AddAdviseDetails = value; }
       }

       private List<MasterListItem> _AddAdviseDList = null;
       public List<MasterListItem> AddAdviseDList
       {
           get { return _AddAdviseDList; }
           set { _AddAdviseDList = value; }
       }

       public string LinkServer { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
   }
   public class clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizActionVO : IBizActionValueObject
   {
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.IPD.clsGetAdvisedDischargeByAdmIdAndAdmUnitIDBizAction";
       }

       public string ToXml()
       {
           //throw new NotImplementedException();
           return this.ToString();
       }

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

       public long ID { get; set; }
       public long UnitID { get; set; }
       public long PatientID { get; set; }
       public long PatientUnitID { get; set; }

       private clsIPDAdmissionVO _GetAdviseDetails = null;
       public clsIPDAdmissionVO GetAdviseDetails
       {
           get { return _GetAdviseDetails; }
           set { _GetAdviseDetails = value; }
       }

       private List<clsIPDAdmissionVO> _GetAdviseDList = null;
       public List<clsIPDAdmissionVO> GetAdviseDList
       {
           get { return _GetAdviseDList; }
           set { _GetAdviseDList = value; }
       }

       public string LinkServer { get; set; }
       public int TotalRows { get; set; }
       public int StartIndex { get; set; }
       public int MaximumRows { get; set; }
       public bool IsPagingEnabled { get; set; }
       public string sortExpression { get; set; }
   }

}
