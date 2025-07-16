namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseUnitMasterDAL
    {
        private static clsBaseUnitMasterDAL _instance;

        protected clsBaseUnitMasterDAL()
        {
        }

        public abstract IValueObject AddUnitMaster(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetDepartmentList(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseUnitMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsUnitMasterDAL";
                    _instance = (clsBaseUnitMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetUnitDetailsByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUnitList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUserWiseUnitList(IValueObject valueObject, clsUserVO objUserVO);
    }
}

