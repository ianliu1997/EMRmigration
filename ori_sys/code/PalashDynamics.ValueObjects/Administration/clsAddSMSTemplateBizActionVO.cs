using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsAddSMSTemplateBizActionVO :IBizActionValueObject
    {

        public string GetBizAction()
        {
           return "PalashDynamics.BusinessLayer.Administration.clsAddSMSTemplateBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
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

        private clsSMSTemplateVO objSMSTemplate = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Password Configuration Details Which is Added.
        /// </summary>

        public clsSMSTemplateVO SMSTemplate
        {
            get { return objSMSTemplate; }
            set { objSMSTemplate = value; }
        }
    }
}
