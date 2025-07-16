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
    using System.Linq;
    using System.Text;

    internal class clsIVFLabDayDAL : clsBaseIVFLabDayDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFLabDayDAL()
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

        private clsAddLabDay1BizActionVO AddDay1(clsAddLabDay1BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay1VO dayvo = BizActionObj.Day1Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                long planTherapyId = 0L;
                using (List<clsFemaleLabDay1FertilizationAssesmentVO>.Enumerator enumerator = dayvo.FertilizationAssesmentDetails.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        planTherapyId = enumerator.Current.PlanTherapyId;
                    }
                }
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyId", DbType.Int64, planTherapyId);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dayvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Day1Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((BizActionObj.Day1Details.ObservationDetails != null) && (BizActionObj.Day1Details.ObservationDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay1InseminationPlatesVO svo in dayvo.ObservationDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1ObservationDetails");
                        this.dbServer.AddInParameter(command2, "Day1ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, svo.Time);
                        this.dbServer.AddInParameter(command2, "HrAtIns", DbType.Double, svo.HrAtIns);
                        this.dbServer.AddInParameter(command2, "Observation", DbType.String, svo.Observation);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "FertiCheckPeriod", DbType.String, svo.FertiCheckPeriod);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        svo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                if ((BizActionObj.Day1Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day1Details.FertilizationAssesmentDetails.Count > 0))
                {
                    List<clsEmbryoTransferDetailsVO> list1 = new List<clsEmbryoTransferDetailsVO>();
                    foreach (clsFemaleLabDay1FertilizationAssesmentVO tvo2 in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1Details");
                        this.dbServer.AddInParameter(command3, "Day1ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "OoNo", DbType.Int64, tvo2.OoNo);
                        this.dbServer.AddInParameter(command3, "SerialOccyteNo", DbType.Int64, tvo2.SerialOccyteNo);
                        this.dbServer.AddInParameter(command3, "PlanTreatmentID", DbType.Int64, tvo2.PlanTreatmentID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, tvo2.Date);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, tvo2.Time);
                        this.dbServer.AddInParameter(command3, "SrcOocyteID", DbType.Int64, tvo2.SrcOocyteID);
                        this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.String, tvo2.OocyteDonorID);
                        this.dbServer.AddInParameter(command3, "SrcOfSemen", DbType.Int64, tvo2.SrcOfSemen);
                        this.dbServer.AddInParameter(command3, "SemenDonorID", DbType.String, tvo2.SemenDonorID);
                        if (tvo2.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, tvo2.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo2.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, tvo2.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Score", DbType.Int64, tvo2.Score);
                        this.dbServer.AddInParameter(command3, "PV", DbType.Boolean, tvo2.PV);
                        this.dbServer.AddInParameter(command3, "XFactor", DbType.Boolean, tvo2.XFactor);
                        this.dbServer.AddInParameter(command3, "Others", DbType.String, tvo2.Others);
                        this.dbServer.AddInParameter(command3, "FileName", DbType.String, tvo2.FileName);
                        this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, tvo2.FileContents);
                        if (tvo2.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, tvo2.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo2.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay2", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay2", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        tvo2.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if ((tvo2.MediaDetails != null) && (tvo2.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo2 in tvo2.MediaDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo2.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, svo2.Date);
                                this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day1);
                                this.dbServer.AddInParameter(command4, "MediaName", DbType.String, svo2.ItemName);
                                this.dbServer.AddInParameter(command4, "Company", DbType.String, svo2.Company);
                                this.dbServer.AddInParameter(command4, "LotNo", DbType.String, svo2.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, svo2.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "PH", DbType.Boolean, svo2.PH);
                                this.dbServer.AddInParameter(command4, "OSM", DbType.Boolean, svo2.OSM);
                                this.dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, svo2.VolumeUsed);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Int64, svo2.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, svo2.BatchID);
                                this.dbServer.AddInParameter(command4, "StoreID", DbType.Int64, svo2.StoreID);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                svo2.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (tvo2.CalculateDetails != null)
                        {
                            clsFemaleLabDay1CalculateDetailsVO calculateDetails = tvo2.CalculateDetails;
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay1CalculateDetails");
                            this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command5, "DetailID", DbType.Int64, tvo2.ID);
                            this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "TwoPNClosed", DbType.Boolean, calculateDetails.TwoPNClosed);
                            this.dbServer.AddInParameter(command5, "TwoPNSeparated", DbType.Boolean, calculateDetails.TwoPNSeparated);
                            this.dbServer.AddInParameter(command5, "NucleoliAlign", DbType.Boolean, calculateDetails.NucleoliAlign);
                            this.dbServer.AddInParameter(command5, "BeginningAlign", DbType.Boolean, calculateDetails.BeginningAlign);
                            this.dbServer.AddInParameter(command5, "Scattered", DbType.Boolean, calculateDetails.Scattered);
                            this.dbServer.AddInParameter(command5, "CytoplasmHetero", DbType.Boolean, calculateDetails.CytoplasmHetero);
                            this.dbServer.AddInParameter(command5, "CytoplasmHomo", DbType.Boolean, calculateDetails.CytoplasmHomo);
                            this.dbServer.AddInParameter(command5, "NuclearMembrane", DbType.Boolean, calculateDetails.NuclearMembrane);
                            this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, calculateDetails.ID);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                            calculateDetails.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day1Details.FUSetting != null) && (BizActionObj.Day1Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day1);
                        this.dbServer.AddInParameter(command6, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command6, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command6, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command6, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                    }
                }
                if (BizActionObj.Day1Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day1Details.SemenDetails;
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "LabDay", DbType.Int16, IVFLabDay.Day1);
                    this.dbServer.AddInParameter(command7, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day1Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command7, "SourceOfSemen", DbType.Int64, BizActionObj.Day1Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command7, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command7, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command7, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command7, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command7, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command7, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command7, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command7, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command7, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command7, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command7, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command7, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command7, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command7, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command7, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command7, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                }
                if (BizActionObj.Day1Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day1Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day1Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day1Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day1Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay1;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day1Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day1Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day1Details.IsFreezed;
                    valueObject.IsUpdate = false;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day1Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day1Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay2BizActionVO AddDay2(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay2VO dayvo = BizActionObj.Day2Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay2");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dayvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Day2Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((BizActionObj.Day2Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0))
                {
                    List<clsEmbryoTransferDetailsVO> list1 = new List<clsEmbryoTransferDetailsVO>();
                    foreach (clsFemaleLabDay2FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay2Details");
                        this.dbServer.AddInParameter(command2, "Day2ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command2, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedFragmentation != null)
                        {
                            this.dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, tvo.SelectedFragmentation.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastomereSymmetry != null)
                        {
                            this.dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, tvo.SelectedBlastomereSymmetry.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command2, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command2, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command2, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command3, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day2);
                                this.dbServer.AddInParameter(command3, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command3, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command3, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command3, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command3, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command3, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command3, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                        if (tvo.Day2CalculateDetails != null)
                        {
                            clsFemaleLabDay2CalculateDetailsVO svo2 = tvo.Day2CalculateDetails;
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay2CalculateDetails");
                            this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "ZonaThickness", DbType.Int32, svo2.ZonaThickness);
                            this.dbServer.AddInParameter(command4, "ZonaTexture", DbType.Int32, svo2.ZonaTexture);
                            this.dbServer.AddInParameter(command4, "BlastomereSize", DbType.Int32, svo2.BlastomereSize);
                            this.dbServer.AddInParameter(command4, "BlastomereShape", DbType.Int32, svo2.BlastomereShape);
                            this.dbServer.AddInParameter(command4, "BlastomeresVolume", DbType.Int32, svo2.BlastomeresVolume);
                            this.dbServer.AddInParameter(command4, "Membrane", DbType.Int32, svo2.Membrane);
                            this.dbServer.AddInParameter(command4, "Cytoplasm", DbType.Int32, svo2.Cytoplasm);
                            this.dbServer.AddInParameter(command4, "Fragmentation", DbType.Int32, svo2.Fragmentation);
                            this.dbServer.AddInParameter(command4, "DevelopmentRate", DbType.Int32, svo2.DevelopmentRate);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day2Details.FUSetting != null) && (BizActionObj.Day2Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command5, "LabDay", DbType.Int16, IVFLabDay.Day2);
                        this.dbServer.AddInParameter(command5, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command5, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command5, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                    }
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day2);
                    this.dbServer.AddInParameter(command6, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command6, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command6, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command6, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command6, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command6, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command6, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command6, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command6, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command6, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command6, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command6, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command6, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command6, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                }
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day2Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay2;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;
                    valueObject.IsUpdate = false;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day2Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day2Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay2BizActionVO AddDay3(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay2VO dayvo = BizActionObj.Day2Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay3");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dayvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Day2Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((BizActionObj.Day2Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0))
                {
                    List<clsEmbryoTransferDetailsVO> list1 = new List<clsEmbryoTransferDetailsVO>();
                    foreach (clsFemaleLabDay2FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay3Details");
                        this.dbServer.AddInParameter(command2, "Day3ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command2, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedFragmentation != null)
                        {
                            this.dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, tvo.SelectedFragmentation.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastomereSymmetry != null)
                        {
                            this.dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, tvo.SelectedBlastomereSymmetry.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command2, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command2, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command2, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command3, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day3);
                                this.dbServer.AddInParameter(command3, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command3, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command3, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command3, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command3, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command3, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command3, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                        if (tvo.Day2CalculateDetails != null)
                        {
                            clsFemaleLabDay2CalculateDetailsVO svo2 = tvo.Day2CalculateDetails;
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay3CalculateDetails");
                            this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "ZonaThickness", DbType.Int32, svo2.ZonaThickness);
                            this.dbServer.AddInParameter(command4, "ZonaTexture", DbType.Int32, svo2.ZonaTexture);
                            this.dbServer.AddInParameter(command4, "BlastomereSize", DbType.Int32, svo2.BlastomereSize);
                            this.dbServer.AddInParameter(command4, "BlastomereShape", DbType.Int32, svo2.BlastomereShape);
                            this.dbServer.AddInParameter(command4, "BlastomeresVolume", DbType.Int32, svo2.BlastomeresVolume);
                            this.dbServer.AddInParameter(command4, "Membrane", DbType.Int32, svo2.Membrane);
                            this.dbServer.AddInParameter(command4, "Cytoplasm", DbType.Int32, svo2.Cytoplasm);
                            this.dbServer.AddInParameter(command4, "Fragmentation", DbType.Int32, svo2.Fragmentation);
                            this.dbServer.AddInParameter(command4, "DevelopmentRate", DbType.Int32, svo2.DevelopmentRate);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day2Details.FUSetting != null) && (BizActionObj.Day2Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command5, "LabDay", DbType.Int16, IVFLabDay.Day3);
                        this.dbServer.AddInParameter(command5, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command5, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command5, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                    }
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day3);
                    this.dbServer.AddInParameter(command6, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command6, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command6, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command6, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command6, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command6, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command6, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command6, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command6, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command6, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command6, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command6, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command6, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command6, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                }
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day2Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay3;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;
                    valueObject.IsUpdate = false;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day2Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day2Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay4BizActionVO AddDay4(clsAddLabDay4BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay4VO dayvo = BizActionObj.Day4Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay4");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed  ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dayvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Day4Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((BizActionObj.Day4Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day4Details.FertilizationAssesmentDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay4FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay4Details");
                        this.dbServer.AddInParameter(command2, "Day4ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command2, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedFragmentation != null)
                        {
                            this.dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, tvo.SelectedFragmentation.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastomereSymmetry != null)
                        {
                            this.dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, tvo.SelectedBlastomereSymmetry.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command2, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command2, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command2, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command3, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day4);
                                this.dbServer.AddInParameter(command3, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command3, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command3, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command3, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command3, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command3, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command3, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                        if (tvo.Day4CalculateDetails != null)
                        {
                            clsFemaleLabDay4CalculateDetailsVO svo2 = tvo.Day4CalculateDetails;
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay4CalculateDetails");
                            this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "Compaction", DbType.Boolean, svo2.Compaction);
                            this.dbServer.AddInParameter(command4, "SignsOfBlastocoel", DbType.Boolean, svo2.SignsOfBlastocoel);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day4Details.FUSetting != null) && (BizActionObj.Day4Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command5, "LabDay", DbType.Int16, IVFLabDay.Day4);
                        this.dbServer.AddInParameter(command5, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command5, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command5, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                    }
                }
                if (BizActionObj.Day4Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day4Details.SemenDetails;
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day4);
                    this.dbServer.AddInParameter(command6, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day4Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command6, "SourceOfSemen", DbType.Int64, BizActionObj.Day4Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command6, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command6, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command6, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command6, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command6, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command6, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command6, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command6, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command6, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command6, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command6, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command6, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                }
                if (BizActionObj.Day4Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day4Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day4Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day4Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day4Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay4;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day4Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day4Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day4Details.IsFreezed;
                    valueObject.IsUpdate = false;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day4Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day4Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay5BizActionVO AddDay5(clsAddLabDay5BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay5VO dayvo = BizActionObj.Day5Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay5");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dayvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Day5Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((BizActionObj.Day5Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day5Details.FertilizationAssesmentDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay5FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay5Details");
                        this.dbServer.AddInParameter(command2, "Day5ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command2, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedICM != null)
                        {
                            this.dbServer.AddInParameter(command2, "ICM", DbType.Int64, tvo.SelectedICM.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "ICM", DbType.Int64, 0);
                        }
                        if (tvo.SelectedTrophectoderm != null)
                        {
                            this.dbServer.AddInParameter(command2, "Trophectoderm", DbType.Int64, tvo.SelectedTrophectoderm.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Trophectoderm", DbType.Int64, 0);
                        }
                        if (tvo.SelectedExpansion != null)
                        {
                            this.dbServer.AddInParameter(command2, "Expansion", DbType.Int64, tvo.SelectedExpansion.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Expansion", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastocytsGrade != null)
                        {
                            this.dbServer.AddInParameter(command2, "BlastocytsGrade", DbType.Int64, tvo.SelectedBlastocytsGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "BlastocytsGrade", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command2, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command2, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command2, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command3, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day5);
                                this.dbServer.AddInParameter(command3, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command3, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command3, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command3, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command3, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command3, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command3, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                        if (tvo.Day5CalculateDetails != null)
                        {
                            clsFemaleLabDay5CalculateDetailsVO svo2 = tvo.Day5CalculateDetails;
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay5CalculateDetails");
                            this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "BlastocoelsCavity", DbType.Boolean, svo2.BlastocoelsCavity);
                            this.dbServer.AddInParameter(command4, "TightlyPackedCells", DbType.Boolean, svo2.TightlyPackedCells);
                            this.dbServer.AddInParameter(command4, "FormingEpithelium", DbType.Boolean, svo2.FormingEpithelium);
                            this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day5Details.FUSetting != null) && (BizActionObj.Day5Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command5, "LabDay", DbType.Int16, IVFLabDay.Day5);
                        this.dbServer.AddInParameter(command5, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command5, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command5, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                    }
                }
                if (BizActionObj.Day5Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day5Details.SemenDetails;
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day5);
                    this.dbServer.AddInParameter(command6, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day5Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command6, "SourceOfSemen", DbType.Int64, BizActionObj.Day5Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command6, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command6, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command6, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command6, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command6, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command6, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command6, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command6, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command6, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command6, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command6, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command6, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command6, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command6, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                }
                if (BizActionObj.Day5Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day5Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day5Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day5Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day5Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay5;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day5Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day5Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day5Details.IsFreezed;
                    valueObject.IsUpdate = false;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day5Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day5Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay6BizActionVO AddDay6(clsAddLabDay6BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay6VO dayvo = BizActionObj.Day6Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay6");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, dayvo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.Day6Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((BizActionObj.Day6Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day6Details.FertilizationAssesmentDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay6FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay6Details");
                        this.dbServer.AddInParameter(command2, "Day6ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command2, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command2, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command2, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command2, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay7", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command2, "ProceedDay7", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command3, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day6);
                                this.dbServer.AddInParameter(command3, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command3, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command3, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command3, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command3, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command3, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command3, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                    }
                }
                if ((BizActionObj.Day6Details.FUSetting != null) && (BizActionObj.Day6Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day6);
                        this.dbServer.AddInParameter(command4, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command4, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command4, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                    }
                }
                if (BizActionObj.Day6Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day6Details.SemenDetails;
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "LabDay", DbType.Int16, IVFLabDay.Day6);
                    this.dbServer.AddInParameter(command5, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day6Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command5, "SourceOfSemen", DbType.Int64, BizActionObj.Day6Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command5, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command5, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command5, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command5, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command5, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command5, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command5, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command5, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command5, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command5, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command5, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command5, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command5, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command5, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command5, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command5, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                }
                if (BizActionObj.Day6Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day6Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day6Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day6Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day6Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay6;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day6Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day6Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day6Details.IsFreezed;
                    valueObject.IsUpdate = false;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day6Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day6Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddLabDay1(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay1BizActionVO bizActionObj = valueObject as clsAddLabDay1BizActionVO;
            bizActionObj = (bizActionObj.Day1Details.ID != 0L) ? this.UpdateDay1(bizActionObj, UserVo) : this.AddDay1(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddLabDay2(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay2BizActionVO bizActionObj = valueObject as clsAddLabDay2BizActionVO;
            bizActionObj = (bizActionObj.Day2Details.ID != 0L) ? this.UpdateDay2(bizActionObj, UserVo) : this.AddDay2(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddLabDay3(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay2BizActionVO bizActionObj = valueObject as clsAddLabDay2BizActionVO;
            bizActionObj = (bizActionObj.Day2Details.ID != 0L) ? this.UpdateDay3(bizActionObj, UserVo) : this.AddDay3(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddLabDay4(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay4BizActionVO bizActionObj = valueObject as clsAddLabDay4BizActionVO;
            bizActionObj = (bizActionObj.Day4Details.ID != 0L) ? this.UpdateDay4(bizActionObj, UserVo) : this.AddDay4(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddLabDay5(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay5BizActionVO bizActionObj = valueObject as clsAddLabDay5BizActionVO;
            bizActionObj = (bizActionObj.Day5Details.ID != 0L) ? this.UpdateDay5(bizActionObj, UserVo) : this.AddDay5(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddLabDay6(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay6BizActionVO bizActionObj = valueObject as clsAddLabDay6BizActionVO;
            bizActionObj = (bizActionObj.Day6Details.ID != 0L) ? this.UpdateDay6(bizActionObj, UserVo) : this.AddDay6(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddUpdateFemaleLabDay0(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            clsAddUpdateFemaleLabDay0BizActionVO nvo = valueObject as clsAddUpdateFemaleLabDay0BizActionVO;
            try
            {
                DbCommand storedProcCommand;
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                if (!nvo.IsUpdate)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleLabDay0");
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FemaleLabDay0.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_UpdateFemaleLabDay0");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.FemaleLabDay0.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.FemaleLabDay0.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, nvo.FemaleLabDay0.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.FemaleLabDay0.ProcDate);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.FemaleLabDay0.ProcTime);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.FemaleLabDay0.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, nvo.FemaleLabDay0.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, nvo.FemaleLabDay0.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, nvo.FemaleLabDay0.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, nvo.FemaleLabDay0.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SrcNeedleID", DbType.Int64, nvo.FemaleLabDay0.SrcOfNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "NeedleCompanyID", DbType.Int64, nvo.FemaleLabDay0.NeedleCompanyID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOocyteID", DbType.Int64, nvo.FemaleLabDay0.SrcOfOocyteID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.String, nvo.FemaleLabDay0.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOfSemen", DbType.Int64, nvo.FemaleLabDay0.SrcOfSemenID);
                this.dbServer.AddInParameter(storedProcCommand, "SemenDonorID", DbType.String, nvo.FemaleLabDay0.SemenDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "TreatmentPlanID", DbType.Int64, nvo.FemaleLabDay0.TreatmentTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "ICSICompleteionTime", DbType.DateTime, nvo.FemaleLabDay0.ICSICompletionTime);
                this.dbServer.AddInParameter(storedProcCommand, "SrcDenudingNeedleID", DbType.Int64, nvo.FemaleLabDay0.SourceOfDenudingNeedle);
                this.dbServer.AddInParameter(storedProcCommand, "FertilizationCheckTime", DbType.DateTime, nvo.FemaleLabDay0.FertilizationCheckTime);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.FemaleLabDay0.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreparationMedia", DbType.String, nvo.FemaleLabDay0.SpermPreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.FemaleLabDay0.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "Matured", DbType.Int32, nvo.FemaleLabDay0.Matured);
                this.dbServer.AddInParameter(storedProcCommand, "Immatured", DbType.Int32, nvo.FemaleLabDay0.Immatured);
                this.dbServer.AddInParameter(storedProcCommand, "PostMatured", DbType.Int32, nvo.FemaleLabDay0.PostMatured);
                this.dbServer.AddInParameter(storedProcCommand, "Total", DbType.Int32, nvo.FemaleLabDay0.Total);
                this.dbServer.AddInParameter(storedProcCommand, "DoneBy", DbType.String, nvo.FemaleLabDay0.DoneBy);
                this.dbServer.AddInParameter(storedProcCommand, "FollicularFluid", DbType.String, nvo.FemaleLabDay0.FollicularFluid);
                this.dbServer.AddInParameter(storedProcCommand, "OPSTypeID", DbType.Int64, nvo.FemaleLabDay0.OPSTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCompleteionTime", DbType.DateTime, nvo.FemaleLabDay0.IVFCompletionTime);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.FemaleLabDay0.IsFreezed);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == -1)
                {
                    throw new Exception();
                }
                if (!nvo.IsUpdate)
                {
                    nvo.FemaleLabDay0.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                else
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_IVF_FemaleLabDay0Details where UnitID=", nvo.FemaleLabDay0.UnitID, " AND OocyteID =", nvo.FemaleLabDay0.ID }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_IVF_LabDayUploadedFiles where UnitID=", nvo.FemaleLabDay0.UnitID, " AND OocyteID =", nvo.FemaleLabDay0.ID, " AND LabDay=0" }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Delete from T_IVF_LabDayMediaDetails where UnitID=", nvo.FemaleLabDay0.UnitID, " AND OocyteID =", nvo.FemaleLabDay0.ID, " AND LabDay=0" }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                if ((nvo.FemaleLabDay0.FUSetting != null) && (nvo.FemaleLabDay0.FUSetting.Count > 0))
                {
                    for (int i = 0; i < nvo.FemaleLabDay0.FUSetting.Count; i++)
                    {
                        storedProcCommand = null;

                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, nvo.FemaleLabDay0.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day0);
                        this.dbServer.AddInParameter(storedProcCommand, "FileName", DbType.String, nvo.FemaleLabDay0.FUSetting[i].FileName);
                        this.dbServer.AddInParameter(storedProcCommand, "FileIndex", DbType.Int32, nvo.FemaleLabDay0.FUSetting[i].Index);
                        this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.Binary, nvo.FemaleLabDay0.FUSetting[i].Data);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FemaleLabDay0.FUSetting[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        nvo.FemaleLabDay0.FUSetting[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (nvo.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                    }
                }
                if (nvo.FemaleLabDay0.SemenDetails != null)
                {
                    storedProcCommand = null;
                   storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab1SemenDetails");

                    this.dbServer.AddInParameter(storedProcCommand, "Day1ID", DbType.Int64, nvo.FemaleLabDay0.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int32, IVFLabDay.Day0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
                if (nvo.FemaleLabDay0.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = nvo.FemaleLabDay0.SemenDetails;
                    storedProcCommand = null;

                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, nvo.FemaleLabDay0.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day0);
                    this.dbServer.AddInParameter(storedProcCommand, "MethodOfSpermPreparation", DbType.Int64, nvo.FemaleLabDay0.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(storedProcCommand, "SourceOfSemen", DbType.Int64, nvo.FemaleLabDay0.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(storedProcCommand, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(storedProcCommand, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(storedProcCommand, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(storedProcCommand, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(storedProcCommand, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(storedProcCommand, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(storedProcCommand, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(storedProcCommand, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(storedProcCommand, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(storedProcCommand, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(storedProcCommand, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(storedProcCommand, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(storedProcCommand, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(storedProcCommand, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(storedProcCommand, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(storedProcCommand, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
                if ((nvo.FemaleLabDay0.IVFSetting != null) && (nvo.FemaleLabDay0.IVFSetting.Count > 0))
                {
                    for (int i = 0; i < nvo.FemaleLabDay0.IVFSetting.Count; i++)
                    {
                        storedProcCommand = null;
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleLabDay0Details");

                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, nvo.FemaleLabDay0.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteNO", DbType.Int64, nvo.FemaleLabDay0.IVFSetting[i].OocyteNO);
                        if (nvo.FemaleLabDay0.IVFSetting[i].Cumulus != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.FemaleLabDay0.IVFSetting[i].Cumulus.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, 0);
                        }
                        if (nvo.FemaleLabDay0.IVFSetting[i].Grade != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.FemaleLabDay0.IVFSetting[i].Grade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, 0);
                        }
                        if (nvo.FemaleLabDay0.IVFSetting[i].MOI != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.FemaleLabDay0.IVFSetting[i].MOI.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, 0);
                        }
                        int score = nvo.FemaleLabDay0.IVFSetting[i].Score;
                        this.dbServer.AddInParameter(storedProcCommand, "Score", DbType.Int32, nvo.FemaleLabDay0.IVFSetting[i].Score);
                        if (nvo.FemaleLabDay0.IVFSetting[i].Plan != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, nvo.FemaleLabDay0.IVFSetting[i].Plan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, 0);
                        }
                        if (nvo.FemaleLabDay0.IVFSetting[i].Plan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "ProceedToDay1", DbType.Int64, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "ProceedToDay1", DbType.Int64, false);
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "MBD", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "DOSID", DbType.Int64, null);
                        this.dbServer.AddInParameter(storedProcCommand, "Comment", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "PICID", DbType.Int64, null);
                        this.dbServer.AddInParameter(storedProcCommand, "IC", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTreatmentID", DbType.Int32, 1);
                        this.dbServer.AddInParameter(storedProcCommand, "FileName", DbType.String, nvo.FemaleLabDay0.IVFSetting[i].FileName);
                        this.dbServer.AddInParameter(storedProcCommand, "FileContents", DbType.Binary, nvo.FemaleLabDay0.IVFSetting[i].FileContents);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FemaleLabDay0.IVFSetting[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        nvo.FemaleLabDay0.IVFSetting[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (nvo.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        if ((nvo.FemaleLabDay0.IVFSetting[i].MediaDetails != null) && (nvo.FemaleLabDay0.IVFSetting[i].MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo2 in nvo.FemaleLabDay0.IVFSetting[i].MediaDetails)
                            {
                                DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, nvo.FemaleLabDay0.ID);
                                this.dbServer.AddInParameter(command7, "DetailID", DbType.Int64, nvo.FemaleLabDay0.IVFSetting[i].ID);
                                this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command7, "LabDay", DbType.Int16, IVFLabDay.Day0);
                                this.dbServer.AddInParameter(command7, "MediaName", DbType.String, svo2.ItemName);
                                this.dbServer.AddInParameter(command7, "Company", DbType.String, svo2.Company);
                                this.dbServer.AddInParameter(command7, "LotNo", DbType.String, svo2.BatchCode);
                                this.dbServer.AddInParameter(command7, "ExpiryDate", DbType.DateTime, svo2.ExpiryDate);
                                this.dbServer.AddInParameter(command7, "PH", DbType.Boolean, svo2.PH);
                                this.dbServer.AddInParameter(command7, "OSM", DbType.Boolean, svo2.OSM);
                                this.dbServer.AddInParameter(command7, "VolumeUsed", DbType.String, svo2.VolumeUsed);
                                this.dbServer.AddInParameter(command7, "Status", DbType.Int64, svo2.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command7, "BatchID", DbType.Int64, svo2.BatchID);
                                this.dbServer.AddInParameter(command7, "StoreID", DbType.Int64, svo2.StoreID);
                                this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                                this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command7, transaction);
                                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                                svo2.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                                if (nvo.SuccessStatus == -1)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                if ((nvo.FemaleLabDay0.ICSISetting != null) && (nvo.FemaleLabDay0.ICSISetting.Count > 0))
                {
                    for (int i = 0; i < nvo.FemaleLabDay0.ICSISetting.Count; i++)
                    {
                        storedProcCommand = null;
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleLabDay0Details");

                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, nvo.FemaleLabDay0.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteNO", DbType.Int64, nvo.FemaleLabDay0.ICSISetting[i].OocyteNO);
                        this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, null);
                        this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, null);
                        this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, null);
                        this.dbServer.AddInParameter(storedProcCommand, "Score", DbType.Int32, null);
                        this.dbServer.AddInParameter(storedProcCommand, "FileName", DbType.String, nvo.FemaleLabDay0.ICSISetting[i].FileName);
                        this.dbServer.AddInParameter(storedProcCommand, "FileContents", DbType.Binary, nvo.FemaleLabDay0.ICSISetting[i].FileContents);
                        if (nvo.FemaleLabDay0.ICSISetting[i].Plan != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, nvo.FemaleLabDay0.ICSISetting[i].Plan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, 0);
                        }
                        if (nvo.FemaleLabDay0.ICSISetting[i].Plan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "ProceedToDay1", DbType.Int64, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "ProceedToDay1", DbType.Int64, false);
                        }
                        if (nvo.FemaleLabDay0.ICSISetting[i].MBD != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "MBD", DbType.String, nvo.FemaleLabDay0.ICSISetting[i].MBD);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "MBD", DbType.String, 0);
                        }
                        if (nvo.FemaleLabDay0.ICSISetting[i].DOS != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "DOSID", DbType.Int64, nvo.FemaleLabDay0.ICSISetting[i].DOS.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "DOSID", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "Comment", DbType.String, nvo.FemaleLabDay0.ICSISetting[i].Comment);
                        if (nvo.FemaleLabDay0.ICSISetting[i].PIC != null)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "PICID", DbType.Int64, nvo.FemaleLabDay0.ICSISetting[i].PIC.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "PICID", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "IC", DbType.String, nvo.FemaleLabDay0.ICSISetting[i].IC);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTreatmentID", DbType.Int32, 2);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FemaleLabDay0.ICSISetting[i].ID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                        nvo.FemaleLabDay0.ICSISetting[i].ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        if (nvo.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        if ((nvo.FemaleLabDay0.ICSISetting[i].MediaDetails != null) && (nvo.FemaleLabDay0.ICSISetting[i].MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo3 in nvo.FemaleLabDay0.ICSISetting[i].MediaDetails)
                            {
                                DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command9, "OocyteID", DbType.Int64, nvo.FemaleLabDay0.ID);
                                this.dbServer.AddInParameter(command9, "DetailID", DbType.Int64, nvo.FemaleLabDay0.ICSISetting[i].ID);
                                this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command9, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command9, "LabDay", DbType.Int16, IVFLabDay.Day0);
                                this.dbServer.AddInParameter(command9, "MediaName", DbType.String, svo3.ItemName);
                                this.dbServer.AddInParameter(command9, "Company", DbType.String, svo3.Company);
                                this.dbServer.AddInParameter(command9, "LotNo", DbType.String, svo3.BatchCode);
                                this.dbServer.AddInParameter(command9, "ExpiryDate", DbType.DateTime, svo3.ExpiryDate);
                                this.dbServer.AddInParameter(command9, "PH", DbType.Boolean, svo3.PH);
                                this.dbServer.AddInParameter(command9, "OSM", DbType.Boolean, svo3.OSM);
                                this.dbServer.AddInParameter(command9, "VolumeUsed", DbType.String, svo3.VolumeUsed);
                                this.dbServer.AddInParameter(command9, "Status", DbType.Int64, svo3.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command9, "BatchID", DbType.Int64, svo3.BatchID);
                                this.dbServer.AddInParameter(command9, "StoreID", DbType.Int64, svo3.StoreID);
                                this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo3.ID);
                                this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command9, transaction);
                                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                                svo3.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                                if (nvo.SuccessStatus == -1)
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                if (nvo.FemaleLabDay0.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO nvo2 = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = nvo.FemaleLabDay0.LabDaySummary
                    };
                    nvo2.LabDaysSummary.OocyteID = nvo.FemaleLabDay0.ID;
                    nvo2.LabDaysSummary.CoupleID = nvo.FemaleLabDay0.CoupleID;
                    nvo2.LabDaysSummary.CoupleUnitID = nvo.FemaleLabDay0.CoupleUnitID;
                    nvo2.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay0;
                    nvo2.LabDaysSummary.IsFreezed = nvo.FemaleLabDay0.IsFreezed;
                    nvo2.LabDaysSummary.Priority = 1;
                    nvo2.LabDaysSummary.ProcDate = nvo.FemaleLabDay0.ProcDate;
                    nvo2.LabDaysSummary.ProcTime = nvo.FemaleLabDay0.ProcTime;
                    nvo2.LabDaysSummary.UnitID = nvo.FemaleLabDay0.UnitID;
                    nvo2.IsUpdate = nvo.IsUpdate;
                    nvo2 = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(nvo2, UserVo, pConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.FemaleLabDay0.LabDaySummary.ID = nvo2.LabDaysSummary.ID;
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                transaction.Rollback();
                nvo.FemaleLabDay0 = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return nvo;
        }

        public override IValueObject GetAllDayMediaDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAllDayMediaDetailsBizActionVO nvo = valueObject as clsGetAllDayMediaDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsForAllDay");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.Int32, nvo.LabDay);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MediaList == null)
                    {
                        nvo.MediaList = new List<clsFemaleMediaDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleMediaDetailsVO Media = new clsFemaleMediaDetailsVO {
                            Date = DALHelper.HandleDate(reader["Date"]),
                            ItemName = (string) DALHelper.HandleDBNull(reader["MediaName"]),
                            Company = (string) DALHelper.HandleDBNull(reader["Company"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["LotNo"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            PH = (bool) DALHelper.HandleDBNull(reader["PH"]),
                            OSM = (bool) DALHelper.HandleDBNull(reader["OSM"]),
                            VolumeUsed = (string) DALHelper.HandleDBNull(reader["VolumeUsed"]),
                            SelectedStatus = { ID = (long) DALHelper.HandleDBNull(reader["Status"]) }
                        };
                        Media.SelectedStatus = Media.Status.FirstOrDefault<MasterListItem>(q => q.ID == Media.SelectedStatus.ID);
                        Media.StoreID = (long) DALHelper.HandleDBNull(reader["StoreID"]);
                        Media.BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]);
                        nvo.MediaList.Add(Media);
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

        public override IValueObject GetCleavageGradeMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCleavageGradeMasterListBizActionVO nvo = valueObject as clsGetCleavageGradeMasterListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCleavageGradeMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "ApplyTo", DbType.Int64, nvo.CleavageGrade.ApplyTo);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CleavageGradeList == null)
                    {
                        nvo.CleavageGradeList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Name"]),
                            Name = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Flag = (string) DALHelper.HandleDBNull(reader["Flag"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ApplyTo = (long) DALHelper.HandleDBNull(reader["ApplyTo"]),
                            FragmentationID = (long) DALHelper.HandleDBNull(reader["FragmentationID"])
                        };
                        nvo.CleavageGradeList.Add(item);
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

        public override IValueObject GetFemaleLabDay0(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFemaleLabDay0BizActionVO nvo = valueObject as clsGetFemaleLabDay0BizActionVO;
            try
            {
                if (nvo.FemaleLabDay0 == null)
                {
                    nvo.FemaleLabDay0 = new clsFemaleLabDay0VO();
                }
                if (nvo.FemaleLabDay0.IVFSetting == null)
                {
                    nvo.FemaleLabDay0.IVFSetting = new List<IVFTreatment>();
                }
                if (nvo.FemaleLabDay0.ICSISetting == null)
                {
                    nvo.FemaleLabDay0.ICSISetting = new List<ICSITreatment>();
                }
                if (nvo.FemaleLabDay0.FUSetting == null)
                {
                    nvo.FemaleLabDay0.FUSetting = new List<FileUpload>();
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay0");
                this.dbServer.AddInParameter(storedProcCommand, "OocyteID", DbType.Int64, nvo.OocyteID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.FemaleLabDay0.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.FemaleLabDay0.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.FemaleLabDay0.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.FemaleLabDay0.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.FemaleLabDay0.ProcDate = DALHelper.HandleDate(reader["Date"]);
                        nvo.FemaleLabDay0.ProcTime = DALHelper.HandleDate(reader["Time"]);
                        nvo.FemaleLabDay0.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.FemaleLabDay0.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.FemaleLabDay0.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.FemaleLabDay0.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.FemaleLabDay0.IVFCycleCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["IVFCycleCount"]));
                        nvo.FemaleLabDay0.SrcOfNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcNeedleID"]));
                        nvo.FemaleLabDay0.NeedleCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NeedleCompanyID"]));
                        nvo.FemaleLabDay0.SrcOfOocyteID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOocyteID"]));
                        nvo.FemaleLabDay0.OocyteDonorID = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.FemaleLabDay0.SrcOfSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOfSemen"]));
                        nvo.FemaleLabDay0.SemenDonorID = Convert.ToString(DALHelper.HandleDBNull(reader["SemenDonorID"]));
                        nvo.FemaleLabDay0.TreatmentTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentPlanID"]));
                        nvo.FemaleLabDay0.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.FemaleLabDay0.SpermPreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.FemaleLabDay0.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.FemaleLabDay0.Matured = Convert.ToInt32(DALHelper.HandleDBNull(reader["Matured"]));
                        nvo.FemaleLabDay0.Immatured = Convert.ToInt32(DALHelper.HandleDBNull(reader["Immatured"]));
                        nvo.FemaleLabDay0.PostMatured = Convert.ToInt32(DALHelper.HandleDBNull(reader["PostMatured"]));
                        nvo.FemaleLabDay0.Total = Convert.ToInt32(DALHelper.HandleDBNull(reader["Total"]));
                        nvo.FemaleLabDay0.DoneBy = Convert.ToString(DALHelper.HandleDBNull(reader["DoneBy"]));
                        nvo.FemaleLabDay0.FollicularFluid = Convert.ToString(DALHelper.HandleDBNull(reader["FollicularFluid"]));
                        nvo.FemaleLabDay0.OPSTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPSTypeID"]));
                        nvo.FemaleLabDay0.IVFCompletionTime = DALHelper.HandleDate(reader["IVFCompleteionTime"]);
                        nvo.FemaleLabDay0.ICSICompletionTime = DALHelper.HandleDate(reader["ICSICompleteionTime"]);
                        nvo.FemaleLabDay0.SourceOfDenudingNeedle = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcDenudingNeedleID"]));
                        nvo.FemaleLabDay0.FertilizationCheckTime = DALHelper.HandleDate(reader["FertilizationCheckTime"]);
                        nvo.FemaleLabDay0.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.FemaleLabDay0.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.NextResult();
                int num = 0;
                int num2 = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        IVFTreatment treatment = new IVFTreatment();
                        ICSITreatment treatment2 = new ICSITreatment();
                        int num3 = Convert.ToInt32(DALHelper.HandleDBNull(reader["PlanTreatmentID"]));
                        if (num3 == 1)
                        {
                            treatment.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            treatment.Index = num;
                            treatment.SerialOccyteNo = Convert.ToInt32(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));
                            treatment.OocyteNO = Convert.ToInt32(DALHelper.HandleDBNull(reader["OocyteNO"]));
                            MasterListItem item = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]))
                            };
                            treatment.Cumulus = item;
                            MasterListItem item2 = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]))
                            };
                            treatment.Grade = item2;
                            MasterListItem item3 = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]))
                            };
                            treatment.MOI = item3;
                            treatment.Score = Convert.ToInt32(DALHelper.HandleDBNull(reader["Score"]));
                            treatment.ProceedToDay = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ProceedToDay1"]));
                            MasterListItem item4 = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"]))
                            };
                            treatment.Plan = item4;
                            treatment.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                            treatment.FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"]);
                            clsBaseIVFLabDayDAL instance = GetInstance();
                            clsGetAllDayMediaDetailsBizActionVO nvo2 = new clsGetAllDayMediaDetailsBizActionVO {
                                ID = nvo.FemaleLabDay0.ID,
                                DetailID = treatment.ID,
                                LabDay = 0
                            };
                            treatment.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) instance.GetAllDayMediaDetails(nvo2, UserVo)).MediaList;
                            nvo.FemaleLabDay0.IVFSetting.Add(treatment);
                            num++;
                            continue;
                        }
                        if (num3 == 2)
                        {
                            treatment2.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            treatment2.Index = num2;
                            treatment2.OocyteNO = Convert.ToInt32(DALHelper.HandleDBNull(reader["OocyteNO"]));
                            MasterListItem item5 = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DOSID"]))
                            };
                            treatment2.DOS = item5;
                            MasterListItem item6 = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PICID"]))
                            };
                            treatment2.PIC = item6;
                            treatment2.MBD = Convert.ToString(DALHelper.HandleDBNull(reader["MBD"]));
                            treatment2.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                            treatment2.IC = Convert.ToString(DALHelper.HandleDBNull(reader["IC"]));
                            treatment2.ProceedToDay = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ProceedToDay1"]));
                            MasterListItem item7 = new MasterListItem {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"]))
                            };
                            treatment2.Plan = item7;
                            treatment2.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                            treatment2.FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"]);
                            clsBaseIVFLabDayDAL instance = GetInstance();
                            clsGetAllDayMediaDetailsBizActionVO nvo3 = new clsGetAllDayMediaDetailsBizActionVO {
                                ID = nvo.FemaleLabDay0.ID,
                                DetailID = treatment2.ID,
                                LabDay = 0
                            };
                            treatment2.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) instance.GetAllDayMediaDetails(nvo3, UserVo)).MediaList;
                            nvo.FemaleLabDay0.ICSISetting.Add(treatment2);
                            num2++;
                        }
                    }
                }
                reader.NextResult();
                int num4 = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FileUpload item = new FileUpload {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Index = num4,
                            FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                        };
                        nvo.FemaleLabDay0.FUSetting.Add(item);
                        num4++;
                    }
                }
                reader.NextResult();
                if (nvo.FemaleLabDay0.SemenDetails == null)
                {
                    nvo.FemaleLabDay0.SemenDetails = new clsFemaleSemenDetailsVO();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        break;
                    }
                    nvo.FemaleLabDay0.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                    nvo.FemaleLabDay0.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                    nvo.FemaleLabDay0.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                    nvo.FemaleLabDay0.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                    nvo.FemaleLabDay0.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                    nvo.FemaleLabDay0.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                    nvo.FemaleLabDay0.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                    nvo.FemaleLabDay0.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                    nvo.FemaleLabDay0.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                    nvo.FemaleLabDay0.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                    nvo.FemaleLabDay0.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                    nvo.FemaleLabDay0.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                    nvo.FemaleLabDay0.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                    nvo.FemaleLabDay0.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                    nvo.FemaleLabDay0.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                    nvo.FemaleLabDay0.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                    nvo.FemaleLabDay0.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                    nvo.FemaleLabDay0.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                    nvo.FemaleLabDay0.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                    nvo.FemaleLabDay0.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                }
            }
            catch (Exception)
            {
                nvo.FemaleLabDay0 = null;
                throw;
            }
            return nvo;
        }

        public override IValueObject GetFemaleLabDay1(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay1DetailsBizActionVO nvo = valueObject as clsGetDay1DetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay1");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day1);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDay1 == null)
                    {
                        nvo.LabDay1 = new clsFemaleLabDay1VO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.LabDay1.ObservationDetails == null)
                            {
                                nvo.LabDay1.ObservationDetails = new List<clsFemaleLabDay1InseminationPlatesVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.LabDay1.FertilizationAssesmentDetails == null)
                                    {
                                        nvo.LabDay1.FertilizationAssesmentDetails = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.LabDay1.SemenDetails == null)
                                            {
                                                nvo.LabDay1.SemenDetails = new clsFemaleSemenDetailsVO();
                                            }
                                            while (true)
                                            {
                                                if (!reader.Read())
                                                {
                                                    reader.NextResult();
                                                    if (nvo.LabDay1.FUSetting == null)
                                                    {
                                                        nvo.LabDay1.FUSetting = new List<FileUpload>();
                                                    }
                                                    while (reader.Read())
                                                    {
                                                        FileUpload upload = new FileUpload {
                                                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                            Index = (int) DALHelper.HandleDBNull(reader["FileIndex"]),
                                                            FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                                            Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                                        };
                                                        nvo.LabDay1.FUSetting.Add(upload);
                                                    }
                                                    break;
                                                }
                                                nvo.LabDay1.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                                                nvo.LabDay1.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                                                nvo.LabDay1.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                                                nvo.LabDay1.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                                                nvo.LabDay1.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                                                nvo.LabDay1.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                                                nvo.LabDay1.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                                                nvo.LabDay1.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                                                nvo.LabDay1.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                                                nvo.LabDay1.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                                                nvo.LabDay1.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                                                nvo.LabDay1.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                                                nvo.LabDay1.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                                                nvo.LabDay1.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                                                nvo.LabDay1.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                                                nvo.LabDay1.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                                                nvo.LabDay1.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                                                nvo.LabDay1.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                                                nvo.LabDay1.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                                                nvo.LabDay1.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                                            }
                                            break;
                                        }
                                        clsFemaleLabDay1FertilizationAssesmentVO tvo = new clsFemaleLabDay1FertilizationAssesmentVO {
                                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                            Date = DALHelper.HandleDate(reader["Date"]),
                                            Time = DALHelper.HandleDate(reader["Time"]),
                                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                                            SelectedFePlan = { 
                                                ID = (long) DALHelper.HandleDBNull(reader["FertilisationStage"]),
                                                Description = (string) DALHelper.HandleDBNull(reader["Fertilization"])
                                            },
                                            SelectedGrade = { 
                                                ID = (long) DALHelper.HandleDBNull(reader["GradeID"]),
                                                Description = (string) DALHelper.HandleDBNull(reader["Grade"])
                                            },
                                            Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                                            PV = (bool) DALHelper.HandleDBNull(reader["PV"]),
                                            XFactor = (bool) DALHelper.HandleDBNull(reader["XFactor"]),
                                            Others = (string) DALHelper.HandleDBNull(reader["Others"]),
                                            ProceedDay2 = (bool) DALHelper.HandleDBNull(reader["ProceedDay2"]),
                                            SelectedPlan = { 
                                                ID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                                                Description = (string) DALHelper.HandleDBNull(reader["PlanName"])
                                            },
                                            FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                            FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"])
                                        };
                                        clsBaseIVFLabDayDAL instance = GetInstance();
                                        clsGetDay1MediaAndCalcDetailsBizActionVO nvo2 = new clsGetDay1MediaAndCalcDetailsBizActionVO {
                                            ID = nvo.ID,
                                            DetailID = tvo.ID
                                        };
                                        tvo.CalculateDetails = ((clsGetDay1MediaAndCalcDetailsBizActionVO) instance.GetFemaleLabDay1MediaAndCalDetails(nvo2, UserVo)).LabDayCalDetails;
                                        clsBaseIVFLabDayDAL ydal2 = GetInstance();
                                        clsGetAllDayMediaDetailsBizActionVO nvo3 = new clsGetAllDayMediaDetailsBizActionVO {
                                            ID = nvo.ID,
                                            DetailID = tvo.ID,
                                            LabDay = 1
                                        };
                                        tvo.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) ydal2.GetAllDayMediaDetails(nvo3, UserVo)).MediaList;
                                        nvo.LabDay1.FertilizationAssesmentDetails.Add(tvo);
                                    }
                                    break;
                                }
                                clsFemaleLabDay1InseminationPlatesVO item = new clsFemaleLabDay1InseminationPlatesVO {
                                    Time = (DateTime?) DALHelper.HandleDBNull(reader["Time"]),
                                    HrAtIns = (double) DALHelper.HandleDBNull(reader["HrAtIns"]),
                                    Observation = (string) DALHelper.HandleDBNull(reader["Observation"]),
                                    FertiCheckPeriod = (string) DALHelper.HandleDBNull(reader["FertiCheckPeriod"])
                                };
                                nvo.LabDay1.ObservationDetails.Add(item);
                            }
                            break;
                        }
                        nvo.LabDay1.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.LabDay1.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.LabDay1.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.LabDay1.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.LabDay1.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.LabDay1.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.LabDay1.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.LabDay1.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.LabDay1.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.LabDay1.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.LabDay1.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        nvo.LabDay1.InfectionObserved = (string) DALHelper.HandleDBNull(reader["InfectionObserved"]);
                        nvo.LabDay1.TotNoOfOocytes = (int) DALHelper.HandleDBNull(reader["TotNoOfOocytes"]);
                        nvo.LabDay1.TotNoOf2PN = (int) DALHelper.HandleDBNull(reader["TotNoOf2PN"]);
                        nvo.LabDay1.TotNoOf3PN = (int) DALHelper.HandleDBNull(reader["TotNoOf3PN"]);
                        nvo.LabDay1.TotNoOf2PB = (int) DALHelper.HandleDBNull(reader["TotNoOf2PB"]);
                        nvo.LabDay1.ToNoOfMI = (int) DALHelper.HandleDBNull(reader["ToNoOfMI"]);
                        nvo.LabDay1.ToNoOfMII = (int) DALHelper.HandleDBNull(reader["ToNoOfMII"]);
                        nvo.LabDay1.ToNoOfGV = (int) DALHelper.HandleDBNull(reader["ToNoOfGV"]);
                        nvo.LabDay1.ToNoOfDeGenerated = (int) DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]);
                        nvo.LabDay1.ToNoOfLost = (int) DALHelper.HandleDBNull(reader["ToNoOfLost"]);
                        nvo.LabDay1.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.LabDay1.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
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

        public override IValueObject GetFemaleLabDay1MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay1MediaAndCalcDetailsBizActionVO nvo = valueObject as clsGetDay1MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetail");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDayCalDetails == null)
                    {
                        nvo.LabDayCalDetails = new clsFemaleLabDay1CalculateDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.LabDayCalDetails.FertilizationID = (long) DALHelper.HandleDBNull(reader["DetailID"]);
                        nvo.LabDayCalDetails.TwoPNClosed = (bool) DALHelper.HandleDBNull(reader["TwoPNClosed"]);
                        nvo.LabDayCalDetails.TwoPNSeparated = (bool) DALHelper.HandleDBNull(reader["TwoPNSeparated"]);
                        nvo.LabDayCalDetails.NucleoliAlign = (bool) DALHelper.HandleDBNull(reader["NucleoliAlign"]);
                        nvo.LabDayCalDetails.BeginningAlign = (bool) DALHelper.HandleDBNull(reader["BeginningAlign"]);
                        nvo.LabDayCalDetails.Scattered = (bool) DALHelper.HandleDBNull(reader["Scattered"]);
                        nvo.LabDayCalDetails.CytoplasmHetero = (bool) DALHelper.HandleDBNull(reader["CytoplasmHetero"]);
                        nvo.LabDayCalDetails.CytoplasmHomo = (bool) DALHelper.HandleDBNull(reader["CytoplasmHomo"]);
                        nvo.LabDayCalDetails.NuclearMembrane = (bool) DALHelper.HandleDBNull(reader["NuclearMembrane"]);
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

        public override IValueObject GetFemaleLabDay2(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2DetailsBizActionVO nvo = valueObject as clsGetDay2DetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay2");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day2);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDay2 == null)
                    {
                        nvo.LabDay2 = new clsFemaleLabDay2VO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.LabDay2.FertilizationAssesmentDetails == null)
                            {
                                nvo.LabDay2.FertilizationAssesmentDetails = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.LabDay2.SemenDetails == null)
                                    {
                                        nvo.LabDay2.SemenDetails = new clsFemaleSemenDetailsVO();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.LabDay2.FUSetting == null)
                                            {
                                                nvo.LabDay2.FUSetting = new List<FileUpload>();
                                            }
                                            while (reader.Read())
                                            {
                                                FileUpload upload = new FileUpload {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    Index = (int) DALHelper.HandleDBNull(reader["FileIndex"]),
                                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                                    Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                                };
                                                nvo.LabDay2.FUSetting.Add(upload);
                                            }
                                            break;
                                        }
                                        nvo.LabDay2.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                                        nvo.LabDay2.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                                        nvo.LabDay2.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                                        nvo.LabDay2.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                                        nvo.LabDay2.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                                        nvo.LabDay2.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                                        nvo.LabDay2.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                                        nvo.LabDay2.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                                        nvo.LabDay2.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                                        nvo.LabDay2.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                                        nvo.LabDay2.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                                        nvo.LabDay2.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                                        nvo.LabDay2.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                                        nvo.LabDay2.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                                        nvo.LabDay2.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                                        nvo.LabDay2.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                                        nvo.LabDay2.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                                        nvo.LabDay2.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                                        nvo.LabDay2.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                                        nvo.LabDay2.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                                    }
                                    break;
                                }
                                clsFemaleLabDay2FertilizationAssesmentVO item = new clsFemaleLabDay2FertilizationAssesmentVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                                    SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                                    Date = DALHelper.HandleDate(reader["Date"]),
                                    Time = DALHelper.HandleDate(reader["Time"]),
                                    PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                                    PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                                    SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                                    SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                                    OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                                    SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                                    SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                                    SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                                    SelectedFePlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FertilisationStage"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fertilization"])
                                    },
                                    SelectedFragmentation = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FragmentationID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fragmentation"])
                                    },
                                    SelectedBlastomereSymmetry = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["BlastomereSymmetryID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["BlastomereSymmetry"])
                                    },
                                    Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                                    PV = (bool) DALHelper.HandleDBNull(reader["PV"]),
                                    XFactor = (bool) DALHelper.HandleDBNull(reader["XFactor"]),
                                    Others = (string) DALHelper.HandleDBNull(reader["Others"]),
                                    ProceedDay3 = (bool) DALHelper.HandleDBNull(reader["ProceedDay3"]),
                                    SelectedGrade = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["GradeID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Grade"])
                                    },
                                    SelectedPlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["PlanName"])
                                    },
                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                    FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"])
                                };
                                clsBaseIVFLabDayDAL instance = GetInstance();
                                clsGetDay2MediaAndCalcDetailsBizActionVO nvo2 = new clsGetDay2MediaAndCalcDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID
                                };
                                item.Day2CalculateDetails = ((clsGetDay2MediaAndCalcDetailsBizActionVO) instance.GetFemaleLabDay2MediaAndCalDetails(nvo2, UserVo)).LabDayCalDetails;
                                clsBaseIVFLabDayDAL ydal2 = GetInstance();
                                clsGetAllDayMediaDetailsBizActionVO nvo3 = new clsGetAllDayMediaDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID,
                                    LabDay = 2
                                };
                                item.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) ydal2.GetAllDayMediaDetails(nvo3, UserVo)).MediaList;
                                nvo.LabDay2.FertilizationAssesmentDetails.Add(item);
                            }
                            break;
                        }
                        nvo.LabDay2.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.LabDay2.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.LabDay2.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.LabDay2.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.LabDay2.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.LabDay2.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.LabDay2.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.LabDay2.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.LabDay2.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.LabDay2.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.LabDay2.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        nvo.LabDay2.InfectionObserved = (string) DALHelper.HandleDBNull(reader["InfectionObserved"]);
                        nvo.LabDay2.TotNoOfOocytes = (int) DALHelper.HandleDBNull(reader["TotNoOfOocytes"]);
                        nvo.LabDay2.TotNoOf2PN = (int) DALHelper.HandleDBNull(reader["TotNoOf2PN"]);
                        nvo.LabDay2.TotNoOf3PN = (int) DALHelper.HandleDBNull(reader["TotNoOf3PN"]);
                        nvo.LabDay2.TotNoOf2PB = (int) DALHelper.HandleDBNull(reader["TotNoOf2PB"]);
                        nvo.LabDay2.ToNoOfMI = (int) DALHelper.HandleDBNull(reader["ToNoOfMI"]);
                        nvo.LabDay2.ToNoOfMII = (int) DALHelper.HandleDBNull(reader["ToNoOfMII"]);
                        nvo.LabDay2.ToNoOfGV = (int) DALHelper.HandleDBNull(reader["ToNoOfGV"]);
                        nvo.LabDay2.ToNoOfDeGenerated = (int) DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]);
                        nvo.LabDay2.ToNoOfLost = (int) DALHelper.HandleDBNull(reader["ToNoOfLost"]);
                        nvo.LabDay2.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        nvo.LabDay2.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetFemaleLabDay2MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2MediaAndCalcDetailsBizActionVO nvo = valueObject as clsGetDay2MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay2");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDayCalDetails == null)
                    {
                        nvo.LabDayCalDetails = new clsFemaleLabDay2CalculateDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.LabDayCalDetails.FertilizationID = (long) DALHelper.HandleDBNull(reader["DetailID"]);
                        nvo.LabDayCalDetails.ZonaThickness = (short) DALHelper.HandleDBNull(reader["ZonaThickness"]);
                        nvo.LabDayCalDetails.ZonaTexture = (short) DALHelper.HandleDBNull(reader["ZonaTexture"]);
                        nvo.LabDayCalDetails.BlastomereSize = (short) DALHelper.HandleDBNull(reader["BlastomereSize"]);
                        nvo.LabDayCalDetails.BlastomereShape = (short) DALHelper.HandleDBNull(reader["BlastomereShape"]);
                        nvo.LabDayCalDetails.BlastomeresVolume = (short) DALHelper.HandleDBNull(reader["BlastomeresVolume"]);
                        nvo.LabDayCalDetails.Membrane = (short) DALHelper.HandleDBNull(reader["Membrane"]);
                        nvo.LabDayCalDetails.Cytoplasm = (short) DALHelper.HandleDBNull(reader["Cytoplasm"]);
                        nvo.LabDayCalDetails.Fragmentation = (short) DALHelper.HandleDBNull(reader["Fragmentation"]);
                        nvo.LabDayCalDetails.DevelopmentRate = (short) DALHelper.HandleDBNull(reader["DevelopmentRate"]);
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

        public override IValueObject GetFemaleLabDay3(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2DetailsBizActionVO nvo = valueObject as clsGetDay2DetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay3");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day3);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDay2 == null)
                    {
                        nvo.LabDay2 = new clsFemaleLabDay2VO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.LabDay2.FertilizationAssesmentDetails == null)
                            {
                                nvo.LabDay2.FertilizationAssesmentDetails = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.LabDay2.SemenDetails == null)
                                    {
                                        nvo.LabDay2.SemenDetails = new clsFemaleSemenDetailsVO();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.LabDay2.FUSetting == null)
                                            {
                                                nvo.LabDay2.FUSetting = new List<FileUpload>();
                                            }
                                            while (reader.Read())
                                            {
                                                FileUpload upload = new FileUpload {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    Index = (int) DALHelper.HandleDBNull(reader["FileIndex"]),
                                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                                    Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                                };
                                                nvo.LabDay2.FUSetting.Add(upload);
                                            }
                                            break;
                                        }
                                        nvo.LabDay2.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                                        nvo.LabDay2.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                                        nvo.LabDay2.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                                        nvo.LabDay2.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                                        nvo.LabDay2.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                                        nvo.LabDay2.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                                        nvo.LabDay2.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                                        nvo.LabDay2.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                                        nvo.LabDay2.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                                        nvo.LabDay2.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                                        nvo.LabDay2.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                                        nvo.LabDay2.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                                        nvo.LabDay2.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                                        nvo.LabDay2.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                                        nvo.LabDay2.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                                        nvo.LabDay2.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                                        nvo.LabDay2.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                                        nvo.LabDay2.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                                        nvo.LabDay2.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                                        nvo.LabDay2.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                                    }
                                    break;
                                }
                                clsFemaleLabDay2FertilizationAssesmentVO item = new clsFemaleLabDay2FertilizationAssesmentVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                                    SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                                    Date = DALHelper.HandleDate(reader["Date"]),
                                    Time = DALHelper.HandleDate(reader["Time"]),
                                    PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                                    PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                                    SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                                    SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                                    OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                                    SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                                    SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                                    SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                                    SelectedFePlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FertilisationStage"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fertilization"])
                                    },
                                    Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                                    PV = (bool) DALHelper.HandleDBNull(reader["PV"]),
                                    XFactor = (bool) DALHelper.HandleDBNull(reader["XFactor"]),
                                    Others = (string) DALHelper.HandleDBNull(reader["Others"]),
                                    ProceedDay3 = (bool) DALHelper.HandleDBNull(reader["ProceedDay4"]),
                                    SelectedGrade = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["GradeID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Grade"])
                                    },
                                    SelectedFragmentation = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FragmentationID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fragmentation"])
                                    },
                                    SelectedBlastomereSymmetry = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["BlastomereSymmetryID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["BlastomereSymmetry"])
                                    },
                                    SelectedPlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["PlanName"])
                                    },
                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                    FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"])
                                };
                                clsBaseIVFLabDayDAL instance = GetInstance();
                                clsGetDay2MediaAndCalcDetailsBizActionVO nvo2 = new clsGetDay2MediaAndCalcDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID
                                };
                                item.Day2CalculateDetails = ((clsGetDay2MediaAndCalcDetailsBizActionVO) instance.GetFemaleLabDay3MediaAndCalDetails(nvo2, UserVo)).LabDayCalDetails;
                                clsBaseIVFLabDayDAL ydal2 = GetInstance();
                                clsGetAllDayMediaDetailsBizActionVO nvo3 = new clsGetAllDayMediaDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID,
                                    LabDay = 3
                                };
                                item.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) ydal2.GetAllDayMediaDetails(nvo3, UserVo)).MediaList;
                                nvo.LabDay2.FertilizationAssesmentDetails.Add(item);
                            }
                            break;
                        }
                        nvo.LabDay2.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.LabDay2.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.LabDay2.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.LabDay2.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.LabDay2.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.LabDay2.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.LabDay2.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.LabDay2.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.LabDay2.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.LabDay2.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.LabDay2.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        nvo.LabDay2.InfectionObserved = (string) DALHelper.HandleDBNull(reader["InfectionObserved"]);
                        nvo.LabDay2.TotNoOfOocytes = (int) DALHelper.HandleDBNull(reader["TotNoOfOocytes"]);
                        nvo.LabDay2.TotNoOf2PN = (int) DALHelper.HandleDBNull(reader["TotNoOf2PN"]);
                        nvo.LabDay2.TotNoOf3PN = (int) DALHelper.HandleDBNull(reader["TotNoOf3PN"]);
                        nvo.LabDay2.TotNoOf2PB = (int) DALHelper.HandleDBNull(reader["TotNoOf2PB"]);
                        nvo.LabDay2.ToNoOfMI = (int) DALHelper.HandleDBNull(reader["ToNoOfMI"]);
                        nvo.LabDay2.ToNoOfMII = (int) DALHelper.HandleDBNull(reader["ToNoOfMII"]);
                        nvo.LabDay2.ToNoOfGV = (int) DALHelper.HandleDBNull(reader["ToNoOfGV"]);
                        nvo.LabDay2.ToNoOfDeGenerated = (int) DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]);
                        nvo.LabDay2.ToNoOfLost = (int) DALHelper.HandleDBNull(reader["ToNoOfLost"]);
                        nvo.LabDay2.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        nvo.LabDay2.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetFemaleLabDay3MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2MediaAndCalcDetailsBizActionVO nvo = valueObject as clsGetDay2MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay3");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDayCalDetails == null)
                    {
                        nvo.LabDayCalDetails = new clsFemaleLabDay2CalculateDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.LabDayCalDetails.FertilizationID = (long) DALHelper.HandleDBNull(reader["DetailID"]);
                        nvo.LabDayCalDetails.ZonaThickness = (short) DALHelper.HandleDBNull(reader["ZonaThickness"]);
                        nvo.LabDayCalDetails.ZonaTexture = (short) DALHelper.HandleDBNull(reader["ZonaTexture"]);
                        nvo.LabDayCalDetails.BlastomereSize = (short) DALHelper.HandleDBNull(reader["BlastomereSize"]);
                        nvo.LabDayCalDetails.BlastomereShape = (short) DALHelper.HandleDBNull(reader["BlastomereShape"]);
                        nvo.LabDayCalDetails.BlastomeresVolume = (short) DALHelper.HandleDBNull(reader["BlastomeresVolume"]);
                        nvo.LabDayCalDetails.Membrane = (short) DALHelper.HandleDBNull(reader["Membrane"]);
                        nvo.LabDayCalDetails.Cytoplasm = (short) DALHelper.HandleDBNull(reader["Cytoplasm"]);
                        nvo.LabDayCalDetails.Fragmentation = (short) DALHelper.HandleDBNull(reader["Fragmentation"]);
                        nvo.LabDayCalDetails.DevelopmentRate = (short) DALHelper.HandleDBNull(reader["DevelopmentRate"]);
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

        public override IValueObject GetFemaleLabDay4(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay4DetailsBizActionVO nvo = valueObject as clsGetDay4DetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay4");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day4);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDay4 == null)
                    {
                        nvo.LabDay4 = new clsFemaleLabDay4VO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.LabDay4.FertilizationAssesmentDetails == null)
                            {
                                nvo.LabDay4.FertilizationAssesmentDetails = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.LabDay4.SemenDetails == null)
                                    {
                                        nvo.LabDay4.SemenDetails = new clsFemaleSemenDetailsVO();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.LabDay4.FUSetting == null)
                                            {
                                                nvo.LabDay4.FUSetting = new List<FileUpload>();
                                            }
                                            while (reader.Read())
                                            {
                                                FileUpload upload = new FileUpload {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    Index = (int) DALHelper.HandleDBNull(reader["FileIndex"]),
                                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                                    Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                                };
                                                nvo.LabDay4.FUSetting.Add(upload);
                                            }
                                            break;
                                        }
                                        nvo.LabDay4.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                                        nvo.LabDay4.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                                        nvo.LabDay4.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                                        nvo.LabDay4.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                                        nvo.LabDay4.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                                        nvo.LabDay4.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                                        nvo.LabDay4.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                                        nvo.LabDay4.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                                        nvo.LabDay4.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                                        nvo.LabDay4.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                                        nvo.LabDay4.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                                        nvo.LabDay4.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                                        nvo.LabDay4.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                                        nvo.LabDay4.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                                        nvo.LabDay4.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                                        nvo.LabDay4.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                                        nvo.LabDay4.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                                        nvo.LabDay4.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                                        nvo.LabDay4.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                                        nvo.LabDay4.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                                    }
                                    break;
                                }
                                clsFemaleLabDay4FertilizationAssesmentVO item = new clsFemaleLabDay4FertilizationAssesmentVO {
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                                    SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                                    Date = DALHelper.HandleDate(reader["Date"]),
                                    Time = DALHelper.HandleDate(reader["Time"]),
                                    PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                                    PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                                    SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                                    SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                                    OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                                    SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                                    SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                                    SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                                    SelectedFePlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FertilisationStage"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fertilization"])
                                    },
                                    Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                                    PV = (bool) DALHelper.HandleDBNull(reader["PV"]),
                                    XFactor = (bool) DALHelper.HandleDBNull(reader["XFactor"]),
                                    Others = (string) DALHelper.HandleDBNull(reader["Others"]),
                                    ProceedDay5 = (bool) DALHelper.HandleDBNull(reader["ProceedDay5"]),
                                    SelectedGrade = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["GradeID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Grade"])
                                    },
                                    SelectedFragmentation = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FragmentationID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fragmentation"])
                                    },
                                    SelectedBlastomereSymmetry = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["BlastomereSymmetryID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["BlastomereSymmetry"])
                                    },
                                    SelectedPlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["PlanName"])
                                    },
                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                    FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"])
                                };
                                clsBaseIVFLabDayDAL instance = GetInstance();
                                clsGetDay4MediaAndCalcDetailsBizActionVO nvo2 = new clsGetDay4MediaAndCalcDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID
                                };
                                item.Day4CalculateDetails = ((clsGetDay4MediaAndCalcDetailsBizActionVO) instance.GetFemaleLabDay4MediaAndCalDetails(nvo2, UserVo)).LabDayCalDetails;
                                clsBaseIVFLabDayDAL ydal2 = GetInstance();
                                clsGetAllDayMediaDetailsBizActionVO nvo3 = new clsGetAllDayMediaDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID,
                                    LabDay = 4
                                };
                                item.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) ydal2.GetAllDayMediaDetails(nvo3, UserVo)).MediaList;
                                nvo.LabDay4.FertilizationAssesmentDetails.Add(item);
                            }
                            break;
                        }
                        nvo.LabDay4.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.LabDay4.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.LabDay4.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.LabDay4.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.LabDay4.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.LabDay4.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.LabDay4.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.LabDay4.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.LabDay4.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.LabDay4.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.LabDay4.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        nvo.LabDay4.InfectionObserved = (string) DALHelper.HandleDBNull(reader["InfectionObserved"]);
                        nvo.LabDay4.TotNoOfOocytes = (int) DALHelper.HandleDBNull(reader["TotNoOfOocytes"]);
                        nvo.LabDay4.TotNoOf2PN = (int) DALHelper.HandleDBNull(reader["TotNoOf2PN"]);
                        nvo.LabDay4.TotNoOf3PN = (int) DALHelper.HandleDBNull(reader["TotNoOf3PN"]);
                        nvo.LabDay4.TotNoOf2PB = (int) DALHelper.HandleDBNull(reader["TotNoOf2PB"]);
                        nvo.LabDay4.ToNoOfMI = (int) DALHelper.HandleDBNull(reader["ToNoOfMI"]);
                        nvo.LabDay4.ToNoOfMII = (int) DALHelper.HandleDBNull(reader["ToNoOfMII"]);
                        nvo.LabDay4.ToNoOfGV = (int) DALHelper.HandleDBNull(reader["ToNoOfGV"]);
                        nvo.LabDay4.ToNoOfDeGenerated = (int) DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]);
                        nvo.LabDay4.ToNoOfLost = (int) DALHelper.HandleDBNull(reader["ToNoOfLost"]);
                        nvo.LabDay4.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        nvo.LabDay4.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetFemaleLabDay4MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay4MediaAndCalcDetailsBizActionVO nvo = valueObject as clsGetDay4MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay4");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDayCalDetails == null)
                    {
                        nvo.LabDayCalDetails = new clsFemaleLabDay4CalculateDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.LabDayCalDetails.FertilizationID = (long) DALHelper.HandleDBNull(reader["DetailID"]);
                        nvo.LabDayCalDetails.Compaction = (bool) DALHelper.HandleDBNull(reader["Compaction"]);
                        nvo.LabDayCalDetails.SignsOfBlastocoel = (bool) DALHelper.HandleDBNull(reader["SignsOfBlastocoel"]);
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

        public override IValueObject GetFemaleLabDay5(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay5DetailsBizActionVO nvo = valueObject as clsGetDay5DetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay5");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day5);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDay5 == null)
                    {
                        nvo.LabDay5 = new clsFemaleLabDay5VO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.LabDay5.FertilizationAssesmentDetails == null)
                            {
                                nvo.LabDay5.FertilizationAssesmentDetails = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.LabDay5.SemenDetails == null)
                                    {
                                        nvo.LabDay5.SemenDetails = new clsFemaleSemenDetailsVO();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.LabDay5.FUSetting == null)
                                            {
                                                nvo.LabDay5.FUSetting = new List<FileUpload>();
                                            }
                                            while (reader.Read())
                                            {
                                                FileUpload upload = new FileUpload {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    Index = (int) DALHelper.HandleDBNull(reader["FileIndex"]),
                                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                                    Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                                };
                                                nvo.LabDay5.FUSetting.Add(upload);
                                            }
                                            break;
                                        }
                                        nvo.LabDay5.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                                        nvo.LabDay5.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                                        nvo.LabDay5.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                                        nvo.LabDay5.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                                        nvo.LabDay5.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                                        nvo.LabDay5.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                                        nvo.LabDay5.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                                        nvo.LabDay5.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                                        nvo.LabDay5.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                                        nvo.LabDay5.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                                        nvo.LabDay5.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                                        nvo.LabDay5.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                                        nvo.LabDay5.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                                        nvo.LabDay5.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                                        nvo.LabDay5.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                                        nvo.LabDay5.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                                        nvo.LabDay5.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                                        nvo.LabDay5.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                                        nvo.LabDay5.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                                        nvo.LabDay5.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                                    }
                                    break;
                                }
                                clsFemaleLabDay5FertilizationAssesmentVO item = new clsFemaleLabDay5FertilizationAssesmentVO {
                                    OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                                    SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    Date = DALHelper.HandleDate(reader["Date"]),
                                    Time = DALHelper.HandleDate(reader["Time"]),
                                    PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                                    PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                                    SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                                    SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                                    OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                                    SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                                    SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                                    SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                                    SelectedFePlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FertilisationStage"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fertilization"])
                                    },
                                    Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                                    PV = (bool) DALHelper.HandleDBNull(reader["PV"]),
                                    XFactor = (bool) DALHelper.HandleDBNull(reader["XFactor"]),
                                    Others = (string) DALHelper.HandleDBNull(reader["Others"]),
                                    ProceedDay6 = (bool) DALHelper.HandleDBNull(reader["ProceedDay6"]),
                                    SelectedGrade = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["GradeID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Grade"])
                                    },
                                    SelectedBlastocytsGrade = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["BlastocytsGradeID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["BlastocytsGrade"])
                                    },
                                    SelectedICM = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["ICMID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["ICM"])
                                    },
                                    SelectedExpansion = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["ExpansionID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Expansion"])
                                    },
                                    SelectedTrophectoderm = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["TrophectodermID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Trophectoderm"])
                                    },
                                    SelectedPlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["PlanName"])
                                    },
                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                    FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"])
                                };
                                clsBaseIVFLabDayDAL instance = GetInstance();
                                clsGetDay5MediaAndCalcDetailsBizActionVO nvo2 = new clsGetDay5MediaAndCalcDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID
                                };
                                item.Day5CalculateDetails = ((clsGetDay5MediaAndCalcDetailsBizActionVO) instance.GetFemaleLabDay5MediaAndCalDetails(nvo2, UserVo)).LabDayCalDetails;
                                clsBaseIVFLabDayDAL ydal2 = GetInstance();
                                clsGetAllDayMediaDetailsBizActionVO nvo3 = new clsGetAllDayMediaDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID,
                                    LabDay = 5
                                };
                                item.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) ydal2.GetAllDayMediaDetails(nvo3, UserVo)).MediaList;
                                nvo.LabDay5.FertilizationAssesmentDetails.Add(item);
                            }
                            break;
                        }
                        nvo.LabDay5.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.LabDay5.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.LabDay5.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.LabDay5.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.LabDay5.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.LabDay5.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.LabDay5.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.LabDay5.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.LabDay5.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.LabDay5.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.LabDay5.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        nvo.LabDay5.InfectionObserved = (string) DALHelper.HandleDBNull(reader["InfectionObserved"]);
                        nvo.LabDay5.TotNoOfOocytes = (int) DALHelper.HandleDBNull(reader["TotNoOfOocytes"]);
                        nvo.LabDay5.TotNoOf2PN = (int) DALHelper.HandleDBNull(reader["TotNoOf2PN"]);
                        nvo.LabDay5.TotNoOf3PN = (int) DALHelper.HandleDBNull(reader["TotNoOf3PN"]);
                        nvo.LabDay5.TotNoOf2PB = (int) DALHelper.HandleDBNull(reader["TotNoOf2PB"]);
                        nvo.LabDay5.ToNoOfMI = (int) DALHelper.HandleDBNull(reader["ToNoOfMI"]);
                        nvo.LabDay5.ToNoOfMII = (int) DALHelper.HandleDBNull(reader["ToNoOfMII"]);
                        nvo.LabDay5.ToNoOfGV = (int) DALHelper.HandleDBNull(reader["ToNoOfGV"]);
                        nvo.LabDay5.ToNoOfDeGenerated = (int) DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]);
                        nvo.LabDay5.ToNoOfLost = (int) DALHelper.HandleDBNull(reader["ToNoOfLost"]);
                        nvo.LabDay5.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        nvo.LabDay5.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetFemaleLabDay5MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay5MediaAndCalcDetailsBizActionVO nvo = valueObject as clsGetDay5MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay5");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDayCalDetails == null)
                    {
                        nvo.LabDayCalDetails = new clsFemaleLabDay5CalculateDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.LabDayCalDetails.FertilizationID = (long) DALHelper.HandleDBNull(reader["DetailID"]);
                        nvo.LabDayCalDetails.BlastocoelsCavity = (bool) DALHelper.HandleDBNull(reader["BlastocoelsCavity"]);
                        nvo.LabDayCalDetails.TightlyPackedCells = (bool) DALHelper.HandleDBNull(reader["TightlyPackedCells"]);
                        nvo.LabDayCalDetails.FormingEpithelium = (bool) DALHelper.HandleDBNull(reader["FormingEpithelium"]);
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

        public override IValueObject GetFemaleLabDay6(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay6DetailsBizActionVO nvo = valueObject as clsGetDay6DetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay6");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDay", DbType.Int16, IVFLabDay.Day6);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.LabDay6 == null)
                    {
                        nvo.LabDay6 = new clsFemaleLabDay6VO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.LabDay6.FertilizationAssesmentDetails == null)
                            {
                                nvo.LabDay6.FertilizationAssesmentDetails = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.LabDay6.SemenDetails == null)
                                    {
                                        nvo.LabDay6.SemenDetails = new clsFemaleSemenDetailsVO();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.LabDay6.FUSetting == null)
                                            {
                                                nvo.LabDay6.FUSetting = new List<FileUpload>();
                                            }
                                            while (reader.Read())
                                            {
                                                FileUpload upload = new FileUpload {
                                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                    Index = (int) DALHelper.HandleDBNull(reader["FileIndex"]),
                                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                                    Data = (byte[]) DALHelper.HandleDBNull(reader["Value"])
                                                };
                                                nvo.LabDay6.FUSetting.Add(upload);
                                            }
                                            break;
                                        }
                                        nvo.LabDay6.SemenDetails.MOSP = (string) DALHelper.HandleDBNull(reader["MOSP"]);
                                        nvo.LabDay6.SemenDetails.SOS = (string) DALHelper.HandleDBNull(reader["SOS"]);
                                        nvo.LabDay6.SemenDetails.MethodOfSpermPreparation = (long) DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]);
                                        nvo.LabDay6.SemenDetails.SourceOfSemen = (long) DALHelper.HandleDBNull(reader["SourceOfSemen"]);
                                        nvo.LabDay6.SemenDetails.PreSelfVolume = (string) DALHelper.HandleDBNull(reader["PreSelfVolume"]);
                                        nvo.LabDay6.SemenDetails.PreSelfConcentration = (string) DALHelper.HandleDBNull(reader["PreSelfConcentration"]);
                                        nvo.LabDay6.SemenDetails.PreSelfMotality = (string) DALHelper.HandleDBNull(reader["PreSelfMotality"]);
                                        nvo.LabDay6.SemenDetails.PreSelfWBC = (string) DALHelper.HandleDBNull(reader["PreSelfWBC"]);
                                        nvo.LabDay6.SemenDetails.PreDonorVolume = (string) DALHelper.HandleDBNull(reader["PreDonorVolume"]);
                                        nvo.LabDay6.SemenDetails.PreDonorConcentration = (string) DALHelper.HandleDBNull(reader["PreDonorConcentration"]);
                                        nvo.LabDay6.SemenDetails.PreDonorMotality = (string) DALHelper.HandleDBNull(reader["PreDonorMotality"]);
                                        nvo.LabDay6.SemenDetails.PreDonorWBC = (string) DALHelper.HandleDBNull(reader["PreDonorWBC"]);
                                        nvo.LabDay6.SemenDetails.PostSelfVolume = (string) DALHelper.HandleDBNull(reader["PostSelfVolume"]);
                                        nvo.LabDay6.SemenDetails.PostSelfConcentration = (string) DALHelper.HandleDBNull(reader["PostSelfConcentration"]);
                                        nvo.LabDay6.SemenDetails.PostSelfMotality = (string) DALHelper.HandleDBNull(reader["PostSelfMotality"]);
                                        nvo.LabDay6.SemenDetails.PostSelfWBC = (string) DALHelper.HandleDBNull(reader["PostSelfWBC"]);
                                        nvo.LabDay6.SemenDetails.PostDonorVolume = (string) DALHelper.HandleDBNull(reader["PostDonorVolume"]);
                                        nvo.LabDay6.SemenDetails.PostDonorConcentration = (string) DALHelper.HandleDBNull(reader["PostDonorConcentration"]);
                                        nvo.LabDay6.SemenDetails.PostDonorMotality = (string) DALHelper.HandleDBNull(reader["PostDonorMotality"]);
                                        nvo.LabDay6.SemenDetails.PostDonorWBC = (string) DALHelper.HandleDBNull(reader["PostDonorWBC"]);
                                    }
                                    break;
                                }
                                clsFemaleLabDay6FertilizationAssesmentVO item = new clsFemaleLabDay6FertilizationAssesmentVO {
                                    OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                                    SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                                    ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                    Date = DALHelper.HandleDate(reader["Date"]),
                                    Time = DALHelper.HandleDate(reader["Time"]),
                                    PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                                    PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                                    SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                                    SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                                    OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                                    SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                                    SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                                    SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                                    SelectedFePlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["FertilisationStage"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Fertilization"])
                                    },
                                    Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                                    PV = (bool) DALHelper.HandleDBNull(reader["PV"]),
                                    XFactor = (bool) DALHelper.HandleDBNull(reader["XFactor"]),
                                    Others = (string) DALHelper.HandleDBNull(reader["Others"]),
                                    ProceedDay7 = (bool) DALHelper.HandleDBNull(reader["ProceedDay7"]),
                                    SelectedGrade = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["GradeID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["Grade"])
                                    },
                                    SelectedPlan = { 
                                        ID = (long) DALHelper.HandleDBNull(reader["PlanID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader["PlanName"])
                                    },
                                    FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                    FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"])
                                };
                                clsBaseIVFLabDayDAL instance = GetInstance();
                                clsGetAllDayMediaDetailsBizActionVO nvo2 = new clsGetAllDayMediaDetailsBizActionVO {
                                    ID = nvo.ID,
                                    DetailID = item.ID,
                                    LabDay = 6
                                };
                                item.MediaDetails = ((clsGetAllDayMediaDetailsBizActionVO) instance.GetAllDayMediaDetails(nvo2, UserVo)).MediaList;
                                nvo.LabDay6.FertilizationAssesmentDetails.Add(item);
                            }
                            break;
                        }
                        nvo.LabDay6.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.LabDay6.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.LabDay6.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        nvo.LabDay6.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        nvo.LabDay6.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.LabDay6.Time = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.LabDay6.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.LabDay6.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        nvo.LabDay6.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.LabDay6.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.LabDay6.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        nvo.LabDay6.InfectionObserved = (string) DALHelper.HandleDBNull(reader["InfectionObserved"]);
                        nvo.LabDay6.TotNoOfOocytes = (int) DALHelper.HandleDBNull(reader["TotNoOfOocytes"]);
                        nvo.LabDay6.TotNoOf2PN = (int) DALHelper.HandleDBNull(reader["TotNoOf2PN"]);
                        nvo.LabDay6.TotNoOf3PN = (int) DALHelper.HandleDBNull(reader["TotNoOf3PN"]);
                        nvo.LabDay6.TotNoOf2PB = (int) DALHelper.HandleDBNull(reader["TotNoOf2PB"]);
                        nvo.LabDay6.ToNoOfMI = (int) DALHelper.HandleDBNull(reader["ToNoOfMI"]);
                        nvo.LabDay6.ToNoOfMII = (int) DALHelper.HandleDBNull(reader["ToNoOfMII"]);
                        nvo.LabDay6.ToNoOfGV = (int) DALHelper.HandleDBNull(reader["ToNoOfGV"]);
                        nvo.LabDay6.ToNoOfDeGenerated = (int) DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]);
                        nvo.LabDay6.ToNoOfLost = (int) DALHelper.HandleDBNull(reader["ToNoOfLost"]);
                        nvo.LabDay6.IsFreezed = (bool) DALHelper.HandleDBNull(reader["IsFreezed"]);
                        nvo.LabDay6.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetLab5And6MasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetLab5_6MasterListBizActionVO nvo = (clsGetLab5_6MasterListBizActionVO) valueObject;
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
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIVFMasterList");
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
                        nvo.MasterList.Add(new MasterListItem((long) reader["ID"], reader["Code"].ToString(), reader["Description"].ToString(), reader["Name"].ToString(), reader["Flag"].ToString(), (bool) reader["Status"]));
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

        public override IValueObject GetLabDay0ForDay1(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay0ForLabDay1BizActionVO nvo = valueObject as clsGetLabDay0ForLabDay1BizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay0ForLabDay1");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day1Details == null)
                    {
                        nvo.Day1Details = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay1FertilizationAssesmentVO item = new clsFemaleLabDay1FertilizationAssesmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DetailID"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                            PlanTreatmentID = (int) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"]),
                            PlanTherapyId = (long) DALHelper.HandleDBNull(reader["PlanTherapyId"])
                        };
                        nvo.Day1Details.Add(item);
                        nvo.AnaesthetistID = (long) DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        nvo.AssAnaesthetistID = (long) DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        nvo.AssEmbryologistID = (long) DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        nvo.EmbryologistID = (long) DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetLabDay1ForDay2(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay1ForLabDay2BizActionVO nvo = valueObject as clsGetLabDay1ForLabDay2BizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay1ForLabDay2");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day2Details == null)
                    {
                        nvo.Day2Details = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay2FertilizationAssesmentVO item = new clsFemaleLabDay2FertilizationAssesmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DetailID"]),
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"])
                        };
                        nvo.Day2Details.Add(item);
                        nvo.AnaesthetistID = (long) DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        nvo.AssAnaesthetistID = (long) DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        nvo.AssEmbryologistID = (long) DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        nvo.EmbryologistID = (long) DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetLabDay1Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay1ScoreBizActionVO nvo = valueObject as clsGetDay1ScoreBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleDay1Score");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day0Score == null)
                    {
                        nvo.Day0Score = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay1FertilizationAssesmentVO item = new clsFemaleLabDay1FertilizationAssesmentVO {
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                            SelectedGrade = { Description = (string) DALHelper.HandleDBNull(reader["Grade"]) },
                            SelectedFePlan = { Description = (string) DALHelper.HandleDBNull(reader["FertilisationStage"]) },
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"])
                        };
                        nvo.Day0Score.Add(item);
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

        public override IValueObject GetLabDay2ForDay3(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay1ForLabDay2BizActionVO nvo = valueObject as clsGetLabDay1ForLabDay2BizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay1ForLabDay3");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day2Details == null)
                    {
                        nvo.Day2Details = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay2FertilizationAssesmentVO item = new clsFemaleLabDay2FertilizationAssesmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DetailID"]),
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"])
                        };
                        nvo.Day2Details.Add(item);
                        nvo.AnaesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        nvo.AssAnaesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        nvo.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
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

        public override IValueObject GetLabDay2Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay3ScoreBizActionVO nvo = valueObject as clsGetDay3ScoreBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleDay2Score");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day3Score == null)
                    {
                        nvo.Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO item = new clsFemaleLabDay4FertilizationAssesmentVO {
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                            SelectedGrade = { Description = (string) DALHelper.HandleDBNull(reader["Grade"]) },
                            SelectedFePlan = { Description = (string) DALHelper.HandleDBNull(reader["FertilisationStage"]) },
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"])
                        };
                        nvo.Day3Score.Add(item);
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

        public override IValueObject GetLabDay3ForDay4(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay3ForLabDay4BizActionVO nvo = valueObject as clsGetLabDay3ForLabDay4BizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay3ForLabDay4");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day4Details == null)
                    {
                        nvo.Day4Details = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO item = new clsFemaleLabDay4FertilizationAssesmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DetailID"]),
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"])
                        };
                        nvo.Day4Details.Add(item);
                        nvo.AnaesthetistID = (long) DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        nvo.AssAnaesthetistID = (long) DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        nvo.AssEmbryologistID = (long) DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        nvo.EmbryologistID = (long) DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetLabDay3Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay3ScoreBizActionVO nvo = valueObject as clsGetDay3ScoreBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleDay3Score");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day3Score == null)
                    {
                        nvo.Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO item = new clsFemaleLabDay4FertilizationAssesmentVO {
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                            SelectedGrade = { Description = (string) DALHelper.HandleDBNull(reader["Grade"]) },
                            SelectedFePlan = { Description = (string) DALHelper.HandleDBNull(reader["FertilisationStage"]) },
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"])
                        };
                        nvo.Day3Score.Add(item);
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

        public override IValueObject GetLabDay4ForDay5(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay4ForLabDay5BizActionVO nvo = valueObject as clsGetLabDay4ForLabDay5BizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay4ForLabDay5");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day5Details == null)
                    {
                        nvo.Day5Details = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay5FertilizationAssesmentVO item = new clsFemaleLabDay5FertilizationAssesmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DetailID"]),
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"])
                        };
                        nvo.Day5Details.Add(item);
                        nvo.AnaesthetistID = (long) DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        nvo.AssAnaesthetistID = (long) DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        nvo.AssEmbryologistID = (long) DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        nvo.EmbryologistID = (long) DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetLabDay4Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay4ScoreBizActionVO nvo = valueObject as clsGetDay4ScoreBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleDay4Score");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day4Score == null)
                    {
                        nvo.Day4Score = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay5FertilizationAssesmentVO item = new clsFemaleLabDay5FertilizationAssesmentVO {
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                            SelectedGrade = { Description = (string) DALHelper.HandleDBNull(reader["Grade"]) },
                            SelectedFePlan = { Description = (string) DALHelper.HandleDBNull(reader["FertilisationStage"]) },
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"])
                        };
                        nvo.Day4Score.Add(item);
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

        public override IValueObject GetLabDay5ForDay6(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay5ForLabDay6BizActionVO nvo = valueObject as clsGetLabDay5ForLabDay6BizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay5ForLabDay6");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day6Details == null)
                    {
                        nvo.Day6Details = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay6FertilizationAssesmentVO item = new clsFemaleLabDay6FertilizationAssesmentVO {
                            ID = (long) DALHelper.HandleDBNull(reader["DetailID"]),
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            SerialOccyteNo = (long) DALHelper.HandleDBNull(reader["SerialOccyteNo"]),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            Time = DALHelper.HandleDate(reader["Time"]),
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"]),
                            PlanTreatment = (string) DALHelper.HandleDBNull(reader["Protocol"]),
                            SrcOocyteID = (long) DALHelper.HandleDBNull(reader["SrcOocyteID"]),
                            SrcOocyteDescription = (string) DALHelper.HandleDBNull(reader["SOOocyte"]),
                            OocyteDonorID = (string) DALHelper.HandleDBNull(reader["OSCode"]),
                            SrcOfSemen = (long) DALHelper.HandleDBNull(reader["SrcOfSemen"]),
                            SrcOfSemenDescription = (string) DALHelper.HandleDBNull(reader["SOSemen"]),
                            SemenDonorID = (string) DALHelper.HandleDBNull(reader["SSCode"])
                        };
                        nvo.Day6Details.Add(item);
                        nvo.AnaesthetistID = (long) DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        nvo.AssAnaesthetistID = (long) DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        nvo.AssEmbryologistID = (long) DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        nvo.EmbryologistID = (long) DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetLabDay5Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay5ScoreBizActionVO nvo = valueObject as clsGetDay5ScoreBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFemaleDay5Score");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, nvo.CoupleUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day5Score == null)
                    {
                        nvo.Day5Score = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
                    }
                    while (reader.Read())
                    {
                        clsFemaleLabDay6FertilizationAssesmentVO item = new clsFemaleLabDay6FertilizationAssesmentVO {
                            OoNo = (long) DALHelper.HandleDBNull(reader["OoNo"]),
                            Score = (long) DALHelper.HandleDBNull(reader["Score"]),
                            SelectedGrade = { Description = (string) DALHelper.HandleDBNull(reader["Grade"]) },
                            SelectedFePlan = { Description = (string) DALHelper.HandleDBNull(reader["FertilisationStage"]) },
                            PlanTreatmentID = (long) DALHelper.HandleDBNull(reader["PlanTreatmentID"])
                        };
                        nvo.Day5Score.Add(item);
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

        private clsAddLabDay1BizActionVO UpdateDay1(clsAddLabDay1BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay1VO dayvo = BizActionObj.Day1Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay1");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dayvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dayvo.UnitID = UserVo.UserLoginInfo.UnitId;
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((BizActionObj.Day1Details.ObservationDetails != null) && (BizActionObj.Day1Details.ObservationDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab1ObservatonDetails");
                    this.dbServer.AddInParameter(command2, "Day1ID", DbType.Int64, dayvo.ID);
                    this.dbServer.ExecuteNonQuery(command2);
                }
                if ((BizActionObj.Day1Details.ObservationDetails != null) && (BizActionObj.Day1Details.ObservationDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay1InseminationPlatesVO svo in dayvo.ObservationDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1ObservationDetails");
                        this.dbServer.AddInParameter(command3, "Day1ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, svo.Time);
                        this.dbServer.AddInParameter(command3, "HrAtIns", DbType.Double, svo.HrAtIns);
                        this.dbServer.AddInParameter(command3, "Observation", DbType.String, svo.Observation);
                        this.dbServer.AddInParameter(command3, "FertiCheckPeriod", DbType.String, svo.FertiCheckPeriod);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        svo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                    }
                }
                if ((BizActionObj.Day1Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day1Details.FertilizationAssesmentDetails.Count > 0))
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab1Details");
                    this.dbServer.AddInParameter(command4, "Day1ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command4, "LabDay", DbType.Int32, IVFLabDay.Day1);
                    this.dbServer.ExecuteNonQuery(command4);
                }
                if ((BizActionObj.Day1Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day1Details.FertilizationAssesmentDetails.Count > 0))
                {
                    List<clsEmbryoTransferDetailsVO> list1 = new List<clsEmbryoTransferDetailsVO>();
                    foreach (clsFemaleLabDay1FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1Details");
                        this.dbServer.AddInParameter(command5, "Day1ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command5, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command5, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command5, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command5, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command5, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command5, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command5, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command5, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command5, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command5, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command5, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command5, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command5, "Grade", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command5, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command5, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command5, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command5, "Others", DbType.String, tvo.Others);
                        this.dbServer.AddInParameter(command5, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command5, "ProceedDay2", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command5, "ProceedDay2", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command5, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command5, "FileContents", DbType.Binary, tvo.FileContents);
                        this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command5, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo2 in tvo.MediaDetails)
                            {
                                DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command6, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "Date", DbType.DateTime, svo2.Date);
                                this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day1);
                                this.dbServer.AddInParameter(command6, "MediaName", DbType.String, svo2.ItemName);
                                this.dbServer.AddInParameter(command6, "Company", DbType.String, svo2.Company);
                                this.dbServer.AddInParameter(command6, "LotNo", DbType.String, svo2.BatchCode);
                                this.dbServer.AddInParameter(command6, "ExpiryDate", DbType.DateTime, svo2.ExpiryDate);
                                this.dbServer.AddInParameter(command6, "PH", DbType.Boolean, svo2.PH);
                                this.dbServer.AddInParameter(command6, "OSM", DbType.Boolean, svo2.OSM);
                                this.dbServer.AddInParameter(command6, "VolumeUsed", DbType.String, svo2.VolumeUsed);
                                this.dbServer.AddInParameter(command6, "Status", DbType.Int64, svo2.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command6, "BatchID", DbType.Int64, svo2.BatchID);
                                this.dbServer.AddInParameter(command6, "StoreID", DbType.Int64, svo2.StoreID);
                                this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                                this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command6, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                                svo2.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                            }
                        }
                        if (tvo.CalculateDetails != null)
                        {
                            clsFemaleLabDay1CalculateDetailsVO calculateDetails = tvo.CalculateDetails;
                            DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay1CalculateDetails");
                            this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command7, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command7, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command7, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command7, "TwoPNClosed", DbType.Boolean, calculateDetails.TwoPNClosed);
                            this.dbServer.AddInParameter(command7, "TwoPNSeparated", DbType.Boolean, calculateDetails.TwoPNSeparated);
                            this.dbServer.AddInParameter(command7, "NucleoliAlign", DbType.Boolean, calculateDetails.NucleoliAlign);
                            this.dbServer.AddInParameter(command7, "BeginningAlign", DbType.Boolean, calculateDetails.BeginningAlign);
                            this.dbServer.AddInParameter(command7, "Scattered", DbType.Boolean, calculateDetails.Scattered);
                            this.dbServer.AddInParameter(command7, "CytoplasmHetero", DbType.Boolean, calculateDetails.CytoplasmHetero);
                            this.dbServer.AddInParameter(command7, "CytoplasmHomo", DbType.Boolean, calculateDetails.CytoplasmHomo);
                            this.dbServer.AddInParameter(command7, "NuclearMembrane", DbType.Boolean, calculateDetails.NuclearMembrane);
                            this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, calculateDetails.ID);
                            this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command7, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                            calculateDetails.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day1Details.FUSetting != null) && (BizActionObj.Day1Details.FUSetting.Count > 0))
                {
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab1UploadFileDetails");
                    this.dbServer.AddInParameter(command8, "Day1ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command8, "LabDay", DbType.Int32, IVFLabDay.Day1);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, dayvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command8);
                }
                if ((BizActionObj.Day1Details.FUSetting != null) && (BizActionObj.Day1Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command9, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command9, "LabDay", DbType.Int16, IVFLabDay.Day1);
                        this.dbServer.AddInParameter(command9, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command9, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command9, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command9, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command9, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                    }
                }
                if (BizActionObj.Day1Details.SemenDetails != null)
                {
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab1SemenDetails");
                    this.dbServer.AddInParameter(command10, "Day1ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command10, "LabDay", DbType.Int32, IVFLabDay.Day1);
                    this.dbServer.ExecuteNonQuery(command10);
                }
                if (BizActionObj.Day1Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day1Details.SemenDetails;
                    DbCommand command11 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command11, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command11, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command11, "LabDay", DbType.Int16, IVFLabDay.Day1);
                    this.dbServer.AddInParameter(command11, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day1Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command11, "SourceOfSemen", DbType.Int64, BizActionObj.Day1Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command11, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command11, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command11, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command11, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command11, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command11, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command11, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command11, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command11, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command11, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command11, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command11, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command11, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command11, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command11, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command11, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command11, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command11, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command11, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command11, "ID");
                }
                if (BizActionObj.Day1Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day1Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day1Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day1Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day1Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay1;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day1Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day1Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day1Details.IsFreezed;
                    valueObject.IsUpdate = true;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day1Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day1Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay2BizActionVO UpdateDay2(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay2VO dayvo = BizActionObj.Day2Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay2");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dayvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dayvo.UnitID = UserVo.UserLoginInfo.UnitId;
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed  ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((BizActionObj.Day2Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab2Details");
                    this.dbServer.AddInParameter(command2, "Day2ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command2, "LabDay", DbType.Int32, IVFLabDay.Day2);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((BizActionObj.Day2Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0))
                {
                    List<clsEmbryoTransferDetailsVO> list1 = new List<clsEmbryoTransferDetailsVO>();
                    foreach (clsFemaleLabDay2FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay2Details");
                        this.dbServer.AddInParameter(command3, "Day2ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command3, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command3, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command3, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command3, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command3, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command3, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedFragmentation != null)
                        {
                            this.dbServer.AddInParameter(command3, "Fragmentation", DbType.Int64, tvo.SelectedFragmentation.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Fragmentation", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastomereSymmetry != null)
                        {
                            this.dbServer.AddInParameter(command3, "BlastomereSymmetry", DbType.Int64, tvo.SelectedBlastomereSymmetry.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "BlastomereSymmetry", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command3, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command3, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command3, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay3", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay3", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day2);
                                this.dbServer.AddInParameter(command4, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command4, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command4, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command4, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command4, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (tvo.Day2CalculateDetails != null)
                        {
                            clsFemaleLabDay2CalculateDetailsVO svo2 = tvo.Day2CalculateDetails;
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay2CalculateDetails");
                            this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command5, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ZonaThickness", DbType.Int32, svo2.ZonaThickness);
                            this.dbServer.AddInParameter(command5, "ZonaTexture", DbType.Int32, svo2.ZonaTexture);
                            this.dbServer.AddInParameter(command5, "BlastomereSize", DbType.Int32, svo2.BlastomereSize);
                            this.dbServer.AddInParameter(command5, "BlastomereShape", DbType.Int32, svo2.BlastomereShape);
                            this.dbServer.AddInParameter(command5, "BlastomeresVolume", DbType.Int32, svo2.BlastomeresVolume);
                            this.dbServer.AddInParameter(command5, "Membrane", DbType.Int32, svo2.Membrane);
                            this.dbServer.AddInParameter(command5, "Cytoplasm", DbType.Int32, svo2.Cytoplasm);
                            this.dbServer.AddInParameter(command5, "Fragmentation", DbType.Int32, svo2.Fragmentation);
                            this.dbServer.AddInParameter(command5, "DevelopmentRate", DbType.Int32, svo2.DevelopmentRate);
                            this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day2Details.FUSetting != null) && (BizActionObj.Day2Details.FUSetting.Count > 0))
                {
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab2UploadFileDetails");
                    this.dbServer.AddInParameter(command6, "Day2ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int32, IVFLabDay.Day2);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, dayvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((BizActionObj.Day2Details.FUSetting != null) && (BizActionObj.Day2Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command7, "LabDay", DbType.Int16, IVFLabDay.Day2);
                        this.dbServer.AddInParameter(command7, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command7, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command7, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                    }
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab2SemenDetails");
                    this.dbServer.AddInParameter(command8, "Day2ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command8, "LabDay", DbType.Int32, IVFLabDay.Day2);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command9, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "LabDay", DbType.Int16, IVFLabDay.Day2);
                    this.dbServer.AddInParameter(command9, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command9, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command9, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command9, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command9, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command9, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command9, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command9, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command9, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command9, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command9, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command9, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command9, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command9, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                }
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day2Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay2;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;
                    valueObject.IsUpdate = true;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day2Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day2Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay2BizActionVO UpdateDay3(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay2VO dayvo = BizActionObj.Day2Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay3");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dayvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dayvo.UnitID = UserVo.UserLoginInfo.UnitId;
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed  ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((BizActionObj.Day2Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab3Details");
                    this.dbServer.AddInParameter(command2, "Day3ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command2, "LabDay", DbType.Int32, IVFLabDay.Day3);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((BizActionObj.Day2Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0))
                {
                    List<clsEmbryoTransferDetailsVO> list1 = new List<clsEmbryoTransferDetailsVO>();
                    foreach (clsFemaleLabDay2FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay3Details");
                        this.dbServer.AddInParameter(command3, "Day3ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command3, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command3, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command3, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command3, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command3, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command3, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedFragmentation != null)
                        {
                            this.dbServer.AddInParameter(command3, "Fragmentation", DbType.Int64, tvo.SelectedFragmentation.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Fragmentation", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastomereSymmetry != null)
                        {
                            this.dbServer.AddInParameter(command3, "BlastomereSymmetry", DbType.Int64, tvo.SelectedBlastomereSymmetry.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "BlastomereSymmetry", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command3, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command3, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command3, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay4", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay4", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day3);
                                this.dbServer.AddInParameter(command4, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command4, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command4, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command4, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command4, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (tvo.Day2CalculateDetails != null)
                        {
                            clsFemaleLabDay2CalculateDetailsVO svo2 = tvo.Day2CalculateDetails;
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay3CalculateDetails");
                            this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command5, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ZonaThickness", DbType.Int32, svo2.ZonaThickness);
                            this.dbServer.AddInParameter(command5, "ZonaTexture", DbType.Int32, svo2.ZonaTexture);
                            this.dbServer.AddInParameter(command5, "BlastomereSize", DbType.Int32, svo2.BlastomereSize);
                            this.dbServer.AddInParameter(command5, "BlastomereShape", DbType.Int32, svo2.BlastomereShape);
                            this.dbServer.AddInParameter(command5, "BlastomeresVolume", DbType.Int32, svo2.BlastomeresVolume);
                            this.dbServer.AddInParameter(command5, "Membrane", DbType.Int32, svo2.Membrane);
                            this.dbServer.AddInParameter(command5, "Cytoplasm", DbType.Int32, svo2.Cytoplasm);
                            this.dbServer.AddInParameter(command5, "Fragmentation", DbType.Int32, svo2.Fragmentation);
                            this.dbServer.AddInParameter(command5, "DevelopmentRate", DbType.Int32, svo2.DevelopmentRate);
                            this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day2Details.FUSetting != null) && (BizActionObj.Day2Details.FUSetting.Count > 0))
                {
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab3UploadFileDetails");
                    this.dbServer.AddInParameter(command6, "Day3ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int32, IVFLabDay.Day3);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, dayvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((BizActionObj.Day2Details.FUSetting != null) && (BizActionObj.Day2Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command7, "LabDay", DbType.Int16, IVFLabDay.Day3);
                        this.dbServer.AddInParameter(command7, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command7, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command7, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                    }
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab3SemenDetails");
                    this.dbServer.AddInParameter(command8, "Day3ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command8, "LabDay", DbType.Int32, IVFLabDay.Day3);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command9, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "LabDay", DbType.Int16, IVFLabDay.Day3);
                    this.dbServer.AddInParameter(command9, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command9, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command9, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command9, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command9, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command9, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command9, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command9, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command9, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command9, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command9, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command9, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command9, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command9, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                }
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day2Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay3;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;
                    valueObject.IsUpdate = true;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day2Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day2Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay4BizActionVO UpdateDay4(clsAddLabDay4BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay4VO dayvo = BizActionObj.Day4Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay4");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dayvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dayvo.UnitID = UserVo.UserLoginInfo.UnitId;
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed  ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((BizActionObj.Day4Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day4Details.FertilizationAssesmentDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab4Details");
                    this.dbServer.AddInParameter(command2, "Day4ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command2, "LabDay", DbType.Int32, IVFLabDay.Day4);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((BizActionObj.Day4Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day4Details.FertilizationAssesmentDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay4FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay4Details");
                        this.dbServer.AddInParameter(command3, "Day4ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command3, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command3, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command3, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command3, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command3, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command3, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedFragmentation != null)
                        {
                            this.dbServer.AddInParameter(command3, "Fragmentation", DbType.Int64, tvo.SelectedFragmentation.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Fragmentation", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastomereSymmetry != null)
                        {
                            this.dbServer.AddInParameter(command3, "BlastomereSymmetry", DbType.Int64, tvo.SelectedBlastomereSymmetry.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "BlastomereSymmetry", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command3, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command3, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command3, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay5", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay5", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day4);
                                this.dbServer.AddInParameter(command4, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command4, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command4, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command4, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command4, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (tvo.Day4CalculateDetails != null)
                        {
                            clsFemaleLabDay4CalculateDetailsVO svo2 = tvo.Day4CalculateDetails;
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay4CalculateDetails");
                            this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command5, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "Compaction", DbType.Boolean, svo2.Compaction);
                            this.dbServer.AddInParameter(command5, "SignsOfBlastocoel", DbType.Boolean, svo2.SignsOfBlastocoel);
                            this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day4Details.FUSetting != null) && (BizActionObj.Day4Details.FUSetting.Count > 0))
                {
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab4UploadFileDetails");
                    this.dbServer.AddInParameter(command6, "Day4ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int32, IVFLabDay.Day4);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, dayvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((BizActionObj.Day4Details.FUSetting != null) && (BizActionObj.Day4Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command7, "LabDay", DbType.Int16, IVFLabDay.Day4);
                        this.dbServer.AddInParameter(command7, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command7, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command7, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                    }
                }
                if (BizActionObj.Day4Details.SemenDetails != null)
                {
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab4SemenDetails");
                    this.dbServer.AddInParameter(command8, "Day4ID ", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command8, "LabDay", DbType.Int32, IVFLabDay.Day4);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if (BizActionObj.Day4Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day4Details.SemenDetails;
                    DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command9, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "LabDay", DbType.Int16, IVFLabDay.Day4);
                    this.dbServer.AddInParameter(command9, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day4Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command9, "SourceOfSemen", DbType.Int64, BizActionObj.Day4Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command9, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command9, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command9, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command9, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command9, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command9, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command9, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command9, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command9, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command9, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command9, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command9, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                }
                if (BizActionObj.Day4Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day4Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day4Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day4Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day4Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay4;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day4Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day4Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.IsUpdate = true;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day4Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day4Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay5BizActionVO UpdateDay5(clsAddLabDay5BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay5VO dayvo = BizActionObj.Day5Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay5");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dayvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dayvo.UnitID = UserVo.UserLoginInfo.UnitId;
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((BizActionObj.Day5Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day5Details.FertilizationAssesmentDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab5Details");
                    this.dbServer.AddInParameter(command2, "Day5ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command2, "LabDay", DbType.Int32, IVFLabDay.Day5);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((BizActionObj.Day5Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day5Details.FertilizationAssesmentDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay5FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay5Details");
                        this.dbServer.AddInParameter(command3, "Day5ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command3, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command3, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command3, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command3, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command3, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command3, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, 0);
                        }
                        if (tvo.SelectedICM != null)
                        {
                            this.dbServer.AddInParameter(command3, "ICM", DbType.Int64, tvo.SelectedICM.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "ICM", DbType.Int64, 0);
                        }
                        if (tvo.SelectedTrophectoderm != null)
                        {
                            this.dbServer.AddInParameter(command3, "Trophectoderm", DbType.Int64, tvo.SelectedTrophectoderm.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Trophectoderm", DbType.Int64, 0);
                        }
                        if (tvo.SelectedExpansion != null)
                        {
                            this.dbServer.AddInParameter(command3, "Expansion", DbType.Int64, tvo.SelectedExpansion.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Expansion", DbType.Int64, 0);
                        }
                        if (tvo.SelectedBlastocytsGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "BlastocytsGrade", DbType.Int64, tvo.SelectedBlastocytsGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "BlastocytsGrade", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command3, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command3, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command3, "Others", DbType.String, tvo.Others);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, 0);
                        }
                        if (tvo.SelectedPlan.ID == 3L)
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay6", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "ProceedDay6", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day5);
                                this.dbServer.AddInParameter(command4, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command4, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command4, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command4, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command4, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (tvo.Day5CalculateDetails != null)
                        {
                            clsFemaleLabDay5CalculateDetailsVO svo2 = tvo.Day5CalculateDetails;
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay5CalculateDetails");
                            this.dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, dayvo.ID);
                            this.dbServer.AddInParameter(command5, "DetailID", DbType.Int64, tvo.ID);
                            this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "BlastocoelsCavity", DbType.Boolean, svo2.BlastocoelsCavity);
                            this.dbServer.AddInParameter(command5, "TightlyPackedCells", DbType.Boolean, svo2.TightlyPackedCells);
                            this.dbServer.AddInParameter(command5, "FormingEpithelium", DbType.Boolean, svo2.FormingEpithelium);
                            this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo2.ID);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                            BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                            svo2.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                        }
                    }
                }
                if ((BizActionObj.Day5Details.FUSetting != null) && (BizActionObj.Day5Details.FUSetting.Count > 0))
                {
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab5UploadFileDetails");
                    this.dbServer.AddInParameter(command6, "Day5ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command6, "LabDay", DbType.Int32, IVFLabDay.Day5);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, dayvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((BizActionObj.Day5Details.FUSetting != null) && (BizActionObj.Day5Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command7, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command7, "LabDay", DbType.Int16, IVFLabDay.Day5);
                        this.dbServer.AddInParameter(command7, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command7, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command7, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command7, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                    }
                }
                if (BizActionObj.Day5Details.SemenDetails != null)
                {
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab5SemenDetails");
                    this.dbServer.AddInParameter(command8, "Day5ID ", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command8, "LabDay", DbType.Int32, IVFLabDay.Day5);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if (BizActionObj.Day5Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day5Details.SemenDetails;
                    DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command9, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "LabDay", DbType.Int16, IVFLabDay.Day5);
                    this.dbServer.AddInParameter(command9, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day5Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command9, "SourceOfSemen", DbType.Int64, BizActionObj.Day5Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command9, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command9, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command9, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command9, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command9, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command9, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command9, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command9, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command9, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command9, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command9, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command9, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command9, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command9, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                }
                if (BizActionObj.Day5Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day5Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day5Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day5Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day5Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay5;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day5Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day5Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day5Details.IsFreezed;
                    valueObject.IsUpdate = true;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day5Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day5Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }

        private clsAddLabDay6BizActionVO UpdateDay6(clsAddLabDay6BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection pConnection = null;
            try
            {
                pConnection = this.dbServer.CreateConnection();
                pConnection.Open();
                transaction = pConnection.BeginTransaction();
                clsFemaleLabDay6VO dayvo = BizActionObj.Day6Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay6");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dayvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, dayvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, dayvo.CoupleUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, dayvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, dayvo.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, dayvo.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, dayvo.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, dayvo.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, dayvo.AssAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "IVFCycleCount", DbType.Int64, dayvo.IVFCycleCount);
                this.dbServer.AddInParameter(storedProcCommand, "SourceNeedleID", DbType.Int64, dayvo.SourceNeedleID);
                this.dbServer.AddInParameter(storedProcCommand, "InfectionObserved", DbType.String, dayvo.InfectionObserved);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOfOocytes", DbType.Int32, dayvo.TotNoOfOocytes);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PN", DbType.Int32, dayvo.TotNoOf2PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf3PN", DbType.Int32, dayvo.TotNoOf3PN);
                this.dbServer.AddInParameter(storedProcCommand, "TotNoOf2PB", DbType.Int32, dayvo.TotNoOf2PB);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMI", DbType.Int32, dayvo.ToNoOfMI);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfMII", DbType.Int32, dayvo.ToNoOfMII);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfGV", DbType.Int32, dayvo.ToNoOfGV);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfDeGenerated", DbType.Int32, dayvo.ToNoOfDeGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "ToNoOfLost ", DbType.Int32, dayvo.ToNoOfLost);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed ", DbType.Boolean, dayvo.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Status  ", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                if ((BizActionObj.Day6Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day6Details.FertilizationAssesmentDetails.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab6Details");
                    this.dbServer.AddInParameter(command2, "Day6ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command2, "LabDay", DbType.Int32, IVFLabDay.Day6);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if ((BizActionObj.Day6Details.FertilizationAssesmentDetails != null) && (BizActionObj.Day6Details.FertilizationAssesmentDetails.Count > 0))
                {
                    foreach (clsFemaleLabDay6FertilizationAssesmentVO tvo in dayvo.FertilizationAssesmentDetails)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay6Details");
                        this.dbServer.AddInParameter(command3, "Day6ID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "OoNo", DbType.Int64, tvo.OoNo);
                        this.dbServer.AddInParameter(command3, "SerialOccyteNo", DbType.Int64, tvo.SerialOccyteNo);
                        this.dbServer.AddInParameter(command3, "PlanTreatmentID", DbType.Int64, tvo.PlanTreatmentID);
                        this.dbServer.AddInParameter(command3, "Date", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command3, "Time", DbType.DateTime, tvo.Time);
                        this.dbServer.AddInParameter(command3, "SrcOocyteID", DbType.Int64, tvo.SrcOocyteID);
                        this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.String, tvo.OocyteDonorID);
                        this.dbServer.AddInParameter(command3, "SrcOfSemen", DbType.Int64, tvo.SrcOfSemen);
                        this.dbServer.AddInParameter(command3, "SemenDonorID", DbType.String, tvo.SemenDonorID);
                        this.dbServer.AddInParameter(command3, "FileName", DbType.String, tvo.FileName);
                        this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, tvo.FileContents);
                        if (tvo.SelectedFePlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, tvo.SelectedFePlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "FertilisationStage", DbType.Int64, 0);
                        }
                        if (tvo.SelectedGrade != null)
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, tvo.SelectedGrade.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "Grade", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Score", DbType.Int64, tvo.Score);
                        this.dbServer.AddInParameter(command3, "PV", DbType.Boolean, tvo.PV);
                        this.dbServer.AddInParameter(command3, "XFactor", DbType.Boolean, tvo.XFactor);
                        this.dbServer.AddInParameter(command3, "Others", DbType.String, tvo.Others);
                        this.dbServer.AddInParameter(command3, "ProceedDay7", DbType.Boolean, tvo.ProceedDay7);
                        if (tvo.SelectedPlan != null)
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, tvo.SelectedPlan.ID);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, 0);
                        }
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        tvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        if ((tvo.MediaDetails != null) && (tvo.MediaDetails.Count > 0))
                        {
                            foreach (clsFemaleMediaDetailsVO svo in tvo.MediaDetails)
                            {
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                this.dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, dayvo.ID);
                                this.dbServer.AddInParameter(command4, "DetailID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, svo.Date);
                                this.dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day6);
                                this.dbServer.AddInParameter(command4, "MediaName", DbType.String, svo.ItemName);
                                this.dbServer.AddInParameter(command4, "Company", DbType.String, svo.Company);
                                this.dbServer.AddInParameter(command4, "LotNo", DbType.String, svo.BatchCode);
                                this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                                this.dbServer.AddInParameter(command4, "PH", DbType.Boolean, svo.PH);
                                this.dbServer.AddInParameter(command4, "OSM", DbType.Boolean, svo.OSM);
                                this.dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, svo.VolumeUsed);
                                this.dbServer.AddInParameter(command4, "Status", DbType.Int64, svo.SelectedStatus.ID);
                                this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, svo.BatchID);
                                this.dbServer.AddInParameter(command4, "StoreID", DbType.Int64, svo.StoreID);
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                                this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                                svo.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                    }
                }
                if ((BizActionObj.Day6Details.FUSetting != null) && (BizActionObj.Day6Details.FUSetting.Count > 0))
                {
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab6UploadFileDetails");
                    this.dbServer.AddInParameter(command5, "Day6ID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command5, "LabDay", DbType.Int32, IVFLabDay.Day6);
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, dayvo.UnitID);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                }
                if ((BizActionObj.Day6Details.FUSetting != null) && (BizActionObj.Day6Details.FUSetting.Count > 0))
                {
                    foreach (FileUpload upload in dayvo.FUSetting)
                    {
                        DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");
                        this.dbServer.AddInParameter(command6, "OocyteID", DbType.Int64, dayvo.ID);
                        this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command6, "LabDay", DbType.Int16, IVFLabDay.Day6);
                        this.dbServer.AddInParameter(command6, "FileName", DbType.String, upload.FileName);
                        this.dbServer.AddInParameter(command6, "FileIndex", DbType.Int32, upload.Index);
                        this.dbServer.AddInParameter(command6, "Value", DbType.Binary, upload.Data);
                        this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                        this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, upload.ID);
                        this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command6, transaction);
                        BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                        upload.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                    }
                }
                if (BizActionObj.Day6Details.SemenDetails != null)
                {
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_DeleteLab6SemenDetails");
                    this.dbServer.AddInParameter(command7, "Day6ID ", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command7, "LabDay", DbType.Int32, IVFLabDay.Day6);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                }
                if (BizActionObj.Day6Details.SemenDetails != null)
                {
                    clsFemaleSemenDetailsVO semenDetails = BizActionObj.Day6Details.SemenDetails;
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    this.dbServer.AddInParameter(command8, "OocyteID", DbType.Int64, dayvo.ID);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "LabDay", DbType.Int16, IVFLabDay.Day6);
                    this.dbServer.AddInParameter(command8, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day6Details.SemenDetails.MethodOfSpermPreparation);
                    this.dbServer.AddInParameter(command8, "SourceOfSemen", DbType.Int64, BizActionObj.Day6Details.SemenDetails.SourceOfSemen);
                    this.dbServer.AddInParameter(command8, "PreSelfVolume", DbType.String, semenDetails.PreSelfVolume);
                    this.dbServer.AddInParameter(command8, "PreSelfConcentration", DbType.String, semenDetails.PreSelfConcentration);
                    this.dbServer.AddInParameter(command8, "PreSelfMotality", DbType.String, semenDetails.PreSelfMotality);
                    this.dbServer.AddInParameter(command8, "PreSelfWBC", DbType.String, semenDetails.PreSelfWBC);
                    this.dbServer.AddInParameter(command8, "PreDonorVolume", DbType.String, semenDetails.PreDonorVolume);
                    this.dbServer.AddInParameter(command8, "PreDonorConcentration", DbType.String, semenDetails.PreDonorConcentration);
                    this.dbServer.AddInParameter(command8, "PreDonorMotality", DbType.String, semenDetails.PreDonorMotality);
                    this.dbServer.AddInParameter(command8, "PreDonorWBC", DbType.String, semenDetails.PreDonorWBC);
                    this.dbServer.AddInParameter(command8, "PostSelfVolume", DbType.String, semenDetails.PostSelfVolume);
                    this.dbServer.AddInParameter(command8, "PostSelfConcentration", DbType.String, semenDetails.PostSelfConcentration);
                    this.dbServer.AddInParameter(command8, "PostSelfMotality", DbType.String, semenDetails.PostSelfMotality);
                    this.dbServer.AddInParameter(command8, "PostSelfWBC", DbType.String, semenDetails.PostSelfWBC);
                    this.dbServer.AddInParameter(command8, "PostDonorVolume ", DbType.String, semenDetails.PostDonorVolume);
                    this.dbServer.AddInParameter(command8, "PostDonorConcentration", DbType.String, semenDetails.PostDonorConcentration);
                    this.dbServer.AddInParameter(command8, "PostDonorMotality", DbType.String, semenDetails.PostDonorMotality);
                    this.dbServer.AddInParameter(command8, "PostDonorWBC", DbType.String, semenDetails.PostDonorWBC);
                    this.dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, semenDetails.ID);
                    this.dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command8, "ResultStatus");
                    semenDetails.ID = (long) this.dbServer.GetParameterValue(command8, "ID");
                }
                if (BizActionObj.Day6Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL instance = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO valueObject = new clsAddUpdateLabDaysSummaryBizActionVO {
                        LabDaysSummary = BizActionObj.Day6Details.LabDaySummary
                    };
                    valueObject.LabDaysSummary.OocyteID = BizActionObj.Day6Details.ID;
                    valueObject.LabDaysSummary.CoupleID = BizActionObj.Day6Details.CoupleID;
                    valueObject.LabDaysSummary.CoupleUnitID = BizActionObj.Day6Details.CoupleUnitID;
                    valueObject.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay6;
                    valueObject.LabDaysSummary.Priority = 1;
                    valueObject.LabDaysSummary.ProcDate = BizActionObj.Day6Details.Date;
                    valueObject.LabDaysSummary.ProcTime = BizActionObj.Day6Details.Time;
                    valueObject.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    valueObject.LabDaysSummary.IsFreezed = BizActionObj.Day6Details.IsFreezed;
                    valueObject.IsUpdate = true;
                    valueObject = (clsAddUpdateLabDaysSummaryBizActionVO) instance.AddUpdateLabDaysSummary(valueObject, UserVo, pConnection, transaction);
                    if (valueObject.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    BizActionObj.Day6Details.LabDaySummary.ID = valueObject.LabDaysSummary.ID;
                }
                transaction.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                transaction.Rollback();
                BizActionObj.Day6Details = null;
            }
            finally
            {
                pConnection.Close();
                transaction = null;
                pConnection = null;
            }
            return BizActionObj;
        }
    }
}

