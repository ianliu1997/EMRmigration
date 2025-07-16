namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseHSGDAL
    {
        private static clsBaseHSGDAL _instance;

        protected clsBaseHSGDAL()
        {
        }

        public abstract IValueObject AddUpdateHSG(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetHSGDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseHSGDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Patient.clsHSGDAL";
                    _instance = (clsBaseHSGDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

