namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseMISConfigDAL
    {
        private static clsBaseMISConfigDAL _instance;

        protected clsBaseMISConfigDAL()
        {
        }

        public abstract IValueObject AddMISConfig(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject AddUpdateMISConfig(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetAutoEmailForMIS(IValueObject valueObject, clsUserVO UserVO);
        public static clsBaseMISConfigDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsMISConfigDAL";
                    _instance = (clsBaseMISConfigDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMISConfig(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMISConfigList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMISDetailsFromCriteria(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMISReportDetails(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMISReportType(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetStaff(IValueObject valueObject, clsUserVO UserVO);
    }
}

