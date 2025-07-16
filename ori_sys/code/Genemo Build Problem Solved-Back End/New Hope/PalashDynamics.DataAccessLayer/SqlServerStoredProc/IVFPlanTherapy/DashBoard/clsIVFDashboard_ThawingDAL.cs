namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using System;
    using System.Data;
    using System.Data.Common;

    internal class clsIVFDashboard_ThawingDAL : clsBaseIVFDashboard_ThawingDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFDashboard_ThawingDAL()
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

        public override IValueObject AddUpdateIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateThawingBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateThawingBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.Thawing.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, nvo.Thawing.LabPersonId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Thawing.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Thawing.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.ThawingObj != null)
                {
                    clsIVFDashBoard_ThawingDetailsVO thawingObj = nvo.ThawingObj;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, thawingObj.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, nvo.Thawing.ID);
                    this.dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                    this.dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, thawingObj.DateTime);
                    this.dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, thawingObj.CellStageID);
                    this.dbServer.AddInParameter(command2, "GradeID", DbType.Int64, thawingObj.GradeID);
                    this.dbServer.AddInParameter(command2, "NextPlan", DbType.Int64, thawingObj.PostThawingPlanID);
                    this.dbServer.AddInParameter(command2, "Comments", DbType.String, thawingObj.Comments);
                    this.dbServer.AddInParameter(command2, "Status", DbType.String, thawingObj.EmbStatus);
                    this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, thawingObj.OocyteDonorID);
                    this.dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, thawingObj.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command2, "TransferDay", DbType.Int64, thawingObj.TransferDayNo);
                    this.dbServer.AddInParameter(command2, "StageOfDevelopmentGradeID", DbType.Int64, thawingObj.StageOfDevelopmentGradeID);
                    this.dbServer.AddInParameter(command2, "InnerCellMassGradeID", DbType.Int64, thawingObj.InnerCellMassGradeID);
                    this.dbServer.AddInParameter(command2, "TrophoectodermGradeID", DbType.Int64, thawingObj.TrophoectodermGradeID);
                    this.dbServer.AddInParameter(command2, "LabPersonId", DbType.Int64, nvo.Thawing.LabPersonId);
                    this.dbServer.AddInParameter(command2, "CellStage", DbType.String, thawingObj.CellStage);
                    this.dbServer.AddInParameter(command2, "DateTime", DbType.DateTime, nvo.Thawing.DateTime);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "IsFreshEmbryoPGDPGS", DbType.Boolean, thawingObj.IsFreshEmbryoPGDPGS);
                    this.dbServer.AddInParameter(command2, "IsFrozenEmbryoPGDPGS", DbType.Boolean, thawingObj.IsFrozenEmbryoPGDPGS);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    if (thawingObj.PostThawingPlanID == 2L)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                        this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                        this.dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                        this.dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                        this.dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                        this.dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                        this.dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                        this.dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                        this.dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                        this.dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddInParameter(command3, "IsAnesthesia", DbType.Boolean, false);
                        this.dbServer.AddInParameter(command3, "FreshEmb", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "FrozenEmb", DbType.Int64, null);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        long parameterValue = (long) this.dbServer.GetParameterValue(command3, "ID");
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                        this.dbServer.AddInParameter(command4, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "ETID", DbType.Int64, parameterValue);
                        this.dbServer.AddInParameter(command4, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "OocyteNumber", DbType.Int64, thawingObj.EmbNumber);
                        this.dbServer.AddInParameter(command4, "SerialOocyteNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.AddInParameter(command4, "TransferDate", DbType.DateTime, thawingObj.DateTime);
                        if (thawingObj.TransferDayNo == 1L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 1");
                        }
                        else if (thawingObj.TransferDayNo == 2L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 2");
                        }
                        else if (thawingObj.TransferDayNo == 3L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 3");
                        }
                        else if (thawingObj.TransferDayNo == 4L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 4");
                        }
                        else if (thawingObj.TransferDayNo == 5L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 5");
                        }
                        else if (thawingObj.TransferDayNo == 6L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 6");
                        }
                        else if (thawingObj.TransferDayNo == 7L)
                        {
                            this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, "Day 7");
                        }
                        this.dbServer.AddInParameter(command4, "GradeID", DbType.Int64, thawingObj.GradeID);
                        this.dbServer.AddInParameter(command4, "Score", DbType.String, null);
                        this.dbServer.AddInParameter(command4, "FertStageID", DbType.Int64, thawingObj.CellStageID);
                        this.dbServer.AddInParameter(command4, "EmbStatus", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command4, "FileName", DbType.String, null);
                        this.dbServer.AddInParameter(command4, "FileContents", DbType.Binary, null);
                        this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, thawingObj.OocyteDonorID);
                        this.dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, thawingObj.OocyteDonorUnitID);
                        this.dbServer.AddInParameter(command4, "EmbTransferDay", DbType.Int64, thawingObj.TransferDayNo);
                        this.dbServer.AddInParameter(command4, "Remark", DbType.String, null);
                        this.dbServer.AddInParameter(command4, "GradeID_New", DbType.Int64, thawingObj.GradeID);
                        this.dbServer.AddInParameter(command4, "StageofDevelopmentGrade", DbType.Int64, thawingObj.StageOfDevelopmentGradeID);
                        this.dbServer.AddInParameter(command4, "InnerCellMassGrade", DbType.Int64, thawingObj.InnerCellMassGradeID);
                        this.dbServer.AddInParameter(command4, "TrophoectodermGrade", DbType.Int64, thawingObj.TrophoectodermGradeID);
                        this.dbServer.AddInParameter(command4, "CellStage", DbType.String, thawingObj.CellStage);
                        this.dbServer.AddInParameter(command4, "IsFreshEmbryoPGDPGS", DbType.Boolean, thawingObj.IsFreshEmbryoPGDPGS);
                        this.dbServer.AddInParameter(command4, "IsFrozenEmbryo", DbType.Boolean, thawingObj.IsFrozenEmbryo);
                        this.dbServer.AddInParameter(command4, "IsFromThawing", DbType.Boolean, true);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetailsForThawing");
                        this.dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, nvo.Thawing.UsedByOtherCycle);
                        this.dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, nvo.Thawing.UsedTherapyID);
                        this.dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, nvo.Thawing.UsedTherapyUnitID);
                        this.dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, nvo.Thawing.DateTime);
                        this.dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                    }
                    if (thawingObj.PostThawingPlanID == 3L)
                    {
                        DbCommand command6 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForEmbryoRefeezeNew");
                        this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command6, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                        this.dbServer.AddInParameter(command6, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                        this.dbServer.AddInParameter(command6, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                        this.dbServer.AddInParameter(command6, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command6, "DateTime", DbType.DateTime, null);
                        this.dbServer.AddInParameter(command6, "VitrificationNo", DbType.String, null);
                        this.dbServer.AddInParameter(command6, "PickUpDate", DbType.DateTime, null);
                        this.dbServer.AddInParameter(command6, "ConsentForm", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command6, "IsFreezed", DbType.Boolean, false);
                        this.dbServer.AddInParameter(command6, "IsOnlyVitrification", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command6, "SrcOoctyID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command6, "SrcSemenID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command6, "SrcOoctyCode", DbType.String, null);
                        this.dbServer.AddInParameter(command6, "SrcSemenCode", DbType.String, null);
                        this.dbServer.AddInParameter(command6, "UsedOwnOocyte", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command6, "FromForm", DbType.Int64, 0);
                        this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddInParameter(command6, "Refreeze", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command6, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                        this.dbServer.AddInParameter(command6, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.AddInParameter(command6, "TransferDay", DbType.Int64, thawingObj.TransferDayNo);
                        this.dbServer.ExecuteNonQuery(command6, transaction);
                        nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command6, "ResultStatus"));
                        nvo.Thawing.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command6, "ID"));
                    }
                    if (thawingObj.PostThawingPlanID == 4L)
                    {
                        DbCommand command7 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddExtendedCultureForEmbryo");
                        this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command7, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                        this.dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                        this.dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                        this.dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command7, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                        this.dbServer.AddInParameter(command7, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                    }
                    if (thawingObj.PostThawingPlanID == 7L)
                    {
                        DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddPGDPGSForEmbryo");
                        this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command8, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                        this.dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                        this.dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                        this.dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command8, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                        this.dbServer.AddInParameter(command8, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(command8, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Thawing = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateIVFDashBoard_ThawingOocyte(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateThawingBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateThawingBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.Thawing.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, nvo.Thawing.LabPersonId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Thawing.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Thawing.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.ThawingObj != null)
                {
                    clsIVFDashBoard_ThawingDetailsVO thawingObj = nvo.ThawingObj;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, thawingObj.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, nvo.Thawing.ID);
                    this.dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                    this.dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, thawingObj.DateTime);
                    this.dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, thawingObj.CellStageID);
                    this.dbServer.AddInParameter(command2, "GradeID", DbType.Int64, thawingObj.GradeID);
                    this.dbServer.AddInParameter(command2, "NextPlan", DbType.Int64, thawingObj.PostThawingPlanID);
                    this.dbServer.AddInParameter(command2, "Comments", DbType.String, thawingObj.Comments);
                    this.dbServer.AddInParameter(command2, "Status", DbType.String, thawingObj.EmbStatus);
                    this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, thawingObj.OocyteDonorID);
                    this.dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, thawingObj.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command2, "TransferDay", DbType.Int64, thawingObj.TransferDayNo);
                    this.dbServer.AddInParameter(command2, "StageOfDevelopmentGradeID", DbType.Int64, thawingObj.StageOfDevelopmentGradeID);
                    this.dbServer.AddInParameter(command2, "InnerCellMassGradeID", DbType.Int64, thawingObj.InnerCellMassGradeID);
                    this.dbServer.AddInParameter(command2, "TrophoectodermGradeID", DbType.Int64, thawingObj.TrophoectodermGradeID);
                    this.dbServer.AddInParameter(command2, "LabPersonId", DbType.Int64, nvo.Thawing.LabPersonId);
                    this.dbServer.AddInParameter(command2, "CellStage", DbType.String, thawingObj.CellStage);
                    this.dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Int64, thawingObj.IsFreezeOocytes);
                    this.dbServer.AddInParameter(command2, "DateTime", DbType.DateTime, nvo.Thawing.DateTime);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    if (thawingObj.PostThawingPlanID == 3L)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForOocyteRefeezeNew");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                        this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                        this.dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                        this.dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, null);
                        this.dbServer.AddInParameter(command3, "VitrificationNo", DbType.String, null);
                        this.dbServer.AddInParameter(command3, "PickUpDate", DbType.DateTime, null);
                        this.dbServer.AddInParameter(command3, "ConsentForm", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                        this.dbServer.AddInParameter(command3, "IsOnlyVitrification", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                        this.dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                        this.dbServer.AddInParameter(command3, "UsedOwnOocyte", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 0);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddInParameter(command3, "Refreeze", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command3, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                        this.dbServer.AddInParameter(command3, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command3, "ResultStatus"));
                        nvo.Thawing.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command3, "ID"));
                    }
                    if (thawingObj.PostThawingPlanID == 6L)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddExtendedCultureForOocyte");
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                        this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                        this.dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                        this.dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, thawingObj.EmbNumber);
                        this.dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, thawingObj.EmbSerialNumber);
                        this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Thawing = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateIVFDashBoard_ThawingWOCryo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.Thawing.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, nvo.Thawing.LabPersonId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Thawing.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Thawing.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.ThawingDetailsList != null) && (nvo.ThawingDetailsList.Count > 0))
                {
                    foreach (clsIVFDashBoard_ThawingDetailsVO svo in nvo.ThawingDetailsList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, nvo.Thawing.ID);
                        this.dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, svo.EmbNumber);
                        this.dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, svo.EmbSerialNumber);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, svo.DateTime);
                        this.dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, svo.CellStageID);
                        this.dbServer.AddInParameter(command2, "GradeID", DbType.Int64, svo.GradeID);
                        this.dbServer.AddInParameter(command2, "NextPlan", DbType.Boolean, svo.Plan);
                        this.dbServer.AddInParameter(command2, "Comments", DbType.String, svo.Comments);
                        this.dbServer.AddInParameter(command2, "Status", DbType.String, svo.EmbStatus);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, svo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, svo.OocyteDonorUnitID);
                        this.dbServer.AddInParameter(command2, "TransferDay", DbType.Int64, svo.TransferDayNo);
                        this.dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Int64, nvo.IsFreezeOocytes);
                        this.dbServer.AddInParameter(command2, "StageOfDevelopmentGradeID", DbType.Int64, svo.StageOfDevelopmentGradeID);
                        this.dbServer.AddInParameter(command2, "InnerCellMassGradeID", DbType.Int64, svo.InnerCellMassGradeID);
                        this.dbServer.AddInParameter(command2, "TrophoectodermGradeID", DbType.Int64, svo.TrophoectodermGradeID);
                        this.dbServer.AddInParameter(command2, "CellStage", DbType.String, svo.CellStage);
                        this.dbServer.AddInParameter(command2, "CycleCode", DbType.String, svo.CycleCode);
                        this.dbServer.AddInParameter(command2, "IsFreshEmbryoPGDPGS", DbType.Boolean, svo.IsFreshEmbryoPGDPGS);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetailsForThawing");
                        this.dbServer.AddInParameter(command3, "UsedByOtherCycle", DbType.Boolean, nvo.Thawing.UsedByOtherCycle);
                        this.dbServer.AddInParameter(command3, "UsedTherapyID", DbType.Int64, nvo.Thawing.UsedTherapyID);
                        this.dbServer.AddInParameter(command3, "UsedTherapyUnitID", DbType.Int64, nvo.Thawing.UsedTherapyUnitID);
                        this.dbServer.AddInParameter(command3, "ReceivingDate", DbType.DateTime, nvo.Thawing.DateTime);
                        this.dbServer.AddInParameter(command3, "EmbSerialNumber", DbType.Int64, svo.EmbSerialNumber);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Thawing = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetThawingBizActionVO nvo = valueObject as clsIVFDashboard_GetThawingBizActionVO;
            try
            {
                DbCommand command = !nvo.IsOnlyForEmbryoThawing ? this.dbServer.GetStoredProcCommand("IVFDashboard_GetThawingDetailsOocyte") : this.dbServer.GetStoredProcCommand("IVFDashboard_GetThawingDetails");
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Thawing.PatientID);
                this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Thawing.PatientUnitID);
                this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Thawing.PlanTherapyID);
                this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Thawing.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Thawing.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Thawing.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Thawing.DateTime = DALHelper.HandleDate(reader["DateTime"]);
                        nvo.Thawing.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        nvo.Thawing.IsETFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsETFreezed"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_ThawingDetailsVO item = new clsIVFDashBoard_ThawingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ThawingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawingID"])),
                            ThawingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawingUnitID"])),
                            CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"])),
                            DateTime = DALHelper.HandleDate(reader["Date"]),
                            Plan = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Plan"])),
                            GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["Status"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"])),
                            OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"])),
                            StageOfDevelopmentGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageOfDevelopmentGradeID"])),
                            InnerCellMassGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeID"])),
                            TrophoectodermGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeID"])),
                            TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDay"])),
                            PostThawingPlanID = Convert.ToInt32(DALHelper.HandleDBNull(reader["Plan"])),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"])),
                            IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"])),
                            IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]))
                        };
                        nvo.ThawingDetailsList.Add(item);
                    }
                }
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

