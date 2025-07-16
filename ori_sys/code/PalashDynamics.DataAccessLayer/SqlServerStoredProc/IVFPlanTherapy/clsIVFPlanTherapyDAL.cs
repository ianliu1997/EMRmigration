using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Patient;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsIVFPlanTherapyDAL : clsBaseIVFPlanTherapyDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        //added by neena
        string ImgIP = string.Empty;
        string ImgVirtualDir = string.Empty;
        string ImgSaveLocation = string.Empty;
        //
        #endregion

        private clsIVFPlanTherapyDAL()
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

                //added by neena
                ImgIP = System.Configuration.ConfigurationManager.AppSettings["RegImgIP"];
                ImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["RegImgVirtualDir"];
                ImgSaveLocation = System.Configuration.ConfigurationManager.AppSettings["ImgSavingLocation"];
                //
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #region Therapy Execution Calendar Created by SHIKHA

        public override IValueObject AddUpdatePlanTherapy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPlanTherapyBizActionVO BizActionObj = valueObject as clsAddPlanTherapyBizActionVO;

            //if (BizActionObj.TherapyDetails.IsSurrogate == false)
            BizActionObj = AddUpdatePlanTherapy(BizActionObj, UserVo);
            //else
            //{
            //    BizActionObj = AddUpdatePlanTherapy(BizActionObj, UserVo);
            //    BizActionObj = AddUpdatePlanTherapySurrogate(BizActionObj, UserVo);
            //}  

            return valueObject;
        }

        private clsAddPlanTherapyBizActionVO AddUpdatePlanTherapy(clsAddPlanTherapyBizActionVO BizActionObj, clsUserVO UserVo)
        {
            // clsAddPlanTherapyBizActionVO BizActionObj = valueObject as clsAddPlanTherapyBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();


                DbCommand command = dbServer.GetStoredProcCommand("IVF_AddUpdatePlanTherapy");

                //dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                //if (objDetailsVO.LinkServer != null)
                //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                dbServer.AddInParameter(command, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                //dbServer.AddInParameter(command, "PatientUintId", DbType.Int64, BizActionObj.TherapyDetails.PatientUintId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                dbServer.AddInParameter(command, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                dbServer.AddInParameter(command, "CycleDuration", DbType.String, BizActionObj.TherapyDetails.CycleDuration);
                dbServer.AddInParameter(command, "PlannedTreatmentID", DbType.Int64, BizActionObj.TherapyDetails.PlannedTreatmentID);

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


                dbServer.AddInParameter(command, "LutealSupport", DbType.String, BizActionObj.TherapyDetails.LutealSupport);
                dbServer.AddInParameter(command, "LutealRemarks", DbType.String, BizActionObj.TherapyDetails.LutealRemarks);

                dbServer.AddInParameter(command, "BHCGAss1Date", DbType.DateTime, BizActionObj.TherapyDetails.BHCGAss1Date);
                dbServer.AddInParameter(command, "BHCGAss1IsBSCG", DbType.Boolean, BizActionObj.TherapyDetails.BHCGAss1IsBSCG);
                dbServer.AddInParameter(command, "BHCGAss1BSCGValue", DbType.String, BizActionObj.TherapyDetails.BHCGAss1BSCGValue);
                dbServer.AddInParameter(command, "BHCGAss1SrProgest", DbType.String, BizActionObj.TherapyDetails.BHCGAss1SrProgest);
                dbServer.AddInParameter(command, "BHCGAss2Date", DbType.DateTime, BizActionObj.TherapyDetails.BHCGAss2Date);
                dbServer.AddInParameter(command, "BHCGAss2IsBSCG", DbType.Boolean, BizActionObj.TherapyDetails.BHCGAss2IsBSCG);
                dbServer.AddInParameter(command, "BHCGAss2BSCGValue", DbType.String, BizActionObj.TherapyDetails.BHCGAss2BSCGValue);
                dbServer.AddInParameter(command, "BHCGAss2USG", DbType.String, BizActionObj.TherapyDetails.BHCGAss2USG);
                dbServer.AddInParameter(command, "PregnancyAchieved", DbType.Boolean, BizActionObj.TherapyDetails.IsPregnancyAchieved);
                dbServer.AddInParameter(command, "PregnanacyConfirmDate", DbType.DateTime, BizActionObj.TherapyDetails.PregnanacyConfirmDate);
                dbServer.AddInParameter(command, "IsClosed", DbType.Boolean, BizActionObj.TherapyDetails.IsClosed);
                dbServer.AddInParameter(command, "OutComeRemarks", DbType.String, BizActionObj.TherapyDetails.OutComeRemarks);
                //added later by priti
                dbServer.AddInParameter(command, "BiochemPregnancy", DbType.Boolean, BizActionObj.TherapyDetails.BiochemPregnancy);
                dbServer.AddInParameter(command, "Ectopic", DbType.Boolean, BizActionObj.TherapyDetails.Ectopic);
                dbServer.AddInParameter(command, "Abortion", DbType.Boolean, BizActionObj.TherapyDetails.Abortion);
                dbServer.AddInParameter(command, "Missed", DbType.Boolean, BizActionObj.TherapyDetails.Missed);
                dbServer.AddInParameter(command, "FetalHeartSound", DbType.Boolean, BizActionObj.TherapyDetails.FetalHeartSound);
                dbServer.AddInParameter(command, "FetalDate", DbType.DateTime, BizActionObj.TherapyDetails.FetalDate);
                dbServer.AddInParameter(command, "Count", DbType.String, BizActionObj.TherapyDetails.Count);
                dbServer.AddInParameter(command, "Incomplete", DbType.Boolean, BizActionObj.TherapyDetails.Incomplete);
                dbServer.AddInParameter(command, "IUD", DbType.Boolean, BizActionObj.TherapyDetails.IUD);
                dbServer.AddInParameter(command, "PretermDelivery", DbType.Boolean, BizActionObj.TherapyDetails.PretermDelivery);
                dbServer.AddInParameter(command, "LiveBirth", DbType.Boolean, BizActionObj.TherapyDetails.LiveBirth);
                dbServer.AddInParameter(command, "Congenitalabnormality", DbType.Boolean, BizActionObj.TherapyDetails.Congenitalabnormality);

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

                //
                dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, BizActionObj.TherapyDocument.Date);
                dbServer.AddInParameter(command, "DocumentID", DbType.Int64, BizActionObj.TherapyDocument.ID);
                dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.TherapyDocument.Description);
                dbServer.AddInParameter(command, "Title", DbType.String, BizActionObj.TherapyDocument.Title);
                dbServer.AddInParameter(command, "AttachedFileName", DbType.String, BizActionObj.TherapyDocument.AttachedFileName);
                dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, BizActionObj.TherapyDocument.AttachedFileContent);
                dbServer.AddInParameter(command, "IsDeleted", DbType.Boolean, BizActionObj.TherapyDocument.IsDeleted);

                dbServer.AddInParameter(command, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);
                if (BizActionObj.TherapyDetails.IsSurrogate == true)
                {
                    dbServer.AddInParameter(command, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                    dbServer.AddInParameter(command, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                }

                dbServer.AddInParameter(command, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                dbServer.AddInParameter(command, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                //     dbServer.AddInParameter(command, "Value", DbType.String, BizActionObj.TherapyDetails.Value);

                if (BizActionObj.TherapyDetails.IsSurrogateCalendar == true)
                {
                    string Value = null;
                    dbServer.AddInParameter(command, "Value", DbType.String, Value);
                }
                else
                {
                    dbServer.AddInParameter(command, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                }
                if (BizActionObj.TherapyDetails.IsSurrogateDrug == false)
                {
                    dbServer.AddInParameter(command, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                    dbServer.AddInParameter(command, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                    dbServer.AddInParameter(command, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                    dbServer.AddInParameter(command, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                }
                else
                {
                    dbServer.AddInParameter(command, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                    dbServer.AddInParameter(command, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                    dbServer.AddInParameter(command, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                    dbServer.AddInParameter(command, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                }
                //dbServer.AddInParameter(command, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                //dbServer.AddInParameter(command, "DrugTime", DbType.DateTime , BizActionObj.TherapyDetails.DrugTime);
                //dbServer.AddInParameter(command, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                //dbServer.AddInParameter(command, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);

                // By BHUSHAN 
                dbServer.AddInParameter(command, "IsChemicalPregnancy", DbType.Boolean, BizActionObj.TherapyDetails.IsChemicalPregnancy);
                dbServer.AddInParameter(command, "IsFullTermDelivery", DbType.Boolean, BizActionObj.TherapyDetails.IsFullTermDelivery);
                dbServer.AddInParameter(command, "OHSSEarly", DbType.Boolean, BizActionObj.TherapyDetails.OHSSEarly);
                dbServer.AddInParameter(command, "OHSSLate", DbType.Boolean, BizActionObj.TherapyDetails.OHSSLate);
                dbServer.AddInParameter(command, "OHSSMild", DbType.Boolean, BizActionObj.TherapyDetails.OHSSMild);
                dbServer.AddInParameter(command, "OHSSMode", DbType.Boolean, BizActionObj.TherapyDetails.OHSSMode);
                dbServer.AddInParameter(command, "OHSSSereve", DbType.Boolean, BizActionObj.TherapyDetails.OHSSSereve);
                dbServer.AddInParameter(command, "OHSSRemark", DbType.String, BizActionObj.TherapyDetails.OHSSRemark);
                dbServer.AddInParameter(command, "BabyTypeID", DbType.Int64, BizActionObj.TherapyDetails.BabyTypeID);
                //
                // Out Come For BAby.....
                dbServer.AddInParameter(command, "SIX_monthFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.SIXmonthFitnessRemark);
                dbServer.AddInParameter(command, "SIX_monthFitnessID", DbType.Int64, BizActionObj.TherapyDetails.SIXmonthFitnessID);

                dbServer.AddInParameter(command, "One_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.ONEyFitnessRemark);
                dbServer.AddInParameter(command, "One_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.ONEyFitnessID);

                dbServer.AddInParameter(command, "FIVE_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.FIVEyFitnessRemark);
                dbServer.AddInParameter(command, "FIVE_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.FIVEyFitnessID);

                dbServer.AddInParameter(command, "TEN_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.TENyFitnessRemark);
                dbServer.AddInParameter(command, "TEN_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.TENyFitnessID);

                dbServer.AddInParameter(command, "TWNTY_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.TWENTYyFitnessRemark);
                dbServer.AddInParameter(command, "TWNTY_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.TWENTYyFitnessID);

                // Out Come For MOther.....
                dbServer.AddInParameter(command, "SIX_monthFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.SIXmonthFitnessRemark_m);
                dbServer.AddInParameter(command, "SIX_monthFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.SIXmonthFitnessID_m);

                dbServer.AddInParameter(command, "One_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.ONEyFitnessRemark_m);
                dbServer.AddInParameter(command, "One_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.ONEyFitnessID_m);

                dbServer.AddInParameter(command, "FIVE_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.FIVEyFitnessRemark_m);
                dbServer.AddInParameter(command, "FIVE_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.FIVEyFitnessID_m);

                dbServer.AddInParameter(command, "TEN_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.TENyFitnessRemark_m);
                dbServer.AddInParameter(command, "TEN_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.TENyFitnessID_m);

                dbServer.AddInParameter(command, "TWNTY_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.TWENTYyFitnessRemark_m);
                dbServer.AddInParameter(command, "TWNTY_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.TWENTYyFitnessID_m);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //By Anjali................................
                dbServer.AddInParameter(command, "OPUDate", DbType.DateTime, BizActionObj.TherapyDetails.OPUtDate);
                dbServer.AddInParameter(command, "OPURemark", DbType.String, BizActionObj.TherapyDetails.OPURemark);

                //For IVF DashBoard...... By Anjali..........................
                dbServer.AddInParameter(command, "LongtermMedication", DbType.String, BizActionObj.TherapyDetails.LongtermMedication);
                dbServer.AddInParameter(command, "AssistedHatching", DbType.Boolean, BizActionObj.TherapyDetails.AssistedHatching);
                dbServer.AddInParameter(command, "IMSI", DbType.Boolean, BizActionObj.TherapyDetails.IMSI);
                dbServer.AddInParameter(command, "CryoPreservation", DbType.Boolean, BizActionObj.TherapyDetails.CryoPreservation);

                dbServer.AddParameter(command, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.TherapyDetails.ID = (long)dbServer.GetParameterValue(command, "TherapyID");

                if (BizActionObj.TherapyDetails.IsSurrogate == true || BizActionObj.TherapyDetails.IsSurrogateDrug == true)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("IVF_AddUpdateSurrogatePlanTherapy");

                    dbServer.AddInParameter(command1, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                    dbServer.AddInParameter(command1, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                    dbServer.AddInParameter(command1, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                    dbServer.AddInParameter(command1, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                    dbServer.AddInParameter(command1, "PlanTherapyId", DbType.Int64, BizActionObj.TherapyDetails.ID);
                    dbServer.AddInParameter(command1, "PlanTherapyUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);

                    dbServer.AddInParameter(command1, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                    dbServer.AddInParameter(command1, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                    dbServer.AddInParameter(command1, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);

                    if (BizActionObj.TherapyDelivery != null)
                    {
                        dbServer.AddInParameter(command1, "DeliveryDate", DbType.DateTime, BizActionObj.TherapyDelivery.DeliveryDate);
                        dbServer.AddInParameter(command1, "DeliveryID", DbType.Int64, BizActionObj.TherapyDelivery.ID);
                        dbServer.AddInParameter(command1, "Weight", DbType.Double, BizActionObj.TherapyDelivery.Weight);
                        dbServer.AddInParameter(command1, "TimeofBirth", DbType.DateTime, BizActionObj.TherapyDelivery.TimeofBirth);
                        dbServer.AddInParameter(command1, "Mode", DbType.String, BizActionObj.TherapyDelivery.Mode);
                        dbServer.AddInParameter(command1, "Baby", DbType.String, BizActionObj.TherapyDelivery.Baby);
                    }

                    //dbServer.AddInParameter(command1, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                    dbServer.AddInParameter(command1, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.SurrogateExecutionId);
                    dbServer.AddInParameter(command1, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                    dbServer.AddInParameter(command1, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                    if (BizActionObj.TherapyDetails.IsSurrogateCalendar == true || BizActionObj.TherapyDetails.IsSurrogateDrug == true)
                    {
                        dbServer.AddInParameter(command1, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                    }
                    else
                    {
                        string Value = null;
                        dbServer.AddInParameter(command1, "Value", DbType.String, Value);
                    }
                    if (BizActionObj.TherapyDetails.IsSurrogateDrug == true)
                    {
                        // dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.TherapyDetails.UnitID);
                        dbServer.AddInParameter(command1, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                        dbServer.AddInParameter(command1, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                        dbServer.AddInParameter(command1, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                    }
                    else
                    {
                        dbServer.AddInParameter(command1, "DrugTime", DbType.DateTime, System.DateTime.Now);

                    }
                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    //dbServer.AddParameter(command1, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.SurrogateExecutionId);
                    dbServer.AddParameter(command1, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    BizActionObj.TherapyDetails.ID = (long)dbServer.GetParameterValue(command1, "TherapyID");

                    if (BizActionObj.ANCList != null && BizActionObj.ANCList.Count > 0)
                    {
                        foreach (var ObjDetails in BizActionObj.ANCList)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("IVF_AddUpdateANCVisit");
                            //dbServer.AddInParameter(command1, "ANCID", DbType.Int64, ObjDetails.ANCID);
                            dbServer.AddInParameter(command2, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.TherapyDetails.UnitID);
                            dbServer.AddInParameter(command2, "ANCDate", DbType.DateTime, ObjDetails.ANCDate);
                            dbServer.AddInParameter(command2, "POG", DbType.String, ObjDetails.POG);
                            dbServer.AddInParameter(command2, "Findings", DbType.String, ObjDetails.Findings);
                            dbServer.AddInParameter(command2, "USGReproductive", DbType.String, ObjDetails.USGReproductive);
                            dbServer.AddInParameter(command2, "Investigations", DbType.String, ObjDetails.Investigation);
                            dbServer.AddInParameter(command2, "Remarks", DbType.String, ObjDetails.Remarks);
                            dbServer.AddParameter(command2, "ANCID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ANCID);
                            int iStatus = dbServer.ExecuteNonQuery(command2);
                            ObjDetails.ANCID = (long)dbServer.GetParameterValue(command2, "ANCID");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }

        private clsAddPlanTherapyBizActionVO AddUpdatePlanTherapySurrogate(clsAddPlanTherapyBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVF_AddUpdateSurrogatePlanTherapy");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                dbServer.AddInParameter(command, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                dbServer.AddInParameter(command, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                dbServer.AddInParameter(command, "PlanTherapyUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);

                //@PlanTherapyId bigint,
                //@PlanTherapyUnitId bigint,

                //added for surrogate
                dbServer.AddInParameter(command, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                dbServer.AddInParameter(command, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                dbServer.AddInParameter(command, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);

                if (BizActionObj.TherapyDelivery != null)
                {
                    dbServer.AddInParameter(command, "DeliveryDate", DbType.DateTime, BizActionObj.TherapyDelivery.DeliveryDate);
                    dbServer.AddInParameter(command, "DeliveryID", DbType.Int64, BizActionObj.TherapyDelivery.ID);
                    dbServer.AddInParameter(command, "Weight", DbType.Double, BizActionObj.TherapyDelivery.Weight);
                    dbServer.AddInParameter(command, "TimeofBirth", DbType.DateTime, BizActionObj.TherapyDelivery.TimeofBirth);
                    dbServer.AddInParameter(command, "Mode", DbType.String, BizActionObj.TherapyDelivery.Mode);
                    dbServer.AddInParameter(command, "Baby", DbType.String, BizActionObj.TherapyDelivery.Baby);
                }

                dbServer.AddInParameter(command, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                dbServer.AddInParameter(command, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                dbServer.AddInParameter(command, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                dbServer.AddInParameter(command, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                dbServer.AddInParameter(command, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                dbServer.AddInParameter(command, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                dbServer.AddInParameter(command, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.TherapyDetails.ID = (long)dbServer.GetParameterValue(command, "TherapyID");

                if (BizActionObj.ANCList != null && BizActionObj.ANCList.Count > 0)
                {
                    foreach (var ObjDetails in BizActionObj.ANCList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("IVF_AddUpdateANCVisit");
                        //dbServer.AddInParameter(command1, "ANCID", DbType.Int64, ObjDetails.ANCID);
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.TherapyDetails.UnitID);
                        dbServer.AddInParameter(command1, "ANCDate", DbType.DateTime, ObjDetails.ANCDate);
                        dbServer.AddInParameter(command1, "POG", DbType.String, ObjDetails.POG);
                        dbServer.AddInParameter(command1, "Findings", DbType.String, ObjDetails.Findings);
                        dbServer.AddInParameter(command1, "USGReproductive", DbType.String, ObjDetails.USGReproductive);
                        dbServer.AddInParameter(command1, "Investigations", DbType.String, ObjDetails.Investigation);
                        dbServer.AddInParameter(command1, "Remarks", DbType.String, ObjDetails.Remarks);
                        dbServer.AddParameter(command1, "ANCID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDetails.ANCID);
                        int iStatus = dbServer.ExecuteNonQuery(command1);
                        ObjDetails.ANCID = (long)dbServer.GetParameterValue(command1, "ANCID");
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyDetails = null;
            }
            finally
            {
                con.Close();
            }
            return BizActionObj;
        }

        public override IValueObject GetFollicularModified(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFollicularModifiedDetailsBizActionVO BizActionObj = valueObject as clsGetFollicularModifiedDetailsBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("GetFollicularModified");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.FollicularID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.FollicularMonitoringDetial.UnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            throw new NotImplementedException();
        }
        public override IValueObject UpdateFollicularMonitoring(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateFollicularMonitoringBizActionVO BizActionObj = valueObject as clsUpdateFollicularMonitoringBizActionVO;
            //clsAddPlanTherapyBizActionVO BizActionObj = valueObject as clsAddPlanTherapyBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();


                DbCommand command = dbServer.GetStoredProcCommand("IVF_AddUpdateFollicularMonitoring");

                //dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                //if (objDetailsVO.LinkServer != null)
                //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.FollicularID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.FollicularMonitoringDetial.UnitID);

                dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, BizActionObj.FollicularMonitoringDetial.TherapyId);

                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.FollicularMonitoringDetial.TherapyUnitId);

                //dbServer.AddInParameter(command, "PatientUintId", DbType.Int64, BizActionObj.TherapyDetails.PatientUintId);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.FollicularMonitoringDetial.Date);
                dbServer.AddInParameter(command, "AttendedPhysicianId", DbType.Int64, BizActionObj.FollicularMonitoringDetial.PhysicianID);
                dbServer.AddInParameter(command, "Notes", DbType.String, BizActionObj.FollicularMonitoringDetial.FollicularNotes);
                dbServer.AddInParameter(command, "AttachmentPath", DbType.String, BizActionObj.FollicularMonitoringDetial.AttachmentPath);
                dbServer.AddInParameter(command, "AttachmentFileContents", DbType.Binary, BizActionObj.FollicularMonitoringDetial.AttachmentFileContent);
                dbServer.AddInParameter(command, "EndometriumThickness", DbType.String, BizActionObj.FollicularMonitoringDetial.EndometriumThickness);

                dbServer.AddInParameter(command, "FollicularNoList", DbType.String, BizActionObj.FollicularMonitoringDetial.FollicularNoList);
                dbServer.AddInParameter(command, "LeftSizeList", DbType.String, BizActionObj.FollicularMonitoringDetial.LeftSizeList);
                dbServer.AddInParameter(command, "RightSizeList", DbType.String, BizActionObj.FollicularMonitoringDetial.RightSizeList);


                /* added By Sudhir */
                //dbServer.AddInParameter(command, "OveryID", DbType.Int64, BizActionObj.FollicularMonitoringSizeDetials.OveryID);
                //dbServer.AddInParameter(command, "NoOf_folicule", DbType.Int64, BizActionObj.FollicularMonitoringSizeDetials.NoOf_folicule);
                //dbServer.AddInParameter(command, "SizeOf_folicule", DbType.Int64, BizActionObj.FollicularMonitoringSizeDetials.SizeOf_folicule);


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

        public override IValueObject GetPatientEMRDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEMRDetailsBizActionVO BizAction = (valueObject) as clsGetEMRDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizAction.TemplateID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));
                        BizAction.EMRDetailsList.Add(EmrDetails);
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

        public override IValueObject GetCoupleDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetCoupleDetailsBizActionVO BizAction = (valueObject) as clsGetCoupleDetailsBizActionVO;
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.CoupleDetails.MalePatient = new clsPatientGeneralVO();
            BizAction.CoupleDetails.FemalePatient = new clsPatientGeneralVO();
            BizAction.AllCoupleDetails = new List<clsCoupleVO>();

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCoupleDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "IsAllCoupleDetails", DbType.Int64, BizAction.IsAllCouple);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (BizAction.IsAllCouple == false)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Common Properties
                            BizAction.CoupleDetails.CoupleRegNo = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleRegNo"]));
                            BizAction.CoupleDetails.CoupleRegDate = Convert.ToDateTime(DALHelper.HandleDate(reader["CoupleRegDate"]));
                            BizAction.CoupleDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleId"]));
                            BizAction.CoupleDetails.CoupleUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));


                            //Female Details
                            clsPatientGeneralVO FemalePatient = new clsPatientGeneralVO();
                            FemalePatient.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                            FemalePatient.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                            FemalePatient.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["FemalePatientMRNO"]));
                            FemalePatient.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["FemalePatientRegDate"]));
                            FemalePatient.FirstName = Security.base64Decode(reader["FemaleFirstName"].ToString());
                            FemalePatient.MiddleName = Security.base64Decode(reader["FemaleMiddleName"].ToString());
                            FemalePatient.LastName = Security.base64Decode(reader["FemaleLastName"].ToString());
                            FemalePatient.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDate(reader["FemaleDOB"]));
                            FemalePatient.Photo = (Byte[])DALHelper.HandleDBNull(reader["FemalePhoto"]);

                            //added by neena
                            //FemalePatient.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleImgPath"]));
                            string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleImgPath"]));
                            FemalePatient.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                            //

                            FemalePatient.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleAlerts"]));
                            //**//---------
                            FemalePatient.ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleContactNo"]));
                            FemalePatient.Email = Security.base64Decode(reader["FemaleEmail"].ToString());
                            FemalePatient.AddressLine1 = Security.base64Decode(reader["FemaleAddressLine"].ToString());
                            FemalePatient.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                            FemalePatient.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));
                            FemalePatient.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));
                            //-------------

                            BizAction.CoupleDetails.FemalePatient = FemalePatient;

                            //Male Details
                            clsPatientGeneralVO MalePatient = new clsPatientGeneralVO();
                            MalePatient.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleId"]));
                            MalePatient.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MAleUnitID"]));
                            MalePatient.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MaleMRNO"]));
                            MalePatient.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["MaleRegDate"]));
                            MalePatient.FirstName = Security.base64Decode(reader["MaleFirstName"].ToString());
                            MalePatient.MiddleName = Security.base64Decode(reader["MaleMiddleName"].ToString());
                            MalePatient.LastName = Security.base64Decode(reader["MaleLastName"].ToString());
                            MalePatient.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDate(reader["MaleDOB"]));
                            MalePatient.Photo = (Byte[])DALHelper.HandleDBNull(reader["MalePhoto"]);

                            //added by neena
                            //MalePatient.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["MaleImgPath"])); 
                            string ImgPath1 = Convert.ToString(DALHelper.HandleDBNull(reader["MaleImgPath"]));
                            MalePatient.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath1;
                            //

                            MalePatient.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["MaleAlerts"]));

                            //**//---------
                            MalePatient.ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["MaleContactNo"]));
                            MalePatient.Email = Security.base64Decode(reader["MaleEmail"].ToString());
                            MalePatient.AddressLine1 = Security.base64Decode(reader["MaleAddressLine"].ToString());
                            FemalePatient.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                            FemalePatient.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));
                            FemalePatient.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));               
                           
                            //-------------
                             
                            BizAction.CoupleDetails.MalePatient = MalePatient;

                        }
                    }
                }
                else
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsCoupleVO coupleINfo = new clsCoupleVO();
                            // Common Properties
                            coupleINfo.CoupleRegNo = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleRegNo"]));
                            coupleINfo.CoupleRegDate = Convert.ToDateTime(DALHelper.HandleDate(reader["CoupleRegDate"]));
                            coupleINfo.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleId"]));
                            coupleINfo.CoupleUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));


                            //Female Details
                            clsPatientGeneralVO FemalePatient = new clsPatientGeneralVO();
                            FemalePatient.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                            FemalePatient.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                            FemalePatient.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["FemalePatientMRNO"]));
                            FemalePatient.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["FemalePatientRegDate"]));
                            FemalePatient.FirstName = Security.base64Decode(reader["FemaleFirstName"].ToString());
                            FemalePatient.MiddleName = Security.base64Decode(reader["FemaleMiddleName"].ToString());
                            FemalePatient.LastName = Security.base64Decode(reader["FemaleLastName"].ToString());
                            FemalePatient.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDate(reader["FemaleDOB"]));
                            FemalePatient.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleAlerts"]));

                            coupleINfo.FemalePatient = FemalePatient;

                            //Male Details
                            clsPatientGeneralVO MalePatient = new clsPatientGeneralVO();
                            MalePatient.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleId"]));
                            MalePatient.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MAleUnitID"]));
                            MalePatient.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MaleMRNO"]));
                            MalePatient.RegistrationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["MaleRegDate"]));
                            MalePatient.FirstName = Security.base64Decode(reader["MaleFirstName"].ToString());
                            MalePatient.MiddleName = Security.base64Decode(reader["MaleMiddleName"].ToString());
                            MalePatient.LastName = Security.base64Decode(reader["MaleLastName"].ToString());
                            MalePatient.DateOfBirth = Convert.ToDateTime(DALHelper.HandleDate(reader["MaleDOB"]));
                            MalePatient.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["MaleAlerts"]));

                            coupleINfo.MalePatient = MalePatient;

                            BizAction.AllCoupleDetails.Add(coupleINfo);

                        }
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

        public override IValueObject GetTherapyDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {

            bool CurrentMethodExecutionStatus = true;
            clsGetTherapyListBizActionVO BizAction = valueObject as clsGetTherapyListBizActionVO;
            if (BizAction.TherapyDetails.IsSurrogate == false)
            {
                BizAction = GetTherapyDetailList(BizAction, objUserVO);
            }
            else
            {
                BizAction = GetTherapyDetailList(BizAction, objUserVO);
                BizAction = GetTherapyDetailsSurrogate(BizAction, objUserVO);
            }
            return valueObject;
        }

        private clsGetTherapyListBizActionVO GetTherapyDetailList(clsGetTherapyListBizActionVO BizAction, clsUserVO objUserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            try
            {
                if (BizAction.Flag == true)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVF_GetTherapyList");

                    dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizAction.CoupleID);
                    dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    dbServer.AddInParameter(command, "TabID", DbType.Int64, BizAction.TabID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUintID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.TherapyUnitID);
                    //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                    //By Anjali..................................
                    dbServer.AddInParameter(command, "PlannedTreatmentID", DbType.Int64, BizAction.PlannedTreatmentID);
                    dbServer.AddInParameter(command, "ProtocolTypeID", DbType.Int64, BizAction.ProtocolTypeID);
                    dbServer.AddInParameter(command, "PhysicianId", DbType.Int64, BizAction.PhysicianId);
                    dbServer.AddInParameter(command, "rdoActive", DbType.Boolean, BizAction.rdoActive);
                    dbServer.AddInParameter(command, "rdoAll", DbType.Boolean, BizAction.rdoAll);
                    dbServer.AddInParameter(command, "rdoClosed", DbType.Boolean, BizAction.rdoClosed);
                    dbServer.AddInParameter(command, "rdoSuccessful", DbType.Boolean, BizAction.rdoSuccessful);
                    dbServer.AddInParameter(command, "rdoUnsuccessful", DbType.Boolean, BizAction.rdoUnsuccessful);
                    //.........................................

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
                                PlanTherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                                PlanTherapyDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                                //PlanTherapyDetails.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                                //PlanTherapyDetails.SurrogateMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMRNo"]));
                                PlanTherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                                PlanTherapyDetails.Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"]));

                                PlanTherapyDetails.PillStartDate = (DateTime?)(DALHelper.HandleDate(reader["PillStartDate"]));
                                PlanTherapyDetails.PillEndDate = (DateTime?)(DALHelper.HandleDate(reader["PillEndDate"]));
                                PlanTherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                                // PlanTherapyDetails.PlannedNoofEmbryos = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                                PlanTherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                                PlanTherapyDetails.PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
                                PlanTherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                                PlanTherapyDetails.PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"]));
                                PlanTherapyDetails.TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"]));

                                PlanTherapyDetails.LutealSupport = Convert.ToString(DALHelper.HandleDBNull(reader["LutealSupport"]));
                                PlanTherapyDetails.LutealRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["LutealRemarks"]));
                                PlanTherapyDetails.BHCGAss1Date = (DateTime?)DALHelper.HandleDate(reader["BHCGAss1Date"]);
                                PlanTherapyDetails.BHCGAss1IsBSCG = ((Boolean?)(DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"])));
                                if (PlanTherapyDetails.BHCGAss1IsBSCG != null)
                                {
                                    if ((bool)PlanTherapyDetails.BHCGAss1IsBSCG)
                                        PlanTherapyDetails.BHCGAss1IsBSCGPositive = true;
                                    else
                                        PlanTherapyDetails.BHCGAss1IsBSCGNagative = true;
                                }
                                //PlanTherapyDetails.BHCGAss1IsBSCG = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"]));
                                PlanTherapyDetails.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1BSCGValue"]));
                                PlanTherapyDetails.BHCGAss1SrProgest = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1SrProgest"]));
                                PlanTherapyDetails.BHCGAss2Date = (DateTime?)DALHelper.HandleDate(reader["BHCGAss2Date"]);
                                PlanTherapyDetails.BHCGAss2IsBSCG = (Boolean?)(DALHelper.HandleDBNull(reader["BHCGAss2IsBSCG"]));
                                if (PlanTherapyDetails.BHCGAss2IsBSCG != null)
                                {
                                    if ((bool)PlanTherapyDetails.BHCGAss2IsBSCG)
                                        PlanTherapyDetails.BHCGAss2IsBSCGPostive = true;
                                    else
                                        PlanTherapyDetails.BHCGAss2IsBSCGNagative = true;
                                }

                                PlanTherapyDetails.BHCGAss2BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2BSCGValue"]));
                                PlanTherapyDetails.BHCGAss2USG = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2USG"]));
                                PlanTherapyDetails.IsPregnancyAchieved = (Boolean?)DALHelper.HandleDBNull(reader["PregnancyAchieved"]);
                                if (PlanTherapyDetails.IsPregnancyAchieved != null)
                                {
                                    if ((bool)PlanTherapyDetails.IsPregnancyAchieved)
                                        PlanTherapyDetails.IsPregnancyAchievedPostive = true;
                                    else
                                        PlanTherapyDetails.IsPregnancyAchievedNegative = true;
                                }
                                //PlanTherapyDetails.IsPregnancyAchieved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PregnancyAchieved"]));
                                PlanTherapyDetails.PregnanacyConfirmDate = (DateTime?)(DALHelper.HandleDate(reader["PregnanacyConfirmDate"]));
                                PlanTherapyDetails.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                                PlanTherapyDetails.OutComeRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["OutComeRemarks"]));
                                PlanTherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                                PlanTherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                                PlanTherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                                PlanTherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                                //PlanTherapyDetails.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogate"]));
                                PlanTherapyDetails.FetalHeartSound = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FetalHeartSound"]));
                                //By Anjali.........On 21/01/2014
                                PlanTherapyDetails.OHSSEarly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSEarly"]));
                                PlanTherapyDetails.OHSSLate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSLate"]));
                                PlanTherapyDetails.OHSSMild = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMild"]));
                                PlanTherapyDetails.OHSSMode = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMode"]));
                                PlanTherapyDetails.OHSSRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OHSSRemark"]));
                                PlanTherapyDetails.OHSSSereve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSSereve"]));
                                PlanTherapyDetails.SIXmonthFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID"]));
                                PlanTherapyDetails.SIXmonthFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID_m"]));
                                PlanTherapyDetails.SIXmonthFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks"]));
                                PlanTherapyDetails.SIXmonthFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks_m"]));
                                PlanTherapyDetails.ONEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID"]));
                                PlanTherapyDetails.ONEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID_m"]));
                                PlanTherapyDetails.ONEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks"]));
                                PlanTherapyDetails.ONEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks_m"]));
                                PlanTherapyDetails.FIVEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID"]));
                                PlanTherapyDetails.FIVEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID_m"]));
                                PlanTherapyDetails.FIVEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks"]));
                                PlanTherapyDetails.FIVEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks_m"]));
                                PlanTherapyDetails.TENyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID"]));
                                PlanTherapyDetails.TENyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID_m"]));
                                PlanTherapyDetails.TENyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks"]));
                                PlanTherapyDetails.TENyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks_m"]));
                                PlanTherapyDetails.TWENTYyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID"]));
                                PlanTherapyDetails.TWENTYyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID_m"]));
                                PlanTherapyDetails.TWENTYyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks"]));
                                PlanTherapyDetails.TWENTYyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks_m"]));
                                PlanTherapyDetails.Missed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Missed"]));
                                PlanTherapyDetails.Incomplete = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Incomplete"]));
                                PlanTherapyDetails.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                                PlanTherapyDetails.FetalDate = (DateTime?)(DALHelper.HandleDate(reader["FetalDate"]));
                                PlanTherapyDetails.Count = Convert.ToString(DALHelper.HandleDBNull(reader["Count"]));
                                PlanTherapyDetails.IUD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IUD"]));
                                PlanTherapyDetails.LiveBirth = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LiveBirth"]));
                                PlanTherapyDetails.Congenitalabnormality = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Congenitalabnormality"]));
                                PlanTherapyDetails.IsChemicalPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChemicalPregnancy"]));
                                PlanTherapyDetails.PretermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PretermDelivery"]));
                                PlanTherapyDetails.IsFullTermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFullTermDelivery"]));
                                PlanTherapyDetails.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));
                                PlanTherapyDetails.BiochemPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BiochemPregnancy"]));
                                PlanTherapyDetails.Ectopic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ectopic"]));
                                if (PlanTherapyDetails.Missed == true || PlanTherapyDetails.Incomplete == true)
                                    PlanTherapyDetails.Abortion = true;
                                else
                                    PlanTherapyDetails.Abortion = false;
                                PlanTherapyDetails.PCOS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PCOS"]));
                                PlanTherapyDetails.Hypogonadotropic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypogonadotropic"]));
                                PlanTherapyDetails.Tuberculosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tuberculosis"]));
                                PlanTherapyDetails.Endometriosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometriosis"]));
                                PlanTherapyDetails.UterineFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineFactors"]));
                                PlanTherapyDetails.TubalFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TubalFactors"]));
                                PlanTherapyDetails.DiminishedOvarian = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DiminishedOvarian"]));
                                PlanTherapyDetails.PrematureOvarianFailure = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrematureOvarianFailure"]));
                                PlanTherapyDetails.LutealPhasedefect = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LutealPhasedefect"]));
                                PlanTherapyDetails.HypoThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HypoThyroid"]));
                                PlanTherapyDetails.MaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MaleFactors"]));
                                PlanTherapyDetails.OtherFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OtherFactors"]));
                                PlanTherapyDetails.UnknownFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnknownFactors"]));
                                PlanTherapyDetails.FemaleFactorsOnly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleFactorsOnly"]));
                                PlanTherapyDetails.FemaleandMaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleandMaleFactors"]));
                                PlanTherapyDetails.HyperThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HyperThyroid"]));

                                //By Anjali.................
                                PlanTherapyDetails.OPUtDate = (DateTime?)(DALHelper.HandleDate(reader["OPUDate"]));
                                PlanTherapyDetails.OPURemark = Convert.ToString(DALHelper.HandleDBNull(reader["OPURemark"]));

                                //By Anjali.............For IVF DashBoard........
                                PlanTherapyDetails.PlannedEmbryos = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                                PlanTherapyDetails.LongtermMedication = Convert.ToString(DALHelper.HandleDBNull(reader["LongtermMedication"]));
                                PlanTherapyDetails.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AssistedHatching"]));
                                PlanTherapyDetails.CryoPreservation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CryoPreservation"]));
                                PlanTherapyDetails.IMSI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IMSI"]));


                                BizAction.TherapyDetailsList.Add(PlanTherapyDetails);
                            }
                        }
                    }
                    else
                    {
                        if (BizAction.TabID == 0)
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyDocument.Add(TherapyDocuments(BizAction, reader));
                                }
                            }
                            reader.NextResult();
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
                        }
                        else if (BizAction.TabID == (int)TherapyTabs.Documents)//Document List
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyDocument.Add(TherapyDocuments(BizAction, reader));
                                }
                            }
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
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVF_GetTherapyListForLabDay");

                    dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizAction.CoupleID);
                    //dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUintID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.TherapyUnitID);

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
                            //BizAction.TherapyDetails.PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"]));
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

        private clsGetTherapyListBizActionVO GetTherapyDetailsSurrogate(clsGetTherapyListBizActionVO BizAction, clsUserVO objUserVO)
        {
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVF_GetTherapyListSurrogate");

                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, BizAction.TherapyID);
                dbServer.AddInParameter(command, "TabID", DbType.Int64, BizAction.TabID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUintID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.TherapyUnitID);

                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
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
                            PlanTherapyDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                            PlanTherapyDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                            PlanTherapyDetails.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                            PlanTherapyDetails.SurrogateMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMRNo"]));
                            PlanTherapyDetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                            PlanTherapyDetails.Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"]));

                            PlanTherapyDetails.PillStartDate = (DateTime?)(DALHelper.HandleDate(reader["PillStartDate"]));
                            PlanTherapyDetails.PillEndDate = (DateTime?)(DALHelper.HandleDate(reader["PillEndDate"]));
                            PlanTherapyDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                            PlanTherapyDetails.PlannedNoofEmbryos = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                            PlanTherapyDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                            PlanTherapyDetails.PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
                            PlanTherapyDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                            PlanTherapyDetails.PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"]));
                            PlanTherapyDetails.TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"]));

                            PlanTherapyDetails.LutealSupport = Convert.ToString(DALHelper.HandleDBNull(reader["LutealSupport"]));
                            PlanTherapyDetails.LutealRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["LutealRemarks"]));
                            PlanTherapyDetails.BHCGAss1Date = (DateTime?)DALHelper.HandleDate(reader["BHCGAss1Date"]);
                            PlanTherapyDetails.BHCGAss1IsBSCG = ((Boolean?)(DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"])));
                            if (PlanTherapyDetails.BHCGAss1IsBSCG != null)
                            {
                                if ((bool)PlanTherapyDetails.BHCGAss1IsBSCG)
                                    PlanTherapyDetails.BHCGAss1IsBSCGPositive = true;
                                else
                                    PlanTherapyDetails.BHCGAss1IsBSCGNagative = true;
                            }

                            //PlanTherapyDetails.BHCGAss1IsBSCG = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"]));
                            PlanTherapyDetails.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1BSCGValue"]));
                            PlanTherapyDetails.BHCGAss1SrProgest = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1SrProgest"]));
                            PlanTherapyDetails.BHCGAss2Date = (DateTime?)DALHelper.HandleDate(reader["BHCGAss2Date"]);
                            PlanTherapyDetails.BHCGAss2IsBSCG = (Boolean?)(DALHelper.HandleDBNull(reader["BHCGAss2IsBSCG"]));
                            if (PlanTherapyDetails.BHCGAss2IsBSCG != null)
                            {
                                if ((bool)PlanTherapyDetails.BHCGAss2IsBSCG)
                                    PlanTherapyDetails.BHCGAss2IsBSCGPostive = true;
                                else
                                    PlanTherapyDetails.BHCGAss2IsBSCGNagative = true;
                            }

                            PlanTherapyDetails.BHCGAss2BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2BSCGValue"]));
                            PlanTherapyDetails.BHCGAss2USG = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2USG"]));
                            PlanTherapyDetails.IsPregnancyAchieved = (Boolean?)DALHelper.HandleDBNull(reader["PregnancyAchieved"]);
                            if (PlanTherapyDetails.IsPregnancyAchieved != null)
                            {
                                if ((bool)PlanTherapyDetails.IsPregnancyAchieved)
                                    PlanTherapyDetails.IsPregnancyAchievedPostive = true;
                                else
                                    PlanTherapyDetails.IsPregnancyAchievedNegative = true;
                            }
                            //PlanTherapyDetails.IsPregnancyAchieved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PregnancyAchieved"]));
                            PlanTherapyDetails.PregnanacyConfirmDate = (DateTime?)(DALHelper.HandleDate(reader["PregnanacyConfirmDate"]));
                            PlanTherapyDetails.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                            PlanTherapyDetails.OutComeRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["OutComeRemarks"]));
                            PlanTherapyDetails.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                            PlanTherapyDetails.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                            PlanTherapyDetails.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                            PlanTherapyDetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                            PlanTherapyDetails.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogate"]));

                            //By Anjali.........On 21/01/2014
                            PlanTherapyDetails.OHSSEarly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSEarly"]));
                            PlanTherapyDetails.OHSSLate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSLate"]));
                            PlanTherapyDetails.OHSSMild = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMild"]));
                            PlanTherapyDetails.OHSSMode = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMode"]));
                            PlanTherapyDetails.OHSSRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OHSSRemark"]));
                            PlanTherapyDetails.OHSSSereve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSSereve"]));
                            PlanTherapyDetails.SIXmonthFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID"]));
                            PlanTherapyDetails.SIXmonthFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID_m"]));
                            PlanTherapyDetails.SIXmonthFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks"]));
                            PlanTherapyDetails.SIXmonthFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks_m"]));
                            PlanTherapyDetails.ONEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID"]));
                            PlanTherapyDetails.ONEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID_m"]));
                            PlanTherapyDetails.ONEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks"]));
                            PlanTherapyDetails.ONEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks_m"]));
                            PlanTherapyDetails.FIVEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID"]));
                            PlanTherapyDetails.FIVEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID_m"]));
                            PlanTherapyDetails.FIVEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks"]));
                            PlanTherapyDetails.FIVEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks_m"]));
                            PlanTherapyDetails.TENyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID"]));
                            PlanTherapyDetails.TENyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID_m"]));
                            PlanTherapyDetails.TENyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks"]));
                            PlanTherapyDetails.TENyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks_m"]));
                            PlanTherapyDetails.TWENTYyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID"]));
                            PlanTherapyDetails.TWENTYyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID_m"]));
                            PlanTherapyDetails.TWENTYyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks"]));
                            PlanTherapyDetails.TWENTYyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks_m"]));
                            PlanTherapyDetails.Missed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Missed"]));
                            PlanTherapyDetails.Incomplete = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Incomplete"]));
                            PlanTherapyDetails.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                            PlanTherapyDetails.FetalDate = (DateTime?)(DALHelper.HandleDate(reader["FetalDate"]));
                            PlanTherapyDetails.Count = Convert.ToString(DALHelper.HandleDBNull(reader["Count"]));
                            PlanTherapyDetails.IUD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IUD"]));
                            PlanTherapyDetails.LiveBirth = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LiveBirth"]));
                            PlanTherapyDetails.Congenitalabnormality = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Congenitalabnormality"]));
                            PlanTherapyDetails.IsChemicalPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChemicalPregnancy"]));
                            PlanTherapyDetails.PretermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PretermDelivery"]));
                            PlanTherapyDetails.IsFullTermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFullTermDelivery"]));
                            PlanTherapyDetails.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));
                            PlanTherapyDetails.BiochemPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BiochemPregnancy"]));
                            PlanTherapyDetails.Ectopic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ectopic"]));
                            if (PlanTherapyDetails.Missed == true || PlanTherapyDetails.Incomplete == true)
                                PlanTherapyDetails.Abortion = true;
                            else
                                PlanTherapyDetails.Abortion = false;
                            PlanTherapyDetails.PCOS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PCOS"]));
                            PlanTherapyDetails.Hypogonadotropic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypogonadotropic"]));
                            PlanTherapyDetails.Tuberculosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tuberculosis"]));
                            PlanTherapyDetails.Endometriosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometriosis"]));
                            PlanTherapyDetails.UterineFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineFactors"]));
                            PlanTherapyDetails.TubalFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TubalFactors"]));
                            PlanTherapyDetails.DiminishedOvarian = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DiminishedOvarian"]));
                            PlanTherapyDetails.PrematureOvarianFailure = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrematureOvarianFailure"]));
                            PlanTherapyDetails.LutealPhasedefect = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LutealPhasedefect"]));
                            PlanTherapyDetails.HypoThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HypoThyroid"]));
                            PlanTherapyDetails.MaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MaleFactors"]));
                            PlanTherapyDetails.OtherFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OtherFactors"]));
                            PlanTherapyDetails.UnknownFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnknownFactors"]));
                            PlanTherapyDetails.FemaleFactorsOnly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleFactorsOnly"]));
                            PlanTherapyDetails.FemaleandMaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleandMaleFactors"]));
                            PlanTherapyDetails.HyperThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HyperThyroid"]));
                            PlanTherapyDetails.FetalHeartSound = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FetalHeartSound"]));



                            BizAction.TherapyDetailsList.Add(PlanTherapyDetails);
                        }
                    }
                }
                //main therapy execution
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
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //if (BizAction.IsSurrogate == false)
                                //    BizAction.TherapyExecutionList.Add(TherapyExcecution(BizAction, reader));
                                //else
                                BizAction.TherapyExecutionListSurrogate.Add(TherapyExcecutionSurrogate(BizAction, reader));
                            }
                        }
                        //reader.NextResult();
                        //if (reader.HasRows)
                        //{
                        //    while (reader.Read())
                        //    {
                        //        BizAction.FollicularMonitoringList.Add(FollicularMonitoring(BizAction, reader));
                        //    }
                        //}
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (BizAction.ANCList != null)
                                {
                                    BizAction.ANCList.Add(ANCVisit(BizAction, reader));
                                }
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //BizAction.TherapyDelivery.Add(Therapydetail(BizAction, reader));
                                clsTherapyDeliveryVO Delivery = new clsTherapyDeliveryVO();
                                Delivery.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                Delivery.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                                Delivery.ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"]));
                                Delivery.DeliveryDate = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                                Delivery.Baby = Convert.ToString(DALHelper.HandleDBNull(reader["Baby"]));
                                Delivery.Mode = Convert.ToString(DALHelper.HandleDBNull(reader["Mode"]));
                                Delivery.TimeofBirth = (DateTime?)(DALHelper.HandleDate(reader["TimeofBirth"]));
                                Delivery.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                                //BizAction.TherapyDetailsList.Add(PlanTherapyDetails);
                                BizAction.TherapyDelivery = Delivery;
                            }
                        }
                    }
                    //else if (BizAction.TabID == (int)TherapyTabs.Documents)//Document List
                    //{
                    //    if (reader.HasRows)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            BizAction.TherapyDocument.Add(TherapyDocuments(BizAction, reader));
                    //        }
                    //    }
                    //}
                    else if (BizAction.TabID == (int)TherapyTabs.Execution)//Therapy Execution Lsit
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //if(BizAction.IsSurrogate==false)
                                //  BizAction.TherapyExecutionList.Add(TherapyExcecution(BizAction, reader));
                                //else
                                BizAction.TherapyExecutionListSurrogate.Add(TherapyExcecutionSurrogate(BizAction, reader));
                            }
                        }
                    }
                    //else if (BizAction.TabID == (int)TherapyTabs.FollicularMonitoring)//Follicular Monitoring
                    //{
                    //    if (reader.HasRows)
                    //    {
                    //        while (reader.Read())
                    //        {
                    //            BizAction.FollicularMonitoringList.Add(FollicularMonitoring(BizAction, reader));
                    //        }
                    //    }
                    //}
                    //added by priti
                    else if (BizAction.TabID == (int)TherapyTabs.ANCVisit)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.ANCList.Add(ANCVisit(BizAction, reader));
                            }
                        }
                    }
                    else if (BizAction.TabID == (int)TherapyTabs.Deliverydetails)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //BizAction.TherapyDelivery.Add(Therapydetail(BizAction, reader));
                                clsTherapyDeliveryVO Delivery = new clsTherapyDeliveryVO();
                                Delivery.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                Delivery.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                                Delivery.ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"]));
                                Delivery.DeliveryDate = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                                Delivery.Baby = Convert.ToString(DALHelper.HandleDBNull(reader["Baby"]));
                                Delivery.Mode = Convert.ToString(DALHelper.HandleDBNull(reader["Mode"]));
                                Delivery.TimeofBirth = (DateTime?)(DALHelper.HandleDate(reader["TimeofBirth"]));
                                Delivery.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                                BizAction.TherapyDelivery = Delivery;
                            }
                        }
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
            return BizAction;
        }

        //added for surrogacy
        public clsTherapyExecutionVO TherapyExcecutionSurrogate(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
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
            if ((int)TherapyGroupSurrogate.Event == (int)TherapyExeSurrogate.TherapyTypeId)
            {
                TherapyExeSurrogate.TherapyGroup = TherapyGroup.Event.ToString();
                TherapyExeSurrogate.Head = "Date of LP";
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
            else if ((int)TherapyGroupSurrogate.EmbryoTransfer == (int)TherapyExeSurrogate.TherapyTypeId)
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

        public clsTherapyANCVO ANCVisit(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            clsTherapyANCVO ANCVisit = new clsTherapyANCVO();
            ANCVisit.ANCID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            ANCVisit.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
            ANCVisit.ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"]));
            ANCVisit.ANCDate = (DateTime?)(DALHelper.HandleDate(reader["ANCDate"]));
            ANCVisit.Findings = Convert.ToString(DALHelper.HandleDBNull(reader["Findings"]));
            ANCVisit.POG = Convert.ToString(DALHelper.HandleDBNull(reader["POG"]));
            ANCVisit.USGReproductive = Convert.ToString(DALHelper.HandleDBNull(reader["USGReproductive"]));
            ANCVisit.Investigation = Convert.ToString(DALHelper.HandleDBNull(reader["Investigations"]));
            ANCVisit.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
            return ANCVisit;
        }

        //

        public clsTherapyExecutionVO TherapyExcecution(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
        {

            clsTherapyExecutionVO TherapyExe = new clsTherapyExecutionVO();
            TherapyExe.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            TherapyExe.PlanTherapyId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
            TherapyExe.PlanTherapyUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitId"]));
            TherapyExe.TherapyTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyTypeId"]));
            TherapyExe.PhysicianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"]));
            TherapyExe.TherapyStartDate = (DateTime?)(DALHelper.HandleDate(reader["TherapyStartDate"]));
            TherapyExe.ThearpyTypeDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"]));

            #region Set Proprties According to Therpy Group
            if ((int)TherapyGroup.Event == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.Event.ToString();
                TherapyExe.Head = "Date of LP";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
            }
            else if ((int)TherapyGroup.Drug == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.Drug.ToString();
                TherapyExe.IsBool = false;
                TherapyExe.IsText = true;
                TherapyExe.Head = Convert.ToString((DALHelper.HandleDBNull(reader["DrugName"])));
            }
            else if ((int)TherapyGroup.UltraSound == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.UltraSound.ToString();
                TherapyExe.Head = "Follicular US";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
            }

            else if ((int)TherapyGroup.OvumPickUp == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.OvumPickUp.ToString();
                TherapyExe.Head = "OPU";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
            }
            else if ((int)TherapyGroup.EmbryoTransfer == (int)TherapyExe.TherapyTypeId)
            {
                TherapyExe.TherapyGroup = TherapyGroup.EmbryoTransfer.ToString();
                TherapyExe.Head = "ET";
                TherapyExe.IsBool = true;
                TherapyExe.IsText = false;
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
            TherapyExe.Day35 = Convert.ToString((DALHelper.HandleDBNull(reader["Day34"])));
            TherapyExe.Day36 = Convert.ToString((DALHelper.HandleDBNull(reader["Day35"])));
            TherapyExe.Day37 = Convert.ToString((DALHelper.HandleDBNull(reader["Day36"])));
            TherapyExe.Day38 = Convert.ToString((DALHelper.HandleDBNull(reader["Day37"])));
            TherapyExe.Day39 = Convert.ToString((DALHelper.HandleDBNull(reader["Day38"])));
            TherapyExe.Day40 = Convert.ToString((DALHelper.HandleDBNull(reader["Day39"])));
            TherapyExe.Day41 = Convert.ToString((DALHelper.HandleDBNull(reader["Day40"])));
            TherapyExe.Day42 = Convert.ToString((DALHelper.HandleDBNull(reader["Day41"])));
            TherapyExe.Day43 = Convert.ToString((DALHelper.HandleDBNull(reader["Day42"])));
            TherapyExe.Day44 = Convert.ToString((DALHelper.HandleDBNull(reader["Day43"])));
            TherapyExe.Day45 = Convert.ToString((DALHelper.HandleDBNull(reader["Day44"])));
            TherapyExe.Day46 = Convert.ToString((DALHelper.HandleDBNull(reader["Day45"])));
            TherapyExe.Day47 = Convert.ToString((DALHelper.HandleDBNull(reader["Day46"])));
            TherapyExe.Day48 = Convert.ToString((DALHelper.HandleDBNull(reader["Day47"])));
            TherapyExe.Day49 = Convert.ToString((DALHelper.HandleDBNull(reader["Day48"])));
            TherapyExe.Day50 = Convert.ToString((DALHelper.HandleDBNull(reader["Day49"])));
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

        public clsTherapyDocumentsVO TherapyDocuments(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
        {

            clsTherapyDocumentsVO TherapyDoc = new clsTherapyDocumentsVO();
            TherapyDoc.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
            TherapyDoc.ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"]));
            TherapyDoc.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
            TherapyDoc.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
            TherapyDoc.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
            TherapyDoc.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
            TherapyDoc.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
            TherapyDoc.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));

            return TherapyDoc;
        }

        public clsFollicularMonitoring FollicularMonitoring(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
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
            return FollicularMon;
        }

        public override IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetFollicularMonitoringSizeDetailsBizActionVO BizAction = (valueObject) as clsGetFollicularMonitoringSizeDetailsBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVF_GetFollicularMonitoringSizeList");

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


        public override IValueObject GetTherapyDrugDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsgetTherapyDrugDetailsBizActionVO BizAction = (valueObject) as clsgetTherapyDrugDetailsBizActionVO;
            BizAction.TherapyDrugDetails = new clsTherapyDrug();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVF_GetTherapyDrugDetails");

                dbServer.AddInParameter(command, "TherapyExeID", DbType.Int64, BizAction.TherapyExeID);

                dbServer.AddInParameter(command, "DayNo", DbType.String, BizAction.DayNo);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);

                BizAction.TherapyDrugDetails = new clsTherapyDrug();

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.TherapyDrugDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"]));
                        //BizAction.TherapyDrugDetails.Dosage = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                        BizAction.TherapyDrugDetails.DrugDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.TherapyDrugDetails.DrugNotes = Convert.ToString(DALHelper.HandleDBNull(reader["DrugNotes"]));
                        //BizAction.TherapyDrugDetails.ForDays = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
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

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {

            Type type = control.GetType();

            string propertyName = pName;

            BindingFlags flags = BindingFlags.GetProperty;

            Binder binder = null;

            object[] args = null;

            object value = type.InvokeMember(

            propertyName,

            flags,

            binder,

            control,

            args

            );

            return value.ToString();

        }

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {
            Type type = control.GetType();
            string propertyName = pName;
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo prop = type.GetProperty(propertyName, flags);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(control, value, null);
            }
        }

        #endregion

        #region Vitrification(Created By Shikha)

        public override IValueObject getVitrificationDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetVitrificationDetailsBizActionVO BizAction = (valueObject) as clsGetVitrificationDetailsBizActionVO;
            BizAction.Vitrification = new clsGetVitrificationVO();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetVirtificationDetailsSavedInLabDays");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);
                dbServer.AddInParameter(command, "FromID", DbType.String, BizAction.FromID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (BizAction.IsEdit == false)// If IsEdit False Then Details Comes From Lab Day 0 to Lab Day 6
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsGetVitrificationDetailsVO vitdetails = new clsGetVitrificationDetailsVO();
                            vitdetails.CanID = "0";
                            vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["LabNo"]));
                            vitdetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"]));
                            vitdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["OoNo"]));
                            //By Anjali.............
                            vitdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                            //................

                            vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                            vitdetails.SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOocyteID"]));
                            vitdetails.SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOfSemen"]));
                            vitdetails.OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                            vitdetails.SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SemenDonorID"]));
                            vitdetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["PlanTreatmentID"]));
                            vitdetails.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                            vitdetails.TransferDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                            BizAction.Vitrification.VitrificationDetails.Add(vitdetails);
                        }
                    }
                }
                else //In this Case Details Comes from Vitrifiction and Vitrification Detials
                {
                    if (BizAction.ID == 0 && BizAction.UnitID == 0)// All Vitrifictaion Against the Couple
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsGetVitrificationDetailsVO vitdetails = new clsGetVitrificationDetailsVO();
                                clsThawingDetailsVO thawingDetails = new clsThawingDetailsVO();
                                vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrivicationID"]));
                                vitdetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                                vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                                vitdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));

                                //By Anjali.............
                                vitdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));
                                thawingDetails.SerialOccyteNo = vitdetails.SerialOccyteNo;
                                //................
                                thawingDetails.EmbNo = vitdetails.EmbNo;
                                thawingDetails.VitrificationID = vitdetails.ID;
                                vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                                vitdetails.SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"]));
                                vitdetails.SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"]));
                                vitdetails.OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"]));
                                vitdetails.SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"]));
                                vitdetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                                vitdetails.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                                vitdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                                vitdetails.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"]));
                                vitdetails.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawId"]));
                                vitdetails.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                                vitdetails.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                                vitdetails.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                                vitdetails.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankId"]));
                                vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                                vitdetails.LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"]));
                                vitdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                                vitdetails.Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                                vitdetails.ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                                vitdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                                System.Drawing.Color newCol = (System.Drawing.ColorTranslator.FromHtml(vitdetails.ColorCode));
                                vitdetails.SelectesColor = System.Windows.Media.Color.FromArgb(newCol.A, newCol.R, newCol.G, newCol.B);
                                BizAction.Vitrification.VitrificationDetails.Add(vitdetails);
                                BizAction.Vitrification.ThawingDetails.Add(thawingDetails);
                            }
                        }
                    }
                    else// Sepecific Vitrification,Vitrification Details ,Media Details By Id Against the Couple
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                BizAction.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                BizAction.Vitrification.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                                BizAction.Vitrification.VitrificationDate = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
                                BizAction.Vitrification.PickupDate = (DateTime?)(DALHelper.HandleDate(reader["PickUpDate"]));
                                BizAction.Vitrification.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                                BizAction.Vitrification.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));


                                if (BizAction.Vitrification.ConsentForm)
                                {
                                    BizAction.Vitrification.ConsentFormYes = true;
                                }
                                else
                                {
                                    BizAction.Vitrification.ConsentFormNo = true;
                                }
                            }
                        }

                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsGetVitrificationDetailsVO vitdetails = new clsGetVitrificationDetailsVO();
                                vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                vitdetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                                vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                                vitdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));

                                //By Anjali.............
                                vitdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                                //................
                                vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                                vitdetails.SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"]));
                                vitdetails.SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"]));
                                vitdetails.OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"]));
                                vitdetails.SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"]));
                                vitdetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                                vitdetails.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                                vitdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                                vitdetails.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"]));
                                vitdetails.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawId"]));
                                vitdetails.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                                vitdetails.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                                vitdetails.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                                vitdetails.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankId"]));
                                vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                                vitdetails.LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"]));
                                vitdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                                vitdetails.Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                                vitdetails.ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                                vitdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                                System.Drawing.Color newCol = (System.Drawing.ColorTranslator.FromHtml(vitdetails.ColorCode));
                                vitdetails.SelectesColor = System.Windows.Media.Color.FromArgb(newCol.A, newCol.R, newCol.G, newCol.B);
                                BizAction.Vitrification.VitrificationDetails.Add(vitdetails);
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsFemaleMediaDetailsVO vitdetails = new clsFemaleMediaDetailsVO();
                                vitdetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["LotNo"]));
                                vitdetails.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                                vitdetails.Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                                vitdetails.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                                vitdetails.DetailedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailID"]));
                                vitdetails.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                                //vitdetails.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SSCode"]));
                                vitdetails.VolumeUsed = Convert.ToString(DALHelper.HandleDBNull(reader["VolumeUsed"]));
                                vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                vitdetails.ItemName = Convert.ToString((reader["MediaName"]));
                                vitdetails.MianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteID"]));
                                vitdetails.OSM = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OSM"]));
                                vitdetails.PH = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PH"]));
                                vitdetails.StatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"]));
                                vitdetails.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                                vitdetails.SelectedStatus.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"]));
                                vitdetails.SelectedStatus = vitdetails.Status.FirstOrDefault(q => q.ID == vitdetails.SelectedStatus.ID);
                                BizAction.MediaDetails.Add(vitdetails);
                            }
                            for (int i = 0; i < BizAction.Vitrification.VitrificationDetails.Count; i++)
                            {
                                var Mediadetils = from p in BizAction.MediaDetails where p.DetailedID == BizAction.Vitrification.VitrificationDetails[i].ID select p;
                                BizAction.Vitrification.VitrificationDetails[i].MediaDetails = ((List<clsFemaleMediaDetailsVO>)Mediadetils.ToList());

                            }

                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FileUpload Filedetails = new FileUpload();
                                Filedetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                Filedetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                                Filedetails.Index = Convert.ToInt16(DALHelper.HandleDBNull(reader["FileIndex"]));
                                Filedetails.Data = Convert.FromBase64String(reader["Data"].ToString());
                                BizAction.Vitrification.FUSetting.Add(Filedetails);
                            }
                        }

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

        public override IValueObject AddUpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //Calling Database Multiple times i Used Datatable and Convert it to Xml  
            clsAddUpdateVitrificationBizActionVO BizActionObj = valueObject as clsAddUpdateVitrificationBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {

                con.Open();

                DbCommand command = dbServer.GetStoredProcCommand("IVF_AddUpdateVitrificationDetails");
                DataTable vitdt = new DataTable();

                #region Vitrification Details

                vitdt = ListToDataTable(BizActionObj.Vitrification.VitrificationDetails);
                vitdt.TableName = "VitrificationDetails";

                string VitrificationDetailsresult;
                using (StringWriter sw = new StringWriter())
                {
                    vitdt.WriteXml(sw); VitrificationDetailsresult = sw.ToString();
                }

                #endregion

                #region Media Details

                string MediaDetailsresult = "";
                int j = -1;
                vitdt = new DataTable();
                foreach (clsGetVitrificationDetailsVO item in BizActionObj.Vitrification.VitrificationDetails)
                {
                    for (int i = 0; i < item.MediaDetails.Count; i++)
                    {
                        item.MediaDetails[i].DetailedID = j;
                        item.MediaDetails[i].StatusID = item.MediaDetails[i].SelectedStatus.ID;
                    }
                    vitdt = ListToDataTable(item.MediaDetails);
                    using (StringWriter sw = new StringWriter())
                    {
                        vitdt.WriteXml(sw); MediaDetailsresult = MediaDetailsresult + " " + sw.ToString();
                    }
                    j = j + (-1);
                }

                #endregion

                #region Upload File Details

                vitdt = ListToDataTable(BizActionObj.Vitrification.FUSetting);
                vitdt.TableName = "FUSettingDetails";

                string FUSettingDetailsresult;
                using (StringWriter sw = new StringWriter())
                {
                    vitdt.WriteXml(sw); FUSettingDetailsresult = sw.ToString();
                }


                #endregion

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.CoupleUintID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "VitrificationNo", DbType.String, BizActionObj.Vitrification.VitrificationNo);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.Vitrification.VitrificationDate);
                dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, BizActionObj.Vitrification.PickupDate);
                dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, BizActionObj.Vitrification.ConsentForm);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.Vitrification.IsFreezed);
                dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizActionObj.Vitrification.IsOnlyVitrification);


                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Vitrification.Impression);

                dbServer.AddInParameter(command, "VitrificationDetails", DbType.Xml, VitrificationDetailsresult);
                dbServer.AddInParameter(command, "MediaDetails", DbType.Xml, MediaDetailsresult);
                dbServer.AddInParameter(command, "FUSettingDetails", DbType.Xml, FUSettingDetailsresult);


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
                BizActionObj.Vitrification = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }

        #region Change List to Data Table
        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            dt.TableName = "MyTable";
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(info.Name);
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {

                    if (info.PropertyType.FullName.Equals("System.Byte[]"))
                    {
                        byte[] tempcontents = (byte[])info.GetValue(t, null);
                        //row[info.Name] = ((System.Data.SqlTypes.SqlBinary) tempcontents).Value;
                        if (tempcontents != null)
                        {
                            row[info.Name] = Convert.ToBase64String(tempcontents, 0, tempcontents.Length, Base64FormattingOptions.None);
                        }
                        //row[info.Name] = Convert.ToString(tempcontents);
                        //row[info.Name] = by
                        //byte[] tempcontents1 = Convert.FromBase64String(row[info.Name].ToString());


                    }
                    else
                    {
                        row[info.Name] = info.GetValue(t, null);
                    }

                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        #endregion

        //Added By Saily P
        public override IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetVitrificationForCryoBankBizActionVO BizAction = (valueObject) as clsGetVitrificationForCryoBankBizActionVO;
            BizAction.Vitrification = new clsGetVitrificationVO();
            //BizAction.Vitrification. = new List<clsPatientGeneralVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetVirtificationDetailsEmbryoCryoBank");
                //  dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                //Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                dbServer.AddInParameter(command, "FName", DbType.String, BizAction.FName + "%"); // Security.base64Encode(BizAction.FName) + "%");
                dbServer.AddInParameter(command, "MName", DbType.String, BizAction.MName + "%"); //Security.base64Encode(BizAction.MName) + "%");
                dbServer.AddInParameter(command, "LName", DbType.String, BizAction.LName + "%"); //Security.base64Encode(BizAction.LName) + "%");
                dbServer.AddInParameter(command, "FamilyName", DbType.String, BizAction.FamilyName + "%"); //Security.base64Encode(BizAction.FamilyName) + "%");
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNo + "%");

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGetVitrificationDetailsVO vitdetails = new clsGetVitrificationDetailsVO();
                        vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        // vitdetails.VitrificationNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        vitdetails.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]));
                        //By Anjali.............
                        vitdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                        //................

                        vitdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));
                        vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        System.Drawing.Color newCol = (System.Drawing.ColorTranslator.FromHtml(vitdetails.ColorCode));
                        vitdetails.SelectesColor = System.Windows.Media.Color.FromArgb(newCol.A, newCol.R, newCol.G, newCol.B);
                        // vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        vitdetails.TransferDate = Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]));
                        vitdetails.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        vitdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        vitdetails.DoneThawing = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                        vitdetails.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankName"]));
                        vitdetails.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["CanisterName"]));
                        vitdetails.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        vitdetails.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        vitdetails.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CaneName"]));
                        vitdetails.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawName"]));
                        vitdetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        vitdetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        vitdetails.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        vitdetails.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        vitdetails.PatientUnitName = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationUnit"]));
                        BizAction.Vitrification.VitrificationDetails.Add(vitdetails);
                    }
                }
                reader.NextResult();
                BizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");


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

        public override IValueObject AddDiscardForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            clsGetSpremFreezingDetailsBizActionVO BizActionObj = (valueObject) as clsGetSpremFreezingDetailsBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Vitrification != null && BizActionObj.Vitrification.Count > 0)
                {
                    foreach (var item in BizActionObj.Vitrification)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDiscardSpermForCryoBank");
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "SpremFreezingID", DbType.Int64, item.SpremFreezingID);
                        dbServer.AddInParameter(command2, "SpremFreezingUnitID", DbType.Int64, item.SpremFreezingUnitID);
                        dbServer.AddInParameter(command2, "SpremNo", DbType.Int64, item.SpremNo);
                        dbServer.AddInParameter(command2, "ID", DbType.Int64, item.ID);                        
                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }                
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Vitrification = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }



        public override IValueObject GetVitrificationForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetSpremFreezingDetailsBizActionVO BizAction = (valueObject) as clsGetSpremFreezingDetailsBizActionVO;
            BizAction.Vitrification = new List<clsSpermFreezingVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetVirtificationDetailsSpermCryoBank");


                dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);
                dbServer.AddInParameter(command, "FName", DbType.String, BizAction.FName + "%");// Security.base64Encode(BizAction.FName) + "%");
                dbServer.AddInParameter(command, "MName", DbType.String, BizAction.MName + "%"); //Security.base64Encode(BizAction.MName) + "%");
                dbServer.AddInParameter(command, "LName", DbType.String, BizAction.LName + "%"); //Security.base64Encode(BizAction.LName) + "%");
                dbServer.AddInParameter(command, "FamilyName", DbType.String, BizAction.FamilyName + "%"); //Security.base64Encode(BizAction.FamilyName) + "%");
                dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNo + "%");
                //rohini
                dbServer.AddInParameter(command, "Cane", DbType.String, BizAction.Cane);
                //
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "PatientID", DbType.String, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.String, BizAction.PatientUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsSpermFreezingVO vitdetails = new clsSpermFreezingVO();
                        vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        vitdetails.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]));
                        vitdetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        vitdetails.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        vitdetails.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        vitdetails.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["GobletColor"]));
                        vitdetails.GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["GobletColor"]));
                        vitdetails.SpermCount = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCount"]));
                        vitdetails.Motility = Convert.ToString(DALHelper.HandleDBNull(reader["Motility"]));
                        vitdetails.Volume = Convert.ToString(DALHelper.HandleDBNull(reader["Volume"]));
                        vitdetails.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        vitdetails.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        vitdetails.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        vitdetails.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        vitdetails.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"]));
                        vitdetails.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        vitdetails.PatientUnitName = Convert.ToString(DALHelper.HandleDBNull(reader["ThawingUnit"]));
                        vitdetails.SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"]));
                        vitdetails.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        vitdetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        vitdetails.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        vitdetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        vitdetails.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremExpiryDate"]));
                        vitdetails.SpremFreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremFreezingID"]));
                        vitdetails.SpremFreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremFreezingUnitID"]));
                        vitdetails.LongTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LongTerm"]));
                        vitdetails.ShortTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ShortTerm"]));
                        vitdetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        vitdetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        vitdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Vitrification.Add(vitdetails);
                    }
                }

                reader.NextResult();
                BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
               
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetSpermVitrificationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetSpermVitrificationBizActionVO BizAction = (valueObject) as clsGetSpermVitrificationBizActionVO;
            BizAction.Vitrification = new clsSpermVitrifiactionVO();

            try
            {
                DbCommand command = null;

                if (BizAction.IsDonor == false)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_GetSpermVirtificationDetails");
                    dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                    //dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                    //dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);
                }
                else if (BizAction.IsDonor == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_GetDonorSpermVirtificationDetails");
                    dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizAction.PatientId);
                    dbServer.AddInParameter(command, "PatientUnitId", DbType.String, BizAction.PatientUnitId);
                    //dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                    //dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);
                    // dbServer.AddInParameter(command, "IsDonor", DbType.Boolean, BizAction.IsDonor);
                }

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsSpermVitrificationDetailsVO ObjBizAction = new clsSpermVitrificationDetailsVO();

                        ObjBizAction.Volume = Convert.ToString(DALHelper.HandleDBNull(reader["Volume"]));
                        ObjBizAction.SpermCount = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCount"]));
                        ObjBizAction.Motillity = Convert.ToString(DALHelper.HandleDBNull(reader["Motility"]));
                        ObjBizAction.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"]));
                        ObjBizAction.CanisterNo = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        ObjBizAction.FreezingDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["dtpFreezingDate"]));
                        if (ObjBizAction.FreezingDate != null)
                            ObjBizAction.FreezingDate = ObjBizAction.FreezingDate.Value.Date;
                        ObjBizAction.FreezingTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["dtpFreezingTime"]));
                        //if (ObjBizAction.FreezingTime != null)
                        ObjBizAction.FreezingTime = (DateTime?)ObjBizAction.FreezingTime.Value.ToLocalTime();
                        ObjBizAction.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        ObjBizAction.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        ObjBizAction.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        ObjBizAction.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        ObjBizAction.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingNo"]));
                        ObjBizAction.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["GobletColor"]));
                        //System.Drawing.Color newCol = (System.Drawing.ColorTranslator.FromHtml(ObjBizAction.ColorCode));
                        //ObjBizAction.SelectesColor = System.Windows.Media.Color.FromArgb(newCol.A, newCol.R, newCol.G, newCol.B);
                        ObjBizAction.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingComments"]));
                        BizAction.Vitrification.VitrificationDetails.Add(ObjBizAction);
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsSpermThawingDetailsVO objThawBizAction = new clsSpermThawingDetailsVO();
                        objThawBizAction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objThawBizAction.Motillity = Convert.ToString(DALHelper.HandleDBNull(reader["Motility"]));
                        objThawBizAction.SpermCount = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCount"]));
                        objThawBizAction.ThawingDate = (DateTime?)(DALHelper.HandleDBNull(reader["ThawingDate"]));
                        objThawBizAction.ThawingTime = (DateTime?)(DALHelper.HandleDBNull(reader["ThawingTime"]));
                        objThawBizAction.PostThawPlanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PostThawPlanId"]));
                        objThawBizAction.PostThawPlan = Convert.ToString(DALHelper.HandleDBNull(reader["PostThawPlan"]));
                        objThawBizAction.VitrificationNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrificationID"]));
                        objThawBizAction.Volume = Convert.ToString(DALHelper.HandleDBNull(reader["Volume"]));
                        objThawBizAction.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        objThawBizAction.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objThawBizAction.LabInchargeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        objThawBizAction.LabInchargeName = Convert.ToString(DALHelper.HandleDBNull(reader["LabPersonName"]));

                        BizAction.Vitrification.ThawingDetails.Add(objThawBizAction);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizAction;// BizAction;
        }

        public override IValueObject GetSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Thawing
        public override IValueObject getThawingDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetThawingDetailsBizActionVO BizAction = (valueObject) as clsGetThawingDetailsBizActionVO;
            BizAction.Thawing = new clsThawingVO();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetThawingDetails");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (BizAction.IsEdit == false)// If IsEdit False Then Details Comes From Lab Day 0 to Lab Day 6
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                        }
                    }
                }
                else //In this Case Details Comes from Vitrifiction and Vitrification Detials
                {


                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsGetVitrificationDetailsVO vitdetails = new clsGetVitrificationDetailsVO();
                            vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            vitdetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                            vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                            vitdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));
                            //By Anjali.............
                            vitdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                            //................
                            vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                            vitdetails.SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"]));
                            vitdetails.SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"]));
                            vitdetails.OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"]));
                            vitdetails.SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"]));
                            vitdetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                            vitdetails.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                            vitdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                            vitdetails.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"]));
                            vitdetails.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawId"]));
                            vitdetails.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                            vitdetails.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                            vitdetails.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                            vitdetails.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankId"]));
                            vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                            vitdetails.LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"]));
                            vitdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                            vitdetails.Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                            vitdetails.ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                            vitdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                            System.Drawing.Color newCol = (System.Drawing.ColorTranslator.FromHtml(vitdetails.ColorCode));
                            vitdetails.SelectesColor = System.Windows.Media.Color.FromArgb(newCol.A, newCol.R, newCol.G, newCol.B);
                            BizAction.VitrificationDetails.Add(vitdetails);
                        }
                    }

                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BizAction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            BizAction.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            BizAction.Thawing.LabPerseonID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                            BizAction.Thawing.Date = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsThawingDetailsVO Thawdetails = new clsThawingDetailsVO();
                            Thawdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            Thawdetails.VitrificationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrificationID"]));
                            Thawdetails.CellStangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                            Thawdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                            Thawdetails.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                            Thawdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));
                            //By Anjali.............
                            Thawdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                            //................

                            Thawdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                            Thawdetails.Plan = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Plan"]));
                            Thawdetails.Status = Convert.ToString(DALHelper.HandleDBNull(reader["Status"]));
                            Thawdetails.SelectedCellStage.ID = Thawdetails.CellStangeID;
                            //Thawdetails.SelectedCellStage = Thawdetails.CellStage.FirstOrDefault(q => q.ID == Thawdetails.SelectedCellStage.ID);
                            Thawdetails.SelectedGrade.ID = Thawdetails.GradeID;
                            // Thawdetails.SelectedGrade = Thawdetails.Grade.FirstOrDefault(q => q.ID == Thawdetails.SelectedGrade.ID);
                            BizAction.Thawing.ThawingDetails.Add(Thawdetails);
                        }
                    }
                    reader.NextResult();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsFemaleMediaDetailsVO Mediadetails = new clsFemaleMediaDetailsVO();
                            Mediadetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["LotNo"]));
                            Mediadetails.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                            Mediadetails.Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                            Mediadetails.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                            Mediadetails.DetailedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailID"]));
                            Mediadetails.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                            Mediadetails.VolumeUsed = Convert.ToString(DALHelper.HandleDBNull(reader["VolumeUsed"]));
                            Mediadetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            Mediadetails.ItemName = Convert.ToString((reader["MediaName"]));
                            Mediadetails.MianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteID"]));
                            Mediadetails.OSM = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OSM"]));
                            Mediadetails.PH = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PH"]));
                            Mediadetails.StatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"]));
                            Mediadetails.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                            Mediadetails.SelectedStatus.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"]));
                            Mediadetails.SelectedStatus = Mediadetails.Status.FirstOrDefault(q => q.ID == Mediadetails.SelectedStatus.ID);
                            BizAction.MediaDetails.Add(Mediadetails);
                        }
                        for (int i = 0; i < BizAction.Thawing.ThawingDetails.Count; i++)
                        {
                            var Mediadetils = from p in BizAction.MediaDetails where p.DetailedID == BizAction.Thawing.ThawingDetails[i].ID select p;
                            BizAction.Thawing.ThawingDetails[i].MediaDetails = ((List<clsFemaleMediaDetailsVO>)Mediadetils.ToList());

                        }

                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            FileUpload Filedetails = new FileUpload();
                            Filedetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            Filedetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                            Filedetails.Index = Convert.ToInt16(DALHelper.HandleDBNull(reader["FileIndex"]));
                            Filedetails.Data = Convert.FromBase64String(reader["Data"].ToString());
                            BizAction.Thawing.FUSetting.Add(Filedetails);
                        }
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

        public override IValueObject AddUpdateThawingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //Calling Database Multiple times i Used Datatable and Convert it to Xml  
            clsAddUpdateThawingBizActionVO BizActionObj = valueObject as clsAddUpdateThawingBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVF_AddUpdateThawingDetails");
                DataTable vitdt = new DataTable();

                #region Thawing Details

                vitdt = ListToDataTable(BizActionObj.Thawing.ThawingDetails);
                vitdt.TableName = "ThawingDetails";

                string ThawingDetailsresult;
                using (StringWriter sw = new StringWriter())
                {
                    vitdt.WriteXml(sw); ThawingDetailsresult = sw.ToString();
                }

                #endregion

                #region Media Details

                string MediaDetailsresult = "";
                int j = -1;
                vitdt = new DataTable();
                foreach (clsThawingDetailsVO item in BizActionObj.Thawing.ThawingDetails)
                {
                    for (int i = 0; i < item.MediaDetails.Count; i++)
                    {
                        item.MediaDetails[i].DetailedID = j;
                        item.MediaDetails[i].StatusID = item.MediaDetails[i].SelectedStatus.ID;
                    }
                    vitdt = ListToDataTable(item.MediaDetails);
                    using (StringWriter sw = new StringWriter())
                    {
                        vitdt.WriteXml(sw); MediaDetailsresult = MediaDetailsresult + " " + sw.ToString();
                    }
                    j = j + (-1);
                }

                #endregion

                #region Upload File Details

                vitdt = ListToDataTable(BizActionObj.Thawing.FUSetting);
                vitdt.TableName = "FUSettingDetails";

                string FUSettingDetailsresult;
                using (StringWriter sw = new StringWriter())
                {
                    vitdt.WriteXml(sw); FUSettingDetailsresult = sw.ToString();
                }


                #endregion

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.CoupleUintID);

                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.Thawing.Date);

                dbServer.AddInParameter(command, "LabPersonId", DbType.Int64, BizActionObj.Thawing.LabPerseonID);

                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Thawing.Impression);

                dbServer.AddInParameter(command, "ThawingDetails", DbType.Xml, ThawingDetailsresult);
                dbServer.AddInParameter(command, "MediaDetails", DbType.Xml, MediaDetailsresult);
                dbServer.AddInParameter(command, "FUSettingDetails", DbType.Xml, FUSettingDetailsresult);


                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                foreach (clsThawingDetailsVO item in BizActionObj.Thawing.ThawingDetails)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateThawingStatusForVitrification");

                    dbServer.AddInParameter(command1, "EmbNo", DbType.String, item.EmbNo);
                    dbServer.AddInParameter(command1, "Plan", DbType.Boolean, item.Plan);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                }

            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Thawing = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;


        }

        //Added By Saily P

        public override IValueObject AddUpdateSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateSpermThawingBizActionVO BizActionObj = valueObject as clsAddUpdateSpermThawingBizActionVO;
            try
            {
                //if (BizActionObj.IsNewForm == false)
                if (BizActionObj.IsNewForm == true)
                {
                    #region OLD Form Code
                    //foreach (var item in BizActionObj.Thawing)
                    foreach (var item in BizActionObj.ThawDeList)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateSpermThawing");
                        if (item.ID == 0)
                        {
                            dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                            dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        }
                        else
                        {
                            dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        }
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                        dbServer.AddInParameter(command, "PatientUnitId", DbType.Int64, BizActionObj.CoupleUintID);                    // dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.FreezingDate);
                        dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientId);
                        dbServer.AddInParameter(command, "LabPersonId", DbType.Int64, item.LabInchargeId);
                        dbServer.AddInParameter(command, "VitrificationID", DbType.String, item.VitrificationNo);
                        dbServer.AddInParameter(command, "ThawingDate", DbType.DateTime, item.ThawingDate.Value.Date);
                        dbServer.AddInParameter(command, "ThawingTime", DbType.DateTime, item.ThawingTime.Value.ToShortTimeString());
                        dbServer.AddInParameter(command, "Motility", DbType.String, item.Motility);
                        dbServer.AddInParameter(command, "SpermCount", DbType.String, item.TotalSpearmCount);
                        dbServer.AddInParameter(command, "Volume", DbType.String, item.Volume);
                        dbServer.AddInParameter(command, "Comments", DbType.String, item.Comments);
                        dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, item.IsFreezed);
                        dbServer.AddInParameter(command, "PostThawPlanId", DbType.Int64, item.PostThawPlanId);
                        dbServer.AddInParameter(command, "SpermFreezingDetailsID", DbType.Int64, item.SpremNo);
                        dbServer.AddInParameter(command, "SpermFreezingDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PlanTherapyID);
                        dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PlanTherapyUnitID);
                        dbServer.AddInParameter(command, "DonorID", DbType.Int64, BizActionObj.DonorID);
                        dbServer.AddInParameter(command, "DonorUnitID", DbType.Int64, BizActionObj.DonorUnitID);                  
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    }
                    #endregion
                }
                else
                {
                    foreach (var item in BizActionObj.ThawingList)
                    {
                        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_UpdateSpermThawing_NEW");
                        dbServer.AddInParameter(command5, "ID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command5, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command5, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command5, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command5, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command5, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command5, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                        dbServer.AddInParameter(command5, "CoupleUnitId", DbType.Int64, item.MainUnitID);
                        dbServer.AddInParameter(command5, "PatientId", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command5, "LabPersonId", DbType.Int64, item.LabPersonId);
                        dbServer.AddInParameter(command5, "ThawingDate", DbType.DateTime, item.ThawingDate.Value.Date);
                        dbServer.AddInParameter(command5, "ThawingTime", DbType.DateTime, item.ThawingTime.Value.ToShortTimeString());
                        dbServer.AddInParameter(command5, "Motility", DbType.String, item.Motility);
                        dbServer.AddInParameter(command5, "SpermCount", DbType.String, item.TotalSpearmCount);
                        dbServer.AddInParameter(command5, "Volume", DbType.String, item.Volume);
                        dbServer.AddInParameter(command5, "Comments", DbType.String, item.Comments);
                        dbServer.AddInParameter(command5, "IsFreezed", DbType.Boolean, item.IsFreezed);
                        //dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus = dbServer.ExecuteNonQuery(command5);
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionObj;
        }

        #endregion

        #region ET

        public override IValueObject getETDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetETDetailsBizActionVO BizAction = (valueObject) as clsGetETDetailsBizActionVO;
            BizAction.ET = new ETVO();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetETDetailsSavedInLabDays");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                dbServer.AddInParameter(command, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, BizAction.UnitID);
                dbServer.AddInParameter(command, "FromID", DbType.String, BizAction.FromID);



                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (BizAction.IsEdit == false)// If IsEdit False Then Details Comes From Lab Day 0 to Lab Day 6
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ETDetailsVO ETDetails = new ETDetailsVO();

                            ETDetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["LabNo"]));
                            ETDetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"]));
                            ETDetails.EmbNO = Convert.ToString(DALHelper.HandleDBNull(reader["OoNo"]));
                            //By Anjali.............
                            ETDetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                            //................
                            ETDetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                            ETDetails.FertilizationStage = Convert.ToString(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                            ETDetails.TransferDate = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                            ETDetails.Score = Convert.ToString(DALHelper.HandleDBNull(reader["Score"]));
                            BizAction.ET.ETDetails.Add(ETDetails);
                        }
                    }
                }
                else //In this Case Details Comes from Vitrifiction and Vitrification Detials
                {
                    if (BizAction.ID == 0 && BizAction.UnitID == 0)// All Vitrifictaion Against the Couple
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //clsGetVitrificationDetailsVO vitdetails = new clsGetVitrificationDetailsVO();
                                //clsThawingDetailsVO thawingDetails = new clsThawingDetailsVO();
                                //vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrivicationID"]));
                                //vitdetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                                //vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                                //vitdetails.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));
                                //thawingDetails.EmbNo = vitdetails.EmbNo;
                                //thawingDetails.VitrificationID = vitdetails.ID;
                                //vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                                //vitdetails.SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"]));
                                //vitdetails.SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"]));
                                //vitdetails.OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"]));
                                //vitdetails.SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"]));
                                //vitdetails.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                                //vitdetails.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                                //vitdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                                //vitdetails.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"]));
                                //vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                                //vitdetails.LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"]));
                                //vitdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                                //vitdetails.Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                                //vitdetails.ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                                //vitdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                                //System.Drawing.Color newCol = (System.Drawing.ColorTranslator.FromHtml(vitdetails.ColorCode));
                                //vitdetails.SelectesColor = System.Windows.Media.Color.FromArgb(newCol.A, newCol.R, newCol.G, newCol.B);
                                //BizAction.Vitrification.VitrificationDetails.Add(vitdetails);
                                //BizAction.Vitrification.ThawingDetails.Add(thawingDetails);
                            }
                        }
                    }
                    else// Sepecific Vitrification,Vitrification Details ,Media Details By Id Against the Couple
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                BizAction.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                BizAction.ET.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                                BizAction.ET.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                                BizAction.ET.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                                BizAction.ET.AnasthesistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                                BizAction.ET.AssAnasthesistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));

                                /* Added By Sudhir On 13/09/2013 */
                                BizAction.ET.EndometriumThickness = Convert.ToInt64(DALHelper.HandleDBNull(reader["EndometriumThickness"]));
                                BizAction.ET.ETPattern = Convert.ToInt64(DALHelper.HandleDBNull(reader["ETPattern"]));
                                BizAction.ET.ColorDopper = Convert.ToString(DALHelper.HandleDBNull(reader["ColorDopper"]));

                                //By Anjali
                                BizAction.ET.IsEndometerialPI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEndometerialPI"]));
                                BizAction.ET.IsEndometerialRI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEndometerialRI"]));
                                BizAction.ET.IsEndometerialSD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEndometerialSD"]));
                                BizAction.ET.IsUterinePI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUterinePI"]));
                                BizAction.ET.IsUterineRI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUterineRI"]));
                                BizAction.ET.IsUterineSD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUterineSD"]));

                                BizAction.ET.IsTreatmentUnderGA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTreatmentUnderGA"]));
                                BizAction.ET.CatheterTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CatheterTypeID"]));
                                BizAction.ET.Difficulty = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Difficulty"]));
                                BizAction.ET.DifficultyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyTypeID"]));
                                BizAction.ET.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                                BizAction.ET.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                                BizAction.ET.TenaculumUsed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TenaculumUsed"]));
                                BizAction.ET.DistanceFromFundus = Convert.ToString(DALHelper.HandleDBNull(reader["DistancefromFundus"]));

                                // BY BHUSHAN . . . . .
                                BizAction.ET.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                                BizAction.ET.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                                BizAction.ET.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                                BizAction.ET.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));

                            }
                        }

                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ETDetailsVO ETdetails = new ETDetailsVO();
                                ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                ETdetails.ETUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ETUnitID"]));
                                ETdetails.ETID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ETID"]));
                                ETdetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                                ETdetails.EmbNO = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNO"]));
                                //By Anjali.............
                                ETdetails.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));

                                //................

                                ETdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                                ETdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                                ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                                ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Grade"]));
                                ETdetails.FertilizationStage = Convert.ToString(DALHelper.HandleDBNull(reader["FertilizationStage"]));
                                ETdetails.FertilizationStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationStage"]));
                                ETdetails.Score = Convert.ToString(DALHelper.HandleDBNull(reader["Score"]));
                                ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                                ETdetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                                ETdetails.PlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"]));
                                ETdetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                                ETdetails.FileContents = Convert.FromBase64String(reader["Data"].ToString());

                                BizAction.ET.ETDetails.Add(ETdetails);
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FileUpload Filedetails = new FileUpload();
                                Filedetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                Filedetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                                Filedetails.Index = Convert.ToInt16(DALHelper.HandleDBNull(reader["FileIndex"]));
                                Filedetails.Data = Convert.FromBase64String(reader["Data"].ToString());
                                BizAction.ET.FUSetting.Add(Filedetails);
                            }
                        }
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


        public override IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //Calling Database Multiple times i Used Datatable and Convert it to Xml  
            clsAddUpdateETDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateETDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {

                con.Open();

                DbCommand command = dbServer.GetStoredProcCommand("IVF_AddUpdateETDetails");
                DataTable vitdt = new DataTable();

                #region ET Details

                vitdt = ListToDataTable(BizActionObj.ET.ETDetails);
                vitdt.TableName = "ETDetails";

                string ETDetailsresult;
                using (StringWriter sw = new StringWriter())
                {
                    vitdt.WriteXml(sw); ETDetailsresult = sw.ToString();
                }

                #endregion

                #region Upload File Details

                vitdt = ListToDataTable(BizActionObj.ET.FUSetting);
                vitdt.TableName = "FUSettingDetails";

                string FUSettingDetailsresult;
                using (StringWriter sw = new StringWriter())
                {
                    vitdt.WriteXml(sw); FUSettingDetailsresult = sw.ToString();
                }


                #endregion


                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.CoupleUintID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.ET.Date);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.ET.EmbryologistID);

                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, BizActionObj.ET.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, BizActionObj.ET.AnasthesistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, BizActionObj.ET.AssAnasthesistID);

                /* Added By Sudhir on 13/09/2013*/
                dbServer.AddInParameter(command, "EndometriumThickness", DbType.Int64, BizActionObj.ET.EndometriumThickness);
                dbServer.AddInParameter(command, "ETPattern", DbType.Int64, BizActionObj.ET.ETPattern);
                dbServer.AddInParameter(command, "ColorDopper", DbType.String, BizActionObj.ET.ColorDopper);

                dbServer.AddInParameter(command, "IsTreatmentUnderGA", DbType.Boolean, BizActionObj.ET.IsTreatmentUnderGA);
                dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, BizActionObj.ET.CatheterTypeID);
                dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, BizActionObj.ET.Difficulty);
                dbServer.AddInParameter(command, "DifficultyTypeID", DbType.Int64, BizActionObj.ET.DifficultyTypeID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ET.Status);

                dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, BizActionObj.ET.TenaculumUsed);
                dbServer.AddInParameter(command, "DistancefromFundus", DbType.String, BizActionObj.ET.DistanceFromFundus);
                //By Anjali
                dbServer.AddInParameter(command, "IsEndometerialPI", DbType.Boolean, BizActionObj.ET.IsEndometerialPI);
                dbServer.AddInParameter(command, "IsEndometerialRI", DbType.Boolean, BizActionObj.ET.IsEndometerialRI);
                dbServer.AddInParameter(command, "IsEndometerialSD", DbType.Boolean, BizActionObj.ET.IsEndometerialSD);
                dbServer.AddInParameter(command, "IsUterinePI", DbType.Boolean, BizActionObj.ET.IsUterinePI);
                dbServer.AddInParameter(command, "IsUterineRI", DbType.Boolean, BizActionObj.ET.IsUterineRI);
                dbServer.AddInParameter(command, "IsUterineSD", DbType.Boolean, BizActionObj.ET.IsUterineSD);

                dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, BizActionObj.ET.SrcSemenCode);
                dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, BizActionObj.ET.SrcOoctyCode);
                dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, BizActionObj.ET.SrcSemenID);
                dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, BizActionObj.ET.SrcOoctyID);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.ET.IsFreezed);
                dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, BizActionObj.ET.IsOnlyET);

                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.ET.Impression);
                dbServer.AddInParameter(command, "ETDetails", DbType.Xml, ETDetailsresult);
                dbServer.AddInParameter(command, "FUSettingDetails", DbType.Xml, FUSettingDetailsresult);

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
                BizActionObj.ET = null;
            }
            finally
            {
                con.Close();
            }
            return BizActionObj;
        }

        #endregion

        #region GET Height and Weight (Cerated by Priyanka)
        public override IValueObject GetCoupleHeightAndWeight(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizActionObj = (valueObject) as clsGetGetCoupleHeightAndWeightBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCoupleHeightAndWeight");
                dbServer.AddInParameter(command, "FemalePatientID", DbType.Int64, BizActionObj.FemalePatientID);
                dbServer.AddInParameter(command, "MalePatientID", DbType.Int64, BizActionObj.MalePatientID);
                dbServer.AddInParameter(command, "FemalePatientUnitID", DbType.Int64, BizActionObj.FemalePatientUnitID);
                dbServer.AddInParameter(command, "MalePatientUnitID", DbType.Int64, BizActionObj.MalePatientUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object DummyID = DALHelper.HandleDBNull(reader["DummyID"]);
                    }
                    reader.NextResult();

                    while (reader.Read())
                    {
                        BizActionObj.CoupleDetails.FemalePatient.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemaleHeight"]));
                        BizActionObj.CoupleDetails.FemalePatient.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemaleWeight"]));
                        BizActionObj.CoupleDetails.FemalePatient.BMI = (BizActionObj.CoupleDetails.FemalePatient.Weight / (BizActionObj.CoupleDetails.FemalePatient.Height * BizActionObj.CoupleDetails.FemalePatient.Height / 10000));
                        if (Double.IsNaN(BizActionObj.CoupleDetails.FemalePatient.BMI))
                        {
                            BizActionObj.CoupleDetails.FemalePatient.BMI = 0.00;
                        }
                        if (Double.IsInfinity(BizActionObj.CoupleDetails.FemalePatient.BMI))
                        {
                            BizActionObj.CoupleDetails.FemalePatient.BMI = 0.00;
                        }

                        //BizActionObj.CoupleDetails.FemalePatient.BMI = (double)DALHelper.HandleDBNull(reader["FemaleBMI"]);
                        //BizActionObj.CoupleDetails.FemalePatient.Alerts = (string)DALHelper.HandleDBNull(reader["FemaleAlerts"]);
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        BizActionObj.CoupleDetails.MalePatient.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleHeight"]));
                        BizActionObj.CoupleDetails.MalePatient.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleWeight"]));
                        BizActionObj.CoupleDetails.MalePatient.BMI = (BizActionObj.CoupleDetails.MalePatient.Weight / (BizActionObj.CoupleDetails.MalePatient.Height * BizActionObj.CoupleDetails.MalePatient.Height / 10000));
                        if (Double.IsNaN(BizActionObj.CoupleDetails.MalePatient.BMI))
                        {
                            BizActionObj.CoupleDetails.MalePatient.BMI = 0.00;
                        }
                        if (Double.IsInfinity(BizActionObj.CoupleDetails.MalePatient.BMI))
                        {
                            BizActionObj.CoupleDetails.MalePatient.BMI = 0.00;
                        }
                        //BizActionObj.CoupleDetails.MalePatient.BMI = (double)DALHelper.HandleDBNull(reader["MaleBMI"]);
                        //BizActionObj.CoupleDetails.MalePatient.Alerts = (string)DALHelper.HandleDBNull(reader["MaleAlerts"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        #endregion

        #region ET and OPU Dashboard (Created by Saily P)
        public override IValueObject GetTherapyDashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTherapyDetailsForDashBoardBizActionVO BizActionObj = valueObject as clsGetTherapyDetailsForDashBoardBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDashBoardAlerts_temp1");

                DbDataReader reader;
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.Int32, BizActionObj.SortExpression);
                // dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "FromDate2", DbType.DateTime, BizActionObj.FromDate2);
                dbServer.AddInParameter(command, "ToDate2", DbType.DateTime, BizActionObj.ToDate2);
                dbServer.AddInParameter(command, "FirstName", DbType.String, BizActionObj.FirstName);
                dbServer.AddInParameter(command, "LastName", DbType.String, BizActionObj.LastName);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.TherapyDetailsList == null)
                        BizActionObj.TherapyDetailsList = new List<clsTherapyDashBoardVO>();
                    while (reader.Read())
                    {
                        clsTherapyDashBoardVO objVO = new clsTherapyDashBoardVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.TherapyDate = (DateTime)DALHelper.HandleDate(reader["DateTime"]);
                        objVO.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                        objVO.EventTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["EventTypeId"]));
                        //objVO.TherapyTime = (DateTime)DALHelper.HandleDate(reader[""]);
                        objVO.MrNo = (string)DALHelper.HandleDBNull(reader["MRNo"]);
                        objVO.FirstName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["FirstName"]));
                        objVO.MiddleName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["MiddleName"]));
                        objVO.LastName = Security.base64Decode((string)DALHelper.HandleDBNull(reader["LastName"]));
                        objVO.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);
                        objVO.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleRgistrationNumber"]));
                        if (objVO.EventTypeId == 5)
                            objVO.Procedure = "Embryo Transfer";// Convert.ToString(TherapyGroup.EmbryoTransfer);
                        else if (objVO.EventTypeId == 4)
                            objVO.Procedure = "Ovum Pick Up";// Convert.ToString(TherapyGroup.OvumPickUp);
                        //objVO.PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader[""]));
                        //objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader[""]));

                        BizActionObj.TherapyDetailsList.Add(objVO);
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
        #endregion

    }
}
