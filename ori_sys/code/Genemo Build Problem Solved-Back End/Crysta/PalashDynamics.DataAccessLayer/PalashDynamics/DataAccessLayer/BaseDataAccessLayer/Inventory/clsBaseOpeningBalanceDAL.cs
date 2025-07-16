namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseOpeningBalanceDAL
    {
        private static clsBaseOpeningBalanceDAL _instance;

        protected clsBaseOpeningBalanceDAL()
        {
        }

        public abstract IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseOpeningBalanceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Inventory.clsOpeningBalanceDAL";
                    _instance = (clsBaseOpeningBalanceDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetStockDetailsForOpeningBalance(IValueObject valueObject, clsUserVO UserVo);
    }
}

