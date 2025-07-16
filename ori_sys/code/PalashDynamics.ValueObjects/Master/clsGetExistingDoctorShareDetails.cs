using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsGetExistingDoctorShareDetails : IBizActionValueObject
    {
        private clsDoctorVO objDetails = null;
        public clsDoctorVO DoctorDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
        private List<clsDoctorShareServicesDetailsVO> objList = null;
        public List<clsDoctorShareServicesDetailsVO> DoctorList
        {
            get { return objList; }
            set { objList = value; }
        }

        private long _ID;
        public long DoctorID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _DoctorIDs;
        public string DoctorIDs
        {
            get { return _DoctorIDs; }
            set { _DoctorIDs = value; }
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
            return "PalashDynamics.BusinessLayer.Master.clsGetExistingDoctorShareDetailsBizAction";
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
