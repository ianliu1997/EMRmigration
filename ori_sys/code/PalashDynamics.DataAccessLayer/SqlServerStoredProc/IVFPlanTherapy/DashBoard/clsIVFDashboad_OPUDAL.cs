using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboad_OPUDAL: clsBaseIVFDashboad_OPUDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion
        private clsIVFDashboad_OPUDAL()
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

        public override IValueObject AddUpdateOPUDetails(IValueObject valueObject, clsUserVO UserVo)
        {
           // throw new NotImplementedException();

            clsIVFDashboard_AddUpdateOPUDetailsBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateOPUDetailsBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateOPU");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.OPUDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.OPUDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.OPUDetails.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.OPUDetails.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.OPUDetails.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.OPUDetails.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetiaDetails", DbType.String, BizActionObj.OPUDetails.AnesthetiaDetails);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.OPUDetails.AnesthetistID);
                dbServer.AddInParameter(command, "NeedleUsedID", DbType.Int64, BizActionObj.OPUDetails.NeedleUsedID);
                dbServer.AddInParameter(command, "OocyteRetrived", DbType.Int64, BizActionObj.OPUDetails.OocyteRetrived);
                dbServer.AddInParameter(command, "OocyteQualityID", DbType.Int64, BizActionObj.OPUDetails.OocyteQualityID);
                dbServer.AddInParameter(command, "OocyteRemark", DbType.String, BizActionObj.OPUDetails.OocyteRemark);
                dbServer.AddInParameter(command, "ELevelDayID", DbType.Int64, BizActionObj.OPUDetails.ELevelDayID);
                dbServer.AddInParameter(command, "Evalue", DbType.String, BizActionObj.OPUDetails.Evalue);
                dbServer.AddInParameter(command, "ELevelDayremark", DbType.String, BizActionObj.OPUDetails.ELevelDayremark);
                dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, BizActionObj.OPUDetails.DifficultyID);
                dbServer.AddInParameter(command, "IsSetForED", DbType.Boolean, BizActionObj.OPUDetails.IsSetForED);
                dbServer.AddInParameter(command, "OocyteForED", DbType.Int64, BizActionObj.OPUDetails.OocyteForED);

                dbServer.AddInParameter(command, "MI", DbType.Decimal, BizActionObj.OPUDetails.MI);
                dbServer.AddInParameter(command, "MII", DbType.Decimal, BizActionObj.OPUDetails.MII);
                dbServer.AddInParameter(command, "GV", DbType.Decimal, BizActionObj.OPUDetails.GV);

                //added by neena
                dbServer.AddInParameter(command, "OocyteCytoplasmDysmorphisimPresent", DbType.Decimal, BizActionObj.OPUDetails.OocyteCytoplasmDysmorphisimPresent);
                dbServer.AddInParameter(command, "OocyteCytoplasmDysmorphisimAbsent", DbType.Decimal, BizActionObj.OPUDetails.OocyteCytoplasmDysmorphisimAbsent);
                dbServer.AddInParameter(command, "ExtracytoplasmicDysmorphisimPresent", DbType.Decimal, BizActionObj.OPUDetails.ExtracytoplasmicDysmorphisimPresent);
                dbServer.AddInParameter(command, "ExtracytoplasmicDysmorphisimAbsent", DbType.Decimal, BizActionObj.OPUDetails.ExtracytoplasmicDysmorphisimAbsent);
                dbServer.AddInParameter(command, "OocyteCoronaCumulusComplexPresent", DbType.Decimal, BizActionObj.OPUDetails.OocyteCoronaCumulusComplexPresent);
                dbServer.AddInParameter(command, "OocyteCoronaCumulusComplexAbsent", DbType.Decimal, BizActionObj.OPUDetails.OocyteCoronaCumulusComplexAbsent);

                // Added by Anumani 
                // As per the changes in the OPU Form 

                dbServer.AddInParameter(command, "TriggerDate", DbType.DateTime, BizActionObj.OPUDetails.TriggerDate);
                dbServer.AddInParameter(command, "TriggerTime", DbType.DateTime, BizActionObj.OPUDetails.TriggerTime);
                dbServer.AddInParameter(command, "TypeOfNeedleID", DbType.Int64, BizActionObj.OPUDetails.TypeOfNeedleID);
                dbServer.AddInParameter(command, "AnesthesiaID", DbType.Int64, BizActionObj.OPUDetails.AnesthesiaID);
                dbServer.AddInParameter(command, "IsFlushing", DbType.Boolean, BizActionObj.OPUDetails.IsFlushing);
                dbServer.AddInParameter(command, "NeedleUsed", DbType.Int64, BizActionObj.OPUDetails.NeedleUsed);
                dbServer.AddInParameter(command, "IsPreAnesthesia", DbType.Boolean, BizActionObj.OPUDetails.IsPreAnesthesia);
                dbServer.AddInParameter(command, "LeftFollicule", DbType.String, BizActionObj.OPUDetails.LeftFollicule);
                dbServer.AddInParameter(command, "RightFollicule", DbType.String, BizActionObj.OPUDetails.RightFollicule);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.OPUDetails.Remark);
                dbServer.AddInParameter(command, "Reason", DbType.String, BizActionObj.OPUDetails.Reasons);
                dbServer.AddInParameter(command, "IsCycleCancellation", DbType.Boolean, BizActionObj.OPUDetails.IsCycleCancellation);  
               








                dbServer.AddInParameter(command, "BalanceOocyte", DbType.Int64, BizActionObj.OPUDetails.BalanceOocyte);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.OPUDetails.Isfreezed);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.OPUDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.OPUDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.OPUDetails.IsSetForED == true && BizActionObj.OPUDetails.Isfreezed == true)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_UpdateEDFagForARTCycle");

                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.OPUDetails.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.OPUDetails.PatientUnitID);
                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyID);
                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyUnitID);
                    dbServer.AddInParameter(command1, "IsEmbryoDonation", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                }
                DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_UpdateTherapyOverView_OPUDate");
                //dbServer.AddInParameter(command2, "Day", DbType.String, BizActionObj.TherapyExecutionDetial.Day);
                dbServer.AddInParameter(command2, "PlanTherapyId", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyID);
                dbServer.AddInParameter(command2, "PlanTherapyUnitId", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command2, "TherapyTypeId", DbType.Int64, 4);
                dbServer.AddInParameter(command2, "Day1", DbType.String, BizActionObj.TherapyExecutionDetial.Day1);
                dbServer.AddInParameter(command2, "Day2", DbType.String, BizActionObj.TherapyExecutionDetial.Day2);
                dbServer.AddInParameter(command2, "Day3", DbType.String, BizActionObj.TherapyExecutionDetial.Day3);
                dbServer.AddInParameter(command2, "Day4", DbType.String, BizActionObj.TherapyExecutionDetial.Day4);
                dbServer.AddInParameter(command2, "Day5", DbType.String, BizActionObj.TherapyExecutionDetial.Day5);
                dbServer.AddInParameter(command2, "Day6", DbType.String, BizActionObj.TherapyExecutionDetial.Day6);
                dbServer.AddInParameter(command2, "Day7", DbType.String, BizActionObj.TherapyExecutionDetial.Day7);
                dbServer.AddInParameter(command2, "Day8", DbType.String, BizActionObj.TherapyExecutionDetial.Day8);
                dbServer.AddInParameter(command2, "Day9", DbType.String, BizActionObj.TherapyExecutionDetial.Day9);
                dbServer.AddInParameter(command2, "Day10", DbType.String, BizActionObj.TherapyExecutionDetial.Day10);
                dbServer.AddInParameter(command2, "Day11", DbType.String, BizActionObj.TherapyExecutionDetial.Day11);
                dbServer.AddInParameter(command2, "Day12", DbType.String, BizActionObj.TherapyExecutionDetial.Day12);
                dbServer.AddInParameter(command2, "Day13", DbType.String, BizActionObj.TherapyExecutionDetial.Day13);
                dbServer.AddInParameter(command2, "Day14", DbType.String, BizActionObj.TherapyExecutionDetial.Day14);
                dbServer.AddInParameter(command2, "Day15", DbType.String, BizActionObj.TherapyExecutionDetial.Day15);
                dbServer.AddInParameter(command2, "Day16", DbType.String, BizActionObj.TherapyExecutionDetial.Day16);
                dbServer.AddInParameter(command2, "Day17", DbType.String, BizActionObj.TherapyExecutionDetial.Day17);
                dbServer.AddInParameter(command2, "Day18", DbType.String, BizActionObj.TherapyExecutionDetial.Day18);
                dbServer.AddInParameter(command2, "Day19", DbType.String, BizActionObj.TherapyExecutionDetial.Day19);
                dbServer.AddInParameter(command2, "Day20", DbType.String, BizActionObj.TherapyExecutionDetial.Day20);
                dbServer.AddInParameter(command2, "Day21", DbType.String, BizActionObj.TherapyExecutionDetial.Day21);
                dbServer.AddInParameter(command2, "Day22", DbType.String, BizActionObj.TherapyExecutionDetial.Day22);
                dbServer.AddInParameter(command2, "Day23", DbType.String, BizActionObj.TherapyExecutionDetial.Day23);
                dbServer.AddInParameter(command2, "Day24", DbType.String, BizActionObj.TherapyExecutionDetial.Day24);
                dbServer.AddInParameter(command2, "Day25", DbType.String, BizActionObj.TherapyExecutionDetial.Day25);
                dbServer.AddInParameter(command2, "Day26", DbType.String, BizActionObj.TherapyExecutionDetial.Day26);
                dbServer.AddInParameter(command2, "Day27", DbType.String, BizActionObj.TherapyExecutionDetial.Day27);
                dbServer.AddInParameter(command2, "Day28", DbType.String, BizActionObj.TherapyExecutionDetial.Day28);
                dbServer.AddInParameter(command2, "Day29", DbType.String, BizActionObj.TherapyExecutionDetial.Day29);
                dbServer.AddInParameter(command2, "Day30", DbType.String, BizActionObj.TherapyExecutionDetial.Day30);
                dbServer.AddInParameter(command2, "Day31", DbType.String, BizActionObj.TherapyExecutionDetial.Day31);
                dbServer.AddInParameter(command2, "Day32", DbType.String, BizActionObj.TherapyExecutionDetial.Day32);
                dbServer.AddInParameter(command2, "Day33", DbType.String, BizActionObj.TherapyExecutionDetial.Day33);
                dbServer.AddInParameter(command2, "Day34", DbType.String, BizActionObj.TherapyExecutionDetial.Day34);
                dbServer.AddInParameter(command2, "Day35", DbType.String, BizActionObj.TherapyExecutionDetial.Day35);
                dbServer.AddInParameter(command2, "Day36", DbType.String, BizActionObj.TherapyExecutionDetial.Day36);
                dbServer.AddInParameter(command2, "Day37", DbType.String, BizActionObj.TherapyExecutionDetial.Day37);
                dbServer.AddInParameter(command2, "Day38", DbType.String, BizActionObj.TherapyExecutionDetial.Day38);
                dbServer.AddInParameter(command2, "Day39", DbType.String, BizActionObj.TherapyExecutionDetial.Day39);
                dbServer.AddInParameter(command2, "Day40", DbType.String, BizActionObj.TherapyExecutionDetial.Day40);
                dbServer.AddInParameter(command2, "Day41", DbType.String, BizActionObj.TherapyExecutionDetial.Day41);
                dbServer.AddInParameter(command2, "Day42", DbType.String, BizActionObj.TherapyExecutionDetial.Day42);
                dbServer.AddInParameter(command2, "Day43", DbType.String, BizActionObj.TherapyExecutionDetial.Day43);
                dbServer.AddInParameter(command2, "Day44", DbType.String, BizActionObj.TherapyExecutionDetial.Day44);
                dbServer.AddInParameter(command2, "Day45", DbType.String, BizActionObj.TherapyExecutionDetial.Day45);
                dbServer.AddInParameter(command2, "Day46", DbType.String, BizActionObj.TherapyExecutionDetial.Day46);
                dbServer.AddInParameter(command2, "Day47", DbType.String, BizActionObj.TherapyExecutionDetial.Day47);
                dbServer.AddInParameter(command2, "Day48", DbType.String, BizActionObj.TherapyExecutionDetial.Day48);
                dbServer.AddInParameter(command2, "Day49", DbType.String, BizActionObj.TherapyExecutionDetial.Day49);
                dbServer.AddInParameter(command2, "Day50", DbType.String, BizActionObj.TherapyExecutionDetial.Day50);
                dbServer.AddInParameter(command2, "Day51", DbType.String, BizActionObj.TherapyExecutionDetial.Day51);
                dbServer.AddInParameter(command2, "Day52", DbType.String, BizActionObj.TherapyExecutionDetial.Day52);
                dbServer.AddInParameter(command2, "Day53", DbType.String, BizActionObj.TherapyExecutionDetial.Day53);
                dbServer.AddInParameter(command2, "Day54", DbType.String, BizActionObj.TherapyExecutionDetial.Day54);
                dbServer.AddInParameter(command2, "Day55", DbType.String, BizActionObj.TherapyExecutionDetial.Day55);
                dbServer.AddInParameter(command2, "Day56", DbType.String, BizActionObj.TherapyExecutionDetial.Day56);
                dbServer.AddInParameter(command2, "Day57", DbType.String, BizActionObj.TherapyExecutionDetial.Day57);
                dbServer.AddInParameter(command2, "Day58", DbType.String, BizActionObj.TherapyExecutionDetial.Day58);
                dbServer.AddInParameter(command2, "Day59", DbType.String, BizActionObj.TherapyExecutionDetial.Day59);
                dbServer.AddInParameter(command2, "Day60", DbType.String, BizActionObj.TherapyExecutionDetial.Day60);
                dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.ExecuteNonQuery(command2, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.OPUDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetOPUDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
        
            clsIVFDashboard_GetOPUDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetOPUDetailsBizActionVO;
             DbConnection con = dbServer.CreateConnection();
             try
             {
                 DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetOPUDetails");
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
                         BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                         BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                         BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                         BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssitantEmbryologistID"]));
                         BizAction.Details.AnesthetiaDetails = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthetiaDetails"]));
                         BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                         BizAction.Details.OocyteQualityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteQualityID"]));
                         BizAction.Details.OocyteRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteRemark"]));
                         BizAction.Details.OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"]));
                         BizAction.Details.NeedleUsedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NeedleUsedID"]));
                         BizAction.Details.ELevelDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ELevelDayID"]));
                         BizAction.Details.Evalue = Convert.ToString(DALHelper.HandleDBNull(reader["Evalue"]));
                         BizAction.Details.DifficultyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyID"]));
                         BizAction.Details.ELevelDayremark = Convert.ToString(DALHelper.HandleDBNull(reader["ELevelDayremark"]));
                         BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                         BizAction.Details.IsSetForED = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSetForED"]));
                         BizAction.Details.OocyteForED = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteForED"]));
                         BizAction.Details.BalanceOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["BalanceOocyte"]));
                         BizAction.Details.MI = Convert.ToInt64(DALHelper.HandleDBNull(reader["MI"]));
                         BizAction.Details.MII = Convert.ToInt64(DALHelper.HandleDBNull(reader["MII"]));
                         BizAction.Details.GV = Convert.ToInt64(DALHelper.HandleDBNull(reader["GV"]));
                         BizAction.Details.OocyteCytoplasmDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmDysmorphisimPresent"]));
                         BizAction.Details.OocyteCytoplasmDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmDysmorphisimAbsent"]));
                         BizAction.Details.ExtracytoplasmicDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicDysmorphisimPresent"]));
                         BizAction.Details.ExtracytoplasmicDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicDysmorphisimAbsent"]));
                         BizAction.Details.OocyteCoronaCumulusComplexPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplexPresent"]));
                         BizAction.Details.OocyteCoronaCumulusComplexAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplexAbsent"]));

                         BizAction.Details.TriggerDate = (DateTime?)(DALHelper.HandleDate(reader["TriggerDate"]));
                         BizAction.Details.TriggerTime = (DateTime?)(DALHelper.HandleDate(reader["TriggerTime"]));
                         BizAction.Details.IsPreAnesthesia = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPreAnesthesia"]));
                         BizAction.Details.AnesthesiaID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaID"]));
                         BizAction.Details.NeedleUsed = Convert.ToInt64(DALHelper.HandleDBNull(reader["NeedleUsed"]));
                         BizAction.Details.TypeOfNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TypeOfNeedleID"]));
                         BizAction.Details.IsFlushing = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFlushing"]));
                         BizAction.Details.IsCycleCancellation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCycleCancellation"]));
                         BizAction.Details.Reasons = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"]));
                         BizAction.Details.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                         BizAction.Details.LeftFollicule = Convert.ToString(DALHelper.HandleDBNull(reader["LeftFollicule"]));
                         BizAction.Details.RightFollicule = Convert.ToString(DALHelper.HandleDBNull(reader["RightFollicule"]));



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

        //added by neena dated 28/7/16
        public override IValueObject AddUpdateOocyteNumber(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateOocyteNumberBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateOocyteNumberBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateOocyteNumber");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.OPUDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.OPUDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OPUDetails.PlanTherapyUnitID);              
                dbServer.AddInParameter(command, "OocyteRetrived", DbType.Int64, BizActionObj.OPUDetails.OocyteRetrived);
               
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.OPUDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //BizActionObj.OPUDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.OPUDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }


        public override IValueObject GetEmbryologySummary(IValueObject valueObject, clsUserVO UserVo)
        {   
            clsIVFDashboard_GetEmbryologySummaryBizActionVO BizAction = valueObject as clsIVFDashboard_GetEmbryologySummaryBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetEmbryologySummary");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.EmbSummary.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.EmbSummary.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.EmbSummary.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.EmbSummary.PatientUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.EmbSummary.NoOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOocyte"]));
                        BizAction.EmbSummary.MI = Convert.ToInt64(DALHelper.HandleDBNull(reader["MI"]));
                        BizAction.EmbSummary.MII = Convert.ToInt64(DALHelper.HandleDBNull(reader["MII"]));
                        BizAction.EmbSummary.GV = Convert.ToInt64(DALHelper.HandleDBNull(reader["GV"]));
                        BizAction.EmbSummary.OocyteCytoplasmDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmPresent"]));
                        BizAction.EmbSummary.OocyteCytoplasmDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmAbsent"]));
                        BizAction.EmbSummary.ExtracytoplasmicDysmorphisimPresent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicPresent"]));
                        BizAction.EmbSummary.ExtracytoplasmicDysmorphisimAbsent = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicAbsent"]));
                        BizAction.EmbSummary.OocyteCoronaCumulusComplexNormal = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaNormal"]));
                        BizAction.EmbSummary.OocyteCoronaCumulusComplexAbnormal = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaAbnormal"]));
                        //BizAction.EmbSummary.TriggerDate = (DateTime?)(DALHelper.HandleDate(reader["TriggerDate"]));
                        //BizAction.EmbSummary.TriggerTime = (DateTime?)(DALHelper.HandleDate(reader["TriggerTime"]));
                        BizAction.EmbSummary.OPUDate = (DateTime?)(DALHelper.HandleDate(reader["OPUDate"]));
                        BizAction.EmbSummary.OPUTime = (DateTime?)(DALHelper.HandleDate(reader["OPUTime"]));
                        BizAction.EmbSummary.FreshPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreshPGDPGS"]));
                        BizAction.EmbSummary.FrozenPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrozenPGDPGS"]));
                        BizAction.EmbSummary.ThawedPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawedPGDPGS"]));
                        BizAction.EmbSummary.PostThawedPGDPGS = Convert.ToInt64(DALHelper.HandleDBNull(reader["PostThawedPGDPGS"]));
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
        //
       
    }
}
