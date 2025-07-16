namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using System;
    using System.Data;
    using System.Data.Common;

    internal class clsIVFDashboard_IUIDAL : clsBaseIVFDashboard_IUIDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFDashboard_IUIDAL()
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

        public override IValueObject AddUpdateIUIDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateIUIDetailsBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateIUIDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateIUI");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.IUIDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.IUIDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.IUIDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.IUIDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IUIDate", DbType.DateTime, nvo.IUIDetails.IUIDate);
                this.dbServer.AddInParameter(storedProcCommand, "IUITime", DbType.DateTime, nvo.IUIDetails.IUITime);
                this.dbServer.AddInParameter(storedProcCommand, "InseminatedByID", DbType.Int64, nvo.IUIDetails.InseminatedByID);
                this.dbServer.AddInParameter(storedProcCommand, "WitnessedByID", DbType.Int64, nvo.IUIDetails.WitnessedByID);
                this.dbServer.AddInParameter(storedProcCommand, "InseminationLocationID", DbType.Int64, nvo.IUIDetails.InseminationLocationID);
                this.dbServer.AddInParameter(storedProcCommand, "IsHomologous", DbType.Boolean, nvo.IUIDetails.IsHomologous);
                this.dbServer.AddInParameter(storedProcCommand, "CollectionDate", DbType.DateTime, nvo.IUIDetails.CollectionDate);
                this.dbServer.AddInParameter(storedProcCommand, "PreperationDate", DbType.DateTime, nvo.IUIDetails.PreperationDate);
                this.dbServer.AddInParameter(storedProcCommand, "ThawingDate", DbType.DateTime, nvo.IUIDetails.ThawingDate);
                this.dbServer.AddInParameter(storedProcCommand, "SampleID", DbType.String, nvo.IUIDetails.SampleID);
                this.dbServer.AddInParameter(storedProcCommand, "Purpose", DbType.String, nvo.IUIDetails.Purpose);
                this.dbServer.AddInParameter(storedProcCommand, "Diagnosis", DbType.String, nvo.IUIDetails.Diagnosis);
                this.dbServer.AddInParameter(storedProcCommand, "CollectionMethodID", DbType.Int64, nvo.IUIDetails.CollectionMethodID);
                this.dbServer.AddInParameter(storedProcCommand, "InseminatedAmounts", DbType.Double, nvo.IUIDetails.InseminatedAmounts);
                this.dbServer.AddInParameter(storedProcCommand, "NumberofMotileSperm", DbType.Double, nvo.IUIDetails.NumberofMotileSperm);
                this.dbServer.AddInParameter(storedProcCommand, "NativeAmount", DbType.Double, nvo.IUIDetails.NativeAmount);
                this.dbServer.AddInParameter(storedProcCommand, "AfterPrepAmount", DbType.Double, nvo.IUIDetails.AfterPrepAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NativeConcentration", DbType.Double, nvo.IUIDetails.NativeConcentration);
                this.dbServer.AddInParameter(storedProcCommand, "AfterPrepConcentration", DbType.Double, nvo.IUIDetails.AfterPrepConcentration);
                this.dbServer.AddInParameter(storedProcCommand, "NativeProgressiveMotatity", DbType.Double, nvo.IUIDetails.NativeProgressiveMotatity);
                this.dbServer.AddInParameter(storedProcCommand, "AfterPrepProgressiveMotatity", DbType.Double, nvo.IUIDetails.AfterPrepProgressiveMotatity);
                this.dbServer.AddInParameter(storedProcCommand, "NativeOverallMotality", DbType.Double, nvo.IUIDetails.NativeOverallMotality);
                this.dbServer.AddInParameter(storedProcCommand, "AfterPrepOverallMotality", DbType.Double, nvo.IUIDetails.AfterPrepOverallMotality);
                this.dbServer.AddInParameter(storedProcCommand, "NativeNormalForms", DbType.Double, nvo.IUIDetails.NativeNormalForms);
                this.dbServer.AddInParameter(storedProcCommand, "AfterPrepNormalForms", DbType.Double, nvo.IUIDetails.AfterPrepNormalForms);
                this.dbServer.AddInParameter(storedProcCommand, "NativeTotalNoOfSperms", DbType.Double, nvo.IUIDetails.NativeTotalNoOfSperms);
                this.dbServer.AddInParameter(storedProcCommand, "AfterPrepTotalNoOfSperms", DbType.Double, nvo.IUIDetails.AfterPrepTotalNoOfSperms);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, nvo.IUIDetails.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.IUIDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.IUIDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.IUIDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetIUIDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetIUIDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetIUIDetailsBizActionVO;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetIUIetails");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.IUIDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["IUIDate"])));
                        nvo.Details.IUITime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["IUITime"])));
                        nvo.Details.InseminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InseminatedByID"]));
                        nvo.Details.WitnessedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessedByID"]));
                        nvo.Details.InseminationLocationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InseminationLocationID"]));
                        nvo.Details.IsHomologous = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsHomologous"]));
                        nvo.Details.CollectionDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["CollectionDate"])));
                        nvo.Details.PreperationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["PreperationDate"])));
                        nvo.Details.ThawingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ThawingDate"])));
                        nvo.Details.SampleID = Convert.ToString(DALHelper.HandleDBNull(reader["SampleID"]));
                        nvo.Details.Purpose = Convert.ToString(DALHelper.HandleDBNull(reader["Purpose"]));
                        nvo.Details.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["Diagnosis"]));
                        nvo.Details.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        nvo.Details.InseminatedAmounts = Convert.ToDouble(DALHelper.HandleDBNull(reader["InseminatedAmounts"]));
                        nvo.Details.NumberofMotileSperm = Convert.ToDouble(DALHelper.HandleDBNull(reader["NumberofMotileSperm"]));
                        nvo.Details.NativeAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeAmount"]));
                        nvo.Details.AfterPrepAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepAmount"]));
                        nvo.Details.NativeConcentration = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeConcentration"]));
                        nvo.Details.AfterPrepConcentration = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepConcentration"]));
                        nvo.Details.NativeProgressiveMotatity = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeProgressiveMotatity"]));
                        nvo.Details.AfterPrepProgressiveMotatity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepProgressiveMotatity"]));
                        nvo.Details.NativeOverallMotality = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeOverallMotality"]));
                        nvo.Details.AfterPrepOverallMotality = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepOverallMotality"]));
                        nvo.Details.NativeNormalForms = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeNormalForms"]));
                        nvo.Details.AfterPrepNormalForms = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepNormalForms"]));
                        nvo.Details.NativeTotalNoOfSperms = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeTotalNoOfSperms"]));
                        nvo.Details.AfterPrepTotalNoOfSperms = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepTotalNoOfSperms"]));
                        nvo.Details.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }
    }
}

