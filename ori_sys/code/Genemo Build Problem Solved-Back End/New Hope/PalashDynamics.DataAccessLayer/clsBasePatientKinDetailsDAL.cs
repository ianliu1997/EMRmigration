namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePatientKinDetailsDAL
    {
        private static clsBasePatientKinDetailsDAL _instance;

        protected clsBasePatientKinDetailsDAL()
        {
        }

        public abstract IValueObject AddPatientKinDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePatientKinDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPatientKinDetailsDAL";
                    _instance = (clsBasePatientKinDetailsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

