namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class cls_NewBaseSpremFreezingDAL
    {
        private static cls_NewBaseSpremFreezingDAL _instance;

        protected cls_NewBaseSpremFreezingDAL()
        {
        }

        public abstract IValueObject AddUpdateSpremFrezing(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO);
        public static cls_NewBaseSpremFreezingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.cls_NewSpremFreezingDAL";
                    _instance = (cls_NewBaseSpremFreezingDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetSpremFreezingList(IValueObject valuObject, clsUserVO UserVO);
        public abstract IValueObject GetSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO);
    }
}

