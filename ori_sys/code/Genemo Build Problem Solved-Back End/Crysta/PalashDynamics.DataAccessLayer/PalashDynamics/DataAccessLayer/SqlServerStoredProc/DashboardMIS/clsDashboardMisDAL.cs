namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.DashboardMIS
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.DashboardMIS;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashboardMIS;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsDashboardMisDAL : clsBaseDashboardMisDAL
    {
        private Database dbServer;
        private LogManager logManager;

        public clsDashboardMisDAL()
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

        public override IValueObject GetReferralReport(IValueObject valueObject)
        {
            cls_DashboardMIS_ReferralReportBizActionVO nvo = valueObject as cls_DashboardMIS_ReferralReportBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("rpt_DashBoard_DoctorReferralByBD");
                this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, nvo.Details.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ReferralReportList == null)
                    {
                        nvo.ReferralReportList = new List<cls_DashboardMIS_ReferralReportListVO>();
                    }
                    while (reader.Read())
                    {
                        cls_DashboardMIS_ReferralReportListVO item = new cls_DashboardMIS_ReferralReportListVO {
                            SrNo = (long) DALHelper.HandleDBNull(reader["SrNo"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            Date = (string) DALHelper.HandleDBNull(reader["Date"]),
                            BDName = Convert.ToString(DALHelper.HandleDBNull(reader["BDName"])),
                            DocName = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"])),
                            RefPaitentCount = Convert.ToInt64(reader["RefPaitentCount"])
                        };
                        nvo.ReferralReportList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

