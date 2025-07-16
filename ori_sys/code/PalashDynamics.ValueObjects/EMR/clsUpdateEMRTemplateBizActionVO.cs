using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsUpdateEMRTemplateBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdateEMRTemplateBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

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

        private clsEMRTemplateVO objEMRTemplate = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsEMRTemplateVO EMRTemplateDetails
        {
            get { return objEMRTemplate; }
            set { objEMRTemplate = value; }
        }
    }
}
