namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePatientVisitDAL
    {
        private static clsBasePatientVisitDAL _instance;

        protected clsBasePatientVisitDAL()
        {
        }

        public static clsBasePatientVisitDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPatientVisitDAL";
                    _instance = (clsBasePatientVisitDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientVisitDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

