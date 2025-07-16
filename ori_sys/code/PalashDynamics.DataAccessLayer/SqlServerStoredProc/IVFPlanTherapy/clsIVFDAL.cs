using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
 class clsIVFDAL : clsBaseIVFDAL
    {
     //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFDAL()
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


        public override IValueObject AddGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGeneralExaminationForMaleBizActionVO BizActionObj = valueObject as clsAddGeneralExaminationForMaleBizActionVO;

            if (BizActionObj.GeneralDetails.ID == 0)
                BizActionObj = AddMaleDetails(BizActionObj, UserVo);


            return valueObject;
        }
        private clsAddGeneralExaminationForMaleBizActionVO AddMaleDetails(clsAddGeneralExaminationForMaleBizActionVO BizActionObj, clsUserVO UserVo)
        {

            try
            {

                clsGeneralExaminationVO GeneralDetails = BizActionObj.GeneralDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddGeneralExaminationForMale");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, GeneralDetails.PatientUnitID); // BY BHSHAN
                dbServer.AddInParameter(command, "Weight", DbType.Double, GeneralDetails.Weight);
                dbServer.AddInParameter(command, "Height", DbType.Double, GeneralDetails.Height);
                dbServer.AddInParameter(command, "BMI", DbType.Double, GeneralDetails.BMI);
                dbServer.AddInParameter(command, "BpInMm", DbType.Double, GeneralDetails.BpInMm);
                dbServer.AddInParameter(command, "BpInHg", DbType.Double, GeneralDetails.BpInHg);
                dbServer.AddInParameter(command, "Built", DbType.String, GeneralDetails.Built);
                dbServer.AddInParameter(command, "Pulse", DbType.Double, GeneralDetails.Pulse);
                dbServer.AddInParameter(command, "Fat", DbType.String, GeneralDetails.Fat);
                dbServer.AddInParameter(command, "PA", DbType.String, GeneralDetails.PA);
                dbServer.AddInParameter(command, "RS", DbType.String, GeneralDetails.RS);
                dbServer.AddInParameter(command, "CVS", DbType.String, GeneralDetails.CVS);
                dbServer.AddInParameter(command, "CNS", DbType.String, GeneralDetails.CNS);
                dbServer.AddInParameter(command, "Thyroid", DbType.String, GeneralDetails.Thyroid);
                dbServer.AddInParameter(command, "Gynaecomastia", DbType.String, GeneralDetails.Gynaecomastia);
                dbServer.AddInParameter(command, "SecondarySexCharacters", DbType.String, GeneralDetails.SecondarySexCharacters);
                dbServer.AddInParameter(command, "HIVPositive", DbType.Boolean, GeneralDetails.HIVPositive);
                dbServer.AddInParameter(command, "EyeColor", DbType.String, GeneralDetails.EyeColor);
                dbServer.AddInParameter(command, "HairColor", DbType.String, GeneralDetails.HairColor);
                dbServer.AddInParameter(command, "SkinColor", DbType.String, GeneralDetails.SkinColor);
                dbServer.AddInParameter(command, "PhysicalBuilt", DbType.String, GeneralDetails.PhysicalBuilt);
                dbServer.AddInParameter(command, "ExternalGenitalExam", DbType.String, GeneralDetails.ExternalGenitalExam);
                dbServer.AddInParameter(command, "Comments", DbType.String, GeneralDetails.Comments);
                dbServer.AddInParameter(command, "Alterts", DbType.String, GeneralDetails.Alterts);
                // By Anjali............
                dbServer.AddInParameter(command, "BuiltID", DbType.Boolean, GeneralDetails.BuiltID);
                dbServer.AddInParameter(command, "SkinColor1", DbType.Int64, GeneralDetails.SkinColor1);
                dbServer.AddInParameter(command, "EyeColor1", DbType.Int64, GeneralDetails.EyeColor1);
                dbServer.AddInParameter(command, "HairColor1", DbType.Int64, GeneralDetails.HairColor1);

                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, GeneralDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.GeneralDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception)
            {
                throw;
            }

            return BizActionObj;

        }

        public override IValueObject AddGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGeneralExaminationFemaleBizActionVO BizActionObj = valueObject as clsAddGeneralExaminationFemaleBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddFemaleGeneralDetails(BizActionObj, UserVo);


            return valueObject;
        }

        private clsAddGeneralExaminationFemaleBizActionVO AddFemaleGeneralDetails(clsAddGeneralExaminationFemaleBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsGeneralExaminationVO GeneralDetails = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddGeneralExaminationForFemale");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, GeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, GeneralDetails.PatientUnitID);  // BY BHSHAN.........................
                dbServer.AddInParameter(command, "Date", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "Weight", DbType.Double, GeneralDetails.Weight);
                dbServer.AddInParameter(command, "Height", DbType.Double, GeneralDetails.Height);
                dbServer.AddInParameter(command, "BMI", DbType.Double, GeneralDetails.BMI);
                dbServer.AddInParameter(command, "BpInMm", DbType.Double, GeneralDetails.BpInMm);
                dbServer.AddInParameter(command, "BpInHg", DbType.Double, GeneralDetails.BpInHg);
                dbServer.AddInParameter(command, "Built", DbType.String, GeneralDetails.Built);
                dbServer.AddInParameter(command, "Pulse", DbType.Double, GeneralDetails.Pulse);
                //dbServer.AddInParameter(command, "PA", DbType.String, GeneralDetails.PA);
                //dbServer.AddInParameter(command, "RS", DbType.String, GeneralDetails.RS);
                //dbServer.AddInParameter(command, "CVS", DbType.String, GeneralDetails.CVS);
                //dbServer.AddInParameter(command, "CNS", DbType.String, GeneralDetails.CNS);
                dbServer.AddInParameter(command, "Thyroid", DbType.String, GeneralDetails.Thyroid);
                dbServer.AddInParameter(command, "Acne", DbType.String, GeneralDetails.Acne);
                dbServer.AddInParameter(command, "Hirsutism", DbType.String, GeneralDetails.Hirsutism);
                dbServer.AddInParameter(command, "HIVPositive", DbType.Boolean, GeneralDetails.HIVPositive);
                //dbServer.AddInParameter(command, "EyeColor", DbType.String, GeneralDetails.EyeColor);
                //dbServer.AddInParameter(command, "HairColor", DbType.String, GeneralDetails.HairColor);
                //dbServer.AddInParameter(command, "SkinColor", DbType.String, GeneralDetails.SkinColor);
                dbServer.AddInParameter(command, "PhysicalBuilt", DbType.String, GeneralDetails.PhysicalBuilt);
                dbServer.AddInParameter(command, "ExternalGenitalExam", DbType.String, GeneralDetails.ExternalGenitalExam);
                dbServer.AddInParameter(command, "Comments", DbType.String, GeneralDetails.Comments);
                dbServer.AddInParameter(command, "Alterts", DbType.String, GeneralDetails.Alterts);

                // By Anjali............
                dbServer.AddInParameter(command, "BuiltID", DbType.Boolean, GeneralDetails.BuiltID);
                dbServer.AddInParameter(command, "Acen1", DbType.Boolean, GeneralDetails.Acen1);
                dbServer.AddInParameter(command, "Hirsutism1", DbType.Boolean, GeneralDetails.Hirsutism1);
                //dbServer.AddInParameter(command, "SkinColor1", DbType.Int64, GeneralDetails.SkinColor1);
                //dbServer.AddInParameter(command, "EyeColor1", DbType.Int64, GeneralDetails.EyeColor1);
                //dbServer.AddInParameter(command, "HairColor1", DbType.Int64, GeneralDetails.HairColor1);
                dbServer.AddInParameter(command, "ThyroidExam", DbType.String, GeneralDetails.ThyroidExam);
                dbServer.AddInParameter(command, "BreastExam", DbType.String, GeneralDetails.BreastExam);
                dbServer.AddInParameter(command, "AbdominalExam", DbType.String, GeneralDetails.AbdominalExam);
                dbServer.AddInParameter(command, "PelvicExam", DbType.String, GeneralDetails.PelvicExam);
                dbServer.AddInParameter(command, "HBVPositive", DbType.Boolean, GeneralDetails.HBVPositive);
                dbServer.AddInParameter(command, "HCVPositive", DbType.Boolean, GeneralDetails.HCVPositive);

                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, GeneralDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;

        }

        public override IValueObject AddMaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMaleHistoryBizActionVO BizActionObj = valueObject as clsAddMaleHistoryBizActionVO;

            if (BizActionObj.HistoryDetails.ID == 0)
                BizActionObj = AddMaleHistoryDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdateMaleHistory(BizActionObj, UserVo);


            return valueObject;
        }

        private clsAddMaleHistoryBizActionVO AddMaleHistoryDetails(clsAddMaleHistoryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsMaleHistoryVO HistoryDetails = BizActionObj.HistoryDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddMaleHistory");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, HistoryDetails.PatientID);             
                dbServer.AddInParameter(command, "Medical", DbType.String, HistoryDetails.Medical);
                dbServer.AddInParameter(command, "Surgical", DbType.String, HistoryDetails.Surgical);
                dbServer.AddInParameter(command, "Family", DbType.String, HistoryDetails.Family);
                dbServer.AddInParameter(command, "Complication", DbType.String, HistoryDetails.Complication);
                dbServer.AddInParameter(command, "Allergies", DbType.String, HistoryDetails.Allergies);
                dbServer.AddInParameter(command, "Undergarments", DbType.String, HistoryDetails.Undergarments);
                dbServer.AddInParameter(command, "Medications", DbType.String, HistoryDetails.Medications);
                dbServer.AddInParameter(command, "ExposerToHeat", DbType.Boolean, HistoryDetails.ExposerToHeat);
                dbServer.AddInParameter(command, "ExposerToHeatDetails", DbType.String, HistoryDetails.ExposerToHeatDetails);
                dbServer.AddInParameter(command, "Smoking", DbType.Boolean, HistoryDetails.Smoking);
                dbServer.AddInParameter(command, "SmokingHabitual", DbType.Boolean, HistoryDetails.SmokingHabitual);
                dbServer.AddInParameter(command, "Alcohol", DbType.Boolean, HistoryDetails.Alcohol);
                dbServer.AddInParameter(command, "AlcoholHabitual", DbType.Boolean, HistoryDetails.AlcoholHabitual);
                dbServer.AddInParameter(command, "AnyOther", DbType.Boolean, HistoryDetails.AnyOther);
                dbServer.AddInParameter(command, "AnyOtherDetails", DbType.String, HistoryDetails.AnyOtherDetails);
                dbServer.AddInParameter(command, "OtherHabitual", DbType.Boolean, HistoryDetails.OtherHabitual);
                
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, HistoryDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.HistoryDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception)
            {
                throw;
            }

            return BizActionObj;

        }

        private clsAddMaleHistoryBizActionVO UpdateMaleHistory(clsAddMaleHistoryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsMaleHistoryVO HistoryDetails = BizActionObj.HistoryDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_UpdateMaleHistory");

                dbServer.AddInParameter(command, "ID", DbType.Int64, HistoryDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, HistoryDetails.PatientID);
                dbServer.AddInParameter(command, "Medical", DbType.String, HistoryDetails.Medical);
                dbServer.AddInParameter(command, "Surgical", DbType.String, HistoryDetails.Surgical);
                dbServer.AddInParameter(command, "Family", DbType.String, HistoryDetails.Family);
                dbServer.AddInParameter(command, "Complication", DbType.String, HistoryDetails.Complication);
                dbServer.AddInParameter(command, "Allergies", DbType.String, HistoryDetails.Allergies);
                dbServer.AddInParameter(command, "Undergarments", DbType.String, HistoryDetails.Undergarments);
                dbServer.AddInParameter(command, "Medications", DbType.String, HistoryDetails.Medications);
                dbServer.AddInParameter(command, "ExposerToHeat", DbType.Boolean, HistoryDetails.ExposerToHeat);
                dbServer.AddInParameter(command, "ExposerToHeatDetails", DbType.String, HistoryDetails.ExposerToHeatDetails);
                dbServer.AddInParameter(command, "Smoking", DbType.Boolean, HistoryDetails.Smoking);
                dbServer.AddInParameter(command, "SmokingHabitual", DbType.Boolean, HistoryDetails.SmokingHabitual);
                dbServer.AddInParameter(command, "Alcohol", DbType.Boolean, HistoryDetails.Alcohol);
                dbServer.AddInParameter(command, "AlcoholHabitual", DbType.Boolean, HistoryDetails.AlcoholHabitual);
                dbServer.AddInParameter(command, "AnyOther", DbType.Boolean, HistoryDetails.AnyOther);
                dbServer.AddInParameter(command, "AnyOtherDetails", DbType.String, HistoryDetails.AnyOtherDetails);
                dbServer.AddInParameter(command, "OtherHabitual", DbType.Boolean, HistoryDetails.OtherHabitual);

                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception)
            {
                throw;
            }

            return BizActionObj;

        }

        public override IValueObject AddFemaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddFamaleHistoryBizActionVO BizActionObj = valueObject as clsAddFamaleHistoryBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddFemaleHistory(BizActionObj, UserVo);


            return valueObject;
        }
     
       private clsAddFamaleHistoryBizActionVO AddFemaleHistory(clsAddFamaleHistoryBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsFemaleHistoryVO HistoryDetails = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleHistory");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, HistoryDetails.PatientID);
                dbServer.AddInParameter(command, "MarriedSinceYears", DbType.Double, HistoryDetails.MarriedSinceYears);
                dbServer.AddInParameter(command, "MarriedSinceMonths", DbType.Double, HistoryDetails.MarriedSinceMonths);
                dbServer.AddInParameter(command, "Menarche", DbType.String, HistoryDetails.Menarche);
                dbServer.AddInParameter(command, "DurationofRelationship", DbType.Double, HistoryDetails.DurationofRelationship);
                dbServer.AddInParameter(command, "ContraceptionUsed", DbType.Boolean, HistoryDetails.ContraceptionUsed);
                dbServer.AddInParameter(command, "DurationofContraceptionUsed", DbType.Double, HistoryDetails.DurationofContraceptionUsed);
                dbServer.AddInParameter(command, "MethodOfContraception", DbType.String, HistoryDetails.MethodOfContraception);
                dbServer.AddInParameter(command, "InfertilitySinceYears", DbType.Double, HistoryDetails.InfertilitySinceYears);
                dbServer.AddInParameter(command, "InfertilityType", DbType.Int16, (Int16)HistoryDetails.InfertilityType);
                dbServer.AddInParameter(command, "FemaleInfertility", DbType.Boolean, HistoryDetails.FemaleInfertility);
                dbServer.AddInParameter(command, "MaleInfertility", DbType.Boolean, HistoryDetails.MaleInfertility);
                dbServer.AddInParameter(command, "Couple", DbType.String, HistoryDetails.Couple);
                dbServer.AddInParameter(command, "MedicationTakenforInfertility", DbType.String, HistoryDetails.MedicationTakenforInfertility);
                dbServer.AddInParameter(command, "SexualDisfunction", DbType.Boolean, HistoryDetails.SexualDisfunction);
                dbServer.AddInParameter(command, "SexualDisfunctionRamarks", DbType.String, HistoryDetails.SexualDisfunctionRamarks);
                dbServer.AddInParameter(command, "LMP", DbType.DateTime, HistoryDetails.LMP);
                dbServer.AddInParameter(command, "Regular", DbType.Boolean, HistoryDetails.Regular);
                dbServer.AddInParameter(command, "Length", DbType.Double, HistoryDetails.Length);
                dbServer.AddInParameter(command, "DurationOfPeriod", DbType.String, HistoryDetails.DurationOfPeriod);
                dbServer.AddInParameter(command, "BloodLoss", DbType.Int16, (Int16)HistoryDetails.BloodLoss);
                dbServer.AddInParameter(command, "Dymenorhea", DbType.Boolean, HistoryDetails.Dymenorhea);
                dbServer.AddInParameter(command, "MidCyclePain", DbType.Boolean, HistoryDetails.MidCyclePain);
                dbServer.AddInParameter(command, "InterMenstrualBleeding", DbType.Boolean, HistoryDetails.InterMenstrualBleeding);
                dbServer.AddInParameter(command, "PreMenstrualTension", DbType.Boolean, HistoryDetails.PreMenstrualTension);
                dbServer.AddInParameter(command, "Dysparuneia", DbType.Boolean, HistoryDetails.Dysparuneia);
                dbServer.AddInParameter(command, "PostCoitalBleeding", DbType.Boolean, HistoryDetails.PostCoitalBleeding);
                dbServer.AddInParameter(command, "DetailsOfPastCycles", DbType.String, HistoryDetails.DetailsOfPastCycles);
                dbServer.AddInParameter(command, "ObstHistoryComplications", DbType.String, HistoryDetails.ObstHistoryComplications);
                dbServer.AddInParameter(command, "Surgeries", DbType.String, HistoryDetails.Surgeries);
                dbServer.AddInParameter(command, "PreviousIUI", DbType.Boolean, HistoryDetails.PreviousIUI);
                dbServer.AddInParameter(command, "IUINoOfTimes", DbType.Double, HistoryDetails.IUINoOfTimes);
                dbServer.AddInParameter(command, "IUIPlace", DbType.String, HistoryDetails.IUIPlace);
                dbServer.AddInParameter(command, "IUISuccessfull", DbType.Boolean, HistoryDetails.IUISuccessfull);
                dbServer.AddInParameter(command, "PreviousIVF", DbType.Boolean, HistoryDetails.PreviousIVF);
                dbServer.AddInParameter(command, "IVFNoOfTimes", DbType.Double, HistoryDetails.IVFNoOfTimes);
                dbServer.AddInParameter(command, "IVFPlace", DbType.String, HistoryDetails.IVFPlace);
                dbServer.AddInParameter(command, "IVFSuccessfull", DbType.Boolean, HistoryDetails.IVFSuccessfull);
                dbServer.AddInParameter(command, "SpecialNotes", DbType.String, HistoryDetails.SpecialNotes);
                dbServer.AddInParameter(command, "Illness", DbType.String, HistoryDetails.Illness);
                dbServer.AddInParameter(command, "Allergies", DbType.String, HistoryDetails.Allergies);
                dbServer.AddInParameter(command, "FamilySocialHistory", DbType.String, HistoryDetails.FamilySocialHistory);

                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, HistoryDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;

        }
      
       public override IValueObject GetFemaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFemaleHistoryBizActionVO BizActionObj = valueObject as clsGetFemaleHistoryBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleHistory");
                DbDataReader reader;
                                          
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);               
                //dbServer.AddInParameter(command, "UnitID", DbType.String, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsFemaleHistoryVO>();

                    while (reader.Read())
                    {
                        clsFemaleHistoryVO HistoryDetails = new clsFemaleHistoryVO();
                        
                        HistoryDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        HistoryDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        HistoryDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        HistoryDetails.MarriedSinceYears = (double)DALHelper.HandleDBNull(reader["MarriedSinceYears"]);
                        HistoryDetails.MarriedSinceMonths = (double)DALHelper.HandleDBNull(reader["MarriedSinceMonths"]);
                        HistoryDetails.Menarche = (string)DALHelper.HandleDBNull(reader["Menarche"]);
                        HistoryDetails.DurationofRelationship = (double)DALHelper.HandleDBNull(reader["DurationofRelationship"]);
                        HistoryDetails.ContraceptionUsed = (bool)DALHelper.HandleDBNull(reader["ContraceptionUsed"]);
                        HistoryDetails.DurationofContraceptionUsed = (double)DALHelper.HandleDBNull(reader["DurationofContraceptionUsed"]);
                        HistoryDetails.MethodOfContraception = (string)DALHelper.HandleDBNull(reader["MethodOfContraception"]);
                        HistoryDetails.InfertilitySinceYears = (double)DALHelper.HandleDBNull(reader["InfertilitySinceYears"]);
                        HistoryDetails.InfertilityType = (IVFInfertilityTypes)((Int16)DALHelper.HandleDBNull(reader["InfertilityType"]));
                        HistoryDetails.FemaleInfertility = (bool)DALHelper.HandleDBNull(reader["FemaleInfertility"]);
                        HistoryDetails.MaleInfertility = (bool)DALHelper.HandleDBNull(reader["MaleInfertility"]);
                        HistoryDetails.Couple = (string)DALHelper.HandleDBNull(reader["Couple"]);
                        HistoryDetails.MedicationTakenforInfertility = (string)DALHelper.HandleDBNull(reader["MedicationTakenforInfertility"]);
                        HistoryDetails.SexualDisfunction = (bool)DALHelper.HandleDBNull(reader["SexualDisfunction"]);
                        HistoryDetails.SexualDisfunctionRamarks = (string)DALHelper.HandleDBNull(reader["SexualDisfunctionRamarks"]);
                        HistoryDetails.LMP = (DateTime?)DALHelper.HandleDate(reader["LMP"]);
                        HistoryDetails.Regular = (bool)DALHelper.HandleDBNull(reader["Regular"]);
                        HistoryDetails.Length = (double)DALHelper.HandleDBNull(reader["Length"]);
                        HistoryDetails.DurationOfPeriod = (string)DALHelper.HandleDBNull(reader["DurationOfPeriod"]);
                        HistoryDetails.BloodLoss = (IVFBloodLoss) ((Int16)DALHelper.HandleDBNull(reader["BloodLoss"]));
                        HistoryDetails.Dymenorhea = (bool)DALHelper.HandleDBNull(reader["Dymenorhea"]);
                        HistoryDetails.MidCyclePain = (bool)DALHelper.HandleDBNull(reader["MidCyclePain"]);
                        HistoryDetails.InterMenstrualBleeding = (bool)DALHelper.HandleDBNull(reader["InterMenstrualBleeding"]);
                        HistoryDetails.PreMenstrualTension = (bool)DALHelper.HandleDBNull(reader["PreMenstrualTension"]);
                        HistoryDetails.Dysparuneia = (bool)DALHelper.HandleDBNull(reader["Dysparuneia"]);
                        HistoryDetails.PostCoitalBleeding = (bool)DALHelper.HandleDBNull(reader["PostCoitalBleeding"]);
                        HistoryDetails.DetailsOfPastCycles = (string)DALHelper.HandleDBNull(reader["DetailsOfPastCycles"]);
                        HistoryDetails.ObstHistoryComplications = (string)DALHelper.HandleDBNull(reader["ObstHistoryComplications"]);
                        HistoryDetails.Surgeries = (string)DALHelper.HandleDBNull(reader["Surgeries"]);
                        HistoryDetails.PreviousIUI = (bool)DALHelper.HandleDBNull(reader["PreviousIUI"]);
                        HistoryDetails.IUINoOfTimes = (double)DALHelper.HandleDBNull(reader["IUINoOfTimes"]);
                        HistoryDetails.IUIPlace = (string)DALHelper.HandleDBNull(reader["IUIPlace"]);
                        HistoryDetails.IUISuccessfull = (bool)DALHelper.HandleDBNull(reader["IUISuccessfull"]);
                        HistoryDetails.PreviousIVF = (bool)DALHelper.HandleDBNull(reader["PreviousIVF"]);
                        HistoryDetails.IVFNoOfTimes = (double)DALHelper.HandleDBNull(reader["IVFNoOfTimes"]);
                        HistoryDetails.IVFPlace = (string)DALHelper.HandleDBNull(reader["IVFPlace"]);
                        HistoryDetails.IVFSuccessfull = (bool)DALHelper.HandleDBNull(reader["IVFSuccessfull"]);
                        HistoryDetails.SpecialNotes = (string)DALHelper.HandleDBNull(reader["SpecialNotes"]);
                        HistoryDetails.Illness = (string)DALHelper.HandleDBNull(reader["Illness"]);
                        HistoryDetails.Allergies = (string)DALHelper.HandleDBNull(reader["Allergies"]);
                        HistoryDetails.FamilySocialHistory = (string)DALHelper.HandleDBNull(reader["FamilySocialHistory"]);
                        HistoryDetails.AddedDateTime = (DateTime)DALHelper.HandleDBNull(reader["AddedDateTime"]);

                        BizActionObj.Details.Add(HistoryDetails);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
               
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return valueObject;
        }

        public override IValueObject UpdateFemaleHistory(IValueObject valueObject, clsUserVO UserVo)
        {

            clsUpdateFemaleHistoryBizActionVO BizActionObj = valueObject as clsUpdateFemaleHistoryBizActionVO;

            try
            {
              clsFemaleHistoryVO HistoryDetails = BizActionObj.Details;
              DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_UpdateFemaleHistory");
              dbServer.AddInParameter(command, "ID", DbType.Int64, HistoryDetails.ID);
              dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
              dbServer.AddInParameter(command, "PatientID", DbType.Int64, HistoryDetails.PatientID);
              dbServer.AddInParameter(command, "MarriedSinceYears", DbType.Double, HistoryDetails.MarriedSinceYears);
              dbServer.AddInParameter(command, "MarriedSinceMonths", DbType.Double, HistoryDetails.MarriedSinceMonths);
              dbServer.AddInParameter(command, "Menarche", DbType.String, HistoryDetails.Menarche);
              dbServer.AddInParameter(command, "DurationofRelationship", DbType.Double, HistoryDetails.DurationofRelationship);
              dbServer.AddInParameter(command, "ContraceptionUsed", DbType.Boolean, HistoryDetails.ContraceptionUsed);
              dbServer.AddInParameter(command, "DurationofContraceptionUsed", DbType.Double, HistoryDetails.DurationofContraceptionUsed);
              dbServer.AddInParameter(command, "MethodOfContraception", DbType.String, HistoryDetails.MethodOfContraception);
              dbServer.AddInParameter(command, "InfertilitySinceYears", DbType.Double, HistoryDetails.InfertilitySinceYears);
              dbServer.AddInParameter(command, "InfertilityType", DbType.Int16, (Int16)HistoryDetails.InfertilityType);
              dbServer.AddInParameter(command, "FemaleInfertility", DbType.Boolean, HistoryDetails.FemaleInfertility);
              dbServer.AddInParameter(command, "MaleInfertility", DbType.Boolean, HistoryDetails.MaleInfertility);
              dbServer.AddInParameter(command, "Couple", DbType.String, HistoryDetails.Couple);
              dbServer.AddInParameter(command, "MedicationTakenforInfertility", DbType.String, HistoryDetails.MedicationTakenforInfertility);
              dbServer.AddInParameter(command, "SexualDisfunction", DbType.Boolean, HistoryDetails.SexualDisfunction);
              dbServer.AddInParameter(command, "SexualDisfunctionRamarks", DbType.String, HistoryDetails.SexualDisfunctionRamarks);
              dbServer.AddInParameter(command, "LMP", DbType.DateTime, HistoryDetails.LMP);
              dbServer.AddInParameter(command, "Regular", DbType.Boolean, HistoryDetails.Regular);
              dbServer.AddInParameter(command, "Length", DbType.Double, HistoryDetails.Length);
              dbServer.AddInParameter(command, "DurationOfPeriod", DbType.String, HistoryDetails.DurationOfPeriod);
              dbServer.AddInParameter(command, "BloodLoss", DbType.Int16, (Int16)HistoryDetails.BloodLoss);
              dbServer.AddInParameter(command, "Dymenorhea", DbType.Boolean, HistoryDetails.Dymenorhea);
              dbServer.AddInParameter(command, "MidCyclePain", DbType.Boolean, HistoryDetails.MidCyclePain);
              dbServer.AddInParameter(command, "InterMenstrualBleeding", DbType.Boolean, HistoryDetails.InterMenstrualBleeding);
              dbServer.AddInParameter(command, "PreMenstrualTension", DbType.Boolean, HistoryDetails.PreMenstrualTension);
              dbServer.AddInParameter(command, "Dysparuneia", DbType.Boolean, HistoryDetails.Dysparuneia);
              dbServer.AddInParameter(command, "PostCoitalBleeding", DbType.Boolean, HistoryDetails.PostCoitalBleeding);
              dbServer.AddInParameter(command, "DetailsOfPastCycles", DbType.String, HistoryDetails.DetailsOfPastCycles);
              dbServer.AddInParameter(command, "ObstHistoryComplications", DbType.String, HistoryDetails.ObstHistoryComplications);
              dbServer.AddInParameter(command, "Surgeries", DbType.String, HistoryDetails.Surgeries);
              dbServer.AddInParameter(command, "PreviousIUI", DbType.Boolean, HistoryDetails.PreviousIUI);
              dbServer.AddInParameter(command, "IUINoOfTimes", DbType.Double, HistoryDetails.IUINoOfTimes);
              dbServer.AddInParameter(command, "IUIPlace", DbType.String, HistoryDetails.IUIPlace);
              dbServer.AddInParameter(command, "IUISuccessfull", DbType.Boolean, HistoryDetails.IUISuccessfull);
              dbServer.AddInParameter(command, "PreviousIVF", DbType.Boolean, HistoryDetails.PreviousIVF);
              dbServer.AddInParameter(command, "IVFNoOfTimes", DbType.Double, HistoryDetails.IVFNoOfTimes);
              dbServer.AddInParameter(command, "IVFPlace", DbType.String, HistoryDetails.IVFPlace);
              dbServer.AddInParameter(command, "IVFSuccessfull", DbType.Boolean, HistoryDetails.IVFSuccessfull);
              dbServer.AddInParameter(command, "SpecialNotes", DbType.String, HistoryDetails.SpecialNotes);
              dbServer.AddInParameter(command, "Illness", DbType.String, HistoryDetails.Illness);
              dbServer.AddInParameter(command, "Allergies", DbType.String, HistoryDetails.Allergies);
              dbServer.AddInParameter(command, "FamilySocialHistory", DbType.String, HistoryDetails.FamilySocialHistory);

              dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
              dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
              dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
              dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
              dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


              dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

              int intStatus = dbServer.ExecuteNonQuery(command);
              BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
     
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;
        }
        
        public override IValueObject GetGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGeneralExaminationForMaleBizActionVO BizActionObj=valueObject as clsGetGeneralExaminationForMaleBizActionVO;
            try
            {
             
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGeneralExaminationForMale");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);               

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.GeneralDetails == null)
                        BizActionObj.GeneralDetails=new List<clsGeneralExaminationVO>();
                    while(reader.Read())
                    {
                        clsGeneralExaminationVO Details=new clsGeneralExaminationVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        Details.Weight = (float)(double)DALHelper.HandleDBNull(reader["Weight"]);
                        Details.Height=(float)(double)DALHelper.HandleDBNull(reader["Height"]);
                        Details.BMI = (float)(double)DALHelper.HandleDBNull(reader["BMI"]);
                        Details.BpInMm = (float)(double)DALHelper.HandleDBNull(reader["BpInMm"]);
                        Details.BpInHg = (float)(double)DALHelper.HandleDBNull(reader["BpInHg"]);
                        Details.Built = (string)DALHelper.HandleDBNull(reader["Built"]);
                        Details.Pulse = (float)(double)DALHelper.HandleDBNull(reader["Pulse"]);
                        Details.Fat = (string)DALHelper.HandleDBNull(reader["Fat"]);
                        Details.PA = (string)DALHelper.HandleDBNull(reader["PA"]);
                        Details.RS = (string)DALHelper.HandleDBNull(reader["RS"]);
                        Details.CVS = (string)DALHelper.HandleDBNull(reader["CVS"]);
                        Details.CNS = (string)DALHelper.HandleDBNull(reader["CNS"]);
                        Details.Thyroid = (string)DALHelper.HandleDBNull(reader["Thyroid"]);
                        Details.Gynaecomastia = (string)DALHelper.HandleDBNull(reader["Gynaecomastia"]);
                        Details.SecondarySexCharacters = (string)DALHelper.HandleDBNull(reader["SecondarySexCharacters"]);
                        Details.HIVPositive = (bool)DALHelper.HandleDBNull(reader["HIVPositive"]);
                        Details.EyeColor = (string)DALHelper.HandleDBNull(reader["EyeColor"]);
                        Details.HairColor = (string)DALHelper.HandleDBNull(reader["HairColor"]);
                        Details.SkinColor = (string)DALHelper.HandleDBNull(reader["SkinColor"]);
                        Details.PhysicalBuilt = (string)DALHelper.HandleDBNull(reader["PhysicalBuilt"]);
                        Details.ExternalGenitalExam = (string)DALHelper.HandleDBNull(reader["ExternalGenitalExam"]);
                        Details.Comments = (string)DALHelper.HandleDBNull(reader["Comments"]);
                                              
                        //By Anjali.....                    
                        Details.HairColor1 = (long)DALHelper.HandleDBNull(reader["HairColorID"]);
                        Details.SkinColor1 = (long)DALHelper.HandleDBNull(reader["SkinColorID"]);
                        Details.EyeColor1 = (long)DALHelper.HandleDBNull(reader["EyeColorID"]);
                        Details.BuiltID = (long)DALHelper.HandleDBNull(reader["BuiltID"]);
                        Details.Alterts = (string)DALHelper.HandleDBNull(reader["Alterts"]);
                        Details.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        // BY BHUSHAn..
                        Details.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);

                        BizActionObj.GeneralDetails.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch(Exception)
            {
            }
            return BizActionObj;
            
        }

        public override IValueObject GetMaleHistory(IValueObject valueobject, clsUserVO UserVO)
        {
            clsGetMaleHistoryBizActionVO BizActionObj = valueobject as clsGetMaleHistoryBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetMaleHistory");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(command, "UnitID", DbType.String, UserVO.UserGeneralDetailVO.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Details == null)
                        BizActionObj.Details = new List<clsMaleHistoryVO>();

                    while (reader.Read())
                    {
                        clsMaleHistoryVO HistoryDetails = new clsMaleHistoryVO();

                        HistoryDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        HistoryDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        HistoryDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        HistoryDetails.Medical = (string)DALHelper.HandleDBNull(reader["Medical"]);
                        HistoryDetails.Surgical = (string)DALHelper.HandleDBNull(reader["Surgical"]);
                        HistoryDetails.Family = (string)DALHelper.HandleDBNull(reader["Family"]);
                        HistoryDetails.Complication = (string)DALHelper.HandleDBNull(reader["Complication"]);
                        HistoryDetails.Allergies = (string)DALHelper.HandleDBNull(reader["Allergies"]);
                        HistoryDetails.Undergarments = (string)DALHelper.HandleDBNull(reader["Undergarments"]);
                        HistoryDetails.Medications = (string)DALHelper.HandleDBNull(reader["Medications"]);
                        HistoryDetails.ExposerToHeat = (bool)DALHelper.HandleDBNull(reader["ExposerToHeat"]);
                        HistoryDetails.ExposerToHeatDetails = (string)DALHelper.HandleDBNull(reader["ExposerToHeatDetails"]);
                        HistoryDetails.Smoking = (bool)DALHelper.HandleDBNull(reader["Smoking"]);
                        HistoryDetails.SmokingHabitual = (bool)DALHelper.HandleDBNull(reader["SmokingHabitual"]);
                        HistoryDetails.Alcohol = (bool)DALHelper.HandleDBNull(reader["Alcohol"]);
                        HistoryDetails.AlcoholHabitual = (bool)DALHelper.HandleDBNull(reader["AlcoholHabitual"]);
                        HistoryDetails.AnyOther = (bool)DALHelper.HandleDBNull(reader["AnyOther"]);
                        HistoryDetails.AnyOtherDetails = (string)DALHelper.HandleDBNull(reader["AnyOtherDetails"]);
                        HistoryDetails.OtherHabitual=(bool)DALHelper.HandleDBNull(reader["OtherHabitual"]);
                        HistoryDetails.AddedDateTime = (DateTime)DALHelper.HandleDBNull(reader["AddedDateTime"]);
                        BizActionObj.Details.Add(HistoryDetails);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo)
        {
           // throw new NotImplementedException();

            clsGetGeneralExaminationFemaleBizActionVO BizActionObj = valueObject as clsGetGeneralExaminationFemaleBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGeneralExaminationForFemale");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGeneralExaminationVO>();
                    while (reader.Read())
                    {
                        clsGeneralExaminationVO Details = new clsGeneralExaminationVO();

                        Details.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Details.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Details.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        Details.Weight = (float)(double)DALHelper.HandleDBNull(reader["Weight"]);
                        Details.Height = (float)(double)DALHelper.HandleDBNull(reader["Height"]);
                        Details.BMI = (float)(double)DALHelper.HandleDBNull(reader["BMI"]);
                        Details.BpInMm = (float)(double)DALHelper.HandleDBNull(reader["BpInMm"]);
                        Details.BpInHg = (float)(double)DALHelper.HandleDBNull(reader["BpInHg"]);
                        Details.Built = (string)DALHelper.HandleDBNull(reader["Built"]);
                        Details.Pulse = (float)(double)DALHelper.HandleDBNull(reader["Pulse"]);
                        //Details.PA = (string)DALHelper.HandleDBNull(reader["PA"]);
                        //Details.RS = (string)DALHelper.HandleDBNull(reader["RS"]);
                        //Details.CVS = (string)DALHelper.HandleDBNull(reader["CVS"]);
                        //Details.CNS = (string)DALHelper.HandleDBNull(reader["CNS"]);
                        Details.Thyroid = (string)DALHelper.HandleDBNull(reader["Thyroid"]);
                        Details.Acne = (string)DALHelper.HandleDBNull(reader["Acne"]);
                        Details.Hirsutism = (string)DALHelper.HandleDBNull(reader["Hirsutism"]);
                        Details.HIVPositive = (bool)DALHelper.HandleDBNull(reader["HIVPositive"]);
                        //Details.EyeColor = (string)DALHelper.HandleDBNull(reader["EyeColor"]);
                        //Details.HairColor = (string)DALHelper.HandleDBNull(reader["HairColor"]);
                        //Details.SkinColor = (string)DALHelper.HandleDBNull(reader["SkinColor"]);
                        Details.PhysicalBuilt = (string)DALHelper.HandleDBNull(reader["PhysicalBuilt"]);
                        Details.ExternalGenitalExam = (string)DALHelper.HandleDBNull(reader["ExternalGenitalExam"]);
                        Details.Comments = (string)DALHelper.HandleDBNull(reader["Comments"]);

                        //By Anjali.....
                        Details.Acen1 = (bool)DALHelper.HandleDBNull(reader["Acen1"]);
                        Details.Hirsutism1 = (bool)DALHelper.HandleDBNull(reader["Hirsutism1"]);
                        //Details.HairColor1 = (long)DALHelper.HandleDBNull(reader["HairColorID"]);
                        //Details.SkinColor1 = (long)DALHelper.HandleDBNull(reader["SkinColorID"]);
                        //Details.EyeColor1 = (long)DALHelper.HandleDBNull(reader["EyeColorID"]);
                        Details.ThyroidExam = (string)DALHelper.HandleDBNull(reader["ThyroidExam"]);
                        Details.BreastExam = (string)DALHelper.HandleDBNull(reader["BreastExam"]);
                        Details.AbdominalExam = (string)DALHelper.HandleDBNull(reader["AbdominalExam"]);
                        Details.PelvicExam = (string)DALHelper.HandleDBNull(reader["PelvicExam"]);
                        Details.HBVPositive = (bool)DALHelper.HandleDBNull(reader["HBVPositive"]);
                        Details.HCVPositive = (bool)DALHelper.HandleDBNull(reader["HCVPositive"]);
                        Details.BuiltID = (long)DALHelper.HandleDBNull(reader["BuiltID"]);
                        // BY BHUSHAn..
                        Details.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);

                        Details.Alterts = (string)DALHelper.HandleDBNull(reader["Alterts"]);
                        Details.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);

                        BizActionObj.List.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception e)
            {
            }
            return BizActionObj;

        }

      #region By Anjali

        public override IValueObject AddClinicalSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsAddClinicalSummaryBizActionVO BizActionObj = valueObject as clsAddClinicalSummaryBizActionVO;
            try
            {
                clsClinicalSummaryVO ClinicalDetails = BizActionObj.Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddClinicalSummary");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, ClinicalDetails.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, ClinicalDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, ClinicalDetails.PatientUnitID);
                dbServer.AddInParameter(command, "SummaryID", DbType.Int64, ClinicalDetails.SummaryID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ClinicalDetails.Date);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, ClinicalDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ClinicalDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                throw;
            }

            return BizActionObj;

        }

        public override IValueObject GetClinicalSummary(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsGetClinicalSummaryBizActionVO BizActionObj = valueObject as clsGetClinicalSummaryBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetClinicalSummary");
                DbDataReader reader;

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.String, BizActionObj.Details.PatientUnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsClinicalSummaryVO>();

                    while (reader.Read())
                    {
                        clsClinicalSummaryVO ClinicalDetails = new clsClinicalSummaryVO();

                        ClinicalDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ClinicalDetails.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        ClinicalDetails.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        ClinicalDetails.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        ClinicalDetails.SummaryID = (long)DALHelper.HandleDBNull(reader["SummaryID"]);
                        ClinicalDetails.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ClinicalDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.DetailsList.Add(ClinicalDetails);
                    }
                }
                reader.NextResult();

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return valueObject;
        }
        #endregion
    }
}
