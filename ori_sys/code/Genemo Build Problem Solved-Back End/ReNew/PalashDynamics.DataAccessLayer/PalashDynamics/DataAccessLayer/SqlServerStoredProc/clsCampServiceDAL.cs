namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.CRM;
    using System;

    public class clsCampServiceDAL : clsBaseCampServiceDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsCampServiceDAL()
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

        public override IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo)
        {
            return (clsAddCampServiceBizActionVO) valueObject;
        }
    }
}

