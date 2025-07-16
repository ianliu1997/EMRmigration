using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsGetDoctorPaymentDetailListByBillIDBizActionVO:IBizActionValueObject
    {
        private List<clsDoctorPaymentVO> _DoctorPaymentDetailsList;
        public List<clsDoctorPaymentVO> DoctorPaymentDetailsList
        {
            get { return _DoctorPaymentDetailsList; }
            set { _DoctorPaymentDetailsList = value; }
        }

        private clsDoctorPaymentVO _DoctorPaymentDetails;
        public clsDoctorPaymentVO DoctorPaymentDetails
        {
            get { return _DoctorPaymentDetails; }
            set { _DoctorPaymentDetails = value; }
        }

        public List<long> BillIdList { get; set; }
        public long BillID { get; set; }
        public long UnitID { get; set; }
        public long DoctorID { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorPaymentDetailListByBillIDBizAction";
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
