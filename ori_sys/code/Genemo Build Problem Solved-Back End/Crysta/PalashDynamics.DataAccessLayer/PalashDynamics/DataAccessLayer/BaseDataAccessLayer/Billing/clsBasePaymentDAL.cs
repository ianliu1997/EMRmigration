namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBasePaymentDAL
    {
        private static clsBasePaymentDAL _instance;

        protected clsBasePaymentDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject AddWithTransaction(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CompanySettlement(IValueObject valueObject, clsUserVO UserVo);
        public static clsBasePaymentDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsPaymentDAL";
                    _instance = (clsBasePaymentDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetList(IValueObject valueObject, clsUserVO UserVo);
    }
}

