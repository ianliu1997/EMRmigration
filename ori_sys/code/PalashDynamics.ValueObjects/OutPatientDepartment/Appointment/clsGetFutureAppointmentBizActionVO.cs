using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Appointment
{
   public class clsGetFutureAppointmentBizActionVO:IBizActionValueObject
    {
          private List<clsAppointmentVO> objAppointment = new List<clsAppointmentVO>();
        public List<clsAppointmentVO> AppointmentList
        {
            get { return objAppointment; }
            set { objAppointment = value; }
        }

        private string _MRNO;
        public string MRNO
        {
            get { return _MRNO; }
            set { _MRNO = value; }
        }
        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set { _UnitID = value; }
        }
        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set { _PatientUnitID = value; }
        }
      private long _SpouseID;
        public long SpouseID
        {
            get { return _SpouseID; }
            set { _SpouseID = value; }
        }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public string LinkServer { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.Appointment.clsGetFutureAppointmentBizAction";
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

