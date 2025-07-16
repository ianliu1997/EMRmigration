using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM.LoyaltyProgram
{
  public class clsGetLoyaltyPatientCategoryBizActionVO:IBizActionValueObject
    {
        private List<clsLoyaltyProgramPatientCategoryVO> _LoyaltyProgramList;
        public List<clsLoyaltyProgramPatientCategoryVO> CategoryList
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
            return "PalashDynamics.BusinessLayer.CRM.clsGetLoyaltyPatientCategoryBizAction";
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
