using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Radiology;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsAddChargeBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsAddChargeBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region OLD variables VARIABLES

        //private int _SuccessStatus;
        ///// <summary>
        ///// Output Property.
        ///// This property states the outcome of BizAction Process.
        ///// </summary>
        //public int SuccessStatus
        //{
        //    get { return _SuccessStatus; }
        //    set { _SuccessStatus = value; }
        //}

        //private clsChargeVO objDetails = null;
        ///// <summary>
        ///// Output Property.
        ///// This Property Contains OPDPatient Details Which is Added.
        ///// </summary>
        //public clsChargeVO Details
        //{
        //    get { return objDetails; }
        //    set { objDetails = value; }
        //}

        #endregion

       

        #region  IPD variables

        private List<clsChargeVO> _ChargesList = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsChargeVO> ChargesList
        {
            get { return _ChargesList; }
            set { _ChargesList = value; }
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

        private bool _IsPathWorkOrder;
        public bool IsPathWorkOrder
        {
            get { return _IsPathWorkOrder; }
            set
            {
                if (_IsPathWorkOrder != value)
                {
                    _IsPathWorkOrder = value;
                }
            }
        }
        private bool _IsRadiologyWorkOrder;
        public bool IsRadiologyWorkOrder
        {
            get { return _IsRadiologyWorkOrder; }
            set
            {
                if (_IsRadiologyWorkOrder != value)
                {
                    _IsRadiologyWorkOrder = value;
                }
            }
        }
        private clsPathOrderBookingVO _PathoWorkOrder = new clsPathOrderBookingVO();
        public clsPathOrderBookingVO PathoWorkOrder
        {
            get { return _PathoWorkOrder; }
            set
            {
                if (_PathoWorkOrder != value)
                {
                    _PathoWorkOrder = value;
                }
            }
        }

        private clsRadOrderBookingVO _RadiologyWorkOrder = new clsRadOrderBookingVO();
        public clsRadOrderBookingVO RadiologyWorkOrder
        {
            get { return _RadiologyWorkOrder; }
            set
            {
                if (_RadiologyWorkOrder != value)
                {
                    _RadiologyWorkOrder = value;
                }
            }
        }
        private clsChargesUpdatationVO _objChargesUpdationDetails = null;
        public clsChargesUpdatationVO ChargesUpdationDetails
        {
            get { return _objChargesUpdationDetails; }
            set
            {
                if (_objChargesUpdationDetails != value)
                {
                    _objChargesUpdationDetails = value;
                }
            }
        }

        private bool _IsCallFromConcessionManager;
        public bool IsCallFromConcessionManager
        {
            get { return _IsCallFromConcessionManager; }
            set
            {
                if (_IsCallFromConcessionManager = value)
                {
                    _IsCallFromConcessionManager = value;
                }
            }
        }

        private bool _IsPathologyWorkOrder;
        public bool IsPathologyWorkOrder
        {
            get { return _IsPathologyWorkOrder; }
            set
            {
                if (_IsPathologyWorkOrder != value)
                {
                    _IsPathologyWorkOrder = value;
                }
            }
        }

        private bool _IsRadWorkOrder;
        public bool IsRadWorkOrder
        {
            get { return _IsRadWorkOrder; }
            set
            {
                if (_IsRadWorkOrder != value)
                {
                    _IsRadWorkOrder = value;
                }
            }
        }

        private bool _IsOPDIPD = false;
        public bool IsOPDIPD
        {
            get { return _IsOPDIPD; }
            set
            {
                if (_IsOPDIPD = value)
                {
                    _IsOPDIPD = value;
                }
            }
        }
        private clsChargeVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsChargeVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        public bool FromVisit { get; set; }

        #endregion

    }


    public class clsGetChargeListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetChargeListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region OLD Variabeles
        //private int _SuccessStatus;
        ///// <summary>
        ///// Output Property.
        ///// This property states the outcome of BizAction Process.
        ///// </summary>
        //public int SuccessStatus
        //{
        //    get { return _SuccessStatus; }
        //    set { _SuccessStatus = value; }
        //}

        //public Int16 Opd_Ipd_External { get; set; }
        //public long Opd_Ipd_External_Id { get; set; }
        //public long Opd_Ipd_External_UnitId { get; set; }
        //public long ID { get; set; }
        //public bool? IsBilled { get; set; }
        //public long BillID { get; set; }
        //public long UnitID { get; set; }
        //private List<clsChargeVO> _List = null;
        ///// <summary>
        ///// Output Property.
        ///// This Property Contains OPDPatient Details Which is Added.
        ///// </summary>
        //public List<clsChargeVO> List
        //{
        //    get { return _List; }
        //    set { _List = value; }
        //}
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

        public Int16 Opd_Ipd_External { get; set; }
        public long Opd_Ipd_External_Id { get; set; }
        public long Opd_Ipd_External_UnitId { get; set; }
        public long ID { get; set; }
        public bool? IsBilled { get; set; }
        public long BillID { get; set; }
        public long UnitID { get; set; }
        public long CostingDivisionID { get; set; }

        public long RequestTypeID { get; set; }
        private List<clsChargeVO> _List = null;

        public Int16 Opd_Ipd { get; set; }
        public long Opd_Ipd_Id { get; set; }
        public long Opd_Ipd_UnitId { get; set; }

        public long ServiceID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long CompanyID { get; set; }
        public bool IsCancel { get; set; }
        public bool IsIPDCharegs { get; set; }
        public bool IsIPDPreviousAutoCharegs { get; set; }
        public bool IsGetUpdatedCharegs { get; set; }
        //By Anjali.........
        public bool IsFromApprovalRequestWindow { get; set; }
        //..............................
        private bool _IsForIPDBill = false;
        public bool IsForIPDBill 
        { get; set; }

        

        // BY SUDHIR PATIL
        public bool IsForBedTransfer { get; set; }
        public long BillingClass { get; set; }

        public bool IsFromPatientID { get; set; }

        public DateTime? Date { get; set; }

        public long SelfCompanyID { get; set; }

        public bool IsGetPendingCharges { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public long TariffServiceID { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsChargeVO> List
        {
            get { return _List; }
            set { _List = value; }
        }


        private List<clsGetPatientPastVisitBizActionVO> objBillDetails = new List<clsGetPatientPastVisitBizActionVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsGetPatientPastVisitBizActionVO> BillDetails
        {
            get { return objBillDetails; }
            set { objBillDetails = value; }
        }

        #region GST Details added by Ashish Z. on dated 24062017
        public bool IsForTaxationDetails = false; // set true from T_Bill form GST Details added by Ashish Z. on dated 24062017
        public bool IsBillFreeze = false;  // set true from T_Bill form GST Details added by Ashish Z. on dated 24062017

        private clsChargeVO _ChargeVO;
        public clsChargeVO ChargeVO
        {
            get { return _ChargeVO; }
            set
            {
                if (_ChargeVO != value)
                {
                    _ChargeVO = value;
                    //OnPropertyChanged("ChargeDetails");
                }
            }
        }
        #endregion
    }


    public class clsAddRefundServiceChargeBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.Billing.clsAddRefundServiceChargeBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private List<clsChargeVO> objDetails = null;
        public List<clsChargeVO> ChargeList
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        public bool IsUpdate { get; set; }
        public long ChargeID { get; set; }
        public long UnitID { get; set; }
    }

    #region For IPD Module

    public class SponsorCompanyDetails
    {
        public long SponsorID { get; set; }
        public long CompanyID { get; set; }
        public string Description { get; set; }
        public long ID { get; set; }
        public long UnitID { get; set; }
        public long TariffID { get; set; }

        public SponsorCompanyDetails()
        {

        }

        public SponsorCompanyDetails(long SponsorID, long CompanyID)
        {
            this.SponsorID = SponsorID;
            this.CompanyID = CompanyID;
        }
        public SponsorCompanyDetails(long SponsorID, long CompanyID, string Description, long ID, long UnitID, long TariffID)
        {
            this.SponsorID = SponsorID;
            this.CompanyID = CompanyID;
            this.Description = Description;

            this.ID = ID;
            this.UnitID = UnitID;
            this.TariffID = TariffID;
        }
    }

    #endregion

    public class clsGetChargeAgainstBillListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetChargeAgainstBillListBizAction";
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

        public Int16 Opd_Ipd_External { get; set; }
        public long Opd_Ipd_External_Id { get; set; }
        public long Opd_Ipd_External_UnitId { get; set; }
        public long ID { get; set; }
        public bool? IsBilled { get; set; }
        public string BillID { get; set; }
        public long UnitID { get; set; }
        public bool IsVisitService { get; set; }
        private List<clsChargeVO> _List = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsChargeVO> List
        {
            get { return _List; }
            set { _List = value; }
        }

        public bool IsDefaultService { get; set; }

    }

}
