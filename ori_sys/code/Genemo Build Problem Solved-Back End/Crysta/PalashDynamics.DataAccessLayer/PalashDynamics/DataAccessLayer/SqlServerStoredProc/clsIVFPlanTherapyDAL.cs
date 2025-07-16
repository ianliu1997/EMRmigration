namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using PalashDynamics.ValueObjects.EMR;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media;

    internal class clsIVFPlanTherapyDAL : clsBaseIVFPlanTherapyDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsIVFPlanTherapyDAL()
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
                this.ImgIP = ConfigurationManager.AppSettings["RegImgIP"];
                this.ImgVirtualDir = ConfigurationManager.AppSettings["RegImgVirtualDir"];
                this.ImgSaveLocation = ConfigurationManager.AppSettings["ImgSavingLocation"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddDiscardForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSpremFreezingDetailsBizActionVO nvo = valueObject as clsGetSpremFreezingDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if ((nvo.Vitrification != null) && (nvo.Vitrification.Count > 0))
                {
                    foreach (clsSpermFreezingVO gvo in nvo.Vitrification)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDiscardSpermForCryoBank");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "SpremFreezingID", DbType.Int64, gvo.SpremFreezingID);
                        this.dbServer.AddInParameter(storedProcCommand, "SpremFreezingUnitID", DbType.Int64, gvo.SpremFreezingUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "SpremNo", DbType.Int64, gvo.SpremNo);
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, gvo.ID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Vitrification = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateETDetailsBizActionVO nvo = valueObject as clsAddUpdateETDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                string str;
                string str2;
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_AddUpdateETDetails");
                DataTable table = new DataTable();
                table = ListToDataTable<ETDetailsVO>(nvo.ET.ETDetails);
                table.TableName = "ETDetails";
                using (StringWriter writer = new StringWriter())
                {
                    table.WriteXml(writer);
                    str = writer.ToString();
                }
                table = ListToDataTable<FileUpload>(nvo.ET.FUSetting);
                table.TableName = "FUSettingDetails";
                using (StringWriter writer2 = new StringWriter())
                {
                    table.WriteXml(writer2);
                    str2 = writer2.ToString();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, nvo.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.ET.Date);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.ET.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssEmbryologistID", DbType.Int64, nvo.ET.AssEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnasthesistID", DbType.Int64, nvo.ET.AnasthesistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssAnasthesistID", DbType.Int64, nvo.ET.AssAnasthesistID);
                this.dbServer.AddInParameter(storedProcCommand, "EndometriumThickness", DbType.Int64, nvo.ET.EndometriumThickness);
                this.dbServer.AddInParameter(storedProcCommand, "ETPattern", DbType.Int64, nvo.ET.ETPattern);
                this.dbServer.AddInParameter(storedProcCommand, "ColorDopper", DbType.String, nvo.ET.ColorDopper);
                this.dbServer.AddInParameter(storedProcCommand, "IsTreatmentUnderGA", DbType.Boolean, nvo.ET.IsTreatmentUnderGA);
                this.dbServer.AddInParameter(storedProcCommand, "CatheterTypeID", DbType.Int64, nvo.ET.CatheterTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Difficulty", DbType.Boolean, nvo.ET.Difficulty);
                this.dbServer.AddInParameter(storedProcCommand, "DifficultyTypeID", DbType.Int64, nvo.ET.DifficultyTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ET.Status);
                this.dbServer.AddInParameter(storedProcCommand, "TenaculumUsed", DbType.Boolean, nvo.ET.TenaculumUsed);
                this.dbServer.AddInParameter(storedProcCommand, "DistancefromFundus", DbType.String, nvo.ET.DistanceFromFundus);
                this.dbServer.AddInParameter(storedProcCommand, "IsEndometerialPI", DbType.Boolean, nvo.ET.IsEndometerialPI);
                this.dbServer.AddInParameter(storedProcCommand, "IsEndometerialRI", DbType.Boolean, nvo.ET.IsEndometerialRI);
                this.dbServer.AddInParameter(storedProcCommand, "IsEndometerialSD", DbType.Boolean, nvo.ET.IsEndometerialSD);
                this.dbServer.AddInParameter(storedProcCommand, "IsUterinePI", DbType.Boolean, nvo.ET.IsUterinePI);
                this.dbServer.AddInParameter(storedProcCommand, "IsUterineRI", DbType.Boolean, nvo.ET.IsUterineRI);
                this.dbServer.AddInParameter(storedProcCommand, "IsUterineSD", DbType.Boolean, nvo.ET.IsUterineSD);
                this.dbServer.AddInParameter(storedProcCommand, "SrcSemenCode", DbType.String, nvo.ET.SrcSemenCode);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyCode", DbType.String, nvo.ET.SrcOoctyCode);
                this.dbServer.AddInParameter(storedProcCommand, "SrcSemenID", DbType.Int64, nvo.ET.SrcSemenID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyID", DbType.Int64, nvo.ET.SrcOoctyID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.ET.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsOnlyET", DbType.Boolean, nvo.ET.IsOnlyET);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.ET.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "ETDetails", DbType.Xml, str);
                this.dbServer.AddInParameter(storedProcCommand, "FUSettingDetails", DbType.Xml, str2);
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
                nvo.ET = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdatePlanTherapy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPlanTherapyBizActionVO bizActionObj = valueObject as clsAddPlanTherapyBizActionVO;
            bizActionObj = this.AddUpdatePlanTherapy(bizActionObj, UserVo);
            return valueObject;
        }

        private clsAddPlanTherapyBizActionVO AddUpdatePlanTherapy(clsAddPlanTherapyBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_AddUpdatePlanTherapy");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "CycleDuration", DbType.String, BizActionObj.TherapyDetails.CycleDuration);
                this.dbServer.AddInParameter(storedProcCommand, "PlannedTreatmentID", DbType.Int64, BizActionObj.TherapyDetails.PlannedTreatmentID);
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
                this.dbServer.AddInParameter(storedProcCommand, "LutealSupport", DbType.String, BizActionObj.TherapyDetails.LutealSupport);
                this.dbServer.AddInParameter(storedProcCommand, "LutealRemarks", DbType.String, BizActionObj.TherapyDetails.LutealRemarks);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1Date", DbType.DateTime, BizActionObj.TherapyDetails.BHCGAss1Date);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1IsBSCG", DbType.Boolean, BizActionObj.TherapyDetails.BHCGAss1IsBSCG);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1BSCGValue", DbType.String, BizActionObj.TherapyDetails.BHCGAss1BSCGValue);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss1SrProgest", DbType.String, BizActionObj.TherapyDetails.BHCGAss1SrProgest);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2Date", DbType.DateTime, BizActionObj.TherapyDetails.BHCGAss2Date);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2IsBSCG", DbType.Boolean, BizActionObj.TherapyDetails.BHCGAss2IsBSCG);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2BSCGValue", DbType.String, BizActionObj.TherapyDetails.BHCGAss2BSCGValue);
                this.dbServer.AddInParameter(storedProcCommand, "BHCGAss2USG", DbType.String, BizActionObj.TherapyDetails.BHCGAss2USG);
                this.dbServer.AddInParameter(storedProcCommand, "PregnancyAchieved", DbType.Boolean, BizActionObj.TherapyDetails.IsPregnancyAchieved);
                this.dbServer.AddInParameter(storedProcCommand, "PregnanacyConfirmDate", DbType.DateTime, BizActionObj.TherapyDetails.PregnanacyConfirmDate);
                this.dbServer.AddInParameter(storedProcCommand, "IsClosed", DbType.Boolean, BizActionObj.TherapyDetails.IsClosed);
                this.dbServer.AddInParameter(storedProcCommand, "OutComeRemarks", DbType.String, BizActionObj.TherapyDetails.OutComeRemarks);
                this.dbServer.AddInParameter(storedProcCommand, "BiochemPregnancy", DbType.Boolean, BizActionObj.TherapyDetails.BiochemPregnancy);
                this.dbServer.AddInParameter(storedProcCommand, "Ectopic", DbType.Boolean, BizActionObj.TherapyDetails.Ectopic);
                this.dbServer.AddInParameter(storedProcCommand, "Abortion", DbType.Boolean, BizActionObj.TherapyDetails.Abortion);
                this.dbServer.AddInParameter(storedProcCommand, "Missed", DbType.Boolean, BizActionObj.TherapyDetails.Missed);
                this.dbServer.AddInParameter(storedProcCommand, "FetalHeartSound", DbType.Boolean, BizActionObj.TherapyDetails.FetalHeartSound);
                this.dbServer.AddInParameter(storedProcCommand, "FetalDate", DbType.DateTime, BizActionObj.TherapyDetails.FetalDate);
                this.dbServer.AddInParameter(storedProcCommand, "Count", DbType.String, BizActionObj.TherapyDetails.Count);
                this.dbServer.AddInParameter(storedProcCommand, "Incomplete", DbType.Boolean, BizActionObj.TherapyDetails.Incomplete);
                this.dbServer.AddInParameter(storedProcCommand, "IUD", DbType.Boolean, BizActionObj.TherapyDetails.IUD);
                this.dbServer.AddInParameter(storedProcCommand, "PretermDelivery", DbType.Boolean, BizActionObj.TherapyDetails.PretermDelivery);
                this.dbServer.AddInParameter(storedProcCommand, "LiveBirth", DbType.Boolean, BizActionObj.TherapyDetails.LiveBirth);
                this.dbServer.AddInParameter(storedProcCommand, "Congenitalabnormality", DbType.Boolean, BizActionObj.TherapyDetails.Congenitalabnormality);
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
                this.dbServer.AddInParameter(storedProcCommand, "DocumentDate", DbType.DateTime, BizActionObj.TherapyDocument.Date);
                this.dbServer.AddInParameter(storedProcCommand, "DocumentID", DbType.Int64, BizActionObj.TherapyDocument.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, BizActionObj.TherapyDocument.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, BizActionObj.TherapyDocument.Title);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, BizActionObj.TherapyDocument.AttachedFileName);
                this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, BizActionObj.TherapyDocument.AttachedFileContent);
                this.dbServer.AddInParameter(storedProcCommand, "IsDeleted", DbType.Boolean, BizActionObj.TherapyDocument.IsDeleted);
                this.dbServer.AddInParameter(storedProcCommand, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);
                if (BizActionObj.TherapyDetails.IsSurrogate)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                    this.dbServer.AddInParameter(storedProcCommand, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                if (!BizActionObj.TherapyDetails.IsSurrogateCalendar)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                }
                else
                {
                    string str = null;
                    this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, str);
                }
                if (!BizActionObj.TherapyDetails.IsSurrogateDrug)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                    this.dbServer.AddInParameter(storedProcCommand, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                    this.dbServer.AddInParameter(storedProcCommand, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                    this.dbServer.AddInParameter(storedProcCommand, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                    this.dbServer.AddInParameter(storedProcCommand, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                    this.dbServer.AddInParameter(storedProcCommand, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                    this.dbServer.AddInParameter(storedProcCommand, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsChemicalPregnancy", DbType.Boolean, BizActionObj.TherapyDetails.IsChemicalPregnancy);
                this.dbServer.AddInParameter(storedProcCommand, "IsFullTermDelivery", DbType.Boolean, BizActionObj.TherapyDetails.IsFullTermDelivery);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSEarly", DbType.Boolean, BizActionObj.TherapyDetails.OHSSEarly);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSLate", DbType.Boolean, BizActionObj.TherapyDetails.OHSSLate);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSMild", DbType.Boolean, BizActionObj.TherapyDetails.OHSSMild);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSMode", DbType.Boolean, BizActionObj.TherapyDetails.OHSSMode);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSSereve", DbType.Boolean, BizActionObj.TherapyDetails.OHSSSereve);
                this.dbServer.AddInParameter(storedProcCommand, "OHSSRemark", DbType.String, BizActionObj.TherapyDetails.OHSSRemark);
                this.dbServer.AddInParameter(storedProcCommand, "BabyTypeID", DbType.Int64, BizActionObj.TherapyDetails.BabyTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "SIX_monthFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.SIXmonthFitnessRemark);
                this.dbServer.AddInParameter(storedProcCommand, "SIX_monthFitnessID", DbType.Int64, BizActionObj.TherapyDetails.SIXmonthFitnessID);
                this.dbServer.AddInParameter(storedProcCommand, "One_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.ONEyFitnessRemark);
                this.dbServer.AddInParameter(storedProcCommand, "One_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.ONEyFitnessID);
                this.dbServer.AddInParameter(storedProcCommand, "FIVE_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.FIVEyFitnessRemark);
                this.dbServer.AddInParameter(storedProcCommand, "FIVE_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.FIVEyFitnessID);
                this.dbServer.AddInParameter(storedProcCommand, "TEN_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.TENyFitnessRemark);
                this.dbServer.AddInParameter(storedProcCommand, "TEN_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.TENyFitnessID);
                this.dbServer.AddInParameter(storedProcCommand, "TWNTY_yearFitnessRemarks", DbType.String, BizActionObj.TherapyDetails.TWENTYyFitnessRemark);
                this.dbServer.AddInParameter(storedProcCommand, "TWNTY_yearFitnessID", DbType.Int64, BizActionObj.TherapyDetails.TWENTYyFitnessID);
                this.dbServer.AddInParameter(storedProcCommand, "SIX_monthFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.SIXmonthFitnessRemark_m);
                this.dbServer.AddInParameter(storedProcCommand, "SIX_monthFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.SIXmonthFitnessID_m);
                this.dbServer.AddInParameter(storedProcCommand, "One_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.ONEyFitnessRemark_m);
                this.dbServer.AddInParameter(storedProcCommand, "One_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.ONEyFitnessID_m);
                this.dbServer.AddInParameter(storedProcCommand, "FIVE_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.FIVEyFitnessRemark_m);
                this.dbServer.AddInParameter(storedProcCommand, "FIVE_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.FIVEyFitnessID_m);
                this.dbServer.AddInParameter(storedProcCommand, "TEN_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.TENyFitnessRemark_m);
                this.dbServer.AddInParameter(storedProcCommand, "TEN_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.TENyFitnessID_m);
                this.dbServer.AddInParameter(storedProcCommand, "TWNTY_yearFitnessRemarks_m", DbType.String, BizActionObj.TherapyDetails.TWENTYyFitnessRemark_m);
                this.dbServer.AddInParameter(storedProcCommand, "TWNTY_yearFitnessID_m", DbType.Int64, BizActionObj.TherapyDetails.TWENTYyFitnessID_m);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "OPUDate", DbType.DateTime, BizActionObj.TherapyDetails.OPUtDate);
                this.dbServer.AddInParameter(storedProcCommand, "OPURemark", DbType.String, BizActionObj.TherapyDetails.OPURemark);
                this.dbServer.AddInParameter(storedProcCommand, "LongtermMedication", DbType.String, BizActionObj.TherapyDetails.LongtermMedication);
                this.dbServer.AddInParameter(storedProcCommand, "AssistedHatching", DbType.Boolean, BizActionObj.TherapyDetails.AssistedHatching);
                this.dbServer.AddInParameter(storedProcCommand, "IMSI", DbType.Boolean, BizActionObj.TherapyDetails.IMSI);
                this.dbServer.AddInParameter(storedProcCommand, "CryoPreservation", DbType.Boolean, BizActionObj.TherapyDetails.CryoPreservation);
                this.dbServer.AddParameter(storedProcCommand, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.TherapyDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "TherapyID");
                if (BizActionObj.TherapyDetails.IsSurrogate || BizActionObj.TherapyDetails.IsSurrogateDrug)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("IVF_AddUpdateSurrogatePlanTherapy");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                    this.dbServer.AddInParameter(command2, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                    this.dbServer.AddInParameter(command2, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                    this.dbServer.AddInParameter(command2, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                    this.dbServer.AddInParameter(command2, "PlanTherapyId", DbType.Int64, BizActionObj.TherapyDetails.ID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                    this.dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                    this.dbServer.AddInParameter(command2, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                    this.dbServer.AddInParameter(command2, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);
                    if (BizActionObj.TherapyDelivery != null)
                    {
                        this.dbServer.AddInParameter(command2, "DeliveryDate", DbType.DateTime, BizActionObj.TherapyDelivery.DeliveryDate);
                        this.dbServer.AddInParameter(command2, "DeliveryID", DbType.Int64, BizActionObj.TherapyDelivery.ID);
                        this.dbServer.AddInParameter(command2, "Weight", DbType.Double, BizActionObj.TherapyDelivery.Weight);
                        this.dbServer.AddInParameter(command2, "TimeofBirth", DbType.DateTime, BizActionObj.TherapyDelivery.TimeofBirth);
                        this.dbServer.AddInParameter(command2, "Mode", DbType.String, BizActionObj.TherapyDelivery.Mode);
                        this.dbServer.AddInParameter(command2, "Baby", DbType.String, BizActionObj.TherapyDelivery.Baby);
                    }
                    this.dbServer.AddInParameter(command2, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.SurrogateExecutionId);
                    this.dbServer.AddInParameter(command2, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                    this.dbServer.AddInParameter(command2, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                    if (BizActionObj.TherapyDetails.IsSurrogateCalendar || BizActionObj.TherapyDetails.IsSurrogateDrug)
                    {
                        this.dbServer.AddInParameter(command2, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                    }
                    else
                    {
                        string str2 = null;
                        this.dbServer.AddInParameter(command2, "Value", DbType.String, str2);
                    }
                    if (!BizActionObj.TherapyDetails.IsSurrogateDrug)
                    {
                        this.dbServer.AddInParameter(command2, "DrugTime", DbType.DateTime, DateTime.Now);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                        this.dbServer.AddInParameter(command2, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                        this.dbServer.AddInParameter(command2, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                    }
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command2, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2);
                    BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    BizActionObj.TherapyDetails.ID = (long) this.dbServer.GetParameterValue(command2, "TherapyID");
                    if ((BizActionObj.ANCList != null) && (BizActionObj.ANCList.Count > 0))
                    {
                        foreach (clsTherapyANCVO yancvo in BizActionObj.ANCList)
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("IVF_AddUpdateANCVisit");
                            this.dbServer.AddInParameter(command3, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, BizActionObj.TherapyDetails.UnitID);
                            this.dbServer.AddInParameter(command3, "ANCDate", DbType.DateTime, yancvo.ANCDate);
                            this.dbServer.AddInParameter(command3, "POG", DbType.String, yancvo.POG);
                            this.dbServer.AddInParameter(command3, "Findings", DbType.String, yancvo.Findings);
                            this.dbServer.AddInParameter(command3, "USGReproductive", DbType.String, yancvo.USGReproductive);
                            this.dbServer.AddInParameter(command3, "Investigations", DbType.String, yancvo.Investigation);
                            this.dbServer.AddInParameter(command3, "Remarks", DbType.String, yancvo.Remarks);
                            this.dbServer.AddParameter(command3, "ANCID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, yancvo.ANCID);
                            this.dbServer.ExecuteNonQuery(command3);
                            yancvo.ANCID = (long) this.dbServer.GetParameterValue(command3, "ANCID");
                        }
                    }
                }
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return BizActionObj;
        }

        private clsAddPlanTherapyBizActionVO AddUpdatePlanTherapySurrogate(clsAddPlanTherapyBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_AddUpdateSurrogatePlanTherapy");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "TabID", DbType.Int64, BizActionObj.TherapyDetails.TabID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyStartDate", DbType.DateTime, BizActionObj.TherapyDetails.TherapyStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyId", DbType.Int64, BizActionObj.TherapyDetails.CoupleId);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitId", DbType.Int64, BizActionObj.TherapyDetails.CoupleUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "SurrogateID", DbType.Int64, BizActionObj.TherapyDetails.SurrogateID);
                this.dbServer.AddInParameter(storedProcCommand, "SurrogateMRNo", DbType.String, BizActionObj.TherapyDetails.SurrogateMRNo);
                this.dbServer.AddInParameter(storedProcCommand, "IsSurrogate", DbType.Boolean, BizActionObj.TherapyDetails.IsSurrogate);
                if (BizActionObj.TherapyDelivery != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DeliveryDate", DbType.DateTime, BizActionObj.TherapyDelivery.DeliveryDate);
                    this.dbServer.AddInParameter(storedProcCommand, "DeliveryID", DbType.Int64, BizActionObj.TherapyDelivery.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "Weight", DbType.Double, BizActionObj.TherapyDelivery.Weight);
                    this.dbServer.AddInParameter(storedProcCommand, "TimeofBirth", DbType.DateTime, BizActionObj.TherapyDelivery.TimeofBirth);
                    this.dbServer.AddInParameter(storedProcCommand, "Mode", DbType.String, BizActionObj.TherapyDelivery.Mode);
                    this.dbServer.AddInParameter(storedProcCommand, "Baby", DbType.String, BizActionObj.TherapyDelivery.Baby);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ThreapyExecutionId", DbType.Int64, BizActionObj.TherapyDetails.ThreapyExecutionId);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyExeTypeID", DbType.Int64, BizActionObj.TherapyDetails.TherapyExeTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.String, BizActionObj.TherapyDetails.Day);
                this.dbServer.AddInParameter(storedProcCommand, "Value", DbType.String, BizActionObj.TherapyDetails.Value);
                this.dbServer.AddInParameter(storedProcCommand, "DrugNotes", DbType.String, BizActionObj.TherapyDetails.DrugNotes);
                this.dbServer.AddInParameter(storedProcCommand, "DrugTime", DbType.DateTime, BizActionObj.TherapyDetails.DrugTime);
                this.dbServer.AddInParameter(storedProcCommand, "ThearpyTypeDetailId", DbType.Int64, BizActionObj.TherapyDetails.ThearpyTypeDetailId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "TherapyID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TherapyDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                BizActionObj.TherapyDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "TherapyID");
                if ((BizActionObj.ANCList != null) && (BizActionObj.ANCList.Count > 0))
                {
                    foreach (clsTherapyANCVO yancvo in BizActionObj.ANCList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("IVF_AddUpdateANCVisit");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, BizActionObj.TherapyDetails.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.TherapyDetails.UnitID);
                        this.dbServer.AddInParameter(command2, "ANCDate", DbType.DateTime, yancvo.ANCDate);
                        this.dbServer.AddInParameter(command2, "POG", DbType.String, yancvo.POG);
                        this.dbServer.AddInParameter(command2, "Findings", DbType.String, yancvo.Findings);
                        this.dbServer.AddInParameter(command2, "USGReproductive", DbType.String, yancvo.USGReproductive);
                        this.dbServer.AddInParameter(command2, "Investigations", DbType.String, yancvo.Investigation);
                        this.dbServer.AddInParameter(command2, "Remarks", DbType.String, yancvo.Remarks);
                        this.dbServer.AddParameter(command2, "ANCID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, yancvo.ANCID);
                        this.dbServer.ExecuteNonQuery(command2);
                        yancvo.ANCID = (long) this.dbServer.GetParameterValue(command2, "ANCID");
                    }
                }
            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.TherapyDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateSpermThawingBizActionVO nvo = valueObject as clsAddUpdateSpermThawingBizActionVO;
            try
            {
                if (!nvo.IsNewForm)
                {
                    foreach (cls_NewThawingDetailsVO svo in nvo.ThawingList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_UpdateSpermThawing_NEW");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.CoupleID);
                        this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, svo.MainUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, svo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, svo.LabPersonId);
                        DateTime? thawingDate = svo.ThawingDate;
                        DateTime time3 = thawingDate.Value;
                        this.dbServer.AddInParameter(storedProcCommand, "ThawingDate", DbType.DateTime, time3.Date);
                        DateTime? thawingTime = svo.ThawingTime;
                        this.dbServer.AddInParameter(storedProcCommand, "ThawingTime", DbType.DateTime, thawingTime.Value.ToShortTimeString());
                        this.dbServer.AddInParameter(storedProcCommand, "Motility", DbType.String, svo.Motility);
                        this.dbServer.AddInParameter(storedProcCommand, "SpermCount", DbType.String, svo.TotalSpearmCount);
                        this.dbServer.AddInParameter(storedProcCommand, "Volume", DbType.String, svo.Volume);
                        this.dbServer.AddInParameter(storedProcCommand, "Comments", DbType.String, svo.Comments);
                        this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, svo.IsFreezed);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                    }
                }
                else
                {
                    foreach (cls_NewGetSpremThawingBizActionVO nvo2 in nvo.ThawDeList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateSpermThawing");
                        if (nvo2.ID == 0L)
                        {
                            this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                            this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo2.ID);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.CoupleID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.Int64, nvo.CoupleUintID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientId);
                        this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, nvo2.LabInchargeId);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrificationID", DbType.String, nvo2.VitrificationNo);
                        this.dbServer.AddInParameter(storedProcCommand, "ThawingDate", DbType.DateTime, nvo2.ThawingDate.Value.Date);
                        this.dbServer.AddInParameter(storedProcCommand, "ThawingTime", DbType.DateTime, nvo2.ThawingTime.Value.ToShortTimeString());
                        this.dbServer.AddInParameter(storedProcCommand, "Motility", DbType.String, nvo2.Motility);
                        this.dbServer.AddInParameter(storedProcCommand, "SpermCount", DbType.String, nvo2.TotalSpearmCount);
                        this.dbServer.AddInParameter(storedProcCommand, "Volume", DbType.String, nvo2.Volume);
                        this.dbServer.AddInParameter(storedProcCommand, "Comments", DbType.String, nvo2.Comments);
                        this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo2.IsFreezed);
                        this.dbServer.AddInParameter(storedProcCommand, "PostThawPlanId", DbType.Int64, nvo2.PostThawPlanId);
                        this.dbServer.AddInParameter(storedProcCommand, "SpermFreezingDetailsID", DbType.Int64, nvo2.SpremNo);
                        this.dbServer.AddInParameter(storedProcCommand, "SpermFreezingDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorID", DbType.Int64, nvo.DonorID);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorUnitID", DbType.Int64, nvo.DonorUnitID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateThawingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateThawingBizActionVO nvo = valueObject as clsAddUpdateThawingBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                string str;
                string str3;
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_AddUpdateThawingDetails");
                DataTable table = new DataTable();
                table = ListToDataTable<clsThawingDetailsVO>(nvo.Thawing.ThawingDetails);
                table.TableName = "ThawingDetails";
                using (StringWriter writer = new StringWriter())
                {
                    table.WriteXml(writer);
                    str = writer.ToString();
                }
                string str2 = "";
                int num = -1;
                table = new DataTable();
                foreach (clsThawingDetailsVO svo in nvo.Thawing.ThawingDetails)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= svo.MediaDetails.Count)
                        {
                            table = ListToDataTable<clsFemaleMediaDetailsVO>(svo.MediaDetails);
                            using (StringWriter writer2 = new StringWriter())
                            {
                                table.WriteXml(writer2);
                                str2 = str2 + " " + writer2.ToString();
                            }
                            num += -1;
                            break;
                        }
                        svo.MediaDetails[num2].DetailedID = num;
                        svo.MediaDetails[num2].StatusID = svo.MediaDetails[num2].SelectedStatus.ID;
                        num2++;
                    }
                }
                table = ListToDataTable<FileUpload>(nvo.Thawing.FUSetting);
                table.TableName = "FUSettingDetails";
                using (StringWriter writer3 = new StringWriter())
                {
                    table.WriteXml(writer3);
                    str3 = writer3.ToString();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, nvo.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.Thawing.Date);
                this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, nvo.Thawing.LabPerseonID);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Thawing.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "ThawingDetails", DbType.Xml, str);
                this.dbServer.AddInParameter(storedProcCommand, "MediaDetails", DbType.Xml, str2);
                this.dbServer.AddInParameter(storedProcCommand, "FUSettingDetails", DbType.Xml, str3);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsThawingDetailsVO svo2 in nvo.Thawing.ThawingDetails)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateThawingStatusForVitrification");
                    this.dbServer.AddInParameter(command2, "EmbNo", DbType.String, svo2.EmbNo);
                    this.dbServer.AddInParameter(command2, "Plan", DbType.Boolean, svo2.Plan);
                    this.dbServer.ExecuteNonQuery(command2);
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.Thawing = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateVitrificationBizActionVO nvo = valueObject as clsAddUpdateVitrificationBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                string str;
                string str3;
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_AddUpdateVitrificationDetails");
                DataTable table = new DataTable();
                table = ListToDataTable<clsGetVitrificationDetailsVO>(nvo.Vitrification.VitrificationDetails);
                table.TableName = "VitrificationDetails";
                using (StringWriter writer = new StringWriter())
                {
                    table.WriteXml(writer);
                    str = writer.ToString();
                }
                string str2 = "";
                int num = -1;
                table = new DataTable();
                foreach (clsGetVitrificationDetailsVO svo in nvo.Vitrification.VitrificationDetails)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= svo.MediaDetails.Count)
                        {
                            table = ListToDataTable<clsFemaleMediaDetailsVO>(svo.MediaDetails);
                            using (StringWriter writer2 = new StringWriter())
                            {
                                table.WriteXml(writer2);
                                str2 = str2 + " " + writer2.ToString();
                            }
                            num += -1;
                            break;
                        }
                        svo.MediaDetails[num2].DetailedID = num;
                        svo.MediaDetails[num2].StatusID = svo.MediaDetails[num2].SelectedStatus.ID;
                        num2++;
                    }
                }
                table = ListToDataTable<FileUpload>(nvo.Vitrification.FUSetting);
                table.TableName = "FUSettingDetails";
                using (StringWriter writer3 = new StringWriter())
                {
                    table.WriteXml(writer3);
                    str3 = writer3.ToString();
                }
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitId", DbType.Int64, nvo.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "VitrificationNo", DbType.String, nvo.Vitrification.VitrificationNo);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.Vitrification.VitrificationDate);
                this.dbServer.AddInParameter(storedProcCommand, "PickUpDate", DbType.DateTime, nvo.Vitrification.PickupDate);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentForm", DbType.Boolean, nvo.Vitrification.ConsentForm);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.Vitrification.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsOnlyVitrification", DbType.Boolean, nvo.Vitrification.IsOnlyVitrification);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Vitrification.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "VitrificationDetails", DbType.Xml, str);
                this.dbServer.AddInParameter(storedProcCommand, "MediaDetails", DbType.Xml, str2);
                this.dbServer.AddInParameter(storedProcCommand, "FUSettingDetails", DbType.Xml, str3);
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
                nvo.Vitrification = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public clsTherapyANCVO ANCVisit(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            return new clsTherapyANCVO { 
                ANCID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"])),
                ANCDate = DALHelper.HandleDate(reader["ANCDate"]),
                Findings = Convert.ToString(DALHelper.HandleDBNull(reader["Findings"])),
                POG = Convert.ToString(DALHelper.HandleDBNull(reader["POG"])),
                USGReproductive = Convert.ToString(DALHelper.HandleDBNull(reader["USGReproductive"])),
                Investigation = Convert.ToString(DALHelper.HandleDBNull(reader["Investigations"])),
                Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]))
            };
        }

        public clsFollicularMonitoring FollicularMonitoring(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
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
                EndometriumThickness = Convert.ToString(DALHelper.HandleDBNull(reader["EndometriumThickness"]))
            };
        }

        public override IValueObject GetCoupleDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCoupleDetailsBizActionVO nvo = valueObject as clsGetCoupleDetailsBizActionVO;
            nvo.CoupleDetails = new clsCoupleVO();
            nvo.CoupleDetails.MalePatient = new clsPatientGeneralVO();
            nvo.CoupleDetails.FemalePatient = new clsPatientGeneralVO();
            nvo.AllCoupleDetails = new List<clsCoupleVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCoupleDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAllCoupleDetails", DbType.Int64, nvo.IsAllCouple);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (nvo.IsAllCouple)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsCoupleVO item = new clsCoupleVO {
                                CoupleRegNo = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleRegNo"])),
                                CoupleRegDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["CoupleRegDate"]))),
                                CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleId"])),
                                CoupleUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]))
                            };
                            clsPatientGeneralVO lvo3 = new clsPatientGeneralVO {
                                PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"])),
                                UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"])),
                                MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["FemalePatientMRNO"])),
                                RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["FemalePatientRegDate"]))),
                                FirstName = Security.base64Decode(reader["FemaleFirstName"].ToString()),
                                MiddleName = Security.base64Decode(reader["FemaleMiddleName"].ToString()),
                                LastName = Security.base64Decode(reader["FemaleLastName"].ToString()),
                                DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["FemaleDOB"]))),
                                Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleAlerts"]))
                            };
                            item.FemalePatient = lvo3;
                            clsPatientGeneralVO lvo4 = new clsPatientGeneralVO {
                                PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleId"])),
                                UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MAleUnitID"])),
                                MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MaleMRNO"])),
                                RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["MaleRegDate"]))),
                                FirstName = Security.base64Decode(reader["MaleFirstName"].ToString()),
                                MiddleName = Security.base64Decode(reader["MaleMiddleName"].ToString()),
                                LastName = Security.base64Decode(reader["MaleLastName"].ToString()),
                                DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["MaleDOB"]))),
                                Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["MaleAlerts"]))
                            };
                            item.MalePatient = lvo4;
                            nvo.AllCoupleDetails.Add(item);
                        }
                    }
                }
                else if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.CoupleDetails.CoupleRegNo = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleRegNo"]));
                        nvo.CoupleDetails.CoupleRegDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["CoupleRegDate"])));
                        nvo.CoupleDetails.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleId"]));
                        nvo.CoupleDetails.CoupleUnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        clsPatientGeneralVO lvo = new clsPatientGeneralVO {
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["FemalePatientMRNO"])),
                            RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["FemalePatientRegDate"]))),
                            FirstName = Security.base64Decode(reader["FemaleFirstName"].ToString()),
                            MiddleName = Security.base64Decode(reader["FemaleMiddleName"].ToString()),
                            LastName = Security.base64Decode(reader["FemaleLastName"].ToString()),
                            DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["FemaleDOB"]))),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["FemalePhoto"])
                        };
                        string str = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleImgPath"]));
                        string[] strArray = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str };
                        lvo.ImageName = string.Concat(strArray);
                        lvo.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleAlerts"]));
                        lvo.ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["FemaleContactNo"]));
                        lvo.Email = Security.base64Decode(reader["FemaleEmail"].ToString());
                        lvo.AddressLine1 = Security.base64Decode(reader["FemaleAddressLine"].ToString());
                        lvo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                        lvo.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));
                        lvo.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));
                        nvo.CoupleDetails.FemalePatient = lvo;
                        clsPatientGeneralVO lvo2 = new clsPatientGeneralVO {
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleId"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MAleUnitID"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MaleMRNO"])),
                            RegistrationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["MaleRegDate"]))),
                            FirstName = Security.base64Decode(reader["MaleFirstName"].ToString()),
                            MiddleName = Security.base64Decode(reader["MaleMiddleName"].ToString()),
                            LastName = Security.base64Decode(reader["MaleLastName"].ToString()),
                            DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["MaleDOB"]))),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["MalePhoto"])
                        };
                        string str2 = Convert.ToString(DALHelper.HandleDBNull(reader["MaleImgPath"]));
                        string[] strArray2 = new string[] { "https://", this.ImgIP, "/", this.ImgVirtualDir, "/", str2 };
                        lvo2.ImageName = string.Concat(strArray2);
                        lvo2.Alerts = Convert.ToString(DALHelper.HandleDBNull(reader["MaleAlerts"]));
                        lvo2.ContactNO1 = Convert.ToString(DALHelper.HandleDBNull(reader["MaleContactNo"]));
                        lvo2.Email = Security.base64Decode(reader["MaleEmail"].ToString());
                        lvo2.AddressLine1 = Security.base64Decode(reader["MaleAddressLine"].ToString());
                        lvo.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferralDoctorName"]));
                        lvo.SourceofReference = Convert.ToString(DALHelper.HandleDBNull(reader["SourceofReference"]));
                        lvo.Camp = Convert.ToString(DALHelper.HandleDBNull(reader["Camp"]));
                        nvo.CoupleDetails.MalePatient = lvo2;
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

        public override IValueObject GetCoupleHeightAndWeight(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO nvo = valueObject as clsGetGetCoupleHeightAndWeightBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCoupleHeightAndWeight");
                this.dbServer.AddInParameter(storedProcCommand, "FemalePatientID", DbType.Int64, nvo.FemalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "MalePatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "FemalePatientUnitID", DbType.Int64, nvo.FemalePatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MalePatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
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
                                    while (reader.Read())
                                    {
                                        nvo.CoupleDetails.MalePatient.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleHeight"]));
                                        nvo.CoupleDetails.MalePatient.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["MaleWeight"]));
                                        nvo.CoupleDetails.MalePatient.BMI = nvo.CoupleDetails.MalePatient.Weight / ((nvo.CoupleDetails.MalePatient.Height * nvo.CoupleDetails.MalePatient.Height) / 10000.0);
                                        if (double.IsNaN(nvo.CoupleDetails.MalePatient.BMI))
                                        {
                                            nvo.CoupleDetails.MalePatient.BMI = 0.0;
                                        }
                                        if (double.IsInfinity(nvo.CoupleDetails.MalePatient.BMI))
                                        {
                                            nvo.CoupleDetails.MalePatient.BMI = 0.0;
                                        }
                                    }
                                    break;
                                }
                                nvo.CoupleDetails.FemalePatient.Height = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemaleHeight"]));
                                nvo.CoupleDetails.FemalePatient.Weight = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemaleWeight"]));
                                nvo.CoupleDetails.FemalePatient.BMI = nvo.CoupleDetails.FemalePatient.Weight / ((nvo.CoupleDetails.FemalePatient.Height * nvo.CoupleDetails.FemalePatient.Height) / 10000.0);
                                if (double.IsNaN(nvo.CoupleDetails.FemalePatient.BMI))
                                {
                                    nvo.CoupleDetails.FemalePatient.BMI = 0.0;
                                }
                                if (double.IsInfinity(nvo.CoupleDetails.FemalePatient.BMI))
                                {
                                    nvo.CoupleDetails.FemalePatient.BMI = 0.0;
                                }
                            }
                            break;
                        }
                        DALHelper.HandleDBNull(reader["DummyID"]);
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

        public override IValueObject getETDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetETDetailsBizActionVO nvo = valueObject as clsGetETDetailsBizActionVO;
            nvo.ET = new ETVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetETDetailsSavedInLabDays");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.String, nvo.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEdit", DbType.Boolean, nvo.IsEdit);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FromID", DbType.String, nvo.FromID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (!nvo.IsEdit)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ETDetailsVO item = new ETDetailsVO {
                                TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["LabNo"])),
                                LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"])),
                                EmbNO = Convert.ToString(DALHelper.HandleDBNull(reader["OoNo"])),
                                SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                                Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                                FertilizationStage = Convert.ToString(DALHelper.HandleDBNull(reader["FertilisationStage"])),
                                TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]))),
                                Score = Convert.ToString(DALHelper.HandleDBNull(reader["Score"]))
                            };
                            nvo.ET.ETDetails.Add(item);
                        }
                    }
                }
                else if ((nvo.ID == 0L) && (nvo.UnitID == 0L))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                        }
                    }
                }
                else
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            nvo.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            nvo.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            nvo.ET.Date = DALHelper.HandleDate(reader["Date"]);
                            nvo.ET.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                            nvo.ET.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                            nvo.ET.AnasthesistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                            nvo.ET.AssAnasthesistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                            nvo.ET.EndometriumThickness = Convert.ToInt64(DALHelper.HandleDBNull(reader["EndometriumThickness"]));
                            nvo.ET.ETPattern = Convert.ToInt64(DALHelper.HandleDBNull(reader["ETPattern"]));
                            nvo.ET.ColorDopper = Convert.ToString(DALHelper.HandleDBNull(reader["ColorDopper"]));
                            nvo.ET.IsEndometerialPI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEndometerialPI"]));
                            nvo.ET.IsEndometerialRI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEndometerialRI"]));
                            nvo.ET.IsEndometerialSD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEndometerialSD"]));
                            nvo.ET.IsUterinePI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUterinePI"]));
                            nvo.ET.IsUterineRI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUterineRI"]));
                            nvo.ET.IsUterineSD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsUterineSD"]));
                            nvo.ET.IsTreatmentUnderGA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTreatmentUnderGA"]));
                            nvo.ET.CatheterTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CatheterTypeID"]));
                            nvo.ET.Difficulty = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Difficulty"]));
                            nvo.ET.DifficultyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyTypeID"]));
                            nvo.ET.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                            nvo.ET.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                            nvo.ET.TenaculumUsed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TenaculumUsed"]));
                            nvo.ET.DistanceFromFundus = Convert.ToString(DALHelper.HandleDBNull(reader["DistancefromFundus"]));
                            nvo.ET.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                            nvo.ET.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                            nvo.ET.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                            nvo.ET.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ETDetailsVO item = new ETDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                ETUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ETUnitID"])),
                                ETID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ETID"])),
                                LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"])),
                                EmbNO = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNO"])),
                                SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                                TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                                TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                                Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                                GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Grade"])),
                                FertilizationStage = Convert.ToString(DALHelper.HandleDBNull(reader["FertilizationStage"])),
                                FertilizationStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationStage"])),
                                Score = Convert.ToString(DALHelper.HandleDBNull(reader["Score"])),
                                EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                PlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"])),
                                FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                FileContents = Convert.FromBase64String(reader["Data"].ToString())
                            };
                            nvo.ET.ETDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            FileUpload item = new FileUpload {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                Index = Convert.ToInt16(DALHelper.HandleDBNull(reader["FileIndex"])),
                                Data = Convert.FromBase64String(reader["Data"].ToString())
                            };
                            nvo.ET.FUSetting.Add(item);
                        }
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

        public override IValueObject GetFollicularModified(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetFollicularModifiedDetailsBizActionVO nvo = valueObject as clsGetFollicularModifiedDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetFollicularModified");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.FollicularID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.FollicularMonitoringDetial.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            throw new NotImplementedException();
        }

        public override IValueObject GetFollicularMonitoringSizeList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetFollicularMonitoringSizeDetailsBizActionVO nvo = valueObject as clsGetFollicularMonitoringSizeDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_GetFollicularMonitoringSizeList");
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

        public override IValueObject GetPatientEMRDetailsList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetEMRDetailsBizActionVO nvo = valueObject as clsGetEMRDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetEMRDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
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
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private string GetPropertyValue(string pName, clsTherapyExecutionVO control)
        {
            object[] args = null;
            return control.GetType().InvokeMember(pName, BindingFlags.GetProperty, null, control, args).ToString();
        }

        public override IValueObject GetSpermThawingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject GetSpermVitrificationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSpermVitrificationBizActionVO nvo = valueObject as clsGetSpermVitrificationBizActionVO;
            nvo.Vitrification = new clsSpermVitrifiactionVO();
            try
            {
                DbCommand storedProcCommand = null;
                if (!nvo.IsDonor)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetSpermVirtificationDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, nvo.CoupleID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.String, nvo.CoupleUintID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                }
                else if (nvo.IsDonor)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetDonorSpermVirtificationDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.String, nvo.PatientUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                }
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsSpermVitrificationDetailsVO item = new clsSpermVitrificationDetailsVO {
                            Volume = Convert.ToString(DALHelper.HandleDBNull(reader["Volume"])),
                            SpermCount = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCount"])),
                            Motillity = Convert.ToString(DALHelper.HandleDBNull(reader["Motility"])),
                            CanID = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"])),
                            CanisterNo = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            FreezingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["dtpFreezingDate"])))
                        };
                        if (item.FreezingDate != null)
                        {
                            item.FreezingDate = new DateTime?(item.FreezingDate.Value.Date);
                        }
                        item.FreezingTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["dtpFreezingTime"])));
                        item.FreezingTime = new DateTime?(item.FreezingTime.Value.ToLocalTime());
                        item.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        item.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        item.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        item.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        item.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingNo"]));
                        item.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["GobletColor"]));
                        item.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["FreezingComments"]));
                        nvo.Vitrification.VitrificationDetails.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsSpermThawingDetailsVO item = new clsSpermThawingDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Motillity = Convert.ToString(DALHelper.HandleDBNull(reader["Motility"])),
                            SpermCount = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCount"])),
                            ThawingDate = (DateTime?) DALHelper.HandleDBNull(reader["ThawingDate"]),
                            ThawingTime = (DateTime?) DALHelper.HandleDBNull(reader["ThawingTime"]),
                            PostThawPlanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PostThawPlanId"])),
                            PostThawPlan = Convert.ToString(DALHelper.HandleDBNull(reader["PostThawPlan"])),
                            VitrificationNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrificationID"])),
                            Volume = Convert.ToString(DALHelper.HandleDBNull(reader["Volume"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            LabInchargeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"])),
                            LabInchargeName = Convert.ToString(DALHelper.HandleDBNull(reader["LabPersonName"]))
                        };
                        nvo.Vitrification.ThawingDetails.Add(item);
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

        public override IValueObject getThawingDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetThawingDetailsBizActionVO BizAction = valueObject as clsGetThawingDetailsBizActionVO;
            BizAction.Thawing = new clsThawingVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetThawingDetails");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, BizAction.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizAction.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, BizAction.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (!BizAction.IsEdit)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                        }
                    }
                }
                else
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsGetVitrificationDetailsVO item = new clsGetVitrificationDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"])),
                                TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                                EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"])),
                                SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                                Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                                SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"])),
                                SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"])),
                                OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"])),
                                SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"])),
                                ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"])),
                                CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                                TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                                CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"])),
                                StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawId"])),
                                GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShapeId"])),
                                GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSizeId"])),
                                CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"])),
                                TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankId"])),
                                ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                                LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"])),
                                Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                                Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                                ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"])),
                                EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]))
                            };
                            System.Drawing.Color color = ColorTranslator.FromHtml(item.ColorCode);
                            item.SelectesColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                            BizAction.VitrificationDetails.Add(item);
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
                            BizAction.Thawing.Date = DALHelper.HandleDate(reader["DateTime"]);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsThawingDetailsVO item = new clsThawingDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                VitrificationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrificationID"])),
                                CellStangeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"])),
                                Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                                Date = DALHelper.HandleDate(reader["Date"]),
                                EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"])),
                                SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                                GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                                Plan = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Plan"])),
                                Status = Convert.ToString(DALHelper.HandleDBNull(reader["Status"]))
                            };
                            item.SelectedCellStage.ID = item.CellStangeID;
                            item.SelectedGrade.ID = item.GradeID;
                            BizAction.Thawing.ThawingDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                Func<clsFemaleMediaDetailsVO, bool> predicate = null;
                                for (int i = 0; i < BizAction.Thawing.ThawingDetails.Count; i++)
                                {
                                    if (predicate == null)
                                    {
                                        predicate = p => p.DetailedID == BizAction.Thawing.ThawingDetails[i].ID;
                                    }
                                    BizAction.Thawing.ThawingDetails[i].MediaDetails = BizAction.MediaDetails.Where<clsFemaleMediaDetailsVO>(predicate).ToList<clsFemaleMediaDetailsVO>();
                                }
                                break;
                            }
                            clsFemaleMediaDetailsVO Mediadetails = new clsFemaleMediaDetailsVO {
                                BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["LotNo"])),
                                BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                                Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"])),
                                Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                                DetailedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailID"])),
                                ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                                VolumeUsed = Convert.ToString(DALHelper.HandleDBNull(reader["VolumeUsed"])),
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                ItemName = Convert.ToString(reader["MediaName"]),
                                MianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteID"])),
                                OSM = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OSM"])),
                                PH = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PH"])),
                                StatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"])),
                                StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                                SelectedStatus = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"])) }
                            };
                            Mediadetails.SelectedStatus = Mediadetails.Status.FirstOrDefault<MasterListItem>(q => q.ID == Mediadetails.SelectedStatus.ID);
                            BizAction.MediaDetails.Add(Mediadetails);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            FileUpload item = new FileUpload {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                Index = Convert.ToInt16(DALHelper.HandleDBNull(reader["FileIndex"])),
                                Data = Convert.FromBase64String(reader["Data"].ToString())
                            };
                            BizAction.Thawing.FUSetting.Add(item);
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetTherapyDashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTherapyDetailsForDashBoardBizActionVO nvo = valueObject as clsGetTherapyDetailsForDashBoardBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientDashBoardAlerts_temp1");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.Int32, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate2", DbType.DateTime, nvo.FromDate2);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate2", DbType.DateTime, nvo.ToDate2);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.LastName);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TherapyDetailsList == null)
                    {
                        nvo.TherapyDetailsList = new List<clsTherapyDashBoardVO>();
                    }
                    while (reader.Read())
                    {
                        clsTherapyDashBoardVO item = new clsTherapyDashBoardVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["DateTime"]);
                        item.TherapyDate = new DateTime?(nullable.Value);
                        item.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyId"]));
                        item.EventTypeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["EventTypeId"]));
                        item.MrNo = (string) DALHelper.HandleDBNull(reader["MRNo"]);
                        item.FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"]));
                        item.MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]));
                        item.ContactNo1 = (string) DALHelper.HandleDBNull(reader["ContactNo1"]);
                        item.CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleRgistrationNumber"]));
                        if (item.EventTypeId == 5L)
                        {
                            item.Procedure = "Embryo Transfer";
                        }
                        else if (item.EventTypeId == 4L)
                        {
                            item.Procedure = "Ovum Pick Up";
                        }
                        nvo.TherapyDetailsList.Add(item);
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

        public override IValueObject GetTherapyDetailList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTherapyListBizActionVO bizAction = valueObject as clsGetTherapyListBizActionVO;
            if (!bizAction.TherapyDetails.IsSurrogate)
            {
                bizAction = this.GetTherapyDetailList(bizAction, objUserVO);
            }
            else
            {
                bizAction = this.GetTherapyDetailList(bizAction, objUserVO);
                bizAction = this.GetTherapyDetailsSurrogate(bizAction, objUserVO);
            }
            return valueObject;
        }

        private clsGetTherapyListBizActionVO GetTherapyDetailList(clsGetTherapyListBizActionVO BizAction, clsUserVO objUserVo)
        {
            this.dbServer.CreateConnection();
            try
            {
                if (!BizAction.Flag)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_GetTherapyListForLabDay");
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizAction.CoupleID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, BizAction.CoupleUintID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, BizAction.TherapyUnitID);
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
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_GetTherapyList");
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizAction.CoupleID);
                    this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, BizAction.TherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "TabID", DbType.Int64, BizAction.TabID);
                    this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, BizAction.CoupleUintID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, BizAction.TherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlannedTreatmentID", DbType.Int64, BizAction.PlannedTreatmentID);
                    this.dbServer.AddInParameter(storedProcCommand, "ProtocolTypeID", DbType.Int64, BizAction.ProtocolTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "PhysicianId", DbType.Int64, BizAction.PhysicianId);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoActive", DbType.Boolean, BizAction.rdoActive);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoAll", DbType.Boolean, BizAction.rdoAll);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoClosed", DbType.Boolean, BizAction.rdoClosed);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoSuccessful", DbType.Boolean, BizAction.rdoSuccessful);
                    this.dbServer.AddInParameter(storedProcCommand, "rdoUnsuccessful", DbType.Boolean, BizAction.rdoUnsuccessful);
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
                                    BizAction.TherapyDocument.Add(this.TherapyDocuments(BizAction, reader));
                                }
                            }
                            reader.NextResult();
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
                        }
                        else if (BizAction.TabID == 4L)
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyDocument.Add(this.TherapyDocuments(BizAction, reader));
                                }
                            }
                        }
                        else if (BizAction.TabID == 2L)
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BizAction.TherapyExecutionList.Add(this.TherapyExcecution(BizAction, reader));
                                }
                            }
                        }
                        else if ((BizAction.TabID == 3L) && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.FollicularMonitoringList.Add(this.FollicularMonitoring(BizAction, reader));
                            }
                        }
                    }
                    else if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsPlanTherapyVO item = new clsPlanTherapyVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"])),
                                TherapyStartDate = DALHelper.HandleDate(reader["TherapyStartDate"]),
                                CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"])),
                                Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"])),
                                ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"])),
                                Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"])),
                                PillStartDate = DALHelper.HandleDate(reader["PillStartDate"]),
                                PillEndDate = DALHelper.HandleDate(reader["PillEndDate"]),
                                PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"])),
                                MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"])),
                                PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"])),
                                ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"])),
                                PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"])),
                                TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"])),
                                LutealSupport = Convert.ToString(DALHelper.HandleDBNull(reader["LutealSupport"])),
                                LutealRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["LutealRemarks"])),
                                BHCGAss1Date = DALHelper.HandleDate(reader["BHCGAss1Date"]),
                                BHCGAss1IsBSCG = (bool?) DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"])
                            };
                            if (item.BHCGAss1IsBSCG != null)
                            {
                                if (item.BHCGAss1IsBSCG.Value)
                                {
                                    item.BHCGAss1IsBSCGPositive = true;
                                }
                                else
                                {
                                    item.BHCGAss1IsBSCGNagative = true;
                                }
                            }
                            item.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1BSCGValue"]));
                            item.BHCGAss1SrProgest = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1SrProgest"]));
                            item.BHCGAss2Date = DALHelper.HandleDate(reader["BHCGAss2Date"]);
                            item.BHCGAss2IsBSCG = (bool?) DALHelper.HandleDBNull(reader["BHCGAss2IsBSCG"]);
                            if (item.BHCGAss2IsBSCG != null)
                            {
                                if (item.BHCGAss2IsBSCG.Value)
                                {
                                    item.BHCGAss2IsBSCGPostive = true;
                                }
                                else
                                {
                                    item.BHCGAss2IsBSCGNagative = true;
                                }
                            }
                            item.BHCGAss2BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2BSCGValue"]));
                            item.BHCGAss2USG = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2USG"]));
                            item.IsPregnancyAchieved = (bool?) DALHelper.HandleDBNull(reader["PregnancyAchieved"]);
                            if (item.IsPregnancyAchieved != null)
                            {
                                if (item.IsPregnancyAchieved.Value)
                                {
                                    item.IsPregnancyAchievedPostive = true;
                                }
                                else
                                {
                                    item.IsPregnancyAchievedNegative = true;
                                }
                            }
                            item.PregnanacyConfirmDate = DALHelper.HandleDate(reader["PregnanacyConfirmDate"]);
                            item.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                            item.OutComeRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["OutComeRemarks"]));
                            item.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                            item.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                            item.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                            item.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                            item.FetalHeartSound = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FetalHeartSound"]));
                            item.OHSSEarly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSEarly"]));
                            item.OHSSLate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSLate"]));
                            item.OHSSMild = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMild"]));
                            item.OHSSMode = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMode"]));
                            item.OHSSRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OHSSRemark"]));
                            item.OHSSSereve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSSereve"]));
                            item.SIXmonthFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID"]));
                            item.SIXmonthFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID_m"]));
                            item.SIXmonthFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks"]));
                            item.SIXmonthFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks_m"]));
                            item.ONEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID"]));
                            item.ONEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID_m"]));
                            item.ONEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks"]));
                            item.ONEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks_m"]));
                            item.FIVEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID"]));
                            item.FIVEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID_m"]));
                            item.FIVEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks"]));
                            item.FIVEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks_m"]));
                            item.TENyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID"]));
                            item.TENyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID_m"]));
                            item.TENyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks"]));
                            item.TENyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks_m"]));
                            item.TWENTYyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID"]));
                            item.TWENTYyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID_m"]));
                            item.TWENTYyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks"]));
                            item.TWENTYyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks_m"]));
                            item.Missed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Missed"]));
                            item.Incomplete = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Incomplete"]));
                            item.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                            item.FetalDate = DALHelper.HandleDate(reader["FetalDate"]);
                            item.Count = Convert.ToString(DALHelper.HandleDBNull(reader["Count"]));
                            item.IUD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IUD"]));
                            item.LiveBirth = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LiveBirth"]));
                            item.Congenitalabnormality = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Congenitalabnormality"]));
                            item.IsChemicalPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChemicalPregnancy"]));
                            item.PretermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PretermDelivery"]));
                            item.IsFullTermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFullTermDelivery"]));
                            item.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));
                            item.BiochemPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BiochemPregnancy"]));
                            item.Ectopic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ectopic"]));
                            item.Abortion = item.Missed || item.Incomplete;
                            item.PCOS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PCOS"]));
                            item.Hypogonadotropic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypogonadotropic"]));
                            item.Tuberculosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tuberculosis"]));
                            item.Endometriosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometriosis"]));
                            item.UterineFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineFactors"]));
                            item.TubalFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TubalFactors"]));
                            item.DiminishedOvarian = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DiminishedOvarian"]));
                            item.PrematureOvarianFailure = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrematureOvarianFailure"]));
                            item.LutealPhasedefect = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LutealPhasedefect"]));
                            item.HypoThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HypoThyroid"]));
                            item.MaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MaleFactors"]));
                            item.OtherFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OtherFactors"]));
                            item.UnknownFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnknownFactors"]));
                            item.FemaleFactorsOnly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleFactorsOnly"]));
                            item.FemaleandMaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleandMaleFactors"]));
                            item.HyperThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HyperThyroid"]));
                            item.OPUtDate = DALHelper.HandleDate(reader["OPUDate"]);
                            item.OPURemark = Convert.ToString(DALHelper.HandleDBNull(reader["OPURemark"]));
                            item.PlannedEmbryos = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"]));
                            item.LongtermMedication = Convert.ToString(DALHelper.HandleDBNull(reader["LongtermMedication"]));
                            item.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["AssistedHatching"]));
                            item.CryoPreservation = Convert.ToBoolean(DALHelper.HandleDBNull(reader["CryoPreservation"]));
                            item.IMSI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IMSI"]));
                            BizAction.TherapyDetailsList.Add(item);
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return BizAction;
        }

        private clsGetTherapyListBizActionVO GetTherapyDetailsSurrogate(clsGetTherapyListBizActionVO BizAction, clsUserVO objUserVO)
        {
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_GetTherapyListSurrogate");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleId", DbType.Int64, BizAction.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "TherapyID", DbType.Int64, BizAction.TherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "TabID", DbType.Int64, BizAction.TabID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.Int64, BizAction.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, BizAction.TherapyUnitID);
                BizAction.TherapyDetailsList = new List<clsPlanTherapyVO>();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (BizAction.TherapyID != 0L)
                {
                    if (BizAction.TabID == 0L)
                    {
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.TherapyExecutionListSurrogate.Add(this.TherapyExcecutionSurrogate(BizAction, reader));
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (BizAction.ANCList == null)
                                {
                                    continue;
                                }
                                BizAction.ANCList.Add(this.ANCVisit(BizAction, reader));
                            }
                        }
                        reader.NextResult();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                clsTherapyDeliveryVO yvo2 = new clsTherapyDeliveryVO {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                    TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                                    ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"])),
                                    DeliveryDate = DALHelper.HandleDate(reader["Date"]),
                                    Baby = Convert.ToString(DALHelper.HandleDBNull(reader["Baby"])),
                                    Mode = Convert.ToString(DALHelper.HandleDBNull(reader["Mode"])),
                                    TimeofBirth = DALHelper.HandleDate(reader["TimeofBirth"]),
                                    Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]))
                                };
                                BizAction.TherapyDelivery = yvo2;
                            }
                        }
                    }
                    else if (BizAction.TabID == 2L)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.TherapyExecutionListSurrogate.Add(this.TherapyExcecutionSurrogate(BizAction, reader));
                            }
                        }
                    }
                    else if (BizAction.TabID == 7L)
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BizAction.ANCList.Add(this.ANCVisit(BizAction, reader));
                            }
                        }
                    }
                    else if ((BizAction.TabID == 8L) && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsTherapyDeliveryVO yvo3 = new clsTherapyDeliveryVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                                ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"])),
                                DeliveryDate = DALHelper.HandleDate(reader["Date"]),
                                Baby = Convert.ToString(DALHelper.HandleDBNull(reader["Baby"])),
                                Mode = Convert.ToString(DALHelper.HandleDBNull(reader["Mode"])),
                                TimeofBirth = DALHelper.HandleDate(reader["TimeofBirth"]),
                                Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]))
                            };
                            BizAction.TherapyDelivery = yvo3;
                        }
                    }
                }
                else if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPlanTherapyVO item = new clsPlanTherapyVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            CoupleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"])),
                            TherapyStartDate = DALHelper.HandleDate(reader["TherapyStartDate"]),
                            CycleDuration = Convert.ToString(DALHelper.HandleDBNull(reader["CycleDuration"])),
                            Cyclecode = Convert.ToString(DALHelper.HandleDBNull(reader["Cyclecode"])),
                            SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"])),
                            SurrogateMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMRNo"])),
                            ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"])),
                            Pill = Convert.ToString(DALHelper.HandleDBNull(reader["Pill"])),
                            PillStartDate = DALHelper.HandleDate(reader["PillStartDate"]),
                            PillEndDate = DALHelper.HandleDate(reader["PillEndDate"]),
                            PlannedTreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentID"])),
                            PlannedNoofEmbryos = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedNoofEmbryos"])),
                            MainInductionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainInductionID"])),
                            PhysicianId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicianId"])),
                            ExternalSimulationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExternalSimulationID"])),
                            PlannedSpermCollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedSpermCollectionID"])),
                            TherapyNotes = Convert.ToString(DALHelper.HandleDBNull(reader["TherapyGeneralNotes"])),
                            LutealSupport = Convert.ToString(DALHelper.HandleDBNull(reader["LutealSupport"])),
                            LutealRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["LutealRemarks"])),
                            BHCGAss1Date = DALHelper.HandleDate(reader["BHCGAss1Date"]),
                            BHCGAss1IsBSCG = (bool?) DALHelper.HandleDBNull(reader["BHCGAss1IsBSCG"])
                        };
                        if (item.BHCGAss1IsBSCG != null)
                        {
                            if (item.BHCGAss1IsBSCG.Value)
                            {
                                item.BHCGAss1IsBSCGPositive = true;
                            }
                            else
                            {
                                item.BHCGAss1IsBSCGNagative = true;
                            }
                        }
                        item.BHCGAss1BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1BSCGValue"]));
                        item.BHCGAss1SrProgest = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss1SrProgest"]));
                        item.BHCGAss2Date = DALHelper.HandleDate(reader["BHCGAss2Date"]);
                        item.BHCGAss2IsBSCG = (bool?) DALHelper.HandleDBNull(reader["BHCGAss2IsBSCG"]);
                        if (item.BHCGAss2IsBSCG != null)
                        {
                            if (item.BHCGAss2IsBSCG.Value)
                            {
                                item.BHCGAss2IsBSCGPostive = true;
                            }
                            else
                            {
                                item.BHCGAss2IsBSCGNagative = true;
                            }
                        }
                        item.BHCGAss2BSCGValue = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2BSCGValue"]));
                        item.BHCGAss2USG = Convert.ToString(DALHelper.HandleDBNull(reader["BHCGAss2USG"]));
                        item.IsPregnancyAchieved = (bool?) DALHelper.HandleDBNull(reader["PregnancyAchieved"]);
                        if (item.IsPregnancyAchieved != null)
                        {
                            if (item.IsPregnancyAchieved.Value)
                            {
                                item.IsPregnancyAchievedPostive = true;
                            }
                            else
                            {
                                item.IsPregnancyAchievedNegative = true;
                            }
                        }
                        item.PregnanacyConfirmDate = DALHelper.HandleDate(reader["PregnanacyConfirmDate"]);
                        item.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        item.OutComeRemarks = Convert.ToString(DALHelper.HandleDBNull(reader["OutComeRemarks"]));
                        item.Physician = Convert.ToString(DALHelper.HandleDBNull(reader["Physician"]));
                        item.PlannedTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["PlannedTreatment"]));
                        item.SpermCollection = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCollection"]));
                        item.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                        item.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogate"]));
                        item.OHSSEarly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSEarly"]));
                        item.OHSSLate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSLate"]));
                        item.OHSSMild = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMild"]));
                        item.OHSSMode = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSMode"]));
                        item.OHSSRemark = Convert.ToString(DALHelper.HandleDBNull(reader["OHSSRemark"]));
                        item.OHSSSereve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OHSSSereve"]));
                        item.SIXmonthFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID"]));
                        item.SIXmonthFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["SIX_monthFitnessID_m"]));
                        item.SIXmonthFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks"]));
                        item.SIXmonthFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["SIX_monthFitnessRemarks_m"]));
                        item.ONEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID"]));
                        item.ONEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["ONE_yearFitnessID_m"]));
                        item.ONEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks"]));
                        item.ONEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["ONE_yearFitnessRemarks_m"]));
                        item.FIVEyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID"]));
                        item.FIVEyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["FIVE_yearFitnessID_m"]));
                        item.FIVEyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks"]));
                        item.FIVEyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["FIVE_yearFitnessRemarks_m"]));
                        item.TENyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID"]));
                        item.TENyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TEN_yearFitnessID_m"]));
                        item.TENyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks"]));
                        item.TENyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TEN_yearFitnessRemarks_m"]));
                        item.TWENTYyFitnessID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID"]));
                        item.TWENTYyFitnessID_m = Convert.ToInt64(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessID_m"]));
                        item.TWENTYyFitnessRemark = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks"]));
                        item.TWENTYyFitnessRemark_m = Convert.ToString(DALHelper.HandleDBNull(reader["TWNTY_YearFitnessRemarks_m"]));
                        item.Missed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Missed"]));
                        item.Incomplete = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Incomplete"]));
                        item.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        item.FetalDate = DALHelper.HandleDate(reader["FetalDate"]);
                        item.Count = Convert.ToString(DALHelper.HandleDBNull(reader["Count"]));
                        item.IUD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IUD"]));
                        item.LiveBirth = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LiveBirth"]));
                        item.Congenitalabnormality = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Congenitalabnormality"]));
                        item.IsChemicalPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChemicalPregnancy"]));
                        item.PretermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PretermDelivery"]));
                        item.IsFullTermDelivery = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFullTermDelivery"]));
                        item.BabyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BabyTypeID"]));
                        item.BiochemPregnancy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BiochemPregnancy"]));
                        item.Ectopic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Ectopic"]));
                        item.Abortion = item.Missed || item.Incomplete;
                        item.PCOS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PCOS"]));
                        item.Hypogonadotropic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypogonadotropic"]));
                        item.Tuberculosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tuberculosis"]));
                        item.Endometriosis = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometriosis"]));
                        item.UterineFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineFactors"]));
                        item.TubalFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TubalFactors"]));
                        item.DiminishedOvarian = Convert.ToBoolean(DALHelper.HandleDBNull(reader["DiminishedOvarian"]));
                        item.PrematureOvarianFailure = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrematureOvarianFailure"]));
                        item.LutealPhasedefect = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LutealPhasedefect"]));
                        item.HypoThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HypoThyroid"]));
                        item.MaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["MaleFactors"]));
                        item.OtherFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OtherFactors"]));
                        item.UnknownFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnknownFactors"]));
                        item.FemaleFactorsOnly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleFactorsOnly"]));
                        item.FemaleandMaleFactors = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FemaleandMaleFactors"]));
                        item.HyperThyroid = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HyperThyroid"]));
                        item.FetalHeartSound = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FetalHeartSound"]));
                        BizAction.TherapyDetailsList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return BizAction;
        }

        public override IValueObject GetTherapyDrugDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsgetTherapyDrugDetailsBizActionVO nvo = valueObject as clsgetTherapyDrugDetailsBizActionVO;
            nvo.TherapyDrugDetails = new clsTherapyDrug();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_GetTherapyDrugDetails");
                this.dbServer.AddInParameter(storedProcCommand, "TherapyExeID", DbType.Int64, nvo.TherapyExeID);
                this.dbServer.AddInParameter(storedProcCommand, "DayNo", DbType.String, nvo.DayNo);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                nvo.TherapyDrugDetails = new clsTherapyDrug();
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.TherapyDrugDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyTypeDetailId"]));
                        nvo.TherapyDrugDetails.DrugDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])));
                        nvo.TherapyDrugDetails.DrugNotes = Convert.ToString(DALHelper.HandleDBNull(reader["DrugNotes"]));
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

        public override IValueObject getVitrificationDetails(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetVitrificationDetailsBizActionVO BizAction = valueObject as clsGetVitrificationDetailsBizActionVO;
            BizAction.Vitrification = new clsGetVitrificationVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetVirtificationDetailsSavedInLabDays");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleID", DbType.Int64, BizAction.CoupleID);
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.String, BizAction.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "IsEdit", DbType.Boolean, BizAction.IsEdit);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, BizAction.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, BizAction.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FromID", DbType.String, BizAction.FromID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (!BizAction.IsEdit)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsGetVitrificationDetailsVO item = new clsGetVitrificationDetailsVO {
                                CanID = "0",
                                TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["LabNo"])),
                                LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DayID"])),
                                EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["OoNo"])),
                                SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                                Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                                SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOocyteID"])),
                                SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOfSemen"])),
                                OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteDonorID"])),
                                SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SemenDonorID"])),
                                ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["PlanTreatmentID"])),
                                CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["FertilisationStage"])),
                                TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])))
                            };
                            BizAction.Vitrification.VitrificationDetails.Add(item);
                        }
                    }
                }
                else if ((BizAction.ID == 0L) && (BizAction.UnitID == 0L))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsGetVitrificationDetailsVO item = new clsGetVitrificationDetailsVO();
                            clsThawingDetailsVO svo3 = new clsThawingDetailsVO();
                            item.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitrivicationID"]));
                            item.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                            item.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                            item.EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"]));
                            item.SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));
                            svo3.SerialOccyteNo = item.SerialOccyteNo;
                            svo3.EmbNo = item.EmbNo;
                            svo3.VitrificationID = item.ID;
                            item.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                            item.SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"]));
                            item.SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"]));
                            item.OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"]));
                            item.SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"]));
                            item.ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"]));
                            item.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                            item.TransferDate = DALHelper.HandleDate(reader["TransferDate"]);
                            item.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"]));
                            item.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawId"]));
                            item.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                            item.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                            item.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                            item.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankId"]));
                            item.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                            item.LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"]));
                            item.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                            item.Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                            item.ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"]));
                            item.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                            System.Drawing.Color color = ColorTranslator.FromHtml(item.ColorCode);
                            item.SelectesColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                            BizAction.Vitrification.VitrificationDetails.Add(item);
                            BizAction.Vitrification.ThawingDetails.Add(svo3);
                        }
                    }
                }
                else
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            BizAction.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            BizAction.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                            BizAction.Vitrification.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                            BizAction.Vitrification.VitrificationDate = DALHelper.HandleDate(reader["DateTime"]);
                            BizAction.Vitrification.PickupDate = DALHelper.HandleDate(reader["PickUpDate"]);
                            BizAction.Vitrification.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                            BizAction.Vitrification.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                            if (BizAction.Vitrification.ConsentForm)
                            {
                                BizAction.Vitrification.ConsentFormYes = true;
                                continue;
                            }
                            BizAction.Vitrification.ConsentFormNo = true;
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            clsGetVitrificationDetailsVO item = new clsGetVitrificationDetailsVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"])),
                                TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                                EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"])),
                                SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                                Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                                SOOcytes = Convert.ToString(DALHelper.HandleDBNull(reader["SOOCytes"])),
                                SOSemen = Convert.ToString(DALHelper.HandleDBNull(reader["OSSemen"])),
                                OSCode = Convert.ToString(DALHelper.HandleDBNull(reader["OSCode"])),
                                SSCode = Convert.ToString(DALHelper.HandleDBNull(reader["SSCode"])),
                                ProtocolType = Convert.ToString(DALHelper.HandleDBNull(reader["ProtocolType"])),
                                CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                                TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                                CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CanId"])),
                                StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawId"])),
                                GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShapeId"])),
                                GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSizeId"])),
                                CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"])),
                                TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankId"])),
                                ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                                LeafNo = Convert.ToString(DALHelper.HandleDBNull(reader["LeafNo"])),
                                Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                                Status = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                                ConistorNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConistorNo"])),
                                EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]))
                            };
                            System.Drawing.Color color2 = ColorTranslator.FromHtml(item.ColorCode);
                            item.SelectesColor = System.Windows.Media.Color.FromArgb(color2.A, color2.R, color2.G, color2.B);
                            BizAction.Vitrification.VitrificationDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                Func<clsFemaleMediaDetailsVO, bool> predicate = null;
                                for (int i = 0; i < BizAction.Vitrification.VitrificationDetails.Count; i++)
                                {
                                    if (predicate == null)
                                    {
                                        predicate = p => p.DetailedID == BizAction.Vitrification.VitrificationDetails[i].ID;
                                    }
                                    BizAction.Vitrification.VitrificationDetails[i].MediaDetails = BizAction.MediaDetails.Where<clsFemaleMediaDetailsVO>(predicate).ToList<clsFemaleMediaDetailsVO>();
                                }
                                break;
                            }
                            clsFemaleMediaDetailsVO vitdetails = new clsFemaleMediaDetailsVO {
                                BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["LotNo"])),
                                BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                                Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"])),
                                Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                                DetailedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DetailID"])),
                                ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                                VolumeUsed = Convert.ToString(DALHelper.HandleDBNull(reader["VolumeUsed"])),
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                ItemName = Convert.ToString(reader["MediaName"]),
                                MianID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteID"])),
                                OSM = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OSM"])),
                                PH = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PH"])),
                                StatusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"])),
                                StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                                SelectedStatus = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Status"])) }
                            };
                            vitdetails.SelectedStatus = vitdetails.Status.FirstOrDefault<MasterListItem>(q => q.ID == vitdetails.SelectedStatus.ID);
                            BizAction.MediaDetails.Add(vitdetails);
                        }
                    }
                    reader.NextResult();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            FileUpload item = new FileUpload {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                                Index = Convert.ToInt16(DALHelper.HandleDBNull(reader["FileIndex"])),
                                Data = Convert.FromBase64String(reader["Data"].ToString())
                            };
                            BizAction.Vitrification.FUSetting.Add(item);
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;
        }

        public override IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitrificationForCryoBankBizActionVO nvo = valueObject as clsGetVitrificationForCryoBankBizActionVO;
            nvo.Vitrification = new clsGetVitrificationVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetVirtificationDetailsEmbryoCryoBank");
                this.dbServer.AddInParameter(storedProcCommand, "CoupleUnitID", DbType.String, nvo.CoupleUintID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "FName", DbType.String, nvo.FName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "MName", DbType.String, nvo.MName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "LName", DbType.String, nvo.LName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.FamilyName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo + "%");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGetVitrificationDetailsVO item = new clsGetVitrificationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"])),
                            SerialOccyteNo = Convert.ToString(DALHelper.HandleDBNull(reader["SerialOccyteNo"])),
                            EmbNo = Convert.ToString(DALHelper.HandleDBNull(reader["EmbNo"])),
                            ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]))
                        };
                        System.Drawing.Color color = ColorTranslator.FromHtml(item.ColorCode);
                        item.SelectesColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                        item.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        item.TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"])));
                        item.CellStange = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        item.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        item.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        item.DoneThawing = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                        item.TankId = Convert.ToString(DALHelper.HandleDBNull(reader["TankName"]));
                        item.CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["CanisterName"]));
                        item.GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        item.GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        item.CanID = Convert.ToString(DALHelper.HandleDBNull(reader["CaneName"]));
                        item.StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["StrawName"]));
                        item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        item.LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]));
                        item.MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]));
                        item.FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        item.PatientUnitName = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationUnit"]));
                        nvo.Vitrification.VitrificationDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetVitrificationForSpermCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetSpremFreezingDetailsBizActionVO nvo = valueObject as clsGetSpremFreezingDetailsBizActionVO;
            nvo.Vitrification = new List<clsSpermFreezingVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVF_GetVirtificationDetailsSpermCryoBank");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FName", DbType.String, nvo.FName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "MName", DbType.String, nvo.MName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "LName", DbType.String, nvo.LName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, nvo.FamilyName + "%");
                this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, nvo.MRNo + "%");
                this.dbServer.AddInParameter(storedProcCommand, "Cane", DbType.String, nvo.Cane);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.String, nvo.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsSpermFreezingVO item = new clsSpermFreezingVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["GobletColor"])),
                            GobletColor = Convert.ToString(DALHelper.HandleDBNull(reader["GobletColor"])),
                            SpermCount = Convert.ToString(DALHelper.HandleDBNull(reader["SpermCount"])),
                            Motility = Convert.ToString(DALHelper.HandleDBNull(reader["Motility"])),
                            Volume = Convert.ToString(DALHelper.HandleDBNull(reader["Volume"])),
                            TankId = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            CanisterId = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            GobletSizeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            GobletShapeId = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            CanID = Convert.ToString(DALHelper.HandleDBNull(reader["Cane"])),
                            StrawId = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            PatientUnitName = Convert.ToString(DALHelper.HandleDBNull(reader["ThawingUnit"])),
                            SpremNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremNo"])),
                            InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            Lab = Convert.ToString(DALHelper.HandleDBNull(reader["Lab"])),
                            DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"])),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SpremExpiryDate"]))),
                            SpremFreezingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremFreezingID"])),
                            SpremFreezingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpremFreezingUnitID"])),
                            LongTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LongTerm"])),
                            ShortTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ShortTerm"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]))
                        };
                        nvo.Vitrification.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable table = new DataTable {
                TableName = "MyTable"
            };
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                table.Columns.Add(info.Name);
            }
            foreach (T local in list)
            {
                DataRow row = table.NewRow();
                PropertyInfo[] properties = typeof(T).GetProperties();
                int index = 0;
                while (true)
                {
                    if (index >= properties.Length)
                    {
                        table.Rows.Add(row);
                        break;
                    }
                    PropertyInfo info2 = properties[index];
                    if (!info2.PropertyType.FullName.Equals("System.Byte[]"))
                    {
                        row[info2.Name] = info2.GetValue(local, null);
                    }
                    else
                    {
                        byte[] inArray = (byte[]) info2.GetValue(local, null);
                        if (inArray != null)
                        {
                            row[info2.Name] = Convert.ToBase64String(inArray, 0, inArray.Length, Base64FormattingOptions.None);
                        }
                    }
                    index++;
                }
            }
            return table;
        }

        private void SetPropertyValue(string pName, clsTherapyExecutionVO control, string value)
        {
            PropertyInfo property = control.GetType().GetProperty(pName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if ((property != null) && property.CanWrite)
            {
                property.SetValue(control, value, null);
            }
        }

        public clsTherapyDocumentsVO TherapyDocuments(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
        {
            return new clsTherapyDocumentsVO { 
                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                ThearpyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThearpyUnitID"])),
                TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                Date = DALHelper.HandleDate(reader["Date"]),
                Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"])),
                Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"])),
                AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"])
            };
        }

        public clsTherapyExecutionVO TherapyExcecution(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
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
                nvo.Head = "Date of LP";
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
            else if (3 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.UltraSound.ToString();
                nvo.Head = "Follicular US";
                nvo.IsBool = true;
                nvo.IsText = false;
            }
            else if (4 == ((int) nvo.TherapyTypeId))
            {
                nvo.TherapyGroup = TherapyGroup.OvumPickUp.ToString();
                nvo.Head = "OPU";
                nvo.IsBool = true;
                nvo.IsText = false;
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

        public clsTherapyExecutionVO TherapyExcecutionSurrogate(clsGetTherapyListBizActionVO BizAction, DbDataReader reader)
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
                nvo.Head = "Date of LP";
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
            clsUpdateFollicularMonitoringBizActionVO nvo = valueObject as clsUpdateFollicularMonitoringBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVF_AddUpdateFollicularMonitoring");
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
    }
}

