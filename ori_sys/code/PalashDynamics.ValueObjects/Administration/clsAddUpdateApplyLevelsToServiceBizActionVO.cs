using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration
{
  public  class clsAddUpdateApplyLevelsToServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionVO Member
       public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.clsAddUpdateApplyLevelsToServiceBizAction";
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

        private clsServiceLevelsVO _obj = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains Password Configuration Details Which is Added.
        /// </summary>

        public clsServiceLevelsVO Obj
        {
            get { return _obj; }
            set { _obj = value; }
        }
   }


  public class clsGetApplyLevelsToServiceBizActionVO : IBizActionValueObject
  {
      #region IBizActionVO Member
      public string GetBizAction()
      {
          return "PalashDynamics.BusinessLayer.Administration.clsGetApplyLevelsToServiceBizAction";
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

      private clsServiceLevelsVO _obj = null;
      /// <summary>
      /// Output Property.
      /// This Property Contains Password Configuration Details Which is Added.
      /// </summary>

      public clsServiceLevelsVO Obj
      {
          get { return _obj; }
          set { _obj = value; }
      }
  }
}
