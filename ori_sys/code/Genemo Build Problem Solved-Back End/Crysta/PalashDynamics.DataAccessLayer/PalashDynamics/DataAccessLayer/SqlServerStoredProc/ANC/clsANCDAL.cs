namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.ANC
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ANC;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.ANC;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsANCDAL : clsBaseANCDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsANCDAL()
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

        public override IValueObject AddUpdateANCDocuments(IValueObject valueObject, clsUserVO UserVo)
        {
            AddUpdateANCDocumentBizActionVO nvo = valueObject as AddUpdateANCDocumentBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                clsANCDocumentsVO aNCDocuments = nvo.ANCDocuments;
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_ANC_Document where ANCID = ", nvo.ANCDocuments.ANCID, " And PatientID=", nvo.ANCDocuments.PatientID, " And PatientUnitID=", nvo.ANCDocuments.PatientUnitID }));
                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                foreach (clsANCDocumentsVO svo in nvo.ANCDocumentsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCDocument");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCDocuments.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DocumentDate", DbType.DateTime, svo.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ANCDocuments.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ANCDocuments.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.ANCDocuments.CoupleID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.ANCDocuments.CoupleUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCDocuments.ANCID);
                    this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, svo.Description);
                    this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, svo.Title);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, svo.AttachedFileName);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, svo.AttachedFileContent);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDeleted", DbType.Boolean, svo.IsDeleted);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.ANCDocuments.Isfreezed);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ANCDocuments.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "DocumentID", DbType.Int64, svo.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddANCBizActionVO nvo = valueObject as clsAddANCBizActionVO;
            try
            {
                clsANCVO aNCGeneralDetails = nvo.ANCGeneralDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCExaminationGeneralDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, aNCGeneralDetails.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, aNCGeneralDetails.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, aNCGeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, aNCGeneralDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, aNCGeneralDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "M", DbType.String, aNCGeneralDetails.M);
                this.dbServer.AddInParameter(storedProcCommand, "G", DbType.String, aNCGeneralDetails.G);
                this.dbServer.AddInParameter(storedProcCommand, "A", DbType.String, aNCGeneralDetails.A);
                this.dbServer.AddInParameter(storedProcCommand, "P", DbType.String, aNCGeneralDetails.P);
                this.dbServer.AddInParameter(storedProcCommand, "L", DbType.String, aNCGeneralDetails.L);
                this.dbServer.AddInParameter(storedProcCommand, "LMPDate", DbType.DateTime, aNCGeneralDetails.LMPDate);
                this.dbServer.AddInParameter(storedProcCommand, "EDDDate", DbType.DateTime, aNCGeneralDetails.EDDDate);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialRemark", DbType.String, aNCGeneralDetails.SpecialRemark);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, aNCGeneralDetails.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, aNCGeneralDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "DateofMarriage", DbType.DateTime, aNCGeneralDetails.DateofMarriage);
                this.dbServer.AddInParameter(storedProcCommand, "AgeOfMenarche", DbType.String, aNCGeneralDetails.AgeOfMenarche);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, aNCGeneralDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateANCHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateANCHistoryBizActionVO nvo = valueObject as clsAddUpdateANCHistoryBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                clsANCHistoryVO aNCHistory = nvo.ANCHistory;
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_ANC_ObstetricHistory where ANCID = ", aNCHistory.ANCID, " And PatientID=", aNCHistory.PatientID, " And PatientUnitID=", aNCHistory.PatientUnitID }));
                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCExaminationHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCHistory.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCHistory.ANCID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, aNCHistory.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, aNCHistory.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, aNCHistory.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, aNCHistory.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Hypertension", DbType.Boolean, aNCHistory.Hypertension);
                this.dbServer.AddInParameter(storedProcCommand, "TB", DbType.Boolean, aNCHistory.TB);
                this.dbServer.AddInParameter(storedProcCommand, "Diabeties", DbType.Boolean, aNCHistory.Diabeties);
                this.dbServer.AddInParameter(storedProcCommand, "Twins", DbType.Boolean, aNCHistory.Twins);
                this.dbServer.AddInParameter(storedProcCommand, "PersonalHistory", DbType.Boolean, aNCHistory.PersonalHistory);
                this.dbServer.AddInParameter(storedProcCommand, "LLMPDate", DbType.DateTime, aNCHistory.LLMPDate);
                this.dbServer.AddInParameter(storedProcCommand, "Menstrualcycle", DbType.String, aNCHistory.Menstrualcycle);
                this.dbServer.AddInParameter(storedProcCommand, "Duration", DbType.String, aNCHistory.Duration);
                this.dbServer.AddInParameter(storedProcCommand, "Disorder", DbType.String, aNCHistory.Disorder);
                this.dbServer.AddInParameter(storedProcCommand, "txtDiabeties", DbType.String, aNCHistory.txtDiabeties);
                this.dbServer.AddInParameter(storedProcCommand, "txtHypertension", DbType.String, aNCHistory.txtHypertension);
                this.dbServer.AddInParameter(storedProcCommand, "txtPersonalHistory", DbType.String, aNCHistory.txtPersonalHistory);
                this.dbServer.AddInParameter(storedProcCommand, "txtTB", DbType.String, aNCHistory.txtTB);
                this.dbServer.AddInParameter(storedProcCommand, "txtTwins", DbType.String, aNCHistory.txtTwins);
                this.dbServer.AddInParameter(storedProcCommand, "Weight", DbType.Single, aNCHistory.Weight);
                this.dbServer.AddInParameter(storedProcCommand, "Surgery", DbType.String, aNCHistory.Surgery);
                this.dbServer.AddInParameter(storedProcCommand, "RS", DbType.String, aNCHistory.RS);
                this.dbServer.AddInParameter(storedProcCommand, "Pulse", DbType.Single, aNCHistory.Pulse);
                this.dbServer.AddInParameter(storedProcCommand, "Otherimportantreleventfactor", DbType.String, aNCHistory.Otherimportantreleventfactor);
                this.dbServer.AddInParameter(storedProcCommand, "Lymphadenopathy", DbType.String, aNCHistory.Lymphadenopathy);
                this.dbServer.AddInParameter(storedProcCommand, "Hirsutism", DbType.Boolean, aNCHistory.Hirsutism);
                this.dbServer.AddInParameter(storedProcCommand, "HirsutismID", DbType.Int64, aNCHistory.HirsutismID);
                this.dbServer.AddInParameter(storedProcCommand, "Height", DbType.Single, aNCHistory.Height);
                this.dbServer.AddInParameter(storedProcCommand, "Goitre", DbType.String, aNCHistory.Goitre);
                this.dbServer.AddInParameter(storedProcCommand, "frequencyandtimingofintercourse", DbType.String, aNCHistory.frequencyandtimingofintercourse);
                this.dbServer.AddInParameter(storedProcCommand, "Flurseminis", DbType.String, aNCHistory.Flurseminis);
                this.dbServer.AddInParameter(storedProcCommand, "Edema", DbType.String, aNCHistory.Edema);
                this.dbServer.AddInParameter(storedProcCommand, "DrugsPresent", DbType.String, aNCHistory.DrugsPresent);
                this.dbServer.AddInParameter(storedProcCommand, "DrugsPast", DbType.String, aNCHistory.DrugsPast);
                this.dbServer.AddInParameter(storedProcCommand, "Drugs", DbType.String, aNCHistory.Drugs);
                this.dbServer.AddInParameter(storedProcCommand, "CVS", DbType.String, aNCHistory.CVS);
                this.dbServer.AddInParameter(storedProcCommand, "CNS", DbType.String, aNCHistory.CNS);
                this.dbServer.AddInParameter(storedProcCommand, "Breasts", DbType.Single, aNCHistory.Breasts);
                this.dbServer.AddInParameter(storedProcCommand, "BpInMm", DbType.Single, aNCHistory.BpInMm);
                this.dbServer.AddInParameter(storedProcCommand, "BpInHg", DbType.Single, aNCHistory.BpInHg);
                this.dbServer.AddInParameter(storedProcCommand, "AnyOtherfactor", DbType.String, aNCHistory.AnyOtherfactor);
                this.dbServer.AddInParameter(storedProcCommand, "Anaemia", DbType.String, aNCHistory.Anaemia);
                this.dbServer.AddInParameter(storedProcCommand, "TTIstDose", DbType.Boolean, aNCHistory.TTIstDose);
                this.dbServer.AddInParameter(storedProcCommand, "DateIstDose", DbType.DateTime, aNCHistory.DateIstDose);
                this.dbServer.AddInParameter(storedProcCommand, "TTIIndDose", DbType.Boolean, aNCHistory.TTIIstDose);
                this.dbServer.AddInParameter(storedProcCommand, "DateIIndDose", DbType.DateTime, aNCHistory.DateIIstDose);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, aNCHistory.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, aNCHistory.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, aNCHistory.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ANCHistory.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.ANCObsetricHistoryList != null) && (nvo.ANCObsetricHistoryList.Count > 0))
                {
                    foreach (clsANCObstetricHistoryVO yvo2 in nvo.ANCObsetricHistoryList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCObstetricHistory");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, aNCHistory.UnitID);
                        this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, aNCHistory.PatientID);
                        this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, aNCHistory.PatientUnitID);
                        this.dbServer.AddInParameter(command3, "Baby", DbType.Int64, yvo2.Baby);
                        this.dbServer.AddInParameter(command3, "DateYear", DbType.DateTime, yvo2.DateYear);
                        this.dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, aNCHistory.Isfreezed);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, yvo2.Status);
                        this.dbServer.AddInParameter(command3, "Gestation", DbType.String, yvo2.Gestation);
                        this.dbServer.AddInParameter(command3, "TypeofDelivery", DbType.String, yvo2.TypeofDelivery);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "ANCID", DbType.Int64, aNCHistory.ANCID);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, yvo2.ID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateExaminationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            AddUpdateANCExaminationBizActionVO nvo = valueObject as AddUpdateANCExaminationBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                clsANCExaminationDetailsVO aNCExaminationDetails = nvo.ANCExaminationDetails;
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_ANC_ExaminationDetails where ANCID = ", nvo.ANCExaminationDetails.ANCID, " And PatientID=", nvo.ANCExaminationDetails.PatientID, " And PatientUnitID=", nvo.ANCExaminationDetails.PatientUnitID }));
                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                foreach (clsANCExaminationDetailsVO svo2 in nvo.ANCExaminationDetailsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCExamination");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCExaminationDetails.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, aNCExaminationDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, aNCExaminationDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, aNCExaminationDetails.CoupleID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, aNCExaminationDetails.CoupleUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCExaminationDetails.ANCID);
                    this.dbServer.AddInParameter(storedProcCommand, "ExaDate", DbType.DateTime, svo2.ExaDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ExaTime", DbType.DateTime, svo2.ExaTime);
                    this.dbServer.AddInParameter(storedProcCommand, "Doctor", DbType.Int64, svo2.Doctor);
                    this.dbServer.AddInParameter(storedProcCommand, "PeriodOfPreg", DbType.String, svo2.PeriodOfPreg);
                    this.dbServer.AddInParameter(storedProcCommand, "Bp", DbType.Single, svo2.BP);
                    this.dbServer.AddInParameter(storedProcCommand, "Weight", DbType.Single, svo2.Weight);
                    this.dbServer.AddInParameter(storedProcCommand, "OademaID", DbType.Int64, svo2.OademaID);
                    this.dbServer.AddInParameter(storedProcCommand, "FundalHeight", DbType.Single, svo2.FundalHeight);
                    this.dbServer.AddInParameter(storedProcCommand, "PresenAndPos", DbType.Int64, svo2.PresenAndPos);
                    this.dbServer.AddInParameter(storedProcCommand, "FHS", DbType.Int64, svo2.FHS);
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, svo2.Remark);
                    this.dbServer.AddInParameter(storedProcCommand, "RelationtoBrim", DbType.String, svo2.RelationtoBrim);
                    this.dbServer.AddInParameter(storedProcCommand, "Treatment", DbType.String, svo2.Treatment);
                    this.dbServer.AddInParameter(storedProcCommand, "HTofUterus", DbType.String, svo2.HTofUterus);
                    this.dbServer.AddInParameter(storedProcCommand, "AbdGirth", DbType.String, svo2.AbdGirth);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, aNCExaminationDetails.Isfreezed);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo2.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo2.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateInvestigationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateANCInvestigationDetailsBizActionVO nvo = valueObject as clsAddUpdateANCInvestigationDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                clsANCInvestigationDetailsVO aNCInvestigationDetails = nvo.ANCInvestigationDetails;
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_ANC_InvestigationDetails where ANCID = ", nvo.ANCInvestigationDetails.ANCID, " And PatientID=", nvo.ANCInvestigationDetails.PatientID, " And PatientUnitID=", nvo.ANCInvestigationDetails.PatientUnitID }));
                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                foreach (clsANCInvestigationDetailsVO svo2 in nvo.ANCInvestigationDetailsList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCInvestigationDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, aNCInvestigationDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, aNCInvestigationDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, aNCInvestigationDetails.CoupleID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, aNCInvestigationDetails.CoupleUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCInvestigationDetails.ANCID);
                    this.dbServer.AddInParameter(storedProcCommand, "InvestigationID", DbType.Int64, svo2.InvestigationID);
                    this.dbServer.AddInParameter(storedProcCommand, "InvDate", DbType.DateTime, svo2.InvDate);
                    this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.String, svo2.Report);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, aNCInvestigationDetails.Isfreezed);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, svo2.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo2.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateObestricHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateObestricHistoryBizActionVO nvo = valueObject as clsAddUpdateObestricHistoryBizActionVO;
            try
            {
                clsANCObstetricHistoryVO aNCObstetricHistory = nvo.ANCObstetricHistory;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCObstetricHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, aNCObstetricHistory.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, aNCObstetricHistory.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MaturityComplication", DbType.String, aNCObstetricHistory.MaturityComplication);
                this.dbServer.AddInParameter(storedProcCommand, "ObstetricRemark", DbType.String, aNCObstetricHistory.ObstetricRemark);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, aNCObstetricHistory.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, aNCObstetricHistory.Status);
                this.dbServer.AddInParameter(storedProcCommand, "DeliveryMode", DbType.String, aNCObstetricHistory.ModeOfDelivary);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "HistoryID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, aNCObstetricHistory.HistoryID);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, aNCObstetricHistory.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ANCObstetricHistory.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ANCObstetricHistory.HistoryID = (long) this.dbServer.GetParameterValue(storedProcCommand, "HistoryID");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateSuggestion(IValueObject valueObject, clsUserVO UserVo)
        {
            AddUpdateANCSuggestionBizActionVO nvo = valueObject as AddUpdateANCSuggestionBizActionVO;
            try
            {
                clsANCSuggestionVO aNCSuggestion = nvo.ANCSuggestion;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCSuggestion");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ANCSuggestion.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCSuggestion.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ANCSuggestion.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ANCSuggestion.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.ANCSuggestion.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.ANCSuggestion.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCSuggestion.ANCID);
                this.dbServer.AddInParameter(storedProcCommand, "Smoking", DbType.Boolean, nvo.ANCSuggestion.Smoking);
                this.dbServer.AddInParameter(storedProcCommand, "Alcoholic", DbType.Boolean, nvo.ANCSuggestion.Alcoholic);
                this.dbServer.AddInParameter(storedProcCommand, "Medication", DbType.Boolean, nvo.ANCSuggestion.Medication);
                this.dbServer.AddInParameter(storedProcCommand, "Exercise", DbType.Boolean, nvo.ANCSuggestion.Exercise);
                this.dbServer.AddInParameter(storedProcCommand, "Xray", DbType.Boolean, nvo.ANCSuggestion.Xray);
                this.dbServer.AddInParameter(storedProcCommand, "IrregularDiet", DbType.Boolean, nvo.ANCSuggestion.IrregularDiet);
                this.dbServer.AddInParameter(storedProcCommand, "Exertion", DbType.Boolean, nvo.ANCSuggestion.Exertion);
                this.dbServer.AddInParameter(storedProcCommand, "Cplace", DbType.Boolean, nvo.ANCSuggestion.Cplace);
                this.dbServer.AddInParameter(storedProcCommand, "HeavyObject", DbType.Boolean, nvo.ANCSuggestion.HeavyObject);
                this.dbServer.AddInParameter(storedProcCommand, "Tea", DbType.Boolean, nvo.ANCSuggestion.Tea);
                this.dbServer.AddInParameter(storedProcCommand, "Bag", DbType.Boolean, nvo.ANCSuggestion.Bag);
                this.dbServer.AddInParameter(storedProcCommand, "Sheets", DbType.Boolean, nvo.ANCSuggestion.Sheets);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.ANCSuggestion.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.ANCSuggestion.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ANCSuggestion.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ANCSuggestion.UnitID = (long) this.dbServer.GetParameterValue(storedProcCommand, "UnitID");
                if (nvo.ANCSuggestion.IsClosed)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateANCTherapyStatus");
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.ANCSuggestion.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.ANCSuggestion.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "CoupleID", DbType.Int64, nvo.ANCSuggestion.CoupleID);
                    this.dbServer.AddInParameter(command2, "CoupleUnitID", DbType.Int64, nvo.ANCSuggestion.CoupleUnitID);
                    this.dbServer.AddInParameter(command2, "ANCID", DbType.Int64, nvo.ANCSuggestion.ANCID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "IsClosed", DbType.Boolean, nvo.ANCSuggestion.IsClosed);
                    this.dbServer.ExecuteNonQuery(command2);
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateUSGDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateUSGdetailsBizActionVO nvo = valueObject as clsAddUpdateUSGdetailsBizActionVO;
            try
            {
                clsANCUSGDetailsVO aNCUSGDetails = nvo.ANCUSGDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateANCUSGDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCUSGDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, aNCUSGDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, aNCUSGDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, aNCUSGDetails.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, aNCUSGDetails.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCUSGDetails.ANCID);
                this.dbServer.AddInParameter(storedProcCommand, "SIFT", DbType.DateTime, aNCUSGDetails.SIFT);
                this.dbServer.AddInParameter(storedProcCommand, "txtSIFT", DbType.String, aNCUSGDetails.txtSIFT);
                this.dbServer.AddInParameter(storedProcCommand, "GIFT", DbType.DateTime, aNCUSGDetails.GIFT);
                this.dbServer.AddInParameter(storedProcCommand, "txtGIFT", DbType.String, aNCUSGDetails.txtGIFT);
                this.dbServer.AddInParameter(storedProcCommand, "IVF", DbType.DateTime, aNCUSGDetails.IVF);
                this.dbServer.AddInParameter(storedProcCommand, "txtIVF", DbType.String, aNCUSGDetails.txtIVF);
                this.dbServer.AddInParameter(storedProcCommand, "Sonography", DbType.String, aNCUSGDetails.Sonography);
                this.dbServer.AddInParameter(storedProcCommand, "OvulationMonitors", DbType.String, aNCUSGDetails.OvulationMonitors);
                this.dbServer.AddInParameter(storedProcCommand, "Mysteroscopy", DbType.String, aNCUSGDetails.Mysteroscopy);
                this.dbServer.AddInParameter(storedProcCommand, "Laparoscopy", DbType.String, aNCUSGDetails.Laparoscopy);
                this.dbServer.AddInParameter(storedProcCommand, "INVTreatment", DbType.String, aNCUSGDetails.INVTreatment);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, aNCUSGDetails.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, aNCUSGDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCDocumentBizActionVO nvo = valueObject as clsDeleteANCDocumentBizActionVO;
            try
            {
                clsANCDocumentsVO aNCDocuments = nvo.ANCDocuments;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteANCDocument");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, aNCDocuments.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCDocuments.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCDocuments.ANCID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteExamination(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCExaminationBizActionVO nvo = valueObject as clsDeleteANCExaminationBizActionVO;
            try
            {
                clsANCExaminationDetailsVO aNCExaminationDetails = nvo.ANCExaminationDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteANCExamination");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, aNCExaminationDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCExaminationDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCExaminationDetails.ANCID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteInvestigation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCInvestigationBizActionVO nvo = valueObject as clsDeleteANCInvestigationBizActionVO;
            try
            {
                clsANCInvestigationDetailsVO aNCInvestigationDetails = nvo.ANCInvestigationDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteANCInvestigation");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCInvestigationDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, aNCInvestigationDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCInvestigationDetails.ANCID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteObestericHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteObstericHistoryBizActionVO nvo = valueObject as clsDeleteObstericHistoryBizActionVO;
            try
            {
                clsANCObstetricHistoryVO aNCObstetricHistory = nvo.ANCObstetricHistory;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteANCObstericHistory");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, aNCObstetricHistory.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCObstetricHistory.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCObstetricHistory.HistoryID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject DeleteUSG(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCUSGBizActionVO nvo = valueObject as clsDeleteANCUSGBizActionVO;
            try
            {
                clsANCUSGDetailsVO aNCUSGDetails = nvo.ANCUSGDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteANCUSG");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, aNCUSGDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, aNCUSGDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, aNCUSGDetails.ANCID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return valueObject;
        }

        public override IValueObject GetANCDocumentList(IValueObject valueObject, clsUserVO UserVo)
        {
            GetANCDocumentBizActionVO nvo = valueObject as GetANCDocumentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCDocumentList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCDocument.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCDocument.ANCID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCDocumentList == null)
                    {
                        nvo.ANCDocumentList = new List<clsANCDocumentsVO>();
                    }
                    while (reader.Read())
                    {
                        clsANCDocumentsVO item = new clsANCDocumentsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"])),
                            AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        nvo.ANCDocumentList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCExaminationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCExaminationBizActionVO nvo = valueObject as clsGetANCExaminationBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCExaminationList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCExaminationDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCExaminationDetails.ANCID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCExaminationList == null)
                    {
                        nvo.ANCExaminationList = new List<clsANCExaminationDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsANCExaminationDetailsVO item = new clsANCExaminationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ExaDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExaDate"]))),
                            ExaTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExaTime"]))),
                            Doctor = Convert.ToInt64(DALHelper.HandleDBNull(reader["Doctor"])),
                            PeriodOfPreg = Convert.ToString(DALHelper.HandleDBNull(reader["PeriodOfPreg"])),
                            BP = Convert.ToSingle(DALHelper.HandleDBNull(reader["Bp"])),
                            Weight = Convert.ToSingle(DALHelper.HandleDBNull(reader["Weight"])),
                            Oadema = Convert.ToString(DALHelper.HandleDBNull(reader["Oadema"])),
                            FundalHeight = Convert.ToSingle(DALHelper.HandleDBNull(reader["FundalHeight"])),
                            PresenAndPos = Convert.ToInt64(DALHelper.HandleDBNull(reader["PresenAndPos"])),
                            FHS = Convert.ToInt64(DALHelper.HandleDBNull(reader["FHS"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            Isfreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]),
                            Treatment = Convert.ToString(DALHelper.HandleDBNull(reader["Treatment"])),
                            HTofUterus = Convert.ToString(DALHelper.HandleDBNull(reader["HTofUterus"])),
                            RelationtoBrim = Convert.ToString(DALHelper.HandleDBNull(reader["RelationtoBrim"])),
                            AbdGirth = Convert.ToString(DALHelper.HandleDBNull(reader["AbdGirth"])),
                            PresenAndPosDescription = Convert.ToString(DALHelper.HandleDBNull(reader["PresenAndPosDescription"])),
                            FHSDescription = Convert.ToString(DALHelper.HandleDBNull(reader["FHSDescription"])),
                            OademaID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OademaID"])),
                            DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]))
                        };
                        nvo.ANCExaminationList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCGeneralDetailsBizActionVO nvo = valueObject as clsGetANCGeneralDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCGeneralDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ANCGeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ANCGeneralDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCGeneralDetails == null)
                    {
                        nvo.ANCGeneralDetails = new clsANCVO();
                    }
                    while (reader.Read())
                    {
                        nvo.ANCGeneralDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ANCGeneralDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ANCGeneralDetails.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.ANCGeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.ANCGeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.ANCGeneralDetails.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.ANCGeneralDetails.ANC_Code = Convert.ToString(DALHelper.HandleDBNull(reader["ANC_Code"]));
                        nvo.ANCGeneralDetails.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ANCDate"])));
                        nvo.ANCGeneralDetails.LMPDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["LMPDate"])));
                        nvo.ANCGeneralDetails.EDDDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["EDDDate"])));
                        nvo.ANCGeneralDetails.M = Convert.ToString(DALHelper.HandleDBNull(reader["M"]));
                        nvo.ANCGeneralDetails.G = Convert.ToString(DALHelper.HandleDBNull(reader["G"]));
                        nvo.ANCGeneralDetails.L = Convert.ToString(DALHelper.HandleDBNull(reader["L"]));
                        nvo.ANCGeneralDetails.P = Convert.ToString(DALHelper.HandleDBNull(reader["P"]));
                        nvo.ANCGeneralDetails.A = Convert.ToString(DALHelper.HandleDBNull(reader["A"]));
                        nvo.ANCGeneralDetails.SpecialRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRemark"]));
                        nvo.ANCGeneralDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.ANCGeneralDetails.DateofMarriage = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateofMarriage"])));
                        nvo.ANCGeneralDetails.AgeOfMenarche = Convert.ToString(DALHelper.HandleDBNull(reader["AgeOfMenarche"]));
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsANCGetGeneralDetailsListBizActionVO nvo = valueObject as clsANCGetGeneralDetailsListBizActionVO;
            nvo.ANCGeneralDetailsList = new List<clsANCVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCGeneralDetailsList_New");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ANCGeneralDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ANCGeneralDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsANCVO item = new clsANCVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"])),
                            ANC_Code = Convert.ToString(DALHelper.HandleDBNull(reader["ANC_Code"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            LMPDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["LMPDate"]))),
                            EDDDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["EDDDate"]))),
                            M = Convert.ToString(DALHelper.HandleDBNull(reader["M"])),
                            G = Convert.ToString(DALHelper.HandleDBNull(reader["G"])),
                            L = Convert.ToString(DALHelper.HandleDBNull(reader["L"])),
                            P = Convert.ToString(DALHelper.HandleDBNull(reader["P"])),
                            A = Convert.ToString(DALHelper.HandleDBNull(reader["A"])),
                            SpecialRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRemark"])),
                            Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"])),
                            DateofMarriage = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateofMarriage"]))),
                            AgeOfMenarche = Convert.ToString(DALHelper.HandleDBNull(reader["AgeOfMenarche"]))
                        };
                        nvo.ANCGeneralDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCHistoryBizActionVO nvo = valueObject as clsGetANCHistoryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCHistory.ANCID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCHistory == null)
                    {
                        nvo.ANCHistory = new clsANCHistoryVO();
                    }
                    while (reader.Read())
                    {
                        nvo.ANCHistory.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HisID"]));
                        nvo.ANCHistory.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HisUnitID"]));
                        nvo.ANCHistory.ANCID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ANCID"]));
                        nvo.ANCHistory.Hypertension = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypertension"]));
                        nvo.ANCHistory.TB = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TB"]));
                        nvo.ANCHistory.Diabeties = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Diabeties"]));
                        nvo.ANCHistory.Twins = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Twins"]));
                        nvo.ANCHistory.PersonalHistory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PersonalHistory"]));
                        nvo.ANCHistory.Hirsutism = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hirsutism"]));
                        nvo.ANCHistory.HirsutismID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HirsutismID"]));
                        nvo.ANCHistory.LLMPDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["LLMPDate"])));
                        nvo.ANCHistory.Menstrualcycle = Convert.ToString(DALHelper.HandleDBNull(reader["Menstrualcycle"]));
                        nvo.ANCHistory.Duration = Convert.ToString(DALHelper.HandleDBNull(reader["Duration"]));
                        nvo.ANCHistory.Disorder = Convert.ToString(DALHelper.HandleDBNull(reader["Disorder"]));
                        nvo.ANCHistory.txtDiabeties = Convert.ToString(DALHelper.HandleDBNull(reader["txtDiabeties"]));
                        nvo.ANCHistory.txtTwins = Convert.ToString(DALHelper.HandleDBNull(reader["txtTwins"]));
                        nvo.ANCHistory.txtHypertension = Convert.ToString(DALHelper.HandleDBNull(reader["txtHypertension"]));
                        nvo.ANCHistory.txtTB = Convert.ToString(DALHelper.HandleDBNull(reader["txtTB"]));
                        nvo.ANCHistory.txtPersonalHistory = Convert.ToString(DALHelper.HandleDBNull(reader["txtPersonalHistory"]));
                        nvo.ANCHistory.DrugsPresent = Convert.ToString(DALHelper.HandleDBNull(reader["DrugsPresent"]));
                        nvo.ANCHistory.DrugsPast = Convert.ToString(DALHelper.HandleDBNull(reader["DrugsPast"]));
                        nvo.ANCHistory.Surgery = Convert.ToString(DALHelper.HandleDBNull(reader["Surgery"]));
                        nvo.ANCHistory.Drugs = Convert.ToString(DALHelper.HandleDBNull(reader["Drugs"]));
                        nvo.ANCHistory.Anaemia = Convert.ToString(DALHelper.HandleDBNull(reader["Anaemia"]));
                        nvo.ANCHistory.Lymphadenopathy = Convert.ToString(DALHelper.HandleDBNull(reader["Lymphadenopathy"]));
                        nvo.ANCHistory.Edema = Convert.ToString(DALHelper.HandleDBNull(reader["Edema"]));
                        nvo.ANCHistory.Goitre = Convert.ToString(DALHelper.HandleDBNull(reader["Goitre"]));
                        nvo.ANCHistory.CVS = Convert.ToString(DALHelper.HandleDBNull(reader["CVS"]));
                        nvo.ANCHistory.CNS = Convert.ToString(DALHelper.HandleDBNull(reader["CNS"]));
                        nvo.ANCHistory.RS = Convert.ToString(DALHelper.HandleDBNull(reader["RS"]));
                        nvo.ANCHistory.Flurseminis = Convert.ToString(DALHelper.HandleDBNull(reader["Flurseminis"]));
                        nvo.ANCHistory.AnyOtherfactor = Convert.ToString(DALHelper.HandleDBNull(reader["AnyOtherfactor"]));
                        nvo.ANCHistory.Otherimportantreleventfactor = Convert.ToString(DALHelper.HandleDBNull(reader["Otherimportantreleventfactor"]));
                        nvo.ANCHistory.frequencyandtimingofintercourse = Convert.ToString(DALHelper.HandleDBNull(reader["frequencyandtimingofintercourse"]));
                        nvo.ANCHistory.TTIstDose = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TTIstDose"]));
                        nvo.ANCHistory.DateIstDose = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DateIstDose"])));
                        nvo.ANCHistory.TTIIstDose = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TTIIndDose"]));
                        nvo.ANCHistory.DateIIstDose = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["DateIIndDose"])));
                        nvo.ANCHistory.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.ANCHistory.Pulse = Convert.ToSingle(DALHelper.HandleDBNull(reader["Pulse"]));
                        nvo.ANCHistory.BpInMm = Convert.ToSingle(DALHelper.HandleDBNull(reader["BpInMm"]));
                        nvo.ANCHistory.BpInHg = Convert.ToSingle(DALHelper.HandleDBNull(reader["BpInHg"]));
                        nvo.ANCHistory.Weight = Convert.ToSingle(DALHelper.HandleDBNull(reader["Weight"]));
                        nvo.ANCHistory.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        nvo.ANCHistory.Breasts = Convert.ToString(DALHelper.HandleDBNull(reader["Breasts"]));
                        nvo.ANCHistory.Edema = Convert.ToString(DALHelper.HandleDBNull(reader["Edema"]));
                        clsANCObstetricHistoryVO item = new clsANCObstetricHistoryVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            TypeofDelivery = Convert.ToString(DALHelper.HandleDBNull(reader["TypeofDelivery"])),
                            DateYear = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateYear"]))),
                            Baby = Convert.ToInt64(DALHelper.HandleDBNull(reader["Baby"])),
                            Gestation = Convert.ToString(DALHelper.HandleDBNull(reader["Gestation"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ANCObsetricHistoryList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCInvestigationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCInvestigationListBizActionVO nvo = valueObject as clsGetANCInvestigationListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCInvestigationList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCInvestigationDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCInvestigationDetails.ANCID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCInvestigationDetailsList == null)
                    {
                        nvo.ANCInvestigationDetailsList = new List<clsANCInvestigationDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsANCInvestigationDetailsVO item = new clsANCInvestigationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            InvDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["InvDate"])),
                            InvestigationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InvestigationID"])),
                            Investigation = Convert.ToString(DALHelper.HandleDBNull(reader["Investigation"])),
                            Report = Convert.ToString(DALHelper.HandleDBNull(reader["Report"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]))
                        };
                        nvo.ANCInvestigationDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCSuggestion(IValueObject valueObject, clsUserVO UserVo)
        {
            GetANCSuggestionBizActionVO nvo = valueObject as GetANCSuggestionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCSuggestion");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCSuggestion.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCSuggestion.ANCID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCSuggestion == null)
                    {
                        nvo.ANCSuggestion = new clsANCSuggestionVO();
                    }
                    while (reader.Read())
                    {
                        nvo.ANCSuggestion.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ANCSuggestion.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ANCSuggestion.Smoking = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Smoking"]));
                        nvo.ANCSuggestion.Alcoholic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Alcoholic"]));
                        nvo.ANCSuggestion.Medication = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Medication"]));
                        nvo.ANCSuggestion.Exercise = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Exercise"]));
                        nvo.ANCSuggestion.Xray = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Xray"]));
                        nvo.ANCSuggestion.IrregularDiet = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IrregularDiet"]));
                        nvo.ANCSuggestion.Exertion = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Exertion"]));
                        nvo.ANCSuggestion.Cplace = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Cplace"]));
                        nvo.ANCSuggestion.Tea = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tea"]));
                        nvo.ANCSuggestion.Bag = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Bag"]));
                        nvo.ANCSuggestion.Sheets = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Sheets"]));
                        nvo.ANCSuggestion.HeavyObject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HeavyObject"]));
                        nvo.ANCSuggestion.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        nvo.ANCSuggestion.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetANCUSGList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCUSGListBizActionVO nvo = valueObject as clsGetANCUSGListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCUSGList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ANCUSGDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ANCID", DbType.Int64, nvo.ANCUSGDetails.ANCID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCUSGDetails == null)
                    {
                        nvo.ANCUSGDetails = new clsANCUSGDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.ANCUSGDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ANCUSGDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ANCUSGDetails.SIFT = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["SIFT"])));
                        nvo.ANCUSGDetails.txtSIFT = Convert.ToString(DALHelper.HandleDBNull(reader["txtSIFT"]));
                        nvo.ANCUSGDetails.GIFT = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["GIFT"])));
                        nvo.ANCUSGDetails.txtGIFT = Convert.ToString(DALHelper.HandleDBNull(reader["txtGIFT"]));
                        nvo.ANCUSGDetails.IVF = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["IVF"])));
                        nvo.ANCUSGDetails.txtIVF = Convert.ToString(DALHelper.HandleDBNull(reader["txtIVF"]));
                        nvo.ANCUSGDetails.Sonography = Convert.ToString(DALHelper.HandleDBNull(reader["Sonography"]));
                        nvo.ANCUSGDetails.OvulationMonitors = Convert.ToString(DALHelper.HandleDBNull(reader["OvulationMonitors"]));
                        nvo.ANCUSGDetails.Laparoscopy = Convert.ToString(DALHelper.HandleDBNull(reader["Laparoscopy"]));
                        nvo.ANCUSGDetails.INVTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["INVTreatment"]));
                        nvo.ANCUSGDetails.Mysteroscopy = Convert.ToString(DALHelper.HandleDBNull(reader["Mysteroscopy"]));
                        nvo.ANCUSGDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.ANCUSGDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetObestricHistoryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetObstricHistoryListBizActionVO nvo = valueObject as clsGetObstricHistoryListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetANCObestrecHistoryList");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "HistoryID", DbType.Int64, nvo.ANCObstetricHistory.HistoryID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ANCObsetricHistoryList == null)
                    {
                        nvo.ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
                    }
                    while (reader.Read())
                    {
                        clsANCObstetricHistoryVO item = new clsANCObstetricHistoryVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            MaturityComplication = (string) DALHelper.HandleDBNull(reader["MaturityComplication"]),
                            ObstetricRemark = (string) DALHelper.HandleDBNull(reader["ObstetricRemark"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            Isfreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]),
                            ModeOfDelivary = (string) DALHelper.HandleDBNull(reader["DeliveryMode"])
                        };
                        nvo.ANCObsetricHistoryList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }
    }
}

