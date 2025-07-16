using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public  class clsCancelAppointmentBizActionVO:IBizActionValueObject
    {
       public clsAppointmentVO AppointmentDetails { get; set; }


       #region IBizAction Members
       /// <summary>
       /// Retuns the bizAction Class Name.
       /// </summary>
       /// <returns></returns>
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsCancelAppointmentBizAction";
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
