using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.Administration.DiagnosisServiceDrugLink
{
    public class clsDeleteDiagnosisServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.DiagnosisServiceDrugLink.clsDeleteDiagnosisServiceBizAction";
        }
        #endregion


        #region IValueObject Members
        public string ToXml()
        {
            return this.ToString();
        }
        #endregion
        private clsEMRAddDiagnosisVO _DiagnosisDetails;
        public clsEMRAddDiagnosisVO DiagnosisDetails
        {
            get
            {
                return _DiagnosisDetails;
            }
            set
            {
                _DiagnosisDetails = value;
            }
        }
        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }

        }
    }
}
