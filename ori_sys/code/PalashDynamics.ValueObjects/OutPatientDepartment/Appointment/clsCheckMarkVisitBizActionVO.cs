using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public class clsCheckMarkVisitBizActionVO:IBizActionValueObject
    {

        private clsAppointmentVO _Details;
        public clsAppointmentVO Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        public long ID { get; set; }
        public long UnitID { get; set; }
        public bool VisitMark { get; set; }
        public bool SuccessStatus { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsCheckMarkVisitBizAction";
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
