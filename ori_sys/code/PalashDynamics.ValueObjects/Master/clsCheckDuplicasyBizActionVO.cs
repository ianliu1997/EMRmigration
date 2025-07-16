using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsCheckDuplicasyBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsCheckDuplicasyBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public clsCheckDuplicasyBizActionVO()
        {

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

        public clsCheckDuplicasyVO objDuplicasyVO { get; set; }
        public List<clsCheckDuplicasyVO> lstDuplicasy { get; set; }
        public string ItemName { get; set; }
        public string BatchCode { get; set; }
        public bool IsBatchRequired { get; set; }
    }
}
