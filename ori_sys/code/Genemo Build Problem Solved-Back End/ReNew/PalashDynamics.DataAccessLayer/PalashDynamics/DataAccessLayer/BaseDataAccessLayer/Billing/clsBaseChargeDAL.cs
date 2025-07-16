namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseChargeDAL
    {
        private static clsBaseChargeDAL _instance;

        protected clsBaseChargeDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction, long iParentID, long iCDParentID);
        public abstract IValueObject AddRefundServices(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetChargeListAgainstBills(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetChargeListForApprovalRequestWindow(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetChargeTaxDetails(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseChargeDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsChargeDAL";
                    _instance = (clsBaseChargeDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
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

