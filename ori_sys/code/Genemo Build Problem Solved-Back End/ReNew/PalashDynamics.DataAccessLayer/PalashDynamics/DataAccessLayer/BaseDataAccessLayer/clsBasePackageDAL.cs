namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePackageDAL
    {
        private static clsBasePackageDAL _instance;

        protected clsBasePackageDAL()
        {
        }

        public static clsBasePackageDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPackageDAL";
                    _instance = (clsBasePackageDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPackageList(IValueObject valueObject);
    }
}

