namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ClientDetails
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseClientDetailsDAL
    {
        private static clsBaseClientDetailsDAL _instance;

        protected clsBaseClientDetailsDAL()
        {
        }

        public abstract IValueObject GetClient(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseClientDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsClientDetailsDAL";
                    _instance = (clsBaseClientDetailsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }
    }
}

