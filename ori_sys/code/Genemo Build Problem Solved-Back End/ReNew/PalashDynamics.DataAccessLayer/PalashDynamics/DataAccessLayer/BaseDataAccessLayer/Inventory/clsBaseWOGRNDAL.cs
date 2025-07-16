namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseWOGRNDAL
    {
        private static clsBaseWOGRNDAL _instance;

        protected clsBaseWOGRNDAL()
        {
        }

        public static clsBaseWOGRNDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsWOGRNDAL";
                    _instance = (clsBaseWOGRNDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject WOAdd(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject WODeleteGRNItems(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject WOGetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject WOGetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject WOGetItemDetailsList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject WOGetSearchList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject WOUpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo);
    }
}

