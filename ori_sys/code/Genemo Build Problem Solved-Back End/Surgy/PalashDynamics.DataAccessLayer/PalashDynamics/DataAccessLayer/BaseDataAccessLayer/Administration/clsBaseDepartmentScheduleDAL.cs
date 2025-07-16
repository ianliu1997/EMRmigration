namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDepartmentScheduleDAL
    {
        private static clsBaseDepartmentScheduleDAL _instance;

        protected clsBaseDepartmentScheduleDAL()
        {
        }

        public abstract IValueObject AddDepartmentScheduleMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentScheduleList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseDepartmentScheduleDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsDepartmentScheduleDAL";
                    _instance = (clsBaseDepartmentScheduleDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

