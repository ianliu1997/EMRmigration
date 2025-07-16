using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetDoctorAppointmentSlotVO : IBizActionValueObject
    {
        private long _DoctorID;
        public long DoctorID
        {
            get { return _DoctorID; }
            set { _DoctorID = value; }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (value != _UnitID)
                {
                    _UnitID = value;
                }
            }
        }

        private long _AppointmentSlot;
        public long AppointmentSlot
        {
            get { return _AppointmentSlot; }
            set
            {
                if (value != _AppointmentSlot)
                {
                    _AppointmentSlot = value;
                }
            }
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

            return "PalashDynamics.BusinessLayer.Master.clsGetDoctorAppointmentSlot";
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
