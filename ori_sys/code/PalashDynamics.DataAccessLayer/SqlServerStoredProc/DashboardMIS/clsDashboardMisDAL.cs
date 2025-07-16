using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.DashboardMIS;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects.DashboardMIS;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.DashboardMIS
{
    public class clsDashboardMisDAL : clsBaseDashboardMisDAL
    {
         #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        public clsDashboardMisDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject GetReferralReport(IValueObject valueObject)
        {
            cls_DashboardMIS_ReferralReportBizActionVO BizActionObj = valueObject as cls_DashboardMIS_ReferralReportBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("rpt_DashBoard_DoctorReferralByBD");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ClinicID", DbType.Int64, BizActionObj.Details.UnitID);
                //dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ReferralReportList == null)
                        BizActionObj.ReferralReportList = new List<cls_DashboardMIS_ReferralReportListVO>();
                    while (reader.Read())
                    {
                        cls_DashboardMIS_ReferralReportListVO objVO = new cls_DashboardMIS_ReferralReportListVO();

                        objVO.SrNo = (long)DALHelper.HandleDBNull(reader["SrNo"]);
                        objVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objVO.Date = (string)DALHelper.HandleDBNull(reader["Date"]);
                        objVO.BDName = Convert.ToString(DALHelper.HandleDBNull(reader["BDName"]));
                        objVO.DocName = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"]));
                        //objVO.InternalDoctorRefCount = Convert.ToInt64(reader["InternalDoctorRefCount"]);
                        //objVO.ExternalDoctorRefCount = Convert.ToInt64(reader["ExternalDoctorRefCount"]);
                        objVO.RefPaitentCount = Convert.ToInt64(reader["RefPaitentCount"]);
                        BizActionObj.ReferralReportList.Add(objVO);

                    }

                }
                reader.NextResult();


                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }
    }
}
