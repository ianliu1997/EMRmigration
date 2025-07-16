using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public  class clsUpdateCampDetailsBizActionVO:IBizActionValueObject

    {
        private clsCampMasterVO objCampMaster;
        public clsCampMasterVO CampMasterDetails
        {
            get { return objCampMaster; }
            set { objCampMaster = value; }
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
            return "PalashDynamics.BusinessLayer.CRM.clsUpdateCampDetailsBizAction";
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
