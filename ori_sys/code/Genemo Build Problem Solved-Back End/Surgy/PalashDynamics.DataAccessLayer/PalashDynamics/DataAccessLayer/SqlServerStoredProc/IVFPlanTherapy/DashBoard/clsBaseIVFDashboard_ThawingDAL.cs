namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboard_ThawingDAL
    {
        private static clsBaseIVFDashboard_ThawingDAL _instance;

        protected clsBaseIVFDashboard_ThawingDAL()
        {
        }

        public abstract IValueObject AddUpdateIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_ThawingOocyte(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_ThawingWOCryo(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboard_ThawingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboard_ThawingDAL";
                    _instance = (clsBaseIVFDashboard_ThawingDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo);
    }
}

