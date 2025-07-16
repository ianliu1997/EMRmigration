namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseCampServiceDAL
    {
        private static clsBaseCampServiceDAL _instance;

        protected clsBaseCampServiceDAL()
        {
        }

        public abstract IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseCampServiceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsCampServiceDAL";
                    _instance = (clsBaseCampServiceDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

