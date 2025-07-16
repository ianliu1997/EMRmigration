namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseMenuDAL
    {
        private static clsBaseMenuDAL _instance;

        protected clsBaseMenuDAL()
        {
        }

        public static clsBaseMenuDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsMenuDAL";
                    _instance = (clsBaseMenuDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMenuGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMenuList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUserMenu(IValueObject valueObject, clsUserVO UserVo);
    }
}

