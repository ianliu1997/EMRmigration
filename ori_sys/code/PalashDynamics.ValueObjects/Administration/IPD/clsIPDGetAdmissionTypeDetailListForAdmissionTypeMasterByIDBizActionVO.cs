using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.IPD
{

    public class clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO : IBizActionValueObject
    {
        private clsIPDAdmissionTypeVO objDetails = null;
        public clsIPDAdmissionTypeVO DoctorDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private long _ID;
        public long AdmissionTypeID
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


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.IPD.clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizAction";
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
