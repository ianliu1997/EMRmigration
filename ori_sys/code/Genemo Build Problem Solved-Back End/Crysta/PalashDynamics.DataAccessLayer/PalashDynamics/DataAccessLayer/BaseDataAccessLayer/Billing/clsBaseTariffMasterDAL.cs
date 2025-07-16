namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseTariffMasterDAL
    {
        private static clsBaseTariffMasterDAL _instance;

        protected clsBaseTariffMasterDAL()
        {
        }

        public abstract IValueObject AddTariff(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseTariffMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsTariffMasterDAL";
                    _instance = (clsBaseTariffMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetServiceByTariffID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetServicesforIssue(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSpecializationsByTariffId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTariffList(IValueObject valueObject, clsUserVO UserVo);
    }
}

