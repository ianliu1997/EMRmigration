using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory.WorkOrder;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
     public abstract class clsBaseWorkOrderDAL
    {
        static private clsBaseWorkOrderDAL _instance = null;

        public static clsBaseWorkOrderDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "Inventory.clsWorkOrderDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseWorkOrderDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        //public abstract IValueObject CheckContractValidity(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject GetRateContract(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject GetRateContractItemDetail(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject AddRateContract(IValueObject valueObject, clsUserVO UserVo);

        
        //public abstract IValueObject GetPurchaseOrderDetails(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject DeletePurchaseOrderItems(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject GetPendingPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject CancelPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject UpdatePurchaseOrderForApproval(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject UpdateRemarkForCancellationPO(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject ClosePurchaseOrderManually(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWorkOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWorkOrderDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteWorkOrderItems(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FreezWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateWorkOrderForApproval(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateRemarkForCancellationWO(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CancelWorkOrder(IValueObject valueObject, clsUserVO UserVo);
    }

}
