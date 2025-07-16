using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
  public  class clsAddDoctorScheduleMasterBizActionVO:IBizActionValueObject
    {

        public clsAddDoctorScheduleMasterBizActionVO()
        {

        }

        private clsDoctorScheduleVO objDoctorSchedule = null;
        public clsDoctorScheduleVO DoctorScheduleDetails
        {
            get { return objDoctorSchedule; }
            set { objDoctorSchedule = value; }

        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public bool IsNewSchedule { get; set; }     // added on 14032018 for New Doctor Schedule

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddDoctorScheduleMasterBizAction";
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
