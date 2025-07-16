using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
   public  class clsGetTariffAndRelationFromApplicationConfigurationBizActionVO:IBizActionValueObject
    {
        private List<clsPatientFamilyDetailsVO> objPatient = null;
        public List<clsPatientFamilyDetailsVO> FamilyDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.clsGetTariffAndRelationFromApplicationConfigurationBizAction";
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
