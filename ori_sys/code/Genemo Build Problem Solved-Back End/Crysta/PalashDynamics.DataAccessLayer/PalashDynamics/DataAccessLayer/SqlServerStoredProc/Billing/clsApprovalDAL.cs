namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Billing
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.ValueObjects;
    using System;

    public class clsApprovalDAL : clsBaseApprovalDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsApprovalDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject GetCancellationAndConcessionApprovalRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }
    }
}

