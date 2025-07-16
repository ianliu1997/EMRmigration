namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIPDDAL
    {
        private static clsBaseIPDDAL _instance;

        protected clsBaseIPDDAL()
        {
        }

        public abstract IValueObject AddIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddIPDDischarge(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddMultipleBed(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetDischargeStatusDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIPDDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsIPDDAL";
                    _instance = (clsBaseIPDDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIPDBedTransfer(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDDischargeDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDPatientDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

