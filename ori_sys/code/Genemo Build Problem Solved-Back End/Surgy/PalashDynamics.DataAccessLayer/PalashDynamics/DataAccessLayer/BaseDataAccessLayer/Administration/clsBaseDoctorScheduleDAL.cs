namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDoctorScheduleDAL
    {
        private static clsBaseDoctorScheduleDAL _instance;

        protected clsBaseDoctorScheduleDAL()
        {
        }

        public abstract IValueObject AddDoctorScheduleMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddDoctorScheduleMasterNew(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject CheckScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorDepartmentUnitList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorSchedule(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleDetailsList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleDetailsListByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleDetailsListNew(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleListNew(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleWise(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseDoctorScheduleDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsDoctorScheduleDAL";
                    _instance = (clsBaseDoctorScheduleDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetVisitTypeDetails(IValueObject valueObject, clsUserVO objUserVO);
    }
}

