using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Administration.PackageNew;

namespace PalashDynamics.ValueObjects
{
    public class clsPackageServiceVO : IValueObject, INotifyPropertyChanged
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

        private long _ValidityUnit;
        public long ValidityUnit
        {
            get { return _ValidityUnit; }
            set
            {
                if (_ValidityUnit != value)
                {
                    _ValidityUnit = value;
                    OnPropertyChanged("ValidityUnit");
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
        private string _Service;
        public string Service
        {
            get { return _Service; }
            set
            {
                if (_Service != value)
                {
                    _Service = value;
                    OnPropertyChanged("Service");
                }
            }
        }


        private string _Validity;
        public string Validity
        {
            get { return _Validity; }
            set
            {
                if (_Validity != value)
                {
                    _Validity = value;
                    OnPropertyChanged("Validity");
                }
            }
        }

        private double _PackageAmount;
        public double PackageAmount
        {
            get { return _PackageAmount; }
            set
            {
                if (_PackageAmount != value)
                {
                    _PackageAmount = value;
                    OnPropertyChanged("PackageAmount");
                }
            }
        }

        private string _NoOfFollowUp;
        public string NoOfFollowUp
        {
            get { return _NoOfFollowUp; }
            set
            {
                if (_NoOfFollowUp != value)
                {
                    _NoOfFollowUp = value;
                    OnPropertyChanged("NoOfFollowUp");
                }
            }
        }

        private bool _ApplicableToAll;
        public bool ApplicableToAll
        {
            get { return _ApplicableToAll; }
            set
            {
                if (_ApplicableToAll != value)
                {
                    _ApplicableToAll = value;
                    OnPropertyChanged("ApplicableToAll");
                }
            }
        }

        private double _ApplicableToAllDiscount;
        public double ApplicableToAllDiscount
        {
            get { return _ApplicableToAllDiscount; }
            set
            {
                if (_ApplicableToAllDiscount != value)
                {
                    _ApplicableToAllDiscount = value;
                    OnPropertyChanged("ApplicableToAllDiscount");
                }
            }
        }
        private double _TotalBudget;
        public double TotalBudget
        {
            get { return _TotalBudget; }
            set
            {
                if (_TotalBudget != value)
                {
                    _TotalBudget = value;
                    OnPropertyChanged("TotalBudget");
                }
            }
        }
        private List<clsPackageServiceDetailsVO> _PackageDetails;
        public List<clsPackageServiceDetailsVO> PackageDetails
        {
            get
            {
                if (_PackageDetails == null)
                    _PackageDetails = new List<clsPackageServiceDetailsVO>();

                return _PackageDetails;
            }

            set
            {

                _PackageDetails = value;

            }
        }

        private List<clsPackageItemMasterVO> _ItemDetails;
        public List<clsPackageItemMasterVO> ItemDetails
        {
            get
            {
                if (_ItemDetails == null)
                    _ItemDetails = new List<clsPackageItemMasterVO>();

                return _ItemDetails;
            }
            set
            {
                _ItemDetails = value;
            }
        }

        private List<clsPackageServiceConditionsVO> _ServiceConditionDetails;
        public List<clsPackageServiceConditionsVO> ServiceConditionDetails
        {
            get
            {
                if (_ServiceConditionDetails == null)
                    _ServiceConditionDetails = new List<clsPackageServiceConditionsVO>();

                return _ServiceConditionDetails;
            }
            set
            {
                _ServiceConditionDetails = value;
            }
        }

        private List<clsPackageServiceConditionsVO> _ServiceConditionDetailsDelete;
        public List<clsPackageServiceConditionsVO> ServiceConditionDetailsDelete
        {
            get
            {
                if (_ServiceConditionDetailsDelete == null)
                    _ServiceConditionDetailsDelete = new List<clsPackageServiceConditionsVO>();

                return _ServiceConditionDetailsDelete;
            }
            set
            {
                _ServiceConditionDetailsDelete = value;
            }
        }

        private List<clsPackageServiceRelationsVO> _PackageServiceRelationDetails;
        public List<clsPackageServiceRelationsVO> PackageServiceRelationDetails
        {
            get
            {
                if (_PackageServiceRelationDetails == null)
                    _PackageServiceRelationDetails = new List<clsPackageServiceRelationsVO>();

                return _PackageServiceRelationDetails;
            }
            set
            {
                _PackageServiceRelationDetails = value;
            }
        }

        private List<clsPackageServiceRelationsVO> _PackageServiceRelationDetailsDelete;
        public List<clsPackageServiceRelationsVO> PackageServiceRelationDetailsDelete
        {
            get
            {
                if (_PackageServiceRelationDetailsDelete == null)
                    _PackageServiceRelationDetailsDelete = new List<clsPackageServiceRelationsVO>();

                return _PackageServiceRelationDetailsDelete;
            }
            set
            {
                _PackageServiceRelationDetailsDelete = value;
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

        // Added By CDS On 19/01/2017
        //private bool _IsFixedRate;
        //public bool IsFixedRate
        //{
        //    get { return _IsFixedRate; }
        //    set
        //    {
        //        if (_IsFixedRate != value)
        //        {
        //            _IsFixedRate = value;
        //            OnPropertyChanged("IsFixedRate");
        //        }
        //    }
        //}

        public bool IsFixedRate { get; set; }

        private double _ServiceFixedRate;
        public double ServiceFixedRate
        {
            get { return _ServiceFixedRate; }
            set
            {
                if (_ServiceFixedRate != value)
                {
                    _ServiceFixedRate = value;
                    OnPropertyChanged("ServiceFixedRate");
                }
            }
        }


        private double _PharmacyFixedRate;
        public double PharmacyFixedRate
        {
            get { return _PharmacyFixedRate; }
            set
            {
                if (_PharmacyFixedRate != value)
                {
                    _PharmacyFixedRate = value;
                    OnPropertyChanged("PharmacyFixedRate");
                }
            }
        }

        private double _ServicePercentage;
        public double ServicePercentage
        {
            get { return _ServicePercentage; }
            set
            {
                if (_ServicePercentage != value)
                {
                    _ServicePercentage = value;
                    OnPropertyChanged("ServicePercentage");
                }
            }
        }

        private double _PharmacyPercentage;
        public double PharmacyPercentage
        {
            get { return _PharmacyPercentage; }
            set
            {
                if (_PharmacyPercentage != value)
                {
                    _PharmacyPercentage = value;
                    OnPropertyChanged("PharmacyPercentage");
                }
            }
        }

        //END

        #endregion

        #region Common Property
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
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
                if (_UpdatedUnitID != value)
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

        private DateTime? _AddedDateTime;
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

        private DateTime? _UpdatedDateTime;
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

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
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

    public class clsPackageServiceDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (_PackageID != value)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
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

        private string _Department;
        public string Department
        {
            get { return _Department; }
            set
            {
                if (_Department != value)
                {
                    _Department = value;
                    OnPropertyChanged("Department");
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
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged("ServiceName");
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


        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (value != _IsActive)
                {
                    _IsActive = value;
                    OnPropertyChanged("IsActive");
                }
            }
        }

        private bool _FreeAtFollowUp;
        public bool FreeAtFollowUp
        {
            get { return _FreeAtFollowUp; }
            set
            {
                if (value != _FreeAtFollowUp)
                {
                    _FreeAtFollowUp = value;
                    OnPropertyChanged("FreeAtFollowUp");
                }
            }
        }

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private string _DisplayQuantity;
        public string DisplayQuantity
        {
            get { return _DisplayQuantity; }
            set
            {
                if (value != _DisplayQuantity)
                {
                    _DisplayQuantity = value;
                    OnPropertyChanged("DisplayQuantity");
                }
            }
        }

        // Commented  By CDS On 20/01/2017
        //private double _Quantity = 1;
        //public double Quantity
        //{
        //    get { return _Quantity; }
        //    set
        //    {
        //        if (_Quantity != value)
        //        {
        //            if (value <= 0)
        //                value = 1;

        //            _Quantity = value;
        //            OnPropertyChanged("Quantity");


        //        }
        //    }
        //}
        //END

        // Added By CDS On 20/01/2017
        private double _Quantity;
        public double Quantity
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
        //END



        private double _ConcessionPercentage;
        public double ConcessionPercentage
        {
            get { return _ConcessionPercentage; }
            set
            {
                if (_ConcessionPercentage != value)
                {
                    if (value < 0)
                        value = 0;
                    _ConcessionPercentage = value;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get
            {
                if (_ConcessionPercentage != 0)
                {
                    return _ConcessionAmount = ((Rate * _ConcessionPercentage) / 100);
                }
                else
                    return _ConcessionAmount;
            }
            set
            {
                if (_ConcessionAmount != value)
                {
                    if (value < 0)
                        value = 0;
                    _ConcessionAmount = value;
                    if (_ConcessionAmount > 0)
                        _ConcessionPercentage = (_ConcessionAmount * 100) / Rate;
                    else
                        _ConcessionPercentage = 0;

                    OnPropertyChanged("ConcessionPercentage");
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {

            get { return _NetAmount = _Rate - _ConcessionAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }
        List<MasterListItem> _Doctor = new List<MasterListItem>();
        public List<MasterListItem> Doctor
        {
            get
            {
                return _Doctor;
            }
            set
            {
                if (value != _Doctor)
                {
                    _Doctor = value;
                }
            }

        }

        MasterListItem _SelectedDoctor = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedDoctor
        {
            get
            {
                return _SelectedDoctor;
            }
            set
            {
                if (value != _SelectedDoctor)
                {
                    _SelectedDoctor = value;
                    OnPropertyChanged("SelectedDoctor");
                }
            }


        }


        // Added By CDS On 19/01/2017

        private bool _AdjustableHead;
        public bool AdjustableHead
        {
            get { return _AdjustableHead; }
            set
            {
                if (_AdjustableHead != value)
                {
                    _AdjustableHead = value;
                    OnPropertyChanged("AdjustableHead");
                }
            }
        }

        private bool _IsFixed;
        public bool IsFixed
        {
            get { return _IsFixed; }
            set
            {
                if (_IsFixed != value)
                {
                    _IsFixed = value;
                    OnPropertyChanged("IsFixed");
                }
            }
        }



        private double _FixedRate;
        public double FixedRate
        {
            get { return _FixedRate; }
            set
            {
                if (_FixedRate != value)
                {
                    _FixedRate = value;
                    OnPropertyChanged("FixedRate");
                }
            }
        }

        private double _RatePercentage;
        public double RatePercentage
        {
            get { return _RatePercentage; }
            set
            {
                if (_RatePercentage != value)
                {
                    _RatePercentage = value;
                    OnPropertyChanged("RatePercentage");
                }
            }
        }

        private bool _IsDoctorSharePercentage;
        public bool IsDoctorSharePercentage
        {
            get { return _IsDoctorSharePercentage; }
            set
            {
                if (_IsDoctorSharePercentage != value)
                {
                    _IsDoctorSharePercentage = value;
                    OnPropertyChanged("IsDoctorSharePercentage");
                }
            }
        }


        private bool _ConsiderAdjustable;
        public bool ConsiderAdjustable
        {
            get { return _ConsiderAdjustable; }
            set
            {
                if (_ConsiderAdjustable != value)
                {
                    _ConsiderAdjustable = value;
                    OnPropertyChanged("ConsiderAdjustable");
                }
            }
        }


        //END

        #endregion

        #region For IPD Module

        private long _ServiceSpecilizationID;
        public long ServiceSpecilizationID
        {
            get { return _ServiceSpecilizationID; }
            set
            {
                if (value != _ServiceSpecilizationID)
                {
                    _ServiceSpecilizationID = value;
                    OnPropertyChanged("ServiceSpecilizationID");
                }
            }
        }

        private long _ServiceSubSpecilizationID;
        public long ServiceSubSpecilizationID
        {
            get { return _ServiceSubSpecilizationID; }
            set
            {
                if (value != _ServiceSubSpecilizationID)
                {
                    _ServiceSubSpecilizationID = value;
                    OnPropertyChanged("ServiceSubSpecilizationID");
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

        #endregion

        #region For Package  Only

        private double _Amount;
        public double Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (value != _Amount)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                }
            }

        }

        private double _Discount;
        public double Discount
        {
            get
            {
                return _Discount;
            }

            set
            {
                if (value != _Discount)
                {
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }

        }

        private bool _IsDiscountOnQuantity;
        public bool IsDiscountOnQuantity
        {
            get { return _IsDiscountOnQuantity; }
            set
            {
                if (value != _IsDiscountOnQuantity)
                {
                    _IsDiscountOnQuantity = value;
                    OnPropertyChanged("IsDiscountOnQuantity");
                }
            }
        }

        private long _AgeLimit;
        public long AgeLimit
        {
            get { return _AgeLimit; }
            set
            {
                if (_AgeLimit != value)
                {
                    _AgeLimit = value;
                    OnPropertyChanged("AgeLimit");
                }
            }
        }

        private bool _IsFollowupNotRequired;
        public bool IsFollowupNotRequired
        {
            get { return _IsFollowupNotRequired; }
            set
            {
                if (value != _IsFollowupNotRequired)
                {
                    _IsFollowupNotRequired = value;
                    OnPropertyChanged("IsFollowupNotRequired");
                }
            }
        }

        MasterListItem _SelectedGender = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGender
        {
            get
            {
                return _SelectedGender;
            }
            set
            {
                if (value != _SelectedGender)
                {
                    _SelectedGender = value;
                    OnPropertyChanged("SelectedGender");
                }
            }


        }

        private bool _Infinite;
        public bool Infinite
        {
            get { return _Infinite; }
            set
            {
                if (_Infinite != value)
                {
                    _Infinite = value;
                    OnPropertyChanged("Infinite");
                }
            }
        }


        private bool _IsSpecilizationGroup;
        public bool IsSpecilizationGroup
        {
            get { return _IsSpecilizationGroup; }
            set
            {
                if (value != _IsSpecilizationGroup)
                {
                    _IsSpecilizationGroup = value;
                    OnPropertyChanged("IsSpecilizationGroup");
                }
            }
        }

        private string _Month;
        public string Month
        {
            get { return _Month; }
            set
            {
                if (_Month != value)
                {
                    _Month = value;
                    OnPropertyChanged("Month");
                }
            }
        }

        private bool _MonthStatus;
        public bool MonthStatus
        {
            get { return _MonthStatus; }
            set
            {
                if (_MonthStatus != value)
                {
                    _MonthStatus = value;
                    OnPropertyChanged("MonthStatus");
                }
            }
        }

        public bool FromPackage { get; set; }

        private bool _Delete;
        public bool Delete
        {
            get
            {
                return _Delete;
            }

            set
            {
                if (value != _Delete)
                {
                    _Delete = value;
                    OnPropertyChanged("Delete");
                }
            }

        }

        private long _ApplicableTo;
        public long ApplicableTo
        {
            get { return _ApplicableTo; }
            set
            {
                if (_ApplicableTo != value)
                {
                    _ApplicableTo = value;
                    OnPropertyChanged("ApplicableTo");
                }
            }
        }

        private string _Validity;
        public string Validity
        {
            get { return _Validity; }
            set
            {
                if (_Validity != value)
                {
                    _Validity = value;
                    OnPropertyChanged("Validity");
                }
            }
        }

        private long _ValidityUnit;
        public long ValidityUnit
        {
            get { return _ValidityUnit; }
            set
            {
                if (_ValidityUnit != value)
                {
                    _ValidityUnit = value;
                    OnPropertyChanged("ValidityUnit");
                }
            }
        }

        private long _GroupID;
        public long GroupID
        {
            get { return _GroupID; }
            set
            {
                if (_GroupID != value)
                {
                    _GroupID = value;
                    OnPropertyChanged("GroupID");
                }
            }
        }

        #region Package New Changes

        // Package New Changes Added on 18042018 for Procedure

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

        // Package New Changes Added on 16042018 for Procedure
        MasterListItem _SelectedProcess = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedProcess
        {
            get
            {
                return _SelectedProcess;
            }
            set
            {
                if (value != _SelectedProcess)
                {
                    _SelectedProcess = value;
                    OnPropertyChanged("SelectedProcess");
                }
            }
        }

        // Package New Changes Added on 16042018 for Procedure
        private long _ProcessID;
        public long ProcessID
        {
            get { return _ProcessID; }
            set
            {
                if (_ProcessID != value)
                {
                    _ProcessID = value;
                    OnPropertyChanged("ProcessID");
                }
            }
        }

        // Package New Changes Added on 25042018 for Procedure
        private bool _IsConsumables;
        public bool IsConsumables
        {
            get { return _IsConsumables; }
            set
            {
                if (_IsConsumables != value)
                {
                    _IsConsumables = value;
                    OnPropertyChanged("IsConsumables");
                }
            }
        }

        #endregion

        #endregion

        #region Common Property
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

        private long _CreatedUnitID;
        public long CreatedUnitID
        {
            get { return _CreatedUnitID; }
            set
            {
                if (_CreatedUnitID != value)
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
                if (_UpdatedUnitID != value)
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

        private DateTime? _AddedDateTime;
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

        private DateTime? _UpdatedDateTime;
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

        private string _UpdateWindowsLoginName = "";
        public string UpdateWindowsLoginName
        {
            get { return _UpdateWindowsLoginName; }
            set
            {
                if (_UpdateWindowsLoginName != value)
                {
                    _UpdateWindowsLoginName = value;
                    OnPropertyChanged("UpdateWindowsLoginName");
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

    #region  for Package Configuration clsPackageItemMasterVO is  Added by CDS

    public class clsFollowUpServiceStatusVO : IValueObject
    {
        public long FollowUpID { get; set; }
        public long FollowUpUnitID { get; set; }
        public long FollowUpRemarkExist { get; set; }
        public string IsFollowUpRemarkExist { get; set; }
        public long FollowUpPostPoneFromId { get; set; }
        public long ServiceID { get; set; }
        public long TariffID { get; set; }
        public DateTime ServerDate { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime FollowUpTime { get; set; }
        public string FollowUpFor { get; set; }
        public string FollowUpRemarks { get; set; }
        public bool IsPostpone { get; set; }
        public string FollowUpDateFormat { get; set; }
        public DateTime? FollowUpPostPoneDate { get; set; }
        public DateTime FollowUpPostponeTime { get; set; }
        public bool IsSchedule { get; set; }
        public DateTime? FollowUpScheduleDate { get; set; }
        public DateTime? PreviousFollowUpDate { get; set; }
        public DateTime? ActualServiceConsumedDate { get; set; }
        public bool IsClose { get; set; }
        public string ColourCode { get; set; }
        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion

    }
    public class PackageMonths
    {
        public string Month { get; set; }
        public bool MonthStatus { get; set; }
        public clsFollowUpServiceStatusVO FollowUpServiceDetails { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public string IsFollowUpRemarkExist { get; set; }
        public string ColourCode { get; set; }
        public string FollowUpDateFormat { get; set; }
        public long FollowUpID { get; set; }
        public long FollowUpUnitID { get; set; }

    }

    public class PackageList : INotifyPropertyChanged
    {
        private ObservableCollection<clsFollowUpServiceStatusVO> _FollowUpServiceDetails;

        public ObservableCollection<clsFollowUpServiceStatusVO> FollowUpServiceDetails
        {
            get
            {
                return this._FollowUpServiceDetails;
            }
            set
            {
                this._FollowUpServiceDetails = value;
                this.OnPropertyChanged("FollowUpServiceDetails");
            }
        }

        //  public clsFollowUpServiceStatusVO FollowUpServiceDetails { get; set; }
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
        public long FollowUpID { get; set; }
        public long FollowUpUnitID { get; set; }
        public clsFollowUpServiceStatusVO FollowUpServiceStatus { get; set; }
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
        public string IsFollowUpRemarkExist { get; set; }
        private double _Rate;
        public double Rate
        {
            get
            {
                return _Rate;
            }

            set
            {
                if (value != _Rate)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }

        }

        private double _Amount;
        public double Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (value != _Amount)
                {
                    _Amount = value;
                    OnPropertyChanged("Amount");
                }
            }

        }

        private double _Discount;
        public double Discount
        {
            get
            {
                return _Discount;
            }

            set
            {
                if (value != _Discount)
                {
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }

        }

        List<MasterListItem> _GenderList = new List<MasterListItem>();
        public List<MasterListItem> GenderList
        {
            get
            {
                return _GenderList;
            }
            set
            {
                if (value != _GenderList)
                {
                    _GenderList = value;
                }
            }

        }

        MasterListItem _SelectedGender = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedGender
        {
            get
            {
                return _SelectedGender;
            }
            set
            {
                if (value != _SelectedGender)
                {
                    _SelectedGender = value;
                    OnPropertyChanged("SelectedGender");
                }
            }


        }

        private bool _Infinite;
        public bool Infinite
        {
            get
            {
                return _Infinite;
            }

            set
            {
                if (value != _Infinite)
                {
                    _Infinite = value;
                    OnPropertyChanged("Infinite");
                }
            }

        }

        private double _Quantity;
        public double Quantity
        {
            get
            {
                return _Quantity;
            }

            set
            {
                if (value != _Quantity)
                {
                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }

        }

        private bool _FreeAtFollowUp;
        public bool FreeAtFollowUp
        {
            get
            {
                return _FreeAtFollowUp;
            }

            set
            {
                if (value != _FreeAtFollowUp)
                {
                    _FreeAtFollowUp = value;
                    OnPropertyChanged("FreeAtFollowUp");
                }
            }

        }

        private bool _Delete;
        public bool Delete
        {
            get
            {
                return _Delete;
            }

            set
            {
                if (value != _Delete)
                {
                    _Delete = value;
                    OnPropertyChanged("Delete");
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

        private bool _IsSpecilizationGroup;
        public bool IsSpecilizationGroup
        {
            get { return _IsSpecilizationGroup; }
            set
            {
                if (value != _IsSpecilizationGroup)
                {
                    _IsSpecilizationGroup = value;
                    OnPropertyChanged("IsSpecilizationGroup");
                }
            }
        }




        //public ObservableCollection<PackageMonths> Months = new ObservableCollection<PackageMonths>();

        private ObservableCollection<PackageMonths> months;

        public ObservableCollection<PackageMonths> Months
        {
            get
            {
                return this.months;
            }
            set
            {
                this.months = value;
                this.OnPropertyChanged("Months");
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

    public class StringRow : INotifyPropertyChanged //: Dictionary<string, object>
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public object this[string index]
        {
            get
            {
                return _data[index];
            }
            set
            {
                _data[index] = value;

                OnPropertyChanged("Data");
            }
        }

        public object Data
        {
            get
            {
                return this;
            }

            set
            {
                OnPropertyChanged("Data");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }

    public class StringTable : Dictionary<int, StringRow>
    {
        public List<string> ColumnNames { get; set; }

        public StringTable()
        {
            ColumnNames = new List<string>();
        }
    }

    public class ColumnValueSelector : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringRow row = (StringRow)value;
            string columnName = (string)parameter;
            return (row[columnName]);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new Exception();

            return value;
            //return new PropertyValueChange(parameter as string, value);
        }
    }

    public class clsPackageItemMasterVO : IValueObject, INotifyPropertyChanged
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

        private long _PackageID;
        public long PackageID
        {
            get { return _PackageID; }
            set
            {
                if (value != _PackageID)
                {
                    _PackageID = value;
                    OnPropertyChanged("PackageID");
                }
            }
        }

        private long _PackageUnitID;
        public long PackageUnitID
        {
            get { return _PackageUnitID; }
            set
            {
                if (value != _PackageUnitID)
                {
                    _PackageUnitID = value;
                    OnPropertyChanged("PackageUnitID");
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

        private double _Discount;
        public double Discount
        {
            get { return _Discount; }
            set
            {
                if (value != _Discount)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 0;
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }
        }
        private double _Budget;
        public double Budget
        {
            get { return _Budget; }
            set
            {
                if (value != _Budget)
                {
                    _Budget = value;
                    OnPropertyChanged("Budget");
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
                    if (value < 0)
                        value = 1;
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

        private string _ItemCode = "";
        public string ItemCode
        {
            get { return _ItemCode; }
            set
            {
                if (value != _ItemCode)
                {
                    _ItemCode = value;
                    OnPropertyChanged("ItemCode");
                }
            }
        }

        private long _ItemGroup;
        public long ItemGroup
        {
            get { return _ItemGroup; }
            set
            {
                if (value != _ItemGroup)
                {
                    _ItemGroup = value;
                    OnPropertyChanged("ItemGroup");
                }
            }
        }

        private string _ItemGroupName = "";
        public string ItemGroupName
        {
            get { return _ItemGroupName; }
            set
            {
                if (value != _ItemGroupName)
                {
                    _ItemGroupName = value;
                    OnPropertyChanged("ItemGroupName");
                }
            }
        }



        private long _ItemCategory;
        public long ItemCategory
        {
            get { return _ItemCategory; }
            set
            {
                if (value != _ItemCategory)
                {
                    _ItemCategory = value;
                    OnPropertyChanged("ItemCategory");
                }
            }
        }

        private string _ItemCategoryName = "";
        public string ItemCategoryName
        {
            get { return _ItemCategoryName; }
            set
            {
                if (value != _ItemCategoryName)
                {
                    _ItemCategoryName = value;
                    OnPropertyChanged("ItemCategoryName");
                }
            }
        }

        private bool _IsCategory;
        public bool IsCategory
        {
            get { return _IsCategory; }
            set
            {
                if (value != _IsCategory)
                {
                    _IsCategory = value;
                    OnPropertyChanged("IsCategory");
                }

            }
        }

        private bool _IsGroup;
        public bool IsGroup
        {
            get { return _IsGroup; }
            set
            {
                if (value != _IsGroup)
                {
                    _IsGroup = value;
                    OnPropertyChanged("IsGroup");
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
}
