namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIPDVitalSDetailsDAL
    {
        private static clsBaseIPDVitalSDetailsDAL _instance;

        protected clsBaseIPDVitalSDetailsDAL()
        {
        }

        public abstract IValueObject AddVitalSDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetGraphDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIPDVitalSDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsIPDVitalSDetailsDAL";
                    _instance = (clsBaseIPDVitalSDetailsDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetListofVitalDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetTPRDetailsListByAdmIDAdmUnitID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUnitWiseEmpDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitalsDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateStatusVitalDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

