using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsUpdateIndentBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsUpdateIndentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public clsIndentMasterVO objIndent { get; set; }
        public bool IsForChangeAndApproveIndent { get; set; }
        public bool IsForApproveDirect { get; set; } //***//
        


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
