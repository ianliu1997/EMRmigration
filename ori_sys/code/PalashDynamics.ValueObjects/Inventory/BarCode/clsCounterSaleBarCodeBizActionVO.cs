using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Inventory;

namespace PalashDynamics.ValueObjects.Inventory.BarCode
{
    public class clsCounterSaleBarCodeBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.BarCode.clsCounterSaleBarCodeBizAction";
        }
        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

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

        public long UnitID { get; set; }
        public long ItemID { get; set; }
        public String ItemCode { get; set; }
        public String BatchCode { get; set; }
        public long BatchID { get; set; }
        public long StoreId { get; set; }
        public int TotalRows { get; set; }
        // Added By Rohit On 23August2013 for the BarCode Functionality.
        public String BarCode { get; set; }
        // End
        private clsItemSalesDetailsVO objBarCode = new clsItemSalesDetailsVO();
        public clsItemSalesDetailsVO AppBarCode
        {
            get { return objBarCode; }
            set { objBarCode = value; }
        }

        public List<clsItemSalesDetailsVO> IssueList { get; set; }
        public List<clsItembatchSearchVO> ItemBatchListForBarCode { get; set; }
        
    }
}
