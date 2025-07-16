using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsUpdateRemarkForCancellationPO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsUpdateRemarkForCancellationBizAction";
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

        public Boolean IsPOAutoCloseCalled { get; set; }    // This vairable is used to call DAL method to change the Colse Duration for a Purchase Order : added on 12Oct2018 by RexM

    }
}
