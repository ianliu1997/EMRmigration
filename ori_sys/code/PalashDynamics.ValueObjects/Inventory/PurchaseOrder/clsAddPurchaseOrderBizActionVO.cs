using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.ValueObjects.Inventory.PurchaseOrder
{
    public class clsAddPurchaseOrderBizActionVO:IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsAddPurchaseOrderBizAction";
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

        public string ItemCode = string.Empty;

        // For the Activity Log List
        private List<LogInfo> _LogInfoList = null;
        public List<LogInfo> LogInfoList
        {
            get { return _LogInfoList; }
            set { _LogInfoList = value; }
        }

        public clsPurchaseOrderVO PurchaseOrder { get; set; }
        public clsPurchaseOrderDetailVO PurchaseOrderItems { get; set; }
        public List<clsPurchaseOrderDetailVO> PurchaseOrderList  { get; set; }

        public List<clsPurchaseOrderDetailVO> POIndentList { get; set; }
        public List<clsPurchaseOrderTerms> POTerms { get; set; }
    }
    public class clsCancelPurchaseOrderBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.PurchaseOrder.clsCancelPurchaseOrderBizAction";
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

    public class clsAddPurchaseOrderTermsBizActionVO : IBizActionValueObject
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
        public clsPurchaseOrderTerms POterms { get; set; }
    }
}
