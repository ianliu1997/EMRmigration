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

    internal class cls_NewSpremFreezingDAL : cls_NewBaseSpremFreezingDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private cls_NewSpremFreezingDAL()
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

        public override IValueObject AddUpdateSpremFrezing(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_NewAddUpdateSpremFreezingBizActionVO nvo = valueObject as cls_NewAddUpdateSpremFreezingBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.SpremFreezingMainVO.BatchID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, nvo.SpremFreezingMainVO.BatchCode);
                this.dbServer.AddInParameter(storedProcCommand, "InvoiceNo", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedByID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "ReceivedDate", DbType.DateTime, null);
                this.dbServer.AddInParameter(storedProcCommand, "LabID", DbType.Int64, nvo.SpremFreezingMainVO.LabID);
                this.dbServer.AddInParameter(storedProcCommand, "NoOfVails", DbType.Int32, nvo.SpremFreezingMainVO.NoOfVails);
                this.dbServer.AddInParameter(storedProcCommand, "Volume", DbType.Single, nvo.SpremFreezingMainVO.Volume);
                this.dbServer.AddInParameter(storedProcCommand, "AvailableVolume", DbType.Single, 0);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, null);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.SpremFreezingMainVO.BatchID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.SpremFreezingMainVO.BatchUnitID = UserVo.UserLoginInfo.UnitId;
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New");
                if (nvo.ID > 0L)
                {
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, nvo.ID);
                }
                else
                {
                    this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }
                this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                this.dbServer.AddInParameter(command2, "TherapyID", DbType.Int64, nvo.SpremFreezingMainVO.TherapyID);
                this.dbServer.AddInParameter(command2, "TherapyUnitID", DbType.Int64, nvo.SpremFreezingMainVO.TherapyUnitID);
                this.dbServer.AddInParameter(command2, "CycleCode", DbType.String, nvo.SpremFreezingMainVO.CycleCode);
                this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, nvo.SpremFreezingMainVO.BatchID);
                this.dbServer.AddInParameter(command2, "BatchUnitID", DbType.Int64, nvo.SpremFreezingMainVO.BatchUnitID);
                this.dbServer.AddInParameter(command2, "DoctorID", DbType.Int64, nvo.SpremFreezingMainVO.DoctorID);
                this.dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, nvo.SpremFreezingMainVO.EmbryologistID);
                this.dbServer.AddInParameter(command2, "SpremFreezingTime", DbType.DateTime, nvo.SpremFreezingMainVO.SpremFreezingDate);
                this.dbServer.AddInParameter(command2, "SpremFreezingDate", DbType.DateTime, nvo.SpremFreezingMainVO.SpremFreezingDate);
                this.dbServer.AddInParameter(command2, "CollectionMethodID", DbType.Int64, nvo.SpremFreezingMainVO.CollectionMethodID);
                this.dbServer.AddInParameter(command2, "ViscosityID", DbType.Int64, nvo.SpremFreezingMainVO.ViscosityID);
                this.dbServer.AddInParameter(command2, "CollectionProblem", DbType.String, nvo.SpremFreezingMainVO.CollectionProblem);
                this.dbServer.AddInParameter(command2, "Other", DbType.String, nvo.SpremFreezingMainVO.Other);
                this.dbServer.AddInParameter(command2, "Comments", DbType.String, nvo.SpremFreezingMainVO.Comments);
                this.dbServer.AddInParameter(command2, "Abstience", DbType.String, nvo.SpremFreezingMainVO.Abstience);
                this.dbServer.AddInParameter(command2, "Volume", DbType.Single, nvo.SpremFreezingMainVO.Volume);
                this.dbServer.AddInParameter(command2, "Motility", DbType.Decimal, nvo.SpremFreezingMainVO.Motility);
                this.dbServer.AddInParameter(command2, "GradeA", DbType.Decimal, nvo.SpremFreezingMainVO.GradeA);
                this.dbServer.AddInParameter(command2, "GradeB", DbType.Decimal, nvo.SpremFreezingMainVO.GradeB);
                this.dbServer.AddInParameter(command2, "GradeC", DbType.Decimal, nvo.SpremFreezingMainVO.GradeC);
                this.dbServer.AddInParameter(command2, "TotalSpremCount", DbType.Int64, nvo.SpremFreezingMainVO.TotalSpremCount);
                this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, nvo.SpremFreezingMainVO.Status);
                this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(command2, "SpremExpiryDate", DbType.DateTime, nvo.SpremFreezingMainVO.SpremExpiryDate);
                this.dbServer.AddInParameter(command2, "SpermTypeID", DbType.Int64, nvo.SpremFreezingMainVO.SpermTypeID);
                this.dbServer.AddInParameter(command2, "SampleCode", DbType.String, nvo.SpremFreezingMainVO.SampleCode);
                this.dbServer.AddInParameter(command2, "SampleLinkID", DbType.Int64, nvo.SpremFreezingVO.SampleLinkID);
                this.dbServer.AddInParameter(command2, "Spillage", DbType.String, nvo.SpremFreezingMainVO.Spillage);
                this.dbServer.AddInParameter(command2, "SpermCount", DbType.Decimal, nvo.SpremFreezingMainVO.SpermCount);
                this.dbServer.AddInParameter(command2, "DFI", DbType.Decimal, nvo.SpremFreezingMainVO.DFI);
                this.dbServer.AddInParameter(command2, "ROS", DbType.Decimal, nvo.SpremFreezingMainVO.ROS);
                this.dbServer.AddInParameter(command2, "HIV", DbType.Int64, nvo.SpremFreezingMainVO.HIV);
                this.dbServer.AddInParameter(command2, "HBSAG", DbType.Int64, nvo.SpremFreezingMainVO.HBSAG);
                this.dbServer.AddInParameter(command2, "VDRL", DbType.Int64, nvo.SpremFreezingMainVO.VDRL);
                this.dbServer.AddInParameter(command2, "HCV", DbType.Int64, nvo.SpremFreezingMainVO.HCV);
                this.dbServer.AddInParameter(command2, "CheckedByDoctorID", DbType.Int64, nvo.SpremFreezingMainVO.CheckedByDoctorID);
                this.dbServer.AddInParameter(command2, "IsConsentCheck", DbType.Boolean, nvo.SpremFreezingMainVO.IsConsentCheck);
                this.dbServer.AddInParameter(command2, "IsFreezed", DbType.Boolean, nvo.SpremFreezingMainVO.IsFreezed);
                this.dbServer.AddInParameter(command2, "AbstienceID", DbType.Int64, nvo.SpremFreezingMainVO.AbstienceID);
                this.dbServer.AddInParameter(command2, "PusCells", DbType.String, nvo.SpremFreezingMainVO.PusCells);
                this.dbServer.AddInParameter(command2, "RoundCells", DbType.String, nvo.SpremFreezingMainVO.RoundCells);
                this.dbServer.AddInParameter(command2, "EpithelialCells", DbType.String, nvo.SpremFreezingMainVO.EpithelialCells);
                this.dbServer.AddInParameter(command2, "OtherCells", DbType.String, nvo.SpremFreezingMainVO.OtherCells);
                this.dbServer.AddInParameter(command2, "VisitID", DbType.Int64, nvo.SpremFreezingMainVO.VisitID);
                this.dbServer.AddInParameter(command2, "VisitUnitID", DbType.Int64, nvo.SpremFreezingMainVO.VisitUnitID);
                this.dbServer.AddInParameter(command2, "Sperm5thPercentile", DbType.Single, nvo.SpremFreezingMainVO.Sperm5thPercentile);
                this.dbServer.AddInParameter(command2, "Sperm75thPercentile", DbType.Single, nvo.SpremFreezingMainVO.Sperm75thPercentile);
                this.dbServer.AddInParameter(command2, "Ejaculate5thPercentile", DbType.Single, nvo.SpremFreezingMainVO.Ejaculate5thPercentile);
                this.dbServer.AddInParameter(command2, "Ejaculate75thPercentile", DbType.Single, nvo.SpremFreezingMainVO.Ejaculate75thPercentile);
                this.dbServer.AddInParameter(command2, "TotalMotility5thPercentile", DbType.Single, nvo.SpremFreezingMainVO.TotalMotility5thPercentile);
                this.dbServer.AddInParameter(command2, "TotalMotility75thPercentile", DbType.Single, nvo.SpremFreezingMainVO.TotalMotility75thPercentile);
                this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(command2, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                nvo.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                nvo.UintID = (long) this.dbServer.GetParameterValue(command2, "UnitID");
                foreach (clsNew_SpremFreezingVO gvo in nvo.SpremFreezingDetails)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezingDetails_New");
                    if (gvo.ID > 0L)
                    {
                        this.dbServer.AddInParameter(command3, "ID", DbType.Int64, gvo.ID);
                    }
                    else
                    {
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    }
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "SpremFreezingID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(command3, "SpremFreezingUnitID", DbType.Int64, nvo.UintID);
                    this.dbServer.AddInParameter(command3, "ColorCodeID", DbType.Int64, gvo.GobletColorID);
                    this.dbServer.AddInParameter(command3, "StrawID", DbType.Int64, gvo.StrawId);
                    this.dbServer.AddInParameter(command3, "GlobletShapeID", DbType.Int64, gvo.GobletShapeId);
                    this.dbServer.AddInParameter(command3, "GlobletSizeID", DbType.Int64, gvo.GobletSizeId);
                    this.dbServer.AddInParameter(command3, "PlanTherapy", DbType.Int64, gvo.PlanTherapy);
                    this.dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, gvo.PlanTherapyUnitID);
                    this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, gvo.Status);
                    this.dbServer.AddInParameter(command3, "IsModify", DbType.String, gvo.IsModify);
                    this.dbServer.AddInParameter(command3, "IsThaw", DbType.String, gvo.IsThaw);
                    this.dbServer.AddInParameter(command3, "CaneID", DbType.Int64, gvo.CanID);
                    this.dbServer.AddInParameter(command3, "CanisterID", DbType.Int64, gvo.CanisterId);
                    this.dbServer.AddInParameter(command3, "TankID", DbType.Int64, gvo.TankId);
                    this.dbServer.AddInParameter(command3, "Comments", DbType.String, gvo.Comments);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    long num = Convert.ToInt64(this.dbServer.GetParameterValue(command3, "ID"));
                    if (gvo.IsThaw)
                    {
                        if (gvo.IsModify)
                        {
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New1");
                            if (nvo.ID > 0L)
                            {
                                this.dbServer.AddInParameter(command4, "ID", DbType.Int64, nvo.ID);
                            }
                            else
                            {
                                this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            }
                            this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, nvo.MalePatientID);
                            this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                            this.dbServer.AddInParameter(command4, "TherapyID", DbType.Int64, gvo.PlanTherapy);
                            this.dbServer.AddInParameter(command4, "TherapyUnitID", DbType.Int64, gvo.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command4, "SpermFreezingDetailsID", DbType.Int64, num);
                            this.dbServer.AddInParameter(command4, "SpermFreezingDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "BatchID", DbType.Int64, nvo.SpremFreezingMainVO.BatchID);
                            this.dbServer.AddInParameter(command4, "BatchUnitID", DbType.Int64, nvo.SpremFreezingMainVO.BatchUnitID);
                            this.dbServer.AddInParameter(command4, "UsedVolume", DbType.Single, 0);
                            this.dbServer.AddInParameter(command4, "CycleCode", DbType.String, null);
                            this.dbServer.AddInParameter(command4, "FreezingID", DbType.Int64, nvo.ID);
                            this.dbServer.AddInParameter(command4, "FreezingUnitID", DbType.Int64, nvo.UintID);
                            this.dbServer.AddInParameter(command4, "SpermNo", DbType.Int64, num);
                            this.dbServer.AddInParameter(command4, "LabPersonID", DbType.Int64, nvo.SpremFreezingMainVO.DoctorID);
                            this.dbServer.AddInParameter(command4, "VitrificationDate", DbType.DateTime, nvo.SpremFreezingMainVO.SpremFreezingDate);
                            this.dbServer.AddInParameter(command4, "VitrificationTime", DbType.DateTime, nvo.SpremFreezingMainVO.SpremFreezingTime);
                            this.dbServer.AddInParameter(command4, "Volume", DbType.Decimal, nvo.SpremFreezingMainVO.Volume);
                            this.dbServer.AddInParameter(command4, "Motility", DbType.Decimal, nvo.SpremFreezingMainVO.Motility);
                            this.dbServer.AddInParameter(command4, "TotalSpremCount", DbType.Int64, nvo.SpremFreezingMainVO.TotalSpremCount);
                            this.dbServer.AddInParameter(command4, "Status", DbType.Boolean, nvo.SpremFreezingMainVO.Status);
                            this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command4, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command4, "ResultStatus");
                        }
                        else
                        {
                            DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New1");
                            if (nvo.ID > 0L)
                            {
                                this.dbServer.AddInParameter(command5, "ID", DbType.Int64, nvo.ID);
                            }
                            else
                            {
                                this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            }
                            this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "PatientID", DbType.Int64, nvo.MalePatientID);
                            this.dbServer.AddInParameter(command5, "PatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                            this.dbServer.AddInParameter(command5, "LabPersonID", DbType.Int64, nvo.SpremFreezingMainVO.DoctorID);
                            this.dbServer.AddInParameter(command5, "TherapyID", DbType.Int64, gvo.PlanTherapy);
                            this.dbServer.AddInParameter(command5, "TherapyUnitID", DbType.Int64, gvo.PlanTherapyUnitID);
                            this.dbServer.AddInParameter(command5, "SpermFreezingDetailsID", DbType.Int64, num);
                            this.dbServer.AddInParameter(command5, "SpermFreezingDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "BatchID", DbType.Int64, nvo.SpremFreezingMainVO.BatchID);
                            this.dbServer.AddInParameter(command5, "BatchUnitID", DbType.Int64, nvo.SpremFreezingMainVO.BatchUnitID);
                            this.dbServer.AddInParameter(command5, "UsedVolume", DbType.Single, 0);
                            this.dbServer.AddInParameter(command5, "CycleCode", DbType.String, null);
                            this.dbServer.AddInParameter(command5, "FreezingID", DbType.Int64, nvo.ID);
                            this.dbServer.AddInParameter(command5, "FreezingUnitID", DbType.Int64, nvo.UintID);
                            this.dbServer.AddInParameter(command5, "SpermNo", DbType.Int64, num);
                            this.dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, nvo.SpremFreezingMainVO.SpremFreezingDate);
                            this.dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, nvo.SpremFreezingMainVO.SpremFreezingTime);
                            this.dbServer.AddInParameter(command5, "Volume", DbType.Decimal, nvo.SpremFreezingMainVO.Volume);
                            this.dbServer.AddInParameter(command5, "Motility", DbType.Decimal, nvo.SpremFreezingMainVO.Motility);
                            this.dbServer.AddInParameter(command5, "TotalSpremCount", DbType.Int64, nvo.SpremFreezingMainVO.TotalSpremCount);
                            this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, nvo.SpremFreezingMainVO.Status);
                            this.dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command5, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command5, "ResultStatus");
                        }
                    }
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject DeleteSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO)
        {
            cls_NewDeleteSpremFreezingBizActionVO nvo = valuObject as cls_NewDeleteSpremFreezingBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteSpremfreezingDetails_New");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.MalePatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SpremID", DbType.Int64, nvo.SpremID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception)
            {
                connection.Close();
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetSpremFreezingList(IValueObject valuObject, clsUserVO UserVO)
        {
            cls_NewGetListSpremFreezingBizActionVO nvo = valuObject as cls_NewGetListSpremFreezingBizActionVO;
            nvo.SpremFreezingMainList = new List<cls_NewSpremFreezingMainVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpremFreezingList");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.MalePatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.String, nvo.MalePatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cls_NewSpremFreezingMainVO item = new cls_NewSpremFreezingMainVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            SpremFreezingDate = DALHelper.HandleDate(reader["SpremFreezingDate"]),
                            SpremFreezingTime = DALHelper.HandleDate(reader["SpremFreezingTime"]),
                            CollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"])),
                            Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"])),
                            Embryologist = Convert.ToString(DALHelper.HandleDBNull(reader["Embryologist"])),
                            SpermType = Convert.ToString(DALHelper.HandleDBNull(reader["SpermType"])),
                            TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"])),
                            TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"])),
                            CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"])),
                            NoOfVails = Convert.ToSingle(DALHelper.HandleDBNull(reader["NoOfVails"])),
                            IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"])),
                            Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]))
                        };
                        nvo.SpremFreezingMainList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO)
        {
            cls_NewGetSpremFreezingBizActionVO nvo = valuObject as cls_NewGetSpremFreezingBizActionVO;
            nvo.SpremFreezingMainVO = new cls_NewSpremFreezingMainVO();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSpremFreezing_New");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.String, nvo.UintID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.SpremFreezingMainVO.SpermTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TypeOfSpermID"]));
                        nvo.SpremFreezingMainVO.SampleCode = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCode"]));
                        nvo.SpremFreezingMainVO.SpremFreezingDate = DALHelper.HandleDate(reader["SpremFreezingDate"]);
                        nvo.SpremFreezingMainVO.SpremFreezingTime = DALHelper.HandleDate(reader["SpremFreezingDate"]);
                        nvo.SpremFreezingMainVO.Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"]));
                        nvo.SpremFreezingMainVO.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        nvo.SpremFreezingMainVO.CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"]));
                        nvo.SpremFreezingMainVO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        nvo.SpremFreezingMainVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        nvo.SpremFreezingMainVO.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        nvo.SpremFreezingMainVO.GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"]));
                        nvo.SpremFreezingMainVO.GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"]));
                        nvo.SpremFreezingMainVO.GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"]));
                        nvo.SpremFreezingMainVO.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));
                        nvo.SpremFreezingMainVO.Other = Convert.ToString(DALHelper.HandleDBNull(reader["Other"]));
                        nvo.SpremFreezingMainVO.TotalSpremCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        nvo.SpremFreezingMainVO.ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"]));
                        nvo.SpremFreezingMainVO.Volume = Convert.ToSingle(DALHelper.HandleDBNull(reader["Volume"]));
                        nvo.SpremFreezingMainVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainID"]));
                        nvo.SpremFreezingMainVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainUnitID"]));
                        nvo.SpremFreezingMainVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        nvo.SpremFreezingMainVO.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        nvo.SpremFreezingMainVO.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        nvo.SpremFreezingMainVO.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        nvo.SpremFreezingMainVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        nvo.SpremFreezingMainVO.BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"]));
                        nvo.SpremFreezingMainVO.NoOfVails = Convert.ToSingle(DALHelper.HandleDBNull(reader["NoOfVails"]));
                        nvo.SpremFreezingMainVO.SpremExpiryDate = DALHelper.HandleDate(reader["SpremExpiryDate"]);
                        clsNew_SpremFreezingVO item = new clsNew_SpremFreezingVO {
                            CanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CaneID"])),
                            CanisterId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanisterID"])),
                            GobletColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCodeID"])),
                            GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletShapeID"])),
                            GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletSizeID"])),
                            SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["SpremNo"])),
                            StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawID"])),
                            TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankID"])),
                            Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"])),
                            IsThaw = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThaw"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]))
                        };
                        nvo.SpremFreezingMainVO.Spillage = Convert.ToString(DALHelper.HandleDBNull(reader["Spillage"]));
                        nvo.SpremFreezingMainVO.SpermCount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SpermCount"]));
                        nvo.SpremFreezingMainVO.DFI = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DFI"]));
                        nvo.SpremFreezingMainVO.ROS = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ROS"]));
                        nvo.SpremFreezingMainVO.HIV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HIV"]));
                        nvo.SpremFreezingMainVO.HBSAG = Convert.ToInt64(DALHelper.HandleDBNull(reader["HBSAG"]));
                        nvo.SpremFreezingMainVO.VDRL = Convert.ToInt64(DALHelper.HandleDBNull(reader["VDRL"]));
                        nvo.SpremFreezingMainVO.HCV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HCV"]));
                        nvo.SpremFreezingMainVO.CheckedByDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CheckedByDoctorID"]));
                        nvo.SpremFreezingMainVO.IsConsentCheck = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsentCheck"]));
                        nvo.SpremFreezingMainVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        nvo.SpremFreezingMainVO.AbstienceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AbstienceID"]));
                        nvo.SpremFreezingMainVO.PusCells = Convert.ToString(DALHelper.HandleDBNull(reader["PusCells"]));
                        nvo.SpremFreezingMainVO.RoundCells = Convert.ToString(DALHelper.HandleDBNull(reader["RoundCells"]));
                        nvo.SpremFreezingMainVO.EpithelialCells = Convert.ToString(DALHelper.HandleDBNull(reader["EpithelialCells"]));
                        nvo.SpremFreezingMainVO.OtherCells = Convert.ToString(DALHelper.HandleDBNull(reader["OtherCells"]));
                        nvo.SpremFreezingMainVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        nvo.SpremFreezingMainVO.VisitUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"]));
                        nvo.SpremFreezingMainVO.Sperm5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Sperm5thPercentile"]));
                        nvo.SpremFreezingMainVO.Sperm75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Sperm75thPercentile"]));
                        nvo.SpremFreezingMainVO.Ejaculate5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Ejaculate5thPercentile"]));
                        nvo.SpremFreezingMainVO.Ejaculate75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["Ejaculate75thPercentile"]));
                        nvo.SpremFreezingMainVO.TotalMotility5thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility5thPercentile"]));
                        nvo.SpremFreezingMainVO.TotalMotility75thPercentile = (float) ((double) DALHelper.HandleDBNull(reader["TotalMotility75thPercentile"]));
                        nvo.SpremFreezingDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

