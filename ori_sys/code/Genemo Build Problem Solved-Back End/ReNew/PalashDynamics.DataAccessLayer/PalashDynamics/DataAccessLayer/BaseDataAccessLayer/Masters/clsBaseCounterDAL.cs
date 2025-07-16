namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseCounterDAL
    {
        private static clsBaseCounterDAL _instance;

        protected clsBaseCounterDAL()
        {
        }

        public abstract IValueObject GetCounterListByUnitId(IValueObject valueObject, clsUserVO UserVO);
        public static clsBaseCounterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsCounterDAL";
                    _instance = (clsBaseCounterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

