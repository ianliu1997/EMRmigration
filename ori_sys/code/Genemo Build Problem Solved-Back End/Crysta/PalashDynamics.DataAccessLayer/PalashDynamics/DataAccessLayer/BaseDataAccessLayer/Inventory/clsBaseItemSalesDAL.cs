namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseItemSalesDAL
    {
        private static clsBaseItemSalesDAL _instance;

        protected clsBaseItemSalesDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddItemSaleReturn(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseItemSalesDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsItemSalesDAL";
                    _instance = (clsBaseItemSalesDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetItemSale(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleComplete(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleDetailsForCashSettlement(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemSaleReturnDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}

