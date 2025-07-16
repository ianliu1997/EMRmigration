using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetDoctorScheduleMasterBizActionVO:IBizActionValueObject
    {
        private clsDoctorScheduleVO _Details;
        public clsDoctorScheduleVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        public string DayID { get; set; }
        public string Schedule1_StartTime { get; set; }
        public string Schedule1_EndTime { get; set; }
        public string Schedule2_EndTime { get; set; }
        public string Schedule2_StartTime { get; set; }

        public long DoctorID { get; set; }
        public long UnitID { get; set; }
        public long DepartmentID { get; set; }

        public bool SuccessStatus { get; set; }
        public long Status { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetDoctorScheduleMasterBizAction";
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
