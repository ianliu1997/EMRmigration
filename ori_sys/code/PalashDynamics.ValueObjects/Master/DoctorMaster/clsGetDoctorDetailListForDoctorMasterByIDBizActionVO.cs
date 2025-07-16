using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
   public class clsGetDoctorDetailListForDoctorMasterByIDBizActionVO:IBizActionValueObject
    {
        private clsDoctorVO objDetails = null;
        public clsDoctorVO DoctorDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private long _ID;
        public long DoctorID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _Status;
        public long Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        //public byte[] Photo { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetDoctorDetailListForDoctorMasterByIDBizAction";
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
