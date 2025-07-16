using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.DoctorScheduleMaster
{
   public class clsCheckTimeForScheduleExistanceBizActionVO:IBizActionValueObject
    {
        private List<clsDoctorScheduleDetailsVO> _Details;
        public List<clsDoctorScheduleDetailsVO> Details
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

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsCheckTimeForScheduleExistanceBizAction";
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
