namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.CompoundDrug;
    using PalashDynamics.ValueObjects.EMR;
    using PalashDynamics.ValueObjects.EMR.EMR_Field_Values;
    using PalashDynamics.ValueObjects.OutPatientDepartment;
    using PalashDynamics.ValueObjects.RSIJ;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class clsPatientEMRDataDAL : clsBasePatientEMRDataDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsPatientEMRDataDAL()
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
                this.ImgIP = ConfigurationManager.AppSettings["EMRImgIP"];
                this.ImgVirtualDir = ConfigurationManager.AppSettings["EMRImgVirtualDir"];
                this.ImgSaveLocation = ConfigurationManager.AppSettings["EMRImgSavingLocation"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        private clsAddUpdateFieldValueMasterBizActionVO AddEMRFieldValue(clsAddUpdateFieldValueMasterBizActionVO objItem, clsUserVO UserVo)
        {
            try
            {
                clsFieldValuesMasterVO objFieldMatserDetails = new clsFieldValuesMasterVO();
                objFieldMatserDetails = objItem.objFieldMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEMRValuesMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objFieldMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objFieldMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "UsedFor", DbType.String, objFieldMatserDetails.UsedFor);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objFieldMatserDetails.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                objItem.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return objItem;
        }

        public override IValueObject AddPatientEMRData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientEMRDataBizActionVO nvo = valueObject as clsAddPatientEMRDataBizActionVO;
            try
            {
                clsPatientEMRDataVO patientEMRDataDetails = nvo.PatientEMRDataDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientEMRData");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientEMRDataDetails.LinkServer);
                if (patientEMRDataDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientEMRDataDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientEMRDataDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientEMRDataDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, patientEMRDataDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateByNurse", DbType.String, patientEMRDataDetails.TemplateByNurse);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateByDoctor", DbType.String, patientEMRDataDetails.TemplateByDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "HistoryTemplate", DbType.String, patientEMRDataDetails.HistoryTemplate);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientEMRDataDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "SavedBy", DbType.String, patientEMRDataDetails.SavedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientEMRDataDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientEMRDataDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRDataDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientEMRDataDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddPatientFeedback(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientFeedbackBizActionVO nvo = valueObject as clsAddPatientFeedbackBizActionVO;
            try
            {
                clsPatientFeedbackVO patientFeedbackDetails = nvo.PatientFeedbackDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientFeedback");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientFeedbackDetails.LinkServer);
                if (patientFeedbackDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientFeedbackDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientFeedbackDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientFeedbackDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Feedback", DbType.String, patientFeedbackDetails.Feedback);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientFeedbackDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientFeedbackDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientFeedbackDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientFeedbackDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PatientFeedbackDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateCompoundMedicationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateCompoundPrescriptionBizActionVO nvo = valueObject as clsAddUpdateCompoundPrescriptionBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            long parameterValue = 0L;
            connection = this.dbServer.CreateConnection();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            transaction = connection.BeginTransaction();
            try
            {
                if ((nvo.CoumpoundDrugList != null) && (nvo.CoumpoundDrugList.Count > 0))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientCompoundPrescription");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "PrescriptionID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "PrescriptionID");
                    storedProcCommand.Dispose();
                    foreach (clsCompoundDrugMasterVO rvo in nvo.CompoundDrugMaster)
                    {
                        clsCompoundDrugMasterVO rvo2 = rvo;
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddCompoundDrug");
                        this.dbServer.AddInParameter(command, "Code", DbType.String, rvo2.Code);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, rvo2.Description);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, rvo2.Status);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo2.ID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        rvo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        foreach (clsPatientCompoundPrescriptionVO nvo2 in nvo.CoumpoundDrugList)
                        {
                            if (nvo2.CompoundDrug == rvo2.Description)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientCompoundPrescription");
                                this.dbServer.AddInParameter(command3, "PrescriptionID", DbType.Int64, parameterValue);
                                if (nvo2.CompoundDrugID > 0L)
                                {
                                    this.dbServer.AddInParameter(command3, "CompoundDrugID", DbType.Int64, nvo2.CompoundDrugID);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command3, "CompoundDrugID", DbType.Int64, rvo2.ID);
                                }
                                this.dbServer.AddInParameter(command3, "ItemCode", DbType.String, nvo2.ItemCode);
                                this.dbServer.AddInParameter(command3, "ItemName", DbType.String, nvo2.ItemName);
                                this.dbServer.AddInParameter(command3, "ComponentQuantity", DbType.String, nvo2.sComponentQuantity);
                                this.dbServer.AddInParameter(command3, "CompoundQuantity", DbType.String, nvo2.sCompoundQuantity);
                                this.dbServer.AddInParameter(command3, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                                this.dbServer.AddInParameter(command3, "Instruction", DbType.String, nvo2.Instruction);
                                this.dbServer.AddInParameter(command3, "Frequency", DbType.String, nvo2.Frequency);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo2.ID);
                                if (this.dbServer.ExecuteNonQuery(command3, transaction) < 1)
                                {
                                    throw new InvalidOperationException();
                                }
                                if (nvo.SuccessStatus == 1)
                                {
                                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");
                                    this.dbServer.AddInParameter(command4, "CompoundDrugId", DbType.Int64, rvo2.ID);
                                    this.dbServer.AddInParameter(command4, "ItemCode", DbType.String, nvo2.ItemCode);
                                    this.dbServer.AddInParameter(command4, "ItemName", DbType.String, nvo2.ItemName);
                                    this.dbServer.AddInParameter(command4, "Quantity", DbType.String, nvo2.sComponentQuantity);
                                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(command4, transaction);
                                }
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
            catch (Exception exception2)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                throw exception2;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return nvo;
        }

        public override IValueObject AddUpdateEMRFieldValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateFieldValueMasterBizActionVO objItem = valueObject as clsAddUpdateFieldValueMasterBizActionVO;
            return ((objItem.objFieldMatserDetails.ID != 0L) ? this.UpdateEMRFieldValue(objItem, UserVo) : this.AddEMRFieldValue(objItem, UserVo));
        }

        public override IValueObject AddUpdateFollowUpDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateFollowUpDetailsBizActionVO nvo = valueObject as clsAddUpdateFollowUpDetailsBizActionVO;
            DbCommand command = nvo.ISFollowUpNewQueueList ? this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientFollowUpQueue") : this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientFollowUp");
            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            this.dbServer.AddInParameter(command, "FollowUpDateTime", DbType.DateTime, nvo.FollowupDate);
            this.dbServer.AddInParameter(command, "DoctorCode", DbType.String, nvo.DoctorCode);
            this.dbServer.AddInParameter(command, "NoFollowReq", DbType.Boolean, nvo.FolloWUPRequired);
            this.dbServer.AddInParameter(command, "AppoinmentReson", DbType.Int64, nvo.AppoinmentReson);
            this.dbServer.AddInParameter(command, "DepartmentCode", DbType.String, nvo.DepartmentCode);
            this.dbServer.AddInParameter(command, "FollowUpRemarks", DbType.String, nvo.FollowUpRemark);
            this.dbServer.AddInParameter(command, "Advice", DbType.String, nvo.Advice);
            this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FollowUpID);
            this.dbServer.ExecuteNonQuery(command);
            nvo.SuccessStatus = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ResultStatus"));
            command.Dispose();
            return nvo;
        }

        public override IValueObject AddUpdatePatientCPOEDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientCPOEDetailsBizActionVO nvo = valueObject as clsAddUpdatePatientCPOEDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                long num = 0L;
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientPrescriptionFromCPOE");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "Advice", DbType.String, nvo.Advice);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpDate", DbType.DateTime, nvo.FollowupDate);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpRemark", DbType.String, nvo.FollowUpRemark);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, num);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                int parameterValue = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                num = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                storedProcCommand.Dispose();
                if (nvo.FollowupDate == null)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientFollowUpFromCPOE");
                    this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command3, "DoctorID", DbType.Int64, nvo.DoctorID);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    command3.Dispose();
                }
                else
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientFollowUpFromCPOE");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "FollowUpNo", DbType.Int32, 0);
                    this.dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, nvo.DepartmentID);
                    this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, nvo.DoctorID);
                    this.dbServer.AddInParameter(command2, "ServiceId", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "FollowUpDate", DbType.DateTime, nvo.FollowupDate);
                    this.dbServer.AddInParameter(command2, "FollowUpTime", DbType.DateTime, nvo.FollowupDate);
                    this.dbServer.AddInParameter(command2, "FollowUpFor", DbType.String, "");
                    this.dbServer.AddInParameter(command2, "FollowUpRemarks", DbType.String, nvo.FollowUpRemark);
                    this.dbServer.AddInParameter(command2, "FollowUpFrom", DbType.String, FollowUpFrom.EMR.ToString());
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "TariffID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "PackageServiceID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, nvo.FollowUpID);
                    this.dbServer.AddParameter(command2, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    command2.Dispose();
                }
                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientPrescriptionDetailFromCPOE");
                this.dbServer.AddInParameter(command4, "PrescriptionID", DbType.Int64, num);
                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.ExecuteNonQuery(command4, transaction);
                command4.Dispose();
                if ((nvo.PatientPrescriptionDetailList != null) && (nvo.PatientPrescriptionDetailList.Count > 0))
                {
                    foreach (clsPatientPrescriptionDetailVO lvo in nvo.PatientPrescriptionDetailList)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailFromCPOE");
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(command5, "PrescriptionID", DbType.Int64, num);
                        this.dbServer.AddInParameter(command5, "DrugID", DbType.Int64, lvo.DrugID);
                        this.dbServer.AddInParameter(command5, "Dose", DbType.String, lvo.Dose);
                        if ((lvo.SelectedRoute != null) && (lvo.SelectedRoute.Description != "--Select--"))
                        {
                            this.dbServer.AddInParameter(command5, "Route", DbType.String, lvo.SelectedRoute.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command5, "Route", DbType.String, null);
                        }
                        if ((lvo.SelectedInstruction != null) && (lvo.SelectedInstruction.Description != "--Select--"))
                        {
                            this.dbServer.AddInParameter(command5, "Instruction", DbType.String, lvo.SelectedInstruction.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command5, "Instruction", DbType.String, null);
                        }
                        if (lvo.FromHistory)
                        {
                            lvo.SelectedFrequency.Description = lvo.Frequency;
                        }
                        if (lvo.SelectedFrequency.Description != "--Select--")
                        {
                            this.dbServer.AddInParameter(command5, "Frequency", DbType.String, lvo.SelectedFrequency.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command5, "Frequency", DbType.String, null);
                        }
                        this.dbServer.AddInParameter(command5, "ItemName", DbType.String, lvo.DrugName);
                        this.dbServer.AddInParameter(command5, "Days", DbType.Int64, lvo.Days);
                        this.dbServer.AddInParameter(command5, "Quantity", DbType.Double, lvo.Quantity);
                        this.dbServer.AddInParameter(command5, "Rate", DbType.Int64, lvo.Rate);
                        this.dbServer.AddInParameter(command5, "IsOther", DbType.Boolean, lvo.IsOther);
                        this.dbServer.AddInParameter(command5, "Reason", DbType.String, lvo.Reason);
                        this.dbServer.AddInParameter(command5, "Timing", DbType.String, null);
                        this.dbServer.AddInParameter(command5, "ID", DbType.Int64, lvo.ID);
                        this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                    }
                }
                DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDoctorSuggestedServiceDeatailFromCPOE");
                this.dbServer.AddInParameter(command6, "PrescriptionID", DbType.Int64, num);
                this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command6, transaction);
                int num2 = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                command6.Dispose();
                if ((nvo.PatientServiceDetailDetailList != null) && (nvo.PatientServiceDetailDetailList.Count > 0))
                {
                    foreach (clsDoctorSuggestedServiceDetailVO lvo2 in nvo.PatientServiceDetailDetailList)
                    {
                        DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDeatailFromCPOE");
                        this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(command7, "PrescriptionID", DbType.Int64, num);
                        this.dbServer.AddInParameter(command7, "ServiceID", DbType.Int64, lvo2.ServiceID);
                        this.dbServer.AddInParameter(command7, "ServiceName", DbType.String, lvo2.ServiceName);
                        this.dbServer.AddInParameter(command7, "IsOther", DbType.Boolean, lvo2.IsOther);
                        this.dbServer.AddInParameter(command7, "Reason", DbType.String, lvo2.Reason);
                        this.dbServer.AddInParameter(command7, "Rate", DbType.Double, 0);
                        if (lvo2.SelectedDoctor != null)
                        {
                            this.dbServer.AddInParameter(command7, "DoctorID", DbType.Int64, lvo2.SelectedDoctor.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command7, "DoctorID", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command7, "ID", DbType.Int64, lvo2.ID);
                        this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                        int num3 = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                    }
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientCurrentMedicationDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientCurrentMedicationDetailBizActionVO nvo = valueObject as clsAddUpdatePatientCurrentMedicationDetailBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                long num = 0L;
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                if (nvo.PatientCurrentMedicationDetail != null)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionID");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.Int64, nvo.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand, transaction);
                    if (reader.HasRows)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.Close();
                                break;
                            }
                            num = (long) DALHelper.HandleDBNull(reader["ID"]);
                        }
                    }
                    reader.Dispose();
                    storedProcCommand.Dispose();
                    if (num == 0L)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionFromEMR");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "VisitID", DbType.Int64, nvo.VisitID);
                        this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, nvo.TemplateID);
                        this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, nvo.DoctorID);
                        this.dbServer.AddInParameter(command2, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, num);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        int parameterValue = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        num = (long) this.dbServer.GetParameterValue(command2, "ID");
                        command2.Dispose();
                    }
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientCurrentMedication");
                    this.dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, num);
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    command.Dispose();
                    foreach (clsPatientPrescriptionDetailVO lvo in nvo.PatientCurrentMedicationDetail)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailCurrentMedication");
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "PrescriptionID", DbType.Int64, num);
                        this.dbServer.AddInParameter(command4, "DrugID", DbType.Int64, lvo.DrugID);
                        this.dbServer.AddInParameter(command4, "Dose", DbType.String, lvo.Dose);
                        if ((lvo.SelectedRoute != null) && (lvo.SelectedRoute.Description != "--Select--"))
                        {
                            this.dbServer.AddInParameter(command4, "Route", DbType.String, lvo.SelectedRoute.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command4, "Route", DbType.String, null);
                        }
                        if ((lvo.SelectedInstruction != null) && (lvo.SelectedInstruction.Description != "--Select--"))
                        {
                            this.dbServer.AddInParameter(command4, "Instruction", DbType.String, lvo.SelectedInstruction.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command4, "Instruction", DbType.String, null);
                        }
                        if (lvo.SelectedFrequency.Description != "--Select--")
                        {
                            this.dbServer.AddInParameter(command4, "Frequency", DbType.String, lvo.SelectedFrequency.Description);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command4, "Frequency", DbType.String, null);
                        }
                        this.dbServer.AddInParameter(command4, "ItemName", DbType.String, lvo.DrugName);
                        this.dbServer.AddInParameter(command4, "Days", DbType.Int64, lvo.Days);
                        this.dbServer.AddInParameter(command4, "year", DbType.String, lvo.Year);
                        this.dbServer.AddInParameter(command4, "Quantity", DbType.Double, lvo.Quantity);
                        this.dbServer.AddInParameter(command4, "Rate", DbType.Int64, lvo.Rate);
                        this.dbServer.AddInParameter(command4, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                        this.dbServer.AddInParameter(command4, "FromHistory", DbType.Boolean, lvo.FromHistory);
                        this.dbServer.AddInParameter(command4, "IsOther", DbType.Boolean, lvo.IsOther);
                        this.dbServer.AddInParameter(command4, "Reason", DbType.String, lvo.Reason);
                        this.dbServer.AddInParameter(command4, "Timing", DbType.String, null);
                        this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                    }
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientEMRDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            DateTime date = DateTime.Now.Date;
            clsAddUpdatePatientEMRDetailBizActionVO nvo = valueObject as clsAddUpdatePatientEMRDetailBizActionVO;
            try
            {
                if (nvo.FalgForAddUpdate == 1)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientEMRDetail");
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.TempVariance.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.TempVariance.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TempVariance.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TempVariance.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.TempVariance.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "EMRTemplateDataId", DbType.Int64, nvo.TemplateDataId);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                List<clsPatientEMRDetailsVO> patientEMRDataDetails = nvo.PatientEMRDataDetails;
                int count = patientEMRDataDetails.Count;
                int num2 = 0;
                while (true)
                {
                    if (num2 >= count)
                    {
                        if (nvo.TempVariance.TemplateID == 0x29)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddSpremThawingFromVitrifiaction");
                            this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, nvo.TempVariance.UnitId);
                            this.dbServer.AddInParameter(command, "IsDonor", DbType.Boolean, nvo.IsDonor);
                            this.dbServer.ExecuteNonQuery(command);
                        }
                        break;
                    }
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientEMRDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.TempVariance.LinkServer);
                    if (nvo.TempVariance.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.TempVariance.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.TempVariance.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.TempVariance.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TempVariance.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TempVariance.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.TempVariance.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.TempVariance.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlCaption", DbType.String, patientEMRDataDetails[num2].ControlCaption);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlName", DbType.String, patientEMRDataDetails[num2].ControlName);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlType", DbType.String, patientEMRDataDetails[num2].ControlType);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlUnit", DbType.String, patientEMRDataDetails[num2].ControlUnit);
                    this.dbServer.AddInParameter(storedProcCommand, "CurrDate", DbType.DateTime, date);
                    this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, patientEMRDataDetails[num2].Value);
                    this.dbServer.AddInParameter(storedProcCommand, "EMRTemplateDataId", DbType.Int64, nvo.TemplateDataId);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRDataDetails[num2].ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.PatientEMRDataDetails[num2].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    num2++;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientEMRUploadedFiles(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientEMRUploadedFilesBizActionVO nvo = valueObject as clsAddUpdatePatientEMRUploadedFilesBizActionVO;
            try
            {
                if (nvo.FalgForAddUpdate == 1)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientEMRUploadedFiles");
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.TempVariance.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.TempVariance.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TempVariance.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TempVariance.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.TempVariance.UnitId);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                List<clsPatientEMRUploadedFilesVO> patientEMRUploadedFiles = nvo.PatientEMRUploadedFiles;
                int count = patientEMRUploadedFiles.Count;
                for (int i = 0; i < count; i++)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientEMRUploadedFiles");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.TempVariance.LinkServer);
                    if (nvo.TempVariance.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.TempVariance.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.TempVariance.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.TempVariance.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TempVariance.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TempVariance.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.TempVariance.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.TempVariance.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "IsIvfId", DbType.Int32, nvo.IsivfID);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlCaption", DbType.String, patientEMRUploadedFiles[i].ControlCaption);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlName", DbType.String, patientEMRUploadedFiles[i].ControlName);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlType", DbType.String, patientEMRUploadedFiles[i].ControlType);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlIndex", DbType.Int32, patientEMRUploadedFiles[i].ControlIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.Binary, patientEMRUploadedFiles[i].Value);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRUploadedFiles[i].ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.PatientEMRUploadedFiles[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientForIPDLAPAndHistro(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientHistoryForIPDLapAndHistroBizActionVO nvo = valueObject as clsAddUpdatePatientHistoryForIPDLapAndHistroBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPatientEMRDataVO patientEMRData = nvo.PatientEMRData;
                if (patientEMRData != null)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRDataForIPD");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientEMRData.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientEMRData.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, patientEMRData.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.String, nvo.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "SavedBy", DbType.String, patientEMRData.SavedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "TakenBy", DbType.Int64, nvo.Takenby);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRData.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    patientEMRData.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                foreach (clsPatientEMRDetailsVO svo in nvo.PatientHistoryDetailsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientHistoryDetailsForIPD");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                    if (nvo.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientEMRdata", DbType.Int64, patientEMRData.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlCaption", DbType.String, svo.ControlCaption);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlName", DbType.String, svo.ControlName);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlType", DbType.String, svo.ControlType);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlUnit", DbType.String, svo.ControlUnit);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlHeader", DbType.String, svo.Header);
                    this.dbServer.AddInParameter(storedProcCommand, "ControlSection", DbType.String, svo.Section);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, patientEMRData.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, svo.Value);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                    {
                        throw new Exception();
                    }
                }
                if ((nvo.HystroLaproImg != null) && (nvo.HystroLaproImg.Count > 0))
                {
                    foreach (ListItems2 items in nvo.HystroLaproImg)
                    {
                        string str = this.GetImageName(nvo.TemplateID, patientEMRData.PatientID, patientEMRData.VisitID, UserVo.UserLoginInfo.UnitId, nvo.IsOPDIPD);
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddImageForHistroScopyAndLaproScopy");
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientEMRData.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientEMRData.VisitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int32, nvo.TemplateID);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "ImgPath", DbType.String, str);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                        this.dbServer.AddInParameter(storedProcCommand, "EMRID", DbType.Int64, patientEMRData.ID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        MemoryStream stream = new MemoryStream(items.Photo);
                        Image original = Image.FromStream(stream);
                        Image image2 = new Bitmap(original, new Size(150, 120));
                        ImageConverter converter1 = new ImageConverter();
                        MemoryStream stream1 = new MemoryStream();
                        ImageCodecInfo info = this.GetEncoder(ImageFormat.Jpeg);
                        Encoder quality = Encoder.Quality;
                        EncoderParameters encoderParams = new EncoderParameters(1);
                        EncoderParameter parameter = new EncoderParameter(quality, (long) 50);
                        encoderParams.Param[0] = parameter;
                        image2.Save(this.ImgSaveLocation + str, info, encoderParams);
                    }
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                throw exception;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientHistoryData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientHistoryDataBizActionVO nvo = valueObject as clsAddUpdatePatientHistoryDataBizActionVO;
            try
            {
                clsPatientEMRDataVO patientEMRData = nvo.PatientEMRData;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientHistoryData");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientEMRData.LinkServer);
                if (patientEMRData.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientEMRData.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientEMRData.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientEMRData.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, patientEMRData.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateByNurse", DbType.String, patientEMRData.TemplateByNurse);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateByDoctor", DbType.String, patientEMRData.TemplateByDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "HistoryTemplate", DbType.String, patientEMRData.HistoryTemplate);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientEMRData.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "SavedBy", DbType.String, patientEMRData.SavedBy);
                this.dbServer.AddInParameter(storedProcCommand, "Freeze", DbType.Boolean, patientEMRData.Freeze);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientEMRData.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientEMRData.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRData.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                patientEMRData.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdatePatientHistoryDataAndDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientHistoryDataAndDetailsBizActionVO nvo = valueObject as clsAddUpdatePatientHistoryDataAndDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPatientEMRDataVO patientEMRData = nvo.PatientEMRData;
                if (patientEMRData != null)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRData");
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, patientEMRData.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, patientEMRData.VisitID);
                    this.dbServer.AddInParameter(command, "Doctorid", DbType.Int64, patientEMRData.DoctorID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.String, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "SavedBy", DbType.String, patientEMRData.SavedBy);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(command, "TakenBy", DbType.Int64, nvo.Takenby);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRData.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    patientEMRData.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientHistoryDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                foreach (clsPatientEMRDetailsVO svo in nvo.PatientHistoryDetailsList)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientHistoryDetails");
                    this.dbServer.AddInParameter(command, "LinkServer", DbType.String, nvo.LinkServer);
                    if (nvo.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command, "PatientEMRdata", DbType.Int64, patientEMRData.ID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, nvo.Status);
                    this.dbServer.AddInParameter(command, "ControlCaption", DbType.String, svo.ControlCaption);
                    this.dbServer.AddInParameter(command, "ControlName", DbType.String, svo.ControlName);
                    this.dbServer.AddInParameter(command, "ControlType", DbType.String, svo.ControlType);
                    this.dbServer.AddInParameter(command, "ControlUnit", DbType.String, svo.ControlUnit);
                    this.dbServer.AddInParameter(command, "ControlHeader", DbType.String, svo.Header);
                    this.dbServer.AddInParameter(command, "ControlSection", DbType.String, svo.Section);
                    this.dbServer.AddInParameter(command, "Doctorid", DbType.Int64, patientEMRData.DoctorID);
                    this.dbServer.AddInParameter(command, "Value", DbType.String, svo.Value);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                    {
                        throw new Exception();
                    }
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                throw exception;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientIVFHistoryDataAndDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientIVFHistoryDataAndDetailsBizActionVO nvo = valueObject as clsAddUpdatePatientIVFHistoryDataAndDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPatientEMRDataVO patientEMRData = nvo.PatientEMRData;
                if (patientEMRData != null)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIVFPatientEMRData");
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, patientEMRData.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, patientEMRData.PatientUnitID);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, patientEMRData.VisitID);
                    this.dbServer.AddInParameter(command, "Doctorid", DbType.String, patientEMRData.DoctorID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.String, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "SavedBy", DbType.String, patientEMRData.SavedBy);
                    this.dbServer.AddInParameter(command, "IsIvfHistory", DbType.Boolean, nvo.ISIvfhistory);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, patientEMRData.UnitId);
                    this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "IvfID", DbType.Int64, nvo.SaveIvfID);
                    this.dbServer.AddInParameter(command, "TakenBy", DbType.Int64, nvo.TakenBy);
                    this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientEMRData.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    patientEMRData.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateIVFPatientHistoryDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IvfID", DbType.Int64, nvo.SaveIvfID);
                this.dbServer.AddInParameter(storedProcCommand, "Isopdipd", DbType.Int64, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "IsIvfHistory", DbType.Boolean, nvo.ISIvfhistory);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                foreach (clsPatientEMRDetailsVO svo in nvo.PatientHistoryDetailsList)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIVFPatientHistoryDetails");
                    this.dbServer.AddInParameter(command, "LinkServer", DbType.String, nvo.LinkServer);
                    if (nvo.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PatientEMRdata", DbType.Int64, patientEMRData.ID);
                    this.dbServer.AddInParameter(command, "IvfID", DbType.Int64, nvo.SaveIvfID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, nvo.Status);
                    this.dbServer.AddInParameter(command, "ControlCaption", DbType.String, svo.ControlCaption);
                    this.dbServer.AddInParameter(command, "ControlName", DbType.String, svo.ControlName);
                    this.dbServer.AddInParameter(command, "ControlType", DbType.String, svo.ControlType);
                    this.dbServer.AddInParameter(command, "Tab", DbType.String, nvo.Tab);
                    this.dbServer.AddInParameter(command, "Value", DbType.String, svo.Value);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                    {
                        throw new Exception();
                    }
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                throw exception;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientOTDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientOTDetailsBizActionVO nvo = valueObject as clsAddUpdatePatientOTDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection myConnection = null;
            try
            {
                myConnection = this.dbServer.CreateConnection();
                if (myConnection.State != ConnectionState.Open)
                {
                    myConnection.Open();
                }
                transaction = myConnection.BeginTransaction();
                clsPatientEMRDataVO patientPhysicalExamData = nvo.PatientPhysicalExamData;
                if (patientPhysicalExamData != null)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTData");
                    this.dbServer.AddInParameter(command, "SceduleID", DbType.Int64, patientPhysicalExamData.ScheduleID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientPhysicalExamData.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    patientPhysicalExamData.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteOTTemplatedetails");
                this.dbServer.AddInParameter(storedProcCommand, "SecduleID", DbType.Int64, patientPhysicalExamData.ScheduleID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "EMrID", DbType.Int64, patientPhysicalExamData.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                foreach (clsPatientEMRDetailsVO svo in nvo.PatientPhysicalExamDetailsList)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateOTTemplateDetails");
                    this.dbServer.AddInParameter(command, "PatientEmrDataId", DbType.String, patientPhysicalExamData.ID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, svo.TemplateID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, svo.Status);
                    this.dbServer.AddInParameter(command, "Header", DbType.String, svo.Header);
                    this.dbServer.AddInParameter(command, "ControlUnit", DbType.String, svo.ControlUnit);
                    this.dbServer.AddInParameter(command, "Section", DbType.String, svo.Section);
                    this.dbServer.AddInParameter(command, "ControlCaption", DbType.String, svo.ControlCaption);
                    this.dbServer.AddInParameter(command, "ControlName", DbType.String, svo.ControlName);
                    this.dbServer.AddInParameter(command, "ControlType", DbType.String, svo.ControlType);
                    this.dbServer.AddInParameter(command, "Value", DbType.String, svo.Value);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                    {
                        throw new Exception();
                    }
                }
                foreach (ListItems2 items in nvo.UploadImg)
                {
                    string str = this.GetImageName(nvo.TemplateID, patientPhysicalExamData.ScheduleID, UserVo.UserLoginInfo.UnitId, myConnection, transaction);
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddImageForOT");
                    this.dbServer.AddInParameter(command, "SceduleID", DbType.Int64, patientPhysicalExamData.ScheduleID);
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int32, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "ImgPath", DbType.String, str);
                    this.dbServer.AddInParameter(command, "EMRID", DbType.Int64, patientPhysicalExamData.ID);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    MemoryStream stream = new MemoryStream(items.Photo);
                    Image original = Image.FromStream(stream);
                    Image image2 = new Bitmap(original, new Size(150, 120));
                    ImageConverter converter1 = new ImageConverter();
                    MemoryStream stream1 = new MemoryStream();
                    ImageCodecInfo info = this.GetEncoder(ImageFormat.Jpeg);
                    Encoder quality = Encoder.Quality;
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    EncoderParameter parameter = new EncoderParameter(quality, (long) 50);
                    encoderParams.Param[0] = parameter;
                    image2.Save(this.ImgSaveLocation + str, info, encoderParams);
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (myConnection.State != ConnectionState.Closed)
                {
                    myConnection.Close();
                }
                myConnection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientPhysicalExamDataAndDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientPhysicalExamDataAndDetailsBizActionVO nvo = valueObject as clsAddUpdatePatientPhysicalExamDataAndDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                clsPatientEMRDataVO patientPhysicalExamData = nvo.PatientPhysicalExamData;
                if (patientPhysicalExamData != null)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRData");
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, patientPhysicalExamData.PatientID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, nvo.TemplateID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "DoctorId", DbType.String, patientPhysicalExamData.DoctorID);
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, patientPhysicalExamData.VisitID);
                    this.dbServer.AddInParameter(command, "SavedBy", DbType.String, patientPhysicalExamData.SavedBy);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "TakenBy", DbType.Int64, nvo.TakenBy);
                    this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, patientPhysicalExamData.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    patientPhysicalExamData.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientPhysicalExamDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.String, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsPatientEMRDetailsVO svo in nvo.PatientPhysicalExamDetailsList)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientPhysicalExamDetails");
                    this.dbServer.AddInParameter(command, "LinkServer", DbType.String, nvo.LinkServer);
                    if (nvo.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(command, "Doctorid", DbType.String, nvo.DoctorId);
                    this.dbServer.AddInParameter(command, "PatientEmrDataId", DbType.String, patientPhysicalExamData.ID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, svo.TemplateID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, svo.Status);
                    this.dbServer.AddInParameter(command, "Header", DbType.String, svo.Header);
                    this.dbServer.AddInParameter(command, "ControlUnit", DbType.String, svo.ControlUnit);
                    this.dbServer.AddInParameter(command, "Section", DbType.String, svo.Section);
                    this.dbServer.AddInParameter(command, "ControlCaption", DbType.String, svo.ControlCaption);
                    this.dbServer.AddInParameter(command, "ControlName", DbType.String, svo.ControlName);
                    this.dbServer.AddInParameter(command, "ControlType", DbType.String, svo.ControlType);
                    this.dbServer.AddInParameter(command, "Value", DbType.String, svo.Value);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                    {
                        throw new Exception();
                    }
                }
                nvo.SuccessStatus = 1;
                transaction.Commit();
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return valueObject;
        }

        public override IValueObject DeleteCPOEMedicine(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCPOEMedicineBizActionVO nvo = valueObject as clsDeleteCPOEMedicineBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteCPOEDrug");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                storedProcCommand.Dispose();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteCPOEService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteCPOEServiceBizActionVO nvo = valueObject as clsDeleteCPOEServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteCPOEService");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SuccessStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                storedProcCommand.Dispose();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetEMRFieldValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFieldValueMasterBizActionVO nvo = valueObject as clsGetFieldValueMasterBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFeildValue_EMR");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objFieldMasterDetails == null)
                    {
                        nvo.objFieldMasterDetails = new List<clsFieldValuesMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsFieldValuesMasterVO item = new clsFieldValuesMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UsedFor = Convert.ToString(DALHelper.HandleDBNull(reader["UsedFor"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.objFieldMasterDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageDecoders())
            {
                if (info.FormatID == format.Guid)
                {
                    return info;
                }
            }
            return null;
        }

        public override IValueObject GetImage(IValueObject valueObject, clsUserVO UserVo)
        {
            ClsGetImageBizActionVO nvo = (ClsGetImageBizActionVO) valueObject;
            try
            {
                if (File.Exists(nvo.ImagePath))
                {
                    nvo.ImageByte = File.ReadAllBytes(nvo.ImagePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public string GetImageName(long TemplateID, long PatientID, long UnitID, DbConnection myConnection, DbTransaction myTransaction)
        {
            string str = null;
            string str2;
            string[] strArray = new string[] { "OTOtherDetails-", Convert.ToString(PatientID), "-", Convert.ToString(UnitID), "-", Convert.ToString((long) new Random().Next(0x1b207, 0xa2c2a)), ".png" };
            str = string.Concat(strArray);
            DbConnection connection = null;
            DbTransaction transaction = null;
            connection = (myConnection == null) ? this.dbServer.CreateConnection() : myConnection;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            transaction = (myTransaction == null) ? connection.BeginTransaction() : myTransaction;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckImageDupliactionForOT");
                this.dbServer.AddInParameter(storedProcCommand, "ImgName", DbType.String, str);
                if (Convert.ToInt32(this.dbServer.ExecuteScalar(storedProcCommand, transaction)) != 0)
                {
                    str2 = str;
                }
                else
                {
                    this.GetImageName(TemplateID, PatientID, UnitID, connection, transaction);
                    return "";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return str2;
        }

        public string GetImageName(long TemplateID, long PatientID, long VisitID, long UnitID, bool OPDIPD)
        {
            string str = null;
            long num = new Random().Next(0x1b207, 0xa2c2a);
            if (TemplateID == 0x17)
            {
                string[] strArray = new string[] { "HystroScopy-", Convert.ToString(PatientID), "-", Convert.ToString(VisitID), "-", Convert.ToString(UnitID), "-", Convert.ToString(num), "-" };
                strArray[9] = Convert.ToString(OPDIPD);
                strArray[10] = ".png";
                str = string.Concat(strArray);
            }
            else
            {
                string[] strArray2 = new string[] { "LaproScopy-", Convert.ToString(PatientID), "-", Convert.ToString(VisitID), "-", Convert.ToString(UnitID), "-", Convert.ToString(num), "-" };
                strArray2[9] = Convert.ToString(OPDIPD);
                strArray2[10] = ".png";
                str = string.Concat(strArray2);
            }
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckImageDupliaction");
                this.dbServer.AddInParameter(storedProcCommand, "ImgName", DbType.String, str);
                if (Convert.ToInt32(this.dbServer.ExecuteScalar(storedProcCommand)) != 0)
                {
                    return str;
                }
                else
                {
                    this.GetImageName(TemplateID, PatientID, VisitID, UnitID, OPDIPD);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            return "";
        }

        public override IValueObject GetItemsNServiceBySelectedDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCPOEServicItemDiagnosisWiseBizActionVO nvo = valueObject as clsGetCPOEServicItemDiagnosisWiseBizActionVO;
            try
            {
                if (nvo.DiagnosisIds != "")
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemDetailsByDiagnosisID");
                    this.dbServer.AddInParameter(storedProcCommand, "DiagnosisId", DbType.String, nvo.DiagnosisIds);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.ItemBySelectedDiagnosisDetailList == null)
                        {
                            nvo.ItemBySelectedDiagnosisDetailList = new List<clsGetItemBySelectedDiagnosisVO>();
                        }
                        while (reader.Read())
                        {
                            clsGetItemBySelectedDiagnosisVO item = new clsGetItemBySelectedDiagnosisVO {
                                DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                DrugID = DALHelper.HandleIntegerNull(reader["ItemID"]),
                                RouteID = (long) DALHelper.HandleDBNull(reader["RouteID"]),
                                SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]))
                            };
                            nvo.ItemBySelectedDiagnosisDetailList.Add(item);
                        }
                    }
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceDetailsByDiagnosisID");
                    this.dbServer.AddInParameter(storedProcCommand, "DiagnosisId", DbType.String, nvo.DiagnosisIds);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.ServiceBySelectedDiagnosisDetailList == null)
                        {
                            nvo.ServiceBySelectedDiagnosisDetailList = new List<clsGetServiceBySelectedDiagnosisVO>();
                        }
                        while (reader.Read())
                        {
                            clsGetServiceBySelectedDiagnosisVO item = new clsGetServiceBySelectedDiagnosisVO {
                                ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["status"]),
                                ServiceRate = (decimal) DALHelper.HandleDBNull(reader["ServiceRate"])
                            };
                            nvo.ServiceBySelectedDiagnosisDetailList.Add(item);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientAllVisit(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientAllVisitBizActionVO nvo = valueObject as clsGetPatientAllVisitBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientAllVisit");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IsfemaleTemplate", DbType.Boolean, nvo.IsFemaleTemplate);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "ISFromNursing", DbType.Boolean, nvo.ISFromNursing);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID1);
                this.dbServer.AddInParameter(storedProcCommand, "IsPhysicalExamination", DbType.Int64, nvo.IsPhysicalExamination);
                this.dbServer.AddInParameter(storedProcCommand, "AllTemplateID", DbType.String, nvo.AllTemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VisitList == null)
                    {
                        nvo.VisitList = new List<clsVisitVO>();
                    }
                    while (reader.Read())
                    {
                        clsVisitVO item = new clsVisitVO {
                            ArtDashboardDate = Convert.ToString(DALHelper.HandleDBNull(reader["Date"])),
                            VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"])),
                            HistoryFlag = Convert.ToString(DALHelper.HandleDBNull(reader["histoyFlag"])),
                            IsOPDIPDFlag = Convert.ToString(DALHelper.HandleDBNull(reader["ISOPDIPDFLAG"])),
                            TakenBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["TakenBy"])),
                            LoginNm = Convert.ToString(DALHelper.HandleDBNull(reader["loginnm"]))
                        };
                        if (nvo.TemplateID == 0x3f)
                        {
                            item.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        }
                        if (nvo.ISFromNursing)
                        {
                            item.EMRID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EMRID"]));
                            item.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        }
                        nvo.VisitList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject GetPatientCompoundPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCompoundPrescriptionBizActionVO nvo = valueObject as clsGetCompoundPrescriptionBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCompoundPrescription");
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                if (nvo.CoumpoundDrugList == null)
                {
                    nvo.CoumpoundDrugList = new List<clsPatientPrescriptionDetailVO>();
                }
                while (reader.Read())
                {
                    clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                        DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                        DrugCode = Convert.ToString(reader["ItemCode"]),
                        Frequency = Convert.ToString(reader["Frequency"]),
                        sCompoundQuantity = Convert.ToString(reader["CompoundQty"]),
                        sComponentQuantity = Convert.ToString(reader["ComponentQty"]),
                        CompoundDrugID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["CompoundDrugID"])),
                        CompoundDrug = Convert.ToString(reader["CompoundDrug"]),
                        Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"])),
                        AvailableStock = Convert.ToDouble(reader["AvailableStock"])
                    };
                    nvo.CoumpoundDrugList.Add(item);
                }
            }
            return nvo;
        }

        public override IValueObject GetPatientCPOEDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCPOEDetailsBizActionVO nvo = valueObject as clsGetPatientCPOEDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionForCPOE");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PrescriptionID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        nvo.Advice = (string) DALHelper.HandleDBNull(reader["Advice"]);
                        nvo.FollowupDate = DALHelper.HandleDate(reader["FollowUpDate"]);
                        nvo.FollowUpRemark = (string) DALHelper.HandleDBNull(reader["FollowUpRemark"]);
                    }
                }
                if (nvo.PrescriptionID >= 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionForCPOEDetails");
                    this.dbServer.AddInParameter(command2, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command2, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(command2, "Doctorid", DbType.Int64, nvo.DoctorID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientPrescriptionDetailList == null)
                        {
                            nvo.PatientPrescriptionDetailList = new List<clsPatientPrescriptionDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                                SelectedItem = { 
                                    ID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                                    Description = (string) DALHelper.HandleDBNull(reader["ItemName"])
                                },
                                DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                DrugID = DALHelper.HandleIntegerNull(reader["DrugID"]),
                                SelectedFrequency = { Description = (string) DALHelper.HandleDBNull(reader["Frequency"]) },
                                SelectedInstruction = { Description = (string) DALHelper.HandleDBNull(reader["Reason"]) },
                                Instruction = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                NewInstruction = (string) DALHelper.HandleDBNull(reader["Instruction"]),
                                SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                Quantity = (int) DALHelper.HandleDBNull(reader["Quantity"]),
                                Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                                IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                                FromHistory = false,
                                Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                                UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])),
                                Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"])),
                                ArtEnabled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["ARTEnables"])),
                                DrugSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugSourceId"])),
                                PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"])),
                                PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]))
                            };
                            nvo.PatientPrescriptionDetailList.Add(item);
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientCPOEPrescriptionDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCPOEPrescriptionDetailsBizActionVO nvo = valueObject as clsGetPatientCPOEPrescriptionDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionID");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PrescriptionID = (long) DALHelper.HandleDBNull(reader["ID"]);
                    }
                }
                storedProcCommand.Dispose();
                if (nvo.PrescriptionID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionForCPOEDetails");
                    this.dbServer.AddInParameter(command2, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientPrescriptionDetailList == null)
                        {
                            nvo.PatientPrescriptionDetailList = new List<clsPatientPrescriptionDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                                SelectedItem = { 
                                    ID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                                    Description = (string) DALHelper.HandleDBNull(reader["ItemName"])
                                },
                                DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                DrugID = DALHelper.HandleIntegerNull(reader["DrugID"]),
                                SelectedFrequency = { Description = (string) DALHelper.HandleDBNull(reader["Frequency"]) },
                                SelectedInstruction = { Description = (string) DALHelper.HandleDBNull(reader["Reason"]) },
                                Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                                SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                                Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                NewInstruction = (string) DALHelper.HandleDBNull(reader["Instruction"]),
                                Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                                Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                                IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                                FromHistory = false,
                                Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]))
                            };
                            nvo.PatientPrescriptionDetailList.Add(item);
                        }
                    }
                    reader.Close();
                    command2.Dispose();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientCurrentMedicationDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCurrentMedicationDetailListBizActionVO nvo = valueObject as clsGetPatientCurrentMedicationDetailListBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationDetailList");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Int64, nvo.IsOPDIPD);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                if (nvo.PatientCurrentMedicationDetailList == null)
                {
                    nvo.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                }
                while (reader.Read())
                {
                    clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                        PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"])),
                        SelectedItem = { 
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]))
                        },
                        DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                        DrugID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DrugID"])),
                        SelectedFrequency = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])) },
                        SelectedInstruction = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"])) },
                        Dose = Convert.ToString(DALHelper.HandleDBNull(reader["Dose"])),
                        SelectedRoute = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])) },
                        Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                        Reason = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"])),
                        Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                        Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                        IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                        Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                        Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])),
                        Instruction = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"])),
                        Year = Convert.ToString(DALHelper.HandleDBNull(reader["Pyear"]))
                    };
                    nvo.PatientCurrentMedicationDetailList.Add(item);
                }
            }
            return nvo;
        }

        public override IValueObject GetPatientCurrentMedicationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCurrentMedicationDetailsBizActionVO nvo = valueObject as clsGetPatientCurrentMedicationDetailsBizActionVO;
            try
            {
                DbDataReader reader;
                if (!nvo.IsPrevious)
                {
                    if (nvo.IsFromPresc)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionDetailsForCPOE");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader.HasRows)
                        {
                            if (nvo.PatientCurrentMedicationDetailList == null)
                            {
                                nvo.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                            }
                            while (reader.Read())
                            {
                                clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                                    SelectedItem = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["ItemName"])
                                    },
                                    DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                    DrugID = DALHelper.HandleIntegerNull(reader["DrugID"]),
                                    SelectedFrequency = { Description = (string) DALHelper.HandleDBNull(reader["Frequency"]) },
                                    SelectedInstruction = { Description = (string) DALHelper.HandleDBNull(reader["Reason"]) },
                                    Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                                    SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                    Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                                    Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                    Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                                    Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                                    IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                                    FromHistory = false,
                                    Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                                    Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])),
                                    Instruction = (string) DALHelper.HandleDBNull(reader["Reason"])
                                };
                                nvo.PatientCurrentMedicationDetailList.Add(item);
                            }
                        }
                    }
                    else
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationDetais");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                        reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader.HasRows)
                        {
                            if (nvo.PatientCurrentMedicationDetailList == null)
                            {
                                nvo.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                            }
                            while (reader.Read())
                            {
                                clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                                    SelectedItem = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["ItemName"])
                                    },
                                    DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                    DrugID = DALHelper.HandleIntegerNull(reader["DrugID"]),
                                    SelectedFrequency = { Description = (string) DALHelper.HandleDBNull(reader["Frequency"]) },
                                    SelectedInstruction = { Description = (string) DALHelper.HandleDBNull(reader["Reason"]) },
                                    Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                                    SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                    Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                                    Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                    Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                                    Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                                    IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                                    FromHistory = false,
                                    Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                                    Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])),
                                    Instruction = (string) DALHelper.HandleDBNull(reader["Reason"])
                                };
                                nvo.PatientCurrentMedicationDetailList.Add(item);
                            }
                        }
                    }
                }
                else if (nvo.IsFromPresc)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionByPrescriptionIDDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientCurrentMedicationDetailList == null)
                        {
                            nvo.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                                SelectedItem = { 
                                    ID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                                    Description = (string) DALHelper.HandleDBNull(reader["ItemName"])
                                },
                                DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                DrugID = DALHelper.HandleIntegerNull(reader["DrugID"]),
                                SelectedFrequency = { Description = (string) DALHelper.HandleDBNull(reader["Frequency"]) },
                                SelectedInstruction = { Description = (string) DALHelper.HandleDBNull(reader["Reason"]) },
                                Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                                SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                                Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                                Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                                IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                                FromHistory = false,
                                Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                                Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])),
                                Instruction = (string) DALHelper.HandleDBNull(reader["Reason"])
                            };
                            nvo.PatientCurrentMedicationDetailList.Add(item);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentMedicationByPrescriptionIDDetais");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromPresc", DbType.Boolean, nvo.IsFromPresc);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PrescriptionID", DbType.Int64, nvo.PrescriptionID);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.PatientCurrentMedicationDetailList == null)
                        {
                            nvo.PatientCurrentMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PrescriptionID = (long) DALHelper.HandleDBNull(reader["PrescriptionID"]),
                                SelectedItem = { 
                                    ID = (long) DALHelper.HandleDBNull(reader["DrugID"]),
                                    Description = (string) DALHelper.HandleDBNull(reader["ItemName"])
                                },
                                DrugName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                DrugID = DALHelper.HandleIntegerNull(reader["DrugID"]),
                                SelectedFrequency = { Description = (string) DALHelper.HandleDBNull(reader["Frequency"]) },
                                SelectedInstruction = { Description = (string) DALHelper.HandleDBNull(reader["Reason"]) },
                                Dose = (string) DALHelper.HandleDBNull(reader["Dose"]),
                                SelectedRoute = { Description = (string) DALHelper.HandleDBNull(reader["Route"]) },
                                Quantity = DALHelper.HandleDoubleNull(reader["Quantity"]),
                                Reason = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]))),
                                Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                                IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"])),
                                FromHistory = false,
                                Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])),
                                Route = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])),
                                Instruction = (string) DALHelper.HandleDBNull(reader["Reason"]),
                                UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])),
                                UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOMID"]))
                            };
                            nvo.PatientCurrentMedicationDetailList.Add(item);
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientEMRData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRDataBizActionVO nvo = valueObject as clsGetPatientEMRDataBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                if (!nvo.IsPrevious)
                {
                    storedProcCommand = !nvo.IsHistory ? this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRData") : this.dbServer.GetStoredProcCommand("CIMS_GetPatientHistoryEMRData");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPreviousVisitPatientEMRData");
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (!reader.HasRows)
                {
                    if (nvo.IsPrevious)
                    {
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
                else
                {
                    while (reader.Read())
                    {
                        nvo.objPatientEMRData = new clsPatientEMRDataVO();
                        nvo.objPatientEMRData.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.objPatientEMRData.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        nvo.objPatientEMRData.TemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"]);
                        nvo.objPatientEMRData.TemplateByNurse = (string) DALHelper.HandleDBNull(reader["TemplateByNurse"]);
                        nvo.objPatientEMRData.TemplateByDoctor = (string) DALHelper.HandleDBNull(reader["TemplateByDoctor"]);
                        nvo.objPatientEMRData.HistoryTemplate = (string) DALHelper.HandleDBNull(reader["HistoryTemplate"]);
                        nvo.objPatientEMRData.VisitID = (long) DALHelper.HandleDBNull(reader["VisitID"]);
                        nvo.objPatientEMRData.SavedBy = (string) DALHelper.HandleDBNull(reader["SavedBy"]);
                        nvo.objPatientEMRData.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.objPatientEMRData.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.objPatientEMRData.CreatedUnitID = (long) DALHelper.HandleDBNull(reader["CreatedUnitID"]);
                        nvo.objPatientEMRData.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.objPatientEMRData.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.objPatientEMRData.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.objPatientEMRData.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.objPatientEMRData.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.objPatientEMRData.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.objPatientEMRData.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.objPatientEMRData.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientEMRDataDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRDetailsBizActionVO nvo = valueObject as clsGetPatientEMRDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Tab", DbType.String, nvo.Tab);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.EMRDetailsList == null)
                    {
                        nvo.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.EMRImgList == null)
                            {
                                nvo.EMRImgList = new List<clsPatientEMRDetailsVO>();
                            }
                            while (reader.Read())
                            {
                                clsPatientEMRDetailsVO svo2 = new clsPatientEMRDetailsVO {
                                    ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"])),
                                    ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"])),
                                    ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"])),
                                    Value1 = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                };
                                nvo.EMRImgList.Add(svo2);
                            }
                            break;
                        }
                        clsPatientEMRDetailsVO item = new clsPatientEMRDetailsVO {
                            ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"])),
                            ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"])),
                            Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"])),
                            ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]))
                        };
                        nvo.EMRDetailsList.Add(item);
                    }
                }
                reader.Close();
                storedProcCommand.Dispose();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientEMRIvfHistoryDataDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRIvfhistoryDetailsBizActionVO nvo = valueObject as clsGetPatientEMRIvfhistoryDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRIvfHistoryDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Tab", DbType.String, nvo.Tab);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "EMRID", DbType.Int64, nvo.EMRID);
                this.dbServer.AddInParameter(storedProcCommand, "ISFromNursing", DbType.Boolean, nvo.ISFromNursing);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.EMRDetailsList == null)
                    {
                        nvo.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.EMRImgList == null)
                            {
                                nvo.EMRImgList = new List<clsPatientEMRDetailsVO>();
                            }
                            while (reader.Read())
                            {
                                clsPatientEMRDetailsVO svo2 = new clsPatientEMRDetailsVO {
                                    ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"])),
                                    ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"])),
                                    ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"])),
                                    Value1 = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                };
                                nvo.EMRImgList.Add(svo2);
                            }
                            break;
                        }
                        clsPatientEMRDetailsVO item = new clsPatientEMRDetailsVO {
                            ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"])),
                            ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"])),
                            Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"])),
                            ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]))
                        };
                        nvo.EMRDetailsList.Add(item);
                    }
                }
                reader.Close();
                storedProcCommand.Dispose();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientEMRPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRPhysicalExamDetailsBizActionVO nvo = valueObject as clsGetPatientEMRPhysicalExamDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRPhysicalExamDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Tab", DbType.String, nvo.Tab);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "ISFromOTDetails", DbType.Boolean, nvo.ISFromOTDetails);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.EMRDetailsList == null)
                    {
                        nvo.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO item = new clsPatientEMRDetailsVO {
                            ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"])),
                            ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"])),
                            Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"])),
                            ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]))
                        };
                        nvo.EMRDetailsList.Add(item);
                    }
                }
                reader.Close();
                storedProcCommand.Dispose();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPatientEMRUploadedFiles(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRUploadedFilesBizActionVO nvo = valueObject as clsGetPatientEMRUploadedFilesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRUploadFiles");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ControlName", DbType.String, nvo.ControlName);
                this.dbServer.AddInParameter(storedProcCommand, "ControlIndex", DbType.Int32, nvo.ControlIndex);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.objPatientEMRUploadedFiles == null)
                        {
                            nvo.objPatientEMRUploadedFiles = new List<clsPatientEMRUploadedFilesVO>();
                        }
                        clsPatientEMRUploadedFilesVO item = new clsPatientEMRUploadedFilesVO {
                            ControlCaption = (string) DALHelper.HandleDBNull(reader["ControlCaption"]),
                            ControlName = (string) DALHelper.HandleDBNull(reader["ControlName"]),
                            ControlType = (string) DALHelper.HandleDBNull(reader["ControlType"]),
                            ControlIndex = (int) DALHelper.HandleDBNull(reader["ControlIndex"]),
                            Value = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                        };
                        nvo.objPatientEMRUploadedFiles.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientFeedback(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFeedbackBizActionVO nvo = valueObject as clsGetPatientFeedbackBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientFeedback");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.objPatientFeedback = new clsPatientFeedbackVO();
                        nvo.objPatientFeedback.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.objPatientFeedback.PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]);
                        nvo.objPatientFeedback.PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        nvo.objPatientFeedback.Feedback = (string) DALHelper.HandleDBNull(reader["Feedback"]);
                        nvo.objPatientFeedback.VisitID = (long) DALHelper.HandleDBNull(reader["VisitID"]);
                        nvo.objPatientFeedback.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.objPatientFeedback.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.objPatientFeedback.CreatedUnitID = (long) DALHelper.HandleDBNull(reader["CreatedUnitID"]);
                        nvo.objPatientFeedback.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.objPatientFeedback.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.objPatientFeedback.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.objPatientFeedback.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.objPatientFeedback.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.objPatientFeedback.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.objPatientFeedback.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.objPatientFeedback.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientFollowUpDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFollowUpDetailsBizActionVO nvo = valueObject as clsGetPatientFollowUpDetailsBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUpDetails");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOpdIpd", DbType.Int64, nvo.Isopdipd);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    nvo.PrescriptionID = (long) DALHelper.HandleDBNull(reader["ID"]);
                    nvo.FollowUpAdvice = (string) DALHelper.HandleDBNull(reader["Advice"]);
                    nvo.FollowupDate = DALHelper.HandleDate(reader["FollowUpDate"]);
                    nvo.FollowUpRemark = (string) DALHelper.HandleDBNull(reader["FollowUpRemark"]);
                    nvo.NoFollowReq = (bool) DALHelper.HandleDBNull(reader["NoFollowReq"]);
                    nvo.AppoinmentReson = (long) DALHelper.HandleDBNull(reader["AppoinmentReson"]);
                }
            }
            reader.Close();
            storedProcCommand.Dispose();
            return nvo;
        }

        public override IValueObject GetPatientFollowUpList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFollowUpDetailsBizActionVO nvo = (clsGetPatientFollowUpDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUpList");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.InputPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.InputStartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.InputMaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo);
                this.dbServer.AddInParameter(storedProcCommand, "@Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MrNo);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FollowUpDetailsList == null)
                    {
                        nvo.FollowUpDetailsList = new List<clsFollowUpVO>();
                    }
                    while (reader.Read())
                    {
                        clsFollowUpVO item = new clsFollowUpVO {
                            FollowUpID = Convert.ToInt64(reader["ID"]),
                            PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]))),
                            MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]))),
                            LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]))),
                            FamilyName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FamilyName"]))),
                            ContactNo1 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact1"])),
                            ContactNo2 = Convert.ToString(DALHelper.HandleDBNull(reader["Contact2"])),
                            MobileCountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["MobileCountryCode"])),
                            ResiNoCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResiNoCountryCode"])),
                            ResiSTDCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResiSTDCode"])),
                            FaxNo = Convert.ToString(DALHelper.HandleDBNull(reader["FaxNo"])),
                            Email = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["EmailId"]))),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            DepartmentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DepartmentId"])),
                            AppointmentReasonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["AppointmentReasonID"])),
                            AppointmentReason = Convert.ToString(DALHelper.HandleDBNull(reader["AppointmentReason"])),
                            FollowUpDate = DALHelper.HandleDate(reader["FollowUpDate"]),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            DoctorId = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])),
                            MrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            AddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["AddedDateTime"]))),
                            createdByName = Convert.ToString(DALHelper.HandleDBNull(reader["createdByName"])),
                            ModifiedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ModifiedByName"])),
                            IsAge = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAge"]))
                        };
                        if (item.IsAge)
                        {
                            item.DateOfBirthFromAge = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        }
                        else
                        {
                            item.DOB = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"])));
                        }
                        item.VisitDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])));
                        item.OPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                        item.VisitID = Convert.ToInt32(DALHelper.HandleDBNull(reader["VisitID"]));
                        item.ISAppointment = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ISAppointment"]));
                        nvo.FollowUpDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.OutputTotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientHistoryData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPatientHistoryDataBizActionVO nvo = valueObject as clsGetPatientPatientHistoryDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientHistoryData");
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                            break;
                        }
                        nvo.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.TemplateByNurse = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateByNurse"]));
                        nvo.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        nvo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        nvo.IsUpdated = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUpdated"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientIVFEMR(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientEMRSummaryDataBizActionVO nvo = valueObject as clsGetPatientEMRSummaryDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientIVFEMRData");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.SummaryList == null)
                        {
                            nvo.SummaryList = new List<clsPatientEMRDataVO>();
                        }
                        clsPatientEMRDataVO item = new clsPatientEMRDataVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            TemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"]),
                            TemplateByNurse = (string) DALHelper.HandleDBNull(reader["TemplateByNurse"]),
                            TemplateByDoctor = (string) DALHelper.HandleDBNull(reader["TemplateByDoctor"]),
                            VisitID = (long) DALHelper.HandleDBNull(reader["VisitID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"])
                        };
                        nvo.SummaryList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPatientIvfID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientIvfIDBizActionVO nvo = valueObject as clsGetPatientIvfIDBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientIvfID");
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    nvo.IvfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IvfID"]));
                }
            }
            return valueObject;
        }

        public override IValueObject GetPatientPastChiefComplaints(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastChiefComplaintsBizActionVO nvo = valueObject as clsGetPatientPastChiefComplaintsBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPastChiefComplaints");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                nvo.ChiefComplaintList = new List<clsChiefComplaintsVO>();
                while (reader.Read())
                {
                    clsChiefComplaintsVO item = new clsChiefComplaintsVO {
                        Description = Convert.ToString(reader["Description"]),
                        DoctorName = Convert.ToString(reader["DoctorName"]),
                        VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])),
                        DoctorSpec = Convert.ToString(reader["SpecializationName"])
                    };
                    nvo.ChiefComplaintList.Add(item);
                }
            }
            reader.Close();
            storedProcCommand.Dispose();
            return nvo;
        }

        public override IValueObject GetPatientPastcostNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastcostBizActionVO nvo = valueObject as clsGetPatientPastcostBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPastcostnotes");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
            this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
            this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
            this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                nvo.PastFollowUPList = new List<clsPastFollowUpnoteVO>();
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                        break;
                    }
                    clsPastFollowUpnoteVO item = new clsPastFollowUpnoteVO {
                        ID = Convert.ToInt64(reader["ID"]),
                        Notes = Convert.ToString(reader["notes"]),
                        Description = Convert.ToString(reader["Description"]),
                        DoctorName = Convert.ToString(reader["DoctorName"]),
                        VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])),
                        DoctorSpec = Convert.ToString(reader["SpecializationName"])
                    };
                    nvo.PastFollowUPList.Add(item);
                }
            }
            reader.Close();
            storedProcCommand.Dispose();
            return nvo;
        }

        public override IValueObject GetPatientPastFollowUPNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastFollowUPNotesBizActionVO nvo = valueObject as clsGetPatientPastFollowUPNotesBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPastfollowupnotes");
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
            this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
            this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
            this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            if (reader.HasRows)
            {
                nvo.PastFollowUPList = new List<clsPastFollowUpnoteVO>();
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                        break;
                    }
                    clsPastFollowUpnoteVO item = new clsPastFollowUpnoteVO {
                        ID = Convert.ToInt64(reader["ID"]),
                        Notes = Convert.ToString(reader["notes"]),
                        Description = Convert.ToString(reader["Description"]),
                        DoctorName = Convert.ToString(reader["DoctorName"]),
                        VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"])),
                        DoctorSpec = Convert.ToString(reader["SpecializationName"])
                    };
                    nvo.PastFollowUPList.Add(item);
                }
            }
            reader.Close();
            storedProcCommand.Dispose();
            return nvo;
        }

        public override IValueObject GetPatientPastMedicationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastMedicationDetailsBizActionVO nvo = valueObject as clsGetPatientPastMedicationDetailsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = null;
                if (nvo.IsForCompound)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCompoundMedicationDetails");
                }
                else if (!nvo.IsFromPresc)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPastMedicationDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPastPrescriptionDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorId", DbType.String, nvo.DoctorId);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "@startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "@maximumRows", DbType.Int64, nvo.MaximumRows);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientMedicationDetailList == null)
                    {
                        nvo.PatientMedicationDetailList = new List<clsPatientPrescriptionDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsPatientPrescriptionDetailVO item = new clsPatientPrescriptionDetailVO {
                            PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"])),
                            SelectedItem = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])) },
                            DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            SelectedFrequency = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"])) },
                            SelectedInstruction = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["Reason"])) },
                            SelectedRoute = { Description = Convert.ToString(DALHelper.HandleDBNull(reader["Route"])) }
                        };
                        if (nvo.IsFromPresc)
                        {
                            item.Instruction = Convert.ToString(reader["Instruction"]);
                            item.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                            item.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                            item.ArtEnabled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["ARTEnables"]));
                            item.DrugSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugSourceId"]));
                            item.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                            item.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
                        }
                        item.Frequency = Convert.ToString(reader["Frequency"]);
                        item.Days = new int?(Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"])));
                        if (!nvo.IsFromPresc)
                        {
                            item.Year = Convert.ToString(reader["year"]);
                        }
                        if (nvo.IsFromPresc && nvo.IsOPDIPD)
                        {
                            item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            item.DoctorSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpecilization"]));
                        }
                        if (!nvo.IsForCompound)
                        {
                            item.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                            item.PastPrescriptionQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        }
                        else
                        {
                            item.sComponentQuantity = Convert.ToString(reader["Quantity"]);
                            item.sCompoundQuantity = Convert.ToString(reader["CompdQuantity"]);
                            item.CompoundDrug = Convert.ToString(reader["CompoundDrug"]);
                        }
                        item.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                        item.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        nvo.PatientMedicationDetailList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }
            return nvo;
        }

        public override IValueObject GetPatientPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPhysicalExamDetailsBizActionVO nvo = valueObject as clsGetPatientPhysicalExamDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPhysicalExamDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                            break;
                        }
                        nvo.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.TemplateByNurse = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateByNurse"]));
                        nvo.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        nvo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        nvo.IsUpdated = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsUpdated"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionAndVisitDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPrescriptionandVisitDetailsBizActionVO nvo = valueObject as clsGetPatientPrescriptionandVisitDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionListForEMR");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromPresc", DbType.Boolean, nvo.IsFromPresc);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientVisitEMRDetails == null)
                    {
                        nvo.PatientVisitEMRDetails = new List<clsVisitEMRDetails>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsVisitEMRDetails item = new clsVisitEMRDetails {
                            VisitId = DALHelper.HandleIntegerNull(reader["VisitID"]),
                            PatientId = DALHelper.HandleIntegerNull(reader["PatientId"]),
                            PatientUnitId = DALHelper.HandleIntegerNull(reader["PatientUnitId"]),
                            UnitId = DALHelper.HandleIntegerNull(reader["UnitId"]),
                            PrescriptionID = DALHelper.HandleIntegerNull(reader["PrescriptionID"]),
                            DoctorCode = Convert.ToString(reader["DoctorID"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.VisitDate = nullable.Value;
                        item.OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]);
                        item.Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"]);
                        item.VisitType = (string) DALHelper.HandleDBNull(reader["VisitType"]);
                        item.Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"]);
                        item.VisitCenter = (string) DALHelper.HandleDBNull(reader["UnitName"]);
                        nvo.PatientVisitEMRDetails.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientPrescriptionAndVisitDetails_IPD(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPrescriptionandVisitDetailsBizActionVO nvo = valueObject as clsGetPatientPrescriptionandVisitDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientPrescriptionListForEMR_IPD");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientVisitEMRDetailsIPD == null)
                    {
                        nvo.PatientVisitEMRDetailsIPD = new List<clsVisitEMRDetails>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsVisitEMRDetails item = new clsVisitEMRDetails {
                            PrescriptionID = DALHelper.HandleIntegerNull(reader["PrescriptionID"])
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.VisitDate = nullable.Value;
                        item.Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"]);
                        item.Specialization = (string) DALHelper.HandleDBNull(reader["Specialization"]);
                        nvo.PatientVisitEMRDetailsIPD.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientVisitSummaryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientVisitSummaryListForEMRBizActionVO nvo = valueObject as clsGetPatientVisitSummaryListForEMRBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientSummaryListForEMR");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "unitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VisitEMRDetailsList == null)
                    {
                        nvo.VisitEMRDetailsList = new List<clsVisitEMRDetails>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsVisitEMRDetails item = new clsVisitEMRDetails {
                            VisitId = (long) DALHelper.HandleDBNull(reader["Id"]),
                            PatientId = (long) DALHelper.HandleDBNull(reader["PatientId"]),
                            Department = Convert.ToString(DALHelper.HandleDBNull(reader["DepartmentName"])),
                            VisitTypeID = (long) DALHelper.HandleDBNull(reader["VisitTypeID"]),
                            DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorID"])),
                            VisitDate = (DateTime) DALHelper.HandleDBNull(reader["Date"]),
                            OPDNO = (string) DALHelper.HandleDBNull(reader["OPDNO"]),
                            Doctor = (string) DALHelper.HandleDBNull(reader["Doctor"])
                        };
                        nvo.VisitEMRDetailsList.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetServicesCPOEDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCurrentServicesBizActionVO nvo = valueObject as clsGetPatientCurrentServicesBizActionVO;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCurrentServicesDetails");
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            nvo.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
            if (reader.HasRows)
            {
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        break;
                    }
                    clsDoctorSuggestedServiceDetailVO item = new clsDoctorSuggestedServiceDetailVO {
                        PrescriptionID = DALHelper.HandleIntegerNull(reader["PrescriptionID"]),
                        ServiceID = Convert.ToInt64(reader["ServiceID"]),
                        ServiceName = Convert.ToString(reader["ServiceName"]),
                        ServiceRate = Convert.ToDouble(reader["serviceRate"]),
                        ServiceCode = Convert.ToString(reader["ServiceCode"]),
                        PriorityIndex = Convert.ToInt32(reader["PriorityID"]),
                        GroupName = Convert.ToString(reader["GroupName"]),
                        SpecializationId = Convert.ToInt64(reader["SpecializationId"])
                    };
                    nvo.ServiceDetails.Add(item);
                }
            }
            return nvo;
        }

        public override IValueObject GetUsedForValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetUsedForMasterBizActionVO nvo = new clsGetUsedForMasterBizActionVO();
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUsedForValue_EMR");
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objFieldMasterDetails == null)
                    {
                        nvo.objFieldMasterDetails = new List<clsFieldValuesMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsFieldValuesMasterVO item = new clsFieldValuesMasterVO {
                            UsedFor = Convert.ToString(DALHelper.HandleDBNull(reader["UsedFor"]))
                        };
                        nvo.objFieldMasterDetails.Add(item);
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

        public override IValueObject GetVisitAdmission(IValueObject valueObject, clsUserVO UserVo)
        {
            int num = 3;
            ClsGetVisitAdmissionBizActionVO nvo = valueObject as ClsGetVisitAdmissionBizActionVO;
            if (nvo.DMSViewerVisitAdmissionList == null)
            {
                nvo.DMSViewerVisitAdmissionList = new List<clsDMSViewerVisitAdmissionVO>();
            }
            DbCommand storedProcCommand = null;
            storedProcCommand = this.dbServer.GetStoredProcCommand("DMS_GETFilesDetails");
            this.dbServer.AddInParameter(storedProcCommand, "MRNO", DbType.String, nvo.MRNO);
            this.dbServer.AddInParameter(storedProcCommand, "OPD_IPD", DbType.Int16, 0);
            DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
            clsDMSViewerVisitAdmissionVO item = new clsDMSViewerVisitAdmissionVO {
                ID = 1L,
                Date = DateTime.Now,
                DoctorName = "OPD",
                NoOfFiles = 0L,
                DepartmentName = "",
                PatientID = 0L,
                PatientUnitID = 0L,
                OPD_IPD_External = 0L
            };
            nvo.DMSViewerVisitAdmissionList.Add(item);
            clsDMSViewerVisitAdmissionVO nvo3 = new clsDMSViewerVisitAdmissionVO {
                ID = 2L,
                Date = DateTime.Now,
                DoctorName = "IPD",
                NoOfFiles = 0L,
                DepartmentName = "",
                PatientID = 0L,
                PatientUnitID = 0L,
                OPD_IPD_External = 1L
            };
            nvo.DMSViewerVisitAdmissionList.Add(nvo3);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    clsDMSViewerVisitAdmissionVO nvo4 = new clsDMSViewerVisitAdmissionVO {
                        ID = num,
                        Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                        DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                        NoOfFiles = Convert.ToInt32(DALHelper.HandleDBNull(reader["NoOfFiles"])),
                        PatientVisitId = Convert.ToInt32(DALHelper.HandleDBNull(reader["OpdIpdId"])),
                        PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                        ParentID = 1L,
                        OPD_IPD_External = 0L
                    };
                    num++;
                    nvo.DMSViewerVisitAdmissionList.Add(nvo4);
                    DbCommand command2 = null;
                    command2 = this.dbServer.GetStoredProcCommand("DMS_GetFilePathAndName");
                    this.dbServer.AddInParameter(command2, "VisitId", DbType.Int64, nvo4.PatientVisitId);
                    this.dbServer.AddInParameter(command2, "OPD_IPD", DbType.Int16, 0);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            clsDMSViewerVisitAdmissionVO nvo5 = new clsDMSViewerVisitAdmissionVO {
                                ID = num,
                                ImgName = Convert.ToString(DALHelper.HandleDBNull(reader2["Filename"])),
                                ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader2["ServerPathRegNo"])),
                                DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader2["Filename"])),
                                ParentID = nvo4.ID,
                                OPD_IPD_External = 0L
                            };
                            num++;
                            nvo.DMSViewerVisitAdmissionList.Add(nvo5);
                        }
                    }
                    reader2.Close();
                }
            }
            reader.Close();
            DbCommand command = null;
            command = this.dbServer.GetStoredProcCommand("DMS_GETFilesDetails");
            this.dbServer.AddInParameter(command, "MRNO", DbType.String, nvo.MRNO);
            this.dbServer.AddInParameter(command, "OPD_IPD", DbType.Boolean, true);
            DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(command);
            if (reader3.HasRows)
            {
                while (reader3.Read())
                {
                    clsDMSViewerVisitAdmissionVO nvo6 = new clsDMSViewerVisitAdmissionVO {
                        ID = num,
                        Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader3["Date"])),
                        DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader3["DoctorName"])),
                        NoOfFiles = Convert.ToInt32(DALHelper.HandleDBNull(reader3["NoOfFiles"])),
                        PatientVisitId = Convert.ToInt32(DALHelper.HandleDBNull(reader3["OpdIpdId"])),
                        PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientID"])),
                        OPD_IPD_External = 1L,
                        ParentID = 2L
                    };
                    num++;
                    DbCommand command4 = null;
                    command4 = this.dbServer.GetStoredProcCommand("DMS_GetFolderMaster");
                    DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(command4);
                    if (reader4.HasRows)
                    {
                        while (reader4.Read())
                        {
                            clsDMSViewerVisitAdmissionVO nvo7 = new clsDMSViewerVisitAdmissionVO {
                                ID = num,
                                DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader4["Description"])),
                                PatientID = nvo.PatientID,
                                FolderID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ID"])),
                                OPD_IPD_External = 1L,
                                ParentID = nvo6.ID
                            };
                            num++;
                            nvo.DMSViewerVisitAdmissionList.Add(nvo7);
                            DbCommand command5 = null;
                            command5 = this.dbServer.GetStoredProcCommand("DMS_GetFilePathForIPD");
                            this.dbServer.AddInParameter(command5, "FolderID", DbType.Int32, nvo7.FolderID);
                            this.dbServer.AddInParameter(command5, "AdmissionID", DbType.Int32, nvo6.PatientVisitId);
                            DbDataReader reader5 = (DbDataReader) this.dbServer.ExecuteReader(command5);
                            if (reader5.HasRows)
                            {
                                while (reader5.Read())
                                {
                                    clsDMSViewerVisitAdmissionVO nvo8 = new clsDMSViewerVisitAdmissionVO {
                                        ID = num,
                                        ImgName = Convert.ToString(DALHelper.HandleDBNull(reader5["Filename"])),
                                        ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader5["ServerPathFolder"])),
                                        DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader5["Filename"])),
                                        ParentID = nvo7.ID,
                                        OPD_IPD_External = 1L
                                    };
                                    num++;
                                    nvo.DMSViewerVisitAdmissionList.Add(nvo8);
                                }
                            }
                            reader5.Close();
                        }
                    }
                    reader4.Close();
                    nvo.DMSViewerVisitAdmissionList.Add(nvo6);
                }
            }
            reader3.Close();
            return nvo;
        }

        private clsAddUpdateFieldValueMasterBizActionVO UpdateEMRFieldValue(clsAddUpdateFieldValueMasterBizActionVO objItem, clsUserVO UserVo)
        {
            try
            {
                clsFieldValuesMasterVO objFieldMatserDetails = new clsFieldValuesMasterVO();
                objFieldMatserDetails = objItem.objFieldMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEMRValueMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objFieldMatserDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objFieldMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objFieldMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "UsedFor", DbType.String, objFieldMatserDetails.UsedFor);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objFieldMatserDetails.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                objItem.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return objItem;
        }

        public override IValueObject UpdatePatientEMRData(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientEMRDataBizActionVO nvo = valueObject as clsUpdatePatientEMRDataBizActionVO;
            try
            {
                clsPatientEMRDataVO patientEMRDataDetails = nvo.PatientEMRDataDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientEMRData");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientEMRDataDetails.LinkServer);
                if (patientEMRDataDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientEMRDataDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientEMRDataDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientEMRDataDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientEMRDataDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, patientEMRDataDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateByNurse", DbType.String, patientEMRDataDetails.TemplateByNurse);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateByDoctor", DbType.String, patientEMRDataDetails.TemplateByDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "HistoryTemplate", DbType.String, patientEMRDataDetails.HistoryTemplate);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientEMRDataDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "SavedBy", DbType.String, patientEMRDataDetails.SavedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientEMRDataDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientEMRDataDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdatePatientFeedback(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePatientFeedbackBizActionVO nvo = valueObject as clsUpdatePatientFeedbackBizActionVO;
            try
            {
                clsPatientFeedbackVO patientFeedbackDetails = nvo.PatientFeedbackDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientFeedback");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, patientFeedbackDetails.LinkServer);
                if (patientFeedbackDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, patientFeedbackDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, patientFeedbackDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, patientFeedbackDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, patientFeedbackDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Feedback", DbType.String, patientFeedbackDetails.Feedback);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, patientFeedbackDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, patientFeedbackDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, patientFeedbackDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject UpdateStatusEMRFieldValue(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusFieldValueMasterBizActionVO nvo = valueObject as clsUpdateStatusFieldValueMasterBizActionVO;
            try
            {
                clsFieldValuesMasterVO fieldStatus = nvo.FieldStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEMRFieldStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, fieldStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, fieldStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }
    }
}

