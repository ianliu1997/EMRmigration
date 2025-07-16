namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Linq;

    internal class clsIVFDashboad_LabDaysDAL : clsBaseIVFDashboad_LabDaysDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsIVFDashboad_LabDaysDAL()
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
                this.ImgIP = ConfigurationManager.AppSettings["IVFImgIP"];
                this.ImgVirtualDir = ConfigurationManager.AppSettings["IVFImgVirtualDir"];
                this.ImgSaveLocation = ConfigurationManager.AppSettings["IVFImgSavingLocation"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddDay0OocList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddDay0OocyteListBizActionVO nvo = valueObject as clsIVFDashboard_AddDay0OocyteListBizActionVO;
            try
            {
                for (int i = 0; i < nvo.Details.PlannedNoOfEmb; i++)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddDay0OocList");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, i + 1);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Details.OocyteDonorID);
                    this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddDay0OocListInGhaphicalRepresentationTable");
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Day0", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, i + 1);
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command);
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddLabDayDocuments(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay3BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay3BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if ((nvo.Day3Details.DetailList != null) && (nvo.Day3Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day3Details.DetailList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(storedProcCommand, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(storedProcCommand, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(storedProcCommand, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
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
                        this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(storedProcCommand, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(storedProcCommand, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(storedProcCommand, "DocNo"));
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day3Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddObervationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            return (valueObject as clsIVFDashboard_AddObservationBizActionVO);
        }

        public override IValueObject AddUpdateDay0Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay0BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay0BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay0");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day0Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day0Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day0Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day0Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day0Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day0Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day0Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day0Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day0Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day0Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day0Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day0Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day0Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day0Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day0Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day0Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day0Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day0Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day0Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "Comment", DbType.String, nvo.Day0Details.Comment);
                this.dbServer.AddInParameter(storedProcCommand, "IC", DbType.String, nvo.Day0Details.IC);
                this.dbServer.AddInParameter(storedProcCommand, "MBD", DbType.String, nvo.Day0Details.MBD);
                this.dbServer.AddInParameter(storedProcCommand, "DOSID", DbType.Int64, nvo.Day0Details.DOSID);
                this.dbServer.AddInParameter(storedProcCommand, "PICID", DbType.Int64, nvo.Day0Details.PICID);
                this.dbServer.AddInParameter(storedProcCommand, "TreatmentID", DbType.Int64, nvo.Day0Details.TreatmentID);
                this.dbServer.AddInParameter(storedProcCommand, "TreatmentStartDate", DbType.DateTime, nvo.Day0Details.TreatmentStartDate);
                this.dbServer.AddInParameter(storedProcCommand, "TreatmentEndDate", DbType.DateTime, nvo.Day0Details.TreatmentEndDate);
                this.dbServer.AddInParameter(storedProcCommand, "ObservationDate", DbType.DateTime, nvo.Day0Details.ObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "ObservationTime", DbType.DateTime, nvo.Day0Details.ObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteMaturity", DbType.Int64, nvo.Day0Details.OocyteMaturityID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteCytoplasmDysmorphisim", DbType.Int64, nvo.Day0Details.OocyteCytoplasmDysmorphisim);
                this.dbServer.AddInParameter(storedProcCommand, "ExtracytoplasmicDysmorphisim", DbType.Int64, nvo.Day0Details.ExtracytoplasmicDysmorphisim);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteCoronaCumulusComplex", DbType.Int64, nvo.Day0Details.OocyteCoronaCumulusComplex);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureDate", DbType.DateTime, nvo.Day0Details.ProcedureDate);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureTime", DbType.DateTime, nvo.Day0Details.ProcedureTime);
                this.dbServer.AddInParameter(storedProcCommand, "SourceOfSperm", DbType.Int64, nvo.Day0Details.SourceOfSperm);
                this.dbServer.AddInParameter(storedProcCommand, "SpermCollectionMethod", DbType.Int64, nvo.Day0Details.SpermCollectionMethod);
                this.dbServer.AddInParameter(storedProcCommand, "IMSI", DbType.Boolean, nvo.Day0Details.IMSI);
                this.dbServer.AddInParameter(storedProcCommand, "Embryoscope", DbType.Boolean, nvo.Day0Details.Embryoscope);
                this.dbServer.AddInParameter(storedProcCommand, "DiscardReason", DbType.String, nvo.Day0Details.DiscardReason);
                this.dbServer.AddInParameter(storedProcCommand, "DonorCode", DbType.String, nvo.Day0Details.DonorCode);
                this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.Day0Details.SampleNo);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonate", DbType.Boolean, nvo.Day0Details.IsDonate);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonateCryo);
                this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientID", DbType.Int64, nvo.Day0Details.RecepientPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientUnitID", DbType.Int64, nvo.Day0Details.RecepientPatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SemenSample", DbType.String, nvo.Day0Details.SemenSample);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycleDonate", DbType.Boolean, nvo.Day0Details.IsDonorCycleDonate);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycleDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonorCycleDonateCryo);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day0Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteZonaID", DbType.Int64, nvo.Day0Details.OocyteZonaID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteZona", DbType.String, nvo.Day0Details.OocyteZona);
                this.dbServer.AddInParameter(storedProcCommand, "PVSID", DbType.Int64, nvo.Day0Details.PVSID);
                this.dbServer.AddInParameter(storedProcCommand, "PVS", DbType.String, nvo.Day0Details.PVS);
                this.dbServer.AddInParameter(storedProcCommand, "IstPBID", DbType.Int64, nvo.Day0Details.IstPBID);
                this.dbServer.AddInParameter(storedProcCommand, "IstPB", DbType.String, nvo.Day0Details.IstPB);
                this.dbServer.AddInParameter(storedProcCommand, "CytoplasmID", DbType.Int64, nvo.Day0Details.CytoplasmID);
                this.dbServer.AddInParameter(storedProcCommand, "Cytoplasm", DbType.String, nvo.Day0Details.Cytoplasm);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day0Details.ImgList != null) && (nvo.Day0Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day0Details.ImgList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                        this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                        this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, nvo.Day0Details.OocyteNumber);
                        this.dbServer.AddInParameter(command2, "Day", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command2, "DayID", DbType.Int64, nvo.Day0Details.ID);
                        this.dbServer.AddInParameter(command2, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, nvo.Day0Details.CellStageID);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command2, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command2, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command2, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command2, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command2, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command2, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command2, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command2, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day0Details.NextPlanID == 3L) && nvo.Day0Details.Isfreezed)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day1=", 1, " where  PlanTherapyID=", nvo.Day0Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day0Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day0Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                    this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                    this.dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command4, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command4, "OocyteNumber", DbType.Int64, nvo.Day0Details.OocyteNumber);
                    this.dbServer.AddInParameter(command4, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command4, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command4, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "AnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "AssitantAnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "CumulusID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "MOIID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "GradeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "CellStageID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "OccDiamension", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "SpermPreperationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "OocytePreparationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "IncubatorID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "FinalLayering", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "NextPlanID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "Isfreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                }
                if ((nvo.Day0Details.NextPlanID == 4L) && nvo.Day0Details.Isfreezed)
                {
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                    this.dbServer.AddInParameter(command5, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                    this.dbServer.AddInParameter(command5, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command5, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command5, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command5, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command5, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command5, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command5, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command5, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command5, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command5, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command5, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                    nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "ETID", DbType.Int64, nvo.Day0Details.ID);
                    this.dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, nvo.Day0Details.OocyteNumber);
                    this.dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, nvo.Day0Details.Date);
                    this.dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day0");
                    this.dbServer.AddInParameter(command6, "GradeID", DbType.Int64, nvo.Day0Details.GradeID);
                    this.dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, nvo.Day0Details.CellStageID);
                    this.dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((nvo.Day0Details.NextPlanID == 2L) && nvo.Day0Details.Isfreezed)
                {
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                    this.dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                    this.dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command7, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command7, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command7, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command7, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command7, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command7, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command7, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command7, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command7, "ResultStatus");
                    nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "VitrivicationID", DbType.Int64, nvo.Day0Details.ID);
                    this.dbServer.AddInParameter(command8, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "EmbNumber", DbType.Int64, nvo.Day0Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "EmbSerialNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day0Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day0");
                    this.dbServer.AddInParameter(command8, "CellStageID", DbType.String, nvo.Day0Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day0Details.OocyteMaturityID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command8, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command8, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command8, "TransferDayNo", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if ((nvo.Day0Details.NextPlanID == 9) && nvo.Day0Details.Isfreezed)
                {
                    DbCommand command9 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                    this.dbServer.AddInParameter(command9, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                    this.dbServer.AddInParameter(command9, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command9, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command9, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command9, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command9, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command9, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command9, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command9, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command9, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command9, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command9, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command9, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command9, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command9, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command9, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command9, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command9, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command9, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command9, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command9, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command9, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command9, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                    this.dbServer.ExecuteNonQuery(command9, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command9, "ResultStatus");
                    nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command9, "ID");
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command10, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "VitrivicationID", DbType.Int64, nvo.Day0Details.ID);
                    this.dbServer.AddInParameter(command10, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "EmbNumber", DbType.Int64, nvo.Day0Details.OocyteNumber);
                    this.dbServer.AddInParameter(command10, "EmbSerialNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command10, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TransferDate", DbType.DateTime, nvo.Day0Details.Date);
                    this.dbServer.AddInParameter(command10, "TransferDay", DbType.String, "Day0");
                    this.dbServer.AddInParameter(command10, "CellStageID", DbType.String, nvo.Day0Details.CellStageID);
                    this.dbServer.AddInParameter(command10, "GradeID", DbType.Int64, nvo.Day0Details.OocyteMaturityID);
                    this.dbServer.AddInParameter(command10, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command10, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command10, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command10, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command10, "TransferDayNo", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                    this.dbServer.AddInParameter(command10, "IsDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonateCryo);
                    this.dbServer.AddInParameter(command10, "RecepientPatientID", DbType.Int64, nvo.Day0Details.RecepientPatientID);
                    this.dbServer.AddInParameter(command10, "RecepientPatientUnitID", DbType.Int64, nvo.Day0Details.RecepientPatientUnitID);
                    this.dbServer.AddInParameter(command10, "IsDonorCycleDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonorCycleDonateCryo);
                    this.dbServer.AddInParameter(command10, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                    this.dbServer.AddInParameter(command10, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                    this.dbServer.ExecuteNonQuery(command10, transaction);
                }
                if (nvo.Day0Details.OcyteListList != null)
                {
                    foreach (MasterListItem item in nvo.Day0Details.OcyteListList)
                    {
                        DbCommand command11 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay0");
                        this.dbServer.AddInParameter(command11, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command11, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                        this.dbServer.AddInParameter(command11, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                        this.dbServer.AddInParameter(command11, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command11, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command11, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber + item.FilterID);
                        this.dbServer.AddInParameter(command11, "OocyteNumber", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(command11, "Date", DbType.DateTime, nvo.Day0Details.Date);
                        this.dbServer.AddInParameter(command11, "Time", DbType.DateTime, nvo.Day0Details.Time);
                        this.dbServer.AddInParameter(command11, "EmbryologistID", DbType.Int64, nvo.Day0Details.EmbryologistID);
                        this.dbServer.AddInParameter(command11, "AssitantEmbryologistID", DbType.Int64, nvo.Day0Details.AssitantEmbryologistID);
                        this.dbServer.AddInParameter(command11, "AnesthetistID", DbType.Int64, nvo.Day0Details.AnesthetistID);
                        this.dbServer.AddInParameter(command11, "AssitantAnesthetistID", DbType.Int64, nvo.Day0Details.AssitantAnesthetistID);
                        this.dbServer.AddInParameter(command11, "CumulusID", DbType.Int64, nvo.Day0Details.CumulusID);
                        this.dbServer.AddInParameter(command11, "MOIID", DbType.Int64, nvo.Day0Details.MOIID);
                        this.dbServer.AddInParameter(command11, "GradeID", DbType.Int64, nvo.Day0Details.GradeID);
                        this.dbServer.AddInParameter(command11, "CellStageID", DbType.Int64, nvo.Day0Details.CellStageID);
                        this.dbServer.AddInParameter(command11, "OccDiamension", DbType.String, nvo.Day0Details.OccDiamension);
                        this.dbServer.AddInParameter(command11, "SpermPreperationMedia", DbType.String, nvo.Day0Details.SpermPreperationMedia);
                        this.dbServer.AddInParameter(command11, "OocytePreparationMedia", DbType.String, nvo.Day0Details.OocytePreparationMedia);
                        this.dbServer.AddInParameter(command11, "IncubatorID", DbType.Int64, nvo.Day0Details.IncubatorID);
                        this.dbServer.AddInParameter(command11, "FinalLayering", DbType.String, nvo.Day0Details.FinalLayering);
                        this.dbServer.AddInParameter(command11, "NextPlanID", DbType.Int64, nvo.Day0Details.NextPlanID);
                        this.dbServer.AddInParameter(command11, "Isfreezed", DbType.Boolean, nvo.Day0Details.Isfreezed);
                        this.dbServer.AddInParameter(command11, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command11, "Impression", DbType.String, nvo.Day0Details.Impression);
                        this.dbServer.AddInParameter(command11, "Comment", DbType.String, nvo.Day0Details.Comment);
                        this.dbServer.AddInParameter(command11, "IC", DbType.String, nvo.Day0Details.IC);
                        this.dbServer.AddInParameter(command11, "MBD", DbType.String, nvo.Day0Details.MBD);
                        this.dbServer.AddInParameter(command11, "DOSID", DbType.Int64, nvo.Day0Details.DOSID);
                        this.dbServer.AddInParameter(command11, "PICID", DbType.Int64, nvo.Day0Details.PICID);
                        this.dbServer.AddInParameter(command11, "TreatmentID", DbType.Int64, nvo.Day0Details.TreatmentID);
                        this.dbServer.AddInParameter(command11, "TreatmentStartDate", DbType.DateTime, nvo.Day0Details.TreatmentStartDate);
                        this.dbServer.AddInParameter(command11, "TreatmentEndDate", DbType.DateTime, nvo.Day0Details.TreatmentEndDate);
                        this.dbServer.AddInParameter(command11, "ObservationDate", DbType.DateTime, nvo.Day0Details.ObservationDate);
                        this.dbServer.AddInParameter(command11, "ObservationTime", DbType.DateTime, nvo.Day0Details.ObservationTime);
                        this.dbServer.AddInParameter(command11, "OocyteMaturity", DbType.Int64, nvo.Day0Details.OocyteMaturityID);
                        this.dbServer.AddInParameter(command11, "OocyteCytoplasmDysmorphisim", DbType.Int64, nvo.Day0Details.OocyteCytoplasmDysmorphisim);
                        this.dbServer.AddInParameter(command11, "ExtracytoplasmicDysmorphisim", DbType.Int64, nvo.Day0Details.ExtracytoplasmicDysmorphisim);
                        this.dbServer.AddInParameter(command11, "OocyteCoronaCumulusComplex", DbType.Int64, nvo.Day0Details.OocyteCoronaCumulusComplex);
                        this.dbServer.AddInParameter(command11, "ProcedureDate", DbType.DateTime, nvo.Day0Details.ProcedureDate);
                        this.dbServer.AddInParameter(command11, "ProcedureTime", DbType.DateTime, nvo.Day0Details.ProcedureTime);
                        this.dbServer.AddInParameter(command11, "SourceOfSperm", DbType.Int64, nvo.Day0Details.SourceOfSperm);
                        this.dbServer.AddInParameter(command11, "SpermCollectionMethod", DbType.Int64, nvo.Day0Details.SpermCollectionMethod);
                        this.dbServer.AddInParameter(command11, "IMSI", DbType.Boolean, nvo.Day0Details.IMSI);
                        this.dbServer.AddInParameter(command11, "Embryoscope", DbType.Boolean, nvo.Day0Details.Embryoscope);
                        this.dbServer.AddInParameter(command11, "DiscardReason", DbType.String, nvo.Day0Details.DiscardReason);
                        this.dbServer.AddInParameter(command11, "DonorCode", DbType.String, nvo.Day0Details.DonorCode);
                        this.dbServer.AddInParameter(command11, "SampleNo", DbType.String, nvo.Day0Details.SampleNo);
                        this.dbServer.AddInParameter(command11, "IsDonate", DbType.Boolean, nvo.Day0Details.IsDonate);
                        this.dbServer.AddInParameter(command11, "IsDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonateCryo);
                        this.dbServer.AddInParameter(command11, "RecepientPatientID", DbType.Int64, nvo.Day0Details.RecepientPatientID);
                        this.dbServer.AddInParameter(command11, "RecepientPatientUnitID", DbType.Int64, nvo.Day0Details.RecepientPatientUnitID);
                        this.dbServer.AddInParameter(command11, "SemenSample", DbType.String, nvo.Day0Details.SemenSample);
                        this.dbServer.AddInParameter(command11, "IsDonorCycleDonate", DbType.Boolean, nvo.Day0Details.IsDonorCycleDonate);
                        this.dbServer.AddInParameter(command11, "IsDonorCycleDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonorCycleDonateCryo);
                        this.dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day0Details.ID);
                        this.dbServer.AddOutParameter(command11, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddInParameter(command11, "OocyteZonaID", DbType.Int64, nvo.Day0Details.OocyteZonaID);
                        this.dbServer.AddInParameter(command11, "OocyteZona", DbType.String, nvo.Day0Details.OocyteZona);
                        this.dbServer.AddInParameter(command11, "PVSID", DbType.Int64, nvo.Day0Details.PVSID);
                        this.dbServer.AddInParameter(command11, "PVS", DbType.String, nvo.Day0Details.PVS);
                        this.dbServer.AddInParameter(command11, "IstPBID", DbType.Int64, nvo.Day0Details.IstPBID);
                        this.dbServer.AddInParameter(command11, "IstPB", DbType.String, nvo.Day0Details.IstPB);
                        this.dbServer.AddInParameter(command11, "CytoplasmID", DbType.Int64, nvo.Day0Details.CytoplasmID);
                        this.dbServer.AddInParameter(command11, "Cytoplasm", DbType.String, nvo.Day0Details.Cytoplasm);
                        this.dbServer.ExecuteNonQuery(command11, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command11, "ResultStatus");
                        nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command11, "ID");
                        if ((nvo.Day0Details.ImgList != null) && (nvo.Day0Details.ImgList.Count > 0))
                        {
                            foreach (clsAddImageVO evo2 in nvo.Day0Details.ImgList)
                            {
                                DbCommand command12 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                this.dbServer.AddInParameter(command12, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command12, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                                this.dbServer.AddInParameter(command12, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                                this.dbServer.AddInParameter(command12, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                                this.dbServer.AddInParameter(command12, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                                this.dbServer.AddInParameter(command12, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber + item.FilterID);
                                this.dbServer.AddInParameter(command12, "OocyteNumber", DbType.Int64, item.ID);
                                this.dbServer.AddInParameter(command12, "Day", DbType.Int64, 0);
                                this.dbServer.AddInParameter(command12, "DayID", DbType.Int64, nvo.Day0Details.ID);
                                this.dbServer.AddInParameter(command12, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command12, "CellStageID", DbType.Int64, nvo.Day0Details.CellStageID);
                                this.dbServer.AddInParameter(command12, "FileName", DbType.String, evo2.ImagePath);
                                this.dbServer.AddInParameter(command12, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command12, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command12, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command12, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command12, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command12, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                this.dbServer.AddInParameter(command12, "IsApplyTo", DbType.Int32, 1);
                                this.dbServer.AddParameter(command12, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                this.dbServer.AddParameter(command12, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                this.dbServer.ExecuteNonQuery(command12, transaction);
                                evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command12, "ID"));
                                evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command12, "ServerImageName"));
                                evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command12, "SeqNo"));
                            }
                        }
                        if ((nvo.Day0Details.NextPlanID == 3L) && nvo.Day0Details.Isfreezed)
                        {
                            DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day1=", 1, " where  PlanTherapyID=", nvo.Day0Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day0Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day0Details.SerialOocyteNumber + item.FilterID }));
                            this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");
                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                            this.dbServer.AddInParameter(command14, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                            this.dbServer.AddInParameter(command14, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command14, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber + item.FilterID);
                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, item.ID);
                            this.dbServer.AddInParameter(command14, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command14, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command14, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "AnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "AssitantAnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "CumulusID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "MOIID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "GradeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "CellStageID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "OccDiamension", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "SpermPreperationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "OocytePreparationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "IncubatorID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "FinalLayering", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "NextPlanID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command14, "Isfreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command14, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command14, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command14, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command14, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command14, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command14, "ResultStatus");
                        }
                        if ((nvo.Day0Details.NextPlanID == 4L) && nvo.Day0Details.Isfreezed)
                        {
                            DbCommand command15 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command15, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command15, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                            this.dbServer.AddInParameter(command15, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                            this.dbServer.AddInParameter(command15, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command15, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command15, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command15, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command15, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command15, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command15, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command15, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command15, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command15, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command15, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command15, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command15, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command15, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command15, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command15, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command15, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command15, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command15, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command15, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command15, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command15, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command15, "ResultStatus");
                            nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command15, "ID");
                            DbCommand command16 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command16, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command16, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "ETID", DbType.Int64, nvo.Day0Details.ID);
                            this.dbServer.AddInParameter(command16, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "OocyteNumber", DbType.Int64, item.ID);
                            this.dbServer.AddInParameter(command16, "SerialOocyteNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber + item.FilterID);
                            this.dbServer.AddInParameter(command16, "TransferDate", DbType.DateTime, nvo.Day0Details.Date);
                            this.dbServer.AddInParameter(command16, "TransferDay", DbType.String, "Day0");
                            this.dbServer.AddInParameter(command16, "GradeID", DbType.Int64, nvo.Day0Details.GradeID);
                            this.dbServer.AddInParameter(command16, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "FertStageID", DbType.Int64, nvo.Day0Details.CellStageID);
                            this.dbServer.AddInParameter(command16, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command16, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command16, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command16, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command16, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command16, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command16, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command16, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command16, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command16, transaction);
                        }
                        if ((nvo.Day0Details.NextPlanID == 2L) && nvo.Day0Details.Isfreezed)
                        {
                            DbCommand command17 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command17, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command17, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                            this.dbServer.AddInParameter(command17, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                            this.dbServer.AddInParameter(command17, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command17, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command17, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command17, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command17, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command17, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command17, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command17, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command17, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command17, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command17, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command17, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command17, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command17, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command17, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command17, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command17, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command17, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command17, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command17, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command17, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command17, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                            this.dbServer.ExecuteNonQuery(command17, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command17, "ResultStatus");
                            nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command17, "ID");
                            DbCommand command18 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command18, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "VitrivicationID", DbType.Int64, nvo.Day0Details.ID);
                            this.dbServer.AddInParameter(command18, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "EmbNumber", DbType.Int64, item.ID);
                            this.dbServer.AddInParameter(command18, "EmbSerialNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber + item.FilterID);
                            this.dbServer.AddInParameter(command18, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "TransferDate", DbType.DateTime, nvo.Day0Details.Date);
                            this.dbServer.AddInParameter(command18, "TransferDay", DbType.String, "Day0");
                            this.dbServer.AddInParameter(command18, "CellStageID", DbType.String, nvo.Day0Details.CellStageID);
                            this.dbServer.AddInParameter(command18, "GradeID", DbType.Int64, nvo.Day0Details.OocyteMaturityID);
                            this.dbServer.AddInParameter(command18, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command18, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command18, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command18, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command18, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command18, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command18, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command18, "TransferDayNo", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                            this.dbServer.ExecuteNonQuery(command18, transaction);
                        }
                        if ((nvo.Day0Details.NextPlanID == 9) && nvo.Day0Details.Isfreezed)
                        {
                            DbCommand command19 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command19, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command19, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                            this.dbServer.AddInParameter(command19, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                            this.dbServer.AddInParameter(command19, "PlanTherapyID", DbType.Int64, nvo.Day0Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command19, "PlanTherapyUnitID", DbType.Int64, nvo.Day0Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command19, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command19, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command19, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command19, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command19, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command19, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command19, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command19, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command19, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command19, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command19, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command19, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command19, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command19, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command19, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command19, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command19, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command19, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command19, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command19, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                            this.dbServer.ExecuteNonQuery(command19, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command19, "ResultStatus");
                            nvo.Day0Details.ID = (long) this.dbServer.GetParameterValue(command19, "ID");
                            DbCommand command20 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command20, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "VitrivicationID", DbType.Int64, nvo.Day0Details.ID);
                            this.dbServer.AddInParameter(command20, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "EmbNumber", DbType.Int64, item.ID);
                            this.dbServer.AddInParameter(command20, "EmbSerialNumber", DbType.Int64, nvo.Day0Details.SerialOocyteNumber + item.FilterID);
                            this.dbServer.AddInParameter(command20, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TransferDate", DbType.DateTime, nvo.Day0Details.Date);
                            this.dbServer.AddInParameter(command20, "TransferDay", DbType.String, "Day0");
                            this.dbServer.AddInParameter(command20, "CellStageID", DbType.String, nvo.Day0Details.CellStageID);
                            this.dbServer.AddInParameter(command20, "GradeID", DbType.Int64, nvo.Day0Details.OocyteMaturityID);
                            this.dbServer.AddInParameter(command20, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command20, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "OocyteDonorID", DbType.Int64, nvo.Day0Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command20, "OocyteDonorUnitID", DbType.Int64, nvo.Day0Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command20, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command20, "TransferDayNo", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "IsFreezeOocytes", DbType.Boolean, nvo.Day0Details.IsFreezeOocytes);
                            this.dbServer.AddInParameter(command20, "IsDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonateCryo);
                            this.dbServer.AddInParameter(command20, "RecepientPatientID", DbType.Int64, nvo.Day0Details.RecepientPatientID);
                            this.dbServer.AddInParameter(command20, "RecepientPatientUnitID", DbType.Int64, nvo.Day0Details.RecepientPatientUnitID);
                            this.dbServer.AddInParameter(command20, "IsDonorCycleDonateCryo", DbType.Boolean, nvo.Day0Details.IsDonorCycleDonateCryo);
                            this.dbServer.AddInParameter(command20, "PatientID", DbType.Int64, nvo.Day0Details.PatientID);
                            this.dbServer.AddInParameter(command20, "PatientUnitID", DbType.Int64, nvo.Day0Details.PatientUnitID);
                            this.dbServer.ExecuteNonQuery(command20, transaction);
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day0Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay1Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay1BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay1BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day1Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day1=", 1, " where  PlanTherapyID=", nvo.Day1Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day1Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day1Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day1Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day1Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day1Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day1Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day1Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day1Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day1Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day1Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day1Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day1Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day1Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day1Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day1Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day1Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day1Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day1Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day1Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day1Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day1Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day1Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day1Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day1Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day1Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day1Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day1Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day1Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day1Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day1Details.CellNo);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day1Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day1Details.ImgList != null) && (nvo.Day1Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day1Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day1Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day1Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day1Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day1Details.DetailList != null) && (nvo.Day1Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day1Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day1Details.NextPlanID == 3L) && nvo.Day1Details.Isfreezed)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day2=", 1, " where  PlanTherapyID=", nvo.Day1Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day1Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day1Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day1Details.OocyteNumber);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                if ((nvo.Day1Details.NextPlanID == 4L) && nvo.Day1Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day1Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "ETID", DbType.Int64, nvo.Day1Details.ID);
                    this.dbServer.AddInParameter(command8, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, nvo.Day1Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day1Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day1");
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day1Details.GradeID);
                    this.dbServer.AddInParameter(command8, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FertStageID", DbType.Int64, nvo.Day1Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if ((nvo.Day1Details.NextPlanID == 2L) && nvo.Day1Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day1Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command10, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "VitrivicationID", DbType.Int64, nvo.Day1Details.ID);
                    this.dbServer.AddInParameter(command10, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "EmbNumber", DbType.Int64, nvo.Day1Details.OocyteNumber);
                    this.dbServer.AddInParameter(command10, "EmbSerialNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command10, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TransferDate", DbType.DateTime, nvo.Day1Details.Date);
                    this.dbServer.AddInParameter(command10, "TransferDay", DbType.String, "Day1");
                    this.dbServer.AddInParameter(command10, "CellStageID", DbType.String, nvo.Day1Details.CellStageID);
                    this.dbServer.AddInParameter(command10, "GradeID", DbType.Int64, nvo.Day1Details.GradeID);
                    this.dbServer.AddInParameter(command10, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command10, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command10, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command10, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command10, transaction);
                }
                if (nvo.Day1Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day1Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_0020;
                    TR_000C:
                        if ((nvo.Day1Details.NextPlanID == 3L) && nvo.Day1Details.Isfreezed)
                        {
                            DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day2=", 1, " where  PlanTherapyID=", nvo.Day1Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day1Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day1Details.SerialOocyteNumber, current.FilterID }));
                            this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                            this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        if ((nvo.Day1Details.NextPlanID == 4L) && nvo.Day1Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day1Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command18 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command18, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "ETID", DbType.Int64, nvo.Day1Details.ID);
                            this.dbServer.AddInParameter(command18, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command18, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command18, "TransferDate", DbType.DateTime, nvo.Day1Details.Date);
                            this.dbServer.AddInParameter(command18, "TransferDay", DbType.String, "Day1");
                            this.dbServer.AddInParameter(command18, "GradeID", DbType.Int64, nvo.Day1Details.GradeID);
                            this.dbServer.AddInParameter(command18, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FertStageID", DbType.Int64, nvo.Day1Details.CellStageID);
                            this.dbServer.AddInParameter(command18, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command18, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command18, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command18, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command18, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command18, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command18, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command18, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command18, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command18, transaction);
                        }
                        if ((nvo.Day1Details.NextPlanID == 2L) && nvo.Day1Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day1Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command20 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command20, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "VitrivicationID", DbType.Int64, nvo.Day1Details.ID);
                            this.dbServer.AddInParameter(command20, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command20, "EmbSerialNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command20, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TransferDate", DbType.DateTime, nvo.Day1Details.Date);
                            this.dbServer.AddInParameter(command20, "TransferDay", DbType.String, "Day1");
                            this.dbServer.AddInParameter(command20, "CellStageID", DbType.String, nvo.Day1Details.CellStageID);
                            this.dbServer.AddInParameter(command20, "GradeID", DbType.Int64, nvo.Day1Details.GradeID);
                            this.dbServer.AddInParameter(command20, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command20, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command20, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command20, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command20, transaction);
                        }
                    TR_0020:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day1=", 1, " where  PlanTherapyID=", nvo.Day1Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day1Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day1Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day1Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day1Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day1Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day1Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day1Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day1Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day1Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day1Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day1Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day1Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day1Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day1Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day1Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day1Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day1Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day1Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day1Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day1Details.Impression);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day1Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day1Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day1Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day1Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day1Details.OocyteDonorUnitID);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day1Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day1Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day1Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day1Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day1Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day1Details.CellNo);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day1Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day1Details.ImgList != null) && (nvo.Day1Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day1Details.ImgList)
                                        {
                                            DbCommand command13 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command13, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "PatientID", DbType.Int64, nvo.Day1Details.PatientID);
                                            this.dbServer.AddInParameter(command13, "PatientUnitID", DbType.Int64, nvo.Day1Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyID", DbType.Int64, nvo.Day1Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyUnitID", DbType.Int64, nvo.Day1Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command13, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command13, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command13, "Day", DbType.Int64, 1);
                                            this.dbServer.AddInParameter(command13, "DayID", DbType.Int64, nvo.Day1Details.ID);
                                            this.dbServer.AddInParameter(command13, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "CellStageID", DbType.Int64, nvo.Day1Details.CellStageID);
                                            this.dbServer.AddInParameter(command13, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command13, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command13, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command13, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command13, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command13, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command13, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command13, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command13, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command13, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command13, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command13, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command13, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day1Details.DetailList != null) && (nvo.Day1Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day1Details.DetailList)
                                        {
                                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command14, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command14, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command14, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command14, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command14, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command14, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command14, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command14, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day1Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command14, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command14, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command14, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command14, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command14);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command14, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000C;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day1Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay2Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay2BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay2BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day2Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day2=", 1, " where  PlanTherapyID=", nvo.Day2Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day2Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day2Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day2Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day2Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day2Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day2Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day2Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day2Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day2Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day2Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day2Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day2Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day2Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day2Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day2Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day2Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day2Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day2Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day2Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day2Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day2Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "FrgmentationID", DbType.Int64, nvo.Day2Details.FrgmentationID);
                this.dbServer.AddInParameter(storedProcCommand, "BlastmereSymmetryID", DbType.Int64, nvo.Day2Details.BlastmereSymmetryID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDetails", DbType.String, nvo.Day2Details.OtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day2Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day2Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day2Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day2Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day2Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day2Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day2Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day2Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day2Details.CellNo);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day2Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day2Details.ImgList != null) && (nvo.Day2Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day2Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day2Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 2);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day2Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day2Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day2Details.DetailList != null) && (nvo.Day2Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day2Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day2Details.NextPlanID == 3L) && nvo.Day2Details.Isfreezed)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day3=", 1, " where  PlanTherapyID=", nvo.Day2Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day2Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day2Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day2Details.OocyteNumber);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                if ((nvo.Day2Details.NextPlanID == 4L) && nvo.Day2Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day2Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "ETID", DbType.Int64, nvo.Day2Details.ID);
                    this.dbServer.AddInParameter(command8, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, nvo.Day2Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day2Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day2");
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day2Details.GradeID);
                    this.dbServer.AddInParameter(command8, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FertStageID", DbType.Int64, nvo.Day2Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if ((nvo.Day2Details.NextPlanID == 2L) && nvo.Day2Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day2Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command10, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "VitrivicationID", DbType.Int64, nvo.Day2Details.ID);
                    this.dbServer.AddInParameter(command10, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "EmbNumber", DbType.Int64, nvo.Day2Details.OocyteNumber);
                    this.dbServer.AddInParameter(command10, "EmbSerialNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command10, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TransferDate", DbType.DateTime, nvo.Day2Details.Date);
                    this.dbServer.AddInParameter(command10, "TransferDay", DbType.String, "Day2");
                    this.dbServer.AddInParameter(command10, "CellStageID", DbType.String, nvo.Day2Details.CellStageID);
                    this.dbServer.AddInParameter(command10, "GradeID", DbType.Int64, nvo.Day2Details.GradeID);
                    this.dbServer.AddInParameter(command10, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command10, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command10, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command10, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command10, transaction);
                }
                if (nvo.Day2Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day2Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_0020;
                    TR_000C:
                        if ((nvo.Day2Details.NextPlanID == 3L) && nvo.Day2Details.Isfreezed)
                        {
                            DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day3=", 1, " where  PlanTherapyID=", nvo.Day2Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day2Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day2Details.SerialOocyteNumber, current.FilterID }));
                            this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                            this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        if ((nvo.Day2Details.NextPlanID == 4L) && nvo.Day2Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day2Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command18 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command18, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "ETID", DbType.Int64, nvo.Day2Details.ID);
                            this.dbServer.AddInParameter(command18, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command18, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command18, "TransferDate", DbType.DateTime, nvo.Day2Details.Date);
                            this.dbServer.AddInParameter(command18, "TransferDay", DbType.String, "Day2");
                            this.dbServer.AddInParameter(command18, "GradeID", DbType.Int64, nvo.Day2Details.GradeID);
                            this.dbServer.AddInParameter(command18, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FertStageID", DbType.Int64, nvo.Day2Details.CellStageID);
                            this.dbServer.AddInParameter(command18, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command18, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command18, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command18, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command18, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command18, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command18, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command18, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command18, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command18, transaction);
                        }
                        if ((nvo.Day2Details.NextPlanID == 2L) && nvo.Day2Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day2Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command20 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command20, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "VitrivicationID", DbType.Int64, nvo.Day2Details.ID);
                            this.dbServer.AddInParameter(command20, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command20, "EmbSerialNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command20, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TransferDate", DbType.DateTime, nvo.Day2Details.Date);
                            this.dbServer.AddInParameter(command20, "TransferDay", DbType.String, "Day2");
                            this.dbServer.AddInParameter(command20, "CellStageID", DbType.String, nvo.Day2Details.CellStageID);
                            this.dbServer.AddInParameter(command20, "GradeID", DbType.Int64, nvo.Day2Details.GradeID);
                            this.dbServer.AddInParameter(command20, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command20, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command20, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command20, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command20, transaction);
                        }
                    TR_0020:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day2=", 1, " where  PlanTherapyID=", nvo.Day2Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day2Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day2Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day2Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day2Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day2Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day2Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day2Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day2Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day2Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day2Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day2Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day2Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day2Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day2Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day2Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day2Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day2Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day2Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day2Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day2Details.Impression);
                                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, nvo.Day2Details.FrgmentationID);
                                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, nvo.Day2Details.BlastmereSymmetryID);
                                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, nvo.Day2Details.OtherDetails);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day2Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day2Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day2Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day2Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day2Details.OocyteDonorUnitID);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day2Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day2Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day2Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day2Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day2Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day2Details.CellNo);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day2Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day2Details.ImgList != null) && (nvo.Day2Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day2Details.ImgList)
                                        {
                                            DbCommand command13 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command13, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "PatientID", DbType.Int64, nvo.Day2Details.PatientID);
                                            this.dbServer.AddInParameter(command13, "PatientUnitID", DbType.Int64, nvo.Day2Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyID", DbType.Int64, nvo.Day2Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyUnitID", DbType.Int64, nvo.Day2Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command13, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command13, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command13, "Day", DbType.Int64, 2);
                                            this.dbServer.AddInParameter(command13, "DayID", DbType.Int64, nvo.Day2Details.ID);
                                            this.dbServer.AddInParameter(command13, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "CellStageID", DbType.Int64, nvo.Day2Details.CellStageID);
                                            this.dbServer.AddInParameter(command13, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command13, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command13, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command13, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command13, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command13, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command13, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command13, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command13, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command13, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command13, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command13, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command13, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day2Details.DetailList != null) && (nvo.Day2Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day2Details.DetailList)
                                        {
                                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command14, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command14, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command14, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command14, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command14, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command14, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command14, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command14, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day2Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command14, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command14, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command14, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command14, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command14);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command14, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000C;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day2Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay3Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay3BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay3BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day3Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day3=", 1, " where  PlanTherapyID=", nvo.Day3Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day3Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day3Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day3Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day3Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day3Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day3Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day3Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day3Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day3Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day3Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day3Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day3Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day3Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day3Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day3Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day3Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day3Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day3Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day3Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day3Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day3Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "FrgmentationID", DbType.Int64, nvo.Day3Details.FrgmentationID);
                this.dbServer.AddInParameter(storedProcCommand, "BlastmereSymmetryID", DbType.Int64, nvo.Day3Details.BlastmereSymmetryID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDetails", DbType.String, nvo.Day3Details.OtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day3Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day3Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day3Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day3Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day3Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day3Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day3Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day3Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day3Details.CellNo);
                this.dbServer.AddInParameter(storedProcCommand, "IsEmbryoCompacted", DbType.Boolean, nvo.Day3Details.IsEmbryoCompacted);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day3Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day3Details.ImgList != null) && (nvo.Day3Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day3Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day3Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 3);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day3Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day3Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day3Details.DetailList != null) && (nvo.Day3Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day3Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day3Details.NextPlanID == 3L) && nvo.Day3Details.Isfreezed)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day4=", 1, " where  PlanTherapyID=", nvo.Day3Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day3Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day3Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day3Details.OocyteNumber);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                if ((nvo.Day3Details.NextPlanID == 4L) && nvo.Day3Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day3Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "ETID", DbType.Int64, nvo.Day3Details.ID);
                    this.dbServer.AddInParameter(command8, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, nvo.Day3Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day3Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day3");
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day3Details.GradeID);
                    this.dbServer.AddInParameter(command8, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FertStageID", DbType.Int64, nvo.Day3Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if ((nvo.Day3Details.NextPlanID == 2L) && nvo.Day3Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day3Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command10, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "VitrivicationID", DbType.Int64, nvo.Day3Details.ID);
                    this.dbServer.AddInParameter(command10, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "EmbNumber", DbType.Int64, nvo.Day3Details.OocyteNumber);
                    this.dbServer.AddInParameter(command10, "EmbSerialNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command10, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TransferDate", DbType.DateTime, nvo.Day3Details.Date);
                    this.dbServer.AddInParameter(command10, "TransferDay", DbType.String, "Day3");
                    this.dbServer.AddInParameter(command10, "CellStageID", DbType.String, nvo.Day3Details.CellStageID);
                    this.dbServer.AddInParameter(command10, "GradeID", DbType.Int64, nvo.Day3Details.GradeID);
                    this.dbServer.AddInParameter(command10, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command10, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command10, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command10, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command10, transaction);
                }
                if (nvo.Day3Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day3Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_0020;
                    TR_000C:
                        if ((nvo.Day3Details.NextPlanID == 3L) && nvo.Day3Details.Isfreezed)
                        {
                            DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day4=", 1, " where  PlanTherapyID=", nvo.Day3Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day3Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day3Details.SerialOocyteNumber, current.FilterID }));
                            this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                            this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        if ((nvo.Day3Details.NextPlanID == 4L) && nvo.Day3Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day3Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command18 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command18, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "ETID", DbType.Int64, nvo.Day3Details.ID);
                            this.dbServer.AddInParameter(command18, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command18, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command18, "TransferDate", DbType.DateTime, nvo.Day3Details.Date);
                            this.dbServer.AddInParameter(command18, "TransferDay", DbType.String, "Day3");
                            this.dbServer.AddInParameter(command18, "GradeID", DbType.Int64, nvo.Day3Details.GradeID);
                            this.dbServer.AddInParameter(command18, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FertStageID", DbType.Int64, nvo.Day3Details.CellStageID);
                            this.dbServer.AddInParameter(command18, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command18, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command18, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command18, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command18, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command18, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command18, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command18, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command18, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command18, transaction);
                        }
                        if ((nvo.Day3Details.NextPlanID == 2L) && nvo.Day3Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day3Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command20 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command20, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "VitrivicationID", DbType.Int64, nvo.Day3Details.ID);
                            this.dbServer.AddInParameter(command20, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command20, "EmbSerialNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command20, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TransferDate", DbType.DateTime, nvo.Day3Details.Date);
                            this.dbServer.AddInParameter(command20, "TransferDay", DbType.String, "Day3");
                            this.dbServer.AddInParameter(command20, "CellStageID", DbType.String, nvo.Day3Details.CellStageID);
                            this.dbServer.AddInParameter(command20, "GradeID", DbType.Int64, nvo.Day3Details.GradeID);
                            this.dbServer.AddInParameter(command20, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command20, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command20, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command20, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command20, transaction);
                        }
                    TR_0020:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day3=", 1, " where  PlanTherapyID=", nvo.Day3Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day3Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day3Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day3Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day3Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day3Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day3Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day3Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day3Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day3Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day3Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day3Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day3Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day3Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day3Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day3Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day3Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day3Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day3Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day3Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day3Details.Impression);
                                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, nvo.Day3Details.FrgmentationID);
                                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, nvo.Day3Details.BlastmereSymmetryID);
                                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, nvo.Day3Details.OtherDetails);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day3Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day3Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day3Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day3Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day3Details.OocyteDonorUnitID);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day3Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day3Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day3Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day3Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day3Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day3Details.CellNo);
                                    this.dbServer.AddInParameter(command, "IsEmbryoCompacted", DbType.Boolean, nvo.Day3Details.IsEmbryoCompacted);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day3Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day3Details.ImgList != null) && (nvo.Day3Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day3Details.ImgList)
                                        {
                                            DbCommand command13 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command13, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "PatientID", DbType.Int64, nvo.Day3Details.PatientID);
                                            this.dbServer.AddInParameter(command13, "PatientUnitID", DbType.Int64, nvo.Day3Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyID", DbType.Int64, nvo.Day3Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyUnitID", DbType.Int64, nvo.Day3Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command13, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command13, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command13, "Day", DbType.Int64, 3);
                                            this.dbServer.AddInParameter(command13, "DayID", DbType.Int64, nvo.Day3Details.ID);
                                            this.dbServer.AddInParameter(command13, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "CellStageID", DbType.Int64, nvo.Day3Details.CellStageID);
                                            this.dbServer.AddInParameter(command13, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command13, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command13, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command13, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command13, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command13, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command13, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command13, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command13, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command13, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command13, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command13, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command13, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day3Details.DetailList != null) && (nvo.Day3Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day3Details.DetailList)
                                        {
                                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command14, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command14, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command14, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command14, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command14, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command14, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command14, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command14, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day3Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command14, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command14, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command14, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command14, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command14);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command14, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000C;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day3Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay4Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay4BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay4BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day4Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day4=", 1, " where  PlanTherapyID=", nvo.Day4Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day4Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day4Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day4Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day4Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day4Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day4Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day4Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day4Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day4Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day4Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day4Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day4Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day4Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day4Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day4Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day4Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day4Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day4Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day4Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day4Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day4Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "FrgmentationID", DbType.Int64, nvo.Day4Details.FrgmentationID);
                this.dbServer.AddInParameter(storedProcCommand, "BlastmereSymmetryID", DbType.Int64, nvo.Day4Details.BlastmereSymmetryID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDetails", DbType.String, nvo.Day4Details.OtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day4Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day4Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day4Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day4Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day4Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day4Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day4Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day4Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day4Details.CellNo);
                this.dbServer.AddInParameter(storedProcCommand, "IsAssistedHatching", DbType.Boolean, nvo.Day4Details.AssistedHatching);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day4Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day4Details.ImgList != null) && (nvo.Day4Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day4Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day4Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 4);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day4Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day4Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day4Details.DetailList != null) && (nvo.Day4Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day4Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day4Details.NextPlanID == 3L) && nvo.Day4Details.Isfreezed)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day5=", 1, " where  PlanTherapyID=", nvo.Day4Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day4Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day4Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day4Details.OocyteNumber);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                if ((nvo.Day4Details.NextPlanID == 4L) && nvo.Day4Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day4Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "ETID", DbType.Int64, nvo.Day4Details.ID);
                    this.dbServer.AddInParameter(command8, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, nvo.Day4Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day4Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day4");
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day4Details.GradeID);
                    this.dbServer.AddInParameter(command8, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FertStageID", DbType.Int64, nvo.Day4Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if ((nvo.Day4Details.NextPlanID == 2L) && nvo.Day4Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day4Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command10, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "VitrivicationID", DbType.Int64, nvo.Day4Details.ID);
                    this.dbServer.AddInParameter(command10, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "EmbNumber", DbType.Int64, nvo.Day4Details.OocyteNumber);
                    this.dbServer.AddInParameter(command10, "EmbSerialNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command10, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TransferDate", DbType.DateTime, nvo.Day4Details.Date);
                    this.dbServer.AddInParameter(command10, "TransferDay", DbType.String, "Day4");
                    this.dbServer.AddInParameter(command10, "CellStageID", DbType.String, nvo.Day4Details.CellStageID);
                    this.dbServer.AddInParameter(command10, "GradeID", DbType.Int64, nvo.Day4Details.GradeID);
                    this.dbServer.AddInParameter(command10, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command10, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command10, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command10, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command10, transaction);
                }
                if (nvo.Day4Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day4Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_0020;
                    TR_000C:
                        if ((nvo.Day4Details.NextPlanID == 3L) && nvo.Day4Details.Isfreezed)
                        {
                            DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day5=", 1, " where  PlanTherapyID=", nvo.Day4Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day4Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day4Details.SerialOocyteNumber, current.FilterID }));
                            this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                            this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        if ((nvo.Day4Details.NextPlanID == 4L) && nvo.Day4Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day4Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command18 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command18, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "ETID", DbType.Int64, nvo.Day4Details.ID);
                            this.dbServer.AddInParameter(command18, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command18, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command18, "TransferDate", DbType.DateTime, nvo.Day4Details.Date);
                            this.dbServer.AddInParameter(command18, "TransferDay", DbType.String, "Day4");
                            this.dbServer.AddInParameter(command18, "GradeID", DbType.Int64, nvo.Day4Details.GradeID);
                            this.dbServer.AddInParameter(command18, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FertStageID", DbType.Int64, nvo.Day4Details.CellStageID);
                            this.dbServer.AddInParameter(command18, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command18, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command18, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command18, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command18, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command18, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command18, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command18, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command18, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command18, transaction);
                        }
                        if ((nvo.Day4Details.NextPlanID == 2L) && nvo.Day4Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day4Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command20 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command20, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "VitrivicationID", DbType.Int64, nvo.Day4Details.ID);
                            this.dbServer.AddInParameter(command20, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command20, "EmbSerialNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command20, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TransferDate", DbType.DateTime, nvo.Day4Details.Date);
                            this.dbServer.AddInParameter(command20, "TransferDay", DbType.String, "Day4");
                            this.dbServer.AddInParameter(command20, "CellStageID", DbType.String, nvo.Day4Details.CellStageID);
                            this.dbServer.AddInParameter(command20, "GradeID", DbType.Int64, nvo.Day4Details.GradeID);
                            this.dbServer.AddInParameter(command20, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command20, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command20, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command20, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command20, transaction);
                        }
                    TR_0020:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day4=", 1, " where  PlanTherapyID=", nvo.Day4Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day4Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day4Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day4Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day4Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day4Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day4Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day4Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day4Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day4Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day4Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day4Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day4Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day4Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day4Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day4Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day4Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day4Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day4Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day4Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day4Details.Impression);
                                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, nvo.Day4Details.FrgmentationID);
                                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, nvo.Day4Details.BlastmereSymmetryID);
                                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, nvo.Day4Details.OtherDetails);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day4Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day4Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day4Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day4Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day4Details.OocyteDonorUnitID);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day4Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day4Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day4Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day4Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day4Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day4Details.CellNo);
                                    this.dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, nvo.Day4Details.AssistedHatching);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day4Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day4Details.ImgList != null) && (nvo.Day4Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day4Details.ImgList)
                                        {
                                            DbCommand command13 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command13, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "PatientID", DbType.Int64, nvo.Day4Details.PatientID);
                                            this.dbServer.AddInParameter(command13, "PatientUnitID", DbType.Int64, nvo.Day4Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyID", DbType.Int64, nvo.Day4Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyUnitID", DbType.Int64, nvo.Day4Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command13, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command13, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command13, "Day", DbType.Int64, 4);
                                            this.dbServer.AddInParameter(command13, "DayID", DbType.Int64, nvo.Day4Details.ID);
                                            this.dbServer.AddInParameter(command13, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "CellStageID", DbType.Int64, nvo.Day4Details.CellStageID);
                                            this.dbServer.AddInParameter(command13, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command13, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command13, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command13, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command13, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command13, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command13, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command13, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command13, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command13, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command13, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command13, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command13, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day4Details.DetailList != null) && (nvo.Day4Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day4Details.DetailList)
                                        {
                                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command14, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command14, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command14, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command14, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command14, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command14, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command14, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command14, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day4Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command14, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command14, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command14, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command14, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command14);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command14, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000C;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day4Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay5Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay5BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay5BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day5Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day5=", 1, " where  PlanTherapyID=", nvo.Day5Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day5Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day5Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day5Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day5Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day5Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day5Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day5Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day5Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day5Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day5Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day5Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day5Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day5Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day5Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day5Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day5Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day5Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day5Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day5Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day5Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day5Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "FrgmentationID", DbType.Int64, nvo.Day5Details.FrgmentationID);
                this.dbServer.AddInParameter(storedProcCommand, "BlastmereSymmetryID", DbType.Int64, nvo.Day5Details.BlastmereSymmetryID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDetails", DbType.String, nvo.Day5Details.OtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day5Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day5Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day5Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "StageofDevelopmentGrade", DbType.Int64, nvo.Day5Details.StageofDevelopmentGrade);
                this.dbServer.AddInParameter(storedProcCommand, "InnerCellMassGrade", DbType.Int64, nvo.Day5Details.InnerCellMassGrade);
                this.dbServer.AddInParameter(storedProcCommand, "TrophoectodermGrade", DbType.Int64, nvo.Day5Details.TrophoectodermGrade);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day5Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day5Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day5Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day5Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day5Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "IsAssistedHatching", DbType.Boolean, nvo.Day5Details.AssistedHatching);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day5Details.CellNo);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day5Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day5Details.ImgList != null) && (nvo.Day5Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day5Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day5Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 5);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day5Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day5Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day5Details.DetailList != null) && (nvo.Day5Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day5Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day5Details.NextPlanID == 3L) && nvo.Day5Details.Isfreezed)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day6=", 1, " where  PlanTherapyID=", nvo.Day5Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day5Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day5Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day5Details.OocyteNumber);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                }
                if ((nvo.Day5Details.NextPlanID == 4L) && nvo.Day5Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day5Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "ETID", DbType.Int64, nvo.Day5Details.ID);
                    this.dbServer.AddInParameter(command8, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, nvo.Day5Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day5Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day5");
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day5Details.GradeID);
                    this.dbServer.AddInParameter(command8, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FertStageID", DbType.Int64, nvo.Day5Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command8, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if ((nvo.Day5Details.NextPlanID == 2L) && nvo.Day5Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day5Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command10 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command10, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "VitrivicationID", DbType.Int64, nvo.Day5Details.ID);
                    this.dbServer.AddInParameter(command10, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command10, "EmbNumber", DbType.Int64, nvo.Day5Details.OocyteNumber);
                    this.dbServer.AddInParameter(command10, "EmbSerialNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command10, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "TransferDate", DbType.DateTime, nvo.Day5Details.Date);
                    this.dbServer.AddInParameter(command10, "TransferDay", DbType.String, "Day5");
                    this.dbServer.AddInParameter(command10, "CellStageID", DbType.String, nvo.Day5Details.CellStageID);
                    this.dbServer.AddInParameter(command10, "GradeID", DbType.Int64, nvo.Day5Details.GradeID);
                    this.dbServer.AddInParameter(command10, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command10, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command10, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command10, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command10, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command10, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command10, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command10, transaction);
                }
                if (nvo.Day5Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day5Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_0020;
                    TR_000C:
                        if ((nvo.Day5Details.NextPlanID == 3L) && nvo.Day5Details.Isfreezed)
                        {
                            DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day6=", 1, " where  PlanTherapyID=", nvo.Day5Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day5Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day5Details.SerialOocyteNumber, current.FilterID }));
                            this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, null);
                            this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, null);
                            this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, null);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                        }
                        if ((nvo.Day5Details.NextPlanID == 4L) && nvo.Day5Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day5Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command18 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command18, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command18, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "ETID", DbType.Int64, nvo.Day5Details.ID);
                            this.dbServer.AddInParameter(command18, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command18, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command18, "TransferDate", DbType.DateTime, nvo.Day5Details.Date);
                            this.dbServer.AddInParameter(command18, "TransferDay", DbType.String, "Day5");
                            this.dbServer.AddInParameter(command18, "GradeID", DbType.Int64, nvo.Day5Details.GradeID);
                            this.dbServer.AddInParameter(command18, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FertStageID", DbType.Int64, nvo.Day5Details.CellStageID);
                            this.dbServer.AddInParameter(command18, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command18, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command18, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command18, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command18, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command18, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command18, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command18, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command18, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command18, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command18, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command18, transaction);
                        }
                        if ((nvo.Day5Details.NextPlanID == 2L) && nvo.Day5Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day5Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command20 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command20, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command20, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "VitrivicationID", DbType.Int64, nvo.Day5Details.ID);
                            this.dbServer.AddInParameter(command20, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command20, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command20, "EmbSerialNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command20, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "TransferDate", DbType.DateTime, nvo.Day5Details.Date);
                            this.dbServer.AddInParameter(command20, "TransferDay", DbType.String, "Day5");
                            this.dbServer.AddInParameter(command20, "CellStageID", DbType.String, nvo.Day5Details.CellStageID);
                            this.dbServer.AddInParameter(command20, "GradeID", DbType.Int64, nvo.Day5Details.GradeID);
                            this.dbServer.AddInParameter(command20, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command20, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command20, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command20, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command20, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command20, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command20, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command20, transaction);
                        }
                    TR_0020:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day5=", 1, " where  PlanTherapyID=", nvo.Day5Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day5Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day5Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day5Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day5Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day5Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day5Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day5Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day5Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day5Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day5Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day5Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day5Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day5Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day5Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day5Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day5Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day5Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day5Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day5Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day5Details.Impression);
                                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, nvo.Day5Details.FrgmentationID);
                                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, nvo.Day5Details.BlastmereSymmetryID);
                                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, nvo.Day5Details.OtherDetails);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day5Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day5Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day5Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, nvo.Day5Details.StageofDevelopmentGrade);
                                    this.dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, nvo.Day5Details.InnerCellMassGrade);
                                    this.dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, nvo.Day5Details.TrophoectodermGrade);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day5Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day5Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day5Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day5Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day5Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, nvo.Day5Details.AssistedHatching);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day5Details.CellNo);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day5Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day5Details.OocyteDonorUnitID);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day5Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day5Details.ImgList != null) && (nvo.Day5Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day5Details.ImgList)
                                        {
                                            DbCommand command13 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command13, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "PatientID", DbType.Int64, nvo.Day5Details.PatientID);
                                            this.dbServer.AddInParameter(command13, "PatientUnitID", DbType.Int64, nvo.Day5Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyID", DbType.Int64, nvo.Day5Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command13, "PlanTherapyUnitID", DbType.Int64, nvo.Day5Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command13, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command13, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command13, "Day", DbType.Int64, 5);
                                            this.dbServer.AddInParameter(command13, "DayID", DbType.Int64, nvo.Day5Details.ID);
                                            this.dbServer.AddInParameter(command13, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "CellStageID", DbType.Int64, nvo.Day5Details.CellStageID);
                                            this.dbServer.AddInParameter(command13, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command13, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command13, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command13, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command13, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command13, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command13, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command13, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command13, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command13, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command13, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command13, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command13, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command13, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day5Details.DetailList != null) && (nvo.Day5Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day5Details.DetailList)
                                        {
                                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command14, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command14, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command14, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command14, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command14, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command14, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command14, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command14, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command14, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command14, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command14, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day5Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command14, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command14, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command14, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command14, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command14);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command14, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000C;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day5Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay6Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay6BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay6BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day6Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day6=", 1, " where  PlanTherapyID=", nvo.Day6Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day6Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day6Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day6Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day6Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day6Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day6Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day6Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day6Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day6Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day6Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day6Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day6Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day6Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day6Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day6Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day6Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day6Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day6Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day6Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day6Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day6Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "FrgmentationID", DbType.Int64, nvo.Day6Details.FrgmentationID);
                this.dbServer.AddInParameter(storedProcCommand, "BlastmereSymmetryID", DbType.Int64, nvo.Day6Details.BlastmereSymmetryID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDetails", DbType.String, nvo.Day6Details.OtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day6Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day6Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day6Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "StageofDevelopmentGrade", DbType.Int64, nvo.Day6Details.StageofDevelopmentGrade);
                this.dbServer.AddInParameter(storedProcCommand, "InnerCellMassGrade", DbType.Int64, nvo.Day6Details.InnerCellMassGrade);
                this.dbServer.AddInParameter(storedProcCommand, "TrophoectodermGrade", DbType.Int64, nvo.Day6Details.TrophoectodermGrade);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day6Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day6Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day6Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day6Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day6Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "IsAssistedHatching", DbType.Boolean, nvo.Day6Details.AssistedHatching);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day6Details.CellNo);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day6Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day6Details.OocyteDonorUnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day6Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day6Details.ImgList != null) && (nvo.Day6Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day6Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day6Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 6);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day6Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day6Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day6Details.DetailList != null) && (nvo.Day6Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day6Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day6Details.NextPlanID == 4L) && nvo.Day6Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day6Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "ETID", DbType.Int64, nvo.Day6Details.ID);
                    this.dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, nvo.Day6Details.OocyteNumber);
                    this.dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, nvo.Day6Details.Date);
                    this.dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day6");
                    this.dbServer.AddInParameter(command6, "GradeID", DbType.Int64, nvo.Day6Details.GradeID);
                    this.dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, nvo.Day6Details.CellStageID);
                    this.dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, nvo.Day6Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, nvo.Day6Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((nvo.Day6Details.NextPlanID == 2L) && nvo.Day6Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day6Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "VitrivicationID", DbType.Int64, nvo.Day6Details.ID);
                    this.dbServer.AddInParameter(command8, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "EmbNumber", DbType.Int64, nvo.Day6Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "EmbSerialNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day6Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day6");
                    this.dbServer.AddInParameter(command8, "CellStageID", DbType.String, nvo.Day6Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day6Details.GradeID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day6Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day6Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command8, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command8, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if (nvo.Day6Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day6Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_001E;
                    TR_000A:
                        if ((nvo.Day6Details.NextPlanID == 4L) && nvo.Day6Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day6Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command14, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "ETID", DbType.Int64, nvo.Day6Details.ID);
                            this.dbServer.AddInParameter(command14, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command14, "TransferDate", DbType.DateTime, nvo.Day6Details.Date);
                            this.dbServer.AddInParameter(command14, "TransferDay", DbType.String, "Day6");
                            this.dbServer.AddInParameter(command14, "GradeID", DbType.Int64, nvo.Day6Details.GradeID);
                            this.dbServer.AddInParameter(command14, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "FertStageID", DbType.Int64, nvo.Day6Details.CellStageID);
                            this.dbServer.AddInParameter(command14, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command14, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command14, "OocyteDonorID", DbType.Int64, nvo.Day6Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command14, "OocyteDonorUnitID", DbType.Int64, nvo.Day6Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command14, transaction);
                        }
                        if ((nvo.Day6Details.NextPlanID == 2L) && nvo.Day6Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day6Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command16 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command16, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command16, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "VitrivicationID", DbType.Int64, nvo.Day6Details.ID);
                            this.dbServer.AddInParameter(command16, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command16, "EmbSerialNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command16, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "TransferDate", DbType.DateTime, nvo.Day6Details.Date);
                            this.dbServer.AddInParameter(command16, "TransferDay", DbType.String, "Day6");
                            this.dbServer.AddInParameter(command16, "CellStageID", DbType.String, nvo.Day6Details.CellStageID);
                            this.dbServer.AddInParameter(command16, "GradeID", DbType.Int64, nvo.Day6Details.GradeID);
                            this.dbServer.AddInParameter(command16, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command16, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command16, "OocyteDonorID", DbType.Int64, nvo.Day6Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command16, "OocyteDonorUnitID", DbType.Int64, nvo.Day6Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command16, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command16, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command16, transaction);
                        }
                    TR_001E:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day6=", 1, " where  PlanTherapyID=", nvo.Day6Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day6Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day6Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day6Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day6Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day6Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day6Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day6Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day6Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day6Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day6Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day6Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day6Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day6Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day6Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day6Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day6Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day6Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day6Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day6Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day6Details.Impression);
                                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, nvo.Day6Details.FrgmentationID);
                                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, nvo.Day6Details.BlastmereSymmetryID);
                                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, nvo.Day6Details.OtherDetails);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day6Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day6Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day6Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, nvo.Day6Details.StageofDevelopmentGrade);
                                    this.dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, nvo.Day6Details.InnerCellMassGrade);
                                    this.dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, nvo.Day6Details.TrophoectodermGrade);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day6Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day6Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day6Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day6Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day6Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, nvo.Day6Details.AssistedHatching);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day6Details.CellNo);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day6Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day6Details.OocyteDonorUnitID);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day6Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day6Details.ImgList != null) && (nvo.Day6Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day6Details.ImgList)
                                        {
                                            DbCommand command11 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command11, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command11, "PatientID", DbType.Int64, nvo.Day6Details.PatientID);
                                            this.dbServer.AddInParameter(command11, "PatientUnitID", DbType.Int64, nvo.Day6Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command11, "PlanTherapyID", DbType.Int64, nvo.Day6Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command11, "PlanTherapyUnitID", DbType.Int64, nvo.Day6Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command11, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command11, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command11, "Day", DbType.Int64, 6);
                                            this.dbServer.AddInParameter(command11, "DayID", DbType.Int64, nvo.Day6Details.ID);
                                            this.dbServer.AddInParameter(command11, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command11, "CellStageID", DbType.Int64, nvo.Day6Details.CellStageID);
                                            this.dbServer.AddInParameter(command11, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command11, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command11, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command11, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command11, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command11, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command11, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command11, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command11, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day6Details.DetailList != null) && (nvo.Day6Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day6Details.DetailList)
                                        {
                                            DbCommand command12 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command12, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command12, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command12, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command12, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command12, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command12, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command12, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command12, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command12, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command12, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command12, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command12, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command12, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command12, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command12, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command12, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command12, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command12, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command12, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command12, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command12, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command12, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command12, "SerialOocyteNumber", DbType.Int64, nvo.Day6Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command12, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command12, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command12, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command12, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command12);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command12, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000A;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day6Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDay7Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay7BizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateDay7BizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.Day7Details.DecisionID == 0L)
                {
                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day7=", 1, " where  PlanTherapyID=", nvo.Day7Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day7Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day7Details.SerialOocyteNumber }));
                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                }
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay7");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.Day7Details.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.Day7Details.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.Day7Details.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.Day7Details.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.Day7Details.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AnesthetistID", DbType.Int64, nvo.Day7Details.AnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantAnesthetistID", DbType.Int64, nvo.Day7Details.AssitantAnesthetistID);
                this.dbServer.AddInParameter(storedProcCommand, "CumulusID", DbType.Int64, nvo.Day7Details.CumulusID);
                this.dbServer.AddInParameter(storedProcCommand, "MOIID", DbType.Int64, nvo.Day7Details.MOIID);
                this.dbServer.AddInParameter(storedProcCommand, "GradeID", DbType.Int64, nvo.Day7Details.GradeID);
                this.dbServer.AddInParameter(storedProcCommand, "CellStageID", DbType.Int64, nvo.Day7Details.CellStageID);
                this.dbServer.AddInParameter(storedProcCommand, "OccDiamension", DbType.String, nvo.Day7Details.OccDiamension);
                this.dbServer.AddInParameter(storedProcCommand, "SpermPreperationMedia", DbType.String, nvo.Day7Details.SpermPreperationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "OocytePreparationMedia", DbType.String, nvo.Day7Details.OocytePreparationMedia);
                this.dbServer.AddInParameter(storedProcCommand, "IncubatorID", DbType.Int64, nvo.Day7Details.IncubatorID);
                this.dbServer.AddInParameter(storedProcCommand, "FinalLayering", DbType.String, nvo.Day7Details.FinalLayering);
                this.dbServer.AddInParameter(storedProcCommand, "NextPlanID", DbType.Int64, nvo.Day7Details.NextPlanID);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.Day7Details.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "Impression", DbType.String, nvo.Day7Details.Impression);
                this.dbServer.AddInParameter(storedProcCommand, "FrgmentationID", DbType.Int64, nvo.Day7Details.FrgmentationID);
                this.dbServer.AddInParameter(storedProcCommand, "BlastmereSymmetryID", DbType.Int64, nvo.Day7Details.BlastmereSymmetryID);
                this.dbServer.AddInParameter(storedProcCommand, "OtherDetails", DbType.String, nvo.Day7Details.OtherDetails);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day7Details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationDate", DbType.DateTime, nvo.Day7Details.CellObservationDate);
                this.dbServer.AddInParameter(storedProcCommand, "CellObservationTime", DbType.DateTime, nvo.Day7Details.CellObservationTime);
                this.dbServer.AddInParameter(storedProcCommand, "StageofDevelopmentGrade", DbType.Int64, nvo.Day7Details.StageofDevelopmentGrade);
                this.dbServer.AddInParameter(storedProcCommand, "InnerCellMassGrade", DbType.Int64, nvo.Day7Details.InnerCellMassGrade);
                this.dbServer.AddInParameter(storedProcCommand, "TrophoectodermGrade", DbType.Int64, nvo.Day7Details.TrophoectodermGrade);
                this.dbServer.AddInParameter(storedProcCommand, "CellStage", DbType.String, nvo.Day7Details.CellStage);
                this.dbServer.AddInParameter(storedProcCommand, "IsBiopsy", DbType.Boolean, nvo.Day7Details.IsBiopsy);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyDate", DbType.DateTime, nvo.Day7Details.BiopsyDate);
                this.dbServer.AddInParameter(storedProcCommand, "BiopsyTime", DbType.DateTime, nvo.Day7Details.BiopsyTime);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfCell", DbType.Int64, nvo.Day7Details.NoOfCell);
                this.dbServer.AddInParameter(storedProcCommand, "IsAssistedHatching", DbType.Boolean, nvo.Day7Details.AssistedHatching);
                this.dbServer.AddInParameter(storedProcCommand, "CellNo", DbType.Int64, nvo.Day7Details.CellNo);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorID", DbType.Int64, nvo.Day7Details.OocyteDonorID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteDonorUnitID", DbType.Int64, nvo.Day7Details.OocyteDonorUnitID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.Day7Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.Day7Details.ImgList != null) && (nvo.Day7Details.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.Day7Details.ImgList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, nvo.Day7Details.OocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, 7);
                        this.dbServer.AddInParameter(command, "DayID", DbType.Int64, nvo.Day7Details.ID);
                        this.dbServer.AddInParameter(command, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day7Details.CellStageID);
                        this.dbServer.AddInParameter(command, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                if ((nvo.Day7Details.DetailList != null) && (nvo.Day7Details.DetailList.Count > 0))
                {
                    foreach (clsIVFDashboard_TherapyDocumentVO tvo in nvo.Day7Details.DetailList)
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                        this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, tvo.Date);
                        this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, tvo.PatientID);
                        this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, tvo.PatientUnitID);
                        this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, tvo.PlanTherapyID);
                        this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, tvo.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command, "Description", DbType.String, tvo.Description);
                        this.dbServer.AddInParameter(command, "Title", DbType.String, tvo.Title);
                        this.dbServer.AddInParameter(command, "AttachedFileName", DbType.String, tvo.AttachedFileName);
                        this.dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, tvo.AttachedFileContent);
                        this.dbServer.AddInParameter(command, "Status", DbType.Boolean, tvo.Status);
                        this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, tvo.OocyteNumber);
                        this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, tvo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command, "Day", DbType.Int64, tvo.Day);
                        this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                        this.dbServer.AddInParameter(command, "IsApplyTo", DbType.Int32, 0);
                        if (tvo.DocNo == null)
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.DocNo);
                        }
                        this.dbServer.ExecuteNonQuery(command);
                        tvo.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command, "DocNo"));
                    }
                }
                if ((nvo.Day7Details.NextPlanID == 4L) && nvo.Day7Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day7Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "ETID", DbType.Int64, nvo.Day7Details.ID);
                    this.dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, nvo.Day7Details.OocyteNumber);
                    this.dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, nvo.Day7Details.Date);
                    this.dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day7");
                    this.dbServer.AddInParameter(command6, "GradeID", DbType.Int64, nvo.Day7Details.GradeID);
                    this.dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, nvo.Day7Details.CellStageID);
                    this.dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, nvo.Day7Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, nvo.Day7Details.OocyteDonorUnitID);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                }
                if ((nvo.Day7Details.NextPlanID == 2L) && nvo.Day7Details.Isfreezed)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                    nvo.Day7Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                    DbCommand command8 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command8, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "VitrivicationID", DbType.Int64, nvo.Day7Details.ID);
                    this.dbServer.AddInParameter(command8, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command8, "EmbNumber", DbType.Int64, nvo.Day7Details.OocyteNumber);
                    this.dbServer.AddInParameter(command8, "EmbSerialNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command8, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "TransferDate", DbType.DateTime, nvo.Day7Details.Date);
                    this.dbServer.AddInParameter(command8, "TransferDay", DbType.String, "Day7");
                    this.dbServer.AddInParameter(command8, "CellStageID", DbType.String, nvo.Day7Details.CellStageID);
                    this.dbServer.AddInParameter(command8, "GradeID", DbType.Int64, nvo.Day7Details.GradeID);
                    this.dbServer.AddInParameter(command8, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command8, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command8, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command8, "OocyteDonorID", DbType.Int64, nvo.Day7Details.OocyteDonorID);
                    this.dbServer.AddInParameter(command8, "OocyteDonorUnitID", DbType.Int64, nvo.Day7Details.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command8, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command8, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command8, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.ExecuteNonQuery(command8, transaction);
                }
                if (nvo.Day7Details.OcyteListList != null)
                {
                    using (List<MasterListItem>.Enumerator enumerator3 = nvo.Day7Details.OcyteListList.GetEnumerator())
                    {
                        MasterListItem current;
                        goto TR_001E;
                    TR_000A:
                        if ((nvo.Day7Details.NextPlanID == 4L) && nvo.Day7Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "Time", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "PatternID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, null);
                            this.dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day7Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command14 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                            this.dbServer.AddInParameter(command14, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command14, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "ETID", DbType.Int64, nvo.Day7Details.ID);
                            this.dbServer.AddInParameter(command14, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "OocyteNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command14, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command14, "TransferDate", DbType.DateTime, nvo.Day7Details.Date);
                            this.dbServer.AddInParameter(command14, "TransferDay", DbType.String, "Day7");
                            this.dbServer.AddInParameter(command14, "GradeID", DbType.Int64, nvo.Day7Details.GradeID);
                            this.dbServer.AddInParameter(command14, "Score", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "FertStageID", DbType.Int64, nvo.Day7Details.CellStageID);
                            this.dbServer.AddInParameter(command14, "EmbStatus", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command14, "FileName", DbType.String, null);
                            this.dbServer.AddInParameter(command14, "FileContents", DbType.Binary, null);
                            this.dbServer.AddInParameter(command14, "Status", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command14, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command14, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command14, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command14, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command14, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command14, "OocyteDonorID", DbType.Int64, nvo.Day7Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command14, "OocyteDonorUnitID", DbType.Int64, nvo.Day7Details.OocyteDonorUnitID);
                            this.dbServer.ExecuteNonQuery(command14, transaction);
                        }
                        if ((nvo.Day7Details.NextPlanID == 2L) && nvo.Day7Details.Isfreezed)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                            this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                            this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                            this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command, "DateTime", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, null);
                            this.dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, null);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command, "FromForm", DbType.Int64, 1);
                            this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                            nvo.Day7Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                            DbCommand command16 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command16, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command16, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "VitrivicationID", DbType.Int64, nvo.Day7Details.ID);
                            this.dbServer.AddInParameter(command16, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command16, "EmbNumber", DbType.Int64, current.ID);
                            this.dbServer.AddInParameter(command16, "EmbSerialNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber + current.FilterID);
                            this.dbServer.AddInParameter(command16, "LeafNo", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "EmbDays", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "ColorCodeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "CanId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "StrawId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "GobletShapeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "GobletSizeId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "TankId", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "ConistorNo", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "ProtocolTypeID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "TransferDate", DbType.DateTime, nvo.Day7Details.Date);
                            this.dbServer.AddInParameter(command16, "TransferDay", DbType.String, "Day7");
                            this.dbServer.AddInParameter(command16, "CellStageID", DbType.String, nvo.Day7Details.CellStageID);
                            this.dbServer.AddInParameter(command16, "GradeID", DbType.Int64, nvo.Day7Details.GradeID);
                            this.dbServer.AddInParameter(command16, "EmbStatus", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command16, "UsedOwnOocyte", DbType.Boolean, null);
                            this.dbServer.AddInParameter(command16, "IsThawingDone", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command16, "OocyteDonorID", DbType.Int64, nvo.Day7Details.OocyteDonorID);
                            this.dbServer.AddInParameter(command16, "OocyteDonorUnitID", DbType.Int64, nvo.Day7Details.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command16, "UsedByOtherCycle", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command16, "UsedTherapyID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "UsedTherapyUnitID", DbType.Int64, null);
                            this.dbServer.AddInParameter(command16, "ReceivingDate", DbType.DateTime, null);
                            this.dbServer.ExecuteNonQuery(command16, transaction);
                        }
                    TR_001E:
                        while (true)
                        {
                            if (enumerator3.MoveNext())
                            {
                                current = enumerator3.Current;
                                try
                                {
                                    DbCommand sqlStringCommand = this.dbServer.GetSqlStringCommand(string.Concat(new object[] { "Update T_IVFDashboard_GraphicalRepresentation set Day7=", 1, " where  PlanTherapyID=", nvo.Day7Details.PlanTherapyID, " and PlanTherapyUnitID=", nvo.Day7Details.PlanTherapyUnitID, " and SerialOocyteNumber=", nvo.Day7Details.SerialOocyteNumber + current.FilterID }));
                                    this.dbServer.ExecuteNonQuery(sqlStringCommand, transaction);
                                    DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay7");
                                    this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                                    this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                                    this.dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber + current.FilterID);
                                    this.dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(command, "Date", DbType.DateTime, nvo.Day7Details.Date);
                                    this.dbServer.AddInParameter(command, "Time", DbType.DateTime, nvo.Day7Details.Time);
                                    this.dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, nvo.Day7Details.EmbryologistID);
                                    this.dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, nvo.Day7Details.AssitantEmbryologistID);
                                    this.dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, nvo.Day7Details.AnesthetistID);
                                    this.dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, nvo.Day7Details.AssitantAnesthetistID);
                                    this.dbServer.AddInParameter(command, "CumulusID", DbType.Int64, nvo.Day7Details.CumulusID);
                                    this.dbServer.AddInParameter(command, "MOIID", DbType.Int64, nvo.Day7Details.MOIID);
                                    this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, nvo.Day7Details.GradeID);
                                    this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, nvo.Day7Details.CellStageID);
                                    this.dbServer.AddInParameter(command, "OccDiamension", DbType.String, nvo.Day7Details.OccDiamension);
                                    this.dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, nvo.Day7Details.SpermPreperationMedia);
                                    this.dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, nvo.Day7Details.OocytePreparationMedia);
                                    this.dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, nvo.Day7Details.IncubatorID);
                                    this.dbServer.AddInParameter(command, "FinalLayering", DbType.String, nvo.Day7Details.FinalLayering);
                                    this.dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, nvo.Day7Details.NextPlanID);
                                    this.dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, nvo.Day7Details.Isfreezed);
                                    this.dbServer.AddInParameter(command, "Impression", DbType.String, nvo.Day7Details.Impression);
                                    this.dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, nvo.Day7Details.FrgmentationID);
                                    this.dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, nvo.Day7Details.BlastmereSymmetryID);
                                    this.dbServer.AddInParameter(command, "OtherDetails", DbType.String, nvo.Day7Details.OtherDetails);
                                    this.dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.Day7Details.ID);
                                    this.dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, nvo.Day7Details.CellObservationDate);
                                    this.dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, nvo.Day7Details.CellObservationTime);
                                    this.dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, nvo.Day7Details.StageofDevelopmentGrade);
                                    this.dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, nvo.Day7Details.InnerCellMassGrade);
                                    this.dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, nvo.Day7Details.TrophoectodermGrade);
                                    this.dbServer.AddInParameter(command, "CellStage", DbType.String, nvo.Day7Details.CellStage);
                                    this.dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, nvo.Day7Details.IsBiopsy);
                                    this.dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, nvo.Day7Details.BiopsyDate);
                                    this.dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, nvo.Day7Details.BiopsyTime);
                                    this.dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, nvo.Day7Details.NoOfCell);
                                    this.dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, nvo.Day7Details.AssistedHatching);
                                    this.dbServer.AddInParameter(command, "CellNo", DbType.Int64, nvo.Day7Details.CellNo);
                                    this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, nvo.Day7Details.OocyteDonorID);
                                    this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, nvo.Day7Details.OocyteDonorUnitID);
                                    this.dbServer.ExecuteNonQuery(command, transaction);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command, "ResultStatus");
                                    nvo.Day7Details.ID = (long) this.dbServer.GetParameterValue(command, "ID");
                                    if ((nvo.Day7Details.ImgList != null) && (nvo.Day7Details.ImgList.Count > 0))
                                    {
                                        foreach (clsAddImageVO evo2 in nvo.Day7Details.ImgList)
                                        {
                                            DbCommand command11 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                            this.dbServer.AddInParameter(command11, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command11, "PatientID", DbType.Int64, nvo.Day7Details.PatientID);
                                            this.dbServer.AddInParameter(command11, "PatientUnitID", DbType.Int64, nvo.Day7Details.PatientUnitID);
                                            this.dbServer.AddInParameter(command11, "PlanTherapyID", DbType.Int64, nvo.Day7Details.PlanTherapyID);
                                            this.dbServer.AddInParameter(command11, "PlanTherapyUnitID", DbType.Int64, nvo.Day7Details.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command11, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command11, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command11, "Day", DbType.Int64, 7);
                                            this.dbServer.AddInParameter(command11, "DayID", DbType.Int64, nvo.Day7Details.ID);
                                            this.dbServer.AddInParameter(command11, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command11, "CellStageID", DbType.Int64, nvo.Day7Details.CellStageID);
                                            this.dbServer.AddInParameter(command11, "FileName", DbType.String, evo2.ImagePath);
                                            this.dbServer.AddInParameter(command11, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddParameter(command11, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.SeqNo);
                                            this.dbServer.AddInParameter(command11, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.AddParameter(command11, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ServerImageName);
                                            this.dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                            this.dbServer.ExecuteNonQuery(command11, transaction);
                                            evo2.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command11, "ID"));
                                            evo2.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command11, "ServerImageName"));
                                            evo2.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command11, "SeqNo"));
                                        }
                                    }
                                    if ((nvo.Day7Details.DetailList != null) && (nvo.Day7Details.DetailList.Count > 0))
                                    {
                                        foreach (clsIVFDashboard_TherapyDocumentVO tvo2 in nvo.Day7Details.DetailList)
                                        {
                                            DbCommand command12 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");
                                            this.dbServer.AddInParameter(command12, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                            this.dbServer.AddInParameter(command12, "DocumentDate", DbType.DateTime, tvo2.Date);
                                            this.dbServer.AddInParameter(command12, "PatientID", DbType.Int64, tvo2.PatientID);
                                            this.dbServer.AddInParameter(command12, "PatientUnitID", DbType.Int64, tvo2.PatientUnitID);
                                            this.dbServer.AddInParameter(command12, "PlanTherapyID", DbType.Int64, tvo2.PlanTherapyID);
                                            this.dbServer.AddInParameter(command12, "PlanTherapyUnitID", DbType.Int64, tvo2.PlanTherapyUnitID);
                                            this.dbServer.AddInParameter(command12, "Description", DbType.String, tvo2.Description);
                                            this.dbServer.AddInParameter(command12, "Title", DbType.String, tvo2.Title);
                                            this.dbServer.AddInParameter(command12, "AttachedFileName", DbType.String, tvo2.AttachedFileName);
                                            this.dbServer.AddInParameter(command12, "AttachedFileContent", DbType.Binary, tvo2.AttachedFileContent);
                                            this.dbServer.AddInParameter(command12, "Status", DbType.Boolean, tvo2.Status);
                                            this.dbServer.AddInParameter(command12, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command12, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                            this.dbServer.AddInParameter(command12, "AddedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command12, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command12, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                            this.dbServer.AddInParameter(command12, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command12, "UpdatedBy", DbType.Int64, UserVo.ID);
                                            this.dbServer.AddInParameter(command12, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                            this.dbServer.AddInParameter(command12, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                            this.dbServer.AddInParameter(command12, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                            this.dbServer.AddInParameter(command12, "OocyteNumber", DbType.Int64, current.ID);
                                            this.dbServer.AddInParameter(command12, "SerialOocyteNumber", DbType.Int64, nvo.Day7Details.SerialOocyteNumber + current.FilterID);
                                            this.dbServer.AddInParameter(command12, "Day", DbType.Int64, tvo2.Day);
                                            this.dbServer.AddParameter(command12, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.DocNo);
                                            this.dbServer.AddParameter(command12, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo2.ID);
                                            this.dbServer.AddInParameter(command12, "IsApplyTo", DbType.Int32, 1);
                                            this.dbServer.ExecuteNonQuery(command12);
                                            tvo2.DocNo = Convert.ToString(this.dbServer.GetParameterValue(command12, "DocNo"));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_000A;
                    }
                }
            TR_0006:
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.Day7Details = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDecision(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdateDecisionBizActionVO nvo = valueObject as cls_IVFDashboard_AddUpdateDecisionBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDecision");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.ETDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.ETDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.ETDetails.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.ETDetails.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "DecisionID", DbType.Int64, nvo.ETDetails.DecisionID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonate", DbType.Boolean, nvo.ETDetails.IsDonate);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonateCryo", DbType.Boolean, nvo.ETDetails.IsDonateCryo);
                this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientID", DbType.Int64, nvo.ETDetails.RecepientPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientUnitID", DbType.Int64, nvo.ETDetails.RecepientPatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycleDonate", DbType.Boolean, nvo.ETDetails.IsDonorCycleDonate);
                this.dbServer.AddInParameter(storedProcCommand, "IsDonorCycleDonateCryo", DbType.Boolean, nvo.ETDetails.IsDonorCycleDonateCryo);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.ETDetails.DecisionID == 3L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.ETDetails.PlanTherapyID);
                    this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.ETDetails.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "PatternID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "UterineArtery_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "UterineArtery_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "UterineArtery_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "Endometerial_PI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "Endometerial_RI", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "Endometerial_SD", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "CatheterTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "DistanceFundus", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command2, "EndometriumThickness", DbType.Decimal, null);
                    this.dbServer.AddInParameter(command2, "TeatmentUnderGA", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "Difficulty", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "DifficultyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "TenaculumUsed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command2, "IsFreezed", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command2, "IsOnlyET", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command2, "AnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "AssistantAnethetistID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command2, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command2, "IsAnesthesia", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command2, "FreshEmb", DbType.Int64, null);
                    this.dbServer.AddInParameter(command2, "FrozenEmb", DbType.Int64, null);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    nvo.ETDetails.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                    this.dbServer.AddInParameter(command3, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "ETID", DbType.Int64, nvo.ETDetails.ID);
                    this.dbServer.AddInParameter(command3, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "OocyteNumber", DbType.Int64, nvo.ETDetails.OocyteNumber);
                    this.dbServer.AddInParameter(command3, "SerialOocyteNumber", DbType.Int64, nvo.ETDetails.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command3, "TransferDate", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command3, "TransferDay", DbType.String, nvo.ETDetails.Day);
                    this.dbServer.AddInParameter(command3, "GradeID", DbType.Int64, nvo.ETDetails.GradeID);
                    this.dbServer.AddInParameter(command3, "Score", DbType.String, null);
                    this.dbServer.AddInParameter(command3, "FertStageID", DbType.Int64, nvo.ETDetails.CellStageID);
                    this.dbServer.AddInParameter(command3, "EmbStatus", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command3, "FileName", DbType.String, null);
                    this.dbServer.AddInParameter(command3, "FileContents", DbType.Binary, null);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command3, "OocyteDonorID", DbType.Int64, nvo.ETDetails.OocyteDonorID);
                    this.dbServer.AddInParameter(command3, "OocyteDonorUnitID", DbType.Int64, nvo.ETDetails.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command3, "EmbTransferDay", DbType.Int64, nvo.ETDetails.DayNo);
                    this.dbServer.AddInParameter(command3, "Remark", DbType.String, nvo.ETDetails.Remark);
                    this.dbServer.AddInParameter(command3, "GradeID_New", DbType.Int64, nvo.ETDetails.GradeID);
                    this.dbServer.AddInParameter(command3, "StageofDevelopmentGrade", DbType.Int64, nvo.ETDetails.StageofDevelopmentGrade);
                    this.dbServer.AddInParameter(command3, "InnerCellMassGrade", DbType.Int64, nvo.ETDetails.InnerCellMassGrade);
                    this.dbServer.AddInParameter(command3, "TrophoectodermGrade", DbType.Int64, nvo.ETDetails.TrophoectodermGrade);
                    this.dbServer.AddInParameter(command3, "CellStage", DbType.String, nvo.ETDetails.CellStage);
                    this.dbServer.AddInParameter(command3, "IsFreshEmbryo", DbType.Boolean, nvo.ETDetails.IsFreshEmbryo);
                    this.dbServer.AddInParameter(command3, "IsFrozenEmbryo", DbType.Boolean, nvo.ETDetails.IsFrozenEmbryo);
                    this.dbServer.AddInParameter(command3, "IsFreshEmbryoPGDPGS", DbType.Boolean, nvo.ETDetails.IsFreshEmbryoPGDPGS);
                    this.dbServer.AddInParameter(command3, "IsFrozenEmbryoPGDPGS", DbType.Boolean, nvo.ETDetails.IsFrozenEmbryoPGDPGS);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                }
                if (nvo.ETDetails.DecisionID == 2L)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                    this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, nvo.ETDetails.PlanTherapyID);
                    this.dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, nvo.ETDetails.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, false);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                    nvo.ETDetails.ID = (long) this.dbServer.GetParameterValue(command4, "ID");
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, nvo.ETDetails.ID);
                    this.dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, nvo.ETDetails.OocyteNumber);
                    this.dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, nvo.ETDetails.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, nvo.ETDetails.PlanDate);
                    this.dbServer.AddInParameter(command5, "TransferDay", DbType.String, nvo.ETDetails.Day);
                    this.dbServer.AddInParameter(command5, "CellStageID", DbType.String, nvo.ETDetails.CellStageID);
                    this.dbServer.AddInParameter(command5, "GradeID", DbType.Int64, nvo.ETDetails.GradeID);
                    this.dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, nvo.ETDetails.OocyteDonorID);
                    this.dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, nvo.ETDetails.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, nvo.ETDetails.DayNo);
                    this.dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, nvo.ETDetails.GradeID);
                    this.dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, nvo.ETDetails.StageofDevelopmentGrade);
                    this.dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, nvo.ETDetails.InnerCellMassGrade);
                    this.dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, nvo.ETDetails.TrophoectodermGrade);
                    this.dbServer.AddInParameter(command5, "CellStage", DbType.String, nvo.ETDetails.CellStage);
                    this.dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, nvo.ETDetails.VitrificationDate);
                    this.dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, nvo.ETDetails.VitrificationTime);
                    this.dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, nvo.ETDetails.VitrificationNo);
                    this.dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command5, "IsFreshEmbryoPGDPGS", DbType.Boolean, nvo.ETDetails.IsFreshEmbryoPGDPGS);
                    this.dbServer.AddInParameter(command5, "IsFrozenEmbryoPGDPGS", DbType.Boolean, nvo.ETDetails.IsFrozenEmbryoPGDPGS);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                }
                if (nvo.ETDetails.DecisionID == 5L)
                {
                    DbCommand command6 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                    this.dbServer.AddInParameter(command6, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command6, "PlanTherapyID", DbType.Int64, nvo.ETDetails.PlanTherapyID);
                    this.dbServer.AddInParameter(command6, "PlanTherapyUnitID", DbType.Int64, nvo.ETDetails.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command6, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command6, "VitrificationNo", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "PickUpDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command6, "ConsentForm", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command6, "IsFreezed", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command6, "IsOnlyVitrification", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(command6, "SrcOoctyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command6, "SrcSemenID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command6, "SrcOoctyCode", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "SrcSemenCode", DbType.String, null);
                    this.dbServer.AddInParameter(command6, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command6, "FromForm", DbType.Int64, 1);
                    this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command6, "IsFreezeOocytes", DbType.Boolean, false);
                    this.dbServer.ExecuteNonQuery(command6, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command6, "ResultStatus");
                    nvo.ETDetails.ID = (long) this.dbServer.GetParameterValue(command6, "ID");
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command7, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "VitrivicationID", DbType.Int64, nvo.ETDetails.ID);
                    this.dbServer.AddInParameter(command7, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "EmbNumber", DbType.Int64, nvo.ETDetails.OocyteNumber);
                    this.dbServer.AddInParameter(command7, "EmbSerialNumber", DbType.Int64, nvo.ETDetails.SerialOocyteNumber);
                    this.dbServer.AddInParameter(command7, "LeafNo", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "EmbDays", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "ColorCodeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "CanId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "StrawId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "GobletShapeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "GobletSizeId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "TankId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "ConistorNo", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "ProtocolTypeID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "TransferDate", DbType.DateTime, nvo.ETDetails.PlanDate);
                    this.dbServer.AddInParameter(command7, "TransferDay", DbType.String, nvo.ETDetails.Day);
                    this.dbServer.AddInParameter(command7, "CellStageID", DbType.String, nvo.ETDetails.CellStageID);
                    this.dbServer.AddInParameter(command7, "GradeID", DbType.Int64, nvo.ETDetails.GradeID);
                    this.dbServer.AddInParameter(command7, "EmbStatus", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "Comments", DbType.String, null);
                    this.dbServer.AddInParameter(command7, "UsedOwnOocyte", DbType.Boolean, null);
                    this.dbServer.AddInParameter(command7, "IsThawingDone", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, nvo.ETDetails.OocyteDonorID);
                    this.dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, nvo.ETDetails.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command7, "UsedByOtherCycle", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command7, "UsedTherapyID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "UsedTherapyUnitID", DbType.Int64, null);
                    this.dbServer.AddInParameter(command7, "ReceivingDate", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command7, "TransferDayNo", DbType.Int64, nvo.ETDetails.DayNo);
                    this.dbServer.AddInParameter(command7, "CleavageGrade", DbType.Int64, nvo.ETDetails.GradeID);
                    this.dbServer.AddInParameter(command7, "StageofDevelopmentGrade", DbType.Int64, nvo.ETDetails.StageofDevelopmentGrade);
                    this.dbServer.AddInParameter(command7, "InnerCellMassGrade", DbType.Int64, nvo.ETDetails.InnerCellMassGrade);
                    this.dbServer.AddInParameter(command7, "TrophoectodermGrade", DbType.Int64, nvo.ETDetails.TrophoectodermGrade);
                    this.dbServer.AddInParameter(command7, "CellStage", DbType.String, nvo.ETDetails.CellStage);
                    this.dbServer.AddInParameter(command7, "VitrificationDate", DbType.DateTime, nvo.ETDetails.VitrificationDate);
                    this.dbServer.AddInParameter(command7, "VitrificationTime", DbType.DateTime, nvo.ETDetails.VitrificationTime);
                    this.dbServer.AddInParameter(command7, "VitrificationNo", DbType.String, nvo.ETDetails.VitrificationNo);
                    this.dbServer.AddInParameter(command7, "IsFreezeOocytes", DbType.Boolean, false);
                    this.dbServer.AddInParameter(command7, "IsDonateCryo", DbType.Boolean, nvo.ETDetails.IsDonateCryo);
                    this.dbServer.AddInParameter(command7, "RecepientPatientID", DbType.Int64, nvo.ETDetails.RecepientPatientID);
                    this.dbServer.AddInParameter(command7, "RecepientPatientUnitID", DbType.Int64, nvo.ETDetails.RecepientPatientUnitID);
                    this.dbServer.AddInParameter(command7, "IsDonorCycleDonateCryo", DbType.Boolean, nvo.ETDetails.IsDonorCycleDonateCryo);
                    this.dbServer.AddInParameter(command7, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                    this.dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command7, "IsFreshEmbryoPGDPGS", DbType.Boolean, nvo.ETDetails.IsFreshEmbryoPGDPGS);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
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
            clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.ETDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.ETDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.ETDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.ETDetails.Time);
                this.dbServer.AddInParameter(storedProcCommand, "EmbryologistID", DbType.Int64, nvo.ETDetails.EmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssitantEmbryologistID", DbType.Int64, nvo.ETDetails.AssitantEmbryologistID);
                this.dbServer.AddInParameter(storedProcCommand, "PatternID", DbType.Int64, nvo.ETDetails.PatternID);
                this.dbServer.AddInParameter(storedProcCommand, "UterineArtery_PI", DbType.Boolean, nvo.ETDetails.UterineArtery_PI);
                this.dbServer.AddInParameter(storedProcCommand, "UterineArtery_RI", DbType.Boolean, nvo.ETDetails.UterineArtery_RI);
                this.dbServer.AddInParameter(storedProcCommand, "UterineArtery_SD", DbType.Boolean, nvo.ETDetails.UterineArtery_SD);
                this.dbServer.AddInParameter(storedProcCommand, "Endometerial_PI", DbType.Boolean, nvo.ETDetails.Endometerial_PI);
                this.dbServer.AddInParameter(storedProcCommand, "Endometerial_RI", DbType.Boolean, nvo.ETDetails.Endometerial_RI);
                this.dbServer.AddInParameter(storedProcCommand, "Endometerial_SD", DbType.Boolean, nvo.ETDetails.Endometerial_SD);
                this.dbServer.AddInParameter(storedProcCommand, "CatheterTypeID", DbType.Int64, nvo.ETDetails.CatheterTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "DistanceFundus", DbType.Decimal, nvo.ETDetails.DistanceFundus);
                this.dbServer.AddInParameter(storedProcCommand, "EndometriumThickness", DbType.Decimal, nvo.ETDetails.EndometriumThickness);
                this.dbServer.AddInParameter(storedProcCommand, "TeatmentUnderGA", DbType.Boolean, nvo.ETDetails.TeatmentUnderGA);
                this.dbServer.AddInParameter(storedProcCommand, "Difficulty", DbType.Boolean, nvo.ETDetails.Difficulty);
                this.dbServer.AddInParameter(storedProcCommand, "DifficultyID", DbType.Int64, nvo.ETDetails.DifficultyID);
                this.dbServer.AddInParameter(storedProcCommand, "TenaculumUsed", DbType.Boolean, nvo.ETDetails.TenaculumUsed);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.ETDetails.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsOnlyET", DbType.Boolean, nvo.ETDetails.IsOnlyET);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ETDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "AnethetistID", DbType.Int64, nvo.ETDetails.AnethetistID);
                this.dbServer.AddInParameter(storedProcCommand, "AssistantAnethetistID", DbType.Int64, nvo.ETDetails.AssistantAnethetistID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyID", DbType.Int64, nvo.ETDetails.SrcOoctyID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcSemenID", DbType.Int64, nvo.ETDetails.SrcSemenID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyCode", DbType.String, nvo.ETDetails.SrcOoctyCode);
                this.dbServer.AddInParameter(storedProcCommand, "SrcSemenCode", DbType.String, nvo.ETDetails.SrcSemenCode);
                this.dbServer.AddInParameter(storedProcCommand, "FromForm", DbType.Int64, 2);
                this.dbServer.AddInParameter(storedProcCommand, "IsAnesthesia", DbType.Boolean, nvo.ETDetails.IsAnesthesia);
                this.dbServer.AddInParameter(storedProcCommand, "FreshEmb", DbType.Int64, nvo.ETDetails.FreshEmb);
                this.dbServer.AddInParameter(storedProcCommand, "FrozenEmb", DbType.Int64, nvo.ETDetails.FrozenEmb);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ETDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ETDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.ETDetailsList != null) && (nvo.ETDetailsList.Count > 0))
                {
                    foreach (clsIVFDashboard_EmbryoTransferDetailsVO svo in nvo.ETDetailsList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "ETID", DbType.Int64, nvo.ETDetails.ID);
                        this.dbServer.AddInParameter(command2, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, svo.OocyteNumber);
                        this.dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, svo.SerialOocyteNumber);
                        this.dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, svo.TransferDate);
                        this.dbServer.AddInParameter(command2, "TransferDay", DbType.String, svo.TransferDay);
                        this.dbServer.AddInParameter(command2, "GradeID", DbType.Int64, svo.GradeID);
                        this.dbServer.AddInParameter(command2, "Score", DbType.String, svo.Score);
                        this.dbServer.AddInParameter(command2, "FertStageID", DbType.Int64, svo.FertStageID);
                        this.dbServer.AddInParameter(command2, "EmbStatus", DbType.String, svo.EmbStatus);
                        this.dbServer.AddInParameter(command2, "FileName", DbType.String, svo.FileName);
                        this.dbServer.AddInParameter(command2, "FileContents", DbType.Binary, svo.FileContents);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, svo.Status);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, svo.OocyteDonorID);
                        this.dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, svo.OocyteDonorUnitID);
                        this.dbServer.AddInParameter(command2, "EmbTransferDay", DbType.Int64, svo.EmbTransferDay);
                        this.dbServer.AddInParameter(command2, "Remark", DbType.String, svo.Remark);
                        this.dbServer.AddInParameter(command2, "GradeID_New", DbType.Int64, svo.CleavageGrade);
                        this.dbServer.AddInParameter(command2, "StageofDevelopmentGrade", DbType.Int64, svo.StageofDevelopmentGrade);
                        this.dbServer.AddInParameter(command2, "InnerCellMassGrade", DbType.Int64, svo.InnerCellMassGrade);
                        this.dbServer.AddInParameter(command2, "TrophoectodermGrade", DbType.Int64, svo.TrophoectodermGrade);
                        this.dbServer.AddInParameter(command2, "CellStage", DbType.String, svo.CellStage);
                        this.dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, svo.SurrogateID);
                        this.dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, svo.SurrogateUnitID);
                        this.dbServer.AddInParameter(command2, "SurrogatePatientMrNo", DbType.String, svo.SurrogatePatientMrNo);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.ETDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject AddUpdateFertCheckDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateFertCheckBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateFertCheckBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateFertCheck");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.FertCheckDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.FertCheckDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.FertCheckDetails.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.FertCheckDetails.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.FertCheckDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, nvo.FertCheckDetails.Time);
                this.dbServer.AddInParameter(storedProcCommand, "Isfreezed", DbType.Boolean, nvo.FertCheckDetails.Isfreezed);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "FertCheck", DbType.Int64, nvo.FertCheckDetails.FertCheck);
                this.dbServer.AddInParameter(storedProcCommand, "FertCheckResult", DbType.Int64, nvo.FertCheckDetails.FertCheckResult);
                this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, nvo.FertCheckDetails.Remarks);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FertCheckDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.FertCheckDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.FertCheckDetails.OcyteListList != null)
                {
                    foreach (MasterListItem item in nvo.FertCheckDetails.OcyteListList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateFertCheck");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.FertCheckDetails.PatientID);
                        this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.FertCheckDetails.PatientUnitID);
                        this.dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyID);
                        this.dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, nvo.FertCheckDetails.SerialOocyteNumber + item.FilterID);
                        this.dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, nvo.FertCheckDetails.Date);
                        this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, nvo.FertCheckDetails.Time);
                        this.dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, nvo.FertCheckDetails.Isfreezed);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command2, "FertCheck", DbType.Int64, nvo.FertCheckDetails.FertCheck);
                        this.dbServer.AddInParameter(command2, "FertCheckResult", DbType.Int64, nvo.FertCheckDetails.FertCheckResult);
                        this.dbServer.AddInParameter(command2, "Remarks", DbType.String, nvo.FertCheckDetails.Remarks);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.FertCheckDetails.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        nvo.FertCheckDetails.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.FertCheckDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateGraphicalRepList(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject AddUpdateMediaDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateMediaDetailsBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateMediaDetailsBizActionVO;
            DbTransaction myTransaction = null;
            DbConnection myConnection = null;
            try
            {
                foreach (clsIVFDashboard_MediaDetailsVO svo in nvo.ObserMediaList)
                {
                    myConnection = this.dbServer.CreateConnection();
                    myConnection.Open();
                    myTransaction = myConnection.BeginTransaction();
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateMediaDetails");
                    storedProcCommand.Connection = myConnection;
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.MediaDetails.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.MediaDetails.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.MediaDetails.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.MediaDetails.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ProcedureName", DbType.String, nvo.MediaDetails.ProcedureName);
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, svo.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "MediaName", DbType.String, svo.ItemName);
                    this.dbServer.AddInParameter(storedProcCommand, "LotNo", DbType.String, svo.BatchCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, svo.ExpiryDate);
                    this.dbServer.AddInParameter(storedProcCommand, "PH", DbType.Boolean, svo.PH);
                    this.dbServer.AddInParameter(storedProcCommand, "OSM", DbType.Boolean, svo.OSM);
                    this.dbServer.AddInParameter(storedProcCommand, "VolumeUsed", DbType.Int64, svo.VolumeUsed);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.String, svo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "BatchID", DbType.Int64, svo.BatchID);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, svo.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "Finalized", DbType.Boolean, svo.Finalized);
                    this.dbServer.AddInParameter(storedProcCommand, "StatusIS", DbType.Boolean, svo.StatusIS);
                    this.dbServer.AddInParameter(storedProcCommand, "Company", DbType.String, svo.Company);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.ResultStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.MediaDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if (nvo.ResultStatus == 3)
                    {
                        svo.StockDetails.BatchID = svo.BatchID;
                        svo.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        svo.StockDetails.ItemID = svo.ItemID;
                        svo.StockDetails.TransactionTypeID = InventoryTransactionType.OTDetails;
                        svo.StockDetails.TransactionID = nvo.MediaDetails.ID;
                        svo.StockDetails.TransactionQuantity = svo.VolumeUsed;
                        svo.StockDetails.Date = (DALHelper.HandleDBNull(svo.Date) != null) ? Convert.ToDateTime(svo.Date) : DateTime.Now;
                        svo.StockDetails.Time = (DALHelper.HandleDBNull(svo.Date) != null) ? Convert.ToDateTime(svo.Date) : DateTime.Now;
                        svo.StockDetails.StoreID = new long?(svo.StoreID);
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                            Details = svo.StockDetails
                        };
                        nvo2.Details.ID = 0L;
                        nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, UserVo, myConnection, myTransaction);
                        if (nvo2.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        svo.StockDetails.ID = nvo2.Details.ID;
                    }
                    myTransaction.Commit();
                }
            }
            catch (Exception exception)
            {
                myTransaction.Rollback();
                nvo.SuccessStatus = -1;
                throw exception;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                myConnection = null;
                myTransaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdatePlanDecision(IValueObject valueObject, clsUserVO UserVo)
        {
            return (valueObject as cls_IVFDashboard_AddUpdatePlanDecisionBizActionVO);
        }

        public override IValueObject GetDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay0DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0Date");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ProcedureDate = DALHelper.HandleDate(reader["ProcedureDate"]);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay0Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay0DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OoctyePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.DOSID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DOSID"]));
                        nvo.Details.PICID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PICID"]));
                        nvo.Details.MBD = Convert.ToString(DALHelper.HandleDBNull(reader["MBD"]));
                        nvo.Details.IC = Convert.ToString(DALHelper.HandleDBNull(reader["IC"]));
                        nvo.Details.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.TreatmentStartDate = DALHelper.HandleDate(reader["TreatmentStartDate"]);
                        nvo.Details.TreatmentEndDate = DALHelper.HandleDate(reader["TreatmentEndDate"]);
                        nvo.Details.ObservationDate = DALHelper.HandleDate(reader["ObservationDate"]);
                        nvo.Details.ObservationTime = DALHelper.HandleDate(reader["ObservationTime"]);
                        nvo.Details.OocyteMaturityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteMaturity"]));
                        nvo.Details.OocyteCytoplasmDysmorphisim = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmDysmorphisim"]));
                        nvo.Details.ExtracytoplasmicDysmorphisim = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicDysmorphisim"]));
                        nvo.Details.OocyteCoronaCumulusComplex = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplex"]));
                        nvo.Details.OocyteCoronaCumulusComplex = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplex"]));
                        nvo.Details.ProcedureDate = DALHelper.HandleDate(reader["ProcedureDate"]);
                        nvo.Details.ProcedureTime = DALHelper.HandleDate(reader["ProcedureTime"]);
                        nvo.Details.SourceOfSperm = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceOfSperm"]));
                        nvo.Details.SpermCollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermCollectionMethod"]));
                        nvo.Details.IMSI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IMSI"]));
                        nvo.Details.Embryoscope = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Embryoscope"]));
                        nvo.Details.DiscardReason = Convert.ToString(DALHelper.HandleDBNull(reader["DiscardReason"]));
                        nvo.Details.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        nvo.Details.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        nvo.Details.RecepientPatientName = Convert.ToString(DALHelper.HandleDBNull(reader["RecepientPatientName"]));
                        nvo.Details.RecepientMrNO = Convert.ToString(DALHelper.HandleDBNull(reader["RecepientMrNO"]));
                        nvo.Details.SemenSample = Convert.ToString(DALHelper.HandleDBNull(reader["SemenSample"]));
                        nvo.Details.OocyteZonaID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteZonaID"]));
                        nvo.Details.OocyteZona = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteZona"]));
                        nvo.Details.PVSID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PVSID"]));
                        nvo.Details.PVS = Convert.ToString(DALHelper.HandleDBNull(reader["PVS"]));
                        nvo.Details.IstPBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IstPBID"]));
                        nvo.Details.IstPB = Convert.ToString(DALHelper.HandleDBNull(reader["IstPB"]));
                        nvo.Details.CytoplasmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CytoplasmID"]));
                        nvo.Details.Cytoplasm = Convert.ToString(DALHelper.HandleDBNull(reader["Cytoplasm"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay0OocList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0OocyteListBizActionVO nvo = valueObject as clsIVFDashboard_GetDay0OocyteListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0OocList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Day0OocList == null)
                    {
                        nvo.Day0OocList = new List<clsIVFDashboard_LabDaysVO>();
                    }
                    while (reader.Read())
                    {
                        clsIVFDashboard_LabDaysVO item = new clsIVFDashboard_LabDaysVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["FemalePatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["FemalePatientUnitID"]),
                            PlanTherapyID = (long) DALHelper.HandleDBNull(reader["PlanTherapyID"]),
                            PlanTherapyUnitID = (long) DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]),
                            OocyteNumber = (long) DALHelper.HandleDBNull(reader["OocyteNumber"]),
                            SerialOocyteNumber = (long) DALHelper.HandleDBNull(reader["SerialOocuteNumber"])
                        };
                        nvo.Day0OocList.Add(item);
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

        public override IValueObject GetDay0OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay0OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay0OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay0OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"])),
                                NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay0OocyteDetailsOocyteRecipient(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay0OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0DetailsForOocyteRecipient");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay0OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay0OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay1Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay1DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay1DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay1Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay1OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay1OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay1OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheckDetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay1OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay1OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay1OocyteObservations(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay1OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay1OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay1DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay1OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay1OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay2Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay2DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay2DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay2Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        nvo.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        nvo.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay2OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay2OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay2OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay2DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay2OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay2OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay3Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay3DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay3DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay3Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        nvo.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        nvo.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        nvo.Details.IsEmbryoCompacted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmbryoCompacted"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay3OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay3OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay3OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay3DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay3OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay3OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay4Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay4DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay4DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay4Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        nvo.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        nvo.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        nvo.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay4OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay4OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay4OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay4DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay4OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay4OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay5Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay5DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay5DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay5Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        nvo.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        nvo.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        nvo.Details.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        nvo.Details.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        nvo.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay5OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay5OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay5OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay5DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay5OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay5OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay6Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay6DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay6DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay6Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        nvo.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        nvo.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        nvo.Details.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        nvo.Details.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        nvo.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay6OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay6OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay6OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay6DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay6OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay6OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay7Details(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay7DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay7DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay7Details");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        nvo.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        nvo.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.Details.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.Details.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        nvo.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        nvo.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        nvo.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        nvo.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        nvo.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        nvo.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        nvo.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        nvo.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        nvo.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        nvo.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        nvo.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        nvo.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        nvo.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        nvo.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        nvo.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        nvo.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        nvo.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));
                        nvo.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        nvo.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                        nvo.Details.CellObservationTime = DALHelper.HandleDate(reader["CellObservationTime"]);
                        nvo.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        nvo.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        nvo.Details.BiopsyDate = DALHelper.HandleDate(reader["BiopsyDate"]);
                        nvo.Details.BiopsyTime = DALHelper.HandleDate(reader["BiopsyTime"]);
                        nvo.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        nvo.Details.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        nvo.Details.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        nvo.Details.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        nvo.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        nvo.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        if (!nvo.Details.IsBiopsy)
                        {
                            nvo.Details.BiopsyDate = null;
                            nvo.Details.BiopsyTime = null;
                        }
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.Details.ImgList.Add(item);
                    }
                }
                reader.NextResult();
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
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]))
                        };
                        DateTime? nullable3 = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable3.Value;
                        item.Title = Convert.ToString(DALHelper.HandleDBNull(reader["Title"]));
                        item.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        item.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        item.AttachedFileContent = (byte[]) DALHelper.HandleDBNull(reader["AttachedFileContent"]);
                        item.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));
                        nvo.Details.DetailList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetDay7OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay7OocyteDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay7OocyteDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay7DetailsForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay7OocyteDetailsBizActionVO nvo2 = new clsIVFDashboard_GetDay7OocyteDetailsBizActionVO {
                            Details = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.Details);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetETDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetEmbryoTansferBizActionVO nvo = valueObject as clsIVFDashboard_GetEmbryoTansferBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_GetETDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ETDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ETDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.ETDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.ETDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOnlyET", DbType.Boolean, nvo.ETDetails.IsOnlyET);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.ETDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ETDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ETDetails.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.ETDetails.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.ETDetails.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssisEmbryologistID"]));
                        nvo.ETDetails.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.ETDetails.EndometriumThickness = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EndometriumThickness"]));
                        nvo.ETDetails.PatternID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatternID"]));
                        nvo.ETDetails.Endometerial_PI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometerial_PI"]));
                        nvo.ETDetails.Endometerial_RI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometerial_RI"]));
                        nvo.ETDetails.Endometerial_SD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometerial_SD"]));
                        nvo.ETDetails.UterineArtery_PI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineArtery_PI"]));
                        nvo.ETDetails.UterineArtery_RI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineArtery_RI"]));
                        nvo.ETDetails.UterineArtery_SD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineArtery_SD"]));
                        nvo.ETDetails.DistanceFundus = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DistanceFundus"]));
                        nvo.ETDetails.CatheterTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CatheterTypeID"]));
                        nvo.ETDetails.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.ETDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.ETDetails.TenaculumUsed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TenaculumUsed"]));
                        nvo.ETDetails.TeatmentUnderGA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TeatmentUnderGA"]));
                        nvo.ETDetails.Difficulty = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Difficulty"]));
                        nvo.ETDetails.DifficultyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyID"]));
                        nvo.ETDetails.AnethetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnethetistID"]));
                        nvo.ETDetails.AssistantAnethetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnethetistID"]));
                        nvo.ETDetails.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                        nvo.ETDetails.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                        nvo.ETDetails.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        nvo.ETDetails.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                        nvo.ETDetails.IsAnesthesia = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAnesthesia"]));
                        nvo.ETDetails.FreshEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreshEmb"]));
                        nvo.ETDetails.FrozenEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrozenEmb"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_EmbryoTransferDetailsVO item = new clsIVFDashboard_EmbryoTransferDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ET_ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ET_ID"])),
                            ET_UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ET_UnitID"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                            FertStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertStageID"])),
                            GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                            FertStage = Convert.ToString(DALHelper.HandleDBNull(reader["FertStage"])),
                            Score = Convert.ToString(DALHelper.HandleDBNull(reader["Score"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            FileContents = (byte[]) DALHelper.HandleDBNull(reader["FileContents"]),
                            OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"])),
                            EmbTransferDay = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbTransferDay"])),
                            ServerTransferDate = DALHelper.HandleDate(reader["ServerTransferDate"]),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID_New"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            StageofDevelopmentGradeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDesc"])),
                            InnerCellMassGradeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeDesc"])),
                            TrophoectodermGradeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeDesc"])),
                            StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"])),
                            InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"])),
                            TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"])),
                            SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"])),
                            SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"])),
                            SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"])),
                            IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"])),
                            IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]))
                        };
                        nvo.ETDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.ETDetails.FreshEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreshEmb"]));
                        nvo.ETDetails.FrozenEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrozenEmb"]));
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

        public override IValueObject GetFertCheckDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetFertCheckBizActionVO nvo = valueObject as clsIVFDashboard_GetFertCheckBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheckDate");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.FertCheckDetails.Date = DALHelper.HandleDate(reader["Date"]);
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

        public override IValueObject GetFertCheckDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetFertCheckBizActionVO nvo = valueObject as clsIVFDashboard_GetFertCheckBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheck");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.FertCheckDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.FertCheckDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.FertCheckDetails.OocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.FertCheckDetails.Date = DALHelper.HandleDate(reader["Date"]);
                        nvo.FertCheckDetails.Time = DALHelper.HandleDate(reader["Time"]);
                        nvo.FertCheckDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        nvo.FertCheckDetails.FertCheck = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertCheck"]));
                        nvo.FertCheckDetails.FertCheckResult = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertCheckResult"]));
                        nvo.FertCheckDetails.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
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

        public override IValueObject GetGraphicalRepOocList(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboar_GetGraphicalRepBizActionVO nvo = valueObject as cls_IVFDashboar_GetGraphicalRepBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetGraphicalRepOocList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.Details.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.Details.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GraphicalOocList == null)
                    {
                        nvo.GraphicalOocList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();
                    }
                    while (reader.Read())
                    {
                        cls_IVFDashboard_GraphicalRepresentationVO item = new cls_IVFDashboard_GraphicalRepresentationVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            PatientUnitID = (long) DALHelper.HandleDBNull(reader["PatientUnitID"]),
                            PlanTherapyID = (long) DALHelper.HandleDBNull(reader["PlanTherapyID"]),
                            PlanTherapyUnitID = (long) DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]),
                            OocyteNumber = (long) DALHelper.HandleDBNull(reader["OocyteNumber"]),
                            SerialOocyteNumber = (long) DALHelper.HandleDBNull(reader["SerialOocyteNumber"]),
                            Day0 = (bool?) DALHelper.HandleDBNull(reader["Day0"]),
                            Day1 = (bool?) DALHelper.HandleDBNull(reader["Day1"]),
                            Day2 = (bool?) DALHelper.HandleDBNull(reader["Day2"]),
                            Day3 = (bool?) DALHelper.HandleDBNull(reader["Day3"]),
                            Day4 = (bool?) DALHelper.HandleDBNull(reader["Day4"]),
                            Day5 = (bool?) DALHelper.HandleDBNull(reader["Day5"]),
                            Day6 = (bool?) DALHelper.HandleDBNull(reader["Day6"]),
                            Day7 = (bool?) DALHelper.HandleDBNull(reader["Day7"]),
                            Day0CellStage = DALHelper.HandleIntegerNull(reader["Day0CellStage"]),
                            Day1CellStage = DALHelper.HandleIntegerNull(reader["Day1CellStage"]),
                            Day2CellStage = DALHelper.HandleIntegerNull(reader["Day2CellStage"]),
                            Day3CellStage = DALHelper.HandleIntegerNull(reader["Day3CellStage"]),
                            Day4CellStage = DALHelper.HandleIntegerNull(reader["Day4CellStage"]),
                            Day5CellStage = DALHelper.HandleIntegerNull(reader["Day5CellStage"]),
                            Day6CellStage = DALHelper.HandleIntegerNull(reader["Day6CellStage"]),
                            Day7CellStage = DALHelper.HandleIntegerNull(reader["Day7CellStage"]),
                            ImpressionDay0 = (string) DALHelper.HandleDBNull(reader["ImpressionDay0"]),
                            ImpressionDay1 = (string) DALHelper.HandleDBNull(reader["ImpressionDay1"]),
                            ImpressionDay2 = (string) DALHelper.HandleDBNull(reader["ImpressionDay2"]),
                            ImpressionDay3 = (string) DALHelper.HandleDBNull(reader["ImpressionDay3"]),
                            ImpressionDay4 = (string) DALHelper.HandleDBNull(reader["ImpressionDay4"]),
                            ImpressionDay5 = (string) DALHelper.HandleDBNull(reader["ImpressionDay5"]),
                            ImpressionDay6 = (string) DALHelper.HandleDBNull(reader["ImpressionDay6"]),
                            ImpressionDay7 = (string) DALHelper.HandleDBNull(reader["ImpressionDay7"]),
                            Plan0 = (string) DALHelper.HandleDBNull(reader["Plan0"]),
                            Plan1 = (string) DALHelper.HandleDBNull(reader["Plan1"]),
                            Plan2 = (string) DALHelper.HandleDBNull(reader["Plan2"]),
                            Plan3 = (string) DALHelper.HandleDBNull(reader["Plan3"]),
                            Plan4 = (string) DALHelper.HandleDBNull(reader["Plan4"]),
                            Plan5 = (string) DALHelper.HandleDBNull(reader["Plan5"]),
                            Plan6 = (string) DALHelper.HandleDBNull(reader["Plan6"]),
                            Plan7 = (string) DALHelper.HandleDBNull(reader["Plan7"]),
                            CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"])),
                            FertCheck = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertCheck"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"])),
                            IsLabDay0Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay0Freezed"])),
                            ProcedureDate = DALHelper.HandleDate(reader["ProcedureDate"]),
                            ProcedureTime = DALHelper.HandleDate(reader["ProcedureTime"]),
                            IsLabDay1Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay1Freezed"])),
                            IsLabDay2Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay2Freezed"])),
                            IsLabDay3Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay3Freezed"])),
                            IsLabDay4Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay4Freezed"])),
                            IsLabDay5Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay5Freezed"])),
                            IsLabDay6Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay6Freezed"])),
                            IsLabDay7Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay7Freezed"])),
                            GradeIDDay1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay1"])),
                            GradeIDDay2 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay2"])),
                            GradeIDDay3 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay3"])),
                            GradeIDDay4 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay4"])),
                            StageofDevelopmentGradeDay5 = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDay5"])),
                            InnerCellMassGradeDay5 = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeDay5"])),
                            TrophoectodermGradeDay5 = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeDay5"])),
                            StageofDevelopmentGradeDay6 = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDay6"])),
                            InnerCellMassGradeDay6 = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeDay6"])),
                            TrophoectodermGradeDay6 = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeDay6"])),
                            StageofDevelopmentGradeDay7 = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDay7"])),
                            InnerCellMassGradeDay7 = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeDay7"])),
                            TrophoectodermGradeDay7 = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeDay7"])),
                            CellStageDay1 = (string) DALHelper.HandleDBNull(reader["CellStageDay1"]),
                            CellStageDay2 = (string) DALHelper.HandleDBNull(reader["CellStageDay2"]),
                            CellStageDay3 = (string) DALHelper.HandleDBNull(reader["CellStageDay3"]),
                            CellStageDay4 = (string) DALHelper.HandleDBNull(reader["CellStageDay4"]),
                            CellStageDay5 = (string) DALHelper.HandleDBNull(reader["CellStageDay5"]),
                            CellStageDay6 = (string) DALHelper.HandleDBNull(reader["CellStageDay6"]),
                            CellStageDay7 = (string) DALHelper.HandleDBNull(reader["CellStageDay7"]),
                            IsExtendedCulture = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExtendedCulture"])),
                            IsExtendedCultureFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExtendedCultureFromOtherCycle"])),
                            ObservationDate1 = DALHelper.HandleDate(reader["CellObservationDate1"]),
                            ObservationTime1 = DALHelper.HandleDate(reader["CellObservationTime1"]),
                            ObservationDate2 = DALHelper.HandleDate(reader["CellObservationDate2"]),
                            ObservationTime2 = DALHelper.HandleDate(reader["CellObservationTime2"]),
                            ObservationDate3 = DALHelper.HandleDate(reader["CellObservationDate3"]),
                            ObservationTime3 = DALHelper.HandleDate(reader["CellObservationTime3"]),
                            ObservationDate4 = DALHelper.HandleDate(reader["CellObservationDate4"]),
                            ObservationTime4 = DALHelper.HandleDate(reader["CellObservationTime4"]),
                            ObservationDate5 = DALHelper.HandleDate(reader["CellObservationDate5"]),
                            ObservationTime5 = DALHelper.HandleDate(reader["CellObservationTime5"]),
                            ObservationDate6 = DALHelper.HandleDate(reader["CellObservationDate6"]),
                            ObservationTime6 = DALHelper.HandleDate(reader["CellObservationTime6"]),
                            ObservationDate7 = DALHelper.HandleDate(reader["CellObservationDate7"]),
                            ObservationTime7 = DALHelper.HandleDate(reader["CellObservationTime7"]),
                            PatientName = (string) DALHelper.HandleDBNull(reader["RecepientPatientName"]),
                            MRNO = (string) DALHelper.HandleDBNull(reader["RecepientMRNO"]),
                            BlDay3PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day3PGDPGS"])),
                            BlFrozenDay3PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day3PGDPGSFrozen"]))
                        };
                        if (item.BlDay3PGDPGS || item.BlFrozenDay3PGDPGS)
                        {
                            item.Day3PGDPGS = "PGD/PGS";
                        }
                        item.BlDay5PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day5PGDPGS"]));
                        item.BlFrozenDay5PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day5PGDPGSFrozen"]));
                        if (item.BlDay5PGDPGS || item.BlFrozenDay5PGDPGS)
                        {
                            item.Day5PGDPGS = "PGD/PGS";
                        }
                        item.BlDay6PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day6PGDPGS"]));
                        item.BlFrozenDay6PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day6PGDPGSFrozen"]));
                        if (item.BlDay6PGDPGS || item.BlFrozenDay6PGDPGS)
                        {
                            item.Day6PGDPGS = "PGD/PGS";
                        }
                        item.BlDay7PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day7PGDPGS"]));
                        item.BlFrozenDay7PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day7PGDPGSFrozen"]));
                        if (item.BlDay7PGDPGS || item.BlFrozenDay7PGDPGS)
                        {
                            item.Day7PGDPGS = "PGD/PGS";
                        }
                        item.IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]));
                        if ((item.Plan0 != null) && (item.IsLabDay0Freezed && (item.Plan0.Equals("IVF") || item.Plan0.Equals("ICSI"))))
                        {
                            item.IsFertCheck = true;
                        }
                        bool? nullable = item.Day1;
                        if (nullable.GetValueOrDefault() && (nullable != null))
                        {
                            item.Day1Visible = true;
                        }
                        bool? nullable2 = item.Day2;
                        if (nullable2.GetValueOrDefault() && (nullable2 != null))
                        {
                            item.Day2Visible = true;
                        }
                        bool? nullable3 = item.Day3;
                        if (nullable3.GetValueOrDefault() && (nullable3 != null))
                        {
                            item.Day3Visible = true;
                        }
                        bool? nullable4 = item.Day4;
                        if (nullable4.GetValueOrDefault() && (nullable4 != null))
                        {
                            item.Day4Visible = true;
                        }
                        bool? nullable5 = item.Day5;
                        if (nullable5.GetValueOrDefault() && (nullable5 != null))
                        {
                            item.Day5Visible = true;
                        }
                        bool? nullable6 = item.Day6;
                        if (nullable6.GetValueOrDefault() && (nullable6 != null))
                        {
                            item.Day6Visible = true;
                        }
                        bool? nullable7 = item.Day6;
                        if (nullable7.GetValueOrDefault() && (nullable7 != null))
                        {
                            item.Day6Visible = true;
                        }
                        bool? nullable8 = item.Day7;
                        if (nullable8.GetValueOrDefault() && (nullable8 != null))
                        {
                            item.Day7Visible = true;
                        }
                        item.DecisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DecisionID"]));
                        List<ClsAddObervationEmbryo> list = new List<ClsAddObervationEmbryo>();
                        bool? nullable9 = item.Day1;
                        if ((nullable9.GetValueOrDefault() && (nullable9 != null)) && !item.IsLabDay1Freezed)
                        {
                            ClsAddObervationEmbryo embryo = new ClsAddObervationEmbryo {
                                Day = item.Day1,
                                StrDay = "Day1",
                                IsFreezed = new bool?(item.IsLabDay1Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate1"])
                            };
                            list.Add(embryo);
                        }
                        bool? nullable10 = item.Day2;
                        if ((nullable10.GetValueOrDefault() && (nullable10 != null)) && !item.IsLabDay2Freezed)
                        {
                            ClsAddObervationEmbryo embryo2 = new ClsAddObervationEmbryo {
                                Day = item.Day2,
                                StrDay = "Day2",
                                IsFreezed = new bool?(item.IsLabDay2Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate2"])
                            };
                            list.Add(embryo2);
                        }
                        bool? nullable11 = item.Day3;
                        if ((nullable11.GetValueOrDefault() && (nullable11 != null)) && !item.IsLabDay3Freezed)
                        {
                            ClsAddObervationEmbryo embryo3 = new ClsAddObervationEmbryo {
                                Day = item.Day3,
                                StrDay = "Day3",
                                IsFreezed = new bool?(item.IsLabDay3Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate3"])
                            };
                            list.Add(embryo3);
                        }
                        bool? nullable12 = item.Day4;
                        if ((nullable12.GetValueOrDefault() && (nullable12 != null)) && !item.IsLabDay4Freezed)
                        {
                            ClsAddObervationEmbryo embryo4 = new ClsAddObervationEmbryo {
                                Day = item.Day4,
                                StrDay = "Day4",
                                IsFreezed = new bool?(item.IsLabDay4Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate4"])
                            };
                            list.Add(embryo4);
                        }
                        bool? nullable13 = item.Day5;
                        if ((nullable13.GetValueOrDefault() && (nullable13 != null)) && !item.IsLabDay5Freezed)
                        {
                            ClsAddObervationEmbryo embryo5 = new ClsAddObervationEmbryo {
                                Day = item.Day5,
                                StrDay = "Day5",
                                IsFreezed = new bool?(item.IsLabDay5Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate5"])
                            };
                            list.Add(embryo5);
                        }
                        bool? nullable14 = item.Day6;
                        if ((nullable14.GetValueOrDefault() && (nullable14 != null)) && !item.IsLabDay6Freezed)
                        {
                            ClsAddObervationEmbryo embryo6 = new ClsAddObervationEmbryo {
                                Day = item.Day6,
                                StrDay = "Day6",
                                IsFreezed = new bool?(item.IsLabDay6Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate6"])
                            };
                            list.Add(embryo6);
                        }
                        bool? nullable15 = item.Day7;
                        if ((nullable15.GetValueOrDefault() && (nullable15 != null)) && !item.IsLabDay7Freezed)
                        {
                            ClsAddObervationEmbryo embryo7 = new ClsAddObervationEmbryo {
                                Day = item.Day7,
                                StrDay = "Day7",
                                IsFreezed = new bool?(item.IsLabDay7Freezed),
                                ServerDate = DALHelper.HandleDate(reader["ServerObservationDate7"])
                            };
                            list.Add(embryo7);
                        }
                        item.EmbryoDayObservation = list;
                        nvo.GraphicalOocList.Add(item);
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

        public override IValueObject GetIVFICSIPlannedOocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetFertCheckBizActionVO nvo = valueObject as clsIVFDashboard_GetFertCheckBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0IVFICSIPlanForOocyte");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.FertCheckDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.FertCheckDetails.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetFertCheckBizActionVO nvo2 = new clsIVFDashboard_GetFertCheckBizActionVO {
                            FertCheckDetails = { 
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"])),
                                Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]))
                            }
                        };
                        nvo.Oocytelist.Add(nvo2.FertCheckDetails);
                    }
                }
                reader.NextResult();
                List<clsIVFDashboard_FertCheck> Oocytelist = new List<clsIVFDashboard_FertCheck>();
                while (true)
                {
                    if (!reader.Read())
                    {
                        nvo.Oocytelist = (from a in nvo.Oocytelist
                            where !Oocytelist.Any<clsIVFDashboard_FertCheck>(b => (b.OocyteNumber == a.OocyteNumber))
                            select a).ToList<clsIVFDashboard_FertCheck>();
                        break;
                    }
                    clsIVFDashboard_GetFertCheckBizActionVO nvo3 = new clsIVFDashboard_GetFertCheckBizActionVO {
                        FertCheckDetails = { OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"])) }
                    };
                    Oocytelist.Add(nvo3.FertCheckDetails);
                }
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
            return nvo;
        }

        public override IValueObject GetMediaDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetMediaDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetMediaDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetMediaDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ProcedureName", DbType.String, nvo.ProcedureName);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_MediaDetailsVO item = new clsIVFDashboard_MediaDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["LotNo"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"])),
                            Date = DALHelper.HandleDate(reader["Date"]),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["MediaName"])),
                            OSM = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OSM"])),
                            PH = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PH"])),
                            Status = Convert.ToString(DALHelper.HandleDBNull(reader["Status"])),
                            StatusIS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StatusIS"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            VolumeUsed = Convert.ToInt64(DALHelper.HandleDBNull(reader["VolumeUsed"])),
                            Finalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Finalized"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            AvailableQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableQuantity"]))
                        };
                        nvo.MediaDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                connection.Close();
                nvo.SuccessStatus = -1;
                throw;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetObservationDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay1DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay1DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetObservationDate");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.Details.CellObservationDate = DALHelper.HandleDate(reader["CellObservationDate"]);
                    }
                }
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
            return nvo;
        }

        public override IValueObject GetSemenSampleList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0DetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetDay0DetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSemenSampleListForDay0");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.Details.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]))
                        };
                        nvo.SemenSampleList.Add(item);
                    }
                }
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
            return nvo;
        }

        public override IValueObject UpdateAndGetImageListDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO nvo = valueObject as clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_UpdateLabDaysImagesListStatus");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.ImageObj.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.ImageObj.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.ImageObj.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.ImageObj.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.ImageObj.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumber", DbType.Int64, nvo.ImageObj.SerialOocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Day", DbType.Int64, nvo.ImageObj.Day);
                this.dbServer.AddInParameter(storedProcCommand, "OriginalImagePath", DbType.String, nvo.ImageObj.OriginalImagePath);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.ImageList = new List<clsAddImageVO>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocyteNumber"])),
                            OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.ImageList.Add(item);
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
    }
}

