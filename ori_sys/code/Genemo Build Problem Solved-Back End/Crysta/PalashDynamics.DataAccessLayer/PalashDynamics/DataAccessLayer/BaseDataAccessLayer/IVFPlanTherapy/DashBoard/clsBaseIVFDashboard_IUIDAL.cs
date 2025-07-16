namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboard_IUIDAL
    {
        private static clsBaseIVFDashboard_IUIDAL _instance;

        protected clsBaseIVFDashboard_IUIDAL()
        {
        }

        public abstract IValueObject AddUpdateIUIDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboard_IUIDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboard_IUIDAL";
                    _instance = (clsBaseIVFDashboard_IUIDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIUIDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

