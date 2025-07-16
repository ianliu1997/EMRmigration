namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseAppConfigDAL
    {
        private static clsBaseAppConfigDAL _instance;

        protected clsBaseAppConfigDAL()
        {
        }

        public abstract IValueObject AddEmailIDCCTo(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAppConfig(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAutoEmailCCTOConfig(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseAppConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsAppConfigDAL";
                    _instance = (clsBaseAppConfigDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject SetStatusAutoEmailCCTO(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateAppConfig(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateAutoEmailConfig(IValueObject valueObject, clsUserVO UserVo);
    }
}

