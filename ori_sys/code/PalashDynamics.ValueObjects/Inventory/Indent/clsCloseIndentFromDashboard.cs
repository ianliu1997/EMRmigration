using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsCloseIndentFromDashboard : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return " PalashDynamics.BusinessLayer.Inventory.clsCloseIndentFromDashboardBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion
        public long? UnitID { get; set; }
        public long? IndentID { get; set; }
        public bool isIndentClosed { get; set; }

        public bool isBulkIndentClosed { get; set; }  // x-x-x-x-x-x-x-x-x-x The below variable is added to confirm Bulk Indent Close status (Rex Mathew) x-x-x-x-x-x-x-x-x-x
        public bool isBulkPRClosed { get; set; }  // x-x-x-x-x-x-x-x-x-x The below variable is added to confirm Bulk PR Close status (Rex Mathew) x-x-x-x-x-x-x-x-x-x

        public bool isPRCloseCall { get; set; }   // x-x-x-x-x-x-x-x-x-x The below variable is added to confirm Bulk PR Close status (Rex Mathew) x-x-x-x-x-x-x-x-x-x

        public List<clsIndentMasterVO> BulkCloseIndetList { get; set; }      // x-x-x-x-x-x-x-x-x-x This List to store Bulk Close Indent List (Rex Mathew) x-x-x-x-x-x-x-x-x-x


        public string Remarks { get; set; }
       
        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        /// 
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
