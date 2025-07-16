namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseDischargeTemplateMasterDAL
    {
        private static clsBaseDischargeTemplateMasterDAL _instance;

        protected clsBaseDischargeTemplateMasterDAL()
        {
        }

        public abstract IValueObject AddUpdateDischargeTemplateMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject CheckCountDischargeSummaryByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDischargeTemplateMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseDischargeTemplateMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Administration.clsDischargeTemplateMasterDAL";
                    _instance = (clsBaseDischargeTemplateMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

