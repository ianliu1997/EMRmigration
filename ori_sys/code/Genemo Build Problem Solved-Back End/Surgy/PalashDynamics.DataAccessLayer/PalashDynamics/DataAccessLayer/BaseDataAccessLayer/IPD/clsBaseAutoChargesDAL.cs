namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseAutoChargesDAL
    {
        private static clsBaseAutoChargesDAL _instance;

        protected clsBaseAutoChargesDAL()
        {
        }

        public abstract IValueObject AddAutoChargesServiceList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteService(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAutoChargesServiceList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseAutoChargesDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsAutoChargesDAL";
                    _instance = (clsBaseAutoChargesDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }
    }
}

