using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsGetVitalDetailsForRadiologyBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsGetVitalDetailsForRadiologyBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private clsVitalDetailsForRadiologyVO _VitalDetails = new clsVitalDetailsForRadiologyVO();
        public clsVitalDetailsForRadiologyVO VitalDetails
        {
            get { return _VitalDetails; }
            set { _VitalDetails = value; }
        }
    }
}
