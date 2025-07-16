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

    internal class clsIVFDashboad_OPUDAL : clsBaseIVFDashboad_OPUDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFDashboad_OPUDAL()
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

        public override IValueObject AddUpdateOocyteNumber(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateOocyteNumberBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateOocyteNumberBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateOocyteNumber");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.OPUDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.OPUDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.OPUDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.OPUDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteRetrived", DbType.Int64, nvo.OPUDetails.OocyteRetrived);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.OPUDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateOPUDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateOPUDetailsBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateOPUDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateOPU");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.OPUDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.OPUDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.OPUDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.OPUDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.OPUDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.OPUDetails.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.OPUDetails.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.OPUDetails.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetiaDetails", DbType.String, nvo.OPUDetails.AnesthetiaDetails);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.OPUDetails.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "NeedleUsedID", DbType.Int64, nvo.OPUDetails.NeedleUsedID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteRetrived", DbType.Int64, nvo.OPUDetails.OocyteRetrived);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteQualityID", DbType.Int64, nvo.OPUDetails.OocyteQualityID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteRemark", DbType.String, nvo.OPUDetails.OocyteRemark);
                this.dbServer.AddInParameter(storedProcCommand, "ELevelDayID", DbType.Int64, nvo.OPUDetails.ELevelDayID);
                this.dbServer.AddInParameter(storedProcCommand, "Evalue", DbType.String, nvo.OPUDetails.Evalue);
                this.dbServer.AddInParameter(storedProcCommand, "ELevelDayremark", DbType.String, nvo.OPUDetails.ELevelDayremark);
                this.dbServer.AddInParameter(storedProcCommand, "DifficultyID", DbType.Int64, nvo.OPUDetails.DifficultyID);
                this.dbServer.AddInParameter(storedProcCommand, "IsSetForED", DbType.Boolean, nvo.OPUDetails.IsSetForED);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteForED", DbType.Int64, nvo.OPUDetails.OocyteForED);
                this.dbServer.AddInParameter(storedProcCommand, "MI", DbType.Decimal, nvo.OPUDetails.MI);
                this.dbServer.AddInParameter(storedProcCommand, "MII", DbType.Decimal, nvo.OPUDetails.MII);
                this.dbServer.AddInParameter(storedProcCommand, "GV", DbType.Decimal, nvo.OPUDetails.GV);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteCytoplasmDysmorphisimPresent", DbType.Decimal, nvo.OPUDetails.OocyteCytoplasmDysmorphisimPresent);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteCytoplasmDysmorphisimAbsent", DbType.Decimal, nvo.OPUDetails.OocyteCytoplasmDysmorphisimAbsent);
                this.dbServer.AddInParameter(storedProcCommand, "ExtracytoplasmicDysmorphisimPresent", DbType.Decimal, nvo.OPUDetails.ExtracytoplasmicDysmorphisimPresent);
                this.dbServer.AddInParameter(storedProcCommand, "ExtracytoplasmicDysmorphisimAbsent", DbType.Decimal, nvo.OPUDetails.ExtracytoplasmicDysmorphisimAbsent);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteCoronaCumulusComplexPresent", DbType.Decimal, nvo.OPUDetails.OocyteCoronaCumulusComplexPresent);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteCoronaCumulusComplexAbsent", DbType.Decimal, nvo.OPUDetails.OocyteCoronaCumulusComplexAbsent);
                this.dbServer.AddInParameter(storedProcCommand, "TriggerDate", DbType.DateTime, nvo.OPUDetails.TriggerDate);
                this.dbServer.AddInParameter(storedProcCommand, "TriggerTime", DbType.DateTime, nvo.OPUDetails.TriggerTime);
                this.dbServer.AddInParameter(storedProcCommand, "TypeOfNeedleID", DbType.Int64, nvo.OPUDetails.TypeOfNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthesiaID", DbType.Int64, nvo.OPUDetails.AnesthesiaID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFlushing", DbType.Boolean, nvo.OPUDetails.IsFlushing);
                this.dbServer.AddInParameter(storedProcCommand, "NeedleUsed", DbType.Int64, nvo.OPUDetails.NeedleUsed);
                this.dbServer.AddInParameter(storedProcCommand, "IsPreAnesthesia", DbType.Boolean, nvo.OPUDetails.IsPreAnesthesia);
                this.dbServer.AddInParameter(storedProcCommand, "LeftFollicule", DbType.String, nvo.OPUDetails.LeftFollicule);
                this.dbServer.AddInParameter(storedProcCommand, "RightFollicule", DbType.String, nvo.OPUDetails.RightFollicule);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.OPUDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "Reason", DbType.String, nvo.OPUDetails.Reasons);
                this.dbServer.AddInParameter(storedProcCommand, "IsCycleCancellation", DbType.Boolean, nvo.OPUDetails.IsCycleCancellation);
                this.dbServer.AddInParameter(storedProcCommand, "BalanceOocyte", DbType.Int64, nvo.OPUDetails.BalanceOocyte);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.OPUDetails.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OPUDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.OPUDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.OPUDetails.IsSetForED && nvo.OPUDetails.Isfreezed)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashBoard_UpdateEDFagForARTCycle");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.OPUDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.OPUDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.OPUDetails.PlanTherapyID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.OPUDetails.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command2, "IsEmbryoDonation", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                }
                DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashBoard_UpdateTherapyOverView_OPUDate");
                this.dbServer.AddInParameter(command3, "PlanTherapyId", DbType.Int64, nvo.OPUDetails.PlanTherapyID);
                this.dbServer.AddInParameter(command3, "PlanTherapyUnitId", DbType.Int64, nvo.OPUDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(command3, "TherapyTypeId", DbType.Int64, 4);
                this.dbServer.AddInParameter(command3, "Day1", DbType.String, nvo.TherapyExecutionDetial.Day1);
                this.dbServer.AddInParameter(command3, "Day2", DbType.String, nvo.TherapyExecutionDetial.Day2);
                this.dbServer.AddInParameter(command3, "Day3", DbType.String, nvo.TherapyExecutionDetial.Day3);
                this.dbServer.AddInParameter(command3, "Day4", DbType.String, nvo.TherapyExecutionDetial.Day4);
                this.dbServer.AddInParameter(command3, "Day5", DbType.String, nvo.TherapyExecutionDetial.Day5);
                this.dbServer.AddInParameter(command3, "Day6", DbType.String, nvo.TherapyExecutionDetial.Day6);
                this.dbServer.AddInParameter(command3, "Day7", DbType.String, nvo.TherapyExecutionDetial.Day7);
                this.dbServer.AddInParameter(command3, "Day8", DbType.String, nvo.TherapyExecutionDetial.Day8);
                this.dbServer.AddInParameter(command3, "Day9", DbType.String, nvo.TherapyExecutionDetial.Day9);
                this.dbServer.AddInParameter(command3, "Day10", DbType.String, nvo.TherapyExecutionDetial.Day10);
                this.dbServer.AddInParameter(command3, "Day11", DbType.String, nvo.TherapyExecutionDetial.Day11);
                this.dbServer.AddInParameter(command3, "Day12", DbType.String, nvo.TherapyExecutionDetial.Day12);
                this.dbServer.AddInParameter(command3, "Day13", DbType.String, nvo.TherapyExecutionDetial.Day13);
                this.dbServer.AddInParameter(command3, "Day14", DbType.String, nvo.TherapyExecutionDetial.Day14);
                this.dbServer.AddInParameter(command3, "Day15", DbType.String, nvo.TherapyExecutionDetial.Day15);
                this.dbServer.AddInParameter(command3, "Day16", DbType.String, nvo.TherapyExecutionDetial.Day16);
                this.dbServer.AddInParameter(command3, "Day17", DbType.String, nvo.TherapyExecutionDetial.Day17);
                this.dbServer.AddInParameter(command3, "Day18", DbType.String, nvo.TherapyExecutionDetial.Day18);
                this.dbServer.AddInParameter(command3, "Day19", DbType.String, nvo.TherapyExecutionDetial.Day19);
                this.dbServer.AddInParameter(command3, "Day20", DbType.String, nvo.TherapyExecutionDetial.Day20);
                this.dbServer.AddInParameter(command3, "Day21", DbType.String, nvo.TherapyExecutionDetial.Day21);
                this.dbServer.AddInParameter(command3, "Day22", DbType.String, nvo.TherapyExecutionDetial.Day22);
                this.dbServer.AddInParameter(command3, "Day23", DbType.String, nvo.TherapyExecutionDetial.Day23);
                this.dbServer.AddInParameter(command3, "Day24", DbType.String, nvo.TherapyExecutionDetial.Day24);
                this.dbServer.AddInParameter(command3, "Day25", DbType.String, nvo.TherapyExecutionDetial.Day25);
                this.dbServer.AddInParameter(command3, "Day26", DbType.String, nvo.TherapyExecutionDetial.Day26);
                this.dbServer.AddInParameter(command3, "Day27", DbType.String, nvo.TherapyExecutionDetial.Day27);
                this.dbServer.AddInParameter(command3, "Day28", DbType.String, nvo.TherapyExecutionDetial.Day28);
                this.dbServer.AddInParameter(command3, "Day29", DbType.String, nvo.TherapyExecutionDetial.Day29);
                this.dbServer.AddInParameter(command3, "Day30", DbType.String, nvo.TherapyExecutionDetial.Day30);
                this.dbServer.AddInParameter(command3, "Day31", DbType.String, nvo.TherapyExecutionDetial.Day31);
                this.dbServer.AddInParameter(command3, "Day32", DbType.String, nvo.TherapyExecutionDetial.Day32);
                this.dbServer.AddInParameter(command3, "Day33", DbType.String, nvo.TherapyExecutionDetial.Day33);
                this.dbServer.AddInParameter(command3, "Day34", DbType.String, nvo.TherapyExecutionDetial.Day34);
                this.dbServer.AddInParameter(command3, "Day35", DbType.String, nvo.TherapyExecutionDetial.Day35);
                this.dbServer.AddInParameter(command3, "Day36", DbType.String, nvo.TherapyExecutionDetial.Day36);
                this.dbServer.AddInParameter(command3, "Day37", DbType.String, nvo.TherapyExecutionDetial.Day37);
                this.dbServer.AddInParameter(command3, "Day38", DbType.String, nvo.TherapyExecutionDetial.Day38);
                this.dbServer.AddInParameter(command3, "Day39", DbType.String, nvo.TherapyExecutionDetial.Day39);
                this.dbServer.AddInParameter(command3, "Day40", DbType.String, nvo.TherapyExecutionDetial.Day40);
                this.dbServer.AddInParameter(command3, "Day41", DbType.String, nvo.TherapyExecutionDetial.Day41);
                this.dbServer.AddInParameter(command3, "Day42", DbType.String, nvo.TherapyExecutionDetial.Day42);
                this.dbServer.AddInParameter(command3, "Day43", DbType.String, nvo.TherapyExecutionDetial.Day43);
                this.dbServer.AddInParameter(command3, "Day44", DbType.String, nvo.TherapyExecutionDetial.Day44);
                this.dbServer.AddInParameter(command3, "Day45", DbType.String, nvo.TherapyExecutionDetial.Day45);
                this.dbServer.AddInParameter(command3, "Day46", DbType.String, nvo.TherapyExecutionDetial.Day46);
                this.dbServer.AddInParameter(command3, "Day47", DbType.String, nvo.TherapyExecutionDetial.Day47);
                this.dbServer.AddInParameter(command3, "Day48", DbType.String, nvo.TherapyExecutionDetial.Day48);
                this.dbServer.AddInParameter(command3, "Day49", DbType.String, nvo.TherapyExecutionDetial.Day49);
                this.dbServer.AddInParameter(command3, "Day50", DbType.String, nvo.TherapyExecutionDetial.Day50);
                this.dbServer.AddInParameter(command3, "Day51", DbType.String, nvo.TherapyExecutionDetial.Day51);
                this.dbServer.AddInParameter(command3, "Day52", DbType.String, nvo.TherapyExecutionDetial.Day52);
                this.dbServer.AddInParameter(command3, "Day53", DbType.String, nvo.TherapyExecutionDetial.Day53);
                this.dbServer.AddInParameter(command3, "Day54", DbType.String, nvo.TherapyExecutionDetial.Day54);
                this.dbServer.AddInParameter(command3, "Day55", DbType.String, nvo.TherapyExecutionDetial.Day55);
                this.dbServer.AddInParameter(command3, "Day56", DbType.String, nvo.TherapyExecutionDetial.Day56);
                this.dbServer.AddInParameter(command3, "Day57", DbType.String, nvo.TherapyExecutionDetial.Day57);
                this.dbServer.AddInParameter(command3, "Day58", DbType.String, nvo.TherapyExecutionDetial.Day58);
                this.dbServer.AddInParameter(command3, "Day59", DbType.String, nvo.TherapyExecutionDetial.Day59);
                this.dbServer.AddInParameter(command3, "Day60", DbType.String, nvo.TherapyExecutionDetial.Day60);
                this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(command3, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.OPUDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetEmbryologySummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetEmbryologySummaryBizActionVO nvo = valueObject as clsIVFDashboard_GetEmbryologySummaryBizActionVO;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetEmbryologySummary");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.EmbSummary.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.EmbSummary.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.EmbSummary.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.EmbSummary.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.EmbSummary.NoOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOocyte"]));
                        nvo.EmbSummary.MI = Convert.ToInt64(DALHelper.HandleDBNull(reader["MI"]));
                        nvo.EmbSummary.MII = Convert.ToInt64(DALHelper.HandleDBNull(reader["MII"]));
                        nvo.EmbSummary.GV = Convert.ToInt64(DALHelper.HandleDBNull(reader["GV"]));
                        nvo.EmbSummary.OocyteCytoplasmDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmPresent"]));
                        nvo.EmbSummary.OocyteCytoplasmDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmAbsent"]));
                        nvo.EmbSummary.ExtracytoplasmicDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicPresent"]));
                        nvo.EmbSummary.ExtracytoplasmicDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicAbsent"]));
                        nvo.EmbSummary.OocyteCoronaCumulusComplexNormal = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaNormal"]));
                        nvo.EmbSummary.OocyteCoronaCumulusComplexAbnormal = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaAbnormal"]));
                        nvo.EmbSummary.OPUDate = DALHelper.HandleDate(reader["OPUDate"]);
                        nvo.EmbSummary.OPUTime = DALHelper.HandleDate(reader["OPUTime"]);
                        nvo.EmbSummary.FreshPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreshPGDPGS"]));
                        nvo.EmbSummary.FrozenPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrozenPGDPGS"]));
                        nvo.EmbSummary.ThawedPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawedPGDPGS"]));
                        nvo.EmbSummary.PostThawedPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["PostThawedPGDPGS"]));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetOPUDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetOPUDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetOPUDetailsBizActionVO;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetOPUDetails");
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
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssitantEmbryologistID"]));
                        nvo.Details.AnesthetiaDetails = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthetiaDetails"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.OocyteQualityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteQualityID"]));
                        nvo.Details.OocyteRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteRemark"]));
                        nvo.Details.OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"]));
                        nvo.Details.NeedleUsedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NeedleUsedID"]));
                        nvo.Details.ELevelDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ELevelDayID"]));
                        nvo.Details.Evalue = Convert.ToString(DALHelper.HandleDBNull(reader["Evalue"]));
                        nvo.Details.DifficultyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyID"]));
                        nvo.Details.ELevelDayremark = Convert.ToString(DALHelper.HandleDBNull(reader["ELevelDayremark"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.IsSetForED = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetForED"]));
                        nvo.Details.OocyteForED = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteForED"]));
                        nvo.Details.BalanceOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["BalanceOocyte"]));
                        nvo.Details.MI = Convert.ToInt64(DALHelper.HandleDBNull(reader["MI"]));
                        nvo.Details.MII = Convert.ToInt64(DALHelper.HandleDBNull(reader["MII"]));
                        nvo.Details.GV = Convert.ToInt64(DALHelper.HandleDBNull(reader["GV"]));
                        nvo.Details.OocyteCytoplasmDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmDysmorphisimPresent"]));
                        nvo.Details.OocyteCytoplasmDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmDysmorphisimAbsent"]));
                        nvo.Details.ExtracytoplasmicDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicDysmorphisimPresent"]));
                        nvo.Details.ExtracytoplasmicDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicDysmorphisimAbsent"]));
                        nvo.Details.OocyteCoronaCumulusComplexPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplexPresent"]));
                        nvo.Details.OocyteCoronaCumulusComplexAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplexAbsent"]));
                        nvo.Details.TriggerDate = DALHelper.HandleDate(reader["TriggerDate"]);
                        nvo.Details.TriggerTime = DALHelper.HandleDate(reader["TriggerTime"]);
                        nvo.Details.IsPreAnesthesia = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPreAnesthesia"]));
                        nvo.Details.AnesthesiaID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaID"]));
                        nvo.Details.NeedleUsed = Convert.ToInt64(DALHelper.HandleDBNull(reader["NeedleUsed"]));
                        nvo.Details.TypeOfNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TypeOfNeedleID"]));
                        nvo.Details.IsFlushing = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFlushing"]));
                        nvo.Details.IsCycleCancellation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCycleCancellation"]));
                        nvo.Details.Reasons = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                        nvo.Details.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        nvo.Details.LeftFollicule = Convert.ToString(DALHelper.HandleDBNull(reader["LeftFollicule"]));
                        nvo.Details.RightFollicule = Convert.ToString(DALHelper.HandleDBNull(reader["RightFollicule"]));
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

