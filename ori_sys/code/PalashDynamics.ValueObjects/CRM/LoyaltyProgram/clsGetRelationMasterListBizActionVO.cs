using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public class clsGetRelationMasterListBizActionVO : IBizActionValueObject
    {
        private List<clsLoyaltyProgramFamilyDetails> _LoyaltyProgramList;
        public List<clsLoyaltyProgramFamilyDetails> FamilyList
        {
            get { return _LoyaltyProgramList; }
            set { _LoyaltyProgramList = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetRelationMasterListBizAction";
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
