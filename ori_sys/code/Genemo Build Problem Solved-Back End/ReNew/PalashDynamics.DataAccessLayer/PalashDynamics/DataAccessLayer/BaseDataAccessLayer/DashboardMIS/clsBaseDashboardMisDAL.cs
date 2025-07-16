namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.DashboardMIS
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDashboardMisDAL
    {
        private static clsBaseDashboardMisDAL _instance;

        protected clsBaseDashboardMisDAL()
        {
        }

        public static clsBaseDashboardMisDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "DashboardMIS.clsDashboardMisDAL";
                    _instance = (clsBaseDashboardMisDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetReferralReport(IValueObject valueObject);
    }
}

