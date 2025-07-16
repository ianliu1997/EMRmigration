namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.RSIJ
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseRSIJMasterDAL
    {
        private static clsBaseRSIJMasterDAL _instance;

        protected clsBaseRSIJMasterDAL()
        {
        }

        public abstract IValueObject GetDiagnosisList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorDepartmentDetails(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDoctorScheduleTime(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseRSIJMasterDAL GetInstance()
        {
            try
            {
                string str = "RSIJ.clsRSIJMasterDAL";
                _instance = (clsBaseRSIJMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMasterList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetMasterListByTableName(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetOPDQueueList(IValueObject valueObject, clsUserVO objUserVO);
    }
}

