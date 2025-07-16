using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsGetDoctorWaiverDetailListByIDBizActionVO : IBizActionValueObject
    {
        private List<clsDoctorWaiverDetailVO> _DoctorWaiverDetailsList;
        public List<clsDoctorWaiverDetailVO> DoctorWaiverDetailsList
        {
            get { return _DoctorWaiverDetailsList; }
            set { _DoctorWaiverDetailsList = value; }
        }

        private clsDoctorWaiverDetailVO _DoctorWaiverDetails;
        public clsDoctorWaiverDetailVO DoctorWaiverDetails
        {
            get { return _DoctorWaiverDetails; }
            set { _DoctorWaiverDetails = value; }
        }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long UnitID { get; set; }

        public bool IsPagingEnabled { get; set; }

        public int StartRowIndex { get; set; }

        public int MaximumRows { get; set; }

        public string SearchExpression { get; set; }

        public int TotalRows { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorWaiverDetailListByIDBizAction";
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
