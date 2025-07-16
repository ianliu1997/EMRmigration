namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.EMR;
    using PalashDynamics.ValueObjects.EMR.GrowthChart;
    using PalashDynamics.ValueObjects.EMR.NewEMR;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;

    public class clsEMRDAL : clsBaseEMRDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsEMRDAL()
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

        public override IValueObject AddEMRTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddEMRTemplateBizActionVO nvo = valueObject as clsAddEMRTemplateBizActionVO;
            try
            {
                clsEMRTemplateVO eMRTemplateDetails = nvo.EMRTemplateDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddEMRTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, eMRTemplateDetails.LinkServer);
                if (eMRTemplateDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, eMRTemplateDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, eMRTemplateDetails.Title);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, eMRTemplateDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, eMRTemplateDetails.Template);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableCriteria", DbType.Int16, eMRTemplateDetails.ApplicableCriteria);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableToPhysicalExam", DbType.Boolean, eMRTemplateDetails.IsPhysicalExam);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableToOT", DbType.Boolean, eMRTemplateDetails.IsForOT);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateTypeID", DbType.String, eMRTemplateDetails.TemplateTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateType", DbType.String, eMRTemplateDetails.TemplateType);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateSubtypeID", DbType.String, eMRTemplateDetails.TemplateSubtypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateSubtype", DbType.String, eMRTemplateDetails.TemplateSubtype);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, eMRTemplateDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, eMRTemplateDetails.TemplateID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.EMRTemplateDetails.TemplateID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateCostNote(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientcostNoteBizActionVO nvo = valueObject as clsAddUpdatePatientcostNoteBizActionVO;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientcostNote");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpNote", DbType.String, nvo.CurrentFollowUpNotes.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.CurrentFollowUpNotes.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
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
            return nvo;
        }

        public override IValueObject AddUpdateDeleteDiagnosisDetailsBizAction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDeleteDiagnosisDetailsBizActionVO nvo = valueObject as clsAddUpdateDeleteDiagnosisDetailsBizActionVO;
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
                if (nvo.IsICD10)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteICD10DiagnosisDeatails");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    foreach (clsEMRAddDiagnosisVO svo in nvo.DiagnosisDetails)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDeleteICD10DiagnosisDeatails");
                        this.dbServer.AddOutParameter(command, "ID", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                        this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                        this.dbServer.AddInParameter(command, "DoctorId", DbType.Int32, nvo.DoctorID);
                        this.dbServer.AddInParameter(command, "Categori", DbType.String, svo.Code);
                        this.dbServer.AddInParameter(command, "DTD", DbType.String, svo.DTD);
                        this.dbServer.AddInParameter(command, "Class", DbType.String, svo.Class);
                        this.dbServer.AddInParameter(command, "DiagnosisName", DbType.String, svo.Diagnosis);
                        this.dbServer.AddInParameter(command, "DiagnosisTypeID", DbType.Int64, svo.SelectedDiagnosisType.ID);
                        this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, svo.TemplateID);
                        this.dbServer.AddInParameter(command, "TemplateName", DbType.String, svo.TemplateName);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        if (nvo.SuccessStatus != 1)
                        {
                            throw new Exception();
                        }
                        nvo.PatientDiagnosisID = (long) this.dbServer.GetParameterValue(command, "ID");
                        if (svo.listPatientEMRDetails != null)
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            this.dbServer.AddInParameter(command3, "VisitID", DbType.Int64, nvo.VisitID);
                            this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, nvo.PatientID);
                            this.dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, svo.TemplateID);
                            this.dbServer.AddInParameter(command3, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                            this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                            foreach (clsPatientEMRDetailsVO svo2 in svo.listPatientEMRDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientDiagnosisEMRDetails");
                                this.dbServer.AddInParameter(command4, "VisitID", DbType.Int64, nvo.VisitID);
                                this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, nvo.PatientID);
                                this.dbServer.AddInParameter(command4, "TemplateID", DbType.Int64, svo2.TemplateID);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command4, "ControlCaption", DbType.String, svo2.ControlCaption);
                                this.dbServer.AddInParameter(command4, "ControlName", DbType.String, svo2.ControlName);
                                this.dbServer.AddInParameter(command4, "ControlType", DbType.String, svo2.ControlType);
                                this.dbServer.AddInParameter(command4, "Value", DbType.String, svo2.Value);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        if (!svo.Status)
                        {
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            this.dbServer.AddInParameter(command5, "VisitID", DbType.Int64, nvo.VisitID);
                            this.dbServer.AddInParameter(command5, "PatientID", DbType.Int64, nvo.PatientID);
                            this.dbServer.AddInParameter(command5, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                            this.dbServer.AddInParameter(command5, "TemplateID", DbType.Int64, svo.TemplateID);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteDiagnosisDeatails");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.String, nvo.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    foreach (clsEMRAddDiagnosisVO svo3 in nvo.DiagnosisDetails)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDeletePatientEMRDiagnosisDeatails");
                        this.dbServer.AddOutParameter(command, "ID", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                        this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                        this.dbServer.AddInParameter(command, "Doctorid", DbType.String, nvo.DoctorID);
                        this.dbServer.AddInParameter(command, "Code", DbType.String, svo3.Code);
                        this.dbServer.AddInParameter(command, "Class", DbType.String, svo3.Class);
                        this.dbServer.AddInParameter(command, "ServiceCode", DbType.String, svo3.ServiceCode);
                        this.dbServer.AddInParameter(command, "IsICD9", DbType.Boolean, svo3.IsICD9);
                        this.dbServer.AddInParameter(command, "DiagnosisName", DbType.String, svo3.Diagnosis);
                        this.dbServer.AddInParameter(command, "DiagnosisTypeID", DbType.Int64, svo3.SelectedDiagnosisType.ID);
                        this.dbServer.AddInParameter(command, "TemplateName", DbType.String, svo3.TemplateName);
                        this.dbServer.AddInParameter(command, "TemplateID", DbType.Int64, svo3.TemplateID);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, svo3.Status);
                        this.dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                        this.dbServer.AddInParameter(command, "IsArt", DbType.Boolean, svo3.ArtEnabled);
                        this.dbServer.AddInParameter(command, "PlannedTreatmentId", DbType.Int64, svo3.PlanTreatmentId);
                        this.dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, svo3.BilledEnabled);
                        this.dbServer.AddInParameter(command, "IsClosed", DbType.Boolean, svo3.IsClosedEnabled);
                        this.dbServer.AddInParameter(command, "PriorityId", DbType.Int64, svo3.PriorityId);
                        this.dbServer.AddInParameter(command, "IsArtStatus", DbType.Boolean, svo3.IsArtStatus);
                        this.dbServer.AddInParameter(command, "IsPAC", DbType.Boolean, svo3.PACEnabled);
                        this.dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, svo3.PlanTherapyId);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitId", DbType.Int64, svo3.PlanTherapyUnitId);
                        this.dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, svo3.IsDonorCycle);
                        this.dbServer.AddInParameter(command, "DonorID", DbType.Int64, svo3.DonorID);
                        this.dbServer.AddInParameter(command, "DonarUnitID", DbType.Int64, svo3.DonarUnitID);
                        this.dbServer.AddInParameter(command, "CoupleMRNO", DbType.String, svo3.CoupleMRNO);
                        this.dbServer.AddInParameter(command, "IsSurrogacy", DbType.Boolean, svo3.IsSurrogate);
                        this.dbServer.AddInParameter(command, "SurrogateMrNo", DbType.String, svo3.SurrogateMRNO);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        if (nvo.SuccessStatus != 1)
                        {
                            throw new Exception();
                        }
                        nvo.PatientDiagnosisID = (long) this.dbServer.GetParameterValue(command, "ID");
                        if (svo3.IsSurrogate && ((nvo.objSurrogatedPatient != null) && (nvo.objSurrogatedPatient.Count > 0)))
                        {
                            foreach (clsPatientGeneralVO lvo in nvo.objSurrogatedPatient)
                            {
                                DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateSurrogateLinking");
                                this.dbServer.AddOutParameter(command8, "ID", DbType.Int64, 0x7fffffff);
                                this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command8, "SurrogateID", DbType.Int64, lvo.PatientID);
                                this.dbServer.AddInParameter(command8, "SurrogateUnitID", DbType.Int64, lvo.UnitId);
                                this.dbServer.AddInParameter(command8, "MrNo", DbType.String, lvo.MRNo);
                                this.dbServer.AddInParameter(command8, "PatientEMRDiagnosisID", DbType.Int64, nvo.PatientDiagnosisID);
                                this.dbServer.AddInParameter(command8, "PatientEMRDiagnosisUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddParameter(command8, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                                this.dbServer.ExecuteNonQuery(command8, transaction);
                            }
                        }
                        if (svo3.listPatientEMRDetails != null)
                        {
                            DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            this.dbServer.AddInParameter(command9, "VisitID", DbType.Int64, nvo.VisitID);
                            this.dbServer.AddInParameter(command9, "PatientID", DbType.Int64, nvo.PatientID);
                            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                            this.dbServer.AddInParameter(command9, "TemplateID", DbType.Int64, svo3.TemplateID);
                            this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command9, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                            foreach (clsPatientEMRDetailsVO svo4 in svo3.listPatientEMRDetails)
                            {
                                DbCommand command10 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientDiagnosisEMRDetails");
                                this.dbServer.AddInParameter(command10, "VisitID", DbType.Int64, nvo.VisitID);
                                this.dbServer.AddInParameter(command10, "PatientID", DbType.Int64, nvo.PatientID);
                                this.dbServer.AddInParameter(command10, "TemplateID", DbType.Int64, svo4.TemplateID);
                                this.dbServer.AddInParameter(command10, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command10, "ControlCaption", DbType.String, svo4.ControlCaption);
                                this.dbServer.AddInParameter(command10, "ControlName", DbType.String, svo4.ControlName);
                                this.dbServer.AddInParameter(command10, "ControlType", DbType.String, svo4.ControlType);
                                this.dbServer.AddInParameter(command10, "Value", DbType.String, svo4.Value);
                                this.dbServer.AddParameter(command10, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo4.ID);
                                this.dbServer.AddOutParameter(command10, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command10, transaction);
                                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command10, "ResultStatus");
                                if ((nvo.SuccessStatus != 1) && (nvo.SuccessStatus != 2))
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        if (!svo3.Status)
                        {
                            DbCommand command11 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            this.dbServer.AddInParameter(command11, "VisitID", DbType.Int64, nvo.VisitID);
                            this.dbServer.AddInParameter(command11, "PatientID", DbType.Int64, nvo.PatientID);
                            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                            this.dbServer.AddInParameter(command11, "TemplateID", DbType.Int64, svo3.TemplateID);
                            this.dbServer.AddOutParameter(command11, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command11, transaction);
                        }
                    }
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return nvo;
        }

        public override IValueObject AddUpdateDeleteVitalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDeleteVitalDetailsBizActionVO nvo = valueObject as clsAddUpdateDeleteVitalDetailsBizActionVO;
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
                long num = 0L;
                long num2 = 0L;
                if (nvo.IsOPDIPD)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_GetIPDFlag]");
                    num = Convert.ToInt64(this.dbServer.ExecuteScalar(storedProcCommand, transaction));
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOPDFlag");
                    num2 = Convert.ToInt64(this.dbServer.ExecuteScalar(storedProcCommand, transaction));
                }
                foreach (clsEMRVitalsVO svo in nvo.PatientVitalDetails)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateDeletePatientEMRVitalDeatails");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo.PatientVitalID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
                    if (nvo.IsOPDIPD)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "TakenBy", DbType.Int64, nvo.TakenBy);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "TakenBy", DbType.Int64, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, svo.TemplateID);
                    this.dbServer.AddInParameter(storedProcCommand, "VitalID", DbType.Int64, svo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.Double, svo.Value);
                    this.dbServer.AddInParameter(storedProcCommand, "Unit", DbType.String, svo.Unit);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "IpdFlag", DbType.String, num);
                    this.dbServer.AddInParameter(storedProcCommand, "OPDFlag", DbType.String, num2);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    if (nvo.SuccessStatus != 1)
                    {
                        throw new Exception();
                    }
                    nvo.PatientDiagnosisID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return nvo;
        }

        public override IValueObject AddUpdateFollowupNote(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientFollowupNotesBizActionVO nvo = valueObject as clsAddUpdatePatientFollowupNotesBizActionVO;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRFollowUPNote");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FollowUpNote", DbType.String, nvo.CurrentFollowUpNotes.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.CurrentFollowUpNotes.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
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
            return nvo;
        }

        public override IValueObject AddUpdatePatientAllergies(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientAllergiesBizActionVO nvo = valueObject as clsAddUpdatePatientAllergiesBizActionVO;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRAllergies");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorCode", DbType.String, nvo.DoctorCode);
                this.dbServer.AddInParameter(storedProcCommand, "OPDIPD", DbType.Boolean, nvo.OPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "VisitUnitID", DbType.Int64, nvo.VisitUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FoodAllergy", DbType.String, nvo.CurrentAllergies.FoodAllergy.Replace("'", ""));
                this.dbServer.AddInParameter(storedProcCommand, "DrugAllergy", DbType.String, nvo.CurrentAllergies.DrugAllergy.Replace("'", ""));
                this.dbServer.AddInParameter(storedProcCommand, "OtherAllergy", DbType.String, nvo.CurrentAllergies.OtherAllergy.Replace("'", ""));
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.AllergyID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.AllergyID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientDrugAllergies");
                this.dbServer.AddInParameter(command2, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.ExecuteNonQuery(command2);
                List<clsGetDrugForAllergies> drugAllergies = nvo.DrugAllergies;
                for (int i = 0; i < drugAllergies.Count; i++)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPatientDrugAllergies");
                    this.dbServer.AddInParameter(command3, "PatientId", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command3, "DrugID", DbType.Int64, drugAllergies[i].DrugId);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command3);
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
            return nvo;
        }

        public override IValueObject AddUpdatePatientChiefComplaints(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientChiefComplaintsBizActionVO nvo = valueObject as clsAddUpdatePatientChiefComplaintsBizActionVO;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRChiefComplaints");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitUnitID", DbType.Int64, nvo.VisitUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ChiefComplaints", DbType.String, nvo.CurrentChiefComplaints.ChiefComplaints);
                this.dbServer.AddInParameter(storedProcCommand, "AssChiefComplaints", DbType.String, nvo.CurrentChiefComplaints.AssChiefComplaints);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.CurrentChiefComplaints.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.CurrentChiefComplaints.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
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
            return nvo;
        }

        public override IValueObject AddUpdatePatientInvestigations(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientInvestigationsBizActionVO nvo = valueObject as clsAddUpdatePatientInvestigationsBizActionVO;
            DbConnection connection = null;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDoctorSuggestedServiceDetails");
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            this.dbServer.AddParameter(storedProcCommand, "PrescriptionID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
            this.dbServer.ExecuteNonQuery(storedProcCommand);
            long parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "PrescriptionID");
            nvo.SuccessStatus = (int) parameterValue;
            List<clsDoctorSuggestedServiceDetailVO> investigationList = nvo.InvestigationList;
            int num2 = 0;
            while (true)
            {
                while (true)
                {
                    if (num2 >= investigationList.Count)
                    {
                        return nvo;
                    }
                    try
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDetails");
                        this.dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, parameterValue);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "ServiceID", DbType.Int64, investigationList[num2].ServiceID);
                        this.dbServer.AddInParameter(command, "ServiceName", DbType.String, investigationList[num2].ServiceName);
                        this.dbServer.AddInParameter(command, "GroupName", DbType.String, investigationList[num2].GroupName);
                        this.dbServer.AddInParameter(command, "Rate", DbType.Double, investigationList[num2].ServiceRate);
                        this.dbServer.AddInParameter(command, "ServiceCode", DbType.String, investigationList[num2].ServiceCode);
                        this.dbServer.AddInParameter(command, "SpecializationID", DbType.String, investigationList[num2].SpecializationId);
                        this.dbServer.AddInParameter(command, "Doctorid", DbType.String, nvo.DoctorID);
                        this.dbServer.AddInParameter(command, "PriorityID", DbType.Int64, investigationList[num2].SelectedPriority.ID);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, investigationList[num2].ID);
                        this.dbServer.ExecuteNonQuery(command);
                        nvo.InvestigationList[num2].ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if ((connection != null) && (connection.State == ConnectionState.Open))
                        {
                            connection.Close();
                        }
                    }
                    break;
                }
                num2++;
            }
        }

        public override IValueObject AddUpdatePatientMedicationFromCPOE(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientCurrentMedicationsBizActionVO nvo = valueObject as clsAddUpdatePatientCurrentMedicationsBizActionVO;
            DbConnection connection = null;
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePatientPrescriptionDetailsFromCPOE");
            this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
            this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
            this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
            this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
            this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, nvo.DoctorID);
            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            this.dbServer.AddParameter(storedProcCommand, "PrescriptionID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
            this.dbServer.ExecuteNonQuery(storedProcCommand);
            long parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "PrescriptionID");
            storedProcCommand.Dispose();
            List<clsPatientPrescriptionDetailVO> patientCurrentMedicationDetailList = nvo.PatientCurrentMedicationDetailList;
            foreach (clsPatientPrescriptionDetailVO lvo in nvo.PatientCurrentMedicationDetailList)
            {
                try
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailFromCPOE");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, parameterValue);
                    this.dbServer.AddInParameter(command, "DrugID", DbType.String, lvo.DrugID);
                    this.dbServer.AddInParameter(command, "Dose", DbType.String, lvo.Dose);
                    if ((lvo.SelectedRoute != null) && (lvo.SelectedRoute.Description != "--Select--"))
                    {
                        this.dbServer.AddInParameter(command, "Route", DbType.String, lvo.SelectedRoute.Description);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command, "Route", DbType.String, null);
                    }
                    if ((lvo.SelectedInstruction != null) && (lvo.SelectedInstruction.Description != "--Select--"))
                    {
                        this.dbServer.AddInParameter(command, "Instruction", DbType.String, lvo.SelectedInstruction.Description);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command, "Instruction", DbType.String, null);
                    }
                    if ((lvo.SelectedFrequency != null) && (lvo.SelectedFrequency.Description != "--Select--"))
                    {
                        this.dbServer.AddInParameter(command, "Frequency", DbType.String, lvo.SelectedFrequency.Description);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command, "Frequency", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(command, "NewInstruction", DbType.String, lvo.NewInstruction);
                    this.dbServer.AddInParameter(command, "ItemName", DbType.String, lvo.DrugName);
                    this.dbServer.AddInParameter(command, "UOM", DbType.String, lvo.UOM);
                    this.dbServer.AddInParameter(command, "UOMID", DbType.Int64, lvo.UOMID);
                    this.dbServer.AddInParameter(command, "Comment", DbType.String, lvo.Comment);
                    this.dbServer.AddInParameter(command, "Days", DbType.Int64, lvo.Days);
                    this.dbServer.AddInParameter(command, "Quantity", DbType.Double, lvo.Quantity);
                    this.dbServer.AddInParameter(command, "IsOther", DbType.Boolean, lvo.IsOther);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command, "ARTEnables", DbType.Boolean, lvo.ArtEnabled);
                    this.dbServer.AddInParameter(command, "DrugSourceId ", DbType.Int64, lvo.SelectedDrugSource.ID);
                    this.dbServer.AddInParameter(command, "PlanTherapyId ", DbType.Int64, lvo.PlanTherapyId);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitId ", DbType.Int64, lvo.PlanTherapyUnitId);
                    this.dbServer.ExecuteNonQuery(command);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            if ((connection != null) && (connection.State == ConnectionState.Open))
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateReferralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateReferralDetailsBizActionVO nvo = valueObject as clsAddUpdateReferralDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            long parameterValue = 0L;
            long num2 = 0L;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                foreach (clsDoctorSuggestedServiceDetailVO lvo in nvo.DoctorSuggestedServiceDetail)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRReferralDeatails");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.String, nvo.DoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "SpecialiizationCode", DbType.String, lvo.ReferalSpecializationCode);
                    this.dbServer.AddInParameter(storedProcCommand, "SpecialiizationName", DbType.String, lvo.ReferalSpecialization);
                    this.dbServer.AddInParameter(storedProcCommand, "RefDocCode", DbType.String, lvo.ReferalDoctorCode);
                    this.dbServer.AddInParameter(storedProcCommand, "RefDocName", DbType.String, lvo.ReferalDoctor);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, lvo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                    this.dbServer.AddParameter(storedProcCommand, "PrescID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, num2);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    parameterValue = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    num2 = (long) this.dbServer.GetParameterValue(storedProcCommand, "PrescID");
                    if (nvo.SuccessStatus != 1)
                    {
                        throw new Exception();
                    }
                    if (lvo.ReferralLetter != null)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddEMRReferralLetter");
                        this.dbServer.AddInParameter(command, "PrescriptionID", DbType.Int64, num2);
                        this.dbServer.AddInParameter(command, "ReferralID", DbType.Int64, parameterValue);
                        this.dbServer.AddInParameter(command, "IsReferral", DbType.Boolean, lvo.IsRefferal);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        if (lvo.IsRefferal)
                        {
                            this.dbServer.AddInParameter(command, "VisitDetails", DbType.String, lvo.ReferralLetter.VisitDetails);
                            this.dbServer.AddInParameter(command, "Diagnosis", DbType.String, lvo.ReferralLetter.Diagnosis);
                            this.dbServer.AddInParameter(command, "Treatment", DbType.String, lvo.ReferralLetter.Treatment);
                            this.dbServer.AddInParameter(command, "ReferralType", DbType.Int16, lvo.ReferralLetter.ReferalType);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command, "ReferralRemark", DbType.String, lvo.ReferralLetter.ReferralRemark);
                            this.dbServer.AddInParameter(command, "ReferralTreatment", DbType.String, lvo.ReferralLetter.ReferralTreatment);
                            this.dbServer.AddInParameter(command, "Conclusion", DbType.String, lvo.ReferralLetter.Conclusion);
                            this.dbServer.AddInParameter(command, "ConsultEndDate", DbType.DateTime, lvo.ReferralLetter.ConsultEndDate);
                            this.dbServer.AddInParameter(command, "JointCareDate", DbType.DateTime, lvo.ReferralLetter.JointCareDate);
                            this.dbServer.AddInParameter(command, "NextConsultDate", DbType.DateTime, lvo.ReferralLetter.NextConsultDate);
                            this.dbServer.AddInParameter(command, "TakeOverDate", DbType.DateTime, lvo.ReferralLetter.TakeOverDate);
                        }
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, lvo.Status);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        this.dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, nvo.SuccessStatus);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    }
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
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
                transaction.Dispose();
            }
            return nvo;
        }

        public override IValueObject deleteUploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteUploadPatientHystoLapBizActionVO nvo = valueObject as clsDeleteUploadPatientHystoLapBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteImageHistory");
                this.dbServer.AddInParameter(storedProcCommand, "ImgID", DbType.Int64, nvo.ImageID);
                nvo.SuccessStatus = (this.dbServer.ExecuteNonQuery(storedProcCommand) != -1) ? 0 : 1;
            }
            catch
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject DoctorlistonReferal(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorlistReferralServiceBizActionVO nvo = valueObject as clsGetDoctorlistReferralServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDoctorListOnReferralLoad");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), true));
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

        public override IValueObject getDoctorlistonreferralasperService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorlistforReferralAsperServiceBizActionVO nvo = valueObject as clsGetDoctorlistforReferralAsperServiceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralDoctorAsperServiceDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DepartmentID", DbType.Int64, nvo.DepartmentId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]))));
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

        public override IValueObject GetEMR_CaseReferral_FieldList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMR_CaseReferral_FieldListBizActionVO nvo = (clsGetEMR_CaseReferral_FieldListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMR_CaseReferral_FieldList");
                this.dbServer.AddInParameter(storedProcCommand, "SectionID", DbType.Int64, nvo.SectionID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CaseReferral_FieldMasterList == null)
                    {
                        nvo.CaseReferral_FieldMasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.CaseReferral_FieldMasterList.Add(new MasterListItem((long) reader["ID"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetEMR_PCR_FieldList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMR_PCR_FieldListBizActionVO nvo = (clsGetEMR_PCR_FieldListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMR_PCR_FieldList");
                this.dbServer.AddInParameter(storedProcCommand, "SectionID", DbType.Int64, nvo.SectionID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PCR_FieldMasterList == null)
                    {
                        nvo.PCR_FieldMasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.PCR_FieldMasterList.Add(new MasterListItem((long) reader["ID"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetEMRTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRTemplateBizActionVO nvo = valueObject as clsGetEMRTemplateBizActionVO;
            try
            {
                clsEMRTemplateVO objEMRTemplate = nvo.objEMRTemplate;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objEMRTemplate.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.objEMRTemplate.TemplateID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.objEMRTemplate.Title = (string) DALHelper.HandleDBNull(reader["Title"]);
                        nvo.objEMRTemplate.Description = (string) DALHelper.HandleDBNull(reader["Description"]);
                        nvo.objEMRTemplate.Template = (string) DALHelper.HandleDBNull(reader["Template"]);
                        nvo.objEMRTemplate.ApplicableCriteria = (int) DALHelper.HandleDBNull(reader["AppTo"]);
                        nvo.objEMRTemplate.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                        nvo.objEMRTemplate.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.objEMRTemplate.CreatedUnitID = (long) DALHelper.HandleDBNull(reader["CreatedUnitID"]);
                        nvo.objEMRTemplate.AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]);
                        nvo.objEMRTemplate.AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]);
                        nvo.objEMRTemplate.AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]);
                        nvo.objEMRTemplate.AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        nvo.objEMRTemplate.UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        nvo.objEMRTemplate.UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        nvo.objEMRTemplate.UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        nvo.objEMRTemplate.UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);
                        nvo.objEMRTemplate.IsPhysicalExam = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPhysicalExam"]));
                        nvo.objEMRTemplate.IsForOT = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsProcedure"]));
                        nvo.objEMRTemplate.TemplateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateTypeID"]));
                        nvo.objEMRTemplate.TemplateSubtypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateSubtypeID"]));
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

        public override IValueObject GetEMRTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRTemplateListBizActionVO nvo = valueObject as clsGetEMRTemplateListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsPhysicalExam", DbType.Boolean, nvo.IsphysicalExam);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objEMRTemplateList == null)
                    {
                        nvo.objEMRTemplateList = new List<clsEMRTemplateVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRTemplateVO item = new clsEMRTemplateVO {
                            TemplateID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Template = (string) DALHelper.HandleDBNull(reader["Template"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            CreatedUnitID = (long) DALHelper.HandleDBNull(reader["CreatedUnitID"]),
                            AddedBy = (long?) DALHelper.HandleDBNull(reader["AddedBy"]),
                            AddedOn = (string) DALHelper.HandleDBNull(reader["AddedOn"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"]),
                            AddedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]),
                            UpdatedBy = (long?) DALHelper.HandleDBNull(reader["UpdatedBy"]),
                            UpdatedOn = (string) DALHelper.HandleDBNull(reader["UpdatedOn"]),
                            UpdatedDateTime = DALHelper.HandleDate(reader["UpdatedDateTime"]),
                            UpdatedWindowsLoginName = (string) DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"])
                        };
                        nvo.objEMRTemplateList.Add(item);
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

        public override IValueObject GetEMRTemplateListForOT(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRTemplateListForOTBizActionVO nvo = valueObject as clsGetEMRTemplateListForOTBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRTemplateForOT");
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureID", DbType.String, nvo.ProcedureTemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objEMRTemplateList == null)
                    {
                        nvo.objEMRTemplateList = new List<clsEMRTemplateVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRTemplateVO item = new clsEMRTemplateVO {
                            TemplateID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Title = (string) DALHelper.HandleDBNull(reader["Title"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.objEMRTemplateList.Add(item);
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

        public override IValueObject GetEMRTemplateListForOTProcedure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRTemplateListBizActionVO nvo = valueObject as clsGetEMRTemplateListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "IsProcedureTemplate", DbType.Boolean, nvo.IsProcedureTemplate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.objEMRTemplateList == null)
                    {
                        nvo.objEMRTemplateList = new List<clsEMRTemplateVO>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.objMasterList.Add(item);
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

        public override IValueObject GetPatientAllergies(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientAllergiesBizActionVO nvo = valueObject as clsGetPatientAllergiesBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientAllergies");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.AllergiesList == null)
                    {
                        nvo.AllergiesList = new List<clsEMRAllergiesVO>();
                    }
                    clsEMRAllergiesVO svo = new clsEMRAllergiesVO();
                    if (reader.Read())
                    {
                        svo.FoodAllergy = string.Format(svo.FoodAllergy + "," + Convert.ToString(DALHelper.HandleDBNull(reader["FoodAllergy"])), new object[0]).Trim(new char[] { ',' });
                        svo.DrugAllergy = string.Format(svo.DrugAllergy + "," + Convert.ToString(DALHelper.HandleDBNull(reader["DrugAllergy"])), new object[0]).Trim(new char[] { ',' });
                        svo.OtherAllergy = string.Format(svo.OtherAllergy + "," + Convert.ToString(DALHelper.HandleDBNull(reader["OtherAllergy"])), new object[0]).Trim(new char[] { ',' });
                        svo.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        nvo.CurrentAllergy = svo;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetPatientChiefComplaints(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientChiefComplaintsBizActionVO nvo = valueObject as clsGetPatientChiefComplaintsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentChiefComplaints");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, nvo.DoctorID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ChiefComplaintList == null)
                    {
                        nvo.ChiefComplaintList = new List<clsEMRChiefComplaintsVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRChiefComplaintsVO svo = new clsEMRChiefComplaintsVO {
                            AssChiefComplaints = Convert.ToString(DALHelper.HandleDBNull(reader["AssChiefComplaints"])),
                            ChiefComplaints = Convert.ToString(DALHelper.HandleDBNull(reader["ChiefComplaints"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))
                        };
                        nvo.CurrentChiefComplaints = svo;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetPatientCostNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCostBizActionVO nvo = valueObject as clsGetPatientCostBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCostNotes");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, nvo.DoctorID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FollowUpNotesList == null)
                    {
                        nvo.FollowUpNotesList = new List<clsEMRFollowNoteVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRFollowNoteVO evo = new clsEMRFollowNoteVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Notes = Convert.ToString(DALHelper.HandleDBNull(reader["FollowUpNote"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))
                        };
                        nvo.CurrentFollowUPNotes = evo;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetPatientDiagnosisEMRDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisEMRDetailsBizActionVO nvo = valueObject as clsGetPatientDiagnosisEMRDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDiagnosisEMRDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Tab", DbType.String, nvo.Tab);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
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
                            ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]))
                        };
                        bool flag1 = item.ControlCaption == "BPControl";
                        bool flag2 = item.ControlCaption == "VisionControl";
                        bool flag3 = item.ControlCaption == "GlassPower";
                        item.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        item.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        item.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
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

        public override IValueObject GetPatientDrugAllergies(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDrugAllergiesBizActionVO nvo = valueObject as clsGetPatientDrugAllergiesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDrugAllergies");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DrugAllergiesList == null)
                    {
                        nvo.DrugAllergiesList = new List<clsGetDrugForAllergies>();
                    }
                    while (reader.Read())
                    {
                        clsGetDrugForAllergies item = new clsGetDrugForAllergies {
                            DrugId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DrugId"])),
                            DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]))
                        };
                        nvo.DrugAllergiesList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject GetPatientDrugAllergiesList(IValueObject valueObject, clsUserVO UserVo)
        {
            ClsGetPatientdrugAllergiesListBizActionVO nvo = valueObject as ClsGetPatientdrugAllergiesListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDrugAllergiesList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "unitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DrugAllergiesList == null)
                    {
                        nvo.DrugAllergiesList = new List<clsGetDrugForAllergies>();
                    }
                    while (reader.Read())
                    {
                        clsGetDrugForAllergies item = new clsGetDrugForAllergies {
                            DrugId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DrugId"]))
                        };
                        nvo.DrugAllergiesList.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetPatientEMRICDXDiagnosisList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisDataBizActionVO nvo = valueObject as clsGetPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRICDXDiagnosisDetailsHistory");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.String, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDiagnosisDetails == null)
                    {
                        nvo.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsEMRAddDiagnosisVO item = new clsEMRAddDiagnosisVO {
                            Categori = Convert.ToString(reader["Categori"]),
                            Class = Convert.ToString(reader["Class"]),
                            Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"])),
                            IsSelected = (bool) DALHelper.HandleBoolDBNull(reader["PrimaryDiagnosis"]),
                            SelectedDiagnosisType = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                            }
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        if (nvo.IsOPDIPD)
                        {
                            item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            item.DocSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpeclization"]));
                        }
                        nvo.PatientDiagnosisDetails.Add(item);
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

        public override IValueObject GetPatientFollowUpNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFollowUpNoteBizActionVO nvo = valueObject as clsGetPatientFollowUpNoteBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUPNotes");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, nvo.DoctorID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.FollowUpNotesList == null)
                    {
                        nvo.FollowUpNotesList = new List<clsEMRFollowNoteVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRFollowNoteVO evo = new clsEMRFollowNoteVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Notes = Convert.ToString(DALHelper.HandleDBNull(reader["FollowUpNote"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))
                        };
                        nvo.CurrentFollowUPNotes = evo;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetPatientGrowthChartMonthlyVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGrowthChartMonthlyBizActionVO nvo = valueObject as clsGetPatientGrowthChartMonthlyBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGrowthChartVisitMonthlyDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                long drID = nvo.DrID;
                if (nvo.DrID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrID", DbType.Int64, nvo.DrID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrID", DbType.Int64, null);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GrowthChartDetailList == null)
                    {
                        nvo.GrowthChartDetailList = new List<clsGrowthChartVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsGrowthChartVO item = new clsGrowthChartVO();
                        nvo.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        item.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        item.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitId"]));
                        item.ChiefComplaint = Convert.ToString(DALHelper.HandleDBNull(reader["ComplentChief"]));
                        item.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        item.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                        if (item.Height > 0.0)
                        {
                            item.BMI = item.Weight / ((item.Height * item.Height) / 10000.0);
                        }
                        item.SBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["SBP"]));
                        item.DBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["DBP"]));
                        item.HC = Convert.ToDouble(DALHelper.HandleDBNull(reader["HC"]));
                        item.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        item.RR = Convert.ToDouble(DALHelper.HandleDBNull(reader["RR"]));
                        item.Spo2 = Convert.ToDouble(DALHelper.HandleDBNull(reader["Spo2"]));
                        item.Temprature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temprature"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.MOB = Convert.ToString(DALHelper.HandleDBNull(reader["MOB"]));
                        item.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        item.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        item.Age = Convert.ToInt64(DALHelper.HandleDBNull(reader["Age"]));
                        item.AgeInMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInMonth"]));
                        item.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        item.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.DrName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        item.OPD = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNo"]));
                        if (((item.Height > 0.0) && (item.Weight > 0.0)) || (item.HC > 0.0))
                        {
                            item.ViewDetails = item.AgeInMonth <= 240L;
                        }
                        nvo.GrowthChartDetailList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientGrowthChartVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGrowthChartBizActionVO nvo = valueObject as clsGetPatientGrowthChartBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGrowthChartVisitDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                long drID = nvo.DrID;
                if (nvo.DrID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrID", DbType.Int64, nvo.DrID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrID", DbType.Int64, null);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GrowthChartDetailList == null)
                    {
                        nvo.GrowthChartDetailList = new List<clsGrowthChartVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsGrowthChartVO item = new clsGrowthChartVO();
                        nvo.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        item.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        item.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitId"]));
                        item.ChiefComplaint = Convert.ToString(DALHelper.HandleDBNull(reader["ComplentChief"]));
                        item.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        item.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                        item.BMI = Convert.ToDouble(DALHelper.HandleDBNull(reader["BMI"]));
                        item.SBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["SBP"]));
                        item.DBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["DBP"]));
                        item.HC = Convert.ToDouble(DALHelper.HandleDBNull(reader["HC"]));
                        item.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        item.RR = Convert.ToDouble(DALHelper.HandleDBNull(reader["RR"]));
                        item.Spo2 = Convert.ToDouble(DALHelper.HandleDBNull(reader["Spo2"]));
                        item.Temprature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temprature"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.MobileCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        item.MOB = Convert.ToString(DALHelper.HandleDBNull(reader["MOB"]));
                        item.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        item.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        item.Age = Convert.ToInt64(DALHelper.HandleDBNull(reader["Age"]));
                        item.AgeInMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInMonth"]));
                        item.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        item.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        item.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.DrName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        item.OPD = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNo"]));
                        if (((item.Height > 0.0) && (item.Weight > 0.0)) || (item.HC > 0.0))
                        {
                            item.ViewDetails = item.AgeInMonth <= 240L;
                        }
                        nvo.GrowthChartDetailList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientGrowthChartYearlyVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGrowthChartYearlyBizActionVO nvo = valueObject as clsGetPatientGrowthChartYearlyBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGrowthChartVisitYearlyDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Int64, nvo.IsopdIPd);
                long drID = nvo.DrID;
                if (nvo.DrID > 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrID", DbType.Int64, nvo.DrID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrID", DbType.Int64, null);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GrowthChartDetailList == null)
                    {
                        nvo.GrowthChartDetailList = new List<clsGrowthChartVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsGrowthChartVO item = new clsGrowthChartVO();
                        nvo.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        item.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        item.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitId"]));
                        item.ChiefComplaint = Convert.ToString(DALHelper.HandleDBNull(reader["ComplentChief"]));
                        item.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        item.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                        if (item.Height > 0.0)
                        {
                            item.BMI = Math.Round((double) (item.Weight / ((item.Height * item.Height) / 10000.0)), 2);
                        }
                        item.SBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["SBP"]));
                        item.DBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["DBP"]));
                        item.HC = Convert.ToDouble(DALHelper.HandleDBNull(reader["HC"]));
                        item.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        item.RR = Convert.ToDouble(DALHelper.HandleDBNull(reader["RR"]));
                        item.Spo2 = Convert.ToDouble(DALHelper.HandleDBNull(reader["Spo2"]));
                        item.Temprature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temprature"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.MobileCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        item.MOB = Convert.ToString(DALHelper.HandleDBNull(reader["MOB"]));
                        item.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        item.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));
                        item.Age = Convert.ToInt64(DALHelper.HandleDBNull(reader["Age"]));
                        item.AgeInMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInMonth"]));
                        item.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        item.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        item.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.DrName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        item.OPD = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNo"]));
                        if (((item.Height > 0.0) && (item.Weight > 0.0)) || (item.HC > 0.0))
                        {
                            item.ViewDetails = item.AgeInMonth <= 240L;
                        }
                        nvo.GrowthChartDetailList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientNewEMRDiagnosisList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisDataBizActionVO nvo = valueObject as clsGetPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = null;
                command = nvo.IsICDX ? this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRICDXDiagnosisDetails") : this.dbServer.GetStoredProcCommand("CIMS_GetPatientNewEMRDiagnosisDetails");
                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(command, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(command, "Doctorid", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (nvo.PatientDiagnosisDetails == null)
                    {
                        nvo.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(command, "TotalRows"));
                            break;
                        }
                        clsEMRAddDiagnosisVO item = new clsEMRAddDiagnosisVO {
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Class = Convert.ToString(DALHelper.HandleDBNull(reader["Class"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            IsICOPIMHead = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsICOPIMHead"])),
                            Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"])),
                            IsSelected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrimaryDiagnosis"])),
                            SelectedDiagnosisType = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                            },
                            Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                            TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                            TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"])),
                            ArtEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArt"])),
                            PlanTreatmentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentId"])),
                            BilledEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"])),
                            IsClosedEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"])),
                            PriorityId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PriorityId"])),
                            IsArtStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArtStatus"])),
                            PACEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPAC"])),
                            IsDonorCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycle"])),
                            DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"])),
                            DonarUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonarUnitID"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"])),
                            CoupleMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleMRNO"])),
                            IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogacy"])),
                            SurrogateMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMrNo"]))
                        };
                        nvo.PatientDiagnosisDetails.Add(item);
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

        public override IValueObject GetPatientNewEMRDiagnosisListHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisDataBizActionVO nvo = valueObject as clsGetPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = null;
                command = !nvo.ISDashBoard ? this.dbServer.GetStoredProcCommand("CIMS_GetPatientNewEMRDiagnosisDetailsHistory") : this.dbServer.GetStoredProcCommand("CIMS_GetPatientNewEMRDiagnosisDetailsHistoryForDashBoard");
                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(command, "@startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(command, "@maximumRows", DbType.Int64, nvo.MaximumRows);
                if (!nvo.ISDashBoard)
                {
                    this.dbServer.AddInParameter(command, "DoctorID", DbType.String, nvo.DoctorID);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (nvo.PatientDiagnosisDetails == null)
                    {
                        nvo.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(command, "TotalRows"));
                            break;
                        }
                        clsEMRAddDiagnosisVO item = new clsEMRAddDiagnosisVO {
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Class = Convert.ToString(DALHelper.HandleDBNull(reader["Class"])),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"])),
                            Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"])),
                            IsSelected = (bool) DALHelper.HandleBoolDBNull(reader["PrimaryDiagnosis"]),
                            SelectedDiagnosisType = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                            }
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        if (nvo.IsOPDIPD)
                        {
                            item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            item.DocSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpeciliztion"]));
                        }
                        item.ArtEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArt"]));
                        item.PlanTreatmentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentId"]));
                        item.BilledEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        item.IsClosedEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        item.PriorityId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PriorityId"]));
                        item.IsArtStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArtStatus"]));
                        item.PACEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPAC"]));
                        item.IsDonorCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycle"]));
                        item.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        item.DonarUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonarUnitID"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        item.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        item.CoupleMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleMRNO"]));
                        item.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogacy"]));
                        item.SurrogateMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMrNo"]));
                        nvo.PatientDiagnosisDetails.Add(item);
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

        public override IValueObject GetPatientPastHistroScopyAndLaproscopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastHistroScopyandLaproscopyHistoryBizActionVO nvo = valueObject as clsGetPatientPastHistroScopyandLaproscopyHistoryBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRPatientPastHistory");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "visitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Boolean, nvo.IsOpdIpd);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientPasthistory == null)
                    {
                        nvo.PatientPasthistory = new List<GetPastHistroandlapro>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        GetPastHistroandlapro item = new GetPastHistroandlapro {
                            DoctorName = Convert.ToString(reader["DoctorName"]),
                            DateTime = Convert.ToString(DALHelper.HandleDBNull(reader["Date"])),
                            EmrId = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmrID"]))
                        };
                        nvo.PatientPasthistory.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientPastPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastPhysicalexamDetailsBizActionVO nvo = valueObject as clsGetPatientPastPhysicalexamDetailsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;
                command = !nvo.IsOpdIpd ? this.dbServer.GetStoredProcCommand("CIMS_OPDEMRPatientConsultationSummary") : this.dbServer.GetStoredProcCommand("CIMS_IPDEMRPatientConsultationSummary");
                this.dbServer.AddInParameter(command, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(command, "DoctorCode", DbType.String, nvo.DoctorCode);
                this.dbServer.AddInParameter(command, "DoctorId", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, nvo.IsOpdIpd);
                this.dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(command, "maximumRows", DbType.Int64, nvo.MaximumRows);
                reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (nvo.PatientPastPhysicalexam == null)
                    {
                        nvo.PatientPastPhysicalexam = new List<GetPastPhysicalexam>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(command, "TotalRows"));
                            break;
                        }
                        GetPastPhysicalexam item = new GetPastPhysicalexam {
                            Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"])),
                            TemplateId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            TemplateValue = Convert.ToString(DALHelper.HandleDBNull(reader["Value"])),
                            TemplateHeader = Convert.ToString(reader["Header"]),
                            DoctorName = Convert.ToString(reader["DoctorName"]),
                            DoctorSpeclization = Convert.ToString(reader["DrSpeclizition"]),
                            VisitDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"])),
                            DateTime = Convert.ToString(DALHelper.HandleDBNull(reader["AddedDateTime"]))
                        };
                        nvo.PatientPastPhysicalexam.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientPatientVitalChartList(IValueObject valueObject, clsUserVO UserVo)
        {
            int num = 0;
            clsGetPatientVitalChartBizActionVO nvo = valueObject as clsGetPatientVitalChartBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                if (!nvo.IsFromDashBoard || nvo.IsOPDIPD)
                {
                    storedProcCommand = !nvo.IsOPDIPD ? this.dbServer.GetStoredProcCommand("CIMS_GetGetPatientVitalChartDetails") : this.dbServer.GetStoredProcCommand("CIMS_GetGetIPDPatientVitalChartDetails");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientVitalChartDetailsForDashBoard");
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                    num = 1;
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientVitalChartlst == null)
                    {
                        nvo.PatientVitalChartlst = new List<clsVitalChartVO>();
                    }
                    while (reader.Read())
                    {
                        clsVitalChartVO item = new clsVitalChartVO {
                            Height = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Height"])),
                            Weight = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Weight"]))
                        };
                        if (item.Height > 0.0)
                        {
                            item.BMI = item.Weight / ((item.Height * item.Height) / 10000.0);
                        }
                        item.SystolicBP = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["SBP"]));
                        item.DiastolicBP = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["DBP"]));
                        item.HC = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["HC"]));
                        item.Pulse = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Pulse"]));
                        item.RR = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["RR"]));
                        item.O2 = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["O2"]));
                        item.Temperature = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Temprature"]));
                        item.Waistgirth = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Waistgirth"]));
                        item.Hipgirth = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Hipgirth"]));
                        item.RBS = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["RBS"]));
                        item.TotalCholesterol = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["TotalCholesterol"]));
                        item.RandomBloodSugar = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["RandomBloodSugar"]));
                        item.FastingSugar = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["FastingSugar"]));
                        item.HeadCircumference = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["HeadCircumference"]));
                        item.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        if (nvo.IsOPDIPD)
                        {
                            item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["Doctorname"]));
                            item.DoctorSpeclization = Convert.ToString(DALHelper.HandleDBNull(reader["DrSpec"]));
                        }
                        nvo.PatientVitalChartlst.Add(item);
                    }
                }
                if (num == 1)
                {
                    reader.NextResult();
                    nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                    num = 0;
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetPatientPreviousVisitServices(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServicesBizActionVO nvo = valueObject as clsGetServicesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPastInvestigations");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int32, nvo.DoctorID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VisitServicesList == null)
                    {
                        nvo.VisitServicesList = new List<clsServiceMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"])),
                            DoctorID = Convert.ToInt32(reader["Doctorid"]),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]))
                        };
                        if (nvo.IsOPDIPD)
                        {
                            item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            item.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["DrSpec"]));
                        }
                        item.Group = Convert.ToString(reader["GroupName"]);
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        item.VisitDate = Convert.ToDateTime(reader["VisitDate"]);
                        item.SelectedPriority = new MasterListItem();
                        item.SelectedPriority.Description = Convert.ToString(DALHelper.HandleDBNull(reader["PriorityDescription"]));
                        nvo.VisitServicesList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject GetPatientProcedureDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientProcedureDataBizActionVO nvo = valueObject as clsGetPatientProcedureDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientProcedureDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDiagnosisDetails == null)
                    {
                        nvo.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsEMRAddDiagnosisVO item = new clsEMRAddDiagnosisVO {
                            Categori = Convert.ToString(reader["Class"]),
                            Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"])),
                            SelectedDiagnosisType = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                            }
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        item.TemplateID = Convert.ToInt64(reader["TemplateID"]);
                        item.TemplateName = Convert.ToString(reader["TemplateName"]);
                        if (nvo.IsOPDIPD)
                        {
                            item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            item.DocSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpeclization"]));
                        }
                        nvo.PatientDiagnosisDetails.Add(item);
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

        public override IValueObject GetPatientReferraldetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReferralDetailsBizActionVO nvo = valueObject as clsGetReferralDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "doctorID", DbType.Int64, nvo.DoctorID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                DbDataReader reader2 = null;
                if (reader.HasRows)
                {
                    if (nvo.DoctorSuggestedServiceDetail == null)
                    {
                        nvo.DoctorSuggestedServiceDetail = new List<clsDoctorSuggestedServiceDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO item = new clsDoctorSuggestedServiceDetailVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralID"])),
                            PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"])),
                            DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["Doctorid"])),
                            Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                            SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["Specializationcode"])),
                            IsRefferal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferral"]))
                        };
                        item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctorName"]));
                        item.PrintFlag = "Visible";
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralLetter");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, item.ID);
                        reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                clsEMRReferralLetterVO rvo = new clsEMRReferralLetterVO {
                                    Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["Date"])),
                                    VisitDetails = Convert.ToString(DALHelper.HandleDBNull(reader2["VisitDetails"])),
                                    Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader2["Diagnosis"])),
                                    Treatment = Convert.ToString(DALHelper.HandleDBNull(reader2["Treatment"])),
                                    ReferalType = Convert.ToInt16(DALHelper.HandleDBNull(reader2["ReferralType"]))
                                };
                                item.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferredSpeciality"]));
                                item.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferredSpecialityCode"]));
                                item.ReferalSpecialization = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferalSpeciality"]));
                                item.ReferalSpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferalSpecialityCode"]));
                                item.ReferalDoctor = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferalDoctor"]));
                                item.ReferalDoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferalDoctorCode"]));
                                item.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferredDoctorCode"]));
                                item.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader2["ReferredDoctor"]));
                                item.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["Date"]));
                                rvo.AckDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader2["AcknowledgedDate"]));
                                rvo.Conclusion = Convert.ToString(reader2["Conclusion"]);
                                rvo.ReferralRemark = Convert.ToString(reader2["Remark"]);
                                rvo.ReferralTreatment = Convert.ToString(reader2["ReferralTreatment"]);
                                rvo.ConsultEndDate = (DateTime?) DALHelper.HandleDBNull(reader2["ConsultEndDate"]);
                                rvo.TakeOverDate = (DateTime?) DALHelper.HandleDBNull(reader2["TakeOverDate"]);
                                rvo.NextConsultDate = (DateTime?) DALHelper.HandleDBNull(reader2["NextConsultDate"]);
                                rvo.JointCareDate = (DateTime?) DALHelper.HandleDBNull(reader2["JointCareDate"]);
                                rvo.ReferredDoctor = item.DoctorName;
                                rvo.ReferredDoctorCode = item.DoctorCode;
                                rvo.ReferalDoctorCode = item.ReferalDoctorCode;
                                rvo.ReferalDoctor = item.ReferalDoctor;
                                rvo.ReferalSpeciality = item.ReferalSpecialization;
                                rvo.ReferredSpeciality = item.Specialization;
                                item.ReferralLetter = rvo;
                            }
                        }
                        nvo.DoctorSuggestedServiceDetail.Add(item);
                        reader2.Close();
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

        public override IValueObject GetPatientReferraldetailsListHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReferralDetailsBizActionVO nvo = valueObject as clsGetReferralDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralDetailsHistory");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Doctorid", DbType.Int64, nvo.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "@startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "@maximumRows", DbType.Int64, nvo.MaximumRows);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorSuggestedServiceDetail == null)
                    {
                        nvo.DoctorSuggestedServiceDetail = new List<clsDoctorSuggestedServiceDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        clsDoctorSuggestedServiceDetailVO item = new clsDoctorSuggestedServiceDetailVO {
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"])),
                            ReferalDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalDoctor"])),
                            Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"])),
                            ReferalSpecialization = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalSpecialization"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        nvo.DoctorSuggestedServiceDetail.Add(item);
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

        public override IValueObject GetPatientReferreddetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReferredDetailsBizActionVO nvo = valueObject as clsGetReferredDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferredDoctorDetails");
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Int64, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DoctorSuggestedServiceDetailforReferred == null)
                    {
                        nvo.DoctorSuggestedServiceDetailforReferred = new List<clsDoctorSuggestedServiceDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO item = new clsDoctorSuggestedServiceDetailVO {
                            ReferalDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            ReferalDoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ID"])),
                            Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.DoctorSuggestedServiceDetailforReferred.Add(item);
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

        public override IValueObject GetUploadPatientImage(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsGetUploadPatientImageBizActionVO nvo = valueObject as clsGetUploadPatientImageBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetUploadPatientFile");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VISITID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Boolean, nvo.ISOPDIPD);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientFollowUpImageVO item = new clsPatientFollowUpImageVO {
                            EditImage = (byte[]) DALHelper.HandleDBNull(reader["EditReport"]),
                            Remark = (string) DALHelper.HandleDBNull(reader["Remark"]),
                            SourceURL = (string) DALHelper.HandleDBNull(reader["SourceURL"]),
                            AddedDateTime = DALHelper.HandleDate(reader["AddedDateTime"])
                        };
                        nvo.ImageDetails.Add(item);
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

        public override IValueObject GetUploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientUploadedImagetHystoLapBizActionVO nvo = valueObject as clsGetPatientUploadedImagetHystoLapBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRPatientHistoryScopyandLaproImage");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ISOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromOtImg", DbType.Boolean, nvo.IsFromOtImg);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Img1 == null)
                    {
                        nvo.Img1 = new List<ClsImages>();
                    }
                    while (reader.Read())
                    {
                        ClsImages item = new ClsImages {
                            ImageID = Convert.ToInt64(reader["id"])
                        };
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                        item.ImageName = string.Concat(strArray);
                        if (nvo.IsFromOtImg)
                        {
                            item.UserImage = new WebClient().DownloadData(item.ImageName);
                        }
                        nvo.Img1.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetVitalListDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalListDetailsBizActionVO nvo = valueObject as clsGetVitalListDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientEMRVitalDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.vitalListDetail == null)
                    {
                        nvo.vitalListDetail = new List<clsEMRVitalsVO>();
                    }
                    while (reader.Read())
                    {
                        clsEMRVitalsVO item = new clsEMRVitalsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["VitalID"]),
                            PatientVitalID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Vital"]),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            Time = (DateTime?) DALHelper.HandleDBNull(reader["Time"]),
                            Value = (double) DALHelper.HandleDBNull(reader["Value"]),
                            Unit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"])),
                            ValueDescription = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDecimal"])) ? string.Format("{0:0.00}", Convert.ToDouble(DALHelper.HandleDBNull(reader["Value"]))) : string.Format("{0:0}", Convert.ToDouble(DALHelper.HandleDBNull(reader["Value"])))
                        };
                        nvo.vitalListDetail.Add(item);
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

        public override IValueObject UpdateEMRTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateEMRTemplateBizActionVO nvo = valueObject as clsUpdateEMRTemplateBizActionVO;
            try
            {
                clsEMRTemplateVO eMRTemplateDetails = nvo.EMRTemplateDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEMRTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, eMRTemplateDetails.LinkServer);
                if (eMRTemplateDetails.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, eMRTemplateDetails.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, eMRTemplateDetails.Title);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, eMRTemplateDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, eMRTemplateDetails.Template);
                this.dbServer.AddInParameter(storedProcCommand, "IsPhysicalExam", DbType.Boolean, eMRTemplateDetails.IsPhysicalExam);
                this.dbServer.AddInParameter(storedProcCommand, "IsForOT", DbType.Boolean, eMRTemplateDetails.IsForOT);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateTypeID", DbType.Int64, eMRTemplateDetails.TemplateTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateType", DbType.String, eMRTemplateDetails.TemplateType);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateSubtypeID", DbType.Int64, eMRTemplateDetails.TemplateSubtypeID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateSubtype", DbType.String, eMRTemplateDetails.TemplateSubtype);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, eMRTemplateDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, eMRTemplateDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, eMRTemplateDetails.Status);
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

        public override IValueObject UpdateUploadPatientImage(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsUpdateUploadPatientImageBizActionVO nvo = valueObject as clsUpdateUploadPatientImageBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateUploadPatientFile");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "EditReport", DbType.Binary, nvo.EditImage);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, nvo.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject UploadPatientImage(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsUploadPatientImageBizActionVO nvo = valueObject as clsUploadPatientImageBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUploadPatientFile");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.UploadMatserDetails.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, nvo.UploadMatserDetails.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "OriginalReport", DbType.Binary, nvo.OriginalImage);
                this.dbServer.AddInParameter(storedProcCommand, "EditReport", DbType.Binary, nvo.EditImage);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.AddInParameter(storedProcCommand, "DocumentName", DbType.String, nvo.UploadMatserDetails.DocumentName);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject UploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUploadPatientHystoLapBizActionVO nvo = valueObject as clsUploadPatientHystoLapBizActionVO;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                string str = this.GetImageName(nvo.TemplateID, nvo.PatientID, nvo.VisitID, UserVo.UserLoginInfo.UnitId, nvo.IsOPDIPD);
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddImageForHistroScopyAndLaproScopy");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int32, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ImgPath", DbType.String, str);
                this.dbServer.AddInParameter(storedProcCommand, "IsOPDIPD", DbType.Boolean, nvo.IsOPDIPD);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                ImageConverter converter1 = new ImageConverter();
                MemoryStream stream1 = new MemoryStream();
                ImageCodecInfo encoder = this.GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, (long) 50);
                encoderParams.Param[0] = parameter;
                new Bitmap(Image.FromStream(new MemoryStream(nvo.Image)), new Size(150, 120)).Save(this.ImgSaveLocation + str, encoder, encoderParams);
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
            return valueObject;
        }
    }
}

