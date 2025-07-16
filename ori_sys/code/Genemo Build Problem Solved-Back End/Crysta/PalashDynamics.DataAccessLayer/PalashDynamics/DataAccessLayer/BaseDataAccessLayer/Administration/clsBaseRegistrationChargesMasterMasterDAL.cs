namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseRegistrationChargesMasterMasterDAL
    {
        private static clsBaseRegistrationChargesMasterMasterDAL _instance;

        protected clsBaseRegistrationChargesMasterMasterDAL()
        {
        }

        public abstract IValueObject AddRegistartionChargesMaster(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseRegistrationChargesMasterMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsRegistrationChargesMasterDAL";
                    _instance = (clsBaseRegistrationChargesMasterMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetRegistrationChargesByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetRegistrationChargesByPatientTypeID(IValueObject valueObject, clsUserVO objUserVO);
    }
}

