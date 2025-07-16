using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Master.CompanyPayment;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration
{
    public class clsAddPaymentBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsAddPaymentBizAction";
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

        public bool myTransaction { get; set; }

        private clsPaymentVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPaymentVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        /* Properties Set to Update T_Bill  for BalanceAmountSelf Column Using Transaction   */
        public bool isUpdateBillPaymentDetails { get; set; }
        public clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails { get; set; }
    }

    public class clsGetPaymentListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsGetPaymentListBizAction";
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


        public long BillID { get; set; }
        public long UnitID { get; set; }
        private List<clsPaymentVO> objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPaymentVO> List
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }


    public class clsCompanySettlementBizActionVO : IBizActionValueObject
    {
        public long InvoiceID { get; set; }
        public long InvoiceUnitID { get; set; }
        public double InvoicePaidAmount { get; set; }
        public double InvoiceBalanceAmount { get; set; }
        public double InvoiceTDSAmount { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Billing.clsCompanySettlementBizAction";
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

        public bool myTransaction { get; set; }

        private List<clsPaymentVO> objDetails = new List<clsPaymentVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPaymentVO> Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        public clsPaymentVO PaymentDetailobj { get; set; }

        private List<clsCompanyPaymentDetailsVO> objbill = new List<clsCompanyPaymentDetailsVO>();
        public List<clsCompanyPaymentDetailsVO> BillDetails
        {
            get { return objbill; }
            set { objbill = value; }
        }

        public List<clsChargeVO> objChargeDetails = null;
        public List<clsChargeVO> ChargeDetails
        {
            get { return objChargeDetails; }
            set { objChargeDetails = value; }
        }



    }
}
