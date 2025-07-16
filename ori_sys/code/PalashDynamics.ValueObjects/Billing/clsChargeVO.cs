using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.Administration;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsChargeVO : IValueObject, INotifyPropertyChanged
    {


        #region Property Declaration Section  OLD IPD


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

        private bool _IsFirstApproval = true;
        public bool IsFirstApproval
        {
            get { return _IsFirstApproval; }
            set
            {
                if (_IsFirstApproval != value)
                {
                    _IsFirstApproval = value;
                    OnPropertyChanged("IsFirstApproval");
                }
            }
        }

        private bool _IsSecondApproval = true;
        public bool IsSecondApproval
        {
            get { return _IsSecondApproval; }
            set
            {
                if (_IsSecondApproval != value)
                {
                    _IsSecondApproval = value;
                    OnPropertyChanged("IsSecondApproval");
                }
            }
        }

        private long _ApprovalID;
        public long ApprovalID
        {
            get { return _ApprovalID; }
            set
            {
                if (_ApprovalID != value)
                {
                    _ApprovalID = value;
                    OnPropertyChanged("ApprovalID");
                }
            }
        }
        private long _ApprovalHistoryID;
        public long ApprovalHistoryID
        {
            get { return _ApprovalHistoryID; }
            set
            {
                if (_ApprovalHistoryID != value)
                {
                    _ApprovalHistoryID = value;
                    OnPropertyChanged("ApprovalHistoryID");
                }
            }
        }
        private long _ApprovalHistoryUnitID;
        public long ApprovalHistoryUnitID
        {
            get { return _ApprovalHistoryUnitID; }
            set
            {
                if (_ApprovalHistoryUnitID != value)
                {
                    _ApprovalHistoryUnitID = value;
                    OnPropertyChanged("ApprovalHistoryUnitID");
                }
            }
        }
        private long _ApprovalUnitID;
        public long ApprovalUnitID
        {
            get { return _ApprovalUnitID; }
            set
            {
                if (_ApprovalUnitID != value)
                {
                    _ApprovalUnitID = value;
                    OnPropertyChanged("ApprovalUnitID");
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

        private DateTime? _Date;
        public DateTime? Date
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

        //Added by CDS  For IPD Billing Service Consumed date 

        private bool _IsIPDBill = false;
        public bool IsIPDBill
        {
            get { return _IsIPDBill; }
            set
            {
                if (_IsIPDBill != value)
                {
                    _IsIPDBill = value;
                    OnPropertyChanged("IsIPDBill");
                }
            }
        }

        private DateTime? _ServiceDate;
        public DateTime? ServiceDate
        {
            get { return _ServiceDate; }
            set
            {
                if (_ServiceDate != value)
                {
                    _ServiceDate = value;
                    OnPropertyChanged("ServiceDate");
                }
            }
        }


        private long _Opd_Ipd_External_Id;
        public long Opd_Ipd_External_Id
        {
            get { return _Opd_Ipd_External_Id; }
            set
            {
                if (_Opd_Ipd_External_Id != value)
                {
                    _Opd_Ipd_External_Id = value;
                    OnPropertyChanged("Opd_Ipd_External_Id");
                }
            }
        }

        private long _Opd_Ipd_External_UnitId;
        public long Opd_Ipd_External_UnitId
        {
            get { return _Opd_Ipd_External_UnitId; }
            set
            {
                if (_Opd_Ipd_External_UnitId != value)
                {
                    _Opd_Ipd_External_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_External_UnitId");
                }
            }
        }

        private short _Opd_Ipd_External;
        public short Opd_Ipd_External
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

        private long _TariffServiceId;
        public long TariffServiceId
        {
            get { return _TariffServiceId; }
            set
            {
                if (_TariffServiceId != value)
                {
                    _TariffServiceId = value;
                    OnPropertyChanged("TariffServiceId");
                }
            }
        }

        private long _ServiceId;
        public long ServiceId
        {
            get { return _ServiceId; }
            set
            {
                if (_ServiceId != value)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceId");
                }
            }
        }

        private string _ApprovalRequestRemark;
        public string ApprovalRequestRemark
        {
            get { return _ApprovalRequestRemark; }
            set
            {
                if (_ApprovalRequestRemark != value)
                {
                    _ApprovalRequestRemark = value;
                    OnPropertyChanged("ApprovalRequestRemark");
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

        private string _ServiceCode;
        public string ServiceCode
        {
            get { return _ServiceCode; }
            set
            {
                if (_ServiceCode != value)
                {
                    _ServiceCode = value;
                    OnPropertyChanged("ServiceCode");
                }
            }
        }

        //***//
        public long VisitID { get; set; }
        public long PrescriptionDetailsID { get; set; }
        public long InvestigationDetailsID { get; set; }
        //

        private long _DoctorId;
        public long DoctorId
        {
            get { return _DoctorId; }
            set
            {
                if (_DoctorId != value)
                {
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }


        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    if (value < 0)
                        value = 0;

                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        //Added by Ashish Z. on Dated 150616
        private double _InitialRate;
        public double InitialRate
        {
            get { return _InitialRate; }
            set
            {
                if (_InitialRate != value)
                {
                    if (value < 0)
                        value = 0;

                    _InitialRate = value;
                    OnPropertyChanged("InitialRate");
                }
            }
        }
        //

        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {

                    if (value <= 0)
                        value = 1;

                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("ServiceTaxPercent");
                    // OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get
            {
                _TotalAmount = _Quantity * _Rate;
                return _TotalAmount;

            }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                    if (_TotalAmount == 0)
                        OnPropertyChanged("Concession");
                    OnPropertyChanged("NetAmount");

                }
            }
        }



        public long ServiceSpecilizationID { get; set; }


        //private double _Concession;
        //public double Concession
        //{
        //    get { 
        //        if (_TotalAmount == 0) _Concession = 0;
        //        return _Concession; 
        //    }
        //    set
        //    {
        //        if (_Concession != value)
        //        {
        //            if (value < 0)
        //                value = 0;
        //            _Concession = value;
        //            OnPropertyChanged("Concession");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        //private double _ConcessionPercent;
        //public double ConcessionPercent
        //{
        //    get { return _ConcessionPercent; }
        //    set
        //    {
        //        if (_ConcessionPercent != value)
        //        {
        //            if (value < 0)
        //                value = 0;

        //            if (value > 100)
        //                value = 100;

        //            _ConcessionPercent = value;
        //            OnPropertyChanged("ConcessionPercent");
        //            OnPropertyChanged("Concession");     
        //            OnPropertyChanged("ServiceTaxAmount");                   
        //            OnPropertyChanged("TotalAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        //private double _Concession;
        //public double Concession
        //{
        //    get
        //    {
        //        if (_ConcessionPercent > 0)
        //        {
        //            _Concession = ((_Rate * _ConcessionPercent) / 100);

        //            _Concession = Math.Round(_Concession, 2);
        //            return Math.Round(_Concession * _Quantity,2);
        //        }
        //        else
        //        {
        //            return _Concession = 0;
        //        }
        //    }
        //    set
        //    {

        //            if (value < 0)
        //                value = 0;


        //            if (_Concession > 0)
        //                _ConcessionPercent = ((_Concession / _Quantity) * 100) / _Rate;
        //            else
        //                _ConcessionPercent = 0;

        //            OnPropertyChanged("Concession");
        //            OnPropertyChanged("ConcessionPercent");
        //            OnPropertyChanged("ServiceTaxAmount");
        //            OnPropertyChanged("TotalAmount");
        //            OnPropertyChanged("NetAmount");

        //    }
        //}

        //private double _ConcessionPercent;
        //public double ConcessionPercent
        //{
        //    get 
        //    {
        //       // _ConcessionPercent = Math.Round(_ConcessionPercent, 2);
        //        return _ConcessionPercent;
        //    }
        //    set
        //    {
        //        if (_ConcessionPercent != value)
        //        {
        //            if (value < 0)
        //                value = 0;

        //            if (value > 100)
        //                value = 100;
        //            _ConcessionPercent = value;


        //            OnPropertyChanged("ConcessionPercent");
        //            OnPropertyChanged("Concession");
        //            OnPropertyChanged("ServiceTaxAmount");
        //            OnPropertyChanged("TotalAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        //private double _Concession;
        //public double Concession
        //{
        //    get
        //    {
        //        if (_ConcessionPercent > 0)
        //            _Concession = (((_Rate * _Quantity) * _ConcessionPercent) / 100);
        //        else
        //            _Concession = 0;

        //      //  _Concession = Math.Round(_Concession, 2);

        //        return _Concession;
        //    }
        //    set
        //    {
        //        if (_Concession != value)
        //        {
        //            if (value < 0)
        //                value = 0;
        //            //if(_ConcessionAmount !=)
        //         //   _Concession = Math.Round(value, 2);
        //            if (_Concession > 0)
        //                _ConcessionPercent = ((_Concession / _Quantity) * 100) / _Rate;
        //            else
        //                _ConcessionPercent = 0;


        //            OnPropertyChanged("Concession");
        //            OnPropertyChanged("ConcessionPercent");
        //            OnPropertyChanged("ServiceTaxAmount");
        //            OnPropertyChanged("TotalAmount");
        //            OnPropertyChanged("NetAmount");

        //        }
        //    }
        //}

        //private double _ConcessionPercent;
        //public double ConcessionPercent
        //{
        //    get
        //    {
        //        _ConcessionPercent = Math.Round(_ConcessionPercent, 2);
        //        return _ConcessionPercent;
        //    }
        //    set
        //    {
        //        if (_ConcessionPercent != value)
        //        {
        //            if (value < 0)
        //                value = 0;

        //            if (value > 100)
        //                value = 100;
        //            _ConcessionPercent = value;


        //            OnPropertyChanged("ConcessionPercent");
        //            OnPropertyChanged("Concession");
        //            OnPropertyChanged("ServiceTaxAmount");
        //            OnPropertyChanged("TotalAmount");
        //            OnPropertyChanged("NetAmount");
        //        }
        //    }
        //}

        //private double _Concession;
        //public double Concession
        //{
        //    get
        //    {
        //        if (_ConcessionPercent > 0)
        //            _Concession = (((_Rate * _Quantity) * _ConcessionPercent) / 100);
        //        else
        //            _Concession = 0;

        //        // _Concession = Math.Round(_Concession, 2);

        //        _Concession = Math.Round(_Concession);

        //        return _Concession;
        //    }
        //    set
        //    {
        //        if (_Concession != value)
        //        {
        //            if (value < 0)
        //                value = 0;
        //            //if(_ConcessionAmount !=)
        //            _Concession = Math.Round(value, 2);
        //            if (_Concession > 0)
        //                _ConcessionPercent = ((_Concession / _Quantity) * 100) / _Rate;
        //            else
        //                _ConcessionPercent = 0;


        //            OnPropertyChanged("Concession");
        //            OnPropertyChanged("ConcessionPercent");
        //            OnPropertyChanged("ServiceTaxAmount");
        //            OnPropertyChanged("TotalAmount");
        //            OnPropertyChanged("NetAmount");

        //        }
        //    }
        //}

        private double _ConcessionPercent;
        public double ConcessionPercent
        {
            get
            {
                //_ConcessionPercent = Math.Round(_ConcessionPercent, 2);
                //_ConcessionPercent = _ConcessionPercent;
                return _ConcessionPercent;
            }
            set
            {
                if (_ConcessionPercent != value)
                {
                    if (value < 0)
                        value = 0.0;

                    if (value > 100)
                        value = 100;
                    _ConcessionPercent = value;


                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _Concession;
        public double Concession
        {
            get
            {
                if (_ConcessionPercent > 0)
                    _Concession = (((_Rate * _Quantity) * _ConcessionPercent) / 100);
                else
                    _Concession = 0.0;

                _Concession = Math.Round(_Concession, 2);


                return _Concession;
            }
            set
            {
                if (_Concession != value)
                {
                    if (value < 0)
                        value = 0.0;
                    //if(_ConcessionAmount !=)

                    _Concession = Math.Round(value, 2);
                    if (_Concession > 0)

                        _ConcessionPercent = ((_Concession / _Quantity) * 100) / _Rate;


                    else
                        _ConcessionPercent = 0.0;


                    OnPropertyChanged("Concession");
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _ServiceTaxPercent;
        public double ServiceTaxPercent
        {
            get { return _ServiceTaxPercent; }
            set
            {
                if (_ServiceTaxPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _ServiceTaxPercent = value;
                    OnPropertyChanged("ServiceTaxPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _ServiceTaxAmount;
        public double ServiceTaxAmount
        {
            get
            {
                if (_ServiceTaxPercent > 0)
                    return _ServiceTaxAmount = ((_TotalAmount - Concession - _StaffDiscountAmount - _StaffParentDiscountAmount) * _ServiceTaxPercent / 100);
                else
                    return _ServiceTaxAmount = 0;
            }
            set
            {

                if (value < 0)
                    value = 0;

                _ServiceTaxAmount = value;
                if (_ServiceTaxAmount > 0 && (_TotalAmount - (_Concession * _Quantity)) > 0)
                    _ServiceTaxPercent = (_ServiceTaxAmount / _Quantity) / (_TotalAmount - _Concession);
                else
                    _ServiceTaxPercent = 0;

                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("ServiceTaxPercent");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("NetAmount");

            }
        }


        private double _StaffDiscountPercent;
        public double StaffDiscountPercent
        {
            get { return _StaffDiscountPercent; }
            set
            {
                if (_StaffDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _StaffDiscountPercent = value;
                    OnPropertyChanged("StaffDiscountPercent");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffDiscountAmount;
        public double StaffDiscountAmount
        {
            get
            {
                if (_StaffDiscountPercent != 0)
                    _StaffDiscountAmount = (((Rate * _StaffDiscountPercent) / 100));

                return _StaffDiscountAmount * _Quantity;
            }
            set
            {
                if (_StaffDiscountAmount * _Quantity != value)
                {
                    if (value < 0)
                        value = 0;

                    _StaffDiscountAmount = value;
                    _StaffDiscountPercent = 0;
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffDiscountPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffParentDiscountPercent;
        public double StaffParentDiscountPercent
        {
            get { return _StaffParentDiscountPercent; }
            set
            {
                if (_StaffParentDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _StaffParentDiscountPercent = value;
                    OnPropertyChanged("StaffParentDiscountPercent");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _StaffParentDiscountAmount;
        public double StaffParentDiscountAmount
        {
            get
            {
                if (_StaffParentDiscountPercent != 0)
                    _StaffParentDiscountAmount = ((_Rate * _StaffParentDiscountPercent) / 100);

                return _StaffParentDiscountAmount * _Quantity;
            }
            set
            {

                if (value < 0)
                    value = 0;

                _StaffParentDiscountAmount = value;

                _StaffParentDiscountPercent = 0;
                OnPropertyChanged("StaffParentDiscountAmount");
                OnPropertyChanged("StaffParentDiscountPercent");
                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("NetAmount");

            }
        }


        private double _Discount;
        public double Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount != value)
                {
                    if (value < 0)
                        value = 0;
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }
        }

        private double _StaffFree;
        public double StaffFree
        {
            get { return _StaffFree; }
            set
            {
                if (_StaffFree != value)
                {
                    _StaffFree = value;
                    OnPropertyChanged("StaffFree");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                //_NetAmount = _TotalAmount - Concession - StaffDiscountAmount - StaffParentDiscountAmount + ServiceTaxAmount;
                if (_TaxType == 2)  //Exclusive of TAX
                {
                    _NetAmount = _TotalAmount - Concession - StaffDiscountAmount - StaffParentDiscountAmount + TotalServiceTaxAmount; // GST Details added by Ashish Z. on dated 24062017
                }
                else if (_TaxType == 1)
                {
                    _NetAmount = _TotalAmount - Concession - StaffDiscountAmount - StaffParentDiscountAmount;
                }
                else
                {
                    _NetAmount = _TotalAmount - Concession - StaffDiscountAmount - StaffParentDiscountAmount + TotalServiceTaxAmount;
                }


                if (_NetAmount < 0) _NetAmount = 0;
                return _NetAmount;

            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _HospShareAmount;
        public double HospShareAmount
        {
            get { return _HospShareAmount; }
            set
            {
                if (_HospShareAmount != value)
                {
                    _HospShareAmount = value;
                    OnPropertyChanged("HospShareAmount");
                }
            }
        }

        private double _DoctorShareAmount;
        public double DoctorShareAmount
        {
            get { return _DoctorShareAmount; }
            set
            {
                if (_DoctorShareAmount != value)
                {
                    _DoctorShareAmount = value;
                    OnPropertyChanged("DoctorShareAmount");
                }
            }
        }

        private bool _isPackageService;
        public bool isPackageService
        {
            get { return _isPackageService; }
            set
            {
                if (_isPackageService != value)
                {
                    _isPackageService = value;
                    OnPropertyChanged("isPackageService");
                }
            }
        }

        private bool _IsPrescribedService = false;
        public bool IsPrescribedService
        {
            get { return _IsPrescribedService; }
            set
            {
                if (_IsPrescribedService != value)
                {
                    _IsPrescribedService = value;
                    OnPropertyChanged("IsPrescribedService");
                }
            }
        }

        private long _ChargeIDPackage;
        public long ChargeIDPackage
        {
            get { return _ChargeIDPackage; }
            set
            {
                if (_ChargeIDPackage != value)
                {
                    _ChargeIDPackage = value;
                    OnPropertyChanged("ChargeIDPackage");
                }
            }
        }

        private bool _Emergency;
        public bool Emergency
        {
            get { return _Emergency; }
            set
            {
                if (_Emergency != value)
                {
                    _Emergency = value;
                    OnPropertyChanged("Emergency");
                }
            }
        }

        private bool _SponsorType;
        public bool SponsorType
        {
            get { return _SponsorType; }
            set
            {
                if (_SponsorType != value)
                {
                    _SponsorType = value;
                    OnPropertyChanged("SponsorType");
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

        private long? _CancelledBy;
        public long? CancelledBy
        {
            get { return _CancelledBy; }
            set
            {
                if (_CancelledBy != value)
                {
                    _CancelledBy = value;
                    OnPropertyChanged("CancelledBy");
                }
            }
        }

        private DateTime? _CancelledDate;
        public DateTime? CancelledDate
        {
            get { return _CancelledDate; }
            set
            {
                if (_CancelledDate != value)
                {
                    _CancelledDate = value;
                    OnPropertyChanged("CancelledDate");
                }
            }
        }

        private string _CancellationRemark;
        public string CancellationRemark
        {
            get { return _CancellationRemark; }
            set
            {
                if (_CancellationRemark != value)
                {
                    _CancellationRemark = value;
                    OnPropertyChanged("CancellationRemark");
                }
            }
        }

        private bool _IsBilled;
        public bool IsBilled
        {
            get { return _IsBilled; }
            set
            {
                if (_IsBilled != value)
                {
                    _IsBilled = value;
                    OnPropertyChanged("IsBilled");
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

        public double BalanceAmount { get; set; }
      

        private bool _IsPackaged;
        public bool IsPackaged
        {
            get { return _IsPackaged; }
            set
            {
                if (_IsPackaged != value)
                {
                    _IsPackaged = value;
                    OnPropertyChanged("IsPackaged");
                }
            }
        }


        private long _ProcedureServiceId;
        public long ProcedureServiceId
        {
            get { return _ProcedureServiceId; }
            set
            {
                if (_ProcedureServiceId != value)
                {
                    _ProcedureServiceId = value;
                    OnPropertyChanged("ProcedureServiceId");
                }
            }
        }

        private short _NewPackageService;
        public short NewPackageService
        {
            get { return _NewPackageService; }
            set
            {
                if (_NewPackageService != value)
                {
                    _NewPackageService = value;
                    OnPropertyChanged("NewPackageService");
                }
            }
        }

        private double _CompRate;
        public double CompRate
        {
            get { return _CompRate; }
            set
            {
                if (_CompRate != value)
                {
                    _CompRate = value;
                    OnPropertyChanged("CompRate");
                }
            }
        }

        private double _CompDisc;
        public double CompDisc
        {
            get { return _CompDisc; }
            set
            {
                if (_CompDisc != value)
                {
                    _CompDisc = value;
                    OnPropertyChanged("CompDisc");
                }
            }
        }

        private double _PatientDeductible;
        public double PatientDeductible
        {
            get { return _PatientDeductible; }
            set
            {
                if (_PatientDeductible != value)
                {
                    _PatientDeductible = value;
                    OnPropertyChanged("PatientDeductible");
                }
            }
        }

        private string _AuthorizationNo;
        public string AuthorizationNo
        {
            get { return _AuthorizationNo; }
            set
            {
                if (_AuthorizationNo != value)
                {
                    _AuthorizationNo = value;
                    OnPropertyChanged("AuthorizationNo");
                }
            }
        }

        private long _ClassId;
        public long ClassId
        {
            get { return _ClassId; }
            set
            {
                if (_ClassId != value)
                {
                    _ClassId = value;
                    OnPropertyChanged("ClassId");
                }
            }
        }

        private double _CompanyRate;
        public double CompanyRate
        {
            get { return _CompanyRate; }
            set
            {
                if (_CompanyRate != value)
                {
                    _CompanyRate = value;
                    OnPropertyChanged("CompanyRate");
                }
            }
        }

        private double _DiscountPerc;
        public double DiscountPerc
        {
            get { return _DiscountPerc; }
            set
            {
                if (_DiscountPerc != value)
                {
                    if (value < 0)
                        value = 0;
                    _DiscountPerc = value;
                    OnPropertyChanged("DiscountPerc");
                }
            }
        }

        private double _DiscountAmt;
        public double DiscountAmt
        {
            get { return _DiscountAmt; }
            set
            {
                if (_DiscountAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _DiscountAmt = value;
                    OnPropertyChanged("DiscountAmt");
                }
            }
        }

        private double _DeductionPerc;
        public double DeductionPerc
        {
            get { return _DeductionPerc; }
            set
            {
                if (_DeductionPerc != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductionPerc = value;
                    OnPropertyChanged("DeductionPerc");
                }
            }
        }

        private double _DeductionAmt;
        public double DeductionAmt
        {
            get { return _DeductionAmt; }
            set
            {
                if (_DeductionAmt != value)
                {
                    if (value < 0)
                        value = 0;
                    _DeductionAmt = value;
                    OnPropertyChanged("DeductionAmt");
                }
            }
        }

        private double _Deductable;
        public double Deductable
        {
            get { return _Deductable; }
            set
            {
                if (_Deductable != value)
                {
                    _Deductable = value;
                    OnPropertyChanged("Deductable");
                }
            }
        }

        private double _NetAmt;
        public double NetAmt
        {
            get { return _NetAmt; }
            set
            {
                if (_NetAmt != value)
                {
                    _NetAmt = value;
                    OnPropertyChanged("NetAmt");
                }
            }
        }

        public bool RateEditable { get; set; }

        //Added by PMG

        public bool IsDefaultService { get; set; }


        public double MaxRate { get; set; }

        public double MinRate { get; set; }

        //public bool FirstApprovalChecked { get; set; }
        //public bool SecondApprovalChecked { get; set; }
        //public bool IsFirstApproval { get; set; }

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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedWindowsLoginName;
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

        public bool SelectCharge { get; set; }

        public bool FirstApprovalEnable { get; set; }
        public bool SecondApprovalEnable { get; set; }

        public bool FirstApprovalChecked { get; set; }
        public bool SecondApprovalChecked { get; set; }

        //public bool IsFirstApproval { get; set; }
        //public bool IsSecondApproval { get; set; }

        public bool IsSetForApproval { get; set; }

        private long _RefundID;
        public long RefundID
        {
            get { return _RefundID; }
            set
            {
                if (_RefundID != value)
                {
                    _RefundID = value;
                    OnPropertyChanged("RefundID");
                }
            }
        }

        //By Anjali....................

        private long _LevelID;
        public long LevelID
        {
            get { return _LevelID; }
            set
            {
                if (_LevelID != value)
                {
                    _LevelID = value;
                    OnPropertyChanged("LevelID");
                }
            }
        }

        private bool _IsFromApprovedRequest;
        public bool IsFromApprovedRequest
        {
            get { return _IsFromApprovedRequest; }
            set
            {
                if (_IsFromApprovedRequest != value)
                {
                    _IsFromApprovedRequest = value;
                    OnPropertyChanged("IsFromApprovedRequest");
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


        #endregion

        #region For IPD Module



        private long _CompanyID;
        public long CompanyID
        {
            get { return _CompanyID; }
            set
            {
                if (_CompanyID != value)
                {
                    _CompanyID = value;
                    OnPropertyChanged("CompanyID");
                }
            }
        }

        private string _IsSelfAmountTextBoxVisible;
        public string IsSelfAmountTextBoxVisible
        {
            get { return _IsSelfAmountTextBoxVisible; }
            set
            {
                if (_IsSelfAmountTextBoxVisible != value)
                {
                    if (IsSelfCompany)
                    {
                        _IsSelfAmountTextBoxVisible = "Collapsed";
                    }
                    else
                    {
                        _IsSelfAmountTextBoxVisible = "Visible";
                    }
                    OnPropertyChanged("IsSelfAmountTextBoxVisible");
                }
            }
        }
        private string _IsSelfAmountTextBlockVisible;
        public string IsSelfAmountTextBlockVisible
        {
            get { return _IsSelfAmountTextBlockVisible; }
            set
            {
                if (_IsSelfAmountTextBlockVisible != value)
                {
                    if (IsSelfCompany)
                    {
                        _IsSelfAmountTextBlockVisible = "Visible";
                    }
                    else
                    {
                        _IsSelfAmountTextBlockVisible = "Collapsed";
                    }
                    OnPropertyChanged("IsSelfAmountTextBlockVisible");
                }
            }
        }
        private bool _IsSelfCompany;
        public bool IsSelfCompany
        {
            get { return _IsSelfCompany; }
            set
            {
                _IsSelfCompany = value;

                if (_IsSelfCompany)
                {
                    _IsSelfAmountTextBoxVisible = "Collapsed";
                    _IsSelfAmountTextBlockVisible = "Visible";
                }
                else
                {
                    _IsSelfAmountTextBoxVisible = "Visible";
                    _IsSelfAmountTextBlockVisible = "Collapsed";
                }

                OnPropertyChanged("IsSelfCompany");
                OnPropertyChanged("IsSelfAmountTextBlockVisible");
                OnPropertyChanged("IsSelfAmountTextBoxVisible");
            }
        }

        private double _TaxAmount;
        public double TaxAmount
        {
            get { return _TaxAmount; }
            set
            {
                if (_TaxAmount != value)
                {
                    _TaxAmount = value;
                    OnPropertyChanged("TaxAmount");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get
            {
                if (_ConcessionPercent > 0)
                {
                    _ConcessionAmount = ((_Rate * _ConcessionPercent) / 100);
                    _ConcessionAmount = Math.Round(_ConcessionAmount, 2);
                    return Math.Round(_ConcessionAmount * _Quantity, 2);
                }
                else
                {
                    return _ConcessionAmount = 0;
                }
            }
            set
            {
                if (value < 0)
                    value = 0;
                if (_ConcessionAmount != value)
                {
                    _ConcessionAmount = value;
                    if (_ConcessionAmount > 0)
                    {
                        _ConcessionPercent = ((_ConcessionAmount / _Quantity) * 100) / _Rate;
                    }
                    else
                    {
                        _ConcessionPercent = 0;
                    }
                    NetAmount = (_TotalAmount - _ConcessionAmount) + _TaxAmount;
                    TaxAmount = _TaxAmount;
                    NetAmt = NetAmount;
                    OnPropertyChanged("ConcessionAmount");
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }

            }
        }

        private double _ActualRate;
        public double ActualRate
        {
            get { return _ActualRate; }
            set
            {
                if (_ActualRate != value)
                {
                    _ActualRate = value;
                    OnPropertyChanged("ActualRate");
                }
            }
        }

        private double _SelfBalance;
        public double SelfBalance
        {
            get { return _SelfBalance; }
            set
            {
                if (_SelfBalance != value)
                {
                    _SelfBalance = value;
                    OnPropertyChanged("_SelfBalance");
                }
            }
        }

        private int _RowID;
        public int RowID
        {
            get { return _RowID; }
            set
            {
                if (_RowID != value)
                {
                    _RowID = value;
                    OnPropertyChanged("RowID");
                }
            }
        }

        private List<clsTaxBuilderVO> _TaxMasterList;
        public List<clsTaxBuilderVO> TaxMasterList
        {
            get
            {
                return _TaxMasterList;
            }
            set
            {
                if (_TaxMasterList != value)
                {
                    _TaxMasterList = value;
                }
            }
        }

        private string _IsRateEditableEnable;
        public string IsRateEditableEnable
        {
            get { return _IsRateEditableEnable; }
            set
            {
                if (_IsRateEditableEnable != value)
                {
                    _IsRateEditableEnable = value;
                    OnPropertyChanged("IsRateEditableEnable");
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

        // Added  by CDS 
        public bool IsAdjustableHead { get; set; }

        public bool IsConsiderAdjustable { get; set; }

        public bool IsTemp { get; set; }


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

        // END

        private string _PackageName;
        public string PackageName
        {
            get { return _PackageName; }
            set
            {
                if (value != _PackageName)
                {
                    _PackageName = value;
                    OnPropertyChanged("PackageName");
                }
            }
        }

        private bool _IsServiceAsPackage;
        public bool IsServiceAsPackage
        {
            get { return _IsServiceAsPackage; }
            set
            {
                if (_IsServiceAsPackage != value)
                {
                    _IsServiceAsPackage = value;
                    OnPropertyChanged("IsServiceAsPackage");
                }
            }
        }

        private bool _IsServiceAsHealthPlan;
        public bool IsServiceAsHealthPlan
        {
            get { return _IsServiceAsHealthPlan; }
            set
            {
                if (_IsServiceAsHealthPlan != value)
                {
                    _IsServiceAsHealthPlan = value;
                    OnPropertyChanged("IsServiceAsHealthPlan");
                }
            }
        }

        private short _Opd_Ipd;
        public short Opd_Ipd
        {
            get { return _Opd_Ipd; }
            set
            {
                if (_Opd_Ipd != value)
                {
                    _Opd_Ipd = value;
                    OnPropertyChanged("Opd_Ipd");
                }
            }
        }

        private long _Opd_Ipd_Id;
        public long Opd_Ipd_Id
        {
            get { return _Opd_Ipd_Id; }
            set
            {
                if (_Opd_Ipd_Id != value)
                {
                    _Opd_Ipd_Id = value;
                    OnPropertyChanged("Opd_Ipd_Id");
                }
            }
        }

        private long _Opd_Ipd_UnitId;
        public long Opd_Ipd_UnitId
        {
            get { return _Opd_Ipd_UnitId; }
            set
            {
                if (_Opd_Ipd_UnitId != value)
                {
                    _Opd_Ipd_UnitId = value;
                    OnPropertyChanged("Opd_Ipd_UnitId");
                }
            }
        }

        private string _IsRateEditableDisable;
        public string IsRateEditableDisable
        {
            get { return _IsRateEditableDisable; }
            set
            {
                if (_IsRateEditableDisable != value)
                {
                    _IsRateEditableDisable = value;
                    OnPropertyChanged("IsRateEditableDisable");
                }
            }
        }

        private List<clsChargeVO> _SubChargesList = null;
        public List<clsChargeVO> SubChargesList
        {
            get { return _SubChargesList; }
            set
            {
                _SubChargesList = value;

            }
        }


        private double _SelfAmount;
        public double SelfAmount
        {
            get { return _SelfAmount; }
            set
            {
                if (_SelfAmount != value)
                {
                    _SelfAmount = value;

                    if (_SelfAmount > _NetAmount)
                    {
                        _SelfAmount = 0;
                        NonSelfAmount = _NetAmount;
                    }
                    else if (_SelfAmount <= _NetAmount)
                    {
                        NonSelfAmount = _NetAmount - _SelfAmount;
                    }

                    OnPropertyChanged("SelfAmount");
                }
            }
        }


        private double _NonSelfAmount;
        public double NonSelfAmount
        {
            get { return _NonSelfAmount; }
            set
            {
                if (_NonSelfAmount != value)
                {
                    _NonSelfAmount = value;

                    if (_NonSelfAmount > _NetAmount)
                    {
                        _NonSelfAmount = 0;
                        SelfAmount = _NetAmount;
                    }
                    else if (_NonSelfAmount <= _NetAmount)
                    {
                        SelfAmount = _NetAmount - _NonSelfAmount;
                    }

                    OnPropertyChanged("NonSelfAmount");
                }
            }
        }


        #region For Multi sponser in single bill

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
        #endregion


        private double _NonSelfBalance;
        public double NonSelfBalance
        {
            get { return _NonSelfBalance; }
            set
            {
                if (_NonSelfBalance != value)
                {
                    _NonSelfBalance = value;
                    OnPropertyChanged("NonSelfBalance");
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

        private string _MainService;
        public string Service
        {
            get { return _MainService; }
            set
            {
                if (value != _MainService)
                {
                    _MainService = value;
                    OnPropertyChanged("MainService");
                }
            }
        }

        private long _TariffId;
        public long TariffId
        {
            get { return _TariffId; }
            set
            {
                if (_TariffId != value)
                {
                    _TariffId = value;
                    OnPropertyChanged("TariffId");
                }
            }
        }

        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set
            {
                if (_ClassName != value)
                {
                    _ClassName = value;
                    OnPropertyChanged("ClassName");
                }
            }
        }

        private string _IsBillOptionEnable;
        public string IsBillOptionEnable
        {
            get { return _IsBillOptionEnable; }
            set
            {
                if (_IsBillOptionEnable != value)
                {
                    _IsBillOptionEnable = value;
                    OnPropertyChanged("IsBillOptionEnable");
                }
            }
        }

        private string _IsServiceOptionEnable;
        public string IsServiceOptionEnable
        {
            get { return _IsServiceOptionEnable; }
            set
            {
                if (_IsServiceOptionEnable != value)
                {
                    _IsServiceOptionEnable = value;
                    OnPropertyChanged("IsServiceOptionEnable");
                }
            }
        }


        public bool ChildPackageService { get; set; }


        private double _TotalNetAmount;
        public double TotalNetAmount
        {
            get { return _TotalNetAmount; }
            set
            {
                if (_TotalNetAmount != value)
                {
                    _TotalNetAmount = value;
                    OnPropertyChanged("TotalNetAmount");
                }
            }
        }

        private double _TotalServicePaidAmount;
        public double TotalServicePaidAmount
        {
            get { return _TotalServicePaidAmount; }
            set
            {
                if (_TotalServicePaidAmount != value)
                {
                    _TotalServicePaidAmount = value;
                    OnPropertyChanged("TotalServicePaidAmount");
                }
            }
        }

        private double _ServicePaidAmount;
        public double ServicePaidAmount
        {
            get { return _ServicePaidAmount; }
            set
            {
                if (_ServicePaidAmount != value)
                {
                    _ServicePaidAmount = value;
                    OnPropertyChanged("ServicePaidAmount");
                }
            }
        }

        private double _TotalConcession;
        public double TotalConcession
        {
            get { return _TotalConcession; }
            set
            {
                if (_TotalConcession != value)
                {
                    _TotalConcession = value;
                    OnPropertyChanged("TotalConcession");
                }
            }
        }

        private bool _IsEnable;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                if (_IsEnable != value)
                {
                    _IsEnable = value;
                    OnPropertyChanged("IsEnable");
                }
            }
        }

        public bool IsSameDate { get; set; }

        public long ParentID { get; set; }

        private double _PackagePaidAmount;  //***//
        public double PackagePaidAmount
        {
            get { return _PackagePaidAmount; }
            set { _PackagePaidAmount = value; }
        }
        public long PackageBillID { get; set; }
        public long PackageBillUnitID { get; set; }

        public bool IsUpdate { get; set; }

        private double _SettleNetAmount;
        public double SettleNetAmount
        {
            get { return _SettleNetAmount; }
            set
            {
                if (_SettleNetAmount != value)
                {
                    _SettleNetAmount = value;
                    OnPropertyChanged("SettleNetAmount");
                }
            }
        }

        private double _TotalConcessionPercentage;
        public double TotalConcessionPercentage
        {
            get { return _TotalConcessionPercentage; }
            set
            {
                if (_TotalConcessionPercentage != value)
                {
                    _TotalConcessionPercentage = value;
                    OnPropertyChanged("TotalConcessionPercentage");
                }
            }
        }

        List<MasterListItem> _DoctorList = new List<MasterListItem>();
        public List<MasterListItem> DoctorList
        {
            get
            {
                return _DoctorList;
            }
            set
            {
                if (value != _DoctorList)
                {
                    _DoctorList = value;
                }
            }

        }

        //List<MasterListItem> _Doctor = new List<MasterListItem>();
        //public List<MasterListItem> DoctorList
        //{
        //    get
        //    {
        //        return _Doctor;
        //    }
        //    set
        //    {
        //        if (value != _Doctor)
        //        {
        //            _Doctor = value;
        //        }
        //    }

        //}

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

        private double _GrossDiscountPercentage;
        public double GrossDiscountPercentage
        {
            get { return _GrossDiscountPercentage; }
            set
            {
                if (_GrossDiscountPercentage != value)
                {
                    _GrossDiscountPercentage = value;
                    OnPropertyChanged("GrossDiscountPercentage");
                }
            }
        }

        //public long ServiceSpecilizationID { get; set; }
        public long ServiceSubSpecilizationID { get; set; }

        public bool ChkID { get; set; }

        private clsChargeDetailsVO _ChargeDetails;
        public clsChargeDetailsVO ChargeDetails
        {
            get { return _ChargeDetails; }
            set
            {
                if (_ChargeDetails != value)
                {
                    _ChargeDetails = value;
                    OnPropertyChanged("ChargeDetails");
                }
            }
        }

        private double _RefundAmount;
        public double RefundAmount
        {
            get { return _RefundAmount; }
            set
            {
                if (_RefundAmount != value)
                {
                    _RefundAmount = value;
                    OnPropertyChanged("RefundAmount");
                }
            }
        }

        //Added By Bhushanp 12072017
        private double _RefundedAmount;
        public double RefundedAmount
        {
            get { return _RefundedAmount; }
            set
            {
                if (_RefundedAmount != value)
                {
                    _RefundedAmount = value;
                    OnPropertyChanged("RefundedAmount");
                }
            }
        }

        private double _TotalRefundAmount;

        public double TotalRefundAmount
        {
            get { return _TotalRefundAmount; }
            set
            {
                if (_TotalRefundAmount != value)
                {
                    _TotalRefundAmount = value;
                    OnPropertyChanged("TotalRefundAmount");
                }
            }
        }


        private bool _IsSendForApproval;
        public bool IsSendForApproval
        {
            get { return _IsSendForApproval; }
            set
            {
                if (_IsSendForApproval != value)
                {
                    _IsSendForApproval = value;
                    OnPropertyChanged("IsSendForApproval");
                }
            }
        }
        private bool? _IsChargeApproved;
        public bool? IsChargeApproved
        {
            get { return _IsChargeApproved; }
            set
            {
                if (_IsChargeApproved != value)
                {
                    _IsChargeApproved = value;
                    OnPropertyChanged("IsChargeApproved");
                }
            }
        }


        private long _ApprovalRequestID;
        public long ApprovalRequestID
        {
            get { return _ApprovalRequestID; }
            set
            {
                if (_ApprovalRequestID != value)
                {
                    _ApprovalRequestID = value;
                    OnPropertyChanged("ApprovalRequestID");
                }
            }
        }
        private long _ApprovalRequestUnitID;
        public long ApprovalRequestUnitID
        {
            get { return _ApprovalRequestUnitID; }
            set
            {
                if (_ApprovalRequestUnitID != value)
                {
                    _ApprovalRequestUnitID = value;
                    OnPropertyChanged("ApprovalRequestUnitID");
                }
            }
        }


        private long _ApprovedRequestID;
        public long ApprovedRequestID
        {
            get { return _ApprovedRequestID; }
            set
            {
                if (_ApprovedRequestID != value)
                {
                    _ApprovedRequestID = value;
                    OnPropertyChanged("ApprovedRequestID");
                }
            }
        }
        private long _ApprovedRequestUnitID;
        public long ApprovedRequestUnitID
        {
            get { return _ApprovedRequestUnitID; }
            set
            {
                if (_ApprovedRequestUnitID != value)
                {
                    _ApprovedRequestUnitID = value;
                    OnPropertyChanged("ApprovedRequestUnitID");
                }
            }
        }

        private long _ApprovalRequestDetailsID;
        public long ApprovalRequestDetailsID
        {
            get { return _ApprovalRequestDetailsID; }
            set
            {
                if (_ApprovalRequestDetailsID != value)
                {
                    _ApprovalRequestDetailsID = value;
                    OnPropertyChanged("ApprovalRequestDetailsID");
                }
            }
        }

        private long _ApprovalRequestDetailsUnitID;
        public long ApprovalRequestDetailsUnitID
        {
            get { return _ApprovalRequestDetailsUnitID; }
            set
            {
                if (_ApprovalRequestDetailsUnitID != value)
                {
                    _ApprovalRequestDetailsUnitID = value;
                    OnPropertyChanged("ApprovalRequestDetailsUnitID");
                }
            }
        }



        private bool _IsAutoCharge = false;
        public bool IsAutoCharge
        {
            get
            {
                return _IsAutoCharge;
            }
            set
            {
                _IsAutoCharge = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsAutoCharge"));
                }
            }
        }

        private string _ApprovalRemark;
        public string ApprovalRemark
        {
            get { return _ApprovalRemark; }
            set
            {
                if (_ApprovalRemark != value)
                {
                    _ApprovalRemark = value;
                    OnPropertyChanged("ApprovalRemark");
                }
            }
        }
        private bool _ApprovalStatus;
        public bool ApprovalStatus
        {
            get { return _ApprovalStatus; }
            set
            {
                if (_ApprovalStatus != value)
                {
                    _ApprovalStatus = value;
                    OnPropertyChanged("ApprovalStatus");
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

        private bool _IsRefund;

        public bool IsRefund
        {
            get { return _IsRefund; }
            set { _IsRefund = value; }
        }

        //Added By Bhushanp For Package Consumption Date 08082017
        private bool _IsConsumption;

        public bool IsConsumption
        {
            get { return _IsConsumption; }
            set { _IsConsumption = value; }
        }
        private bool _IsPackageConsumption;
        public bool IsPackageConsumption
        {
            get { return _IsPackageConsumption; }
            set { _IsPackageConsumption = value; }
        }
        #region GST Details added by Ashish Z. on dated 24062017
        private double _TotalServiceTaxAmount;
        public double TotalServiceTaxAmount
        {
            get { return _TotalServiceTaxAmount; }
            set
            {
                if (_TotalServiceTaxAmount != value)
                {
                    if (value < 0)
                        value = 0;

                    _TotalServiceTaxAmount = value;
                    OnPropertyChanged("TotalServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private clsChargeTaxDetailsVO _ChargeTaxDetailsVO;
        public clsChargeTaxDetailsVO ChargeTaxDetailsVO
        {
            get { return _ChargeTaxDetailsVO; }
            set
            {
                if (_ChargeTaxDetailsVO != value)
                {
                    _ChargeTaxDetailsVO = value;
                    OnPropertyChanged("ChargeTaxDetailsVO");
                }
            }
        }

        private List<clsChargeTaxDetailsVO> _ChargeTaxDetailsList;
        public List<clsChargeTaxDetailsVO> ChargeTaxDetailsList
        {
            get
            {
                return _ChargeTaxDetailsList;
            }
            set
            {
                if (_ChargeTaxDetailsList != value)
                {
                    _ChargeTaxDetailsList = value;
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
                    OnPropertyChanged("TaxType");
                }
            }
        }

        private bool _IsEditTax = false;
        public bool IsEditTax
        {
            get { return _IsEditTax; }
            set
            {
                if (_IsEditTax != value)
                {
                    _IsEditTax = value;
                    OnPropertyChanged("IsEditTax");
                }
            }
        }
        #endregion

        # region Costing Divisions for Clinical & Pharmacy Billing

        private long _CostingDivisionID;
        public long CostingDivisionID
        {
            get
            {
                return _CostingDivisionID;
            }

            set
            {
                if (value != _CostingDivisionID)
                {
                    _CostingDivisionID = value;
                    OnPropertyChanged("CostingDivisionID");
                }
            }
        }

        # endregion


        #region Properties use for unfreezed Patho,Radio services  Added By CDS

        private long _POBID;
        public long POBID
        {
            get { return _POBID; }
            set
            {
                if (_POBID != value)
                {
                    _POBID = value;
                    OnPropertyChanged("POBID");
                }
            }
        }

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

        private long _ROBID;
        public long ROBID
        {
            get { return _ROBID; }
            set
            {
                if (_ROBID != value)
                {
                    _ROBID = value;
                    OnPropertyChanged("ROBID");
                }
            }
        }

        private long _ROBDID;
        public long ROBDID
        {
            get { return _ROBDID; }
            set
            {
                if (_ROBDID != value)
                {
                    _ROBDID = value;
                    OnPropertyChanged("ROBDID");
                }
            }
        }

        private bool _IsReportCollected;
        public bool IsReportCollected
        {
            get { return _IsReportCollected; }
            set
            {
                if (_IsReportCollected != value)
                {
                    _IsReportCollected = value;
                    OnPropertyChanged("IsReportCollected");
                }
            }
        }

        private bool _IsResultEntry; //Added By Yogesh K
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

        private bool _Isupload;//Added By Yogesh K
        public bool Isupload
        {
            get { return _Isupload; }
            set
            {
                if (_Isupload != value)
                {
                    _Isupload = value;
                    OnPropertyChanged("Isupload");
                }
            }
        }

        #endregion

        # region Property used to identify that service is add from Conditional Service Search

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

        #endregion

        #endregion


        private long _BillUnitID;
        public long BillUnitID
        {
            get { return _BillUnitID; }
            set
            {
                if (_BillUnitID != value)
                {
                    _BillUnitID = value;
                    OnPropertyChanged("BillUnitID");
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

        // Added By Bhushanp 22032017 For Refund

        private bool _IsAgainstBill;

        public bool IsAgainstBill
        {
            get { return _IsAgainstBill; }
            set { _IsAgainstBill = value; }
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


        #region Refund Reason
        List<MasterListItem> _RefundReason = new List<MasterListItem>();
        public List<MasterListItem> RefundReason
        {
            get
            {
                return _RefundReason;
            }

            set
            {
                if (value != _RefundReason)
                {
                    _RefundReason = value;

                }
            }


        }

        MasterListItem _SelectedApprovalRefundReason = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedApprovalRefundReason
        {
            get
            {
                return _SelectedApprovalRefundReason;

            }
            set
            {
                if (value != _SelectedApprovalRefundReason)
                {
                    _SelectedApprovalRefundReason = value;
                    OnPropertyChanged("SelectedApprovalRefundReason");
                }
            }
        }

        private long _ApprovalRefundReasonID;
        public long ApprovalRefundReasonID
        {
            get { return _ApprovalRefundReasonID; }
            set
            {
                if (_ApprovalRefundReasonID != value)
                {
                    _ApprovalRefundReasonID = value;
                    OnPropertyChanged("ApprovalRefundReasonID");
                }
            }
        }

        MasterListItem _SelectedRefundReason = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedRefundReason
        {
            get
            {
                return _SelectedRefundReason;

            }
            set
            {
                if (value != _SelectedRefundReason)
                {
                    _SelectedRefundReason = value;
                    OnPropertyChanged("SelectedRefundReason");
                }
            }
        }

        private long _RefundReasonID;
        public long RefundReasonID
        {
            get { return _RefundReasonID; }
            set
            {
                if (_RefundReasonID != value)
                {
                    _RefundReasonID = value;
                    OnPropertyChanged("RefundReasonID");
                }
            }
        }

        MasterListItem _SelectedRequestRefundReason = new MasterListItem { ID = 0, Description = "--Select--" };
        public MasterListItem SelectedRequestRefundReason
        {
            get
            {
                return _SelectedRequestRefundReason;

            }
            set
            {
                if (value != _SelectedRequestRefundReason)
                {
                    _SelectedRequestRefundReason = value;
                    OnPropertyChanged("SelectedRequestRefundReason");
                }
            }
        }

        private long _RequestRefundReasonID;
        public long RequestRefundReasonID
        {
            get { return _RequestRefundReasonID; }
            set
            {
                if (_RequestRefundReasonID != value)
                {
                    _RequestRefundReasonID = value;
                    OnPropertyChanged("RequestRefundReasonID");
                }
            }
        }
        #endregion

        #region Package New Changes for Procedure Added on 21042018

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

        private double _PackageConcessionPercent;       //  For Package New Changes Added on 14062018
        public double PackageConcessionPercent
        {
            get
            {
                //_ConcessionPercent = Math.Round(_ConcessionPercent, 2);
                //_ConcessionPercent = _ConcessionPercent;
                return _PackageConcessionPercent;
            }
            set
            {
                if (_PackageConcessionPercent != value)
                {
                    if (value < 0)
                        value = 0.0;

                    if (value > 100)
                        value = 100;
                    _PackageConcessionPercent = value;


                    OnPropertyChanged("PackageConcession");
                    OnPropertyChanged("PackageConcessionPercent");
                    //OnPropertyChanged("ServiceTaxAmount");
                    //OnPropertyChanged("TotalAmount");
                    //OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _PackageConcession;          //  For Package New Changes Added on 14062018
        public double PackageConcession
        {
            get
            {
                if (_PackageConcessionPercent > 0)
                    _PackageConcession = (((_Rate * _Quantity) * _PackageConcessionPercent) / 100);
                else
                    _PackageConcession = 0.0;

                _PackageConcession = Math.Round(_PackageConcession, 2);


                return _PackageConcession;
            }
            set
            {
                if (_PackageConcession != value)
                {
                    if (value < 0)
                        value = 0.0;
                    //if(_ConcessionAmount !=)

                    _PackageConcession = Math.Round(value, 2);
                    if (_PackageConcession > 0)

                        _PackageConcessionPercent = ((_PackageConcession / _Quantity) * 100) / _Rate;


                    else
                        _PackageConcessionPercent = 0.0;


                    OnPropertyChanged("PackageConcession");
                    OnPropertyChanged("PackageConcessionPercent");
                    //OnPropertyChanged("ServiceTaxAmount");
                    //OnPropertyChanged("TotalAmount");
                    //OnPropertyChanged("NetAmount");

                }
            }
        }

        // For Package New Changes Added on 19062018
        public Int32 AdjustableHeadType { get; set; }

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
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }


    public class clsChargeDetailsVO : IValueObject, INotifyPropertyChanged
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

        private long _ChargeUnitID;
        public long ChargeUnitID
        {
            get { return _ChargeUnitID; }
            set
            {
                if (_ChargeUnitID != value)
                {
                    _ChargeUnitID = value;
                    OnPropertyChanged("ChargeUnitID");
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

        private DateTime? _Date;
        public DateTime? Date
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

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    if (value < 0)
                        value = 0;

                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {

                    if (value <= 0)
                        value = 1;

                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("ServiceTaxPercent");
                    // OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get
            {
                _TotalAmount = _Quantity * _Rate;
                return _TotalAmount;

            }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                    if (_TotalAmount == 0)
                        OnPropertyChanged("Concession");
                    OnPropertyChanged("NetAmount");

                }
            }
        }


        private double _ConcessionPercent;
        public double ConcessionPercent
        {
            get
            {
                _ConcessionPercent = Math.Round(_ConcessionPercent, 2);
                return _ConcessionPercent;
            }
            set
            {
                if (_ConcessionPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _ConcessionPercent = value;


                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _Concession;
        public double Concession
        {
            get
            {
                if (_ConcessionPercent > 0)
                    _Concession = (((_Rate * _Quantity) * _ConcessionPercent) / 100);
                else
                    _Concession = 0;

                // _Concession = Math.Round(_Concession, 2);

                _Concession = Math.Round(_Concession);

                return _Concession;
            }
            set
            {
                if (_Concession != value)
                {
                    if (value < 0)
                        value = 0;
                    //if(_ConcessionAmount !=)
                    _Concession = Math.Round(value, 2);
                    if (_Concession > 0)
                        _ConcessionPercent = ((_Concession / _Quantity) * 100) / _Rate;
                    else
                        _ConcessionPercent = 0;


                    OnPropertyChanged("Concession");
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _ServiceTaxPercent;
        public double ServiceTaxPercent
        {
            get { return _ServiceTaxPercent; }
            set
            {
                if (_ServiceTaxPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _ServiceTaxPercent = value;
                    OnPropertyChanged("ServiceTaxPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _ServiceTaxAmount;
        public double ServiceTaxAmount
        {
            get
            {
                if (_ServiceTaxPercent > 0)
                    return _ServiceTaxAmount = ((_TotalAmount - Concession - _StaffDiscountAmount - _StaffParentDiscountAmount) * _ServiceTaxPercent / 100);
                else
                    return _ServiceTaxAmount = 0;
            }
            set
            {

                if (value < 0)
                    value = 0;

                _ServiceTaxAmount = value;
                if (_ServiceTaxAmount > 0 && (_TotalAmount - (_Concession * _Quantity)) > 0)
                    _ServiceTaxPercent = (_ServiceTaxAmount / _Quantity) / (_TotalAmount - _Concession);
                else
                    _ServiceTaxPercent = 0;

                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("ServiceTaxPercent");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("NetAmount");

            }
        }


        private double _StaffDiscountPercent;
        public double StaffDiscountPercent
        {
            get { return _StaffDiscountPercent; }
            set
            {
                if (_StaffDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _StaffDiscountPercent = value;
                    OnPropertyChanged("StaffDiscountPercent");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffDiscountAmount;
        public double StaffDiscountAmount
        {
            get
            {
                if (_StaffDiscountPercent != 0)
                    _StaffDiscountAmount = (((Rate * _StaffDiscountPercent) / 100));

                return _StaffDiscountAmount * _Quantity;
            }
            set
            {
                if (_StaffDiscountAmount * _Quantity != value)
                {
                    if (value < 0)
                        value = 0;

                    _StaffDiscountAmount = value;
                    _StaffDiscountPercent = 0;
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffDiscountPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffParentDiscountPercent;
        public double StaffParentDiscountPercent
        {
            get { return _StaffParentDiscountPercent; }
            set
            {
                if (_StaffParentDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _StaffParentDiscountPercent = value;
                    OnPropertyChanged("StaffParentDiscountPercent");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffParentDiscountAmount;
        public double StaffParentDiscountAmount
        {
            get
            {
                if (_StaffParentDiscountPercent != 0)
                    _StaffParentDiscountAmount = ((_Rate * _StaffParentDiscountPercent) / 100);

                return _StaffParentDiscountAmount * _Quantity;
            }
            set
            {

                if (value < 0)
                    value = 0;

                _StaffParentDiscountAmount = value;

                _StaffParentDiscountPercent = 0;
                OnPropertyChanged("StaffParentDiscountAmount");
                OnPropertyChanged("StaffParentDiscountPercent");
                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("NetAmount");

            }
        }


        private double _Discount;
        public double Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount != value)
                {
                    if (value < 0)
                        value = 0;
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }
        }

        private double _StaffFree;
        public double StaffFree
        {
            get { return _StaffFree; }
            set
            {
                if (_StaffFree != value)
                {
                    _StaffFree = value;
                    OnPropertyChanged("StaffFree");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                _NetAmount = _TotalAmount - Concession - StaffDiscountAmount - StaffParentDiscountAmount + ServiceTaxAmount;
                if (_NetAmount < 0) _NetAmount = 0;
                return _NetAmount;
            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        //Addedby Priyanka On 04April2012
        private double _ServicePaidAmount;
        public double ServicePaidAmount
        {
            get { return _ServicePaidAmount; }
            set
            {
                if (_ServicePaidAmount != value)
                {
                    _ServicePaidAmount = value;
                    OnPropertyChanged("ServicePaidAmount");
                }
            }
        }

        private double _TotalServicePaidAmount;
        public double TotalServicePaidAmount
        {
            get { return _TotalServicePaidAmount; }
            set
            {
                if (_TotalServicePaidAmount != value)
                {
                    _TotalServicePaidAmount = value;
                    OnPropertyChanged("TotalServicePaidAmount");
                }
            }
        }


        private double _TotalConcession;
        public double TotalConcession
        {
            get { return _TotalConcession; }
            set
            {
                if (_TotalConcession != value)
                {
                    _TotalConcession = value;
                    OnPropertyChanged("TotalConcession");
                }
            }
        }


        public bool ChkID { get; set; }
        //End


        private double _BalanceAmount;
        public double BalanceAmount
        {
            get { return _BalanceAmount; }
            set
            {
                if (_BalanceAmount != value)
                {
                    _BalanceAmount = value;
                    OnPropertyChanged("BalanceAmount");
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedWindowsLoginName;
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
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion
    }

    public class clsChargesUpdatationVO : IValueObject, INotifyPropertyChanged
    {
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
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

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

        private double _TaxAmount;
        public double TaxAmount
        {
            get { return _TaxAmount; }
            set
            {
                if (_TaxAmount != value)
                {
                    _TaxAmount = value;
                    OnPropertyChanged("TaxAmount");
                }
            }
        }

        private double _ConcessionAmount;
        public double ConcessionAmount
        {
            get { return _ConcessionAmount; }
            set
            {
                if (_ConcessionAmount != value)
                {
                    _ConcessionAmount = value;

                    OnPropertyChanged("ConcessionAmount");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get { return _NetAmount; }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _SelfAmount;
        public double SelfAmount
        {
            get { return _SelfAmount; }
            set
            {
                if (_SelfAmount != value)
                {
                    _SelfAmount = value;
                    OnPropertyChanged("SelfAmount");
                }
            }
        }

        private double _NonSelfAmount;
        public double NonSelfAmount
        {
            get { return _NonSelfAmount; }
            set
            {
                if (_NonSelfAmount != value)
                {
                    _NonSelfAmount = value;
                    OnPropertyChanged("NonSelfAmount");
                }
            }
        }

        private double _DoctorShareAmount;
        public double DoctorShareAmount
        {
            get { return _DoctorShareAmount; }
            set
            {
                if (_DoctorShareAmount != value)
                {
                    _DoctorShareAmount = value;
                    OnPropertyChanged("DoctorShareAmount");
                }
            }
        }

        private double _HospShareAmount;
        public double HospShareAmount
        {
            get { return _HospShareAmount; }
            set
            {
                if (_HospShareAmount != value)
                {
                    _HospShareAmount = value;
                    OnPropertyChanged("HospShareAmount");
                }
            }
        }

        private double _SelfBalance;
        public double SelfBalance
        {
            get { return _SelfBalance; }
            set
            {
                if (_SelfBalance != value)
                {
                    _SelfBalance = value;
                    OnPropertyChanged("SelfBalance");
                }
            }
        }

        private double _NonSelfBalance;
        public double NonSelfBalance
        {
            get { return _NonSelfBalance; }
            set
            {
                if (_NonSelfBalance != value)
                {
                    _NonSelfBalance = value;
                    OnPropertyChanged("NonSelfBalance");
                }
            }
        }

    }

    #region GST Details added by Ashish Z. on dated 24062017
    public class clsChargeTaxDetailsVO : IValueObject, INotifyPropertyChanged
    {
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
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion

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

        private long _ChargeUnitID;
        public long ChargeUnitID
        {
            get { return _ChargeUnitID; }
            set
            {
                if (_ChargeUnitID != value)
                {
                    _ChargeUnitID = value;
                    OnPropertyChanged("ChargeUnitID");
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

        private DateTime? _Date;
        public DateTime? Date
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

        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    if (value < 0)
                        value = 0;

                    _Rate = value;
                    OnPropertyChanged("Rate");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }


        private double _Quantity;
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity != value)
                {

                    if (value <= 0)
                        value = 1;

                    _Quantity = value;
                    OnPropertyChanged("Quantity");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("ServiceTaxPercent");
                    // OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _TotalAmount;
        public double TotalAmount
        {
            get
            {
                _TotalAmount = _Quantity * _Rate;
                return _TotalAmount;

            }
            set
            {
                if (_TotalAmount != value)
                {
                    _TotalAmount = value;
                    OnPropertyChanged("TotalAmount");
                    if (_TotalAmount == 0)
                        OnPropertyChanged("Concession");
                    OnPropertyChanged("NetAmount");

                }
            }
        }


        private double _ConcessionPercent;
        public double ConcessionPercent
        {
            get
            {
                _ConcessionPercent = Math.Round(_ConcessionPercent, 2);
                return _ConcessionPercent;
            }
            set
            {
                if (_ConcessionPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;
                    _ConcessionPercent = value;


                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("Concession");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _Concession;
        public double Concession
        {
            get
            {
                if (_ConcessionPercent > 0)
                    _Concession = (((_Rate * _Quantity) * _ConcessionPercent) / 100);
                else
                    _Concession = 0;

                // _Concession = Math.Round(_Concession, 2);

                _Concession = Math.Round(_Concession);

                return _Concession;
            }
            set
            {
                if (_Concession != value)
                {
                    if (value < 0)
                        value = 0;
                    //if(_ConcessionAmount !=)
                    _Concession = Math.Round(value, 2);
                    if (_Concession > 0)
                        _ConcessionPercent = ((_Concession / _Quantity) * 100) / _Rate;
                    else
                        _ConcessionPercent = 0;


                    OnPropertyChanged("Concession");
                    OnPropertyChanged("ConcessionPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");

                }
            }
        }

        private double _ServiceTaxPercent;
        public double ServiceTaxPercent
        {
            get { return _ServiceTaxPercent; }
            set
            {
                if (_ServiceTaxPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _ServiceTaxPercent = value;
                    OnPropertyChanged("ServiceTaxPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    //OnPropertyChanged("ServiceTaxAmount");
                    //OnPropertyChanged("TotalAmount");
                    //OnPropertyChanged("NetAmount");
                }
            }
        }

        //private double _ServiceTaxAmount;
        //public double ServiceTaxAmount
        //{
        //    get
        //    {
        //        //if (_ServiceTaxPercent > 0)
        //        //    return _ServiceTaxAmount = ((_TotalAmount - Concession - _StaffDiscountAmount - _StaffParentDiscountAmount) * _ServiceTaxPercent / 100);
        //        //else
        //        //    return _ServiceTaxAmount = 0;
        //        return _ServiceTaxAmount;
        //    }
        //    set
        //    {

        //        if (value < 0)
        //            value = 0;

        //        _ServiceTaxAmount = value;
        //        //if (_ServiceTaxAmount > 0 && (_TotalAmount - (_Concession * _Quantity)) > 0)
        //        //    _ServiceTaxPercent = (_ServiceTaxAmount / _Quantity) / (_TotalAmount - _Concession);
        //        //else
        //        //    _ServiceTaxPercent = 0;

        //        //OnPropertyChanged("ServiceTaxAmount");
        //        //OnPropertyChanged("ServiceTaxPercent");
        //        //OnPropertyChanged("TotalAmount");
        //        //OnPropertyChanged("NetAmount");
        //        OnPropertyChanged("ServiceTaxAmount");
        //    }
        //}

        private double _ServiceTaxAmount;
        public double ServiceTaxAmount
        {
            get
            {
                if (_ServiceTaxPercent > 0)
                {
                    if (_IsTaxLimitApplicable == false)
                    {
                        _ServiceTaxAmount = ((_TotalAmount - Concession - _StaffDiscountAmount - _StaffParentDiscountAmount) * _ServiceTaxPercent / 100);
                    }
                    else if (_IsTaxLimitApplicable = true)
                    {
                        if (_TotalAmount > Convert.ToDouble(_TaxLimit))
                        {
                            _ServiceTaxAmount = ((_TotalAmount - Concession - _StaffDiscountAmount - _StaffParentDiscountAmount) * _ServiceTaxPercent / 100);
                        }
                        else
                        {
                            _ServiceTaxAmount = 0;
                        }
                        return _ServiceTaxAmount;
                    }
                    return _ServiceTaxAmount;
                }
                else
                {
                    return _ServiceTaxAmount = 0;
                }
            }
            set
            {

                //if (value < 0)
                //    value = 0;

                //_ServiceTaxAmount = value;
                //if (_ServiceTaxAmount > 0 && (_TotalAmount - (_Concession * _Quantity)) > 0)
                //    _ServiceTaxPercent = (_ServiceTaxAmount / _Quantity) / (_TotalAmount - _Concession);
                //else
                //    _ServiceTaxPercent = 0;


                if (value < 0)
                    value = 0.0;

                _ServiceTaxAmount = value;
                if (_ServiceTaxAmount > 0) // && (_TotalAmount - (_Concession * _Quantity)) > 0

                    _ServiceTaxPercent = (_ServiceTaxAmount * 100) / (_TotalAmount - Concession);

                else
                    _ServiceTaxPercent = 0.0;

                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("ServiceTaxPercent");
                //OnPropertyChanged("TotalAmount");
                //OnPropertyChanged("NetAmount");

            }
        }


        private double _StaffDiscountPercent;
        public double StaffDiscountPercent
        {
            get { return _StaffDiscountPercent; }
            set
            {
                if (_StaffDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _StaffDiscountPercent = value;
                    OnPropertyChanged("StaffDiscountPercent");
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffDiscountAmount;
        public double StaffDiscountAmount
        {
            get
            {
                if (_StaffDiscountPercent != 0)
                    _StaffDiscountAmount = (((Rate * _StaffDiscountPercent) / 100));

                return _StaffDiscountAmount * _Quantity;
            }
            set
            {
                if (_StaffDiscountAmount * _Quantity != value)
                {
                    if (value < 0)
                        value = 0;

                    _StaffDiscountAmount = value;
                    _StaffDiscountPercent = 0;
                    OnPropertyChanged("StaffDiscountAmount");
                    OnPropertyChanged("StaffDiscountPercent");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffParentDiscountPercent;
        public double StaffParentDiscountPercent
        {
            get { return _StaffParentDiscountPercent; }
            set
            {
                if (_StaffParentDiscountPercent != value)
                {
                    if (value < 0)
                        value = 0;

                    if (value > 100)
                        value = 100;

                    _StaffParentDiscountPercent = value;
                    OnPropertyChanged("StaffParentDiscountPercent");
                    OnPropertyChanged("StaffParentDiscountAmount");
                    OnPropertyChanged("ServiceTaxAmount");
                    OnPropertyChanged("TotalAmount");
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        private double _StaffParentDiscountAmount;
        public double StaffParentDiscountAmount
        {
            get
            {
                if (_StaffParentDiscountPercent != 0)
                    _StaffParentDiscountAmount = ((_Rate * _StaffParentDiscountPercent) / 100);

                return _StaffParentDiscountAmount * _Quantity;
            }
            set
            {

                if (value < 0)
                    value = 0;

                _StaffParentDiscountAmount = value;

                _StaffParentDiscountPercent = 0;
                OnPropertyChanged("StaffParentDiscountAmount");
                OnPropertyChanged("StaffParentDiscountPercent");
                OnPropertyChanged("ServiceTaxAmount");
                OnPropertyChanged("TotalAmount");
                OnPropertyChanged("NetAmount");

            }
        }


        private double _Discount;
        public double Discount
        {
            get { return _Discount; }
            set
            {
                if (_Discount != value)
                {
                    if (value < 0)
                        value = 0;
                    _Discount = value;
                    OnPropertyChanged("Discount");
                }
            }
        }

        private double _StaffFree;
        public double StaffFree
        {
            get { return _StaffFree; }
            set
            {
                if (_StaffFree != value)
                {
                    _StaffFree = value;
                    OnPropertyChanged("StaffFree");
                }
            }
        }

        private double _NetAmount;
        public double NetAmount
        {
            get
            {
                _NetAmount = _TotalAmount - Concession - StaffDiscountAmount - StaffParentDiscountAmount + ServiceTaxAmount;
                if (_NetAmount < 0) _NetAmount = 0;
                return _NetAmount;
            }
            set
            {
                if (_NetAmount != value)
                {
                    _NetAmount = value;
                    OnPropertyChanged("NetAmount");
                }
            }
        }

        ////Addedby Priyanka On 04April2012
        //private double _ServicePaidAmount;
        //public double ServicePaidAmount
        //{
        //    get { return _ServicePaidAmount; }
        //    set
        //    {
        //        if (_ServicePaidAmount != value)
        //        {
        //            _ServicePaidAmount = value;
        //            OnPropertyChanged("ServicePaidAmount");
        //        }
        //    }
        //}

        //private double _TotalServicePaidAmount;
        //public double TotalServicePaidAmount
        //{
        //    get { return _TotalServicePaidAmount; }
        //    set
        //    {
        //        if (_TotalServicePaidAmount != value)
        //        {
        //            _TotalServicePaidAmount = value;
        //            OnPropertyChanged("TotalServicePaidAmount");
        //        }
        //    }
        //}


        private double _TotalConcession;
        public double TotalConcession
        {
            get { return _TotalConcession; }
            set
            {
                if (_TotalConcession != value)
                {
                    _TotalConcession = value;
                    OnPropertyChanged("TotalConcession");
                }
            }
        }

        private double _BalanceAmount;
        public double BalanceAmount
        {
            get { return _BalanceAmount; }
            set
            {
                if (_BalanceAmount != value)
                {
                    _BalanceAmount = value;
                    OnPropertyChanged("BalanceAmount");
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

        private string _AddedWindowsLoginName;
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

        private string _UpdatedWindowsLoginName;
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

        private List<clsServiceTaxVO> _TaxLinkingDetailsList;
        public List<clsServiceTaxVO> TaxLinkingDetailsList
        {
            get
            {
                return _TaxLinkingDetailsList;
            }
            set
            {
                if (_TaxLinkingDetailsList != value)
                {
                    _TaxLinkingDetailsList = value;
                }
            }
        }

        #endregion

    }
    #endregion
}
