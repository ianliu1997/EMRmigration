using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.StaffMaster
{
   public class clsGetStaffMasterDetailsByIDBizActionVO:IBizActionValueObject
    {


        private clsStaffMasterVO myVar = new clsStaffMasterVO();
        public clsStaffMasterVO StaffMasterList
        {
            get { return myVar; }
            set { myVar = value; }
        }
        private clsStaffMasterVO objDetails = null;
        public clsStaffMasterVO StaffDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private long _ID;
        public long StaffId
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
       
        public long StaffID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.StaffMaster.clsGetStaffMasterDetailsByIDBizAction";
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
