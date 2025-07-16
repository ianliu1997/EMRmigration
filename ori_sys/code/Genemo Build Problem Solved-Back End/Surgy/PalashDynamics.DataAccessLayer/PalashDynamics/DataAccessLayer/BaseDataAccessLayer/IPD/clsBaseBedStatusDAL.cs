namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseBedStatusDAL
    {
        private static clsBaseBedStatusDAL _instance;

        protected clsBaseBedStatusDAL()
        {
        }

        public static clsBaseBedStatusDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsBedStatusDAL";
                    _instance = (clsBaseBedStatusDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetWardByFloor(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ViewBedStatus(IValueObject valueObject, clsUserVO UserVo);
    }
}

