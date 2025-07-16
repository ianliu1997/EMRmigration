namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseStaffMasterDAL
    {
        private static clsBaseStaffMasterDAL _instance;

        protected clsBaseStaffMasterDAL()
        {
        }

        public abstract IValueObject AddStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddStaffMaster(IValueObject valueObject, clsUserVO objUserVO);
        public static clsBaseStaffMasterDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsStaffMasterDAL";
                    _instance = (clsBaseStaffMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffAddressInfoById(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffBankInfoById(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffByUnitID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffByUnitIDandID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffMasterByID(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetStaffMasterList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject GetUserSearchList(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStaffAddressInfo(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateStaffBankInfo(IValueObject valueObject, clsUserVO objUserVO);
    }
}

