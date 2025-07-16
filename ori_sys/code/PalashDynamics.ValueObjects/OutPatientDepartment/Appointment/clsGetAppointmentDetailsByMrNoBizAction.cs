using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
    public class clsGetAppointmentByMrNoBizActionVO : IBizActionValueObject
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

        private string _MrNo;
        public string MRNo
        {
            get { return _MrNo; }
            set { _MrNo = value; }

        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }

        }

        public string LinkServer { get; set; }

        public clsGetAppointmentByMrNoBizActionVO()
        {

        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsGetAppointmentByMrNoBizAction";
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
