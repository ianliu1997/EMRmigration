namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFDashboad_OPUDAL
    {
        private static clsBaseIVFDashboad_OPUDAL _instance;

        protected clsBaseIVFDashboad_OPUDAL()
        {
        }

        public abstract IValueObject AddUpdateOocyteNumber(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateOPUDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetEmbryologySummary(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFDashboad_OPUDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFDashboad_OPUDAL";
                    _instance = (clsBaseIVFDashboad_OPUDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetOPUDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

