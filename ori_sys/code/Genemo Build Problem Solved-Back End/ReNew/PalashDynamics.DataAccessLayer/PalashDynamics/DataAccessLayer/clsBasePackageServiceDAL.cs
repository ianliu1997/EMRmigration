namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBasePackageServiceDAL
    {
        private static clsBasePackageServiceDAL _instance;

        protected clsBasePackageServiceDAL()
        {
        }

        public abstract IValueObject AddPackageServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAllPackageServices(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePackageServiceDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPackageServiceDAL";
                    _instance = (clsBasePackageServiceDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPackageServiceDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageServiceDetailListbyServiceID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageServiceForBill(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPackageServiceList(IValueObject valueObject, clsUserVO UserVo);
    }
}

