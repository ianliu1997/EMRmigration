namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseWorkOrderDAL
    {
        private static clsBaseWorkOrderDAL _instance;

        protected clsBaseWorkOrderDAL()
        {
        }

        public abstract IValueObject AddWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CancelWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteWorkOrderItems(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject FreezWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseWorkOrderDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Inventory.clsWorkOrderDAL";
                    _instance = (clsBaseWorkOrderDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetWorkOrder(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWorkOrderDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetWorkOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateRemarkForCancellationWO(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateWorkOrderForApproval(IValueObject valueObject, clsUserVO UserVo);
    }
}

