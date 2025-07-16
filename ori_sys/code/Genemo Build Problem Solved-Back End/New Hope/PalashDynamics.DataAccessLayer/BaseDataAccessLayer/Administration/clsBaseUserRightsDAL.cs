namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseUserRightsDAL
    {
        private static clsBaseUserRightsDAL _instance;

        protected clsBaseUserRightsDAL()
        {
        }

        public abstract IValueObject AddCreditLimit(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseUserRightsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Administration.clsUserRightsDAL";
                    _instance = (clsBaseUserRightsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetUserRights(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GRNCountWithRightsAndFrequency(IValueObject valueObject, clsUserVO objUserVO);
    }
}

