namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ClientDetails;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.ClientDetails;
    using System;
    using System.Data.Common;

    public class clsClientDetailsDAL : clsBaseClientDetailsDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsClientDetailsDAL()
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
            }
        }

        public override IValueObject GetClient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetClientBizActionVO nvo = valueObject as clsGetClientBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetClientDetails");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    clsClientDetailsVO svo = new clsClientDetailsVO();
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            nvo.Details = svo;
                            break;
                        }
                        svo.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        svo.Date = new DateTime?(nullable.Value);
                        svo.Name = (string) DALHelper.HandleDBNull(reader["Name"]);
                        svo.CustomerCode = (string) DALHelper.HandleDBNull(reader["CustomerCode"]);
                        svo.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

