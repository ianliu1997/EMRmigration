namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class cls_BaseIVFDashboard_SurrogateDAL
    {
        private static cls_BaseIVFDashboard_SurrogateDAL _instance;

        protected cls_BaseIVFDashboard_SurrogateDAL()
        {
        }

        public abstract IValueObject GetAgencyListOfSurrogate(IValueObject valueObject, clsUserVO UserVo);
        public static cls_BaseIVFDashboard_SurrogateDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.cls_IVFDashboard_SurrogateDAL";
                    _instance = (cls_BaseIVFDashboard_SurrogateDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

