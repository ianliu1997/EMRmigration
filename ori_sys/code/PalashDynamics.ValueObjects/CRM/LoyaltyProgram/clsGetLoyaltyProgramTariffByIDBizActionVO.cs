using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
   public  class clsGetLoyaltyProgramTariffByIDBizActionVO:IBizActionValueObject
    {
       private clsLoyaltyProgramVO _Details;
       public clsLoyaltyProgramVO Details
       {
           get { return _Details; }
           set { _Details = value; }
       }

       public long ID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CRM.clsGetLoyaltyProgramTariffByIDBizAction";
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
