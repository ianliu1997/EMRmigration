using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
    public class clsGetPassConfigBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsGetPassConfigBizAction";
                                                                
        }

        #region IValueObject Member
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
        /// 
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsPassConfigurationVO objPassConfig = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>

        public clsPassConfigurationVO PassConfig
        {
            get { return objPassConfig; }
            set { objPassConfig = value; }
        }
    }

}
