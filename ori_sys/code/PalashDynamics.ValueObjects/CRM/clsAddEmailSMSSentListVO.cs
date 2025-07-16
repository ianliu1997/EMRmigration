using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CRM
{
  public class clsAddEmailSMSSentListVO:IBizActionValueObject
    {
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

        private clsEmailSMSSentListVO objEmailTemplate = null;

        /// <summary>
        /// Output Property.
        /// This Property Contains Password Configuration Details Which is Added.
        /// </summary>

        public clsEmailSMSSentListVO EmailTemplate
        {
            get { return objEmailTemplate; }
            set { objEmailTemplate = value; }
        }

      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.CRM.clsAddEmailSMSSentDetailsBizAction";
      }

      public string ToXml()
      {
          return this.ToString();
      }
    }
}
