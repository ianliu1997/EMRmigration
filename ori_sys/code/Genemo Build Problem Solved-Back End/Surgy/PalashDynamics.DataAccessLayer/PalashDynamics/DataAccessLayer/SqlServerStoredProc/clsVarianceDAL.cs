namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.EMR;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsVarianceDAL : clsBaseVarianceDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsVarianceDAL()
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

        public override IValueObject AddVariance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddVarianceBizActionVO nvo = valueObject as clsAddVarianceBizActionVO;
            try
            {
                clsVarianceVO varianceDetails = nvo.VarianceDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddVariance");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, varianceDetails.LinkServer);
                if (varianceDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, varianceDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, varianceDetails.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, varianceDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, varianceDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, varianceDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, varianceDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, varianceDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "Variance1", DbType.String, varianceDetails.Variance1);
                this.dbServer.AddInParameter(storedProcCommand, "Variance2", DbType.String, varianceDetails.Variance2);
                this.dbServer.AddInParameter(storedProcCommand, "Variance3", DbType.String, varianceDetails.Variance3);
                this.dbServer.AddInParameter(storedProcCommand, "Variance4", DbType.String, varianceDetails.Variance4);
                this.dbServer.AddInParameter(storedProcCommand, "Variance5", DbType.String, varianceDetails.Variance5);
                this.dbServer.AddInParameter(storedProcCommand, "Variance6", DbType.String, varianceDetails.Variance6);
                this.dbServer.AddInParameter(storedProcCommand, "Variance7", DbType.String, varianceDetails.Variance7);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance1", DbType.String, varianceDetails.ListVariance1);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance2", DbType.String, varianceDetails.ListVariance2);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance3", DbType.String, varianceDetails.ListVariance3);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance4", DbType.String, varianceDetails.ListVariance4);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance5", DbType.String, varianceDetails.ListVariance5);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance6", DbType.String, varianceDetails.ListVariance6);
                this.dbServer.AddInParameter(storedProcCommand, "ListVariance7", DbType.String, varianceDetails.ListVariance7);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, varianceDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, varianceDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, varianceDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.VarianceDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

