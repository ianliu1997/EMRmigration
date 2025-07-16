namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFROSEstimationDAL
    {
        private static clsBaseIVFROSEstimationDAL _instance;

        protected clsBaseIVFROSEstimationDAL()
        {
        }

        public abstract IValueObject AddUpdateROSEstimation(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFROSEstimationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFROSEstimationDAL";
                    _instance = (clsBaseIVFROSEstimationDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetROSEstimationList(IValueObject valueObject, clsUserVO UserVo);
    }
}

