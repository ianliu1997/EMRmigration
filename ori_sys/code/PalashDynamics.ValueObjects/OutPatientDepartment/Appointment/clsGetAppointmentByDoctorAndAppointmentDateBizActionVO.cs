using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
    public class clsGetAppointmentByDoctorAndAppointmentDateBizActionVO : IBizActionValueObject
    {

        private List<clsAppointmentVO> myVar = new List<clsAppointmentVO>();

        public List<clsAppointmentVO> AppointmentDetailsList
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public long? DoctorId { get; set; }
        public long? DepartmentId { get; set; }
        public long? UnitId { get; set; }

        public bool SuccessStatus { get; set; }
        
        public DateTime? AppointmentDate { get; set; }


        public string LinkServer { get; set; }
        

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.GetAppointmentByDoctorAndAppointmentDateBizAction";
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

