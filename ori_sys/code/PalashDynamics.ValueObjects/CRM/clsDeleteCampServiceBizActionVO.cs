using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public  class clsDeleteCampServiceBizActionVO:IBizActionValueObject
    {

        private clsCampMasterVO objDetails = null;
        public clsCampMasterVO CampMasterDetails
        {
            get { return objDetails; }
            set { objDetails = value; }

        }

        private long _ID;
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsDeleteCampServiceBizAction";
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
