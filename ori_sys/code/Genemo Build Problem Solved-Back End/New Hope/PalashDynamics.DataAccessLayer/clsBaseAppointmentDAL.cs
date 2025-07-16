namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseAppointmentDAL
    {
        private static clsBaseAppointmentDAL _instance;

        protected clsBaseAppointmentDAL()
        {
        }

        public abstract IValueObject AddAppointment(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddCancelAppReason(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddMarkVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CancelAppointment(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckAppointmentPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckAppointmentTime(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckMarkVisit(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckMRNO(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAppointmentByDoctorAndAppointmentDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAppointmentBYId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAppointmentBYMrNo(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetAppointmentDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDoctorTime(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFutureAppointment(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseAppointmentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsAppointmentDAL";
                    _instance = (clsBaseAppointmentDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPastAppointment(IValueObject valueObject, clsUserVO UserVo);
    }
}

