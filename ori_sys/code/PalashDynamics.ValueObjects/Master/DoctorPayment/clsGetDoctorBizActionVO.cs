using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorPayment
{
    public class clsGetDoctorBizActionVO : IBizActionValueObject
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

        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitId { get; set; }
        public long DepartmentId { get; set; }
        public long ClassificationId { get; set; }
        public long DoctorTypeId { get; set; }


        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.DocPayment.clsGetDoctorBizAction";
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
