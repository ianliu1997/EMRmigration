using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM.LoyaltyProgram
{
   public  class clsGetLoyaltyClinicBizActionVO:IBizActionValueObject
    {
        private List<clsLoyaltyClinicVO> _LoyaltyProgramList;
        public List<clsLoyaltyClinicVO> ClinicList
        {
            get { return _LoyaltyProgramList; }
            set { _LoyaltyProgramList = value; }
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
            return "PalashDynamics.BusinessLayer.CRM.clsGetLoyaltyClinicBizAction";
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
