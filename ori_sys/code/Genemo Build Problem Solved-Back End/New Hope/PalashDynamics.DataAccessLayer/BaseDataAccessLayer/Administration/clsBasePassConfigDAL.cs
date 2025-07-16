namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePassConfigDAL
    {
        private static clsBasePassConfigDAL _Instance;

        protected clsBasePassConfigDAL()
        {
        }

        public static clsBasePassConfigDAL GetInstance()
        {
            try
            {
                if (_Instance == null)
                {
                    string str = "clsPassConfigDAL";
                    _Instance = (clsBasePassConfigDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _Instance;
        }

        public abstract IValueObject GetPassConfig(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdatePasswordConfig(IValueObject valueObject, clsUserVO UserVo);
    }
}

