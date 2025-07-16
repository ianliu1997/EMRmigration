using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsFreezPurchaseOrderBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsFreezPurchaseOrderBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public Boolean IsEditMode { get; set; }
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

        public clsPurchaseOrderVO PurchaseOrder { get; set; }
        public List<clsPurchaseOrderDetailVO> PurchaseOrderList { get; set; }

    }
}
