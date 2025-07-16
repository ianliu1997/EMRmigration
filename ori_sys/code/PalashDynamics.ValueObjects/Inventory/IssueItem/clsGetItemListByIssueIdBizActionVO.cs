using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemListByIssueIdBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListByIssueIdBizAction";
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


        public List<clsIssueItemDetailsVO> ItemList { get; set; }
        public long? IssueId { get; set; }
        public List<clsReturnItemDetailsVO> ReturnItemList { get; set; }
        public List<clsReceivedItemDetailsVO> ReceivedItemList { get; set; }
        public long UnitID { get; set; }
        public bool flagReceivedIssue { get; set; }
        public long ToStoreID { get; set; }
        public InventoryTransactionType TransactionType { get; set; }


        public long PatientID = 0;
        public long PatientunitID = 0;
        public bool IsAgainstPatient = false;


    }

    public class clsGetItemListByIssueIdSrchBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListByIndentIdSrchBizAction";
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

        public List<clsItemListByIssueId> ItemList { get; set; }
        public long? IssueId { get; set; }

    }

    public class clsItemListByIssueId
    {
        private Boolean? _IsChecked = false;
        public Boolean? IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                _IsChecked = value;
            }
        }
        public long? ItemId { get; set; }
        public String ItemName { get; set; }
        public String ItemCode { get; set; }
        public long? IssueId { get; set; }
        public String IndentNumber { get; set; }
        public decimal? IssueQty { get; set; }
        public bool flagReceiveIssue { get; set; }
        //public string PUM { get; set; }

    }
}
