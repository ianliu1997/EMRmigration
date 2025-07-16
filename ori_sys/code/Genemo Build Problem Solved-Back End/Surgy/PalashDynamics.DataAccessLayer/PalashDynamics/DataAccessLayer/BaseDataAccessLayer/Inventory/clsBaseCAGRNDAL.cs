namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseCAGRNDAL
    {
        private static clsBaseCAGRNDAL _instance;

        protected clsBaseCAGRNDAL()
        {
        }

        public abstract IValueObject AddCAGRNItem(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteCAItemById(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAGRNItemDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAGRNSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAItemDetailList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAItemDetailListById(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetCAList(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseCAGRNDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Inventory.clsCAGRNDAL";
                    _instance = (clsBaseCAGRNDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject UpdateCAGRNItem(IValueObject valueObject, clsUserVO UserVo);
    }
}

