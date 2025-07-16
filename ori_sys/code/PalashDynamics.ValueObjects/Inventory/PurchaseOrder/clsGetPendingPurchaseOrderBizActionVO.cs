using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
  public class clsGetPendingPurchaseOrderBizActionVO:IBizActionValueObject
    {
        public DateTime? Date { get; set; }
        public long UnitID { get; set; }
        public long UserID { get; set; }

        public Boolean? IsOrderBy { get; set; }

        public Boolean IsPagingEnabled { get; set; }
        public long? StartIndex { get; set; }
        public long? NoOfRecordShow { get; set; }
        private long _TotalRows = 0;
        public DateTime searchFromDate { get; set; }
        public DateTime searchToDate { get; set; }
        public long SearchStoreID { get; set; }
        public long SearchSupplierID { get; set; }
        public string PONO { get; set; }

        public long OutputTotalRows
        {
            get { return _TotalRows; }
            set { _TotalRows = value; }
        }

        public clsPurchaseOrderVO PurchaseOrder { get; set; }
        public List<clsPurchaseOrderVO> PurchaseOrderList { get; set; }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsGetPendingPurchaseOrderBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }
    }
}
