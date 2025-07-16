using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.WorkOrder
{
     public class clsFreezWorkOrderBizActionVO :IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsFreezWorkOrderBizAction";
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

        public clsWorkOrderVO PurchaseOrder { get; set; }
        public List<clsWorkOrderDetailVO> PurchaseOrderList { get; set; }

    }
}
    

