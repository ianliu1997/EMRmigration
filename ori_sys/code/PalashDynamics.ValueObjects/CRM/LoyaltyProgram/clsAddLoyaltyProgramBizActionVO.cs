using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
  public  class clsAddLoyaltyProgramBizActionVO:IBizActionValueObject
    {
      private clsLoyaltyProgramVO objLoyaltyProgram;
        public clsLoyaltyProgramVO LoyaltyProgramDetails
        {
            get { return objLoyaltyProgram; }
            set { objLoyaltyProgram = value; }
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
            return "PalashDynamics.BusinessLayer.CRM.clsAddLoyaltyProgramBizAction";
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
