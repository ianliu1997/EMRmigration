using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Administration;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboad_PlanTherapyDAL : clsBaseIVFDashboad_PlanTherapyDAL
    {
        //This Region Contains Variables Which are Used At Form Level

        #region Variables Declaration
        //Declare the database object
        //private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        #region Variables Declaration
        private Database dbServer = null;
        string IsIsthambul = string.Empty;  //for isthambul criteria
        #endregion

        private clsIVFDashboad_PlanTherapyDAL()
        {
            try
            {
                //#region Create Instance of database,LogManager object and BaseSql object
                ////Create Instance of the database object and BaseSql object
                //if (dbServer == null)
                //{
                //    dbServer = HMSConfigurationManager.GetDatabaseReference();
                //}

                ////Create Instance of the LogManager object 
                //if (logManager == null)
                //{
                //    logManager = LogManager.GetInstance();
                //}
                //#endregion


                try
                {
                    #region Create Instance of database,LogManager object and BaseSql object
                    if (dbServer == null)
                    {
                        dbServer = HMSConfigurationManager.GetDatabaseReference();
                    }
                    if (logManager == null)
                    {
                        logManager = LogManager.GetInstance();
                    }

                    IsIsthambul = System.Configuration.ConfigurationManager.AppSettings["IsIstanbul"];
                    #endregion
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public override IValueObject AddUpdateIVFDashBoardPlanTherapy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddPlanTherapyBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddPlanTherapyBizActionVO;
            BizActionObj = AddUpdateIVFDashBoardPlanTherapy(BizActionObj, UserVo);

            return valueObject;
        }
        public override IValueObject AddUpdateDiagnosis(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection con = null;
            //= dbServer.CreateConnection();
            DbTransaction trans = null;

            clsIVFDashboard_AddDiagnosisBizActionVO BizActionObj = (clsIVFDashboard_AddDiagnosisBizActionVO)valueObject;
            clsDignosisVO objVO = BizActionObj.DiagnosisDetails;
            List<clsDignosisVO> objList = BizActionObj.DiagnosisList;
            if (objVO.IsModify == true)
            {
                try
                {
                    //DbCommand cmd = dbServer.GetStoredProcCommand("CIMS_DeleteAgencyClinicLinkDetails");
                    //dbServer.AddInParameter(cmd, "UnitID", DbType.Int64, objVO.UnitID);
                    //dbServer.ExecuteNonQuery(cmd);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            try
            {
                foreach (clsDignosisVO item in objList)
                {
                    con = dbServer.CreateConnection();
                    con.Open();
                    trans = con.BeginTransaction();
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDiagnosis");
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "DiagnosisID", DbType.Int64, item.ID);
                    //dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.TherapyDetails.PatientId);
                    //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.TherapyDetails.PatientUintId);
                    //dbServer.AddInParameter(command, "TransactionID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                    //dbServer.AddInParameter(command, "TransactionUnitID", DbType.Int64, BizActionObj.TherapyDetails.UnitID);
                    dbServer.AddInParameter(command, "TransactionTypeID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objUserVO.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, 0);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    trans.Commit();


                }
            }

            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.DiagnosisDetails = null;

            }


            finally
            {

            }

            return BizActionObj;
        }
        private clsIVFDashboard_AddPlanTherapyBizActionVO AddUpdateIVFDashBoardPlanTherapy(clsIVFDashboard_AddPlanTherapyBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                // con.Open();
                //DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePlanTherapy");
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePlanTherapyForTest");
                //IVFDashBoard_AddUpdatePlanTherapyForTest
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                dbServer.AddInParameter(command, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                //...................................................
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.TherapyDetails.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.TherapyDetails.PatientUintId);
                dbServer.AddInParameter(command, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                dbServer.AddInParameter(command, "SurrogateUnitID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateUnitID);
                dbServer.AddInParameter(command, "AttachedSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.AttachedSurrogate);
                dbServer.AddInParameter(command, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                //....................................................
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                dbServer.AddInParameter(command, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                dbServer.AddInParameter(command, "CycleDuration", DbType.String, BizActionObj.TherapyDetails.CycleDuration);
                dbServer.AddInParameter(command, "PlannedTreatmentID", DbType.Int64, BizActionObj.TherapyDetails.PlannedTreatmentID);
                dbServer.AddInParameter(command, "FinalPlannedTreatmentID", DbType.Int64, BizActionObj.TherapyDetails.FinalPlannedTreatmentID);
                dbServer.AddInParameter(command, "PlannedNoofEmbryos", DbType.String, BizActionObj.TherapyDetails.PlannedEmbryos);
                dbServer.AddInParameter(command, "MainInductionID", DbType.Int64, BizActionObj.TherapyDetails.MainInductionID);
                dbServer.AddInParameter(command, "PhysicianId", DbType.Int64, BizActionObj.TherapyDetails.PhysicianId);


                dbServer.AddInParameter(command, "ExternalSimulationID", DbType.Int64, BizActionObj.TherapyDetails.ExternalSimulationID);
                dbServer.AddInParameter(command, "Cyclecode", DbType.String, BizActionObj.TherapyDetails.Cyclecode);
                dbServer.AddInParameter(command, "PlannedSpermCollectionID", DbType.Int64, BizActionObj.TherapyDetails.PlannedSpermCollectionID);
                dbServer.AddInParameter(command, "ProtocolTypeID", DbType.Int64, BizActionObj.TherapyDetails.ProtocolTypeID);
                dbServer.AddInParameter(command, "Pill", DbType.String, BizActionObj.TherapyDetails.Pill);
                dbServer.AddInParameter(command, "PillStartDate", DbType.DateTime, BizActionObj.TherapyDetails.PillStartDate);
                dbServer.AddInParameter(command, "PillEndDate", DbType.DateTime, BizActionObj.TherapyDetails.PillEndDate);
                dbServer.AddInParameter(command, "TherapyGeneralNotes", DbType.String, BizActionObj.TherapyDetails.TherapyNotes);
                dbServer.AddInParameter(command, "PCOS", DbType.Boolean, BizActionObj.TherapyDetails.PCOS);
                dbServer.AddInParameter(command, "Hypogonadotropic", DbType.Boolean, BizActionObj.TherapyDetails.Hypogonadotropic);
                dbServer.AddInParameter(command, "Tuberculosis", DbType.Boolean, BizActionObj.TherapyDetails.Tuberculosis);
                dbServer.AddInParameter(command, "Endometriosis", DbType.Boolean, BizActionObj.TherapyDetails.Endometriosis);
                dbServer.AddInParameter(command, "UterineFactors", DbType.Boolean, BizActionObj.TherapyDetails.UterineFactors);
                dbServer.AddInParameter(command, "TubalFactors", DbType.Boolean, BizActionObj.TherapyDetails.TubalFactors);
                dbServer.AddInParameter(command, "DiminishedOvarian", DbType.Boolean, BizActionObj.TherapyDetails.DiminishedOvarian);
                dbServer.AddInParameter(command, "PrematureOvarianFailure", DbType.Boolean, BizActionObj.TherapyDetails.PrematureOvarianFailure);
                dbServer.AddInParameter(command, "LutealPhasedefect", DbType.Boolean, BizActionObj.TherapyDetails.LutealPhasedefect);
                dbServer.AddInParameter(command, "HypoThyroid", DbType.Boolean, BizActionObj.TherapyDetails.HypoThyroid);
                dbServer.AddInParameter(command, "HyperThyroid", DbType.Boolean, BizActionObj.TherapyDetails.HyperThyroid);
                dbServer.AddInParameter(command, "MaleFactors", DbType.Boolean, BizActionObj.TherapyDetails.MaleFactors);
                dbServer.AddInParameter(command, "OtherFactors", DbType.Boolean, BizActionObj.TherapyDetails.OtherFactors);
                dbServer.AddInParameter(command, "UnknownFactors", DbType.Boolean, BizActionObj.TherapyDetails.UnknownFactors);
                dbServer.AddInParameter(command, "FemaleFactorsOnly", DbType.Boolean, BizActionObj.TherapyDetails.FemaleFactorsOnly);
                dbServer.AddInParameter(command, "FemaleandMaleFactors", DbType.Boolean, BizActionObj.TherapyDetails.FemaleandMaleFactors);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "OPUDate", DbType.DateTime, BizActionObj.TherapyDetails.OPUtDate);
                dbServer.AddInParameter(command, "LongtermMedication", DbType.String, BizActionObj.TherapyDetails.LongtermMedication);
                dbServer.AddInParameter(command, "AssistedHatching", DbType.Boolean, BizActionObj.TherapyDetails.AssistedHatching);
                dbServer.AddInParameter(command, "IMSI", DbType.Boolean, BizActionObj.TherapyDetails.IMSI);
                dbServer.AddInParameter(command, "CryoPreservation", DbType.Boolean, BizActionObj.TherapyDetails.CryoPreservation);
                dbServer.AddInParameter(command, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                dbServer.AddInParameter(command, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                dbServer.AddInParameter(command, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                dbServer.AddInParameter(command, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                dbServer.AddInParameter(command, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                dbServer.AddInParameter(command, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                dbServer.AddInParameter(command, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                dbServer.AddParameter(command, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                dbServer.AddInParameter(command, "IsSurrogateCalendar", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogateCalendar);
                dbServer.AddInParameter(command, "IsSurrogateDrug", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogateDrug);

                // Fields added according new Therapy change
                dbServer.AddInParameter(command, "SpermSourceId", DbType.Int64, BizActionObj.TherapyDetails.SpermSource);
                dbServer.AddInParameter(command, "SpermCollectionId", DbType.Int64, BizActionObj.TherapyDetails.PlannedSpermCollection);
                dbServer.AddInParameter(command, "StartOvarian", DbType.DateTime, BizActionObj.TherapyDetails.StartOvarian);
                dbServer.AddInParameter(command, "StartTrigger", DbType.DateTime, BizActionObj.TherapyDetails.StartTrigger);
                dbServer.AddInParameter(command, "StartStimulation", DbType.DateTime, BizActionObj.TherapyDetails.StartStimulation);
                dbServer.AddInParameter(command, "EndOvarian", DbType.DateTime, BizActionObj.TherapyDetails.EndOvarian);
                dbServer.AddInParameter(command, "EndStimulation", DbType.DateTime, BizActionObj.TherapyDetails.EndStimulation);
                dbServer.AddInParameter(command, "TriggerTime", DbType.DateTime, BizActionObj.TherapyDetails.TriggerTime);
                dbServer.AddInParameter(command, "SpermCollectionDate", DbType.DateTime, BizActionObj.TherapyDetails.SpermCollectionDate);
                dbServer.AddInParameter(command, "Note", DbType.String, BizActionObj.TherapyDetails.Note);
                dbServer.AddInParameter(command, "CancellationReason", DbType.String, BizActionObj.TherapyDetails.CancellationReason);
                dbServer.AddInParameter(command, "IsCancelled", DbType.String, BizActionObj.TherapyDetails.IsCancellation);
                dbServer.AddInParameter(command, "MainSubIndicationID", DbType.Int64, BizActionObj.TherapyDetails.MainSubInductionID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.TherapyDetails.AnesthesistId);


                dbServer.AddInParameter(command, "LutealStartDate", DbType.DateTime, BizActionObj.TherapyDetails.LutealStartDate);
                dbServer.AddInParameter(command, "LutealEndDate", DbType.DateTime, BizActionObj.TherapyDetails.LutealEndDate);

                dbServer.AddInParameter(command, "EMRProcedureID", DbType.Int64, BizActionObj.TherapyDetails.EMRProcedureID);
                dbServer.AddInParameter(command, "EMRProcedureUnitID", DbType.Int64, BizActionObj.TherapyDetails.EMRProcedureUnitID);
                dbServer.AddInParameter(command, "IsSurrogacy", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);
                dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, BizActionObj.TherapyDetails.IsDonorCycle);




                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.TherapyDetails.ID = (long)dbServer.GetParameterValue(command, "TherapyID");

                DbCommand commandDel = dbServer.GetStoredProcCommand("CIMS_DeleteIvfDashboardDiagnosisDeatails");
                dbServer.AddInParameter(commandDel, "PlanTherapy", DbType.Int64, BizActionObj.TherapyDetails.ID);
                dbServer.ExecuteNonQuery(commandDel, trans);

                if (BizActionObj.DiagnosisDetails != null)
                {
                    foreach (var item in BizActionObj.DiagnosisDetails)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateIvfDashboardDiagnosisDeatails");
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.TherapyDetails.PatientId);
                        dbServer.AddInParameter(command1, "PlanTherapy", DbType.Int64, BizActionObj.TherapyDetails.ID);
                        dbServer.AddInParameter(command1, "Categori", DbType.String, item.Code);
                        dbServer.AddInParameter(command1, "DiagnosisName", DbType.String, item.Diagnosis);
                        dbServer.AddInParameter(command1, "DiagnosisTypeID", DbType.Int64, item.SelectedDiagnosisType.ID);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyDetails = null;
                trans.Rollback();
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }
        public override IValueObject GetTherapyDetailListForIVFDashboard(IValueObject valueObject, clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsIVFDashboard_GetTherapyListBizActionVO BizAction = valueObject as clsIVFDashboard_GetTherapyListBizActionVO;
            BizAction = GetTherapyDetailListForIVFDashboard(BizAction, objUserVO);
            return valueObject;
        }
        private clsIVFDashboard_GetTherapyListBizActionVO GetTherapyDetailListForIVFDashboard(clsIVFDashboard_GetTherapyListBizActionVO BizAction, clsUserVO objUserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            try
            {
                if (BizAction.Flag == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetTherapyList");

                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                    dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    dbServer.AddInParameter(command, "TabID", DbType.Int64, BizAction.TabID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.TherapyUnitID);
                    dbServer.AddInParameter(command, "PlannedTreatmentID", DbType.Int64, BizAction.PlannedTreatmentID);
                    dbServer.AddInParameter(command, "ProtocolTypeID", DbType.Int64, BizAction.ProtocolTypeID);
                    dbServer.AddInParameter(command, "PhysicianId", DbType.Int64, BizAction.PhysicianId);
                    dbServer.AddInParameter(command, "rdoActive", DbType.Boolean, BizAction.rdoActive);
                    dbServer.AddInParameter(command, "rdoAll", DbType.Boolean, BizAction.rdoAll);
                    dbServer.AddInParameter(command, "rdoClosed", DbType.Boolean, BizAction.rdoClosed);
                    dbServer.AddInParameter(command, "rdoSuccessful", DbType.Boolean, BizAction.rdoSuccessful);
                    dbServer.AddInParameter(command, "rdoUnsuccessful", DbType.Boolean, BizAction.rdoUnsuccessful);
                    dbServer.AddInParameter(command, "AttachedSurrogate", DbType.Boolean, BizAction.AttachedSurrogate);
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, objUserVo.ID);
                    dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, BizAction.TherapyDetails.IsDonorCycle);
                    dbServer.AddInParameter(command, "IsIVFBillingCriteria", DbType.Boolean, BizAction.TherapyDetails.IsIVFBillingCriteria);

                    BizAction.TherapyDetailsList = new List<clsPlanTherapyVO>();

                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (BizAction.TherapyID == 0)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsPlanTherapyVO PlanTherapyDetails = new clsPlanTherapyVO();
                                PlanTherapyDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                PlanTherapyDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                PlanTherapyDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                                PlanTherapyDetails.TherapyStartDate = (DateTime?)(DALHelper.HandleDate(reader["TherapyStartDate"]));
                                PlanTherapyDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                                PlanTherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                                PlanTherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                                PlanTherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                                PlanTherapyDetails.MainSubInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainSubIndicationID"]));
                                PlanTherapyDetails.PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
                                PlanTherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                                PlanTherapyDetails.PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"]));
                                PlanTherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                                PlanTherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                                PlanTherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                                PlanTherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                                PlanTherapyDetails.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                                PlanTherapyDetails.OPUDONEBY = Convert.ToString(DALHelper.HandleDBNull(reader["OPUDONEBY"]));
                                PlanTherapyDetails.ETDate = (DateTime?)(DALHelper.HandleDate(reader["ETDate"]));
                                PlanTherapyDetails.SourceOfOoctye = Convert.ToString(DALHelper.HandleDBNull(reader["SourceOfOoctye"]));
                                PlanTherapyDetails.SourceOfSemen = Convert.ToString(DALHelper.HandleDBNull(reader["SourceOfSemen"]));
                                PlanTherapyDetails.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                                PlanTherapyDetails.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                                PlanTherapyDetails.AttachedSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AttachedSurrogate"]));
                                PlanTherapyDetails.SurrogateMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMRNo"]));
                                PlanTherapyDetails.IsEmbryoDonation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmbryoDonation"]));
                                //Added By Neena
                                PlanTherapyDetails.SpermSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermSourceId"]));
                                PlanTherapyDetails.StartOvarian = (DateTime?)(DALHelper.HandleDate(reader["StartOvarian"]));
                                PlanTherapyDetails.EndOvarian = (DateTime?)(DALHelper.HandleDate(reader["EndOvarian"]));
                                PlanTherapyDetails.StartTrigger = (DateTime?)(DALHelper.HandleDate(reader["StartTrigger"]));
                                PlanTherapyDetails.TriggerTime = (DateTime?)(DALHelper.HandleDate(reader["TriggerTime"]));
                                PlanTherapyDetails.StartStimulation = (DateTime?)(DALHelper.HandleDate(reader["StartStimulation"]));
                                PlanTherapyDetails.EndStimulation = (DateTime?)(DALHelper.HandleDate(reader["EndStimulation"]));
                                PlanTherapyDetails.SpermCollectionDate = (DateTime?)(DALHelper.HandleDate(reader["SpermCollectionDate"]));
                                PlanTherapyDetails.IsCancellation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCancelled"]));
                                PlanTherapyDetails.Note = Convert.ToString(DALHelper.HandleDBNull(reader["Note"]));
                                PlanTherapyDetails.CancellationReason = Convert.ToString(DALHelper.HandleDBNull(reader["CancellationReason"]));
                                PlanTherapyDetails.PACAnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PACEnabled"]));
                                PlanTherapyDetails.AnesthesistId = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthesistID"]));
                                PlanTherapyDetails.ConsentCheck = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentCheck"]));
                                PlanTherapyDetails.LutealStartDate = (DateTime?)(DALHelper.HandleDate(reader["LutealStartDate"]));
                                PlanTherapyDetails.LutealEndDate = (DateTime?)(DALHelper.HandleDate(reader["LutealEndDate"]));
                                PlanTherapyDetails.EMRProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EMRProcedureID"]));
                                PlanTherapyDetails.EMRProcedureUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EMRProcedureUnitID"]));
                                PlanTherapyDetails.IsDonorCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycle"]));
                                PlanTherapyDetails.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogacy"]));
                                PlanTherapyDetails.MainIndication = Convert.ToString(DALHelper.HandleDBNull(reader["MainIndication"]));
                                PlanTherapyDetails.SourceOfSperm = Convert.ToString(DALHelper.HandleDBNull(reader["SourceOfSperm"]));
                                PlanTherapyDetails.OPUtDate = (DateTime?)(DALHelper.HandleDate(reader["OPUDate"]));
                                PlanTherapyDetails.OocyteRetrived = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteRetrived"]));
                                PlanTherapyDetails.NoOocytesCryo = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOocytesCryo"]));
                                PlanTherapyDetails.NoEmbryoCryo = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoEmbryoCryo"]));
                                PlanTherapyDetails.NoEmbryosTransferred = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoEmbryosTransferred"]));
                                PlanTherapyDetails.NoFrozenEmbryosTransferred = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoFrozenEmbryosTransferred"]));
                                PlanTherapyDetails.NoOocytesDonated = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOocytesDonated"]));
                                PlanTherapyDetails.NoOfEmbryos = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfEmbryos"]));
                                PlanTherapyDetails.NoEmbryoDonated = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoEmbryoDonated"]));
                                PlanTherapyDetails.NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"]));
                                PlanTherapyDetails.FetalHeartCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartCount"]));
                                PlanTherapyDetails.BHCGAssessment = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAssessment"]));
                                PlanTherapyDetails.NoOfEmbExtCulture = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfEmbExtCulture"]));
                                PlanTherapyDetails.NoOfEmbFzOocyte = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfEmbFzOocyte"]));
                                PlanTherapyDetails.OPUCycleCancellationReason = Convert.ToString(DALHelper.HandleDBNull(reader["IsOPUCancelReason"]));
                                PlanTherapyDetails.IsOPUCycleCancellation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOPUCancel"]));
                                PlanTherapyDetails.OPUFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OPUFreeze"]));

                                if (IsIsthambul.Equals("Yes"))
                                    PlanTherapyDetails.IsIsthambul = true;
                                else if (IsIsthambul.Equals("No"))
                                    PlanTherapyDetails.IsIsthambul = false;
                                //

                                //commented by neena
                                //PlanTherapyDetails.PCOS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PCOS"]));
                                //PlanTherapyDetails.Hypogonadotropic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypogonadotropic"]));
                                //PlanTherapyDetails.Tuberculosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tuberculosis"]));
                                //PlanTherapyDetails.Endometriosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometriosis"]));
                                //PlanTherapyDetails.UterineFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineFactors"]));
                                //PlanTherapyDetails.TubalFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TubalFactors"]));
                                //PlanTherapyDetails.DiminishedOvarian = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DiminishedOvarian"]));
                                //PlanTherapyDetails.PrematureOvarianFailure = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrematureOvarianFailure"]));
                                //PlanTherapyDetails.LutealPhasedefect = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LutealPhasedefect"]));
                                //PlanTherapyDetails.HypoThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HypoThyroid"]));
                                //PlanTherapyDetails.MaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MaleFactors"]));
                                //PlanTherapyDetails.OtherFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OtherFactors"]));
                                //PlanTherapyDetails.UnknownFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnknownFactors"]));
                                //PlanTherapyDetails.FemaleFactorsOnly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleFactorsOnly"]));
                                //PlanTherapyDetails.FemaleandMaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleandMaleFactors"]));
                                //PlanTherapyDetails.HyperThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HyperThyroid"]));                                
                                //PlanTherapyDetails.PlannedEmbryos = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                                //PlanTherapyDetails.LongtermMedication = Convert.ToString(DALHelper.HandleDBNull(reader["LongtermMedication"]));
                                //PlanTherapyDetails.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AssistedHatching"]));
                                //PlanTherapyDetails.CryoPreservation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CryoPreservation"]));
                                //PlanTherapyDetails.IMSI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IMSI"]));
                                //PlanTherapyDetails.IsPregnancyAchieved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PregnancyAchieved"]));
                                //PlanTherapyDetails.PregnanacyConfirmDate = (DateTime?)(DALHelper.HandleDate(reader["PregnanacyConfirmDate"]));                         
                                //PlanTherapyDetails.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));
                                //PlanTherapyDetails.OPUDate1 = (DateTime?)(DALHelper.HandleDate(reader["OPUDate1"]));  
                                //PlanTherapyDetails.CryoDate = (DateTime?)(DALHelper.HandleDate(reader["CryoDate"]));
                                //PlanTherapyDetails.CryoNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CryoNo"]));
                                //PlanTherapyDetails.ThwaDate = (DateTime?)(DALHelper.HandleDate(reader["ThwaDate"]));                             
                                //PlanTherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));  
                                //PlanTherapyDetails.Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"]));
                                //PlanTherapyDetails.PillStartDate = (DateTime?)(DALHelper.HandleDate(reader["PillStartDate"]));
                                //PlanTherapyDetails.PillEndDate = (DateTime?)(DALHelper.HandleDate(reader["PillEndDate"]));
                                //PlanTherapyDetails.FinalPlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FinalPlannedTreatmentID"]));
                                //PlanTherapyDetails.TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"]));
                                BizAction.TherapyDetailsList.Add(PlanTherapyDetails);
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.NewButtonVisibility = Convert.ToBoolean(DALHelper.HandleDBNull(reader["NewButtonVisibility"]));
                            }

                        }
                    }
                    else
                    {
                        if (BizAction.TabID == 0)
                        {
                            //if (reader.HasRows)
                            //{
                            //    while (reader.Read())
                            //    {
                            //        BizAction.TherapyDocument.Add(TherapyDocuments(BizAction, reader));
                            //    }
                            //}
                            //  reader.NextResult();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyExecutionList.Add(TherapyExcecution(BizAction, reader));

                                }
                            }

                            reader.NextResult();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.FollicularMonitoringList.Add(FollicularMonitoring(BizAction, reader));
                                }
                            }
                            reader.NextResult();
                            if (BizAction.AttachedSurrogate == true)
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        BizAction.TherapyExecutionListSurrogate.Add(TherapyExcecutionSurrogate(BizAction, reader));
                                    }
                                }
                            }
                        }
                        else if (BizAction.TabID == (int)TherapyTabs.Documents)//Document List
                        {
                            //if (reader.HasRows)
                            //{
                            //    while (reader.Read())
                            //    {
                            //        BizAction.TherapyDocument.Add(TherapyDocuments(BizAction, reader));
                            //    }
                            //}
                        }
                        else if (BizAction.TabID == (int)TherapyTabs.Execution)//Therapy Execution Lsit
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                    BizAction.TherapyExecutionList.Add(TherapyExcecution(BizAction, reader));

                                }
                            }
                            reader.NextResult();
                            if (BizAction.AttachedSurrogate == true)
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        BizAction.TherapyExecutionListSurrogate.Add(TherapyExcecutionSurrogate(BizAction, reader));
                                    }
                                }
                            }
                        }

                        else if (BizAction.TabID == (int)TherapyTabs.FollicularMonitoring)//Follicular Monitoring
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.FollicularMonitoringList.Add(FollicularMonitoring(BizAction, reader));
                                }
                            }
                        }
                    }
                    reader.Close();
                    reader.Close();
                }

                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetTherapyListForLabDay");
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                    dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    dbServer.AddInParameter(command, "TherapyUnitID", DbType.Int64, BizAction.TherapyUnitID);
                    BizAction.TherapyDetailsList = new List<clsPlanTherapyVO>();
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BizAction.TherapyDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            BizAction.TherapyDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            BizAction.TherapyDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                            BizAction.TherapyDetails.TherapyStartDate = (DateTime?)(DALHelper.HandleDate(reader["TherapyStartDate"]));
                            BizAction.TherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                            BizAction.TherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                            BizAction.TherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                            BizAction.TherapyDetails.PlannedNoofEmbryos = Convert.ToInt32(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                            BizAction.TherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                            BizAction.TherapyDetails.MainInduction = Convert.ToString(DALHelper.HandleDBNull(reader["MainIndication"]));
                            BizAction.TherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                            BizAction.TherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                            BizAction.TherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                            BizAction.TherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                            BizAction.TherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                        }
                    }
                    reader.Close();
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
        public clsTherapyExecutionVO TherapyExcecution(clsIVFDashboard_GetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            clsTherapyExecutionVO TherapyExe = new clsTherapyExecutionVO();
            TherapyExe.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            TherapyExe.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
            TherapyExe.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
            TherapyExe.TherapyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyTypeId"]));
            TherapyExe.PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
            TherapyExe.TherapyStartDate = (DateTime?)(DALHelper.HandleDate(reader["TherapyStartDate"]));
            TherapyExe.ThearpyTypeDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"]));
            //By Anjali...............................................
            TherapyExe.IsOocyteDonorExists = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOocyteDonorExists"]));
            TherapyExe.IsSemenDonorExists = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSemenDonorExists"]));
            TherapyExe.OoctyDonorMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["OoctyDonorMrNo"]));
            TherapyExe.SemenDonorMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SemenDonorMrNo"]));
            TherapyExe.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
            TherapyExe.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
            //........................................................
            #region Set Proprties According to Therpy Group
            if ((int)TherapyGroup.Event == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.Event.ToString();
                TherapyExe.Head = "Date of LMP";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
                TherapyExe.IsReadOnly = false;
            }
            else if ((int)TherapyGroup.Drug == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.Drug.ToString();
                TherapyExe.IsBool = false;
                TherapyExe.IsText = true;
                TherapyExe.IsReadOnly = false;
                TherapyExe.Head = Convert.ToString((DALHelper.HandleDBNull(reader["DrugName"])));
            }
            else if ((int)TherapyGroup.UltraSound == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.UltraSound.ToString();
                //TherapyExe.Head = "Follicular US";
                TherapyExe.Head = "Follicular Scan";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
                TherapyExe.IsReadOnly = false;
            }
            else if ((int)TherapyGroup.OvumPickUp == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.OvumPickUp.ToString();
                TherapyExe.Head = "OPU";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
                TherapyExe.IsReadOnly = false;
            }
            else if ((int)TherapyGroup.EmbryoTransfer == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.EmbryoTransfer.ToString();
                TherapyExe.Head = "ET";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
                TherapyExe.IsReadOnly = false;
            }
            else
                if ((int)TherapyGroup.AMH == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.E2.ToString();
                    TherapyExe.Head = "AMH";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.E2 == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.E2.ToString();
                    TherapyExe.Head = "E2";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.Progesterone == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.Progesterone.ToString();
                    TherapyExe.Head = "Progesterone";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.LH == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.FSH.ToString();
                    TherapyExe.Head = "LH";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.FSH == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.FSH.ToString();
                    TherapyExe.Head = "FSH";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.Prolactin == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.Prolactin.ToString();
                    TherapyExe.Head = "Prolactin";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.TSH == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.BHCG.ToString();
                    TherapyExe.Head = "TSH";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.Testesterone == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.BHCG.ToString();
                    TherapyExe.Head = "Testosterone";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.BHCG == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.BHCG.ToString();
                    TherapyExe.Head = "BHCG";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.HIV == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.HIV.ToString();
                    TherapyExe.Head = "HIV";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.HCV == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.HCV.ToString();
                    TherapyExe.Head = "HCV";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }
                else if ((int)TherapyGroup.HbSAg == (int)TherapyExe.TherapyTypeId)
                {
                    TherapyExe.TherapyGroup = TherapyGroup.HbSAg.ToString();
                    TherapyExe.Head = "HbSAg";
                    //TherapyExe.IsBool = true;
                    //TherapyExe.IsText = false;
                    TherapyExe.IsBool = false;
                    TherapyExe.IsText = true;
                    TherapyExe.IsReadOnly = true;
                }


            #endregion

            TherapyExe.Day1 = Convert.ToString((DALHelper.HandleDBNull(reader["Day1"])));
            TherapyExe.Day2 = Convert.ToString((DALHelper.HandleDBNull(reader["Day2"])));
            TherapyExe.Day3 = Convert.ToString((DALHelper.HandleDBNull(reader["Day3"])));
            TherapyExe.Day4 = Convert.ToString((DALHelper.HandleDBNull(reader["Day4"])));
            TherapyExe.Day5 = Convert.ToString((DALHelper.HandleDBNull(reader["Day5"])));
            TherapyExe.Day6 = Convert.ToString((DALHelper.HandleDBNull(reader["Day6"])));
            TherapyExe.Day7 = Convert.ToString((DALHelper.HandleDBNull(reader["Day7"])));
            TherapyExe.Day8 = Convert.ToString((DALHelper.HandleDBNull(reader["Day8"])));
            TherapyExe.Day9 = Convert.ToString((DALHelper.HandleDBNull(reader["Day9"])));
            TherapyExe.Day10 = Convert.ToString((DALHelper.HandleDBNull(reader["Day10"])));
            TherapyExe.Day11 = Convert.ToString((DALHelper.HandleDBNull(reader["Day11"])));
            TherapyExe.Day12 = Convert.ToString((DALHelper.HandleDBNull(reader["Day12"])));
            TherapyExe.Day13 = Convert.ToString((DALHelper.HandleDBNull(reader["Day13"])));
            TherapyExe.Day14 = Convert.ToString((DALHelper.HandleDBNull(reader["Day14"])));
            TherapyExe.Day15 = Convert.ToString((DALHelper.HandleDBNull(reader["Day15"])));
            TherapyExe.Day16 = Convert.ToString((DALHelper.HandleDBNull(reader["Day16"])));
            TherapyExe.Day17 = Convert.ToString((DALHelper.HandleDBNull(reader["Day17"])));
            TherapyExe.Day18 = Convert.ToString((DALHelper.HandleDBNull(reader["Day18"])));
            TherapyExe.Day19 = Convert.ToString((DALHelper.HandleDBNull(reader["Day19"])));
            TherapyExe.Day20 = Convert.ToString((DALHelper.HandleDBNull(reader["Day20"])));
            TherapyExe.Day21 = Convert.ToString((DALHelper.HandleDBNull(reader["Day21"])));
            TherapyExe.Day22 = Convert.ToString((DALHelper.HandleDBNull(reader["Day22"])));
            TherapyExe.Day23 = Convert.ToString((DALHelper.HandleDBNull(reader["Day23"])));
            TherapyExe.Day24 = Convert.ToString((DALHelper.HandleDBNull(reader["Day24"])));
            TherapyExe.Day25 = Convert.ToString((DALHelper.HandleDBNull(reader["Day25"])));
            TherapyExe.Day26 = Convert.ToString((DALHelper.HandleDBNull(reader["Day26"])));
            TherapyExe.Day27 = Convert.ToString((DALHelper.HandleDBNull(reader["Day27"])));
            TherapyExe.Day28 = Convert.ToString((DALHelper.HandleDBNull(reader["Day28"])));
            TherapyExe.Day29 = Convert.ToString((DALHelper.HandleDBNull(reader["Day29"])));
            TherapyExe.Day30 = Convert.ToString((DALHelper.HandleDBNull(reader["Day30"])));
            TherapyExe.Day31 = Convert.ToString((DALHelper.HandleDBNull(reader["Day31"])));
            TherapyExe.Day32 = Convert.ToString((DALHelper.HandleDBNull(reader["Day32"])));
            TherapyExe.Day33 = Convert.ToString((DALHelper.HandleDBNull(reader["Day33"])));
            TherapyExe.Day34 = Convert.ToString((DALHelper.HandleDBNull(reader["Day34"])));
            TherapyExe.Day35 = Convert.ToString((DALHelper.HandleDBNull(reader["Day35"])));
            TherapyExe.Day36 = Convert.ToString((DALHelper.HandleDBNull(reader["Day36"])));
            TherapyExe.Day37 = Convert.ToString((DALHelper.HandleDBNull(reader["Day37"])));
            TherapyExe.Day38 = Convert.ToString((DALHelper.HandleDBNull(reader["Day38"])));
            TherapyExe.Day39 = Convert.ToString((DALHelper.HandleDBNull(reader["Day39"])));
            TherapyExe.Day40 = Convert.ToString((DALHelper.HandleDBNull(reader["Day40"])));
            TherapyExe.Day41 = Convert.ToString((DALHelper.HandleDBNull(reader["Day41"])));
            TherapyExe.Day42 = Convert.ToString((DALHelper.HandleDBNull(reader["Day42"])));
            TherapyExe.Day43 = Convert.ToString((DALHelper.HandleDBNull(reader["Day43"])));
            TherapyExe.Day44 = Convert.ToString((DALHelper.HandleDBNull(reader["Day44"])));
            TherapyExe.Day45 = Convert.ToString((DALHelper.HandleDBNull(reader["Day45"])));
            TherapyExe.Day46 = Convert.ToString((DALHelper.HandleDBNull(reader["Day46"])));
            TherapyExe.Day47 = Convert.ToString((DALHelper.HandleDBNull(reader["Day47"])));
            TherapyExe.Day48 = Convert.ToString((DALHelper.HandleDBNull(reader["Day48"])));
            TherapyExe.Day49 = Convert.ToString((DALHelper.HandleDBNull(reader["Day49"])));
            TherapyExe.Day50 = Convert.ToString((DALHelper.HandleDBNull(reader["Day50"])));
            TherapyExe.Day51 = Convert.ToString((DALHelper.HandleDBNull(reader["Day51"])));
            TherapyExe.Day52 = Convert.ToString((DALHelper.HandleDBNull(reader["Day52"])));
            TherapyExe.Day53 = Convert.ToString((DALHelper.HandleDBNull(reader["Day53"])));
            TherapyExe.Day54 = Convert.ToString((DALHelper.HandleDBNull(reader["Day54"])));
            TherapyExe.Day55 = Convert.ToString((DALHelper.HandleDBNull(reader["Day55"])));
            TherapyExe.Day56 = Convert.ToString((DALHelper.HandleDBNull(reader["Day56"])));
            TherapyExe.Day57 = Convert.ToString((DALHelper.HandleDBNull(reader["Day57"])));
            TherapyExe.Day58 = Convert.ToString((DALHelper.HandleDBNull(reader["Day58"])));
            TherapyExe.Day59 = Convert.ToString((DALHelper.HandleDBNull(reader["Day59"])));
            TherapyExe.Day60 = Convert.ToString((DALHelper.HandleDBNull(reader["Day60"])));

            return TherapyExe;
        }
        public clsFollicularMonitoring FollicularMonitoring(clsIVFDashboard_GetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            clsFollicularMonitoring FollicularMon = new clsFollicularMonitoring();
            FollicularMon.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            FollicularMon.TherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyId"]));
            FollicularMon.TherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitId"]));
            FollicularMon.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitId"]));
            FollicularMon.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
            FollicularMon.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
            FollicularMon.PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianID"]));
            FollicularMon.FollicularNotes = Convert.ToString(DALHelper.HandleDBNull(reader["Notes"]));
            FollicularMon.AttachmentPath = Convert.ToString(DALHelper.HandleDBNull(reader["AttachmentPath"]));

            FollicularMon.AttachmentFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachmentFileContent"]));
            FollicularMon.EndometriumThickness = Convert.ToString(DALHelper.HandleDBNull(reader["EndometriumThickness"]));

            FollicularMon.UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
            return FollicularMon;
        }
        public override IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetFollicularMonitoringSizeDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetFollicularMonitoringSizeList");

                dbServer.AddInParameter(command, "FollicularId", DbType.String, BizAction.FollicularID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                BizAction.FollicularMonitoringSizeList = new List<clsFollicularMonitoringSizeDetails>();

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsFollicularMonitoringSizeDetails Follicsize = new clsFollicularMonitoringSizeDetails();
                        Follicsize.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Follicsize.FollicularMonitoringId = Convert.ToInt64(DALHelper.HandleDBNull(reader["FollicularMonitoringId"]));
                        Follicsize.FollicularNumber = Convert.ToString(DALHelper.HandleDBNull(reader["FollicularNumber"]));
                        Follicsize.LeftSize = Convert.ToString(DALHelper.HandleDBNull(reader["LeftSize"]));
                        Follicsize.RightSIze = Convert.ToString(DALHelper.HandleDBNull(reader["RightSIze"]));
                        BizAction.FollicularMonitoringSizeList.Add(Follicsize);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }
        public override IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_UpdateFollicularMonitoringBizActionVO BizActionObj = valueObject as clsIVFDashboard_UpdateFollicularMonitoringBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateFollicularMonitoring");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.FollicularID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.FollicularMonitoringDetial.UnitID);
                dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, BizActionObj.FollicularMonitoringDetial.TherapyId);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.FollicularMonitoringDetial.TherapyUnitId);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.FollicularMonitoringDetial.Date);
                dbServer.AddInParameter(command, "AttendedPhysicianId", DbType.Int64, BizActionObj.FollicularMonitoringDetial.PhysicianID);
                dbServer.AddInParameter(command, "Notes", DbType.String, BizActionObj.FollicularMonitoringDetial.FollicularNotes);
                dbServer.AddInParameter(command, "AttachmentPath", DbType.String, BizActionObj.FollicularMonitoringDetial.AttachmentPath);
                dbServer.AddInParameter(command, "AttachmentFileContents", DbType.Binary, BizActionObj.FollicularMonitoringDetial.AttachmentFileContent);
                dbServer.AddInParameter(command, "EndometriumThickness", DbType.String, BizActionObj.FollicularMonitoringDetial.EndometriumThickness);
                dbServer.AddInParameter(command, "FollicularNoList", DbType.String, BizActionObj.FollicularMonitoringDetial.FollicularNoList);
                dbServer.AddInParameter(command, "LeftSizeList", DbType.String, BizActionObj.FollicularMonitoringDetial.LeftSizeList);
                dbServer.AddInParameter(command, "RightSizeList", DbType.String, BizActionObj.FollicularMonitoringDetial.RightSizeList);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.FollicularMonitoringDetial = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }
        public override IValueObject AddUpdateLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();

            clsIVFDashboard_AddUpdateLutealPhaseBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateLutealPhaseBizActionVO;


            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateLutealPhase");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.LutualPhaseDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.LutualPhaseDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.LutualPhaseDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.LutualPhaseDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "LutealSupport", DbType.String, BizActionObj.LutualPhaseDetails.LutealSupport);
                dbServer.AddInParameter(command, "LutealRemark", DbType.String, BizActionObj.LutualPhaseDetails.LutealRemark);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.LutualPhaseDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.LutualPhaseDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.LutualPhaseDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;



        }
        public override IValueObject GetLutealPhaseDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();

            clsIVFDashboard_GetLutealPhaseBizActionVO BizAction = valueObject as clsIVFDashboard_GetLutealPhaseBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetLutealPhaseDetails");
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
                        BizAction.Details.LutealSupport = Convert.ToString(DALHelper.HandleDBNull(reader["LutealSupport"]));
                        BizAction.Details.LutealRemark = Convert.ToString(DALHelper.HandleDBNull(reader["LutealRemark"]));

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
        public override IValueObject AddUpdateOutcomeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateOutcomeBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateOutcomeBizActionVO;


            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateOutcomeDetails");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.OutcomeDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyUnitID);

                dbServer.AddInParameter(command, "BHCGAss1Date", DbType.DateTime, BizActionObj.OutcomeDetails.BHCGAss1Date);
                dbServer.AddInParameter(command, "BHCGAss1IsBSCG", DbType.Boolean, BizActionObj.OutcomeDetails.BHCGAss1IsBSCG);
                dbServer.AddInParameter(command, "BHCGAss1BSCGValue", DbType.String, BizActionObj.OutcomeDetails.BHCGAss1BSCGValue);
                dbServer.AddInParameter(command, "BHCGAss1SrProgest", DbType.String, BizActionObj.OutcomeDetails.BHCGAss1SrProgest);
                dbServer.AddInParameter(command, "BHCGAss2Date", DbType.DateTime, BizActionObj.OutcomeDetails.BHCGAss2Date);
                dbServer.AddInParameter(command, "BHCGAss2IsBSCG", DbType.Boolean, BizActionObj.OutcomeDetails.BHCGAss2IsBSCG);
                dbServer.AddInParameter(command, "BHCGAss2BSCGValue", DbType.String, BizActionObj.OutcomeDetails.BHCGAss2BSCGValue);
                dbServer.AddInParameter(command, "BHCGAss2USG", DbType.String, BizActionObj.OutcomeDetails.BHCGAss2USG);
                dbServer.AddInParameter(command, "PregnancyAchieved", DbType.Boolean, BizActionObj.OutcomeDetails.IsPregnancyAchieved);
                dbServer.AddInParameter(command, "PregnanacyConfirmDate", DbType.DateTime, BizActionObj.OutcomeDetails.PregnanacyConfirmDate);
                dbServer.AddInParameter(command, "IsClosed", DbType.Boolean, BizActionObj.OutcomeDetails.IsClosed);
                dbServer.AddInParameter(command, "OutComeRemarks", DbType.String, BizActionObj.OutcomeDetails.OutComeRemarks);
                dbServer.AddInParameter(command, "OHSSRemark", DbType.String, BizActionObj.OutcomeDetails.OHSSRemark);
                dbServer.AddInParameter(command, "BiochemPregnancy", DbType.Boolean, BizActionObj.OutcomeDetails.BiochemPregnancy);
                dbServer.AddInParameter(command, "Ectopic", DbType.Boolean, BizActionObj.OutcomeDetails.Ectopic);
                dbServer.AddInParameter(command, "Abortion", DbType.Boolean, BizActionObj.OutcomeDetails.Abortion);
                dbServer.AddInParameter(command, "Missed", DbType.Boolean, BizActionObj.OutcomeDetails.Missed);
                dbServer.AddInParameter(command, "FetalHeartSound", DbType.Boolean, BizActionObj.OutcomeDetails.FetalHeartSound);
                dbServer.AddInParameter(command, "FetalDate", DbType.DateTime, BizActionObj.OutcomeDetails.FetalDate);
                dbServer.AddInParameter(command, "Count", DbType.String, BizActionObj.OutcomeDetails.Count);
                dbServer.AddInParameter(command, "Incomplete", DbType.Boolean, BizActionObj.OutcomeDetails.Incomplete);
                dbServer.AddInParameter(command, "IUD", DbType.Boolean, BizActionObj.OutcomeDetails.IUD);
                dbServer.AddInParameter(command, "PretermDelivery", DbType.Boolean, BizActionObj.OutcomeDetails.PretermDelivery);
                dbServer.AddInParameter(command, "LiveBirth", DbType.Boolean, BizActionObj.OutcomeDetails.LiveBirth);
                dbServer.AddInParameter(command, "Congenitalabnormality", DbType.Boolean, BizActionObj.OutcomeDetails.Congenitalabnormality);
                dbServer.AddInParameter(command, "IsChemicalPregnancy", DbType.Boolean, BizActionObj.OutcomeDetails.IsChemicalPregnancy);
                dbServer.AddInParameter(command, "IsFullTermDelivery", DbType.Boolean, BizActionObj.OutcomeDetails.IsFullTermDelivery);
                dbServer.AddInParameter(command, "OHSSEarly", DbType.Boolean, BizActionObj.OutcomeDetails.OHSSEarly);
                dbServer.AddInParameter(command, "OHSSLate", DbType.Boolean, BizActionObj.OutcomeDetails.OHSSLate);
                dbServer.AddInParameter(command, "OHSSMild", DbType.Boolean, BizActionObj.OutcomeDetails.OHSSMild);
                dbServer.AddInParameter(command, "OHSSMode", DbType.Boolean, BizActionObj.OutcomeDetails.OHSSMode);
                dbServer.AddInParameter(command, "OHSSSereve", DbType.Boolean, BizActionObj.OutcomeDetails.OHSSSereve);
                dbServer.AddInParameter(command, "BabyTypeID", DbType.Int64, BizActionObj.OutcomeDetails.BabyTypeID);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //added by neena
                dbServer.AddInParameter(command, "NoOfSacs", DbType.Int64, BizActionObj.OutcomeDetails.NoOfSacs);
                dbServer.AddInParameter(command, "SacsObservationDate", DbType.DateTime, BizActionObj.OutcomeDetails.SacsObservationDate);
                dbServer.AddInParameter(command, "PregnancyAchievedID", DbType.Int64, BizActionObj.OutcomeDetails.PregnancyAchievedID);
                dbServer.AddInParameter(command, "SacRemark", DbType.String, BizActionObj.OutcomeDetails.SacRemarks);
                dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, BizActionObj.OutcomeDetails.IsDonorCycle);
                dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean, BizActionObj.OutcomeDetails.IsFreeze);               
                //

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.OutcomeDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.OutcomeDetails.ID = (long)dbServer.GetParameterValue(command, "ID");


                if (BizActionObj.OutcomeDetails.IsSurrogate)
                {
                    if (BizActionObj.OutcomeDetailsList != null && BizActionObj.OutcomeDetailsList.Count > 0)
                    {
                        DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashBoard_OutcomeBHCGResultDetails set Status=" + 0 + " where  OutcomeID=" + BizActionObj.OutcomeDetails.ID);
                        int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1);

                        foreach (var item in BizActionObj.OutcomeDetailsList)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateOutcomeBHCGResultDetails");
                            dbServer.AddInParameter(command2, "ID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "OutcomeID", DbType.Int64, BizActionObj.OutcomeDetails.ID);
                            dbServer.AddInParameter(command2, "BHCGResultDate", DbType.DateTime, item.BHCGAss1Date);
                            dbServer.AddInParameter(command2, "ResultID", DbType.Int64, item.ResultListID);
                            dbServer.AddInParameter(command2, "BHCGValue", DbType.String, item.BHCGAss1BSCGValue);
                            dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, item.SurrogateID);
                            dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, item.SurrogateUnitID);
                            dbServer.AddInParameter(command2, "MrNo", DbType.String, item.SurrogatePatientMrNo);

                            int intStatus2 = dbServer.ExecuteNonQuery(command2);
                        }
                    }

                    long FetalHeartSurrogacy = 0;
                    if (BizActionObj.OutcomePregnancySacList != null && BizActionObj.OutcomePregnancySacList.Count > 0)
                    {
                        foreach (var PregnancySac in BizActionObj.OutcomePregnancySacList)
                        {
                            //long PregnancySacId = 0;
                            FetalHeartSurrogacy = 0;
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySac");
                            dbServer.AddInParameter(command1, "OutcomeID", DbType.Int64, BizActionObj.OutcomeDetails.ID);
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.OutcomeDetails.PatientID);
                            dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PatientUnitID);
                            dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyID);
                            dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyUnitID);
                            dbServer.AddInParameter(command1, "NoOfSacs", DbType.Int64, PregnancySac.NoOfSacs);
                            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, PregnancySac.ID);
                            dbServer.AddInParameter(command1, "SacsObservationDate", DbType.DateTime, PregnancySac.SacsObservationDate);
                            dbServer.AddInParameter(command1, "PregnancyAchievedID", DbType.Int64, PregnancySac.PregnancyAchievedID);
                            dbServer.AddInParameter(command1, "SacRemark", DbType.String, PregnancySac.SacRemarks);
                            dbServer.AddInParameter(command1, "SurrogateTypeID", DbType.Int64, PregnancySac.SurrogateTypeID);
                            dbServer.AddInParameter(command1, "SurrogateID", DbType.Int64, PregnancySac.SurrogateID);
                            dbServer.AddInParameter(command1, "SurrogateUnitID", DbType.Int64, PregnancySac.SurrogateUnitID);
                            dbServer.AddInParameter(command1, "SurrogatePatientMrNo", DbType.String, PregnancySac.SurrogatePatientMrNo);
                            dbServer.AddInParameter(command1, "IsUnlinkSurrogate", DbType.Boolean, PregnancySac.IsUnlinkSurrogate);
                            dbServer.AddInParameter(command1, "IsFreeze", DbType.Boolean, BizActionObj.OutcomeDetails.IsFreeze);
                            dbServer.AddInParameter(command1, "FreeSurrogate", DbType.Boolean, BizActionObj.OutcomeDetails.FreeSurrogate);
                            int intStatus1 = dbServer.ExecuteNonQuery(command1);
                            PregnancySac.ID = (long)dbServer.GetParameterValue(command1, "ID");

                            foreach (var item in PregnancySac.PregnancySacsList)
                            {
                                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySacDetails");
                                dbServer.AddInParameter(command2, "OutcomeID", DbType.Int64, BizActionObj.OutcomeDetails.ID);
                                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "PregnancySacID", DbType.Int64, PregnancySac.ID);
                                dbServer.AddInParameter(command2, "SacNoStr", DbType.String, item.SaceNoStr);
                                dbServer.AddInParameter(command2, "IsFetalheart", DbType.Boolean, item.IsFetalHeart);
                                if (item.IsFetalHeart == true)
                                    FetalHeartSurrogacy = FetalHeartSurrogacy + 1;
                                dbServer.AddInParameter(command2, "ResultID", DbType.Int64, item.ResultListID);
                                dbServer.AddInParameter(command2, "PregnancyID", DbType.Int64, item.PregnancyListID);
                                dbServer.AddInParameter(command2, "CongenitalAbnormalityYes", DbType.Boolean, item.CongenitalAbnormalityYes);
                                dbServer.AddInParameter(command2, "CongenitalAbnormalityNo", DbType.Boolean, item.CongenitalAbnormalityNo);
                                dbServer.AddInParameter(command2, "CongenitalAbnormalityReason", DbType.String, item.CongenitalAbnormalityReason);
                                dbServer.AddInParameter(command2, "FetalHeartCount", DbType.Int64, FetalHeartSurrogacy);
                                dbServer.AddInParameter(command2, "IsUnlinkSurrogate", DbType.Boolean, PregnancySac.IsUnlinkSurrogate);
                                dbServer.AddInParameter(command2, "IsFreeze", DbType.Boolean, BizActionObj.OutcomeDetails.IsFreeze);
                                dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, PregnancySac.SurrogateID);
                                dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, PregnancySac.SurrogateUnitID);
                                int intStatus2 = dbServer.ExecuteNonQuery(command2);
                            }


                            if (BizActionObj.OutcomeDetails.IsClosed == true)
                            {
                                if (FetalHeartSurrogacy > 0)
                                {
                                    long BirthDetailID = 0;
                                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetails");
                                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.OutcomeDetails.PatientID);
                                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PatientUnitID);
                                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyID);
                                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command3, "FetalHeartNo", DbType.Int64, FetalHeartSurrogacy);
                                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BirthDetailID);
                                    dbServer.AddInParameter(command3, "SurrogateID", DbType.Int64, PregnancySac.SurrogateID);
                                    dbServer.AddInParameter(command3, "SurrogateUnitID", DbType.Int64, PregnancySac.SurrogateUnitID);
                                    dbServer.AddInParameter(command3, "SurrogatePatientMrNo", DbType.String, PregnancySac.SurrogatePatientMrNo);
                                    int intStatus3 = dbServer.ExecuteNonQuery(command3);
                                    BirthDetailID = (long)dbServer.GetParameterValue(command3, "ID");

                                    for (int i = 0; i < FetalHeartSurrogacy; i++)
                                    {
                                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetailsList");
                                        dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        dbServer.AddInParameter(command2, "BirthDetailsID", DbType.Int64, BirthDetailID);
                                        dbServer.AddInParameter(command2, "ChildNoStr", DbType.String, "Child" + (i + 1));
                                        dbServer.AddInParameter(command2, "BirthWeight", DbType.String, "");
                                        dbServer.AddInParameter(command2, "MedicalConditions", DbType.String, "");
                                        dbServer.AddInParameter(command2, "WeeksofGestation", DbType.String, "");
                                        dbServer.AddInParameter(command2, "DeliveryTypeID", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "ActivityID", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "ActivityPoint", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "Pulse", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "PulsePoint", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "Grimace", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "GrimacePoint", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "Appearance", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "AppearancePoint", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "Respiration", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "RespirationPoint", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "APGARScore", DbType.Int64, 0);
                                        dbServer.AddInParameter(command2, "Conclusion", DbType.Int64, null);
                                        dbServer.AddInParameter(command2, "APGARScoreID", DbType.Int64, 0);
                                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                                        int intStatus2 = dbServer.ExecuteNonQuery(command2);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    long FetalHeartCount = 0;
                    if (BizActionObj.PregnancySacsList.Count > 0)
                    {
                        long PregnancySacId = 0;
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySac");
                        dbServer.AddInParameter(command1, "OutcomeID", DbType.Int64, BizActionObj.OutcomeDetails.ID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.OutcomeDetails.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "NoOfSacs", DbType.Int64, BizActionObj.OutcomeDetails.NoOfSacs);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (BizActionObj.OutcomePregnancySacList.Count > 0)
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.OutcomePregnancySacList[0].ID);
                        else
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, PregnancySacId);
                        dbServer.AddInParameter(command1, "SacsObservationDate", DbType.DateTime, BizActionObj.OutcomeDetails.SacsObservationDate);
                        dbServer.AddInParameter(command1, "PregnancyAchievedID", DbType.Int64, BizActionObj.OutcomeDetails.PregnancyAchievedID);
                        dbServer.AddInParameter(command1, "SacRemark", DbType.String, BizActionObj.OutcomeDetails.SacRemarks);
                        dbServer.AddInParameter(command1, "SurrogateTypeID", DbType.Int64, BizActionObj.OutcomeDetails.SurrogateTypeID);
                        dbServer.AddInParameter(command1, "SurrogateID", DbType.Int64, BizActionObj.OutcomeDetails.SurrogateID);
                        dbServer.AddInParameter(command1, "SurrogateUnitID", DbType.Int64, BizActionObj.OutcomeDetails.SurrogateUnitID);
                        dbServer.AddInParameter(command1, "SurrogatePatientMrNo", DbType.String, BizActionObj.OutcomeDetails.SurrogatePatientMrNo);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1);
                        PregnancySacId = (long)dbServer.GetParameterValue(command1, "ID");

                        foreach (var item in BizActionObj.PregnancySacsList)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePregnancySacDetails");
                            dbServer.AddInParameter(command2, "OutcomeID", DbType.Int64, BizActionObj.OutcomeDetails.ID);
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PregnancySacID", DbType.Int64, PregnancySacId);
                            dbServer.AddInParameter(command2, "SacNoStr", DbType.String, item.SaceNoStr);
                            dbServer.AddInParameter(command2, "IsFetalheart", DbType.Boolean, item.IsFetalHeart);
                            if (item.IsFetalHeart == true)
                                FetalHeartCount = FetalHeartCount + 1;
                            dbServer.AddInParameter(command2, "ResultID", DbType.Int64, item.ResultListID);
                            dbServer.AddInParameter(command2, "PregnancyID", DbType.Int64, item.PregnancyListID);
                            dbServer.AddInParameter(command2, "CongenitalAbnormalityYes", DbType.Boolean, item.CongenitalAbnormalityYes);
                            dbServer.AddInParameter(command2, "CongenitalAbnormalityNo", DbType.Boolean, item.CongenitalAbnormalityNo);
                            dbServer.AddInParameter(command2, "CongenitalAbnormalityReason", DbType.String, item.CongenitalAbnormalityReason);
                            dbServer.AddInParameter(command2, "FetalHeartCount", DbType.Int64, FetalHeartCount);
                            int intStatus2 = dbServer.ExecuteNonQuery(command2);
                        }

                    }

                    if (BizActionObj.OutcomeDetails.IsClosed == true)
                    {
                        if (FetalHeartCount > 0)
                        {
                            long BirthDetailID = 0;
                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetails");
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.OutcomeDetails.PatientID);
                            dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PatientUnitID);
                            dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyID);
                            dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.OutcomeDetails.PlanTherapyUnitID);
                            dbServer.AddInParameter(command1, "FetalHeartNo", DbType.Int64, FetalHeartCount);
                            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BirthDetailID);
                            int intStatus1 = dbServer.ExecuteNonQuery(command1);
                            BirthDetailID = (long)dbServer.GetParameterValue(command1, "ID");



                            //foreach (var item in BizActionObj.PregnancySacsList)
                            //{
                            for (int i = 0; i < FetalHeartCount; i++)
                            {
                                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetailsList");
                                dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command2, "BirthDetailsID", DbType.Int64, BirthDetailID);
                                dbServer.AddInParameter(command2, "ChildNoStr", DbType.String, "Child" + (i + 1));
                                dbServer.AddInParameter(command2, "BirthWeight", DbType.String, "");
                                dbServer.AddInParameter(command2, "MedicalConditions", DbType.String, "");
                                dbServer.AddInParameter(command2, "WeeksofGestation", DbType.String, "");
                                dbServer.AddInParameter(command2, "DeliveryTypeID", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "ActivityID", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "ActivityPoint", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "Pulse", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "PulsePoint", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "Grimace", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "GrimacePoint", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "Appearance", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "AppearancePoint", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "Respiration", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "RespirationPoint", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "APGARScore", DbType.Int64, 0);
                                dbServer.AddInParameter(command2, "Conclusion", DbType.Int64, null);
                                dbServer.AddInParameter(command2, "APGARScoreID", DbType.Int64, 0);
                                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                                int intStatus2 = dbServer.ExecuteNonQuery(command2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.OutcomeDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;
        }
        public override IValueObject GetOutcomeDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_GetOutcomeBizActionVO BizAction = valueObject as clsIVFDashboard_GetOutcomeBizActionVO;
            BizAction.PregnancySacsList = new List<clsPregnancySacsDetailsVO>();
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetOutcomeDetails");
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
                        BizAction.Details.BHCGAss1Date = Convert.ToDateTime(DALHelper.HandleDate(reader["BHCGAss1Date"]));
                        BizAction.Details.BHCGAss1IsBSCG = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"]));
                        BizAction.Details.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1BSCGValue"]));
                        BizAction.Details.BHCGAss1SrProgest = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1SrProgest"]));
                        BizAction.Details.BHCGAss2Date = Convert.ToDateTime(DALHelper.HandleDate(reader["BHCGAss2Date"]));
                        BizAction.Details.BHCGAss2IsBSCG = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BHCGAss2IsBSCG"]));
                        BizAction.Details.BHCGAss2BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2BSCGValue"]));
                        BizAction.Details.BHCGAss2USG = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2USG"]));
                        BizAction.Details.IsPregnancyAchieved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PregnancyAchieved"]));
                        BizAction.Details.PregnanacyConfirmDate = Convert.ToDateTime(DALHelper.HandleDate(reader["PregnanacyConfirmDate"]));
                        BizAction.Details.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        BizAction.Details.OutComeRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["OutComeRemarks"]));
                        BizAction.Details.OHSSRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OHSSRemark"]));
                        BizAction.Details.BiochemPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BiochemPregnancy"]));
                        BizAction.Details.Ectopic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ectopic"]));
                        BizAction.Details.Abortion = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Abortion"]));
                        BizAction.Details.Missed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Missed"]));
                        BizAction.Details.FetalHeartSound = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FetalHeartSound"]));
                        BizAction.Details.FetalDate = Convert.ToDateTime(DALHelper.HandleDate(reader["FetalDate"]));
                        BizAction.Details.Count = Convert.ToString(DALHelper.HandleDBNull(reader["Count"]));
                        BizAction.Details.Incomplete = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Incomplete"]));
                        BizAction.Details.IUD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IUD"]));
                        BizAction.Details.PretermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PretermDelivery"]));
                        BizAction.Details.LiveBirth = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LiveBirth"]));
                        BizAction.Details.Congenitalabnormality = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Congenitalabnormality"]));
                        BizAction.Details.IsChemicalPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChemicalPregnancy"]));
                        BizAction.Details.IsFullTermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFullTermDelivery"]));
                        BizAction.Details.OHSSEarly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSEarly"]));
                        BizAction.Details.OHSSLate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSLate"]));
                        BizAction.Details.OHSSMild = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMild"]));
                        BizAction.Details.OHSSMode = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMode"]));
                        BizAction.Details.OHSSSereve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSSereve"]));
                        BizAction.Details.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));

                        //added by neena
                        BizAction.Details.NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"]));
                        BizAction.Details.SacsObservationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["SacsObservationDate"]));
                        BizAction.Details.PregnancyAchievedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyAchievedID"]));
                        BizAction.Details.SacRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["SacRemark"]));
                        BizAction.Details.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));
                        //
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        clsIVFDashboard_OutcomeVO Obj = new clsIVFDashboard_OutcomeVO();
                        Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Obj.OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"]));
                        Obj.BHCGAss1Date = Convert.ToDateTime(DALHelper.HandleDate(reader["BHCGResultDate"]));
                        Obj.ResultListID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultID"]));
                        Obj.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGValue"]));
                        Obj.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                        Obj.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                        Obj.SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"]));
                        BizAction.OutcomeDetailsList.Add(Obj);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        ////clsIVFDashboard_OutcomeVO ObjNew = new clsIVFDashboard_OutcomeVO();
                        //BizAction.ObjOutcomePregnancySacList.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //BizAction.ObjOutcomePregnancySacList.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //BizAction.ObjOutcomePregnancySacList.OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"]));
                        //BizAction.ObjOutcomePregnancySacList.NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"]));
                        //BizAction.ObjOutcomePregnancySacList.SacsObservationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["SacsObservationDate"]));
                        //BizAction.ObjOutcomePregnancySacList.PregnancyAchievedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyAchievedID"]));
                        //BizAction.ObjOutcomePregnancySacList.SacRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["SacRemark"]));
                        //BizAction.ObjOutcomePregnancySacList.SurrogateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateTypeID"]));
                        //BizAction.ObjOutcomePregnancySacList.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                        //BizAction.ObjOutcomePregnancySacList.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                        //BizAction.ObjOutcomePregnancySacList.SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"]));
                        //BizAction.ObjOutcomePregnancySacList.FetalHeartCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartCount"]));
                        //BizAction.ObjOutcomePregnancySacList.IsUnlinkSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUnlinkSurrogate"]));
                        ////BizAction.OutcomePregnancySacList.Add(ObjNew);

                        clsIVFDashboard_OutcomeVO ObjNew = new clsIVFDashboard_OutcomeVO();
                        ObjNew.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjNew.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ObjNew.OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"]));
                        ObjNew.NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"]));
                        ObjNew.SacsObservationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["SacsObservationDate"]));
                        ObjNew.PregnancyAchievedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyAchievedID"]));
                        ObjNew.SacRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["SacRemark"]));
                        ObjNew.SurrogateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateTypeID"]));
                        ObjNew.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                        ObjNew.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                        ObjNew.SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"]));
                        ObjNew.FetalHeartCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartCount"]));
                        ObjNew.IsUnlinkSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUnlinkSurrogate"]));
                        BizAction.OutcomePregnancySacList.Add(ObjNew);
                    }


                    reader.NextResult();

                    while (reader.Read())
                    {
                        clsPregnancySacsDetailsVO ObjNew = new clsPregnancySacsDetailsVO();
                        ObjNew.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjNew.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ObjNew.PregnancySacID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancySacID"]));
                        ObjNew.SaceNoStr = Convert.ToString(DALHelper.HandleDBNull(reader["SacNoStr"]));
                        ObjNew.IsFetalHeart = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFetalHeart"]));
                        ObjNew.ResultListID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultID"]));
                        ObjNew.PregnancyListID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyID"]));
                        ObjNew.CongenitalAbnormalityYes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CongenitalAbnormalityYes"]));
                        ObjNew.CongenitalAbnormalityNo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CongenitalAbnormalityNo"]));
                        ObjNew.CongenitalAbnormalityReason = Convert.ToString(DALHelper.HandleDBNull(reader["CongenitalAbnormalityReason"]));
                        ObjNew.OutcomeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OutcomeID"]));
                        //ObjNew.NoOfSacs = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfSacs"]));
                        //ObjNew.SacsObservationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["SacsObservationDate"]));
                        //ObjNew.PregnancyAchievedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PregnancyAchievedID"]));
                        //ObjNew.SacRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["SacRemarks"]));
                        ObjNew.SurrogateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateTypeID"]));
                        //ObjNew.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                        //ObjNew.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                        //ObjNew.SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"]));
                        //ObjNew.FetalHeartCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartCount"]));
                        //ObjNew.IsUnlinkSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUnlinkSurrogate"]));

                        BizAction.PregnancySacsList.Add(ObjNew);
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

        #region BirthDetails
        public override IValueObject AddUpdateBirthDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddBirthDetailsBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddBirthDetailsBizActionVO;


            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateBirthDetails");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.BirthDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.BirthDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.BirthDetails.TherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.BirthDetails.TherapyUnitID);
                dbServer.AddInParameter(command, "DateOfBirth", DbType.DateTime, BizActionObj.BirthDetails.DateOfBirth);
                dbServer.AddInParameter(command, "Week", DbType.Int64, BizActionObj.BirthDetails.Week);
                dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.BirthDetails.GenderID);
                dbServer.AddInParameter(command, "TownOfBirth", DbType.String, BizActionObj.BirthDetails.TownOfBirth);
                dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.BirthDetails.Country));
                dbServer.AddInParameter(command, "CountryOfBirthID", DbType.Int64, BizActionObj.BirthDetails.CountryOfBirthID);
                dbServer.AddInParameter(command, "DiedPerinatallyID", DbType.Int64, BizActionObj.BirthDetails.DiedPerinatallyID);
                dbServer.AddInParameter(command, "DeathPostportumID", DbType.Int64, BizActionObj.BirthDetails.DeathPostportumID);
                dbServer.AddInParameter(command, "ChildID", DbType.Int64, BizActionObj.BirthDetails.ChildID);
                dbServer.AddInParameter(command, "ConditionID", DbType.Int64, BizActionObj.BirthDetails.ConditionID);
                dbServer.AddInParameter(command, "DeliveryMethodID", DbType.Int64, BizActionObj.BirthDetails.DeliveryMethodID);
                dbServer.AddInParameter(command, "WeightAtBirth", DbType.Single, BizActionObj.BirthDetails.WeightAtBirth);
                dbServer.AddInParameter(command, "LengthAtBirth", DbType.Single, BizActionObj.BirthDetails.LengthAtBirth);
                dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.BirthDetails.FirstName));
                dbServer.AddInParameter(command, "Surname", DbType.String, Security.base64Encode(BizActionObj.BirthDetails.Surname));
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.BirthDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.BirthDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.BirthDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }
        public override IValueObject GetBirthDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();
            clsIVFDashboard_GetBirthDetailsBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetBirthDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetBirthDetails");

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.BirthDetails.TherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.BirthDetails.TherapyUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.BirthDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.BirthDetails.PatientUnitID);
                BizAction.BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_BirthDetailsVO Details = new clsIVFDashboard_BirthDetailsVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        Details.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        Details.DateOfBirth = (DateTime?)(DALHelper.HandleDate(reader["DateOfBirth"]));
                        Details.Week = Convert.ToInt64(DALHelper.HandleDBNull(reader["Week"]));
                        Details.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        Details.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        Details.ConditionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConditionID"]));
                        Details.Condition = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"]));
                        //Details.CountryOfBirthID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CountryOfBirthID"]));
                        Details.Country = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Country"])));
                        Details.TownOfBirth = Convert.ToString(DALHelper.HandleDBNull(reader["TownOfBirth"]));
                        Details.DeathPostportumID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeathPostportumID"]));
                        Details.DeathPostportum = Convert.ToString(DALHelper.HandleDBNull(reader["DeathPostportum"]));
                        Details.DeliveryMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeliveryMethodID"]));
                        Details.DeliveryMethod = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryMethod"]));
                        Details.DiedPerinatallyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiedPerinatallyID"]));
                        Details.DiedPerinatally = Convert.ToString(DALHelper.HandleDBNull(reader["DiedPerinatally"]));
                        Details.Child = Convert.ToString(DALHelper.HandleDBNull(reader["Child"]));
                        Details.ChildID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChildID"]));
                        Details.WeightAtBirth = Convert.ToSingle(DALHelper.HandleDBNull(reader["WeightAtBirth"]));
                        Details.LengthAtBirth = Convert.ToSingle(DALHelper.HandleDBNull(reader["LengthAtBirth"]));
                        Details.FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])));
                        Details.Surname = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["Surname"])));
                        Details.IdentityNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IdentityNumber"]));
                        BizAction.BirthDetailsList.Add(Details);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }
        public override IValueObject DeleteBirthDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_DeleteBirthDetailsBizActionVO BizActionObj = valueObject as clsIVFDashboard_DeleteBirthDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_BirthDetails_New where ID =" + BizActionObj.BirthDetails.ID + " and UnitID=" + UserVo.UserLoginInfo.UnitId);
                int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.BirthDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }

        #endregion

        #region Document
        public override IValueObject AddDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddTherapyDocumentBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddTherapyDocumentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddTherapyDocument");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, BizActionObj.Details.Date);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.Details.Description);
                dbServer.AddInParameter(command, "Title", DbType.String, BizActionObj.Details.Title);
                dbServer.AddInParameter(command, "AttachedFileName", DbType.String, BizActionObj.Details.AttachedFileName);
                dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, BizActionObj.Details.AttachedFileContent);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Details.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return valueObject;

        }
        public override IValueObject GetDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_GetTherapyDocumentBizActionVO BizActionObj = valueObject as clsIVFDashboard_GetTherapyDocumentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_GetTherapyDocument");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DetailList == null)
                        BizActionObj.DetailList = new List<clsIVFDashboard_TherapyDocumentVO>();
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        BizActionObj.DetailList.Add(Details);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return BizActionObj;

        }
        public override IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_DeleteTherapyDocumentBizActionVO BizActionObj = valueObject as clsIVFDashboard_DeleteTherapyDocumentBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_UpdateLabDaysDocListStatus");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Details.OocyteNumber);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "Day", DbType.Int64, BizActionObj.Details.Day);
                dbServer.AddInParameter(command, "DocNo", DbType.String, BizActionObj.Details.DocNo);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.Details.ID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.DetailsList = new List<clsIVFDashboard_TherapyDocumentVO>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocyteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizActionObj.DetailsList.Add(Details);
                    }
                }
                reader.Close();


                //commented by neena
                //DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from  T_IVFDAshBoard_Document where ID =" + BizActionObj.Details.ID + " and UnitID=" + UserVo.UserLoginInfo.UnitId);
                //int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);
                //

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }

        //public override IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    // throw new NotImplementedException();
        //    clsIVFDashboard_DeleteTherapyDocumentBizActionVO BizActionObj = valueObject as clsIVFDashboard_DeleteTherapyDocumentBizActionVO;
        //    DbTransaction trans = null;
        //    DbConnection con = null;

        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();
        //        DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from  T_IVFDAshBoard_Document where ID =" + BizActionObj.Details.ID + " and UnitID=" + UserVo.UserLoginInfo.UnitId);
        //        int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);
        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        BizActionObj.Details = null;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        trans = null;
        //        con = null;
        //    }

        //    return BizActionObj;
        //}

        #endregion

        #region For CycleCode
        public override IValueObject GetCycleCodeForCombobox(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            bool CurrentMethodExecutionStatus = true;
            GetCycleCodeForPatientBizActionVO BizActionObj = (GetCycleCodeForPatientBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCycleCodeForCombobox");

                dbServer.AddInParameter(command, "CoupleID", DbType.String, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.String, BizActionObj.CoupleUnitID);
                BizActionObj.List = new List<string>();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // BizActionObj.List.Add((string)(reader[BizActionObj.CycleCode].ToString()));
                        BizActionObj.List.Add(reader["CycleCode"].ToString());
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return BizActionObj;
        }
        public override IValueObject GetTherapyDetailsFromCycleCode(IValueObject valueObject, clsUserVO UserVo)
        {
            GetCycleDetailsFromCycleCodeBizActionVO BizActionObj = (GetCycleDetailsFromCycleCodeBizActionVO)valueObject;
            bool CurrentMethodExecutionStatus = true;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCycleDetailsFromCycleCode");

                dbServer.AddInParameter(command, "CycleCode", DbType.String, BizActionObj.CycleCode);
                BizActionObj.TherapyDetails = new clsPlanTherapyVO();
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizActionObj.TherapyDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.TherapyDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.TherapyDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizActionObj.TherapyDetails.TherapyStartDate = (DateTime?)(DALHelper.HandleDate(reader["TherapyStartDate"]));
                        BizActionObj.TherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                        BizActionObj.TherapyDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                        BizActionObj.TherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                        BizActionObj.TherapyDetails.Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"]));
                        BizActionObj.TherapyDetails.PillStartDate = (DateTime?)(DALHelper.HandleDate(reader["PillStartDate"]));
                        BizActionObj.TherapyDetails.PillEndDate = (DateTime?)(DALHelper.HandleDate(reader["PillEndDate"]));
                        BizActionObj.TherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                        BizActionObj.TherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                        BizActionObj.TherapyDetails.PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
                        BizActionObj.TherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                        BizActionObj.TherapyDetails.PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"]));
                        BizActionObj.TherapyDetails.TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"]));
                        BizActionObj.TherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                        BizActionObj.TherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                        BizActionObj.TherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                        BizActionObj.TherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                        BizActionObj.TherapyDetails.MainInduction = Convert.ToString(DALHelper.HandleDBNull(reader["MainIndication"]));
                        BizActionObj.TherapyDetails.PlannedEmbryos = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return BizActionObj;

        }
        #endregion

        #region Patient List
        public override IValueObject GetPatientListForDashboard(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            GetPatientListForDashboardBizActionVO BizActionObj = valueObject as GetPatientListForDashboardBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientListForDashboard");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "SearchKeyword", DbType.String, BizActionObj.SearchKeyword);

                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                //    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(BizActionObj.FamilyName));
                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                {
                    if (BizActionObj.VisitWise == true)
                        dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                    else if (BizActionObj.AdmissionWise == true)
                        dbServer.AddInParameter(command, "AdmissionNo", DbType.String, BizActionObj.OPDNo + "%");
                }

                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                if (BizActionObj.Country != null && BizActionObj.Country.Length != 0)
                    dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.Country) + "%");
                if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                if (BizActionObj.Pincode != null && BizActionObj.Pincode.Length != 0)
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(BizActionObj.Pincode) + "%");
                if (BizActionObj.CivilID != null && BizActionObj.CivilID.Length != 0)
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(BizActionObj.CivilID) + "%");
                if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                if (BizActionObj.PatientCategoryID != null && BizActionObj.PatientCategoryID != 0)
                    dbServer.AddInParameter(command, "PatientCategory", DbType.Int64, BizActionObj.PatientCategoryID);

                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        //if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }

                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }

                if (BizActionObj.VisitWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);

                    if (BizActionObj.VisitFromDate != null)
                        dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    if (BizActionObj.VisitToDate != null)
                    {
                        if (BizActionObj.VisitFromDate != null)
                        {
                            BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                    }
                }

                if (BizActionObj.AdmissionWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);

                    if (BizActionObj.AdmissionFromDate != null)
                        dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                    if (BizActionObj.AdmissionToDate != null)
                    {
                        if (BizActionObj.AdmissionToDate != null)
                        {
                            BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                    }
                }
                //=====================================================
                if (BizActionObj.DOBWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);

                    if (BizActionObj.DOBFromDate != null)
                        dbServer.AddInParameter(command, "DOBFromDate", DbType.DateTime, BizActionObj.DOBFromDate);
                    if (BizActionObj.DOBToDate != null)
                    {
                        if (BizActionObj.DOBToDate != null)
                        {
                            BizActionObj.DOBToDate = BizActionObj.DOBToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "DOBToDate", DbType.DateTime, BizActionObj.DOBToDate);
                    }
                }
                //=====================================================

                if (BizActionObj.IsLoyaltyMember == true)
                {
                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, BizActionObj.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizActionObj.LoyaltyProgramID);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);



                if (BizActionObj.SearchInAnotherClinic == true)
                {
                    dbServer.AddInParameter(command, "SearchInAnotherClinic", DbType.Boolean, BizActionObj.SearchInAnotherClinic);
                }

                //dbServer.AddInParameter(command, "IncludeSpouse", DbType.Boolean, BizActionObj.IncludeSpouse);

                dbServer.AddInParameter(command, "IsDonorLink", DbType.Boolean, BizActionObj.IsDonorLink);  //***//

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();

                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        //objPatientVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        //objPatientVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        //objPatientVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));

                        objPatientVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        objPatientVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        objPatientVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);

                        objPatientVO.PatientName = objPatientVO.FirstName + " " + objPatientVO.MiddleName + " " + objPatientVO.MiddleName;
                        objPatientVO.DateOfBirth = (DateTime?)DALHelper.HandleDate(reader["DateofBirth"]);
                        objPatientVO.RegistrationDate = (DateTime)DALHelper.HandleDate(reader["RegistrationDate"]);
                        if (BizActionObj.VisitWise == true || BizActionObj.RegistrationWise == true)
                        {
                            objPatientVO.VisitID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.OPDNO = (string)DALHelper.HandleDBNull(reader["OINO"]);

                        }
                        else
                        {
                            objPatientVO.IPDAdmissionID = (long)DALHelper.HandleDBNull(reader["VAID"]);
                            objPatientVO.IPDAdmissionNo = (string)DALHelper.HandleDBNull(reader["OINO"]);
                        }

                        if (BizActionObj.VisitWise == true)
                            objPatientVO.PatientKind = PatientsKind.OPD;
                        else if (BizActionObj.AdmissionWise == true)
                            objPatientVO.PatientKind = PatientsKind.IPD;
                        else if (objPatientVO.VisitID == 0 && objPatientVO.IPDAdmissionID == 0)
                            objPatientVO.PatientKind = PatientsKind.Registration;

                        objPatientVO.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objPatientVO.ContactNO1 = (string)DALHelper.HandleDBNull(reader["ContactNO1"]);
                        objPatientVO.Gender = (string)DALHelper.HandleDBNull(reader["Gender"]);
                        objPatientVO.MaritalStatus = (string)DALHelper.HandleDBNull(reader["MaritalStatus"]);
                        objPatientVO.UniversalID = Security.base64Decode((string)DALHelper.HandleDBNull(reader["CivilID"]));
                        objPatientVO.PatientTypeID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        objPatientVO.SearchKeyword = Convert.ToString(DALHelper.HandleDBNull(reader["SearchKeyword"]));       // ..................BHUSHAN
                        objPatientVO.AgentName = (string)DALHelper.HandleDBNull(reader["AgentName"]);  //***//
                        //objPatientVO.SpouseID = (long)DALHelper.HandleDBNull(reader["SpouseID"]); 
                        objPatientVO.LinkServer = BizActionObj.LinkServer;
                        BizActionObj.PatientDetailsList.Add(objPatientVO);
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["SearchKeyword"].ToString()));
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                con.Close();
                con = null;
            }
            return valueObject;
        }


        public override IValueObject GetSearchKeywordforPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            GetSearchkeywordForPatientBizActionVO BizActionObj = valueObject as GetSearchkeywordForPatientBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientListForDashboard");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "MrNo", DbType.String, BizActionObj.MRNo);
                dbServer.AddInParameter(command, "SearchKeyword", DbType.String, BizActionObj.SearchKeyword);

                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                //    dbServer.AddInParameter(command, "FirstName", DbType.String, Security.base64Encode(BizActionObj.FirstName) + "%");
                //if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                //    dbServer.AddInParameter(command, "MiddleName", DbType.String, Security.base64Encode(BizActionObj.MiddleName) + "%");
                //if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                //    dbServer.AddInParameter(command, "LastName", DbType.String, Security.base64Encode(BizActionObj.LastName) + "%");
                //if (BizActionObj.FirstName != null && BizActionObj.FirstName.Length != 0)
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                if (BizActionObj.MiddleName != null && BizActionObj.MiddleName.Length != 0)
                    dbServer.AddInParameter(command, "MiddleName", DbType.String, BizActionObj.MiddleName);
                if (BizActionObj.LastName != null && BizActionObj.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)

                    if (BizActionObj.FamilyName != null && BizActionObj.FamilyName.Length != 0)
                        dbServer.AddInParameter(command, "FamilyName", DbType.String, Security.base64Encode(BizActionObj.FamilyName));
                if (BizActionObj.OPDNo != null && BizActionObj.OPDNo.Length != 0)
                {
                    if (BizActionObj.VisitWise == true)
                        dbServer.AddInParameter(command, "OPDNo", DbType.String, BizActionObj.OPDNo + "%");
                    else if (BizActionObj.AdmissionWise == true)
                        dbServer.AddInParameter(command, "AdmissionNo", DbType.String, BizActionObj.OPDNo + "%");
                }

                if (BizActionObj.ContactNo != null && BizActionObj.ContactNo.Length != 0)
                    dbServer.AddInParameter(command, "ContactNo", DbType.String, BizActionObj.ContactNo + "%");
                if (BizActionObj.Country != null && BizActionObj.Country.Length != 0)
                    dbServer.AddInParameter(command, "Country", DbType.String, Security.base64Encode(BizActionObj.Country) + "%");
                if (BizActionObj.State != null && BizActionObj.State.Length != 0)
                    dbServer.AddInParameter(command, "State", DbType.String, Security.base64Encode(BizActionObj.State) + "%");
                if (BizActionObj.City != null && BizActionObj.City.Length != 0)
                    dbServer.AddInParameter(command, "City", DbType.String, Security.base64Encode(BizActionObj.City) + "%");
                if (BizActionObj.Pincode != null && BizActionObj.Pincode.Length != 0)
                    dbServer.AddInParameter(command, "Pincode", DbType.String, Security.base64Encode(BizActionObj.Pincode) + "%");
                if (BizActionObj.CivilID != null && BizActionObj.CivilID.Length != 0)
                    dbServer.AddInParameter(command, "CivilID", DbType.String, Security.base64Encode(BizActionObj.CivilID) + "%");
                if (BizActionObj.GenderID != null && BizActionObj.GenderID != 0)
                    dbServer.AddInParameter(command, "GenderID", DbType.Int64, BizActionObj.GenderID);
                if (BizActionObj.PatientCategoryID != null && BizActionObj.PatientCategoryID != 0)
                    dbServer.AddInParameter(command, "PatientCategory", DbType.Int64, BizActionObj.PatientCategoryID);

                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        //if (BizActionObj.FromDate.Equals(BizActionObj.ToDate))
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }

                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }

                if (BizActionObj.VisitWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)1);

                    if (BizActionObj.VisitFromDate != null)
                        dbServer.AddInParameter(command, "VisitFromDate", DbType.DateTime, BizActionObj.VisitFromDate);
                    if (BizActionObj.VisitToDate != null)
                    {
                        if (BizActionObj.VisitFromDate != null)
                        {
                            BizActionObj.VisitToDate = BizActionObj.VisitToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "VisitToDate", DbType.DateTime, BizActionObj.VisitToDate);
                    }
                }

                if (BizActionObj.AdmissionWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)2);

                    if (BizActionObj.AdmissionFromDate != null)
                        dbServer.AddInParameter(command, "AdmissionFromDate", DbType.DateTime, BizActionObj.AdmissionFromDate);
                    if (BizActionObj.AdmissionToDate != null)
                    {
                        if (BizActionObj.AdmissionToDate != null)
                        {
                            BizActionObj.AdmissionToDate = BizActionObj.AdmissionToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "AdmissionToDate", DbType.DateTime, BizActionObj.AdmissionToDate);
                    }
                }
                //=====================================================
                if (BizActionObj.DOBWise == true)
                {
                    dbServer.AddInParameter(command, "Serchfor", DbType.Int16, (Int16)0);

                    if (BizActionObj.DOBFromDate != null)
                        dbServer.AddInParameter(command, "DOBFromDate", DbType.DateTime, BizActionObj.DOBFromDate);
                    if (BizActionObj.DOBToDate != null)
                    {
                        if (BizActionObj.DOBToDate != null)
                        {
                            BizActionObj.DOBToDate = BizActionObj.DOBToDate.Value.Date.AddDays(1);
                        }
                        dbServer.AddInParameter(command, "DOBToDate", DbType.DateTime, BizActionObj.DOBToDate);
                    }
                }
                //=====================================================

                if (BizActionObj.IsLoyaltyMember == true)
                {
                    dbServer.AddInParameter(command, "IsLoyaltyMember", DbType.Boolean, BizActionObj.IsLoyaltyMember);
                    dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizActionObj.LoyaltyProgramID);
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);



                if (BizActionObj.SearchInAnotherClinic == true)
                {
                    dbServer.AddInParameter(command, "SearchInAnotherClinic", DbType.Boolean, BizActionObj.SearchInAnotherClinic);
                }

                //dbServer.AddInParameter(command, "IncludeSpouse", DbType.Boolean, BizActionObj.IncludeSpouse);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "LinkServer", DbType.String, BizActionObj.LinkServer);
                if (BizActionObj.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, BizActionObj.LinkServer.Replace(@"\", "_"));

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                        BizActionObj.MasterList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["SearchKeyword"].ToString()));
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");

                reader.Close();


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                con.Close();
                con = null;
            }
            return valueObject;
        }
        #endregion

        public override IValueObject UpdateTherapyExecution(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateTherapyExecutionBizActionVO BizActionObj = valueObject as clsUpdateTherapyExecutionBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_UpdateTherapyExecution");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TherapyExecutionDetial.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.TherapyExecutionDetial.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.TherapyExecutionDetial.CoupleUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.TherapyExecutionDetial.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.TherapyExecutionDetial.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, BizActionObj.TherapyExecutionDetial.PlanTherapyId);
                dbServer.AddInParameter(command, "PlanTherapyUnitId", DbType.Int64, BizActionObj.TherapyExecutionDetial.PlanTherapyUnitId);
                dbServer.AddInParameter(command, "TherapyTypeId", DbType.Int64, BizActionObj.TherapyExecutionDetial.TherapyTypeId);
                dbServer.AddInParameter(command, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyExecutionDetial.ThearpyTypeDetailId);
                dbServer.AddInParameter(command, "PhysicianID", DbType.Int64, BizActionObj.TherapyExecutionDetial.PhysicianID);
                dbServer.AddInParameter(command, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyExecutionDetial.TherapyStartDate);
                dbServer.AddInParameter(command, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyExecutionDetial.IsSurrogate);
                dbServer.AddInParameter(command, "IsActive", DbType.Boolean, true);
                dbServer.AddInParameter(command, "Day", DbType.String, BizActionObj.TherapyExecutionDetial.Day);
                dbServer.AddInParameter(command, "Day1", DbType.String, BizActionObj.TherapyExecutionDetial.Day1);
                dbServer.AddInParameter(command, "Day2", DbType.String, BizActionObj.TherapyExecutionDetial.Day2);
                dbServer.AddInParameter(command, "Day3", DbType.String, BizActionObj.TherapyExecutionDetial.Day3);
                dbServer.AddInParameter(command, "Day4", DbType.String, BizActionObj.TherapyExecutionDetial.Day4);
                dbServer.AddInParameter(command, "Day5", DbType.String, BizActionObj.TherapyExecutionDetial.Day5);
                dbServer.AddInParameter(command, "Day6", DbType.String, BizActionObj.TherapyExecutionDetial.Day6);
                dbServer.AddInParameter(command, "Day7", DbType.String, BizActionObj.TherapyExecutionDetial.Day7);
                dbServer.AddInParameter(command, "Day8", DbType.String, BizActionObj.TherapyExecutionDetial.Day8);
                dbServer.AddInParameter(command, "Day9", DbType.String, BizActionObj.TherapyExecutionDetial.Day9);
                dbServer.AddInParameter(command, "Day10", DbType.String, BizActionObj.TherapyExecutionDetial.Day10);
                dbServer.AddInParameter(command, "Day11", DbType.String, BizActionObj.TherapyExecutionDetial.Day11);
                dbServer.AddInParameter(command, "Day12", DbType.String, BizActionObj.TherapyExecutionDetial.Day12);
                dbServer.AddInParameter(command, "Day13", DbType.String, BizActionObj.TherapyExecutionDetial.Day13);
                dbServer.AddInParameter(command, "Day14", DbType.String, BizActionObj.TherapyExecutionDetial.Day14);
                dbServer.AddInParameter(command, "Day15", DbType.String, BizActionObj.TherapyExecutionDetial.Day15);
                dbServer.AddInParameter(command, "Day16", DbType.String, BizActionObj.TherapyExecutionDetial.Day16);
                dbServer.AddInParameter(command, "Day17", DbType.String, BizActionObj.TherapyExecutionDetial.Day17);
                dbServer.AddInParameter(command, "Day18", DbType.String, BizActionObj.TherapyExecutionDetial.Day18);
                dbServer.AddInParameter(command, "Day19", DbType.String, BizActionObj.TherapyExecutionDetial.Day19);
                dbServer.AddInParameter(command, "Day20", DbType.String, BizActionObj.TherapyExecutionDetial.Day20);
                dbServer.AddInParameter(command, "Day21", DbType.String, BizActionObj.TherapyExecutionDetial.Day21);
                dbServer.AddInParameter(command, "Day22", DbType.String, BizActionObj.TherapyExecutionDetial.Day22);
                dbServer.AddInParameter(command, "Day23", DbType.String, BizActionObj.TherapyExecutionDetial.Day23);
                dbServer.AddInParameter(command, "Day24", DbType.String, BizActionObj.TherapyExecutionDetial.Day24);
                dbServer.AddInParameter(command, "Day25", DbType.String, BizActionObj.TherapyExecutionDetial.Day25);
                dbServer.AddInParameter(command, "Day26", DbType.String, BizActionObj.TherapyExecutionDetial.Day26);
                dbServer.AddInParameter(command, "Day27", DbType.String, BizActionObj.TherapyExecutionDetial.Day27);
                dbServer.AddInParameter(command, "Day28", DbType.String, BizActionObj.TherapyExecutionDetial.Day28);
                dbServer.AddInParameter(command, "Day29", DbType.String, BizActionObj.TherapyExecutionDetial.Day29);
                dbServer.AddInParameter(command, "Day30", DbType.String, BizActionObj.TherapyExecutionDetial.Day30);
                dbServer.AddInParameter(command, "Day31", DbType.String, BizActionObj.TherapyExecutionDetial.Day31);
                dbServer.AddInParameter(command, "Day32", DbType.String, BizActionObj.TherapyExecutionDetial.Day32);
                dbServer.AddInParameter(command, "Day33", DbType.String, BizActionObj.TherapyExecutionDetial.Day33);
                dbServer.AddInParameter(command, "Day34", DbType.String, BizActionObj.TherapyExecutionDetial.Day34);
                dbServer.AddInParameter(command, "Day35", DbType.String, BizActionObj.TherapyExecutionDetial.Day35);
                dbServer.AddInParameter(command, "Day36", DbType.String, BizActionObj.TherapyExecutionDetial.Day36);
                dbServer.AddInParameter(command, "Day37", DbType.String, BizActionObj.TherapyExecutionDetial.Day37);
                dbServer.AddInParameter(command, "Day38", DbType.String, BizActionObj.TherapyExecutionDetial.Day38);
                dbServer.AddInParameter(command, "Day39", DbType.String, BizActionObj.TherapyExecutionDetial.Day39);
                dbServer.AddInParameter(command, "Day40", DbType.String, BizActionObj.TherapyExecutionDetial.Day40);
                dbServer.AddInParameter(command, "Day41", DbType.String, BizActionObj.TherapyExecutionDetial.Day41);
                dbServer.AddInParameter(command, "Day42", DbType.String, BizActionObj.TherapyExecutionDetial.Day42);
                dbServer.AddInParameter(command, "Day43", DbType.String, BizActionObj.TherapyExecutionDetial.Day43);
                dbServer.AddInParameter(command, "Day44", DbType.String, BizActionObj.TherapyExecutionDetial.Day44);
                dbServer.AddInParameter(command, "Day45", DbType.String, BizActionObj.TherapyExecutionDetial.Day45);
                dbServer.AddInParameter(command, "Day46", DbType.String, BizActionObj.TherapyExecutionDetial.Day46);
                dbServer.AddInParameter(command, "Day47", DbType.String, BizActionObj.TherapyExecutionDetial.Day47);
                dbServer.AddInParameter(command, "Day48", DbType.String, BizActionObj.TherapyExecutionDetial.Day48);
                dbServer.AddInParameter(command, "Day49", DbType.String, BizActionObj.TherapyExecutionDetial.Day49);
                dbServer.AddInParameter(command, "Day50", DbType.String, BizActionObj.TherapyExecutionDetial.Day50);
                dbServer.AddInParameter(command, "Day51", DbType.String, BizActionObj.TherapyExecutionDetial.Day51);
                dbServer.AddInParameter(command, "Day52", DbType.String, BizActionObj.TherapyExecutionDetial.Day52);
                dbServer.AddInParameter(command, "Day53", DbType.String, BizActionObj.TherapyExecutionDetial.Day53);
                dbServer.AddInParameter(command, "Day54", DbType.String, BizActionObj.TherapyExecutionDetial.Day54);
                dbServer.AddInParameter(command, "Day55", DbType.String, BizActionObj.TherapyExecutionDetial.Day55);
                dbServer.AddInParameter(command, "Day56", DbType.String, BizActionObj.TherapyExecutionDetial.Day56);
                dbServer.AddInParameter(command, "Day57", DbType.String, BizActionObj.TherapyExecutionDetial.Day57);
                dbServer.AddInParameter(command, "Day58", DbType.String, BizActionObj.TherapyExecutionDetial.Day58);
                dbServer.AddInParameter(command, "Day59", DbType.String, BizActionObj.TherapyExecutionDetial.Day59);
                dbServer.AddInParameter(command, "Day60", DbType.String, BizActionObj.TherapyExecutionDetial.Day60);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyExecutionDetial = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;
        }

        public clsTherapyExecutionVO TherapyExcecutionSurrogate(clsIVFDashboard_GetTherapyListBizActionVO BizAction, DbDataReader reader)
        {

            clsTherapyExecutionVO TherapyExeSurrogate = new clsTherapyExecutionVO();
            TherapyExeSurrogate.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            TherapyExeSurrogate.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
            TherapyExeSurrogate.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
            TherapyExeSurrogate.TherapyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyTypeId"]));
            TherapyExeSurrogate.PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
            TherapyExeSurrogate.TherapyStartDate = (DateTime?)(DALHelper.HandleDate(reader["TherapyStartDate"]));
            TherapyExeSurrogate.ThearpyTypeDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"]));
            #region Set Proprties According to Therpy Group
            if ((int)TherapyGroup.Event == (int)TherapyExeSurrogate.TherapyTypeId)
            {
                TherapyExeSurrogate.TherapyGroup = TherapyGroup.Event.ToString();
                TherapyExeSurrogate.Head = "Date of LMP";
                TherapyExeSurrogate.IsBool = true;
                TherapyExeSurrogate.IsText = false;
            }
            else if ((int)TherapyGroup.Drug == (int)TherapyExeSurrogate.TherapyTypeId)
            {
                TherapyExeSurrogate.TherapyGroup = TherapyGroup.Drug.ToString();
                TherapyExeSurrogate.IsBool = false;
                TherapyExeSurrogate.IsText = true;
                TherapyExeSurrogate.Head = Convert.ToString((DALHelper.HandleDBNull(reader["DrugName"])));
            }
            //else if ((int)TherapyGroup.UltraSound == (int)TherapyExe.TherapyTypeId)
            //{
            //    TherapyExe.TherapyGroup = TherapyGroup.UltraSound.ToString();
            //    TherapyExe.Head = "Follicular US";
            //    TherapyExe.IsBool = true;
            //    TherapyExe.IsText = false;
            //}

            //else if ((int)TherapyGroup.OvumPickUp == (int)TherapyExe.TherapyTypeId)
            //{
            //    TherapyExe.TherapyGroup = TherapyGroup.OvumPickUp.ToString();
            //    TherapyExe.Head = "OPU";
            //    TherapyExe.IsBool = true;
            //    TherapyExe.IsText = false;
            //}
            else if ((int)TherapyGroup.EmbryoTransfer == (int)TherapyExeSurrogate.TherapyTypeId)
            {
                TherapyExeSurrogate.TherapyGroup = TherapyGroup.EmbryoTransfer.ToString();
                TherapyExeSurrogate.Head = "ET";
                TherapyExeSurrogate.IsBool = true;
                TherapyExeSurrogate.IsText = false;
            }

            #endregion

            TherapyExeSurrogate.Day1 = Convert.ToString((DALHelper.HandleDBNull(reader["Day1"])));
            TherapyExeSurrogate.Day2 = Convert.ToString((DALHelper.HandleDBNull(reader["Day2"])));
            TherapyExeSurrogate.Day3 = Convert.ToString((DALHelper.HandleDBNull(reader["Day3"])));
            TherapyExeSurrogate.Day4 = Convert.ToString((DALHelper.HandleDBNull(reader["Day4"])));
            TherapyExeSurrogate.Day5 = Convert.ToString((DALHelper.HandleDBNull(reader["Day5"])));
            TherapyExeSurrogate.Day6 = Convert.ToString((DALHelper.HandleDBNull(reader["Day6"])));
            TherapyExeSurrogate.Day7 = Convert.ToString((DALHelper.HandleDBNull(reader["Day7"])));
            TherapyExeSurrogate.Day8 = Convert.ToString((DALHelper.HandleDBNull(reader["Day8"])));
            TherapyExeSurrogate.Day9 = Convert.ToString((DALHelper.HandleDBNull(reader["Day9"])));
            TherapyExeSurrogate.Day10 = Convert.ToString((DALHelper.HandleDBNull(reader["Day10"])));
            TherapyExeSurrogate.Day11 = Convert.ToString((DALHelper.HandleDBNull(reader["Day11"])));
            TherapyExeSurrogate.Day12 = Convert.ToString((DALHelper.HandleDBNull(reader["Day12"])));
            TherapyExeSurrogate.Day13 = Convert.ToString((DALHelper.HandleDBNull(reader["Day13"])));
            TherapyExeSurrogate.Day14 = Convert.ToString((DALHelper.HandleDBNull(reader["Day14"])));
            TherapyExeSurrogate.Day15 = Convert.ToString((DALHelper.HandleDBNull(reader["Day15"])));
            TherapyExeSurrogate.Day16 = Convert.ToString((DALHelper.HandleDBNull(reader["Day16"])));
            TherapyExeSurrogate.Day17 = Convert.ToString((DALHelper.HandleDBNull(reader["Day17"])));
            TherapyExeSurrogate.Day18 = Convert.ToString((DALHelper.HandleDBNull(reader["Day18"])));
            TherapyExeSurrogate.Day19 = Convert.ToString((DALHelper.HandleDBNull(reader["Day19"])));
            TherapyExeSurrogate.Day20 = Convert.ToString((DALHelper.HandleDBNull(reader["Day20"])));
            TherapyExeSurrogate.Day21 = Convert.ToString((DALHelper.HandleDBNull(reader["Day21"])));
            TherapyExeSurrogate.Day22 = Convert.ToString((DALHelper.HandleDBNull(reader["Day22"])));
            TherapyExeSurrogate.Day23 = Convert.ToString((DALHelper.HandleDBNull(reader["Day23"])));
            TherapyExeSurrogate.Day24 = Convert.ToString((DALHelper.HandleDBNull(reader["Day24"])));
            TherapyExeSurrogate.Day25 = Convert.ToString((DALHelper.HandleDBNull(reader["Day25"])));
            TherapyExeSurrogate.Day26 = Convert.ToString((DALHelper.HandleDBNull(reader["Day26"])));
            TherapyExeSurrogate.Day27 = Convert.ToString((DALHelper.HandleDBNull(reader["Day27"])));
            TherapyExeSurrogate.Day28 = Convert.ToString((DALHelper.HandleDBNull(reader["Day28"])));
            TherapyExeSurrogate.Day29 = Convert.ToString((DALHelper.HandleDBNull(reader["Day29"])));
            TherapyExeSurrogate.Day30 = Convert.ToString((DALHelper.HandleDBNull(reader["Day30"])));
            TherapyExeSurrogate.Day31 = Convert.ToString((DALHelper.HandleDBNull(reader["Day31"])));
            TherapyExeSurrogate.Day32 = Convert.ToString((DALHelper.HandleDBNull(reader["Day32"])));
            TherapyExeSurrogate.Day33 = Convert.ToString((DALHelper.HandleDBNull(reader["Day33"])));
            TherapyExeSurrogate.Day35 = Convert.ToString((DALHelper.HandleDBNull(reader["Day34"])));
            TherapyExeSurrogate.Day36 = Convert.ToString((DALHelper.HandleDBNull(reader["Day35"])));
            TherapyExeSurrogate.Day37 = Convert.ToString((DALHelper.HandleDBNull(reader["Day36"])));
            TherapyExeSurrogate.Day38 = Convert.ToString((DALHelper.HandleDBNull(reader["Day37"])));
            TherapyExeSurrogate.Day39 = Convert.ToString((DALHelper.HandleDBNull(reader["Day38"])));
            TherapyExeSurrogate.Day40 = Convert.ToString((DALHelper.HandleDBNull(reader["Day39"])));
            TherapyExeSurrogate.Day41 = Convert.ToString((DALHelper.HandleDBNull(reader["Day40"])));
            TherapyExeSurrogate.Day42 = Convert.ToString((DALHelper.HandleDBNull(reader["Day41"])));
            TherapyExeSurrogate.Day43 = Convert.ToString((DALHelper.HandleDBNull(reader["Day42"])));
            TherapyExeSurrogate.Day44 = Convert.ToString((DALHelper.HandleDBNull(reader["Day43"])));
            TherapyExeSurrogate.Day45 = Convert.ToString((DALHelper.HandleDBNull(reader["Day44"])));
            TherapyExeSurrogate.Day46 = Convert.ToString((DALHelper.HandleDBNull(reader["Day45"])));
            TherapyExeSurrogate.Day47 = Convert.ToString((DALHelper.HandleDBNull(reader["Day46"])));
            TherapyExeSurrogate.Day48 = Convert.ToString((DALHelper.HandleDBNull(reader["Day47"])));
            TherapyExeSurrogate.Day49 = Convert.ToString((DALHelper.HandleDBNull(reader["Day48"])));
            TherapyExeSurrogate.Day50 = Convert.ToString((DALHelper.HandleDBNull(reader["Day49"])));
            TherapyExeSurrogate.Day50 = Convert.ToString((DALHelper.HandleDBNull(reader["Day50"])));
            TherapyExeSurrogate.Day51 = Convert.ToString((DALHelper.HandleDBNull(reader["Day51"])));
            TherapyExeSurrogate.Day52 = Convert.ToString((DALHelper.HandleDBNull(reader["Day52"])));
            TherapyExeSurrogate.Day53 = Convert.ToString((DALHelper.HandleDBNull(reader["Day53"])));
            TherapyExeSurrogate.Day54 = Convert.ToString((DALHelper.HandleDBNull(reader["Day54"])));
            TherapyExeSurrogate.Day55 = Convert.ToString((DALHelper.HandleDBNull(reader["Day55"])));
            TherapyExeSurrogate.Day56 = Convert.ToString((DALHelper.HandleDBNull(reader["Day56"])));
            TherapyExeSurrogate.Day57 = Convert.ToString((DALHelper.HandleDBNull(reader["Day57"])));
            TherapyExeSurrogate.Day58 = Convert.ToString((DALHelper.HandleDBNull(reader["Day58"])));
            TherapyExeSurrogate.Day59 = Convert.ToString((DALHelper.HandleDBNull(reader["Day59"])));
            TherapyExeSurrogate.Day60 = Convert.ToString((DALHelper.HandleDBNull(reader["Day60"])));


            return TherapyExeSurrogate;
        }

        public override IValueObject GetIVFDashboardPreviousDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardPatientDiagnosisDataBizActionVO BizActionObj = valueObject as clsGetIVFDashboardPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardPreviousDiagnosis");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDiagnosisDetails == null)
                        BizActionObj.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    while (reader.Read())
                    {
                        clsEMRAddDiagnosisVO Obj = new clsEMRAddDiagnosisVO();
                        Obj.Categori = Convert.ToString(reader["Categori"]);
                        Obj.Class = Convert.ToString(reader["Class"]);
                        Obj.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"]));
                        Obj.IsSelected = (bool)DALHelper.HandleBoolDBNull(reader["PrimaryDiagnosis"]);
                        Obj.SelectedDiagnosisType.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"]));
                        Obj.SelectedDiagnosisType.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Obj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        BizActionObj.PatientDiagnosisDetails.Add(Obj);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();



            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        //added by neena
        public override IValueObject GetIVFDashboardPrescriptionDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardPatientPrescriptionDataBizActionVO BizActionObj = valueObject as clsGetIVFDashboardPatientPrescriptionDataBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardPreviousPrescription");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientPrescriptionList == null)
                        BizActionObj.PatientPrescriptionList = new List<clsPatientPrescriptionDetailVO>();
                    while (reader.Read())
                    {
                        clsPatientPrescriptionDetailVO objPrescriptionVO = new clsPatientPrescriptionDetailVO();
                        objPrescriptionVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPrescriptionVO.PrescriptionID = (long)DALHelper.HandleDBNull(reader["PrescriptionID"]);
                        objPrescriptionVO.SelectedItem.ID = (long)(DALHelper.HandleDBNull(reader["DrugID"]));
                        objPrescriptionVO.SelectedItem.Description = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                        objPrescriptionVO.DrugName = (string)(DALHelper.HandleDBNull(reader["ItemName"]));
                        objPrescriptionVO.DrugID = (long)(DALHelper.HandleIntegerNull(reader["DrugID"]));
                        objPrescriptionVO.SelectedFrequency.Description = (string)(DALHelper.HandleDBNull(reader["Frequency"]));
                        objPrescriptionVO.SelectedInstruction.Description = (string)(DALHelper.HandleDBNull(reader["Reason"]));
                        objPrescriptionVO.Instruction = (string)DALHelper.HandleDBNull(reader["Reason"]);
                        objPrescriptionVO.NewInstruction = (string)DALHelper.HandleDBNull(reader["Instruction"]);
                        //objPrescriptionVO.Dose = (string)DALHelper.HandleDBNull(reader["Dose"]);
                        objPrescriptionVO.SelectedRoute.Description = (string)DALHelper.HandleDBNull(reader["Route"]);
                        objPrescriptionVO.Quantity = (int)(DALHelper.HandleDBNull(reader["Quantity"]));
                        objPrescriptionVO.Reason = (string)DALHelper.HandleDBNull(reader["Reason"]);
                        objPrescriptionVO.Days = Convert.ToInt32(DALHelper.HandleDBNull(reader["Days"]));
                        //objPrescriptionVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objPrescriptionVO.IsOther = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsOther"]));
                        objPrescriptionVO.FromHistory = false;
                        objPrescriptionVO.Frequency = Convert.ToString(DALHelper.HandleDBNull(reader["Frequency"]));
                        objPrescriptionVO.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objPrescriptionVO.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                        //added by neena
                        objPrescriptionVO.ArtEnabled = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["ARTEnables"]));
                        objPrescriptionVO.DrugSource = Convert.ToInt64(DALHelper.HandleDBNull(reader["DrugSourceId"]));
                        objPrescriptionVO.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                        objPrescriptionVO.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
                        objPrescriptionVO.VisitDate = (DateTime)DALHelper.HandleDate(reader["VisitDate"]);
                        //
                        BizActionObj.PatientPrescriptionList.Add(objPrescriptionVO);
                    }
                }



                reader.Close();



            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetIVFDashboardInvestigation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardPatientInvestigationDataBizActionVO BizActionObj = valueObject as clsGetIVFDashboardPatientInvestigationDataBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardPreviousInvestigation");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientInvestigationList == null)
                        BizActionObj.PatientInvestigationList = new List<ValueObjects.Administration.clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO ObjService = new clsServiceMasterVO();
                        ObjService.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                        ObjService.DoctorID = Convert.ToInt32(reader["Doctorid"]);
                        // ObjService.DoctorCode = Convert.ToString(reader["DoctorCode"]);
                        ObjService.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        ObjService.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        //if (BizActionObj.IsOPDIPD)
                        //{
                        //    ObjService.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        //    ObjService.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["DrSpec"]));
                        //}
                        ObjService.Group = Convert.ToString(reader["GroupName"]);
                        ObjService.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        ObjService.VisitDate = Convert.ToDateTime(reader["VisitDate"]);
                        ObjService.SelectedPriority = new MasterListItem();
                        ObjService.SelectedPriority.Description = Convert.ToString(DALHelper.HandleDBNull(reader["PriorityDescription"]));
                        ObjService.Specialization = Convert.ToInt64(reader["SpecializationId"]);
                        BizActionObj.PatientInvestigationList.Add(ObjService);
                    }
                    //reader.NextResult();
                    //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return BizActionObj;
        }

        //

        public override IValueObject GetIVFDashboardcurrentDiagnosis(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO BizActionObj = valueObject as clsGetIVFDashboardCurrentPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardCurrentDiagnosis");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDiagnosisDetails == null)
                        BizActionObj.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    while (reader.Read())
                    {
                        clsEMRAddDiagnosisVO Obj = new clsEMRAddDiagnosisVO();
                        Obj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        Obj.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"]));
                        Obj.SelectedDiagnosisType.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"]));
                        Obj.SelectedDiagnosisType.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        BizActionObj.PatientDiagnosisDetails.Add(Obj);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject AddUpdateIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO BizactionObj = valueObject as clsIVFDashboard_AddPlanTherapyAdditionalmeasureBizActionVO;
            try
            {
                DbConnection con = dbServer.CreateConnection();
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePlanTherapyAddtionmeasure");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizactionObj.TherapyDetails.ID);
                dbServer.AddInParameter(command, "AssistedHatching", DbType.Int64, BizactionObj.TherapyDetails.AssistedHatching);
                dbServer.AddInParameter(command, "IMSI", DbType.Int64, BizactionObj.TherapyDetails.IMSI);
                dbServer.AddInParameter(command, "CryoPreservation", DbType.Int64, BizactionObj.TherapyDetails.CryoPreservation);
                dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
            }
            return valueObject;
        }

        public override IValueObject GetIVFDashBoardPlanTherapyadditionmeasure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO BizActionObj = valueObject as clsIVFDashboard_GetPlanTherapyAdditionalmeasureBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardAddtionmeasure");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.TherapyID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.TherapyDetails == null)
                        BizActionObj.TherapyDetails = new clsPlanTherapyVO();
                    while (reader.Read())
                    {
                        BizActionObj.TherapyDetails.AssistedHatching = Convert.ToBoolean(reader["AssistedHatching"]);
                        BizActionObj.TherapyDetails.IMSI = Convert.ToBoolean(reader["IMSI"]);
                        BizActionObj.TherapyDetails.CryoPreservation = Convert.ToBoolean(reader["CryoPreservation"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO BizAction = valueObject as clsIVFDashboard_GetPlanTherapyCPOEServicesBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFDashBoardCPOEServiceDetails");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                BizAction.ServiceDetails = new List<clsDoctorSuggestedServiceDetailVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO objServices = new clsDoctorSuggestedServiceDetailVO();
                        objServices.ServiceID = Convert.ToInt64(reader["ServiceID"]);
                        objServices.ServiceName = Convert.ToString(reader["ServiceName"]);
                        objServices.ServiceRate = Convert.ToDouble(reader["serviceRate"]);
                        objServices.ServiceCode = Convert.ToString(reader["ServiceCode"]);
                        objServices.PriorityIndex = Convert.ToInt32(reader["PriorityID"]);
                        objServices.SpecializationId = Convert.ToInt64(reader["SpecializationId"]);
                        BizAction.ServiceDetails.Add(objServices);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject AddUpdateIVFDashBoardPlanTherapyCPOEServices(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUPDatePlanTherapyCPOEServicesBizActionVO;
            try
            {
                DbConnection con = null;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteIVFDashboardCPOEServices");
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizActionObj.TherapyID);
                int intStatus = dbServer.ExecuteNonQuery(command);

                List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceVO = BizActionObj.InvestigationList;
                for (int i = 0; i < objDoctorSuggestedServiceVO.Count; i++)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddIVFDashboardCPOEServices");
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objDoctorSuggestedServiceVO[i].ServiceID);
                    dbServer.AddInParameter(command1, "ServiceName", DbType.String, objDoctorSuggestedServiceVO[i].ServiceName);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, objDoctorSuggestedServiceVO[i].ServiceRate);
                    dbServer.AddInParameter(command1, "ServiceCode", DbType.String, objDoctorSuggestedServiceVO[i].ServiceCode);
                    dbServer.AddInParameter(command1, "SpecializationID", DbType.String, objDoctorSuggestedServiceVO[i].SpecializationId);
                    dbServer.AddInParameter(command1, "PriorityID", DbType.Int64, objDoctorSuggestedServiceVO[i].SelectedPriority.ID);
                    dbServer.AddInParameter(command1, "TherapyID", DbType.Int64, BizActionObj.TherapyID);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    //BizActionObj.SuccessStatus=
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;
        }

        //added by neena
        public override IValueObject GetFolliculeLRSum(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsIVFDashboard_GetFolliculeLRSumBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetFolliculeLRSumBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetLRFolliculeSum");

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.String, BizAction.FollicularMonitoringDetial.TherapyId);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.FollicularMonitoringDetial.TherapyUnitId);
                BizAction.FollicularMonitoringSizeDetials = new clsFollicularMonitoringSizeDetails();

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.FollicularMonitoringSizeDetials.LeftSum = Convert.ToInt64(DALHelper.HandleDBNull(reader["LeftSize"]));
                        BizAction.FollicularMonitoringSizeDetials.RightSum = Convert.ToInt64(DALHelper.HandleDBNull(reader["RightSize"]));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetManagementVisible(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection con = null;
            //= dbServer.CreateConnection();
            DbTransaction trans = null;

            clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetManagementVisibleBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDetailsIVFManagementServiceBilled");

                dbServer.AddInParameter(command, "PatientId", DbType.String, BizAction.TherapyDetails.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.TherapyDetails.PatientUintId);
                dbServer.AddInParameter(command, "IsIVFBillingCriteria", DbType.Int64, BizAction.TherapyDetails.IsIVFBillingCriteria); //added by neena for ivf billing 
                //dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, BizAction.TherapyDetails.IsDonorCycle);
                dbServer.AddParameter(command, "IsDonorCycle", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TherapyDetails.IsDonorCycle);
                dbServer.AddParameter(command, "PlanTreatmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TherapyDetails.PlannedTreatmentID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TherapyDetails.ID);
                dbServer.AddParameter(command, "UnitID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TherapyDetails.UnitID);
                dbServer.AddParameter(command, "IsSurrogate", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TherapyDetails.IsSurrogate);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizAction.TherapyDetails.PlannedTreatmentID = (long)dbServer.GetParameterValue(command, "PlanTreatmentID");
                BizAction.TherapyDetails.IsDonorCycle = Convert.ToBoolean(dbServer.GetParameterValue(command, "IsDonorCycle"));
                BizAction.TherapyDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizAction.TherapyDetails.UnitID = (long)dbServer.GetParameterValue(command, "UnitID");
                BizAction.TherapyDetails.IsSurrogate = Convert.ToBoolean(dbServer.GetParameterValue(command, "IsSurrogate"));

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizAction.SuccessStatus = -1;
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetPACVisible(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbConnection con = null;
            //= dbServer.CreateConnection();
            DbTransaction trans = null;

            clsIVFDashboard_GetPACVisibleBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetPACVisibleBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDetailsIVFPACServiceBilled");

                dbServer.AddInParameter(command, "PatientId", DbType.String, BizAction.TherapyDetails.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.TherapyDetails.PatientUintId);
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyDetails.ID);
                dbServer.AddInParameter(command, "TherapyUnitID", DbType.Int64, BizAction.TherapyDetails.UnitID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizAction.SuccessStatus = -1;
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetBirthDetailsMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsIVFDashboard_GetBirthDetailsMasterListBizActionVO BizActionObj = (clsIVFDashboard_GetBirthDetailsMasterListBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.IsActive.HasValue)
                    FilterExpression.Append("Status = '" + BizActionObj.IsActive.Value + "'");

                if (BizActionObj.Parent != null)
                {
                    if (FilterExpression.Length > 0)
                        FilterExpression.Append(" And ");
                    FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                }


                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFBirthMasterList");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        if (BizActionObj.MasterTable.ToString() == "M_IVf_BirthAPGARScore")
                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["Code"].ToString(), reader["Description"].ToString(), (bool)reader["Status"], (long)reader["MinPoint"], (long)reader["MaxPoint"]));//HandleDBNull(reader["Date"])));
                        else
                            BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["Code"].ToString(), reader["Description"].ToString(), (long)reader["Point"], (bool)reader["Status"]));//HandleDBNull(reader["Date"])));

                        ////Added By CDS 22/2/16
                        //BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));

                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }


        public override IValueObject GetBirthDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();
            clsIVFDashboard_GetBirthDetailsListBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetBirthDetailsListBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetBirthDetailsList");

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.BirthDetails.TherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.BirthDetails.TherapyUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.BirthDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.BirthDetails.PatientUnitID);
                BizAction.BirthDetailsList = new List<clsIVFDashboard_BirthDetailsVO>();

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_BirthDetailsVO Details = new clsIVFDashboard_BirthDetailsVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.FetalHeartNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["FetalHeartNo"]));
                        Details.BirthDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BirthDetailsID"]));
                        Details.ChildNoStr = Convert.ToString(DALHelper.HandleDBNull(reader["ChildNoStr"]));
                        Details.BirthWeight = Convert.ToString(DALHelper.HandleDBNull(reader["BirthWeight"]));
                        Details.MedicalConditions = Convert.ToString(DALHelper.HandleDBNull(reader["MedicalConditions"]));
                        Details.WeeksofGestation = Convert.ToString(DALHelper.HandleDBNull(reader["WeeksofGestation"]));
                        Details.DeliveryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeliveryTypeID"]));
                        Details.ActivityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ActivityID"]));
                        Details.ActivityPoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["ActivityPoint"]));
                        Details.Pulse = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pulse"]));
                        Details.PulsePoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["PulsePoint"]));
                        Details.Grimace = Convert.ToInt64(DALHelper.HandleDBNull(reader["Grimace"]));
                        Details.GrimacePoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["GrimacePoint"]));
                        Details.Appearance = Convert.ToInt64(DALHelper.HandleDBNull(reader["Appearance"]));
                        Details.AppearancePoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["AppearancePoint"]));
                        Details.Respiration = Convert.ToInt64(DALHelper.HandleDBNull(reader["Respiration"]));
                        Details.RespirationPoint = Convert.ToInt64(DALHelper.HandleDBNull(reader["RespirationPoint"]));
                        Details.APGARScore = Convert.ToInt64(DALHelper.HandleDBNull(reader["APGARScore"]));
                        Details.APGARScoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["APGARScoreID"]));
                        Details.Conclusion = Convert.ToString(DALHelper.HandleDBNull(reader["Conclusion"]));
                        Details.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                        Details.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                        Details.SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"]));
                        BizAction.BirthDetailsList.Add(Details);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }


        public override IValueObject AddUpdateBirthDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddBirthDetailsListBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddBirthDetailsListBizActionVO;


            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                if (BizActionObj.BirthDetailsList != null && BizActionObj.BirthDetailsList.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetails");
                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.BirthDetails.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.BirthDetails.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.BirthDetails.TherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.BirthDetails.TherapyUnitID);
                    dbServer.AddInParameter(command3, "FetalHeartNo", DbType.Int64, BizActionObj.BirthDetails.FetalHeartNo);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.BirthDetails.ID);
                    dbServer.AddInParameter(command3, "SurrogateID", DbType.Int64, BizActionObj.BirthDetails.SurrogateID);
                    dbServer.AddInParameter(command3, "SurrogateUnitID", DbType.Int64, BizActionObj.BirthDetails.SurrogateUnitID);
                    dbServer.AddInParameter(command3, "SurrogatePatientMrNo", DbType.String, BizActionObj.BirthDetails.SurrogatePatientMrNo);
                    int intStatus3 = dbServer.ExecuteNonQuery(command3);
                    BizActionObj.BirthDetails.ID = (long)dbServer.GetParameterValue(command3, "ID");

                    foreach (var item in BizActionObj.BirthDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateBirthDetailsList");
                        dbServer.AddInParameter(command2, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "BirthDetailsID", DbType.Int64, item.BirthDetailsID);
                        dbServer.AddInParameter(command2, "ChildNoStr", DbType.String, item.ChildNoStr);
                        dbServer.AddInParameter(command2, "BirthWeight", DbType.String, item.BirthWeight);
                        dbServer.AddInParameter(command2, "MedicalConditions", DbType.String, item.MedicalConditions);
                        dbServer.AddInParameter(command2, "WeeksofGestation", DbType.String, item.WeeksofGestation);
                        dbServer.AddInParameter(command2, "DeliveryTypeID", DbType.Int64, item.DeliveryTypeID);
                        dbServer.AddInParameter(command2, "ActivityID", DbType.Int64, item.ActivityID);
                        dbServer.AddInParameter(command2, "ActivityPoint", DbType.Int64, item.ActivityPoint);
                        dbServer.AddInParameter(command2, "Pulse", DbType.Int64, item.Pulse);
                        dbServer.AddInParameter(command2, "PulsePoint", DbType.Int64, item.PulsePoint);
                        dbServer.AddInParameter(command2, "Grimace", DbType.Int64, item.Grimace);
                        dbServer.AddInParameter(command2, "GrimacePoint", DbType.Int64, item.GrimacePoint);
                        dbServer.AddInParameter(command2, "Appearance", DbType.Int64, item.Appearance);
                        dbServer.AddInParameter(command2, "AppearancePoint", DbType.Int64, item.AppearancePoint);
                        dbServer.AddInParameter(command2, "Respiration", DbType.Int64, item.Respiration);
                        dbServer.AddInParameter(command2, "RespirationPoint", DbType.Int64, item.RespirationPoint);
                        dbServer.AddInParameter(command2, "APGARScore", DbType.Int64, item.APGARScore);
                        dbServer.AddInParameter(command2, "Conclusion", DbType.String, item.Conclusion);
                        dbServer.AddInParameter(command2, "APGARScoreID", DbType.Int64, item.APGARScoreID);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.BirthDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }


        public override IValueObject GetVisitForARTPrescription(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();
            clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetVisitForARTPrescriptionBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetVisitForARTPrescription");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject GetHalfBilled(IValueObject valueObject, clsUserVO objUserVO)
        {

            clsIVFDashboard_GetManagementVisibleBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetManagementVisibleBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDetailsIVFServiceHalfBilled");

                dbServer.AddInParameter(command, "PatientId", DbType.String, BizAction.TherapyDetails.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.TherapyDetails.PatientUintId);
                dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, BizAction.TherapyDetails.IsDonorCycle);
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyDetails.ID);
                dbServer.AddInParameter(command, "TherapyUnitID", DbType.Int64, BizAction.TherapyDetails.UnitID);
                //dbServer.AddParameter(command, "PlanTreatmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.TherapyDetails.PlannedTreatmentID);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.TherapyDetails.BillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BillAmount"]));
                        BizAction.TherapyDetails.BillBalanceAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BillBalanceAmount"]));
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return BizAction;
        }


        public override IValueObject GetSurrogatePatientListForDashboard(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            GetPatientListForDashboardBizActionVO BizActionObj = valueObject as GetPatientListForDashboardBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSurrogatePatientListForDashboard");
                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();
                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["SurrogateID"]);
                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["SurrogateUnitID"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MrNo"]);
                        objPatientVO.IsSurrogateUsed = (bool)DALHelper.HandleDBNull(reader["IsSurrogateUsed"]);
                        BizActionObj.PatientDetailsList.Add(objPatientVO);
                    }
                }
                reader.Close();


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                con.Close();
                con = null;
            }
            return valueObject;
        }

        public override IValueObject GetSurrogatePatientListForTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            GetPatientListForDashboardBizActionVO BizActionObj = valueObject as GetPatientListForDashboardBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSurrogatePatientListForTransfer");
                dbServer.AddInParameter(command, "EMRProcedureID", DbType.String, BizActionObj.EMRProcedureID);
                dbServer.AddInParameter(command, "EMRProcedureUnitID", DbType.Int64, BizActionObj.EMRProcedureUnitID);
                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDetailsList == null)
                        BizActionObj.PatientDetailsList = new List<clsPatientGeneralVO>();
                    while (reader.Read())
                    {
                        clsPatientGeneralVO objPatientVO = new clsPatientGeneralVO();
                        objPatientVO.PatientID = (long)DALHelper.HandleDBNull(reader["SurrogateID"]);
                        objPatientVO.UnitId = (long)DALHelper.HandleDBNull(reader["SurrogateUnitID"]);
                        objPatientVO.MRNo = (string)DALHelper.HandleDBNull(reader["MrNo"]);
                        BizActionObj.PatientDetailsList.Add(objPatientVO);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return valueObject;
        }

        //
    }
}
