using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsServiceMasterVO : IValueObject, INotifyPropertyChanged
    {
        public clsServiceMasterVO()
        {

        }

        public bool FromPackage { get; set; }
        public int SrNo { get; set; }
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

        private long _PrescriptionID;
        public long PrescriptionID
        {
            get { return _PrescriptionID; }
            set
            {
                if (value != _PrescriptionID)
                {
                    _PrescriptionID = value;
                    OnPropertyChanged("PrescriptionID");
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

        private string _DoctorCode;
        public string DoctorCode
        {
            get { return _DoctorCode; }
            set
            {
                if (value != _DoctorCode)
                {
                    _DoctorCode = value;
                    OnPropertyChanged("DoctorCode");
                }
            }
        }
        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (value != _ServiceCode)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        private string _CodeDetails;
        public string CodeDetails
        {
            get { return _CodeDetails; }
            set
            {
                if (value != _CodeDetails)
                {
                    _CodeDetails = value;
                    OnPropertyChanged("CodeDetails");
                }
            }
        }



        private long _CodeType;
        public long CodeType
        {
            get { return _CodeType; }
            set
            {
                if (value != _CodeType)
                {
                    _CodeType = value;
                    OnPropertyChanged("CodeType");
                }
            }
        }


        private bool _IsDefaultAgency;
        public bool IsDefaultAgency
        {
            get { return _IsDefaultAgency; }
            set
            {
                if (value != _IsDefaultAgency)
                {
                    _IsDefaultAgency = value;
                    OnPropertyChanged("IsDefaultAgency");
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

        private string _CodeName;
        public string CodeName
        {
            get { return _CodeName; }
            set
            {
                if (value != _CodeName)
                {
                    _CodeName = value;
                    OnPropertyChanged("CodeName");
                }
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (value != _ExpiryDate)
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
                if (value != _EffectiveDate)
                {
                    _EffectiveDate = value;
                    OnPropertyChanged("EffectiveDate");
                }
            }
        }

        private long _Specialization;
        public long Specialization
        {
            get { return _Specialization; }
            set
            {
                if (value != _Specialization)
                {
                    _Specialization = value;
                    OnPropertyChanged("Specialization");
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

        private string _SpecializationString;
        public string SpecializationString
        {
            get { return _SpecializationString; }
            set
            {
                if (value != _SpecializationString)
                {
                    _SpecializationString = value;
                    OnPropertyChanged("SpecializationString");
                }
            }
        }

        private DateTime _RoundDate;
        public DateTime RoundDate
        {
            get { return _RoundDate; }
            set
            {
                if (value != _RoundDate)
                {
                    _RoundDate = value;
                    OnPropertyChanged("RoundDate");
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

        private string _SubSpecializationString;
        public string SubSpecializationString
        {
            get { return _SubSpecializationString; }
            set
            {
                if (value != _SubSpecializationString)
                {
                    _SubSpecializationString = value;
                    OnPropertyChanged("SubSpecializationString");
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

        private long _SubSpecialization;
        public long SubSpecialization
        {
            get { return _SubSpecialization; }
            set
            {
                if (value != _SubSpecialization)
                {
                    _SubSpecialization = value;
                    OnPropertyChanged("SubSpecialization");
                }
            }
        }

        private string _ShortDescription;
        public string ShortDescription
        {
            get { return _ShortDescription; }
            set
            {
                if (value != _ShortDescription)
                {
                    _ShortDescription = value;
                    OnPropertyChanged("ShortDescription");
                }
            }
        }

        private string _LongDescription;
        public string LongDescription
        {
            get { return _LongDescription; }
            set
            {
                if (value != _LongDescription)
                {
                    _LongDescription = value;
                    OnPropertyChanged("LongDescription");
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


        private bool _AssignedAgency;
        public bool AssignedAgency
        {
            get { return _AssignedAgency; }
            set
            {
                if (value != _AssignedAgency)
                {
                    _AssignedAgency = value;
                    OnPropertyChanged("AssignedAgency");
                }
            }
        }
        private long _AssignedAgencyID;
        public long AssignedAgencyID
        {
            get { return _AssignedAgencyID; }
            set
            {
                if (value != _AssignedAgencyID)
                {
                    _AssignedAgencyID = value;
                    OnPropertyChanged("AssignedAgencyID");
                }
            }
        }


        private string _SpecializationCode;
        public string SpecializationCode
        {
            get { return _SpecializationCode; }
            set
            {
                if (value != _SpecializationCode)
                {
                    _SpecializationCode = value;
                    OnPropertyChanged("SpecializationCode");
                }
            }
        }

        private string _Group;
        public string Group
        {
            get { return _Group; }
            set
            {
                if (value != _Group)
                {
                    _Group = value;
                    OnPropertyChanged("Group");
                }
            }
        }

        private bool _IsNewMaster = false;
        public bool IsNewMaster
        {
            get { return _IsNewMaster; }
            set { _IsNewMaster = value; }
        }

        private bool _SelectService;
        public bool SelectService
        {
            get { return _SelectService; }
            set
            {
                if (value != _SelectService)
                {
                    _SelectService = value;
                    OnPropertyChanged("SelectService");
                }
            }
        }
        //---------------------------------------------------------------
        private bool _StaffDiscount;
        public bool StaffDiscount
        {
            get { return _StaffDiscount; }
            set
            {
                if (value != _StaffDiscount)
                {
                    _StaffDiscount = value;
                    OnPropertyChanged("StaffDiscount");
                }
            }
        }


        private decimal _StaffDiscountAmount;
        public decimal StaffDiscountAmount
        {
            get { return _StaffDiscountAmount; }
            set
            {
                if (value != _StaffDiscountAmount)
                {
                    _StaffDiscountAmount = value;
                    OnPropertyChanged("StaffDiscountAmount");
                }
            }
        }

        private decimal _StaffDiscountPercent;
        public decimal StaffDiscountPercent
        {
            get { return _StaffDiscountPercent; }
            set
            {
                if (value != _StaffDiscountPercent)
                {
                    _StaffDiscountPercent = value;
                    OnPropertyChanged("StaffDiscountPercent");
                }
            }
        }

        //-----------------------------------------------------------------------------

        private bool _StaffDependantDiscount;
        public bool StaffDependantDiscount
        {
            get { return _StaffDependantDiscount; }
            set
            {
                if (value != _StaffDependantDiscount)
                {
                    _StaffDependantDiscount = value;
                    OnPropertyChanged("StaffDependantDiscount");
                }
            }
        }




        private decimal _StaffDependantDiscountAmount;
        public decimal StaffDependantDiscountAmount
        {
            get { return _StaffDependantDiscountAmount; }
            set
            {
                if (value != _StaffDependantDiscountAmount)
                {
                    _StaffDependantDiscountAmount = value;
                    OnPropertyChanged("StaffDependantDiscountAmount");
                }
            }
        }


        private decimal _StaffDependantDiscountPercent;
        public decimal StaffDependantDiscountPercent
        {
            get { return _StaffDependantDiscountPercent; }
            set
            {
                if (value != _StaffDependantDiscountPercent)
                {
                    _StaffDependantDiscountPercent = value;
                    OnPropertyChanged("StaffDependantDiscountPercent");
                }
            }
        }



        //----------------------------------------------------------------------------------------------------
        private bool _GeneralDiscount;
        public bool GeneralDiscount
        {
            get { return _GeneralDiscount; }
            set
            {
                if (value != _GeneralDiscount)
                {
                    _GeneralDiscount = value;
                    OnPropertyChanged("GeneralDiscount");
                }
            }
        }
        //----------------------------------------------------------------------------------------------------
        private bool _Concession;
        public bool Concession
        {
            get { return _Concession; }
            set
            {
                if (value != _Concession)
                {
                    _Concession = value;
                    OnPropertyChanged("Concession");
                }
            }
        }



        private decimal _ConcessionAmount;
        public decimal ConcessionAmount
        {
            get { return _ConcessionAmount; }
            set
            {
                if (value != _ConcessionAmount)
                {
                    _ConcessionAmount = value;
                    OnPropertyChanged("ConcessionAmount");
                }
            }
        }


        private decimal _ConcessionPercent;
        public decimal ConcessionPercent
        {
            get { return _ConcessionPercent; }
            set
            {
                if (value != _ConcessionPercent)
                {
                    _ConcessionPercent = value;
                    OnPropertyChanged("ConcessionPercent");
                }
            }
        }
        //------------------------------------------------------------------------------------------- 

        private bool _ServiceTax;
        public bool ServiceTax
        {
            get { return _ServiceTax; }
            set
            {
                if (value != _ServiceTax)
                {
                    _ServiceTax = value;
                    OnPropertyChanged("ServiceTax");
                }
            }
        }

        private bool _SeniorCitizen;
        public bool SeniorCitizen
        {
            get { return _SeniorCitizen; }
            set
            {
                if (value != _SeniorCitizen)
                {
                    _SeniorCitizen = value;
                    OnPropertyChanged("SeniorCitizen");
                }
            }
        }

        private decimal _SeniorCitizenConAmount;
        public decimal SeniorCitizenConAmount
        {
            get { return _SeniorCitizenConAmount; }
            set
            {
                if (value != _SeniorCitizenConAmount)
                {
                    _SeniorCitizenConAmount = value;
                    OnPropertyChanged("SeniorCitizenConAmount");
                }
            }
        }


        private decimal _SeniorCitizenConPercent;
        public decimal SeniorCitizenConPercent
        {
            get { return _SeniorCitizenConPercent; }
            set
            {
                if (value != _SeniorCitizenConPercent)
                {
                    _SeniorCitizenConPercent = value;
                    OnPropertyChanged("SeniorCitizenConPercent");
                }
            }
        }

        private int _SeniorCitizenAge;
        public int SeniorCitizenAge
        {
            get { return _SeniorCitizenAge; }
            set
            {
                if (value != _SeniorCitizenAge)
                {
                    _SeniorCitizenAge = value;
                    OnPropertyChanged("SeniorCitizenAge");
                }
            }
        }
        private decimal _ServiceTaxAmount;
        public decimal ServiceTaxAmount
        {
            get { return _ServiceTaxAmount; }
            set
            {
                if (value != _ServiceTaxAmount)
                {
                    _ServiceTaxAmount = value;
                    OnPropertyChanged("ServiceTaxAmount");
                }
            }
        }


        private decimal _ServiceTaxPercent;
        public decimal ServiceTaxPercent
        {
            get { return _ServiceTaxPercent; }
            set
            {
                if (value != _ServiceTaxPercent)
                {
                    _ServiceTaxPercent = value;
                    OnPropertyChanged("ServiceTaxPercent");
                }
            }
        }

        private decimal _LuxuryTaxAmount;
        public decimal LuxuryTaxAmount
        {
            get { return _LuxuryTaxAmount; }
            set
            {
                if (value != _LuxuryTaxAmount)
                {
                    _LuxuryTaxAmount = value;
                    OnPropertyChanged("LuxuryTaxAmount");
                }
            }
        }

        private decimal _LuxuryTaxPercent;
        public decimal LuxuryTaxPercent
        {
            get { return _LuxuryTaxPercent; }
            set
            {
                if (value != _LuxuryTaxPercent)
                {
                    _LuxuryTaxPercent = value;
                    OnPropertyChanged("LuxuryTaxPercent");
                }
            }
        }

        //-----------------------------------------------------------------------------------------------
        private bool _OutSource;
        public bool OutSource
        {
            get { return _OutSource; }
            set
            {
                if (value != _OutSource)
                {
                    _OutSource = value;
                    OnPropertyChanged("OutSource");
                }
            }
        }

        private bool _InHouse;
        public bool InHouse
        {
            get { return _InHouse; }
            set
            {
                if (value != _InHouse)
                {
                    _InHouse = value;
                    OnPropertyChanged("InHouse");
                }
            }
        }
        //----------------------------------------------------------------------------------------
        private bool _DoctorShare;
        public bool DoctorShare
        {
            get { return _DoctorShare; }
            set
            {
                if (value != _DoctorShare)
                {
                    _DoctorShare = value;
                    OnPropertyChanged("DoctorShare");
                }
            }
        }

        private decimal _DoctorSharePercentage;
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

        private decimal _DoctorShareAmount;
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
        //-----------------------------------------------------------------------------
        private bool _RateEditable;
        public bool RateEditable
        {
            get { return _RateEditable; }
            set
            {
                if (value != _RateEditable)
                {
                    _RateEditable = value;
                    OnPropertyChanged("RateEditable");
                }
            }
        }

        private decimal _MaxRate;
        public decimal MaxRate
        {
            get { return _MaxRate; }
            set
            {
                if (value != _MaxRate)
                {
                    _MaxRate = value;
                    OnPropertyChanged("MaxRate");
                }
            }
        }

        public MasterListItem SelectedPriority { get; set; }

        private decimal _BaseServiceRate;
        public decimal BaseServiceRate
        {
            get { return _BaseServiceRate; }
            set
            {
                if (value != _BaseServiceRate)
                {
                    _BaseServiceRate = value;
                    OnPropertyChanged("BaseServiceRate");
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
        public string Datetime { get; set; }
        private DateTime _VisitDate;
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set
            {
                if (value != _VisitDate)
                {
                    _VisitDate = value;
                    OnPropertyChanged("_VisitDate");
                }
            }
        }

        private decimal _MinRate;
        public decimal MinRate
        {
            get { return _MinRate; }
            set
            {
                if (value != _MinRate)
                {
                    _MinRate = value;
                    OnPropertyChanged("MinRate");
                }
            }
        }
        //-------------------------------------------------------------------------------------
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

        private bool _CheckedAllTariffs;
        public bool CheckedAllTariffs
        {
            get { return _CheckedAllTariffs; }
            set
            {
                if (value != _CheckedAllTariffs)
                {
                    _CheckedAllTariffs = value;
                    OnPropertyChanged("CheckedAllTariffs");
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

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public bool EditMode { get; set; }

        public string Code { get; set; }
        public bool IsPackage { get; set; }
        public bool IsFavourite { get; set; }
        public bool IsOTProcedure { get; set; }
        public bool IsLinkWithInventory { get; set; }
        public bool IsPrescribedService { get; set; }
        public bool IsFamily { get; set; }
        public Int32 FamilyMemberCount { get; set; }
        public bool BasePackage { get; set; }
        public bool HealthPlan { get; set; }
        public long PackageID { get; set; }
        public bool IsTariffChecked { get; set; }
        public long ServiceID { get; set; }
        public long ClassID { get; set; }
        public string ClassName { get; set; }
        public long TariffID { get; set; }
        public string Query { get; set; }
        public bool ServiceTariffMasterStatus { get; set; }
        public long TariffServiceMasterID { get; set; }
        public List<long> TariffIDList { get; set; }
        public string TariffIDs { get; set; }
        public string TariffCode { get; set; }
        public TariffOperationType OperationType { get; set; }

        public Boolean IsPrescribeServiceFromEMR { get; set; }
        public bool IsBilledEMR { get; set; }
        public string VisibleBill { get; set; }
        public string collapseBill { get; set; }

        public long ChargeID { get; set; }
        public long PackageBillID { get; set; }
        public long PackageBillUnitID { get; set; }


        //added by neena
        public long TemplateID { get; set; }
        public long DepartmentID { get; set; }
        private bool _IsConsentCheck;
        public bool IsConsentCheck
        {
            get
            {
                return _IsConsentCheck;
            }
            set
            {
                _IsConsentCheck = value;
                OnPropertyChanged("IsConsentCheck");
            }
        }
        //

        //Added by PMG to avoid Multi sponsers in single bill
        #region to avoid Multi sponsers in single bill

        public long CompanyID { get; set; }
        public long PatientSourceID { get; set; }

        #endregion

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                if (value != _Description)
                {
                    _Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string TarrifCode { get; set; }

        public string TarrifName { get; set; }

        public string PatientCategoryL3 { get; set; }  //set to group according to Tariff

        private long _PackageServiceConditionID;
        public long PackageServiceConditionID
        {
            get { return _PackageServiceConditionID; }
            set
            {
                if (value != _PackageServiceConditionID)
                {
                    _PackageServiceConditionID = value;

                    if (_PackageServiceConditionID > 0)
                    {
                        _BackColorString = "";
                        _IsEnableConditionalServices = true;
                    }
                    else
                    {
                        _IsEnableConditionalServices = false;
                    }
                    OnPropertyChanged("PackageServiceConditionID");
                }
            }
        }

        private string _BackColorString;
        public string BackColorString
        {
            get { return _BackColorString; }
            set
            {
                if (value != _BackColorString)
                {
                    _BackColorString = "Beige";
                    OnPropertyChanged("BackColorString");
                }
            }
        }

        //  set to enable view link for services having Package Conditional Services
        private bool _IsEnableConditionalServices;
        public bool IsEnableConditionalServices
        {
            get { return _IsEnableConditionalServices; }
            set
            {
                if (value != _IsEnableConditionalServices)
                {
                    _IsEnableConditionalServices = value;
                    OnPropertyChanged("IsEnableConditionalServices");
                }
            }
        }
        public bool IsMarkUp { get; set; }

        //***//-------
        public long PrescriptionDetailsID { get; set; }
        public long InvestigationDetailsID { get; set; }
        public long VisitID { get; set; }
        public bool Billed { get; set; }
        public bool InvestigationBilled { get; set; }
        public bool IsBillEnabled { get; set; }
        //------------

        public bool IsFavorite { get; set; }

        public string Patient_Name { get; set; }
        public string Package_Name { get; set; }

        #region For IPD Module

        private long _AdmissionTypeID;
        public long AdmissionTypeID
        {
            get { return _AdmissionTypeID; }
            set
            {
                if (value != _AdmissionTypeID)
                {
                    _AdmissionTypeID = value;
                    OnPropertyChanged("AdmissionTypeID");
                }
            }
        }


        private string _AdmissionTypeName;
        public string AdmissionTypeName
        {
            get { return _AdmissionTypeName; }
            set
            {
                if (value != _AdmissionTypeName)
                {
                    _AdmissionTypeName = value;
                    OnPropertyChanged("AdmissionTypeName");
                }
            }
        }

        private bool _IsClassRateReadonly;
        public bool IsClassRateReadonly
        {
            get { return _IsClassRateReadonly; }
            set
            {
                if (value != _IsClassRateReadonly)
                {
                    _IsClassRateReadonly = value;
                    OnPropertyChanged("IsClassRateReadonly");
                }
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    //OnPropertyChanged("IsSelected");
                }
            }
        }

        private bool _IsEnabled = true;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (value != _IsEnabled)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }
        #endregion


        // Added By CDS For Package 
        public long PackageUnitID { get; set; }
        public bool IsFreezed { get; set; }
        public bool IsApproved { get; set; }


        public bool IsAdjustableHead { get; set; }

        public bool IsConsiderAdjustable { get; set; }

        private bool _IsInGroupSpecialization = false;
        public bool IsInGroupSpecialization
        {
            get { return _IsInGroupSpecialization; }
            set
            {
                if (_IsInGroupSpecialization != value)
                {
                    _IsInGroupSpecialization = value;
                    OnPropertyChanged("IsInGroupSpecialization");
                }
            }
        }

        private decimal _SumOfExludedServices;
        public decimal SumOfExludedServices
        {
            get { return _SumOfExludedServices; }
            set
            {
                if (value != _SumOfExludedServices)
                {
                    _SumOfExludedServices = value;
                    OnPropertyChanged("SumOfExludedServices");
                }
            }
        }

        private decimal _ServiceComponentRate;
        public decimal ServiceComponentRate
        {
            get { return _ServiceComponentRate; }
            set
            {
                if (value != _ServiceComponentRate)
                {
                    _ServiceComponentRate = value;
                    OnPropertyChanged("ServiceComponentRate");
                }
            }
        }

        // End 

        private decimal _PercentageOnMrp;
        public decimal PercentageOnMrp
        {
            get { return _PercentageOnMrp; }
            set
            {
                if (value != _PercentageOnMrp)
                {
                    _PercentageOnMrp = value;
                    OnPropertyChanged("PercentageOnMrp");
                }
            }
        }


        private string _ApplicableToString;
        public string ApplicableToString
        {
            get { return _ApplicableToString; }
            set
            {
                if (value != _ApplicableToString)
                {
                    _ApplicableToString = value;
                    OnPropertyChanged("ApplicableToString");
                }
            }
        }

        private long _ApplicableTo;
        public long ApplicableTo
        {
            get { return _ApplicableTo; }
            set
            {
                if (value != _ApplicableTo)
                {
                    _ApplicableTo = value;
                    OnPropertyChanged("ApplicableTo");
                }
            }
        }


        private long _TariffIsPackage;
        public long TariffIsPackage
        {
            get { return _TariffIsPackage; }
            set
            {
                if (value != _TariffIsPackage)
                {
                    _TariffIsPackage = value;
                    OnPropertyChanged("TariffIsPackage");
                }
            }
        }

        private long _CategoryID;
        public long CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        private string _CategoryName;
        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public List<clsPackageServiceDetailsVO> PackageInPackageItemList = new List<clsPackageServiceDetailsVO>();


        # region Properties used for Conditional Services

        //ConditionID
        private long _ConditionID;
        public long ConditionID
        {
            get { return _ConditionID; }
            set
            {
                if (value != _ConditionID)
                {
                    _ConditionID = value;
                    OnPropertyChanged("ConditionID");
                }
            }
        }

        //ConditionUnitID
        private long _ConditionUnitID;
        public long ConditionUnitID
        {
            get { return _ConditionUnitID; }
            set
            {
                if (value != _ConditionUnitID)
                {
                    _ConditionUnitID = value;
                    OnPropertyChanged("ConditionUnitID");
                }
            }
        }


        //MainServicePackageID
        private long _MainServicePackageID;
        public long MainServicePackageID
        {
            get { return _MainServicePackageID; }
            set
            {
                if (value != _MainServicePackageID)
                {
                    _MainServicePackageID = value;
                    OnPropertyChanged("MainServicePackageID");
                }
            }
        }

        //MainServicePackageUnitID
        private long _MainServicePackageUnitID;
        public long MainServicePackageUnitID
        {
            get { return _MainServicePackageUnitID; }
            set
            {
                if (value != _MainServicePackageUnitID)
                {
                    _MainServicePackageUnitID = value;
                    OnPropertyChanged("MainServicePackageUnitID");
                }
            }
        }

        //PackageServiceID	
        private long _PackageServiceID;
        public long PackageServiceID
        {
            get { return _PackageServiceID; }
            set
            {
                if (value != _PackageServiceID)
                {
                    _PackageServiceID = value;
                    OnPropertyChanged("PackageServiceID");
                }
            }
        }

        //ConditionServiceID	
        private long _ConditionServiceID;
        public long ConditionServiceID
        {
            get { return _ConditionServiceID; }
            set
            {
                if (value != _ConditionServiceID)
                {
                    _ConditionServiceID = value;
                    OnPropertyChanged("ConditionServiceID");
                }
            }
        }

        //ConditionServiceName
        private string _ConditionServiceName;
        public string ConditionServiceName
        {
            get { return _ConditionServiceName; }
            set
            {
                if (value != _ConditionServiceName)
                {
                    _ConditionServiceName = value;
                    OnPropertyChanged("ConditionServiceName");
                }
            }
        }

        //MainServiceSpecilizationID
        private long _MainServiceSpecilizationID;
        public long MainServiceSpecilizationID
        {
            get { return _MainServiceSpecilizationID; }
            set
            {
                if (value != _MainServiceSpecilizationID)
                {
                    _MainServiceSpecilizationID = value;
                    OnPropertyChanged("MainServiceSpecilizationID");
                }
            }
        }

        //MainSerivceIsSpecilizationGroup	
        private bool _MainSerivceIsSpecilizationGroup;
        public bool MainSerivceIsSpecilizationGroup
        {
            get { return _MainSerivceIsSpecilizationGroup; }
            set
            {
                if (value != _MainSerivceIsSpecilizationGroup)
                {
                    _MainSerivceIsSpecilizationGroup = value;
                    OnPropertyChanged("MainSerivceIsSpecilizationGroup");
                }
            }
        }

        //ConditionalRate
        private decimal _ConditionalRate;
        public decimal ConditionalRate
        {
            get { return _ConditionalRate; }
            set
            {
                if (value != _ConditionalRate)
                {
                    _ConditionalRate = value;
                    OnPropertyChanged("ConditionalRate");
                }
            }
        }

        //ConditionalQuantity
        private double _ConditionalQuantity;
        public double ConditionalQuantity
        {
            get { return _ConditionalQuantity; }
            set
            {
                if (value != _ConditionalQuantity)
                {
                    _ConditionalQuantity = value;
                    OnPropertyChanged("ConditionalQuantity");
                }
            }
        }

        //ConditionalDiscount	
        private decimal _ConditionalDiscount;
        public decimal ConditionalDiscount
        {
            get { return _ConditionalDiscount; }
            set
            {
                if (value != _ConditionalDiscount)
                {
                    _ConditionalDiscount = value;
                    OnPropertyChanged("ConditionalDiscount");
                }
            }
        }

        //ConditionType	
        private String _ConditionType;
        public String ConditionType
        {
            get { return _ConditionType; }
            set
            {
                if (_ConditionType != value)
                {
                    _ConditionType = value;
                    OnPropertyChanged("ConditionType");
                }
            }
        }

        //ConditionTypeID	
        private long _ConditionTypeID;
        public long ConditionTypeID
        {
            get { return _ConditionTypeID; }
            set
            {
                if (value != _ConditionTypeID)
                {
                    _ConditionTypeID = value;
                    OnPropertyChanged("ConditionTypeID");
                }
            }
        }

        //ConditionalUsedQuantity
        private double _ConditionalUsedQuantity;
        public double ConditionalUsedQuantity
        {
            get { return _ConditionalUsedQuantity; }
            set
            {
                if (value != _ConditionalUsedQuantity)
                {
                    _ConditionalUsedQuantity = value;
                    OnPropertyChanged("ConditionalUsedQuantity");
                }
            }
        }

        //MainServiceUsedQuantity
        private double _MainServiceUsedQuantity;
        public double MainServiceUsedQuantity
        {
            get { return _MainServiceUsedQuantity; }
            set
            {
                if (value != _MainServiceUsedQuantity)
                {
                    _MainServiceUsedQuantity = value;
                    OnPropertyChanged("MainServiceUsedQuantity");
                }
            }
        }

        private double _TotalORUsedQuantity;
        public double TotalORUsedQuantity
        {
            get { return _TotalORUsedQuantity; }
            set
            {
                if (value != _TotalORUsedQuantity)
                {
                    _TotalORUsedQuantity = value;
                    OnPropertyChanged("TotalORUsedQuantity");
                }
            }
        }

        private bool _IsSet;
        public bool IsSet
        {
            get { return _IsSet; }
            set
            {
                if (value != _IsSet)
                {
                    _IsSet = value;
                    OnPropertyChanged("IsSet");
                }

            }
        }


        private long _ServiceMemberRelationID;
        public long ServiceMemberRelationID
        {
            get { return _ServiceMemberRelationID; }
            set
            {
                if (value != _ServiceMemberRelationID)
                {
                    _ServiceMemberRelationID = value;
                    OnPropertyChanged("ServiceMemberRelationID");
                }
            }
        }

        private bool _IsAgeApplicable;
        public bool IsAgeApplicable
        {
            get { return _IsAgeApplicable; }
            set
            {
                if (value != _IsAgeApplicable)
                {
                    _IsAgeApplicable = value;
                    OnPropertyChanged("IsAgeApplicable");
                }

            }
        }

        # endregion

        private long _SACCodeID;
        public long SACCodeID
        {
            get { return _SACCodeID; }
            set
            {
                if (value != _SACCodeID)
                {
                    _SACCodeID = value;
                    OnPropertyChanged("SACCodeID");
                }
            }
        }

        private double _OPDConsumption;

        public double OPDConsumption
        {
            get { return _OPDConsumption; }
            set { _OPDConsumption = value; }
        }

        private double _OpdExcludeServiceConsumption;

        public double OpdExcludeServiceConsumption
        {
            get { return _OpdExcludeServiceConsumption; }
            set { _OpdExcludeServiceConsumption = value; }
        }

        #region Package New Changes for Procedure Added on 17042018

        private long _ProcessID;
        public long ProcessID
        {
            get { return _ProcessID; }
            set
            {
                if (value != _ProcessID)
                {
                    _ProcessID = value;
                    OnPropertyChanged("ProcessID");
                }
            }
        }

        private string _ProcessName;    // Package New Changes for Procedure Added on 20042018
        public string ProcessName
        {
            get { return _ProcessName; }
            set
            {
                if (value != _ProcessName)
                {
                    _ProcessName = value;
                    OnPropertyChanged("ProcessName");
                }
            }
        }

        // Package New Changes Added on 19062018 for Procedure
        private Int32 _AdjustableHeadType;
        public Int32 AdjustableHeadType
        {
            get { return _AdjustableHeadType; }
            set
            {
                if (_AdjustableHeadType != value)
                {
                    _AdjustableHeadType = value;
                    OnPropertyChanged("AdjustableHeadType");
                }
            }
        }

        #endregion

    }

    public class clsServiceTarrifClassRateDetailsVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        public Int64 ServiceID { get; set; }
        public string ServiceName { get; set; }
        public Int64 TariffID { get; set; }
        public string TariffName { get; set; }
        public Int64 ClassID { get; set; }
        public string ClassName { get; set; }
        public double Rate { get; set; }
        public bool Status { get; set; }



    }

    #region  for IPD Module These VO Are Added by CDS

    public class clsServiceTarrifClassRateDetailsNewVO : IValueObject, INotifyPropertyChanged
    {

        public string ToXml()
        {
            // throw new NotImplementedException();
            return this.ToString();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion


        public int OperationType { get; set; }
        public bool? Modify { get; set; }
        public bool ViewPatient { get; set; }
        public bool FollowupPatientlist { get; set; }
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
        private long _TSMID;
        public long TSMID
        {
            get { return _TSMID; }
            set
            {
                if (value != _TSMID)
                {
                    _TSMID = value;
                    OnPropertyChanged("TSMID");
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
        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set
            {
                if (value != _ClassName)
                {
                    _ClassName = value;
                    //OnPropertyChanged("ClassName");
                }
            }
        }
        private long _ClassID;
        public long ClassID
        {
            get { return _ClassID; }
            set
            {
                if (value != _ClassID)
                {
                    _ClassID = value;
                    OnPropertyChanged("ClassID");
                }
            }
        }

        public long UnitId { get; set; }
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SortExpression { get; set; }
        public bool GetAllTariffServicesClass { get; set; }
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


        private bool _IsEnable = true;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                if (value != _IsEnable)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }

        private bool _IsChkEnable = true;
        public bool IsChkEnable
        {
            get { return _IsChkEnable; }
            set
            {
                if (value != _IsChkEnable)
                {
                    _IsChkEnable = value;
                    OnPropertyChanged("IsChkEnable");
                }
            }
        }
        private bool _Status = false;
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

        private bool _IsChecked = false;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                if (value != _IsChecked)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked");
                }
            }
        }

    }

    public class clsGetMasterForServiceBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetMasterForServiceBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
        private List<clsServiceMasterVO> _ServiceDetails;
        public List<clsServiceMasterVO> ServiceDetails
        {
            get
            {

                return _ServiceDetails;
            }

            set
            {
                _ServiceDetails = value;

            }

        }

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitID { get; set; }
        public string Description { get; set; }

        public string ServiceCode { get; set; }
        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }
        public long SpecializationID { get; set; }
        public string ModalityID { get; set; }
        public long SubSpecializationID { get; set; }
        public string TariffId { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

    }

    #endregion

    #region  for Package Configuration clsServiceItemMasterVO is  Added by CDS

    public class clsServiceItemMasterVO : IValueObject, INotifyPropertyChanged
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
                if (_UnitID != value)
                {
                    _UnitID = value;
                    OnPropertyChanged("UnitID");
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


        private long _ItemID;
        public long ItemID
        {
            get { return _ItemID; }
            set
            {
                if (value != _ItemID)
                {
                    _ItemID = value;
                    OnPropertyChanged("ItemID");
                }
            }
        }

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (value != _Quantity)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }


        private string _ItemName = "";
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (value != _ItemName)
                {
                    _ItemName = value;
                    OnPropertyChanged("ItemName");
                }
            }
        }

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

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

    }

    #endregion


    //By Anjali......................
    public class clsServiceLevelsVO : IValueObject, INotifyPropertyChanged
    {
        public clsServiceLevelsVO()
        {

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

        private long _L1ID;
        public long L1ID
        {
            get { return _L1ID; }
            set
            {
                if (_L1ID != value)
                {
                    _L1ID = value;
                    OnPropertyChanged("L1ID");
                }
            }
        }
        private long _L2ID;
        public long L2ID
        {
            get { return _L2ID; }
            set
            {
                if (_L2ID != value)
                {
                    _L2ID = value;
                    OnPropertyChanged("L2ID");
                }
            }
        }
        private long _L3ID;
        public long L3ID
        {
            get { return _L3ID; }
            set
            {
                if (_L3ID != value)
                {
                    _L3ID = value;
                    OnPropertyChanged("L3ID");
                }
            }
        }
        private long _L4ID;
        public long L4ID
        {
            get { return _L4ID; }
            set
            {
                if (_L4ID != value)
                {
                    _L4ID = value;
                    OnPropertyChanged("L4ID");
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

        private long _ServiceUnitID;
        public long ServiceUnitID
        {
            get { return _ServiceUnitID; }
            set
            {
                if (_ServiceUnitID != value)
                {
                    _ServiceUnitID = value;
                    OnPropertyChanged("ServiceUnitID");
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

        private string _UpdateWindowsLoginName;
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (value != _UpdateWindowsLoginName)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
                }
            }
        }
        /// <summary>
        /// ///////////////for parent id
        /// </summary>
        private long _ChargeID;
        public long ChargeID
        {
            get { return _ChargeID; }
            set
            {
                if (value != _ChargeID)
                {
                    _ChargeID = value;
                    OnPropertyChanged("ChargeID");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    #region GST Details added by Ashish Z. on dated 24062017
    public class clsServiceTaxVO : INotifyPropertyChanged, IValueObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string ToXml()
        {
            throw new NotImplementedException();
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

        private long _TaxID;
        public long TaxID
        {
            get
            {
                return _TaxID;
            }
            set
            {
                if (value != _TaxID)
                {
                    _TaxID = value;
                    OnPropertyChanged("TaxID");
                }
            }
        }

        private Boolean _status;
        public Boolean status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged("status");
                }
            }
        }

        private string _TaxName;
        public string TaxName
        {
            get
            {
                return _TaxName;
            }
            set
            {
                if (value != _TaxName)
                {
                    _TaxName = value;
                    OnPropertyChanged("TaxName");
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
                    OnPropertyChanged("AddedBy");
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
        public string UpdateddOn
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

        private long _UnitId;
        public long UnitId
        {
            get
            {
                return _UnitId;
            }
            set
            {
                if (value != _UnitId)
                {
                    _UnitId = value;
                    OnPropertyChanged("UnitId");
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

        private decimal _Percentage;
        public decimal Percentage
        {
            get
            {
                return _Percentage;
            }
            set
            {
                if (value != _Percentage)
                {
                    _Percentage = value;
                    OnPropertyChanged("Percentage");
                }
            }
        }

        private int _TaxType;
        public int TaxType
        {
            get
            {
                return _TaxType;
            }
            set
            {
                if (value != _TaxType)
                {
                    _TaxType = value;
                    //OnPropertyChanged("Percentage");
                }
            }
        }

        private Boolean _IsTaxLimitApplicable;
        public Boolean IsTaxLimitApplicable
        {
            get
            {
                return _IsTaxLimitApplicable;
            }
            set
            {
                if (value != _IsTaxLimitApplicable)
                {
                    _IsTaxLimitApplicable = value;
                    OnPropertyChanged("IsTaxLimitApplicable");
                }
            }
        }

        private decimal _TaxLimit;
        public decimal TaxLimit
        {
            get
            {
                return _TaxLimit;
            }
            set
            {
                if (value != _TaxLimit)
                {
                    _TaxLimit = value;
                    OnPropertyChanged("TaxLimit");
                }
            }
        }

        private long _ServiceId;
        public long ServiceId
        {
            get
            {
                return _ServiceId;
            }
            set
            {
                if (value != _ServiceId)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceId");
                }
            }
        }

        private long _TariffId;
        public long TariffId
        {
            get
            {
                return _TariffId;
            }
            set
            {
                if (value != _TariffId)
                {
                    _TariffId = value;
                    OnPropertyChanged("TariffId");
                }
            }
        }

        private long _ClassId;
        public long ClassId
        {
            get
            {
                return _ClassId;
            }
            set
            {
                if (value != _ClassId)
                {
                    _ClassId = value;
                    OnPropertyChanged("ClassId");
                }
            }
        }

        private string _ServiceName;
        public string ServiceName
        {
            get
            {
                return _ServiceName;
            }
            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
                }
            }
        }

        private string _TariffName;
        public string TariffName
        {
            get
            {
                return _TariffName;
            }
            set
            {
                if (value != _TariffName)
                {
                    _TariffName = value;
                    OnPropertyChanged("TariffName");
                }
            }
        }

        private string _ClassName;
        public string ClassName
        {
            get
            {
                return _ClassName;
            }
            set
            {
                if (value != _ClassName)
                {
                    _ClassName = value;
                    OnPropertyChanged("ClassName");
                }
            }
        }

        private string _TaxTypeName;
        public string TaxTypeName
        {
            get
            {
                return _TaxTypeName;
            }
            set
            {
                if (value != _TaxTypeName)
                {
                    _TaxTypeName = value;
                    OnPropertyChanged("TaxTypeName");
                }
            }
        }
    }
    #endregion

}


