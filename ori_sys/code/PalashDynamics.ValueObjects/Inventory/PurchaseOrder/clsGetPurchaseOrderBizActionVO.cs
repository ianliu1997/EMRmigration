using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsGetPurchaseOrderBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsGetPurchaseOrderBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

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

        public DateTime? searchFromDate { get; set; }
        public long SearchDeliveryStoreID { get; set; }
        public DateTime? searchToDate { get; set; }
        public long SearchSupplierID { get; set; }
        public long UnitID { get; set; }
        public long SearchStoreID { get; set; }
        public clsPurchaseOrderVO PurchaseOrder { get; set; }
        public List<clsPurchaseOrderVO> PurchaseOrderList { get; set; }
        public bool CancelPO { get; set; }
        public bool UnApprovePo { get; set; }
        public bool ApprovePo { get; set; }
        public int MaximumRows { get; set; }
        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int TotalRowCount { get; set; }

        public bool? Freezed { get; set; }
        public bool flagPOFromGRN { get; set; }
        public string PONO { get; set; }
    }

    public class clsGetPurchaseOrderForCloseBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsGetPurchaseOrderForCloseBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

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

        public DateTime? searchFromDate { get; set; }
        public DateTime? searchToDate { get; set; }
        public long SearchSupplierID { get; set; }
        public long UnitID { get; set; }
        public long SearchStoreID { get; set; }
        public clsPurchaseOrderVO PurchaseOrder { get; set; }
        public List<clsPurchaseOrderVO> PurchaseOrderList { get; set; }

        public int MaximumRows { get; set; }
        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int TotalRowCount { get; set; }

        public string PONO { get; set; }
    }
}
