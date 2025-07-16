using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsGetDoctorPaymentAllBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorPaymentVO> _DoctorDetails;
        public List<clsDoctorPaymentVO> DoctorDetails
        {
            get { return _DoctorDetails; }
            set { _DoctorDetails = value; }
        }
        private clsDoctorPaymentVO _DoctorInfo;
        public clsDoctorPaymentVO DoctorInfo
        {
            get { return _DoctorInfo; }
            set { _DoctorInfo = value; }
        }


        public long DoctorId { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorPaymentAllBizAction";
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
