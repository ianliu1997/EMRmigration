namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    internal class clsIVFDashboard_SemenDAL : clsBaseIVFDashboard_SemenDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFDashboard_SemenDAL()
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

        public override IValueObject AddUpdateSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddSemenBizActionVO nvo = valueObject as cls_IVFDashboard_AddSemenBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientSemenExamination");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.SemensExaminationDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.SemensExaminationDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.SemensExaminationDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.SemensExaminationDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "CollectionDate", DbType.DateTime, nvo.SemensExaminationDetails.CollectionDate);
                this.dbServer.AddInParameter(storedProcCommand, "TimeRecSampLab", DbType.DateTime, nvo.SemensExaminationDetails.TimeRecSampLab);
                this.dbServer.AddInParameter(storedProcCommand, "MethodOfCollection", DbType.Int64, nvo.SemensExaminationDetails.MethodOfCollectionID);
                this.dbServer.AddInParameter(storedProcCommand, "Abstinence", DbType.Int64, nvo.SemensExaminationDetails.AbstinenceID);
                this.dbServer.AddInParameter(storedProcCommand, "Complete", DbType.Boolean, nvo.SemensExaminationDetails.Complete);
                this.dbServer.AddInParameter(storedProcCommand, "CollecteAtCentre", DbType.Boolean, nvo.SemensExaminationDetails.CollecteAtCentre);
                this.dbServer.AddInParameter(storedProcCommand, "Color", DbType.Int64, nvo.SemensExaminationDetails.ColorID);
                this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Single, nvo.SemensExaminationDetails.Quantity);
                this.dbServer.AddInParameter(storedProcCommand, "PH", DbType.Single, nvo.SemensExaminationDetails.PH);
                this.dbServer.AddInParameter(storedProcCommand, "LiquificationTime", DbType.String, nvo.SemensExaminationDetails.LiquificationTime);
                this.dbServer.AddInParameter(storedProcCommand, "Viscosity", DbType.Boolean, nvo.SemensExaminationDetails.Viscosity);
                this.dbServer.AddInParameter(storedProcCommand, "RangeViscosityID", DbType.Int64, nvo.SemensExaminationDetails.RangeViscosityID);
                this.dbServer.AddInParameter(storedProcCommand, "Odour", DbType.Boolean, nvo.SemensExaminationDetails.Odour);
                this.dbServer.AddInParameter(storedProcCommand, "SpermCount", DbType.Single, nvo.SemensExaminationDetails.SpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalSpermCount", DbType.Single, nvo.SemensExaminationDetails.TotalSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "Motility", DbType.Single, nvo.SemensExaminationDetails.Motility);
                this.dbServer.AddInParameter(storedProcCommand, "NonMotility", DbType.Single, nvo.SemensExaminationDetails.NonMotility);
                this.dbServer.AddInParameter(storedProcCommand, "TotalMotility", DbType.Single, nvo.SemensExaminationDetails.TotalMotility);
                this.dbServer.AddInParameter(storedProcCommand, "MotilityGradeI", DbType.Single, nvo.SemensExaminationDetails.MotilityGradeI);
                this.dbServer.AddInParameter(storedProcCommand, "MotilityGradeII", DbType.Single, nvo.SemensExaminationDetails.MotilityGradeII);
                this.dbServer.AddInParameter(storedProcCommand, "MotilityGradeIII", DbType.Single, nvo.SemensExaminationDetails.MotilityGradeIII);
                this.dbServer.AddInParameter(storedProcCommand, "MotilityGradeIV", DbType.Single, nvo.SemensExaminationDetails.MotilityGradeIV);
                this.dbServer.AddInParameter(storedProcCommand, "Amorphus", DbType.Single, nvo.SemensExaminationDetails.Amorphus);
                this.dbServer.AddInParameter(storedProcCommand, "NeckAppendages", DbType.Single, nvo.SemensExaminationDetails.NeckAppendages);
                this.dbServer.AddInParameter(storedProcCommand, "Pyriform", DbType.Single, nvo.SemensExaminationDetails.Pyriform);
                this.dbServer.AddInParameter(storedProcCommand, "Macrocefalic", DbType.Single, nvo.SemensExaminationDetails.Macrocefalic);
                this.dbServer.AddInParameter(storedProcCommand, "Microcefalic", DbType.Single, nvo.SemensExaminationDetails.Microcefalic);
                this.dbServer.AddInParameter(storedProcCommand, "BrockenNeck", DbType.Single, nvo.SemensExaminationDetails.BrockenNeck);
                this.dbServer.AddInParameter(storedProcCommand, "RoundHead", DbType.Single, nvo.SemensExaminationDetails.RoundHead);
                this.dbServer.AddInParameter(storedProcCommand, "DoubleHead", DbType.Single, nvo.SemensExaminationDetails.DoubleHead);
                this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Single, nvo.SemensExaminationDetails.Total);
                this.dbServer.AddInParameter(storedProcCommand, "MorphologicalAbnormilities", DbType.Single, nvo.SemensExaminationDetails.MorphologicalAbnormilities);
                this.dbServer.AddInParameter(storedProcCommand, "NormalMorphology", DbType.Single, nvo.SemensExaminationDetails.NormalMorphology);
                this.dbServer.AddInParameter(storedProcCommand, "Comment", DbType.String, nvo.SemensExaminationDetails.Comment);
                this.dbServer.AddInParameter(storedProcCommand, "CytoplasmicDroplet", DbType.Single, nvo.SemensExaminationDetails.CytoplasmicDroplet);
                this.dbServer.AddInParameter(storedProcCommand, "Others", DbType.Single, nvo.SemensExaminationDetails.Others);
                this.dbServer.AddInParameter(storedProcCommand, "MidPieceTotal", DbType.Single, nvo.SemensExaminationDetails.MidPieceTotal);
                this.dbServer.AddInParameter(storedProcCommand, "CoiledTail", DbType.Single, nvo.SemensExaminationDetails.CoiledTail);
                this.dbServer.AddInParameter(storedProcCommand, "ShortTail", DbType.Single, nvo.SemensExaminationDetails.ShortTail);
                this.dbServer.AddInParameter(storedProcCommand, "HairpinTail", DbType.Single, nvo.SemensExaminationDetails.HairpinTail);
                this.dbServer.AddInParameter(storedProcCommand, "DoubleTail", DbType.Single, nvo.SemensExaminationDetails.DoubleTail);
                this.dbServer.AddInParameter(storedProcCommand, "TailOthers", DbType.Single, nvo.SemensExaminationDetails.TailOthers);
                this.dbServer.AddInParameter(storedProcCommand, "TailTotal", DbType.Single, nvo.SemensExaminationDetails.TailTotal);
                this.dbServer.AddInParameter(storedProcCommand, "HeadToHead", DbType.String, nvo.SemensExaminationDetails.HeadToHead);
                this.dbServer.AddInParameter(storedProcCommand, "TailToTail", DbType.String, nvo.SemensExaminationDetails.TailToTail);
                this.dbServer.AddInParameter(storedProcCommand, "HeadToTail", DbType.String, nvo.SemensExaminationDetails.HeadToTail);
                this.dbServer.AddInParameter(storedProcCommand, "SpermToOther", DbType.String, nvo.SemensExaminationDetails.SpermToOther);
                this.dbServer.AddInParameter(storedProcCommand, "PusCells", DbType.String, nvo.SemensExaminationDetails.PusCells);
                this.dbServer.AddInParameter(storedProcCommand, "RoundCells", DbType.String, nvo.SemensExaminationDetails.RoundCells);
                this.dbServer.AddInParameter(storedProcCommand, "EpithelialCells", DbType.String, nvo.SemensExaminationDetails.EpithelialCells);
                this.dbServer.AddInParameter(storedProcCommand, "Infections", DbType.String, nvo.SemensExaminationDetails.Infections);
                this.dbServer.AddInParameter(storedProcCommand, "OtherFindings", DbType.String, nvo.SemensExaminationDetails.OtherFindings);
                this.dbServer.AddInParameter(storedProcCommand, "InterpretationsID", DbType.Int64, nvo.SemensExaminationDetails.InterpretationsID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.SemensExaminationDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, nvo.SemensExaminationDetails.CreatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, nvo.SemensExaminationDetails.UpdatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.SemensExaminationDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.SemensExaminationDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.SemensExaminationDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, nvo.SemensExaminationDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.SemensExaminationDetails.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, nvo.SemensExaminationDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.SemensExaminationDetails.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, nvo.SemensExaminationDetails.UpdatedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, nvo.SemensExaminationDetails.Synchronized);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.SemensExaminationDetails.EmbryologistID);
                this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SemensExaminationDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Comments", DbType.String, nvo.SemensExaminationDetails.AllComments);
                this.dbServer.AddInParameter(storedProcCommand, "Sperm5thPercentile", DbType.Single, nvo.SemensExaminationDetails.Sperm5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "Sperm75thPercentile", DbType.Single, nvo.SemensExaminationDetails.Sperm75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "Ejaculate5thPercentile", DbType.Single, nvo.SemensExaminationDetails.Ejaculate5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "Ejaculate75thPercentile", DbType.Single, nvo.SemensExaminationDetails.Ejaculate75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "TotalMotility5thPercentile", DbType.Single, nvo.SemensExaminationDetails.TotalMotility5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "TotalMotility75thPercentile", DbType.Single, nvo.SemensExaminationDetails.TotalMotility75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "RapidProgressive", DbType.Single, nvo.SemensExaminationDetails.RapidProgressive);
                this.dbServer.AddInParameter(storedProcCommand, "SlowProgressive", DbType.Single, nvo.SemensExaminationDetails.SlowProgressive);
                this.dbServer.AddInParameter(storedProcCommand, "SpermMorphology5thPercentile", DbType.Single, nvo.SemensExaminationDetails.SpermMorphology5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "SpermMorphology75thPercentile", DbType.Single, nvo.SemensExaminationDetails.SpermMorphology75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "NormalFormsComments", DbType.String, nvo.SemensExaminationDetails.NormalFormsComments);
                this.dbServer.AddInParameter(storedProcCommand, "OverAllDefectsComments", DbType.String, nvo.SemensExaminationDetails.OverAllDefectsComments);
                this.dbServer.AddInParameter(storedProcCommand, "HeadDefectsComments", DbType.String, nvo.SemensExaminationDetails.HeadDefectsComments);
                this.dbServer.AddInParameter(storedProcCommand, "MidPieceNeckDefectsComments", DbType.String, nvo.SemensExaminationDetails.MidPieceNeckDefectsComments);
                this.dbServer.AddInParameter(storedProcCommand, "TailDefectsComments", DbType.String, nvo.SemensExaminationDetails.TailDefectsComments);
                this.dbServer.AddInParameter(storedProcCommand, "ExcessiveResidualComments", DbType.String, nvo.SemensExaminationDetails.ExcessiveResidualComments);
                this.dbServer.AddInParameter(storedProcCommand, "SpermMorphologySubNormal", DbType.String, nvo.SemensExaminationDetails.SpermMorphologySubNormal);
                this.dbServer.AddInParameter(storedProcCommand, "Spillage", DbType.String, nvo.SemensExaminationDetails.Spillage);
                this.dbServer.AddInParameter(storedProcCommand, "Fructose", DbType.String, nvo.SemensExaminationDetails.Fructose);
                this.dbServer.AddInParameter(storedProcCommand, "Live", DbType.Single, nvo.SemensExaminationDetails.Live);
                this.dbServer.AddInParameter(storedProcCommand, "Dead", DbType.Single, nvo.SemensExaminationDetails.Dead);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAdvanceMotility", DbType.Single, nvo.SemensExaminationDetails.TotalAdvanceMotility);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAdvance5thPercentile", DbType.Single, nvo.SemensExaminationDetails.TotalAdvance5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAdvance75thPercentile", DbType.Single, nvo.SemensExaminationDetails.TotalAdvance75thPercentile);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.SemensExaminationDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.SemensExaminationDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdateSemenUsedBizActionVO nvo = valueObject as cls_IVFDashboard_AddUpdateSemenUsedBizActionVO;
            cls_IVFDashboard_SemenWashVO hvo = new cls_IVFDashboard_SemenWashVO();
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                hvo = nvo.ListSemenUsed[0];
                foreach (cls_IVFDashboard_SemenWashVO hvo2 in nvo.ListSemenUsed)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSemenUsedDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, hvo2.SemenUsedDetailsID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, hvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "TypeOfSpreamID", DbType.Int64, hvo2.SpermTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "SemenUsed", DbType.Int64, hvo2.IsUsed);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, hvo.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "SemenWashID", DbType.Int64, hvo2.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, hvo.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, hvo2.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, hvo2.CreatedUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, hvo2.AddedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, hvo2.AddedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, hvo2.AddedWindowsLoginName);
                    this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, hvo2.Synchronized);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                nvo.SemensWashDetails = null;
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateSemenWashDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdateSemenWashBizActionVO nvo = valueObject as cls_IVFDashboard_AddUpdateSemenWashBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSemenWash");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.SemensWashDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.SemensWashDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitID", DbType.Int64, nvo.SemensWashDetails.VisitID);
                this.dbServer.AddInParameter(storedProcCommand, "SpermTypeID", DbType.Int64, nvo.SemensWashDetails.SpermTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleCode", DbType.String, nvo.SemensWashDetails.SampleCode);
                this.dbServer.AddInParameter(storedProcCommand, "SampleLinkID", DbType.String, nvo.SemensWashDetails.SampleLinkID);
                this.dbServer.AddInParameter(storedProcCommand, "CollectionDate", DbType.DateTime, nvo.SemensWashDetails.CollectionDate);
                this.dbServer.AddInParameter(storedProcCommand, "MethodOfCollection", DbType.Int64, nvo.SemensWashDetails.MethodOfCollectionID);
                this.dbServer.AddInParameter(storedProcCommand, "Abstinence", DbType.Int64, nvo.SemensWashDetails.AbstinenceID);
                this.dbServer.AddInParameter(storedProcCommand, "IUIDate", DbType.DateTime, nvo.SemensWashDetails.IUIDate);
                this.dbServer.AddInParameter(storedProcCommand, "InSeminatedByID", DbType.Int64, nvo.SemensWashDetails.InSeminatedByID);
                this.dbServer.AddInParameter(storedProcCommand, "InSeminationLocationID", DbType.Int64, nvo.SemensWashDetails.InSeminationLocationID);
                this.dbServer.AddInParameter(storedProcCommand, "WitnessByID", DbType.Int64, nvo.SemensWashDetails.WitnessByID);
                this.dbServer.AddInParameter(storedProcCommand, "InSeminationMethodID", DbType.Int64, nvo.SemensWashDetails.InSeminationMethodID);
                this.dbServer.AddInParameter(storedProcCommand, "ThawDate", DbType.DateTime, nvo.SemensWashDetails.ThawDate);
                this.dbServer.AddInParameter(storedProcCommand, "SampleID", DbType.String, nvo.SemensWashDetails.SampleID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorID", DbType.Int64, nvo.SemensWashDetails.DonorID);
                this.dbServer.AddInParameter(storedProcCommand, "DonorUnitID", DbType.Int64, nvo.SemensWashDetails.DonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ISFromIUI", DbType.Boolean, nvo.SemensWashDetails.ISFromIUI);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.SemensWashDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.SemensWashDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, nvo.SemensWashDetails.BatchID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchUnitID", DbType.Int64, nvo.SemensWashDetails.BatchUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, nvo.SemensWashDetails.BatchCode);
                this.dbServer.AddInParameter(storedProcCommand, "CollecteAtCentre", DbType.Boolean, nvo.SemensWashDetails.CollecteAtCentre);
                this.dbServer.AddInParameter(storedProcCommand, "IsFrozenSample", DbType.Boolean, nvo.SemensWashDetails.IsFrozenSample);
                this.dbServer.AddInParameter(storedProcCommand, "Color", DbType.Int64, nvo.SemensWashDetails.ColorID);
                this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Single, nvo.SemensWashDetails.Quantity);
                this.dbServer.AddInParameter(storedProcCommand, "PH", DbType.Single, nvo.SemensWashDetails.PH);
                this.dbServer.AddInParameter(storedProcCommand, "LiquificationTime", DbType.String, nvo.SemensWashDetails.LiquificationTime);
                this.dbServer.AddInParameter(storedProcCommand, "Viscosity", DbType.Boolean, nvo.SemensWashDetails.Viscosity);
                this.dbServer.AddInParameter(storedProcCommand, "RangeViscosityID", DbType.Int64, nvo.SemensWashDetails.RangeViscosityID);
                this.dbServer.AddInParameter(storedProcCommand, "Odour", DbType.Boolean, nvo.SemensWashDetails.Odour);
                this.dbServer.AddInParameter(storedProcCommand, "PreSpermCount", DbType.Single, nvo.SemensWashDetails.PreSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "PreTotalSpermCount", DbType.Single, nvo.SemensWashDetails.PreTotalSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "PreMotility", DbType.Single, nvo.SemensWashDetails.PreMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PreNonMotility", DbType.Single, nvo.SemensWashDetails.PreNonMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PreTotalMotility", DbType.Single, nvo.SemensWashDetails.PreTotalMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PreMotilityGradeI", DbType.Single, nvo.SemensWashDetails.PreMotilityGradeI);
                this.dbServer.AddInParameter(storedProcCommand, "PreMotilityGradeII", DbType.Single, nvo.SemensWashDetails.PreMotilityGradeII);
                this.dbServer.AddInParameter(storedProcCommand, "PreMotilityGradeIII", DbType.Single, nvo.SemensWashDetails.PreMotilityGradeIII);
                this.dbServer.AddInParameter(storedProcCommand, "PreMotilityGradeIV", DbType.Single, nvo.SemensWashDetails.PreMotilityGradeIV);
                this.dbServer.AddInParameter(storedProcCommand, "PreNormalMorphology", DbType.Single, nvo.SemensWashDetails.PreNormalMorphology);
                this.dbServer.AddInParameter(storedProcCommand, "PostSpermCount", DbType.Single, nvo.SemensWashDetails.PostSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "PostTotalSpermCount", DbType.Single, nvo.SemensWashDetails.PostTotalSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "PostMotility", DbType.Single, nvo.SemensWashDetails.PostMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PostNonMotility", DbType.Single, nvo.SemensWashDetails.PostNonMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PostTotalMotility", DbType.Single, nvo.SemensWashDetails.PostTotalMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PostMotilityGradeI", DbType.Single, nvo.SemensWashDetails.PostMotilityGradeI);
                this.dbServer.AddInParameter(storedProcCommand, "PostMotilityGradeII", DbType.Single, nvo.SemensWashDetails.PostMotilityGradeII);
                this.dbServer.AddInParameter(storedProcCommand, "PostMotilityGradeIII", DbType.Single, nvo.SemensWashDetails.PostMotilityGradeIII);
                this.dbServer.AddInParameter(storedProcCommand, "PostMotilityGradeIV", DbType.Single, nvo.SemensWashDetails.PostMotilityGradeIV);
                this.dbServer.AddInParameter(storedProcCommand, "PostNormalMorphology", DbType.Single, nvo.SemensWashDetails.PostNormalMorphology);
                this.dbServer.AddInParameter(storedProcCommand, "PusCells", DbType.String, nvo.SemensWashDetails.PusCells);
                this.dbServer.AddInParameter(storedProcCommand, "RoundCells", DbType.String, nvo.SemensWashDetails.RoundCells);
                this.dbServer.AddInParameter(storedProcCommand, "EpithelialCells", DbType.String, nvo.SemensWashDetails.EpithelialCells);
                this.dbServer.AddInParameter(storedProcCommand, "CheckedByDoctorID", DbType.Int64, nvo.SemensWashDetails.CheckedByDoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherCells", DbType.String, nvo.SemensWashDetails.AnyOtherCells);
                this.dbServer.AddInParameter(storedProcCommand, "Comment", DbType.String, nvo.SemensWashDetails.Comment);
                this.dbServer.AddInParameter(storedProcCommand, "CommentID", DbType.String, nvo.SemensWashDetails.CommentID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.SemensWashDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Synchronized", DbType.Boolean, nvo.SemensWashDetails.Synchronized);
                this.dbServer.AddInParameter(storedProcCommand, "PreAmount", DbType.Single, nvo.SemensWashDetails.PreAmount);
                this.dbServer.AddInParameter(storedProcCommand, "PostAmount", DbType.Single, nvo.SemensWashDetails.PostAmount);
                this.dbServer.AddInParameter(storedProcCommand, "PreProgMotility", DbType.Single, nvo.SemensWashDetails.PreProgMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PostProgMotility", DbType.Single, nvo.SemensWashDetails.PostProgMotility);
                this.dbServer.AddInParameter(storedProcCommand, "PreNonProgressive", DbType.Single, nvo.SemensWashDetails.PreNonProgressive);
                this.dbServer.AddInParameter(storedProcCommand, "PostNonProgressive", DbType.Single, nvo.SemensWashDetails.PostNonProgressive);
                this.dbServer.AddInParameter(storedProcCommand, "PreNonMotile", DbType.Single, nvo.SemensWashDetails.PreNonMotile);
                this.dbServer.AddInParameter(storedProcCommand, "PostNonMotile", DbType.Single, nvo.SemensWashDetails.PostNonMotile);
                this.dbServer.AddInParameter(storedProcCommand, "PreMotileSpermCount", DbType.Single, nvo.SemensWashDetails.PreMotileSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "PostMotileSpermCount", DbType.Single, nvo.SemensWashDetails.PostMotileSpermCount);
                this.dbServer.AddInParameter(storedProcCommand, "PreNormalForms", DbType.Single, nvo.SemensWashDetails.PreNormalForms);
                this.dbServer.AddInParameter(storedProcCommand, "PostNormalForms", DbType.Single, nvo.SemensWashDetails.PostNormalForms);
                this.dbServer.AddInParameter(storedProcCommand, "PreperationDate", DbType.DateTime, nvo.SemensWashDetails.PreperationDate);
                this.dbServer.AddInParameter(storedProcCommand, "InseminatedAmount", DbType.String, nvo.SemensWashDetails.Inseminated);
                this.dbServer.AddInParameter(storedProcCommand, "MotileSperm", DbType.String, nvo.SemensWashDetails.MotileSperm);
                this.dbServer.AddInParameter(storedProcCommand, "Spillage", DbType.String, nvo.SemensWashDetails.Spillage);
                this.dbServer.AddInParameter(storedProcCommand, "MediaUsed", DbType.String, nvo.SemensWashDetails.MediaUsed);
                this.dbServer.AddInParameter(storedProcCommand, "SemenProcessingMethodID", DbType.Int64, nvo.SemensWashDetails.SemenProcessingMethodID);
                this.dbServer.AddInParameter(storedProcCommand, "HIV", DbType.Single, nvo.SemensWashDetails.HIV);
                this.dbServer.AddInParameter(storedProcCommand, "VDRL", DbType.Single, nvo.SemensWashDetails.VDRL);
                this.dbServer.AddInParameter(storedProcCommand, "HBSAG", DbType.Single, nvo.SemensWashDetails.HBSAG);
                this.dbServer.AddInParameter(storedProcCommand, "HCV", DbType.Single, nvo.SemensWashDetails.HCV);
                this.dbServer.AddInParameter(storedProcCommand, "TransacationTypeID", DbType.Single, nvo.SemensWashDetails.TransacationTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "BloodGroupID", DbType.Single, nvo.SemensWashDetails.BloodGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "SpermRecoveredFrom", DbType.String, nvo.SemensWashDetails.SpermRecoveredFrom);
                this.dbServer.AddInParameter(storedProcCommand, "Comments", DbType.String, nvo.SemensWashDetails.AllComments);
                this.dbServer.AddInParameter(storedProcCommand, "Sperm5thPercentile", DbType.Single, nvo.SemensWashDetails.Sperm5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "Sperm75thPercentile", DbType.Single, nvo.SemensWashDetails.Sperm75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "Ejaculate5thPercentile", DbType.Single, nvo.SemensWashDetails.Ejaculate5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "Ejaculate75thPercentile", DbType.Single, nvo.SemensWashDetails.Ejaculate75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "TotalMotility5thPercentile", DbType.Single, nvo.SemensWashDetails.TotalMotility5thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "TotalMotility75thPercentile", DbType.Single, nvo.SemensWashDetails.TotalMotility75thPercentile);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.SemensWashDetails.IsFreezed);
                this.dbServer.AddParameter(storedProcCommand, "Err", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.SemensWashDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                nvo.SuccessStatus = -1;
                nvo.SemensWashDetails = null;
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetNewIUIDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetIVFDashboard_NewIUIDetailsBizActionVO nvo = valueObject as cls_GetIVFDashboard_NewIUIDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDashBoard_GetNewIUIDetails_New_1");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.SemensExaminationDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.SemensExaminationDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.SemensExaminationDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.SemensExaminationDetails.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.SemensExaminationDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        nvo.SemensExaminationDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Unitid"]));
                        nvo.SemensExaminationDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.SemensExaminationDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.SemensExaminationDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.SemensExaminationDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.SemensExaminationDetails.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        nvo.SemensExaminationDetails.DonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorUnitID"]));
                        nvo.SemensExaminationDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        nvo.SemensExaminationDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        nvo.SemensExaminationDetails.BloodGroup = Convert.ToString(DALHelper.HandleDBNull(reader["BloodGroup"]));
                        nvo.SemensExaminationDetails.SampleID = Convert.ToString(DALHelper.HandleDBNull(reader["SampleID"]));
                        nvo.SemensExaminationDetails.Hair = Convert.ToString(DALHelper.HandleDBNull(reader["Hair"]));
                        nvo.SemensExaminationDetails.Skin = Convert.ToString(DALHelper.HandleDBNull(reader["Skin"]));
                        nvo.SemensExaminationDetails.Eye = Convert.ToString(DALHelper.HandleDBNull(reader["Eye"]));
                        nvo.SemensExaminationDetails.BoneStructure = Convert.ToString(DALHelper.HandleDBNull(reader["BoneStructure"]));
                        nvo.SemensExaminationDetails.Height = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        nvo.SemensExaminationDetails.Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"]));
                        nvo.SemensExaminationDetails.ThawDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawDate"])));
                        nvo.SemensExaminationDetails.IUIDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["IUIDate"])));
                        nvo.SemensExaminationDetails.InSeminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminatedByID"]));
                        nvo.SemensExaminationDetails.WitnessByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessByID"]));
                        nvo.SemensExaminationDetails.InSeminationLocationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminationLocationID"]));
                        nvo.SemensExaminationDetails.InSeminationMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminationMethodID"]));
                        nvo.SemensExaminationDetails.CollectionDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["CollectionDate"])));
                        nvo.SemensExaminationDetails.MethodOfCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MethodOfCollection"]));
                        nvo.SemensExaminationDetails.TimeRecSampLab = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["TimeRecSampLab"])));
                        nvo.SemensExaminationDetails.AbstinenceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Abstinence"]));
                        nvo.SemensExaminationDetails.CollecteAtCentre = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CollecteAtCentre"]));
                        nvo.SemensExaminationDetails.ColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Color"]));
                        nvo.SemensExaminationDetails.Quantity = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        nvo.SemensExaminationDetails.PH = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PH"]));
                        nvo.SemensExaminationDetails.LiquificationTime = Convert.ToString(DALHelper.HandleDBNull(reader["LiquificationTime"]));
                        nvo.SemensExaminationDetails.Viscosity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Viscosity"]));
                        nvo.SemensExaminationDetails.Odour = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Odour"]));
                        nvo.SemensExaminationDetails.IsFrozenSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenSample"]));
                        nvo.SemensExaminationDetails.PostMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotility"]));
                        nvo.SemensExaminationDetails.PostMotilityGradeI = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeI"]));
                        nvo.SemensExaminationDetails.PostMotilityGradeII = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeII"]));
                        nvo.SemensExaminationDetails.PostMotilityGradeIII = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeIII"]));
                        nvo.SemensExaminationDetails.PostMotilityGradeIV = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotilityGradeIV"]));
                        nvo.SemensExaminationDetails.PostNonMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonMotility"]));
                        nvo.SemensExaminationDetails.PostNormalMorphology = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNormalMorphology"]));
                        nvo.SemensExaminationDetails.PostSpermCount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostSpermCount"]));
                        nvo.SemensExaminationDetails.PostTotalMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostTotalMotility"]));
                        nvo.SemensExaminationDetails.PostTotalSpermCount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostTotalSpermCount"]));
                        nvo.SemensExaminationDetails.PreMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotility"]));
                        nvo.SemensExaminationDetails.PreMotilityGradeI = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeI"]));
                        nvo.SemensExaminationDetails.PreMotilityGradeII = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeII"]));
                        nvo.SemensExaminationDetails.PreMotilityGradeIII = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeIII"]));
                        nvo.SemensExaminationDetails.PreMotilityGradeIV = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotilityGradeIV"]));
                        nvo.SemensExaminationDetails.PreNonMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonMotility"]));
                        nvo.SemensExaminationDetails.PreNormalMorphology = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNormalMorphology"]));
                        nvo.SemensExaminationDetails.PreSpermCount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreSpermCount"]));
                        nvo.SemensExaminationDetails.PreTotalMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreTotalMotility"]));
                        nvo.SemensExaminationDetails.ThawDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ThawDate"])));
                        nvo.SemensExaminationDetails.TimeRecSampLab = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["TimeRecSampLab"])));
                        nvo.SemensExaminationDetails.PreTotalSpermCount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreTotalSpermCount"]));
                        nvo.SemensExaminationDetails.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                        nvo.SemensExaminationDetails.PusCells = Convert.ToString(DALHelper.HandleDBNull(reader["PusCells"]));
                        nvo.SemensExaminationDetails.RoundCells = Convert.ToString(DALHelper.HandleDBNull(reader["RoundCells"]));
                        nvo.SemensExaminationDetails.EpithelialCells = Convert.ToString(DALHelper.HandleDBNull(reader["EpithelialCells"]));
                        nvo.SemensExaminationDetails.AnyOtherCells = Convert.ToString(DALHelper.HandleDBNull(reader["OtherCells"]));
                        nvo.SemensExaminationDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        nvo.SemensExaminationDetails.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        nvo.SemensExaminationDetails.BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"]));
                        nvo.SemensExaminationDetails.PostAmount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostAmount"]));
                        nvo.SemensExaminationDetails.PostProgMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostProgMotility"]));
                        nvo.SemensExaminationDetails.PostNormalForms = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNormalForms"]));
                        nvo.SemensExaminationDetails.PreAmount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreAmount"]));
                        nvo.SemensExaminationDetails.PreProgMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreProgMotility"]));
                        nvo.SemensExaminationDetails.PreNormalForms = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNormalForms"]));
                        nvo.SemensExaminationDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                        nvo.SemensExaminationDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                        nvo.SemensExaminationDetails.Inseminated = Convert.ToString(DALHelper.HandleDBNull(reader["InseminatedAmount"]));
                        nvo.SemensExaminationDetails.MotileSperm = Convert.ToString(DALHelper.HandleDBNull(reader["MotileSperm"]));
                        nvo.SemensExaminationDetails.PreperationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["PreperationDate"])));
                        nvo.SemensExaminationDetails.ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"]));
                        nvo.SemensExaminationDetails.PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"]));
                        nvo.SemensExaminationDetails.Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"]));
                        nvo.SemensExaminationDetails.CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"]));
                        nvo.SemensExaminationDetails.MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"]));
                        nvo.SemensExaminationDetails.PreNonProgressive = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonProgressive"]));
                        nvo.SemensExaminationDetails.PostNonProgressive = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonProgressive"]));
                        nvo.SemensExaminationDetails.PreNonMotile = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonMotile"]));
                        nvo.SemensExaminationDetails.PostNonMotile = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonMotile"]));
                        nvo.SemensExaminationDetails.PreMotileSpermCount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreMotileSpermCount"]));
                        nvo.SemensExaminationDetails.PostMotileSpermCount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostMotileSpermCount"]));
                        nvo.SemensExaminationDetails.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCodeMrno"]));
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

        public override IValueObject GetSemenDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetIVFDashboard_SemenDetailsBizActionVO nvo = valueObject as cls_GetIVFDashboard_SemenDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSemenDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<cls_IVFDashboard_SemenWashVO>();
                    }
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenWashVO item = new cls_IVFDashboard_SemenWashVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            SpermTypeID = (long) DALHelper.HandleDBNull(reader["TypeOfSpreamID"]),
                            TypeOfSperm = (string) DALHelper.HandleDBNull(reader["TypeOfSperm"]),
                            CollectionDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["CollectionDate"])),
                            ISChecked = (bool) DALHelper.HandleBoolDBNull(reader["ISChecked"]),
                            UsedPlanTherapyID = (long) DALHelper.HandleDBNull(reader["UsedPlanTherapyID"]),
                            UsedPlanTherapyUnitID = (long) DALHelper.HandleDBNull(reader["UsedPlanTherapyUnitID"])
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

        public override IValueObject GetSemenExaminationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetIVFDashboard_SemenBizActionVO nvo = valueObject as cls_GetIVFDashboard_SemenBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSemenExaminationDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<cls_IVFDashboard_SemenVO>();
                    }
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenVO item = new cls_IVFDashboard_SemenVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            CollectionDate = DALHelper.HandleDate(reader["CollectionDate"]),
                            MethodOfCollection = (string) DALHelper.HandleDBNull(reader["MethodOfCollection"]),
                            MethodOfCollectionID = (long) DALHelper.HandleDBNull(reader["MethodOfCollectionID"]),
                            TimeRecSampLab = (DateTime?) DALHelper.HandleDBNull(reader["TimeRecSampLab"]),
                            AbstinenceID = (long) DALHelper.HandleDBNull(reader["Abstinence"]),
                            Complete = (bool) DALHelper.HandleDBNull(reader["Complete"]),
                            CollecteAtCentre = (bool) DALHelper.HandleDBNull(reader["CollecteAtCentre"]),
                            ColorID = (long) DALHelper.HandleDBNull(reader["Color"]),
                            Quantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                            PH = (float) ((double) DALHelper.HandleDBNull(reader["PH"])),
                            LiquificationTime = (string) DALHelper.HandleDBNull(reader["LiquificationTime"]),
                            Viscosity = (bool) DALHelper.HandleDBNull(reader["Viscosity"]),
                            Odour = (bool) DALHelper.HandleDBNull(reader["Odour"]),
                            SpermCount = (float) ((double) DALHelper.HandleDBNull(reader["SpermCount"])),
                            TotalSpermCount = (float) ((double) DALHelper.HandleDBNull(reader["TotalSpermCount"])),
                            Motility = (float) ((double) DALHelper.HandleDBNull(reader["Motility"])),
                            NonMotility = (float) ((double) DALHelper.HandleDBNull(reader["NonMotility"])),
                            TotalMotility = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility"])),
                            MotilityGradeI = (float) ((double) DALHelper.HandleDBNull(reader["MotilityGradeI"])),
                            MotilityGradeII = (float) ((double) DALHelper.HandleDBNull(reader["MotilityGradeII"])),
                            MotilityGradeIII = (float) ((double) DALHelper.HandleDBNull(reader["MotilityGradeIII"])),
                            MotilityGradeIV = (float) ((double) DALHelper.HandleDBNull(reader["MotilityGradeIV"])),
                            Amorphus = (float) ((double) DALHelper.HandleDBNull(reader["Amorphus"])),
                            NeckAppendages = (float) ((double) DALHelper.HandleDBNull(reader["NeckAppendages"])),
                            Pyriform = (float) ((double) DALHelper.HandleDBNull(reader["Pyriform"])),
                            Macrocefalic = (float) ((double) DALHelper.HandleDBNull(reader["Macrocefalic"])),
                            Microcefalic = (float) ((double) DALHelper.HandleDBNull(reader["Microcefalic"])),
                            BrockenNeck = (float) ((double) DALHelper.HandleDBNull(reader["BrockenNeck"])),
                            RoundHead = (float) ((double) DALHelper.HandleDBNull(reader["RoundHead"])),
                            DoubleHead = (float) ((double) DALHelper.HandleDBNull(reader["DoubleHead"])),
                            Total = (float) ((double) DALHelper.HandleDBNull(reader["Total"])),
                            MorphologicalAbnormilities = (float) ((double) DALHelper.HandleDBNull(reader["MorphologicalAbnormilities"])),
                            NormalMorphology = (float) ((double) DALHelper.HandleDBNull(reader["NormalMorphology"])),
                            Comment = (string) DALHelper.HandleDBNull(reader["Comment"]),
                            CytoplasmicDroplet = (float) ((double) DALHelper.HandleDBNull(reader["CytoplasmicDroplet"])),
                            Others = (float) ((double) DALHelper.HandleDBNull(reader["Others"])),
                            MidPieceTotal = (float) ((double) DALHelper.HandleDBNull(reader["MidPieceTotal"])),
                            CoiledTail = (float) ((double) DALHelper.HandleDBNull(reader["CoiledTail"])),
                            ShortTail = (float) ((double) DALHelper.HandleDBNull(reader["ShortTail"])),
                            HairpinTail = (float) ((double) DALHelper.HandleDBNull(reader["HairpinTail"])),
                            DoubleTail = (float) ((double) DALHelper.HandleDBNull(reader["DoubleTail"])),
                            TailOthers = (float) ((double) DALHelper.HandleDBNull(reader["TailOthers"])),
                            TailTotal = (float) ((double) DALHelper.HandleDBNull(reader["TailTotal"])),
                            HeadToHead = (string) DALHelper.HandleDBNull(reader["HeadToHead"]),
                            TailToTail = (string) DALHelper.HandleDBNull(reader["TailToTail"]),
                            HeadToTail = (string) DALHelper.HandleDBNull(reader["HeadToTail"]),
                            SpermToOther = (string) DALHelper.HandleDBNull(reader["SpermToOther"]),
                            PusCells = (string) DALHelper.HandleDBNull(reader["PusCells"]),
                            RoundCells = (string) DALHelper.HandleDBNull(reader["RoundCells"]),
                            EpithelialCells = (string) DALHelper.HandleDBNull(reader["EpithelialCells"]),
                            Infections = (string) DALHelper.HandleDBNull(reader["Infections"]),
                            OtherFindings = (string) DALHelper.HandleDBNull(reader["OtherFindings"]),
                            InterpretationsID = (long) DALHelper.HandleDBNull(reader["InterpretationsID"]),
                            RangeViscosityID = (long) DALHelper.HandleDBNull(reader["RangeViscosityID"]),
                            EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"])),
                            VolumeRange = "<1.5 - Low, 2.0 - Normal, >2.0 - High",
                            PHRange = "<7.0 - Acidic, >=7.2 - Normal",
                            PusCellsRange = "<04 - Not Significant",
                            MorphologyAbnormilityRange = "<4% - Abnormal forms",
                            NormalMorphologyRange = ">=4 Normal forms",
                            SpermConcentrationRange = ">15 mill/ml - Normal",
                            EjaculateRange = ">= 39 millions - Normal",
                            TotalMotilityRange = ">40% - Normal",
                            AllComments = (string) DALHelper.HandleDBNull(reader["Comments"]),
                            Sperm5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Sperm5thPercentile"])),
                            Sperm75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Sperm75thPercentile"])),
                            Ejaculate5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Ejaculate5thPercentile"])),
                            Ejaculate75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Ejaculate75thPercentile"])),
                            TotalMotility5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility5thPercentile"])),
                            TotalMotility75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility75thPercentile"])),
                            RapidProgressive = (float) ((double) DALHelper.HandleDBNull(reader["RapidProgressive"])),
                            SlowProgressive = (float) ((double) DALHelper.HandleDBNull(reader["SlowProgressive"])),
                            SpermMorphology5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["SpermMorphology5thPercentile"])),
                            SpermMorphology75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["SpermMorphology75thPercentile"])),
                            NormalFormsComments = (string) DALHelper.HandleDBNull(reader["NormalFormsComments"]),
                            OverAllDefectsComments = (string) DALHelper.HandleDBNull(reader["OverAllDefectsComments"]),
                            HeadDefectsComments = (string) DALHelper.HandleDBNull(reader["HeadDefectsComments"]),
                            MidPieceNeckDefectsComments = (string) DALHelper.HandleDBNull(reader["MidPieceNeckDefectsComments"]),
                            TailDefectsComments = (string) DALHelper.HandleDBNull(reader["TailDefectsComments"]),
                            ExcessiveResidualComments = (string) DALHelper.HandleDBNull(reader["ExcessiveResidualComments"]),
                            SpermMorphologySubNormal = (string) DALHelper.HandleDBNull(reader["SpermMorphologySubNormal"]),
                            Spillage = (string) DALHelper.HandleDBNull(reader["Spillage"]),
                            Fructose = (string) DALHelper.HandleDBNull(reader["Fructose"]),
                            Live = (float) ((double) DALHelper.HandleDBNull(reader["Live"])),
                            Dead = (float) ((double) DALHelper.HandleDBNull(reader["Dead"])),
                            TotalAdvanceMotility = (float) ((double) DALHelper.HandleDBNull(reader["TotalAdvanceMotility"])),
                            TotalAdvance5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalAdvance5thPercentile"])),
                            TotalAdvance75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalAdvance75thPercentile"]))
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

        public override IValueObject GetSemenUsedDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetIVFDashboard_SemenUsedBizActionVO nvo = valueObject as cls_GetIVFDashboard_SemenUsedBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSemenUsedDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ListSaved == null)
                    {
                        nvo.ListSaved = new List<cls_IVFDashboard_SemenWashVO>();
                    }
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenWashVO item = new cls_IVFDashboard_SemenWashVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            SemenUsedDetailsID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            TypeOfSperm = (string) DALHelper.HandleDBNull(reader["TypeOfSperm"]),
                            CollectionDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["CollectionDate"])),
                            IsUsed = (bool) DALHelper.HandleDBNull(reader["SemenUsed"])
                        };
                        nvo.ListSaved.Add(item);
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

        public override IValueObject GetSemenWashDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_GetIVFDashboard_SemenWashBizActionVO nvo = valueObject as cls_GetIVFDashboard_SemenWashBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSemenWashDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<cls_IVFDashboard_SemenWashVO>();
                    }
                    while (reader.Read())
                    {
                        cls_IVFDashboard_SemenWashVO item = new cls_IVFDashboard_SemenWashVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            CollectionDate = DALHelper.HandleDate(reader["CollectionDate"]),
                            MethodOfCollection = (string) DALHelper.HandleDBNull(reader["MethodOfCollection"]),
                            MethodOfCollectionID = (long) DALHelper.HandleDBNull(reader["MethodOfCollectionID"]),
                            TimeRecSampLab = (DateTime?) DALHelper.HandleDBNull(reader["TimeRecSampLab"]),
                            AbstinenceID = (long) DALHelper.HandleDBNull(reader["Abstinence"]),
                            IsFrozenSample = (bool) DALHelper.HandleDBNull(reader["IsFrozenSample"]),
                            CollecteAtCentre = (bool) DALHelper.HandleDBNull(reader["CollecteAtCentre"]),
                            ColorID = (long) DALHelper.HandleDBNull(reader["Color"]),
                            Quantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                            PH = (float) ((double) DALHelper.HandleDBNull(reader["PH"])),
                            LiquificationTime = (string) DALHelper.HandleDBNull(reader["LiquificationTime"]),
                            Viscosity = (bool) DALHelper.HandleDBNull(reader["Viscosity"]),
                            Odour = (bool) DALHelper.HandleDBNull(reader["Odour"]),
                            CheckedByDoctorID = (long) DALHelper.HandleDBNull(reader["CheckedByDoctorID"]),
                            PreSpermCount = (float) ((double) DALHelper.HandleDBNull(reader["PreSpermCount"])),
                            PreTotalSpermCount = (float) ((double) DALHelper.HandleDBNull(reader["PreTotalSpermCount"])),
                            PreMotility = (float) ((double) DALHelper.HandleDBNull(reader["PreMotility"])),
                            PreNonMotility = (float) ((double) DALHelper.HandleDBNull(reader["PreNonMotility"])),
                            PreTotalMotility = (float) ((double) DALHelper.HandleDBNull(reader["PreTotalMotility"])),
                            PreMotilityGradeI = (float) ((double) DALHelper.HandleDBNull(reader["PreMotilityGradeI"])),
                            PreMotilityGradeII = (float) ((double) DALHelper.HandleDBNull(reader["PreMotilityGradeII"])),
                            PreMotilityGradeIII = (float) ((double) DALHelper.HandleDBNull(reader["PreMotilityGradeIII"])),
                            PreMotilityGradeIV = (float) ((double) DALHelper.HandleDBNull(reader["PreMotilityGradeIV"])),
                            PreNormalMorphology = (float) ((double) DALHelper.HandleDBNull(reader["PreNormalMorphology"])),
                            TypeOfSperm = Convert.ToString(DALHelper.HandleDBNull(reader["TypeOfSperm"])),
                            SpermTypeID = (long) DALHelper.HandleDBNull(reader["TypeOfSpermID"]),
                            SampleCode = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCode"])),
                            SampleLinkID = Convert.ToString(DALHelper.HandleDBNull(reader["SampleLinkID"])),
                            CommentID = (long) DALHelper.HandleDBNull(reader["CommentID"]),
                            RangeViscosityID = (long) DALHelper.HandleDBNull(reader["RangeViscosityID"]),
                            PostSpermCount = (float) ((double) DALHelper.HandleDBNull(reader["PostSpermCount"])),
                            PostTotalSpermCount = (float) ((double) DALHelper.HandleDBNull(reader["PostTotalSpermCount"])),
                            PostMotility = (float) ((double) DALHelper.HandleDBNull(reader["PostMotility"])),
                            PostNonMotility = (float) ((double) DALHelper.HandleDBNull(reader["PostNonMotility"])),
                            PostTotalMotility = (float) ((double) DALHelper.HandleDBNull(reader["PostTotalMotility"])),
                            PostMotilityGradeI = (float) ((double) DALHelper.HandleDBNull(reader["PostMotilityGradeI"])),
                            PostMotilityGradeII = (float) ((double) DALHelper.HandleDBNull(reader["PostMotilityGradeII"])),
                            PostMotilityGradeIII = (float) ((double) DALHelper.HandleDBNull(reader["PostMotilityGradeIII"])),
                            PostMotilityGradeIV = (float) ((double) DALHelper.HandleDBNull(reader["PostMotilityGradeIV"])),
                            PostNormalMorphology = (float) ((double) DALHelper.HandleDBNull(reader["PostNormalMorphology"])),
                            PusCells = (string) DALHelper.HandleDBNull(reader["PusCells"]),
                            RoundCells = (string) DALHelper.HandleDBNull(reader["RoundCells"]),
                            EpithelialCells = (string) DALHelper.HandleDBNull(reader["EpithelialCells"]),
                            AnyOtherCells = (string) DALHelper.HandleDBNull(reader["OtherCells"]),
                            Spillage = (string) DALHelper.HandleDBNull(reader["Spillage"]),
                            MediaUsed = (string) DALHelper.HandleDBNull(reader["MediaUsed"]),
                            SemenProcessingMethodID = (long) DALHelper.HandleDBNull(reader["SemenProcessingMethodID"]),
                            HIV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HIV"])),
                            HCV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HCV"])),
                            VDRL = Convert.ToInt64(DALHelper.HandleDBNull(reader["VDRL"])),
                            HBSAG = Convert.ToInt64(DALHelper.HandleDBNull(reader["HBSAG"])),
                            SpermRecoveredFrom = Convert.ToString(DALHelper.HandleDBNull(reader["SpermRecoverdFrom"])),
                            TransacationTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionTypeId"])),
                            BloodGroupID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BloodGroupID"])),
                            Interpretations = Convert.ToString(DALHelper.HandleDBNull(reader["Interpretations"])),
                            Andrologist = Convert.ToString(DALHelper.HandleDBNull(reader["Andrologist"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            IUIDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["IUIDate"]))),
                            PreperationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["PreperationDate"]))),
                            InSeminatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InSeminatedByID"])),
                            WitnessByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WitnessByID"])),
                            Inseminated = Convert.ToString(DALHelper.HandleDBNull(reader["InseminatedAmount"])),
                            PostAmount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostAmount"])),
                            PostProgMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostProgMotility"])),
                            PostNormalForms = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNormalForms"])),
                            PreAmount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreAmount"])),
                            PreProgMotility = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreProgMotility"])),
                            PreNormalForms = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNormalForms"])),
                            PreNonProgressive = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonProgressive"])),
                            PostNonProgressive = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonProgressive"])),
                            PreNonMotile = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PreNonMotile"])),
                            PostNonMotile = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["PostNonMotile"])),
                            Comment = Convert.ToString(DALHelper.HandleDBNull(reader["IUIComment"])),
                            AllComments = (string) DALHelper.HandleDBNull(reader["Comments"]),
                            Sperm5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Sperm5thPercentile"])),
                            Sperm75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Sperm75thPercentile"])),
                            Ejaculate5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Ejaculate5thPercentile"])),
                            Ejaculate75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Ejaculate75thPercentile"])),
                            TotalMotility5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility5thPercentile"])),
                            TotalMotility75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility75thPercentile"]))
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
    }
}

