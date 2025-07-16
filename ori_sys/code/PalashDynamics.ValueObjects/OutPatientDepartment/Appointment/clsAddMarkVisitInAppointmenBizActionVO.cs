using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
  public class clsAddMarkVisitInAppointmenBizActionVO:IBizActionValueObject
    {
        public clsAppointmentVO AppointmentDetails { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsAddMarkVisitInAppointmenBizAction";
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
