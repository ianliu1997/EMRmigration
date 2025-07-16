namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsPatientReminderLogDAL
    {
        private static clsPatientReminderLogDAL _instance;

        protected clsPatientReminderLogDAL()
        {
        }

        public abstract IValueObject AddIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo);
        public static clsPatientReminderLogDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsBedReservationDAL";
                    _instance = (clsPatientReminderLogDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

