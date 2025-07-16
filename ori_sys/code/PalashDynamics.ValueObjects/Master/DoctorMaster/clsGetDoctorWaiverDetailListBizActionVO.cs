using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Master.DoctorMaster;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetDoctorWaiverDetailListBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorWaiverDetailVO> _DoctorWaiverDetails;
        public List<clsDoctorWaiverDetailVO> DoctorWaiverDetails
        {
            get { return _DoctorWaiverDetails; }
            set { _DoctorWaiverDetails = value; }
        }
        private clsDoctorWaiverDetailVO _DoctorWaiverDetailsInfo;
        public clsDoctorWaiverDetailVO DoctorWaiverDetailsInfo
        {
            get { return _DoctorWaiverDetailsInfo; }
            set { _DoctorWaiverDetailsInfo = value; }
        }
        public string PageName { get; set; }
        public long UnitID { get; set; }
        public long DepartmentID { get; set; }
        public long DoctorID { get; set; }
        public long TariffID { get; set; }
        public long ServiceID { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDoctorWaiverDetailListBizAction";
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
