using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboard_IUIDAL: clsBaseIVFDashboard_IUIDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion
        private clsIVFDashboard_IUIDAL()
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

        public override IValueObject AddUpdateIUIDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddUpdateIUIDetailsBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateIUIDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateIUI");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.IUIDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.IUIDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.IUIDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.IUIDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "IUIDate", DbType.DateTime, BizActionObj.IUIDetails.IUIDate);
                dbServer.AddInParameter(command, "IUITime", DbType.DateTime, BizActionObj.IUIDetails.IUITime);
                dbServer.AddInParameter(command, "InseminatedByID", DbType.Int64, BizActionObj.IUIDetails.InseminatedByID);
                dbServer.AddInParameter(command, "WitnessedByID", DbType.Int64, BizActionObj.IUIDetails.WitnessedByID);
                dbServer.AddInParameter(command, "InseminationLocationID", DbType.Int64, BizActionObj.IUIDetails.InseminationLocationID);
                dbServer.AddInParameter(command, "IsHomologous", DbType.Boolean, BizActionObj.IUIDetails.IsHomologous);
                dbServer.AddInParameter(command, "CollectionDate", DbType.DateTime, BizActionObj.IUIDetails.CollectionDate);
                dbServer.AddInParameter(command, "PreperationDate", DbType.DateTime, BizActionObj.IUIDetails.PreperationDate);
                dbServer.AddInParameter(command, "ThawingDate", DbType.DateTime, BizActionObj.IUIDetails.ThawingDate);
                dbServer.AddInParameter(command, "SampleID", DbType.String, BizActionObj.IUIDetails.SampleID);
                dbServer.AddInParameter(command, "Purpose", DbType.String, BizActionObj.IUIDetails.Purpose);
                dbServer.AddInParameter(command, "Diagnosis", DbType.String, BizActionObj.IUIDetails.Diagnosis);
                dbServer.AddInParameter(command, "CollectionMethodID", DbType.Int64, BizActionObj.IUIDetails.CollectionMethodID);
                dbServer.AddInParameter(command, "InseminatedAmounts", DbType.Double, BizActionObj.IUIDetails.InseminatedAmounts);
                dbServer.AddInParameter(command, "NumberofMotileSperm", DbType.Double, BizActionObj.IUIDetails.NumberofMotileSperm);
                dbServer.AddInParameter(command, "NativeAmount", DbType.Double, BizActionObj.IUIDetails.NativeAmount);
                dbServer.AddInParameter(command, "AfterPrepAmount", DbType.Double, BizActionObj.IUIDetails.AfterPrepAmount);
                dbServer.AddInParameter(command, "NativeConcentration", DbType.Double, BizActionObj.IUIDetails.NativeConcentration);
                dbServer.AddInParameter(command, "AfterPrepConcentration", DbType.Double, BizActionObj.IUIDetails.AfterPrepConcentration);
                dbServer.AddInParameter(command, "NativeProgressiveMotatity", DbType.Double, BizActionObj.IUIDetails.NativeProgressiveMotatity);
                dbServer.AddInParameter(command, "AfterPrepProgressiveMotatity", DbType.Double, BizActionObj.IUIDetails.AfterPrepProgressiveMotatity);
                dbServer.AddInParameter(command, "NativeOverallMotality", DbType.Double, BizActionObj.IUIDetails.NativeOverallMotality);
                dbServer.AddInParameter(command, "AfterPrepOverallMotality", DbType.Double, BizActionObj.IUIDetails.AfterPrepOverallMotality);
                dbServer.AddInParameter(command, "NativeNormalForms", DbType.Double, BizActionObj.IUIDetails.NativeNormalForms);
                dbServer.AddInParameter(command, "AfterPrepNormalForms", DbType.Double, BizActionObj.IUIDetails.AfterPrepNormalForms);
                dbServer.AddInParameter(command, "NativeTotalNoOfSperms", DbType.Double, BizActionObj.IUIDetails.NativeTotalNoOfSperms);
                dbServer.AddInParameter(command, "AfterPrepTotalNoOfSperms", DbType.Double, BizActionObj.IUIDetails.AfterPrepTotalNoOfSperms);
                dbServer.AddInParameter(command, "Notes", DbType.String, BizActionObj.IUIDetails.Notes);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.IUIDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.IUIDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.IUIDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }
        public override IValueObject GetIUIDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetIUIDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetIUIDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetIUIetails");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.Details.PatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.IUIDate = Convert.ToDateTime(DALHelper.HandleDate(reader["IUIDate"]));
                        BizAction.Details.IUITime = Convert.ToDateTime(DALHelper.HandleDate(reader["IUITime"]));
                        BizAction.Details.InseminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InseminatedByID"]));
                        BizAction.Details.WitnessedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessedByID"]));
                        BizAction.Details.InseminationLocationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InseminationLocationID"]));
                        BizAction.Details.IsHomologous = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsHomologous"]));
                        BizAction.Details.CollectionDate = Convert.ToDateTime(DALHelper.HandleDate(reader["CollectionDate"]));
                        BizAction.Details.PreperationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["PreperationDate"]));
                        BizAction.Details.ThawingDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ThawingDate"]));
                        BizAction.Details.SampleID = Convert.ToString(DALHelper.HandleDBNull(reader["SampleID"]));
                        BizAction.Details.Purpose = Convert.ToString(DALHelper.HandleDBNull(reader["Purpose"]));
                        BizAction.Details.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["Diagnosis"]));
                        BizAction.Details.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        BizAction.Details.InseminatedAmounts = Convert.ToDouble(DALHelper.HandleDBNull(reader["InseminatedAmounts"]));
                        BizAction.Details.NumberofMotileSperm = Convert.ToDouble(DALHelper.HandleDBNull(reader["NumberofMotileSperm"]));
                        BizAction.Details.NativeAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeAmount"]));
                        BizAction.Details.AfterPrepAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepAmount"]));
                        BizAction.Details.NativeConcentration = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeConcentration"]));
                        BizAction.Details.AfterPrepConcentration = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepConcentration"]));
                        BizAction.Details.NativeProgressiveMotatity = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeProgressiveMotatity"]));
                        BizAction.Details.AfterPrepProgressiveMotatity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepProgressiveMotatity"]));
                        BizAction.Details.NativeOverallMotality = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeOverallMotality"]));
                        BizAction.Details.AfterPrepOverallMotality = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepOverallMotality"]));
                        BizAction.Details.NativeNormalForms = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeNormalForms"]));
                        BizAction.Details.AfterPrepNormalForms = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepNormalForms"]));
                        BizAction.Details.NativeTotalNoOfSperms = Convert.ToDouble(DALHelper.HandleDBNull(reader["NativeTotalNoOfSperms"]));
                        BizAction.Details.AfterPrepTotalNoOfSperms = Convert.ToDouble(DALHelper.HandleDBNull(reader["AfterPrepTotalNoOfSperms"]));
                        BizAction.Details.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizAction;
        }
    }
}