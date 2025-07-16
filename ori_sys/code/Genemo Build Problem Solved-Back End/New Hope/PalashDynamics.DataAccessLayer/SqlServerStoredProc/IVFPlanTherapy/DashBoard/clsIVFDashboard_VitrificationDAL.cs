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

    internal class clsIVFDashboard_VitrificationDAL : clsBaseIVFDashboard_VitrificationDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsIVFDashboard_VitrificationDAL()
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

        public override IValueObject AddUpdateDonateDiscardEmbryoForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank bank = valueObject as cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if ((bank.Vitrification.VitrificationDetailsList != null) && (bank.Vitrification.VitrificationDetailsList.Count > 0))
                {
                    foreach (clsIVFDashBoard_VitrificationDetailsVO svo in bank.Vitrification.VitrificationDetailsList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDonateDiscardEmbryoForCryoBank");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrivicationID", DbType.Int64, svo.VitrivicationID);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrificationUnitID", DbType.Int64, svo.VitrificationUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbSerialNumber", DbType.Int64, svo.EmbSerialNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbNumber", DbType.Int64, svo.EmbNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, svo.PlanId);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorPatientID", DbType.Int64, svo.DonorPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorPatientUnitID", DbType.Int64, svo.DonorPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientID", DbType.Int64, svo.RecepientPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientUnitID", DbType.Int64, svo.RecepientPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                bank.SuccessStatus = -1;
                bank.Vitrification = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return bank;
        }

        public override IValueObject AddUpdateDonateDiscardOocyteForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank bank = valueObject as cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if ((bank.Vitrification.VitrificationDetailsList != null) && (bank.Vitrification.VitrificationDetailsList.Count > 0))
                {
                    foreach (clsIVFDashBoard_VitrificationDetailsVO svo in bank.Vitrification.VitrificationDetailsList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank");
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrivicationID", DbType.Int64, svo.VitrivicationID);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrificationUnitID", DbType.Int64, svo.VitrificationUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbSerialNumber", DbType.Int64, svo.EmbSerialNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "EmbNumber", DbType.Int64, svo.EmbNumber);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanID", DbType.Int64, svo.PlanId);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorPatientID", DbType.Int64, svo.DonorPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorPatientUnitID", DbType.Int64, svo.DonorPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientID", DbType.Int64, svo.RecepientPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "RecepientPatientUnitID", DbType.Int64, svo.RecepientPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                bank.SuccessStatus = -1;
                bank.Vitrification = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return bank;
        }

        public override IValueObject AddUpdateIVFDashBoard_RenewalDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateVitrificationBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDashBoard_AddUpdateRenewalDate");
                this.dbServer.AddInParameter(storedProcCommand, "VitificationID", DbType.Int64, nvo.VitificationID);
                this.dbServer.AddInParameter(storedProcCommand, "VitificationUnitID", DbType.Int64, nvo.VitificationUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VitificationDetailsID", DbType.Int64, nvo.VitificationDetailsID);
                this.dbServer.AddInParameter(storedProcCommand, "VitificationDetailsUnitID", DbType.Int64, nvo.VitificationDetailsUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SpremFreezingID", DbType.Int64, nvo.SpremFreezingID);
                this.dbServer.AddInParameter(storedProcCommand, "SpremFreezingUnitID", DbType.Int64, nvo.SpremFreezingUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsOocyteFreezed", DbType.Boolean, nvo.IsOocyteFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsSprem", DbType.Boolean, nvo.IsSprem);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.StartDate);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.StartTime);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, nvo.ExpiryDate);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryTime", DbType.DateTime, nvo.ExpiryTime);
                this.dbServer.AddInParameter(storedProcCommand, "LongTerm", DbType.Boolean, nvo.LongTerm);
                this.dbServer.AddInParameter(storedProcCommand, "ShortTerm", DbType.Boolean, nvo.ShortTerm);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.VitrificationMain = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateVitrificationBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if (nvo.VitrificationMain != null)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.VitrificationMain.DateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "VitrificationNo", DbType.String, nvo.VitrificationMain.VitrificationNo);
                    this.dbServer.AddInParameter(storedProcCommand, "PickUpDate", DbType.DateTime, nvo.VitrificationMain.PickUpDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ConsentForm", DbType.Boolean, nvo.VitrificationMain.ConsentForm);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.VitrificationMain.IsFreezed);
                    this.dbServer.AddInParameter(storedProcCommand, "IsOnlyVitrification", DbType.Boolean, nvo.VitrificationMain.IsOnlyVitrification);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyID", DbType.Int64, nvo.VitrificationMain.SrcOoctyID);
                    this.dbServer.AddInParameter(storedProcCommand, "SrcSemenID", DbType.Int64, nvo.VitrificationMain.SrcSemenID);
                    this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyCode", DbType.String, nvo.VitrificationMain.SrcOoctyCode);
                    this.dbServer.AddInParameter(storedProcCommand, "SrcSemenCode", DbType.String, nvo.VitrificationMain.SrcSemenCode);
                    this.dbServer.AddInParameter(storedProcCommand, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                    this.dbServer.AddInParameter(storedProcCommand, "IsRefreeze", DbType.Boolean, nvo.VitrificationMain.IsRefeeze);
                    this.dbServer.AddInParameter(storedProcCommand, "FromForm", DbType.Int64, 2);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.VitrificationMain.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFreezeOocytes", DbType.Boolean, nvo.IsFreezeOocytes);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, nvo.VitrificationMain.ExpiryDate);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.VitrificationMain.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if ((nvo.VitrificationDetailsList != null) && (nvo.VitrificationDetailsList.Count > 0))
                    {
                        foreach (clsIVFDashBoard_VitrificationDetailsVO svo in nvo.VitrificationDetailsList)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                            this.dbServer.AddInParameter(command, "ID", DbType.Int64, svo.ID);
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "VitrivicationID", DbType.Int64, nvo.VitrificationMain.ID);
                            this.dbServer.AddInParameter(command, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "EmbNumber", DbType.Int64, svo.EmbNumber);
                            this.dbServer.AddInParameter(command, "EmbSerialNumber", DbType.Int64, svo.EmbSerialNumber);
                            this.dbServer.AddInParameter(command, "LeafNo", DbType.String, svo.LeafNo);
                            this.dbServer.AddInParameter(command, "EmbDays", DbType.String, svo.EmbDays);
                            this.dbServer.AddInParameter(command, "ColorCodeID", DbType.Int64, svo.ColorCodeID);
                            this.dbServer.AddInParameter(command, "CanId", DbType.Int64, svo.CanId);
                            this.dbServer.AddInParameter(command, "StrawId", DbType.Int64, svo.StrawId);
                            this.dbServer.AddInParameter(command, "GobletShapeId", DbType.Int64, svo.GobletShapeId);
                            this.dbServer.AddInParameter(command, "GobletSizeId", DbType.Int64, svo.GobletSizeId);
                            this.dbServer.AddInParameter(command, "TankId", DbType.Int64, svo.TankId);
                            this.dbServer.AddInParameter(command, "ConistorNo", DbType.Int64, svo.ConistorNo);
                            this.dbServer.AddInParameter(command, "ProtocolTypeID", DbType.Int64, svo.ProtocolTypeID);
                            this.dbServer.AddInParameter(command, "TransferDate", DbType.DateTime, svo.TransferDate);
                            this.dbServer.AddInParameter(command, "TransferDay", DbType.String, svo.TransferDay);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.String, svo.CellStageID);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, svo.GradeID);
                            this.dbServer.AddInParameter(command, "EmbStatus", DbType.String, svo.EmbStatus);
                            this.dbServer.AddInParameter(command, "Comments", DbType.String, svo.Comments);
                            this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                            if (nvo.VitrificationMain.IsFreezed && !nvo.VitrificationMain.IsOnlyVitrification)
                            {
                                this.dbServer.AddInParameter(command, "IsThawingDone", DbType.Boolean, true);
                            }
                            else
                            {
                                this.dbServer.AddInParameter(command, "IsThawingDone", DbType.Boolean, false);
                            }
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, svo.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, svo.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command, "UsedByOtherCycle", DbType.Boolean, nvo.VitrificationMain.UsedByOtherCycle);
                            this.dbServer.AddInParameter(command, "UsedTherapyID", DbType.Int64, nvo.VitrificationMain.UsedTherapyID);
                            this.dbServer.AddInParameter(command, "UsedTherapyUnitID", DbType.Int64, nvo.VitrificationMain.UsedTherapyUnitID);
                            this.dbServer.AddInParameter(command, "ReceivingDate", DbType.DateTime, nvo.VitrificationMain.DateTime);
                            this.dbServer.AddInParameter(command, "TransferDayNo", DbType.Int64, svo.TransferDayNo);
                            this.dbServer.AddInParameter(command, "CleavageGrade", DbType.Int64, svo.CleavageGrade);
                            this.dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, svo.StageofDevelopmentGrade);
                            this.dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, svo.InnerCellMassGrade);
                            this.dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, svo.TrophoectodermGrade);
                            this.dbServer.AddInParameter(command, "CellStage", DbType.String, svo.CellStage);
                            this.dbServer.AddInParameter(command, "VitrificationDate", DbType.DateTime, svo.VitrificationDate);
                            this.dbServer.AddInParameter(command, "VitrificationTime", DbType.DateTime, svo.VitrificationTime);
                            this.dbServer.AddInParameter(command, "VitrificationNo", DbType.String, svo.VitrificationNo);
                            this.dbServer.AddInParameter(command, "IsSaved", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command, "IsFreezeOocytes", DbType.Boolean, svo.IsFreezeOocytes);
                            this.dbServer.AddInParameter(command, "CryoCode", DbType.String, svo.CryoCode);
                            this.dbServer.AddInParameter(command, "IsDonateCryo", DbType.Boolean, svo.IsDonateCryo);
                            this.dbServer.AddInParameter(command, "RecepientPatientID", DbType.Int64, svo.RecepientPatientID);
                            this.dbServer.AddInParameter(command, "RecepientPatientUnitID", DbType.Int64, svo.RecepientPatientUnitID);
                            this.dbServer.AddInParameter(command, "IsDonorCycleDonateCryo", DbType.Boolean, svo.IsDonorCycleDonateCryo);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                            if (svo.IsDonateCryo || svo.IsDonorCycleDonateCryo)
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                                this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, svo.RecepientPatientID);
                                this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, svo.RecepientPatientUnitID);
                                this.dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                                this.dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                                this.dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, nvo.VitrificationMain.DateTime);
                                this.dbServer.AddInParameter(command3, "VitrificationNo", DbType.String, nvo.VitrificationMain.VitrificationNo);
                                this.dbServer.AddInParameter(command3, "PickUpDate", DbType.DateTime, nvo.VitrificationMain.PickUpDate);
                                this.dbServer.AddInParameter(command3, "ConsentForm", DbType.Boolean, nvo.VitrificationMain.ConsentForm);
                                this.dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, nvo.VitrificationMain.IsFreezed);
                                this.dbServer.AddInParameter(command3, "IsOnlyVitrification", DbType.Boolean, nvo.VitrificationMain.IsOnlyVitrification);
                                this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, nvo.VitrificationMain.SrcOoctyID);
                                this.dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, nvo.VitrificationMain.SrcSemenID);
                                this.dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, nvo.VitrificationMain.SrcOoctyCode);
                                this.dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, nvo.VitrificationMain.SrcSemenCode);
                                this.dbServer.AddInParameter(command3, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                                this.dbServer.AddInParameter(command3, "IsRefreeze", DbType.Boolean, nvo.VitrificationMain.IsRefeeze);
                                this.dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 2);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.VitrificationMain.DonateCryoID);
                                this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.AddInParameter(command3, "IsFreezeOocytes", DbType.Boolean, nvo.IsFreezeOocytes);
                                this.dbServer.AddInParameter(command3, "ExpiryDate", DbType.DateTime, nvo.VitrificationMain.ExpiryDate);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                                nvo.VitrificationMain.DonateCryoID = (long) this.dbServer.GetParameterValue(command3, "ID");
                                DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                                this.dbServer.AddInParameter(command4, "ID", DbType.Int64, svo.ID);
                                this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "VitrivicationID", DbType.Int64, nvo.VitrificationMain.DonateCryoID);
                                this.dbServer.AddInParameter(command4, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, svo.EmbNumber);
                                this.dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, svo.EmbSerialNumber);
                                this.dbServer.AddInParameter(command4, "LeafNo", DbType.String, svo.LeafNo);
                                this.dbServer.AddInParameter(command4, "EmbDays", DbType.String, svo.EmbDays);
                                this.dbServer.AddInParameter(command4, "ColorCodeID", DbType.Int64, svo.ColorCodeID);
                                this.dbServer.AddInParameter(command4, "CanId", DbType.Int64, svo.CanId);
                                this.dbServer.AddInParameter(command4, "StrawId", DbType.Int64, svo.StrawId);
                                this.dbServer.AddInParameter(command4, "GobletShapeId", DbType.Int64, svo.GobletShapeId);
                                this.dbServer.AddInParameter(command4, "GobletSizeId", DbType.Int64, svo.GobletSizeId);
                                this.dbServer.AddInParameter(command4, "TankId", DbType.Int64, svo.TankId);
                                this.dbServer.AddInParameter(command4, "ConistorNo", DbType.Int64, svo.ConistorNo);
                                this.dbServer.AddInParameter(command4, "ProtocolTypeID", DbType.Int64, svo.ProtocolTypeID);
                                this.dbServer.AddInParameter(command4, "TransferDate", DbType.DateTime, svo.TransferDate);
                                this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, svo.TransferDay);
                                this.dbServer.AddInParameter(command4, "CellStageID", DbType.String, svo.CellStageID);
                                this.dbServer.AddInParameter(command4, "GradeID", DbType.Int64, svo.GradeID);
                                this.dbServer.AddInParameter(command4, "EmbStatus", DbType.String, svo.EmbStatus);
                                this.dbServer.AddInParameter(command4, "Comments", DbType.String, svo.Comments);
                                this.dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                                if (nvo.VitrificationMain.IsFreezed && !nvo.VitrificationMain.IsOnlyVitrification)
                                {
                                    this.dbServer.AddInParameter(command4, "IsThawingDone", DbType.Boolean, true);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command4, "IsThawingDone", DbType.Boolean, false);
                                }
                                this.dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, svo.OocyteDonorID);
                                this.dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, svo.OocyteDonorUnitID);
                                this.dbServer.AddInParameter(command4, "UsedByOtherCycle", DbType.Boolean, nvo.VitrificationMain.UsedByOtherCycle);
                                this.dbServer.AddInParameter(command4, "UsedTherapyID", DbType.Int64, nvo.VitrificationMain.UsedTherapyID);
                                this.dbServer.AddInParameter(command4, "UsedTherapyUnitID", DbType.Int64, nvo.VitrificationMain.UsedTherapyUnitID);
                                this.dbServer.AddInParameter(command4, "ReceivingDate", DbType.DateTime, nvo.VitrificationMain.DateTime);
                                this.dbServer.AddInParameter(command4, "TransferDayNo", DbType.Int64, svo.TransferDayNo);
                                this.dbServer.AddInParameter(command4, "CleavageGrade", DbType.Int64, svo.CleavageGrade);
                                this.dbServer.AddInParameter(command4, "StageofDevelopmentGrade", DbType.Int64, svo.StageofDevelopmentGrade);
                                this.dbServer.AddInParameter(command4, "InnerCellMassGrade", DbType.Int64, svo.InnerCellMassGrade);
                                this.dbServer.AddInParameter(command4, "TrophoectodermGrade", DbType.Int64, svo.TrophoectodermGrade);
                                this.dbServer.AddInParameter(command4, "CellStage", DbType.String, svo.CellStage);
                                this.dbServer.AddInParameter(command4, "VitrificationDate", DbType.DateTime, svo.VitrificationDate);
                                this.dbServer.AddInParameter(command4, "VitrificationTime", DbType.DateTime, svo.VitrificationTime);
                                this.dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, svo.VitrificationNo);
                                this.dbServer.AddInParameter(command4, "IsSaved", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, svo.IsFreezeOocytes);
                                this.dbServer.AddInParameter(command4, "CryoCode", DbType.String, svo.CryoCode);
                                this.dbServer.AddInParameter(command4, "IsDonateCryo", DbType.Boolean, null);
                                this.dbServer.AddInParameter(command4, "RecepientPatientID", DbType.Int64, null);
                                this.dbServer.AddInParameter(command4, "RecepientPatientUnitID", DbType.Int64, null);
                                this.dbServer.AddInParameter(command4, "IsDonatedCryoReceived", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command4, "DonorPatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                                this.dbServer.AddInParameter(command4, "DonorPatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                                this.dbServer.ExecuteNonQuery(command4, transaction);
                            }
                        }
                    }
                }
                if (nvo.VitrificationMain.IsFreezed && (!nvo.VitrificationMain.IsOnlyVitrification && !nvo.VitrificationMain.IsCryoWOThaw))
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                    this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(storedProcCommand, "LabPersonId", DbType.Int64, null);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.VitrificationMain.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if ((nvo.VitrificationDetailsList != null) && (nvo.VitrificationDetailsList.Count > 0))
                    {
                        foreach (clsIVFDashBoard_VitrificationDetailsVO svo2 in nvo.VitrificationDetailsList)
                        {
                            DbCommand command = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");
                            this.dbServer.AddInParameter(command, "ID", DbType.Int64, svo2.ID);
                            this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "ThawingID", DbType.Int64, nvo.VitrificationMain.ID);
                            this.dbServer.AddInParameter(command, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command, "EmbNumber", DbType.Int64, svo2.EmbNumber);
                            this.dbServer.AddInParameter(command, "EmbSerialNumber", DbType.Int64, svo2.EmbSerialNumber);
                            this.dbServer.AddInParameter(command, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command, "CellStageID", DbType.Int64, svo2.CellStageID);
                            this.dbServer.AddInParameter(command, "GradeID", DbType.Int64, svo2.GradeID);
                            this.dbServer.AddInParameter(command, "NextPlan", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command, "Status", DbType.String, null);
                            this.dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, svo2.OocyteDonorID);
                            this.dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, svo2.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command, "TransferDay", DbType.Int64, svo2.TransferDayNo);
                            this.dbServer.ExecuteNonQuery(command, transaction);
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.VitrificationMain = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateIVFDashBoard_VitrificationSingle(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateVitrificationBizActionVO nvo = valueObject as clsIVFDashboard_AddUpdateVitrificationBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, nvo.VitrificationMain.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "VitrificationNo", DbType.String, nvo.VitrificationMain.VitrificationNo);
                this.dbServer.AddInParameter(storedProcCommand, "PickUpDate", DbType.DateTime, nvo.VitrificationMain.PickUpDate);
                this.dbServer.AddInParameter(storedProcCommand, "ConsentForm", DbType.Boolean, nvo.VitrificationMain.ConsentForm);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezed", DbType.Boolean, nvo.VitrificationMain.IsFreezed);
                this.dbServer.AddInParameter(storedProcCommand, "IsOnlyVitrification", DbType.Boolean, nvo.VitrificationMain.IsOnlyVitrification);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyID", DbType.Int64, nvo.VitrificationMain.SrcOoctyID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcSemenID", DbType.Int64, nvo.VitrificationMain.SrcSemenID);
                this.dbServer.AddInParameter(storedProcCommand, "SrcOoctyCode", DbType.String, nvo.VitrificationMain.SrcOoctyCode);
                this.dbServer.AddInParameter(storedProcCommand, "SrcSemenCode", DbType.String, nvo.VitrificationMain.SrcSemenCode);
                this.dbServer.AddInParameter(storedProcCommand, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                this.dbServer.AddInParameter(storedProcCommand, "FromForm", DbType.Int64, 2);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.VitrificationMain.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezeOocytes", DbType.Boolean, nvo.IsFreezeOocytes);
                if (nvo.VitrificationDetailsObj.IsRefreeze || nvo.VitrificationDetailsObj.IsRefreezeFromOtherCycle)
                {
                    nvo.IsRefreeze = true;
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsRefreeze", DbType.Boolean, nvo.IsRefreeze);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.VitrificationMain.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (nvo.VitrificationDetailsObj != null)
                {
                    clsIVFDashBoard_VitrificationDetailsVO vitrificationDetailsObj = nvo.VitrificationDetailsObj;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, vitrificationDetailsObj.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, nvo.VitrificationMain.ID);
                    this.dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, vitrificationDetailsObj.EmbNumber);
                    this.dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, vitrificationDetailsObj.EmbSerialNumber);
                    this.dbServer.AddInParameter(command2, "LeafNo", DbType.String, vitrificationDetailsObj.LeafNo);
                    this.dbServer.AddInParameter(command2, "EmbDays", DbType.String, vitrificationDetailsObj.EmbDays);
                    this.dbServer.AddInParameter(command2, "ColorCodeID", DbType.Int64, vitrificationDetailsObj.ColorCodeID);
                    this.dbServer.AddInParameter(command2, "CanId", DbType.Int64, vitrificationDetailsObj.CanId);
                    this.dbServer.AddInParameter(command2, "StrawId", DbType.Int64, vitrificationDetailsObj.StrawId);
                    this.dbServer.AddInParameter(command2, "GobletShapeId", DbType.Int64, vitrificationDetailsObj.GobletShapeId);
                    this.dbServer.AddInParameter(command2, "GobletSizeId", DbType.Int64, vitrificationDetailsObj.GobletSizeId);
                    this.dbServer.AddInParameter(command2, "TankId", DbType.Int64, vitrificationDetailsObj.TankId);
                    this.dbServer.AddInParameter(command2, "ConistorNo", DbType.Int64, vitrificationDetailsObj.ConistorNo);
                    this.dbServer.AddInParameter(command2, "ProtocolTypeID", DbType.Int64, vitrificationDetailsObj.ProtocolTypeID);
                    this.dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, vitrificationDetailsObj.TransferDate);
                    this.dbServer.AddInParameter(command2, "TransferDay", DbType.String, vitrificationDetailsObj.TransferDay);
                    this.dbServer.AddInParameter(command2, "CellStageID", DbType.String, vitrificationDetailsObj.CellStageID);
                    this.dbServer.AddInParameter(command2, "GradeID", DbType.Int64, vitrificationDetailsObj.GradeID);
                    this.dbServer.AddInParameter(command2, "EmbStatus", DbType.String, vitrificationDetailsObj.EmbStatus);
                    this.dbServer.AddInParameter(command2, "Comments", DbType.String, vitrificationDetailsObj.Comments);
                    this.dbServer.AddInParameter(command2, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                    if (nvo.VitrificationMain.IsFreezed && !nvo.VitrificationMain.IsOnlyVitrification)
                    {
                        this.dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, true);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, false);
                    }
                    this.dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, vitrificationDetailsObj.OocyteDonorID);
                    this.dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, vitrificationDetailsObj.OocyteDonorUnitID);
                    this.dbServer.AddInParameter(command2, "UsedByOtherCycle", DbType.Boolean, nvo.VitrificationMain.UsedByOtherCycle);
                    this.dbServer.AddInParameter(command2, "UsedTherapyID", DbType.Int64, nvo.VitrificationMain.UsedTherapyID);
                    this.dbServer.AddInParameter(command2, "UsedTherapyUnitID", DbType.Int64, nvo.VitrificationMain.UsedTherapyUnitID);
                    this.dbServer.AddInParameter(command2, "ReceivingDate", DbType.DateTime, nvo.VitrificationMain.DateTime);
                    this.dbServer.AddInParameter(command2, "TransferDayNo", DbType.Int64, vitrificationDetailsObj.TransferDayNo);
                    this.dbServer.AddInParameter(command2, "CleavageGrade", DbType.Int64, vitrificationDetailsObj.CleavageGrade);
                    this.dbServer.AddInParameter(command2, "StageofDevelopmentGrade", DbType.Int64, vitrificationDetailsObj.StageofDevelopmentGrade);
                    this.dbServer.AddInParameter(command2, "InnerCellMassGrade", DbType.Int64, vitrificationDetailsObj.InnerCellMassGrade);
                    this.dbServer.AddInParameter(command2, "TrophoectodermGrade", DbType.Int64, vitrificationDetailsObj.TrophoectodermGrade);
                    this.dbServer.AddInParameter(command2, "CellStage", DbType.String, vitrificationDetailsObj.CellStage);
                    this.dbServer.AddInParameter(command2, "VitrificationDate", DbType.DateTime, vitrificationDetailsObj.VitrificationDate);
                    this.dbServer.AddInParameter(command2, "VitrificationTime", DbType.DateTime, vitrificationDetailsObj.VitrificationTime);
                    this.dbServer.AddInParameter(command2, "VitrificationNo", DbType.String, vitrificationDetailsObj.VitrificationNo);
                    this.dbServer.AddInParameter(command2, "IsSaved", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, vitrificationDetailsObj.ExpiryDate);
                    this.dbServer.AddInParameter(command2, "ExpiryTime", DbType.DateTime, vitrificationDetailsObj.ExpiryTime);
                    this.dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Boolean, vitrificationDetailsObj.IsFreezeOocytes);
                    this.dbServer.AddInParameter(command2, "CryoCode", DbType.String, vitrificationDetailsObj.CryoCode);
                    this.dbServer.AddInParameter(command2, "IsDonateCryo", DbType.Boolean, vitrificationDetailsObj.IsDonateCryo);
                    this.dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, vitrificationDetailsObj.RecepientPatientID);
                    this.dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, vitrificationDetailsObj.RecepientPatientUnitID);
                    this.dbServer.AddInParameter(command2, "IsDonorCycleDonateCryo", DbType.Boolean, vitrificationDetailsObj.IsDonorCycleDonateCryo);
                    this.dbServer.AddInParameter(command2, "IsFreshEmbryoPGDPGS", DbType.Boolean, vitrificationDetailsObj.IsFreshEmbryoPGDPGS);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    if (vitrificationDetailsObj.IsDonateCryo || vitrificationDetailsObj.IsDonorCycleDonateCryo)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "PatientID", DbType.Int64, vitrificationDetailsObj.RecepientPatientID);
                        this.dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, vitrificationDetailsObj.RecepientPatientUnitID);
                        this.dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                        this.dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, nvo.VitrificationMain.DateTime);
                        this.dbServer.AddInParameter(command3, "VitrificationNo", DbType.String, nvo.VitrificationMain.VitrificationNo);
                        this.dbServer.AddInParameter(command3, "PickUpDate", DbType.DateTime, nvo.VitrificationMain.PickUpDate);
                        this.dbServer.AddInParameter(command3, "ConsentForm", DbType.Boolean, nvo.VitrificationMain.ConsentForm);
                        this.dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, nvo.VitrificationMain.IsFreezed);
                        this.dbServer.AddInParameter(command3, "IsOnlyVitrification", DbType.Boolean, nvo.VitrificationMain.IsOnlyVitrification);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, nvo.VitrificationMain.SrcOoctyID);
                        this.dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, nvo.VitrificationMain.SrcSemenID);
                        this.dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, nvo.VitrificationMain.SrcOoctyCode);
                        this.dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, nvo.VitrificationMain.SrcSemenCode);
                        this.dbServer.AddInParameter(command3, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                        this.dbServer.AddInParameter(command3, "IsRefreeze", DbType.Boolean, nvo.VitrificationMain.IsRefeeze);
                        this.dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 2);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.VitrificationMain.DonateCryoID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.AddInParameter(command3, "IsFreezeOocytes", DbType.Boolean, nvo.IsFreezeOocytes);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        nvo.VitrificationMain.DonateCryoID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");
                        this.dbServer.AddInParameter(command4, "ID", DbType.Int64, vitrificationDetailsObj.ID);
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "VitrivicationID", DbType.Int64, nvo.VitrificationMain.DonateCryoID);
                        this.dbServer.AddInParameter(command4, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, vitrificationDetailsObj.EmbNumber);
                        this.dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, vitrificationDetailsObj.EmbSerialNumber);
                        this.dbServer.AddInParameter(command4, "LeafNo", DbType.String, vitrificationDetailsObj.LeafNo);
                        this.dbServer.AddInParameter(command4, "EmbDays", DbType.String, vitrificationDetailsObj.EmbDays);
                        this.dbServer.AddInParameter(command4, "ColorCodeID", DbType.Int64, vitrificationDetailsObj.ColorCodeID);
                        this.dbServer.AddInParameter(command4, "CanId", DbType.Int64, vitrificationDetailsObj.CanId);
                        this.dbServer.AddInParameter(command4, "StrawId", DbType.Int64, vitrificationDetailsObj.StrawId);
                        this.dbServer.AddInParameter(command4, "GobletShapeId", DbType.Int64, vitrificationDetailsObj.GobletShapeId);
                        this.dbServer.AddInParameter(command4, "GobletSizeId", DbType.Int64, vitrificationDetailsObj.GobletSizeId);
                        this.dbServer.AddInParameter(command4, "TankId", DbType.Int64, vitrificationDetailsObj.TankId);
                        this.dbServer.AddInParameter(command4, "ConistorNo", DbType.Int64, vitrificationDetailsObj.ConistorNo);
                        this.dbServer.AddInParameter(command4, "ProtocolTypeID", DbType.Int64, vitrificationDetailsObj.ProtocolTypeID);
                        this.dbServer.AddInParameter(command4, "TransferDate", DbType.DateTime, vitrificationDetailsObj.TransferDate);
                        this.dbServer.AddInParameter(command4, "TransferDay", DbType.String, vitrificationDetailsObj.TransferDay);
                        this.dbServer.AddInParameter(command4, "CellStageID", DbType.String, vitrificationDetailsObj.CellStageID);
                        this.dbServer.AddInParameter(command4, "GradeID", DbType.Int64, vitrificationDetailsObj.GradeID);
                        this.dbServer.AddInParameter(command4, "EmbStatus", DbType.String, vitrificationDetailsObj.EmbStatus);
                        this.dbServer.AddInParameter(command4, "Comments", DbType.String, vitrificationDetailsObj.Comments);
                        this.dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                        if (nvo.VitrificationMain.IsFreezed && !nvo.VitrificationMain.IsOnlyVitrification)
                        {
                            this.dbServer.AddInParameter(command4, "IsThawingDone", DbType.Boolean, true);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(command4, "IsThawingDone", DbType.Boolean, false);
                        }
                        this.dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, vitrificationDetailsObj.OocyteDonorID);
                        this.dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, vitrificationDetailsObj.OocyteDonorUnitID);
                        this.dbServer.AddInParameter(command4, "UsedByOtherCycle", DbType.Boolean, nvo.VitrificationMain.UsedByOtherCycle);
                        this.dbServer.AddInParameter(command4, "UsedTherapyID", DbType.Int64, nvo.VitrificationMain.UsedTherapyID);
                        this.dbServer.AddInParameter(command4, "UsedTherapyUnitID", DbType.Int64, nvo.VitrificationMain.UsedTherapyUnitID);
                        this.dbServer.AddInParameter(command4, "ReceivingDate", DbType.DateTime, nvo.VitrificationMain.DateTime);
                        this.dbServer.AddInParameter(command4, "TransferDayNo", DbType.Int64, vitrificationDetailsObj.TransferDayNo);
                        this.dbServer.AddInParameter(command4, "CleavageGrade", DbType.Int64, vitrificationDetailsObj.CleavageGrade);
                        this.dbServer.AddInParameter(command4, "StageofDevelopmentGrade", DbType.Int64, vitrificationDetailsObj.StageofDevelopmentGrade);
                        this.dbServer.AddInParameter(command4, "InnerCellMassGrade", DbType.Int64, vitrificationDetailsObj.InnerCellMassGrade);
                        this.dbServer.AddInParameter(command4, "TrophoectodermGrade", DbType.Int64, vitrificationDetailsObj.TrophoectodermGrade);
                        this.dbServer.AddInParameter(command4, "CellStage", DbType.String, vitrificationDetailsObj.CellStage);
                        this.dbServer.AddInParameter(command4, "VitrificationDate", DbType.DateTime, vitrificationDetailsObj.VitrificationDate);
                        this.dbServer.AddInParameter(command4, "VitrificationTime", DbType.DateTime, vitrificationDetailsObj.VitrificationTime);
                        this.dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, vitrificationDetailsObj.VitrificationNo);
                        this.dbServer.AddInParameter(command4, "IsSaved", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, vitrificationDetailsObj.ExpiryDate);
                        this.dbServer.AddInParameter(command4, "ExpiryTime", DbType.DateTime, vitrificationDetailsObj.ExpiryTime);
                        this.dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, vitrificationDetailsObj.IsFreezeOocytes);
                        this.dbServer.AddInParameter(command4, "CryoCode", DbType.String, vitrificationDetailsObj.CryoCode);
                        this.dbServer.AddInParameter(command4, "IsDonateCryo", DbType.Boolean, null);
                        this.dbServer.AddInParameter(command4, "RecepientPatientID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command4, "RecepientPatientUnitID", DbType.Int64, null);
                        this.dbServer.AddInParameter(command4, "IsDonatedCryoReceived", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command4, "DonorPatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                        this.dbServer.AddInParameter(command4, "DonorPatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                        this.dbServer.AddInParameter(command4, "IsFreshEmbryoPGDPGS", DbType.Boolean, vitrificationDetailsObj.IsFreshEmbryoPGDPGS);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                    }
                }
                if (nvo.VitrificationMain.IsFreezed && (!nvo.VitrificationMain.IsOnlyVitrification && !nvo.VitrificationMain.IsCryoWOThaw))
                {
                    DbCommand command5 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");
                    this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                    this.dbServer.AddInParameter(command5, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                    this.dbServer.AddInParameter(command5, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                    this.dbServer.AddInParameter(command5, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command5, "DateTime", DbType.DateTime, null);
                    this.dbServer.AddInParameter(command5, "LabPersonId", DbType.Int64, null);
                    this.dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command5, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                    nvo.VitrificationMain.ID = (long) this.dbServer.GetParameterValue(command5, "ID");
                    if ((nvo.VitrificationDetailsList != null) && (nvo.VitrificationDetailsList.Count > 0))
                    {
                        foreach (clsIVFDashBoard_VitrificationDetailsVO svo2 in nvo.VitrificationDetailsList)
                        {
                            DbCommand command6 = this.dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");
                            this.dbServer.AddInParameter(command6, "ID", DbType.Int64, svo2.ID);
                            this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command6, "ThawingID", DbType.Int64, nvo.VitrificationMain.ID);
                            this.dbServer.AddInParameter(command6, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command6, "EmbNumber", DbType.Int64, svo2.EmbNumber);
                            this.dbServer.AddInParameter(command6, "EmbSerialNumber", DbType.Int64, svo2.EmbSerialNumber);
                            this.dbServer.AddInParameter(command6, "Date", DbType.DateTime, null);
                            this.dbServer.AddInParameter(command6, "CellStageID", DbType.Int64, svo2.CellStageID);
                            this.dbServer.AddInParameter(command6, "GradeID", DbType.Int64, svo2.GradeID);
                            this.dbServer.AddInParameter(command6, "NextPlan", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command6, "Comments", DbType.String, null);
                            this.dbServer.AddInParameter(command6, "Status", DbType.String, null);
                            this.dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, svo2.OocyteDonorID);
                            this.dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, svo2.OocyteDonorUnitID);
                            this.dbServer.AddInParameter(command6, "TransferDay", DbType.Int64, svo2.TransferDayNo);
                            this.dbServer.ExecuteNonQuery(command6, transaction);
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.VitrificationMain = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject GetIVFDashBoard_PreviousEmbFromVitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetPreviousVitrificationBizActionVO nvo = valueObject as clsIVFDashboard_GetPreviousVitrificationBizActionVO;
            try
            {
                DbCommand command = !nvo.IsFreezeOocytes ? this.dbServer.GetStoredProcCommand("IVFDashboard_PreviousEmbFromVitrification") : this.dbServer.GetStoredProcCommand("IVFDashboard_PreviousEmbFromVitrificationForOocytes");
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                this.dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, nvo.VitrificationMain.UsedOwnOocyte);
                this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, nvo.VitrificationMain.IsOnlyVitrification);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO item = new clsIVFDashBoard_VitrificationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"])),
                            CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                            StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"])),
                            GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                            GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"])),
                            GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"])),
                            TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"])),
                            CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"])),
                            IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"])),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"])),
                            OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"])),
                            OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"])),
                            TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"])),
                            CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"])),
                            StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"])),
                            InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"])),
                            TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"])),
                            VitrificationDate = DALHelper.HandleDate(reader["VitrificationDate"]),
                            VitrificationTime = DALHelper.HandleDate(reader["VitrificationTime"]),
                            VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"])),
                            StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"])),
                            InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"])),
                            TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"]))
                        };
                        nvo.VitrificationDetailsList.Add(item);
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

        public override IValueObject GetIVFDashBoard_PreviousOocyteFromVitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            return (valueObject as clsIVFDashboard_GetPreviousVitrificationBizActionVO);
        }

        public override IValueObject GetIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetVitrificationBizActionVO nvo = valueObject as clsIVFDashboard_GetVitrificationBizActionVO;
            try
            {
                DbCommand command = nvo.IsFreezeOocytes ? this.dbServer.GetStoredProcCommand("IVFDashboard_GetVitrificationDetailsForOocytes") : this.dbServer.GetStoredProcCommand("IVFDashboard_GetVitrificationDetails");
                this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                this.dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                this.dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                this.dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, nvo.VitrificationMain.IsOnlyVitrification);
                this.dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command, "IsForThawTab", DbType.Boolean, nvo.IsForThawTab);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.VitrificationMain.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.VitrificationMain.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.VitrificationMain.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        nvo.VitrificationMain.PickUpDate = DALHelper.HandleDate(reader["PickUpDate"]);
                        nvo.VitrificationMain.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                        nvo.VitrificationMain.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.VitrificationMain.IsOnlyVitrification = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOnlyVitrification"]));
                        nvo.VitrificationMain.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.VitrificationMain.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.VitrificationMain.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.VitrificationMain.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.VitrificationMain.DateTime = DALHelper.HandleDate(reader["DateTime"]);
                        nvo.VitrificationMain.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                        nvo.VitrificationMain.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                        nvo.VitrificationMain.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        nvo.VitrificationMain.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                        nvo.VitrificationMain.UsedOwnOocyte = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UsedOwnOocyte"]));
                        nvo.VitrificationRefeezeMain.IsRefeeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"]));
                        nvo.VitrificationMain.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO item = new clsIVFDashBoard_VitrificationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"])),
                            CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                            StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"])),
                            GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                            GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"])),
                            GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"])),
                            TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"])),
                            CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"])),
                            IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"])),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"])),
                            OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"])),
                            OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"])),
                            IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"])),
                            TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"])),
                            CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"])),
                            StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"])),
                            InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"])),
                            TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"])),
                            VitrificationDate = DALHelper.HandleDate(reader["VitrificationDate"]),
                            VitrificationTime = DALHelper.HandleDate(reader["VitrificationTime"]),
                            VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"])),
                            StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"])),
                            InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"])),
                            TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"])),
                            IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"])),
                            IsRefreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"])),
                            IsRefreezeFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreezeFromOtherCycle"])),
                            CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"])),
                            IsDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonateCryo"])),
                            RecepientPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientID"])),
                            RecepientPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientUnitID"])),
                            IsDonatedCryoReceived = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonatedCryoReceived"])),
                            DonorPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientID"])),
                            DonorPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientUnitID"])),
                            IsDonorCycleDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycleDonateCryo"])),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ExpiryTime = DALHelper.HandleDate(reader["ExpiryTime"]),
                            IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"])),
                            IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]))
                        };
                        nvo.VitrificationDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.VitrificationRefeezeMain.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.VitrificationRefeezeMain.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.VitrificationRefeezeMain.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        nvo.VitrificationRefeezeMain.PickUpDate = DALHelper.HandleDate(reader["PickUpDate"]);
                        nvo.VitrificationRefeezeMain.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                        nvo.VitrificationRefeezeMain.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.VitrificationRefeezeMain.IsOnlyVitrification = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOnlyVitrification"]));
                        nvo.VitrificationRefeezeMain.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.VitrificationRefeezeMain.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.VitrificationRefeezeMain.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.VitrificationRefeezeMain.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.VitrificationRefeezeMain.DateTime = DALHelper.HandleDate(reader["DateTime"]);
                        nvo.VitrificationRefeezeMain.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                        nvo.VitrificationRefeezeMain.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                        nvo.VitrificationRefeezeMain.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        nvo.VitrificationRefeezeMain.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                        nvo.VitrificationRefeezeMain.UsedOwnOocyte = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UsedOwnOocyte"]));
                        nvo.VitrificationRefeezeMain.IsRefeeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"]));
                        nvo.VitrificationRefeezeMain.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO item = new clsIVFDashBoard_VitrificationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"])),
                            CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = DALHelper.HandleDate(reader["TransferDate"]),
                            StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"])),
                            GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                            GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"])),
                            GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"])),
                            TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"])),
                            CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"])),
                            IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"])),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"])),
                            OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"])),
                            OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"])),
                            IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"])),
                            TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"])),
                            CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"])),
                            StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"])),
                            InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"])),
                            TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"])),
                            VitrificationDate = DALHelper.HandleDate(reader["VitrificationDate"]),
                            VitrificationTime = DALHelper.HandleDate(reader["VitrificationTime"]),
                            VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"])),
                            StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"])),
                            InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"])),
                            TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"])),
                            IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"])),
                            IsRefreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"])),
                            IsRefreezeFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreezeFromOtherCycle"])),
                            CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"])),
                            IsDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonateCryo"])),
                            RecepientPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientID"])),
                            RecepientPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientUnitID"])),
                            IsDonatedCryoReceived = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonatedCryoReceived"])),
                            DonorPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientID"])),
                            DonorPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientUnitID"])),
                            IsDonorCycleDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycleDonateCryo"])),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ExpiryTime = DALHelper.HandleDate(reader["ExpiryTime"]),
                            IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDate(reader["IsFreshEmbryoPGDPGS"]))
                        };
                        nvo.VitrificationRefeezeDetailsList.Add(item);
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

        public override IValueObject GetOocyteVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank bank = valueObject as cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank;
            bank.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDashboard_GetOocyteVirtificationDetailsForEmbryoCryoBank");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, bank.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, bank.SearchExpression);
                if (bank.FName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FName", DbType.String, bank.FName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FName", DbType.String, null);
                }
                if (bank.MName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MName", DbType.String, bank.MName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MName", DbType.String, null);
                }
                if (bank.LName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LName", DbType.String, bank.LName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LName", DbType.String, null);
                }
                if (bank.FamilyName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, bank.FamilyName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, null);
                }
                if (bank.MRNo != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, bank.MRNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, bank.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, bank.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, bank.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO item = new clsIVFDashBoard_VitrificationDetailsVO {
                            VitrivicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            VitrificationUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"])),
                            VitrificationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]))),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteSerialNumber"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"])),
                            ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]))),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteStatus"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            Conistor = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            GobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            Can = Convert.ToString(DALHelper.HandleDBNull(reader["Can"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsUnitID"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]))
                        };
                        bank.Vitrification.VitrificationDetailsForOocyteList.Add(item);
                    }
                }
                reader.NextResult();
                bank.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return bank;
        }

        public override IValueObject GetUsedEmbryoDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO nvo = valueObject as clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDashboard_GetUsedEmbryoDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VitrificationDetailsList == null)
                    {
                        nvo.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO item = new clsIVFDashBoard_VitrificationDetailsVO {
                            VitrivicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            VitrificationUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"])),
                            VitrificationDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]))),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]))),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            Conistor = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            GobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            Can = Convert.ToString(DALHelper.HandleDBNull(reader["Can"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsUnitID"])),
                            ReceivingDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivingDate"])))
                        };
                        nvo.VitrificationDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_GetVitrificationDetailsForCryoBank bank = valueObject as cls_IVFDashboard_GetVitrificationDetailsForCryoBank;
            bank.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IVFDashboard_GetVirtificationDetailsForEmbryoCryoBank");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, bank.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, bank.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, bank.SearchExpression);
                if (bank.FName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FName", DbType.String, bank.FName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FName", DbType.String, null);
                }
                if (bank.MName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MName", DbType.String, bank.MName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MName", DbType.String, null);
                }
                if (bank.LName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LName", DbType.String, bank.LName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LName", DbType.String, null);
                }
                if (bank.FamilyName != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, bank.FamilyName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FamilyName", DbType.String, null);
                }
                if (bank.MRNo != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, bank.MRNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "Cane", DbType.Int64, bank.Cane);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, bank.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, bank.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, bank.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreezeOocytes", DbType.Boolean, bank.IsFreezeOocytes);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO item = new clsIVFDashBoard_VitrificationDetailsVO {
                            VitrivicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            VitrificationUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"])),
                            EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"])),
                            ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"])),
                            TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"])),
                            TransferDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]))),
                            CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"])),
                            Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"])),
                            EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"])),
                            Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"])),
                            Conistor = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"])),
                            GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"])),
                            GobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"])),
                            Can = Convert.ToString(DALHelper.HandleDBNull(reader["Can"])),
                            Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsUnitID"])),
                            PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"])),
                            PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"])),
                            IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"])),
                            TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"])),
                            CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"])),
                            StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"])),
                            InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"])),
                            TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"])),
                            VitrificationDate = DALHelper.HandleDate(reader["VitrificationDate"]),
                            VitrificationTime = DALHelper.HandleDate(reader["VitrificationTime"]),
                            VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"])),
                            StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"])),
                            InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"])),
                            TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"])),
                            IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"])),
                            GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])),
                            IsRefreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"])),
                            IsRefreezeFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreezeFromOtherCycle"])),
                            CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            DonorMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["DonorMRNO"])),
                            DonorPatientName = Convert.ToString(DALHelper.HandleDBNull(reader["DonorPatientName"])),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            ExpiryTime = DALHelper.HandleDate(reader["ExpiryTime"]),
                            LongTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LongTerm"])),
                            ShortTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ShortTerm"])),
                            IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"])),
                            IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]))
                        };
                        bank.Vitrification.VitrificationDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                bank.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return bank;
        }

        public override IValueObject UpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_UpdateVitrificationDetailsBizActionVO nvo = valueObject as clsIVFDashboard_UpdateVitrificationDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                if ((nvo.VitrificationDetailsList != null) && (nvo.VitrificationDetailsList.Count > 0))
                {
                    foreach (clsIVFDashBoard_VitrificationDetailsVO svo in nvo.VitrificationDetailsList)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, svo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, svo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrificationID", DbType.Int64, svo.VitrivicationID);
                        this.dbServer.AddInParameter(storedProcCommand, "VitrificationUnitID", DbType.Int64, svo.VitrificationUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UsedByOtherCycle", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "UsedTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                        this.dbServer.AddInParameter(storedProcCommand, "UsedTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "ReceivingDate", DbType.DateTime, nvo.VitrificationMain.ReceivingDate);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                    if (nvo.VitrificationMain.SerialOocyteNumberString != string.Empty)
                    {
                        DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddDay0OocListInGhaphicalRepresentationTableForEmbryoRecipient");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.VitrificationMain.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.VitrificationMain.PatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.VitrificationMain.PlanTherapyID);
                        this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.VitrificationMain.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorPatientID", DbType.Int64, nvo.VitrificationMain.DonorPatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "DonorPatientUnitID", DbType.Int64, nvo.VitrificationMain.DonorPatientUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "SerialOocyteNumberString", DbType.String, nvo.VitrificationMain.SerialOocyteNumberString);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.VitrificationMain = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }
    }
}

