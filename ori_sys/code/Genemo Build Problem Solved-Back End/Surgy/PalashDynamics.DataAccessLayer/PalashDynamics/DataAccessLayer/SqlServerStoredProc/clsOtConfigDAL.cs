namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.OTConfiguration;
    using PalashDynamics.ValueObjects.EMR;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsOtConfigDAL : clsBaseOtConfigDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsOtConfigDAL()
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
                this.ImgIP = ConfigurationManager.AppSettings["RegImgIP"];
                this.ImgVirtualDir = ConfigurationManager.AppSettings["RegImgVirtualDir"];
                this.ImgSaveLocation = ConfigurationManager.AppSettings["ImgSavingLocation"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddCommonParametersForAddUpdateParameter(DbCommand command, clsInstructionMasterVO objInstruction, clsUserVO objUserVO)
        {
            try
            {
                this.dbServer.AddInParameter(command, "Code", DbType.String, objInstruction.Code);
                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command, "Description", DbType.String, objInstruction.Description);
                this.dbServer.AddInParameter(command, "InstructionType", DbType.Int64, objInstruction.SelectInstruction.ID);
                this.dbServer.AddInParameter(command, "Status", DbType.Boolean, objInstruction.Status);
                this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private clsAddOTScheduleMasterBizActionVO AddOtSchedule(clsAddOTScheduleMasterBizActionVO BizAction, clsUserVO UserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsOTScheduleVO oTScheduleDetails = BizAction.OTScheduleDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddOTSchedule");
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, oTScheduleDetails.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, oTScheduleDetails.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, oTScheduleDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizAction.OTScheduleDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteOTScheduleDetails");
                this.dbServer.AddInParameter(command2, "OTScheduleID", DbType.Int64, oTScheduleDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                foreach (clsOTScheduleDetailsVO svo in oTScheduleDetails.OTScheduleDetailsList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddOTScheduleDetails");
                    this.dbServer.AddInParameter(command3, "OTScheduleID", DbType.Int64, oTScheduleDetails.ID);
                    this.dbServer.AddInParameter(command3, "DayID", DbType.Int64, svo.DayID);
                    this.dbServer.AddInParameter(command3, "ScheduleID", DbType.Int64, svo.ScheduleID);
                    this.dbServer.AddInParameter(command3, "StartTime", DbType.DateTime, svo.StartTime);
                    this.dbServer.AddInParameter(command3, "EndTime", DbType.DateTime, svo.EndTime);
                    this.dbServer.AddInParameter(command3, "ApplyToAllDay", DbType.Boolean, svo.ApplyToAllDay);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                }
                transaction.Commit();
                BizAction.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizAction.SuccessStatus = -1;
                transaction.Rollback();
                BizAction.OTScheduleDetails = null;
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return BizAction;
        }

        public override IValueObject AddOTScheduleMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddOTScheduleMasterBizActionVO bizAction = valueObject as clsAddOTScheduleMasterBizActionVO;
            return this.AddOtSchedule(bizAction, objUserVO);
        }

        public override IValueObject AddProcedureMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddProcedureMasterBizActionVO nvo = valueObject as clsAddProcedureMasterBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureMaster");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.ProcDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ProcDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.ProcDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ProcDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "Duration", DbType.String, nvo.ProcDetails.Duration);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureTypeID", DbType.Int64, nvo.ProcDetails.ProcedureTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "RecommandedAnesthesiaTypeID", DbType.Int64, nvo.ProcDetails.RecommandedAnesthesiaTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "OperationTheatreID", DbType.Int64, nvo.ProcDetails.OperationTheatreID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, nvo.ProcDetails.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.ProcDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ProcDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.ProcDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                DbCommand command2 = null;
                command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteProcedureDetails");
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                foreach (clsProcedureConsentDetailsVO svo in nvo.ProcDetails.ConsentList)
                {
                    DbCommand command3 = null;
                    command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureConsentDetails");
                    command3.Connection = connection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command3, "ConsentID", DbType.Int64, svo.ConsentID);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
                foreach (clsProcedureInstructionDetailsVO svo2 in nvo.ProcDetails.InstructionList)
                {
                    DbCommand command4 = null;
                    command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureInstructionDetails");
                    command4.Connection = connection;
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command4, "InstructionID", DbType.Int64, svo2.InstructionID);
                    this.dbServer.AddInParameter(command4, "InstructionTypeID", DbType.Int64, svo2.InstructionTypeID);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                }
                foreach (clsProcedureItemDetailsVO svo3 in nvo.ProcDetails.ItemList)
                {
                    DbCommand command5 = null;
                    command5 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureItemDetails");
                    command5.Connection = connection;
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command5, "ItemID", DbType.Int64, svo3.ItemID);
                    this.dbServer.AddInParameter(command5, "ItemCode", DbType.String, svo3.ItemCode);
                    this.dbServer.AddInParameter(command5, "ItemName", DbType.String, svo3.ItemName);
                    this.dbServer.AddInParameter(command5, "Quantity", DbType.Double, svo3.Quantity);
                    this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo3.ID);
                    this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                }
                foreach (clsDoctorSuggestedServiceDetailVO lvo in nvo.ProcDetails.ServiceList)
                {
                    DbCommand command6 = null;
                    command6 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureServiceDetails");
                    command6.Connection = connection;
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command6, "ServiceCode", DbType.String, lvo.ServiceCode);
                    this.dbServer.AddInParameter(command6, "ServiceName", DbType.String, lvo.ServiceName);
                    this.dbServer.AddInParameter(command6, "ServiceType", DbType.String, lvo.ServiceType);
                    this.dbServer.AddInParameter(command6, "GroupName", DbType.String, lvo.GroupName);
                    this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                foreach (clsProcedureStaffDetailsVO svo4 in nvo.ProcDetails.StaffList)
                {
                    DbCommand command7 = null;
                    command7 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureStaffDetails");
                    command7.Connection = connection;
                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command7, "IsDoctor", DbType.Boolean, svo4.IsDoctor);
                    this.dbServer.AddInParameter(command7, "DocOrStaffCode", DbType.String, svo4.DocOrStaffCode);
                    this.dbServer.AddInParameter(command7, "NoofDocOrStaff", DbType.Int64, svo4.NoofDocOrStaff);
                    this.dbServer.AddInParameter(command7, "DocSpecialization", DbType.String, svo4.DocClassification);
                    this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo4.ID);
                    this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                }
                foreach (clsProcedureChecklistDetailsVO svo5 in nvo.ProcDetails.CheckList)
                {
                    DbCommand command8 = null;
                    command8 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcChecklistUsingProcedureMaster");
                    command8.Connection = connection;
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command8, "ChecklistID", DbType.Int64, svo5.CheckListId);
                    this.dbServer.AddInParameter(command8, "ChecklistUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "Remark", DbType.String, svo5.Remark);
                    this.dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo5.ID);
                    this.dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command8, "ResultStatus");
                }
                foreach (clsProcedureTemplateDetailsVO svo6 in nvo.ProcDetails.ProcedureTempalateList)
                {
                    DbCommand command9 = null;
                    command9 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureTempateDetails");
                    command9.Connection = connection;
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "ProcedureID", DbType.Int64, nvo.ProcDetails.ID);
                    this.dbServer.AddInParameter(command9, "TemplateID", DbType.Int64, svo6.TemplateID);
                    this.dbServer.AddInParameter(command9, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command9, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command9, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command9, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo6.ID);
                    this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateConsentMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsConsentMasterVO rvo = new clsConsentMasterVO();
            clsAddUpdateConsentMasterBizActionVO nvo = valueObject as clsAddUpdateConsentMasterBizActionVO;
            try
            {
                rvo = nvo.OTTableMasterMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateConsentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateName", DbType.String, rvo.TemplateName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateInstructionDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateInstructionDetailsBizActionVO nvo = valueObject as clsAddUpdateInstructionDetailsBizActionVO;
            clsInstructionMasterVO instMaster = nvo.InstMaster;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateInstructionMaster");
                if (nvo.InstMaster.ID == 0L)
                {
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, instMaster.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.InstMaster.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.InstMaster.Description);
                this.dbServer.AddInParameter(storedProcCommand, "InstructionType", DbType.Int64, nvo.InstMaster.SelectInstruction.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateOtTableDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsOTTableVO evo = new clsOTTableVO();
            clsAddUpdateOTTableDetailsBizActionVO nvo = valueObject as clsAddUpdateOTTableDetailsBizActionVO;
            try
            {
                evo = nvo.OTTableMasterMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTTable");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, evo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, evo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTheatreId", DbType.Int64, evo.OTTheatreID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, evo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, evo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, evo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, evo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, evo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, evo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, evo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, evo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, evo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, evo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, evo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddupdatePatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddupdatePatientProcedureSchedulebizActionVO nvo = valueObject as clsAddupdatePatientProcedureSchedulebizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            DbCommand storedProcCommand = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcedureScheduleMaster");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.patientProcScheduleDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.patientProcScheduleDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmID", DbType.Int64, nvo.patientProcScheduleDetails.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmUnitID", DbType.Int64, nvo.patientProcScheduleDetails.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd", DbType.Int64, nvo.patientProcScheduleDetails.Opd_Ipd);
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, nvo.patientProcScheduleDetails.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "OperationTheatreID", DbType.Int64, nvo.patientProcScheduleDetails.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "OperationTheatreUnitID", DbType.Int64, nvo.patientProcScheduleDetails.OTUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.patientProcScheduleDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "StartTime", DbType.DateTime, nvo.patientProcScheduleDetails.StartTime);
                this.dbServer.AddInParameter(storedProcCommand, "EndTime", DbType.DateTime, nvo.patientProcScheduleDetails.EndTime);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialRequirement", DbType.String, nvo.patientProcScheduleDetails.SpecialRequirement);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, nvo.patientProcScheduleDetails.Remarks);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.patientProcScheduleDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.patientProcScheduleDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.patientProcScheduleDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.patientProcScheduleDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsPatientProcedureVO evo in nvo.patientProcScheduleDetails.PatientProcList)
                {
                     storedProcCommand = null;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcdure");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, evo.ProcedureID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcedureScheduleID", DbType.Int64, nvo.patientProcScheduleDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcedureScheduleUnitID", DbType.Int64, nvo.patientProcScheduleDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                foreach (clsPatientProcDocScheduleDetailsVO svo in nvo.patientProcScheduleDetails.DocScheduleList)
                {
                    storedProcCommand = null;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcDocSchedule");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcedureScheduleID", DbType.Int64, nvo.patientProcScheduleDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcedureScheduleUnitID", DbType.Int64, nvo.patientProcScheduleDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DocTypeID", DbType.Int64, svo.DocTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApplyToAllDay", DbType.Boolean, svo.ApplyToAllDay);
                    this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, svo.ScheduleID);
                    this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, svo.ProcedureID);
                    this.dbServer.AddInParameter(storedProcCommand, "DocID", DbType.Int64, svo.DocID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, svo.DoctorCode);
                    this.dbServer.AddInParameter(storedProcCommand, "DayID", DbType.Int64, svo.DayID);
                    this.dbServer.AddInParameter(storedProcCommand, "StartTime", DbType.DateTime, svo.StartTime);
                    this.dbServer.AddInParameter(storedProcCommand, "EndTime", DbType.DateTime, svo.EndTime);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.patientProcScheduleDetails.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                foreach (clsPatientProcStaffDetailsVO svo2 in nvo.patientProcScheduleDetails.StaffList)
                {
                    storedProcCommand = null;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcStaffDetails");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcedureScheduleID", DbType.Int64, nvo.patientProcScheduleDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcedureScheduleUnitID", DbType.Int64, nvo.patientProcScheduleDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, svo2.ProcedureID);
                    this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, svo2.DesignationID);
                    this.dbServer.AddInParameter(storedProcCommand, "StaffID", DbType.Int64, svo2.StaffID);
                    this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, svo2.Quantity);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                foreach (clsPatientProcedureChecklistDetailsVO svo3 in nvo.patientProcScheduleDetails.PatientProcCheckList)
                {
                    storedProcCommand = null;
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientProcedureChecklistDetails");
                    storedProcCommand.Connection = connection;
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcScheduleID", DbType.Int64, nvo.patientProcScheduleDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientProcScheduleUnitID", DbType.Int64, nvo.patientProcScheduleDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Category", DbType.String, svo3.Category);
                    this.dbServer.AddInParameter(storedProcCommand, "SubCategory1", DbType.String, svo3.SubCategory1);
                    this.dbServer.AddInParameter(storedProcCommand, "SubCategory2", DbType.String, svo3.SubCategory2);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, svo3.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, svo3.Name);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo3.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateProcedureCheckList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsProcedureCheckListVO tvo = new clsProcedureCheckListVO();
            clsAddUpdateProcedureCheckListBizActionVO nvo = valueObject as clsAddUpdateProcedureCheckListBizActionVO;
            try
            {
                tvo = nvo.CheckList[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcedureCheckList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, tvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, tvo.CategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "SubCategoryID", DbType.Int64, tvo.SubCategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tvo.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.CheckListDetails.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateProcedureSubCategory(IValueObject valueObject, clsUserVO UserVO)
        {
            clsProcedureSubCategoryVO yvo = new clsProcedureSubCategoryVO();
            clsAddUpdateProcedureSubCategoryBizActionVO nvo = valueObject as clsAddUpdateProcedureSubCategoryBizActionVO;
            try
            {
                yvo = nvo.SubCategoryList[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateProcSubCategory");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, yvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, yvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureCategoryID", DbType.Int64, yvo.CategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, yvo.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, yvo.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, yvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, yvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, yvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, yvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, yvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, yvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, yvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, yvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, yvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SubCategoryDetails.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject CancelOTBooking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCancelOTBookingBizActionVO nvo = (clsCancelOTBookingBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CancelOTBooking1");
                this.dbServer.AddInParameter(storedProcCommand, "PatientProcScheduleID", DbType.Int64, nvo.patientProcScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "CancelledReason", DbType.String, nvo.CancelledReason);
                this.dbServer.AddInParameter(storedProcCommand, "CancelledBy", DbType.Int64, nvo.CancelledBy);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CancelledDate", DbType.DateTime, DateTime.Now);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject CheckOTScheduleExistance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckTimeForOTScheduleExistanceBizActionVO nvo = valueObject as clsCheckTimeForOTScheduleExistanceBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckOTScheduleExistance");
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, nvo.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, nvo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "StartTime", DbType.DateTime, nvo.StartTime);
                this.dbServer.AddInParameter(storedProcCommand, "EndTime", DbType.DateTime, nvo.EndTime);
                this.dbServer.AddInParameter(storedProcCommand, "DayID", DbType.Int64, nvo.DayID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.IsSchedulePresent = new bool?((bool) DALHelper.HandleDBNull(reader["IsSchedulePresent"]));
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

        public override IValueObject GetCheckListByProcedureID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCheckListByProcedureIDBizActionVO nvo = (clsGetCheckListByProcedureIDBizActionVO) valueObject;
            List<clsPatientProcedureChecklistDetailsVO> list = new List<clsPatientProcedureChecklistDetailsVO>();
            try
            {
                nvo.ChecklistDetails = new List<clsPatientProcedureChecklistDetailsVO>();
                for (int i = 0; i < nvo.procedureIDList.Count; i++)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCheckListByProcedureID");
                    this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, nvo.procedureIDList[i]);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsPatientProcedureChecklistDetailsVO item = new clsPatientProcedureChecklistDetailsVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                                CategoryID = (long) DALHelper.HandleDBNull(reader["CategoryID"]),
                                ChecklistID = (long) DALHelper.HandleDBNull(reader["ChecklistID"]),
                                SubCategoryID = (long) DALHelper.HandleDBNull(reader["SubCategoryID"]),
                                Remarks = (string) DALHelper.HandleDBNull(reader["Remark"]),
                                Name = (string) DALHelper.HandleDBNull(reader["Name"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                ProcedureID = (long) DALHelper.HandleDBNull(reader["ProcedureID"])
                            };
                            list.Add(item);
                        }
                    }
                    using (List<clsPatientProcedureChecklistDetailsVO>.Enumerator enumerator = list.GetEnumerator())
                    {
                        Func<clsPatientProcedureChecklistDetailsVO, bool> predicate = null;
                        while (enumerator.MoveNext())
                        {
                            clsPatientProcedureChecklistDetailsVO item = enumerator.Current;
                            if (predicate == null)
                            {
                                predicate = checkItem => checkItem.ChecklistID == item.ChecklistID;
                            }
                            if (!nvo.ChecklistDetails.Where<clsPatientProcedureChecklistDetailsVO>(predicate).Any<clsPatientProcedureChecklistDetailsVO>())
                            {
                                clsPatientProcedureChecklistDetailsVO svo2 = new clsPatientProcedureChecklistDetailsVO {
                                    ID = item.ID,
                                    UnitID = item.UnitID,
                                    CategoryID = item.CategoryID,
                                    ChecklistID = item.ChecklistID,
                                    SubCategoryID = item.SubCategoryID,
                                    Remarks = item.Remarks,
                                    Name = item.Name,
                                    Status = item.Status,
                                    ProcedureID = item.ProcedureID
                                };
                                nvo.ChecklistDetails.Add(svo2);
                            }
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetCheckListByScheduleID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCheckListByScheduleIDBizActionVO nvo = (clsGetCheckListByScheduleIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCheckListByScheduleID");
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, nvo.ScheduleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ChecklistDetails == null)
                    {
                        nvo.ChecklistDetails = new List<clsPatientProcedureChecklistDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientProcedureChecklistDetailsVO item = new clsPatientProcedureChecklistDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Category = (string) DALHelper.HandleDBNull(reader["Category"]),
                            PatientProcScheduleID = (long) DALHelper.HandleDBNull(reader["PatientProcScheduleID"]),
                            PatientProcScheduleUnitID = (long) DALHelper.HandleDBNull(reader["PatientProcScheduleUnitID"]),
                            SubCategory1 = (string) DALHelper.HandleDBNull(reader["SubCategory1"]),
                            SubCategory2 = (string) DALHelper.HandleDBNull(reader["SubCategory2"]),
                            Remarks = (string) DALHelper.HandleDBNull(reader["Remarks"]),
                            Name = (string) DALHelper.HandleDBNull(reader["Name"])
                        };
                        nvo.ChecklistDetails.Add(item);
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

        public override IValueObject GetConfig_Patient_Fields(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetPatientConfigFieldsBizActionVO nvo = valueObject as clsGetPatientConfigFieldsBizActionVO;
            clsPatientFieldsConfigVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConfig_Patient_Fields");
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsPatientFieldsConfigVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            TableName = Convert.ToString(DALHelper.HandleDBNull(reader["TableName"])),
                            FieldName = Convert.ToString(DALHelper.HandleDBNull(reader["FieldName"])),
                            FieldColumn = Convert.ToString(DALHelper.HandleDBNull(reader["FieldColumn"]))
                        };
                        nvo.OtPateintConfigFieldsMatserDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetConsentByConsentType(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsentByConsentTypeBizActionVO nvo = (clsGetConsentByConsentTypeBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConsentByConsentTypeID");
                this.dbServer.AddInParameter(storedProcCommand, "ConsentTypeID", DbType.Int64, nvo.ConsentTypeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentList == null)
                    {
                        nvo.ConsentList = new List<clsConsentDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsConsentDetailsVO item = new clsConsentDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TemplateName = (string) DALHelper.HandleDBNull(reader["Template"])
                        };
                        nvo.ConsentList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetConsentForProcedureID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsentForProcedureIDBizActionVO nvo = (clsGetConsentForProcedureIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConsentForProcedureID");
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, nvo.ProcedureID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentList == null)
                    {
                        nvo.ConsentList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.ConsentList.Add(item);
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

        public override IValueObject GetConsentMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetConsentMasterBizActionVO nvo = valueObject as clsGetConsentMasterBizActionVO;
            clsConsentMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConsentMaster");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsConsentMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]))
                        };
                        nvo.ConsentMatserDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDeleteConsents(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteConsentBizActionVO nvo = (clsDeleteConsentBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteConsentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.Consent.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Consent.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentId", DbType.Int64, nvo.Consent.ConsentID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDocScheduleList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDocScheduleByDocIDBizActionVO nvo = (clsGetDocScheduleByDocIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDocScheduleListByDocIDIDAndDate");
                this.dbServer.AddInParameter(storedProcCommand, "DocID", DbType.Int64, nvo.DocID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DocTypeID", DbType.Int64, nvo.DocTableID);
                this.dbServer.AddInParameter(storedProcCommand, "ProcDate", DbType.DateTime, nvo.procDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DocScheduleListDetails == null)
                    {
                        nvo.DocScheduleListDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientProcDocScheduleDetailsVO item = new clsPatientProcDocScheduleDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            DocID = (long) DALHelper.HandleDBNull(reader["DocID"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = new DateTime?(nullable.Value);
                        DateTime? nullable2 = DALHelper.HandleDate(reader["FromTime"]);
                        item.StartTime = nullable2.Value;
                        DateTime? nullable3 = DALHelper.HandleDate(reader["ToTime"]);
                        item.EndTime = nullable3.Value;
                        item.DocTypeID = (long) DALHelper.HandleDBNull(reader["DocTypeID"]);
                        nvo.DocScheduleListDetails.Add(item);
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

        public override IValueObject GetDoctorForDocType(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorListBySpecializationBizActionVO nvo = valueObject as clsGetDoctorListBySpecializationBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorByDocType");
                this.dbServer.AddInParameter(storedProcCommand, "DocType", DbType.Int64, nvo.SpecializationCode);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.DocDetails.Add(item);
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

        public override IValueObject GetDoctorListBySpecialization(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorListBySpecializationBizActionVO nvo = valueObject as clsGetDoctorListBySpecializationBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorListBySpecialization");
                this.dbServer.AddInParameter(storedProcCommand, "SepcializationCode", DbType.String, nvo.SpecializationCode.Trim());
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"]))
                        };
                        nvo.DocDetails.Add(item);
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

        public override IValueObject GetDoctorOrderForOTScheduling(IValueObject valueObject, clsUserVO UserVo)
        {
            IValueObject obj2;
            clsGetDoctorForDoctorTypeBizActionVO nvo = valueObject as clsGetDoctorForDoctorTypeBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                DbDataReader reader = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorOrderForOTScheduling");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "Priority", DbType.Int32, nvo.PriorityID);
                this.dbServer.AddInParameter(storedProcCommand, "OPD_IPD", DbType.Int32, nvo.Opd_Ipd);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientProcedureScheduleVO item = new clsPatientProcedureScheduleVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Group = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"])),
                            PatientType = Convert.ToString(DALHelper.HandleDBNull(reader["PatientType"])),
                            PriorityName = Convert.ToString(DALHelper.HandleDBNull(reader["Priority"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDate"]))),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]))
                        };
                        nvo.DoctorOrderList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
                obj2 = nvo;
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetEMRTemplateByProcID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProcDetailsByGetEMRTemplateBizactionVO nvo = valueObject as clsGetProcDetailsByGetEMRTemplateBizactionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcBYScheduleID");
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.PatientProcList = new List<clsPatientProcedureVO>();
                    while (reader.Read())
                    {
                        clsPatientProcedureVO item = new clsPatientProcedureVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["procID"])),
                            AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"])),
                            ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"])),
                            ProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureUnitID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]))
                        };
                        nvo.PatientProcList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetInstructionDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            Func<MasterListItem, bool> predicate = null;
            DbDataReader reader = null;
            clsGetInstructionDetailsBizActionVO nvo = valueObject as clsGetInstructionDetailsBizActionVO;
            clsInstructionMasterVO objItemVO = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetInstructionDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "FilterCriteria", DbType.String, nvo.FilterCriteria);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsInstructionMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            SelectInstruction = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionType"])) }
                        };
                        if (predicate == null)
                        {
                            predicate = q => q.ID == objItemVO.SelectInstruction.ID;
                        }
                        objItemVO.SelectInstruction = objItemVO.Instruction.FirstOrDefault<MasterListItem>(predicate);
                        objItemVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        nvo.InstructionMasterDetails.Add(objItemVO);
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

        public override IValueObject GetInstructionDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            Func<MasterListItem, bool> predicate = null;
            clsGetInstructionDetailsByIDBizActionVO BizAction = valueObject as clsGetInstructionDetailsByIDBizActionVO;
            try
            {
                clsInstructionMasterVO instructionDetails = BizAction.InstructionDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetInstructionDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizAction.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.InstructionDetails == null)
                        {
                            BizAction.InstructionDetails = new clsInstructionMasterVO();
                        }
                        BizAction.InstructionDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        BizAction.InstructionDetails.Code = (string) DALHelper.HandleDBNull(reader["Code"]);
                        BizAction.InstructionDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        BizAction.InstructionDetails.SelectInstruction.ID = (long) DALHelper.HandleDBNull(reader["InstructionType"]);
                        if (predicate == null)
                        {
                            predicate = q => q.ID == BizAction.InstructionDetails.SelectInstruction.ID;
                        }
                        BizAction.InstructionDetails.SelectInstruction = BizAction.InstructionDetails.Instruction.FirstOrDefault<MasterListItem>(predicate);
                        BizAction.InstructionDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        BizAction.InstructionDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetOTBookingByDateID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTBookingByOTTablebookingDateBizActionVO nvo = (clsGetOTBookingByOTTablebookingDateBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProOTScheduleListByOTTableIDAndDate");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, nvo.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, nvo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "OTDate", DbType.DateTime, nvo.OTDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.bookingDetailsList == null)
                    {
                        nvo.bookingDetailsList = new List<clsPatientProcedureScheduleVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsPatientProcedureScheduleVO item = new clsPatientProcedureScheduleVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            OTID = (long) DALHelper.HandleDBNull(reader["OperationTheatreID"]),
                            OTTableID = (long) DALHelper.HandleDBNull(reader["OTTableID"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            StartTime = (DateTime) reader["StartTime"],
                            EndTime = (DateTime) reader["EndTime"],
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["Patientname"]))
                        };
                        nvo.bookingDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOTForProcedure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTForProcedureBizActionVO nvo = valueObject as clsGetOTForProcedureBizActionVO;
            nvo.DocDetails = new List<MasterListItem>();
            nvo.DesignationDetails = new List<MasterListItem>();
            List<MasterListItem> list = new List<MasterListItem>();
            List<MasterListItem> list2 = new List<MasterListItem>();
            try
            {
                for (int i = 0; i < nvo.procedureIDList.Count; i++)
                {
                    DbDataReader reader = null;
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("OT_GetOTForProcedure");
                    this.dbServer.AddInParameter(storedProcCommand, "procedureID", DbType.Int64, nvo.procedureIDList[i]);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MasterListItem item = new MasterListItem {
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"]))
                            };
                            list.Add(item);
                        }
                    }
                    reader.Close();
                    DbDataReader reader2 = null;
                    DbCommand command = this.dbServer.GetStoredProcCommand("OT_GetOTStaffForProcedure");
                    this.dbServer.AddInParameter(command, "procedureID", DbType.Int64, nvo.procedureIDList[i]);
                    reader2 = (DbDataReader) this.dbServer.ExecuteReader(command);
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            MasterListItem item = new MasterListItem {
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader2["DocStaffCode"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader2["DocSpecialization"]))
                            };
                            list2.Add(item);
                        }
                    }
                    reader2.Close();
                    using (List<MasterListItem>.Enumerator enumerator = list.GetEnumerator())
                    {
                        Func<MasterListItem, bool> predicate = null;
                        while (enumerator.MoveNext())
                        {
                            MasterListItem item = enumerator.Current;
                            if (predicate == null)
                            {
                                predicate = checkItem => checkItem.Code == item.Code;
                            }
                            if (!nvo.DocDetails.Where<MasterListItem>(predicate).Any<MasterListItem>())
                            {
                                MasterListItem item3 = new MasterListItem {
                                    Code = item.Code,
                                    Description = item.Description
                                };
                                nvo.DocDetails.Add(item3);
                            }
                        }
                    }
                    using (List<MasterListItem>.Enumerator enumerator2 = list2.GetEnumerator())
                    {
                        Func<MasterListItem, bool> predicate = null;
                        while (enumerator2.MoveNext())
                        {
                            MasterListItem item1 = enumerator2.Current;
                            if (predicate == null)
                            {
                                predicate = checkItem => checkItem.Code == item1.Code;
                            }
                            if (!nvo.DesignationDetails.Where<MasterListItem>(predicate).Any<MasterListItem>())
                            {
                                MasterListItem item = new MasterListItem {
                                    Code = item1.Code,
                                    Description = item1.Description
                                };
                                nvo.DesignationDetails.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOTScheduleDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTScheduleListBizActionVO nvo = (clsGetOTScheduleListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTScheduleList");
                this.dbServer.AddInParameter(storedProcCommand, "OTScheduleID ", DbType.Int64, nvo.OTScheduleID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.OTScheduleList == null)
                    {
                        nvo.OTScheduleList = new List<clsOTScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsOTScheduleDetailsVO item = new clsOTScheduleDetailsVO {
                            ID = (long) reader["ID"],
                            OTScheduleID = (long) reader["OTScheduleID"],
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"])
                        };
                        if (item.DayID == 1L)
                        {
                            item.Day = "Sunday";
                        }
                        else if (item.DayID == 2L)
                        {
                            item.Day = "Monday";
                        }
                        else if (item.DayID == 3L)
                        {
                            item.Day = "Tuesday";
                        }
                        else if (item.DayID == 4L)
                        {
                            item.Day = "Wednesday";
                        }
                        else if (item.DayID == 5L)
                        {
                            item.Day = "Thursday";
                        }
                        else if (item.DayID == 6L)
                        {
                            item.Day = "Friday";
                        }
                        else if (item.DayID == 7L)
                        {
                            item.Day = "Saturday";
                        }
                        item.ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"]));
                        item.Schedule = Convert.ToString(DALHelper.HandleDBNull(reader["Schedule"]));
                        item.StartTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["StartTime"])));
                        item.EndTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["EndTime"])));
                        nvo.OTScheduleList.Add(item);
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

        public override IValueObject GetOTScheduleMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTScheduleMasterListBizActionVO nvo = (clsGetOTScheduleMasterListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTScheduleBySearchCriteria");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTableId", DbType.Int64, nvo.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, nvo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.OTScheduleList == null)
                    {
                        nvo.OTScheduleList = new List<clsOTScheduleVO>();
                    }
                    while (reader.Read())
                    {
                        clsOTScheduleVO item = new clsOTScheduleVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            OTID = (long) DALHelper.HandleDBNull(reader["OTID"]),
                            OTName = (string) DALHelper.HandleDBNull(reader["OTName"]),
                            OTTableID = (long) DALHelper.HandleDBNull(reader["OTTableID"]),
                            OTTableName = (string) DALHelper.HandleDBNull(reader["OTTableName"]),
                            Status = new bool?((bool) DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.OTScheduleList.Add(item);
                    }
                }
                reader.NextResult();
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        break;
                    }
                    nvo.TotalRows = Convert.ToInt32(reader["TotalRows"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOTScheduleTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTScheduleTimeVO evo = (clsGetOTScheduleTimeVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTScheduleTime");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, evo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, evo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, evo.OTTabelID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (evo.OTScheduleDetailsList == null)
                    {
                        evo.OTScheduleDetailsList = new List<clsOTScheduleDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsOTScheduleDetailsVO item = new clsOTScheduleDetailsVO {
                            OTTableID = (long) DALHelper.HandleDBNull(reader["OTTableID"]),
                            StartTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["StartTime"])),
                            EndTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["EndTime"])),
                            ScheduleID = (long) DALHelper.HandleDBNull(reader["ScheduleID"]),
                            DayID = (long) DALHelper.HandleDBNull(reader["DayID"])
                        };
                        if (item.DayID == 1L)
                        {
                            item.Day = "Sunday";
                        }
                        else if (item.DayID == 2L)
                        {
                            item.Day = "Monday";
                        }
                        else if (item.DayID == 3L)
                        {
                            item.Day = "Tuesday";
                        }
                        else if (item.DayID == 4L)
                        {
                            item.Day = "Wednesday";
                        }
                        else if (item.DayID == 5L)
                        {
                            item.Day = "Thursday";
                        }
                        else if (item.DayID == 6L)
                        {
                            item.Day = "Friday";
                        }
                        else if (item.DayID == 7L)
                        {
                            item.Day = "Saturday";
                        }
                        evo.OTScheduleDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return evo;
        }

        public override IValueObject GetOtTableDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetOTTableDetailsBizActionVO nvo = valueObject as clsGetOTTableDetailsBizActionVO;
            clsOTTableVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOtTableDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsOTTableVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            OTTheatreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTheatreId"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            TheatreName = Convert.ToString(DALHelper.HandleDBNull(reader["TheatreName"]))
                        };
                        nvo.OtTableMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientConsents(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientConsentsBizActionVO nvo = (clsGetPatientConsentsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientConsentsOT");
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentList == null)
                    {
                        nvo.ConsentList = new List<clsConsentDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsConsentDetailsVO item = new clsConsentDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["Id"]),
                            ConsentID = (long) DALHelper.HandleDBNull(reader["ConsentId"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Consent = (string) DALHelper.HandleDBNull(reader["Consent"]),
                            PatientName = (string) DALHelper.HandleDBNull(reader["PatientName"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            Gender = (string) DALHelper.HandleDBNull(reader["Gender"]),
                            Age = (int) DALHelper.HandleDBNull(reader["Age"])
                        };
                        nvo.ConsentList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientConsentsDetailsInHTML(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientConsentsDetailsInHTMLBizActionVO nvo = (clsGetPatientConsentsDetailsInHTMLBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientConsentsDetailsInHTML");
                this.dbServer.AddInParameter(storedProcCommand, "ConsentType", DbType.Int64, nvo.ConsentTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmID", DbType.Int64, nvo.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmUnitID", DbType.Int64, nvo.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PrintID", DbType.Int64, nvo.ConsentTypeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ResultDetails == null)
                    {
                        nvo.ResultDetails = new clsConsentDetailsVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        nvo.ResultDetails.ID = (long) DALHelper.HandleDBNull(reader["Id"]);
                        nvo.ResultDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.ResultDetails.Date = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                        nvo.ResultDetails.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.ResultDetails.Consent = (string) DALHelper.HandleDBNull(reader["Consent"]);
                        nvo.ResultDetails.PatientName = (string) DALHelper.HandleDBNull(reader["PatientName"]);
                        nvo.ResultDetails.MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        nvo.ResultDetails.Gender = (string) DALHelper.HandleDBNull(reader["Gender"]);
                        nvo.ResultDetails.Age = (int) DALHelper.HandleDBNull(reader["Age"]);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientDetailsForConsent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsentDetailsBizActionVO nvo = (clsGetConsentDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDetailsForConsent");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ConsentDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ConsentDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ConsentDetails == null)
                    {
                        nvo.ConsentDetails = new clsConsentDetailsVO();
                    }
                    if (reader.Read())
                    {
                        clsConsentDetailsVO svo1 = new clsConsentDetailsVO();
                        nvo.ConsentDetails.ID = Convert.ToInt64(reader["Id"]);
                        nvo.ConsentDetails.UnitID = Convert.ToInt64(reader["UnitID"]);
                        nvo.ConsentDetails.MRNo = Convert.ToString(reader["MRNo"]);
                        nvo.ConsentDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.ConsentDetails.PatientAddress = Security.base64Decode((string) DALHelper.HandleDBNull(reader["ResAddress"]));
                        nvo.ConsentDetails.PatientContachNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileNo"]));
                        nvo.ConsentDetails.KinName = Convert.ToString(DALHelper.HandleDBNull(reader["KinName"]));
                        nvo.ConsentDetails.KinAddress = Convert.ToString(DALHelper.HandleDBNull(reader["KinAddress"]));
                        nvo.ConsentDetails.KinMobileNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["KinMobileNo"]));
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

        public override IValueObject GetPatientProcedureSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientProcedureScheduleBizActionVO nvo = valueObject as clsGetPatientProcedureScheduleBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientProcedureScheduleList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, nvo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "OTUnitID", DbType.Int64, nvo.OTUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTableID", DbType.Int64, nvo.OTTableID);
                this.dbServer.AddInParameter(storedProcCommand, "OTBookingDate", DbType.DateTime, nvo.OTBookingDate);
                this.dbServer.AddInParameter(storedProcCommand, "OTToDate", DbType.DateTime, nvo.OTTODate);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, nvo.IsCancelled);
                if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName + "%");
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName + "%");
                }
                if ((nvo.MRNo != null) && (nvo.MRNo.Length > 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo + "%");
                }
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientProcedureScheduleVO item = new clsPatientProcedureScheduleVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"])),
                            DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]))),
                            MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"])),
                            Religion = Convert.ToString(DALHelper.HandleDBNull(reader["Religion"])),
                            ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"])),
                            EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                            SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirement"])),
                            OT = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"])),
                            OTTable = Convert.ToString(DALHelper.HandleDBNull(reader["OTTableName"])),
                            OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"])),
                            OTUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTUnitID"])),
                            OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"])),
                            OTDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTDetailsID"])),
                            OTDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTDetailsUnitID"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"])),
                            IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"])),
                            IsCancelled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                            AnesthetistName = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthetistName"])),
                            BillClearanceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillClearanceID"])),
                            BillClearanceUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillClearanceUnitID"])),
                            BillClearanceIsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BillClearanceIsFreezed"]))
                        };
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImagePath"]));
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                        item.ImageName = string.Concat(strArray);
                        nvo.patientProcScheduleDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetProcDetailsByProcID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetProcDetailsByProcIDBizActionVO nvo = valueObject as clsGetProcDetailsByProcIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcDetailsByProcID");
                this.dbServer.AddInParameter(storedProcCommand, "procID", DbType.Int64, nvo.ProcID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                nvo.InstructionList = new List<clsProcedureInstructionDetailsVO>();
                                nvo.PreInstructionList = new List<clsProcedureInstructionDetailsVO>();
                                nvo.PostInstructionList = new List<clsProcedureInstructionDetailsVO>();
                                while (true)
                                {
                                    if (!reader.Read())
                                    {
                                        reader.NextResult();
                                        while (true)
                                        {
                                            if (!reader.Read())
                                            {
                                                reader.NextResult();
                                                while (true)
                                                {
                                                    if (!reader.Read())
                                                    {
                                                        reader.NextResult();
                                                        while (true)
                                                        {
                                                            if (!reader.Read())
                                                            {
                                                                reader.NextResult();
                                                                while (true)
                                                                {
                                                                    if (!reader.Read())
                                                                    {
                                                                        reader.NextResult();
                                                                        while (true)
                                                                        {
                                                                            if (!reader.Read())
                                                                            {
                                                                                reader.Close();
                                                                                break;
                                                                            }
                                                                            clsProcedureTemplateDetailsVO svo7 = new clsProcedureTemplateDetailsVO {
                                                                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                                                                ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"])),
                                                                                TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                                                                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                                                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]))
                                                                            };
                                                                            nvo.ProcedureTempalateList.Add(svo7);
                                                                        }
                                                                        break;
                                                                    }
                                                                    clsProcedureStaffDetailsVO svo6 = new clsProcedureStaffDetailsVO {
                                                                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                                        DocOrStaffCode = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"])),
                                                                        DocClassification = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"])),
                                                                        NoofDocOrStaff = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"])),
                                                                        IsDoctor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]))
                                                                    };
                                                                    nvo.StaffList.Add(svo6);
                                                                }
                                                                break;
                                                            }
                                                            clsProcedureStaffDetailsVO svo5 = new clsProcedureStaffDetailsVO {
                                                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                                DocOrStaffCode = Convert.ToString(DALHelper.HandleDBNull(reader["DocStaffCode"])),
                                                                DocClassification = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecialization"])),
                                                                NoofDocOrStaff = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"])),
                                                                IsDoctor = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDoctor"]))
                                                            };
                                                            nvo.StaffList.Add(svo5);
                                                        }
                                                        break;
                                                    }
                                                    clsDoctorSuggestedServiceDetailVO lvo = new clsDoctorSuggestedServiceDetailVO {
                                                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                        ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                                                        ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                                                        GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"])),
                                                        ServiceType = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceType"]))
                                                    };
                                                    nvo.ServiceList.Add(lvo);
                                                }
                                                break;
                                            }
                                            clsProcedureItemDetailsVO svo4 = new clsProcedureItemDetailsVO {
                                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                                Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]))
                                            };
                                            nvo.ItemList.Add(svo4);
                                        }
                                        break;
                                    }
                                    clsProcedureInstructionDetailsVO svo3 = new clsProcedureInstructionDetailsVO {
                                        InstructionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionTypeID"])),
                                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                        InstructionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InstructionID"])),
                                        Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                                    };
                                    if (svo3.InstructionTypeID == 2L)
                                    {
                                        svo3.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Pre"]));
                                        nvo.PreInstructionList.Add(svo3);
                                        continue;
                                    }
                                    if (svo3.InstructionTypeID == 3L)
                                    {
                                        svo3.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Post"]));
                                        nvo.PostInstructionList.Add(svo3);
                                        continue;
                                    }
                                    if (svo3.InstructionTypeID == 1L)
                                    {
                                        svo3.InstructionDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Intra"]));
                                        nvo.InstructionList.Add(svo3);
                                    }
                                }
                                break;
                            }
                            clsProcedureConsentDetailsVO svo2 = new clsProcedureConsentDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                ConsentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsentID"])),
                                ConsentDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["Template"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                            };
                            nvo.ConsentList.Add(svo2);
                        }
                        break;
                    }
                    clsProcedureChecklistDetailsVO item = new clsProcedureChecklistDetailsVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                        CategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"])),
                        SubCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubCategoryID"])),
                        CheckListId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChecklistID"])),
                        Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"])),
                        Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]))
                    };
                    nvo.CheckList.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetProcedureCheckList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetProcedureCheckListBizActionVO nvo = (clsGetProcedureCheckListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcedureCheckList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "TotalRows", DbType.Int64, nvo.TotalRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CheckList == null)
                    {
                        nvo.CheckList = new List<clsProcedureCheckListVO>();
                    }
                    while (reader.Read())
                    {
                        clsProcedureCheckListVO item = new clsProcedureCheckListVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            CategoryId = (long) DALHelper.HandleDBNull(reader["CategoryID"]),
                            SubCategoryId = (long) DALHelper.HandleDBNull(reader["SubCategoryID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Category = (string) DALHelper.HandleDBNull(reader["Category"]),
                            SubCategory = (string) DALHelper.HandleDBNull(reader["SubCategory"]),
                            Status = (bool) reader["Status"]
                        };
                        nvo.CheckList.Add(item);
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

        public override IValueObject GetProcedureMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetProcedureMasterBizActionVO nvo = valueObject as clsGetProcedureMasterBizActionVO;
            clsProcedureMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcedureMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureTypeID", DbType.Int64, nvo.ProcedureTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsProcedureMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            ServiceID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]))),
                            Duration = Convert.ToString(DALHelper.HandleDBNull(reader["Duration"])),
                            ProcedureTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"]))),
                            RecommandedAnesthesiaTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"]))),
                            OperationTheatreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreID"]))),
                            OTTableID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"]))),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]))
                        };
                        nvo.ProcDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetProceduresForOTBookingID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProceduresForOTBookingIDBizActionVO nvo = (clsGetProceduresForOTBookingIDBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcedureIDForOTBookingID");
                this.dbServer.AddInParameter(storedProcCommand, "OTBookingID", DbType.Int64, nvo.OTBokingID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.procedureList == null)
                    {
                        nvo.procedureList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ProcedureID"])
                        };
                        nvo.procedureList.Add(item);
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

        public override IValueObject GetProcedureSubCategoryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetProcedureSubCategoryListBizActionVO nvo = (clsGetProcedureSubCategoryListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_GetSubCategoryMasterList]");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "TotalRows", DbType.Int64, nvo.TotalRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.SubCategoryList == null)
                    {
                        nvo.SubCategoryList = new List<clsProcedureSubCategoryVO>();
                    }
                    while (reader.Read())
                    {
                        clsProcedureSubCategoryVO item = new clsProcedureSubCategoryVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            Code = Convert.ToString(reader["Code"]),
                            Description = Convert.ToString(reader["Description"]),
                            CategoryId = Convert.ToInt64(reader["ProcedureCategoryID"]),
                            Category = Convert.ToString(reader["Category"]),
                            UnitId = Convert.ToInt64(reader["UnitID"]),
                            Status = Convert.ToBoolean(reader["Status"])
                        };
                        nvo.SubCategoryList.Add(item);
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

        public override IValueObject GetProcOTSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProcOTScheduleBizActionVO nvo = (clsGetProcOTScheduleBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcOTScedule");
                this.dbServer.AddInParameter(storedProcCommand, "OTTheaterID", DbType.Int64, nvo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "OTTable", DbType.Int64, nvo.OTTableID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.OTScheduleDetailsList == null)
                    {
                        nvo.OTScheduleDetailsList = new List<clsPatientProcOTScheduleDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientProcOTScheduleDetailsVO item = new clsPatientProcOTScheduleDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            OTID = new long?((long) DALHelper.HandleDBNull(reader["OTID"])),
                            OTDesc = (string) DALHelper.HandleDBNull(reader["OTDesc"]),
                            OTTableID = new long?((long) DALHelper.HandleDBNull(reader["OTTableID"])),
                            OTTableDesc = (string) DALHelper.HandleDBNull(reader["OTTableDesc"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["StartTime"]);
                        item.StartTime = new DateTime?(nullable.Value);
                        DateTime? nullable2 = DALHelper.HandleDate(reader["EndTime"]);
                        item.EndTime = new DateTime?(nullable2.Value);
                        item.DayID = new long?((long) DALHelper.HandleDBNull(reader["DayID"]));
                        nvo.OTScheduleDetailsList.Add(item);
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

        public override IValueObject GetProcScheduleDetailsByProcScheduleID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetProcScheduleDetailsByProcScheduleIDBizActionVO nvo = valueObject as clsGetProcScheduleDetailsByProcScheduleIDBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProcScheduleDetailsByProcScheduleID");
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.DocScheduleDetails = new List<clsPatientProcDocScheduleDetailsVO>();
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    nvo.StaffDetailList = new List<clsPatientProcStaffDetailsVO>();
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            while (true)
                                            {
                                                if (!reader.Read())
                                                {
                                                    reader.NextResult();
                                                    while (true)
                                                    {
                                                        if (!reader.Read())
                                                        {
                                                            reader.NextResult();
                                                            nvo.CheckList = new List<clsPatientProcedureChecklistDetailsVO>();
                                                            while (true)
                                                            {
                                                                if (!reader.Read())
                                                                {
                                                                    reader.NextResult();
                                                                    while (true)
                                                                    {
                                                                        if (!reader.Read())
                                                                        {
                                                                            reader.NextResult();
                                                                            nvo.ServiceList = new List<clsDoctorSuggestedServiceDetailVO>();
                                                                            while (true)
                                                                            {
                                                                                if (!reader.Read())
                                                                                {
                                                                                    reader.NextResult();
                                                                                    nvo.PreOperativeInstructionList = new List<string>();
                                                                                    nvo.InstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
                                                                                    while (true)
                                                                                    {
                                                                                        if (!reader.Read())
                                                                                        {
                                                                                            reader.NextResult();
                                                                                            nvo.IntraOperativeInstructionList = new List<string>();
                                                                                            while (true)
                                                                                            {
                                                                                                if (!reader.Read())
                                                                                                {
                                                                                                    reader.NextResult();
                                                                                                    nvo.PostOperativeInstructionList = new List<string>();
                                                                                                    while (true)
                                                                                                    {
                                                                                                        if (!reader.Read())
                                                                                                        {
                                                                                                            reader.NextResult();
                                                                                                            nvo.AddedPatientProcList = new List<clsPatientProcedureVO>();
                                                                                                            while (true)
                                                                                                            {
                                                                                                                if (!reader.Read())
                                                                                                                {
                                                                                                                    reader.NextResult();
                                                                                                                    nvo.ItemList = new List<clsProcedureItemDetailsVO>();
                                                                                                                    while (reader.Read())
                                                                                                                    {
                                                                                                                        clsProcedureItemDetailsVO svo7 = new clsProcedureItemDetailsVO {
                                                                                                                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                                                                                                                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                                                                                                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                                                                                                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]))
                                                                                                                        };
                                                                                                                        nvo.ItemList.Add(svo7);
                                                                                                                    }
                                                                                                                    break;
                                                                                                                }
                                                                                                                clsPatientProcedureVO evo3 = new clsPatientProcedureVO {
                                                                                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"])),
                                                                                                                    ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                                                                                                                };
                                                                                                                nvo.AddedPatientProcList.Add(evo3);
                                                                                                            }
                                                                                                            break;
                                                                                                        }
                                                                                                        clsOTDetailsInstructionListDetailsVO svo6 = new clsOTDetailsInstructionListDetailsVO {
                                                                                                            GroupName = "Post Operative Instruction Notes",
                                                                                                            Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                                                                                                        };
                                                                                                        nvo.InstructionList.Add(svo6);
                                                                                                        nvo.PostOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
                                                                                                    }
                                                                                                    break;
                                                                                                }
                                                                                                clsOTDetailsInstructionListDetailsVO svo5 = new clsOTDetailsInstructionListDetailsVO {
                                                                                                    GroupName = "Intra Operative Instruction Notes",
                                                                                                    Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                                                                                                };
                                                                                                nvo.InstructionList.Add(svo5);
                                                                                                nvo.IntraOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
                                                                                            }
                                                                                            break;
                                                                                        }
                                                                                        clsOTDetailsInstructionListDetailsVO svo4 = new clsOTDetailsInstructionListDetailsVO {
                                                                                            GroupName = "Pre Operative Instruction Notes",
                                                                                            Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                                                                                        };
                                                                                        nvo.InstructionList.Add(svo4);
                                                                                        nvo.PreOperativeInstructionList.Add(Convert.ToString(DALHelper.HandleDBNull(reader["Description"])));
                                                                                    }
                                                                                    break;
                                                                                }
                                                                                clsDoctorSuggestedServiceDetailVO lvo = new clsDoctorSuggestedServiceDetailVO {
                                                                                    ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                                                                                    ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                                                                                    ServiceType = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceType"])),
                                                                                    GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["GroupName"]))
                                                                                };
                                                                                nvo.ServiceList.Add(lvo);
                                                                            }
                                                                            break;
                                                                        }
                                                                        nvo.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));
                                                                        nvo.detailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailsID"]));
                                                                    }
                                                                    break;
                                                                }
                                                                clsPatientProcedureChecklistDetailsVO svo3 = new clsPatientProcedureChecklistDetailsVO {
                                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                                    Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"])),
                                                                    SubCategory1 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory1"])),
                                                                    SubCategory2 = Convert.ToString(DALHelper.HandleDBNull(reader["SubCategory2"])),
                                                                    Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                                                                    Name = Convert.ToString(DALHelper.HandleDBNull(reader["Name"])),
                                                                    Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                                                                };
                                                                nvo.CheckList.Add(svo3);
                                                            }
                                                            break;
                                                        }
                                                        nvo.patientInfoObject.pateintID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                                        nvo.patientInfoObject.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                                                        nvo.patientInfoObject.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                                                        nvo.patientInfoObject.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                                                        nvo.patientInfoObject.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]));
                                                        nvo.patientInfoObject.patientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                                    }
                                                    break;
                                                }
                                                clsPatientProcedureScheduleVO evo2 = new clsPatientProcedureScheduleVO {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    OTID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreID"])),
                                                    OTUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OperationTheatreUnitID"])),
                                                    OTTableID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OTTableID"])),
                                                    StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"])),
                                                    Opd_Ipd = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Opd_Ipd"])),
                                                    VisitAdmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmID"])),
                                                    VisitAdmUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitAdmUnitID"])),
                                                    Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                                                    SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirement"])),
                                                    EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"])),
                                                    Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                                                    IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]))
                                                };
                                                nvo.OTScheduleList.Add(evo2);
                                            }
                                            break;
                                        }
                                        clsPatientProcStaffDetailsVO svo2 = new clsPatientProcStaffDetailsVO {
                                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                            DesignationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"])),
                                            designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DesignationDesc"])),
                                            StaffID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StaffID"])),
                                            ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"])),
                                            ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"])),
                                            stffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["staffDesc"])),
                                            Quantity = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]))
                                        };
                                        nvo.StaffDetailList.Add(svo2);
                                    }
                                    break;
                                }
                                clsPatientProcDocScheduleDetailsVO svo = new clsPatientProcDocScheduleDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"])),
                                    DocID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocID"])),
                                    DocTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DocTypeID"])),
                                    DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"])),
                                    docTypeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocTypeDesc"])),
                                    Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                    ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"])),
                                    ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"])),
                                    docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocDesc"])),
                                    DayID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"]))),
                                    StartTime = Convert.ToDateTime(DALHelper.HandleDate(reader["StartTime"])),
                                    EndTime = Convert.ToDateTime(DALHelper.HandleDate(reader["EndTime"])),
                                    Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                                };
                                svo.StrStartTime = svo.StartTime.ToShortTimeString();
                                svo.StrEndTime = svo.EndTime.ToShortTimeString();
                                nvo.DocScheduleDetails.Add(svo);
                            }
                            break;
                        }
                        clsPatientProcedureVO item = new clsPatientProcedureVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["procID"])),
                            AnesthesiaTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecommandedAnesthesiaTypeID"])),
                            ProcedureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"])),
                            ProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureUnitID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]))
                        };
                        nvo.PatientProcList.Add(item);
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

        public override IValueObject GetServicesForProcedure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServicesForProcedureBizActionVO nvo = (clsGetServicesForProcedureBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServicesForProcedure");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["DESCRIPTION"])
                        };
                        nvo.MasterList.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStaffByDesignation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsStaffByDesignationIDBizActionVO nvo = valueObject as clsStaffByDesignationIDBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffByDesignation");
                this.dbServer.AddInParameter(storedProcCommand, "DesignationID", DbType.Int64, nvo.DesignationID);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, nvo.ProcedureID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.StaffDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.staffQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoofDocOrStaff"]));
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

        public override IValueObject GetStaffForOTScheduling(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStaffForOTSchedulingBizActionVO nvo = (clsGetStaffForOTSchedulingBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStaffForOTScheduling");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.staffList == null)
                    {
                        nvo.staffList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.staffList.Add(item);
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

        public override IValueObject ProcedureUpdtStatus(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdStatusProcedureMasterBizActionVO nvo = (clsUpdStatusProcedureMasterBizActionVO) valueObject;
            try
            {
                clsProcedureMasterVO procedureObj = nvo.ProcedureObj;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStatusProcedureMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, procedureObj.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, procedureObj.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject SaveConsentDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsSaveConsentDetailsBizActionVO nvo = (clsSaveConsentDetailsBizActionVO) valueObject;
            try
            {
                clsConsentDetailsVO consentDetails = nvo.ConsentDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddConsentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ConsentDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ConsentDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ConsentDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.Date, nvo.ConsentDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmID", DbType.Int64, nvo.ConsentDetails.VisitAdmID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitAdmUnitID", DbType.Int64, nvo.ConsentDetails.VisitAdmUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd", DbType.Int32, nvo.ConsentDetails.Opd_Ipd);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentId", DbType.Int64, nvo.ConsentDetails.ConsentID);
                this.dbServer.AddInParameter(storedProcCommand, "Consent", DbType.String, nvo.ConsentDetails.Consent);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ConsentSummaryID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ConsentDetails.ConsentSummaryID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateInstructionStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateInstructionStatusBizActionVO nvo = valueObject as clsUpdateInstructionStatusBizActionVO;
            try
            {
                clsInstructionMasterVO instructionTempStatus = new clsInstructionMasterVO();
                instructionTempStatus = nvo.InstructionTempStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateInstructionStatus");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, instructionTempStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, instructionTempStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.InstructionTempStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateOTSchedule(IValueObject valueObject, clsUserVO UserVo)
        {
            return valueObject;
        }

        public override IValueObject UpdateStatusProcedureCheckList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusProcedureCheckListBizActionVO nvo = (clsUpdateStatusProcedureCheckListBizActionVO) valueObject;
            try
            {
                clsProcedureCheckListVO checkListDetails = nvo.CheckListDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStatusProcedureCheckList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, checkListDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, checkListDetails.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStatusProcedureSubCategory(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusProcedureSubCategoryBizActionVO nvo = (clsUpdateStatusProcedureSubCategoryBizActionVO) valueObject;
            try
            {
                clsProcedureSubCategoryVO subCategoryDetails = nvo.SubCategoryDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStatusProcedureSubCategory");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, subCategoryDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, subCategoryDetails.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

