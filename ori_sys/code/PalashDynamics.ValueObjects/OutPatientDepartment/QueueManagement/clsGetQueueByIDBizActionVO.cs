using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.QueueManagement
{
    public class clsGetQueueByIDBizActionVO : IBizActionValueObject
    {

        public clsQueueVO _Objqueue;
        public clsQueueVO QueueDetails
        {
            get { return _Objqueue; }
            set { _Objqueue = value; }
        }

        public long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set { _PatientUnitID = value; }
        }

        public bool _Status;
        public bool Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.OutPatientDepartment.QueueManagement.clsGetQueueByIDBizAction";
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

