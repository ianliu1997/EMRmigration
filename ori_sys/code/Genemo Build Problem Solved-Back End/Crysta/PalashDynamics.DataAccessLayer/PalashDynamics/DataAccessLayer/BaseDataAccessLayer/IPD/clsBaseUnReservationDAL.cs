namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseUnReservationDAL
    {
        private static clsBaseUnReservationDAL _instance;

        protected clsBaseUnReservationDAL()
        {
        }

        public abstract IValueObject AddIPDBedUnReservation(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseUnReservationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsBedUnReservationDAL";
                    _instance = (clsBaseUnReservationDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

