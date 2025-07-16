using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAppConfigVO : IValueObject, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
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
            throw new NotImplementedException();
        }

        #endregion

        public long ClientID { get; set; }
        public DateTime? SubsciptionEndDate { get; set; }

        public DateTime? FollowUpDate { get; set; } //***//

        private long _CountryID;
        public long CountryID
        {
            get
            {
                return _CountryID;
            }

            set
            {
                if (value != _CountryID)
                {
                    _CountryID = value;
                    OnPropertyChanged("CountryID");
                }
            }
        }
        private long _StateID;
        public long StateID
        {
            get
            {
                return _StateID;
            }

            set
            {
                if (value != _StateID)
                {
                    _StateID = value;
                    OnPropertyChanged("StateID");
                }
            }
        }
        private long _CityID;
        public long CityID
        {
            get
            {
                return _CityID;
            }

            set
            {
                if (value != _CityID)
                {
                    _CityID = value;
                    OnPropertyChanged("CityID");
                }
            }
        }
        private long _RegionID;
        public long RegionID
        {
            get
            {
                return _RegionID;
            }

            set
            {
                if (value != _RegionID)
                {
                    _RegionID = value;
                    OnPropertyChanged("RegionID");
                }
            }
        }

        //added by neena
        private bool _IsDonorCycle;
        public bool IsDonorCycle
        {
            get
            {
                return _IsDonorCycle;
            }

            set
            {
                if (value != _IsDonorCycle)
                {
                    _IsDonorCycle = value;
                    OnPropertyChanged("IsDonorCycle");
                }
            }
        }


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

        private bool _IsSellBySellingUnit;
        public bool IsSellBySellingUnit
        {
            get
            {
                return _IsSellBySellingUnit;
            }

            set
            {
                if (value != _IsSellBySellingUnit)
                {
                    _IsSellBySellingUnit = value;
                    OnPropertyChanged("IsSellBySellingUnit");
                }
            }
        }

        private clsAutoEmailConfigVO _EmailConfig = new clsAutoEmailConfigVO();
        public clsAutoEmailConfigVO EmailConfig
        {
            get
            {
                return _EmailConfig;
            }

            set
            {

                _EmailConfig = value;
                OnPropertyChanged("EmailConfig");

            }
        }

        //rohinee for set cash counter from app config
        private long _PathologyVisitTypeID;
        public long PathologyVisitTypeID
        {
            get
            {
                return _PathologyVisitTypeID;
            }

            set
            {
                if (value != _PathologyVisitTypeID)
                {
                    _PathologyVisitTypeID = value;
                    OnPropertyChanged("PathologyVisitTypeID");
                }
            }
        }

        private long _LabCounterID;
        public long LabCounterID
        {
            get
            {
                return _LabCounterID;
            }

            set
            {
                if (value != _LabCounterID)
                {
                    _LabCounterID = value;
                    OnPropertyChanged("LabCounterID");
                }
            }
        }

        private long _IPDCounterID;
        public long IPDCounterID
        {
            get
            {
                return _IPDCounterID;
            }

            set
            {
                if (value != _IPDCounterID)
                {
                    _IPDCounterID = value;
                    OnPropertyChanged("IPDCounterID");
                }
            }
        }
        private long _OPDCounterID;
        public long OPDCounterID
        {
            get
            {
                return _OPDCounterID;
            }

            set
            {
                if (value != _OPDCounterID)
                {
                    _OPDCounterID = value;
                    OnPropertyChanged("OPDCounterID");
                }
            }
        }
        private long _PharmacyCounterID;
        public long PharmacyCounterID
        {
            get
            {
                return _PharmacyCounterID;
            }

            set
            {
                if (value != _PharmacyCounterID)
                {
                    _PharmacyCounterID = value;
                    OnPropertyChanged("PharmacyCounterID");
                }
            }
        }
        private long _RadiologyCounterID;
        public long RadiologyCounterID
        {
            get
            {
                return _RadiologyCounterID;
            }

            set
            {
                if (value != _RadiologyCounterID)
                {
                    _RadiologyCounterID = value;
                    OnPropertyChanged("RadiologyCounterID");
                }
            }
        }

        private bool _IsCounterLogin;
        public bool IsCounterLogin
        {
            get
            {
                return _IsCounterLogin;
            }

            set
            {
                if (value != _IsCounterLogin)
                {
                    _IsCounterLogin = value;
                    OnPropertyChanged("IsCounterLogin");
                }
            }
        }
        private bool _IsProcessingUnit;
        public bool IsProcessingUnit
        {
            get
            {
                return _IsProcessingUnit;
            }

            set
            {
                if (value != _IsProcessingUnit)
                {
                    _IsProcessingUnit = value;
                    OnPropertyChanged("IsProcessingUnit");
                }
            }
        }
        //...............
        private clsAppAccountsConfigVO _Accounts = new clsAppAccountsConfigVO();
        public clsAppAccountsConfigVO Accounts
        {
            get
            {
                return _Accounts;
            }

            set
            {

                _Accounts = value;
                OnPropertyChanged("Accounts");

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

        private DateTime _ApplicationDateTime;
        public DateTime ApplicationDateTime
        {
            get { return _ApplicationDateTime; }
            set
            {
                if (_ApplicationDateTime != value)
                {
                    _ApplicationDateTime = value;
                    OnPropertyChanged("ApplicationDateTime");
                }
            }
        }

        private bool _IsAllowDischargeRequest;
        public bool IsAllowDischargeRequest
        {
            get { return _IsAllowDischargeRequest; }
            set
            {
                if (value != _IsAllowDischargeRequest)
                {
                    _IsAllowDischargeRequest = value;
                    OnPropertyChanged("IsAllowDischargeRequest");
                }
            }
        }

        private string _DatabaseName;
        public string DatabaseName
        {
            get
            {
                return _DatabaseName;
            }

            set
            {
                if (value != _DatabaseName)
                {
                    _DatabaseName = value;
                    OnPropertyChanged("DatabaseName");
                }
            }
        }

        //by rohini AS PER DISSCUSSED WITH MANGESH
        private string _Disclaimer;
        public string Disclaimer
        {
            get
            {
                return _Disclaimer;
            }

            set
            {
                if (value != _Disclaimer)
                {
                    _Disclaimer = value;
                    OnPropertyChanged("Disclaimer");
                }
            }
        }
        //

        private int _RoundingDigit;
        public int RoundingDigit
        {
            get
            {
                return _RoundingDigit;
            }

            set
            {
                if (value != _RoundingDigit)
                {
                    _RoundingDigit = value;
                    OnPropertyChanged("RoundingDigit");
                }
            }
        }
        private long _CurrentVisit;
        public long CurrentVisit
        {
            get { return _CurrentVisit; }
            set
            {
                if (value != _CurrentVisit)
                {
                    _CurrentVisit = value;
                    OnPropertyChanged("CurrentVisit");
                }
            }
        }

        private string _RoundupDigitString;
        public string RoundupDigitString
        {
            get
            {
                return _RoundupDigitString;
            }

            set
            {
                if (value != _RoundupDigitString)
                {
                    _RoundupDigitString = value;
                    OnPropertyChanged("RoundupDigitString");
                }
            }
        }

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (value != _FileName)
                {
                    _FileName = value;
                    OnPropertyChanged("LocalLanguageFileName");
                }
            }
        }

        private string _FilePath;
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                if (value != _FilePath)
                {
                    _FilePath = value;
                    OnPropertyChanged("LocalLanguageFilePath");
                }
            }
        }
        private string _UnitName;
        public string UnitName
        {
            get
            {
                return _UnitName;
            }

            set
            {
                if (value != _UnitName)
                {
                    _UnitName = value;
                    OnPropertyChanged("UnitName");
                }
            }
        }

        private bool _IsConfigured;
        public bool IsConfigured
        {
            get
            {
                return _IsConfigured;
            }

            set
            {
                if (value != _IsConfigured)
                {
                    _IsConfigured = value;
                    OnPropertyChanged("IsConfigured");
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
                    OnPropertyChanged("UnitID");
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
                    OnPropertyChanged("DepartmentID");
                }
            }
        }

        private long _PatientCategoryID;
        public long PatientCategoryID
        {
            get
            {
                return _PatientCategoryID;
            }

            set
            {
                if (value != _PatientCategoryID)
                {
                    _PatientCategoryID = value;
                    OnPropertyChanged("PatientCategoryID");
                }
            }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get
            {
                return _CategoryID;
            }

            set
            {
                if (value != _CategoryID)
                {
                    _CategoryID = value;
                    OnPropertyChanged("CategoryID");
                }
            }
        }

        private long _PharmacyPatientCategoryID;
        public long PharmacyPatientCategoryID
        {
            get
            {
                return _PharmacyPatientCategoryID;
            }

            set
            {
                if (value != _PharmacyPatientCategoryID)
                {
                    _PharmacyPatientCategoryID = value;
                    OnPropertyChanged("PharmacyPatientCategoryID");
                }
            }
        }
        private string _Country;
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                if (value != _Country)
                {
                    _Country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string _State;
        public string State
        {
            get
            {
                return _State;
            }

            set
            {
                if (value != _State)
                {
                    _State = value;
                    OnPropertyChanged("State");
                }
            }
        }

        private string _District;
        public string District
        {
            get
            {
                return _District;
            }

            set
            {
                if (value != _District)
                {
                    _District = value;
                    OnPropertyChanged("District");
                }
            }
        }

        public long ItemExpiredIndays { get; set; }  //By Umesh for inventry
        public string DefaultCountryCode { get; set; }  //By Umesh for Countersale
        private string _Pincode;
        public string Pincode
        {
            get
            {
                return _Pincode;
            }

            set
            {
                if (value != _Pincode)
                {
                    _Pincode = value;
                    OnPropertyChanged("Pincode");
                }
            }
        }

        private string _WebSite = "";
        public string WebSite
        {
            get { return _WebSite; }
            set
            {
                if (_WebSite != value)
                {
                    _WebSite = value;
                    OnPropertyChanged("WebSite");
                }
            }
        }

        private bool _IsPhotoMoveToServer;
        public bool IsPhotoMoveToServer
        {
            get
            {
                return _IsPhotoMoveToServer;
            }

            set
            {
                if (value != _IsPhotoMoveToServer)
                {
                    _IsPhotoMoveToServer = value;
                    OnPropertyChanged("IsPhotoMoveToServer");
                }
            }
        }

        //added by neena
        private bool _IsIVFBillingCriteria;
        public bool IsIVFBillingCriteria
        {
            get
            {
                return _IsIVFBillingCriteria;
            }

            set
            {
                if (value != _IsIVFBillingCriteria)
                {
                    _IsIVFBillingCriteria = value;
                    OnPropertyChanged("IsIVFBillingCriteria");
                }
            }
        }

        private string _ForIVFProcedureBilling;
        public string ForIVFProcedureBilling
        {
            get
            {
                return _ForIVFProcedureBilling;
            }

            set
            {
                if (value != _ForIVFProcedureBilling)
                {
                    _ForIVFProcedureBilling = value;
                    OnPropertyChanged("ForIVFProcedureBilling");
                }
            }
        }

        private string _ForTriggerProcedureBilling;
        public string ForTriggerProcedureBilling
        {
            get
            {
                return _ForTriggerProcedureBilling;
            }

            set
            {
                if (value != _ForTriggerProcedureBilling)
                {
                    _ForTriggerProcedureBilling = value;
                    OnPropertyChanged("ForTriggerProcedureBilling");
                }
            }
        }

        private string _ForOPUProcedureBilling;
        public string ForOPUProcedureBilling
        {
            get
            {
                return _ForOPUProcedureBilling;
            }

            set
            {
                if (value != _ForOPUProcedureBilling)
                {
                    _ForOPUProcedureBilling = value;
                    OnPropertyChanged("ForOPUProcedureBilling");
                }
            }
        }
        //



        private bool _IsConcessionReadOnly;  //***//
        public bool IsConcessionReadOnly
        {
            get
            {
                return _IsConcessionReadOnly;
            }

            set
            {
                if (value != _IsConcessionReadOnly)
                {
                    _IsConcessionReadOnly = value;
                    OnPropertyChanged("IsConcessionReadOnly");
                }
            }
        }

        private bool _IsFertilityPoint;//***//
        public bool IsFertilityPoint
        {
            get
            {
                return _IsFertilityPoint;

            }
            set
            {
                if (value != _IsFertilityPoint)
                {
                    _IsFertilityPoint = value;
                    OnPropertyChanged("IsFertilityPoint");
                }
            }
        }

        private bool _ValidationsFlag; //Added by NileshD on 19April2019
        public bool ValidationsFlag
        {
            get
            {
                return _ValidationsFlag;
            }

            set
            {
                if (value != _ValidationsFlag)
                {
                    _ValidationsFlag = value;
                    OnPropertyChanged("ValidationsFlag");
                }
            }
        }

        private long _InternationalId; //Added by NileshD on 19April2019
        public long InternationalId
        {
            get
            {
                return _InternationalId;
            }

            set
            {
                if (value != _InternationalId)
                {
                    _InternationalId = value;
                    OnPropertyChanged("InternationalId");
                }
            }
        }

        private string _City;
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                if (value != _City)
                {
                    _City = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _Area;
        public string Area
        {
            get
            {
                return _Area;
            }

            set
            {
                if (value != _Area)
                {
                    _Area = value;
                    OnPropertyChanged("Area");
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

        private long _PatientSourceID;
        public long PatientSourceID
        {
            get
            {
                return _PatientSourceID;
            }

            set
            {
                if (value != _PatientSourceID)
                {
                    _PatientSourceID = value;
                    OnPropertyChanged("PatientSourceID");
                }
            }
        }

        private long _EmbryologistID;
        public long EmbryologistID
        {
            get
            {
                return _EmbryologistID;
            }

            set
            {
                if (value != _EmbryologistID)
                {
                    _EmbryologistID = value;
                    OnPropertyChanged("EmbryologistID");
                }
            }
        }


        private long _AnesthetistID;
        public long AnesthetistID
        {
            get
            {
                return _AnesthetistID;
            }

            set
            {
                if (value != _AnesthetistID)
                {
                    _AnesthetistID = value;
                    OnPropertyChanged("AnesthetistID");
                }
            }
        }

        private double _AppointmentSlot;
        public double AppointmentSlot
        {
            get
            {
                return _AppointmentSlot;
            }

            set
            {
                if (value != _AppointmentSlot)
                {
                    _AppointmentSlot = value;
                    OnPropertyChanged("AppointmentSlot");
                }
            }
        }


        // Added by Saily P
        private long _RadiologistID;
        public long RadiologistID
        {
            get
            {
                return _RadiologistID;
            }

            set
            {
                if (value != _RadiologistID)
                {
                    _RadiologistID = value;
                    OnPropertyChanged("RadiologistID");
                }
            }
        }

        private long _GynecologistID;
        public long GynecologistID
        {
            get
            {
                return _GynecologistID;
            }

            set
            {
                if (value != _GynecologistID)
                {
                    _GynecologistID = value;
                    OnPropertyChanged("GynecologistID");
                }
            }
        }

        /*
         *  Added for creating Pathologist in the application developed for MFC
         *  Added  on  25.05.2016
         */


        private long _PathologistID;
        public long PathologistID
        {
            get
            {
                return _PathologistID;
            }

            set
            {
                if (value != _PathologistID)
                {
                    _PathologistID = value;
                    OnPropertyChanged("PathologistID");
                }
            }
        }

        //end 




        private long _PhysicianID;
        public long PhysicianID
        {
            get
            {
                return _PhysicianID;
            }

            set
            {
                if (value != _PhysicianID)
                {
                    _PhysicianID = value;
                    OnPropertyChanged("PhysicianID");
                }
            }
        }

        private long _AndrologistID;
        public long AndrologistID
        {
            get
            {
                return _AndrologistID;
            }

            set
            {
                if (value != _AndrologistID)
                {
                    _AndrologistID = value;
                    OnPropertyChanged("AndrologistID");
                }
            }
        }
        private long _InhouseLabID;
        public long InhouseLabID
        {
            get
            {
                return _InhouseLabID;
            }

            set
            {
                if (value != _InhouseLabID)
                {
                    _InhouseLabID = value;
                    OnPropertyChanged("InhouseLabID");
                }
            }
        }
        private long _OocyteDonationID;
        public long OocyteDonationID
        {
            get
            {
                return _OocyteDonationID;
            }

            set
            {
                if (value != _OocyteDonationID)
                {
                    _OocyteDonationID = value;
                    OnPropertyChanged("OocyteDonationID");
                }
            }
        }
        private long _EmbryoReceipentID;
        public long EmbryoReceipentID
        {
            get
            {
                return _EmbryoReceipentID;
            }

            set
            {
                if (value != _EmbryoReceipentID)
                {
                    _EmbryoReceipentID = value;
                    OnPropertyChanged("EmbryoReceipentID");
                }
            }
        }
        private long _DoctorTypeForReferral;
        public long DoctorTypeForReferral
        {
            get
            {
                return _DoctorTypeForReferral;
            }

            set
            {
                if (value != _DoctorTypeForReferral)
                {
                    _DoctorTypeForReferral = value;
                    OnPropertyChanged("DoctorTypeForReferral");
                }
            }
        }

        private long _IdentityForInternationalPatient;
        public long IdentityForInternationalPatient
        {
            get
            {
                return _IdentityForInternationalPatient;
            }

            set
            {
                if (value != _IdentityForInternationalPatient)
                {
                    _IdentityForInternationalPatient = value;
                    OnPropertyChanged("IdentityForInternationalPatient");
                }
            }
        }
        private long _OocyteReceipentID;
        public long OocyteReceipentID
        {
            get
            {
                return _OocyteReceipentID;
            }

            set
            {
                if (value != _OocyteReceipentID)
                {
                    _OocyteReceipentID = value;
                    OnPropertyChanged("OocyteReceipentID");
                }
            }
        }
        private long _BiologistID;
        public long BiologistID
        {
            get
            {
                return _BiologistID;
            }

            set
            {
                if (value != _BiologistID)
                {
                    _BiologistID = value;
                    OnPropertyChanged("BiologistID");
                }
            }
        }
        private long _AuthorizationLevelForRefundID;
        public long AuthorizationLevelForRefundID
        {
            get
            {
                return _AuthorizationLevelForRefundID;
            }

            set
            {
                if (value != _AuthorizationLevelForRefundID)
                {
                    _AuthorizationLevelForRefundID = value;
                    OnPropertyChanged("AuthorizationLevelForRefundID");
                }
            }
        }
        private long _AuthorizationLevelForConcessionID;
        public long AuthorizationLevelForConcessionID
        {
            get
            {
                return _AuthorizationLevelForConcessionID;
            }

            set
            {
                if (value != _AuthorizationLevelForConcessionID)
                {
                    _AuthorizationLevelForConcessionID = value;
                    OnPropertyChanged("AuthorizationLevelForConcessionID");
                }
            }
        }

        private long _AuthorizationLevelForMRPAdjustmentID;
        public long AuthorizationLevelForMRPAdjustmentID
        {
            get
            {
                return _AuthorizationLevelForMRPAdjustmentID;
            }

            set
            {
                if (value != _AuthorizationLevelForMRPAdjustmentID)
                {
                    _AuthorizationLevelForMRPAdjustmentID = value;
                    OnPropertyChanged("AuthorizationLevelForMRPAdjustmentID");
                }
            }
        }

        //Added By Bhushanp 31052017
        private long _AuthorizationLevelForPatientAdvRefundID;
        public long AuthorizationLevelForPatientAdvRefundID
        {
            get
            {
                return _AuthorizationLevelForPatientAdvRefundID;
            }

            set
            {
                if (value != _AuthorizationLevelForPatientAdvRefundID)
                {
                    _AuthorizationLevelForPatientAdvRefundID = value;
                    OnPropertyChanged("AuthorizationLevelForPatientAdvRefundID");
                }
            }
        }

        private long _DateFormatID;
        public long DateFormatID
        {
            get
            {
                return _DateFormatID;
            }

            set
            {
                if (value != _DateFormatID)
                {
                    _DateFormatID = value;
                    OnPropertyChanged("DateFormatID");
                }
            }
        }

        private string _DateFormat;
        public string DateFormat
        {
            get
            {
                return _DateFormat;
            }

            set
            {
                if (value != _DateFormat)
                {
                    _DateFormat = value;
                    OnPropertyChanged("DateFormat");
                }
            }
        }
        //

        private bool _IsHO;
        public bool IsHO
        {
            get
            {
                return _IsHO;
            }

            set
            {
                if (value != _IsHO)
                {
                    _IsHO = value;
                    OnPropertyChanged("IsHO");
                }
            }
        }

        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                if (value != _Email)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        //Begin - added for Inventory Merging

        private long _PharmacyStoreID;
        public long PharmacyStoreID
        {
            get
            {
                return _PharmacyStoreID;
            }

            set
            {
                if (value != _PharmacyStoreID)
                {
                    _PharmacyStoreID = value;
                    OnPropertyChanged("PharmacyStoreID");
                }
            }
        }

        private long _InventoryCatagoryID;
        public long InventoryCatagoryID
        {
            get
            {
                return _InventoryCatagoryID;
            }

            set
            {
                if (value != _InventoryCatagoryID)
                {
                    _InventoryCatagoryID = value;
                    OnPropertyChanged("InventoryCatagoryID");
                }
            }
        }
        //ROHINEE FOR PATIENT SOURCE CATEGORY MASTER
        private Int64 _PharmacyExpiryDuration;
        public Int64 PharmacyExpiryDuration
        {
            get
            {
                return _PharmacyExpiryDuration;
            }

            set
            {
                if (value != _PharmacyExpiryDuration)
                {
                    _PharmacyExpiryDuration = value;
                    OnPropertyChanged("PharmacyExpiryDuration");
                }
            }
        }



        private long _CorporatePatientCategoryL1Id;
        public long Corporate_PatientCategoryL1Id
        {
            get
            {
                return _CorporatePatientCategoryL1Id;
            }

            set
            {
                if (value != _CorporatePatientCategoryL1Id)
                {
                    _CorporatePatientCategoryL1Id = value;
                    OnPropertyChanged("Corporate_PatientCategoryL1Id");
                }
            }
        }

     
        //NOTE : asume other patient Category Level 2 are belongs to packages except walkin package category L2 
        private long _PatientCategoryL2Id_WalkIn;
        public long PatientCategoryL2Id_WalkIn
        {
            get
            {
                return _PatientCategoryL2Id_WalkIn;
            }

            set
            {
                if (value != _PatientCategoryL2Id_WalkIn)
                {
                    _PatientCategoryL2Id_WalkIn = value;
                    OnPropertyChanged("PatientCategoryL2Id_WalkIn");
                }
            }
        }

        //End - added for Inventory Merging

        private long _NurseRoleID;
        public long NurseRoleID
        {
            get
            {
                return _NurseRoleID;
            }

            set
            {
                if (value != _NurseRoleID)
                {
                    _NurseRoleID = value;
                    OnPropertyChanged("NurseRoleID");
                }
            }
        }
        private long _PrintFormatID;
        public long PrintFormatID
        {
            get
            {
                return _PrintFormatID;
            }

            set
            {
                if (value != _PrintFormatID)
                {
                    _PrintFormatID = value;
                    OnPropertyChanged("PrintFormatID");
                }
            }
        }

        private long _DoctorRoleID;
        public long DoctorRoleID
        {
            get
            {
                return _DoctorRoleID;
            }

            set
            {
                if (value != _DoctorRoleID)
                {
                    _DoctorRoleID = value;
                    OnPropertyChanged("DoctorRoleID");
                }
            }
        }

        private long _AdminRoleID;
        public long AdminRoleID
        {
            get
            {
                return _AdminRoleID;
            }

            set
            {
                if (value != _AdminRoleID)
                {
                    _AdminRoleID = value;
                    OnPropertyChanged("AdminRoleID");
                }
            }
        }

        private bool _ConftnMsgForAdd;
        public bool ConftnMsgForAdd
        {
            get
            {
                return _ConftnMsgForAdd;
            }

            set
            {
                if (value != _ConftnMsgForAdd)
                {
                    _ConftnMsgForAdd = value;
                    OnPropertyChanged("ConftnMsgForAdd");
                }
            }
        }

        private Int16? _SearchPatientsInterval;
        public Int16? SearchPatientsInterval
        {
            get
            {
                return _SearchPatientsInterval;
            }

            set
            {
                if (value != _SearchPatientsInterval)
                {
                    _SearchPatientsInterval = value;
                    OnPropertyChanged("SearchPatientsInterval");
                }
            }
        }

        private long _PaymentModeID;
        public long PaymentModeID
        {
            get
            {
                return _PaymentModeID;
            }

            set
            {
                if (value != _PaymentModeID)
                {
                    _PaymentModeID = value;
                    OnPropertyChanged("PaymentModeID");
                }
            }
        }


        private double _RefundAmount;
        public double RefundAmount
        {
            get
            {
                return _RefundAmount;
            }

            set
            {
                if (value != _RefundAmount)
                {
                    _RefundAmount = value;
                    OnPropertyChanged("RefundAmount");
                }
            }
        }



        /// //***// <Added by Ajit ,6/92016  Billing Exceeds Limit>

        private double _BillingExceedsLimit;
        public double BillingExceedsLimit
        {
            get
            {
                return _BillingExceedsLimit;
            }

            set
            {
                if (value != _BillingExceedsLimit)
                {
                    _BillingExceedsLimit = value;
                    OnPropertyChanged("BillingExceedsLimit");
                }
            }
        }
   /// </summary>

        private long _MaritalStatus;
        public long MaritalStatus
        {
            get
            {
                return _MaritalStatus;
            }

            set
            {
                if (value != _MaritalStatus)
                {
                    _MaritalStatus = value;
                    OnPropertyChanged("MaritalStatus");
                }
            }
        }
        private long _AddressTypeID;
        public long AddressTypeID
        {
            get
            {
                return _AddressTypeID;
            }

            set
            {
                if (value != _AddressTypeID)
                {
                    _AddressTypeID = value;
                    OnPropertyChanged("AddressTypeID");
                }
            }
        }

        private long _PathoSpecializationID;
        public long PathoSpecializationID
        {
            get
            {
                return _PathoSpecializationID;
            }

            set
            {
                if (value != _PathoSpecializationID)
                {
                    _PathoSpecializationID = value;
                    OnPropertyChanged("PathoSpecializationID");
                }
            }
        }

        private long _RadiologySpecializationID;
        public long RadiologySpecializationID
        {
            get
            {
                return _RadiologySpecializationID;
            }

            set
            {
                if (value != _RadiologySpecializationID)
                {
                    _RadiologySpecializationID = value;
                    OnPropertyChanged("RadiologySpecializationID");
                }
            }
        }

        private long _PharmacySpecializationID;
        public long PharmacySpecializationID
        {
            get
            {
                return _PharmacySpecializationID;
            }

            set
            {
                if (value != _PharmacySpecializationID)
                {
                    _PharmacySpecializationID = value;
                    OnPropertyChanged("PharmacySpecializationID");
                }
            }
        }

        private long _ConsultationID;
        public long ConsultationID
        {
            get
            {
                return _ConsultationID;
            }

            set
            {
                if (value != _ConsultationID)
                {
                    _ConsultationID = value;
                    OnPropertyChanged("ConsultationID");
                }
            }
        }

        private long _PathologyCompanyTypeID;
        public long PathologyCompanyTypeID
        {
            get
            {
                return _PathologyCompanyTypeID;
            }

            set
            {
                if (value != _PathologyCompanyTypeID)
                {
                    _PathologyCompanyTypeID = value;
                    OnPropertyChanged("PathologyCompanyTypeID");
                }
            }
        }

        private long _SelfCompanyID;
        public long SelfCompanyID
        {
            get
            {
                return _SelfCompanyID;
            }

            set
            {
                if (value != _SelfCompanyID)
                {
                    _SelfCompanyID = value;
                    OnPropertyChanged("SelfCompanyID");
                }
            }
        }

        private long _CompanyPatientSourceID;
        public long CompanyPatientSourceID
        {
            get
            {
                return _CompanyPatientSourceID;
            }

            set
            {
                if (value != _CompanyPatientSourceID)
                {
                    _CompanyPatientSourceID = value;
                    OnPropertyChanged("CompanyPatientSourceID");
                }
            }
        }

        private long _TariffID;
        public long TariffID
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

        //private long _RelationID;
        //public long RelationID
        //{
        //    get
        //    {
        //        return _RelationID;
        //    }

        //    set
        //    {
        //        if (value != _RelationID)
        //        {
        //            _RelationID = value;
        //            OnPropertyChanged("RelationID");
        //        }
        //    }
        //}

        private Int16 _FreeFollowupDays;
        public Int16 FreeFollowupDays
        {
            get
            {
                return _FreeFollowupDays;
            }

            set
            {
                if (value != _FreeFollowupDays)
                {
                    _FreeFollowupDays = value;
                    OnPropertyChanged("FreeFollowupDays");
                }
            }
        }

        private long _FreeFollowupVisitTypeID;
        public long FreeFollowupVisitTypeID
        {
            get
            {
                return _FreeFollowupVisitTypeID;
            }

            set
            {
                if (value != _FreeFollowupVisitTypeID)
                {
                    _FreeFollowupVisitTypeID = value;
                    OnPropertyChanged("FreeFollowupVisitTypeID");
                }
            }
        }

        private long _PharmacyVisitTypeID;
        public long PharmacyVisitTypeID
        {
            get
            {
                return _PharmacyVisitTypeID;
            }

            set
            {
                if (value != _PharmacyVisitTypeID)
                {
                    _PharmacyVisitTypeID = value;
                    OnPropertyChanged("PharmacyVisitTypeID");
                }
            }
        }

        private long _SelfRelationID;
        public long SelfRelationID
        {
            get
            {
                return _SelfRelationID;
            }

            set
            {
                if (value != _SelfRelationID)
                {
                    _SelfRelationID = value;
                    OnPropertyChanged("SelfRelationID");
                }
            }
        }

        private long _AppConfirmation;
        public long AppConfirmation
        {
            get
            {
                return _AppConfirmation;
            }

            set
            {
                if (value != _AppConfirmation)
                {
                    _AppConfirmation = value;
                    OnPropertyChanged("AppConfirmation");
                }
            }
        }

        private long _AutoUserPassword;
        public long AutoUserPassword
        {
            get
            {
                return _AutoUserPassword;
            }

            set
            {
                if (value != _AutoUserPassword)
                {
                    _AutoUserPassword = value;
                    OnPropertyChanged("AutoUserPassword");
                }
            }
        }

        private long _CampInfo;
        public long CampInfo
        {
            get
            {
                return _CampInfo;
            }

            set
            {
                if (value != _CampInfo)
                {
                    _CampInfo = value;
                    OnPropertyChanged("CampInfo");
                }
            }
        }

        private long _AppCancellation;
        public long AppCancellation
        {
            get
            {
                return _AppCancellation;
            }

            set
            {
                if (value != _AppCancellation)
                {
                    _AppCancellation = value;
                    OnPropertyChanged("AppCancellation");
                }
            }
        }

        private bool _ApplyConcessionToStaff;
        public bool ApplyConcessionToStaff
        {
            get
            {
                return _ApplyConcessionToStaff;
            }

            set
            {
                if (value != _ApplyConcessionToStaff)
                {
                    _ApplyConcessionToStaff = value;
                    OnPropertyChanged("ApplyConcessionToStaff");
                }
            }
        }


        private bool _AllowClinicalTransaction;
        public bool AllowClinicalTransaction
        {
            get
            {
                return _AllowClinicalTransaction;
            }

            set
            {
                if (value != _AllowClinicalTransaction)
                {
                    _AllowClinicalTransaction = value;
                    OnPropertyChanged("AllowClinicalTransaction");
                }
            }
        }


        private long _RadiologyStoreID;
        public long RadiologyStoreID
        {
            get
            {
                return _RadiologyStoreID;
            }

            set
            {
                if (value != _RadiologyStoreID)
                {
                    _RadiologyStoreID = value;
                    OnPropertyChanged("RadiologyStoreID");
                }
            }
        }

        private long _RefundPayModeID;
        public long RefundPayModeID
        {
            get
            {
                return _RefundPayModeID;
            }

            set
            {
                if (value != _RefundPayModeID)
                {
                    _RefundPayModeID = value;
                    OnPropertyChanged("RefundPayModeID");
                }
            }
        }

        private long _PathologyStoreID;
        public long PathologyStoreID
        {
            get
            {
                return _PathologyStoreID;
            }

            set
            {
                if (value != _PathologyStoreID)
                {
                    _PathologyStoreID = value;
                    OnPropertyChanged("PathologyStoreID");
                }
            }
        }

        private long _OTStoreID;
        public long OTStoreID
        {
            get
            {
                return _OTStoreID;
            }

            set
            {
                if (value != _OTStoreID)
                {
                    _OTStoreID = value;
                    OnPropertyChanged("OTStoreID");
                }
            }
        }

        private long _OTSlot;
        public long OTSlot
        {
            get
            {
                return _OTSlot;
            }

            set
            {
                if (value != _OTSlot)
                {
                    _OTSlot = value;
                    OnPropertyChanged("OTSlot");
                }
            }
        }

        private bool _AutoDeductStockFromRadiology;
        public bool AutoDeductStockFromRadiology
        {
            get
            {
                return _AutoDeductStockFromRadiology;

            }
            set
            {
                if (value != _AutoDeductStockFromRadiology)
                {
                    _AutoDeductStockFromRadiology = value;
                    OnPropertyChanged("AutoDeductStockFromRadiology");
                }
            }
        }

        private bool _AutoDeductStockFromPathology;
        public bool AutoDeductStockFromPathology
        {
            get
            {
                return _AutoDeductStockFromPathology;

            }
            set
            {
                if (value != _AutoDeductStockFromPathology)
                {
                    _AutoDeductStockFromPathology = value;
                    OnPropertyChanged("AutoDeductStockFromPathology");
                }
            }
        }

        private bool _AutoGenerateSampleNo;
        public bool AutoGenerateSampleNo
        {
            get
            {
                return _AutoGenerateSampleNo;

            }
            set
            {
                if (value != _AutoGenerateSampleNo)
                {
                    _AutoGenerateSampleNo = value;
                    OnPropertyChanged("AutoGenerateSampleNo");
                }
            }
        }

        private bool _AddLogoToAllReports;
        public bool AddLogoToAllReports
        {
            get
            {
                return _AddLogoToAllReports;

            }
            set
            {
                if (value != _AddLogoToAllReports)
                {
                    _AddLogoToAllReports = value;
                    OnPropertyChanged("AddLogoToAllReports");
                }
            }
        }


        // BY BHUSHAN . . . .  for ESA 
        public byte[] Attachment { get; set; }

        public string AttachmentFileName { get; set; }

        private string _host;
        public string Host
        {
            get
            {
                return _host;
            }

            set
            {
                if (value != _host)
                {
                    _host = value;
                    OnPropertyChanged("Host");
                }
            }
        }

        private bool _EnableSsl;
        public bool EnableSsl
        {
            get
            {
                return _EnableSsl;
            }

            set
            {
                if (value != _EnableSsl)
                {
                    _EnableSsl = value;
                    OnPropertyChanged("EnableSsl");
                }
            }
        }

        private int _port;
        public int Port
        {
            get
            {
                return _port;
            }

            set
            {
                if (value != _port)
                {
                    _port = value;
                    OnPropertyChanged("Port");
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (value != _password)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        private string _SMSUrl;
        public string SMSUrl
        {
            get
            {
                return _SMSUrl;
            }

            set
            {
                if (value != _SMSUrl)
                {
                    _SMSUrl = value;
                    OnPropertyChanged("SMS_Url");
                }
            }
        }

        private string _SMSpassword;
        public string SMSPassword
        {
            get
            {
                return _SMSpassword;
            }

            set
            {
                if (value != _SMSpassword)
                {
                    _SMSpassword = value;
                    OnPropertyChanged("SMS_Password");
                }
            }
        }

        private string _SMS_UserName;
        public string SMS_UserName
        {
            get
            {
                return _SMS_UserName;
            }

            set
            {
                if (value != _SMS_UserName)
                {
                    _SMS_UserName = value;
                    OnPropertyChanged("SMS_UserName");
                }
            }
        }

        private long _OTConsentTypeID;
        public long OTConsentTypeID
        {
            get
            {
                return _OTConsentTypeID;
            }

            set
            {
                if (value != _OTConsentTypeID)
                {
                    _OTConsentTypeID = value;
                    OnPropertyChanged("OTConsentTypeID");
                }
            }
        }

        // Added by BHUSHAN for Auto SMS/Email ....in Application configuration...21/05/2014
        List<clsAppConfigAutoEmailSMSVO> AutoEmailSMS = new List<clsAppConfigAutoEmailSMSVO>();
        public List<clsAppConfigAutoEmailSMSVO> EmailSMS
        {
            get
            {
                return AutoEmailSMS;
            }
            set
            {
                AutoEmailSMS = value;
                OnPropertyChanged("AutoEmailSMS");
            }
        }

        //EMR Changes Added by Ashish Z. on dated 02062017
        private int _EMRModVisitDateInDays;
        public int EMRModVisitDateInDays
        {
            get { return _EMRModVisitDateInDays; }
            set
            {
                if (value != _EMRModVisitDateInDays)
                {
                    _EMRModVisitDateInDays = value;
                    OnPropertyChanged("EMRModVisitDateInDays");
                }
            }
        }
        //End

        # region Costing Divisions for Clinical & Pharmacy Billing

        private long _ClinicalCostingDivisionID;
        public long ClinicalCostingDivisionID
        {
            get
            {
                return _ClinicalCostingDivisionID;
            }

            set
            {
                if (value != _ClinicalCostingDivisionID)
                {
                    _ClinicalCostingDivisionID = value;
                    OnPropertyChanged("ClinicalCostingDivisionID");
                }
            }
        }

        private long _PharmacyCostingDivisionID;
        public long PharmacyCostingDivisionID
        {
            get
            {
                return _PharmacyCostingDivisionID;
            }

            set
            {
                if (value != _PharmacyCostingDivisionID)
                {
                    _PharmacyCostingDivisionID = value;
                    OnPropertyChanged("PharmacyCostingDivisionID");
                }
            }
        }

        # endregion



        #region IPDVariables

        private long _MaleTitle;
        public long MaleTitle
        {
            get
            {
                return _MaleTitle;
            }

            set
            {
                if (value != _MaleTitle)
                {
                    _MaleTitle = value;
                    OnPropertyChanged("MaleTitle");
                }
            }
        }

        private long _FemaleTitle;
        public long FemaleTitle
        {
            get
            {
                return _FemaleTitle;
            }

            set
            {
                if (value != _FemaleTitle)
                {
                    _FemaleTitle = value;
                    OnPropertyChanged("FemaleTitle");
                }
            }
        }

        private long _MedicineServiceID;
        public long MedicineServiceID
        {
            get
            {
                return _MedicineServiceID;
            }

            set
            {
                if (value != _MedicineServiceID)
                {
                    _MedicineServiceID = value;
                    OnPropertyChanged("MedicineServiceID");
                }
            }
        }

        private long _implantServiceID;
        public long implantServiceID
        {
            get
            {
                return _implantServiceID;
            }
            set
            {
                if (value != _implantServiceID)
                {
                    _implantServiceID = value;
                    OnPropertyChanged("implantServiceID");
                }
            }
        }





        #endregion

        private long _RadiologyDepartmentID;
        public long RadiologyDepartmentID
        {
            get
            {
                return _RadiologyDepartmentID;
            }

            set
            {
                if (value != _RadiologyDepartmentID)
                {
                    _RadiologyDepartmentID = value;
                    OnPropertyChanged("RadiologyDepartmentID");
                }
            }
        }

        private long _PathologyDepartmentID;
        public long PathologyDepartmentID
        {
            get
            {
                return _PathologyDepartmentID;
            }

            set
            {
                if (value != _PathologyDepartmentID)
                {
                    _PathologyDepartmentID = value;
                    OnPropertyChanged("PathologyDepartmentID");
                }
            }
        }

        // Added By CDS For Package 
        private long _PatientCategoryL1Id_Retail;
        public long PatientCategoryL1Id_Retail
        {
            get
            {
                return _PatientCategoryL1Id_Retail;
            }

            set
            {
                if (value != _PatientCategoryL1Id_Retail)
                {
                    _PatientCategoryL1Id_Retail = value;
                    OnPropertyChanged("PatientCategoryL1Id_Retail");
                }
            }
        }

        private bool _IsIPD;
        public bool IsIPD
        {
            get
            {
                return _IsIPD;
            }

            set
            {
                if (value != _IsIPD)
                {
                    _IsIPD = value;
                    OnPropertyChanged("IsIPD");
                }
            }
        }

        private bool _IsOPD;
        public bool IsOPD
        {
            get
            {
                return _IsOPD;
            }

            set
            {
                if (value != _IsOPD)
                {
                    _IsOPD = value;
                    OnPropertyChanged("IsOPD");
                }
            }
        }

        private long _IPDCreditLimit;
        public long CreditLimitIPD
        {
            get
            {
                return _IPDCreditLimit;
            }

            set
            {
                if (value != _IPDCreditLimit)
                {
                    _IPDCreditLimit = value;
                    OnPropertyChanged("IPDCreditLimit");
                }
            }
        }

        private long _OPDCreditLimit;
        public long CreditLimitOPD
        {
            get
            {
                return _OPDCreditLimit;
            }

            set
            {
                if (value != _OPDCreditLimit)
                {
                    _OPDCreditLimit = value;
                    OnPropertyChanged("OPDCreditLimit");
                }
            }
        }



        public enum AuthorizationLevel
        {
            Level1 = 1,
            Level2 = 2,
            Both = 3
        };


        private bool _IsCentralPurchaseStore;
        public bool IsCentralPurchaseStore
        {
            get
            {
                return _IsCentralPurchaseStore;
            }

            set
            {
                if (value != _IsCentralPurchaseStore)
                {
                    _IsCentralPurchaseStore = value;
                    OnPropertyChanged("IsCentralPurchaseStore");
                }
            }
        }

        private long _IndentStoreID;
        public long IndentStoreID
        {
            get
            {
                return _IndentStoreID;
            }

            set
            {
                if (value != _IndentStoreID)
                {
                    _IndentStoreID = value;
                    OnPropertyChanged("IndentStoreID");
                }
            }
        }


        // Added by Anumani on 16.02.2015 in order to reflect Pathology color code

        private long _SampleNoLevel;
        public long SampleNoLevel
        {
            get { return _SampleNoLevel; }
            set
            {
                if (value != _SampleNoLevel)
                {
                    _SampleNoLevel = value;
                    OnPropertyChanged("SampleNoLevel");
                }
            }
        }

        public string pathominColorCode { get; set; }
        public string pathonormalColorCode { get; set; }
        public string pathomaxColorCode { get; set; }

        private string _FirstLevelResultColor;
        public string FirstLevelResultColor
        {
            get { return _FirstLevelResultColor; }
            set { if (value != _FirstLevelResultColor) { _FirstLevelResultColor = value; OnPropertyChanged("FirstLevelResultColor"); } }
        }

        private string _SecondLevelResultColor;
        public string SecondLevelResultColor
        {
            get { return _SecondLevelResultColor; }
            set { if (value != _SecondLevelResultColor) { _SecondLevelResultColor = value; OnPropertyChanged("SecondLevelResultColor"); } }
        }

        private string _ThirdLevelResultColor;
        public string ThirdLevelResultColor
        {
            get { return _ThirdLevelResultColor; }
            set { if (value != _ThirdLevelResultColor) { _ThirdLevelResultColor = value; OnPropertyChanged("ThirdLevelResultColor"); } }
        }

        private string _CheckResultColor;
        public string CheckResultColor
        {
            get { return _CheckResultColor; }
            set { if (value != _CheckResultColor) { _CheckResultColor = value; OnPropertyChanged("CheckResultColor"); } }
        }

        private long _RefundToAdvancePayModeID;     // Refund To Advance 22042017
        public long RefundToAdvancePayModeID
        {
            get
            {
                return _RefundToAdvancePayModeID;
            }

            set
            {
                if (value != _RefundToAdvancePayModeID)
                {
                    _RefundToAdvancePayModeID = value;
                    OnPropertyChanged("RefundToAdvancePayModeID");
                }
            }
        }

        private decimal _PatientDailyCashLimit;
        public decimal PatientDailyCashLimit
        {
            get { return _PatientDailyCashLimit; }
            set 
            {
                if (value != _PatientDailyCashLimit)
                {
                    _PatientDailyCashLimit = value;
                    OnPropertyChanged("PatientDailyCashLimit");
                }
            }
        }

        private decimal _CounterSaleBillAdresslimit;
        public decimal CounterSaleBillAdresslimit
        {
            get { return _CounterSaleBillAdresslimit; }
            set
            {
                if (value != _CounterSaleBillAdresslimit)
                {
                    _CounterSaleBillAdresslimit = value;
                    OnPropertyChanged("CounterSaleBillAdresslimit");
                }
            }
        }
        

    }

    public class clsAppAccountsConfigVO : IValueObject, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
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
            throw new NotImplementedException();
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
                    OnPropertyChanged("ID");
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
                    OnPropertyChanged("UnitID");
                }
            }
        }


        private long _ChequeDDBankID;
        public long ChequeDDBankID
        {
            get
            {
                return _ChequeDDBankID;
            }

            set
            {
                if (value != _ChequeDDBankID)
                {
                    _ChequeDDBankID = value;
                    OnPropertyChanged("ChequeDDBankID");
                }
            }
        }

        public string ChequeDDBankName { get; set; }

        private long _CrDBBankID;
        public long CrDBBankID
        {
            get
            {
                return _CrDBBankID;
            }

            set
            {
                if (value != _CrDBBankID)
                {
                    _CrDBBankID = value;
                    OnPropertyChanged("CrDBBankID");
                }
            }
        }

        public string CrDbBankName { get; set; }

        private string _CashLedgerName;
        public string CashLedgerName
        {
            get
            {
                return _CashLedgerName;
            }

            set
            {
                if (value != _CashLedgerName)
                {
                    _CashLedgerName = value;
                    OnPropertyChanged("CashLedgerName");
                }
            }
        }

        private string _AdvanceLedgerName;
        public string AdvanceLedgerName
        {
            get
            {
                return _AdvanceLedgerName;
            }

            set
            {
                if (value != _AdvanceLedgerName)
                {
                    _AdvanceLedgerName = value;
                    OnPropertyChanged("AdvanceLedgerName");
                }
            }
        }

        private string _ConsultationLedgerName;
        public string ConsultationLedgerName
        {
            get
            {
                return _ConsultationLedgerName;
            }

            set
            {
                if (value != _ConsultationLedgerName)
                {
                    _ConsultationLedgerName = value;
                    OnPropertyChanged("ConsultationLedgerName");
                }
            }
        }

        private string _DiagnosticLedgerName;
        public string DiagnosticLedgerName
        {
            get
            {
                return _DiagnosticLedgerName;
            }

            set
            {
                if (value != _DiagnosticLedgerName)
                {
                    _DiagnosticLedgerName = value;
                    OnPropertyChanged("DiagnosticLedgerName");
                }
            }
        }

        private string _OtherServicesLedgerName;
        public string OtherServicesLedgerName
        {
            get
            {
                return _OtherServicesLedgerName;
            }

            set
            {
                if (value != _OtherServicesLedgerName)
                {
                    _OtherServicesLedgerName = value;
                    OnPropertyChanged("OtherServicesLedgerName");
                }
            }
        }


        public string _PurchaseLedgerName;
        public string PurchaseLedgerName
        {
            get
            {
                return _PurchaseLedgerName;
            }

            set
            {
                if (value != _PurchaseLedgerName)
                {
                    _PurchaseLedgerName = value;
                    OnPropertyChanged("PurchaseLedgerName");
                }
            }
        }


        public string _COGSLedgerName;
        public string COGSLedgerName
        {
            get
            {
                return _COGSLedgerName;
            }

            set
            {
                if (value != _COGSLedgerName)
                {
                    _COGSLedgerName = value;
                    OnPropertyChanged("COGSLedgerName");
                }
            }
        }

        public string _ProfitLedgerName;
        public string ProfitLedgerName
        {
            get
            {
                return _ProfitLedgerName;
            }

            set
            {
                if (value != _ProfitLedgerName)
                {
                    _ProfitLedgerName = value;
                    OnPropertyChanged("ProfitLedgerName");
                }
            }
        }


        public string _ScrapIncomeLedgerName;
        public string ScrapIncomeLedgerName
        {
            get
            {
                return _ScrapIncomeLedgerName;
            }

            set
            {
                if (value != _ScrapIncomeLedgerName)
                {
                    _ScrapIncomeLedgerName = value;
                    OnPropertyChanged("ScrapIncomeLedgerName");
                }
            }
        }


        public string _CurrentAssetLedgerName;
        public string CurrentAssetLedgerName
        {
            get
            {
                return _CurrentAssetLedgerName;
            }

            set
            {
                if (value != _CurrentAssetLedgerName)
                {
                    _CurrentAssetLedgerName = value;
                    OnPropertyChanged("CurrentAssetLedgerName");
                }
            }
        }

        public string _ExpenseLedgerName;
        public string ExpenseLedgerName
        {
            get
            {
                return _ExpenseLedgerName;
            }

            set
            {
                if (value != _ExpenseLedgerName)
                {
                    _ExpenseLedgerName = value;
                    OnPropertyChanged("ExpenseLedgerName");
                }
            }
        }


        public long _ItemScrapCategory;
        public long ItemScrapCategory
        {
            get
            {
                return _ItemScrapCategory;
            }

            set
            {
                if (value != _ItemScrapCategory)
                {
                    _ItemScrapCategory = value;
                    OnPropertyChanged("ItemScrapCategory");
                }
            }
        }


     
    

    }

    //public class clsGetAppUserUnitBizActionVO : IBizActionValueObject
    //{

    //    public string GetBizAction()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string ToXml()
    //    {
    //        return this.ToString();
    //    }

    //    private int _SuccessStatus;
    //    /// <summary>
    //    /// Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>

    //    public int SuccessStatus
    //    {
    //        get { return _SuccessStatus; }
    //        set { _SuccessStatus = value; }
    //    }

    //    private clsAppConfigVO objLoginUnit = new clsAppConfigVO();
    //    /// <summary>
    //    /// Output Property.
    //    /// This Property Contains Login Unit Details Which is Added.
    //    /// </summary>

    //    public clsAppConfigVO LoginUnit
    //    {
    //        get { return objLoginUnit; }
    //        set { objLoginUnit = value; }
    //    }
    //}

    public class clsGetAppConfigBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetAppConfigBizAction";
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

        public long UnitID { get; set; }

        private clsAppConfigVO objApp = new clsAppConfigVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public string Error { get; set; }
        public clsAppConfigVO AppConfig
        {
            get { return objApp; }
            set { objApp = value; }
        }

        // BY BHUSHAN . . . . . .
        private List<clsAppConfigAutoEmailSMSVO> objEmail = new List<clsAppConfigAutoEmailSMSVO>();
        public List<clsAppConfigAutoEmailSMSVO> AppEmail
        {
            get { return objEmail; }
            set { objEmail = value; }
        }
    }

    public class clsUpdateAppConfigBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdateAppConfigBizAction";
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

        private clsAppConfigVO objApp = new clsAppConfigVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsAppConfigVO AppConfig
        {
            get { return objApp; }
            set { objApp = value; }
        }

        // Added by BHUSHAN ......20-3-13
        private List<clsAppConfigAutoEmailSMSVO> objEmail = new List<clsAppConfigAutoEmailSMSVO>();
        public List<clsAppConfigAutoEmailSMSVO> AppEmail
        {
            get { return objEmail; }
            set { objEmail = value; }
        }

    }

    // Added by BHUSHAN ....VO for AddEmailCCTo...Added by BHUSHAN-----20-1-14
    public class clsAppEmailCCToVo : IValueObject, INotifyPropertyChanged
    {
        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
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
                    OnPropertyChanged("ID");
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
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _ConfigAutoEmailID;
        public long ConfigAutoEmailID
        {
            get
            {
                return _ConfigAutoEmailID;
            }
            set
            {
                if (value != _ConfigAutoEmailID)
                {
                    _ConfigAutoEmailID = value;
                    OnPropertyChanged("ConfigAutoEmailID");
                }
            }
        }

        private string _CCToEmailID;
        public string CCToEmailID
        {
            get
            {
                return _CCToEmailID;
            }
            set
            {
                if (value != _CCToEmailID)
                {
                    _CCToEmailID = value;
                    OnPropertyChanged("CCToEmailID");
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get
            {
                return _CreatedUnitID;
            }
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
            get
            {
                return _UpdatedUnitID;
            }
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
            get
            {
                return _AddedBy;
            }
            set
            {
                if (value != _AddedBy)
                {
                    _AddedBy = value;
                    OnPropertyChanged("sAddedBy");
                }
            }
        }

        private string _AddedOn;
        public string AddedOn
        {
            get
            {
                return _AddedOn;
            }
            set
            {
                if (value != _AddedOn)
                {
                    _AddedOn = value;
                    OnPropertyChanged("AddedOn");
                }
            }
        }

        private DateTime _AddedDateTime;
        public DateTime AddedDateTime
        {
            get
            {
                return _AddedDateTime;
            }
            set
            {
                if (value != _AddedDateTime)
                {
                    _AddedDateTime = value;
                    OnPropertyChanged("AddedDateTime");
                }
            }
        }

        private long _UpdatedBy;
        public long UpdatedBy
        {
            get
            {
                return _UpdatedBy;
            }
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
            get
            {
                return _UpdatedOn;
            }
            set
            {
                if (value != _UpdatedOn)
                {
                    _UpdatedOn = value;
                    OnPropertyChanged("UpdatedOn");
                }
            }
        }

        private DateTime _UpdatedDateTime;
        public DateTime UpdatedDateTime
        {
            get
            {
                return _UpdatedDateTime;
            }
            set
            {
                if (value != _UpdatedDateTime)
                {
                    _UpdatedDateTime = value;
                    OnPropertyChanged("UpdatedDateTime");
                }
            }
        }
        private string _AddedWindowsLoginName;
        public string AddedWindowsLoginName
        {
            get
            {
                return _AddedWindowsLoginName;
            }
            set
            {
                if (value != _AddedWindowsLoginName)
                {
                    _AddedWindowsLoginName = value;
                    OnPropertyChanged("AddedWindowsLoginName");
                }
            }
        }

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get
            {
                return _UpdateWindowsLoginName;
            }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        public int TotalRowCount { get; set; }
    }

    // By BHUSHAN . . . .  21/01/2014
    public class clsAppConfigAutoEmailSMSVO : IValueObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
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
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private long _EventID;
        public long EventID
        {
            get
            {
                return _EventID;
            }
            set
            {
                if (value != _EventID)
                {
                    _EventID = value;
                    OnPropertyChanged("EventId");
                }
            }
        }

        private long _AppEmail;
        public long AppEmail
        {
            get
            {
                return _AppEmail;
            }

            set
            {
                if (value != _AppEmail)
                {
                    _AppEmail = value;
                    OnPropertyChanged("EmailTemplateID");
                }
            }
        }

        private long _AppSMS;
        public long AppSMS
        {
            get
            {
                return _AppSMS;
            }

            set
            {
                if (value != _AppSMS)
                {
                    _AppSMS = value;
                    OnPropertyChanged("SMSTemplateID");
                }
            }
        }

        private string _EmailTemplatName;
        public string EmailTemplatName
        {
            get
            {
                return _EmailTemplatName;
            }

            set
            {
                if (value != _EmailTemplatName)
                {
                    _EmailTemplatName = value;
                    OnPropertyChanged("EmailTemplatName");
                }
            }
        }

        private string _SMSTemplatName;
        public string SMSTemplatName
        {
            get
            {
                return _SMSTemplatName;
            }

            set
            {
                if (value != _SMSTemplatName)
                {
                    _SMSTemplatName = value;
                    OnPropertyChanged("SMSTemplatName");
                }
            }
        }

        private string _SendEmailId;
        public string SendEmailId
        {
            get
            {
                return _SendEmailId;
            }

            set
            {
                if (value != _SendEmailId)
                {
                    _SendEmailId = value;
                    OnPropertyChanged("SendEmailId");
                }
            }
        }

        private string _EventType;
        public string EventType
        {
            get
            {
                return _EventType;
            }
            set
            {
                if (value != _EventType)
                {
                    _EventType = value;
                    OnPropertyChanged("EventType");
                }
            }
        }

        //private long _ConfigAutoEmailID;
        //public long ConfigAutoEmailID
        //{
        //    get
        //    {
        //        return _ConfigAutoEmailID;
        //    }
        //    set
        //    {
        //        if (value != _ConfigAutoEmailID)
        //        {
        //            _ConfigAutoEmailID = value;
        //            OnPropertyChanged("ConfigAutoEmailID");
        //        }
        //    }
        //}

        //private List<string> _CCToEmailID;
        //public List<string> CCToEmailID
        //{
        //    get
        //    {
        //        return _CCToEmailID;
        //    }
        //    set
        //    {
        //        if (value != _CCToEmailID)
        //        {
        //            _CCToEmailID = value;
        //            OnPropertyChanged("CCToEmailID");
        //        }
        //    }
        //}

        #region CommonFileds


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

        private string _AddedOn = "";
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

        private DateTime? _AddedDateTime;
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

        private string _AddedWindowsLoginName = "";
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

        private long _UpdatedBy;
        public long UpdatedBy
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

        private string _UpdatedOn = "";
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

        private DateTime? _UpdatedDateTime;
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

        private string _UpdatedWindowsLoginName = "";
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

    }

    // Biz Action VO for EmailCCTo.....Added by BHUSHAN-----20-1-14
    public class clsAppEmailCCToBizActionVo : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAppEmailCCToBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private long _UnitID;
        public long UnitID
        {
            get
            {
                return _UnitID;
            }
            set
            {
                _UnitID = value;

            }

        }

        private long _ConfigAutoEmailID;
        public long ConfigAutoEmailID
        {
            get
            {
                return _ConfigAutoEmailID;
            }
            set
            {
                _ConfigAutoEmailID = value;
            }
        }

        private clsAppEmailCCToVo objAppEmailCC = new clsAppEmailCCToVo();
        public clsAppEmailCCToVo AppEmailCC
        {
            get { return objAppEmailCC; }
            set { objAppEmailCC = value; }
        }

        public List<clsAppEmailCCToVo> ItemList { get; set; }

        public int TotalRowCount { get; set; }
    }

    // Biz Action VO for delete checkbox.....Added by BHUSHAN-----20-1-14
    public class clsStatusEmailCCToBizActionVo : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsStatusEmailCCToBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public int ResultStatus { get; set; }

        private clsAppEmailCCToVo objAppEmailCC = new clsAppEmailCCToVo();
        public clsAppEmailCCToVo AppEmailCC
        {
            get { return objAppEmailCC; }
            set { objAppEmailCC = value; }
        }
        public List<clsAppEmailCCToVo> ItemList { get; set; }

    }

    // Biz Action VO for Add email id.....Added by BHUSHAN-----20-1-14
    public class clsAddEmailIDCCToBizActionVo : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddEmailIDCCToBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public int ResultStatus { get; set; }

        private clsAppEmailCCToVo objAppEmailCC = new clsAppEmailCCToVo();
        public clsAppEmailCCToVo AppEmailCC
        {
            get { return objAppEmailCC; }
            set { objAppEmailCC = value; }
        }
        public List<clsAppEmailCCToVo> ItemList { get; set; }
    }

}
