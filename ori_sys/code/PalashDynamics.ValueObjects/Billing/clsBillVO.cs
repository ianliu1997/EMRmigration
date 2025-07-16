using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsBillVO : IValueObject, INotifyPropertyChanged
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

        private clsBillDetailsVO _BillDetails = new clsBillDetailsVO();
        public clsBillDetailsVO BillDetails
        {
            get { return _BillDetails; }
            set
            {

                _BillDetails = value;
                OnPropertyChanged("BillDetails");

            }
        }

        private List<clsPatientFollowUpVO> _FollowUpDetails = new List<clsPatientFollowUpVO>();
        public List<clsPatientFollowUpVO> FollowUpDetails
        {
            get { return _FollowUpDetails; }
            set
            {

                _FollowUpDetails = value;
                OnPropertyChanged("FollowUpDetails");

            }
        }

        private clsPathOrderBookingVO _PathoWorkOrder = new clsPathOrderBookingVO();
        public clsPathOrderBookingVO PathoWorkOrder
        {
            get { return _PathoWorkOrder; }
            set
            {

                _PathoWorkOrder = value;
                OnPropertyChanged("PathoWorkOrder");

            }
        }


        private clsRadOrderBookingVO _RadiologyWorkOrder = new clsRadOrderBookingVO();
        public clsRadOrderBookingVO RadiologyWorkOrder
        {
            get { return _RadiologyWorkOrder; }
            set
            {

                _RadiologyWorkOrder = value;
                OnPropertyChanged("RadiologyWorkOrder");

            }
        }

        private clsItemSalesVO _PharmacyItems = new clsItemSalesVO();
        public clsItemSalesVO PharmacyItems
        {
            get { return _PharmacyItems; }
            set
            {

                _PharmacyItems = value;
                OnPropertyChanged("PharmacyItems");

            }
        }
        //private List<KeyValuePair<long, bool>> _Charges = new List<KeyValuePair<long, bool>>();
        //public List<KeyValuePair<long, bool>> Charges
        //{
        //    get { return _Charges; }
        //    set
        //    {

        //        _Charges = value;
        //        OnPropertyChanged("Charges");

        //    }
        //}

        private List<clsChargeVO> _ChargeDetails = new List<clsChargeVO>();
        public List<clsChargeVO> ChargeDetails
        {
            get { return _ChargeDetails; }
            set
            {

                _ChargeDetails = value;
                OnPropertyChanged("ChargeDetails");

            }
        }

        //***//------
        private List<clsChargeVO> _DeleteChargeDetails = new List<clsChargeVO>();
        public List<clsChargeVO> DeleteChargeDetails
        {
            get { return _DeleteChargeDetails; }
            set
            {

                _DeleteChargeDetails = value;
                OnPropertyChanged("DeleteChargeDetails");

            }
        }

        //---------

        private clsPaymentVO _PaymentDetails = new clsPaymentVO();
        public clsPaymentVO PaymentDetails
        {
            get { return _PaymentDetails; }
            set
            {

                _PaymentDetails = value;
                OnPropertyChanged("PaymentDetails");

            }
        }

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


        private long _BankID;
        public long BankID
        {
            get { return _BankID; }
            set
            {
                if (_BankID != value)
                {
                    _BankID = value;
                    OnPropertyChanged("BankID");
                }
            }
        }
        private long _PaymentId;
        public long PaymentID
        {
            get { return _PaymentId; }
            set
            {
                if(_PaymentId != value )
                {
                    _PaymentId = value ;
                    OnPropertyChanged("PaymentID");
                }
            }
        }

        private long _PaymentModeId;
        public long PaymentModeId
        {
            get { return _PaymentModeId; }
            set
            {
                if (_PaymentModeId != value)
                {
                    _PaymentModeId = value;
                    OnPropertyChanged("PaymentModeId");
                }
            }
        }

        private long _PaymentDetailId;
        public long PaymentDetailID
        {
            get { return _PaymentDetailId; }
            set
            {
                if (_PaymentDetailId != value)
                {
                    _PaymentDetailId = value;
                    OnPropertyChanged("PaymentDetailID");
                }
            }
        }




            private string _Bank;
            public string Bank
            {
                get { return _Bank; }
                set{
                    if (_Bank != value)
                    {
                        _Bank = value;
                        OnPropertyChanged("Bank");
                    }
                }
        }


        public string MRNO { get; set; }

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

        private BillPaymentTypes _BillPaymentType;
        public BillPaymentTypes BillPaymentType
        {
            get { return _BillPaymentType; }
            set
            {
                if (_BillPaymentType != value)
                {
                    _BillPaymentType = value;
                    OnPropertyChanged("BillPaymentType");
                }
            }
        }

        private String _PaymentMode;
            public String PaymentMode
            {
                get { return _PaymentMode; }
            set
            {
                if (_PaymentMode != value)
                {
                    _PaymentMode = value;
                    OnPropertyChanged("PaymentMode");
                }
            }
            }

        public String BillPaymentTypeStr
        {
            get { return _BillPaymentType.ToString(); }
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
        public long _BillID;
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

        private string _Date1;
        public string Date1
        {
            get { return _Date1; }
            set
            {
                if (_Date1 != value)
                {
                    _Date1 = value;
                    OnPropertyChanged("Date1");
                }
            }
        }

        private DateTime _Time;
        public DateTime Time
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

        public string Opd_Ipd_External_No { get; set; }

        private long _CashCounterId;
        public long CashCounterId
        {
            get { return _CashCounterId; }
            set
            {
                if (_CashCounterId != value)
                {
                    _CashCounterId = value;
                    OnPropertyChanged("CashCounterId");
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

        private long _PatientSourceId;
        public long PatientSourceId
        {
            get { return _PatientSourceId; }
            set
            {
                if (_PatientSourceId != value)
                {
                    _PatientSourceId = value;
                    OnPropertyChanged("PatientSourceId");
                }
            }
        }

        private long _PatientCategoryId;
        public long PatientCategoryId
        {
            get { return _PatientCategoryId; }
            set
            {
                if (_PatientCategoryId != value)
                {
                    _PatientCategoryId = value;
                    OnPropertyChanged("PatientCategoryId");
                }
            }
        }

        private long _RelationID;
        public long RelationID
        {
            get { return _RelationID; }
            set
            {
                if (_RelationID != value)
                {
                    _RelationID = value;
                    OnPropertyChanged("RelationID");
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

        private string _CompRefNo;
        public string CompRefNo
        {
            get { return _CompRefNo; }
            set
            {
                if (_CompRefNo != value)
                {
                    _CompRefNo = value;
                    OnPropertyChanged("CompRefNo");
                }
            }
        }

        private DateTime? _Expirydate;
        public DateTime? Expirydate
        {
            get { return _Expirydate; }
            set
            {
                if (_Expirydate != value)
                {
                    _Expirydate = value;
                    OnPropertyChanged("Expirydate");
                }
            }
        }

        private long _CampId;
        public long CampId
        {
            get { return _CampId; }
            set
            {
                if (_CampId != value)
                {
                    _CampId = value;
                    OnPropertyChanged("CampId");
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

        private bool _InterORFinal;
        public bool InterORFinal
        {
            get { return _InterORFinal; }
            set
            {
                if (_InterORFinal != value)
                {
                    _InterORFinal = value;
                    OnPropertyChanged("InterORFinal");
                }
            }
        }
        private bool _IsPackageServiceInclude;
        public bool IsPackageServiceInclude
        {
            get { return _IsPackageServiceInclude; }
            set
            {
                if (_IsPackageServiceInclude != value)
                {
                    _IsPackageServiceInclude = value;
                    OnPropertyChanged("IsPackageServiceInclude");
                }
            }
        }
        //public bool  { get; set; }

        private double _TotalBillAmount;
        public double TotalBillAmount
        {
            get { return _TotalBillAmount; }
            set
            {
                if (_TotalBillAmount != value)
                {
                    _TotalBillAmount = value;
                    OnPropertyChanged("TotalBillAmount");
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
                        


        private double _TotalDiscountAmount;
        public double TotalDiscountAmount
        {
            get { return _TotalDiscountAmount; }
            set
            {
                if (_TotalDiscountAmount != value)
                {
                    _TotalDiscountAmount = value;
                    OnPropertyChanged("TotalDiscountAmount");
                }
            }
        }

        private double _TotalConcessionAmount;
        public double TotalConcessionAmount
        {
            get { return _TotalConcessionAmount; }
            set
            {
                if (_TotalConcessionAmount != value)
                {
                    _TotalConcessionAmount = value;
                    OnPropertyChanged("TotalConcessionAmount");
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

        private double _NetBillAmount;
        public double NetBillAmount
        {
            get { return _NetBillAmount; }
            set
            {
                if (_NetBillAmount != value)
                {
                    _NetBillAmount = value;
                    OnPropertyChanged("NetBillAmount");
                }
            }
        }
        private double _CalculatedNetBillAmount;
        public double CalculatedNetBillAmount
        {
            get { return _CalculatedNetBillAmount; }
            set
            {
                if (_CalculatedNetBillAmount != value)
                {
                    _CalculatedNetBillAmount = value;
                    OnPropertyChanged("CalculatedNetBillAmount");
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

        private double _BalanceAmountSelf;
        public double BalanceAmountSelf
        {
            get { return _BalanceAmountSelf; }
            set
            {
                if (_BalanceAmountSelf != value)
                {
                    _BalanceAmountSelf = value;
                    OnPropertyChanged("BalanceAmountSelf");
                }
            }
        }

        public double PaidAmountSelf
        {
            get
            {
                if (_IsFreezed)
                    return _NetBillAmount - _BalanceAmountSelf;
                else
                    return 0;

            }

        }


        private double _BalanceAmountNonSelf;
        public double BalanceAmountNonSelf
        {
            get { return _BalanceAmountNonSelf; }
            set
            {
                if (_BalanceAmountNonSelf != value)
                {
                    _BalanceAmountNonSelf = value;
                    OnPropertyChanged("BalanceAmountNonSelf");
                }
            }
        }

        private double _CrAmount;
        public double CrAmount
        {
            get { return _CrAmount; }
            set
            {
                if (_CrAmount != value)
                {
                    _CrAmount = value;
                    OnPropertyChanged("CrAmount");
                }
            }
        }

        private bool _IsFree;
        public bool IsFree
        {
            get { return _IsFree; }
            set
            {
                if (_IsFree != value)
                {
                    _IsFree = value;
                    OnPropertyChanged("IsFree");
                }
            }
        }

        private bool _IsSettled;
        public bool IsSettled
        {
            get { return _IsSettled; }
            set
            {
                if (_IsSettled != value)
                {
                    _IsSettled = value;
                    OnPropertyChanged("IsSettled");
                }
            }
        }

        private BillTypes _BillType;
        public BillTypes BillType
        {
            get { return _BillType; }
            set
            {
                if (_BillType != value)
                {
                    _BillType = value;
                    OnPropertyChanged("BillType");
                }
            }
        }

        private bool _IsCashTariff;
        public bool IsCashTariff
        {
            get { return _IsCashTariff; }
            set
            {
                if (_IsCashTariff != value)
                {
                    _IsCashTariff = value;
                    OnPropertyChanged("IsCashTariff");
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

        public bool SelectCharge { get; set; }
        private DateTime? _CancellationDate;
        public DateTime? CancellationDate
        {
            get { return _CancellationDate; }
            set
            {
                if (_CancellationDate != value)
                {
                    _CancellationDate = value;
                    OnPropertyChanged("CancellationDate");
                }
            }
        }

        private DateTime? _CancellationTime;
        public DateTime? CancellationTime
        {
            get { return _CancellationTime; }
            set
            {
                if (_CancellationTime != value)
                {
                    _CancellationTime = value;
                    OnPropertyChanged("CancellationTime");
                }
            }
        }

        private string _CancellationReason;
        public string CancellationReason
        {
            get { return _CancellationReason; }
            set
            {
                if (_CancellationReason != value)
                {
                    _CancellationReason = value;
                    OnPropertyChanged("CancellationReason");
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

        private bool _SponserType;
        public bool SponserType
        {
            get { return _SponserType; }
            set
            {
                if (_SponserType != value)
                {
                    _SponserType = value;
                    OnPropertyChanged("SponserType");
                }
            }
        }

        private string _ComputerName;
        public string ComputerName
        {
            get { return _ComputerName; }
            set
            {
                if (_ComputerName != value)
                {
                    _ComputerName = value;
                    OnPropertyChanged("ComputerName");
                }
            }
        }

        private bool _SeniorCitizenCon;
        public bool SeniorCitizenCon
        {
            get { return _SeniorCitizenCon; }
            set
            {
                if (_SeniorCitizenCon != value)
                {
                    _SeniorCitizenCon = value;
                    OnPropertyChanged("SeniorCitizenCon");
                }
            }
        }

        private string _BillCancellationRemark;
        public string BillCancellationRemark
        {
            get { return _BillCancellationRemark; }
            set
            {
                if (_BillCancellationRemark != value)
                {
                    _BillCancellationRemark = value;
                    OnPropertyChanged("BillCancellationRemark");
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

        private double _TotalAdvance;
        public double TotalAdvance
        {
            get { return _TotalAdvance; }
            set
            {
                if (_TotalAdvance != value)
                {
                    _TotalAdvance = value;
                    OnPropertyChanged("TotalAdvance");
                }
            }
        }

        private string _ClaimNo;
        public string ClaimNo
        {
            get { return _ClaimNo; }
            set
            {
                if (_ClaimNo != value)
                {
                    _ClaimNo = value;
                    OnPropertyChanged("ClaimNo");
                }
            }
        }

        private string _BillRemark;
        public string BillRemark
        {
            get { return _BillRemark; }
            set
            {
                if (_BillRemark != value)
                {
                    _BillRemark = value;
                    OnPropertyChanged("BillRemark");
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

        private long _PathoSpecilizationID;
        public long PathoSpecilizationID
        {
            get { return _PathoSpecilizationID; }
            set
            {
                if (_PathoSpecilizationID != value)
                {
                    _PathoSpecilizationID = value;
                    OnPropertyChanged("PathoSpecilizationID");
                }
            }
        }
        private long _RadioSpecilizationID;
        public long RadioSpecilizationID
        {
            get { return _RadioSpecilizationID; }
            set
            {
                if (_RadioSpecilizationID != value)
                {
                    _RadioSpecilizationID = value;
                    OnPropertyChanged("RadioSpecilizationID");
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

        // BY BHUSHAN . . . . . 
        private double _Rate;
        public double Rate
        {
            get { return _Rate; }
            set
            {
                if (_Rate != value)
                {
                    _Rate = value;
                    OnPropertyChanged("Rate");
                }
            }
        }

        //By Yogesh k Added concessionReasonid 20-Apr-16

        public long _ConcessionReasonId;
        public long ConcessionReasonId
        {
            get { return _ConcessionReasonId; }
            set
            {
                if (_ConcessionReasonId != value)
                {
                    _ConcessionReasonId = value;
                    OnPropertyChanged("ConcessionReasonId");
                }
            }
        }

        //Added By Bhushanp 09032017

        private string _ConcessionRemark;

        public string ConcessionRemark
        {
            get { return _ConcessionRemark; }
            set
            {
                if (_ConcessionRemark != value)
                {
                    _ConcessionRemark = value;
                    OnPropertyChanged("ConcessionRemark");
                }
            }
        }


        # region Costing Divisions for Clinical & Pharmacy Billing
        private long _RequestTypeID;
        public long RequestTypeID
        {
            get { return _RequestTypeID; }
            set
            {
                if (_RequestTypeID != value)
                {
                    _RequestTypeID = value;
                    OnPropertyChanged("RequestTypeID");
                }
            }
        }

        private string _AuthorityPerson;
        public string AuthorityPerson
        {
            get { return _AuthorityPerson; }
            set
            {
                if (_AuthorityPerson != value)
                {
                    _AuthorityPerson = value;
                    OnPropertyChanged("AuthorityPerson");
                }
            }
        }
        private string _RequestType;
        public string RequestType
        {
            get { return _RequestType; }
            set
            {
                if (_RequestType != value)
                {
                    _RequestType = value;
                    OnPropertyChanged("RequestType");
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

        public long VisitTypeID { get; set; }

        public long PatientID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long PackageID { get; set; }


        private double _TotalRefund;
        public double TotalRefund
        {
            get { return _TotalRefund; }
            set
            {
                if (_TotalRefund != value)
                {
                    _TotalRefund = value;
                    OnPropertyChanged("TotalRefund");
                }
            }
        }
        //***//--------------AJ Date 9/12/0216
        private bool _IsModify;
        public bool IsModify
        {
            get { return _IsModify; }
            set
            {
                if (_IsModify != value)
                {
                    _IsModify = value;
                    OnPropertyChanged("IsModify");
                }
            }
        }



        public long OTBillClearanceID { get; set; }

        private double _PatientAdvance;
        public double PatientAdvance
        {
            get { return _PatientAdvance; }
            set
            {
                if (_PatientAdvance != value)
                {
                    _PatientAdvance = value;
                    OnPropertyChanged("PatientAdvance");
                }
            }
        }

        private double _PackageAdvance;
        public double PackageAdvance
        {
            get { return _PackageAdvance; }
            set
            {
                if (_PackageAdvance != value)
                {
                    _PackageAdvance = value;
                    OnPropertyChanged("PackageAdvance");
                }
            }
        }

        private double _BalanceAdvance;
        public double BalanceAdvance
        {
            get { return _BalanceAdvance; }
            set
            {
                if (_BalanceAdvance != value)
                {
                    _BalanceAdvance = value;
                    OnPropertyChanged("BalanceAdvance");
                }
            }
        }

        public string PacBilledCount { get; set; }
        public string SemenfreezingCount { get; set; }

        //private long _PacBilledCount;
        //public long PacBilledCount
        //{
        //    get { return _PacBilledCount; }
        //    set
        //    {
        //        if (_PacBilledCount != value)
        //        {
        //            _PacBilledCount = value;
        //            OnPropertyChanged("PacBilledCount");
        //        }
        //    }
        //}

        //private bool _SemenfreezingCount;
        //public bool SemenfreezingCount
        //{
        //    get { return _SemenfreezingCount; }
        //    set
        //    {
        //        if (_SemenfreezingCount != value)
        //        {
        //            _SemenfreezingCount = value;
        //            OnPropertyChanged("SemenfreezingCount");
        //        }
        //    }
        //}

        //***//---------------------------

		private bool _IsDraftBill;

        public bool IsDraftBill
        {
            get { return _IsDraftBill; }
            set { _IsDraftBill = value; }
        }


        #region For IPD Bill

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

        public string ContactNo { get; set; }

        private long _AdmissionType;
        public long AdmissionType
        {
            get { return _AdmissionType; }
            set
            {
                if (_AdmissionType != value)
                {
                    _AdmissionType = value;
                    OnPropertyChanged("AdmissionType");
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

        public bool IsRefunded { get; set; }
        public long PatientUnitID { get; set; }

        private bool _IsComanyBillCancelled;
        public bool IsComanyBillCancelled
        {
            get { return _IsComanyBillCancelled; }
            set
            {
                if (_IsComanyBillCancelled != value)
                {
                    _IsComanyBillCancelled = value;
                    OnPropertyChanged("IsComanyBillCancelled");
                }
            }
        }

        private bool _IsInvoiceGenerated;
        public bool IsInvoiceGenerated
        {
            get { return _IsInvoiceGenerated; }
            set
            {
                if (_IsInvoiceGenerated != value)
                {
                    _IsInvoiceGenerated = value;
                    OnPropertyChanged("IsInvoiceGenerated");
                }
            }
        }


        //By Anjali..............................
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


        private bool _IsRequestSend;
        public bool IsRequestSend
        {
            get { return _IsRequestSend; }
            set
            {
                if (_IsRequestSend != value)
                {
                    _IsRequestSend = value;
                    OnPropertyChanged("IsRequestSend");
                }
            }
        }
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
        private string _LevelDescription;
        public string LevelDescription
        {
            get { return _LevelDescription; }
            set
            {
                if (_LevelDescription != value)
                {
                    _LevelDescription = value;
                    OnPropertyChanged("LevelDescription");
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
        private bool _IsOPDBill;
        public bool IsOPDBill
        {
            get { return _IsOPDBill; }
            set
            {
                if (_IsOPDBill != value)
                {
                    _IsOPDBill = value;
                    OnPropertyChanged("IsOPDBill");
                }
            }
        }
        //........................................



        public string PaymentModeDetails { get; set; }

        private bool _IsIPDBill=false;
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

        // List used to send deleted item list to dal
        private List<clsChargeVO> _DeletedChargeDetails = new List<clsChargeVO>();
        public List<clsChargeVO> DeletedChargeDetails
        {
            get { return _DeletedChargeDetails; }
            set
            {

                _DeletedChargeDetails = value;
                OnPropertyChanged("DeletedChargeDetails");

            }
        }
        #endregion

        public bool ISForMaterialConsumption { get; set; } //Added by AJ Date 10/4/2017
        private bool _ISDischarged;   //Added by AJ Date 2/5/2017

        public bool ISDischarged
        {
            get { return _ISDischarged; }
            set
            {
                if (_ISDischarged != value)
                {
                    _ISDischarged = value;
                    OnPropertyChanged("ISDischarged");
                }
            }
        }

        private bool _IsPackageConsumption;

        public bool IsPackageConsumption
        {
            get { return _IsPackageConsumption; }
            set { _IsPackageConsumption = value; }
        }
        #endregion

        private long _ParentID;
        public long ParentID
        {
            get { return _ParentID; }
            set
            {
                if (_ParentID != value)
                {
                    _ParentID = value;
                    OnPropertyChanged("ParentID");
                }
            }
        }

        private long _AdvanceID;
        public long AdvanceID
        {
            get { return _AdvanceID; }
            set
            {
                if (_AdvanceID != value)
                {
                    _AdvanceID = value;
                    OnPropertyChanged("AdvanceID");
                }
            }
        }

        private long _AdvanceUnitID;
        public long AdvanceUnitID
        {
            get { return _AdvanceUnitID; }
            set
            {
                if (_AdvanceUnitID != value)
                {
                    _AdvanceUnitID = value;
                    OnPropertyChanged("AdvanceUnitID");
                }
            }
        }

        //private long _PackageBillID;
        //public long PackageBillID
        //{
        //    get { return _PackageBillID; }
        //    set
        //    {
        //        if (_PackageBillID != value)
        //        {
        //            _PackageBillID = value;
        //            OnPropertyChanged("PackageBillID");
        //        }
        //    }
        //}

        //private long _PackageBillUnitID;
        //public long PackageBillUnitID
        //{
        //    get { return _PackageBillUnitID; }
        //    set
        //    {
        //        if (_PackageBillUnitID != value)
        //        {
        //            _PackageBillUnitID = value;
        //            OnPropertyChanged("PackageBillUnitID");
        //        }
        //    }
        //}

        //***//------------------
        private long _LinkPatientID;
        public long LinkPatientID
        {
            get { return _LinkPatientID; }
            set
            {
                if (_LinkPatientID != value)
                {
                    _LinkPatientID = value;
                    OnPropertyChanged("LinkPatientID");
                }
            }
        }

        private long _LinkPatientUnitID;
        public long LinkPatientUnitID
        {
            get { return _LinkPatientUnitID; }
            set
            {
                if (_LinkPatientUnitID != value)
                {
                    _LinkPatientUnitID = value;
                    OnPropertyChanged("LinkPatientUnitID");
                }
            }
        }

        private bool _AgainstDonor;
        public bool AgainstDonor
        {
            get { return _AgainstDonor; }
            set { _AgainstDonor = value; }
        }

        // For Package New Changes Added on 18062018
        private long _PackageBillID;
        public long PackageBillID
        {
            get { return _PackageBillID; }
            set
            {
                if (_PackageBillID != value)
                {
                    _PackageBillID = value;
                    OnPropertyChanged("PackageBillID");
                }

            }
        }

        // For Package New Changes Added on 18062018
        private double _PackageConcessionAmount;
        public double PackageConcessionAmount
        {
            get { return _PackageConcessionAmount; }
            set
            {
                if (_PackageConcessionAmount != value)
                {
                    _PackageConcessionAmount = value;
                    OnPropertyChanged("PackageConcessionAmount");
                }
            }
        }

        // For Package New Changes Added on 20062018
        private bool _IsAdjustableBillDone;
        public bool IsAdjustableBillDone
        {
            get { return _IsAdjustableBillDone; }
            set { _IsAdjustableBillDone = value; }
        }

        // For Package New Changes Added on 20062018
        private bool _IsAllBillSettle;
        public bool IsAllBillSettle
        {
            get { return _IsAllBillSettle; }
            set { _IsAllBillSettle = value; }
        }

        // For Package New Changes Added on 20062018
        private double _PackageSettleRate;
        public double PackageSettleRate
        {
            get { return _PackageSettleRate; }
            set
            {
                if (_PackageSettleRate != value)
                {
                    _PackageSettleRate = value;
                    //OnPropertyChanged("PackageSettleRate");
                }
            }
        }

    }

    public class clsBillDetailsVO : IValueObject, INotifyPropertyChanged
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
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion

        #region Property Declaration Section

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


        private long _BillId;
        public long BillId
        {
            get { return _BillId; }
            set
            {
                if (_BillId != value)
                {
                    _BillId = value;
                    OnPropertyChanged("BillId");
                }
            }
        }

        private long _ChargeId;
        public long ChargeId
        {
            get { return _ChargeId; }
            set
            {
                if (_ChargeId != value)
                {
                    _ChargeId = value;
                    OnPropertyChanged("ChargeId");
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


        private long _AddedOn;
        public long AddedOn
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


        private long _UpdatedOn;
        public long UpdatedOn
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
        #endregion
    }



    public class clsCompanyCreditDetailsVO : IValueObject, INotifyPropertyChanged
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
        public long ID { get; set; }
        public double TotalAdvance { get; set; }
        public double Refund { get; set; }
        public double Used { get; set; }
        public double Balance { get; set; }
        public double CreditLimit { get; set; }

        #endregion
    }
}
