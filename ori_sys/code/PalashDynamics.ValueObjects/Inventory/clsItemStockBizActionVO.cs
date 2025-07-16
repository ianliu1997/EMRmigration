using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsAddItemStockBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsAddItemStockBizAction";
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

        private clsItemStockVO objDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsItemStockVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsGetItemStockBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemStockBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }


        public long ItemID { get; set; }
        public long StoreID { get; set; }
        public long BatchID { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public List<clsItemStockVO> BatchList { get; set; }

        #region For Free Items

        public bool? IsFree { get; set; }  // Set to show only Free Item Batches

        #endregion

        #region For GRN To QS
        public bool IsForGRNItemsToQS = false;  // Set to get GRN Items Batches
        public long TransactionID { get; set; }
        public long UnitID { get; set; }
        #endregion
    }

    public class clsGetItemCurrentStockListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemCurrentStockListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private int _SuccessStatus;

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public string SortExpression { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long UnitID { get; set; }
        public long UserID { get; set; }
        public long ItemID { get; set; }
        public long StoreID { get; set; }
        public long BatchID { get; set; }
        public string BatchCode { get; set; }
        public string ItemName { get; set; }
        public bool IsStockZero { get; set; }
        public bool IsConsignment { get; set; }
        public long IndentID { get; set; }
        public long IndentUnitID { get; set; }
        public bool IsForCentralPurChaseFromApproveIndent { get; set; }
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public List<clsItemStockVO> BatchList { get; set; }


        // Begin : added by aniketk on 20-Oct-2018 for Item Group & Category filter
        public long ItemGroupID { get; set; }
        public long ItemCategoryID { get; set; }
        // End : added by aniketk on 20-Oct-2018 for Item Group & Category filter

    }

    public class clsGetAvailableStockListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetAvailableStockListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion



        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }



        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BatchCode { get; set; }
        public double AvailableStock { get; set; }
        public double PhysicalStock { get; set; }
        public double VarianceStock { get; set; }
        public double PurchaseRate { get; set; }
        public double VarianceAmount { get; set; }
        public long clinic { get; set; }
        public long store { get; set; }

        public List<clsItemStockVO> BatchList { get; set; }



    }

    public class clsGetLatestBatchBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            //throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.Inventory.clsGetLatestBatchBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        public long ItemID { get; set; }
        public long StoreID { get; set; }

        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public List<clsItemStockVO> BatchList { get; set; }
    }

}
