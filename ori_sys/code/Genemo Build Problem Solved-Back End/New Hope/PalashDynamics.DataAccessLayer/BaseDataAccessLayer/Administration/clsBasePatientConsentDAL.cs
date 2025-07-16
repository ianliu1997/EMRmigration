namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePatientConsentDAL
    {
        private static clsBasePatientConsentDAL _instance;

        protected clsBasePatientConsentDAL()
        {
        }

        public abstract IValueObject AddPatientConsent(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePatientConsentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPatientConsentDAL";
                    _instance = (clsBasePatientConsentDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIVFPackegeConsentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFSavedPackegeConsentList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientConsent(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientConsentList(IValueObject valueObject, clsUserVO UserVo);
    }
}

