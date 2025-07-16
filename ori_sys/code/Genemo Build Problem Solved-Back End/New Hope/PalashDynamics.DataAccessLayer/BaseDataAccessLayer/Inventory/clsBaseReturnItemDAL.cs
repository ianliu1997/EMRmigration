namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseReturnItemDAL
    {
        private static clsBaseReturnItemDAL _instance;

        protected clsBaseReturnItemDAL()
        {
        }

        public abstract IValueObject AddReturnItemToStore(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseReturnItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsReturnItemDAL";
                    _instance = (clsBaseReturnItemDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemListByReturnId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetReturnList(IValueObject valueObject, clsUserVO UserVo);
    }
}

