using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
  public class clsGetDoctorTimeBizActionVO : IBizActionValueObject
    {

      public clsGetDoctorTimeBizActionVO()
      {

      }
      private List<clsAppointmentVO> _AppointmentDetails = new List<clsAppointmentVO>();

      public List<clsAppointmentVO> AppointmentDetailsList
      {
          get { return _AppointmentDetails; }
          set { _AppointmentDetails = value; }
      }

      public long? DoctorId { get; set; }

      private string _SearchExpression;
      public string InputSearchExpression
      {
          get { return _SearchExpression; }
          set { _SearchExpression = value; }
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
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsGetDoctorTimeBizAction";
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
