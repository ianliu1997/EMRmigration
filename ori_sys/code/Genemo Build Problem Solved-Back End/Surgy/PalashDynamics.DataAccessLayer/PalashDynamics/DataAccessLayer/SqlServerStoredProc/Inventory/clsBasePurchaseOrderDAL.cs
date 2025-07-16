namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePurchaseOrderDAL
    {
        private static clsBasePurchaseOrderDAL _instance;

        protected clsBasePurchaseOrderDAL()
        {
        }

        public abstract IValueObject AddPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddRateContract(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CancelPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckContractValidity(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ClosePurchaseOrderManually(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeletePurchaseOrderItems(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FreezPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePurchaseOrderDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Inventory.clsPurchaseOrderDAL";
                    _instance = (clsBasePurchaseOrderDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPendingPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPurchaseOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPurchaseOrderDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPurchaseOrderDetailsForGRNAgainstPOSearch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPurchaseOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRateContract(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRateContractItemDetail(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateForPOCloseDuration(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePurchaseOrderForApproval(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateRemarkForCancellationPO(IValueObject valueObject, clsUserVO UserVo);
    }
}

