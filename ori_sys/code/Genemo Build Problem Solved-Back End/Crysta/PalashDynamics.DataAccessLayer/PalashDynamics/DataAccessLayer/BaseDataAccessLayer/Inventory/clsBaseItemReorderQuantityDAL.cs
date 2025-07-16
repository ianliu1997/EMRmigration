namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseItemReorderQuantityDAL
    {
        private static clsBaseItemReorderQuantityDAL _instance;

        protected clsBaseItemReorderQuantityDAL()
        {
        }

        public static clsBaseItemReorderQuantityDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Inventory.clsItemReorderQuantityDAL";
                    _instance = (clsBaseItemReorderQuantityDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemReorderQuantity(IValueObject valueObject, clsUserVO UserVo);
    }
}

