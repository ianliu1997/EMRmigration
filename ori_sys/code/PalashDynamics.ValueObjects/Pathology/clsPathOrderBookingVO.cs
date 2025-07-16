using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathOrderBookingVO : IValueObject, INotifyPropertyChanged
    {

        #region Property Declaration Section
        public long RadSpecilizationID { get; set; }
        public long PathoSpecilizationID { get; set; }
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

        private List<clsPathOrderBookingDetailVO> _Items = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> Items
        {
            get { return _Items; }
            set
            {

                _Items = value;
                OnPropertyChanged("Items");

            }
        }

        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                if (_OrderNo != value)
                {
                    _OrderNo = value;
                    OnPropertyChanged("OrderNo");
                }
            }
        }

        private DateTime? _OrderDate;
        public DateTime? OrderDate
        {
            get { return _OrderDate; }
            set
            {
                if (_OrderDate != value)
                {
                    _OrderDate = value;
                    OnPropertyChanged("OrderDate");
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

        private bool _SampleType;
        public bool SampleType
        {
            get { return _SampleType; }
            set
            {
                if (_SampleType != value)
                {
                    _SampleType = value;
                    OnPropertyChanged("SampleType");
                }
            }
        }

        private long _Opd_Ipd_External_ID;
        public long Opd_Ipd_External_ID
        {
            get { return _Opd_Ipd_External_ID; }
            set
            {
                if (_Opd_Ipd_External_ID != value)
                {
                    _Opd_Ipd_External_ID = value;
                    OnPropertyChanged("Opd_Ipd_External_ID");
                }
            }
        }
        private long _ResultColor;
        public long ResultColor
        {
            get { return _ResultColor; }
            set
            {
                if (_ResultColor != value)
                {
                    _ResultColor = value;
                    OnPropertyChanged("ResultColor");
                }
            }
        }

        private long _Opd_Ipd_External_UnitID;
        public long Opd_Ipd_External_UnitID
        {
            get { return _Opd_Ipd_External_UnitID; }
            set
            {
                if (_Opd_Ipd_External_UnitID != value)
                {
                    _Opd_Ipd_External_UnitID = value;
                    OnPropertyChanged("Opd_Ipd_External_ID");
                }
            }
        }

        private long? _Opd_Ipd_External;
        public long? Opd_Ipd_External
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

        private bool _IsApproved;
        public bool IsApproved
        {
            get { return _IsApproved; }
            set
            {
                if (_IsApproved != value)
                {
                    _IsApproved = value;
                    OnPropertyChanged("IsApproved");
                }
            }
        }

        private bool _IsOutSourced;
        public bool IsOutSourced
        {
            get { return _IsOutSourced; }
            set
            {
                if (_IsOutSourced != value)
                {
                    _IsOutSourced = value;
                    OnPropertyChanged("IsOutSourced");
                }
            }
        }
        private bool _IsSampleDispatched;
        public bool IsSampleDispatched
        {
            get { return _IsSampleDispatched; }
            set
            {
                if (_IsSampleDispatched != value)
                {
                    _IsSampleDispatched = value;
                    OnPropertyChanged("IsSampleDispatched");
                }
            }
        }

        private string _AgencyChangedImage;
        public string AgencyChangedImage
        {
            get { return _AgencyChangedImage; }
            set
            {
                if (_AgencyChangedImage != value)
                {
                    _AgencyChangedImage = value;
                    OnPropertyChanged("AgencyChangedImage");
                }
            }
        }

        private bool _IsSampleCollected;
        public bool IsSampleCollected
        {
            get { return _IsSampleCollected; }
            set
            {
                if (_IsSampleCollected != value)
                {
                    _IsSampleCollected = value;
                    OnPropertyChanged("IsSampleCollected");
                }
            }
        }

        private string _SampleCollecedImage;
        public string SampleCollecedImage
        {
            get { return _SampleCollecedImage; }
            set
            {
                if (_SampleCollecedImage != value)
                {
                    _SampleCollecedImage = value;
                    OnPropertyChanged("SampleCollecedImage");
                }
            }
        }
        private bool _IsResultEntry;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                if (_IsResultEntry != value)
                {
                    _IsResultEntry = value;
                    OnPropertyChanged("IsResultEntry");
                }
            }
        }


        private bool _IsDelivered;
        public bool IsDelivered
        {
            get { return _IsDelivered; }
            set
            {
                if (_IsDelivered != value)
                {
                    _IsDelivered = value;
                    OnPropertyChanged("IsDelivered");
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



        private bool _IsPrinted;
        public bool IsPrinted
        {
            get { return _IsPrinted; }
            set
            {
                if (_IsPrinted != value)
                {
                    _IsPrinted = value;
                    OnPropertyChanged("IsPrinted");
                }
            }
        }

        private bool _IsOrderGenerated;
        public bool IsOrderGenerated
        {
            get { return _IsOrderGenerated; }
            set
            {
                if (_IsOrderGenerated != value)
                {
                    _IsOrderGenerated = value;
                    OnPropertyChanged("IsOrderGenerated");
                }
            }
        }

        //added by rohini dated 8/3/16
        private bool _IsExternalPatient;
        public bool IsExternalPatient
        {
            get { return _IsExternalPatient; }
            set
            {
                if (_IsExternalPatient != value)
                {
                    _IsExternalPatient = value;
                    OnPropertyChanged("IsExternalPatient");
                }
            }
        }
        private bool _IsResendForNewSample;
        public bool IsResendForNewSample
        {
            get { return _IsResendForNewSample; }
            set
            {
                if (_IsResendForNewSample != value)
                {
                    _IsResendForNewSample = value;
                    OnPropertyChanged("IsResendForNewSample");
                }
            }
        }
        
        //
        private long? _TestType;
        public long? TestType
        {
            get { return _TestType; }
            set
            {
                if (_TestType != value)
                {
                    _TestType = value;
                    OnPropertyChanged("TestType");
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

        private long? _TestProfile;
        public long? TestProfile
        {
            get { return _TestProfile; }
            set
            {
                if (_TestProfile != value)
                {
                    _TestProfile = value;
                    OnPropertyChanged("TestProfile");
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
        private DateTime _SampleDispatchDateTime;
        public DateTime SampleDispatchDateTime
        {
            get { return _SampleDispatchDateTime; }
            set
            {
                if (_SampleDispatchDateTime != value)
                {
                    _SampleDispatchDateTime = value;
                    OnPropertyChanged("SampleDispatchDateTime");
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
        

        //by rohinee for company patient
        private long _PatientCategoryID;
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

        private string strPatientName = "";
        public string PatientName
        {
            get { return strPatientName = _Prefix+" "+_FirstName + " " + _MiddleName + " " + _LastName; }
            set
            {
                if (value != strPatientName)
                {
                    strPatientName = value;
                    OnPropertyChanged("PatientName");
                }
            }
        }



        private double _TestCharges;
        public double TestCharges
        {
            get { return _TestCharges; }
            set
            {
                if (_TestCharges != value)
                {
                    _TestCharges = value;
                    OnPropertyChanged("TestCharges");
                }
            }
        }

        private long? _DoctorID;
        public long? DoctorID
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


        private long _DispatchToID;
        public long DispatchToID
        {
            get { return _DispatchToID; }
            set
            {
                if (_DispatchToID != value)
                {
                    _DispatchToID = value;
                    OnPropertyChanged("DispatchToID");
                }
            }
        }
        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (_ChargeID != value)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }



        private long _TariffServiceID;
        public long TariffServiceID
        {
            get { return _TariffServiceID; }
            set
            {
                if (_TariffServiceID != value)
                {
                    _TariffServiceID = value;
                    OnPropertyChanged("TariffServiceID");
                }
            }
        }

        #  region Pathology Additions

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

        private string _ReferredByEmailID;
        public string ReferredByEmailID
        {
            get { return _ReferredByEmailID; }
            set
            {
                if (_ReferredByEmailID != value)
                {
                    _ReferredByEmailID = value;
                    OnPropertyChanged("ReferredByEmailID");
                }
            }
        }

        private long _ReferredDoctorID;
        public long ReferredDoctorID
        {
            get { return _ReferredDoctorID; }
            set
            {
                if (_ReferredDoctorID != value)
                {
                    _ReferredDoctorID = value;
                    OnPropertyChanged("ReferredDoctorID");
                }
            }
        }

        private DateTime? _RegistrationTime;
        public DateTime? RegistrationTime
        {
            get { return _RegistrationTime; }
            set
            {
                if (_RegistrationTime != value)
                {
                    _RegistrationTime = value;
                    OnPropertyChanged("_RegistrationTime");
                }
            }
        }

        private string _PatientEmailId;
        public string PatientEmailId
        {
            get { return _PatientEmailId; }
            set
            {
                if (_PatientEmailId != value)
                {
                    _PatientEmailId = value;
                    OnPropertyChanged("PatientEmailId");
                }
            }
        }

        //Set to identify whether it is IPD Patient or OPD
        private bool _IsIPDPatient;
        public bool IsIPDPatient
        {
            get { return _IsIPDPatient; }
            set
            {
                if (_IsIPDPatient != value)
                {
                    _IsIPDPatient = value;
                    OnPropertyChanged("IsIPDPatient");
                }
            }
        }

        #endregion

        #endregion

        private long _POBDID;
        public long POBDID
        {
            get { return _POBDID; }
            set
            {
                if (_POBDID != value)
                {
                    _POBDID = value;
                    OnPropertyChanged("POBDID");
                }
            }
        }


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
        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get { return _TotalAmount; }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                }
            }
        }


        private double _CompanyAmount;
        public double CompanyAmount
        {
            get { return _CompanyAmount; }
            set
            {
                if (_CompanyAmount != value)
                {
                    _CompanyAmount = value;
                    OnPropertyChanged("CompanyAmount");
                }
            }
        }

        private double _PatientAmount;
        public double PatientAmount
        {
            get { return _PatientAmount; }
            set
            {
                if (_PatientAmount != value)
                {
                    _PatientAmount = value;
                    OnPropertyChanged("PatientAmount");
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


        private double _Balance;
        public double Balance
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
        private long _TestID;
        public long TestID
        {
            get { return _TestID; }
            set
            {
                if (_TestID != value)
                {
                    _TestID = value;
                    OnPropertyChanged("TestID");
                }
            }
        }
        private string _TestName;
        public string TestName
        {
            get { return _TestName; }
            set
            {
                if (_TestName != value)
                {
                    _TestName = value;
                    OnPropertyChanged("TestName");
                }
            }
        }
        private string _TubeName;
        public string TubeName
        {
            get { return _TubeName; }
            set
            {
                if (_TubeName != value)
                {
                    _TubeName = value;
                    OnPropertyChanged("TubeName");
                }
            }
        }
        private string _TestCode;
        public string TestCode
        {
            get { return _TestCode; }
            set
            {
                if (_TestCode != value)
                {
                    _TestCode = value;
                    OnPropertyChanged("TestCode");
                }
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code != value)
                {
                    _Code = value;
                    OnPropertyChanged("Code");
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

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

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

        private long _ServiceID;
        public long ServiceID
        {
            get { return _ServiceID; }
            set
            {
                if (_ServiceID != value)
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
                if (_ServiceName != value)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
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

        #endregion

        #region Newly added properties

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

        private long _PrefixID;
        public long PrefixID
        {
            get { return _PrefixID; }
            set
            {
                if (_PrefixID != value)
                {
                    _PrefixID = value;
                    OnPropertyChanged("PrefixID");
                }
            }
        }

        private long _AgeInDays;
        public long AgeInDays
        {
            get { return _AgeInDays; }
            set
            {
                if (_AgeInDays != value)
                {
                    _AgeInDays = value;
                    OnPropertyChanged("AgeInDays");
                }
            }
        }

        private string _Prefix ;
        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                if (_Prefix != value)
                {
                    _Prefix = value;
                    OnPropertyChanged("_Prefix");
                }
            }
        }

        private string _SampleNumber;
        public string SampleNumber
        {
            get { return _SampleNumber; }
            set
            {
                if (_SampleNumber != value)
                {
                    _SampleNumber = value;
                    OnPropertyChanged(" SampleNumber");
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
