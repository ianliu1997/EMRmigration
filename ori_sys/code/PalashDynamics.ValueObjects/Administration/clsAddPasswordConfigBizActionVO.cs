using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
   public class clsAddPasswordConfigBizActionVO:IBizActionValueObject
   {
       #region IBizActionVO Member
       public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdatePasswordConfigBizAction";
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

        private clsPassConfigurationVO objPasswordConfig = null;
       /// <summary>
       /// Output Property.
       /// This Property Contains Password Configuration Details Which is Added.
       /// </summary>

        public clsPassConfigurationVO PasswordConfig
        {
            get { return objPasswordConfig;}
            set { objPasswordConfig = value; }
        }
   }
}
