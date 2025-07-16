using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory.WorkOrder
{
    public class clsAddWorkOrderBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsAddWorkOrderBizAction";
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
        public clsWorkOrderDetailVO PurchaseOrderItems { get; set; }
        public List<clsWorkOrderDetailVO> PurchaseOrderList  { get; set; }

        public List<clsWorkOrderDetailVO> POIndentList { get; set; }
        public List<clsWorkOrderTerms> POTerms { get; set; }
    }


    public class clsCancelWorkOrderBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.WorkOrder.clsCancelWorkOrderBizAction";
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
        public clsWorkOrderVO WorkOrder { get; set; }

       




    }


    public class clsAddRateContractBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsAddRateContractBizAction";
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

        public clsRateContractVO RateContract { get; set; }
     


    }


    public class clsGetRateContractBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsGetRateContractBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public Boolean IsEditMode { get; set; }
     
        public string Code { get; set; }
        public string Description { get; set; }
        public long SupplierID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

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

        private List<clsRateContractVO> _RateContract=new List<clsRateContractVO>();
        public List<clsRateContractVO> RateContract
        {
            get
            {
                return _RateContract;
            }
            set
            {
                _RateContract = value;
            }
        }

        public int MaximumRows { get; set; }
        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public int TotalRowCount { get; set; }
        public string sortExpression { get; set; }
        
    }

    public class clsGetRateContractItemDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsGetRateContractItemDetailsBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }


  
        public long ContractID { get; set; }
        public long ContractUnitId { get; set; }     

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

        private List<clsRateContractDetailsVO> _RateContractList = new List<clsRateContractDetailsVO>();
        public List<clsRateContractDetailsVO> RateContractList
        {
            get
            {
                return _RateContractList;
            }
            set
            {
                _RateContractList = value;
            }
        }

  

    }

    public class clsCheckContractBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsCheckContractBizAction";
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public Boolean Result = false;


        public string ItemIDs { get; set; }
        public long SupplierID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

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




    }

    public class clsAddWorkOrderTermsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsAddPurchaseOrderTermsBizAction";
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
        public clsWorkOrderTerms POterms { get; set; }
    }
    
}
