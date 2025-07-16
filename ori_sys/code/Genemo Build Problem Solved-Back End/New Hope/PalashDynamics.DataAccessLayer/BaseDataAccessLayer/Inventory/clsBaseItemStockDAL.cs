namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseItemStockDAL
    {
        private static clsBaseItemStockDAL _instance;

        protected clsBaseItemStockDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject AddFree(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        public abstract IValueObject GetAvailableStockList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGRNItemBatchwiseStockForQS(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseItemStockDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsItemStockDAL";
                    _instance = (clsBaseItemStockDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemBatchwiseStock(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemBatchwiseStockFree(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemCurrentStockList(IValueObject valueObject, clsUserVO UserVo);
    }
}

