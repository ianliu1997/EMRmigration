using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public class clsUpdateLoyaltyProgramBizActionVO:IBizActionValueObject
    {


       private clsLoyaltyProgramVO objLoyaltyProgram;
       public clsLoyaltyProgramVO LoyaltyProgram
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
            return "PalashDynamics.BusinessLayer.CRM.clsUpdateLoyaltyProgramBizAction";
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
