using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
  public class clsAddPatientServiceDetailsVO:IBizActionValueObject
    {
        private List<clsPatientServiceDetails> objPatient = null;
        public List<clsPatientServiceDetails> PatientServiceList
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {

            return "PalashDynamics.BusinessLayer.clsAddPatientServiceDetails";
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
