using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.WorkOrder
{
   public  class clsGetWorkOrderDetailsBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsGetWorkOrderDetailsBizAction";
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
        public bool IsPagingEnabled { get; set; }

        public Int32 StartIndex { get; set; }

        public Int32 MinRows { get; set; }
      //Added By Somnath
        public int TotalRowCount { get; set; }
      //End
        public long SearchID { get; set; }
        public long UnitID { get; set; }
       
        public clsWorkOrderVO PurchaseOrder { get; set; }

        public Boolean FilterPendingQuantity { get; set; }
      public List<clsWorkOrderDetailVO> PurchaseOrderList { get; set; }
      public List<clsWorkOrderDetailVO> PoIndentList { get; set; }
      public List<clsWorkOrderTerms> POTerms { get; set; }
    }
   
}
