using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM.LoyaltyProgram
{
    public class clsFillFamilyTariffUsingRelationIDBizActionVO : IBizActionValueObject
    {
        private clsLoyaltyProgramFamilyDetails _Details;
        public clsLoyaltyProgramFamilyDetails Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        public long RelationID { get; set; }
        public long LoyaltyProgramID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsFillFamilyTariffUsingRelationIDBizAction";
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
