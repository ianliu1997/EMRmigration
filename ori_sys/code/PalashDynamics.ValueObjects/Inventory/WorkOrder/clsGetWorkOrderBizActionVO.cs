using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.WorkOrder
{
    public class clsGetWorkOrderBizActionVO: IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsGetWorkOrderBizAction";
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
        public clsWorkOrderVO WorkOrder { get; set; }
        public List<clsWorkOrderVO> WorkOrderList { get; set; }
        public bool CancelWO { get; set; }
        public int MaximumRows { get; set; }
        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int TotalRowCount { get; set; }

        public bool? Freezed { get; set; }
        public bool flagWOFromGRN { get; set; }
        public string WONO { get; set; }
    }

    public class clsGetWorkOrderForCloseBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsGetWorkOrderForCloseBizAction";
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

        public clsWorkOrderVO WorkOrder { get; set; }
        public List<clsWorkOrderVO> WorkOrderList { get; set; }

        public int MaximumRows { get; set; }
        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int TotalRowCount { get; set; }

        public string WONO { get; set; }
    }
}

   
    

