namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseReservationDAL
    {
        private static clsBaseReservationDAL _instance;

        protected clsBaseReservationDAL()
        {
        }

        public abstract IValueObject AddIPDBedReservation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseReservationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsBedReservationDAL";
                    _instance = (clsBaseReservationDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIPDBedReservationList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDBedReservationStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo);
    }
}

