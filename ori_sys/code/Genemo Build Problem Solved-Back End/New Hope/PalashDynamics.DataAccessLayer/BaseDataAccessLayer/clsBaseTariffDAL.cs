namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseTariffDAL
    {
        private static clsBaseTariffDAL _instance;

        protected clsBaseTariffDAL()
        {
        }

        public abstract IValueObject GetCompanyList(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseTariffDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsTariffDAL";
                    _instance = (clsBaseTariffDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientCategoryList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO);
    }
}

