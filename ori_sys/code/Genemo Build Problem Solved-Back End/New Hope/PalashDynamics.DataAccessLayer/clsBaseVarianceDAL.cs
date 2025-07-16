namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseVarianceDAL
    {
        private static clsBaseVarianceDAL _instance;

        protected clsBaseVarianceDAL()
        {
        }

        public abstract IValueObject AddVariance(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseVarianceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsVarianceDAL";
                    _instance = (clsBaseVarianceDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

