namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseQueueDAL
    {
        private static clsBaseQueueDAL _instance;

        protected clsBaseQueueDAL()
        {
        }

        public abstract IValueObject AddQueueList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseQueueDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsQueueDAL";
                    _instance = (clsBaseQueueDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetQueueByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetQueueList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateDoctorInQueue(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateQueueSortOrder(IValueObject valueObject, clsUserVO UserVo);
    }
}

