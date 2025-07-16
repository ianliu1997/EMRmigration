using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public class clsCheckMRnoBizActionVO :IBizActionValueObject
    {
        private clsAppointmentVO _Details;
        public clsAppointmentVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        public string  MRNO { get; set; }
        public bool SuccessStatus { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsCheckMRnoBizAction";
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
