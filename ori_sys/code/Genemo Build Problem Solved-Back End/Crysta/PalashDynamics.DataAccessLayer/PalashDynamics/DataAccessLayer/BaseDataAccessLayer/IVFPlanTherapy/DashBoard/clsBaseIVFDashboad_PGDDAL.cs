namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboad_PGDDAL
    {
        private static clsBaseIVFDashboad_PGDDAL _instance;

        protected clsBaseIVFDashboad_PGDDAL()
        {
        }

        public abstract IValueObject AddUpdatePGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdatePGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboad_PGDDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboad_PGDDAL";
                    _instance = (clsBaseIVFDashboad_PGDDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetPGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

