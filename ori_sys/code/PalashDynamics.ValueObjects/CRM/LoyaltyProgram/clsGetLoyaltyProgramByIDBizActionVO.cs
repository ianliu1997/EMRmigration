using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
  public class clsGetLoyaltyProgramByIDBizActionVO:IBizActionValueObject
    {

        private clsLoyaltyProgramVO objDetails = null;
        public clsLoyaltyProgramVO LoyaltyProgramDetails
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
            return "PalashDynamics.BusinessLayer.CRM.clsGetLoyaltyProgramByIDBizAction";
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
