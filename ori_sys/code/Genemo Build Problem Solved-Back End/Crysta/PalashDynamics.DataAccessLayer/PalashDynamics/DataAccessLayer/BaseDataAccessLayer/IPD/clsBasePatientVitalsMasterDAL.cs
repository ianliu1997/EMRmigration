namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePatientVitalsMasterDAL
    {
        private static clsBasePatientVitalsMasterDAL _instance;

        protected clsBasePatientVitalsMasterDAL()
        {
        }

        public abstract IValueObject AddUpdatePatientVitalMaster(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBasePatientVitalsMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsPatientVitalsMasterDAL";
                    _instance = (clsBasePatientVitalsMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientVitalMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStatusPatientVitalMaster(IValueObject valueObject, clsUserVO objUserVO);
    }
}

