using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class GetItemListByIndentIdForIssueItemBizActionVO : IBizActionValueObject
    {
         #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.GetItemListByIndentIdForIssueItemBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

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

        public List<clsIssueItemDetailsVO> IssueItemDetailsList { get; set; }
        public long? IndentID { get; set; }
        public long? IssueFromStoreId { get; set; }
        public long? IssueToStoreId { get; set; }
        public long? UnitID { get; set; }
        public long?IndentUnitID { get; set; }
        public long? ItemID { get; set; }
        public InventoryTransactionType TransactionType { get; set; }




       


    }

}
