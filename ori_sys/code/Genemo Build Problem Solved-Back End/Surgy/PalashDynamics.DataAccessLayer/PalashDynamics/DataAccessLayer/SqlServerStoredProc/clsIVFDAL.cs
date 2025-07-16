namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsIVFDAL : clsBaseIVFDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFDAL()
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

        public override IValueObject AddClinicalSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddClinicalSummaryBizActionVO nvo = valueObject as clsAddClinicalSummaryBizActionVO;
            try
            {
                clsClinicalSummaryVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddClinicalSummary");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SummaryID", DbType.Int64, details.SummaryID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddGeneralExaminationFemaleBizActionVO AddFemaleGeneralDetails(clsAddGeneralExaminationFemaleBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsGeneralExaminationVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddGeneralExaminationForFemale");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "Weight", DbType.Double, details.Weight);
                this.dbServer.AddInParameter(storedProcCommand, "Height", DbType.Double, details.Height);
                this.dbServer.AddInParameter(storedProcCommand, "BMI", DbType.Double, details.BMI);
                this.dbServer.AddInParameter(storedProcCommand, "BpInMm", DbType.Double, details.BpInMm);
                this.dbServer.AddInParameter(storedProcCommand, "BpInHg", DbType.Double, details.BpInHg);
                this.dbServer.AddInParameter(storedProcCommand, "Built", DbType.String, details.Built);
                this.dbServer.AddInParameter(storedProcCommand, "Pulse", DbType.Double, details.Pulse);
                this.dbServer.AddInParameter(storedProcCommand, "Thyroid", DbType.String, details.Thyroid);
                this.dbServer.AddInParameter(storedProcCommand, "Acne", DbType.String, details.Acne);
                this.dbServer.AddInParameter(storedProcCommand, "Hirsutism", DbType.String, details.Hirsutism);
                this.dbServer.AddInParameter(storedProcCommand, "HIVPositive", DbType.Boolean, details.HIVPositive);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicalBuilt", DbType.String, details.PhysicalBuilt);
                this.dbServer.AddInParameter(storedProcCommand, "ExternalGenitalExam", DbType.String, details.ExternalGenitalExam);
                this.dbServer.AddInParameter(storedProcCommand, "Comments", DbType.String, details.Comments);
                this.dbServer.AddInParameter(storedProcCommand, "Alterts", DbType.String, details.Alterts);
                this.dbServer.AddInParameter(storedProcCommand, "BuiltID", DbType.Boolean, details.BuiltID);
                this.dbServer.AddInParameter(storedProcCommand, "Acen1", DbType.Boolean, details.Acen1);
                this.dbServer.AddInParameter(storedProcCommand, "Hirsutism1", DbType.Boolean, details.Hirsutism1);
                this.dbServer.AddInParameter(storedProcCommand, "ThyroidExam", DbType.String, details.ThyroidExam);
                this.dbServer.AddInParameter(storedProcCommand, "BreastExam", DbType.String, details.BreastExam);
                this.dbServer.AddInParameter(storedProcCommand, "AbdominalExam", DbType.String, details.AbdominalExam);
                this.dbServer.AddInParameter(storedProcCommand, "PelvicExam", DbType.String, details.PelvicExam);
                this.dbServer.AddInParameter(storedProcCommand, "HBVPositive", DbType.Boolean, details.HBVPositive);
                this.dbServer.AddInParameter(storedProcCommand, "HCVPositive", DbType.Boolean, details.HCVPositive);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddFemaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddFamaleHistoryBizActionVO bizActionObj = valueObject as clsAddFamaleHistoryBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddFemaleHistory(bizActionObj, UserVo);
            }
            return valueObject;
        }

        private clsAddFamaleHistoryBizActionVO AddFemaleHistory(clsAddFamaleHistoryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsFemaleHistoryVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "MarriedSinceYears", DbType.Double, details.MarriedSinceYears);
                this.dbServer.AddInParameter(storedProcCommand, "MarriedSinceMonths", DbType.Double, details.MarriedSinceMonths);
                this.dbServer.AddInParameter(storedProcCommand, "Menarche", DbType.String, details.Menarche);
                this.dbServer.AddInParameter(storedProcCommand, "DurationofRelationship", DbType.Double, details.DurationofRelationship);
                this.dbServer.AddInParameter(storedProcCommand, "ContraceptionUsed", DbType.Boolean, details.ContraceptionUsed);
                this.dbServer.AddInParameter(storedProcCommand, "DurationofContraceptionUsed", DbType.Double, details.DurationofContraceptionUsed);
                this.dbServer.AddInParameter(storedProcCommand, "MethodOfContraception", DbType.String, details.MethodOfContraception);
                this.dbServer.AddInParameter(storedProcCommand, "InfertilitySinceYears", DbType.Double, details.InfertilitySinceYears);
                this.dbServer.AddInParameter(storedProcCommand, "InfertilityType", DbType.Int16, (short) details.InfertilityType);
                this.dbServer.AddInParameter(storedProcCommand, "FemaleInfertility", DbType.Boolean, details.FemaleInfertility);
                this.dbServer.AddInParameter(storedProcCommand, "MaleInfertility", DbType.Boolean, details.MaleInfertility);
                this.dbServer.AddInParameter(storedProcCommand, "Couple", DbType.String, details.Couple);
                this.dbServer.AddInParameter(storedProcCommand, "MedicationTakenforInfertility", DbType.String, details.MedicationTakenforInfertility);
                this.dbServer.AddInParameter(storedProcCommand, "SexualDisfunction", DbType.Boolean, details.SexualDisfunction);
                this.dbServer.AddInParameter(storedProcCommand, "SexualDisfunctionRamarks", DbType.String, details.SexualDisfunctionRamarks);
                this.dbServer.AddInParameter(storedProcCommand, "LMP", DbType.DateTime, details.LMP);
                this.dbServer.AddInParameter(storedProcCommand, "Regular", DbType.Boolean, details.Regular);
                this.dbServer.AddInParameter(storedProcCommand, "Length", DbType.Double, details.Length);
                this.dbServer.AddInParameter(storedProcCommand, "DurationOfPeriod", DbType.String, details.DurationOfPeriod);
                this.dbServer.AddInParameter(storedProcCommand, "BloodLoss", DbType.Int16, (short) details.BloodLoss);
                this.dbServer.AddInParameter(storedProcCommand, "Dymenorhea", DbType.Boolean, details.Dymenorhea);
                this.dbServer.AddInParameter(storedProcCommand, "MidCyclePain", DbType.Boolean, details.MidCyclePain);
                this.dbServer.AddInParameter(storedProcCommand, "InterMenstrualBleeding", DbType.Boolean, details.InterMenstrualBleeding);
                this.dbServer.AddInParameter(storedProcCommand, "PreMenstrualTension", DbType.Boolean, details.PreMenstrualTension);
                this.dbServer.AddInParameter(storedProcCommand, "Dysparuneia", DbType.Boolean, details.Dysparuneia);
                this.dbServer.AddInParameter(storedProcCommand, "PostCoitalBleeding", DbType.Boolean, details.PostCoitalBleeding);
                this.dbServer.AddInParameter(storedProcCommand, "DetailsOfPastCycles", DbType.String, details.DetailsOfPastCycles);
                this.dbServer.AddInParameter(storedProcCommand, "ObstHistoryComplications", DbType.String, details.ObstHistoryComplications);
                this.dbServer.AddInParameter(storedProcCommand, "Surgeries", DbType.String, details.Surgeries);
                this.dbServer.AddInParameter(storedProcCommand, "PreviousIUI", DbType.Boolean, details.PreviousIUI);
                this.dbServer.AddInParameter(storedProcCommand, "IUINoOfTimes", DbType.Double, details.IUINoOfTimes);
                this.dbServer.AddInParameter(storedProcCommand, "IUIPlace", DbType.String, details.IUIPlace);
                this.dbServer.AddInParameter(storedProcCommand, "IUISuccessfull", DbType.Boolean, details.IUISuccessfull);
                this.dbServer.AddInParameter(storedProcCommand, "PreviousIVF", DbType.Boolean, details.PreviousIVF);
                this.dbServer.AddInParameter(storedProcCommand, "IVFNoOfTimes", DbType.Double, details.IVFNoOfTimes);
                this.dbServer.AddInParameter(storedProcCommand, "IVFPlace", DbType.String, details.IVFPlace);
                this.dbServer.AddInParameter(storedProcCommand, "IVFSuccessfull", DbType.Boolean, details.IVFSuccessfull);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialNotes", DbType.String, details.SpecialNotes);
                this.dbServer.AddInParameter(storedProcCommand, "Illness", DbType.String, details.Illness);
                this.dbServer.AddInParameter(storedProcCommand, "Allergies", DbType.String, details.Allergies);
                this.dbServer.AddInParameter(storedProcCommand, "FamilySocialHistory", DbType.String, details.FamilySocialHistory);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGeneralExaminationFemaleBizActionVO bizActionObj = valueObject as clsAddGeneralExaminationFemaleBizActionVO;
            if (bizActionObj.Details.ID == 0L)
            {
                bizActionObj = this.AddFemaleGeneralDetails(bizActionObj, UserVo);
            }
            return valueObject;
        }

        public override IValueObject AddGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGeneralExaminationForMaleBizActionVO bizActionObj = valueObject as clsAddGeneralExaminationForMaleBizActionVO;
            if (bizActionObj.GeneralDetails.ID == 0L)
            {
                bizActionObj = this.AddMaleDetails(bizActionObj, UserVo);
            }
            return valueObject;
        }

        private clsAddGeneralExaminationForMaleBizActionVO AddMaleDetails(clsAddGeneralExaminationForMaleBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsGeneralExaminationVO generalDetails = BizActionObj.GeneralDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddGeneralExaminationForMale");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, generalDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, generalDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Weight", DbType.Double, generalDetails.Weight);
                this.dbServer.AddInParameter(storedProcCommand, "Height", DbType.Double, generalDetails.Height);
                this.dbServer.AddInParameter(storedProcCommand, "BMI", DbType.Double, generalDetails.BMI);
                this.dbServer.AddInParameter(storedProcCommand, "BpInMm", DbType.Double, generalDetails.BpInMm);
                this.dbServer.AddInParameter(storedProcCommand, "BpInHg", DbType.Double, generalDetails.BpInHg);
                this.dbServer.AddInParameter(storedProcCommand, "Built", DbType.String, generalDetails.Built);
                this.dbServer.AddInParameter(storedProcCommand, "Pulse", DbType.Double, generalDetails.Pulse);
                this.dbServer.AddInParameter(storedProcCommand, "Fat", DbType.String, generalDetails.Fat);
                this.dbServer.AddInParameter(storedProcCommand, "PA", DbType.String, generalDetails.PA);
                this.dbServer.AddInParameter(storedProcCommand, "RS", DbType.String, generalDetails.RS);
                this.dbServer.AddInParameter(storedProcCommand, "CVS", DbType.String, generalDetails.CVS);
                this.dbServer.AddInParameter(storedProcCommand, "CNS", DbType.String, generalDetails.CNS);
                this.dbServer.AddInParameter(storedProcCommand, "Thyroid", DbType.String, generalDetails.Thyroid);
                this.dbServer.AddInParameter(storedProcCommand, "Gynaecomastia", DbType.String, generalDetails.Gynaecomastia);
                this.dbServer.AddInParameter(storedProcCommand, "SecondarySexCharacters", DbType.String, generalDetails.SecondarySexCharacters);
                this.dbServer.AddInParameter(storedProcCommand, "HIVPositive", DbType.Boolean, generalDetails.HIVPositive);
                this.dbServer.AddInParameter(storedProcCommand, "EyeColor", DbType.String, generalDetails.EyeColor);
                this.dbServer.AddInParameter(storedProcCommand, "HairColor", DbType.String, generalDetails.HairColor);
                this.dbServer.AddInParameter(storedProcCommand, "SkinColor", DbType.String, generalDetails.SkinColor);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicalBuilt", DbType.String, generalDetails.PhysicalBuilt);
                this.dbServer.AddInParameter(storedProcCommand, "ExternalGenitalExam", DbType.String, generalDetails.ExternalGenitalExam);
                this.dbServer.AddInParameter(storedProcCommand, "Comments", DbType.String, generalDetails.Comments);
                this.dbServer.AddInParameter(storedProcCommand, "Alterts", DbType.String, generalDetails.Alterts);
                this.dbServer.AddInParameter(storedProcCommand, "BuiltID", DbType.Boolean, generalDetails.BuiltID);
                this.dbServer.AddInParameter(storedProcCommand, "SkinColor1", DbType.Int64, generalDetails.SkinColor1);
                this.dbServer.AddInParameter(storedProcCommand, "EyeColor1", DbType.Int64, generalDetails.EyeColor1);
                this.dbServer.AddInParameter(storedProcCommand, "HairColor1", DbType.Int64, generalDetails.HairColor1);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, generalDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.GeneralDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddMaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMaleHistoryBizActionVO bizActionObj = valueObject as clsAddMaleHistoryBizActionVO;
            bizActionObj = (bizActionObj.HistoryDetails.ID != 0L) ? this.UpdateMaleHistory(bizActionObj, UserVo) : this.AddMaleHistoryDetails(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddMaleHistoryBizActionVO AddMaleHistoryDetails(clsAddMaleHistoryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsMaleHistoryVO historyDetails = BizActionObj.HistoryDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddMaleHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, historyDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "Medical", DbType.String, historyDetails.Medical);
                this.dbServer.AddInParameter(storedProcCommand, "Surgical", DbType.String, historyDetails.Surgical);
                this.dbServer.AddInParameter(storedProcCommand, "Family", DbType.String, historyDetails.Family);
                this.dbServer.AddInParameter(storedProcCommand, "Complication", DbType.String, historyDetails.Complication);
                this.dbServer.AddInParameter(storedProcCommand, "Allergies", DbType.String, historyDetails.Allergies);
                this.dbServer.AddInParameter(storedProcCommand, "Undergarments", DbType.String, historyDetails.Undergarments);
                this.dbServer.AddInParameter(storedProcCommand, "Medications", DbType.String, historyDetails.Medications);
                this.dbServer.AddInParameter(storedProcCommand, "ExposerToHeat", DbType.Boolean, historyDetails.ExposerToHeat);
                this.dbServer.AddInParameter(storedProcCommand, "ExposerToHeatDetails", DbType.String, historyDetails.ExposerToHeatDetails);
                this.dbServer.AddInParameter(storedProcCommand, "Smoking", DbType.Boolean, historyDetails.Smoking);
                this.dbServer.AddInParameter(storedProcCommand, "SmokingHabitual", DbType.Boolean, historyDetails.SmokingHabitual);
                this.dbServer.AddInParameter(storedProcCommand, "Alcohol", DbType.Boolean, historyDetails.Alcohol);
                this.dbServer.AddInParameter(storedProcCommand, "AlcoholHabitual", DbType.Boolean, historyDetails.AlcoholHabitual);
                this.dbServer.AddInParameter(storedProcCommand, "AnyOther", DbType.Boolean, historyDetails.AnyOther);
                this.dbServer.AddInParameter(storedProcCommand, "AnyOtherDetails", DbType.String, historyDetails.AnyOtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "OtherHabitual", DbType.Boolean, historyDetails.OtherHabitual);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, historyDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.HistoryDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetClinicalSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetClinicalSummaryBizActionVO nvo = valueObject as clsGetClinicalSummaryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetClinicalSummary");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.String, nvo.Details.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsClinicalSummaryVO>();
                    }
                    while (reader.Read())
                    {
                        clsClinicalSummaryVO item = new clsClinicalSummaryVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            SummaryID = (long) DALHelper.HandleDBNull(reader["SummaryID"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.DetailsList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetFemaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFemaleHistoryBizActionVO nvo = valueObject as clsGetFemaleHistoryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleHistory");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsFemaleHistoryVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleHistoryVO item = new clsFemaleHistoryVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            MarriedSinceYears = (double) DALHelper.HandleDBNull(reader["MarriedSinceYears"]),
                            MarriedSinceMonths = (double) DALHelper.HandleDBNull(reader["MarriedSinceMonths"]),
                            Menarche = (string) DALHelper.HandleDBNull(reader["Menarche"]),
                            DurationofRelationship = (double) DALHelper.HandleDBNull(reader["DurationofRelationship"]),
                            ContraceptionUsed = (bool) DALHelper.HandleDBNull(reader["ContraceptionUsed"]),
                            DurationofContraceptionUsed = (double) DALHelper.HandleDBNull(reader["DurationofContraceptionUsed"]),
                            MethodOfContraception = (string) DALHelper.HandleDBNull(reader["MethodOfContraception"]),
                            InfertilitySinceYears = (double) DALHelper.HandleDBNull(reader["InfertilitySinceYears"]),
                            InfertilityType = (IVFInfertilityTypes) ((short) DALHelper.HandleDBNull(reader["InfertilityType"])),
                            FemaleInfertility = (bool) DALHelper.HandleDBNull(reader["FemaleInfertility"]),
                            MaleInfertility = (bool) DALHelper.HandleDBNull(reader["MaleInfertility"]),
                            Couple = (string) DALHelper.HandleDBNull(reader["Couple"]),
                            MedicationTakenforInfertility = (string) DALHelper.HandleDBNull(reader["MedicationTakenforInfertility"]),
                            SexualDisfunction = (bool) DALHelper.HandleDBNull(reader["SexualDisfunction"]),
                            SexualDisfunctionRamarks = (string) DALHelper.HandleDBNull(reader["SexualDisfunctionRamarks"]),
                            LMP = DALHelper.HandleDate(reader["LMP"]),
                            Regular = (bool) DALHelper.HandleDBNull(reader["Regular"]),
                            Length = (double) DALHelper.HandleDBNull(reader["Length"]),
                            DurationOfPeriod = (string) DALHelper.HandleDBNull(reader["DurationOfPeriod"]),
                            BloodLoss = (IVFBloodLoss) ((short) DALHelper.HandleDBNull(reader["BloodLoss"])),
                            Dymenorhea = (bool) DALHelper.HandleDBNull(reader["Dymenorhea"]),
                            MidCyclePain = (bool) DALHelper.HandleDBNull(reader["MidCyclePain"]),
                            InterMenstrualBleeding = (bool) DALHelper.HandleDBNull(reader["InterMenstrualBleeding"]),
                            PreMenstrualTension = (bool) DALHelper.HandleDBNull(reader["PreMenstrualTension"]),
                            Dysparuneia = (bool) DALHelper.HandleDBNull(reader["Dysparuneia"]),
                            PostCoitalBleeding = (bool) DALHelper.HandleDBNull(reader["PostCoitalBleeding"]),
                            DetailsOfPastCycles = (string) DALHelper.HandleDBNull(reader["DetailsOfPastCycles"]),
                            ObstHistoryComplications = (string) DALHelper.HandleDBNull(reader["ObstHistoryComplications"]),
                            Surgeries = (string) DALHelper.HandleDBNull(reader["Surgeries"]),
                            PreviousIUI = (bool) DALHelper.HandleDBNull(reader["PreviousIUI"]),
                            IUINoOfTimes = (double) DALHelper.HandleDBNull(reader["IUINoOfTimes"]),
                            IUIPlace = (string) DALHelper.HandleDBNull(reader["IUIPlace"]),
                            IUISuccessfull = (bool) DALHelper.HandleDBNull(reader["IUISuccessfull"]),
                            PreviousIVF = (bool) DALHelper.HandleDBNull(reader["PreviousIVF"]),
                            IVFNoOfTimes = (double) DALHelper.HandleDBNull(reader["IVFNoOfTimes"]),
                            IVFPlace = (string) DALHelper.HandleDBNull(reader["IVFPlace"]),
                            IVFSuccessfull = (bool) DALHelper.HandleDBNull(reader["IVFSuccessfull"]),
                            SpecialNotes = (string) DALHelper.HandleDBNull(reader["SpecialNotes"]),
                            Illness = (string) DALHelper.HandleDBNull(reader["Illness"]),
                            Allergies = (string) DALHelper.HandleDBNull(reader["Allergies"]),
                            FamilySocialHistory = (string) DALHelper.HandleDBNull(reader["FamilySocialHistory"]),
                            AddedDateTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["AddedDateTime"]))
                        };
                        nvo.Details.Add(item);
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
            return valueObject;
        }

        public override IValueObject GetGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGeneralExaminationFemaleBizActionVO nvo = valueObject as clsGetGeneralExaminationFemaleBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGeneralExaminationForFemale");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsGeneralExaminationVO>();
                    }
                    while (reader.Read())
                    {
                        clsGeneralExaminationVO item = new clsGeneralExaminationVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            Weight = (float) ((double) DALHelper.HandleDBNull(reader["Weight"])),
                            Height = (float) ((double) DALHelper.HandleDBNull(reader["Height"])),
                            BMI = (float) ((double) DALHelper.HandleDBNull(reader["BMI"])),
                            BpInMm = (float) ((double) DALHelper.HandleDBNull(reader["BpInMm"])),
                            BpInHg = (float) ((double) DALHelper.HandleDBNull(reader["BpInHg"])),
                            Built = (string) DALHelper.HandleDBNull(reader["Built"]),
                            Pulse = (float) ((double) DALHelper.HandleDBNull(reader["Pulse"])),
                            Thyroid = (string) DALHelper.HandleDBNull(reader["Thyroid"]),
                            Acne = (string) DALHelper.HandleDBNull(reader["Acne"]),
                            Hirsutism = (string) DALHelper.HandleDBNull(reader["Hirsutism"]),
                            HIVPositive = (bool) DALHelper.HandleDBNull(reader["HIVPositive"]),
                            PhysicalBuilt = (string) DALHelper.HandleDBNull(reader["PhysicalBuilt"]),
                            ExternalGenitalExam = (string) DALHelper.HandleDBNull(reader["ExternalGenitalExam"]),
                            Comments = (string) DALHelper.HandleDBNull(reader["Comments"]),
                            Acen1 = (bool) DALHelper.HandleDBNull(reader["Acen1"]),
                            Hirsutism1 = (bool) DALHelper.HandleDBNull(reader["Hirsutism1"]),
                            ThyroidExam = (string) DALHelper.HandleDBNull(reader["ThyroidExam"]),
                            BreastExam = (string) DALHelper.HandleDBNull(reader["BreastExam"]),
                            AbdominalExam = (string) DALHelper.HandleDBNull(reader["AbdominalExam"]),
                            PelvicExam = (string) DALHelper.HandleDBNull(reader["PelvicExam"]),
                            HBVPositive = (bool) DALHelper.HandleDBNull(reader["HBVPositive"]),
                            HCVPositive = (bool) DALHelper.HandleDBNull(reader["HCVPositive"]),
                            BuiltID = (long) DALHelper.HandleDBNull(reader["BuiltID"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                            Alterts = (string) DALHelper.HandleDBNull(reader["Alterts"]),
                            Date = DALHelper.HandleDate(reader["Date"])
                        };
                        nvo.List.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGeneralExaminationForMaleBizActionVO nvo = valueObject as clsGetGeneralExaminationForMaleBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGeneralExaminationForMale");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GeneralDetails == null)
                    {
                        nvo.GeneralDetails = new List<clsGeneralExaminationVO>();
                    }
                    while (reader.Read())
                    {
                        clsGeneralExaminationVO item = new clsGeneralExaminationVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            Weight = (float) ((double) DALHelper.HandleDBNull(reader["Weight"])),
                            Height = (float) ((double) DALHelper.HandleDBNull(reader["Height"])),
                            BMI = (float) ((double) DALHelper.HandleDBNull(reader["BMI"])),
                            BpInMm = (float) ((double) DALHelper.HandleDBNull(reader["BpInMm"])),
                            BpInHg = (float) ((double) DALHelper.HandleDBNull(reader["BpInHg"])),
                            Built = (string) DALHelper.HandleDBNull(reader["Built"]),
                            Pulse = (float) ((double) DALHelper.HandleDBNull(reader["Pulse"])),
                            Fat = (string) DALHelper.HandleDBNull(reader["Fat"]),
                            PA = (string) DALHelper.HandleDBNull(reader["PA"]),
                            RS = (string) DALHelper.HandleDBNull(reader["RS"]),
                            CVS = (string) DALHelper.HandleDBNull(reader["CVS"]),
                            CNS = (string) DALHelper.HandleDBNull(reader["CNS"]),
                            Thyroid = (string) DALHelper.HandleDBNull(reader["Thyroid"]),
                            Gynaecomastia = (string) DALHelper.HandleDBNull(reader["Gynaecomastia"]),
                            SecondarySexCharacters = (string) DALHelper.HandleDBNull(reader["SecondarySexCharacters"]),
                            HIVPositive = (bool) DALHelper.HandleDBNull(reader["HIVPositive"]),
                            EyeColor = (string) DALHelper.HandleDBNull(reader["EyeColor"]),
                            HairColor = (string) DALHelper.HandleDBNull(reader["HairColor"]),
                            SkinColor = (string) DALHelper.HandleDBNull(reader["SkinColor"]),
                            PhysicalBuilt = (string) DALHelper.HandleDBNull(reader["PhysicalBuilt"]),
                            ExternalGenitalExam = (string) DALHelper.HandleDBNull(reader["ExternalGenitalExam"]),
                            Comments = (string) DALHelper.HandleDBNull(reader["Comments"]),
                            HairColor1 = (long) DALHelper.HandleDBNull(reader["HairColorID"]),
                            SkinColor1 = (long) DALHelper.HandleDBNull(reader["SkinColorID"]),
                            EyeColor1 = (long) DALHelper.HandleDBNull(reader["EyeColorID"]),
                            BuiltID = (long) DALHelper.HandleDBNull(reader["BuiltID"]),
                            Alterts = (string) DALHelper.HandleDBNull(reader["Alterts"]),
                            Date = (DateTime?) DALHelper.HandleDBNull(reader["Date"]),
                            UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"])
                        };
                        nvo.GeneralDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetMaleHistory(IValueObject valueobject, clsUserVO UserVO)
        {
            clsGetMaleHistoryBizActionVO nvo = valueobject as clsGetMaleHistoryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetMaleHistory");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Details == null)
                    {
                        nvo.Details = new List<clsMaleHistoryVO>();
                    }
                    while (reader.Read())
                    {
                        clsMaleHistoryVO item = new clsMaleHistoryVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            Medical = (string) DALHelper.HandleDBNull(reader["Medical"]),
                            Surgical = (string) DALHelper.HandleDBNull(reader["Surgical"]),
                            Family = (string) DALHelper.HandleDBNull(reader["Family"]),
                            Complication = (string) DALHelper.HandleDBNull(reader["Complication"]),
                            Allergies = (string) DALHelper.HandleDBNull(reader["Allergies"]),
                            Undergarments = (string) DALHelper.HandleDBNull(reader["Undergarments"]),
                            Medications = (string) DALHelper.HandleDBNull(reader["Medications"]),
                            ExposerToHeat = (bool) DALHelper.HandleDBNull(reader["ExposerToHeat"]),
                            ExposerToHeatDetails = (string) DALHelper.HandleDBNull(reader["ExposerToHeatDetails"]),
                            Smoking = (bool) DALHelper.HandleDBNull(reader["Smoking"]),
                            SmokingHabitual = (bool) DALHelper.HandleDBNull(reader["SmokingHabitual"]),
                            Alcohol = (bool) DALHelper.HandleDBNull(reader["Alcohol"]),
                            AlcoholHabitual = (bool) DALHelper.HandleDBNull(reader["AlcoholHabitual"]),
                            AnyOther = (bool) DALHelper.HandleDBNull(reader["AnyOther"]),
                            AnyOtherDetails = (string) DALHelper.HandleDBNull(reader["AnyOtherDetails"]),
                            OtherHabitual = (bool) DALHelper.HandleDBNull(reader["OtherHabitual"]),
                            AddedDateTime = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["AddedDateTime"]))
                        };
                        nvo.Details.Add(item);
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

        public override IValueObject UpdateFemaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateFemaleHistoryBizActionVO nvo = valueObject as clsUpdateFemaleHistoryBizActionVO;
            try
            {
                clsFemaleHistoryVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_UpdateFemaleHistory");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "MarriedSinceYears", DbType.Double, details.MarriedSinceYears);
                this.dbServer.AddInParameter(storedProcCommand, "MarriedSinceMonths", DbType.Double, details.MarriedSinceMonths);
                this.dbServer.AddInParameter(storedProcCommand, "Menarche", DbType.String, details.Menarche);
                this.dbServer.AddInParameter(storedProcCommand, "DurationofRelationship", DbType.Double, details.DurationofRelationship);
                this.dbServer.AddInParameter(storedProcCommand, "ContraceptionUsed", DbType.Boolean, details.ContraceptionUsed);
                this.dbServer.AddInParameter(storedProcCommand, "DurationofContraceptionUsed", DbType.Double, details.DurationofContraceptionUsed);
                this.dbServer.AddInParameter(storedProcCommand, "MethodOfContraception", DbType.String, details.MethodOfContraception);
                this.dbServer.AddInParameter(storedProcCommand, "InfertilitySinceYears", DbType.Double, details.InfertilitySinceYears);
                this.dbServer.AddInParameter(storedProcCommand, "InfertilityType", DbType.Int16, (short) details.InfertilityType);
                this.dbServer.AddInParameter(storedProcCommand, "FemaleInfertility", DbType.Boolean, details.FemaleInfertility);
                this.dbServer.AddInParameter(storedProcCommand, "MaleInfertility", DbType.Boolean, details.MaleInfertility);
                this.dbServer.AddInParameter(storedProcCommand, "Couple", DbType.String, details.Couple);
                this.dbServer.AddInParameter(storedProcCommand, "MedicationTakenforInfertility", DbType.String, details.MedicationTakenforInfertility);
                this.dbServer.AddInParameter(storedProcCommand, "SexualDisfunction", DbType.Boolean, details.SexualDisfunction);
                this.dbServer.AddInParameter(storedProcCommand, "SexualDisfunctionRamarks", DbType.String, details.SexualDisfunctionRamarks);
                this.dbServer.AddInParameter(storedProcCommand, "LMP", DbType.DateTime, details.LMP);
                this.dbServer.AddInParameter(storedProcCommand, "Regular", DbType.Boolean, details.Regular);
                this.dbServer.AddInParameter(storedProcCommand, "Length", DbType.Double, details.Length);
                this.dbServer.AddInParameter(storedProcCommand, "DurationOfPeriod", DbType.String, details.DurationOfPeriod);
                this.dbServer.AddInParameter(storedProcCommand, "BloodLoss", DbType.Int16, (short) details.BloodLoss);
                this.dbServer.AddInParameter(storedProcCommand, "Dymenorhea", DbType.Boolean, details.Dymenorhea);
                this.dbServer.AddInParameter(storedProcCommand, "MidCyclePain", DbType.Boolean, details.MidCyclePain);
                this.dbServer.AddInParameter(storedProcCommand, "InterMenstrualBleeding", DbType.Boolean, details.InterMenstrualBleeding);
                this.dbServer.AddInParameter(storedProcCommand, "PreMenstrualTension", DbType.Boolean, details.PreMenstrualTension);
                this.dbServer.AddInParameter(storedProcCommand, "Dysparuneia", DbType.Boolean, details.Dysparuneia);
                this.dbServer.AddInParameter(storedProcCommand, "PostCoitalBleeding", DbType.Boolean, details.PostCoitalBleeding);
                this.dbServer.AddInParameter(storedProcCommand, "DetailsOfPastCycles", DbType.String, details.DetailsOfPastCycles);
                this.dbServer.AddInParameter(storedProcCommand, "ObstHistoryComplications", DbType.String, details.ObstHistoryComplications);
                this.dbServer.AddInParameter(storedProcCommand, "Surgeries", DbType.String, details.Surgeries);
                this.dbServer.AddInParameter(storedProcCommand, "PreviousIUI", DbType.Boolean, details.PreviousIUI);
                this.dbServer.AddInParameter(storedProcCommand, "IUINoOfTimes", DbType.Double, details.IUINoOfTimes);
                this.dbServer.AddInParameter(storedProcCommand, "IUIPlace", DbType.String, details.IUIPlace);
                this.dbServer.AddInParameter(storedProcCommand, "IUISuccessfull", DbType.Boolean, details.IUISuccessfull);
                this.dbServer.AddInParameter(storedProcCommand, "PreviousIVF", DbType.Boolean, details.PreviousIVF);
                this.dbServer.AddInParameter(storedProcCommand, "IVFNoOfTimes", DbType.Double, details.IVFNoOfTimes);
                this.dbServer.AddInParameter(storedProcCommand, "IVFPlace", DbType.String, details.IVFPlace);
                this.dbServer.AddInParameter(storedProcCommand, "IVFSuccessfull", DbType.Boolean, details.IVFSuccessfull);
                this.dbServer.AddInParameter(storedProcCommand, "SpecialNotes", DbType.String, details.SpecialNotes);
                this.dbServer.AddInParameter(storedProcCommand, "Illness", DbType.String, details.Illness);
                this.dbServer.AddInParameter(storedProcCommand, "Allergies", DbType.String, details.Allergies);
                this.dbServer.AddInParameter(storedProcCommand, "FamilySocialHistory", DbType.String, details.FamilySocialHistory);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsAddMaleHistoryBizActionVO UpdateMaleHistory(clsAddMaleHistoryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsMaleHistoryVO historyDetails = BizActionObj.HistoryDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_UpdateMaleHistory");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, historyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, historyDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "Medical", DbType.String, historyDetails.Medical);
                this.dbServer.AddInParameter(storedProcCommand, "Surgical", DbType.String, historyDetails.Surgical);
                this.dbServer.AddInParameter(storedProcCommand, "Family", DbType.String, historyDetails.Family);
                this.dbServer.AddInParameter(storedProcCommand, "Complication", DbType.String, historyDetails.Complication);
                this.dbServer.AddInParameter(storedProcCommand, "Allergies", DbType.String, historyDetails.Allergies);
                this.dbServer.AddInParameter(storedProcCommand, "Undergarments", DbType.String, historyDetails.Undergarments);
                this.dbServer.AddInParameter(storedProcCommand, "Medications", DbType.String, historyDetails.Medications);
                this.dbServer.AddInParameter(storedProcCommand, "ExposerToHeat", DbType.Boolean, historyDetails.ExposerToHeat);
                this.dbServer.AddInParameter(storedProcCommand, "ExposerToHeatDetails", DbType.String, historyDetails.ExposerToHeatDetails);
                this.dbServer.AddInParameter(storedProcCommand, "Smoking", DbType.Boolean, historyDetails.Smoking);
                this.dbServer.AddInParameter(storedProcCommand, "SmokingHabitual", DbType.Boolean, historyDetails.SmokingHabitual);
                this.dbServer.AddInParameter(storedProcCommand, "Alcohol", DbType.Boolean, historyDetails.Alcohol);
                this.dbServer.AddInParameter(storedProcCommand, "AlcoholHabitual", DbType.Boolean, historyDetails.AlcoholHabitual);
                this.dbServer.AddInParameter(storedProcCommand, "AnyOther", DbType.Boolean, historyDetails.AnyOther);
                this.dbServer.AddInParameter(storedProcCommand, "AnyOtherDetails", DbType.String, historyDetails.AnyOtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "OtherHabitual", DbType.Boolean, historyDetails.OtherHabitual);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

