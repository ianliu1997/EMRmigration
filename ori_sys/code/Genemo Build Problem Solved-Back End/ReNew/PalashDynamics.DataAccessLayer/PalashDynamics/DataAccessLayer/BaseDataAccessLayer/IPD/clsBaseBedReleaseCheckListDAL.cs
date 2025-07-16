namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseBedReleaseCheckListDAL
    {
        private static clsBaseBedReleaseCheckListDAL _instance;

        protected clsBaseBedReleaseCheckListDAL()
        {
        }

        public abstract IValueObject AddUpdateBedReleaseCheckListDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBedReleaseCheckList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetBedReleaseList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseBedReleaseCheckListDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IPD.clsBedReleaseCheckListDAL";
                    _instance = (clsBaseBedReleaseCheckListDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

