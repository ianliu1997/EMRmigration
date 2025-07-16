namespace PalashDynamics.DataAccessLayer
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;
    using System.Data.Common;

    public abstract class clsBaseRefundDAL
    {
        private static clsBaseRefundDAL _instance;

        protected clsBaseRefundDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject ApproveAdvanceRefundRequestDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApproveConcessionRequest(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject ApproveRefundRequest(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject Delete(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject DeleteApprovalRequest(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseRefundDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsRefundDAL";
                    _instance = (clsBaseRefundDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetRefundReceiptList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SendApprovalRequest(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SendApprovalRequestForAdvanceRefundDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject SendApprovalRequestForBill(IValueObject valueObject, clsUserVO UserVo);
    }
}

