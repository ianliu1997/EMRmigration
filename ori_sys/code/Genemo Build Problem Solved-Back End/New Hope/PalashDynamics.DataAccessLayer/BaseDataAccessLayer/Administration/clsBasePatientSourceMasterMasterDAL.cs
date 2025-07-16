namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePatientSourceMasterMasterDAL
    {
        private static clsBasePatientSourceMasterMasterDAL _instance;

        protected clsBasePatientSourceMasterMasterDAL()
        {
        }

        public abstract IValueObject AddPatientSourceMaster(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBasePatientSourceMasterMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPatientSourceMasterDAL";
                    _instance = (clsBasePatientSourceMasterMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemGroupMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientSourceByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientSourceList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetPatientSourceListByPatientCategoryId(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetTariffListForCompMaster(IValueObject valueObject, clsUserVO objUserVO);
    }
}

