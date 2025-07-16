using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Inventory.WorkOrder;

namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsUpdateWorkOrderForApproval : IBizActionValueObject
    {
           
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsUpdateWorkOrderForApprovalBizAction";
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
        public List<clsWorkOrderDetailVO> POIndentList { get; set; }
    }

    public class clsUpdatePurchaseOrderForApproval : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsUpdatePurchaseOrderForApprovalBizAction";
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
        public List<clsPurchaseOrderDetailVO> POIndentList { get; set; }
    }

    public class clsClosePurchaseOrderManuallyBizActionVO: IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsClosePurchaseOrderManuallyBizAction";
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
    }
}
