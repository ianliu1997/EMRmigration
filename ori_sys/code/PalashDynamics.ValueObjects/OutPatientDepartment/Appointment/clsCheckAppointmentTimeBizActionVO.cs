using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public  class clsCheckAppointmentTimeBizActionVO:IBizActionValueObject
    {
        private clsAppointmentVO _Details;
        public clsAppointmentVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        public long DoctorId { get; set; }
        
        public bool SuccessStatus { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsCheckAppointmentTimeBizAction";
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
