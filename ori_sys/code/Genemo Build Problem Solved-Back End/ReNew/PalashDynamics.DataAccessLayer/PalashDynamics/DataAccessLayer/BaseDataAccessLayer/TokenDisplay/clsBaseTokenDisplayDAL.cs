namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.TokenDisplay
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseTokenDisplayDAL
    {
        private static clsBaseTokenDisplayDAL _instance;

        protected clsBaseTokenDisplayDAL()
        {
        }

        public abstract IValueObject AddUpdateTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseTokenDisplayDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "TokenDisplay.clsTokenDisplayDAL";
                    _instance = (clsBaseTokenDisplayDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPatientTokenDisplayDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTokenDisplayDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusTokenDisplay(IValueObject valueObject, clsUserVO UserVo);
    }
}

