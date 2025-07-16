namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboard_SemenDAL
    {
        private static clsBaseIVFDashboard_SemenDAL _instance;

        protected clsBaseIVFDashboard_SemenDAL()
        {
        }

        public abstract IValueObject AddUpdateSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateSemenWashDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboard_SemenDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboard_SemenDAL";
                    _instance = (clsBaseIVFDashboard_SemenDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetNewIUIDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenWashDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

