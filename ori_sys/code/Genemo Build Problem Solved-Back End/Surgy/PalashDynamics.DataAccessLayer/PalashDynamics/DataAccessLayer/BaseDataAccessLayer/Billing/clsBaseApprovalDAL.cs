namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.DataAccessLayer.SqlServerStoredProc.Billing;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseApprovalDAL
    {
        private static clsBaseApprovalDAL _instance;

        protected clsBaseApprovalDAL()
        {
        }

        public abstract IValueObject GetCancellationAndConcessionApprovalRequest(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseApprovalDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "Billing.clsApprovalDAL";
                    _instance = (clsApprovalDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }
    }
}

