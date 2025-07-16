namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class cls_NewBaseSpremThawingDAL
    {
        private static cls_NewBaseSpremThawingDAL _instance;

        protected cls_NewBaseSpremThawingDAL()
        {
        }

        public static cls_NewBaseSpremThawingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.cls_NewSpremThawingDAL";
                    _instance = (cls_NewBaseSpremThawingDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetSpermFrezingDetailsForThawing(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetSpermFrezingDetailsForThawingView(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetSpremFreezingforThawingNew(IValueObject valuObject, clsUserVO UserVO);
        public abstract IValueObject GetTesaPesaForCode(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetThawingDetailsList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetThawingDetailsListForIUI(IValueObject valueObject, clsUserVO UserVO);
    }
}

