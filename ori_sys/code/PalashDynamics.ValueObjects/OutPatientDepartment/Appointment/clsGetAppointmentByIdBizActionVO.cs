using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public class clsGetAppointmentByIdBizActionVO:IBizActionValueObject
    {

        private clsAppointmentVO objAppointment = null;
        public clsAppointmentVO AppointmentDetails
        {
            get { return objAppointment; }
            set { objAppointment = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        #region IBizAction Members
        /// <summary>
        /// Retuns the bizAction Class Name.
        /// </summary>
        /// <returns></returns>
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsGetAppointmentByIdBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
