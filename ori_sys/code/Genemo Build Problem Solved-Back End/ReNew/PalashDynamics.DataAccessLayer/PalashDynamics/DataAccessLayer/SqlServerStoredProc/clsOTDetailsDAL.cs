namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.OpeartionTheatre;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.OTConfiguration;
    using PalashDynamics.ValueObjects.EMR;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsOTDetailsDAL : clsBaseOTDetailsDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsOTDetailsDAL()
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

        public override IValueObject AddPatientWiseConsentPrinting(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientWiseConsentPrintingBizActionVO nvo = valueObject as clsAddPatientWiseConsentPrintingBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientWiseConsentPrinting");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ConsetPrintingObj.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ProcDate", DbType.DateTime, nvo.ConsetPrintingObj.ProcDate);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentID", DbType.Int64, nvo.ConsetPrintingObj.ConsentID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateName", DbType.String, nvo.ConsetPrintingObj.TemplateName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ConsetPrintingObj.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                transaction = null;
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateDoctorNotesDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateOTDoctorNotesDetailsBizActionVO nvo = valueObject as clsAddUpdateOTDoctorNotesDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDoctorNotes");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.AddInParameter(command2, "DoctorNotesID", DbType.Double, nvo.objOTDetails.DoctorNotesList.DoctorNotesID);
                this.dbServer.AddInParameter(command2, "DoctorNotes", DbType.String, nvo.objOTDetails.DoctorNotesList.DoctorNotes);
                this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command2, "ResultStatus"));
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                transaction.Commit();
                transaction = null;
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateOtDocEmpDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtDocEmpDetailsBizActionVO nvo = valueObject as clsAddUpdatOtDocEmpDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDocEmpStatusDetails");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                foreach (clsOTDetailsDocDetailsVO svo in nvo.objOTDetails.DocList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsDocDetails");
                    command3.Connection = connection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                    this.dbServer.AddInParameter(command3, "DesignationID", DbType.Int64, svo.DesignationID);
                    this.dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, svo.DoctorID);
                    this.dbServer.AddInParameter(command3, "DoctorCode", DbType.String, svo.DoctorCode);
                    this.dbServer.AddInParameter(command3, "DocFees", DbType.Double, svo.DocFees);
                    this.dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, svo.ProcedureID);
                    this.dbServer.AddInParameter(command3, "StartTime", DbType.String, svo.StrStartTime);
                    this.dbServer.AddInParameter(command3, "EndTime", DbType.String, svo.StrStartTime);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
                foreach (clsOTDetailsStaffDetailsVO svo2 in nvo.objOTDetails.StaffList)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsStaffDetails");
                    command4.Connection = connection;
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                    this.dbServer.AddInParameter(command4, "StaffID", DbType.Int64, svo2.StaffID);
                    this.dbServer.AddInParameter(command4, "ProcedureID", DbType.Int64, svo2.ProcedureID);
                    this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command4, "ResultStatus"));
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                transaction.Commit();
                transaction = null;
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateOtItemDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdateOtItemDetailsBizActionVO nvo = valueObject as clsAddUpdateOtItemDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTItemStatusDetails");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                foreach (clsProcedureItemDetailsVO svo in nvo.objOTDetails.ItemList1)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsItemDetails");
                    command3.Connection = connection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                    this.dbServer.AddInParameter(command3, "ItemCode", DbType.String, svo.ItemCode);
                    this.dbServer.AddInParameter(command3, "ItemName", DbType.String, svo.ItemName);
                    this.dbServer.AddInParameter(command3, "Quantity", DbType.String, svo.Quantity);
                    this.dbServer.AddInParameter(command3, "ItemID", DbType.String, svo.ItemID);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                transaction.Commit();
                transaction = null;
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateOtNotesDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtNotesDetailsBizActionVO nvo = valueObject as clsAddUpdatOtNotesDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTNotesStatusDetails");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                List<clsOTDetailsInstructionListDetailsVO> surgeryInstructionList = nvo.SurgeryInstructionList;
                List<clsOTDetailsInstructionListDetailsVO> anesthesiaInstructionList = nvo.AnesthesiaInstructionList;
                int num = 0;
                while (true)
                {
                    if (num >= anesthesiaInstructionList.Count)
                    {
                        for (int i = 0; i < surgeryInstructionList.Count; i++)
                        {
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsSurgeryNotesDetails");
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                            this.dbServer.AddInParameter(command4, "SurgeyNotes", DbType.String, surgeryInstructionList[i].Instruction);
                            this.dbServer.AddInParameter(command4, "GroupName", DbType.String, surgeryInstructionList[i].GroupName);
                            this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.objSurgeryNotes.ID);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int64, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command4, "ResultStatus"));
                        }
                        break;
                    }
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsAnesthesiaNotesDetails");
                    command3.Connection = connection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                    this.dbServer.AddInParameter(command3, "AnesthesiaNotes", DbType.String, anesthesiaInstructionList[num].Instruction);
                    this.dbServer.AddInParameter(command3, "GroupName", DbType.String, anesthesiaInstructionList[num].GroupName);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.objAnesthesiaNotes.ID);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    num++;
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                transaction = null;
                throw;
            }
            finally
            {
                transaction.Commit();
                transaction = null;
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateOtServicesDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtServicesDetailsBizActionVO nvo = valueObject as clsAddUpdatOtServicesDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTServicesStatusDetails");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                List<clsDoctorSuggestedServiceDetailVO> oTServicesList = nvo.objOTDetails.OTServicesList;
                for (int i = 0; i < oTServicesList.Count; i++)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsServiceDetails");
                    command3.Connection = connection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                    this.dbServer.AddInParameter(command3, "ServiceID", DbType.Int64, oTServicesList[i].ServiceID);
                    this.dbServer.AddInParameter(command3, "ServiceCode", DbType.String, oTServicesList[i].ServiceCode);
                    this.dbServer.AddInParameter(command3, "ServiceName", DbType.String, oTServicesList[i].ServiceName);
                    this.dbServer.AddInParameter(command3, "ServiceType", DbType.String, oTServicesList[i].ServiceType);
                    this.dbServer.AddInParameter(command3, "GroupName", DbType.String, oTServicesList[i].GroupName);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "Quantity", DbType.Double, oTServicesList[i].Quantity);
                    this.dbServer.AddInParameter(command3, "Priority", DbType.String, oTServicesList[i].SelectedPriority.ID);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, oTServicesList[i].ID);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command3, "ResultStatus"));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                transaction.Commit();
                if ((connection != null) && (connection.State == ConnectionState.Open))
                {
                    connection.Close();
                }
            }
            return nvo;
        }

        public override IValueObject AddUpdateOtSurgeryDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtSurgeryDetailsBizActionVO nvo = valueObject as clsAddUpdatOtSurgeryDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "OTDetailsMainID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTStatusDetails");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                foreach (clsPatientProcedureVO evo in nvo.objOTDetails.ProcedureList)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSurgeryDetails");
                    command3.Connection = connection;
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                    this.dbServer.AddInParameter(command3, "ProcedureID", DbType.Int64, evo.ProcedureID);
                    this.dbServer.AddInParameter(command3, "Quantity", DbType.Double, evo.Quantity);
                    this.dbServer.AddInParameter(command3, "IsEmergency", DbType.Boolean, evo.IsEmergency);
                    this.dbServer.AddInParameter(command3, "IsHighRisk", DbType.Boolean, evo.IsHighRisk);
                    this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command3, "ResultStatus"));
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                transaction.Commit();
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdatePostInstructionDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddUpdatOtPostInstructionDetailsBizActionVO nvo = valueObject as clsAddUpdatOtPostInstructionDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetailsPostInstruction");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.AddInParameter(command2, "PostInsID", DbType.Double, nvo.objOTDetails.PostInstructionList.PostInstructionID);
                this.dbServer.AddInParameter(command2, "PostInstructions", DbType.String, nvo.objOTDetails.PostInstructionList.PostInstruction);
                this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command2, "ResultStatus"));
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                transaction.Commit();
                transaction = null;
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddupdatOtDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsAddupdatOtDetailsBizActionVO nvo = valueObject as clsAddupdatOtDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTDetails");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmergency", DbType.Boolean, nvo.objOTDetails.IsEmergency);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.objOTDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTSheetDetails");
                command2.Connection = connection;
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.objOTDetails.ID);
                this.dbServer.AddInParameter(command2, "FromTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.FromTime);
                this.dbServer.AddInParameter(command2, "ToTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.ToTime);
                this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.Date);
                this.dbServer.AddInParameter(command2, "OTID", DbType.Int64, nvo.objOTDetails.objOtSheetDetails.OTID);
                this.dbServer.AddInParameter(command2, "TotalHours", DbType.String, nvo.objOTDetails.objOtSheetDetails.TotalHours);
                this.dbServer.AddInParameter(command2, "AnesthesiaTypeID", DbType.Int64, nvo.objOTDetails.objOtSheetDetails.AnesthesiaTypeID);
                this.dbServer.AddInParameter(command2, "ProcedureTypeID", DbType.Int64, nvo.objOTDetails.objOtSheetDetails.ProcedureTypeID);
                this.dbServer.AddInParameter(command2, "OTResultID", DbType.Int64, nvo.objOTDetails.objOtSheetDetails.OTResultID);
                this.dbServer.AddInParameter(command2, "OTStatusID", DbType.Int64, nvo.objOTDetails.objOtSheetDetails.OTStatusID);
                this.dbServer.AddInParameter(command2, "ManPower", DbType.Int64, nvo.objOTDetails.objOtSheetDetails.ManPower);
                this.dbServer.AddInParameter(command2, "IsEmergency", DbType.Boolean, nvo.objOTDetails.objOtSheetDetails.IsEmergency);
                this.dbServer.AddInParameter(command2, "Remark", DbType.String, nvo.objOTDetails.objOtSheetDetails.Remark);
                this.dbServer.AddInParameter(command2, "SpecialRequirements", DbType.String, nvo.objOTDetails.objOtSheetDetails.SpecialRequirement);
                this.dbServer.AddInParameter(command2, "AnesthesiaStartTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.AnesthesiaStartTime);
                this.dbServer.AddInParameter(command2, "AnesthesiaEndTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.AnesthesiaEndTime);
                this.dbServer.AddInParameter(command2, "WheelInTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.WheelInTime);
                this.dbServer.AddInParameter(command2, "WheelOutTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.WheelOutTime);
                this.dbServer.AddInParameter(command2, "SurgeryStartTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.SurgeryStartTime);
                this.dbServer.AddInParameter(command2, "SurgeryEndTime", DbType.DateTime, nvo.objOTDetails.objOtSheetDetails.SurgeryEndTime);
                this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objOTDetails.objOtSheetDetails.ID);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetConsetDetailsForConsentID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetConsetDetailsForConsentIDBizActionVO nvo = valueObject as clsGetConsetDetailsForConsentIDBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetConsentDetailsForConsetID");
                this.dbServer.AddInParameter(storedProcCommand, "ConsentID", DbType.Int64, nvo.ConsentID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        nvo.consentmaster.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.consentmaster.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        nvo.consentmaster.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        nvo.consentmaster.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));
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

        public override IValueObject GetDetailTablesOfOTDetailsByOTDetailID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO nvo = valueObject as clsGetDetailTablesOfOTDetailsByOTDetailIDBizActionVO;
            DbConnection connection = null;
            DbDataReader reader = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsForOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime? nullable1;
                            DateTime? nullable7;
                            DateTime? nullable8;
                            DateTime? nullable9;
                            DateTime? nullable10;
                            DateTime? nullable11;
                            nvo.OTSheetDetailsObj = new clsOTDetailsOTSheetDetailsVO();
                            nvo.OTSheetDetailsObj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            nvo.OTSheetDetailsObj.FromTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"])));
                            nvo.OTSheetDetailsObj.ToTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"])));
                            nvo.OTSheetDetailsObj.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])));
                            nvo.OTSheetDetailsObj.OTID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"])));
                            nvo.OTSheetDetailsObj.TotalHours = Convert.ToString(DALHelper.HandleDBNull(reader["TotalHours"]));
                            nvo.OTSheetDetailsObj.AnesthesiaTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaTypeID"])));
                            nvo.OTSheetDetailsObj.ProcedureTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"])));
                            nvo.OTSheetDetailsObj.OTResultID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTResultID"])));
                            nvo.OTSheetDetailsObj.OTStatusID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTStatusID"])));
                            nvo.OTSheetDetailsObj.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                            nvo.OTSheetDetailsObj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                            nvo.OTSheetDetailsObj.SpecialRequirement = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRequirements"]));
                            nvo.OTSheetDetailsObj.ManPower = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ManPower"])));
                            object obj2 = reader["AnesthesiaStartTime"];
                            if (obj2 != DBNull.Value)
                            {
                                nullable1 = new DateTime?(Convert.ToDateTime(obj2));
                            }
                            else
                            {
                                nullable1 = null;
                            }
                            nvo.OTSheetDetailsObj.AnesthesiaStartTime = nullable1;
                            object obj3 = reader["AnesthesiaEndTime"];
                            if (obj3 != DBNull.Value)
                            {
                                nullable7 = new DateTime?(Convert.ToDateTime(obj3));
                            }
                            else
                            {
                                nullable7 = null;
                            }
                            nvo.OTSheetDetailsObj.AnesthesiaEndTime = nullable7;
                            object obj4 = reader["WheelInTime"];
                            if (obj4 != DBNull.Value)
                            {
                                nullable8 = new DateTime?(Convert.ToDateTime(obj4));
                            }
                            else
                            {
                                nullable8 = null;
                            }
                            nvo.OTSheetDetailsObj.WheelInTime = nullable8;
                            object obj5 = reader["WheelOutTime"];
                            if (obj5 != DBNull.Value)
                            {
                                nullable9 = new DateTime?(Convert.ToDateTime(obj5));
                            }
                            else
                            {
                                nullable9 = null;
                            }
                            nvo.OTSheetDetailsObj.WheelOutTime = nullable9;
                            object obj6 = reader["SurgeryStartTime"];
                            if (obj6 != DBNull.Value)
                            {
                                nullable10 = new DateTime?(Convert.ToDateTime(obj6));
                            }
                            else
                            {
                                nullable10 = null;
                            }
                            nvo.OTSheetDetailsObj.SurgeryStartTime = nullable10;
                            object obj7 = reader["SurgeryEndTime"];
                            if (obj7 != DBNull.Value)
                            {
                                nullable11 = new DateTime?(Convert.ToDateTime(obj7));
                            }
                            else
                            {
                                nullable11 = null;
                            }
                            nvo.OTSheetDetailsObj.SurgeryEndTime = nullable11;
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

        public override IValueObject GetDocEmpDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDocEmpDetailsByOTDetailsIDBizActionVO nvo = valueObject as clsGetDocEmpDetailsByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetDocEmpDetailsByOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            MasterListItem item = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                            };
                            nvo.ProcedureList.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsOTDetailsDocDetailsVO item = new clsOTDetailsDocDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                DesignationID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DesignationID"]))),
                                DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]))),
                                DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorCode"])),
                                ProcedureID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]))),
                                docDesc = Convert.ToString(DALHelper.HandleDBNull(reader["DocName"])),
                                StrStartTime = Convert.ToString(DALHelper.HandleDBNull(reader["StartTime"])),
                                StrEndTime = Convert.ToString(DALHelper.HandleDBNull(reader["EndTime"])),
                                designationDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Designation"])),
                                ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]))
                            };
                            MasterListItem item2 = new MasterListItem {
                                ID = Convert.ToInt64(item.ProcedureID),
                                Description = item.ProcedureName
                            };
                            item.SelectedProcedure.ID = item2.ID;
                            item.SelectedProcedure.Description = item2.Description;
                            item.ProcedureList.Add(item2);
                            nvo.DoctorList.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsOTDetailsStaffDetailsVO item = new clsOTDetailsStaffDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                StaffID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["staffID"]))),
                                ProcedureID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"]))),
                                StaffDesc = Convert.ToString(DALHelper.HandleDBNull(reader["StaffName"])),
                                ProcedureName = Convert.ToString(DALHelper.HandleDBNull(reader["ProcedureName"]))
                            };
                            MasterListItem item3 = new MasterListItem {
                                ID = Convert.ToInt64(item.ProcedureID),
                                Description = item.ProcedureName
                            };
                            item.SelectedProcedure.ID = item3.ID;
                            item.SelectedProcedure.Description = item3.Description;
                            item.ProcedureList.Add(item3);
                            nvo.StaffList.Add(item);
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

        public override IValueObject GetDoctorForOTDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetDoctorForOTDetailsBizActionVO nvo = (clsGetDoctorForOTDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorForOTDetails");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                this.dbServer.AddInParameter(storedProcCommand, "DocTypeID", DbType.Int64, nvo.DocTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, "ID");
                this.dbServer.AddParameter(storedProcCommand, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TotalRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DocList == null)
                    {
                        nvo.DocList = new List<clsOTDetailsDocDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsOTDetailsDocDetailsVO item = new clsOTDetailsDocDetailsVO {
                            DoctorID = new long?((long) DALHelper.HandleDBNull(reader["DoctorID"])),
                            docDesc = (string) DALHelper.HandleDBNull(reader["DoctorName"]),
                            DesignationID = new long?((long) DALHelper.HandleDBNull(reader["ClassificationID"])),
                            designationDesc = (string) DALHelper.HandleDBNull(reader["Designation"])
                        };
                        nvo.DocList.Add(item);
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

        public override IValueObject GetDoctorNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorNotesByOTDetailsIDBizActionVO nvo = valueObject as clsGetDoctorNotesByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorNotesDetailsByOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.Close();
                                break;
                            }
                            nvo.DoctorNotes.DoctorNotes = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorNotes"]));
                            nvo.DoctorNotes.DoctorNotesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorNotesID"]));
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

        public override IValueObject GetItemDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemDetailsByOTDetailsIDBizActionVO nvo = valueObject as clsGetItemDetailsByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetItemDetailsByOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsProcedureItemDetailsVO item = new clsProcedureItemDetailsVO {
                                ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                                ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                Quantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["Quantity"]))
                            };
                            nvo.ItemList1.Add(item);
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

        public override IValueObject GetOTDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTDetailsListizActionVO nvo = valueObject as clsGetOTDetailsListizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOtDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "OTID", DbType.Int64, nvo.OTID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
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
                        clsOTDetailsVO item = new clsOTDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            ScheduleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ScheduleID"])),
                            MaritalStatus = Convert.ToString(DALHelper.HandleDBNull(reader["MaritalStatus"])),
                            Religion = Convert.ToString(DALHelper.HandleDBNull(reader["Religion"])),
                            ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo1"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Photo"]),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"])),
                            DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DateOfBirth"]))),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            FromTime = Convert.ToDateTime(DALHelper.HandleDate(reader["FromTime"])),
                            ToTime = Convert.ToDateTime(DALHelper.HandleDate(reader["ToTime"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            OTName = Convert.ToString(DALHelper.HandleDBNull(reader["OTName"])),
                            IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]))
                        };
                        nvo.objOTDetails.Add(item);
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

        public override IValueObject GetOTNotesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetOTNotesByOTDetailsIDBizActionVO nvo = valueObject as clsGetOTNotesByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetOTNotesDetailsByOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        nvo.AnesthesiaInstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
                        nvo.SurgeryInstructionList = new List<clsOTDetailsInstructionListDetailsVO>();
                        while (reader.Read())
                        {
                            clsOTDetailsInstructionListDetailsVO item = new clsOTDetailsInstructionListDetailsVO {
                                Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"])),
                                GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["groupName"]))
                            };
                            nvo.AnesthesiaInstructionList.Add(item);
                            nvo.AnesthesiaNotesObj.AnesthesiaNotes = Convert.ToString(DALHelper.HandleDBNull(reader["AnesthesiaNotes"]));
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.Close();
                                break;
                            }
                            clsOTDetailsInstructionListDetailsVO item = new clsOTDetailsInstructionListDetailsVO {
                                Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["SurgeyNotes"])),
                                GroupName = Convert.ToString(DALHelper.HandleDBNull(reader["groupName"]))
                            };
                            nvo.SurgeryInstructionList.Add(item);
                            nvo.SurgeryNotesObj.SurgeyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["SurgeyNotes"]));
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

        public override IValueObject GetOTSheetDetailsByOTID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetOTSheetDetailsBizActionVO nvo = valueObject as clsGetOTSheetDetailsBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTSheetDetailsByOTID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.objOTDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.objOTDetails.ScheduleID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.objOTDetails.objOtSheetDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.objOTDetails.objOtSheetDetails.FromTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["FromTime"])));
                        nvo.objOTDetails.objOtSheetDetails.ToTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ToTime"])));
                        nvo.objOTDetails.objOtSheetDetails.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])));
                        nvo.objOTDetails.objOtSheetDetails.OTID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTID"])));
                        nvo.objOTDetails.objOtSheetDetails.TotalHours = Convert.ToString(DALHelper.HandleDBNull(reader["TotalHours"]));
                        nvo.objOTDetails.objOtSheetDetails.AnesthesiaTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesiaTypeID"])));
                        nvo.objOTDetails.objOtSheetDetails.ProcedureTypeID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureTypeID"])));
                        nvo.objOTDetails.objOtSheetDetails.OTResultID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTResultID"])));
                        nvo.objOTDetails.objOtSheetDetails.OTStatusID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["OTStatusID"])));
                        nvo.objOTDetails.objOtSheetDetails.AnesthesiaStartTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AnesthesiaStartTime"])));
                        nvo.objOTDetails.objOtSheetDetails.AnesthesiaEndTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AnesthesiaEndTime"])));
                        nvo.objOTDetails.objOtSheetDetails.WheelInTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["WheelInTime"])));
                        nvo.objOTDetails.objOtSheetDetails.WheelOutTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["WheelOutTime"])));
                        nvo.objOTDetails.objOtSheetDetails.SurgeryStartTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SurgeryStartTime"])));
                        nvo.objOTDetails.objOtSheetDetails.SurgeryEndTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SurgeryEndTime"])));
                        nvo.objOTDetails.objOtSheetDetails.IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"]));
                        nvo.objOTDetails.objOtSheetDetails.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        nvo.objOTDetails.objOtSheetDetails.ManPower = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ManPower"])));
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

        public override IValueObject GetPatientUnitIDForOtDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            return valueObject;
        }

        public override IValueObject GetProceduresForServiceID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetProceduresForServiceIdBizActionVO nvo = valueObject as clsGetProceduresForServiceIdBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetProceduresForServiceID");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsProcedureMasterVO item = new clsProcedureMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]))
                        };
                        nvo.procedureList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetServicesByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServicesByOTDetailsIDBizActionVO nvo = valueObject as clsGetServicesByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetServicesByOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        nvo.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
                        while (reader.Read())
                        {
                            clsDoctorSuggestedServiceDetailVO item = new clsDoctorSuggestedServiceDetailVO {
                                SelectedPriority = new MasterListItem(),
                                ID = Convert.ToInt64(reader["ID"]),
                                ServiceType = Convert.ToString(reader["ServiceType"]),
                                ServiceName = Convert.ToString(reader["ServiceName"]),
                                ServiceCode = Convert.ToString(reader["ServiceCode"]),
                                GroupName = Convert.ToString(reader["GroupName"]),
                                Quantity = Convert.ToDouble(reader["Quantity"])
                            };
                            item.SelectedPriority.ID = Convert.ToInt64(reader["Priority"]);
                            nvo.ServiceDetails.Add(item);
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

        public override IValueObject GetServicesForProcedureID(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetServicesForProcedureIDBizActionVO nvo = valueObject as clsGetServicesForProcedureIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServicesForProcedureID");
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.Int64, nvo.ProcedureID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            while (reader.Read())
                            {
                                clsOTDetailsItemDetailsVO svo = new clsOTDetailsItemDetailsVO {
                                    ItemID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))),
                                    ItemDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                    Quantity = new double?(Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]))),
                                    ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]))
                                };
                                nvo.ItemList.Add(svo);
                            }
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"])),
                            ServiceTaxPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ServiceTaxPercent"])),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]))
                        };
                        nvo.serviceList.Add(item);
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

        public override IValueObject GetSurgeryDetailsByOTDetailsID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSurgeryDetailsByOTDetailsIDBizActionVO nvo = valueObject as clsGetSurgeryDetailsByOTDetailsIDBizActionVO;
            DbDataReader reader = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOTDetailsID");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ScheduleID", DbType.Int64, nvo.ScheduleId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OTDetailsID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, connection.BeginTransaction());
                nvo.OTDetailsID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                if (nvo.OTDetailsID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetSurgeryDetailsByOTDetailsID");
                    this.dbServer.AddInParameter(command2, "OTDetailsID", DbType.Int64, nvo.OTDetailsID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    nvo.ProcedureList = new List<clsPatientProcedureVO>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsPatientProcedureVO item = new clsPatientProcedureVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Quantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["Quantity"])),
                                ProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProcedureID"])),
                                ProcDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                IsEmergency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmergency"])),
                                IsHighRisk = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsHighRisk"]))
                            };
                            nvo.ProcedureList.Add(item);
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

        public override IValueObject UpdateProcedureScheduleStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateOTScheduleStatusBizActionVO nvo = valueObject as clsUpdateOTScheduleStatusBizActionVO;
            try
            {
                clsPatientProcedureScheduleVO updateStatusField = nvo.UpdateStatusField;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateOTScheduleStatus");
                this.dbServer.AddInParameter(storedProcCommand, "IsPAC", DbType.Boolean, nvo.IsCalledForPAC);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, updateStatusField.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, updateStatusField.Status);
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

