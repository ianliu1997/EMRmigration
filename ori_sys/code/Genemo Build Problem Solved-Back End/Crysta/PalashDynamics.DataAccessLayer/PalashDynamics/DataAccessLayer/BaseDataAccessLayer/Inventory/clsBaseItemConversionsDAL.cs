namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseItemConversionsDAL
    {
        private static clsBaseItemConversionsDAL _instance;

        protected clsBaseItemConversionsDAL()
        {
        }

        public abstract IValueObject AddUpdateConversionFactorMaster(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteConversionFactorMaster(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseItemConversionsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsItemConversionsDAL";
                    _instance = (clsBaseItemConversionsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemConversionsList(IValueObject valueObject, clsUserVO UserVo);
    }
}

