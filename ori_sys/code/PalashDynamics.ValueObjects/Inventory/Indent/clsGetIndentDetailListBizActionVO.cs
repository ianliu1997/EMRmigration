using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetIndentDetailListBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetIndentDetailListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion


        public long IndentID { get; set; }
        public long UnitID { get; set; }

        public List<clsIndentDetailVO> IndentDetailList { get; set; }



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
