using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master.DoctorMaster
{
    public class clsAddDoctorWaiverDetailsBizActionVO : IBizActionValueObject
    {
        private clsDoctorWaiverDetailVO _DoctorWaiverDetails;
        public clsDoctorWaiverDetailVO DoctorWaiverDetails
        {
            get { return _DoctorWaiverDetails; }
            set { _DoctorWaiverDetails = value; }
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
            return "PalashDynamics.BusinessLayer.Master.clsAddDoctorWaiverDetailsBizAction";
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
