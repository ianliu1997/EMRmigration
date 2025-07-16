namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseExpiryItemDAL
    {
        private static clsBaseExpiryItemDAL _instance;

        protected clsBaseExpiryItemDAL()
        {
        }

        public abstract IValueObject AddExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApproveExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetExpiredReturnDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetExpiryItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetExpiryItemReturnForDashBoard(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetExpiryList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetExpiryListForExpiredReturn(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseExpiryItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsExpiryItemDAL";
                    _instance = (clsBaseExpiryItemDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
    }
}

