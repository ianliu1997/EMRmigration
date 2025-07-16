namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using PalashDynamics.ValueObjects.EMR;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    internal class clsIVFDashboad_PlanTherapyDAL : clsBaseIVFDashboad_PlanTherapyDAL
    {
        private LogManager logManager;
        private Database dbServer;
        private string IsIsthambul = string.Empty;

        private clsIVFDashboad_PlanTherapyDAL()
        {
            try
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
                    this.IsIsthambul = ConfigurationManager.AppSettings["IsIstanbul"];
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddTherapyDocumentBizActionVO nvo = valueObject as clsIVFDashboard_AddTherapyDocumentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddTherapyDocument");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DocumentDate", DbType.DateTime, nvo.Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Details.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, nvo.Details.Title);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, nvo.Details.AttachedFileName);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, nvo.Details.AttachedFileContent);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Details.Status);
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
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Details.ID);
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

        public override IValueObject AddUpdateBirthDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddBirthDetailsBizActionVO nvo = valueObject as clsIVFDashboard_AddBirthDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateBirthDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.BirthDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.BirthDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.BirthDetails.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.BirthDetails.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DateOfBirth", DbType.DateTime, nvo.BirthDetails.DateOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "Week", DbType.Int64, nvo.BirthDetails.Week);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.BirthDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "TownOfBirth", DbType.String, nvo.BirthDetails.TownOfBirth);
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.BirthDetails.Country));
                this.dbServer.AddInParameter(storedProcCommand, "CountryOfBirthID", DbType.Int64, nvo.BirthDetails.CountryOfBirthID);
                this.dbServer.AddInParameter(storedProcCommand, "DiedPerinatallyID", DbType.Int64, nvo.BirthDetails.DiedPerinatallyID);
                this.dbServer.AddInParameter(storedProcCommand, "DeathPostportumID", DbType.Int64, nvo.BirthDetails.DeathPostportumID);
                this.dbServer.AddInParameter(storedProcCommand, "ChildID", DbType.Int64, nvo.BirthDetails.ChildID);
                this.dbServer.AddInParameter(storedProcCommand, "ConditionID", DbType.Int64, nvo.BirthDetails.ConditionID);
                this.dbServer.AddInParameter(storedProcCommand, "DeliveryMethodID", DbType.Int64, nvo.BirthDetails.DeliveryMethodID);
                this.dbServer.AddInParameter(storedProcCommand, "WeightAtBirth", DbType.Single, nvo.BirthDetails.WeightAtBirth);
                this.dbServer.AddInParameter(storedProcCommand, "LengthAtBirth", DbType.Single, nvo.BirthDetails.LengthAtBirth);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, Security.base64Encode(nvo.BirthDetails.FirstName));
                this.dbServer.AddInParameter(storedProcCommand, "Surname", DbType.String, Security.base64Encode(nvo.BirthDetails.Surname));
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.BirthDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.BirthDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.BirthDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateBirthDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddBirthDetailsListBizActionVO nvo = valueObject as clsIVFDashboard_AddBirthDetailsListBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                if ((nvo.BirthDetailsList != null) && (nvo.BirthDetailsList.Count > 0))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.BirthDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.BirthDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.BirthDetails.TherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.BirthDetails.TherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "FetalHeartNo", DbType.Int64, nvo.BirthDetails.FetalHeartNo);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.BirthDetails.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "SurrogateID", DbType.Int64, nvo.BirthDetails.SurrogateID);
                    this.dbServer.AddInParameter(storedProcCommand, "SurrogateUnitID", DbType.Int64, nvo.BirthDetails.SurrogateUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "SurrogatePatientMrNo", DbType.String, nvo.BirthDetails.SurrogatePatientMrNo);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.BirthDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    foreach (clsIVFDashboard_BirthDetailsVO svo in nvo.BirthDetailsList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetailsList");
                        this.dbServer.AddInParameter(command, "ID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "BirthDetailsID", DbType.Int64, svo.BirthDetailsID);
                        this.dbServer.AddInParameter(command, "ChildNoStr", DbType.String, svo.ChildNoStr);
                        this.dbServer.AddInParameter(command, "BirthWeight", DbType.String, svo.BirthWeight);
                        this.dbServer.AddInParameter(command, "MedicalConditions", DbType.String, svo.MedicalConditions);
                        this.dbServer.AddInParameter(command, "WeeksofGestation", DbType.String, svo.WeeksofGestation);
                        this.dbServer.AddInParameter(command, "DeliveryTypeID", DbType.Int64, svo.DeliveryTypeID);
                        this.dbServer.AddInParameter(command, "ActivityID", DbType.Int64, svo.ActivityID);
                        this.dbServer.AddInParameter(command, "ActivityPoint", DbType.Int64, svo.ActivityPoint);
                        this.dbServer.AddInParameter(command, "Pulse", DbType.Int64, svo.Pulse);
                        this.dbServer.AddInParameter(command, "PulsePoint", DbType.Int64, svo.PulsePoint);
                        this.dbServer.AddInParameter(command, "Grimace", DbType.Int64, svo.Grimace);
                        this.dbServer.AddInParameter(command, "GrimacePoint", DbType.Int64, svo.GrimacePoint);
                        this.dbServer.AddInParameter(command, "Appearance", DbType.Int64, svo.Appearance);
                        this.dbServer.AddInParameter(command, "AppearancePoint", DbType.Int64, svo.AppearancePoint);
                        this.dbServer.AddInParameter(command, "Respiration", DbType.Int64, svo.Respiration);
                        this.dbServer.AddInParameter(command, "RespirationPoint", DbType.Int64, svo.RespirationPoint);
                        this.dbServer.AddInParameter(command, "APGARScore", DbType.Int64, svo.APGARScore);
                        this.dbServer.AddInParameter(command, "Conclusion", DbType.String, svo.Conclusion);
                        this.dbServer.AddInParameter(command, "APGARScoreID", DbType.Int64, svo.APGARScoreID);
                        this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.BirthDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateDiagnosis(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsIVFDashboard_AddDiagnosisBizActionVO nvo = (clsIVFDashboard_AddDiagnosisBizActionVO) valueObject;
            List<clsDignosisVO> diagnosisList = nvo.DiagnosisList;
            if (nvo.DiagnosisDetails.IsModify)
            {
            }
            try
            {
                foreach (clsDignosisVO svo2 in diagnosisList)
                {
                    connection = this.dbServer.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDiagnosis");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "DiagnosisID", DbType.Int64, svo2.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "TransactionTypeID", DbType.Int64, svo2.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.DiagnosisDetails = null;
            }
            return nvo;
        }

        private clsIVFDashboard_AddPlanTherapyBizActionVO AddUpdateIVFDashBoardPlanTherapy(clsIVFDashboard_AddPlanTherapyBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePlanTherapyForTest");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, BizActionObj.TherapyDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, BizActionObj.TherapyDetails.PatientUintId);
                this.dbServer.AddInParameter(storedProcCommand, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                this.dbServer.AddInParameter(storedProcCommand, "SurrogateUnitID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.AttachedSurrogate);
                this.dbServer.AddInParameter(storedProcCommand, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "CycleDuration", DbType.String, BizActionObj.TherapyDetails.CycleDuration);
                this.dbServer.AddInParameter(storedProcCommand, "PlannedTreatmentID", DbType.Int64, BizActionObj.TherapyDetails.PlannedTreatmentID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalPlannedTreatmentID", DbType.Int64, BizActionObj.TherapyDetails.FinalPlannedTreatmentID);
                this.dbServer.AddInParameter(storedProcCommand, "PlannedNoofEmbryos", DbType.String, BizActionObj.TherapyDetails.PlannedEmbryos);
                this.dbServer.AddInParameter(storedProcCommand, "MainInductionID", DbType.Int64, BizActionObj.TherapyDetails.MainInductionID);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicianId", DbType.Int64, BizActionObj.TherapyDetails.PhysicianId);
                this.dbServer.AddInParameter(storedProcCommand, "ExternalSimulationID", DbType.Int64, BizActionObj.TherapyDetails.ExternalSimulationID);
                this.dbServer.AddInParameter(storedProcCommand, "Cyclecode", DbType.String, BizActionObj.TherapyDetails.Cyclecode);
                this.dbServer.AddInParameter(storedProcCommand, "PlannedSpermCollectionID", DbType.Int64, BizActionObj.TherapyDetails.PlannedSpermCollectionID);
                this.dbServer.AddInParameter(storedProcCommand, "ProtocolTypeID", DbType.Int64, BizActionObj.TherapyDetails.ProtocolTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Pill", DbType.String, BizActionObj.TherapyDetails.Pill);
                this.dbServer.AddInParameter(storedProcCommand, "PillStartDate", DbType.DateTime, BizActionObj.TherapyDetails.PillStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "PillEndDate", DbType.DateTime, BizActionObj.TherapyDetails.PillEndDate);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyGeneralNotes", DbType.String, BizActionObj.TherapyDetails.TherapyNotes);
                this.dbServer.AddInParameter(storedProcCommand, "PCOS", DbType.Boolean, BizActionObj.TherapyDetails.PCOS);
                this.dbServer.AddInParameter(storedProcCommand, "Hypogonadotropic", DbType.Boolean, BizActionObj.TherapyDetails.Hypogonadotropic);
                this.dbServer.AddInParameter(storedProcCommand, "Tuberculosis", DbType.Boolean, BizActionObj.TherapyDetails.Tuberculosis);
                this.dbServer.AddInParameter(storedProcCommand, "Endometriosis", DbType.Boolean, BizActionObj.TherapyDetails.Endometriosis);
                this.dbServer.AddInParameter(storedProcCommand, "UterineFactors", DbType.Boolean, BizActionObj.TherapyDetails.UterineFactors);
                this.dbServer.AddInParameter(storedProcCommand, "TubalFactors", DbType.Boolean, BizActionObj.TherapyDetails.TubalFactors);
                this.dbServer.AddInParameter(storedProcCommand, "DiminishedOvarian", DbType.Boolean, BizActionObj.TherapyDetails.DiminishedOvarian);
                this.dbServer.AddInParameter(storedProcCommand, "PrematureOvarianFailure", DbType.Boolean, BizActionObj.TherapyDetails.PrematureOvarianFailure);
                this.dbServer.AddInParameter(storedProcCommand, "LutealPhasedefect", DbType.Boolean, BizActionObj.TherapyDetails.LutealPhasedefect);
                this.dbServer.AddInParameter(storedProcCommand, "HypoThyroid", DbType.Boolean, BizActionObj.TherapyDetails.HypoThyroid);
                this.dbServer.AddInParameter(storedProcCommand, "HyperThyroid", DbType.Boolean, BizActionObj.TherapyDetails.HyperThyroid);
                this.dbServer.AddInParameter(storedProcCommand, "MaleFactors", DbType.Boolean, BizActionObj.TherapyDetails.MaleFactors);
                this.dbServer.AddInParameter(storedProcCommand, "OtherFactors", DbType.Boolean, BizActionObj.TherapyDetails.OtherFactors);
                this.dbServer.AddInParameter(storedProcCommand, "UnknownFactors", DbType.Boolean, BizActionObj.TherapyDetails.UnknownFactors);
                this.dbServer.AddInParameter(storedProcCommand, "FemaleFactorsOnly", DbType.Boolean, BizActionObj.TherapyDetails.FemaleFactorsOnly);
                this.dbServer.AddInParameter(storedProcCommand, "FemaleandMaleFactors", DbType.Boolean, BizActionObj.TherapyDetails.FemaleandMaleFactors);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "OPUDate", DbType.DateTime, BizActionObj.TherapyDetails.OPUtDate);
                this.dbServer.AddInParameter(storedProcCommand, "LongtermMedication", DbType.String, BizActionObj.TherapyDetails.LongtermMedication);
                this.dbServer.AddInParameter(storedProcCommand, "AssistedHatching", DbType.Boolean, BizActionObj.TherapyDetails.AssistedHatching);
                this.dbServer.AddInParameter(storedProcCommand, "IMSI", DbType.Boolean, BizActionObj.TherapyDetails.IMSI);
                this.dbServer.AddInParameter(storedProcCommand, "CryoPreservation", DbType.Boolean, BizActionObj.TherapyDetails.CryoPreservation);
                this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                this.dbServer.AddInParameter(storedProcCommand, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                this.dbServer.AddInParameter(storedProcCommand, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                this.dbServer.AddInParameter(storedProcCommand, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                this.dbServer.AddInParameter(storedProcCommand, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                this.dbServer.AddParameter(storedProcCommand, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsSurrogateCalendar", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogateCalendar);
                this.dbServer.AddInParameter(storedProcCommand, "IsSurrogateDrug", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogateDrug);
                this.dbServer.AddInParameter(storedProcCommand, "SpermSourceId", DbType.Int64, BizActionObj.TherapyDetails.SpermSource);
                this.dbServer.AddInParameter(storedProcCommand, "SpermCollectionId", DbType.Int64, BizActionObj.TherapyDetails.PlannedSpermCollection);
                this.dbServer.AddInParameter(storedProcCommand, "StartOvarian", DbType.DateTime, BizActionObj.TherapyDetails.StartOvarian);
                this.dbServer.AddInParameter(storedProcCommand, "StartTrigger", DbType.DateTime, BizActionObj.TherapyDetails.StartTrigger);
                this.dbServer.AddInParameter(storedProcCommand, "StartStimulation", DbType.DateTime, BizActionObj.TherapyDetails.StartStimulation);
                this.dbServer.AddInParameter(storedProcCommand, "EndOvarian", DbType.DateTime, BizActionObj.TherapyDetails.EndOvarian);
                this.dbServer.AddInParameter(storedProcCommand, "EndStimulation", DbType.DateTime, BizActionObj.TherapyDetails.EndStimulation);
                this.dbServer.AddInParameter(storedProcCommand, "TriggerTime", DbType.DateTime, BizActionObj.TherapyDetails.TriggerTime);
                this.dbServer.AddInParameter(storedProcCommand, "SpermCollectionDate", DbType.DateTime, BizActionObj.TherapyDetails.SpermCollectionDate);
                this.dbServer.AddInParameter(storedProcCommand, "Note", DbType.String, BizActionObj.TherapyDetails.Note);
                this.dbServer.AddInParameter(storedProcCommand, "CancellationReason", DbType.String, BizActionObj.TherapyDetails.CancellationReason);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.String, BizActionObj.TherapyDetails.IsCancellation);
                this.dbServer.AddInParameter(storedProcCommand, "MainSubIndicationID", DbType.Int64, BizActionObj.TherapyDetails.MainSubInductionID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, BizActionObj.TherapyDetails.AnesthesistId);
                this.dbServer.AddInParameter(storedProcCommand, "LutealStartDate", DbType.DateTime, BizActionObj.TherapyDetails.LutealStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "LutealEndDate", DbType.DateTime, BizActionObj.TherapyDetails.LutealEndDate);
                this.dbServer.AddInParameter(storedProcCommand, "EMRProcedureID", DbType.Int64, BizActionObj.TherapyDetails.EMRProcedureID);
                this.dbServer.AddInParameter(storedProcCommand, "EMRProcedureUnitID", DbType.Int64, BizActionObj.TherapyDetails.EMRProcedureUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsSurrogacy", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycle", DbType.Boolean, BizActionObj.TherapyDetails.IsDonorCycle);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.TherapyDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "TherapyID");
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteIvfDashboardDiagnosisDeatails");
                this.dbServer.AddInParameter(command2, "PlanTherapy", DbType.Int64, BizActionObj.TherapyDetails.ID);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                if (BizActionObj.DiagnosisDetails != null)
                {
                    foreach (clsEMRAddDiagnosisVO svo in BizActionObj.DiagnosisDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateIvfDashboardDiagnosisDeatails");
                        this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.TherapyDetails.PatientId);
                        this.dbServer.AddInParameter(command3, "PlanTherapy", DbType.Int64, BizActionObj.TherapyDetails.ID);
                        this.dbServer.AddInParameter(command3, "Categori", DbType.String, svo.Code);
                        this.dbServer.AddInParameter(command3, "DiagnosisName", DbType.String, svo.Diagnosis);
                        this.dbServer.AddInParameter(command3, "DiagnosisTypeID", DbType.Int64, svo.SelectedDiagnosisType.ID);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyDetails = null;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateIVFDashBoardPlanTherapy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddPlanTherapyBizActionVO bizActionObj = valueObject as clsIVFDashboard_AddPlanTherapyBizActionVO;
            bizActionObj = this.AddUpdateIVFDashBoardPlanTherapy(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddUpdateIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO nvo = valueObject as clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO;
            try
            {
                this.dbServer.CreateConnection().Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePlanTherapyAddtionmeasure");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AssistedHatching", DbType.Int64, nvo.TherapyDetails.AssistedHatching);
                this.dbServer.AddInParameter(storedProcCommand, "IMSI", DbType.Int64, nvo.TherapyDetails.IMSI);
                this.dbServer.AddInParameter(storedProcCommand, "CryoPreservation", DbType.Int64, nvo.TherapyDetails.CryoPreservation);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject AddUpdateIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO nvo = valueObject as clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteIVFDashboardCPOEServices");
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, nvo.TherapyID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                List<clsDoctorSuggestedServiceDetailVO> investigationList = nvo.InvestigationList;
                for (int i = 0; i < investigationList.Count; i++)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddIVFDashboardCPOEServices");
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ServiceID", DbType.Int64, investigationList[i].ServiceID);
                    this.dbServer.AddInParameter(command2, "ServiceName", DbType.String, investigationList[i].ServiceName);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, investigationList[i].ServiceRate);
                    this.dbServer.AddInParameter(command2, "ServiceCode", DbType.String, investigationList[i].ServiceCode);
                    this.dbServer.AddInParameter(command2, "SpecializationID", DbType.String, investigationList[i].SpecializationId);
                    this.dbServer.AddInParameter(command2, "PriorityID", DbType.Int64, investigationList[i].SelectedPriority.ID);
                    this.dbServer.AddInParameter(command2, "TherapyID", DbType.Int64, nvo.TherapyID);
                    this.dbServer.ExecuteNonQuery(command2);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateLutealPhaseBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateLutealPhaseBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateLutealPhase");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.LutualPhaseDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.LutualPhaseDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.LutualPhaseDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.LutualPhaseDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LutealSupport", DbType.String, nvo.LutualPhaseDetails.LutealSupport);
                this.dbServer.AddInParameter(storedProcCommand, "LutealRemark", DbType.String, nvo.LutualPhaseDetails.LutealRemark);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.LutualPhaseDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.LutualPhaseDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.LutualPhaseDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateOutcomeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateOutcomeBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateOutcomeBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateOutcomeDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.OutcomeDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.OutcomeDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1Date", DbType.DateTime, nvo.OutcomeDetails.BHCGAss1Date);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1IsBSCG", DbType.Boolean, nvo.OutcomeDetails.BHCGAss1IsBSCG);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1BSCGValue", DbType.String, nvo.OutcomeDetails.BHCGAss1BSCGValue);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1SrProgest", DbType.String, nvo.OutcomeDetails.BHCGAss1SrProgest);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2Date", DbType.DateTime, nvo.OutcomeDetails.BHCGAss2Date);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2IsBSCG", DbType.Boolean, nvo.OutcomeDetails.BHCGAss2IsBSCG);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2BSCGValue", DbType.String, nvo.OutcomeDetails.BHCGAss2BSCGValue);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2USG", DbType.String, nvo.OutcomeDetails.BHCGAss2USG);
                this.dbServer.AddInParameter(storedProcCommand, "PregnancyAchieved", DbType.Boolean, nvo.OutcomeDetails.IsPregnancyAchieved);
                this.dbServer.AddInParameter(storedProcCommand, "PregnanacyConfirmDate", DbType.DateTime, nvo.OutcomeDetails.PregnanacyConfirmDate);
                this.dbServer.AddInParameter(storedProcCommand, "IsClosed", DbType.Boolean, nvo.OutcomeDetails.IsClosed);
                this.dbServer.AddInParameter(storedProcCommand, "OutComeRemarks", DbType.String, nvo.OutcomeDetails.OutComeRemarks);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSRemark", DbType.String, nvo.OutcomeDetails.OHSSRemark);
                this.dbServer.AddInParameter(storedProcCommand, "BiochemPregnancy", DbType.Boolean, nvo.OutcomeDetails.BiochemPregnancy);
                this.dbServer.AddInParameter(storedProcCommand, "Ectopic", DbType.Boolean, nvo.OutcomeDetails.Ectopic);
                this.dbServer.AddInParameter(storedProcCommand, "Abortion", DbType.Boolean, nvo.OutcomeDetails.Abortion);
                this.dbServer.AddInParameter(storedProcCommand, "Missed", DbType.Boolean, nvo.OutcomeDetails.Missed);
                this.dbServer.AddInParameter(storedProcCommand, "FetalHeartSound", DbType.Boolean, nvo.OutcomeDetails.FetalHeartSound);
                this.dbServer.AddInParameter(storedProcCommand, "FetalDate", DbType.DateTime, nvo.OutcomeDetails.FetalDate);
                this.dbServer.AddInParameter(storedProcCommand, "Count", DbType.String, nvo.OutcomeDetails.Count);
                this.dbServer.AddInParameter(storedProcCommand, "Incomplete", DbType.Boolean, nvo.OutcomeDetails.Incomplete);
                this.dbServer.AddInParameter(storedProcCommand, "IUD", DbType.Boolean, nvo.OutcomeDetails.IUD);
                this.dbServer.AddInParameter(storedProcCommand, "PretermDelivery", DbType.Boolean, nvo.OutcomeDetails.PretermDelivery);
                this.dbServer.AddInParameter(storedProcCommand, "LiveBirth", DbType.Boolean, nvo.OutcomeDetails.LiveBirth);
                this.dbServer.AddInParameter(storedProcCommand, "Congenitalabnormality", DbType.Boolean, nvo.OutcomeDetails.Congenitalabnormality);
                this.dbServer.AddInParameter(storedProcCommand, "IsChemicalPregnancy", DbType.Boolean, nvo.OutcomeDetails.IsChemicalPregnancy);
                this.dbServer.AddInParameter(storedProcCommand, "IsFullTermDelivery", DbType.Boolean, nvo.OutcomeDetails.IsFullTermDelivery);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSEarly", DbType.Boolean, nvo.OutcomeDetails.OHSSEarly);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSLate", DbType.Boolean, nvo.OutcomeDetails.OHSSLate);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSMild", DbType.Boolean, nvo.OutcomeDetails.OHSSMild);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSMode", DbType.Boolean, nvo.OutcomeDetails.OHSSMode);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSSereve", DbType.Boolean, nvo.OutcomeDetails.OHSSSereve);
                this.dbServer.AddInParameter(storedProcCommand, "BabyTypeID", DbType.Int64, nvo.OutcomeDetails.BabyTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfSacs", DbType.Int64, nvo.OutcomeDetails.NoOfSacs);
                this.dbServer.AddInParameter(storedProcCommand, "SacsObservationDate", DbType.DateTime, nvo.OutcomeDetails.SacsObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "PregnancyAchievedID", DbType.Int64, nvo.OutcomeDetails.PregnancyAchievedID);
                this.dbServer.AddInParameter(storedProcCommand, "SacRemark", DbType.String, nvo.OutcomeDetails.SacRemarks);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycle", DbType.Boolean, nvo.OutcomeDetails.IsDonorCycle);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreeze", DbType.Boolean, nvo.OutcomeDetails.IsFreeze);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OutcomeDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.OutcomeDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (!nvo.OutcomeDetails.IsSurrogate)
                {
                    long num4 = 0L;
                    if (nvo.PregnancySacsList.Count > 0)
                    {
                        long parameterValue = 0L;
                        DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySac");
                        this.dbServer.AddInParameter(command8, "OutcomeID", DbType.Int64, nvo.OutcomeDetails.ID);
                        this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command8, "PatientID", DbType.Int64, nvo.OutcomeDetails.PatientID);
                        this.dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, nvo.OutcomeDetails.PatientUnitID);
                        this.dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyID);
                        this.dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command8, "NoOfSacs", DbType.Int64, nvo.OutcomeDetails.NoOfSacs);
                        this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (nvo.OutcomePregnancySacList.Count > 0)
                        {
                            this.dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.OutcomePregnancySacList[0].ID);
                        }
                        else
                        {
                            this.dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, parameterValue);
                        }
                        this.dbServer.AddInParameter(command8, "SacsObservationDate", DbType.DateTime, nvo.OutcomeDetails.SacsObservationDate);
                        this.dbServer.AddInParameter(command8, "PregnancyAchievedID", DbType.Int64, nvo.OutcomeDetails.PregnancyAchievedID);
                        this.dbServer.AddInParameter(command8, "SacRemark", DbType.String, nvo.OutcomeDetails.SacRemarks);
                        this.dbServer.AddInParameter(command8, "SurrogateTypeID", DbType.Int64, nvo.OutcomeDetails.SurrogateTypeID);
                        this.dbServer.AddInParameter(command8, "SurrogateID", DbType.Int64, nvo.OutcomeDetails.SurrogateID);
                        this.dbServer.AddInParameter(command8, "SurrogateUnitID", DbType.Int64, nvo.OutcomeDetails.SurrogateUnitID);
                        this.dbServer.AddInParameter(command8, "SurrogatePatientMrNo", DbType.String, nvo.OutcomeDetails.SurrogatePatientMrNo);
                        this.dbServer.ExecuteNonQuery(command8);
                        parameterValue = (long) this.dbServer.GetParameterValue(command8, "ID");
                        foreach (clsPregnancySacsDetailsVO svo2 in nvo.PregnancySacsList)
                        {
                            DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySacDetails");
                            this.dbServer.AddInParameter(command9, "OutcomeID", DbType.Int64, nvo.OutcomeDetails.ID);
                            this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command9, "PregnancySacID", DbType.Int64, parameterValue);
                            this.dbServer.AddInParameter(command9, "SacNoStr", DbType.String, svo2.SaceNoStr);
                            this.dbServer.AddInParameter(command9, "IsFetalheart", DbType.Boolean, svo2.IsFetalHeart);
                            if (svo2.IsFetalHeart)
                            {
                                num4 += 1L;
                            }
                            this.dbServer.AddInParameter(command9, "ResultID", DbType.Int64, svo2.ResultListID);
                            this.dbServer.AddInParameter(command9, "PregnancyID", DbType.Int64, svo2.PregnancyListID);
                            this.dbServer.AddInParameter(command9, "CongenitalAbnormalityYes", DbType.Boolean, svo2.CongenitalAbnormalityYes);
                            this.dbServer.AddInParameter(command9, "CongenitalAbnormalityNo", DbType.Boolean, svo2.CongenitalAbnormalityNo);
                            this.dbServer.AddInParameter(command9, "CongenitalAbnormalityReason", DbType.String, svo2.CongenitalAbnormalityReason);
                            this.dbServer.AddInParameter(command9, "FetalHeartCount", DbType.Int64, num4);
                            this.dbServer.ExecuteNonQuery(command9);
                        }
                    }
                    if (nvo.OutcomeDetails.IsClosed && (num4 > 0L))
                    {
                        long parameterValue = 0L;
                        DbCommand command10 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetails");
                        this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command10, "PatientID", DbType.Int64, nvo.OutcomeDetails.PatientID);
                        this.dbServer.AddInParameter(command10, "PatientUnitID", DbType.Int64, nvo.OutcomeDetails.PatientUnitID);
                        this.dbServer.AddInParameter(command10, "PlanTherapyID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyID);
                        this.dbServer.AddInParameter(command10, "PlanTherapyUnitID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command10, "FetalHeartNo", DbType.Int64, num4);
                        this.dbServer.AddInParameter(command10, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command10, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command10, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command10, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command10, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command10, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, parameterValue);
                        this.dbServer.ExecuteNonQuery(command10);
                        parameterValue = (long) this.dbServer.GetParameterValue(command10, "ID");
                        for (int i = 0; i < num4; i++)
                        {
                            DbCommand command11 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetailsList");
                            this.dbServer.AddInParameter(command11, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command11, "BirthDetailsID", DbType.Int64, parameterValue);
                            this.dbServer.AddInParameter(command11, "ChildNoStr", DbType.String, "Child" + (i + 1));
                            this.dbServer.AddInParameter(command11, "BirthWeight", DbType.String, "");
                            this.dbServer.AddInParameter(command11, "MedicalConditions", DbType.String, "");
                            this.dbServer.AddInParameter(command11, "WeeksofGestation", DbType.String, "");
                            this.dbServer.AddInParameter(command11, "DeliveryTypeID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "ActivityID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "ActivityPoint", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "Pulse", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "PulsePoint", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "Grimace", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "GrimacePoint", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "Appearance", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "AppearancePoint", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "Respiration", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "RespirationPoint", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "APGARScore", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command11, "Conclusion", DbType.Int64, null);
                            this.dbServer.AddInParameter(command11, "APGARScoreID", DbType.Int64, 0);
                            this.dbServer.AddOutParameter(command11, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command11);
                        }
                    }
                }
                else
                {
                    if ((nvo.OutcomeDetailsList != null) && (nvo.OutcomeDetailsList.Count > 0))
                    {
                        DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashBoard_OutcomeBHCGResultDetails set Status=", 0, " where  OutcomeID=", nvo.OutcomeDetails.ID }));
                        this.dbServer.ExecuteNonQuery(sqlStringCommand);
                        foreach (clsIVFDashboard_OutcomeVO evo in nvo.OutcomeDetailsList)
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateOutcomeBHCGResultDetails");
                            this.dbServer.AddInParameter(command3, "ID", DbType.Int64, evo.ID);
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "OutcomeID", DbType.Int64, nvo.OutcomeDetails.ID);
                            this.dbServer.AddInParameter(command3, "BHCGResultDate", DbType.DateTime, evo.BHCGAss1Date);
                            this.dbServer.AddInParameter(command3, "ResultID", DbType.Int64, evo.ResultListID);
                            this.dbServer.AddInParameter(command3, "BHCGValue", DbType.String, evo.BHCGAss1BSCGValue);
                            this.dbServer.AddInParameter(command3, "SurrogateID", DbType.Int64, evo.SurrogateID);
                            this.dbServer.AddInParameter(command3, "SurrogateUnitID", DbType.Int64, evo.SurrogateUnitID);
                            this.dbServer.AddInParameter(command3, "MrNo", DbType.String, evo.SurrogatePatientMrNo);
                            this.dbServer.ExecuteNonQuery(command3);
                        }
                    }
                    long num = 0L;
                    if ((nvo.OutcomePregnancySacList != null) && (nvo.OutcomePregnancySacList.Count > 0))
                    {
                        foreach (clsIVFDashboard_OutcomeVO evo2 in nvo.OutcomePregnancySacList)
                        {
                            num = 0L;
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySac");
                            this.dbServer.AddInParameter(command4, "OutcomeID", DbType.Int64, nvo.OutcomeDetails.ID);
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, nvo.OutcomeDetails.PatientID);
                            this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, nvo.OutcomeDetails.PatientUnitID);
                            this.dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyID);
                            this.dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command4, "NoOfSacs", DbType.Int64, evo2.NoOfSacs);
                            this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                            this.dbServer.AddInParameter(command4, "SacsObservationDate", DbType.DateTime, evo2.SacsObservationDate);
                            this.dbServer.AddInParameter(command4, "PregnancyAchievedID", DbType.Int64, evo2.PregnancyAchievedID);
                            this.dbServer.AddInParameter(command4, "SacRemark", DbType.String, evo2.SacRemarks);
                            this.dbServer.AddInParameter(command4, "SurrogateTypeID", DbType.Int64, evo2.SurrogateTypeID);
                            this.dbServer.AddInParameter(command4, "SurrogateID", DbType.Int64, evo2.SurrogateID);
                            this.dbServer.AddInParameter(command4, "SurrogateUnitID", DbType.Int64, evo2.SurrogateUnitID);
                            this.dbServer.AddInParameter(command4, "SurrogatePatientMrNo", DbType.String, evo2.SurrogatePatientMrNo);
                            this.dbServer.AddInParameter(command4, "IsUnlinkSurrogate", DbType.Boolean, evo2.IsUnlinkSurrogate);
                            this.dbServer.AddInParameter(command4, "IsFreeze", DbType.Boolean, nvo.OutcomeDetails.IsFreeze);
                            this.dbServer.AddInParameter(command4, "FreeSurrogate", DbType.Boolean, nvo.OutcomeDetails.FreeSurrogate);
                            this.dbServer.ExecuteNonQuery(command4);
                            evo2.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            foreach (clsPregnancySacsDetailsVO svo in evo2.PregnancySacsList)
                            {
                                DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySacDetails");
                                this.dbServer.AddInParameter(command5, "OutcomeID", DbType.Int64, nvo.OutcomeDetails.ID);
                                this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command5, "PregnancySacID", DbType.Int64, evo2.ID);
                                this.dbServer.AddInParameter(command5, "SacNoStr", DbType.String, svo.SaceNoStr);
                                this.dbServer.AddInParameter(command5, "IsFetalheart", DbType.Boolean, svo.IsFetalHeart);
                                if (svo.IsFetalHeart)
                                {
                                    num += 1L;
                                }
                                this.dbServer.AddInParameter(command5, "ResultID", DbType.Int64, svo.ResultListID);
                                this.dbServer.AddInParameter(command5, "PregnancyID", DbType.Int64, svo.PregnancyListID);
                                this.dbServer.AddInParameter(command5, "CongenitalAbnormalityYes", DbType.Boolean, svo.CongenitalAbnormalityYes);
                                this.dbServer.AddInParameter(command5, "CongenitalAbnormalityNo", DbType.Boolean, svo.CongenitalAbnormalityNo);
                                this.dbServer.AddInParameter(command5, "CongenitalAbnormalityReason", DbType.String, svo.CongenitalAbnormalityReason);
                                this.dbServer.AddInParameter(command5, "FetalHeartCount", DbType.Int64, num);
                                this.dbServer.AddInParameter(command5, "IsUnlinkSurrogate", DbType.Boolean, evo2.IsUnlinkSurrogate);
                                this.dbServer.AddInParameter(command5, "IsFreeze", DbType.Boolean, nvo.OutcomeDetails.IsFreeze);
                                this.dbServer.AddInParameter(command5, "SurrogateID", DbType.Int64, evo2.SurrogateID);
                                this.dbServer.AddInParameter(command5, "SurrogateUnitID", DbType.Int64, evo2.SurrogateUnitID);
                                this.dbServer.ExecuteNonQuery(command5);
                            }
                            if (nvo.OutcomeDetails.IsClosed && (num > 0L))
                            {
                                long parameterValue = 0L;
                                DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetails");
                                this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "PatientID", DbType.Int64, nvo.OutcomeDetails.PatientID);
                                this.dbServer.AddInParameter(command6, "PatientUnitID", DbType.Int64, nvo.OutcomeDetails.PatientUnitID);
                                this.dbServer.AddInParameter(command6, "PlanTherapyID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyID);
                                this.dbServer.AddInParameter(command6, "PlanTherapyUnitID", DbType.Int64, nvo.OutcomeDetails.PlanTherapyUnitID);
                                this.dbServer.AddInParameter(command6, "FetalHeartNo", DbType.Int64, num);
                                this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, parameterValue);
                                this.dbServer.AddInParameter(command6, "SurrogateID", DbType.Int64, evo2.SurrogateID);
                                this.dbServer.AddInParameter(command6, "SurrogateUnitID", DbType.Int64, evo2.SurrogateUnitID);
                                this.dbServer.AddInParameter(command6, "SurrogatePatientMrNo", DbType.String, evo2.SurrogatePatientMrNo);
                                this.dbServer.ExecuteNonQuery(command6);
                                parameterValue = (long) this.dbServer.GetParameterValue(command6, "ID");
                                for (int i = 0; i < num; i++)
                                {
                                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetailsList");
                                    this.dbServer.AddInParameter(command7, "ID", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command7, "BirthDetailsID", DbType.Int64, parameterValue);
                                    this.dbServer.AddInParameter(command7, "ChildNoStr", DbType.String, "Child" + (i + 1));
                                    this.dbServer.AddInParameter(command7, "BirthWeight", DbType.String, "");
                                    this.dbServer.AddInParameter(command7, "MedicalConditions", DbType.String, "");
                                    this.dbServer.AddInParameter(command7, "WeeksofGestation", DbType.String, "");
                                    this.dbServer.AddInParameter(command7, "DeliveryTypeID", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "ActivityID", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "ActivityPoint", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "Pulse", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "PulsePoint", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "Grimace", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "GrimacePoint", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "Appearance", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "AppearancePoint", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "Respiration", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "RespirationPoint", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "APGARScore", DbType.Int64, 0);
                                    this.dbServer.AddInParameter(command7, "Conclusion", DbType.Int64, null);
                                    this.dbServer.AddInParameter(command7, "APGARScoreID", DbType.Int64, 0);
                                    this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(command7);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.OutcomeDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject DeleteBirthDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_DeleteBirthDetailsBizActionVO nvo = valueObject as clsIVFDashboard_DeleteBirthDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_IVF_BirthDetails_New where ID =", nvo.BirthDetails.ID, " and UnitID=", UserVo.UserLoginInfo.UnitId }));
                this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.BirthDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_DeleteTherapyDocumentBizActionVO nvo = valueObject as clsIVFDashboard_DeleteTherapyDocumentBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_UpdateLabDaysDocListStatus");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.Int64, nvo.Details.Day);
                this.dbServer.AddInParameter(storedProcCommand, "DocNo", DbType.String, nvo.Details.DocNo);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.Details.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.DetailsList = new List<clsIVFDashboard_TherapyDocumentVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO item = new clsIVFDashboard_TherapyDocumentVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"])),
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocyteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.DetailsList.Add(item);
                    }
                }
                reader.Close();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public clsFollicularMonitoring FollicularMonitoring(clsIVFDashboard_GetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            return new clsFollicularMonitoring { 
                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                TherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyId"])),
                TherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitId"])),
                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitId"])),
                Date = DALHelper.HandleDate(reader["Date"]),
                Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"])),
                PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianID"])),
                FollicularNotes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"])),
                AttachmentPath = Convert.ToString(DALHelper.HandleDBNull(reader["AttachmentPath"])),
                AttachmentFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachmentFileContent"]),
                EndometriumThickness = Convert.ToString(DALHelper.HandleDBNull(reader["EndometriumThickness"])),
                UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]))
            };
        }

        public override IValueObject GetBirthDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetBirthDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetBirthDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetBirthDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.BirthDetails.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.BirthDetails.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.BirthDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.BirthDetails.PatientUnitID);
                nvo.BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_BirthDetailsVO item = new clsIVFDashboard_BirthDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                            TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"])),
                            DateOfBirth = DALHelper.HandleDate(reader["DateOfBirth"]),
                            Week = Convert.ToInt64(DALHelper.HandleDBNull(reader["Week"])),
                            GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"])),
                            Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"])),
                            ConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionID"])),
                            Condition = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"])),
                            Country = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Country"]))),
                            TownOfBirth = Convert.ToString(DALHelper.HandleDBNull(reader["TownOfBirth"])),
                            DeathPostportumID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeathPostportumID"])),
                            DeathPostportum = Convert.ToString(DALHelper.HandleDBNull(reader["DeathPostportum"])),
                            DeliveryMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeliveryMethodID"])),
                            DeliveryMethod = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryMethod"])),
                            DiedPerinatallyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiedPerinatallyID"])),
                            DiedPerinatally = Convert.ToString(DALHelper.HandleDBNull(reader["DiedPerinatally"])),
                            Child = Convert.ToString(DALHelper.HandleDBNull(reader["Child"])),
                            ChildID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChildID"])),
                            WeightAtBirth = Convert.ToSingle(DALHelper.HandleDBNull(reader["WeightAtBirth"])),
                            LengthAtBirth = Convert.ToSingle(DALHelper.HandleDBNull(reader["LengthAtBirth"])),
                            FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]))),
                            Surname = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Surname"]))),
                            IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]))
                        };
                        nvo.BirthDetailsList.Add(item);
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

        public override IValueObject GetBirthDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetBirthDetailsListBizActionVO nvo = valueObject as clsIVFDashboard_GetBirthDetailsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetBirthDetailsList");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.BirthDetails.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.BirthDetails.TherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.BirthDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.BirthDetails.PatientUnitID);
                nvo.BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_BirthDetailsVO item = new clsIVFDashboard_BirthDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            FetalHeartNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartNo"])),
                            BirthDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BirthDetailsID"])),
                            ChildNoStr = Convert.ToString(DALHelper.HandleDBNull(reader["ChildNoStr"])),
                            BirthWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BirthWeight"])),
                            MedicalConditions = Convert.ToString(DALHelper.HandleDBNull(reader["MedicalConditions"])),
                            WeeksofGestation = Convert.ToString(DALHelper.HandleDBNull(reader["WeeksofGestation"])),
                            DeliveryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeliveryTypeID"])),
                            ActivityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ActivityID"])),
                            ActivityPoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["ActivityPoint"])),
                            Pulse = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pulse"])),
                            PulsePoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["PulsePoint"])),
                            Grimace = Convert.ToInt64(DALHelper.HandleDBNull(reader["Grimace"])),
                            GrimacePoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrimacePoint"])),
                            Appearance = Convert.ToInt64(DALHelper.HandleDBNull(reader["Appearance"])),
                            AppearancePoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["AppearancePoint"])),
                            Respiration = Convert.ToInt64(DALHelper.HandleDBNull(reader["Respiration"])),
                            RespirationPoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["RespirationPoint"])),
                            APGARScore = Convert.ToInt64(DALHelper.HandleDBNull(reader["APGARScore"])),
                            APGARScoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["APGARScoreID"])),
                            Conclusion = Convert.ToString(DALHelper.HandleDBNull(reader["Conclusion"])),
                            SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"])),
                            SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"])),
                            SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"]))
                        };
                        nvo.BirthDetailsList.Add(item);
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

        public override IValueObject GetBirthDetailsMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO nvo = (clsIVFDashboard_GetBirthDetailsMasterListBizActionVO) valueObject;
            try
            {
                StringBuilder builder = new StringBuilder();
                if (nvo.IsActive != null)
                {
                    builder.Append("Status = '" + nvo.IsActive.Value + "'");
                }
                if (nvo.Parent != null)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" And ");
                    }
                    builder.Append(nvo.Parent.Value.ToString() + "='" + nvo.Parent.Key.ToString() + "'");
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFBirthMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "MasterTableName", DbType.String, nvo.MasterTable.ToString());
                this.dbServer.AddInParameter(storedProcCommand, "FilterExpression", DbType.String, builder.ToString());
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (nvo.MasterTable.ToString() == "M_IVf_BirthAPGARScore")
                        {
                            nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["Code"].ToString(), reader["Description"].ToString(), (bool) reader["Status"], (long) reader["MinPoint"], (long) reader["MaxPoint"]));
                            continue;
                        }
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["Code"].ToString(), reader["Description"].ToString(), (long) reader["Point"], (bool) reader["Status"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                nvo.Error = exception1.Message;
            }
            return nvo;
        }

        public override IValueObject GetCycleCodeForCombobox(IValueObject valueObject, clsUserVO UserVo)
        {
            GetCycleCodeForPatientBizActionVO nvo = (GetCycleCodeForPatientBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCycleCodeForCombobox");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.String, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.String, nvo.CoupleUnitID);
                nvo.List = new List<string>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.List.Add(reader["CycleCode"].ToString());
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

        public override IValueObject GetDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetTherapyDocumentBizActionVO nvo = valueObject as clsIVFDashboard_GetTherapyDocumentBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_GetTherapyDocument");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailList == null)
                    {
                        nvo.DetailList = new List<clsIVFDashboard_TherapyDocumentVO>();
                    }
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO item = new clsIVFDashboard_TherapyDocumentVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"]
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        nvo.DetailList.Add(item);
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

        public override IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetFollicularMonitoringSizeList");
                this.dbServer.AddInParameter(storedProcCommand, "FollicularId", DbType.String, nvo.FollicularID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                nvo.FollicularMonitoringSizeList = new List<clsFollicularMonitoringSizeDetails>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsFollicularMonitoringSizeDetails item = new clsFollicularMonitoringSizeDetails {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            FollicularMonitoringId = Convert.ToInt64(DALHelper.HandleDBNull(reader["FollicularMonitoringId"])),
                            FollicularNumber = Convert.ToString(DALHelper.HandleDBNull(reader["FollicularNumber"])),
                            LeftSize = Convert.ToString(DALHelper.HandleDBNull(reader["LeftSize"])),
                            RightSIze = Convert.ToString(DALHelper.HandleDBNull(reader["RightSIze"]))
                        };
                        nvo.FollicularMonitoringSizeList.Add(item);
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

        public override IValueObject GetFolliculeLRSum(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetFolliculeLRSumBizActionVO nvo = valueObject as clsIVFDashboard_GetFolliculeLRSumBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetLRFolliculeSum");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.String, nvo.FollicularMonitoringDetial.TherapyId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.FollicularMonitoringDetial.TherapyUnitId);
                nvo.FollicularMonitoringSizeDetials = new clsFollicularMonitoringSizeDetails();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.FollicularMonitoringSizeDetials.LeftSum = Convert.ToInt64(DALHelper.HandleDBNull(reader["LeftSize"]));
                        nvo.FollicularMonitoringSizeDetials.RightSum = Convert.ToInt64(DALHelper.HandleDBNull(reader["RightSize"]));
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

        public override IValueObject GetHalfBilled(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetManagementVisibleBizActionVO nvo = valueObject as clsIVFDashboard_GetManagementVisibleBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDetailsIVFServiceHalfBilled");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.String, nvo.TherapyDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TherapyDetails.PatientUintId);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycle", DbType.Boolean, nvo.TherapyDetails.IsDonorCycle);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, nvo.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyUnitID", DbType.Int64, nvo.TherapyDetails.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.TherapyDetails.BillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BillAmount"]));
                        nvo.TherapyDetails.BillBalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BillBalanceAmount"]));
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

        public override IValueObject GetIVFDashboardcurrentDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO nvo = valueObject as clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardCurrentDiagnosis");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
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
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"])),
                            SelectedDiagnosisType = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"])),
                                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                            }
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

        public override IValueObject GetIVFDashboardInvestigation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardPatientInvestigationDataBizActionVO nvo = valueObject as clsGetIVFDashboardPatientInvestigationDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardPreviousInvestigation");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientInvestigationList == null)
                    {
                        nvo.PatientInvestigationList = new List<clsServiceMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"])),
                            DoctorID = Convert.ToInt32(reader["Doctorid"]),
                            ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            Group = Convert.ToString(reader["GroupName"]),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            VisitDate = Convert.ToDateTime(reader["VisitDate"]),
                            SelectedPriority = new MasterListItem()
                        };
                        item.SelectedPriority.Description = Convert.ToString(DALHelper.HandleDBNull(reader["PriorityDescription"]));
                        item.Specialization = Convert.ToInt64(reader["SpecializationId"]);
                        nvo.PatientInvestigationList.Add(item);
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

        public override IValueObject GetIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO nvo = valueObject as clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardAddtionmeasure");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.TherapyID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TherapyDetails == null)
                    {
                        nvo.TherapyDetails = new clsPlanTherapyVO();
                    }
                    while (reader.Read())
                    {
                        nvo.TherapyDetails.AssistedHatching = Convert.ToBoolean(reader["AssistedHatching"]);
                        nvo.TherapyDetails.IMSI = Convert.ToBoolean(reader["IMSI"]);
                        nvo.TherapyDetails.CryoPreservation = Convert.ToBoolean(reader["CryoPreservation"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO nvo = valueObject as clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardCPOEServiceDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, nvo.TherapyID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO item = new clsDoctorSuggestedServiceDetailVO {
                            ServiceID = Convert.ToInt64(reader["ServiceID"]),
                            ServiceName = Convert.ToString(reader["ServiceName"]),
                            ServiceRate = Convert.ToDouble(reader["serviceRate"]),
                            ServiceCode = Convert.ToString(reader["ServiceCode"]),
                            PriorityIndex = Convert.ToInt32(reader["PriorityID"]),
                            SpecializationId = Convert.ToInt64(reader["SpecializationId"])
                        };
                        nvo.ServiceDetails.Add(item);
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

        public override IValueObject GetIVFDashboardPrescriptionDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardPatientPrescriptionDataBizActionVO nvo = valueObject as clsGetIVFDashboardPatientPrescriptionDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardPreviousPrescription");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientPrescriptionList == null)
                    {
                        nvo.PatientPrescriptionList = new List<clsPatientPrescriptionDetailVO>();
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
                        DateTime? nullable = DALHelper.HandleDate(reader["VisitDate"]);
                        item.VisitDate = nullable.Value;
                        nvo.PatientPrescriptionList.Add(item);
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

        public override IValueObject GetIVFDashboardPreviousDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardPatientDiagnosisDataBizActionVO nvo = valueObject as clsGetIVFDashboardPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardPreviousDiagnosis");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
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

        public override IValueObject GetLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetLutealPhaseBizActionVO nvo = valueObject as clsIVFDashboard_GetLutealPhaseBizActionVO;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetLutealPhaseDetails");
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
                        nvo.Details.LutealSupport = Convert.ToString(DALHelper.HandleDBNull(reader["LutealSupport"]));
                        nvo.Details.LutealRemark = Convert.ToString(DALHelper.HandleDBNull(reader["LutealRemark"]));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetManagementVisible(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsIVFDashboard_GetManagementVisibleBizActionVO nvo = valueObject as clsIVFDashboard_GetManagementVisibleBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDetailsIVFManagementServiceBilled");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.String, nvo.TherapyDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TherapyDetails.PatientUintId);
                this.dbServer.AddInParameter(storedProcCommand, "IsIVFBillingCriteria", DbType.Int64, nvo.TherapyDetails.IsIVFBillingCriteria);
                this.dbServer.AddParameter(storedProcCommand, "IsDonorCycle", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TherapyDetails.IsDonorCycle);
                this.dbServer.AddParameter(storedProcCommand, "PlanTreatmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TherapyDetails.PlannedTreatmentID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TherapyDetails.ID);
                this.dbServer.AddParameter(storedProcCommand, "UnitID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TherapyDetails.UnitID);
                this.dbServer.AddParameter(storedProcCommand, "IsSurrogate", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.TherapyDetails.IsSurrogate);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.TherapyDetails.PlannedTreatmentID = (long) this.dbServer.GetParameterValue(storedProcCommand, "PlanTreatmentID");
                nvo.TherapyDetails.IsDonorCycle = Convert.ToBoolean(this.dbServer.GetParameterValue(storedProcCommand, "IsDonorCycle"));
                nvo.TherapyDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.TherapyDetails.UnitID = (long) this.dbServer.GetParameterValue(storedProcCommand, "UnitID");
                nvo.TherapyDetails.IsSurrogate = Convert.ToBoolean(this.dbServer.GetParameterValue(storedProcCommand, "IsSurrogate"));
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                throw;
            }
            return nvo;
        }

        public override IValueObject GetOutcomeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetOutcomeBizActionVO nvo = valueObject as clsIVFDashboard_GetOutcomeBizActionVO;
            nvo.PregnancySacsList = new List<clsPregnancySacsDetailsVO>();
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetOutcomeDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
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
                                            while (reader.Read())
                                            {
                                                clsPregnancySacsDetailsVO svo = new clsPregnancySacsDetailsVO {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                                    PregnancySacID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancySacID"])),
                                                    SaceNoStr = Convert.ToString(DALHelper.HandleDBNull(reader["SacNoStr"])),
                                                    IsFetalHeart = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFetalHeart"])),
                                                    ResultListID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultID"])),
                                                    PregnancyListID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyID"])),
                                                    CongenitalAbnormalityYes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CongenitalAbnormalityYes"])),
                                                    CongenitalAbnormalityNo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CongenitalAbnormalityNo"])),
                                                    CongenitalAbnormalityReason = Convert.ToString(DALHelper.HandleDBNull(reader["CongenitalAbnormalityReason"])),
                                                    OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"])),
                                                    SurrogateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateTypeID"]))
                                                };
                                                nvo.PregnancySacsList.Add(svo);
                                            }
                                            break;
                                        }
                                        clsIVFDashboard_OutcomeVO evo2 = new clsIVFDashboard_OutcomeVO {
                                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                            OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"])),
                                            NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"])),
                                            SacsObservationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["SacsObservationDate"]))),
                                            PregnancyAchievedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyAchievedID"])),
                                            SacRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["SacRemark"])),
                                            SurrogateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateTypeID"])),
                                            SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"])),
                                            SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"])),
                                            SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"])),
                                            FetalHeartCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartCount"])),
                                            IsUnlinkSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUnlinkSurrogate"]))
                                        };
                                        nvo.OutcomePregnancySacList.Add(evo2);
                                    }
                                    break;
                                }
                                clsIVFDashboard_OutcomeVO item = new clsIVFDashboard_OutcomeVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                    OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"])),
                                    BHCGAss1Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["BHCGResultDate"]))),
                                    ResultListID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultID"])),
                                    BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGValue"])),
                                    SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"])),
                                    SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"])),
                                    SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"]))
                                };
                                nvo.OutcomeDetailsList.Add(item);
                            }
                            break;
                        }
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.BHCGAss1Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["BHCGAss1Date"])));
                        nvo.Details.BHCGAss1IsBSCG = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"])));
                        nvo.Details.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1BSCGValue"]));
                        nvo.Details.BHCGAss1SrProgest = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1SrProgest"]));
                        nvo.Details.BHCGAss2Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["BHCGAss2Date"])));
                        nvo.Details.BHCGAss2IsBSCG = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["BHCGAss2IsBSCG"])));
                        nvo.Details.BHCGAss2BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2BSCGValue"]));
                        nvo.Details.BHCGAss2USG = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2USG"]));
                        nvo.Details.IsPregnancyAchieved = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["PregnancyAchieved"])));
                        nvo.Details.PregnanacyConfirmDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["PregnanacyConfirmDate"])));
                        nvo.Details.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        nvo.Details.OutComeRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["OutComeRemarks"]));
                        nvo.Details.OHSSRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OHSSRemark"]));
                        nvo.Details.BiochemPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BiochemPregnancy"]));
                        nvo.Details.Ectopic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ectopic"]));
                        nvo.Details.Abortion = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Abortion"]));
                        nvo.Details.Missed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Missed"]));
                        nvo.Details.FetalHeartSound = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FetalHeartSound"]));
                        nvo.Details.FetalDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["FetalDate"])));
                        nvo.Details.Count = Convert.ToString(DALHelper.HandleDBNull(reader["Count"]));
                        nvo.Details.Incomplete = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Incomplete"]));
                        nvo.Details.IUD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IUD"]));
                        nvo.Details.PretermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PretermDelivery"]));
                        nvo.Details.LiveBirth = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LiveBirth"]));
                        nvo.Details.Congenitalabnormality = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Congenitalabnormality"]));
                        nvo.Details.IsChemicalPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChemicalPregnancy"]));
                        nvo.Details.IsFullTermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFullTermDelivery"]));
                        nvo.Details.OHSSEarly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSEarly"]));
                        nvo.Details.OHSSLate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSLate"]));
                        nvo.Details.OHSSMild = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMild"]));
                        nvo.Details.OHSSMode = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMode"]));
                        nvo.Details.OHSSSereve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSSereve"]));
                        nvo.Details.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));
                        nvo.Details.NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"]));
                        nvo.Details.SacsObservationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["SacsObservationDate"])));
                        nvo.Details.PregnancyAchievedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyAchievedID"]));
                        nvo.Details.SacRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["SacRemark"]));
                        nvo.Details.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPACVisible(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsIVFDashboard_GetPACVisibleBizActionVO nvo = valueObject as clsIVFDashboard_GetPACVisibleBizActionVO;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDetailsIVFPACServiceBilled");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.String, nvo.TherapyDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TherapyDetails.PatientUintId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, nvo.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyUnitID", DbType.Int64, nvo.TherapyDetails.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPatientListForDashboard(IValueObject valueObject, clsUserVO UserVo)
        {
            GetPatientListForDashboardBizActionVO nvo = valueObject as GetPatientListForDashboardBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientListForDashboard");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "SearchKeyword", DbType.String, nvo.SearchKeyword);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                }
                if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(nvo.FamilyName));
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    if (nvo.VisitWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                    }
                    else if (nvo.AdmissionWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, nvo.OPDNo + "%");
                    }
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                }
                if ((nvo.Country != null) && (nvo.Country.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.Country) + "%");
                }
                if ((nvo.State != null) && (nvo.State.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                }
                if ((nvo.City != null) && (nvo.City.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                }
                if ((nvo.Pincode != null) && (nvo.Pincode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(nvo.Pincode) + "%");
                }
                if ((nvo.CivilID != null) && (nvo.CivilID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(nvo.CivilID) + "%");
                }
                long genderID = nvo.GenderID;
                if (nvo.GenderID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                }
                long patientCategoryID = nvo.PatientCategoryID;
                if (nvo.PatientCategoryID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategory", DbType.Int64, nvo.PatientCategoryID);
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                if (nvo.VisitWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                    if (nvo.VisitFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                    }
                    if (nvo.VisitToDate != null)
                    {
                        if (nvo.VisitFromDate != null)
                        {
                            nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                    }
                }
                if (nvo.AdmissionWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                    if (nvo.AdmissionFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                    }
                    if (nvo.AdmissionToDate != null)
                    {
                        if (nvo.AdmissionToDate != null)
                        {
                            nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                    }
                }
                if (nvo.DOBWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                    if (nvo.DOBFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.DateTime, nvo.DOBFromDate);
                    }
                    if (nvo.DOBToDate != null)
                    {
                        if (nvo.DOBToDate != null)
                        {
                            nvo.DOBToDate = new DateTime?(nvo.DOBToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.DateTime, nvo.DOBToDate);
                    }
                }
                if (nvo.IsLoyaltyMember)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, nvo.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if (nvo.SearchInAnotherClinic)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SearchInAnotherClinic", DbType.Boolean, nvo.SearchInAnotherClinic);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorLink", DbType.Boolean, nvo.IsDonorLink);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                            LastName = (string) DALHelper.HandleDBNull(reader["LastName"]),
                            FirstName = (string) DALHelper.HandleDBNull(reader["FirstName"]),
                            MiddleName = (string) DALHelper.HandleDBNull(reader["MiddleName"])
                        };
                        string[] strArray = new string[] { item.FirstName, " ", item.MiddleName, " ", item.MiddleName };
                        item.PatientName = string.Concat(strArray);
                        item.DateOfBirth = DALHelper.HandleDate(reader["DateofBirth"]);
                        DateTime? nullable17 = DALHelper.HandleDate(reader["RegistrationDate"]);
                        item.RegistrationDate = new DateTime?(nullable17.Value);
                        if (!nvo.VisitWise && !nvo.RegistrationWise)
                        {
                            item.IPDAdmissionID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.IPDAdmissionNo = (string) DALHelper.HandleDBNull(reader["OINO"]);
                        }
                        else
                        {
                            item.VisitID = (long) DALHelper.HandleDBNull(reader["VAID"]);
                            item.OPDNO = (string) DALHelper.HandleDBNull(reader["OINO"]);
                        }
                        if (nvo.VisitWise)
                        {
                            item.PatientKind = PatientsKind.OPD;
                        }
                        else if (nvo.AdmissionWise)
                        {
                            item.PatientKind = PatientsKind.IPD;
                        }
                        else if ((item.VisitID == 0L) && (item.IPDAdmissionID == 0L))
                        {
                            item.PatientKind = PatientsKind.Registration;
                        }
                        item.TariffID = (long) DALHelper.HandleDBNull(reader["TariffID"]);
                        item.PatientSourceID = (long) DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        item.CompanyID = (long) DALHelper.HandleDBNull(reader["CompanyID"]);
                        item.ContactNO1 = (string) DALHelper.HandleDBNull(reader["ContactNO1"]);
                        item.Gender = (string) DALHelper.HandleDBNull(reader["Gender"]);
                        item.MaritalStatus = (string) DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        item.UniversalID = Security.base64Decode((string) DALHelper.HandleDBNull(reader["CivilID"]));
                        item.PatientTypeID = (long) DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        item.SearchKeyword = Convert.ToString(DALHelper.HandleDBNull(reader["SearchKeyword"]));
                        item.AgentName = (string) DALHelper.HandleDBNull(reader["AgentName"]);
                        item.LinkServer = nvo.LinkServer;
                        nvo.PatientDetailsList.Add(item);
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["SearchKeyword"].ToString()));
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
            finally
            {
                connection.Close();
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject GetSearchKeywordforPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            GetSearchkeywordForPatientBizActionVO nvo = valueObject as GetSearchkeywordForPatientBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientListForDashboard");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "SearchKeyword", DbType.String, nvo.SearchKeyword);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                if ((nvo.MiddleName != null) && (nvo.MiddleName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MiddleName", DbType.String, nvo.MiddleName);
                }
                if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                }
                if ((nvo.FamilyName != null) && ((nvo.FamilyName.Length != 0) && ((nvo.FamilyName != null) && (nvo.FamilyName.Length != 0))))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, Security.base64Encode(nvo.FamilyName));
                }
                if ((nvo.OPDNo != null) && (nvo.OPDNo.Length != 0))
                {
                    if (nvo.VisitWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "OPDNo", DbType.String, nvo.OPDNo + "%");
                    }
                    else if (nvo.AdmissionWise)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionNo", DbType.String, nvo.OPDNo + "%");
                    }
                }
                if ((nvo.ContactNo != null) && (nvo.ContactNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ContactNo", DbType.String, nvo.ContactNo + "%");
                }
                if ((nvo.Country != null) && (nvo.Country.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, Security.base64Encode(nvo.Country) + "%");
                }
                if ((nvo.State != null) && (nvo.State.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, Security.base64Encode(nvo.State) + "%");
                }
                if ((nvo.City != null) && (nvo.City.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, Security.base64Encode(nvo.City) + "%");
                }
                if ((nvo.Pincode != null) && (nvo.Pincode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, Security.base64Encode(nvo.Pincode) + "%");
                }
                if ((nvo.CivilID != null) && (nvo.CivilID.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "CivilID", DbType.String, Security.base64Encode(nvo.CivilID) + "%");
                }
                long genderID = nvo.GenderID;
                if (nvo.GenderID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                }
                long patientCategoryID = nvo.PatientCategoryID;
                if (nvo.PatientCategoryID != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "PatientCategory", DbType.Int64, nvo.PatientCategoryID);
                }
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                if (nvo.VisitWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 1);
                    if (nvo.VisitFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "VisitFromDate", DbType.DateTime, nvo.VisitFromDate);
                    }
                    if (nvo.VisitToDate != null)
                    {
                        if (nvo.VisitFromDate != null)
                        {
                            nvo.VisitToDate = new DateTime?(nvo.VisitToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "VisitToDate", DbType.DateTime, nvo.VisitToDate);
                    }
                }
                if (nvo.AdmissionWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 2);
                    if (nvo.AdmissionFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionFromDate", DbType.DateTime, nvo.AdmissionFromDate);
                    }
                    if (nvo.AdmissionToDate != null)
                    {
                        if (nvo.AdmissionToDate != null)
                        {
                            nvo.AdmissionToDate = new DateTime?(nvo.AdmissionToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "AdmissionToDate", DbType.DateTime, nvo.AdmissionToDate);
                    }
                }
                if (nvo.DOBWise)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Serchfor", DbType.Int16, (short) 0);
                    if (nvo.DOBFromDate != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "DOBFromDate", DbType.DateTime, nvo.DOBFromDate);
                    }
                    if (nvo.DOBToDate != null)
                    {
                        if (nvo.DOBToDate != null)
                        {
                            nvo.DOBToDate = new DateTime?(nvo.DOBToDate.Value.Date.AddDays(1.0));
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "DOBToDate", DbType.DateTime, nvo.DOBToDate);
                    }
                }
                if (nvo.IsLoyaltyMember)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsLoyaltyMember", DbType.Boolean, nvo.IsLoyaltyMember);
                    this.dbServer.AddInParameter(storedProcCommand, "LoyaltyProgramID", DbType.Int64, nvo.LoyaltyProgramID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if (nvo.SearchInAnotherClinic)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SearchInAnotherClinic", DbType.Boolean, nvo.SearchInAnotherClinic);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, nvo.LinkServer);
                if (nvo.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, nvo.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["SearchKeyword"].ToString()));
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
            finally
            {
                connection.Close();
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject GetSurrogatePatientListForDashboard(IValueObject valueObject, clsUserVO UserVo)
        {
            GetPatientListForDashboardBizActionVO nvo = valueObject as GetPatientListForDashboardBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSurrogatePatientListForDashboard");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["SurrogateID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["SurrogateUnitID"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MrNo"]),
                            IsSurrogateUsed = (bool) DALHelper.HandleDBNull(reader["IsSurrogateUsed"])
                        };
                        nvo.PatientDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                connection.Close();
                connection = null;
            }
            return valueObject;
        }

        public override IValueObject GetSurrogatePatientListForTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            GetPatientListForDashboardBizActionVO nvo = valueObject as GetPatientListForDashboardBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSurrogatePatientListForTransfer");
                this.dbServer.AddInParameter(storedProcCommand, "EMRProcedureID", DbType.String, nvo.EMRProcedureID);
                this.dbServer.AddInParameter(storedProcCommand, "EMRProcedureUnitID", DbType.Int64, nvo.EMRProcedureUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientDetailsList == null)
                    {
                        nvo.PatientDetailsList = new List<clsPatientGeneralVO>();
                    }
                    while (reader.Read())
                    {
                        clsPatientGeneralVO item = new clsPatientGeneralVO {
                            PatientID = (long) DALHelper.HandleDBNull(reader["SurrogateID"]),
                            UnitId = (long) DALHelper.HandleDBNull(reader["SurrogateUnitID"]),
                            MRNo = (string) DALHelper.HandleDBNull(reader["MrNo"])
                        };
                        nvo.PatientDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            finally
            {
                connection.Close();
                connection = null;
            }
            return valueObject;
        }

        private clsIVFDashboard_GetTherapyListBizActionVO GetTherapyDetailListForIVFDashboard(clsIVFDashboard_GetTherapyListBizActionVO BizAction, clsUserVO objUserVo)
        {
            this.dbServer.CreateConnection();
            try
            {
                if (!BizAction.Flag)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetTherapyListForLabDay");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, BizAction.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "TherapyUnitID", DbType.Int64, BizAction.TherapyUnitID);
                    BizAction.TherapyDetailsList = new List<clsPlanTherapyVO>();
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            BizAction.TherapyDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"]));
                            BizAction.TherapyDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"]));
                            BizAction.TherapyDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader2["CoupleID"]));
                            BizAction.TherapyDetails.TherapyStartDate = DALHelper.HandleDate(reader2["TherapyStartDate"]);
                            BizAction.TherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader2["CycleDuration"]));
                            BizAction.TherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ProtocolTypeID"]));
                            BizAction.TherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PlannedTreatmentID"]));
                            BizAction.TherapyDetails.PlannedNoofEmbryos = Convert.ToInt32(DALHelper.HandleDBNull(reader2["PlannedNoofEmbryos"]));
                            BizAction.TherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["MainInductionID"]));
                            BizAction.TherapyDetails.MainInduction = Convert.ToString(DALHelper.HandleDBNull(reader2["MainIndication"]));
                            BizAction.TherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader2["Physician"]));
                            BizAction.TherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ExternalSimulationID"]));
                            BizAction.TherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader2["SpermCollection"]));
                            BizAction.TherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader2["PlannedTreatment"]));
                            BizAction.TherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader2["ProtocolType"]));
                        }
                    }
                    reader2.Close();
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetTherapyList");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, BizAction.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "TabID", DbType.Int64, BizAction.TabID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, BizAction.TherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlannedTreatmentID", DbType.Int64, BizAction.PlannedTreatmentID);
                    this.dbServer.AddInParameter(storedProcCommand, "ProtocolTypeID", DbType.Int64, BizAction.ProtocolTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "PhysicianId", DbType.Int64, BizAction.PhysicianId);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoActive", DbType.Boolean, BizAction.rdoActive);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoAll", DbType.Boolean, BizAction.rdoAll);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoClosed", DbType.Boolean, BizAction.rdoClosed);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoSuccessful", DbType.Boolean, BizAction.rdoSuccessful);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoUnsuccessful", DbType.Boolean, BizAction.rdoUnsuccessful);
                    this.dbServer.AddInParameter(storedProcCommand, "AttachedSurrogate", DbType.Boolean, BizAction.AttachedSurrogate);
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, objUserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycle", DbType.Boolean, BizAction.TherapyDetails.IsDonorCycle);
                    this.dbServer.AddInParameter(storedProcCommand, "IsIVFBillingCriteria", DbType.Boolean, BizAction.TherapyDetails.IsIVFBillingCriteria);
                    BizAction.TherapyDetailsList = new List<clsPlanTherapyVO>();
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (BizAction.TherapyID != 0L)
                    {
                        if (BizAction.TabID == 0L)
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyExecutionList.Add(this.TherapyExcecution(BizAction, reader));
                                }
                            }
                            reader.NextResult();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.FollicularMonitoringList.Add(this.FollicularMonitoring(BizAction, reader));
                                }
                            }
                            reader.NextResult();
                            bool? attachedSurrogate = BizAction.AttachedSurrogate;
                            if ((attachedSurrogate.GetValueOrDefault() && (attachedSurrogate != null)) && reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyExecutionListSurrogate.Add(this.TherapyExcecutionSurrogate(BizAction, reader));
                                }
                            }
                        }
                        else if (BizAction.TabID != 4L)
                        {
                            if (BizAction.TabID != 2L)
                            {
                                if ((BizAction.TabID == 3L) && reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        BizAction.FollicularMonitoringList.Add(this.FollicularMonitoring(BizAction, reader));
                                    }
                                }
                            }
                            else
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        BizAction.TherapyExecutionList.Add(this.TherapyExcecution(BizAction, reader));
                                    }
                                }
                                reader.NextResult();
                                bool? attachedSurrogate = BizAction.AttachedSurrogate;
                                if ((attachedSurrogate.GetValueOrDefault() && (attachedSurrogate != null)) && reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        BizAction.TherapyExecutionListSurrogate.Add(this.TherapyExcecutionSurrogate(BizAction, reader));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsPlanTherapyVO item = new clsPlanTherapyVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                    CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"])),
                                    TherapyStartDate = DALHelper.HandleDate(reader["TherapyStartDate"]),
                                    Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"])),
                                    ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"])),
                                    PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"])),
                                    MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"])),
                                    MainSubInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainSubIndicationID"])),
                                    PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"])),
                                    ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"])),
                                    PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"])),
                                    Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"])),
                                    PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"])),
                                    SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"])),
                                    ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"])),
                                    IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"])),
                                    OPUDONEBY = Convert.ToString(DALHelper.HandleDBNull(reader["OPUDONEBY"])),
                                    ETDate = DALHelper.HandleDate(reader["ETDate"]),
                                    SourceOfOoctye = Convert.ToString(DALHelper.HandleDBNull(reader["SourceOfOoctye"])),
                                    SourceOfSemen = Convert.ToString(DALHelper.HandleDBNull(reader["SourceOfSemen"])),
                                    SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"])),
                                    SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"])),
                                    AttachedSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AttachedSurrogate"])),
                                    SurrogateMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMRNo"])),
                                    IsEmbryoDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmbryoDonation"])),
                                    SpermSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermSourceId"])),
                                    StartOvarian = DALHelper.HandleDate(reader["StartOvarian"]),
                                    EndOvarian = DALHelper.HandleDate(reader["EndOvarian"]),
                                    StartTrigger = DALHelper.HandleDate(reader["StartTrigger"]),
                                    TriggerTime = DALHelper.HandleDate(reader["TriggerTime"]),
                                    StartStimulation = DALHelper.HandleDate(reader["StartStimulation"]),
                                    EndStimulation = DALHelper.HandleDate(reader["EndStimulation"]),
                                    SpermCollectionDate = DALHelper.HandleDate(reader["SpermCollectionDate"]),
                                    IsCancellation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"])),
                                    Note = Convert.ToString(DALHelper.HandleDBNull(reader["Note"])),
                                    CancellationReason = Convert.ToString(DALHelper.HandleDBNull(reader["CancellationReason"])),
                                    PACAnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PACEnabled"])),
                                    AnesthesistId = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesistID"])),
                                    ConsentCheck = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentCheck"])),
                                    LutealStartDate = DALHelper.HandleDate(reader["LutealStartDate"]),
                                    LutealEndDate = DALHelper.HandleDate(reader["LutealEndDate"]),
                                    EMRProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EMRProcedureID"])),
                                    EMRProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EMRProcedureUnitID"])),
                                    IsDonorCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycle"])),
                                    IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogacy"])),
                                    MainIndication = Convert.ToString(DALHelper.HandleDBNull(reader["MainIndication"])),
                                    SourceOfSperm = Convert.ToString(DALHelper.HandleDBNull(reader["SourceOfSperm"])),
                                    OPUtDate = DALHelper.HandleDate(reader["OPUDate"]),
                                    OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"])),
                                    NoOocytesCryo = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOocytesCryo"])),
                                    NoEmbryoCryo = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoEmbryoCryo"])),
                                    NoEmbryosTransferred = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoEmbryosTransferred"])),
                                    NoFrozenEmbryosTransferred = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoFrozenEmbryosTransferred"])),
                                    NoOocytesDonated = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOocytesDonated"])),
                                    NoOfEmbryos = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfEmbryos"])),
                                    NoEmbryoDonated = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoEmbryoDonated"])),
                                    NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"])),
                                    FetalHeartCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartCount"])),
                                    BHCGAssessment = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAssessment"])),
                                    NoOfEmbExtCulture = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfEmbExtCulture"])),
                                    NoOfEmbFzOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfEmbFzOocyte"])),
                                    OPUCycleCancellationReason = Convert.ToString(DALHelper.HandleDBNull(reader["IsOPUCancelReason"])),
                                    IsOPUCycleCancellation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOPUCancel"])),
                                    OPUFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OPUFreeze"]))
                                };
                                if (this.IsIsthambul.Equals("Yes"))
                                {
                                    item.IsIsthambul = true;
                                }
                                else if (this.IsIsthambul.Equals("No"))
                                {
                                    item.IsIsthambul = false;
                                }
                                BizAction.TherapyDetailsList.Add(item);
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.NewButtonVisibility = new bool?(Convert.ToBoolean(DALHelper.HandleDBNull(reader["NewButtonVisibility"])));
                            }
                        }
                    }
                    reader.Close();
                    reader.Close();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return BizAction;
        }

        public override IValueObject GetTherapyDetailListForIVFDashboard(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIVFDashboard_GetTherapyListBizActionVO bizAction = valueObject as clsIVFDashboard_GetTherapyListBizActionVO;
            bizAction = this.GetTherapyDetailListForIVFDashboard(bizAction, objUserVO);
            return valueObject;
        }

        public override IValueObject GetTherapyDetailsFromCycleCode(IValueObject valueObject, clsUserVO UserVo)
        {
            GetCycleDetailsFromCycleCodeBizActionVO nvo = (GetCycleDetailsFromCycleCodeBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCycleDetailsFromCycleCode");
                this.dbServer.AddInParameter(storedProcCommand, "CycleCode", DbType.String, nvo.CycleCode);
                nvo.TherapyDetails = new clsPlanTherapyVO();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.TherapyDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.TherapyDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.TherapyDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.TherapyDetails.TherapyStartDate = DALHelper.HandleDate(reader["TherapyStartDate"]);
                        nvo.TherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                        nvo.TherapyDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                        nvo.TherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                        nvo.TherapyDetails.Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"]));
                        nvo.TherapyDetails.PillStartDate = DALHelper.HandleDate(reader["PillStartDate"]);
                        nvo.TherapyDetails.PillEndDate = DALHelper.HandleDate(reader["PillEndDate"]);
                        nvo.TherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                        nvo.TherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                        nvo.TherapyDetails.PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
                        nvo.TherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                        nvo.TherapyDetails.PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"]));
                        nvo.TherapyDetails.TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"]));
                        nvo.TherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                        nvo.TherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                        nvo.TherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                        nvo.TherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                        nvo.TherapyDetails.MainInduction = Convert.ToString(DALHelper.HandleDBNull(reader["MainIndication"]));
                        nvo.TherapyDetails.PlannedEmbryos = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
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

        public override IValueObject GetVisitForARTPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO nvo = valueObject as clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetVisitForARTPrescription");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
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

        public clsTherapyExecutionVO TherapyExcecution(clsIVFDashboard_GetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            clsTherapyExecutionVO nvo = new clsTherapyExecutionVO {
                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"])),
                PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"])),
                TherapyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyTypeId"])),
                PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"])),
                TherapyStartDate = DALHelper.HandleDate(reader["TherapyStartDate"]),
                ThearpyTypeDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"])),
                IsOocyteDonorExists = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOocyteDonorExists"])),
                IsSemenDonorExists = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSemenDonorExists"])),
                OoctyDonorMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["OoctyDonorMrNo"])),
                SemenDonorMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SemenDonorMrNo"])),
                PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]))
            };
            if (1 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.Event.ToString();
                nvo.Head = "Date of LMP";
                nvo.IsBool = true;
                nvo.IsText = false;
                nvo.IsReadOnly = false;
            }
            else if (2 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.Drug.ToString();
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = false;
                nvo.Head = Convert.ToString(DALHelper.HandleDBNull(reader["DrugName"]));
            }
            else if (3 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.UltraSound.ToString();
                nvo.Head = "Follicular Scan";
                nvo.IsBool = true;
                nvo.IsText = false;
                nvo.IsReadOnly = false;
            }
            else if (4 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.OvumPickUp.ToString();
                nvo.Head = "OPU";
                nvo.IsBool = true;
                nvo.IsText = false;
                nvo.IsReadOnly = false;
            }
            else if (5 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.EmbryoTransfer.ToString();
                nvo.Head = "ET";
                nvo.IsBool = true;
                nvo.IsText = false;
                nvo.IsReadOnly = false;
            }
            else if (6 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.E2.ToString();
                nvo.Head = "AMH";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (7 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.E2.ToString();
                nvo.Head = "E2";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (8 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.Progesterone.ToString();
                nvo.Head = "Progesterone";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (9 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.FSH.ToString();
                nvo.Head = "LH";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (10 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.FSH.ToString();
                nvo.Head = "FSH";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (11 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.Prolactin.ToString();
                nvo.Head = "Prolactin";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (12 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.BHCG.ToString();
                nvo.Head = "TSH";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (13 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.BHCG.ToString();
                nvo.Head = "Testosterone";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (14 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.BHCG.ToString();
                nvo.Head = "BHCG";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (15 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.HIV.ToString();
                nvo.Head = "HIV";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (0x10 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.HCV.ToString();
                nvo.Head = "HCV";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            else if (0x11 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.HbSAg.ToString();
                nvo.Head = "HbSAg";
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.IsReadOnly = true;
            }
            nvo.Day1 = Convert.ToString(DALHelper.HandleDBNull(reader["Day1"]));
            nvo.Day2 = Convert.ToString(DALHelper.HandleDBNull(reader["Day2"]));
            nvo.Day3 = Convert.ToString(DALHelper.HandleDBNull(reader["Day3"]));
            nvo.Day4 = Convert.ToString(DALHelper.HandleDBNull(reader["Day4"]));
            nvo.Day5 = Convert.ToString(DALHelper.HandleDBNull(reader["Day5"]));
            nvo.Day6 = Convert.ToString(DALHelper.HandleDBNull(reader["Day6"]));
            nvo.Day7 = Convert.ToString(DALHelper.HandleDBNull(reader["Day7"]));
            nvo.Day8 = Convert.ToString(DALHelper.HandleDBNull(reader["Day8"]));
            nvo.Day9 = Convert.ToString(DALHelper.HandleDBNull(reader["Day9"]));
            nvo.Day10 = Convert.ToString(DALHelper.HandleDBNull(reader["Day10"]));
            nvo.Day11 = Convert.ToString(DALHelper.HandleDBNull(reader["Day11"]));
            nvo.Day12 = Convert.ToString(DALHelper.HandleDBNull(reader["Day12"]));
            nvo.Day13 = Convert.ToString(DALHelper.HandleDBNull(reader["Day13"]));
            nvo.Day14 = Convert.ToString(DALHelper.HandleDBNull(reader["Day14"]));
            nvo.Day15 = Convert.ToString(DALHelper.HandleDBNull(reader["Day15"]));
            nvo.Day16 = Convert.ToString(DALHelper.HandleDBNull(reader["Day16"]));
            nvo.Day17 = Convert.ToString(DALHelper.HandleDBNull(reader["Day17"]));
            nvo.Day18 = Convert.ToString(DALHelper.HandleDBNull(reader["Day18"]));
            nvo.Day19 = Convert.ToString(DALHelper.HandleDBNull(reader["Day19"]));
            nvo.Day20 = Convert.ToString(DALHelper.HandleDBNull(reader["Day20"]));
            nvo.Day21 = Convert.ToString(DALHelper.HandleDBNull(reader["Day21"]));
            nvo.Day22 = Convert.ToString(DALHelper.HandleDBNull(reader["Day22"]));
            nvo.Day23 = Convert.ToString(DALHelper.HandleDBNull(reader["Day23"]));
            nvo.Day24 = Convert.ToString(DALHelper.HandleDBNull(reader["Day24"]));
            nvo.Day25 = Convert.ToString(DALHelper.HandleDBNull(reader["Day25"]));
            nvo.Day26 = Convert.ToString(DALHelper.HandleDBNull(reader["Day26"]));
            nvo.Day27 = Convert.ToString(DALHelper.HandleDBNull(reader["Day27"]));
            nvo.Day28 = Convert.ToString(DALHelper.HandleDBNull(reader["Day28"]));
            nvo.Day29 = Convert.ToString(DALHelper.HandleDBNull(reader["Day29"]));
            nvo.Day30 = Convert.ToString(DALHelper.HandleDBNull(reader["Day30"]));
            nvo.Day31 = Convert.ToString(DALHelper.HandleDBNull(reader["Day31"]));
            nvo.Day32 = Convert.ToString(DALHelper.HandleDBNull(reader["Day32"]));
            nvo.Day33 = Convert.ToString(DALHelper.HandleDBNull(reader["Day33"]));
            nvo.Day34 = Convert.ToString(DALHelper.HandleDBNull(reader["Day34"]));
            nvo.Day35 = Convert.ToString(DALHelper.HandleDBNull(reader["Day35"]));
            nvo.Day36 = Convert.ToString(DALHelper.HandleDBNull(reader["Day36"]));
            nvo.Day37 = Convert.ToString(DALHelper.HandleDBNull(reader["Day37"]));
            nvo.Day38 = Convert.ToString(DALHelper.HandleDBNull(reader["Day38"]));
            nvo.Day39 = Convert.ToString(DALHelper.HandleDBNull(reader["Day39"]));
            nvo.Day40 = Convert.ToString(DALHelper.HandleDBNull(reader["Day40"]));
            nvo.Day41 = Convert.ToString(DALHelper.HandleDBNull(reader["Day41"]));
            nvo.Day42 = Convert.ToString(DALHelper.HandleDBNull(reader["Day42"]));
            nvo.Day43 = Convert.ToString(DALHelper.HandleDBNull(reader["Day43"]));
            nvo.Day44 = Convert.ToString(DALHelper.HandleDBNull(reader["Day44"]));
            nvo.Day45 = Convert.ToString(DALHelper.HandleDBNull(reader["Day45"]));
            nvo.Day46 = Convert.ToString(DALHelper.HandleDBNull(reader["Day46"]));
            nvo.Day47 = Convert.ToString(DALHelper.HandleDBNull(reader["Day47"]));
            nvo.Day48 = Convert.ToString(DALHelper.HandleDBNull(reader["Day48"]));
            nvo.Day49 = Convert.ToString(DALHelper.HandleDBNull(reader["Day49"]));
            nvo.Day50 = Convert.ToString(DALHelper.HandleDBNull(reader["Day50"]));
            nvo.Day51 = Convert.ToString(DALHelper.HandleDBNull(reader["Day51"]));
            nvo.Day52 = Convert.ToString(DALHelper.HandleDBNull(reader["Day52"]));
            nvo.Day53 = Convert.ToString(DALHelper.HandleDBNull(reader["Day53"]));
            nvo.Day54 = Convert.ToString(DALHelper.HandleDBNull(reader["Day54"]));
            nvo.Day55 = Convert.ToString(DALHelper.HandleDBNull(reader["Day55"]));
            nvo.Day56 = Convert.ToString(DALHelper.HandleDBNull(reader["Day56"]));
            nvo.Day57 = Convert.ToString(DALHelper.HandleDBNull(reader["Day57"]));
            nvo.Day58 = Convert.ToString(DALHelper.HandleDBNull(reader["Day58"]));
            nvo.Day59 = Convert.ToString(DALHelper.HandleDBNull(reader["Day59"]));
            nvo.Day60 = Convert.ToString(DALHelper.HandleDBNull(reader["Day60"]));
            return nvo;
        }

        public clsTherapyExecutionVO TherapyExcecutionSurrogate(clsIVFDashboard_GetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            clsTherapyExecutionVO nvo = new clsTherapyExecutionVO {
                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"])),
                PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"])),
                TherapyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyTypeId"])),
                PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"])),
                TherapyStartDate = DALHelper.HandleDate(reader["TherapyStartDate"]),
                ThearpyTypeDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"]))
            };
            if (1 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.Event.ToString();
                nvo.Head = "Date of LMP";
                nvo.IsBool = true;
                nvo.IsText = false;
            }
            else if (2 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.Drug.ToString();
                nvo.IsBool = false;
                nvo.IsText = true;
                nvo.Head = Convert.ToString(DALHelper.HandleDBNull(reader["DrugName"]));
            }
            else if (5 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.EmbryoTransfer.ToString();
                nvo.Head = "ET";
                nvo.IsBool = true;
                nvo.IsText = false;
            }
            nvo.Day1 = Convert.ToString(DALHelper.HandleDBNull(reader["Day1"]));
            nvo.Day2 = Convert.ToString(DALHelper.HandleDBNull(reader["Day2"]));
            nvo.Day3 = Convert.ToString(DALHelper.HandleDBNull(reader["Day3"]));
            nvo.Day4 = Convert.ToString(DALHelper.HandleDBNull(reader["Day4"]));
            nvo.Day5 = Convert.ToString(DALHelper.HandleDBNull(reader["Day5"]));
            nvo.Day6 = Convert.ToString(DALHelper.HandleDBNull(reader["Day6"]));
            nvo.Day7 = Convert.ToString(DALHelper.HandleDBNull(reader["Day7"]));
            nvo.Day8 = Convert.ToString(DALHelper.HandleDBNull(reader["Day8"]));
            nvo.Day9 = Convert.ToString(DALHelper.HandleDBNull(reader["Day9"]));
            nvo.Day10 = Convert.ToString(DALHelper.HandleDBNull(reader["Day10"]));
            nvo.Day11 = Convert.ToString(DALHelper.HandleDBNull(reader["Day11"]));
            nvo.Day12 = Convert.ToString(DALHelper.HandleDBNull(reader["Day12"]));
            nvo.Day13 = Convert.ToString(DALHelper.HandleDBNull(reader["Day13"]));
            nvo.Day14 = Convert.ToString(DALHelper.HandleDBNull(reader["Day14"]));
            nvo.Day15 = Convert.ToString(DALHelper.HandleDBNull(reader["Day15"]));
            nvo.Day16 = Convert.ToString(DALHelper.HandleDBNull(reader["Day16"]));
            nvo.Day17 = Convert.ToString(DALHelper.HandleDBNull(reader["Day17"]));
            nvo.Day18 = Convert.ToString(DALHelper.HandleDBNull(reader["Day18"]));
            nvo.Day19 = Convert.ToString(DALHelper.HandleDBNull(reader["Day19"]));
            nvo.Day20 = Convert.ToString(DALHelper.HandleDBNull(reader["Day20"]));
            nvo.Day21 = Convert.ToString(DALHelper.HandleDBNull(reader["Day21"]));
            nvo.Day22 = Convert.ToString(DALHelper.HandleDBNull(reader["Day22"]));
            nvo.Day23 = Convert.ToString(DALHelper.HandleDBNull(reader["Day23"]));
            nvo.Day24 = Convert.ToString(DALHelper.HandleDBNull(reader["Day24"]));
            nvo.Day25 = Convert.ToString(DALHelper.HandleDBNull(reader["Day25"]));
            nvo.Day26 = Convert.ToString(DALHelper.HandleDBNull(reader["Day26"]));
            nvo.Day27 = Convert.ToString(DALHelper.HandleDBNull(reader["Day27"]));
            nvo.Day28 = Convert.ToString(DALHelper.HandleDBNull(reader["Day28"]));
            nvo.Day29 = Convert.ToString(DALHelper.HandleDBNull(reader["Day29"]));
            nvo.Day30 = Convert.ToString(DALHelper.HandleDBNull(reader["Day30"]));
            nvo.Day31 = Convert.ToString(DALHelper.HandleDBNull(reader["Day31"]));
            nvo.Day32 = Convert.ToString(DALHelper.HandleDBNull(reader["Day32"]));
            nvo.Day33 = Convert.ToString(DALHelper.HandleDBNull(reader["Day33"]));
            nvo.Day35 = Convert.ToString(DALHelper.HandleDBNull(reader["Day34"]));
            nvo.Day36 = Convert.ToString(DALHelper.HandleDBNull(reader["Day35"]));
            nvo.Day37 = Convert.ToString(DALHelper.HandleDBNull(reader["Day36"]));
            nvo.Day38 = Convert.ToString(DALHelper.HandleDBNull(reader["Day37"]));
            nvo.Day39 = Convert.ToString(DALHelper.HandleDBNull(reader["Day38"]));
            nvo.Day40 = Convert.ToString(DALHelper.HandleDBNull(reader["Day39"]));
            nvo.Day41 = Convert.ToString(DALHelper.HandleDBNull(reader["Day40"]));
            nvo.Day42 = Convert.ToString(DALHelper.HandleDBNull(reader["Day41"]));
            nvo.Day43 = Convert.ToString(DALHelper.HandleDBNull(reader["Day42"]));
            nvo.Day44 = Convert.ToString(DALHelper.HandleDBNull(reader["Day43"]));
            nvo.Day45 = Convert.ToString(DALHelper.HandleDBNull(reader["Day44"]));
            nvo.Day46 = Convert.ToString(DALHelper.HandleDBNull(reader["Day45"]));
            nvo.Day47 = Convert.ToString(DALHelper.HandleDBNull(reader["Day46"]));
            nvo.Day48 = Convert.ToString(DALHelper.HandleDBNull(reader["Day47"]));
            nvo.Day49 = Convert.ToString(DALHelper.HandleDBNull(reader["Day48"]));
            nvo.Day50 = Convert.ToString(DALHelper.HandleDBNull(reader["Day49"]));
            nvo.Day50 = Convert.ToString(DALHelper.HandleDBNull(reader["Day50"]));
            nvo.Day51 = Convert.ToString(DALHelper.HandleDBNull(reader["Day51"]));
            nvo.Day52 = Convert.ToString(DALHelper.HandleDBNull(reader["Day52"]));
            nvo.Day53 = Convert.ToString(DALHelper.HandleDBNull(reader["Day53"]));
            nvo.Day54 = Convert.ToString(DALHelper.HandleDBNull(reader["Day54"]));
            nvo.Day55 = Convert.ToString(DALHelper.HandleDBNull(reader["Day55"]));
            nvo.Day56 = Convert.ToString(DALHelper.HandleDBNull(reader["Day56"]));
            nvo.Day57 = Convert.ToString(DALHelper.HandleDBNull(reader["Day57"]));
            nvo.Day58 = Convert.ToString(DALHelper.HandleDBNull(reader["Day58"]));
            nvo.Day59 = Convert.ToString(DALHelper.HandleDBNull(reader["Day59"]));
            nvo.Day60 = Convert.ToString(DALHelper.HandleDBNull(reader["Day60"]));
            return nvo;
        }

        public override IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_UpdateFollicularMonitoringBizActionVO nvo = valueObject as clsIVFDashboard_UpdateFollicularMonitoringBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateFollicularMonitoring");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.FollicularID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.FollicularMonitoringDetial.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyId", DbType.Int64, nvo.FollicularMonitoringDetial.TherapyId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.FollicularMonitoringDetial.TherapyUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.FollicularMonitoringDetial.Date);
                this.dbServer.AddInParameter(storedProcCommand, "AttendedPhysicianId", DbType.Int64, nvo.FollicularMonitoringDetial.PhysicianID);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, nvo.FollicularMonitoringDetial.FollicularNotes);
                this.dbServer.AddInParameter(storedProcCommand, "AttachmentPath", DbType.String, nvo.FollicularMonitoringDetial.AttachmentPath);
                this.dbServer.AddInParameter(storedProcCommand, "AttachmentFileContents", DbType.Binary, nvo.FollicularMonitoringDetial.AttachmentFileContent);
                this.dbServer.AddInParameter(storedProcCommand, "EndometriumThickness", DbType.String, nvo.FollicularMonitoringDetial.EndometriumThickness);
                this.dbServer.AddInParameter(storedProcCommand, "FollicularNoList", DbType.String, nvo.FollicularMonitoringDetial.FollicularNoList);
                this.dbServer.AddInParameter(storedProcCommand, "LeftSizeList", DbType.String, nvo.FollicularMonitoringDetial.LeftSizeList);
                this.dbServer.AddInParameter(storedProcCommand, "RightSizeList", DbType.String, nvo.FollicularMonitoringDetial.RightSizeList);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.FollicularMonitoringDetial = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject UpdateTherapyExecution(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateTherapyExecutionBizActionVO nvo = valueObject as clsUpdateTherapyExecutionBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_UpdateTherapyExecution");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.TherapyExecutionDetial.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.TherapyExecutionDetial.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.TherapyExecutionDetial.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.TherapyExecutionDetial.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.TherapyExecutionDetial.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyId", DbType.Int64, nvo.TherapyExecutionDetial.PlanTherapyId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitId", DbType.Int64, nvo.TherapyExecutionDetial.PlanTherapyUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyTypeId", DbType.Int64, nvo.TherapyExecutionDetial.TherapyTypeId);
                this.dbServer.AddInParameter(storedProcCommand, "ThearpyTypeDetailId", DbType.Int64, nvo.TherapyExecutionDetial.ThearpyTypeDetailId);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicianID", DbType.Int64, nvo.TherapyExecutionDetial.PhysicianID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyStartDate", DbType.DateTime, nvo.TherapyExecutionDetial.TherapyStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "IsSurrogate", DbType.Boolean, nvo.TherapyExecutionDetial.IsSurrogate);
                this.dbServer.AddInParameter(storedProcCommand, "IsActive", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.String, nvo.TherapyExecutionDetial.Day);
                this.dbServer.AddInParameter(storedProcCommand, "Day1", DbType.String, nvo.TherapyExecutionDetial.Day1);
                this.dbServer.AddInParameter(storedProcCommand, "Day2", DbType.String, nvo.TherapyExecutionDetial.Day2);
                this.dbServer.AddInParameter(storedProcCommand, "Day3", DbType.String, nvo.TherapyExecutionDetial.Day3);
                this.dbServer.AddInParameter(storedProcCommand, "Day4", DbType.String, nvo.TherapyExecutionDetial.Day4);
                this.dbServer.AddInParameter(storedProcCommand, "Day5", DbType.String, nvo.TherapyExecutionDetial.Day5);
                this.dbServer.AddInParameter(storedProcCommand, "Day6", DbType.String, nvo.TherapyExecutionDetial.Day6);
                this.dbServer.AddInParameter(storedProcCommand, "Day7", DbType.String, nvo.TherapyExecutionDetial.Day7);
                this.dbServer.AddInParameter(storedProcCommand, "Day8", DbType.String, nvo.TherapyExecutionDetial.Day8);
                this.dbServer.AddInParameter(storedProcCommand, "Day9", DbType.String, nvo.TherapyExecutionDetial.Day9);
                this.dbServer.AddInParameter(storedProcCommand, "Day10", DbType.String, nvo.TherapyExecutionDetial.Day10);
                this.dbServer.AddInParameter(storedProcCommand, "Day11", DbType.String, nvo.TherapyExecutionDetial.Day11);
                this.dbServer.AddInParameter(storedProcCommand, "Day12", DbType.String, nvo.TherapyExecutionDetial.Day12);
                this.dbServer.AddInParameter(storedProcCommand, "Day13", DbType.String, nvo.TherapyExecutionDetial.Day13);
                this.dbServer.AddInParameter(storedProcCommand, "Day14", DbType.String, nvo.TherapyExecutionDetial.Day14);
                this.dbServer.AddInParameter(storedProcCommand, "Day15", DbType.String, nvo.TherapyExecutionDetial.Day15);
                this.dbServer.AddInParameter(storedProcCommand, "Day16", DbType.String, nvo.TherapyExecutionDetial.Day16);
                this.dbServer.AddInParameter(storedProcCommand, "Day17", DbType.String, nvo.TherapyExecutionDetial.Day17);
                this.dbServer.AddInParameter(storedProcCommand, "Day18", DbType.String, nvo.TherapyExecutionDetial.Day18);
                this.dbServer.AddInParameter(storedProcCommand, "Day19", DbType.String, nvo.TherapyExecutionDetial.Day19);
                this.dbServer.AddInParameter(storedProcCommand, "Day20", DbType.String, nvo.TherapyExecutionDetial.Day20);
                this.dbServer.AddInParameter(storedProcCommand, "Day21", DbType.String, nvo.TherapyExecutionDetial.Day21);
                this.dbServer.AddInParameter(storedProcCommand, "Day22", DbType.String, nvo.TherapyExecutionDetial.Day22);
                this.dbServer.AddInParameter(storedProcCommand, "Day23", DbType.String, nvo.TherapyExecutionDetial.Day23);
                this.dbServer.AddInParameter(storedProcCommand, "Day24", DbType.String, nvo.TherapyExecutionDetial.Day24);
                this.dbServer.AddInParameter(storedProcCommand, "Day25", DbType.String, nvo.TherapyExecutionDetial.Day25);
                this.dbServer.AddInParameter(storedProcCommand, "Day26", DbType.String, nvo.TherapyExecutionDetial.Day26);
                this.dbServer.AddInParameter(storedProcCommand, "Day27", DbType.String, nvo.TherapyExecutionDetial.Day27);
                this.dbServer.AddInParameter(storedProcCommand, "Day28", DbType.String, nvo.TherapyExecutionDetial.Day28);
                this.dbServer.AddInParameter(storedProcCommand, "Day29", DbType.String, nvo.TherapyExecutionDetial.Day29);
                this.dbServer.AddInParameter(storedProcCommand, "Day30", DbType.String, nvo.TherapyExecutionDetial.Day30);
                this.dbServer.AddInParameter(storedProcCommand, "Day31", DbType.String, nvo.TherapyExecutionDetial.Day31);
                this.dbServer.AddInParameter(storedProcCommand, "Day32", DbType.String, nvo.TherapyExecutionDetial.Day32);
                this.dbServer.AddInParameter(storedProcCommand, "Day33", DbType.String, nvo.TherapyExecutionDetial.Day33);
                this.dbServer.AddInParameter(storedProcCommand, "Day34", DbType.String, nvo.TherapyExecutionDetial.Day34);
                this.dbServer.AddInParameter(storedProcCommand, "Day35", DbType.String, nvo.TherapyExecutionDetial.Day35);
                this.dbServer.AddInParameter(storedProcCommand, "Day36", DbType.String, nvo.TherapyExecutionDetial.Day36);
                this.dbServer.AddInParameter(storedProcCommand, "Day37", DbType.String, nvo.TherapyExecutionDetial.Day37);
                this.dbServer.AddInParameter(storedProcCommand, "Day38", DbType.String, nvo.TherapyExecutionDetial.Day38);
                this.dbServer.AddInParameter(storedProcCommand, "Day39", DbType.String, nvo.TherapyExecutionDetial.Day39);
                this.dbServer.AddInParameter(storedProcCommand, "Day40", DbType.String, nvo.TherapyExecutionDetial.Day40);
                this.dbServer.AddInParameter(storedProcCommand, "Day41", DbType.String, nvo.TherapyExecutionDetial.Day41);
                this.dbServer.AddInParameter(storedProcCommand, "Day42", DbType.String, nvo.TherapyExecutionDetial.Day42);
                this.dbServer.AddInParameter(storedProcCommand, "Day43", DbType.String, nvo.TherapyExecutionDetial.Day43);
                this.dbServer.AddInParameter(storedProcCommand, "Day44", DbType.String, nvo.TherapyExecutionDetial.Day44);
                this.dbServer.AddInParameter(storedProcCommand, "Day45", DbType.String, nvo.TherapyExecutionDetial.Day45);
                this.dbServer.AddInParameter(storedProcCommand, "Day46", DbType.String, nvo.TherapyExecutionDetial.Day46);
                this.dbServer.AddInParameter(storedProcCommand, "Day47", DbType.String, nvo.TherapyExecutionDetial.Day47);
                this.dbServer.AddInParameter(storedProcCommand, "Day48", DbType.String, nvo.TherapyExecutionDetial.Day48);
                this.dbServer.AddInParameter(storedProcCommand, "Day49", DbType.String, nvo.TherapyExecutionDetial.Day49);
                this.dbServer.AddInParameter(storedProcCommand, "Day50", DbType.String, nvo.TherapyExecutionDetial.Day50);
                this.dbServer.AddInParameter(storedProcCommand, "Day51", DbType.String, nvo.TherapyExecutionDetial.Day51);
                this.dbServer.AddInParameter(storedProcCommand, "Day52", DbType.String, nvo.TherapyExecutionDetial.Day52);
                this.dbServer.AddInParameter(storedProcCommand, "Day53", DbType.String, nvo.TherapyExecutionDetial.Day53);
                this.dbServer.AddInParameter(storedProcCommand, "Day54", DbType.String, nvo.TherapyExecutionDetial.Day54);
                this.dbServer.AddInParameter(storedProcCommand, "Day55", DbType.String, nvo.TherapyExecutionDetial.Day55);
                this.dbServer.AddInParameter(storedProcCommand, "Day56", DbType.String, nvo.TherapyExecutionDetial.Day56);
                this.dbServer.AddInParameter(storedProcCommand, "Day57", DbType.String, nvo.TherapyExecutionDetial.Day57);
                this.dbServer.AddInParameter(storedProcCommand, "Day58", DbType.String, nvo.TherapyExecutionDetial.Day58);
                this.dbServer.AddInParameter(storedProcCommand, "Day59", DbType.String, nvo.TherapyExecutionDetial.Day59);
                this.dbServer.AddInParameter(storedProcCommand, "Day60", DbType.String, nvo.TherapyExecutionDetial.Day60);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.TherapyExecutionDetial = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }
    }
}

