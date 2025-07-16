namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseTESEDAL
    {
        private static clsBaseTESEDAL _instance;

        protected clsBaseTESEDAL()
        {
        }

        public abstract IValueObject AddUpdateTESE(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseTESEDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsTESE_DAL";
                    _instance = (clsBaseTESEDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetTESEDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

