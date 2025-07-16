using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddIndentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddIndentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        

        public clsIndentMasterVO objIndent { get; set; }

        public List<clsIndentDetailVO> objIndentDetailList { get; set; }

        public bool IsConvertToPR { get; set; }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }
}
