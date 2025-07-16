using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public  class clsAddDoctorDepartmentDetailsBizActionVO:IBizActionValueObject
    {
        private clsDoctorVO _DepartmentDetails;
        public clsDoctorVO DepartmentDetails
        {
            get { return _DepartmentDetails; }
            set { _DepartmentDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

 
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddDoctorDepartmentDetailsBizAction";
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
