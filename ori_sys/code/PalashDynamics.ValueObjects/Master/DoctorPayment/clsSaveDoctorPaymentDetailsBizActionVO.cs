using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsSaveDoctorPaymentDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorPaymentVO> _DoctorDetails;
        public List<clsDoctorPaymentVO> DoctorDetails
        {
            get { return _DoctorDetails; }
            set { _DoctorDetails = value; }
        }
        private List<clsPaymentDetailsVO> _PaymentDetailDetails;
        public List<clsPaymentDetailsVO> PaymentDetailDetails
        {
            get { return _PaymentDetailDetails; }
            set { _PaymentDetailDetails = value; }
        }
        private clsDoctorPaymentVO _DoctorInfo;
        public clsDoctorPaymentVO DoctorInfo
        {
            get { return _DoctorInfo; }
            set { _DoctorInfo = value; }
        }
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsSaveDoctorPaymentDetailsBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public long DoctorServiceLinkingTypeID { get; set;}
        public long DoctorID { get; set; }
        public long UnitID { get; set; }
        public long BillID { get; set; }
        public long BillUnitID { get; set; }
        public DateTime PaymentDate { get; set; }
        public double TotalAmount { get; set; }
        public bool IsPaymentDone { get; set; }
        public double DoctorPaidAmount { get; set; }
        public double BalanceAmount { get; set; }
    }

    public class clsSaveDoctorSettlePaymentDetailsBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorPaymentVO> _DoctorPayment;
        public List<clsDoctorPaymentVO> DoctorPayment
        {
            get { return _DoctorPayment; }
            set { _DoctorPayment = value; }
        }
        private List<clsPaymentDetailsVO> _DoctorPaymentDetails;
        public List<clsPaymentDetailsVO> DoctorPaymentDetails
        {
            get { return _DoctorPaymentDetails; }
            set { _DoctorPaymentDetails = value; }
        }
        private clsDoctorPaymentVO _DoctorPaymentInfo;
        public clsDoctorPaymentVO DoctorPaymentInfo
        {
            get { return _DoctorPaymentInfo; }
            set { _DoctorPaymentInfo = value; }
        }
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsSaveDoctorSettlePaymentDetailsBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public string VoucherNo { get; set; }
        public long DoctorServiceLinkingTypeID { get; set; }
        public long DoctorID { get; set; }
        public long UnitID { get; set; }
        public long BillID { get; set; }
        public long BillUnitID { get; set; }
        public DateTime PaymentDate { get; set; }
        public double TotalAmount { get; set; }
        public bool IsPaymentDone { get; set; }
        public double DoctorPaidAmount { get; set; }
        public double BalanceAmount { get; set; }
    }
}
