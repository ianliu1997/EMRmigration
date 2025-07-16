namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RateContract
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseRateContractDAL
    {
        private static clsBaseRateContractDAL _instance;

        protected clsBaseRateContractDAL()
        {
        }

        public static clsBaseRateContractDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "RateContract.clsRateContractDAL";
                    _instance = (clsBaseRateContractDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetRateContractAgainstSupplierAndItem(IValueObject valueObject, clsUserVO UserVo);
    }
}

